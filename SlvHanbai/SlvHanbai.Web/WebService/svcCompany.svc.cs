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
    public class svcCompany
    {
        private const string CLASS_NM = "svcCompany";
        private readonly string PG_NM = DataPgEvidence.PGName.Mst.Company;

        #region データ取得

        /// <summary> 
        /// 会社データ取得
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebMethod(EnableSession = true)]
        public EntityCompany GetCompany(string random)
        {

            EntityCompany entity;

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
                    entity = new EntityCompany();
                    entity.MESSAGE = _message;
                    return entity;
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetCompany(認証処理)", ex);
                entity = new EntityCompany();
                entity.MESSAGE = CLASS_NM + ".GetCompany : 認証処理に失敗しました。" + Environment.NewLine + ex.Message.ToString(); ;
                return entity;
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

                #region SQL

                sb.Append("SELECT MT.* " + Environment.NewLine);
                sb.Append("      ,SS.ID_FIGURE_SLIP_NO " + Environment.NewLine);
                sb.Append("      ,SS.ID_FIGURE_CUSTOMER " + Environment.NewLine);
                sb.Append("      ,SS.ID_FIGURE_PURCHASE " + Environment.NewLine);
                sb.Append("      ,SS.ID_FIGURE_GOODS " + Environment.NewLine);
                sb.Append("      ,SS.GROUP_DISPLAY_NAME " + Environment.NewLine);
                sb.Append("      ,ES_YMD.YMD AS ES_YMD " + Environment.NewLine);
                sb.Append("      ,JC_YMD.YMD AS JC_YMD " + Environment.NewLine);
                sb.Append("      ,SA_YMD.YMD AS SA_YMD " + Environment.NewLine);
                sb.Append("      ,RP_YMD.YMD AS RP_YMD " + Environment.NewLine);
                sb.Append("      ,ES_CNT.CNT AS ES_CNT " + Environment.NewLine);
                sb.Append("      ,JC_CNT.CNT AS JC_CNT " + Environment.NewLine);
                sb.Append("      ,SA_CNT.CNT AS SA_CNT " + Environment.NewLine);
                sb.Append("      ,RP_CNT.CNT AS RP_CNT " + Environment.NewLine);
                sb.Append("  FROM SYS_M_COMPANY AS MT" + Environment.NewLine);

                #region Join

                // システム設定
                sb.Append("  LEFT JOIN SYS_M_SETTING AS SS" + Environment.NewLine);
                sb.Append("    ON SS.COMPANY_ID = MT.ID" + Environment.NewLine);

                // 見積最終伝票入力日
                sb.Append("  LEFT JOIN (SELECT COMPANY_ID" + Environment.NewLine);
                sb.Append("                  , MAX(date_format(ESTIMATE_YMD , " + ExEscape.SQL_YMD + ")) AS YMD " + Environment.NewLine);
                sb.Append("               FROM T_ESTIMATE_H " + Environment.NewLine);
                sb.Append("              WHERE DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("                AND COMPANY_ID = " + companyId + Environment.NewLine);
                sb.Append("              GROUP BY COMPANY_ID) AS ES_YMD" + Environment.NewLine);
                sb.Append("    ON ES_YMD.COMPANY_ID = MT.ID" + Environment.NewLine);

                // 受注最終伝票入力日
                sb.Append("  LEFT JOIN (SELECT COMPANY_ID" + Environment.NewLine);
                sb.Append("                  , MAX(date_format(ORDER_YMD , " + ExEscape.SQL_YMD + ")) AS YMD " + Environment.NewLine);
                sb.Append("               FROM T_ORDER_H " + Environment.NewLine);
                sb.Append("              WHERE DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("                AND COMPANY_ID = " + companyId + Environment.NewLine);
                sb.Append("              GROUP BY COMPANY_ID) AS JC_YMD" + Environment.NewLine);
                sb.Append("    ON JC_YMD.COMPANY_ID = MT.ID" + Environment.NewLine);

                // 売上最終伝票入力日
                sb.Append("  LEFT JOIN (SELECT COMPANY_ID" + Environment.NewLine);
                sb.Append("                  , MAX(date_format(SALES_YMD , " + ExEscape.SQL_YMD + ")) AS YMD " + Environment.NewLine);
                sb.Append("               FROM T_SALES_H " + Environment.NewLine);
                sb.Append("              WHERE DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("                AND COMPANY_ID = " + companyId + Environment.NewLine);
                sb.Append("              GROUP BY COMPANY_ID) AS SA_YMD" + Environment.NewLine);
                sb.Append("    ON SA_YMD.COMPANY_ID = MT.ID" + Environment.NewLine);

                // 入金最終伝票入力日
                sb.Append("  LEFT JOIN (SELECT COMPANY_ID" + Environment.NewLine);
                sb.Append("                  , MAX(date_format(RECEIPT_YMD , " + ExEscape.SQL_YMD + ")) AS YMD " + Environment.NewLine);
                sb.Append("               FROM T_RECEIPT_H " + Environment.NewLine);
                sb.Append("              WHERE DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("                AND COMPANY_ID = " + companyId + Environment.NewLine);
                sb.Append("              GROUP BY COMPANY_ID) AS RP_YMD" + Environment.NewLine);
                sb.Append("    ON RP_YMD.COMPANY_ID = MT.ID" + Environment.NewLine);

                // 見積現在伝票件数
                sb.Append("  LEFT JOIN (SELECT COMPANY_ID" + Environment.NewLine);
                sb.Append("                  , COUNT(COMPANY_ID) AS CNT " + Environment.NewLine);
                sb.Append("               FROM T_ESTIMATE_H " + Environment.NewLine);
                sb.Append("              WHERE DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("                AND COMPANY_ID = " + companyId + Environment.NewLine);
                sb.Append("              GROUP BY COMPANY_ID) AS ES_CNT" + Environment.NewLine);
                sb.Append("    ON ES_CNT.COMPANY_ID = MT.ID" + Environment.NewLine);

                // 受注現在伝票件数
                sb.Append("  LEFT JOIN (SELECT COMPANY_ID" + Environment.NewLine);
                sb.Append("                  , COUNT(COMPANY_ID) AS CNT " + Environment.NewLine);
                sb.Append("               FROM T_ORDER_H " + Environment.NewLine);
                sb.Append("              WHERE DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("                AND COMPANY_ID = " + companyId + Environment.NewLine);
                sb.Append("              GROUP BY COMPANY_ID) AS JC_CNT" + Environment.NewLine);
                sb.Append("    ON JC_CNT.COMPANY_ID = MT.ID" + Environment.NewLine);

                // 売上現在伝票件数
                sb.Append("  LEFT JOIN (SELECT COMPANY_ID" + Environment.NewLine);
                sb.Append("                  , COUNT(COMPANY_ID) AS CNT " + Environment.NewLine);
                sb.Append("               FROM T_SALES_H " + Environment.NewLine);
                sb.Append("              WHERE DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("                AND COMPANY_ID = " + companyId + Environment.NewLine);
                sb.Append("              GROUP BY COMPANY_ID) AS SA_CNT" + Environment.NewLine);
                sb.Append("    ON SA_CNT.COMPANY_ID = MT.ID" + Environment.NewLine);

                // 入金現在伝票件数
                sb.Append("  LEFT JOIN (SELECT COMPANY_ID" + Environment.NewLine);
                sb.Append("                  , COUNT(COMPANY_ID) AS CNT " + Environment.NewLine);
                sb.Append("               FROM T_RECEIPT_H " + Environment.NewLine);
                sb.Append("              WHERE DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("                AND COMPANY_ID = " + companyId + Environment.NewLine);
                sb.Append("              GROUP BY COMPANY_ID) AS RP_CNT" + Environment.NewLine);
                sb.Append("    ON RP_CNT.COMPANY_ID = MT.ID" + Environment.NewLine);

                #endregion

                sb.Append(" WHERE MT.ID = " + companyId + Environment.NewLine);
                sb.Append("   AND MT.DELETE_FLG = 0 " + Environment.NewLine);

                #endregion

                dt = db.GetDataTable(sb.ToString());

                if (dt.DefaultView.Count > 0)
                {
                    entity = new EntityCompany();

                    // 排他制御
                    DataPgLock.geLovkFlg lockFlg;
                    string strErr = DataPgLock.SetLockPg(companyId, userId, PG_NM, companyId.ToString(), ipAdress, db, out lockFlg);
                    if (strErr != "")
                    {
                        entity.MESSAGE = CLASS_NM + ".GetCompany : 排他制御(ロック情報取得)に失敗しました。" + Environment.NewLine + strErr;
                    }

                    #region Set Entity

                    entity.name = ExCast.zCStr(dt.DefaultView[0]["NAME"]);
                    entity.kana = ExCast.zCStr(dt.DefaultView[0]["KANA"]);

                    string _zip = ExCast.zCStr(dt.DefaultView[0]["ZIP_CODE"]);
                    if (!string.IsNullOrEmpty(_zip) && ExCast.zCStr(_zip) != "0")
                    {
                        _zip = string.Format("{0:0000000}", ExCast.zCDbl(_zip));
                        entity.zip_code_from = _zip.Substring(0, 3);
                        entity.zip_code_to = _zip.Substring(3, 4);
                    }
                    else
                    {
                        entity.zip_code_from = "";
                        entity.zip_code_to = "";
                    }

                    entity.prefecture_id = ExCast.zCInt(dt.DefaultView[0]["PREFECTURE_ID"]);
                    entity.city_id = ExCast.zCInt(dt.DefaultView[0]["CITY_ID"]);
                    entity.town_id = ExCast.zCInt(dt.DefaultView[0]["TOWN_ID"]);
                    entity.adress_city = ExCast.zCStr(dt.DefaultView[0]["ADRESS_CITY"]);
                    entity.adress_town = ExCast.zCStr(dt.DefaultView[0]["ADRESS_TOWN"]);
                    entity.adress1 = ExCast.zCStr(dt.DefaultView[0]["ADRESS1"]);
                    entity.adress2 = ExCast.zCStr(dt.DefaultView[0]["ADRESS2"]);
                    entity.tel = ExCast.zCStr(dt.DefaultView[0]["TEL"]);
                    entity.fax = ExCast.zCStr(dt.DefaultView[0]["FAX"]);
                    entity.mail_adress = ExCast.zCStr(dt.DefaultView[0]["MAIL_ADRESS"]);
                    entity.mobile_tel = ExCast.zCStr(dt.DefaultView[0]["MOBILE_TEL"]);
                    entity.mobile_adress = ExCast.zCStr(dt.DefaultView[0]["MOBILE_ADRESS"]);
                    entity.url = ExCast.zCStr(dt.DefaultView[0]["URL"]);

                    entity.group_display_name = ExCast.zCStr(dt.DefaultView[0]["GROUP_DISPLAY_NAME"]);
                    entity.id_figure_slip_no = ExCast.zCInt(dt.DefaultView[0]["ID_FIGURE_SLIP_NO"]);
                    entity.id_figure_customer = ExCast.zCInt(dt.DefaultView[0]["ID_FIGURE_CUSTOMER"]);
                    entity.id_figure_purchase = ExCast.zCInt(dt.DefaultView[0]["ID_FIGURE_PURCHASE"]);
                    entity.id_figure_commodity = ExCast.zCInt(dt.DefaultView[0]["ID_FIGURE_GOODS"]);

                    entity.estimate_ymd = ExCast.zDateNullToDefault(dt.DefaultView[0]["ES_YMD"]);
                    entity.order_ymd = ExCast.zDateNullToDefault(dt.DefaultView[0]["JC_YMD"]);
                    entity.sales_ymd = ExCast.zDateNullToDefault(dt.DefaultView[0]["SA_YMD"]);
                    entity.receipt_ymd = ExCast.zDateNullToDefault(dt.DefaultView[0]["RP_YMD"]);
                    entity.purchase_order_ymd = "";
                    entity.purchase_ymd = "";
                    entity.cash_payment_ymd = "";
                    entity.produce_ymd = "";
                    entity.ship_ymd = "";

                    entity.estimate_cnt = ExCast.zCLng(dt.DefaultView[0]["ES_CNT"]);
                    entity.order_cnt = ExCast.zCLng(dt.DefaultView[0]["JC_CNT"]);
                    entity.sales_cnt = ExCast.zCLng(dt.DefaultView[0]["SA_CNT"]);
                    entity.receipt_cnt = ExCast.zCLng(dt.DefaultView[0]["RP_CNT"]);
                    entity.purchase_order_cnt = 0;
                    entity.purchase_cnt = 0;
                    entity.cash_payment_cnt = 0;
                    entity.produce_cnt = 0;
                    entity.ship_cnt = 0;

                    entity.lock_flg = (int)lockFlg;
                    entity.memo = ExCast.zCStr(dt.DefaultView[0]["MEMO"]);

                    #endregion

                }
                else
                {
                    entity = null;
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetCompany", ex);
                entity = new EntityCompany();
                entity.MESSAGE = CLASS_NM + ".GetCompany : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message.ToString();
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
                           "ID:" + companyId.ToString());

            return entity;

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
        public string UpdateCompany(string random, int type, EntityCompany entity)
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
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateCompany(認証処理)", ex);
                return CLASS_NM + ".UpdateCompany : 認証処理に失敗しました。" + Environment.NewLine + ex.Message.ToString();
            }

            #endregion

            #region Field

            StringBuilder sb = new StringBuilder();
            DataTable dt;
            ExMySQLData db = null;
            string _Id = "";

            #endregion

            #region Databese Open

            try
            {
                db = new ExMySQLData();
                db.DbOpen();
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateCompany(DbOpen)", ex);
                return CLASS_NM + ".UpdateCompany(DbOpen) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
            }

            #endregion

            #region BeginTransaction

            try
            {
                db.ExBeginTransaction();
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateCompany(BeginTransaction)", ex);
                return CLASS_NM + ".UpdateCompany(BeginTransaction) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
            }

            #endregion

            #region Update

            if (type == 0)
            {
                try
                {
                    #region Company SQL

                    sb.Length = 0;
                    sb.Append("UPDATE SYS_M_COMPANY " + Environment.NewLine);
                    sb.Append(CommonUtl.GetUpdSQLCommonColums(PG_NM,
                                                               ExCast.zCInt(personId),
                                                               ipAdress,
                                                               userId,
                                                               0));
                    sb.Append("      ,NAME = " + ExEscape.zRepStr(entity.name) + Environment.NewLine);
                    sb.Append("      ,KANA = " + ExEscape.zRepStr(entity.kana) + Environment.NewLine);
                    sb.Append("      ,ZIP_CODE = " + ExCast.zNullToZero(entity.zip_code_from + entity.zip_code_to) + Environment.NewLine);
                    sb.Append("      ,PREFECTURE_ID = " + entity.prefecture_id + Environment.NewLine);
                    sb.Append("      ,CITY_ID = " + entity.city_id + Environment.NewLine);
                    sb.Append("      ,TOWN_ID = " + entity.town_id + Environment.NewLine);
                    sb.Append("      ,ADRESS_CITY = " + ExEscape.zRepStr(entity.adress_city) + Environment.NewLine);
                    sb.Append("      ,ADRESS_TOWN = " + ExEscape.zRepStr(entity.adress_town) + Environment.NewLine);
                    sb.Append("      ,ADRESS1 = " + ExEscape.zRepStr(entity.adress1) + Environment.NewLine);
                    sb.Append("      ,ADRESS2 = " + ExEscape.zRepStr(entity.adress2) + Environment.NewLine);
                    sb.Append("      ,TEL = " + ExEscape.zRepStr(entity.tel) + Environment.NewLine);
                    sb.Append("      ,FAX = " + ExEscape.zRepStr(entity.fax) + Environment.NewLine);
                    sb.Append("      ,MAIL_ADRESS = " + ExEscape.zRepStr(entity.mail_adress) + Environment.NewLine);
                    sb.Append("      ,MOBILE_TEL = " + ExEscape.zRepStr(entity.mobile_tel) + Environment.NewLine);
                    sb.Append("      ,MOBILE_ADRESS = " + ExEscape.zRepStr(entity.mobile_adress) + Environment.NewLine);
                    sb.Append("      ,URL = " + ExEscape.zRepStr(entity.url) + Environment.NewLine);
                    sb.Append("      ,MEMO = " + ExEscape.zRepStr(entity.memo) + Environment.NewLine);
                    sb.Append(" WHERE ID = " + companyId + Environment.NewLine);                            // COMPANY_ID

                    #endregion

                    db.ExecuteSQL(sb.ToString(), false);

                    #region System Setting SQL

                    sb.Length = 0;
                    sb.Append("UPDATE SYS_M_SETTING " + Environment.NewLine);
                    sb.Append(CommonUtl.GetUpdSQLCommonColums(PG_NM,
                                                              ExCast.zCInt(personId),
                                                              ipAdress,
                                                              userId,
                                                              0));
                    sb.Append("      ,GROUP_DISPLAY_NAME = " + ExEscape.zRepStr(entity.group_display_name) + Environment.NewLine);
                    sb.Append("      ,ID_FIGURE_SLIP_NO = " + entity.id_figure_slip_no + Environment.NewLine);
                    sb.Append("      ,ID_FIGURE_CUSTOMER = " + entity.id_figure_customer + Environment.NewLine);
                    sb.Append("      ,ID_FIGURE_PURCHASE = " + entity.id_figure_purchase + Environment.NewLine);
                    sb.Append("      ,ID_FIGURE_GOODS = " + entity.id_figure_commodity + Environment.NewLine);
                    sb.Append(" WHERE COMPANY_ID = " + companyId + Environment.NewLine);                            // COMPANY_ID

                    #endregion

                    db.ExecuteSQL(sb.ToString(), false);

                }
                catch (Exception ex)
                {
                    db.ExRollbackTransaction();
                    CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateCompany(Update)", ex);
                    return CLASS_NM + ".UpdateCompany(Update) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
                }
            }

            #endregion

            #region Delete

            if (type == 2)
            {
                //#region Exist Data

                //try
                //{
                //    bool _ret = false;
                //    _ret = DataExists.IsExistData(db, companyId, "", "SYS_M_COMPANY", "INVOICE_ID", ExCast.zNumZeroNothingFormat(Id), "ID", ExCast.zNumZeroNothingFormat(Id), CommonUtl.geStrOrNumKbn.String);
                //    if (_ret == true)
                //    {
                //        return "得意先ID : " + Id + " は得意先マスタの請求IDにが使用されている為、削除できません。";
                //    }

                //    _ret = DataExists.IsExistData(db, companyId, "", "T_ORDER_H", "Company_ID", ExCast.zNumZeroNothingFormat(Id), CommonUtl.geStrOrNumKbn.String);
                //    if (_ret == true)
                //    {
                //        return "得意先ID : " + Id + " は受注データの得意先に使用されている為、削除できません。";
                //    }

                //}
                //catch (Exception ex)
                //{
                //    db.ExRollbackTransaction();
                //    CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateCompany(Exist Data)", ex);
                //    return CLASS_NM + ".UpdateCompany(Exist Data) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
                //}

                //#endregion

                //#region Update

                //try
                //{
                //    sb.Length = 0;
                //    sb.Append("UPDATE SYS_M_COMPANY " + Environment.NewLine);
                //    sb.Append(CommonUtl.GetUpdSQLCommonColums(PG_NM,
                //                                               ExCast.zCInt(personId),
                //                                               ipAdress,
                //                                               userId,
                //                                               1));
                //    sb.Append(" WHERE COMPANY_ID = " + companyId + Environment.NewLine);    // COMPANY_ID
                //    sb.Append("   AND ID = " + ExEscape.zRepStr(ExCast.zNumZeroNothingFormat(Id)) + Environment.NewLine);            // ID

                //    db.ExecuteSQL(sb.ToString(), false);

                //}
                //catch (Exception ex)
                //{
                //    db.ExRollbackTransaction();
                //    CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateCompany(Delete)", ex);
                //    return CLASS_NM + ".UpdateCompany(Delete) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
                //}

                //#endregion
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
                    db.ExRollbackTransaction();
                    CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateCompany(DelLockPg)", ex);
                    return CLASS_NM + ".UpdateCompany(DelLockPg) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
                }
            }

            #endregion

            #region CommitTransaction

            try
            {
                db.ExCommitTransaction();
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateCompany(CommitTransaction)", ex);
                return CLASS_NM + ".UpdateCompany(CommitTransaction) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
            }

            #endregion

            #region Database Close

            try
            {
                db.DbClose();
            }
            catch (Exception ex)
            {
                db.ExRollbackTransaction();
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateCompany(DbClose)", ex);
                return CLASS_NM + ".UpdateCompany(DbClose) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
            }
            finally
            {
                db = null;
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
                                                   "ID:" + companyId.ToString());
                        break;
                    case 1:
                        svcPgEvidence.gAddEvidence(ExCast.zCInt(HttpContext.Current.Session[ExSession.EVIDENCE_SAVE_FLG]),
                                                   companyId,
                                                   userId,
                                                   ipAdress,
                                                   sessionString,
                                                   PG_NM,
                                                   DataPgEvidence.geOperationType.Insert,
                                                   "ID:" + companyId.ToString());
                        break;
                    case 2:
                        svcPgEvidence.gAddEvidence(ExCast.zCInt(HttpContext.Current.Session[ExSession.EVIDENCE_SAVE_FLG]),
                                                   companyId,
                                                   userId,
                                                   ipAdress,
                                                   sessionString,
                                                   PG_NM,
                                                   DataPgEvidence.geOperationType.Delete,
                                                   "ID:" + companyId.ToString());
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateCompany(Add Evidence)", ex);
                return CLASS_NM + ".UpdateCompany(Add Evidence) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
            }

            #endregion

            #region Return

            return "";

            #endregion
        }

        #endregion

    }
}
