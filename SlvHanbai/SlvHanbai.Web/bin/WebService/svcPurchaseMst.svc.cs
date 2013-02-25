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
    public class svcPurchaseMst
    {
        private const string CLASS_NM = "svcPurchaseMst";
        private readonly string PG_NM = DataPgEvidence.PGName.Mst.Purchase;

        #region データ取得

        /// <summary> 
        /// 仕入先データ取得
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebMethod(EnableSession = true)]
        public EntityPurchaseMst GetPurchaseMst(string random, string Id)
        {

            EntityPurchaseMst entity;

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
                    entity = new EntityPurchaseMst();
                    entity.MESSAGE = _message;
                    return entity;
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetPurchaseMst(認証処理)", ex);
                entity = new EntityPurchaseMst();
                entity.MESSAGE = CLASS_NM + ".GetPurchaseMst : 認証処理に失敗しました。" + Environment.NewLine + ex.Message.ToString(); ;
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
                sb.Append("      ,NM1.DESCRIPTION AS BUSINESS_DIVISION_NAME " + Environment.NewLine);
                sb.Append("      ,NM2.DESCRIPTION AS UNIT_KIND_NAME " + Environment.NewLine);
                sb.Append("      ,NM3.DESCRIPTION AS TAX_CHANGE_NAME " + Environment.NewLine);
                sb.Append("      ,CN.NAME AS SUMMING_UP_GROUP_NAME " + Environment.NewLine);
                sb.Append("      ,NM4.DESCRIPTION AS PRICE_FRACTION_PROC_NAME " + Environment.NewLine);
                sb.Append("      ,NM5.DESCRIPTION AS TAX_FRACTION_PROC_NAME " + Environment.NewLine);
                sb.Append("      ,RD.DESCRIPTION AS PAYMENT_DIVISION_NAME " + Environment.NewLine);
                sb.Append("      ,NM6.DESCRIPTION AS PAYMENT_CYCLE_NAME " + Environment.NewLine);
                sb.Append("      ,CL1.NAME AS GROUP1_NAME " + Environment.NewLine);
                sb.Append("      ,CL2.NAME AS GROUP2_NAME " + Environment.NewLine);
                sb.Append("      ,CL3.NAME AS GROUP3_NAME " + Environment.NewLine);
                sb.Append("      ,NM.DESCRIPTION AS DISPLAY_DIVISION_NAME " + Environment.NewLine);
                sb.Append("  FROM M_PURCHASE AS MT" + Environment.NewLine);

                #region Join

                // 取引区分
                sb.Append("  LEFT JOIN SYS_M_NAME AS NM1" + Environment.NewLine);
                sb.Append("    ON NM1.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND NM1.DISPLAY_FLG = 1 " + Environment.NewLine);
                sb.Append("   AND NM1.DIVISION_ID = " + (int)CommonUtl.geNameKbn.BUSINESS_DIVISION_PU_ID + Environment.NewLine);
                sb.Append("   AND NM1.ID = MT.BUSINESS_DIVISION_ID" + Environment.NewLine);

                // 単価種類
                sb.Append("  LEFT JOIN SYS_M_NAME AS NM2" + Environment.NewLine);
                sb.Append("    ON NM2.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND NM2.DISPLAY_FLG = 1 " + Environment.NewLine);
                sb.Append("   AND NM2.DIVISION_ID = " + (int)CommonUtl.geNameKbn.UNIT_PRICE_DIVISION_PU_ID + Environment.NewLine);
                sb.Append("   AND NM2.ID = MT.UNIT_KIND_ID" + Environment.NewLine);

                // 税転換(課税区分)
                sb.Append("  LEFT JOIN SYS_M_NAME AS NM3" + Environment.NewLine);
                sb.Append("    ON NM3.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND NM3.DISPLAY_FLG = 1 " + Environment.NewLine);
                sb.Append("   AND NM3.DIVISION_ID = " + (int)CommonUtl.geNameKbn.TAX_CHANGE_PU_ID + Environment.NewLine);
                sb.Append("   AND NM3.ID = MT.TAX_CHANGE_ID" + Environment.NewLine);

                // 締グループ
                sb.Append("  LEFT JOIN M_CONDITION AS CN" + Environment.NewLine);
                sb.Append("    ON CN.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND CN.DISPLAY_FLG = 1 " + Environment.NewLine);
                sb.Append("   AND CN.COMPANY_ID = MT.COMPANY_ID" + Environment.NewLine);
                sb.Append("   AND CN.CONDITION_DIVISION_ID = 1" + Environment.NewLine);
                sb.Append("   AND CN.ID = MT.SUMMING_UP_GROUP_ID" + Environment.NewLine);

                // 金額端数処理
                sb.Append("  LEFT JOIN SYS_M_NAME AS NM4" + Environment.NewLine);
                sb.Append("    ON NM4.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND NM4.DISPLAY_FLG = 1 " + Environment.NewLine);
                sb.Append("   AND NM4.DIVISION_ID = " + (int)CommonUtl.geNameKbn.FRACTION_PROC_ID + Environment.NewLine);
                sb.Append("   AND NM4.ID = MT.PRICE_FRACTION_PROC_ID" + Environment.NewLine);

                // 税端数処理
                sb.Append("  LEFT JOIN SYS_M_NAME AS NM5" + Environment.NewLine);
                sb.Append("    ON NM5.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND NM5.DISPLAY_FLG = 1 " + Environment.NewLine);
                sb.Append("   AND NM5.DIVISION_ID = " + (int)CommonUtl.geNameKbn.FRACTION_PROC_ID + Environment.NewLine);
                sb.Append("   AND NM5.ID = MT.TAX_FRACTION_PROC_ID" + Environment.NewLine);

                // 支払区分
                sb.Append("  LEFT JOIN SYS_M_RECEIPT_DIVISION AS RD" + Environment.NewLine);
                sb.Append("    ON RD.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND RD.DISPLAY_FLG = 1 " + Environment.NewLine);
                sb.Append("   AND RD.COMPANY_ID = MT.COMPANY_ID" + Environment.NewLine);
                sb.Append("   AND RD.ID = MT.PAYMENT_DIVISION_ID" + Environment.NewLine);

                // 支払サイクル
                sb.Append("  LEFT JOIN SYS_M_NAME AS NM6" + Environment.NewLine);
                sb.Append("    ON NM6.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND NM6.DISPLAY_FLG = 1 " + Environment.NewLine);
                sb.Append("   AND NM6.DIVISION_ID = " + (int)CommonUtl.geNameKbn.COLLECT_CYCLE_ID + Environment.NewLine);
                sb.Append("   AND NM6.ID = MT.PAYMENT_CYCLE_ID" + Environment.NewLine);

                // 仕入先分類1
                sb.Append("  LEFT JOIN M_CLASS AS CL1" + Environment.NewLine);
                sb.Append("    ON CL1.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND CL1.DISPLAY_FLG = 1 " + Environment.NewLine);
                sb.Append("   AND CL1.COMPANY_ID = MT.COMPANY_ID" + Environment.NewLine);
                sb.Append("   AND CL1.CLASS_DIVISION_ID = " + (int)CommonUtl.geMGroupKbn.PurchaseGrouop1 + Environment.NewLine);
                sb.Append("   AND CL1.ID = MT.GROUP1_ID" + Environment.NewLine);

                // 仕入先分類2
                sb.Append("  LEFT JOIN M_CLASS AS CL2" + Environment.NewLine);
                sb.Append("    ON CL2.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND CL2.DISPLAY_FLG = 1 " + Environment.NewLine);
                sb.Append("   AND CL2.COMPANY_ID = MT.COMPANY_ID" + Environment.NewLine);
                sb.Append("   AND CL2.CLASS_DIVISION_ID = " + (int)CommonUtl.geMGroupKbn.PurchaseGrouop2 + Environment.NewLine);
                sb.Append("   AND CL2.ID = MT.GROUP2_ID" + Environment.NewLine);

                // 仕入先分類3
                sb.Append("  LEFT JOIN M_CLASS AS CL3" + Environment.NewLine);
                sb.Append("    ON CL3.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND CL3.DISPLAY_FLG = 1 " + Environment.NewLine);
                sb.Append("   AND CL3.COMPANY_ID = MT.COMPANY_ID" + Environment.NewLine);
                sb.Append("   AND CL3.CLASS_DIVISION_ID = " + (int)CommonUtl.geMGroupKbn.PurchaseGrouop3 + Environment.NewLine);
                sb.Append("   AND CL3.ID = MT.GROUP3_ID" + Environment.NewLine);

                // 表示区分
                sb.Append("  LEFT JOIN SYS_M_NAME AS NM" + Environment.NewLine);
                sb.Append("    ON NM.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND NM.DISPLAY_FLG = 1 " + Environment.NewLine);
                sb.Append("   AND NM.DIVISION_ID = " + (int)CommonUtl.geNameKbn.DISPLAY_DIVISION_ID + Environment.NewLine);
                sb.Append("   AND NM.ID = MT.DISPLAY_FLG" + Environment.NewLine);

                #endregion

                sb.Append(" WHERE MT.COMPANY_ID = " + companyId + Environment.NewLine);
                sb.Append("   AND MT.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND MT.ID = " + ExEscape.zRepStr(ExCast.zNumZeroNothingFormat(Id)) + Environment.NewLine);

                sb.Append(" ORDER BY MT.COMPANY_ID " + Environment.NewLine);
                sb.Append("         ,MT.ID " + Environment.NewLine);

                #endregion

                dt = db.GetDataTable(sb.ToString());

                if (dt.DefaultView.Count > 0)
                {
                    entity = new EntityPurchaseMst();

                    // 排他制御
                    DataPgLock.geLovkFlg lockFlg;
                    string strErr = DataPgLock.SetLockPg(companyId, userId, PG_NM, Id.ToString(), ipAdress, db, out lockFlg);
                    if (strErr != "")
                    {
                        entity.MESSAGE = CLASS_NM + ".GetPurchaseMst : 排他制御(ロック情報取得)に失敗しました。" + Environment.NewLine + strErr;
                    }

                    #region Set Entity

                    entity.id = ExCast.zCStr(dt.DefaultView[0]["ID"]);
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
                    entity.station_name = ExCast.zCStr(dt.DefaultView[0]["STATION_NAME"]);
                    entity.post_name = ExCast.zCStr(dt.DefaultView[0]["POST_NAME"]);
                    entity.person_name = ExCast.zCStr(dt.DefaultView[0]["PERSON_NAME"]);
                    entity.title_id = ExCast.zCInt(dt.DefaultView[0]["TITLE_ID"]);
                    entity.title_name = ExCast.zCStr(dt.DefaultView[0]["TITLE_NAME"]);
                    entity.tel = ExCast.zCStr(dt.DefaultView[0]["TEL"]);
                    entity.fax = ExCast.zCStr(dt.DefaultView[0]["FAX"]);
                    entity.mail_adress = ExCast.zCStr(dt.DefaultView[0]["MAIL_ADRESS"]);
                    entity.mobile_tel = ExCast.zCStr(dt.DefaultView[0]["MOBILE_TEL"]);
                    entity.mobile_adress = ExCast.zCStr(dt.DefaultView[0]["MOBILE_ADRESS"]);
                    entity.url = ExCast.zCStr(dt.DefaultView[0]["URL"]);

                    entity.business_division_id = ExCast.zCInt(dt.DefaultView[0]["BUSINESS_DIVISION_ID"]);
                    entity.business_division_nm = ExCast.zCStr(dt.DefaultView[0]["BUSINESS_DIVISION_NAME"]);

                    entity.unit_kind_id = ExCast.zCInt(dt.DefaultView[0]["UNIT_KIND_ID"]);
                    entity.unit_kind_nm = ExCast.zCStr(dt.DefaultView[0]["UNIT_KIND_NAME"]);

                    entity.credit_rate = ExCast.zCInt(dt.DefaultView[0]["CREDIT_RATE"]);

                    entity.tax_change_id = ExCast.zCInt(dt.DefaultView[0]["TAX_CHANGE_ID"]);
                    entity.tax_change_nm = ExCast.zCStr(dt.DefaultView[0]["TAX_CHANGE_NAME"]);

                    entity.summing_up_group_id = ExCast.zCStr(dt.DefaultView[0]["SUMMING_UP_GROUP_ID"]);
                    entity.summing_up_group_nm = ExCast.zCStr(dt.DefaultView[0]["SUMMING_UP_GROUP_NAME"]);

                    entity.price_fraction_proc_id = ExCast.zCInt(dt.DefaultView[0]["PRICE_FRACTION_PROC_ID"]);
                    entity.price_fraction_proc_nm = ExCast.zCStr(dt.DefaultView[0]["PRICE_FRACTION_PROC_NAME"]);

                    entity.tax_fraction_proc_id = ExCast.zCInt(dt.DefaultView[0]["TAX_FRACTION_PROC_ID"]);
                    entity.tax_fraction_proc_nm = ExCast.zCStr(dt.DefaultView[0]["TAX_FRACTION_PROC_NAME"]);

                    entity.payment_credit_price = ExCast.zCDbl(dt.DefaultView[0]["PAYMENT_CREDIT_PRICE"]);

                    entity.payment_division_id = ExCast.zCStr(dt.DefaultView[0]["PAYMENT_DIVISION_ID"]);
                    entity.payment_division_nm = ExCast.zCStr(dt.DefaultView[0]["PAYMENT_DIVISION_NAME"]);

                    entity.payment_cycle_id = ExCast.zCInt(dt.DefaultView[0]["PAYMENT_CYCLE_ID"]);
                    entity.payment_cycle_nm = ExCast.zCStr(dt.DefaultView[0]["PAYMENT_CYCLE_NAME"]);

                    entity.payment_day = ExCast.zCInt(dt.DefaultView[0]["PAYMENT_DAY"]);
                    entity.bill_site = ExCast.zCInt(dt.DefaultView[0]["BILL_SITE"]);

                    entity.group1_id = ExCast.zCStr(dt.DefaultView[0]["GROUP1_ID"]);
                    entity.group1_nm = ExCast.zCStr(dt.DefaultView[0]["GROUP1_NAME"]);

                    entity.group2_id = ExCast.zCStr(dt.DefaultView[0]["GROUP2_ID"]);
                    entity.group2_nm = ExCast.zCStr(dt.DefaultView[0]["GROUP2_NAME"]);

                    entity.group3_id = ExCast.zCStr(dt.DefaultView[0]["GROUP3_ID"]);
                    entity.group3_nm = ExCast.zCStr(dt.DefaultView[0]["GROUP3_NAME"]);

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
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetPurchaseMst", ex);
                entity = new EntityPurchaseMst();
                entity.MESSAGE = CLASS_NM + ".GetPurchaseMst : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message.ToString();
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
                           "ID:" + Id.ToString());

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
        public string UpdatePurchaseMst(string random, int type, string Id, EntityPurchaseMst entity)
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
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdatePurchaseMst(認証処理)", ex);
                return CLASS_NM + ".UpdatePurchaseMst : 認証処理に失敗しました。" + Environment.NewLine + ex.Message.ToString();
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
                db = new ExMySQLData(ExCast.zCStr(HttpContext.Current.Session[ExSession.DB_CONNECTION_STR]));
                db.DbOpen();
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdatePurchaseMst(DbOpen)", ex);
                return CLASS_NM + ".UpdatePurchaseMst(DbOpen) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
            }

            #endregion

            #region BeginTransaction

            try
            {
                db.ExBeginTransaction();
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdatePurchaseMst(BeginTransaction)", ex);
                return CLASS_NM + ".UpdatePurchaseMst(BeginTransaction) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
            }

            #endregion

            #region Get Max Master ID

            if (type == 1 && (Id == "" || Id == "0"))
            {
                try
                {
                    DataMasterId.GetMaxMasterId(companyId,
                                                "",
                                                db,
                                                DataMasterId.geMasterMaxIdKbn.Purchase,
                                                out _Id);

                    if (_Id == "")
                    {
                        return "ID取得に失敗しました。";
                    }
                }
                catch (Exception ex)
                {
                    CommonUtl.ExLogger.Error(CLASS_NM + ".UpdatePurchaseMst(GetMaxMasterId)", ex);
                    return CLASS_NM + ".UpdatePurchaseMst(GetMaxMasterId) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
                }
            }
            else
            {
                _Id = Id;
            }

            #endregion

            #region Insert

            if (type == 1)
            {
                try
                {
                    #region Delete SQL

                    sb.Length = 0;
                    sb.Append("DELETE FROM M_PURCHASE " + Environment.NewLine);
                    sb.Append(" WHERE DELETE_FLG = 1 " + Environment.NewLine);
                    sb.Append("   AND COMPANY_ID = " + companyId + Environment.NewLine);
                    sb.Append("   AND ID = " + ExEscape.zRepStr(ExCast.zNumZeroNothingFormat(_Id)) + Environment.NewLine);

                    #endregion

                    db.ExecuteSQL(sb.ToString(), false);

                    #region Insert SQL

                    sb.Length = 0;
                    sb.Append("INSERT INTO M_PURCHASE " + Environment.NewLine);
                    sb.Append("       ( COMPANY_ID" + Environment.NewLine);
                    sb.Append("       , ID" + Environment.NewLine);
                    sb.Append("       , ID2" + Environment.NewLine);
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
                    sb.Append("       , STATION_NAME" + Environment.NewLine);
                    sb.Append("       , POST_NAME" + Environment.NewLine);
                    sb.Append("       , PERSON_NAME" + Environment.NewLine);
                    sb.Append("       , TITLE_ID" + Environment.NewLine);
                    sb.Append("       , TITLE_NAME" + Environment.NewLine);
                    sb.Append("       , TEL" + Environment.NewLine);
                    sb.Append("       , FAX" + Environment.NewLine);
                    sb.Append("       , MAIL_ADRESS" + Environment.NewLine);
                    sb.Append("       , MOBILE_TEL" + Environment.NewLine);
                    sb.Append("       , MOBILE_ADRESS" + Environment.NewLine);
                    sb.Append("       , URL" + Environment.NewLine);
                    sb.Append("       , BUSINESS_DIVISION_ID" + Environment.NewLine);
                    sb.Append("       , UNIT_KIND_ID" + Environment.NewLine);
                    sb.Append("       , CREDIT_RATE" + Environment.NewLine);
                    sb.Append("       , TAX_CHANGE_ID" + Environment.NewLine);
                    sb.Append("       , SUMMING_UP_GROUP_ID" + Environment.NewLine);
                    sb.Append("       , PRICE_FRACTION_PROC_ID" + Environment.NewLine);
                    sb.Append("       , TAX_FRACTION_PROC_ID" + Environment.NewLine);
                    sb.Append("       , PAYMENT_CREDIT_PRICE" + Environment.NewLine);
                    sb.Append("       , PAYMENT_DIVISION_ID" + Environment.NewLine);
                    sb.Append("       , PAYMENT_CYCLE_ID" + Environment.NewLine);
                    sb.Append("       , PAYMENT_DAY" + Environment.NewLine);
                    sb.Append("       , BILL_SITE" + Environment.NewLine);
                    sb.Append("       , GROUP1_ID" + Environment.NewLine);
                    sb.Append("       , GROUP2_ID" + Environment.NewLine);
                    sb.Append("       , GROUP3_ID" + Environment.NewLine);
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
                    sb.Append("       ," + ExEscape.zRepStr(ExCast.zNumZeroNothingFormat(_Id)) + Environment.NewLine);              // ID
                    sb.Append("       ," + ExCast.zIdForNumIndex(_Id) + Environment.NewLine);                                       // ID2
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
                    sb.Append("       ," + ExEscape.zRepStr(entity.station_name) + Environment.NewLine);                            // STATION_NAME
                    sb.Append("       ," + ExEscape.zRepStr(entity.post_name) + Environment.NewLine);                               // POST_NAME
                    sb.Append("       ," + ExEscape.zRepStr(entity.person_name) + Environment.NewLine);                             // PERSON_NAME
                    sb.Append("       ," + entity.title_id + Environment.NewLine);                                                  // TITLE_ID
                    sb.Append("       ," + ExEscape.zRepStr(entity.title_name) + Environment.NewLine);                              // TITLE_NAME
                    sb.Append("       ," + ExEscape.zRepStr(entity.tel) + Environment.NewLine);                                     // TEL
                    sb.Append("       ," + ExEscape.zRepStr(entity.fax) + Environment.NewLine);                                     // FAX
                    sb.Append("       ," + ExEscape.zRepStr(entity.mail_adress) + Environment.NewLine);                             // MAIL_ADRESS
                    sb.Append("       ," + ExEscape.zRepStr(entity.mobile_tel) + Environment.NewLine);                              // MOBILE_TEL
                    sb.Append("       ," + ExEscape.zRepStr(entity.mobile_adress) + Environment.NewLine);                           // MOBILE_ADRESS
                    sb.Append("       ," + ExEscape.zRepStr(entity.url) + Environment.NewLine);                                     // URL
                    sb.Append("       ," + entity.business_division_id + Environment.NewLine);                                      // BUSINESS_DIVISION_ID
                    sb.Append("       ," + entity.unit_kind_id + Environment.NewLine);                                              // UNIT_KIND_ID
                    sb.Append("       ," + entity.credit_rate + Environment.NewLine);                                               // CREDIT_RATE
                    sb.Append("       ," + entity.tax_change_id + Environment.NewLine);                                             // TAX_CHANGE_ID
                    sb.Append("       ," + ExEscape.zRepStr(entity.summing_up_group_id) + Environment.NewLine);                     // SUMMING_UP_GROUP_ID
                    sb.Append("       ," + entity.price_fraction_proc_id + Environment.NewLine);                                    // PRICE_FRACTION_PROC_ID
                    sb.Append("       ," + entity.tax_fraction_proc_id + Environment.NewLine);                                      // TAX_FRACTION_PROC_ID
                    sb.Append("       ," + entity.payment_credit_price + Environment.NewLine);                                      // PAYMENT_CREDIT_PRICE
                    sb.Append("       ," + ExEscape.zRepStr(entity.payment_division_id) + Environment.NewLine);                     // PAYMENT_DIVISION_ID
                    sb.Append("       ," + entity.payment_cycle_id + Environment.NewLine);                                          // PAYMENT_CYCLE_ID
                    sb.Append("       ," + entity.payment_day + Environment.NewLine);                                               // PAYMENT_DAY
                    sb.Append("       ," + entity.bill_site + Environment.NewLine);                                                 // BILL_SITE
                    sb.Append("       ," + ExEscape.zRepStr(entity.group1_id) + Environment.NewLine);                               // GROUP1_ID
                    sb.Append("       ," + ExEscape.zRepStr(entity.group2_id) + Environment.NewLine);                               // GROUP2_ID
                    sb.Append("       ," + ExEscape.zRepStr(entity.group3_id) + Environment.NewLine);                               // GROUP3_ID
                    sb.Append("       ," + ExEscape.zRepStr(entity.memo) + Environment.NewLine);                                    // MEMO
                    sb.Append("       ," + entity.display_division_id + Environment.NewLine);                                       // DISPLAY_FLG
                    sb.Append(CommonUtl.GetInsSQLCommonColums(CommonUtl.UpdKbn.Ins,
                                                                PG_NM,
                                                                "M_PURCHASE",
                                                                ExCast.zCInt(personId),
                                                                _Id,
                                                                ipAdress,
                                                                userId));

                    #endregion

                    db.ExecuteSQL(sb.ToString(), false);

                    #region Payment Credit Balance Insert

                    try
                    {
                        #region Delete SQL

                        sb.Length = 0;
                        sb.Append("DELETE FROM M_PAYMENT_CREDIT_BALANCE " + Environment.NewLine);
                        sb.Append(" WHERE COMPANY_ID = " + companyId + Environment.NewLine);    // COMPANY_ID
                        sb.Append("   AND ID = " + ExEscape.zRepStr(ExCast.zNumZeroNothingFormat(_Id)) + Environment.NewLine);            // ID

                        #endregion

                        db.ExecuteSQL(sb.ToString(), false);

                        #region SQL

                        sb.Length = 0;
                        sb.Append("SELECT MT.* " + Environment.NewLine);
                        sb.Append("  FROM SYS_M_COMPANY_GROUP AS MT" + Environment.NewLine);
                        sb.Append(" WHERE MT.COMPANY_ID = " + companyId + Environment.NewLine);
                        sb.Append("   AND MT.DELETE_FLG = 0 " + Environment.NewLine);
                        sb.Append(" ORDER BY MT.ID " + Environment.NewLine);

                        #endregion

                        dt = db.GetDataTable(sb.ToString());

                        if (dt.DefaultView.Count > 0)
                        {
                            for (int i = 0; i <= dt.DefaultView.Count - 1; i++)
                            {
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
                                sb.Append("SELECT  " + companyId + Environment.NewLine);                                                        // COMPANY_ID
                                sb.Append("       ," + ExCast.zCStr(dt.DefaultView[i]["ID"]) + Environment.NewLine);                            // GROUP_ID
                                sb.Append("       ," + ExEscape.zRepStr(ExCast.zNumZeroNothingFormat(_Id)) + Environment.NewLine);              // ID
                                sb.Append("       ,0" + Environment.NewLine);                                                                   // SALES_CREDIT_INIT_PRICE
                                sb.Append("       ,0" + Environment.NewLine);                                                                   // SALES_CREDIT_PRICE
                                sb.Append(CommonUtl.GetInsSQLCommonColums(CommonUtl.UpdKbn.Ins,
                                                                            PG_NM,
                                                                            "M_PAYMENT_CREDIT_BALANCE",
                                                                            ExCast.zCInt(personId),
                                                                            _Id,
                                                                            ipAdress,
                                                                            userId));

                                #endregion

                                db.ExecuteSQL(sb.ToString(), false);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        db.ExRollbackTransaction();
                        CommonUtl.ExLogger.Error(CLASS_NM + ".UpdatePurchaseMst(Payment Credit Balance Insert)", ex);
                        return CLASS_NM + ".UpdatePurchaseMst(Payment Credit Balance Insert) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
                    }

                    #endregion

                }
                catch (Exception ex)
                {
                    db.ExRollbackTransaction();
                    CommonUtl.ExLogger.Error(CLASS_NM + ".UpdatePurchaseMst(Insert)", ex);
                    return CLASS_NM + ".UpdatePurchaseMst(Insert) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
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
                    sb.Append("UPDATE M_PURCHASE " + Environment.NewLine);
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
                    sb.Append("      ,STATION_NAME = " + ExEscape.zRepStr(entity.station_name) + Environment.NewLine);
                    sb.Append("      ,POST_NAME = " + ExEscape.zRepStr(entity.post_name) + Environment.NewLine);
                    sb.Append("      ,PERSON_NAME = " + ExEscape.zRepStr(entity.person_name) + Environment.NewLine);
                    sb.Append("      ,TITLE_ID = " + entity.title_id + Environment.NewLine);
                    sb.Append("      ,TITLE_NAME = " + ExEscape.zRepStr(entity.title_name) + Environment.NewLine);
                    sb.Append("      ,TEL = " + ExEscape.zRepStr(entity.tel) + Environment.NewLine);
                    sb.Append("      ,FAX = " + ExEscape.zRepStr(entity.fax) + Environment.NewLine);
                    sb.Append("      ,MAIL_ADRESS = " + ExEscape.zRepStr(entity.mail_adress) + Environment.NewLine);
                    sb.Append("      ,MOBILE_TEL = " + ExEscape.zRepStr(entity.mobile_tel) + Environment.NewLine);
                    sb.Append("      ,MOBILE_ADRESS = " + ExEscape.zRepStr(entity.mobile_adress) + Environment.NewLine);
                    sb.Append("      ,URL = " + ExEscape.zRepStr(entity.url) + Environment.NewLine);
                    sb.Append("      ,BUSINESS_DIVISION_ID = " + entity.business_division_id + Environment.NewLine);
                    sb.Append("      ,UNIT_KIND_ID = " + entity.unit_kind_id + Environment.NewLine);
                    sb.Append("      ,CREDIT_RATE = " + entity.credit_rate + Environment.NewLine);
                    sb.Append("      ,TAX_CHANGE_ID = " + entity.tax_change_id + Environment.NewLine);
                    sb.Append("      ,SUMMING_UP_GROUP_ID = " + ExEscape.zRepStr(entity.summing_up_group_id) + Environment.NewLine);
                    sb.Append("      ,PRICE_FRACTION_PROC_ID = " + entity.price_fraction_proc_id + Environment.NewLine);
                    sb.Append("      ,TAX_FRACTION_PROC_ID = " + entity.tax_fraction_proc_id + Environment.NewLine);
                    sb.Append("      ,PAYMENT_CREDIT_PRICE = " + entity.payment_credit_price + Environment.NewLine);
                    sb.Append("      ,PAYMENT_DIVISION_ID = " + ExEscape.zRepStr(entity.payment_division_id) + Environment.NewLine);
                    sb.Append("      ,PAYMENT_CYCLE_ID = " + entity.payment_cycle_id + Environment.NewLine);
                    sb.Append("      ,PAYMENT_DAY = " + entity.payment_day + Environment.NewLine);
                    sb.Append("      ,BILL_SITE = " + entity.bill_site + Environment.NewLine);
                    sb.Append("      ,GROUP1_ID = " + ExEscape.zRepStr(entity.group1_id) + Environment.NewLine);
                    sb.Append("      ,GROUP2_ID = " + ExEscape.zRepStr(entity.group2_id) + Environment.NewLine);
                    sb.Append("      ,GROUP3_ID = " + ExEscape.zRepStr(entity.group3_id) + Environment.NewLine);
                    sb.Append("      ,MEMO = " + ExEscape.zRepStr(entity.memo) + Environment.NewLine);
                    sb.Append("      ,DISPLAY_FLG = " + entity.display_division_id + Environment.NewLine);
                    sb.Append(" WHERE COMPANY_ID = " + companyId + Environment.NewLine);                            // COMPANY_ID
                    sb.Append("   AND ID = " + ExEscape.zRepStr(ExCast.zNumZeroNothingFormat(Id)) + Environment.NewLine);     // ID

                    #endregion

                    db.ExecuteSQL(sb.ToString(), false);

                }
                catch (Exception ex)
                {
                    db.ExRollbackTransaction();
                    CommonUtl.ExLogger.Error(CLASS_NM + ".UpdatePurchaseMst(Update)", ex);
                    return CLASS_NM + ".UpdatePurchaseMst(Insert) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
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

                    // 仕入先チェック
                    //_ret = DataExists.IsExistData(db, companyId, "", "T_ESTIMATE_H", "CUSTOMER_ID", ExCast.zNumZeroNothingFormat(Id), CommonUtl.geStrOrNumKbn.String);
                    //if (_ret == true)
                    //{
                    //    return "ID : " + Id + " は見積データの仕入先に使用されている為、削除できません。";
                    //}

                    //_ret = DataExists.IsExistData(db, companyId, "", "T_ORDER_H", "CUSTOMER_ID", ExCast.zNumZeroNothingFormat(Id), CommonUtl.geStrOrNumKbn.String);
                    //if (_ret == true)
                    //{
                    //    return "ID : " + Id + " は受注データの仕入先に使用されている為、削除できません。";
                    //}

                    //_ret = DataExists.IsExistData(db, companyId, "", "T_SALES_H", "CUSTOMER_ID", ExCast.zNumZeroNothingFormat(Id), CommonUtl.geStrOrNumKbn.String);
                    //if (_ret == true)
                    //{
                    //    return "ID : " + Id + " は売上データの仕入先に使用されている為、削除できません。";
                    //}

                    //if (ExCast.zNumZeroNothingFormat(Id) != ExCast.zNumZeroNothingFormat(entity.invoice_id))
                    //{
                    //    _ret = DataExists.IsExistData(db, companyId, "", "M_PURCHASE", "INVOICE_ID", ExCast.zNumZeroNothingFormat(Id), CommonUtl.geStrOrNumKbn.String);
                    //    if (_ret == true)
                    //    {
                    //        return "ID : " + Id + " は仕入先マスタの請求IDに使用されている為、削除できません。";
                    //    }
                    //}

                    //// 請求先チェック
                    //if (ExCast.zNumZeroNothingFormat(Id) == ExCast.zNumZeroNothingFormat(entity.invoice_id))
                    //{
                    //    _ret = DataExists.IsExistData(db, companyId, "", "T_SALES_H", "INVOICE_ID", ExCast.zNumZeroNothingFormat(Id), CommonUtl.geStrOrNumKbn.String);
                    //    if (_ret == true)
                    //    {
                    //        return "ID : " + Id + " は売上データの請求先に使用されている為、削除できません。";
                    //    }

                    //    _ret = DataExists.IsExistData(db, companyId, "", "T_INVOICE", "INVOICE_ID", ExCast.zNumZeroNothingFormat(Id), CommonUtl.geStrOrNumKbn.String);
                    //    if (_ret == true)
                    //    {
                    //        return "ID : " + Id + " は請求データの請求先に使用されている為、削除できません。";
                    //    }

                    //    _ret = DataExists.IsExistData(db, companyId, "", "T_RECEIPT_H", "INVOICE_ID", ExCast.zNumZeroNothingFormat(Id), CommonUtl.geStrOrNumKbn.String);
                    //    if (_ret == true)
                    //    {
                    //        return "ID : " + Id + " は入金データの請求先に使用されている為、削除できません。";
                    //    }
                    //}
                }
                catch (Exception ex)
                {
                    db.ExRollbackTransaction();
                    CommonUtl.ExLogger.Error(CLASS_NM + ".UpdatePurchaseMst(Exist Data)", ex);
                    return CLASS_NM + ".UpdatePurchaseMst(Exist Data) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
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
                    sb.Append("   AND ID = " + ExEscape.zRepStr(ExCast.zNumZeroNothingFormat(Id)) + Environment.NewLine);            // ID

                    db.ExecuteSQL(sb.ToString(), false);
                }
                catch (Exception ex)
                {
                    db.ExRollbackTransaction();
                    CommonUtl.ExLogger.Error(CLASS_NM + ".UpdatePurchaseMst(Delete Payment Credit Balance)", ex);
                    return CLASS_NM + ".UpdatePurchaseMst(Delete Payment Credit Balance) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
                }

                #endregion

                #region Update

                try
                {
                    sb.Length = 0;
                    sb.Append("UPDATE M_PURCHASE " + Environment.NewLine);
                    sb.Append(CommonUtl.GetUpdSQLCommonColums(PG_NM,
                                                               ExCast.zCInt(personId),
                                                               ipAdress,
                                                               userId,
                                                               1));
                    sb.Append(" WHERE COMPANY_ID = " + companyId + Environment.NewLine);    // COMPANY_ID
                    sb.Append("   AND ID = " + ExEscape.zRepStr(ExCast.zNumZeroNothingFormat(Id)) + Environment.NewLine);            // ID

                    db.ExecuteSQL(sb.ToString(), false);

                }
                catch (Exception ex)
                {
                    db.ExRollbackTransaction();
                    CommonUtl.ExLogger.Error(CLASS_NM + ".UpdatePurchaseMst(Delete)", ex);
                    return CLASS_NM + ".UpdatePurchaseMst(Delete) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
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
                    CommonUtl.ExLogger.Error(CLASS_NM + ".UpdatePurchaseMst(DelLockPg)", ex);
                    return CLASS_NM + ".UpdatePurchaseMst(DelLockPg) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
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
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdatePurchaseMst(CommitTransaction)", ex);
                return CLASS_NM + ".UpdatePurchaseMst(CommitTransaction) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
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
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdatePurchaseMst(DbClose)", ex);
                return CLASS_NM + ".UpdatePurchaseMst(DbClose) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
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
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdatePurchaseMst(Add Evidence)", ex);
                return CLASS_NM + ".UpdatePurchaseMst(Add Evidence) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
            }

            #endregion

            #region Return

            if (type == 1 && (Id == "0" || Id == ""))
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
