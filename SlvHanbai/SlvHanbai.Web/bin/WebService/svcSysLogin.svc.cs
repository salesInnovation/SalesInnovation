using System;
using System.Collections.Generic;
using System.Web.Services;
using System.Data;
using System.Text;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Activation;
using System.Web;
using SlvHanbai.Web.Class;
using SlvHanbai.Web.Class.DB;
using SlvHanbai.Web.Class.Utility;
using SlvHanbai.Web.Class.Entity;

namespace SlvHanbai.Web.WebService
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class svcSysLogin
    {
        private const string CLASS_NM = "svcSySLogin";

        #region ログイン

        // ログイン
        [OperationContract()]
        [WebMethod(EnableSession = true)]
        public EntitySysLogin Login(string LoginID, string PassWord, int confirmFlg)
        {
            CommonUtl.ExLogger.Info(CLASS_NM + ".Login");

            CommonUtl.ExLogger.Info(CommonUtl.gConnectionString1);

            #region Field

            EntitySysLogin entity = null;

            int userId = 0;
            string userNm = "";
            int companyId = 0;
            string companyNm = "";
            int groupId = 0;
            string groupNm = "";
            int personId = 0;
            string personNm = "";

            int beforeUserId = 0;

            string accountBeginPeriod = "";
            string accountEndPeriod = "";
            string databaseString = "";
            string databaseProvider = "";
            string groupDisplayNm = "";
            int evidenceSaveFlg = 0;
            int invoicePrintFlg = 0;
            int idFigureSlipNo = 10;
            int idFigureCustomer = 10;
            int idFigurePurchase = 10;
            int idFigureGoods = 10;
            int estimateApprovalFlg = 1;
            int reportSizeUser = 0;
            int reportSizeAll = 0;
            int demoFlg = 0;
            string sysVer = "";

            string message = "";

            ExMySQLData sysdb = null;
            ExMySQLData db;

            StringBuilder sb;
            DataTable dt;

            #endregion

            #region ログインID、パスワードチェック

            try
            {
                //
                sb = new StringBuilder();
                sb.Append("SELECT UR.* " + Environment.NewLine);
                sb.Append("      ,CP.NAME AS COMPANY_NAME " + Environment.NewLine);
                sb.Append("      ,GP.NAME AS GROUP_NAME " + Environment.NewLine);
                sb.Append("      ,GP.ESTIMATE_APPROVAL_FLG " + Environment.NewLine);
                sb.Append("      ,GP.INVOICE_PRINT_FLG " + Environment.NewLine);
                sb.Append("  FROM SYS_M_USER AS UR" + Environment.NewLine);
                sb.Append("  LEFT JOIN SYS_M_COMPANY AS CP" + Environment.NewLine);
                sb.Append("    ON UR.COMPANY_ID = CP.ID " + Environment.NewLine);
                sb.Append("   AND CP.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND CP.DISPLAY_FLG = 1 " + Environment.NewLine);
                sb.Append("  LEFT JOIN SYS_M_COMPANY_GROUP AS GP" + Environment.NewLine);
                sb.Append("    ON UR.GROUP_ID = GP.ID " + Environment.NewLine);
                sb.Append("   AND GP.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND GP.DISPLAY_FLG = 1 " + Environment.NewLine);
                sb.Append(" WHERE UR.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND UR.LOGIN_ID = " + ExEscape.zRepStr(LoginID) + Environment.NewLine);
                sb.Append("   AND UR.PASSWORD = " + ExEscape.zRepStr(PassWord) + Environment.NewLine);
                dt = CommonUtl.gMySqlDt.GetDataTable(sb.ToString());

                if (dt.DefaultView.Count > 0)
                {
                    userId = ExCast.zCInt(dt.DefaultView[0]["ID"]);
                    userNm = ExCast.zCStr(dt.DefaultView[0]["NAME"]);
                    companyId = ExCast.zCInt(dt.DefaultView[0]["COMPANY_ID"]);
                    companyNm = ExCast.zCStr(dt.DefaultView[0]["COMPANY_NAME"]);
                    groupId = ExCast.zCInt(dt.DefaultView[0]["GROUP_ID"]);
                    groupNm = ExCast.zCStr(dt.DefaultView[0]["GROUP_NAME"]);
                    personId = ExCast.zCInt(dt.DefaultView[0]["PERSON_ID"]);
                    estimateApprovalFlg = ExCast.zCInt(dt.DefaultView[0]["ESTIMATE_APPROVAL_FLG"]);
                    invoicePrintFlg = ExCast.zCInt(dt.DefaultView[0]["INVOICE_PRINT_FLG"]);

                    // 前回セッションIDの保持
                    beforeUserId = ExCast.zCInt(HttpContext.Current.Session[ExSession.USER_ID]);
                }
                else
                {
                    entity = new EntitySysLogin((int)EntitySysLogin.geLoginReturn.Failure,   // Return CD
                                                "ログインID、または、パスワードが不正です。");                      // Return Message
                    return entity;
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".Login(ID,Pass Check)", ex);
                entity = new EntitySysLogin((int)EntitySysLogin.geLoginReturn.Error,              // Return CD                
                                            "ログイン処理でエラーが発生しました。" + Environment.NewLine +
                                            "システム管理者へ報告して下さい。" + Environment.NewLine + ex.ToString());   // Return Message
                return entity;
            }

            #endregion

            #region 前回ログインチェック

            try
            {

                // 前回ログイン有り
                if (ExCast.zCStr(HttpContext.Current.Session[ExSession.USER_ID]) != "")
                {
                    // 前回ログインと同じ
                    if (ExCast.zCInt(HttpContext.Current.Session[ExSession.USER_ID]) == userId)
                    {
                        if (CommonUtl.gDemoKbn == 1)
                        {
                            // 再ログインとして返す
                            entity = new EntitySysLogin((int)EntitySysLogin.geLoginReturn.Again,     // Return CD                
                                                        "");                                         // Return Message
                            return entity;
                        }

                        // 同一セッションが存在しているか確認
                        if (ExSession.ExistsSessionInf(userId, ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]), ref message) == true)
                        {
                            // 再ログインとして返す
                            entity = new EntitySysLogin((int)EntitySysLogin.geLoginReturn.Again,     // Return CD                
                                                        "");                                         // Return Message
                            return entity;
                        }
                        else
                        {
                            // 違うセッションパラメータが設定されていた場合、削除
                            ExSession.DelSessionInf(userId);
                        }
                    }
                    //// 前回ログインと別
                    //else
                    //{
                    //    // 一旦ログオフする
                    //    if (pvtLogoff(ExCast.zCStr(HttpContext.Current.Session[ExSession.IP_ADRESS]),
                    //              ExCast.zCStr(HttpContext.Current.Session[ExSession.USER_ID]),
                    //              ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]),
                    //              ExCast.zCStr(HttpContext.Current.Session[ExSession.PERSON_ID])) == false)
                    //    {
                    //        entity = new EntitySysLogin((int)EntitySysLogin.geLoginReturn.Error,              // Return CD                
                    //                                    "ログオフ処理に失敗しました。" + Environment.NewLine +
                    //                                    "システム管理者へ報告して下さい。" + CommonUtl.gstrErrMsg);   // Return Message
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".Login(Before Login Check)", ex);
                entity = new EntitySysLogin((int)EntitySysLogin.geLoginReturn.Error,              // Return CD                
                                            "前回ログインチェック処理でエラーが発生しました。" + Environment.NewLine +
                                            "システム管理者へ報告して下さい。" + Environment.NewLine + ex.ToString());   // Return Message
                return entity;

            }

            #endregion

            #region システム設定取得

            try
            {
                sb.Length = 0;
                sb.Append("SELECT ST.*" + Environment.NewLine);
                sb.Append("  FROM SYS_M_SETTING AS ST" + Environment.NewLine);
                sb.Append(" WHERE ST.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND ST.DISPLAY_FLG = 1 " + Environment.NewLine);
                sb.Append("   AND ST.COMPANY_ID = " + companyId.ToString() + Environment.NewLine);
                dt = CommonUtl.gMySqlDt.GetDataTable(sb.ToString());

                if (dt.DefaultView.Count > 0)
                {
                    accountBeginPeriod = ExCast.zCStr(dt.DefaultView[0]["ACCOUNT_BEGIN_PERIOD"]);
                    accountEndPeriod = ExCast.zCStr(dt.DefaultView[0]["ACCOUNT_END_PERIOD"]);
                    databaseString = ExCast.zCStr(dt.DefaultView[0]["DATABESE_SETTING"]);
                    databaseProvider = ExCast.zCStr(dt.DefaultView[0]["DATABESE_PROVIDER"]);
                    groupDisplayNm = ExCast.zCStr(dt.DefaultView[0]["GROUP_DISPLAY_NAME"]);
                    evidenceSaveFlg = ExCast.zCInt(dt.DefaultView[0]["EVIDENCE_SAVE_FLG"]);
                    idFigureSlipNo = ExCast.zCInt(dt.DefaultView[0]["ID_FIGURE_SLIP_NO"]);
                    idFigureCustomer = ExCast.zCInt(dt.DefaultView[0]["ID_FIGURE_CUSTOMER"]);
                    idFigurePurchase = ExCast.zCInt(dt.DefaultView[0]["ID_FIGURE_PURCHASE"]);
                    idFigureGoods = ExCast.zCInt(dt.DefaultView[0]["ID_FIGURE_GOODS"]);
                    reportSizeUser = ExCast.zCInt(dt.DefaultView[0]["REPORT_SAVE_SIZE_USER"]);
                    reportSizeAll = ExCast.zCInt(dt.DefaultView[0]["REPORT_SAVE_SIZE_ALL"]);
                    demoFlg = ExCast.zCInt(dt.DefaultView[0]["DEMO_FLG"]);
                    sysVer = ExCast.zCStr(dt.DefaultView[0]["SYSTEM_VER"]);
                }
                else
                {
                    entity = new EntitySysLogin((int)EntitySysLogin.geLoginReturn.Failure,   // Return CD                                  
                                                "システム設定データが存在しません。" + Environment.NewLine + 
                                                "システム管理者へ報告して下さい。");                                // Return Message
                    return entity;
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".Login(Get System Setting)", ex);
                entity = new EntitySysLogin((int)EntitySysLogin.geLoginReturn.Error,           // Return CD
                                         "システム設定の取得処理でエラーが発生しました。" + Environment.NewLine + 
                                         "システム管理者へ報告して下さい。" + Environment.NewLine + ex.ToString());   // Return Message
                return entity;
            }

            #endregion

            #region 個別データベース接続確認

            try
            {
                db = new ExMySQLData(databaseString);
                db.DbOpen();
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".Login(DB Connect)", ex);
                entity = new EntitySysLogin((int)EntitySysLogin.geLoginReturn.Error,             // Return CD
                                            "個別データベースの接続に失敗しました。" + Environment.NewLine +
                                            "システム管理者へ報告して下さい。" + Environment.NewLine + ex.Message);     // Return Message
                return entity;
            }

            #endregion

            #region 担当者名取得

            try
            {
                sb.Length = 0;
                sb.Append("SELECT PS.* " + Environment.NewLine);
                sb.Append("  FROM M_PERSON AS PS" + Environment.NewLine);
                sb.Append(" WHERE PS.COMPANY_ID = " + companyId.ToString() + Environment.NewLine);
                sb.Append("   AND PS.ID = " + personId.ToString() + Environment.NewLine);
                sb.Append("   AND PS.DELETE_FLG = 0" + Environment.NewLine);
                sb.Append("   AND PS.DISPLAY_FLG = 1" + Environment.NewLine);
                dt = CommonUtl.gMySqlDt.GetDataTable(sb.ToString());

                if (dt.DefaultView.Count > 0)
                {
                    personNm = ExCast.zCStr(dt.DefaultView[0]["NAME"]);
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".Login(Get Person)", ex);
                entity = new EntitySysLogin((int)EntitySysLogin.geLoginReturn.Error,            // Return CD
                                            "担当者名の取得処理でエラーが発生しました。" + Environment.NewLine +
                                            "システム管理者へ報告して下さい。" + Environment.NewLine + ex.ToString()); // Return Message
                return entity;
            }

            #endregion

            #region ログイン履歴登録情報設定

            string random = "";
            string ipAdress = "";
            string date = "";
            string time = "";
            try
            {
                //ランダム文字列取得
                random = ExRandomString.GetRandomString();

                // IP取得
                OperationContext context = OperationContext.Current;
                MessageProperties properties = context.IncomingMessageProperties;
                RemoteEndpointMessageProperty endpoint = properties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
                ipAdress = endpoint.Address.ToString();

                // 日時取得
                DateTime now = DateTime.Now;
                date = now.ToString("yyyy/MM/dd");
                time = now.ToString("HH:mm:ss");
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".Login(Get History Inf)", ex);
                entity = new EntitySysLogin((int)EntitySysLogin.geLoginReturn.Error,        // Return CD
                                　　        "ログイン履歴情報の設定に失敗しました。" + Environment.NewLine +
                                　　        "システム管理者へ報告して下さい。" + Environment.NewLine +
                                　　        ex.ToString());                                                        // Return Message
                return entity;

            }

            #endregion

            #region セッション情報設定

            try
            {
                if (ExSession.AddSessionInf(userId, random, db, ref message) == false)
                {
                    entity = new EntitySysLogin((int)EntitySysLogin.geLoginReturn.Warmn,     // Return CD
                                                "セッション情報の設定に失敗しました。" + Environment.NewLine +
                                                "システム管理者へ報告して下さい。" + Environment.NewLine +
                                                message);                                                           // Return Message

                } 

            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".Login(Set Session Inf)", ex);
                entity = new EntitySysLogin((int)EntitySysLogin.geLoginReturn.Error,        // Return CD
                                　　        "セッション情報の設定に失敗しました。" + Environment.NewLine +
                                　　        "システム管理者へ報告して下さい。" + Environment.NewLine +
                                　　        ex.ToString());                                                        // Return Message
                return entity;
            }

            #endregion

            if (confirmFlg == 1)
            {
                #region ログオフ処理

                try
                {
                    // 前回ログイン有り
                    if (ExCast.zCStr(HttpContext.Current.Session[ExSession.USER_ID]) != "")
                    {
                        // 前回セッションとログインIDが違う場合
                        if (ExCast.zCInt(HttpContext.Current.Session[ExSession.USER_ID]) != userId)
                        {
                            // ログオフする
                            if (pvtLogoff(ExCast.zCStr(HttpContext.Current.Session[ExSession.IP_ADRESS]),
                                          ExCast.zCStr(HttpContext.Current.Session[ExSession.USER_ID]),
                                          ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]),
                                          ExCast.zCStr(HttpContext.Current.Session[ExSession.PERSON_ID])) == false)
                            {
                                entity = new EntitySysLogin((int)EntitySysLogin.geLoginReturn.Error,              // Return CD                
                                                            "ログオフ処理に失敗しました。" + Environment.NewLine +
                                                            "システム管理者へ報告して下さい。" + CommonUtl.gstrErrMsg);   // Return Message
                                return entity;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    CommonUtl.ExLogger.Error(CLASS_NM + ".Login(Logoff)", ex);
                    entity = new EntitySysLogin((int)EntitySysLogin.geLoginReturn.Error,        // Return CD
                                                "ログオフに失敗しました。" + Environment.NewLine +
                                                "システム管理者へ報告して下さい。" + Environment.NewLine +
                                                ex.ToString());                                                        // Return Message
                    return entity;
                }


                #endregion

                #region ログイン履歴登録

                try
                {
                    #region System Databese Open

                    try
                    {
                        sysdb = new ExMySQLData();
                        sysdb.DbOpen();
                    }
                    catch (Exception ex)
                    {
                        CommonUtl.ExLogger.Error(CLASS_NM + ".Login(DbOpen)", ex);
                        entity = new EntitySysLogin((int)EntitySysLogin.geLoginReturn.Error,     // Return CD
                                                    "ログイン履歴の登録に失敗しました。(DbOpen)" + Environment.NewLine +
                                                    "システム管理者へ報告して下さい。" + Environment.NewLine +
                                                    ex.ToString());                                                     // Return Message
                        return entity;
                    }

                    #endregion

                    #region BeginTransaction

                    try
                    {
                        sysdb.ExBeginTransaction();
                    }
                    catch (Exception ex)
                    {
                        sysdb.ExRollbackTransaction();
                        CommonUtl.ExLogger.Error(CLASS_NM + ".Login(BeginTransaction)", ex);
                        entity = new EntitySysLogin((int)EntitySysLogin.geLoginReturn.Error,     // Return CD
                                                    "ログイン履歴の登録に失敗しました。(BeginTransaction)" + Environment.NewLine +
                                                    "システム管理者へ報告して下さい。" + Environment.NewLine +
                                                    ex.ToString());                                                     // Return Message
                        return entity;
                    }

                    #endregion

                    #region Insert

                    sb.Length = 0;
                    sb.Append("INSERT INTO SYS_H_USER_LOGIN_HISTORY " + Environment.NewLine);
                    sb.Append("       (USER_ID" + Environment.NewLine);
                    sb.Append("       ,LOGIN_DIVISION" + Environment.NewLine);
                    sb.Append("       ,LOGIN_DATE" + Environment.NewLine);
                    sb.Append("       ,LOGIN_TIME" + Environment.NewLine);
                    sb.Append("       ,SESSION_STRING" + Environment.NewLine);
                    sb.Append("       ,IP_ADRESS" + Environment.NewLine);
                    sb.Append("       ,UPDATE_FLG" + Environment.NewLine);
                    sb.Append("       ,DELETE_FLG" + Environment.NewLine);
                    sb.Append("       ,CREATE_PG_ID" + Environment.NewLine);
                    sb.Append("       ,CREATE_ADRESS" + Environment.NewLine);
                    sb.Append("       ,CREATE_USER_ID" + Environment.NewLine);
                    sb.Append("       ,CREATE_PERSON_ID" + Environment.NewLine);
                    sb.Append("       ,CREATE_DATE" + Environment.NewLine);
                    sb.Append("       ,CREATE_TIME" + Environment.NewLine);
                    sb.Append("       ,UPDATE_PG_ID" + Environment.NewLine);
                    sb.Append("       ,UPDATE_ADRESS" + Environment.NewLine);
                    sb.Append("       ,UPDATE_USER_ID" + Environment.NewLine);
                    sb.Append("       ,UPDATE_PERSON_ID" + Environment.NewLine);
                    sb.Append("       ,UPDATE_DATE" + Environment.NewLine);
                    sb.Append("       ,UPDATE_TIME" + Environment.NewLine);
                    sb.Append(")" + Environment.NewLine);
                    sb.Append("VALUES (" + userId + Environment.NewLine);               // USER_ID
                    sb.Append("       ,1" + Environment.NewLine);                       // LOGIN_DIVISION
                    sb.Append("       ," + ExEscape.zRepStr(date) + Environment.NewLine);          // LOGIN_DATE
                    sb.Append("       ," + ExEscape.zRepStr(time) + Environment.NewLine);          // LOGIN_TIME
                    sb.Append("       ," + ExEscape.zRepStr(random) + Environment.NewLine);        // SESSION_STRING
                    sb.Append("       ," + ExEscape.zRepStr(ipAdress) + Environment.NewLine);      // IP_ADRESS
                    sb.Append(CommonUtl.GetInsSQLCommonColums(CommonUtl.UpdKbn.Ins,
                                                              "SYSTEM",
                                                              "",
                                                              ExCast.zCInt(personId),
                                                              "0",
                                                              ExCast.zCStr(ipAdress),
                                                              ExCast.zCStr(userId)));
                    sb.Append(")");

                    sysdb.ExecuteSQL(sb.ToString(), false);

                    #endregion

                    #region CommitTransaction

                    try
                    {
                        sysdb.ExCommitTransaction();
                    }
                    catch (Exception ex)
                    {
                        CommonUtl.gMySqlDt.ExRollbackTransaction();
                        CommonUtl.ExLogger.Error(CLASS_NM + ".Login(CommitTransaction)", ex);
                        entity = new EntitySysLogin((int)EntitySysLogin.geLoginReturn.Error,     // Return CD
                                                    "ログイン履歴の登録に失敗しました。(BeginTransaction)" + Environment.NewLine +
                                                    "システム管理者へ報告して下さい。" + Environment.NewLine +
                                                    ex.ToString());                                                     // Return Message
                        return entity;
                    }

                    #endregion

                    #region System Database Close

                    try
                    {
                        sysdb.DbClose();
                    }
                    catch (Exception ex)
                    {
                        sysdb.ExRollbackTransaction();
                        CommonUtl.ExLogger.Error(CLASS_NM + ".Login(DbClose)", ex);
                        entity = new EntitySysLogin((int)EntitySysLogin.geLoginReturn.Error,     // Return CD
                                                    "ログイン履歴の登録に失敗しました。(DbClose)" + Environment.NewLine +
                                                    "システム管理者へ報告して下さい。" + Environment.NewLine +
                                                    ex.ToString());                                                     // Return Message
                        return entity;
                    }
                    finally
                    {
                        sysdb = null;
                    }

                    #endregion

                    #region セッションの保持

                    // セッションの保持
                    HttpContext.Current.Session[ExSession.COMPANY_ID] = companyId;
                    HttpContext.Current.Session[ExSession.GROUP_ID] = groupId;
                    HttpContext.Current.Session[ExSession.USER_ID] = userId;
                    HttpContext.Current.Session[ExSession.USER_NM] = userNm;
                    HttpContext.Current.Session[ExSession.PERSON_ID] = personId;
                    HttpContext.Current.Session[ExSession.DEFAULT_PERSON_ID] = personId;
                    HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR] = random;
                    HttpContext.Current.Session[ExSession.IP_ADRESS] = ipAdress;
                    HttpContext.Current.Session[ExSession.DB_CONNECTION_STR] = databaseString;
                    HttpContext.Current.Session[ExSession.DATA_CLASS] = db;
                    HttpContext.Current.Session[ExSession.EVIDENCE_SAVE_FLG] = evidenceSaveFlg;
                    HttpContext.Current.Session[ExSession.ACCOUNT_BEGIN_PERIOD] = accountBeginPeriod;
                    HttpContext.Current.Session[ExSession.ACCOUNT_END_PERIOD] = accountEndPeriod;
                    HttpContext.Current.Session[ExSession.ID_FIGURE_SLIP_NO] = idFigureSlipNo;
                    HttpContext.Current.Session[ExSession.ID_FIGURE_CUSTOMER] = idFigureCustomer;
                    HttpContext.Current.Session[ExSession.ID_FIGURE_PURCHASE] = idFigurePurchase;
                    HttpContext.Current.Session[ExSession.ID_FIGURE_GOODS] = idFigureGoods;
                    HttpContext.Current.Session[ExSession.REPORT_SAVE_SIZE_USER] = reportSizeUser;
                    HttpContext.Current.Session[ExSession.REPORT_SAVE_SIZE_ALL] = reportSizeAll;
                    HttpContext.Current.Session[ExSession.GROUP_DISPLAY_NAME] = groupDisplayNm;
                    HttpContext.Current.Session[ExSession.ESTIMATE_APPROVAL_FLG] = estimateApprovalFlg;
                    HttpContext.Current.Session[ExSession.RECEIPT_ACCOUNT_INVOICE_PRINT_FLG] = invoicePrintFlg;

                    #endregion

                }
                catch (Exception ex)
                {
                    sysdb.ExRollbackTransaction();
                    CommonUtl.ExLogger.Error(CLASS_NM + ".Login(Add History)", ex);
                    entity = new EntitySysLogin((int)EntitySysLogin.geLoginReturn.Error,     // Return CD
                                                "ログイン履歴の登録に失敗しました。" + Environment.NewLine +
                                                "システム管理者へ報告して下さい。" + Environment.NewLine +
                                                ex.ToString());                                                     // Return Message
                    return entity;
                }

                #endregion
            }
            else
            {
                #region セッションの保持

                // セッションの保持
                HttpContext.Current.Session[ExSession.COMPANY_ID] = companyId;
                HttpContext.Current.Session[ExSession.GROUP_ID] = groupId;
                HttpContext.Current.Session[ExSession.USER_ID] = userId;
                HttpContext.Current.Session[ExSession.USER_NM] = userNm;
                HttpContext.Current.Session[ExSession.PERSON_ID] = personId;
                HttpContext.Current.Session[ExSession.DEFAULT_PERSON_ID] = personId;
                HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR] = random;
                HttpContext.Current.Session[ExSession.IP_ADRESS] = ipAdress;
                HttpContext.Current.Session[ExSession.DB_CONNECTION_STR] = databaseString;
                HttpContext.Current.Session[ExSession.DATA_CLASS] = db;
                HttpContext.Current.Session[ExSession.EVIDENCE_SAVE_FLG] = evidenceSaveFlg;
                HttpContext.Current.Session[ExSession.ACCOUNT_BEGIN_PERIOD] = accountBeginPeriod;
                HttpContext.Current.Session[ExSession.ACCOUNT_END_PERIOD] = accountEndPeriod;
                HttpContext.Current.Session[ExSession.ID_FIGURE_SLIP_NO] = idFigureSlipNo;
                HttpContext.Current.Session[ExSession.ID_FIGURE_CUSTOMER] = idFigureCustomer;
                HttpContext.Current.Session[ExSession.ID_FIGURE_PURCHASE] = idFigurePurchase;
                HttpContext.Current.Session[ExSession.ID_FIGURE_GOODS] = idFigureGoods;
                HttpContext.Current.Session[ExSession.REPORT_SAVE_SIZE_USER] = reportSizeUser;
                HttpContext.Current.Session[ExSession.REPORT_SAVE_SIZE_ALL] = reportSizeAll;
                HttpContext.Current.Session[ExSession.GROUP_DISPLAY_NAME] = groupDisplayNm;
                HttpContext.Current.Session[ExSession.ESTIMATE_APPROVAL_FLG] = estimateApprovalFlg;
                HttpContext.Current.Session[ExSession.RECEIPT_ACCOUNT_INVOICE_PRINT_FLG] = invoicePrintFlg;

                #endregion
            }

            entity = new EntitySysLogin((int)EntitySysLogin.geLoginReturn.Normal,    // Return CD
                                        "",                                          // Return Message
                                        companyId,                                   // Company ID
                                        companyNm,                                   // Company Name
                                        groupId,                                     // Group ID
                                        groupNm,                                     // Group Name
                                        personId,                                    // Default Person ID
                                        personNm,                                    // Default Person Name
                                        groupDisplayNm,                              // Group Display Name
                                        evidenceSaveFlg,                             // Evidence Flg
                                        idFigureSlipNo,                              // 
                                        idFigureCustomer,                            // 
                                        idFigurePurchase,                            // 
                                        idFigureGoods,                               // 
                                        random);                                     // Session String
            entity.user_id = userId;
            entity.user_nm = userNm;
            entity.estimate_approval_flg = estimateApprovalFlg;
            entity.receipt_account_invoice_print_flg = invoicePrintFlg;
            entity.demo_flg = demoFlg;
            entity.sys_ver = sysVer;

            return entity;

        }

        #endregion

        #region ログオフ

        [OperationContract()]
        [WebMethod(EnableSession = true)]
        public bool Logoff(string random)
        {
            #region 認証処理

            string userId = "";
            string ipAdress = "";
            string sessionString = "";
            string personId = "";
            string _message = "";

            userId = ExCast.zCStr(HttpContext.Current.Session[ExSession.USER_ID]);
            ipAdress = ExCast.zCStr(HttpContext.Current.Session[ExSession.IP_ADRESS]);
            sessionString = ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]);
            personId = ExCast.zCStr(HttpContext.Current.Session[ExSession.PERSON_ID]); 

            _message = ExSession.SessionUserUniqueCheck(random, ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]), ExCast.zCInt(HttpContext.Current.Session[ExSession.USER_ID]));
            if (_message != "")
            {
                return false;
            }

            #endregion

            return pvtLogoff(ipAdress, userId, sessionString, personId);
        }

        private bool pvtLogoff(string ipAdress, string userId, string sessionString, string personId)
        {
            #region Field

            string date = "";
            string time = "";
            DataTable dt;
            ExMySQLData sysdb = null;
            StringBuilder sb = new StringBuilder();

            #endregion

            #region Init

            // 日時取得
            DateTime now = DateTime.Now;
            date = now.ToString("yyyy/MM/dd");
            time = now.ToString("HH:mm:ss");

            #endregion

            #region System Databese Open

            try
            {
                sysdb = new ExMySQLData();
                sysdb.DbOpen();
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".pvtLogoff(DbOpen)", ex);
                CommonUtl.gstrErrMsg = CLASS_NM + ".pvtLogoff(DbOpen)" + Environment.NewLine + ex.Message;
                return false;
            }

            #endregion

            #region BeginTransaction

            try
            {
                sysdb.ExBeginTransaction();
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".pvtLogoff(BeginTransaction)", ex);
                CommonUtl.gstrErrMsg = CLASS_NM + ".pvtLogoff(BeginTransaction)" + Environment.NewLine + ex.Message;
                return false;
            }

            #endregion

            #region Insert

            // ログイン履歴登録
            try
            {

                sb.Length = 0;
                sb.Append("INSERT INTO SYS_H_USER_LOGIN_HISTORY " + Environment.NewLine);
                sb.Append("       (USER_ID" + Environment.NewLine);
                sb.Append("       ,LOGIN_DIVISION" + Environment.NewLine);
                sb.Append("       ,LOGIN_DATE" + Environment.NewLine);
                sb.Append("       ,LOGIN_TIME" + Environment.NewLine);
                sb.Append("       ,SESSION_STRING" + Environment.NewLine);
                sb.Append("       ,IP_ADRESS" + Environment.NewLine);
                sb.Append("       ,UPDATE_FLG" + Environment.NewLine);
                sb.Append("       ,DELETE_FLG" + Environment.NewLine);
                sb.Append("       ,CREATE_PG_ID" + Environment.NewLine);
                sb.Append("       ,CREATE_ADRESS" + Environment.NewLine);
                sb.Append("       ,CREATE_USER_ID" + Environment.NewLine);
                sb.Append("       ,CREATE_PERSON_ID" + Environment.NewLine);
                sb.Append("       ,CREATE_DATE" + Environment.NewLine);
                sb.Append("       ,CREATE_TIME" + Environment.NewLine);
                sb.Append("       ,UPDATE_PG_ID" + Environment.NewLine);
                sb.Append("       ,UPDATE_ADRESS" + Environment.NewLine);
                sb.Append("       ,UPDATE_USER_ID" + Environment.NewLine);
                sb.Append("       ,UPDATE_PERSON_ID" + Environment.NewLine);
                sb.Append("       ,UPDATE_DATE" + Environment.NewLine);
                sb.Append("       ,UPDATE_TIME" + Environment.NewLine);
                sb.Append(")" + Environment.NewLine);
                sb.Append("VALUES (" + userId + Environment.NewLine);                       // USER_ID
                sb.Append("       ,2" + Environment.NewLine);                               // LOGIN_DIVISION
                sb.Append("       ," + ExEscape.zRepStr(date) + Environment.NewLine);                  // LOGIN_DATE
                sb.Append("       ," + ExEscape.zRepStr(time) + Environment.NewLine);                  // LOGIN_TIME
                sb.Append("       ," + ExEscape.zRepStr(sessionString) + Environment.NewLine);         // SESSION_STRING
                sb.Append("       ," + ExEscape.zRepStr(ipAdress) + Environment.NewLine);              // IP_ADRESS
                sb.Append(CommonUtl.GetInsSQLCommonColums(CommonUtl.UpdKbn.Ins, 
                                                          "SYSTEM", 
                                                          "",
                                                          ExCast.zCInt(personId), 
                                                          "0",
                                                          ipAdress,
                                                          userId));
                sb.Append(")");

                sysdb.ExecuteSQL(sb.ToString(), false);

            }
            catch (Exception ex)
            {
                sysdb.ExRollbackTransaction();
                CommonUtl.ExLogger.Error(CLASS_NM + ".pvtLogoff(Insert)", ex);
                CommonUtl.gstrErrMsg = CLASS_NM + ".pvtLogoff(Insert)" + Environment.NewLine + ex.Message;
                return false;
            }

            #endregion

            #region CommitTransaction

            try
            {
                sysdb.ExCommitTransaction();
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".pvtLogoff(CommitTransaction)", ex);
                CommonUtl.gstrErrMsg = CLASS_NM + ".pvtLogoff(CommitTransaction)" + Environment.NewLine + ex.Message;
                return false;
            }

            #endregion

            #region System Database Close

            try
            {
                sysdb.DbClose();
            }
            catch (Exception ex)
            {
                sysdb.ExRollbackTransaction();
                CommonUtl.ExLogger.Error(CLASS_NM + ".pvtLogoff(DbClose)", ex);
                CommonUtl.gstrErrMsg = CLASS_NM + ".pvtLogoff(DbClose)" + Environment.NewLine + ex.Message;
                return false;
            }
            finally
            {
                sysdb = null;
            }

            #endregion

            // グローバルセッション情報削除
            try
            {
                if (ExSession.DelSessionInf(ExCast.zCInt(HttpContext.Current.Session[ExSession.USER_ID])) == false)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".pvtLogoff(Delete Session)", ex);
                CommonUtl.gstrErrMsg = CLASS_NM + ".pvtLogoff(Delete Session)" + Environment.NewLine + ex.Message;
                return false;
            }

        }

        #endregion
    }
}
