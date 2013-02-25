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
using SlvHanbai.Web.Class.Data;
using SlvHanbai.Web.Class.DB;
using SlvHanbai.Web.Class.Utility;
using SlvHanbai.Web.Class.Entity;
using SlvHanbai.Web.Class.Reports;

namespace SlvHanbai.Web.WebService
{
    [ServiceContract(Namespace = "")]
    [SilverlightFaultBehavior]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class svcInquiry
    {
        private const string CLASS_NM = "svcInquiry";
        private readonly string PG_NM = DataPgEvidence.PGName.Inquiry.InquiryInp;

        #region データ取得

        /// <summary> 
        /// データ取得
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebMethod(EnableSession = true)]
        public List<EntityInquiry> GetInquiry(string random, long no)
        {
            List<EntityInquiry> entityList = new List<EntityInquiry>();

            #region 認証処理

            string companyId = "";
            string groupId = "";
            string userId = "";
            string ipAdress = "";
            string sessionString = "";

            try
            {
                companyId = ExCast.zCStr(HttpContext.Current.Session[ExSession.COMPANY_ID]);
                groupId = ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_ID]);
                userId = ExCast.zCStr(HttpContext.Current.Session[ExSession.USER_ID]);
                ipAdress = ExCast.zCStr(HttpContext.Current.Session[ExSession.IP_ADRESS]);
                sessionString = ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]);

                string _message = ExSession.SessionUserUniqueCheck(random, ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]), ExCast.zCInt(HttpContext.Current.Session[ExSession.USER_ID]));
                if (_message != "")
                {
                    EntityInquiry entity = new EntityInquiry();
                    entity.MESSAGE = _message;
                    entityList.Add(entity);
                    return entityList;
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetInquiry(認証処理)", ex);
                EntityInquiry entity = new EntityInquiry();
                entity.MESSAGE = CLASS_NM + ".GetInquiry : 認証処理に失敗しました。" + Environment.NewLine + ex.Message.ToString(); ;
                entityList.Add(entity);
                return entityList;
            }

            #endregion

            StringBuilder sb;
            DataTable dt;
            ExMySQLData db;

            try
            {
                db = ExSession.GetSessionDb(ExCast.zCInt(HttpContext.Current.Session[ExSession.USER_ID]),
                                            ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]));

                sb = new StringBuilder();

                

                if (no == 0)
                {
                    // 一覧表示時
                    #region SQL

                    // 返答時
                    sb.Append("SELECT DISTINCT " + Environment.NewLine);
                    sb.Append("       IH.NO " + Environment.NewLine);
                    sb.Append("      ,IH.DATE " + Environment.NewLine);
                    sb.Append("      ,IH.TIME " + Environment.NewLine);
                    sb.Append("      ,IH.INQUIRY_DIVISION_ID " + Environment.NewLine);
                    sb.Append("      ,NM1.DESCRIPTION AS INQUIRY_DIVISION_NAME " + Environment.NewLine);
                    sb.Append("      ,IH.TITLE " + Environment.NewLine);
                    sb.Append("      ,IH.INQUIRY_STATE_ID " + Environment.NewLine);
                    sb.Append("      ,NM3.DESCRIPTION AS INQUIRY_STATE_NAME " + Environment.NewLine);
                    sb.Append("      ,IH.INQUIRY_LEVEL_ID " + Environment.NewLine);
                    sb.Append("      ,NM2.DESCRIPTION AS INQUIRY_LEVEL_NAME " + Environment.NewLine);
                    sb.Append("  FROM SYS_T_INQUIRY_H AS IH" + Environment.NewLine);

                    #region Join

                    // 担当者(ヘッダ)
                    sb.Append("  LEFT JOIN M_PERSON AS PS" + Environment.NewLine);
                    sb.Append("    ON PS.DELETE_FLG = 0 " + Environment.NewLine);
                    sb.Append("   AND PS.DISPLAY_FLG = 1 " + Environment.NewLine);
                    sb.Append("   AND PS.COMPANY_ID = IH.COMPANY_ID " + Environment.NewLine);
                    sb.Append("   AND PS.ID = IH.PERSON_ID" + Environment.NewLine);

                    // 問い合わせ区分ID
                    sb.Append("  LEFT JOIN SYS_M_NAME AS NM1" + Environment.NewLine);
                    sb.Append("    ON NM1.DELETE_FLG = 0 " + Environment.NewLine);
                    sb.Append("   AND NM1.DISPLAY_FLG = 1 " + Environment.NewLine);
                    sb.Append("   AND NM1.DIVISION_ID = " + (int)CommonUtl.geNameKbn.INQUIRY_DIVISION_ID + Environment.NewLine);
                    sb.Append("   AND NM1.ID = IH.INQUIRY_DIVISION_ID" + Environment.NewLine);

                    // 問い合わせ緊急度ID
                    sb.Append("  LEFT JOIN SYS_M_NAME AS NM2" + Environment.NewLine);
                    sb.Append("    ON NM2.DELETE_FLG = 0 " + Environment.NewLine);
                    sb.Append("   AND NM2.DISPLAY_FLG = 1 " + Environment.NewLine);
                    sb.Append("   AND NM2.DIVISION_ID = " + (int)CommonUtl.geNameKbn.LEVEL_ID + Environment.NewLine);
                    sb.Append("   AND NM2.ID = IH.INQUIRY_LEVEL_ID" + Environment.NewLine);

                    // 問い合わせ状態ID
                    sb.Append("  LEFT JOIN SYS_M_NAME AS NM3" + Environment.NewLine);
                    sb.Append("    ON NM3.DELETE_FLG = 0 " + Environment.NewLine);
                    sb.Append("   AND NM3.DISPLAY_FLG = 1 " + Environment.NewLine);
                    sb.Append("   AND NM3.DIVISION_ID = " + (int)CommonUtl.geNameKbn.INQUIRY_STATE_ID + Environment.NewLine);
                    sb.Append("   AND NM3.ID = IH.INQUIRY_STATE_ID" + Environment.NewLine);

                    #endregion

                    sb.Append(" WHERE IH.COMPANY_ID = " + companyId + Environment.NewLine);
                    sb.Append("   AND IH.DELETE_FLG = 0 " + Environment.NewLine);

                    sb.Append(" ORDER BY IH.DATE DESC " + Environment.NewLine);
                    sb.Append("         ,IH.TIME DESC " + Environment.NewLine);

                    #endregion
                }
                else
                {
                    #region SQL

                    // 返答時
                    sb.Append("SELECT IH.NO " + Environment.NewLine);
                    sb.Append("      ,IH.COMPANY_ID " + Environment.NewLine);
                    sb.Append("      ,CP.NAME AS COMPANY_NAME " + Environment.NewLine);
                    sb.Append("      ,IH.GROUP_ID " + Environment.NewLine);
                    sb.Append("      ,GP.NAME AS GROUP_NAME " + Environment.NewLine);
                    sb.Append("      ,IH.PERSON_ID " + Environment.NewLine);
                    sb.Append("      ,PS.NAME AS PERSON_NAME " + Environment.NewLine);
                    sb.Append("      ,IH.DATE " + Environment.NewLine);
                    sb.Append("      ,IH.TIME " + Environment.NewLine);
                    sb.Append("      ,IH.TITLE " + Environment.NewLine);
                    sb.Append("      ,IH.INQUIRY_DIVISION_ID " + Environment.NewLine);
                    sb.Append("      ,NM1.DESCRIPTION AS INQUIRY_DIVISION_NAME " + Environment.NewLine);
                    sb.Append("      ,IH.INQUIRY_LEVEL_ID " + Environment.NewLine);
                    sb.Append("      ,NM2.DESCRIPTION AS INQUIRY_LEVEL_NAME " + Environment.NewLine);
                    sb.Append("      ,IH.INQUIRY_STATE_ID " + Environment.NewLine);
                    sb.Append("      ,NM3.DESCRIPTION AS INQUIRY_STATE_NAME " + Environment.NewLine);
                    sb.Append("      ,IH.MEMO " + Environment.NewLine);
                    sb.Append("      ,ID.REC_NO " + Environment.NewLine);
                    sb.Append("      ,ID.DATE AS D_DATE" + Environment.NewLine);
                    sb.Append("      ,ID.TIME AS D_TIME" + Environment.NewLine);
                    sb.Append("      ,ID.KBN " + Environment.NewLine);
                    sb.Append("      ,ID.PERSON_ID AS D_PERSON_ID " + Environment.NewLine);
                    sb.Append("      ,PS2.NAME AS D_PERSON_NAME " + Environment.NewLine);
                    sb.Append("      ,ID.SUPPORT_PERSON_NAME " + Environment.NewLine);
                    sb.Append("      ,ID.UPLOAD_FILE_PATH1 " + Environment.NewLine);
                    sb.Append("      ,ID.UPLOAD_FILE_PATH2 " + Environment.NewLine);
                    sb.Append("      ,ID.UPLOAD_FILE_PATH3 " + Environment.NewLine);
                    sb.Append("      ,ID.UPLOAD_FILE_PATH4 " + Environment.NewLine);
                    sb.Append("      ,ID.UPLOAD_FILE_PATH5 " + Environment.NewLine);
                    sb.Append("      ,ID.CONTENT " + Environment.NewLine);
                    sb.Append("  FROM SYS_T_INQUIRY_H AS IH" + Environment.NewLine);

                    #region Join

                    sb.Append("  INNER JOIN SYS_T_INQUIRY_D AS ID" + Environment.NewLine);
                    sb.Append("    ON ID.DELETE_FLG = 0 " + Environment.NewLine);
                    sb.Append("   AND ID.NO = IH.NO" + Environment.NewLine);

                    // 会社
                    sb.Append("  LEFT JOIN SYS_M_COMPANY AS CP" + Environment.NewLine);
                    sb.Append("    ON CP.DELETE_FLG = 0 " + Environment.NewLine);
                    sb.Append("   AND CP.DISPLAY_FLG = 1 " + Environment.NewLine);
                    sb.Append("   AND CP.ID = IH.COMPANY_ID " + Environment.NewLine);

                    // グループ
                    sb.Append("  LEFT JOIN SYS_M_COMPANY_GROUP AS GP" + Environment.NewLine);
                    sb.Append("    ON GP.DELETE_FLG = 0 " + Environment.NewLine);
                    sb.Append("   AND GP.DISPLAY_FLG = 1 " + Environment.NewLine);
                    sb.Append("   AND GP.ID = IH.GROUP_ID " + Environment.NewLine);

                    // 担当者(ヘッダ)
                    sb.Append("  LEFT JOIN M_PERSON AS PS" + Environment.NewLine);
                    sb.Append("    ON PS.DELETE_FLG = 0 " + Environment.NewLine);
                    sb.Append("   AND PS.DISPLAY_FLG = 1 " + Environment.NewLine);
                    sb.Append("   AND PS.COMPANY_ID = IH.COMPANY_ID " + Environment.NewLine);
                    sb.Append("   AND PS.ID = IH.PERSON_ID" + Environment.NewLine);

                    // 担当者(明細)
                    sb.Append("  LEFT JOIN M_PERSON AS PS2" + Environment.NewLine);
                    sb.Append("    ON PS2.DELETE_FLG = 0 " + Environment.NewLine);
                    sb.Append("   AND PS2.DISPLAY_FLG = 1 " + Environment.NewLine);
                    sb.Append("   AND PS2.COMPANY_ID = IH.COMPANY_ID " + Environment.NewLine);
                    sb.Append("   AND PS2.ID = ID.PERSON_ID" + Environment.NewLine);

                    // 問い合わせ区分ID
                    sb.Append("  LEFT JOIN SYS_M_NAME AS NM1" + Environment.NewLine);
                    sb.Append("    ON NM1.DELETE_FLG = 0 " + Environment.NewLine);
                    sb.Append("   AND NM1.DISPLAY_FLG = 1 " + Environment.NewLine);
                    sb.Append("   AND NM1.DIVISION_ID = " + (int)CommonUtl.geNameKbn.INQUIRY_DIVISION_ID + Environment.NewLine);
                    sb.Append("   AND NM1.ID = IH.INQUIRY_DIVISION_ID" + Environment.NewLine);

                    // 問い合わせ緊急度ID
                    sb.Append("  LEFT JOIN SYS_M_NAME AS NM2" + Environment.NewLine);
                    sb.Append("    ON NM2.DELETE_FLG = 0 " + Environment.NewLine);
                    sb.Append("   AND NM2.DISPLAY_FLG = 1 " + Environment.NewLine);
                    sb.Append("   AND NM2.DIVISION_ID = " + (int)CommonUtl.geNameKbn.LEVEL_ID + Environment.NewLine);
                    sb.Append("   AND NM2.ID = IH.INQUIRY_LEVEL_ID" + Environment.NewLine);

                    // 問い合わせ緊急度ID
                    sb.Append("  LEFT JOIN SYS_M_NAME AS NM3" + Environment.NewLine);
                    sb.Append("    ON NM3.DELETE_FLG = 0 " + Environment.NewLine);
                    sb.Append("   AND NM3.DISPLAY_FLG = 1 " + Environment.NewLine);
                    sb.Append("   AND NM3.DIVISION_ID = " + (int)CommonUtl.geNameKbn.INQUIRY_STATE_ID + Environment.NewLine);
                    sb.Append("   AND NM3.ID = IH.INQUIRY_STATE_ID" + Environment.NewLine);

                    #endregion

                    sb.Append(" WHERE IH.COMPANY_ID = " + companyId + Environment.NewLine);
                    sb.Append("   AND IH.DELETE_FLG = 0 " + Environment.NewLine);
                    sb.Append("   AND IH.NO = " + no + Environment.NewLine);

                    sb.Append(" ORDER BY IH.DATE DESC " + Environment.NewLine);
                    sb.Append("         ,IH.TIME DESC " + Environment.NewLine);
                    sb.Append("         ,ID.REC_NO DESC " + Environment.NewLine);

                    #endregion
                }

                dt = db.GetDataTable(sb.ToString());

                if (dt.DefaultView.Count > 0)
                {
                    for (int i = 0; i <= dt.DefaultView.Count - 1; i++)
                    {
                        EntityInquiry entity = new EntityInquiry();

                        // 排他制御
                        DataPgLock.geLovkFlg lockFlg = DataPgLock.geLovkFlg.UnLock;
                        if (no != 0)    // 返答時
                        {
                            string strErr = DataPgLock.SetLockPg(companyId, userId, PG_NM, no.ToString(), ipAdress, db, out lockFlg);
                            if (strErr != "")
                            {
                                entity.MESSAGE = CLASS_NM + ".GetInquiry : 排他制御(ロック情報取得)に失敗しました。" + Environment.NewLine + strErr;
                            }
                        }

                        #region Set Entity


                        entity.no = ExCast.zCLng(dt.DefaultView[i]["NO"]);
                        entity.date_time = ExCast.zDateNullToDefault(dt.DefaultView[i]["DATE"]) + " " + ExCast.zCStr(dt.DefaultView[i]["TIME"]);
                        entity.title = ExCast.zCStr(dt.DefaultView[i]["TITLE"]);
                        entity.inquiry_division_id = ExCast.zCInt(dt.DefaultView[i]["INQUIRY_DIVISION_ID"]);
                        entity.inquiry_division_nm = ExCast.zCStr(dt.DefaultView[i]["INQUIRY_DIVISION_NAME"]);
                        entity.inquiry_level_id = ExCast.zCInt(dt.DefaultView[i]["INQUIRY_LEVEL_ID"]);
                        entity.inquiry_level_nm = ExCast.zCStr(dt.DefaultView[i]["INQUIRY_LEVEL_NAME"]);
                        entity.inquiry_level_state_id = ExCast.zCInt(dt.DefaultView[i]["INQUIRY_STATE_ID"]);
                        entity.inquiry_level_state_nm = ExCast.zCStr(dt.DefaultView[i]["INQUIRY_STATE_NAME"]);

                        if (no != 0)
                        {
                            // 返答時
                            entity.company_id = ExCast.zCInt(dt.DefaultView[i]["COMPANY_ID"]);
                            entity.company_nm = ExCast.zCStr(dt.DefaultView[i]["COMPANY_NAME"]);
                            entity.gropu_id = ExCast.zCInt(dt.DefaultView[i]["GROUP_ID"]);
                            entity.gropu_nm = ExCast.zCStr(dt.DefaultView[i]["GROUP_NAME"]);
                            entity.person_id = ExCast.zCInt(dt.DefaultView[i]["PERSON_ID"]);
                            entity.person_nm = ExCast.zCStr(dt.DefaultView[i]["PERSON_NAME"]);
                            entity.rec_no = ExCast.zCInt(dt.DefaultView[i]["REC_NO"]);
                            entity.d_date_time = ExCast.zDateNullToDefault(dt.DefaultView[i]["D_DATE"]) + " " + ExCast.zCStr(dt.DefaultView[i]["D_TIME"]);
                            entity.kbn = ExCast.zCInt(dt.DefaultView[i]["KBN"]);
                            entity.d_person_id = ExCast.zCInt(dt.DefaultView[i]["D_PERSON_ID"]);
                            entity.d_person_nm = ExCast.zCStr(dt.DefaultView[i]["D_PERSON_NAME"]);
                            entity.support_person_nm = ExCast.zCStr(dt.DefaultView[i]["SUPPORT_PERSON_NAME"]);
                            entity.content = ExCast.zCStr(dt.DefaultView[i]["CONTENT"]);
                            entity.lock_flg = (int)lockFlg;
                            entity.memo = ExCast.zCStr(dt.DefaultView[0]["MEMO"]);
                        }

                        #endregion

                        entityList.Add(entity);

                    }
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetInquiry", ex);
                entityList.Clear();
                EntityInquiry entity = new EntityInquiry();
                entity.MESSAGE = CLASS_NM + ".GetInquiry : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message.ToString(); ;
                entityList.Add(entity);
                return entityList;
            }
            finally
            {
                db = null;
            }

            svcPgEvidence.gAddEvidence(ExCast.zCInt(HttpContext.Current.Session[ExSession.EVIDENCE_SAVE_FLG]),
                           companyId,
                           userId,
                           ipAdress,
                           sessionString,
                           PG_NM,
                           DataPgEvidence.geOperationType.Select,
                           "NO:" + no.ToString());

            return entityList;

        }

        #endregion

        #region データ更新

        /// <summary>
        /// データ更新
        /// </summary>
        /// <param name="random">セッションランダム文字列</param>
        /// <param name="type">0:Update 1:Insert 2:Delete</param>
        /// <param name="Id">Id</param>
        /// <param name="entity">更新データ</param>
        /// <returns></returns>
        [OperationContract()]
        [WebMethod(EnableSession = true)]
        public string UpdateInquiry(string random, int type, long no, EntityInquiry entity)
        {
            #region 認証処理

            string companyId = "";
            string groupId = "";
            string userId = "";
            string ipAdress = "";
            string sessionString = "";
            string personId = "";

            try
            {
                companyId = ExCast.zCStr(HttpContext.Current.Session[ExSession.COMPANY_ID]);
                groupId = ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_ID]);
                userId = ExCast.zCStr(HttpContext.Current.Session[ExSession.USER_ID]);
                ipAdress = ExCast.zCStr(HttpContext.Current.Session[ExSession.IP_ADRESS]);
                sessionString = ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]);
                personId = ExCast.zCStr(HttpContext.Current.Session[ExSession.PERSON_ID]);

                string _message = ExSession.SessionUserUniqueCheck(random, ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]), ExCast.zCInt(HttpContext.Current.Session[ExSession.USER_ID]));
                if (_message != "")
                {
                    return _message;
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateInquiry(認証処理)", ex);
                return CLASS_NM + ".UpdateInquiry : 認証処理に失敗しました。" + Environment.NewLine + ex.Message.ToString();
            }

            #endregion

            #region Field

            StringBuilder sb = new StringBuilder();
            DataTable dt;
            ExMySQLData sys_db = null;
            ExMySQLData db = null;
            string _no = "";

            DateTime now = DateTime.Now;
            string date = now.ToString("yyyy/MM/dd");
            string time = now.ToString("HH:mm:ss");

            #endregion

            #region Databese Open

            try
            {
                sys_db = new ExMySQLData();
                sys_db.DbOpen();
                db = new ExMySQLData(ExCast.zCStr(HttpContext.Current.Session[ExSession.DB_CONNECTION_STR]));
                db.DbOpen();
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateInquiry(DbOpen)", ex);
                return CLASS_NM + ".UpdateInquiry(DbOpen) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
            }

            #endregion

            #region BeginTransaction

            try
            {
                sys_db.ExBeginTransaction();
                if (CommonUtl.gSysDbKbn == 0) db.ExBeginTransaction();
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateInquiry(BeginTransaction)", ex);
                return CLASS_NM + ".UpdateInquiry(BeginTransaction) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
            }

            #endregion

            #region Get Max ID

            if (type == 1)
            {
                try
                {
                    DataMaxId.GetMaxId(sys_db,
                                       "SYS_T_INQUIRY_H",
                                       "NO",
                                       out _no);

                    if (_no == "")
                    {
                        return "サポートNOの取得に失敗しました。";
                    }
                }
                catch (Exception ex)
                {
                    sys_db.ExRollbackTransaction();
                    if (CommonUtl.gSysDbKbn == 0) db.ExRollbackTransaction();
                    CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateInquiry(GetMaxId)", ex);
                    return CLASS_NM + ".UpdateInquiry(GetMaxId) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
                }
            }
            else
            {
                _no = no.ToString();
            }

            #endregion

            #region Insert

            if (type == 1)
            {
                try
                {
                    #region Insert SQL

                    sb.Length = 0;
                    sb.Append("INSERT INTO SYS_T_INQUIRY_H " + Environment.NewLine);
                    sb.Append("       ( NO" + Environment.NewLine);
                    sb.Append("       , COMPANY_ID" + Environment.NewLine);
                    sb.Append("       , GROUP_ID" + Environment.NewLine);
                    sb.Append("       , PERSON_ID" + Environment.NewLine);
                    sb.Append("       , DATE" + Environment.NewLine);
                    sb.Append("       , TIME" + Environment.NewLine);
                    sb.Append("       , TITLE" + Environment.NewLine);
                    sb.Append("       , INQUIRY_DIVISION_ID" + Environment.NewLine);
                    sb.Append("       , INQUIRY_LEVEL_ID" + Environment.NewLine);
                    sb.Append("       , INQUIRY_STATE_ID" + Environment.NewLine);
                    sb.Append("       , MEMO" + Environment.NewLine);
                    sb.Append("       , UPDATE_FLG" + Environment.NewLine);
                    sb.Append("       , DELETE_FLG" + Environment.NewLine);
                    sb.Append("       , CREATE_PG_ID" + Environment.NewLine);
                    sb.Append("       , CREATE_ADRESS" + Environment.NewLine);
                    sb.Append("       , CREATE_USER_ID" + Environment.NewLine);
                    sb.Append("       , CREATE_PERSON_ID" + Environment.NewLine);
                    sb.Append("       , CREATE_DATE" + Environment.NewLine);
                    sb.Append("       , CREATE_TIME" + Environment.NewLine);
                    sb.Append("       , UPDATE_PG_ID" + Environment.NewLine);
                    sb.Append("       , UPDATE_ADRESS" + Environment.NewLine);
                    sb.Append("       , UPDATE_USER_ID" + Environment.NewLine);
                    sb.Append("       , UPDATE_PERSON_ID" + Environment.NewLine);
                    sb.Append("       , UPDATE_DATE" + Environment.NewLine);
                    sb.Append("       , UPDATE_TIME" + Environment.NewLine);
                    sb.Append(")" + Environment.NewLine);
                    sb.Append("SELECT  " + _no + Environment.NewLine);                              // NO
                    sb.Append("       ," + companyId + Environment.NewLine);                        // COMPANY_ID
                    sb.Append("       ," + groupId + Environment.NewLine);                          // GROUP_ID
                    sb.Append("       ," + entity.person_id + Environment.NewLine);                 // PERSON_ID
                    sb.Append("       ," + ExEscape.zRepStr(date) + Environment.NewLine);                      // DATE
                    sb.Append("       ," + ExEscape.zRepStr(time) + Environment.NewLine);                      // TIME
                    sb.Append("       ," + ExEscape.zRepStr(entity.title) + Environment.NewLine);              // TITLE
                    sb.Append("       ," + entity.inquiry_division_id + Environment.NewLine);       // INQUIRY_DIVISION_ID
                    sb.Append("       ," + entity.inquiry_level_id + Environment.NewLine);          // INQUIRY_LEVEL_ID
                    sb.Append("       ," + entity.inquiry_level_state_id + Environment.NewLine);    // INQUIRY_STATE_ID
                    sb.Append("       ," + ExEscape.zRepStr(entity.memo) + Environment.NewLine);               // MEMO
                    sb.Append(CommonUtl.GetInsSQLCommonColums(CommonUtl.UpdKbn.Ins,
                                                                PG_NM,
                                                                "SYS_T_INQUIRY_H",
                                                                ExCast.zCInt(personId),
                                                                _no,
                                                                ipAdress,
                                                                userId));

                    #endregion

                    sys_db.ExecuteSQL(sb.ToString(), false);
                    if (CommonUtl.gSysDbKbn == 0) db.ExecuteSQL(sb.ToString(), false);

                }
                catch (Exception ex)
                {
                    sys_db.ExRollbackTransaction();
                    if (CommonUtl.gSysDbKbn == 0) db.ExRollbackTransaction();
                    CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateInquiry(Insert)", ex);
                    return CLASS_NM + ".UpdateInquiry(Insert) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
                }
            }

            #endregion

            #region Update

            if (type == 0)
            {
                #region Update Head

                try
                {
                    #region SQL

                    sb.Length = 0;
                    sb.Append("UPDATE SYS_T_INQUIRY_H " + Environment.NewLine);
                    sb.Append(CommonUtl.GetUpdSQLCommonColums(PG_NM,
                                                               ExCast.zCInt(personId),
                                                               ipAdress,
                                                               userId,
                                                               0));
                    sb.Append("      ,INQUIRY_STATE_ID = " + entity.inquiry_level_state_id + Environment.NewLine);
                    sb.Append(" WHERE NO = " + _no + Environment.NewLine);

                    #endregion

                    sys_db.ExecuteSQL(sb.ToString(), false);
                    if (CommonUtl.gSysDbKbn == 0) db.ExecuteSQL(sb.ToString(), false);

                }
                catch (Exception ex)
                {
                    sys_db.ExRollbackTransaction();
                    if (CommonUtl.gSysDbKbn == 0) db.ExRollbackTransaction();
                    CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateInquiry(Update Head)", ex);
                    return CLASS_NM + ".UpdateInquiry(Update Head) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
                }

                #endregion
            }

            #endregion

            #region Update Or Insert(Detail)

            if (type <= 1)
            {
                try
                {
                    #region Upload File Set

                    if (!string.IsNullOrEmpty(entity.upload_file_path1))
                    {
                        if (System.IO.Directory.Exists(CommonUtl.gstrFileUpLoadDir + entity.upload_file_path1) && !System.IO.Directory.Exists(CommonUtl.gstrFileUpLoadDir + _no + "-" + entity.rec_no.ToString()))
                        {
                            System.IO.Directory.Move(CommonUtl.gstrFileUpLoadDir + entity.upload_file_path1, CommonUtl.gstrFileUpLoadDir + _no + "-" + entity.rec_no.ToString());
                            entity.upload_file_path1 = CommonUtl.gstrFileUpLoadDir + _no + "-" + entity.rec_no.ToString();
                        }
                    }

                    #endregion

                    #region Insert SQL

                    sb.Length = 0;
                    sb.Append("INSERT INTO SYS_T_INQUIRY_D " + Environment.NewLine);
                    sb.Append("       ( NO" + Environment.NewLine);
                    sb.Append("       , REC_NO" + Environment.NewLine);
                    sb.Append("       , DATE" + Environment.NewLine);
                    sb.Append("       , TIME" + Environment.NewLine);
                    sb.Append("       , KBN" + Environment.NewLine);
                    sb.Append("       , PERSON_ID" + Environment.NewLine);
                    sb.Append("       , SUPPORT_PERSON_NAME" + Environment.NewLine);
                    sb.Append("       , UPLOAD_FILE_PATH1" + Environment.NewLine);
                    sb.Append("       , UPLOAD_FILE_PATH2" + Environment.NewLine);
                    sb.Append("       , UPLOAD_FILE_PATH3" + Environment.NewLine);
                    sb.Append("       , UPLOAD_FILE_PATH4" + Environment.NewLine);
                    sb.Append("       , UPLOAD_FILE_PATH5" + Environment.NewLine);
                    sb.Append("       , CONTENT" + Environment.NewLine);
                    sb.Append("       , MEMO" + Environment.NewLine);
                    sb.Append("       , UPDATE_FLG" + Environment.NewLine);
                    sb.Append("       , DELETE_FLG" + Environment.NewLine);
                    sb.Append("       , CREATE_PG_ID" + Environment.NewLine);
                    sb.Append("       , CREATE_ADRESS" + Environment.NewLine);
                    sb.Append("       , CREATE_USER_ID" + Environment.NewLine);
                    sb.Append("       , CREATE_PERSON_ID" + Environment.NewLine);
                    sb.Append("       , CREATE_DATE" + Environment.NewLine);
                    sb.Append("       , CREATE_TIME" + Environment.NewLine);
                    sb.Append("       , UPDATE_PG_ID" + Environment.NewLine);
                    sb.Append("       , UPDATE_ADRESS" + Environment.NewLine);
                    sb.Append("       , UPDATE_USER_ID" + Environment.NewLine);
                    sb.Append("       , UPDATE_PERSON_ID" + Environment.NewLine);
                    sb.Append("       , UPDATE_DATE" + Environment.NewLine);
                    sb.Append("       , UPDATE_TIME" + Environment.NewLine);
                    sb.Append(")" + Environment.NewLine);
                    sb.Append("SELECT  " + _no + Environment.NewLine);                                  // NO
                    sb.Append("       ," + entity.rec_no + Environment.NewLine);                        // REC_NO
                    sb.Append("       ," + ExEscape.zRepStr(date) + Environment.NewLine);                          // DATE
                    sb.Append("       ," + ExEscape.zRepStr(time) + Environment.NewLine);                          // TIME
                    sb.Append("       ," + entity.kbn + Environment.NewLine);                           // KBN
                    sb.Append("       ," + entity.d_person_id + Environment.NewLine);                   // PERSON_ID
                    sb.Append("       ," + ExEscape.zRepStr(entity.support_person_nm) + Environment.NewLine);      // SUPPORT_PERSON_NAME
                    sb.Append("       ," + ExEscape.zRepStr(entity.upload_file_path1) + Environment.NewLine);      // UPLOAD_FILE_PATH1
                    sb.Append("       ," + ExEscape.zRepStr(entity.upload_file_path2) + Environment.NewLine);      // UPLOAD_FILE_PATH2
                    sb.Append("       ," + ExEscape.zRepStr(entity.upload_file_path3) + Environment.NewLine);      // UPLOAD_FILE_PATH3
                    sb.Append("       ," + ExEscape.zRepStr(entity.upload_file_path4) + Environment.NewLine);      // UPLOAD_FILE_PATH4
                    sb.Append("       ," + ExEscape.zRepStr(entity.upload_file_path5) + Environment.NewLine);      // UPLOAD_FILE_PATH5
                    sb.Append("       ," + ExEscape.zRepStr(entity.content) + Environment.NewLine);                // CONTENT
                    sb.Append("       ," + ExEscape.zRepStr(entity.memo) + Environment.NewLine);                   // MEMO
                    sb.Append(CommonUtl.GetInsSQLCommonColums(CommonUtl.UpdKbn.Ins,
                                                                PG_NM,
                                                                "SYS_T_INQUIRY_D",
                                                                ExCast.zCInt(personId),
                                                                _no,
                                                                ipAdress,
                                                                userId));

                    #endregion

                    sys_db.ExecuteSQL(sb.ToString(), false);
                    if (CommonUtl.gSysDbKbn == 0) db.ExecuteSQL(sb.ToString(), false);

                }
                catch (Exception ex)
                {
                    if (CommonUtl.gSysDbKbn == 0) db.ExRollbackTransaction();
                    sys_db.ExRollbackTransaction();
                    CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateInquiry(Insert Detail)", ex);
                    return CLASS_NM + ".UpdateInquiry(Insert Detail) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
                }
            }

            #endregion

            #region Delete

            if (type == 2)
            {
            }

            #endregion

            #region PG排他制御

            if (type == 0 || type == 2)
            {
                try
                {
                    DataPgLock.DelLockPg(companyId, userId, PG_NM, "", ipAdress, false, db);
                }
                catch (Exception ex)
                {
                    if (CommonUtl.gSysDbKbn == 0) db.ExRollbackTransaction();
                    sys_db.ExRollbackTransaction();
                    CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateInquiry(DelLockPg)", ex);
                    return CLASS_NM + ".UpdateInquiry(DelLockPg) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
                }
            }

            #endregion

            #region CommitTransaction

            try
            {
                sys_db.ExCommitTransaction();
                if (CommonUtl.gSysDbKbn == 0) db.ExCommitTransaction();
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateInquiry(CommitTransaction)", ex);
                return CLASS_NM + ".UpdateInquiry(CommitTransaction) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
            }

            #endregion

            #region Database Close

            try
            {
                sys_db.DbClose();
                db.DbClose();
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateInquiry(DbClose)", ex);
                return CLASS_NM + ".UpdateInquiry(DbClose) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
            }
            finally
            {
                sys_db = null;
            }

            #endregion

            #region Add Evidence

            try
            {
                switch (type)
                {
                    case 0:
                        svcPgEvidence.gAddEvidence(ExCast.zCInt(HttpContext.Current.Session[ExSession.EVIDENCE_SAVE_FLG]),
                                                   companyId,
                                                   userId,
                                                   ipAdress,
                                                   sessionString,
                                                   PG_NM,
                                                   DataPgEvidence.geOperationType.Update,
                                                   "NO:" + _no);
                        break;
                    case 1:
                        svcPgEvidence.gAddEvidence(ExCast.zCInt(HttpContext.Current.Session[ExSession.EVIDENCE_SAVE_FLG]),
                                                   companyId,
                                                   userId,
                                                   ipAdress,
                                                   sessionString,
                                                   PG_NM,
                                                   DataPgEvidence.geOperationType.Insert,
                                                   "NO:" + _no);
                        break;
                    case 2:
                        svcPgEvidence.gAddEvidence(ExCast.zCInt(HttpContext.Current.Session[ExSession.EVIDENCE_SAVE_FLG]),
                                                   companyId,
                                                   userId,
                                                   ipAdress,
                                                   sessionString,
                                                   PG_NM,
                                                   DataPgEvidence.geOperationType.Delete,
                                                   "NO:" + _no);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateInquiry(Add Evidence)", ex);
                return CLASS_NM + ".UpdateInquiry(Add Evidence) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
            }

            #endregion

            #region Return

            if (type == 1)
            {
                //return "Auto Insert success : " + "ID : " + _Id.ToString() + "で登録しました。";
                return "";
            }
            else
            {
                return "";
            }

            #endregion
        }

        #endregion

    }
}
