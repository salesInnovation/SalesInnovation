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
    public class svcPurchase
    {
        private const string CLASS_NM = "svcPurchase";
        private readonly string PG_NM = DataPgEvidence.PGName.Purchase.PurchaseInp;

        #region データ取得

        /// <summary> 
        /// ヘッダリスト取得
        /// </summary>
        /// <param name="IDFrom"></param>
        /// <param name="IDTo"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebMethod(EnableSession = true)]
        public EntityPurchaseH GetPurchaseH(string random, long IDFrom, long IDTo)
        {

            EntityPurchaseH entity = null;

            #region 認証処理

            string companyId = "";
            string groupId = "";
            string userId = "";
            string ipAdress = "";
            string sessionString = "";
            int idFigurePurchase = 0;
            int personId = 0;
            try
            {
                companyId = ExCast.zCStr(HttpContext.Current.Session[ExSession.COMPANY_ID]);
                groupId = ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_ID]);
                userId = ExCast.zCStr(HttpContext.Current.Session[ExSession.USER_ID]);
                ipAdress = ExCast.zCStr(HttpContext.Current.Session[ExSession.IP_ADRESS]);
                sessionString = ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]);
                idFigurePurchase = ExCast.zCInt(HttpContext.Current.Session[ExSession.ID_FIGURE_PURCHASE]);
                personId = ExCast.zCInt(HttpContext.Current.Session[ExSession.PERSON_ID]);

                string _message = ExSession.SessionUserUniqueCheck(random, ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]), ExCast.zCInt(HttpContext.Current.Session[ExSession.USER_ID]));
                if (_message != "")
                {
                    entity = new EntityPurchaseH();
                    entity.message = _message;
                    return entity;
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetPurchaseH(認証処理)", ex);
                entity = new EntityPurchaseH();
                entity.message = CLASS_NM + ".GetPurchaseH : 認証処理に失敗しました。" + Environment.NewLine + ex.Message.ToString();
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

                sb.Append("SELECT T.COMPANY_ID " + Environment.NewLine);
                sb.Append("      ,T.GROUP_ID " + Environment.NewLine);
                sb.Append("      ,T.ID " + Environment.NewLine);
                sb.Append("      ,lpad(CAST(T.NO as char), 10, '0') AS NO" + Environment.NewLine);
                sb.Append("      ,T.ID " + Environment.NewLine);
                //sb.Append("      ,T.RED_BEFORE_NO " + Environment.NewLine);
                sb.Append("      ,T.PURCHASE_ORDER_NO " + Environment.NewLine);
                sb.Append("      ,date_format(T.PURCHASE_YMD , " + ExEscape.SQL_YMD + ") AS PURCHASE_YMD" + Environment.NewLine);
                sb.Append("      ,T.PERSON_ID " + Environment.NewLine);
                sb.Append("      ,PS.NAME AS UPDATE_PERSON_NM " + Environment.NewLine);

                sb.Append("      ,T.PURCHASE_ID " + Environment.NewLine);
                sb.Append("      ,PU.NAME AS PURCHASE_NAME " + Environment.NewLine);

                sb.Append("      ,T.TAX_CHANGE_ID " + Environment.NewLine);
                sb.Append("      ,NM4.DESCRIPTION AS TAX_CHANGE_NAME " + Environment.NewLine);
                sb.Append("      ,T.BUSINESS_DIVISION_ID " + Environment.NewLine);
                sb.Append("      ,NM5.DESCRIPTION AS BUSINESS_DIVISION_NAME " + Environment.NewLine);

                sb.Append("      ,T.SEND_ID " + Environment.NewLine);
                sb.Append("      ,NM3.DESCRIPTION AS SEND_NAME " + Environment.NewLine);

                sb.Append("      ,T.CUSTOMER_ID " + Environment.NewLine);
                sb.Append("      ,CS.NAME AS CUSTOMER_NAME " + Environment.NewLine);
                sb.Append("      ,T.SUPPLIER_ID " + Environment.NewLine);
                sb.Append("      ,SU.NAME AS SUPPLIER_NAME " + Environment.NewLine);
                sb.Append("      ,date_format(T.SUPPLY_YMD , " + ExEscape.SQL_YMD + ") AS SUPPLY_YMD" + Environment.NewLine);
                sb.Append("      ,T.SUM_ENTER_NUMBER " + Environment.NewLine);
                sb.Append("      ,T.SUM_CASE_NUMBER " + Environment.NewLine);
                sb.Append("      ,T.SUM_NUMBER " + Environment.NewLine);
                sb.Append("      ,T.SUM_UNIT_PRICE " + Environment.NewLine);
                sb.Append("      ,T.SUM_TAX " + Environment.NewLine);
                sb.Append("      ,T.SUM_NO_TAX_PRICE " + Environment.NewLine);
                sb.Append("      ,T.SUM_PRICE " + Environment.NewLine);

                sb.Append("      ,PU.PAYMENT_CREDIT_PRICE " + Environment.NewLine);

                sb.Append("      ,T.PAYMENT_NO " + Environment.NewLine);
                sb.Append("      ,date_format(IV.PAYMENT_YYYYMMDD , " + ExEscape.SQL_YMD + ") AS PAYMENT_YYYYMMDD" + Environment.NewLine);
                sb.Append("      ,PU.UNIT_KIND_ID " + Environment.NewLine);
                sb.Append("      ,PU.CREDIT_RATE " + Environment.NewLine);

                sb.Append("      ,PU.PRICE_FRACTION_PROC_ID " + Environment.NewLine);
                sb.Append("      ,PU.TAX_FRACTION_PROC_ID " + Environment.NewLine);

                sb.Append("      ,PU.PAYMENT_CYCLE_ID " + Environment.NewLine);
                sb.Append("      ,PU.PAYMENT_DAY " + Environment.NewLine);

                sb.Append("      ,date_format(IV.PAYMENT_PLAN_DAY , " + ExEscape.SQL_YMD + ") AS PAYMENT_PLAN_DAY" + Environment.NewLine);

                sb.Append("      ,CASE WHEN IFNULL(PY_SUM.SUM_PRICE, 0) = 0 THEN 0 " + Environment.NewLine);
                sb.Append("            WHEN IFNULL(PY_SUM.SUM_PRICE, 0) < IFNULL(IV.PAYMENT_PRICE, 0) THEN 1" + Environment.NewLine);
                sb.Append("            WHEN IFNULL(PY_SUM.SUM_PRICE, 0) = IFNULL(IV.PAYMENT_PRICE, 0) THEN 2" + Environment.NewLine);
                sb.Append("            WHEN IFNULL(PY_SUM.SUM_PRICE, 0) > IFNULL(IV.PAYMENT_PRICE, 0) THEN 3" + Environment.NewLine);
                sb.Append("       END AS RECEIPT_RECEIVABLE_KBN" + Environment.NewLine);

                sb.Append("      ,CASE WHEN IFNULL(PY_SUM.SUM_PRICE, 0) = 0 THEN '消込未' " + Environment.NewLine);
                sb.Append("            WHEN IFNULL(PY_SUM.SUM_PRICE, 0) < IFNULL(IV.PAYMENT_PRICE, 0) THEN '一部消込' " + Environment.NewLine);
                sb.Append("            WHEN IFNULL(PY_SUM.SUM_PRICE, 0) = IFNULL(IV.PAYMENT_PRICE, 0) THEN '消込済' " + Environment.NewLine);
                sb.Append("            WHEN IFNULL(PY_SUM.SUM_PRICE, 0) > IFNULL(IV.PAYMENT_PRICE, 0) THEN '超過消込' " + Environment.NewLine);
                sb.Append("       END AS RECEIPT_RECEIVABLE_KBN_NM" + Environment.NewLine);

                sb.Append("      ,T.MEMO " + Environment.NewLine);
                sb.Append("      ,date_format(T.UPDATE_DATE , " + ExEscape.SQL_YMD + ") AS UPDATE_DATE" + Environment.NewLine);
                sb.Append("      ,T.UPDATE_TIME " + Environment.NewLine);

                sb.Append("  FROM T_PURCHASE_H AS T" + Environment.NewLine);

                #region Join

                // 更新担当者
                sb.Append("  LEFT JOIN M_PERSON AS PS" + Environment.NewLine);
                sb.Append("    ON PS.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND PS.DISPLAY_FLG = 1 " + Environment.NewLine);
                sb.Append("   AND PS.COMPANY_ID = " + companyId + Environment.NewLine);
                sb.Append("   AND PS.GROUP_ID = " + groupId + Environment.NewLine);
                sb.Append("   AND PS.ID = T.PERSON_ID" + Environment.NewLine);

                // 仕入先
                sb.Append("  LEFT JOIN M_PURCHASE AS PU" + Environment.NewLine);
                sb.Append("    ON PU.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND PU.DISPLAY_FLG = 1 " + Environment.NewLine);
                sb.Append("   AND PU.COMPANY_ID = " + companyId + Environment.NewLine);
                sb.Append("   AND PU.ID = T.PURCHASE_ID" + Environment.NewLine);

                // 発送区分
                sb.Append("  LEFT JOIN SYS_M_NAME AS NM3" + Environment.NewLine);
                sb.Append("    ON NM3.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND NM3.DISPLAY_FLG = 1 " + Environment.NewLine);
                sb.Append("   AND NM3.DIVISION_ID = " + (int)CommonUtl.geNameKbn.SEND_KBN + Environment.NewLine);
                sb.Append("   AND T.SEND_ID = NM3.ID" + Environment.NewLine);

                // 得意先
                sb.Append("  LEFT JOIN M_CUSTOMER AS CS" + Environment.NewLine);
                sb.Append("    ON CS.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND CS.DISPLAY_FLG = 1 " + Environment.NewLine);
                sb.Append("   AND CS.COMPANY_ID = " + companyId + Environment.NewLine);
                sb.Append("   AND T.CUSTOMER_ID = CS.ID" + Environment.NewLine);

                // 納入先
                sb.Append("  LEFT JOIN M_SUPPLIER AS SU" + Environment.NewLine);
                sb.Append("    ON SU.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND SU.DISPLAY_FLG = 1 " + Environment.NewLine);
                sb.Append("   AND SU.COMPANY_ID = " + companyId + Environment.NewLine);
                sb.Append("   AND T.CUSTOMER_ID = SU.CUSTOMER_ID" + Environment.NewLine);
                sb.Append("   AND T.SUPPLIER_ID = SU.ID" + Environment.NewLine);

                // 支払データ
                sb.Append("  LEFT JOIN T_PAYMENT AS IV" + Environment.NewLine);
                sb.Append("    ON IV.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND IV.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
                sb.Append("   AND IV.NO = T.PAYMENT_NO" + Environment.NewLine);

                // 出金済額
                sb.Append("  LEFT JOIN (SELECT PY.PAYMENT_NO" + Environment.NewLine);
                sb.Append("                   ,SUM(PY.SUM_PRICE) AS SUM_PRICE" + Environment.NewLine);
                sb.Append("               FROM T_PAYMENT_CASH_H AS PY " + Environment.NewLine);
                sb.Append("              WHERE PY.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("                AND PY.COMPANY_ID = " + companyId + Environment.NewLine);
                sb.Append("                AND PY.GROUP_ID = " + groupId + Environment.NewLine);
                sb.Append("              GROUP BY PAYMENT_NO " + Environment.NewLine);
                sb.Append("            ) AS PY_SUM " + Environment.NewLine);
                sb.Append("    ON PY_SUM.PAYMENT_NO = IV.NO " + Environment.NewLine);

                // 税転換
                sb.Append("  LEFT JOIN SYS_M_NAME AS NM4" + Environment.NewLine);
                sb.Append("    ON NM4.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND NM4.DISPLAY_FLG = 1 " + Environment.NewLine);
                sb.Append("   AND NM4.DIVISION_ID = " + (int)CommonUtl.geNameKbn.TAX_CHANGE_PU_ID + Environment.NewLine);
                sb.Append("   AND T.TAX_CHANGE_ID = NM4.ID" + Environment.NewLine);

                // 取引区分
                sb.Append("  LEFT JOIN SYS_M_NAME AS NM5" + Environment.NewLine);
                sb.Append("    ON NM5.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND NM5.DISPLAY_FLG = 1 " + Environment.NewLine);
                sb.Append("   AND NM5.DIVISION_ID = " + (int)CommonUtl.geNameKbn.BUSINESS_DIVISION_PU_ID + Environment.NewLine);
                sb.Append("   AND T.BUSINESS_DIVISION_ID = NM5.ID" + Environment.NewLine);

                #endregion

                sb.Append(" WHERE T.COMPANY_ID = " + companyId + Environment.NewLine);
                sb.Append("   AND T.GROUP_ID = " + groupId + Environment.NewLine);
                sb.Append("   AND T.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND T.NO >= " + IDFrom.ToString() + Environment.NewLine);
                sb.Append("   AND T.NO <= " + IDTo.ToString() + Environment.NewLine);

                sb.Append(" ORDER BY T.COMPANY_ID " + Environment.NewLine);
                sb.Append("         ,T.GROUP_ID " + Environment.NewLine);
                sb.Append("         ,T.purchase_ymd DESC " + Environment.NewLine);
                sb.Append("         ,T.NO DESC " + Environment.NewLine);
                sb.Append(" LIMIT 0, 1");

                #endregion

                dt = db.GetDataTable(sb.ToString());

                if (dt.DefaultView.Count > 0)
                {
                    entity = new EntityPurchaseH();

                    // 排他制御
                    DataPgLock.geLovkFlg lockFlg = DataPgLock.geLovkFlg.UnLock;
                    string strErr = DataPgLock.SetLockPg(companyId, userId, PG_NM, groupId + "-" + ExCast.zNumZeroNothingFormat(IDFrom.ToString()), ipAdress, db, out lockFlg);
                    if (strErr != "")
                    {
                        entity.message = CLASS_NM + ".GetCommodity : 排他制御(ロック情報取得)に失敗しました。" + Environment.NewLine + strErr;
                    }

                    #region Set Entity

                    entity.id = ExCast.zCLng(dt.DefaultView[0]["ID"]);
                    entity.no = ExCast.zCLng(dt.DefaultView[0]["NO"]);
                    //entity.red_before_no = ExCast.zCLng(dt.DefaultView[0]["RED_BEFORE_NO"]);
                    entity.purchase_order_no = ExCast.zCLng(dt.DefaultView[0]["PURCHASE_ORDER_NO"]);
                    entity.purchase_ymd = ExCast.zDateNullToDefault(dt.DefaultView[0]["PURCHASE_YMD"]);

                    entity.update_person_id = ExCast.zCInt(dt.DefaultView[0]["PERSON_ID"]);
                    entity.update_person_nm = ExCast.zCStr(dt.DefaultView[0]["UPDATE_PERSON_NM"]);

                    entity.purchase_id = ExCast.zFormatForID(dt.DefaultView[0]["PURCHASE_ID"], idFigurePurchase);
                    entity.purchase_name = ExCast.zCStr(dt.DefaultView[0]["PURCHASE_NAME"]);

                    entity.tax_change_id = ExCast.zCInt(dt.DefaultView[0]["TAX_CHANGE_ID"]);
                    entity.tax_change_name = ExCast.zCStr(dt.DefaultView[0]["TAX_CHANGE_NAME"]);

                    entity.business_division_id = ExCast.zCInt(dt.DefaultView[0]["BUSINESS_DIVISION_ID"]);
                    entity.business_division_name = ExCast.zCStr(dt.DefaultView[0]["BUSINESS_DIVISION_NAME"]);

                    entity.send_kbn_id = ExCast.zCInt(dt.DefaultView[0]["SEND_ID"]);
                    entity.send_kbn_nm = ExCast.zCStr(dt.DefaultView[0]["SEND_NAME"]);

                    entity.customer_id = ExCast.zFormatForID(dt.DefaultView[0]["CUSTOMER_ID"], idFigurePurchase);
                    entity.customer_name = ExCast.zCStr(dt.DefaultView[0]["CUSTOMER_NAME"]);

                    entity.supplier_id = ExCast.zFormatForID(dt.DefaultView[0]["SUPPLIER_ID"], idFigurePurchase);
                    entity.supplier_name = ExCast.zCStr(dt.DefaultView[0]["SUPPLIER_NAME"]);

                    entity.supply_ymd = ExCast.zDateNullToDefault(dt.DefaultView[0]["SUPPLY_YMD"]);
                    entity.sum_enter_number = ExCast.zCDbl(dt.DefaultView[0]["SUM_ENTER_NUMBER"]);
                    entity.sum_case_number = ExCast.zCDbl(dt.DefaultView[0]["SUM_CASE_NUMBER"]);
                    entity.sum_number = ExCast.zCDbl(dt.DefaultView[0]["SUM_NUMBER"]);
                    entity.sum_unit_price = ExCast.zCDbl(dt.DefaultView[0]["SUM_UNIT_PRICE"]);
                    entity.sum_tax = ExCast.zCDbl(dt.DefaultView[0]["SUM_TAX"]);
                    entity.sum_no_tax_price = ExCast.zCDbl(dt.DefaultView[0]["SUM_NO_TAX_PRICE"]);
                    entity.sum_price = ExCast.zCDbl(dt.DefaultView[0]["SUM_PRICE"]);
                    entity.payment_credit_price = ExCast.zCDbl(dt.DefaultView[0]["PAYMENT_CREDIT_PRICE"]);
                    entity.before_payment_credit_price = ExCast.zCDbl(dt.DefaultView[0]["PAYMENT_CREDIT_PRICE"]);

                    entity.payment_no = ExCast.zCLng(dt.DefaultView[0]["PAYMENT_NO"]);
                    entity.payment_yyyymmdd = ExCast.zDateNullToDefault(dt.DefaultView[0]["PAYMENT_YYYYMMDD"]);
                    entity.unit_kind_id = ExCast.zCInt(dt.DefaultView[0]["UNIT_KIND_ID"]);
                    entity.price_fraction_proc_id = ExCast.zCInt(dt.DefaultView[0]["PRICE_FRACTION_PROC_ID"]);
                    entity.tax_fraction_proc_id = ExCast.zCInt(dt.DefaultView[0]["TAX_FRACTION_PROC_ID"]);

                    entity.credit_rate = ExCast.zCInt(dt.DefaultView[0]["CREDIT_RATE"]);

                    entity.payment_cycle_id = ExCast.zCInt(dt.DefaultView[0]["PAYMENT_CYCLE_ID"]);
                    entity.payment_day = ExCast.zCInt(dt.DefaultView[0]["PAYMENT_DAY"]);

                    switch (entity.business_division_id)
                    {
                        case 1:     // 払締
                            if (ExCast.zCLng(dt.DefaultView[0]["PAYMENT_NO"]) == 0) entity.payment_state = "払締未";
                            else entity.payment_state = "払締済";
                            break;
                        default:
                            entity.payment_state = "";
                            break;
                    }

                    entity.payment_cash_receivable_kbn = ExCast.zCInt(dt.DefaultView[0]["RECEIPT_RECEIVABLE_KBN"]);
                    entity.payment_cash_receivable_kbn_nm = ExCast.zCStr(dt.DefaultView[0]["RECEIPT_RECEIVABLE_KBN_NM"]);

                    entity.memo = ExCast.zCStr(dt.DefaultView[0]["MEMO"]);

                    entity.lock_flg = (int)lockFlg;

                    // 掛仕入
                    if (entity.business_division_id == 1)
                    {
                        // 支払締切済チェック
                        if (DataClose.IsPaymentClose(companyId, db, entity.purchase_id, entity.purchase_ymd))
                        {
                            entity.payment_close_flg = 1;
                        }
                    }

                    #endregion

                }
                else
                {
                    entity = null;
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetPurchaseH", ex);
                entity = new EntityPurchaseH();
                entity.message = CLASS_NM + ".GetPurchaseH : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message.ToString();
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
                                       DataPgEvidence.PGName.Purchase.PurchaseInp,
                                       DataPgEvidence.geOperationType.Select,
                                       "NO:" + IDFrom.ToString() + ",NO:" + IDTo.ToString());

            return entity;


        }

        /// <summary> 
        /// 明細リスト取得
        /// </summary>
        /// <param name="IDFrom"></param>
        /// <param name="IDTo"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebMethod(EnableSession = true)]
        public List<EntityPurchaseD> GetPurchaseListD(string random, long IDFrom, long IDTo)
        {
            List<EntityPurchaseD> entityList = new List<EntityPurchaseD>();

            #region 認証処理

            string companyId = "";
            string groupId = "";
            string userId = "";
            string ipAdress = "";
            string sessionString = "";
            int idFigureGoods = 0;

            try
            {
                companyId = ExCast.zCStr(HttpContext.Current.Session[ExSession.COMPANY_ID]);
                groupId = ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_ID]);
                userId = ExCast.zCStr(HttpContext.Current.Session[ExSession.USER_ID]);
                ipAdress = ExCast.zCStr(HttpContext.Current.Session[ExSession.IP_ADRESS]);
                sessionString = ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]);
                idFigureGoods = ExCast.zCInt(HttpContext.Current.Session[ExSession.ID_FIGURE_GOODS]);

                string _message = ExSession.SessionUserUniqueCheck(random, ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]), ExCast.zCInt(HttpContext.Current.Session[ExSession.USER_ID]));
                if (_message != "")
                {
                    EntityPurchaseD entity = new EntityPurchaseD();
                    entity.message = _message;
                    entityList.Add(entity);
                    return entityList;
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetPurchaseListD(認証処理)", ex);
                EntityPurchaseD entity = new EntityPurchaseD();
                entity.message = CLASS_NM + ".GetPurchaseListD : 認証処理に失敗しました。" + Environment.NewLine + ex.Message.ToString(); ;
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

                #region SQL

                sb.Append("SELECT OD.PURCHASE_ID " + Environment.NewLine);
                sb.Append("      ,OD.REC_NO " + Environment.NewLine);
                sb.Append("      ,OD.BREAKDOWN_ID " + Environment.NewLine);
                sb.Append("      ,NM1.DESCRIPTION AS BREAKDOWN_NM " + Environment.NewLine);
                sb.Append("      ,OD.DELIVER_DIVISION_ID " + Environment.NewLine);
                sb.Append("      ,NM2.DESCRIPTION AS DELIVER_DIVISION_NM " + Environment.NewLine);
                sb.Append("      ,OD.COMMODITY_ID " + Environment.NewLine);
                sb.Append("      ,OD.COMMODITY_NAME " + Environment.NewLine);
                sb.Append("      ,OD.UNIT_ID " + Environment.NewLine);
                sb.Append("      ,NM3.DESCRIPTION AS UNIT_NM " + Environment.NewLine);
                sb.Append("      ,OD.ENTER_NUMBER " + Environment.NewLine);
                sb.Append("      ,OD.CASE_NUMBER " + Environment.NewLine);
                sb.Append("      ,OD.NUMBER " + Environment.NewLine);
                sb.Append("      ,OD.UNIT_PRICE " + Environment.NewLine);
                sb.Append("      ,OD.TAX " + Environment.NewLine);
                sb.Append("      ,OD.NO_TAX_PRICE " + Environment.NewLine);
                sb.Append("      ,OD.PRICE " + Environment.NewLine);
                sb.Append("      ,OD.MEMO " + Environment.NewLine);
                sb.Append("      ,OD.TAX_DIVISION_ID " + Environment.NewLine);
                sb.Append("      ,NM4.DESCRIPTION AS TAX_DIVISION_NM " + Environment.NewLine);
                sb.Append("      ,OD.TAX_PERCENT " + Environment.NewLine);
                sb.Append("      ,CT.INVENTORY_NUMBER " + Environment.NewLine);
                sb.Append("      ,CT.INVENTORY_MANAGEMENT_DIVISION_ID " + Environment.NewLine);
                sb.Append("      ,CT.NUMBER_DECIMAL_DIGIT " + Environment.NewLine);
                sb.Append("      ,CT.UNIT_DECIMAL_DIGIT " + Environment.NewLine);
                sb.Append("      ,OD.PURCHASE_ORDER_ID " + Environment.NewLine);
                sb.Append("      ,RD.NUMBER AS PURCHASE_ORDER_NUMBER " + Environment.NewLine);
                sb.Append("  FROM T_PURCHASE_D AS OD" + Environment.NewLine);

                #region Join

                // 受注残
                sb.Append("  LEFT JOIN T_PURCHASE_ORDER_D AS RD" + Environment.NewLine);
                sb.Append("    ON RD.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND RD.COMPANY_ID = OD.COMPANY_ID " + Environment.NewLine);
                sb.Append("   AND RD.GROUP_ID = OD.GROUP_ID " + Environment.NewLine);
                sb.Append("   AND RD.PURCHASE_ORDER_ID = OD.PURCHASE_ORDER_ID" + Environment.NewLine);
                sb.Append("   AND RD.REC_NO = OD.REC_NO" + Environment.NewLine);

                // 現在庫など
                sb.Append("  LEFT JOIN M_COMMODITY AS CT" + Environment.NewLine);
                sb.Append("    ON CT.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND CT.DISPLAY_FLG = 1 " + Environment.NewLine);
                sb.Append("   AND CT.COMPANY_ID = " + companyId + Environment.NewLine);
                sb.Append("   AND CT.ID = OD.COMMODITY_ID" + Environment.NewLine);

                // 内訳
                sb.Append("  LEFT JOIN SYS_M_NAME AS NM1" + Environment.NewLine);
                sb.Append("    ON NM1.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND NM1.DISPLAY_FLG = 1 " + Environment.NewLine);
                sb.Append("   AND NM1.DIVISION_ID = " + (int)CommonUtl.geNameKbn.BREAKDOWN_ID + Environment.NewLine);
                sb.Append("   AND OD.BREAKDOWN_ID = NM1.ID" + Environment.NewLine);

                // 納品区分
                sb.Append("  LEFT JOIN SYS_M_NAME AS NM2" + Environment.NewLine);
                sb.Append("    ON NM2.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND NM2.DISPLAY_FLG = 1 " + Environment.NewLine);
                sb.Append("   AND NM2.DIVISION_ID = " + (int)CommonUtl.geNameKbn.DELIVER_DIVISION_ID + Environment.NewLine);
                sb.Append("   AND OD.DELIVER_DIVISION_ID = NM2.ID" + Environment.NewLine);

                // 単位
                sb.Append("  LEFT JOIN SYS_M_NAME AS NM3" + Environment.NewLine);
                sb.Append("    ON NM3.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND NM3.DISPLAY_FLG = 1 " + Environment.NewLine);
                sb.Append("   AND NM3.DIVISION_ID = " + (int)CommonUtl.geNameKbn.UNIT_ID + Environment.NewLine);
                sb.Append("   AND OD.UNIT_ID = NM3.ID" + Environment.NewLine);

                // 課税区分名
                sb.Append("  LEFT JOIN SYS_M_NAME AS NM4" + Environment.NewLine);
                sb.Append("    ON NM4.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND NM4.DISPLAY_FLG = 1 " + Environment.NewLine);
                sb.Append("   AND NM4.DIVISION_ID = " + (int)CommonUtl.geNameKbn.TAX_DIVISION_ID + Environment.NewLine);
                sb.Append("   AND OD.TAX_DIVISION_ID = NM4.ID" + Environment.NewLine);

                #endregion

                sb.Append(" WHERE OD.COMPANY_ID = " + companyId + Environment.NewLine);
                sb.Append("   AND OD.GROUP_ID = " + groupId + Environment.NewLine);
                sb.Append("   AND OD.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND OD.PURCHASE_ID >= " + IDFrom.ToString() + Environment.NewLine);
                sb.Append("   AND OD.PURCHASE_ID <= " + IDTo.ToString() + Environment.NewLine);

                sb.Append(" ORDER BY OD.COMPANY_ID " + Environment.NewLine);
                sb.Append("         ,OD.GROUP_ID " + Environment.NewLine);
                sb.Append("         ,OD.REC_NO " + Environment.NewLine);
                sb.Append(" LIMIT 0, 100");

                #endregion

                dt = db.GetDataTable(sb.ToString());

                if (dt.DefaultView.Count > 0)
                {
                    for (int i = 0; i <= dt.DefaultView.Count - 1; i++)
                    {
                        EntityPurchaseD entityD = new EntityPurchaseD();

                        #region Set Entity

                        entityD.id = ExCast.zCLng(dt.DefaultView[i]["PURCHASE_ID"]);
                        entityD.rec_no = ExCast.zCInt(dt.DefaultView[i]["REC_NO"]);
                        entityD.breakdown_id = ExCast.zCInt(dt.DefaultView[i]["BREAKDOWN_ID"]);
                        entityD.breakdown_nm = ExCast.zCStr(dt.DefaultView[i]["BREAKDOWN_NM"]);
                        entityD.deliver_division_id = ExCast.zCInt(dt.DefaultView[i]["DELIVER_DIVISION_ID"]);
                        entityD.deliver_division_nm = ExCast.zCStr(dt.DefaultView[i]["DELIVER_DIVISION_NM"]);
                        entityD.commodity_id = ExCast.zFormatForID(dt.DefaultView[i]["COMMODITY_ID"], idFigureGoods);
                        entityD.commodity_name = ExCast.zCStr(dt.DefaultView[i]["COMMODITY_NAME"]);
                        entityD.unit_id = ExCast.zCInt(dt.DefaultView[i]["UNIT_ID"]);
                        entityD.unit_nm = ExCast.zCStr(dt.DefaultView[i]["UNIT_NM"]);
                        entityD.enter_number = ExCast.zCDbl(dt.DefaultView[i]["ENTER_NUMBER"]);
                        entityD.case_number = ExCast.zCDbl(dt.DefaultView[i]["CASE_NUMBER"]);
                        entityD.number = ExCast.zCDbl(dt.DefaultView[i]["NUMBER"]);
                        entityD.unit_price = ExCast.zCDbl(dt.DefaultView[i]["UNIT_PRICE"]);
                        entityD.tax = ExCast.zCDbl(dt.DefaultView[i]["TAX"]);
                        entityD.no_tax_price = ExCast.zCDbl(dt.DefaultView[i]["NO_TAX_PRICE"]);
                        entityD.price = ExCast.zCDbl(dt.DefaultView[i]["PRICE"]);
                        entityD.memo = ExCast.zCStr(dt.DefaultView[i]["MEMO"]);
                        entityD.tax_division_id = ExCast.zCInt(dt.DefaultView[i]["TAX_DIVISION_ID"]);
                        entityD.tax_division_nm = ExCast.zCStr(dt.DefaultView[i]["TAX_DIVISION_NM"]);
                        entityD.tax_percent = ExCast.zCInt(dt.DefaultView[i]["TAX_PERCENT"]);
                        entityD.inventory_number = ExCast.zCDbl(dt.DefaultView[i]["INVENTORY_NUMBER"]);
                        entityD.inventory_management_division_id = ExCast.zCInt(dt.DefaultView[i]["INVENTORY_MANAGEMENT_DIVISION_ID"]);
                        entityD.number_decimal_digit = ExCast.zCInt(dt.DefaultView[i]["NUMBER_DECIMAL_DIGIT"]);
                        entityD.unit_decimal_digit = ExCast.zCInt(dt.DefaultView[i]["UNIT_DECIMAL_DIGIT"]);
                        entityD.purchase_order_id = ExCast.zCLng(dt.DefaultView[i]["PURCHASE_ORDER_ID"]);
                        entityD.purchase_order_number = ExCast.zCDbl(dt.DefaultView[i]["PURCHASE_ORDER_NUMBER"]);
                        entityD.purchase_order_stay_number = entityD.purchase_order_number - entityD.number;

                        entityList.Add(entityD);

                        #endregion
                    }
                }

                return entityList;

            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetPurchaseListD", ex);
                entityList.Clear();
                EntityPurchaseD entityD = new EntityPurchaseD();
                entityD.message = CLASS_NM + ".GetPurchaseListD : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message.ToString();
                entityList.Add(entityD);
                return entityList;
            }
            finally
            {
                db = null;
            }

        }

        /// <summary>　
        /// 仕入リスト取得
        /// </summary>
        /// <param name="strWhereSql"></param>
        /// <param name="strOrderbySql"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebMethod(EnableSession = true)]
        public List<EntityPurchase> GetPurchaseList(string random, string strWhereSql, string strOrderbySql)
        {
            List<EntityPurchase> entityList = new List<EntityPurchase>();

            #region 認証処理

            string companyId = "";
            string groupId = "";
            string userId = "";
            string ipAdress = "";
            string sessionString = "";
            int idFigureCommodity = 0;
            int idFigurePurchase = 0;
            int idFigureCustomer = 0;
            int idFigureSlipNo = 0;

            try
            {
                companyId = ExCast.zCStr(HttpContext.Current.Session[ExSession.COMPANY_ID]);
                groupId = ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_ID]);
                userId = ExCast.zCStr(HttpContext.Current.Session[ExSession.USER_ID]);
                ipAdress = ExCast.zCStr(HttpContext.Current.Session[ExSession.IP_ADRESS]);
                sessionString = ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]);
                idFigureCommodity = ExCast.zCInt(HttpContext.Current.Session[ExSession.ID_FIGURE_GOODS]);
                idFigurePurchase = ExCast.zCInt(HttpContext.Current.Session[ExSession.ID_FIGURE_PURCHASE]);
                idFigureCustomer = ExCast.zCInt(HttpContext.Current.Session[ExSession.ID_FIGURE_CUSTOMER]);
                idFigureSlipNo = ExCast.zCInt(HttpContext.Current.Session[ExSession.ID_FIGURE_SLIP_NO]);

                string _message = ExSession.SessionUserUniqueCheck(random, ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]), ExCast.zCInt(HttpContext.Current.Session[ExSession.USER_ID]));
                if (_message != "")
                {
                    EntityPurchase entity = new EntityPurchase();
                    entity.MESSAGE = _message;
                    entityList.Add(entity);
                    return entityList;
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetPurchaseList(認証処理)", ex);
                EntityPurchase entity = new EntityPurchase();
                entity.MESSAGE = "認証処理に失敗しました。" + Environment.NewLine + ex.Message.ToString(); ;
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

                ExReportManeger rptMgr = new ExReportManeger();
                rptMgr.idFigureCommodity = idFigureCommodity;
                rptMgr.idFigureCustomer = idFigureCustomer;
                rptMgr.idFigurePurchase = idFigurePurchase;
                rptMgr.idFigureSlipNo = idFigureSlipNo;
                sb.Append(rptMgr.GetPurchaseListReportSQL(companyId, groupId, strWhereSql, strOrderbySql));

                dt = db.GetDataTable(sb.ToString());

                if (dt.DefaultView.Count > 0)
                {
                    for (int i = 0; i <= dt.DefaultView.Count - 1; i++)
                    {
                        EntityPurchase _entityPurchase = new EntityPurchase();
                        _entityPurchase.ID = ExCast.zCLng(dt.DefaultView[i]["ID"]);
                        _entityPurchase.NO = ExCast.zCLng(dt.DefaultView[i]["NO"]);
                        _entityPurchase.PURCHASE_ORDER_NO = ExCast.zCLng(dt.DefaultView[i]["PURCHASE_ORDER_NO"]);
                        _entityPurchase.PURCHASE_YMD = ExCast.zDateNullToDefault(dt.DefaultView[i]["PURCHASE_YMD"]);
                        _entityPurchase.PURCHASE_ID = ExCast.zCStr(dt.DefaultView[i]["PURCHASE_ID"]);
                        _entityPurchase.PURCHASE_NM = ExCast.zCStr(dt.DefaultView[i]["PURCHASE_NM"]);
                        _entityPurchase.TAX_CHANGE_ID = ExCast.zCInt(dt.DefaultView[i]["TAX_CHANGE_ID"]);
                        _entityPurchase.TAX_CHANGE_NM = ExCast.zCStr(dt.DefaultView[i]["TAX_CHANGE_NM"]);
                        _entityPurchase.BUSINESS_DIVISION_ID = ExCast.zCInt(dt.DefaultView[i]["BUSINESS_DIVISION_ID"]);
                        _entityPurchase.BUSINESS_DIVISION_NM = ExCast.zCStr(dt.DefaultView[i]["BISNESS_DIVISON_NM"]);
                        _entityPurchase.SEND_KBN_ID = ExCast.zCInt(dt.DefaultView[i]["SEND_ID"]);
                        _entityPurchase.SEND_KBN_NM = ExCast.zCStr(dt.DefaultView[i]["SEND_NM"]);
                        _entityPurchase.CUSTOMER_ID = ExCast.zCStr(dt.DefaultView[i]["CUSTOMER_ID"]);
                        _entityPurchase.CUSTOMER_NM = ExCast.zCStr(dt.DefaultView[i]["CUSTOMER_NM"]);
                        _entityPurchase.SUPPLIER_ID = ExCast.zCStr(dt.DefaultView[i]["SUPPLIER_ID"]);
                        _entityPurchase.SUPPLIER_NM = ExCast.zCStr(dt.DefaultView[i]["SUPPLIER_NM"]);
                        _entityPurchase.SUPPLY_YMD = ExCast.zDateNullToDefault(dt.DefaultView[i]["SUPPLY_YMD"]);
                        _entityPurchase.MEMO = ExCast.zCStr(dt.DefaultView[i]["MEMO"]);
                        _entityPurchase.INPUT_PERSON = ExCast.zCInt(dt.DefaultView[i]["INPUT_PERSON"]);
                        _entityPurchase.INPUT_PERSON_NM = ExCast.zCStr(dt.DefaultView[i]["INPUT_PERSON_NM"]);
                        _entityPurchase.REC_NO = ExCast.zCInt(dt.DefaultView[i]["REC_NO"]);
                        _entityPurchase.BREAKDOWN_ID = ExCast.zCInt(dt.DefaultView[i]["BREAKDOWN_ID"]);
                        _entityPurchase.BREAKDOWN_NM = ExCast.zCStr(dt.DefaultView[i]["BREAKDOWN_NM"]);
                        _entityPurchase.DELIVER_DIVISION_ID = ExCast.zCInt(dt.DefaultView[i]["DELIVER_DIVISION_ID"]);
                        _entityPurchase.DELIVER_DIVISION_NM = ExCast.zCStr(dt.DefaultView[i]["DELIVER_DIVISION_NM"]);
                        _entityPurchase.COMMODITY_ID = ExCast.zCStr(dt.DefaultView[i]["COMMODITY_ID"]);
                        _entityPurchase.COMMODITY_NAME = ExCast.zCStr(dt.DefaultView[i]["COMMODITY_NAME"]);
                        _entityPurchase.UNIT_ID = ExCast.zCInt(dt.DefaultView[i]["UNIT_ID"]);
                        _entityPurchase.UNIT_NM = ExCast.zCStr(dt.DefaultView[i]["UNIT_NM"]);
                        _entityPurchase.ENTER_NUMBER = ExCast.zCInt(dt.DefaultView[i]["ENTER_NUMBER"]);
                        _entityPurchase.NUMBER = ExCast.zCDbl(dt.DefaultView[i]["NUMBER"]);
                        _entityPurchase.UNIT_PRICE = ExCast.zCDbl(dt.DefaultView[i]["UNIT_PRICE"]);
                        _entityPurchase.PRICE = ExCast.zCDbl(dt.DefaultView[i]["PRICE"]);
                        _entityPurchase.D_MEMO = ExCast.zCStr(dt.DefaultView[i]["D_MEMO"]);

                        entityList.Add(_entityPurchase);
                    }
                }

            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetPurchaseList", ex);
                entityList.Clear();
                EntityPurchase entity = new EntityPurchase();
                entity.MESSAGE = CLASS_NM + ".GetPurchaseList : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message.ToString();
                entityList.Add(entity);
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
                                       DataPgEvidence.PGName.Purchase.PurchaseList,
                                       DataPgEvidence.geOperationType.Select,
                                       "Where:" + strWhereSql + ",Orderby:" + strOrderbySql);

            return entityList;

        }

        #endregion

        #region データ更新

        /// <summary>
        /// データ更新
        /// </summary>
        /// <param name="random">セッションランダム文字列</param>
        /// <param name="type">0:Update 1:Insert 2:Delete</param>
        /// <param name="PurchaseNo">伝票番号</param>
        /// <param name="entityH">ヘッダデータ</param>
        /// <param name="entityD">明細データ</param>
        /// <returns></returns>
        [OperationContract()]
        [WebMethod(EnableSession = true)]
        public string UpdatePurchase(string random, int type, long PurchaseNo, EntityPurchaseH entityH, List<EntityPurchaseD> entityD, EntityPurchaseH before_entityH, List<EntityPurchaseD> before_entityD)
        {

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
                    return _message;
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdatePurchase(認証処理)", ex);
                return CLASS_NM + ".認証処理に失敗しました。" + Environment.NewLine + ex.Message.ToString();
            }

            #endregion

            #region Field

            StringBuilder sb = new StringBuilder();
            DataTable dt;
            ExMySQLData db = null;

            string accountPeriod = "";
            long id = 0;
            long no = PurchaseNo;
            string ret_message = "";

            EntityInOutDeliveryH _entityInOutDeliveryH = new EntityInOutDeliveryH();
            List<EntityInOutDeliveryD> _entityInOutDeliveryListD = new List<EntityInOutDeliveryD>();

            #endregion

            #region Databese Open

            try
            {
                db = new ExMySQLData(ExCast.zCStr(HttpContext.Current.Session[ExSession.DB_CONNECTION_STR]));
                db.DbOpen();
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdatePurchase(DbOpen)", ex);
                return "UpdatePurchase(DbOpen) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
            }

            #endregion

            #region Payment Close Check

            try
            {
                if (type <= 1)
                {
                    // 掛仕入
                    if (entityH.business_division_id == 1)
                    {
                        // 支払締切済チェック
                        if (DataClose.IsPaymentClose(companyId, db, entityH.purchase_id, entityH.purchase_ymd))
                        {
                            db.DbClose();
                            return "仕入日 : " + entityH.purchase_ymd + " は支払締切済の為、計上できません。";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdatePurchase(Payment Close Check)", ex);
                return "UpdatePurchase(Payment Close Check) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
            }

            #endregion

            #region BeginTransaction

            try
            {
                db.ExBeginTransaction();
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdatePurchase(BeginTransaction)", ex);
                return "UpdatePurchase(BeginTransaction) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
            }

            #endregion

            #region Update or Insert

            if (type <= 1)
            {
                #region Get Accout Period

                try
                {
                    accountPeriod = DataAccount.GetAccountPeriod(ExCast.zCStr(HttpContext.Current.Session[ExSession.ACCOUNT_BEGIN_PERIOD]), entityH.purchase_ymd);
                    if (accountPeriod == "")
                    {
                        return "会計年の取得に失敗しました。(期首月日 : " + ExCast.zCStr(HttpContext.Current.Session[ExSession.ACCOUNT_BEGIN_PERIOD]) +
                                                             " 仕入日 : " + entityH.purchase_ymd + ")";
                    }
                }
                catch (Exception ex)
                {
                    db.ExRollbackTransaction();
                    CommonUtl.ExLogger.Error(CLASS_NM + ".UpdatePurchase(GetAccountPeriod)", ex);
                    return "UpdatePurchase(GetAccountPeriod) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
                }

                #endregion

                #region Get Slip No

                try
                {
                    DataSlipNo.GetSlipNo(companyId,
                                         groupId,
                                         db,
                                         DataSlipNo.geSlipKbn.Vendor,
                                         accountPeriod,
                                         ref no,
                                         out id,
                                         ExCast.zCStr(HttpContext.Current.Session[ExSession.IP_ADRESS]),
                                         ExCast.zCStr(HttpContext.Current.Session[ExSession.USER_ID]));
                    if (no == 0 || id == 0)
                    {
                        db.ExRollbackTransaction();
                        return "伝票番号の取得に失敗しました。(会計年 : " + accountPeriod + ")";
                    }
                }
                catch (Exception ex)
                {
                    CommonUtl.ExLogger.Error(CLASS_NM + ".UpdatePurchase(GetSlipNo)", ex);
                    db.ExRollbackTransaction();
                    return "UpdatePurchase(GetSlipNo) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
                }

                #endregion

                #region Head Insert

                try
                {
                    #region SQL

                    sb.Length = 0;
                    sb.Append("INSERT INTO T_PURCHASE_H " + Environment.NewLine);
                    sb.Append("       ( COMPANY_ID" + Environment.NewLine);
                    sb.Append("       , GROUP_ID" + Environment.NewLine);
                    sb.Append("       , ID" + Environment.NewLine);
                    sb.Append("       , NO" + Environment.NewLine);
                    sb.Append("       , PURCHASE_ORDER_NO" + Environment.NewLine);
                    sb.Append("       , PURCHASE_YMD" + Environment.NewLine);
                    sb.Append("       , PERSON_ID" + Environment.NewLine);
                    sb.Append("       , PURCHASE_ID" + Environment.NewLine);
                    sb.Append("       , TAX_CHANGE_ID" + Environment.NewLine);
                    sb.Append("       , BUSINESS_DIVISION_ID" + Environment.NewLine);
                    sb.Append("       , SEND_ID" + Environment.NewLine);
                    sb.Append("       , CUSTOMER_ID" + Environment.NewLine);
                    sb.Append("       , SUPPLIER_ID" + Environment.NewLine);
                    sb.Append("       , SUPPLY_YMD" + Environment.NewLine);
                    sb.Append("       , DELIVER_DIVISION_ID" + Environment.NewLine);
                    sb.Append("       , SUM_ENTER_NUMBER" + Environment.NewLine);
                    sb.Append("       , SUM_CASE_NUMBER" + Environment.NewLine);
                    sb.Append("       , SUM_NUMBER" + Environment.NewLine);
                    sb.Append("       , SUM_UNIT_PRICE" + Environment.NewLine);
                    sb.Append("       , SUM_TAX" + Environment.NewLine);
                    sb.Append("       , SUM_NO_TAX_PRICE" + Environment.NewLine);
                    sb.Append("       , SUM_PRICE" + Environment.NewLine);
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
                    sb.Append("SELECT  " + companyId + Environment.NewLine);                                                                // COMPANY_ID
                    sb.Append("       ," + groupId + Environment.NewLine);                                                                  // GROUP_ID
                    sb.Append("       ," + id.ToString() + Environment.NewLine);                                                            // ID
                    sb.Append("       ," + no.ToString() + Environment.NewLine);                                                            // NO
                    sb.Append("       ," + entityH.purchase_order_no + Environment.NewLine);                                                // PURCHASE_ORDER_NO
                    sb.Append("       ," + ExEscape.zRepStr(ExCast.zDateNullToDefault(entityH.purchase_ymd)) + Environment.NewLine);        // PURCHASE_YMD
                    sb.Append("       ," + entityH.update_person_id + Environment.NewLine);                                                 // PERSON_ID
                    sb.Append("       ," + ExEscape.zRepStr(ExCast.zNumZeroNothingFormat(entityH.purchase_id)) + Environment.NewLine);      // PURCHASE_ID
                    sb.Append("       ," + entityH.tax_change_id + Environment.NewLine);                                                    // TAX_CHANGE_ID
                    sb.Append("       ," + entityH.business_division_id + Environment.NewLine);                                             // BUSINESS_DIVISION_ID
                    sb.Append("       ," + entityH.send_kbn_id + Environment.NewLine);                                                      // SEND_ID
                    sb.Append("       ," + ExEscape.zRepStr(ExCast.zNumZeroNothingFormat(entityH.customer_id)) + Environment.NewLine);      // CUSTOMER_ID
                    sb.Append("       ," + ExEscape.zRepStr(ExCast.zNumZeroNothingFormat(entityH.supplier_id)) + Environment.NewLine);      // SUPPLIER_ID
                    sb.Append("       ," + ExEscape.zRepStr(ExCast.zTimeNullToDefault(entityH.supply_ymd)) + Environment.NewLine);          // SUPPLY_YMD
                    sb.Append("       ,1" + Environment.NewLine);                                                                           // DELIVER_DIVISION_ID
                    sb.Append("       ," + ExCast.zCDbl(entityH.sum_enter_number) + Environment.NewLine);                                   // SUM_ENTER_NUMBER
                    sb.Append("       ," + ExCast.zCDbl(entityH.sum_case_number) + Environment.NewLine);                                    // SUM_CASE_NUMBER
                    sb.Append("       ," + ExCast.zCDbl(entityH.sum_number) + Environment.NewLine);                                         // SUM_NUMBER
                    sb.Append("       ," + ExCast.zCDbl(entityH.sum_unit_price) + Environment.NewLine);                                     // SUM_UNIT_PRICE
                    sb.Append("       ," + ExCast.zCDbl(entityH.sum_tax) + Environment.NewLine);                                            // SUM_TAX
                    sb.Append("       ," + ExCast.zCDbl(entityH.sum_no_tax_price) + Environment.NewLine);                                   // SUM_NO_TAX_PRICE
                    sb.Append("       ," + ExCast.zCDbl(entityH.sum_price) + Environment.NewLine);                                          // SUM_PRICE
                    sb.Append("       ," + ExEscape.zRepStr(entityH.memo) + Environment.NewLine);                                           // MEMO
                    if (type == 0)
                    {
                        sb.Append(CommonUtl.GetInsSQLCommonColums(CommonUtl.UpdKbn.Upd,
                                                                  PG_NM,
                                                                  "T_PURCHASE_H",
                                                                  entityH.update_person_id,
                                                                  PurchaseNo.ToString(),
                                                                  ipAdress,
                                                                  userId));
                    }
                    else
                    {
                        sb.Append(CommonUtl.GetInsSQLCommonColums(CommonUtl.UpdKbn.Ins,
                                                                  PG_NM,
                                                                  "T_PURCHASE_H",
                                                                  entityH.update_person_id,
                                                                  "0",
                                                                  ipAdress,
                                                                  userId));
                    }

                    #endregion

                    db.ExecuteSQL(sb.ToString(), false);

                }
                catch (Exception ex)
                {
                    db.ExRollbackTransaction();
                    CommonUtl.ExLogger.Error(CLASS_NM + ".UpdatePurchase(Head Insert)", ex);
                    return "UpdatePurchase(Head Insert) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
                }

                #endregion

                #region Detail Insert

                try
                {
                    for (int i = 0; i <= entityD.Count - 1; i++)
                    {
                        #region SQL

                        sb.Length = 0;
                        sb.Append("INSERT INTO T_PURCHASE_D " + Environment.NewLine);
                        sb.Append("       ( COMPANY_ID" + Environment.NewLine);
                        sb.Append("       , GROUP_ID" + Environment.NewLine);
                        sb.Append("       , PURCHASE_ID" + Environment.NewLine);
                        sb.Append("       , REC_NO" + Environment.NewLine);
                        sb.Append("       , BREAKDOWN_ID" + Environment.NewLine);
                        sb.Append("       , DELIVER_DIVISION_ID" + Environment.NewLine);
                        sb.Append("       , COMMODITY_ID" + Environment.NewLine);
                        sb.Append("       , COMMODITY_NAME" + Environment.NewLine);
                        sb.Append("       , UNIT_ID" + Environment.NewLine);
                        sb.Append("       , ENTER_NUMBER" + Environment.NewLine);
                        sb.Append("       , CASE_NUMBER" + Environment.NewLine);
                        sb.Append("       , NUMBER" + Environment.NewLine);
                        sb.Append("       , UNIT_PRICE" + Environment.NewLine);
                        sb.Append("       , TAX" + Environment.NewLine);
                        sb.Append("       , NO_TAX_PRICE" + Environment.NewLine);
                        sb.Append("       , PRICE" + Environment.NewLine);
                        sb.Append("       , TAX_DIVISION_ID" + Environment.NewLine);
                        sb.Append("       , TAX_PERCENT" + Environment.NewLine);
                        sb.Append("       , PURCHASE_ORDER_ID" + Environment.NewLine);
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
                        sb.Append("SELECT  " + companyId + Environment.NewLine);                                                        // COMPANY_ID
                        sb.Append("       ," + groupId + Environment.NewLine);                                                          // GROUP_ID
                        sb.Append("       ," + id.ToString() + Environment.NewLine);                                                    // PURCHASE_ID
                        sb.Append("       ," + entityD[i].rec_no + Environment.NewLine);                                                // REC_NO
                        sb.Append("       ," + entityD[i].breakdown_id + Environment.NewLine);                                          // BREAKDOWN_ID
                        sb.Append("       ," + entityD[i].deliver_division_id + Environment.NewLine);                                   // DELIVER_DIVISION_ID
                        sb.Append("       ," + ExEscape.zRepStr(ExCast.zNumZeroNothingFormat(entityD[i].commodity_id)) + Environment.NewLine);     // COMMODITY_ID
                        sb.Append("       ," + ExEscape.zRepStr(entityD[i].commodity_name) + Environment.NewLine);                                 // COMMODITY_NAME
                        sb.Append("       ," + entityD[i].unit_id + Environment.NewLine);                                               // UNIT_ID
                        sb.Append("       ," + entityD[i].enter_number + Environment.NewLine);                                          // ENTER_NUMBER
                        sb.Append("       ," + entityD[i].case_number + Environment.NewLine);                                           // CASE_NUMBER
                        sb.Append("       ," + entityD[i].number + Environment.NewLine);                                                // NUMBER
                        sb.Append("       ," + entityD[i].unit_price + Environment.NewLine);                                            // UNIT_PRICE
                        sb.Append("       ," + entityD[i].tax + Environment.NewLine);                                                   // TAX
                        sb.Append("       ," + entityD[i].no_tax_price + Environment.NewLine);                                          // NO_TAX_PRICE
                        sb.Append("       ," + entityD[i].price + Environment.NewLine);                                                 // PRICE
                        sb.Append("       ," + entityD[i].tax_division_id + Environment.NewLine);                                       // TAX_DIVISION_ID
                        sb.Append("       ," + entityD[i].tax_percent + Environment.NewLine);                                           // TAX_PERCENT
                        sb.Append("       ," + entityD[i].purchase_order_id + Environment.NewLine);                                     // PURCHASE_ORDER_ID
                        sb.Append("       ," + ExEscape.zRepStr(entityD[i].memo) + Environment.NewLine);                                // MEMO

                        if (type == 0)
                        {
                            sb.Append(CommonUtl.GetInsSQLCommonColums(CommonUtl.UpdKbn.Upd,
                                                                      PG_NM,
                                                                      "T_PURCHASE_D",
                                                                      entityH.update_person_id,
                                                                      ExCast.zCStr(entityH.id),
                                                                      ipAdress,
                                                                      userId));
                        }
                        else
                        {
                            sb.Append(CommonUtl.GetInsSQLCommonColums(CommonUtl.UpdKbn.Ins,
                                                                      PG_NM,
                                                                      "T_PURCHASE_D",
                                                                      entityH.update_person_id,
                                                                      "0",
                                                                      ipAdress,
                                                                      userId));
                        }

                        #endregion

                        if (entityD[i].breakdown_id <= 3 && string.IsNullOrEmpty(entityD[i].commodity_id))
                        {
                            // 摘要以外で商品未選択の場合、登録しない
                        }
                        else
                        {
                            db.ExecuteSQL(sb.ToString(), false);

                            #region Update Commodity Inventory

                            try
                            {
                                if (entityD[i].inventory_management_division_id == 1)
                                {
                                    DataCommodityInventory.UpdCommodityInventory(companyId, 
                                                                                 groupId,
                                                                                 db,
                                                                                 ExCast.zNumZeroNothingFormat(entityD[i].commodity_id),
                                                                                 entityD[i].number,
                                                                                 PG_NM,
                                                                                 ExCast.zCInt(entityH.update_person_id),
                                                                                 ipAdress,
                                                                                 userId);
                                }
                            }
                            catch (Exception ex)
                            {
                                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdatePurchase(UpdatePurchaseCredit)", ex);
                                return "UpdatePurchase(UpdatePurchaseCredit) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
                            }

                            #endregion

                            #region Update Deliver Division

                            try
                            {
                                DataDeliverDivision.UpdDeliverDivision(DataDeliverDivision.gUpdDeliverDivisionKbn.Purchase, 
                                                                       companyId,
                                                                       groupId,
                                                                       ExCast.zCLng(entityD[i].purchase_order_id),
                                                                       entityD[i].rec_no,
                                                                       db,
                                                                       entityD[i].deliver_division_id,
                                                                       PG_NM,
                                                                       ExCast.zCInt(entityH.update_person_id),
                                                                       ipAdress,
                                                                       userId);
                            }
                            catch (Exception ex)
                            {
                                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdatePurchase(UpdateDeliverDivisio)", ex);
                                return "UpdatePurchase(UpdateDeliverDivisio) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
                            }

                            #endregion

                            #region Set Entity InOutDelivery

                            EntityInOutDeliveryD _entityInOutDeliveryD = new EntityInOutDeliveryD();
                            _entityInOutDeliveryD.rec_no = entityD[i].rec_no;
                            _entityInOutDeliveryD.commodity_id = entityD[i].commodity_id;
                            _entityInOutDeliveryD.commodity_name = entityD[i].commodity_name;
                            _entityInOutDeliveryD.unit_id = entityD[i].unit_id;
                            _entityInOutDeliveryD.enter_number = entityD[i].enter_number;
                            _entityInOutDeliveryD.case_number = entityD[i].case_number;
                            _entityInOutDeliveryD.number = entityD[i].number;
                            _entityInOutDeliveryListD.Add(_entityInOutDeliveryD);

                            #endregion

                        }
                    }
                }
                catch (Exception ex)
                {
                    db.ExRollbackTransaction();
                    CommonUtl.ExLogger.Error(CLASS_NM + ".UpdatePurchase(Detail Insert)", ex);
                    return "UpdatePurchase(Detail Insert) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
                }

                #endregion

                #region Head Update

                if (type == 0)
                {
                    try
                    {
                        sb.Length = 0;
                        sb.Append("UPDATE T_PURCHASE_H " + Environment.NewLine);
                        sb.Append(CommonUtl.GetUpdSQLCommonColums(PG_NM,
                                                                  entityH.update_person_id,
                                                                  ipAdress,
                                                                  userId,
                                                                  1));
                        sb.Append(" WHERE COMPANY_ID = " + companyId + Environment.NewLine);        // COMPANY_ID
                        sb.Append("   AND GROUP_ID = " + groupId + Environment.NewLine);            // GROUP_ID
                        sb.Append("   AND ID = " + entityH.id.ToString() + Environment.NewLine);    // ID

                        db.ExecuteSQL(sb.ToString(), false);

                    }
                    catch (Exception ex)
                    {
                        db.ExRollbackTransaction();
                        CommonUtl.ExLogger.Error(CLASS_NM + ".UpdatePurchase(Head Update)", ex);
                        return "UpdatePurchase(Head Update) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
                    }
                }

                #endregion

                #region Detail Update

                if (type == 0)
                {
                    try
                    {
                        sb.Length = 0;
                        sb.Append("UPDATE T_PURCHASE_D " + Environment.NewLine);
                        sb.Append(CommonUtl.GetUpdSQLCommonColums(PG_NM,
                                                                  entityH.update_person_id,
                                                                  ipAdress,
                                                                  userId,
                                                                  1));
                        sb.Append(" WHERE COMPANY_ID = " + companyId + Environment.NewLine);
                        sb.Append("   AND GROUP_ID = " + groupId + Environment.NewLine);
                        sb.Append("   AND PURCHASE_ID = " + entityH.id.ToString() + Environment.NewLine);

                        db.ExecuteSQL(sb.ToString(), false);

                    }
                    catch (Exception ex)
                    {
                        db.ExRollbackTransaction();
                        CommonUtl.ExLogger.Error(CLASS_NM + ".UpdatePurchase(Detail Update)", ex);
                        return "UpdatePurchase(Detail Update) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
                    }
                }

                #endregion

                #region Update InOutDelivery

                try
                {
                    svcInOutDelivery _svcInOutDelivery = new svcInOutDelivery();
                    _entityInOutDeliveryH.in_out_delivery_ymd = entityH.purchase_ymd;
                    _entityInOutDeliveryH.in_out_delivery_kbn = 1;          // 入出庫区分：入庫
                    _entityInOutDeliveryH.in_out_delivery_proc_kbn = 3;     // 入出庫処理区分：売上
                    _entityInOutDeliveryH.in_out_delivery_to_kbn = 2;       // 入出庫先区分：仕入先
                    _entityInOutDeliveryH.update_person_id = entityH.update_person_id;
                    _entityInOutDeliveryH.purchase_id = entityH.purchase_id;

                    for (int i = 0; i <= _entityInOutDeliveryListD.Count - 1; i++)
                    {
                        _entityInOutDeliveryH.sum_enter_number += _entityInOutDeliveryListD[i].enter_number;
                        _entityInOutDeliveryH.sum_case_number += _entityInOutDeliveryListD[i].case_number;
                        _entityInOutDeliveryH.sum_number += _entityInOutDeliveryListD[i].number;
                    }

                    long in_out_delivery_no = 0;
                    long in_out_delivery_id = 0;

                    if (type == 0)
                    {
                        DataSlipNo.GetInOutDeliveryNo(companyId, groupId, db, DataSlipNo.geInOutDeliverySlipKbn.Purchase, no, ref in_out_delivery_no, ref in_out_delivery_id);
                        _entityInOutDeliveryH.no = in_out_delivery_no;
                        _entityInOutDeliveryH.id = in_out_delivery_id;
                    }

                    ret_message = _svcInOutDelivery.UpdateInOutDeliveryExcExc(random, type, 3, in_out_delivery_no, no, _entityInOutDeliveryH, _entityInOutDeliveryListD, null, null);
                    if (ret_message.IndexOf("Auto Insert success : ") == -1 && !string.IsNullOrEmpty(ret_message))
                    {
                        return "UpdatePurchase(Update InOutDelivery) : " + ret_message;
                    }
                }
                catch (Exception ex)
                {
                    db.ExRollbackTransaction();
                    CommonUtl.ExLogger.Error(CLASS_NM + ".UpdatePurchase(Update InOutDelivery)", ex);
                    return "UpdatePurchase(Update InOutDelivery) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
                }

                #endregion

                #region Update Slip No

                try
                {
                    if (type == 0)
                    {
                        DataSlipNo.UpdateSlipNo(companyId, groupId, db, DataSlipNo.geSlipKbn.Vendor, accountPeriod, 0, id);
                    }
                    else
                    {
                        DataSlipNo.UpdateSlipNo(companyId, groupId, db, DataSlipNo.geSlipKbn.Vendor, accountPeriod, no, id);
                    }

                    if (no == 0 || id == 0)
                    {
                        return "伝票番号の更新に失敗しました。(会計年 : " + accountPeriod + ")";
                    }
                }
                catch (Exception ex)
                {
                    CommonUtl.ExLogger.Error(CLASS_NM + ".UpdatePurchase(UpdateSlipNo)", ex);
                    return "UpdatePurchase(UpdateSlipNo) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
                }

                #endregion

            }

            #endregion

            #region Delete

            if (type == 2)
            {
                #region Delete InOutDelivery

                try
                {
                    svcInOutDelivery _svcInOutDelivery = new svcInOutDelivery();

                    long in_out_delivery_no = 0;
                    long in_out_delivery_id = 0;
                    DataSlipNo.GetInOutDeliveryNo(companyId, groupId, db, DataSlipNo.geInOutDeliverySlipKbn.Purchase, no, ref in_out_delivery_no, ref in_out_delivery_id);
                    _entityInOutDeliveryH.no = in_out_delivery_no;
                    _entityInOutDeliveryH.id = in_out_delivery_id;

                    ret_message = _svcInOutDelivery.UpdateInOutDeliveryExcExc(random, type, 3, in_out_delivery_no, no, _entityInOutDeliveryH, _entityInOutDeliveryListD, null, null);
                    if (ret_message.IndexOf("Auto Insert success : ") == -1 && !string.IsNullOrEmpty(ret_message))
                    {
                        return "UpdatePurchase(Delete InOutDelivery) : " + ret_message;
                    }
                }
                catch (Exception ex)
                {
                    db.ExRollbackTransaction();
                    CommonUtl.ExLogger.Error(CLASS_NM + ".UpdatePurchase(Delete InOutDelivery)", ex);
                    return "UpdatePurchase(Delete InOutDelivery) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
                }

                #endregion

                #region Head Delete

                try
                {
                    sb.Length = 0;
                    sb.Append("UPDATE T_PURCHASE_H " + Environment.NewLine);
                    sb.Append(CommonUtl.GetUpdSQLCommonColums(PG_NM,
                                                              entityH.update_person_id,
                                                              ipAdress,
                                                              userId,
                                                              1));
                    sb.Append(" WHERE COMPANY_ID = " + companyId + Environment.NewLine);        // COMPANY_ID
                    sb.Append("   AND GROUP_ID = " + groupId + Environment.NewLine);            // GROUP_ID
                    sb.Append("   AND ID = " + entityH.id.ToString() + Environment.NewLine);    // ID

                    db.ExecuteSQL(sb.ToString(), false);

                }
                catch (Exception ex)
                {
                    db.ExRollbackTransaction();
                    CommonUtl.ExLogger.Error(CLASS_NM + ".UpdatePurchase(Head Delete)", ex);
                    return "UpdatePurchase(Head Update) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
                }

                #endregion

                #region Detail Delete

                try
                {
                    sb.Length = 0;
                    sb.Append("UPDATE T_PURCHASE_D " + Environment.NewLine);
                    sb.Append(CommonUtl.GetUpdSQLCommonColums(PG_NM,
                                                              entityH.update_person_id,
                                                              ipAdress,
                                                              userId,
                                                              1));
                    sb.Append(" WHERE COMPANY_ID = " + companyId + Environment.NewLine);
                    sb.Append("   AND GROUP_ID = " + groupId + Environment.NewLine);
                    sb.Append("   AND PURCHASE_ID = " + entityH.id.ToString() + Environment.NewLine);

                    db.ExecuteSQL(sb.ToString(), false);

                }
                catch (Exception ex)
                {
                    db.ExRollbackTransaction();
                    CommonUtl.ExLogger.Error(CLASS_NM + ".UpdatePurchase(Detail Delete)", ex);
                    return "UpdatePurchase(Detail Update) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
                }

                #endregion
            }

            #endregion

            #region Update Payment Credit

            try
            {
                double _price = 0;
                switch (type)
                {
                    case 0:         // Update
                        // 前回「掛仕入」の場合
                        if (before_entityH.business_division_id == 1)
                        {
                            _price = (before_entityH.sum_no_tax_price + before_entityH.sum_tax) * -1;
                            DataCredit.UpdPaymentCredit(companyId, groupId, db, before_entityH.purchase_id, _price, PG_NM, ExCast.zCInt(entityH.update_person_id), ipAdress, userId);
                        }
                        // 今回「掛仕入」の場合
                        if (entityH.business_division_id == 1)
                        {
                            _price = entityH.sum_no_tax_price + entityH.sum_tax;
                            DataCredit.UpdPaymentCredit(companyId, groupId, db, entityH.purchase_id, _price, PG_NM, ExCast.zCInt(entityH.update_person_id), ipAdress, userId);
                        }
                        break;
                    case 1:         // Insert
                        // 今回「掛仕入」、または、「都度請求」の場合
                        if (entityH.business_division_id == 1)
                        {
                            _price = entityH.sum_no_tax_price + entityH.sum_tax;
                            DataCredit.UpdPaymentCredit(companyId, groupId, db, entityH.purchase_id, _price, PG_NM, ExCast.zCInt(entityH.update_person_id), ipAdress, userId);
                        }
                        break;
                    case 2:         // Delete
                        // 前回「掛仕入」の場合
                        if (before_entityH.business_division_id == 1)
                        {
                            _price = (before_entityH.sum_no_tax_price + before_entityH.sum_tax) * -1;
                            DataCredit.UpdPaymentCredit(companyId, groupId, db, before_entityH.purchase_id, _price, PG_NM, ExCast.zCInt(entityH.update_person_id), ipAdress, userId);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdatePurchase(UpdatePurchaseCredit)", ex);
                return "UpdatePurchase(UpdatePurchaseCredit) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
            }

            #endregion

            #region Update Commodity Inventory

            try
            {
                // Update or Delete
                if (type == 0 || type == 2)
                {

                    for (int i = 0; i <= before_entityD.Count - 1; i++)
                    {
                        if (before_entityD[i].inventory_management_division_id == 1)
                        {
                            DataCommodityInventory.UpdCommodityInventory(companyId,
                                                                         groupId,
                                                                         db,
                                                                         ExCast.zNumZeroNothingFormat(before_entityD[i].commodity_id),
                                                                         before_entityD[i].number * -1,
                                                                         PG_NM,
                                                                         ExCast.zCInt(entityH.update_person_id),
                                                                         ipAdress,
                                                                         userId);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdatePurchase(UpdateCommodityInventory)", ex);
                return "UpdatePurchase(UpdateCommodityInventory) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
            }

            #endregion

            #region Update Deliver Division

            try
            {
                // Delete
                if (type == 2)
                {
                    for (int i = 0; i <= before_entityD.Count - 1; i++)
                    {
                        // 未納に戻す
                        DataDeliverDivision.UpdDeliverDivision(DataDeliverDivision.gUpdDeliverDivisionKbn.Purchase,
                                                               companyId,
                                                               groupId,
                                                               ExCast.zCLng(before_entityD[i].purchase_order_id),
                                                               before_entityD[i].rec_no,
                                                               db,
                                                               1,
                                                               PG_NM,
                                                               ExCast.zCInt(entityH.update_person_id),
                                                               ipAdress,
                                                               userId);
                    }
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdatePurchase(UpdateDeliverDivisio)", ex);
                return "UpdatePurchase(UpdateDeliverDivisio) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
            }

            #endregion

            #region CommitTransaction

            try
            {
                db.ExCommitTransaction();
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdatePurchase(CommitTransaction)", ex);
                return "UpdatePurchase(CommitTransaction) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
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
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdatePurchase(DbClose)", ex);
                return "UpdatePurchase(DbClose) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
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
                                                   DataPgEvidence.PGName.Purchase.PurchaseInp,
                                                   DataPgEvidence.geOperationType.Update,
                                                   "NO:" + no.ToString() + ",ID:" + id.ToString());
                        break;
                    case 1:
                        svcPgEvidence.gAddEvidence(ExCast.zCInt(HttpContext.Current.Session[ExSession.EVIDENCE_SAVE_FLG]),
                                                   companyId,
                                                   userId,
                                                   ipAdress,
                                                   sessionString,
                                                   DataPgEvidence.PGName.Purchase.PurchaseInp,
                                                   DataPgEvidence.geOperationType.Insert,
                                                   "NO:" + no.ToString() + ",ID:" + id.ToString());
                        break;
                    case 2:
                        svcPgEvidence.gAddEvidence(ExCast.zCInt(HttpContext.Current.Session[ExSession.EVIDENCE_SAVE_FLG]),
                                                   companyId,
                                                   userId,
                                                   ipAdress,
                                                   sessionString,
                                                   DataPgEvidence.PGName.Purchase.PurchaseInp,
                                                   DataPgEvidence.geOperationType.Delete,
                                                   "NO:" + no.ToString() + ",ID:" + id.ToString());
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdatePurchase(Add Evidence)", ex);
                return "UpdatePurchase(Add Evidence) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
            }

            #endregion

            #region Return

            if (type == 1 && PurchaseNo == 0)
            {
                return "Auto Insert success : " + "仕入番号 : " + no.ToString().ToString() + "で登録しました。";
            }
            else
            {
                return "";
            }

            #endregion

        }

        /// <summary>
        /// データ更新
        /// </summary>
        /// <param name="random">セッションランダム文字列</param>
        /// <param name="type">0:Update 1:Insert 2:Delete</param>
        /// <returns></returns>
        [OperationContract()]
        [WebMethod(EnableSession = true)]
        public void UpdatePurchasePrint(string random, int type, string _no)
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
                    CommonUtl.ExLogger.Error(CLASS_NM + ".UpdatePurchasePrint(認証処理) " + _message);
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdatePurchasePrint(認証処理)", ex);
            }

            #endregion

            #region Field

            StringBuilder sb = new StringBuilder();
            DataTable dt;
            ExMySQLData db = null;

            #endregion

            #region Databese Open

            try
            {
                db = new ExMySQLData(ExCast.zCStr(HttpContext.Current.Session[ExSession.DB_CONNECTION_STR]));
                db.DbOpen();
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdatePurchasePrint(DbOpen)", ex);
            }

            #endregion

            #region BeginTransaction

            try
            {
                db.ExBeginTransaction();
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdatePurchasePrint(BeginTransaction)", ex);
            }

            #endregion

            #region Update (発行区分更新)

            try
            {
                sb.Length = 0;
                sb.Append("UPDATE T_PURCHASE_H " + Environment.NewLine);
                sb.Append("   SET PURCHASE_PRINT_FLG = 1" + Environment.NewLine);
                sb.Append(" WHERE COMPANY_ID = " + companyId + Environment.NewLine);        // COMPANY_ID
                sb.Append("   AND GROUP_ID = " + groupId + Environment.NewLine);            // GROUP_ID
                sb.Append("   AND DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND NO IN (" + _no.Replace("<<@escape_comma@>>", ",") + ")" + Environment.NewLine);          // ID

                db.ExecuteSQL(sb.ToString(), false);
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdatePurchasePrint", ex);
            }

            #endregion

            #region CommitTransaction

            try
            {
                db.ExCommitTransaction();
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdatePurchasePrint(CommitTransaction)", ex);
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
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdatePurchasePrint(DbClose)", ex);
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
                                                   DataPgEvidence.PGName.Purchase.PurchaseDPrint,
                                                   DataPgEvidence.geOperationType.Update,
                                                   "NO:" + _no.Replace("<<@escape_comma@>>", ","));
                        break;
                    case 1:
                        svcPgEvidence.gAddEvidence(ExCast.zCInt(HttpContext.Current.Session[ExSession.EVIDENCE_SAVE_FLG]),
                                                   companyId,
                                                   userId,
                                                   ipAdress,
                                                   sessionString,
                                                   DataPgEvidence.PGName.Purchase.PurchaseDPrint,
                                                   DataPgEvidence.geOperationType.Insert,
                                                   "NO:" + _no.Replace("<<@escape_comma@>>", ","));
                        break;
                    case 2:
                        svcPgEvidence.gAddEvidence(ExCast.zCInt(HttpContext.Current.Session[ExSession.EVIDENCE_SAVE_FLG]),
                                                   companyId,
                                                   userId,
                                                   ipAdress,
                                                   sessionString,
                                                   DataPgEvidence.PGName.Purchase.PurchaseDPrint,
                                                   DataPgEvidence.geOperationType.Delete,
                                                   "NO:" + _no.Replace("<<@escape_comma@>>", ","));
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdatePurchasePrint(Add Evidence)", ex);
            }

            #endregion

        }

        #endregion

    }
}
