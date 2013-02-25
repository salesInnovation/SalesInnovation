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
    public class svcCompanyGroup
    {
        private const string CLASS_NM = "svcCompanyGroup";
        private readonly string PG_NM = DataPgEvidence.PGName.Mst.CompanyGroup;

        #region データ取得

        /// <summary> 
        /// データ取得
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebMethod(EnableSession = true)]
        public EntityCompanyGroup GetCompanyGroup(string random, int groupId)
        {

            EntityCompanyGroup entity;

            #region 認証処理

            string companyId = "";
            string _groupId = "";
            string userId = "";
            string ipAdress = "";
            string sessionString = "";

            try
            {
                companyId = ExCast.zCStr(HttpContext.Current.Session[ExSession.COMPANY_ID]);
                _groupId = ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_ID]);
                userId = ExCast.zCStr(HttpContext.Current.Session[ExSession.USER_ID]);
                ipAdress = ExCast.zCStr(HttpContext.Current.Session[ExSession.IP_ADRESS]);
                sessionString = ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]);

                string _message = ExSession.SessionUserUniqueCheck(random, ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]), ExCast.zCInt(HttpContext.Current.Session[ExSession.USER_ID]));
                if (_message != "")
                {
                    entity = new EntityCompanyGroup();
                    entity.MESSAGE = _message;
                    return entity;
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetCompanyGroup(認証処理)", ex);
                entity = new EntityCompanyGroup();
                entity.MESSAGE = CLASS_NM + ".GetCompanyGroup : 認証処理に失敗しました。" + Environment.NewLine + ex.Message.ToString(); ;
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
                sb.Append("      ,ES_YMD.YMD AS ES_YMD " + Environment.NewLine);
                sb.Append("      ,JC_YMD.YMD AS JC_YMD " + Environment.NewLine);
                sb.Append("      ,SA_YMD.YMD AS SA_YMD " + Environment.NewLine);
                sb.Append("      ,RP_YMD.YMD AS RP_YMD " + Environment.NewLine);
                sb.Append("      ,ES_CNT.CNT AS ES_CNT " + Environment.NewLine);
                sb.Append("      ,JC_CNT.CNT AS JC_CNT " + Environment.NewLine);
                sb.Append("      ,SA_CNT.CNT AS SA_CNT " + Environment.NewLine);
                sb.Append("      ,RP_CNT.CNT AS RP_CNT " + Environment.NewLine);
                sb.Append("      ,ES_NO.NO AS ES_NO " + Environment.NewLine);
                sb.Append("      ,JC_NO.NO AS JC_NO " + Environment.NewLine);
                sb.Append("      ,SA_NO.NO AS SA_NO " + Environment.NewLine);
                sb.Append("      ,RP_NO.NO AS RP_NO " + Environment.NewLine);
                sb.Append("      ,NM.DESCRIPTION AS DISPLAY_DIVISION_NAME " + Environment.NewLine);
                sb.Append("  FROM SYS_M_COMPANY_GROUP AS MT" + Environment.NewLine);

                #region Join

                // 見積最終伝票入力日
                sb.Append("  LEFT JOIN (SELECT COMPANY_ID" + Environment.NewLine);
                sb.Append("                  , GROUP_ID" + Environment.NewLine);
                sb.Append("                  , MAX(date_format(ESTIMATE_YMD , " + ExEscape.SQL_YMD + ")) AS YMD " + Environment.NewLine);
                sb.Append("               FROM T_ESTIMATE_H " + Environment.NewLine);
                sb.Append("              WHERE DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("                AND COMPANY_ID = " + companyId + Environment.NewLine);
                sb.Append("                AND GROUP_ID = " + groupId + Environment.NewLine);
                sb.Append("              GROUP BY COMPANY_ID" + Environment.NewLine);
                sb.Append("                      ,GROUP_ID) AS ES_YMD" + Environment.NewLine);
                sb.Append("    ON ES_YMD.COMPANY_ID = MT.COMPANY_ID" + Environment.NewLine);
                sb.Append("   AND ES_YMD.GROUP_ID = MT.ID" + Environment.NewLine);

                // 受注最終伝票入力日
                sb.Append("  LEFT JOIN (SELECT COMPANY_ID" + Environment.NewLine);
                sb.Append("                  , GROUP_ID" + Environment.NewLine);
                sb.Append("                  , MAX(date_format(ORDER_YMD , " + ExEscape.SQL_YMD + ")) AS YMD " + Environment.NewLine);
                sb.Append("               FROM T_ORDER_H " + Environment.NewLine);
                sb.Append("              WHERE DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("                AND COMPANY_ID = " + companyId + Environment.NewLine);
                sb.Append("                AND GROUP_ID = " + groupId + Environment.NewLine);
                sb.Append("              GROUP BY COMPANY_ID" + Environment.NewLine);
                sb.Append("                      ,GROUP_ID) AS JC_YMD" + Environment.NewLine);
                sb.Append("    ON JC_YMD.COMPANY_ID = MT.COMPANY_ID" + Environment.NewLine);
                sb.Append("   AND JC_YMD.GROUP_ID = MT.ID" + Environment.NewLine);

                // 売上最終伝票入力日
                sb.Append("  LEFT JOIN (SELECT COMPANY_ID" + Environment.NewLine);
                sb.Append("                  , GROUP_ID" + Environment.NewLine);
                sb.Append("                  , MAX(date_format(SALES_YMD , " + ExEscape.SQL_YMD + ")) AS YMD " + Environment.NewLine);
                sb.Append("               FROM T_SALES_H " + Environment.NewLine);
                sb.Append("              WHERE DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("                AND COMPANY_ID = " + companyId + Environment.NewLine);
                sb.Append("                AND GROUP_ID = " + groupId + Environment.NewLine);
                sb.Append("              GROUP BY COMPANY_ID" + Environment.NewLine);
                sb.Append("                      ,GROUP_ID) AS SA_YMD" + Environment.NewLine);
                sb.Append("    ON SA_YMD.COMPANY_ID = MT.COMPANY_ID" + Environment.NewLine);
                sb.Append("   AND SA_YMD.GROUP_ID = MT.ID" + Environment.NewLine);

                // 入金最終伝票入力日
                sb.Append("  LEFT JOIN (SELECT COMPANY_ID" + Environment.NewLine);
                sb.Append("                  , MAX(date_format(RECEIPT_YMD , " + ExEscape.SQL_YMD + ")) AS YMD " + Environment.NewLine);
                sb.Append("               FROM T_RECEIPT_H " + Environment.NewLine);
                sb.Append("              WHERE DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("                AND COMPANY_ID = " + companyId + Environment.NewLine);
                sb.Append("              GROUP BY COMPANY_ID) AS RP_YMD" + Environment.NewLine);
                sb.Append("    ON RP_YMD.COMPANY_ID = MT.COMPANY_ID" + Environment.NewLine);

                // 見積現在伝票番号
                sb.Append("  LEFT JOIN (SELECT COMPANY_ID" + Environment.NewLine);
                sb.Append("                  , GROUP_ID" + Environment.NewLine);
                sb.Append("                  , MAX(NO) AS NO " + Environment.NewLine);
                sb.Append("               FROM T_ESTIMATE_H " + Environment.NewLine);
                sb.Append("              WHERE DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("                AND COMPANY_ID = " + companyId + Environment.NewLine);
                sb.Append("                AND GROUP_ID = " + groupId + Environment.NewLine);
                sb.Append("              GROUP BY COMPANY_ID" + Environment.NewLine);
                sb.Append("                      ,GROUP_ID) AS ES_NO" + Environment.NewLine);
                sb.Append("    ON ES_NO.COMPANY_ID = MT.COMPANY_ID" + Environment.NewLine);
                sb.Append("   AND ES_NO.GROUP_ID = MT.ID" + Environment.NewLine);

                // 受注現在伝票番号
                sb.Append("  LEFT JOIN (SELECT COMPANY_ID" + Environment.NewLine);
                sb.Append("                  , GROUP_ID" + Environment.NewLine);
                sb.Append("                  , MAX(NO) AS NO " + Environment.NewLine);
                sb.Append("               FROM T_ORDER_H " + Environment.NewLine);
                sb.Append("              WHERE DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("                AND COMPANY_ID = " + companyId + Environment.NewLine);
                sb.Append("                AND GROUP_ID = " + groupId + Environment.NewLine);
                sb.Append("              GROUP BY COMPANY_ID" + Environment.NewLine);
                sb.Append("                      ,GROUP_ID) AS JC_NO" + Environment.NewLine);
                sb.Append("    ON JC_NO.COMPANY_ID = MT.COMPANY_ID" + Environment.NewLine);
                sb.Append("   AND JC_NO.GROUP_ID = MT.ID" + Environment.NewLine);

                // 売上現在伝票番号
                sb.Append("  LEFT JOIN (SELECT COMPANY_ID" + Environment.NewLine);
                sb.Append("                  , GROUP_ID" + Environment.NewLine);
                sb.Append("                  , MAX(NO) AS NO " + Environment.NewLine);
                sb.Append("               FROM T_SALES_H " + Environment.NewLine);
                sb.Append("              WHERE DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("                AND COMPANY_ID = " + companyId + Environment.NewLine);
                sb.Append("                AND GROUP_ID = " + groupId + Environment.NewLine);
                sb.Append("              GROUP BY COMPANY_ID" + Environment.NewLine);
                sb.Append("                      ,GROUP_ID) AS SA_NO" + Environment.NewLine);
                sb.Append("    ON SA_NO.COMPANY_ID = MT.COMPANY_ID" + Environment.NewLine);
                sb.Append("   AND SA_NO.GROUP_ID = MT.ID" + Environment.NewLine);

                // 入金現在伝票番号
                sb.Append("  LEFT JOIN (SELECT COMPANY_ID" + Environment.NewLine);
                sb.Append("                  , MAX(NO) AS NO " + Environment.NewLine);
                sb.Append("               FROM T_RECEIPT_H " + Environment.NewLine);
                sb.Append("              WHERE DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("                AND COMPANY_ID = " + companyId + Environment.NewLine);
                sb.Append("              GROUP BY COMPANY_ID) AS RP_NO" + Environment.NewLine);
                sb.Append("    ON RP_NO.COMPANY_ID = MT.COMPANY_ID" + Environment.NewLine);

                // 見積現在伝票件数
                sb.Append("  LEFT JOIN (SELECT COMPANY_ID" + Environment.NewLine);
                sb.Append("                  , GROUP_ID" + Environment.NewLine);
                sb.Append("                  , COUNT(GROUP_ID) AS CNT " + Environment.NewLine);
                sb.Append("               FROM T_ESTIMATE_H " + Environment.NewLine);
                sb.Append("              WHERE DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("                AND COMPANY_ID = " + companyId + Environment.NewLine);
                sb.Append("                AND GROUP_ID = " + groupId + Environment.NewLine);
                sb.Append("              GROUP BY COMPANY_ID" + Environment.NewLine);
                sb.Append("                      ,GROUP_ID) AS ES_CNT" + Environment.NewLine);
                sb.Append("    ON ES_CNT.COMPANY_ID = MT.COMPANY_ID" + Environment.NewLine);
                sb.Append("   AND ES_CNT.GROUP_ID = MT.ID" + Environment.NewLine);

                // 受注現在伝票件数
                sb.Append("  LEFT JOIN (SELECT COMPANY_ID" + Environment.NewLine);
                sb.Append("                  , GROUP_ID" + Environment.NewLine);
                sb.Append("                  , COUNT(GROUP_ID) AS CNT " + Environment.NewLine);
                sb.Append("               FROM T_ORDER_H " + Environment.NewLine);
                sb.Append("              WHERE DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("                AND COMPANY_ID = " + companyId + Environment.NewLine);
                sb.Append("                AND GROUP_ID = " + groupId + Environment.NewLine);
                sb.Append("              GROUP BY COMPANY_ID" + Environment.NewLine);
                sb.Append("                      ,GROUP_ID) AS JC_CNT" + Environment.NewLine);
                sb.Append("    ON JC_CNT.COMPANY_ID = MT.COMPANY_ID" + Environment.NewLine);
                sb.Append("   AND JC_CNT.GROUP_ID = MT.ID" + Environment.NewLine);

                // 売上現在伝票件数
                sb.Append("  LEFT JOIN (SELECT COMPANY_ID" + Environment.NewLine);
                sb.Append("                  , GROUP_ID" + Environment.NewLine);
                sb.Append("                  , COUNT(GROUP_ID) AS CNT " + Environment.NewLine);
                sb.Append("               FROM T_SALES_H " + Environment.NewLine);
                sb.Append("              WHERE DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("                AND COMPANY_ID = " + companyId + Environment.NewLine);
                sb.Append("                AND GROUP_ID = " + groupId + Environment.NewLine);
                sb.Append("              GROUP BY COMPANY_ID" + Environment.NewLine);
                sb.Append("                      ,GROUP_ID) AS SA_CNT" + Environment.NewLine);
                sb.Append("    ON SA_CNT.COMPANY_ID = MT.COMPANY_ID" + Environment.NewLine);
                sb.Append("   AND SA_CNT.GROUP_ID = MT.ID" + Environment.NewLine);

                // 入金現在伝票件数
                sb.Append("  LEFT JOIN (SELECT COMPANY_ID" + Environment.NewLine);
                sb.Append("                  , COUNT(COMPANY_ID) AS CNT " + Environment.NewLine);
                sb.Append("               FROM T_RECEIPT_H " + Environment.NewLine);
                sb.Append("              WHERE DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("                AND COMPANY_ID = " + companyId + Environment.NewLine);
                sb.Append("              GROUP BY COMPANY_ID) AS RP_CNT" + Environment.NewLine);
                sb.Append("    ON RP_CNT.COMPANY_ID = MT.COMPANY_ID" + Environment.NewLine);

                // 表示区分
                sb.Append("  LEFT JOIN SYS_M_NAME AS NM" + Environment.NewLine);
                sb.Append("    ON NM.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND NM.DISPLAY_FLG = 1 " + Environment.NewLine);
                sb.Append("   AND NM.DIVISION_ID = " + (int)CommonUtl.geNameKbn.DISPLAY_DIVISION_ID + Environment.NewLine);
                sb.Append("   AND NM.ID = MT.DISPLAY_FLG" + Environment.NewLine);

                #endregion

                sb.Append(" WHERE MT.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND MT.COMPANY_ID = " + companyId + Environment.NewLine);
                sb.Append("   AND MT.ID = " + groupId + Environment.NewLine);

                sb.Append(" ORDER BY MT.ID " + Environment.NewLine);

                #endregion

                dt = db.GetDataTable(sb.ToString());

                if (dt.DefaultView.Count > 0)
                {
                    entity = new EntityCompanyGroup();

                    // 排他制御
                    DataPgLock.geLovkFlg lockFlg;
                    string strErr = DataPgLock.SetLockPg(companyId, userId, PG_NM, groupId.ToString(), ipAdress, db, out lockFlg);
                    if (strErr != "")
                    {
                        entity.MESSAGE = CLASS_NM + ".GetCompanyGroup : 排他制御(ロック情報取得)に失敗しました。" + Environment.NewLine + strErr;
                    }

                    #region Set Entity

                    entity.company_id = ExCast.zCInt(companyId);
                    entity.id = ExCast.zCInt(dt.DefaultView[0]["ID"]);
                    entity.name = ExCast.zCStr(dt.DefaultView[0]["NAME"]);
                    entity.kana = ExCast.zCStr(dt.DefaultView[0]["KANA"]);
                    entity.about_name = ExCast.zCStr(dt.DefaultView[0]["ABOUT_NAME"]);

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

                    entity.estimate_no = ExCast.zCLng(dt.DefaultView[0]["ES_NO"]);
                    entity.order_no = ExCast.zCLng(dt.DefaultView[0]["JC_NO"]);
                    entity.sales_no = ExCast.zCLng(dt.DefaultView[0]["SA_NO"]);
                    entity.receipt_no = ExCast.zCLng(dt.DefaultView[0]["RP_NO"]);
                    entity.purchase_order_no = 0;
                    entity.purchase_no = 0;
                    entity.cash_payment_no = 0;
                    entity.produce_no = 0;
                    entity.ship_no = 0;

                    entity.estimate_approval_flg = ExCast.zCInt(dt.DefaultView[0]["ESTIMATE_APPROVAL_FLG"]);

                    entity.bank_nm = ExCast.zCStr(dt.DefaultView[0]["BANK_NAME"]);
                    entity.bank_branch_nm = ExCast.zCStr(dt.DefaultView[0]["BANK_BRANCH_NAME"]);
                    entity.bank_account_no = ExCast.zCStr(dt.DefaultView[0]["BANK_ACCOUNT_NO"]);
                    entity.bank_account_nm = ExCast.zCStr(dt.DefaultView[0]["BANK_ACCOUNT_NAME"]);
                    entity.bank_account_kana = ExCast.zCStr(dt.DefaultView[0]["BANK_ACCOUNT_KANA"]);
                    entity.invoice_print_flg = ExCast.zCInt(dt.DefaultView[0]["INVOICE_PRINT_FLG"]);

                    entity.display_division_id = ExCast.zCInt(dt.DefaultView[0]["DISPLAY_FLG"]);
                    entity.display_division_nm = ExCast.zCStr(dt.DefaultView[0]["DISPLAY_DIVISION_NAME"]);

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
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetCompanyGroup", ex);
                entity = new EntityCompanyGroup();
                entity.MESSAGE = CLASS_NM + ".GetCompanyGroup : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message.ToString();
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
                           "ID:" + groupId.ToString());

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
        public string UpdateCompanyGroup(string random, int type, int Id, EntityCompanyGroup entity)
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
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateCompanyGroup(認証処理)", ex);
                return CLASS_NM + ".UpdateCompanyGroup : 認証処理に失敗しました。" + Environment.NewLine + ex.Message.ToString();
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
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateCompanyGroup(DbOpen)", ex);
                return CLASS_NM + ".UpdateCompanyGroup(DbOpen) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
            }

            #endregion

            #region BeginTransaction

            try
            {
                db.ExBeginTransaction();
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateCompanyGroup(BeginTransaction)", ex);
                return CLASS_NM + ".UpdateCompanyGroup(BeginTransaction) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
            }

            #endregion

            #region Get Max Master ID

            if (type == 1 && Id == 0)
            {
                try
                {
                    DataMasterId.GetMaxMasterId(companyId,
                                                "",
                                                db,
                                                DataMasterId.geMasterMaxIdKbn.CompanyGroup,
                                                out _Id);

                    if (_Id == "")
                    {
                        return "ID取得に失敗しました。";
                    }
                }
                catch (Exception ex)
                {
                    CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateCompanyGroup(GetMaxMasterId)", ex);
                    return CLASS_NM + ".UpdateCompanyGroup(GetMaxMasterId) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
                }
            }
            else
            {
                _Id = Id.ToString();
            }

            #endregion

            #region Insert

            if (type == 1)
            {
                try
                {
                    #region Delete SQL

                    sb.Length = 0;
                    sb.Append("DELETE FROM SYS_M_COMPANY_GROUP " + Environment.NewLine);
                    sb.Append(" WHERE DELETE_FLG = 1 " + Environment.NewLine);
                    sb.Append("   AND COMPANY_ID = " + companyId + Environment.NewLine);
                    sb.Append("   AND ID = " + _Id + Environment.NewLine);

                    #endregion

                    db.ExecuteSQL(sb.ToString(), false);

                    #region Insert SQL

                    sb.Length = 0;
                    sb.Append("INSERT INTO SYS_M_COMPANY_GROUP " + Environment.NewLine);
                    sb.Append("       ( COMPANY_ID" + Environment.NewLine);
                    sb.Append("       , ID" + Environment.NewLine);
                    sb.Append("       , NAME" + Environment.NewLine);
                    sb.Append("       , KANA" + Environment.NewLine);
                    sb.Append("       , ABOUT_NAME" + Environment.NewLine);
                    sb.Append("       , ZIP_CODE" + Environment.NewLine);
                    sb.Append("       , PREFECTURE_ID" + Environment.NewLine);
                    sb.Append("       , CITY_ID" + Environment.NewLine);
                    sb.Append("       , TOWN_ID" + Environment.NewLine);
                    sb.Append("       , ADRESS_CITY" + Environment.NewLine);
                    sb.Append("       , ADRESS_TOWN" + Environment.NewLine);
                    sb.Append("       , ADRESS1" + Environment.NewLine);
                    sb.Append("       , ADRESS2" + Environment.NewLine);
                    sb.Append("       , TEL" + Environment.NewLine);
                    sb.Append("       , FAX" + Environment.NewLine);
                    sb.Append("       , MAIL_ADRESS" + Environment.NewLine);
                    sb.Append("       , MOBILE_TEL" + Environment.NewLine);
                    sb.Append("       , MOBILE_ADRESS" + Environment.NewLine);
                    sb.Append("       , URL" + Environment.NewLine);
                    sb.Append("       , ESTIMATE_APPROVAL_FLG" + Environment.NewLine);
                    sb.Append("       , BANK_NAME" + Environment.NewLine);
                    sb.Append("       , BANK_BRANCH_NAME" + Environment.NewLine);
                    sb.Append("       , BANK_ACCOUNT_NO" + Environment.NewLine);
                    sb.Append("       , BANK_ACCOUNT_NAME" + Environment.NewLine);
                    sb.Append("       , BANK_ACCOUNT_KANA" + Environment.NewLine);
                    sb.Append("       , INVOICE_PRINT_FLG" + Environment.NewLine);
                    sb.Append("       , MEMO" + Environment.NewLine);
                    sb.Append("       , DISPLAY_FLG" + Environment.NewLine);
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
                    sb.Append("SELECT  " + companyId + Environment.NewLine);                                                        // COMPANY_ID
                    sb.Append("       ," + _Id + Environment.NewLine);                                                              // ID
                    sb.Append("       ," + ExEscape.zRepStr(entity.name) + Environment.NewLine);                                    // NAME
                    sb.Append("       ," + ExEscape.zRepStr(entity.kana) + Environment.NewLine);                                    // KANA
                    sb.Append("       ," + ExEscape.zRepStr(entity.about_name) + Environment.NewLine);                              // ABOUT_NAME
                    sb.Append("       ," + ExCast.zNullToZero(entity.zip_code_from + entity.zip_code_to) + Environment.NewLine);    // ZIP_CODE
                    sb.Append("       ," + entity.prefecture_id + Environment.NewLine);                                             // PREFECTURE_ID
                    sb.Append("       ," + entity.city_id + Environment.NewLine);                                                   // CITY_ID
                    sb.Append("       ," + entity.town_id + Environment.NewLine);                                                   // TOWN_ID
                    sb.Append("       ," + ExEscape.zRepStr(entity.adress_city) + Environment.NewLine);                             // ADRESS_CITY
                    sb.Append("       ," + ExEscape.zRepStr(entity.adress_town) + Environment.NewLine);                             // ADRESS_TOWN
                    sb.Append("       ," + ExEscape.zRepStr(entity.adress1) + Environment.NewLine);                                 // ADRESS1
                    sb.Append("       ," + ExEscape.zRepStr(entity.adress2) + Environment.NewLine);                                 // ADRESS2
                    sb.Append("       ," + ExEscape.zRepStr(entity.tel) + Environment.NewLine);                                     // TEL
                    sb.Append("       ," + ExEscape.zRepStr(entity.fax) + Environment.NewLine);                                     // FAX
                    sb.Append("       ," + ExEscape.zRepStr(entity.mail_adress) + Environment.NewLine);                             // MAIL_ADRESS
                    sb.Append("       ," + ExEscape.zRepStr(entity.mobile_tel) + Environment.NewLine);                              // MOBILE_TEL
                    sb.Append("       ," + ExEscape.zRepStr(entity.mobile_adress) + Environment.NewLine);                           // MOBILE_ADRESS
                    sb.Append("       ," + ExEscape.zRepStr(entity.url) + Environment.NewLine);                                     // URL
                    sb.Append("       ," + entity.estimate_approval_flg + Environment.NewLine);                                     // ESTIMATE_APPROVAL_FLG
                    sb.Append("       ," + ExEscape.zRepStr(entity.bank_nm) + Environment.NewLine);                                 // BANK_NAME
                    sb.Append("       ," + ExEscape.zRepStr(entity.bank_branch_nm) + Environment.NewLine);                          // BANK_BRANCH_NAME
                    sb.Append("       ," + ExEscape.zRepStr(entity.bank_account_no) + Environment.NewLine);                         // BANK_ACCOUNT_NO
                    sb.Append("       ," + ExEscape.zRepStr(entity.bank_account_nm) + Environment.NewLine);                         // BANK_ACCOUNT_NAME
                    sb.Append("       ," + ExEscape.zRepStr(entity.bank_account_kana) + Environment.NewLine);                       // BANK_ACCOUNT_KANA
                    sb.Append("       ," + entity.invoice_print_flg + Environment.NewLine);                                         // INVOICE_PRINT_FLG
                    sb.Append("       ," + ExEscape.zRepStr(entity.memo) + Environment.NewLine);                                    // MEMO
                    sb.Append("       ," + entity.display_division_id + Environment.NewLine);                                       // DISPLAY_FLG
                    sb.Append(CommonUtl.GetInsSQLCommonColums(CommonUtl.UpdKbn.Ins,
                                                                PG_NM,
                                                                "SYS_M_COMPANY_GROUP",
                                                                ExCast.zCInt(personId),
                                                                _Id,
                                                                ipAdress,
                                                                userId));

                    #endregion

                    db.ExecuteSQL(sb.ToString(), false);

                    #region Sales Credit Balance Insert

                    try
                    {
                        #region Delete SQL

                        sb.Length = 0;
                        sb.Append("DELETE FROM M_SALES_CREDIT_BALANCE " + Environment.NewLine);
                        sb.Append(" WHERE COMPANY_ID = " + companyId + Environment.NewLine);    // COMPANY_ID
                        sb.Append("   AND GROUP_ID = " + _Id + Environment.NewLine);            // ID

                        #endregion

                        db.ExecuteSQL(sb.ToString(), false);

                        #region Insert SQL

                        sb.Length = 0;
                        sb.Append("INSERT INTO M_SALES_CREDIT_BALANCE " + Environment.NewLine);
                        sb.Append("       ( COMPANY_ID" + Environment.NewLine);
                        sb.Append("       , GROUP_ID" + Environment.NewLine);
                        sb.Append("       , ID" + Environment.NewLine);
                        sb.Append("       , SALES_CREDIT_INIT_PRICE" + Environment.NewLine);
                        sb.Append("       , SALES_CREDIT_PRICE" + Environment.NewLine);
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
                        sb.Append(" SELECT DISTINCT " + Environment.NewLine);
                        sb.Append("        " + companyId + Environment.NewLine);                    // COMPANY_ID
                        sb.Append("       ," + _Id + Environment.NewLine);                          // GROUP_ID
                        sb.Append("       ,INVOICE_ID " + Environment.NewLine);                  // ID
                        sb.Append("       ,0" + Environment.NewLine);                               // SALES_CREDIT_INIT_PRICE
                        sb.Append("       ,0" + Environment.NewLine);                               // SALES_CREDIT_PRICE
                        sb.Append(CommonUtl.GetInsSQLCommonColums(CommonUtl.UpdKbn.Ins,
                                                                    PG_NM,
                                                                    "M_SALES_CREDIT_BALANCE",
                                                                    ExCast.zCInt(personId),
                                                                    _Id,
                                                                    ipAdress,
                                                                    userId));
                        sb.Append("   FROM M_CUSTOMER " + Environment.NewLine);
                        sb.Append("  WHERE COMPANY_ID = " + companyId + Environment.NewLine);
                        sb.Append("    AND INVOICE_ID <> ''" + Environment.NewLine);
                        sb.Append("    AND INVOICE_ID <> 0" + Environment.NewLine);
                        sb.Append("    AND DELETE_FLG = 0" + Environment.NewLine);

                        #endregion

                        db.ExecuteSQL(sb.ToString(), false);
                    }
                    catch (Exception ex)
                    {
                        db.ExRollbackTransaction();
                        CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateCompanyGroup(Sales Credit Balance Insert)", ex);
                        return CLASS_NM + ".UpdateCompanyGroup(Sales Credit Balance Insert) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
                    }

                    #endregion

                    #region Payment Credit Balance Insert

                    try
                    {
                        #region Delete SQL

                        sb.Length = 0;
                        sb.Append("DELETE FROM M_PAYMENT_CREDIT_BALANCE " + Environment.NewLine);
                        sb.Append(" WHERE COMPANY_ID = " + companyId + Environment.NewLine);    // COMPANY_ID
                        sb.Append("   AND GROUP_ID = " + _Id + Environment.NewLine);            // ID

                        #endregion

                        db.ExecuteSQL(sb.ToString(), false);

                        #region Insert SQL

                        sb.Length = 0;
                        sb.Append("INSERT INTO M_PAYMENT_CREDIT_BALANCE " + Environment.NewLine);
                        sb.Append("       ( COMPANY_ID" + Environment.NewLine);
                        sb.Append("       , GROUP_ID" + Environment.NewLine);
                        sb.Append("       , ID" + Environment.NewLine);
                        sb.Append("       , PAYMENT_CREDIT_INIT_PRICE" + Environment.NewLine);
                        sb.Append("       , PAYMENT_CREDIT_PRICE" + Environment.NewLine);
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
                        sb.Append(" SELECT DISTINCT " + Environment.NewLine);
                        sb.Append("        " + companyId + Environment.NewLine);                    // COMPANY_ID
                        sb.Append("       ," + _Id + Environment.NewLine);                          // GROUP_ID
                        sb.Append("       ,ID " + Environment.NewLine);                             // ID
                        sb.Append("       ,0" + Environment.NewLine);                               // PAYMENT_CREDIT_INIT_PRICE
                        sb.Append("       ,0" + Environment.NewLine);                               // PAYMENT_CREDIT_PRICE
                        sb.Append(CommonUtl.GetInsSQLCommonColums(CommonUtl.UpdKbn.Ins,
                                                                    PG_NM,
                                                                    "M_PAYMENT_CREDIT_BALANCE",
                                                                    ExCast.zCInt(personId),
                                                                    _Id,
                                                                    ipAdress,
                                                                    userId));
                        sb.Append("   FROM M_PURCHASE " + Environment.NewLine);
                        sb.Append("  WHERE COMPANY_ID = " + companyId + Environment.NewLine);
                        sb.Append("    AND ID <> ''" + Environment.NewLine);
                        sb.Append("    AND ID <> 0" + Environment.NewLine);
                        sb.Append("    AND DELETE_FLG = 0" + Environment.NewLine);

                        #endregion

                        db.ExecuteSQL(sb.ToString(), false);
                    }
                    catch (Exception ex)
                    {
                        db.ExRollbackTransaction();
                        CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateCompanyGroup(Payment Credit Balance Insert)", ex);
                        return CLASS_NM + ".UpdateCompanyGroup(Payment Credit Balance Insert) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
                    }

                    #endregion

                    #region Commodity Inventory Insert

                    try
                    {
                        #region Delete SQL

                        sb.Length = 0;
                        sb.Append("DELETE FROM M_COMMODITY_INVENTORY " + Environment.NewLine);
                        sb.Append(" WHERE COMPANY_ID = " + companyId + Environment.NewLine);    // COMPANY_ID
                        sb.Append("   AND GROUP_ID = " + _Id + Environment.NewLine);            // ID

                        #endregion

                        db.ExecuteSQL(sb.ToString(), false);

                        #region Insert SQL

                        sb.Length = 0;
                        sb.Append("INSERT INTO M_COMMODITY_INVENTORY " + Environment.NewLine);
                        sb.Append("       ( COMPANY_ID" + Environment.NewLine);
                        sb.Append("       , GROUP_ID" + Environment.NewLine);
                        sb.Append("       , ID" + Environment.NewLine);
                        sb.Append("       , INVENTORY_NUMBER" + Environment.NewLine);
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
                        sb.Append(" SELECT DISTINCT " + Environment.NewLine);
                        sb.Append("        " + companyId + Environment.NewLine);                    // COMPANY_ID
                        sb.Append("       ," + _Id + Environment.NewLine);                          // GROUP_ID
                        sb.Append("       ,ID " + Environment.NewLine);                             // ID
                        sb.Append("       ,0" + Environment.NewLine);                               // INVENTORY_NUMBER
                        sb.Append(CommonUtl.GetInsSQLCommonColums(CommonUtl.UpdKbn.Ins,
                                                                    PG_NM,
                                                                    "M_COMMODITY_INVENTORY",
                                                                    ExCast.zCInt(personId),
                                                                    _Id,
                                                                    ipAdress,
                                                                    userId));
                        sb.Append("   FROM M_COMMODITY " + Environment.NewLine);
                        sb.Append("  WHERE COMPANY_ID = " + companyId + Environment.NewLine);
                        sb.Append("    AND ID <> ''" + Environment.NewLine);
                        sb.Append("    AND ID <> 0" + Environment.NewLine);
                        sb.Append("    AND DELETE_FLG = 0" + Environment.NewLine);

                        #endregion

                        db.ExecuteSQL(sb.ToString(), false);
                    }
                    catch (Exception ex)
                    {
                        db.ExRollbackTransaction();
                        CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateCompanyGroup(Commodity Inventory Insert)", ex);
                        return CLASS_NM + ".UpdateCompanyGroup(Commodity Inventory Insert) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
                    }

                    #endregion

                }
                catch (Exception ex)
                {
                    db.ExRollbackTransaction();
                    CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateCompanyGroup(Insert)", ex);
                    return CLASS_NM + ".UpdateCompanyGroup(Insert) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
                }

            }

            #endregion

            #region Update

            if (type == 0)
            {
                try
                {
                    #region SQL

                    sb.Length = 0;
                    sb.Append("UPDATE SYS_M_COMPANY_GROUP " + Environment.NewLine);
                    sb.Append(CommonUtl.GetUpdSQLCommonColums(PG_NM,
                                                               ExCast.zCInt(personId),
                                                               ipAdress,
                                                               userId,
                                                               0));
                    sb.Append("      ,NAME = " + ExEscape.zRepStr(entity.name) + Environment.NewLine);
                    sb.Append("      ,KANA = " + ExEscape.zRepStr(entity.kana) + Environment.NewLine);
                    sb.Append("      ,ABOUT_NAME = " + ExEscape.zRepStr(entity.about_name) + Environment.NewLine);
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
                    sb.Append("      ,ESTIMATE_APPROVAL_FLG = " + entity.estimate_approval_flg + Environment.NewLine);
                    sb.Append("      ,BANK_NAME = " + ExEscape.zRepStr(entity.bank_nm) + Environment.NewLine);
                    sb.Append("      ,BANK_BRANCH_NAME = " + ExEscape.zRepStr(entity.bank_branch_nm) + Environment.NewLine);
                    sb.Append("      ,BANK_ACCOUNT_NO = " + ExEscape.zRepStr(entity.bank_account_no) + Environment.NewLine);
                    sb.Append("      ,BANK_ACCOUNT_NAME = " + ExEscape.zRepStr(entity.bank_account_nm) + Environment.NewLine);
                    sb.Append("      ,BANK_ACCOUNT_KANA = " + ExEscape.zRepStr(entity.bank_account_kana) + Environment.NewLine);
                    sb.Append("      ,INVOICE_PRINT_FLG = " + entity.invoice_print_flg + Environment.NewLine);
                    sb.Append("      ,MEMO = " + ExEscape.zRepStr(entity.memo) + Environment.NewLine);
                    sb.Append("      ,DISPLAY_FLG = " + entity.display_division_id + Environment.NewLine);
                    sb.Append(" WHERE COMPANY_ID = " + companyId + Environment.NewLine);     // COMPANY_ID
                    sb.Append("   AND ID = " + Id + Environment.NewLine);                    // ID

                    #endregion

                    db.ExecuteSQL(sb.ToString(), false);

                }
                catch (Exception ex)
                {
                    db.ExRollbackTransaction();
                    CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateCompanyGroup(Update)", ex);
                    return CLASS_NM + ".UpdateCompanyGroup(Insert) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
                }
            }

            #endregion

            #region Delete

            if (type == 2)
            {
                #region Exist Data

                try
                {
                    bool _ret = false;
                    _ret = DataExists.IsExistData(db, companyId, "", "T_ESTIMATE_H", "GROUP_ID", ExCast.zNumZeroNothingFormat(Id), CommonUtl.geStrOrNumKbn.Number);
                    if (_ret == true)
                    {
                        return "ID : " + string.Format("{0:000}", Id) + " は見積データに使用されている為、削除できません。";
                    }

                    _ret = DataExists.IsExistData(db, companyId, "", "T_ORDER_H", "GROUP_ID", ExCast.zNumZeroNothingFormat(Id), CommonUtl.geStrOrNumKbn.Number);
                    if (_ret == true)
                    {
                        return "ID : " + string.Format("{0:000}", Id) + " は受注データに使用されている為、削除できません。";
                    }

                    _ret = DataExists.IsExistData(db, companyId, "", "T_SALES_H", "GROUP_ID", ExCast.zNumZeroNothingFormat(Id), CommonUtl.geStrOrNumKbn.Number);
                    if (_ret == true)
                    {
                        return "ID : " + string.Format("{0:000}", Id) + " は売上データに使用されている為、削除できません。";
                    }

                    _ret = DataExists.IsExistData(db, companyId, "", "T_INVOICE", "GROUP_ID", ExCast.zNumZeroNothingFormat(Id), CommonUtl.geStrOrNumKbn.Number);
                    if (_ret == true)
                    {
                        return "ID : " + string.Format("{0:000}", Id) + " は請求データに使用されている為、削除できません。";
                    }

                    _ret = DataExists.IsExistData(db, companyId, "", "T_RECEIPT_H", "GROUP_ID", ExCast.zNumZeroNothingFormat(Id), CommonUtl.geStrOrNumKbn.Number);
                    if (_ret == true)
                    {
                        return "ID : " + string.Format("{0:000}", Id) + " は入金データに使用されている為、削除できません。";
                    }

                    _ret = DataExists.IsExistData(db, companyId, "", "SYS_T_INQUIRY_H", "GROUP_ID", ExCast.zNumZeroNothingFormat(Id), CommonUtl.geStrOrNumKbn.Number);
                    if (_ret == true)
                    {
                        return "ID : " + string.Format("{0:000}", Id) + " は問い合わせデータに使用されている為、削除できません。";
                    }

                    _ret = DataExists.IsExistData(db, companyId, "", "T_DUTIES_COMMUNICATION", "GROUP_ID", ExCast.zNumZeroNothingFormat(Id), CommonUtl.geStrOrNumKbn.Number);
                    if (_ret == true)
                    {
                        return "ID : " + string.Format("{0:000}", Id) + " は業務連絡データに使用されている為、削除できません。";
                    }

                    _ret = DataExists.IsExistData(db, companyId, "", "M_SLIP_MANAGEMENT", "GROUP_ID", ExCast.zNumZeroNothingFormat(Id), CommonUtl.geStrOrNumKbn.Number);
                    if (_ret == true)
                    {
                        return "ID : " + string.Format("{0:000}", Id) + " は伝票管理マスタに使用されている為、削除できません。";
                    }

                    _ret = DataExists.IsExistData(db, companyId, "", "SYS_M_USER", "GROUP_ID", ExCast.zNumZeroNothingFormat(Id), CommonUtl.geStrOrNumKbn.Number);
                    if (_ret == true)
                    {
                        return "ID : " + string.Format("{0:000}", Id) + " はユーザマスタに使用されている為、削除できません。";
                    }

                }
                catch (Exception ex)
                {
                    db.ExRollbackTransaction();
                    CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateCompanyGroup(Exist Data)", ex);
                    return CLASS_NM + ".UpdateCompanyGroup(Exist Data) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
                }

                #endregion

                #region Delete Sales Credit Balance

                try
                {
                    sb.Length = 0;
                    sb.Append("UPDATE M_SALES_CREDIT_BALANCE " + Environment.NewLine);
                    sb.Append(CommonUtl.GetUpdSQLCommonColums(PG_NM,
                                                               ExCast.zCInt(personId),
                                                               ipAdress,
                                                               userId,
                                                               1));
                    sb.Append(" WHERE COMPANY_ID = " + companyId + Environment.NewLine);    // COMPANY_ID
                    sb.Append("   AND GROUP_ID = " + Id + Environment.NewLine);             // GROUP_ID

                    db.ExecuteSQL(sb.ToString(), false);
                }
                catch (Exception ex)
                {
                    db.ExRollbackTransaction();
                    CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateCompanyGroup(Delete Sales Credit Balance)", ex);
                    return CLASS_NM + ".UpdateCompanyGroup(Delete Sales Credit Balance) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
                }

                #endregion

                #region Delete Payment Credit Balance

                try
                {
                    sb.Length = 0;
                    sb.Append("UPDATE M_PAYMENT_CREDIT_BALANCE " + Environment.NewLine);
                    sb.Append(CommonUtl.GetUpdSQLCommonColums(PG_NM,
                                                               ExCast.zCInt(personId),
                                                               ipAdress,
                                                               userId,
                                                               1));
                    sb.Append(" WHERE COMPANY_ID = " + companyId + Environment.NewLine);    // COMPANY_ID
                    sb.Append("   AND GROUP_ID = " + Id + Environment.NewLine);             // GROUP_ID

                    db.ExecuteSQL(sb.ToString(), false);
                }
                catch (Exception ex)
                {
                    db.ExRollbackTransaction();
                    CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateCompanyGroup(Delete Payment Credit Balance)", ex);
                    return CLASS_NM + ".UpdateCompanyGroup(Delete Payment Credit Balance) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
                }

                #endregion

                #region Delete Commodity Inventory

                try
                {
                    sb.Length = 0;
                    sb.Append("UPDATE M_COMMODITY_INVENTORY " + Environment.NewLine);
                    sb.Append(CommonUtl.GetUpdSQLCommonColums(PG_NM,
                                                               ExCast.zCInt(personId),
                                                               ipAdress,
                                                               userId,
                                                               1));
                    sb.Append(" WHERE COMPANY_ID = " + companyId + Environment.NewLine);    // COMPANY_ID
                    sb.Append("   AND GROUP_ID = " + Id + Environment.NewLine);             // GROUP_ID

                    db.ExecuteSQL(sb.ToString(), false);
                }
                catch (Exception ex)
                {
                    db.ExRollbackTransaction();
                    CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateCompanyGroup(Commodity Inventory Delete)", ex);
                    return CLASS_NM + ".UpdateCompanyGroup(Commodity Inventory Delete) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
                }

                #endregion

                #region Update

                try
                {
                    sb.Length = 0;
                    sb.Append("UPDATE SYS_M_COMPANY_GROUP " + Environment.NewLine);
                    sb.Append(CommonUtl.GetUpdSQLCommonColums(PG_NM,
                                                               ExCast.zCInt(personId),
                                                               ipAdress,
                                                               userId,
                                                               1));
                    sb.Append(" WHERE COMPANY_ID = " + companyId + Environment.NewLine);    // COMPANY_ID
                    sb.Append("   AND ID = " + Id + Environment.NewLine);                   // ID

                    db.ExecuteSQL(sb.ToString(), false);

                }
                catch (Exception ex)
                {
                    db.ExRollbackTransaction();
                    CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateCompanyGroup(Delete)", ex);
                    return CLASS_NM + ".UpdateCompanyGroup(Delete) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
                }

                #endregion
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
                    CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateCompanyGroup(DelLockPg)", ex);
                    return CLASS_NM + ".UpdateCompanyGroup(DelLockPg) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
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
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateCompanyGroup(CommitTransaction)", ex);
                return CLASS_NM + ".UpdateCompanyGroup(CommitTransaction) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
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
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateCompanyGroup(DbClose)", ex);
                return CLASS_NM + ".UpdateCompanyGroup(DbClose) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
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
                                                   "ID:" + Id.ToString());
                        break;
                    case 1:
                        svcPgEvidence.gAddEvidence(ExCast.zCInt(HttpContext.Current.Session[ExSession.EVIDENCE_SAVE_FLG]),
                                                   companyId,
                                                   userId,
                                                   ipAdress,
                                                   sessionString,
                                                   PG_NM,
                                                   DataPgEvidence.geOperationType.Insert,
                                                   "ID:" + _Id.ToString());
                        break;
                    case 2:
                        svcPgEvidence.gAddEvidence(ExCast.zCInt(HttpContext.Current.Session[ExSession.EVIDENCE_SAVE_FLG]),
                                                   companyId,
                                                   userId,
                                                   ipAdress,
                                                   sessionString,
                                                   PG_NM,
                                                   DataPgEvidence.geOperationType.Delete,
                                                   "ID:" + Id.ToString());
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateCompanyGroup(Add Evidence)", ex);
                return CLASS_NM + ".UpdateCompanyGroup(Add Evidence) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
            }

            #endregion

            #region Return

            if (type == 1 && Id == 0)
            {
                return "Auto Insert success : " + "ID : " + _Id.ToString() + "で登録しました。";
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
