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
    public class svcEstimate
    {
        private const string CLASS_NM = "svcEstimate";
        private readonly string PG_NM = DataPgEvidence.PGName.Estimate.EstimateInp;

        #region データ取得

        /// <summary> 
        /// ヘッダリスト取得
        /// </summary>
        /// <param name="EstimateIDFrom"></param>
        /// <param name="EstimateIDTo"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebMethod(EnableSession = true)]
        public EntityEstimateH GetEstimateH(string random, long EstimateIDFrom, long EstimateIDTo)
        {

            EntityEstimateH entity = null;

            #region 認証処理

            string companyId = "";
            string groupId = "";
            string userId = "";
            string ipAdress = "";
            string sessionString = "";
            int idFigureCustomer = 0;
            try
            {
                companyId = ExCast.zCStr(HttpContext.Current.Session[ExSession.COMPANY_ID]);
                groupId = ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_ID]);
                userId = ExCast.zCStr(HttpContext.Current.Session[ExSession.USER_ID]);
                ipAdress = ExCast.zCStr(HttpContext.Current.Session[ExSession.IP_ADRESS]);
                sessionString = ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]);
                idFigureCustomer = ExCast.zCInt(HttpContext.Current.Session[ExSession.ID_FIGURE_CUSTOMER]);

                string _message = ExSession.SessionUserUniqueCheck(random, ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]), ExCast.zCInt(HttpContext.Current.Session[ExSession.USER_ID]));
                if (_message != "")
                {
                    entity = new EntityEstimateH();
                    entity.message = _message;
                    return entity;
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetEstimateH(認証処理)", ex);
                entity = new EntityEstimateH();
                entity.message = CLASS_NM + ".GetEstimateH : 認証処理に失敗しました。" + Environment.NewLine + ex.Message.ToString();
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
                sb.Append("      ,T.STATE " + Environment.NewLine);
                sb.Append("      ,date_format(T.ESTIMATE_YMD , " + ExEscape.SQL_YMD + ") AS ESTIMATE_YMD" + Environment.NewLine);
                sb.Append("      ,T.PERSON_ID " + Environment.NewLine);
                sb.Append("      ,T.CUSTOMER_ID " + Environment.NewLine);
                sb.Append("      ,CS.NAME AS CUSTOMER_NAME " + Environment.NewLine);
                sb.Append("      ,T.TAX_CHANGE_ID " + Environment.NewLine);
                sb.Append("      ,NM4.DESCRIPTION AS TAX_CHANGE_NAME " + Environment.NewLine);
                sb.Append("      ,T.BUSINESS_DIVISION_ID " + Environment.NewLine);
                sb.Append("      ,NM5.DESCRIPTION AS BUSINESS_DIVISION_NAME " + Environment.NewLine);
                sb.Append("      ,T.SUPPLIER_ID " + Environment.NewLine);
                sb.Append("      ,SU.NAME AS SUPPLIER_NAME " + Environment.NewLine);
                sb.Append("      ,date_format(T.SUPPLY_YMD , " + ExEscape.SQL_YMD + ") AS SUPPLY_YMD" + Environment.NewLine);
                sb.Append("      ,date_format(T.TIME_LIMIT_YMD , " + ExEscape.SQL_YMD + ") AS TIME_LIMIT_YMD" + Environment.NewLine);
                sb.Append("      ,T.SUM_ENTER_NUMBER " + Environment.NewLine);
                sb.Append("      ,T.SUM_CASE_NUMBER " + Environment.NewLine);
                sb.Append("      ,T.SUM_NUMBER " + Environment.NewLine);
                sb.Append("      ,T.SUM_UNIT_PRICE " + Environment.NewLine);
                sb.Append("      ,T.SUM_SALES_COST " + Environment.NewLine);
                sb.Append("      ,T.SUM_TAX " + Environment.NewLine);
                sb.Append("      ,T.SUM_NO_TAX_PRICE " + Environment.NewLine);
                sb.Append("      ,T.SUM_PRICE " + Environment.NewLine);
                sb.Append("      ,T.SUM_PROFITS " + Environment.NewLine);
                sb.Append("      ,T.PROFITS_PERCENT " + Environment.NewLine);
                sb.Append("      ,CS2.CREDIT_LIMIT_PRICE " + Environment.NewLine);
                sb.Append("      ,CS2.SALES_CREDIT_PRICE " + Environment.NewLine);

                sb.Append("      ,CS.UNIT_KIND_ID " + Environment.NewLine);
                sb.Append("      ,CS2.PRICE_FRACTION_PROC_ID " + Environment.NewLine);
                sb.Append("      ,CS2.TAX_FRACTION_PROC_ID " + Environment.NewLine);

                sb.Append("      ,CS.INVOICE_ID " + Environment.NewLine);
                sb.Append("      ,CS2.NAME AS INVOICE_NAME " + Environment.NewLine);
                sb.Append("      ,CS.CREDIT_RATE " + Environment.NewLine);

                sb.Append("      ,T.MEMO " + Environment.NewLine);
                sb.Append("      ,T.UPDATE_PERSON_ID " + Environment.NewLine);
                sb.Append("      ,PS.NAME AS UPDATE_PERSON_NM " + Environment.NewLine);
                sb.Append("      ,date_format(T.UPDATE_DATE , " + ExEscape.SQL_YMD + ") AS UPDATE_DATE" + Environment.NewLine);
                sb.Append("      ,T.UPDATE_TIME " + Environment.NewLine);

                sb.Append("  FROM T_ESTIMATE_H AS T" + Environment.NewLine);

                #region Join

                // 更新担当者
                sb.Append("  LEFT JOIN M_PERSON AS PS" + Environment.NewLine);
                sb.Append("    ON PS.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND PS.DISPLAY_FLG = 1 " + Environment.NewLine);
                sb.Append("   AND PS.COMPANY_ID = " + companyId + Environment.NewLine);
                sb.Append("   AND PS.GROUP_ID = " + groupId + Environment.NewLine);
                sb.Append("   AND PS.ID = T.PERSON_ID" + Environment.NewLine);

                // 得意先
                sb.Append("  LEFT JOIN M_CUSTOMER AS CS" + Environment.NewLine);
                sb.Append("    ON CS.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND CS.DISPLAY_FLG = 1 " + Environment.NewLine);
                sb.Append("   AND CS.COMPANY_ID = " + companyId + Environment.NewLine);
                sb.Append("   AND T.CUSTOMER_ID = CS.ID" + Environment.NewLine);

                // 請求先
                sb.Append("  LEFT JOIN M_CUSTOMER AS CS2" + Environment.NewLine);
                sb.Append("    ON CS2.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND CS2.DISPLAY_FLG = 1 " + Environment.NewLine);
                sb.Append("   AND CS2.COMPANY_ID = " + companyId + Environment.NewLine);
                sb.Append("   AND CS2.ID = CS.INVOICE_ID" + Environment.NewLine);

                // 税転換
                sb.Append("  LEFT JOIN SYS_M_NAME AS NM4" + Environment.NewLine);
                sb.Append("    ON NM4.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND NM4.DISPLAY_FLG = 1 " + Environment.NewLine);
                sb.Append("   AND NM4.DIVISION_ID = " + (int)CommonUtl.geNameKbn.TAX_CHANGE_ID + Environment.NewLine);
                sb.Append("   AND T.TAX_CHANGE_ID = NM4.ID" + Environment.NewLine);

                // 取引区分
                sb.Append("  LEFT JOIN SYS_M_NAME AS NM5" + Environment.NewLine);
                sb.Append("    ON NM5.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND NM5.DISPLAY_FLG = 1 " + Environment.NewLine);
                sb.Append("   AND NM5.DIVISION_ID = " + (int)CommonUtl.geNameKbn.BUSINESS_DIVISION_ID + Environment.NewLine);
                sb.Append("   AND T.BUSINESS_DIVISION_ID = NM5.ID" + Environment.NewLine);

                // 納入先
                sb.Append("  LEFT JOIN M_SUPPLIER AS SU" + Environment.NewLine);
                sb.Append("    ON SU.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND SU.DISPLAY_FLG = 1 " + Environment.NewLine);
                sb.Append("   AND SU.COMPANY_ID = " + companyId + Environment.NewLine);
                sb.Append("   AND T.CUSTOMER_ID = SU.CUSTOMER_ID" + Environment.NewLine);
                sb.Append("   AND T.SUPPLIER_ID = SU.ID" + Environment.NewLine);

                #endregion

                sb.Append(" WHERE T.COMPANY_ID = " + companyId + Environment.NewLine);
                sb.Append("   AND T.GROUP_ID = " + groupId + Environment.NewLine);
                sb.Append("   AND T.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND T.NO >= " + EstimateIDFrom.ToString() + Environment.NewLine);
                sb.Append("   AND T.NO <= " + EstimateIDTo.ToString() + Environment.NewLine);

                sb.Append(" ORDER BY T.COMPANY_ID " + Environment.NewLine);
                sb.Append("         ,T.GROUP_ID " + Environment.NewLine);
                sb.Append("         ,T.ESTIMATE_YMD DESC " + Environment.NewLine);
                sb.Append("         ,T.NO DESC " + Environment.NewLine);
                sb.Append(" LIMIT 0, 1");

                #endregion

                dt = db.GetDataTable(sb.ToString());

                if (dt.DefaultView.Count > 0)
                {
                    entity = new EntityEstimateH();

                    // 排他制御
                    DataPgLock.geLovkFlg lockFlg = DataPgLock.geLovkFlg.UnLock;
                    string strErr = DataPgLock.SetLockPg(companyId, userId, PG_NM, groupId + "-" + ExCast.zNumZeroNothingFormat(EstimateIDFrom.ToString()), ipAdress, db, out lockFlg);
                    if (strErr != "")
                    {
                        entity.message = CLASS_NM + ".GetCommodity : 排他制御(ロック情報取得)に失敗しました。" + Environment.NewLine + strErr;
                    }

                    #region Set Entity

                    entity.id = ExCast.zCLng(dt.DefaultView[0]["ID"]);
                    entity.no = ExCast.zCLng(dt.DefaultView[0]["NO"]);
                    entity.state = ExCast.zCInt(dt.DefaultView[0]["STATE"]);
                    entity.estimate_ymd = ExCast.zDateNullToDefault(dt.DefaultView[0]["ESTIMATE_YMD"]);

                    entity.customer_id = ExCast.zFormatForID(dt.DefaultView[0]["CUSTOMER_ID"], idFigureCustomer);
                    entity.customer_name = ExCast.zCStr(dt.DefaultView[0]["CUSTOMER_NAME"]);

                    entity.tax_change_id = ExCast.zCInt(dt.DefaultView[0]["TAX_CHANGE_ID"]);
                    entity.tax_change_name = ExCast.zCStr(dt.DefaultView[0]["TAX_CHANGE_NAME"]);

                    entity.business_division_id = ExCast.zCInt(dt.DefaultView[0]["BUSINESS_DIVISION_ID"]);
                    entity.business_division_name = ExCast.zCStr(dt.DefaultView[0]["BUSINESS_DIVISION_NAME"]);

                    entity.supplier_id = ExCast.zFormatForID(dt.DefaultView[0]["SUPPLIER_ID"], idFigureCustomer);
                    entity.supplier_name = ExCast.zCStr(dt.DefaultView[0]["SUPPLIER_NAME"]);

                    entity.supply_ymd = ExCast.zDateNullToDefault(dt.DefaultView[0]["SUPPLY_YMD"]);
                    entity.time_limit_ymd = ExCast.zDateNullToDefault(dt.DefaultView[0]["TIME_LIMIT_YMD"]);
                    entity.sum_enter_number = ExCast.zCDbl(dt.DefaultView[0]["SUM_ENTER_NUMBER"]);
                    entity.sum_case_number = ExCast.zCDbl(dt.DefaultView[0]["SUM_CASE_NUMBER"]);
                    entity.sum_number = ExCast.zCDbl(dt.DefaultView[0]["SUM_NUMBER"]);
                    entity.sum_unit_price = ExCast.zCDbl(dt.DefaultView[0]["SUM_UNIT_PRICE"]);
                    entity.sum_sales_cost = ExCast.zCDbl(dt.DefaultView[0]["SUM_SALES_COST"]);
                    entity.sum_tax = ExCast.zCDbl(dt.DefaultView[0]["SUM_TAX"]);
                    entity.sum_no_tax_price = ExCast.zCDbl(dt.DefaultView[0]["SUM_NO_TAX_PRICE"]);
                    entity.sum_price = ExCast.zCDbl(dt.DefaultView[0]["SUM_PRICE"]);
                    entity.sum_profits = ExCast.zCDbl(dt.DefaultView[0]["SUM_PROFITS"]);
                    entity.profits_percent = ExCast.zCDbl(dt.DefaultView[0]["PROFITS_PERCENT"]);
                    entity.credit_limit_price = ExCast.zCDbl(dt.DefaultView[0]["CREDIT_LIMIT_PRICE"]);
                    entity.sales_credit_price = ExCast.zCDbl(dt.DefaultView[0]["SALES_CREDIT_PRICE"]);

                    entity.unit_kind_id = ExCast.zCInt(dt.DefaultView[0]["UNIT_KIND_ID"]);
                    entity.price_fraction_proc_id = ExCast.zCInt(dt.DefaultView[0]["PRICE_FRACTION_PROC_ID"]);
                    entity.tax_fraction_proc_id = ExCast.zCInt(dt.DefaultView[0]["TAX_FRACTION_PROC_ID"]);

                    entity.invoice_id = ExCast.zCStr(dt.DefaultView[0]["INVOICE_ID"]);
                    entity.invoice_name = ExCast.zCStr(dt.DefaultView[0]["INVOICE_NAME"]);
                    entity.credit_rate = ExCast.zCInt(dt.DefaultView[0]["CREDIT_RATE"]);

                    entity.memo = ExCast.zCStr(dt.DefaultView[0]["MEMO"]);

                    entity.update_person_id = ExCast.zCInt(dt.DefaultView[0]["PERSON_ID"]);
                    entity.update_person_nm = ExCast.zCStr(dt.DefaultView[0]["UPDATE_PERSON_NM"]);

                    entity.lock_flg = (int)lockFlg;

                    // 受注計上済チェック
                    if (DataExists.IsExistData(db, companyId, groupId, "T_ORDER_H", "ESTIMATENO", entity.no.ToString(), CommonUtl.geStrOrNumKbn.Number))
                    {
                        entity.order_allocation_flg = 1;
                    }

                    // 売上計上済チェック
                    if (DataExists.IsExistData(db, companyId, groupId, "T_SALES_H", "ESTIMATENO", entity.no.ToString(), CommonUtl.geStrOrNumKbn.Number))
                    {
                        entity.sales_allocation_flg = 1;
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
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetEstimateH", ex);
                entity = new EntityEstimateH();
                entity.message = CLASS_NM + ".GetEstimateH : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message.ToString();
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
                                       DataPgEvidence.PGName.Estimate.EstimateInp,
                                       DataPgEvidence.geOperationType.Select,
                                       "NO:" + EstimateIDFrom.ToString() + ",NO:" + EstimateIDTo.ToString());

            return entity;


        }

        /// <summary> 
        /// 受注明細リスト取得
        /// </summary>
        /// <param name="EstimateIDFrom"></param>
        /// <param name="EstimateIDTo"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebMethod(EnableSession = true)]
        public List<EntityEstimateD> GetEstimateListD(string random, long EstimateIDFrom, long EstimateIDTo)
        {
            List<EntityEstimateD> entityList = new List<EntityEstimateD>();

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
                    EntityEstimateD entity = new EntityEstimateD();
                    entity.message = _message;
                    entityList.Add(entity);
                    return entityList;
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetEstimateListD(認証処理)", ex);
                EntityEstimateD entity = new EntityEstimateD();
                entity.message = "認証処理に失敗しました。" + Environment.NewLine + ex.Message.ToString(); ;
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

                sb.Append("SELECT OD.Estimate_ID " + Environment.NewLine);
                sb.Append("      ,OD.REC_NO " + Environment.NewLine);
                sb.Append("      ,OD.Estimate_ID " + Environment.NewLine);
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
                sb.Append("      ,OD.SALES_COST " + Environment.NewLine);
                sb.Append("      ,OD.TAX " + Environment.NewLine);
                sb.Append("      ,OD.NO_TAX_PRICE " + Environment.NewLine);
                sb.Append("      ,OD.PRICE " + Environment.NewLine);
                sb.Append("      ,OD.PROFITS " + Environment.NewLine);
                sb.Append("      ,OD.PROFITS_PERCENT " + Environment.NewLine);
                sb.Append("      ,OD.MEMO " + Environment.NewLine);
                sb.Append("      ,OD.TAX_DIVISION_ID " + Environment.NewLine);
                sb.Append("      ,NM4.DESCRIPTION AS TAX_DIVISION_NM " + Environment.NewLine);
                sb.Append("      ,OD.TAX_PERCENT " + Environment.NewLine);
                sb.Append("      ,CT.INVENTORY_NUMBER " + Environment.NewLine);
                sb.Append("      ,CT.INVENTORY_MANAGEMENT_DIVISION_ID " + Environment.NewLine);
                sb.Append("      ,CT.NUMBER_DECIMAL_DIGIT " + Environment.NewLine);
                sb.Append("      ,CT.UNIT_DECIMAL_DIGIT " + Environment.NewLine);
                sb.Append("  FROM T_ESTIMATE_D AS OD" + Environment.NewLine);

                #region Join

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
                sb.Append("   AND OD.Estimate_ID >= " + EstimateIDFrom.ToString() + Environment.NewLine);
                sb.Append("   AND OD.Estimate_ID <= " + EstimateIDTo.ToString() + Environment.NewLine);

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
                        EntityEstimateD entityD = new EntityEstimateD();

                        #region Set Entity

                        entityD.id = ExCast.zCLng(dt.DefaultView[i]["ESTIMATE_ID"]);
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
                        entityD.sales_cost = ExCast.zCDbl(dt.DefaultView[i]["SALES_COST"]);
                        entityD.tax = ExCast.zCDbl(dt.DefaultView[i]["TAX"]);
                        entityD.no_tax_price = ExCast.zCDbl(dt.DefaultView[i]["NO_TAX_PRICE"]);
                        entityD.price = ExCast.zCDbl(dt.DefaultView[i]["PRICE"]);
                        entityD.profits = ExCast.zCDbl(dt.DefaultView[i]["PROFITS"]);
                        entityD.profits_percent = ExCast.zCDbl(dt.DefaultView[i]["PROFITS_PERCENT"]);
                        entityD.memo = ExCast.zCStr(dt.DefaultView[i]["MEMO"]);
                        entityD.tax_division_id = ExCast.zCInt(dt.DefaultView[i]["TAX_DIVISION_ID"]);
                        entityD.tax_division_nm = ExCast.zCStr(dt.DefaultView[i]["TAX_DIVISION_NM"]);
                        entityD.tax_percent = ExCast.zCInt(dt.DefaultView[i]["TAX_PERCENT"]);
                        entityD.inventory_number = ExCast.zCDbl(dt.DefaultView[i]["INVENTORY_NUMBER"]);
                        entityD.inventory_management_division_id = ExCast.zCInt(dt.DefaultView[i]["INVENTORY_MANAGEMENT_DIVISION_ID"]);
                        entityD.number_decimal_digit = ExCast.zCInt(dt.DefaultView[i]["NUMBER_DECIMAL_DIGIT"]);
                        entityD.unit_decimal_digit = ExCast.zCInt(dt.DefaultView[i]["UNIT_DECIMAL_DIGIT"]);
                        entityList.Add(entityD);

                        #endregion
                    }
                }

                return entityList;

            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetEstimateListD", ex);
                entityList.Clear();
                EntityEstimateD entityD = new EntityEstimateD();
                entityD.message = CLASS_NM + ".GetEstimateH : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message.ToString();
                entityList.Add(entityD);
                return entityList;
            }
            finally
            {
                db = null;
            }

        }

        /// <summary>　
        /// リスト取得
        /// </summary>
        /// <param name="strWhereSql"></param>
        /// <param name="strEstimateBySql"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebMethod(EnableSession = true)]
        public List<EntityEstimate> GetEstimateList(string random, string strWhereSql, string strEstimateBySql)
        {
            List<EntityEstimate> entityList = new List<EntityEstimate>();

            #region 認証処理

            string companyId = "";
            string groupId = "";
            string userId = "";
            string ipAdress = "";
            string sessionString = "";
            int idFigureCommodity = 0;
            int idFigureCustomer = 0;
            int idFigurePurchase = 0;
            int idFigureSlipNo = 0;

            try
            {
                companyId = ExCast.zCStr(HttpContext.Current.Session[ExSession.COMPANY_ID]);
                groupId = ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_ID]);
                userId = ExCast.zCStr(HttpContext.Current.Session[ExSession.USER_ID]);
                ipAdress = ExCast.zCStr(HttpContext.Current.Session[ExSession.IP_ADRESS]);
                sessionString = ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]);
                idFigureCommodity = ExCast.zCInt(HttpContext.Current.Session[ExSession.ID_FIGURE_GOODS]);
                idFigureCustomer = ExCast.zCInt(HttpContext.Current.Session[ExSession.ID_FIGURE_CUSTOMER]);
                idFigurePurchase = ExCast.zCInt(HttpContext.Current.Session[ExSession.ID_FIGURE_PURCHASE]);
                idFigureSlipNo = ExCast.zCInt(HttpContext.Current.Session[ExSession.ID_FIGURE_SLIP_NO]);

                string _message = ExSession.SessionUserUniqueCheck(random, ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]), ExCast.zCInt(HttpContext.Current.Session[ExSession.USER_ID]));
                if (_message != "")
                {
                    EntityEstimate entity = new EntityEstimate();
                    entity.MESSAGE = _message;
                    entityList.Add(entity);
                    return entityList;
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetEstimateList(認証処理)", ex);
                EntityEstimate entity = new EntityEstimate();
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
                sb.Append(rptMgr.GetEstimateListReportSQL(companyId, groupId, strWhereSql, strEstimateBySql));

                dt = db.GetDataTable(sb.ToString());

                if (dt.DefaultView.Count > 0)
                {
                    for (int i = 0; i <= dt.DefaultView.Count - 1; i++)
                    {

                        #region Set Entity

                        EntityEstimate _entity = new EntityEstimate();

                        _entity.ID = ExCast.zCLng(dt.DefaultView[i]["ID"]);
                        _entity.NO = ExCast.zFormatForID(dt.DefaultView[i]["NO"], idFigureSlipNo);
                        _entity.ESTIMATE_YMD = ExCast.zDateNullToDefault(dt.DefaultView[i]["Estimate_YMD"]);
                        _entity.CUSTOMER_ID = ExCast.zFormatForID(dt.DefaultView[i]["CUSTOMER_ID"], idFigureCustomer);
                        _entity.CUSTOMER_NM = ExCast.zCStr(dt.DefaultView[i]["CUSTOMER_NM"]);
                        _entity.CUSTOMER_PERSON_NAME = ExCast.zCStr(dt.DefaultView[i]["CUSTOMER_PERSON_NAME"]);
                        _entity.TITLE_NAME = ExCast.zCStr(dt.DefaultView[i]["TITLE_NAME"]);
                        //if (_entity.CUSTOMER_PERSON_NAME == "")
                        //{
                        //    _entity.TITLE_NAME2 = "";
                        //}
                        //else
                        //{
                        //    _entity.TITLE_NAME = "";
                        //    _entity.TITLE_NAME2 = ExCast.zCStr(dt.DefaultView[i]["TITLE_NAME"]); 
                        //}
                        _entity.TAX_CHANGE_ID = ExCast.zCInt(dt.DefaultView[i]["TAX_CHANGE_ID"]);
                        _entity.TAX_CHANGE_NM = ExCast.zCStr(dt.DefaultView[i]["TAX_CHANGE_NM"]);
                        _entity.BUSINESS_DIVISION_ID = ExCast.zCInt(dt.DefaultView[i]["BUSINESS_DIVISION_ID"]);
                        _entity.BUSINESS_DIVISION_NM = ExCast.zCStr(dt.DefaultView[i]["BISNESS_DIVISON_NM"]);
                        _entity.SUPPLIER_ID = ExCast.zFormatForID(dt.DefaultView[i]["SUPPLIER_ID"], idFigureCustomer);
                        _entity.SUPPLIER_NM = ExCast.zCStr(dt.DefaultView[i]["SUPPLIER_NM"]);
                        _entity.SUPPLY_YMD = ExCast.zDateNullToDefault(dt.DefaultView[i]["SUPPLY_YMD"]).Replace(" 頃", "");
                        _entity.TIME_LIMIT_YMD = ExCast.zDateNullToDefault(dt.DefaultView[i]["TIME_LIMIT_YMD"]).Replace(" 頃迄", "");
                        _entity.MEMO = ExCast.zCStr(dt.DefaultView[i]["MEMO"]);
                        _entity.UPDATE_PERSON_ID = ExCast.zCInt(dt.DefaultView[i]["INPUT_PERSON"]);
                        _entity.UPDATE_PERSON_NM = ExCast.zCStr(dt.DefaultView[i]["INPUT_PERSON_NM"]);

                        _entity.COMPANY_NM = ExCast.zCStr(dt.DefaultView[i]["COMPANY_NM"]);
                        string _zip_code = String.Format("{0:0000000}", ExCast.zCDbl(dt.DefaultView[i]["COMPANY_ZIP_CODE"]));
                        _zip_code = _zip_code.Substring(0, 3) + "-" + _zip_code.Substring(3, 4);
                        _entity.COMPANY_ZIP_CODE = _zip_code;
                        _entity.COMPANY_ADRESS1 = ExCast.zCStr(dt.DefaultView[i]["COMPANY_ADRESS1"]);
                        _entity.COMPANY_ADRESS2 = ExCast.zCStr(dt.DefaultView[i]["COMPANY_ADRESS2"]);
                        _entity.COMPANY_TEL = ExCast.zCStr(dt.DefaultView[i]["COMPANY_TEL"]);
                        _entity.COMPANY_FAX = ExCast.zCStr(dt.DefaultView[i]["COMPANY_FAX"]);
                        _entity.COMPANY_MAIL_ADRESS = ExCast.zCStr(dt.DefaultView[i]["COMPANY_MAIL_ADRESS"]);
                        _entity.COMPANY_URL = ExCast.zCStr(dt.DefaultView[i]["COMPANY_URL"]);

                        _entity.REC_NO = ExCast.zCInt(dt.DefaultView[i]["REC_NO"]);
                        _entity.BREAKDOWN_ID = ExCast.zCInt(dt.DefaultView[i]["BREAKDOWN_ID"]);
                        _entity.BREAKDOWN_NM = ExCast.zCStr(dt.DefaultView[i]["BREAKDOWN_NM"]);
                        _entity.DELIVER_DIVISION_ID = ExCast.zCInt(dt.DefaultView[i]["DELIVER_DIVISION_ID"]);
                        _entity.DELIVER_DIVISION_NM = ExCast.zCStr(dt.DefaultView[i]["DELIVER_DIVISION_NM"]);
                        _entity.COMMODITY_ID = ExCast.zFormatForID(dt.DefaultView[i]["COMMODITY_ID"], idFigureCommodity);
                        _entity.COMMODITY_NAME = ExCast.zCStr(dt.DefaultView[i]["COMMODITY_NAME"]);
                        _entity.UNIT_ID = ExCast.zCInt(dt.DefaultView[i]["UNIT_ID"]);
                        _entity.UNIT_NM = ExCast.zCStr(dt.DefaultView[i]["UNIT_NM"]);
                        _entity.ENTER_NUMBER = ExCast.zCInt(dt.DefaultView[i]["ENTER_NUMBER"]);
                        _entity.NUMBER = ExCast.zCDbl(dt.DefaultView[i]["NUMBER"]);
                        _entity.UNIT_PRICE = ExCast.zCDbl(dt.DefaultView[i]["UNIT_PRICE"]);
                        _entity.PRICE = ExCast.zCDbl(dt.DefaultView[i]["PRICE"]);
                        _entity.D_MEMO = ExCast.zCStr(dt.DefaultView[i]["D_MEMO"]);
                        entityList.Add(_entity);

                        #endregion

                    }
                }

            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetEstimateList", ex);
                entityList.Clear();
                EntityEstimate entity = new EntityEstimate();
                entity.MESSAGE = CLASS_NM + ".GetEstimateList : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message.ToString();
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
                                       DataPgEvidence.PGName.Estimate.EstimateList,
                                       DataPgEvidence.geOperationType.Select,
                                       "Where:" + strWhereSql + ",Estimateby:" + strEstimateBySql);

            return entityList;

        }

        #endregion

        #region データ更新

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="random">セッションランダム文字列</param>
        /// <param name="type">0:Update 1:Insert 2:Delete</param>
        /// <param name="EstimateNo">伝票番号</param>
        /// <param name="entityH">ヘッダデータ</param>
        /// <param name="entityD">明細データ</param>
        /// <returns></returns>
        [OperationContract()]
        [WebMethod(EnableSession = true)]
        public string UpdateEstimate(string random, int type, long EstimateNo, EntityEstimateH entityH, List<EntityEstimateD> entityD)
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
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateEstimate(認証処理)", ex);
                return CLASS_NM + ".UpdateEstimate : 認証処理に失敗しました。" + Environment.NewLine + ex.Message.ToString();
            }

            #endregion

            #region Field

            StringBuilder sb = new StringBuilder();
            DataTable dt;
            ExMySQLData db = null;

            string accountPeriod = "";
            long id = 0;
            long no = EstimateNo;

            #endregion

            #region Databese Open

            try
            {
                db = new ExMySQLData(ExCast.zCStr(HttpContext.Current.Session[ExSession.DB_CONNECTION_STR]));
                db.DbOpen();
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateEstimate(DbOpen)", ex);
                return "UpdateEstimate(DbOpen) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
            }

            #endregion

            #region BeginTransaction

            try
            {
                db.ExBeginTransaction();
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateEstimate(BeginTransaction)", ex);
                return "UpdateEstimate(BeginTransaction) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
            }

            #endregion

            #region Update or Insert

            if (type <= 1)
            {
                #region Get Accout Period

                try
                {
                    accountPeriod = DataAccount.GetAccountPeriod(ExCast.zCStr(HttpContext.Current.Session[ExSession.ACCOUNT_BEGIN_PERIOD]), entityH.estimate_ymd);
                    if (accountPeriod == "")
                    {
                        return "会計年の取得に失敗しました。(期首月日 : " + ExCast.zCStr(HttpContext.Current.Session[ExSession.ACCOUNT_BEGIN_PERIOD]) +
                                                             " 見積日 : " + entityH.estimate_ymd + ")";
                    }
                }
                catch (Exception ex)
                {
                    CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateEstimate(GetAccountPeriod)", ex);
                    return "UpdateEstimate(GetAccountPeriod) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
                }

                #endregion

                #region Get Slip No

                try
                {
                    DataSlipNo.GetSlipNo(companyId,
                                         groupId,
                                         db,
                                         DataSlipNo.geSlipKbn.Estimate,
                                         accountPeriod,
                                         ref no,
                                         out id,
                                         ExCast.zCStr(HttpContext.Current.Session[ExSession.IP_ADRESS]),
                                         ExCast.zCStr(HttpContext.Current.Session[ExSession.USER_ID]));
                    if (no == 0 || id == 0)
                    {
                        return "伝票番号の取得に失敗しました。(会計年 : " + accountPeriod + ")";
                    }
                }
                catch (Exception ex)
                {
                    CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateEstimate(GetSlipNo)", ex);
                    return "UpdateEstimate(GetSlipNo) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
                }

                #endregion

                #region Head Insert

                try
                {
                    #region SQL

                    sb.Length = 0;
                    sb.Append("INSERT INTO T_ESTIMATE_H " + Environment.NewLine);
                    sb.Append("       ( COMPANY_ID" + Environment.NewLine);
                    sb.Append("       , GROUP_ID" + Environment.NewLine);
                    sb.Append("       , ID" + Environment.NewLine);
                    sb.Append("       , NO" + Environment.NewLine);
                    sb.Append("       , STATE" + Environment.NewLine);
                    sb.Append("       , ESTIMATE_YMD" + Environment.NewLine);
                    sb.Append("       , PERSON_ID" + Environment.NewLine);
                    sb.Append("       , CUSTOMER_ID" + Environment.NewLine);
                    sb.Append("       , TAX_CHANGE_ID" + Environment.NewLine);
                    sb.Append("       , BUSINESS_DIVISION_ID" + Environment.NewLine);
                    sb.Append("       , SUPPLIER_ID" + Environment.NewLine);
                    sb.Append("       , SUPPLY_YMD" + Environment.NewLine);
                    sb.Append("       , TIME_LIMIT_YMD" + Environment.NewLine);
                    sb.Append("       , DELIVER_DIVISION_ID" + Environment.NewLine);
                    sb.Append("       , SUM_ENTER_NUMBER" + Environment.NewLine);
                    sb.Append("       , SUM_CASE_NUMBER" + Environment.NewLine);
                    sb.Append("       , SUM_NUMBER" + Environment.NewLine);
                    sb.Append("       , SUM_UNIT_PRICE" + Environment.NewLine);
                    sb.Append("       , SUM_SALES_COST" + Environment.NewLine);
                    sb.Append("       , SUM_TAX" + Environment.NewLine);
                    sb.Append("       , SUM_NO_TAX_PRICE" + Environment.NewLine);
                    sb.Append("       , SUM_PRICE" + Environment.NewLine);
                    sb.Append("       , SUM_PROFITS" + Environment.NewLine);
                    sb.Append("       , PROFITS_PERCENT" + Environment.NewLine);
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
                    sb.Append("SELECT  " + companyId + Environment.NewLine);                                                    // COMPANY_ID
                    sb.Append("       ," + groupId + Environment.NewLine);                                                      // GROUP_ID
                    sb.Append("       ," + id.ToString() + Environment.NewLine);                                                // ID
                    sb.Append("       ," + no.ToString() + Environment.NewLine);                                                // NO
                    sb.Append("       ," + entityH.state + Environment.NewLine);                                                // 
                    sb.Append("       ," + ExEscape.zRepStr(ExCast.zDateNullToDefault(entityH.estimate_ymd)) + Environment.NewLine);        // Estimate_YMD
                    sb.Append("       ," + entityH.update_person_id + Environment.NewLine);                                                 // PERSON_ID
                    sb.Append("       ," + ExEscape.zRepStr(ExCast.zNumZeroNothingFormat(entityH.customer_id)) + Environment.NewLine);      // CUSTOMER_ID
                    sb.Append("       ," + entityH.tax_change_id + Environment.NewLine);                                        // TAX_CHANGE_ID
                    sb.Append("       ," + entityH.business_division_id + Environment.NewLine);                                 // BUSINESS_DIVISION_ID
                    sb.Append("       ," + ExEscape.zRepStr(ExCast.zNumZeroNothingFormat(entityH.supplier_id)) + Environment.NewLine);     // SUPPLIER_ID
                    sb.Append("       ," + ExEscape.zRepStr(ExCast.zTimeNullToDefault(entityH.supply_ymd)) + Environment.NewLine);         // SUPPLY_YMD
                    sb.Append("       ," + ExEscape.zRepStr(ExCast.zTimeNullToDefault(entityH.time_limit_ymd)) + Environment.NewLine);     // SUPPLY_YMD
                    sb.Append("       ,1" + Environment.NewLine);                                                               // DELIVER_DIVISION_ID
                    sb.Append("       ," + ExCast.zCDbl(entityH.sum_enter_number) + Environment.NewLine);                       // SUM_ENTER_NUMBER
                    sb.Append("       ," + ExCast.zCDbl(entityH.sum_case_number) + Environment.NewLine);                        // SUM_CASE_NUMBER
                    sb.Append("       ," + ExCast.zCDbl(entityH.sum_number) + Environment.NewLine);                             // SUM_NUMBER
                    sb.Append("       ," + ExCast.zCDbl(entityH.sum_unit_price) + Environment.NewLine);                         // SUM_UNIT_PRICE
                    sb.Append("       ," + ExCast.zCDbl(entityH.sum_sales_cost) + Environment.NewLine);                         // SUM_SALES_COST
                    sb.Append("       ," + ExCast.zCDbl(entityH.sum_tax) + Environment.NewLine);                                // SUM_TAX
                    sb.Append("       ," + ExCast.zCDbl(entityH.sum_no_tax_price) + Environment.NewLine);                       // SUM_NO_TAX_PRICE
                    sb.Append("       ," + ExCast.zCDbl(entityH.sum_price) + Environment.NewLine);                              // SUM_PRICE
                    sb.Append("       ," + ExCast.zCDbl(entityH.sum_profits) + Environment.NewLine);                            // SUM_PROFITS
                    sb.Append("       ," + ExCast.zCDbl(entityH.profits_percent) + Environment.NewLine);                        // PROFITS_PERCENT
                    sb.Append("       ," + ExEscape.zRepStr(entityH.memo) + Environment.NewLine);                                          // MEMO
                    if (type == 0)
                    {
                        sb.Append(CommonUtl.GetInsSQLCommonColums(CommonUtl.UpdKbn.Upd,
                                                                  PG_NM,
                                                                  "T_ESTIMATE_H",
                                                                  entityH.update_person_id,
                                                                  EstimateNo.ToString(),
                                                                  ipAdress,
                                                                  userId));
                    }
                    else
                    {
                        sb.Append(CommonUtl.GetInsSQLCommonColums(CommonUtl.UpdKbn.Ins,
                                                                  PG_NM,
                                                                  "T_ESTIMATE_H",
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
                    CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateEstimate(Head Insert)", ex);
                    return "UpdateEstimate(Head Insert) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
                }

                #endregion

                #region Detail Insert

                try
                {
                    for (int i = 0; i <= entityD.Count - 1; i++)
                    {
                        #region SQL

                        sb.Length = 0;
                        sb.Append("INSERT INTO T_ESTIMATE_D " + Environment.NewLine);
                        sb.Append("       ( COMPANY_ID" + Environment.NewLine);
                        sb.Append("       , GROUP_ID" + Environment.NewLine);
                        sb.Append("       , ESTIMATE_ID" + Environment.NewLine);
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
                        sb.Append("       , SALES_COST" + Environment.NewLine);
                        sb.Append("       , TAX" + Environment.NewLine);
                        sb.Append("       , NO_TAX_PRICE" + Environment.NewLine);
                        sb.Append("       , PRICE" + Environment.NewLine);
                        sb.Append("       , PROFITS" + Environment.NewLine);
                        sb.Append("       , PROFITS_PERCENT" + Environment.NewLine);
                        sb.Append("       , TAX_DIVISION_ID" + Environment.NewLine);
                        sb.Append("       , TAX_PERCENT" + Environment.NewLine);
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
                        sb.Append("       ," + id.ToString() + Environment.NewLine);                                                    // Estimate_ID
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
                        sb.Append("       ," + entityD[i].sales_cost + Environment.NewLine);                                            // SALES_COST
                        sb.Append("       ," + entityD[i].tax + Environment.NewLine);                                                   // TAX
                        sb.Append("       ," + entityD[i].no_tax_price + Environment.NewLine);                                          // NO_TAX_PRICE
                        sb.Append("       ," + entityD[i].price + Environment.NewLine);                                                 // PRICE
                        sb.Append("       ," + entityD[i].profits + Environment.NewLine);                                               // PROFITS
                        sb.Append("       ," + ExCast.zCDbl(entityD[i].profits_percent) + Environment.NewLine);                         // PROFITS_PERCENT
                        sb.Append("       ," + entityD[i].tax_division_id + Environment.NewLine);                                       // TAX_DIVISION_ID
                        sb.Append("       ," + entityD[i].tax_percent + Environment.NewLine);                                           // TAX_PERCENT
                        sb.Append("       ," + ExEscape.zRepStr(entityD[i].memo) + Environment.NewLine);                                           // MEMO

                        if (type == 0)
                        {
                            sb.Append(CommonUtl.GetInsSQLCommonColums(CommonUtl.UpdKbn.Upd,
                                                                      PG_NM,
                                                                      "T_ESTIMATE_D",
                                                                      entityH.update_person_id,
                                                                      ExCast.zCStr(entityH.id),
                                                                      ipAdress,
                                                                      userId));
                        }
                        else
                        {
                            sb.Append(CommonUtl.GetInsSQLCommonColums(CommonUtl.UpdKbn.Ins,
                                                                      PG_NM,
                                                                      "T_ESTIMATE_D",
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
                        }
                    }
                }
                catch (Exception ex)
                {
                    db.ExRollbackTransaction();
                    CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateEstimate(Detail Insert)", ex);
                    return "UpdateEstimate(Detail Insert) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
                }

                #endregion

                #region Head Update

                if (type == 0)
                {
                    try
                    {
                        sb.Length = 0;
                        sb.Append("UPDATE T_ESTIMATE_H " + Environment.NewLine);
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
                        CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateEstimate(Head Update)", ex);
                        return "UpdateEstimate(Head Update) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
                    }
                }

                #endregion

                #region Detail Update

                if (type == 0)
                {
                    try
                    {
                        sb.Length = 0;
                        sb.Append("UPDATE T_ESTIMATE_D " + Environment.NewLine);
                        sb.Append(CommonUtl.GetUpdSQLCommonColums(PG_NM,
                                                                  entityH.update_person_id,
                                                                  ipAdress,
                                                                  userId,
                                                                  1));
                        sb.Append(" WHERE COMPANY_ID = " + companyId + Environment.NewLine);
                        sb.Append("   AND GROUP_ID = " + groupId + Environment.NewLine);
                        sb.Append("   AND Estimate_ID = " + entityH.id.ToString() + Environment.NewLine);

                        db.ExecuteSQL(sb.ToString(), false);

                    }
                    catch (Exception ex)
                    {
                        db.ExRollbackTransaction();
                        CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateEstimate(Detail Update)", ex);
                        return "UpdateEstimate(Detail Update) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
                    }
                }

                #endregion

                #region Update Slip No

                try
                {
                    if (type == 0)
                    {
                        DataSlipNo.UpdateSlipNo(companyId, groupId, db, DataSlipNo.geSlipKbn.Estimate, accountPeriod, 0, id);
                    }
                    else
                    {
                        DataSlipNo.UpdateSlipNo(companyId, groupId, db, DataSlipNo.geSlipKbn.Estimate, accountPeriod, no, id);
                    }

                    if (no == 0 || id == 0)
                    {
                        return "伝票番号の更新に失敗しました。(会計年 : " + accountPeriod + ")";
                    }
                }
                catch (Exception ex)
                {
                    CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateEstimate(UpdateSlipNo)", ex);
                    return "UpdateEstimate(UpdateSlipNo) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
                }

                #endregion

            }

            #endregion

            #region Delete

            if (type == 2)
            {
                #region Head Delete

                try
                {
                    sb.Length = 0;
                    sb.Append("UPDATE T_ESTIMATE_H " + Environment.NewLine);
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
                    CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateEstimate(Head Delete)", ex);
                    return "UpdateEstimate(Head Update) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
                }

                #endregion

                #region Detail Delete

                try
                {
                    sb.Length = 0;
                    sb.Append("UPDATE T_ESTIMATE_D " + Environment.NewLine);
                    sb.Append(CommonUtl.GetUpdSQLCommonColums(PG_NM,
                                                              entityH.update_person_id,
                                                              ipAdress,
                                                              userId,
                                                              1));
                    sb.Append(" WHERE COMPANY_ID = " + companyId + Environment.NewLine);
                    sb.Append("   AND GROUP_ID = " + groupId + Environment.NewLine);
                    sb.Append("   AND Estimate_ID = " + entityH.id.ToString() + Environment.NewLine);

                    db.ExecuteSQL(sb.ToString(), false);

                }
                catch (Exception ex)
                {
                    db.ExRollbackTransaction();
                    CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateEstimate(Detail Delete)", ex);
                    return "UpdateEstimate(Detail Update) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
                }

                #endregion
            }

            #endregion

            #region CommitTransaction

            try
            {
                db.ExCommitTransaction();
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateEstimate(CommitTransaction)", ex);
                return "UpdateEstimate(CommitTransaction) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
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
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateEstimate(DbClose)", ex);
                return "UpdateEstimate(DbClose) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
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
                                                   DataPgEvidence.PGName.Estimate.EstimateInp,
                                                   DataPgEvidence.geOperationType.Update,
                                                   "NO:" + no.ToString() + ",ID:" + id.ToString());
                        break;
                    case 1:
                        svcPgEvidence.gAddEvidence(ExCast.zCInt(HttpContext.Current.Session[ExSession.EVIDENCE_SAVE_FLG]),
                                                   companyId,
                                                   userId,
                                                   ipAdress,
                                                   sessionString,
                                                   DataPgEvidence.PGName.Estimate.EstimateInp,
                                                   DataPgEvidence.geOperationType.Insert,
                                                   "NO:" + no.ToString() + ",ID:" + id.ToString());
                        break;
                    case 2:
                        svcPgEvidence.gAddEvidence(ExCast.zCInt(HttpContext.Current.Session[ExSession.EVIDENCE_SAVE_FLG]),
                                                   companyId,
                                                   userId,
                                                   ipAdress,
                                                   sessionString,
                                                   DataPgEvidence.PGName.Estimate.EstimateInp,
                                                   DataPgEvidence.geOperationType.Delete,
                                                   "NO:" + no.ToString() + ",ID:" + id.ToString());
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateEstimate(Add Evidence)", ex);
                return "UpdateEstimate(Add Evidence) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
            }

            #endregion

            #region Return

            if (type == 1 && EstimateNo == 0)
            {
                return "Auto Insert success : " + "見積番号 : " + no.ToString().ToString() + "で登録しました。";
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
        public void UpdateEstimatePrint(string random, int type, string _no)
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
                    CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateEstimatePrint(認証処理) " + _message);
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateEstimatePrint(認証処理)", ex);
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
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateEstimatePrint(DbOpen)", ex);
            }

            #endregion

            #region BeginTransaction

            try
            {
                db.ExBeginTransaction();
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateEstimatePrint(BeginTransaction)", ex);
            }

            #endregion

            #region Update (発行区分更新)

            try
            {
                sb.Length = 0;
                sb.Append("UPDATE T_ESTIMATE_H " + Environment.NewLine);
                sb.Append("   SET ESTIMATE_PRINT_FLG = 1" + Environment.NewLine);
                sb.Append(" WHERE COMPANY_ID = " + companyId + Environment.NewLine);
                sb.Append("   AND GROUP_ID = " + groupId + Environment.NewLine);
                sb.Append("   AND DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND NO IN (" + _no.Replace("<<@escape_comma@>>", ",") + ")" + Environment.NewLine);

                db.ExecuteSQL(sb.ToString(), false);
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateEstimatePrint", ex);
            }

            #endregion

            #region CommitTransaction

            try
            {
                db.ExCommitTransaction();
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateEstimatePrint(CommitTransaction)", ex);
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
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateEstimatePrint(DbClose)", ex);
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
                                                   DataPgEvidence.PGName.Estimate.EstimatePrint,
                                                   DataPgEvidence.geOperationType.Update,
                                                   "NO:" + _no.Replace("<<@escape_comma@>>", ","));
                        break;
                    case 1:
                        svcPgEvidence.gAddEvidence(ExCast.zCInt(HttpContext.Current.Session[ExSession.EVIDENCE_SAVE_FLG]),
                                                   companyId,
                                                   userId,
                                                   ipAdress,
                                                   sessionString,
                                                   DataPgEvidence.PGName.Estimate.EstimatePrint,
                                                   DataPgEvidence.geOperationType.Insert,
                                                   "NO:" + _no.Replace("<<@escape_comma@>>", ","));
                        break;
                    case 2:
                        svcPgEvidence.gAddEvidence(ExCast.zCInt(HttpContext.Current.Session[ExSession.EVIDENCE_SAVE_FLG]),
                                                   companyId,
                                                   userId,
                                                   ipAdress,
                                                   sessionString,
                                                   DataPgEvidence.PGName.Estimate.EstimatePrint,
                                                   DataPgEvidence.geOperationType.Delete,
                                                   "NO:" + _no.Replace("<<@escape_comma@>>", ","));
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateEstimatePrint(Add Evidence)", ex);
            }

            #endregion

        }

        #endregion

    }
}
