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
    public class svcInvoiceClose
    {
        private const string CLASS_NM = "svcInvoiceClose";
        private readonly string PG_NM = DataPgEvidence.PGName.Invoice.InvoiceClose;

        #region データ取得

        /// <summary> 
        /// データ取得
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebMethod(EnableSession = true)]
        public List<EntityInvoiceClose> GetInvoiceTotal(string random, EntityInvoiceClosePrm entityPrm)
        {

            EntityInvoiceClose entity;
            List<EntityInvoiceClose> entityList = new List<EntityInvoiceClose>();

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
                    entity = new EntityInvoiceClose();
                    entity.MESSAGE = _message;
                    entityList.Add(entity);
                    return entityList;
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetInvoiceTotal(認証処理)", ex);
                entity = new EntityInvoiceClose();
                entity.MESSAGE = CLASS_NM + ".GetInvoiceTotal : 認証処理に失敗しました。" + Environment.NewLine + ex.Message.ToString(); ;
                entityList.Add(entity);
                return entityList;
            }

            #endregion

            StringBuilder sb;
            DataTable dt;
            DataTable dt2;
            ExMySQLData db;

            try
            {
                db = ExSession.GetSessionDb(ExCast.zCInt(HttpContext.Current.Session[ExSession.USER_ID]),
                                            ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]));

                sb = new StringBuilder();

                string yyyymmdd = entityPrm.invoice_yyyymmdd;

                #region SQL

                sb.Length = 0;
                sb.Append("SELECT CM.ID " + Environment.NewLine);
                sb.Append("      ,CM.TAX_FRACTION_PROC_ID " + Environment.NewLine);
                sb.Append("      ,CM.NAME " + Environment.NewLine);
                sb.Append("      ,CM.SUMMING_UP_GROUP_ID " + Environment.NewLine);
                sb.Append("      ,CM.COLLECT_DAY " + Environment.NewLine);
                sb.Append("      ,CM.COLLECT_CYCLE_ID " + Environment.NewLine);
                sb.Append("      ,SL.SUM_INVOICE_OUT_PRICE " + Environment.NewLine);
                sb.Append("      ,SL.SUM_INVOICE_IN_PRICE " + Environment.NewLine);
                sb.Append("      ,SL.SUM_PRICE " + Environment.NewLine);
                sb.Append("      ,SL.SUM_TAX " + Environment.NewLine);
                sb.Append("      ,SL.SUM_NO_TAX_PRICE " + Environment.NewLine);
                sb.Append("  FROM M_CUSTOMER AS CM" + Environment.NewLine);

                #region Join

                // 今回売上
                sb.Append("  INNER JOIN (SELECT T.INVOICE_ID" + Environment.NewLine);
                sb.Append("                    ,SUM(CASE WHEN T.TAX_CHANGE_ID = 3 THEN T.SUM_PRICE" + Environment.NewLine);
                sb.Append("                              ELSE 0 END) AS SUM_INVOICE_OUT_PRICE" + Environment.NewLine);
                sb.Append("                    ,SUM(CASE WHEN T.TAX_CHANGE_ID = 6 THEN T.SUM_PRICE" + Environment.NewLine);
                sb.Append("                              ELSE 0 END) AS SUM_INVOICE_IN_PRICE" + Environment.NewLine);
                sb.Append("                    ,SUM(T.SUM_PRICE) AS SUM_PRICE" + Environment.NewLine);
                sb.Append("                    ,SUM(T.SUM_TAX) AS SUM_TAX" + Environment.NewLine);
                sb.Append("                    ,SUM(T.SUM_NO_TAX_PRICE) AS SUM_NO_TAX_PRICE" + Environment.NewLine);
                sb.Append("                FROM T_SALES_H AS T " + Environment.NewLine);
                sb.Append("               WHERE T.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("                 AND T.COMPANY_ID = " + companyId + Environment.NewLine);
                sb.Append("                 AND T.GROUP_ID = " + groupId + Environment.NewLine);
                sb.Append("                 AND T.SALES_YMD <= " + ExEscape.zRepStr(yyyymmdd) + Environment.NewLine);
                sb.Append("                 AND T.BUSINESS_DIVISION_ID = 1 " + Environment.NewLine);        // 掛売上のみ

                // 前回請求分以外の売上
                sb.Append("                 AND NOT EXISTS (SELECT IV.NO" + Environment.NewLine);
                sb.Append("                                   FROM T_INVOICE AS IV" + Environment.NewLine);
                sb.Append("                                  WHERE IV.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("                                    AND IV.COMPANY_ID = T.COMPANY_ID " + Environment.NewLine);
                sb.Append("                                    AND IV.GROUP_ID = T.GROUP_ID " + Environment.NewLine);
                sb.Append("                                    AND IV.NO = T.INVOICE_NO " + Environment.NewLine);
                sb.Append("                                    AND IV.INVOICE_YYYYMMDD < " + ExEscape.zRepStr(yyyymmdd) + Environment.NewLine);
                sb.Append("                                    AND IV.INVOICE_KBN = 0) " + Environment.NewLine);

                sb.Append("               GROUP BY T.INVOICE_ID " + Environment.NewLine);
                sb.Append("             ) AS SL" + Environment.NewLine);
                sb.Append("     ON SL.INVOICE_ID = CM.ID" + Environment.NewLine);

                                            

                #endregion

                sb.Append(" WHERE CM.COMPANY_ID = " + companyId + Environment.NewLine);
                sb.Append("   AND CM.DELETE_FLG = 0 " + Environment.NewLine);
                if (!string.IsNullOrEmpty(entityPrm.summing_up_group_id))
                {
                    sb.Append("   AND CM.SUMMING_UP_GROUP_ID = " + ExEscape.zRepStr(entityPrm.summing_up_group_id) + Environment.NewLine);
                }
                if (!string.IsNullOrEmpty(entityPrm.invoice_id))
                {
                    sb.Append("   AND CM.ID = " + ExEscape.zRepStr(ExCast.zNumZeroNothingFormat(entityPrm.invoice_id)) + Environment.NewLine);
                }

                sb.Append(" ORDER BY CM.ID2 " + Environment.NewLine);
                sb.Append("         ,CM.ID " + Environment.NewLine);

                #endregion

                dt = db.GetDataTable(sb.ToString());

                // 排他制御
                DataPgLock.geLovkFlg lockFlg;
                string strErr = DataPgLock.SetLockPg(companyId, userId, PG_NM, groupId, ipAdress, db, out lockFlg);

                if (strErr != "")
                {
                    entity = new EntityInvoiceClose();
                    entity.MESSAGE = CLASS_NM + ".GetInvoiceTotal : 排他制御(ロック情報取得)に失敗しました。" + Environment.NewLine + strErr;
                    entityList.Add(entity);
                    return entityList;
                }

                if (dt.DefaultView.Count > 0)
                {
                    for (int i = 0; i <= dt.DefaultView.Count - 1; i++)
                    {
                        #region Set Entity

                        entity = new EntityInvoiceClose();
                        entity.no = "未";
                        entity.invoice_id = ExCast.zCStr(dt.DefaultView[i]["ID"]);
                        entity.invoice_nm = ExCast.zCStr(dt.DefaultView[i]["NAME"]);
                        entity.invoice_yyyymmdd = yyyymmdd;
                        entity.summing_up_group_id = ExCast.zCStr(dt.DefaultView[i]["SUMMING_UP_GROUP_ID"]);
                        entity.person_id = ExCast.zCInt(personId);

                        #region 再集計チェック

                        int _invoieReTotalFlg = 0;
                        double _before_invoice_price = 0;

                        #region SQL

                        sb.Length = 0;
                        sb.Append("SELECT T.NO " + Environment.NewLine);
                        sb.Append("      ,T.BEFORE_INVOICE_PRICE " + Environment.NewLine);
                        sb.Append("      ,date_format(T.COLLECT_PLAN_DAY , " + ExEscape.SQL_YMD + ") AS COLLECT_PLAN_DAY" + Environment.NewLine);
                        sb.Append("      ,T.COLLECT_DAY " + Environment.NewLine);
                        sb.Append("  FROM T_INVOICE AS T" + Environment.NewLine);
                        sb.Append(" WHERE T.COMPANY_ID = " + companyId + Environment.NewLine);
                        sb.Append("   AND T.GROUP_ID = " + groupId + Environment.NewLine);
                        sb.Append("   AND T.DELETE_FLG = 0 " + Environment.NewLine);
                        sb.Append("   AND T.INVOICE_KBN = 0 " + Environment.NewLine);       // 締請求分のみ
                        sb.Append("   AND T.INVOICE_ID = " + ExEscape.zRepStr(ExCast.zNumZeroNothingFormat(entity.invoice_id)) + Environment.NewLine);
                        sb.Append("   AND T.INVOICE_YYYYMMDD = " + ExEscape.zRepStr(yyyymmdd) + Environment.NewLine);
                        sb.Append(" LIMIT 0, 1");

                        #endregion

                        dt2 = db.GetDataTable(sb.ToString());
                        if (dt2.DefaultView.Count > 0)
                        {
                            _invoieReTotalFlg = 1;
                            entity.no = ExCast.zCStr(dt2.DefaultView[0]["NO"]);
                            entity.collect_plan_day = ExCast.zDateNullToDefault(dt2.DefaultView[0]["COLLECT_PLAN_DAY"]);
                            entity.collect_day = ExCast.zCInt(dt2.DefaultView[0]["COLLECT_DAY"]);
                            _before_invoice_price = ExCast.zCDbl(dt2.DefaultView[0]["BEFORE_INVOICE_PRICE"]);
                        }

                        #endregion

                        #region 前回請求取得

                        int _invoieExistsFlg = 0;
                        int _before_invoice_yyyyymm = 0;
                        string _before_invoice_yyyyymmdd = "";

                        #region SQL

                        sb.Length = 0;
                        sb.Append("SELECT T.NO " + Environment.NewLine);
                        sb.Append("      ,date_format(T.INVOICE_YYYYMMDD , " + ExEscape.SQL_YMD + ") AS INVOICE_YYYYMMDD" + Environment.NewLine);
                        sb.Append("      ,T.INVOICE_PRICE " + Environment.NewLine);
                        sb.Append("  FROM T_INVOICE AS T " + Environment.NewLine);

                        sb.Append(" WHERE T.DELETE_FLG = 0 " + Environment.NewLine);
                        sb.Append("   AND T.COMPANY_ID = " + companyId + Environment.NewLine);
                        sb.Append("   AND T.GROUP_ID = " + groupId + Environment.NewLine);
                        sb.Append("   AND T.INVOICE_ID = " +  ExEscape.zRepStr(ExCast.zNumZeroNothingFormat(dt.DefaultView[i]["ID"])) + Environment.NewLine);
                        sb.Append("   AND T.INVOICE_KBN = 0 " + Environment.NewLine);               // 締請求分のみ
                        sb.Append("   AND T.INVOICE_YYYYMMDD < " + ExEscape.zRepStr(yyyymmdd) + Environment.NewLine);
                        sb.Append(" LIMIT 0, 1");

                        #endregion

                        dt2 = db.GetDataTable(sb.ToString());
                        if (dt2.DefaultView.Count > 0)
                        {
                            if (_invoieReTotalFlg == 0)
                            {
                                //entity.no = ExCast.zCStr(dt2.DefaultView[0]["NO"]);
                                _before_invoice_price = ExCast.zCDbl(dt2.DefaultView[0]["INVOICE_PRICE"]);
                            }
                            _invoieExistsFlg = 1;

                            _before_invoice_yyyyymmdd = ExCast.zDateNullToDefault(dt2.DefaultView[0]["INVOICE_YYYYMMDD"]);
                            if (_before_invoice_yyyyymmdd != "")
                            {
                                entity.before_invoice_yyyymmdd = _before_invoice_yyyyymmdd;
                            }
                            else
                            {
                                entity.before_invoice_yyyymmdd = "請求無し";
                            }
                        }
                        else
                        {
                            entity.before_invoice_yyyymmdd = "請求無し";
                        }


                        #region 回収予定日

                        string _collect_plan_day = "";
                        int _collect_day = 0;
                        DataPlanDay.GetPlanDay(entity.collect_cycle_id,
                                                             ExCast.zCInt(dt.DefaultView[i]["COLLECT_DAY"]),
                                                             entityPrm.invoice_yyyymmdd,
                                                             ref _collect_plan_day,
                                                             ref _collect_day);

                        if (_invoieReTotalFlg == 0)
                        {
                            entity.collect_plan_day = _collect_plan_day;
                            entity.collect_day = _collect_day;

                            if (!string.IsNullOrEmpty(entityPrm.collect_plan_yyyymmdd))
                            {
                                entity.collect_plan_day = entityPrm.collect_plan_yyyymmdd;
                            }
                        }

                        #endregion

                        #endregion

                        #region 入金取得

                        double _receipt_price = 0;

                        #region SQL

                        sb.Length = 0;
                        sb.Append("SELECT SUM(T.SUM_PRICE) AS SUM_PRICE " + Environment.NewLine);
                        sb.Append("  FROM T_RECEIPT_H AS T" + Environment.NewLine);
                        sb.Append(" WHERE T.COMPANY_ID = " + companyId + Environment.NewLine);
                        sb.Append("   AND T.GROUP_ID = " + groupId + Environment.NewLine);
                        sb.Append("   AND T.DELETE_FLG = 0 " + Environment.NewLine);
                        sb.Append("   AND T.INVOICE_ID = " + ExEscape.zRepStr(ExCast.zNumZeroNothingFormat(entity.invoice_id)) + Environment.NewLine);
                        sb.Append("   AND T.RECEIPT_YMD <= " + ExEscape.zRepStr(yyyymmdd) + Environment.NewLine);
                        sb.Append("   AND IFNULL(T.INVOICE_NO, 0) > 0 " + Environment.NewLine);
                        // 締請求分のみ
                        sb.Append("   AND EXISTS (SELECT IV.NO" + Environment.NewLine);
                        sb.Append("                 FROM T_INVOICE AS IV" + Environment.NewLine);
                        sb.Append("                WHERE IV.DELETE_FLG = 0 " + Environment.NewLine);
                        sb.Append("                  AND IV.COMPANY_ID = T.COMPANY_ID " + Environment.NewLine);
                        sb.Append("                  AND IV.GROUP_ID = T.GROUP_ID " + Environment.NewLine);
                        sb.Append("                  AND IV.NO = T.INVOICE_NO " + Environment.NewLine);
                        sb.Append("                  AND IV.INVOICE_KBN = 0) " + Environment.NewLine);

                        #endregion

                        dt2 = db.GetDataTable(sb.ToString());
                        if (dt2.DefaultView.Count > 0)
                        {
                            _receipt_price = ExCast.zCDbl(dt2.DefaultView[0]["SUM_PRICE"]);
                        }

                        #endregion

                        entity.before_invoice_yyyymmdd = _before_invoice_yyyyymmdd;
                        entity.before_invoice_price = _before_invoice_price;
                        entity.receipt_price = _receipt_price;
                        entity.transfer_price = _before_invoice_price - _receipt_price;
                        entity.sales_price = ExCast.zCDbl(dt.DefaultView[i]["SUM_PRICE"]);                  // 外税額含めない金額
                        entity.no_tax_sales_price = ExCast.zCDbl(dt.DefaultView[i]["SUM_NO_TAX_PRICE"]);
                        entity.tax = ExCast.zCDbl(dt.DefaultView[i]["SUM_TAX"]);                            // 内税・外税計

                        #region 請求時消費税

                        double _invoice_out_tax = 0;
                        double _invoice_in_tax = 0;
                        int _tax_fraction_proc_id = ExCast.zCInt(dt.DefaultView[i]["TAX_FRACTION_PROC_ID"]);
                        double _sum_invoice_out_price = ExCast.zCDbl(dt.DefaultView[i]["SUM_INVOICE_OUT_PRICE"]);       // 外税/請求時 金額
                        double _sum_invoice_in_price = ExCast.zCDbl(dt.DefaultView[i]["SUM_INVOICE_IN_PRICE"]);         // 内税/請求時 金額

                        if (_sum_invoice_out_price != 0 || _sum_invoice_in_price != 0)
                        {
                            // 外税/請求時 消費税
                            if (_sum_invoice_out_price != 0)
                            {
                                _invoice_out_tax = ExMath.zCalcTax(_sum_invoice_out_price, 3, 0, _tax_fraction_proc_id);
                            }
                            // 内税/請求時 消費税
                            if (_sum_invoice_in_price != 0)
                            {
                                _invoice_in_tax = ExMath.zCalcTax(_sum_invoice_out_price, 6, 0, _tax_fraction_proc_id);
                            }

                            // 税額の加算
                            entity.tax += _invoice_out_tax;
                            entity.tax += _invoice_in_tax;

                            // 税抜額の減算
                            entity.no_tax_sales_price -= _invoice_in_tax;
                        }

                        #endregion

                        // 今回請求額＝繰越金額＋税抜金額＋消費税額
                        entity.invoice_price = entity.transfer_price + entity.no_tax_sales_price + entity.tax;

                        // 外税額＝税抜金額＋消費税額－金額(外税を含めない金額)
                        entity.out_tax = entity.no_tax_sales_price + entity.tax - entity.sales_price;

                        //entity.memo = ExCast.zCStr(dt.DefaultView[i]["MEMO"]);

                        entity.invoice_exists_flg = _invoieExistsFlg;
                        entity.exec_flg = false;

                        entity.lock_flg = (int)lockFlg;

                        entityList.Add(entity);

                        #endregion
                    }
                }

            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetInvoiceTotal", ex);
                entity = new EntityInvoiceClose();
                entity.MESSAGE = CLASS_NM + ".GetInvoiceTotal : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message.ToString();
                entityList.Clear();
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
                           PG_NM,
                           DataPgEvidence.geOperationType.Select,
                           "");

            return entityList;

        }

        /// <summary>　
        /// リスト取得
        /// </summary>
        /// <param name="strWhereSql"></param>
        /// <param name="strOrderBySql"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebMethod(EnableSession = true)]
        public List<EntityInvoiceClose> GetInvoiceList(string random, string strWhereSql, string strOrderBySql)
        {
            List<EntityInvoiceClose> entityList = new List<EntityInvoiceClose>();

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
                    EntityInvoiceClose entity = new EntityInvoiceClose();
                    entity.MESSAGE = _message;
                    entityList.Add(entity);
                    return entityList;
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetInvoiceList(認証処理)", ex);
                EntityInvoiceClose entity = new EntityInvoiceClose();
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
                sb.Append(rptMgr.GetInvoiceListReportSQL(companyId, groupId, strWhereSql, strOrderBySql));

                dt = db.GetDataTable(sb.ToString());

                if (dt.DefaultView.Count > 0)
                {
                    for (int i = 0; i <= dt.DefaultView.Count - 1; i++)
                    {
                        #region Set Entity

                        EntityInvoiceClose entity = new EntityInvoiceClose();
                        entity.no = ExCast.zCStr(dt.DefaultView[i]["NO"]);
                        entity.invoice_id = ExCast.zCStr(dt.DefaultView[i]["INVOICE_ID"]);
                        entity.invoice_nm = ExCast.zCStr(dt.DefaultView[i]["INVOICE_NM"]);
                        entity.invoice_yyyymmdd = ExCast.zDateNullToDefault(dt.DefaultView[i]["INVOICE_YYYYMMDD"]);
                        entity.summing_up_group_id = ExCast.zCStr(dt.DefaultView[i]["SUMMING_UP_GROUP_ID"]);
                        entity.summing_up_group_nm = ExCast.zCStr(dt.DefaultView[i]["SUMMING_UP_GROUP_NM"]);
                        entity.person_id = ExCast.zCInt(dt.DefaultView[i]["INPUT_PERSON"]);
                        entity.person_nm = ExCast.zCStr(dt.DefaultView[i]["INPUT_PERSON_NM"]);

                        entity.collect_plan_day = ExCast.zDateNullToDefault(dt.DefaultView[i]["COLLECT_PLAN_DAY"]);
                        entity.collect_day = ExCast.zCInt(dt.DefaultView[i]["COLLECT_DAY"]);

                        entity.before_invoice_yyyymmdd = ExCast.zDateNullToDefault(dt.DefaultView[i]["BEFORE_INVOICE_YYYYMMDD"]);

                        entity.before_invoice_price = ExCast.zCDbl(dt.DefaultView[i]["BEFORE_INVOICE_PRICE"]);
                        entity.before_invoice_price_upd = ExCast.zCDbl(dt.DefaultView[i]["BEFORE_INVOICE_PRICE"]);
                        entity.receipt_price = ExCast.zCDbl(dt.DefaultView[i]["RECEIPT_PRICE"]);
                        entity.transfer_price = ExCast.zCDbl(dt.DefaultView[i]["TRANSFER_PRICE"]);
                        entity.sales_price = ExCast.zCDbl(dt.DefaultView[i]["SALES_PRICE"]);
                        entity.no_tax_sales_price = ExCast.zCDbl(dt.DefaultView[i]["NO_TAX_SALES_PRICE"]);
                        entity.tax = ExCast.zCDbl(dt.DefaultView[i]["TAX"]);

                        entity.invoice_price = ExCast.zCDbl(dt.DefaultView[i]["INVOICE_PRICE"]);

                        // 外税額＝税抜金額＋消費税額－金額(外税を含めない金額)
                        entity.out_tax = ExCast.zCDbl(dt.DefaultView[i]["OUT_TAX"]);

                        entity.invoice_kbn = ExCast.zCInt(dt.DefaultView[i]["INVOICE_KBN"]);
                        entity.invoice_kbn_nm = ExCast.zCStr(dt.DefaultView[i]["INVOICE_KBN_NM"]);

                        entity.invoice_print_flg = ExCast.zCInt(dt.DefaultView[i]["INVOICE_PRINT_FLG"]);
                        entity.invoice_print_flg_nm = ExCast.zCStr(dt.DefaultView[i]["INVOICE_PRINT_FLG_NM"]);

                        entity.this_receipt_price = ExCast.zCDbl(dt.DefaultView[i]["THIS_RECEIPT_PRICE"]);

                        entity.receipt_receivable_kbn = ExCast.zCInt(dt.DefaultView[i]["RECEIPT_RECEIVABLE_KBN"]);
                        entity.receipt_receivable_kbn_nm = ExCast.zCStr(dt.DefaultView[i]["RECEIPT_RECEIVABLE_KBN_NM"]);

                        //if (entity.this_receipt_price == 0)
                        //{
                        //    if (entity.invoice_price == 0)
                        //    {
                        //        entity.receipt_receivable_kbn = 3;
                        //        entity.receipt_receivable_kbn_nm = "消込済";
                        //    }
                        //    else
                        //    {
                        //        entity.receipt_receivable_kbn = 1;
                        //        entity.receipt_receivable_kbn_nm = "消込未";
                        //    }
                        //}
                        //else if (entity.this_receipt_price < entity.invoice_price)
                        //{
                        //    entity.receipt_receivable_kbn = 2;
                        //    entity.receipt_receivable_kbn_nm = "一部消込";
                        //}
                        //else if (entity.this_receipt_price == entity.invoice_price)
                        //{
                        //    entity.receipt_receivable_kbn = 3;
                        //    entity.receipt_receivable_kbn_nm = "消込済";
                        //}
                        //else if (entity.this_receipt_price > entity.invoice_price)
                        //{
                        //    entity.receipt_receivable_kbn = 4;
                        //    entity.receipt_receivable_kbn_nm = "超過消込";
                        //}
                        //else
                        //{
                        //    entity.receipt_receivable_kbn = 1;
                        //    entity.receipt_receivable_kbn_nm = "消込未";
                        //}

                        entity.invoice_zan_price = ExCast.zCDbl(dt.DefaultView[i]["INVOICE_ZAN_PRICE"]);
                        //entity.invoice_zan_price = entity.invoice_price - entity.this_receipt_price;

                        entity.memo = ExCast.zCStr(dt.DefaultView[i]["MEMO"]);

                        entity.invoice_exists_flg = 0;
                        entity.exec_flg = false;

                        entity.lock_flg = 0;

                        entityList.Add(entity);

                        #endregion

                    }
                }

            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetInvoiceList", ex);
                entityList.Clear();
                EntityInvoiceClose entity = new EntityInvoiceClose();
                entity.MESSAGE = CLASS_NM + ".GetInvoiceList : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message.ToString();
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
                                       DataPgEvidence.PGName.Order.OrderList,
                                       DataPgEvidence.geOperationType.Select,
                                       "Where:" + strWhereSql + ",Orderby:" + strOrderBySql);

            return entityList;

        }

        /// <summary> 
        /// 請求入金データ取得
        /// </summary>
        /// <returns></returns>
        [OperationContract()]
        [WebMethod(EnableSession = true)]
        public EntityInvoiceReceipt GetPaymentCashIn(string random, long no, long receiptNo)
        {

            EntityInvoiceReceipt entity = null;

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
                    entity = new EntityInvoiceReceipt();
                    entity.MESSAGE = _message;
                    return entity;
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetPaymentCashIn(認証処理)", ex);
                entity = new EntityInvoiceReceipt();
                entity.MESSAGE = CLASS_NM + ".GetPaymentCashIn : 認証処理に失敗しました。" + Environment.NewLine + ex.Message.ToString();
                return entity;
            }

            #endregion

            StringBuilder sb;
            DataTable dt;
            DataTable dt2;
            ExMySQLData db;

            try
            {
                db = ExSession.GetSessionDb(ExCast.zCInt(HttpContext.Current.Session[ExSession.USER_ID]),
                                            ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]));

                sb = new StringBuilder();

                #region SQL

                sb.Append("SELECT T.NO " + Environment.NewLine);
                sb.Append("      ,T.INVOICE_ID " + Environment.NewLine);
                sb.Append("      ,CS.NAME AS INVOICE_NAME " + Environment.NewLine);
                sb.Append("      ,T.INVOICE_KBN " + Environment.NewLine);
                sb.Append("      ,CASE WHEN IFNULL(T.INVOICE_KBN, 0) = 0 THEN '締処理' " + Environment.NewLine);
                sb.Append("            ELSE '都度請求' END AS INVOICE_KBN_NM " + Environment.NewLine);
                sb.Append("      ,T.SUMMING_UP_GROUP_ID " + Environment.NewLine);
                sb.Append("      ,CN.NAME AS SUMMING_UP_GROUP_NM " + Environment.NewLine);
                sb.Append("      ,date_format(T.INVOICE_YYYYMMDD , " + ExEscape.SQL_YMD + ") AS INVOICE_YYYYMMDD" + Environment.NewLine);
                sb.Append("      ,date_format(T.COLLECT_PLAN_DAY , " + ExEscape.SQL_YMD + ") AS COLLECT_PLAN_DAY" + Environment.NewLine);
                sb.Append("      ,T.INVOICE_PRICE " + Environment.NewLine);
                sb.Append("      ,RP_SUM.SUM_PRICE AS BEFORE_RECEIPT_PRICE " + Environment.NewLine);
                sb.Append("      ,CS.RECEIPT_DIVISION_ID " + Environment.NewLine);
                sb.Append("      ,CS.SALES_CREDIT_PRICE " + Environment.NewLine);
                sb.Append("      ,RD.DESCRIPTION AS RECEIPT_DIVISION_NAME " + Environment.NewLine);

                sb.Append("  FROM T_INVOICE AS T" + Environment.NewLine);

                #region Join

                // 請求先
                sb.Append("  LEFT JOIN M_CUSTOMER AS CS" + Environment.NewLine);
                sb.Append("    ON CS.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND CS.DISPLAY_FLG = 1 " + Environment.NewLine);
                sb.Append("   AND CS.COMPANY_ID = " + companyId + Environment.NewLine);
                sb.Append("   AND CS.ID = T.INVOICE_ID" + Environment.NewLine);

                // 入金済額
                sb.Append("  LEFT JOIN (SELECT RP.INVOICE_NO" + Environment.NewLine);
                sb.Append("                   ,SUM(RP.SUM_PRICE) AS SUM_PRICE" + Environment.NewLine);
                sb.Append("               FROM T_RECEIPT_H AS RP " + Environment.NewLine);
                sb.Append("              WHERE RP.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("                AND RP.COMPANY_ID = " + companyId + Environment.NewLine);
                sb.Append("                AND RP.GROUP_ID = " + groupId + Environment.NewLine);
                if (receiptNo != 0)
                {
                    sb.Append("                AND RP.NO <> " + receiptNo + Environment.NewLine);
                }
                sb.Append("              GROUP BY INVOICE_NO " + Environment.NewLine);
                sb.Append("            ) AS RP_SUM " + Environment.NewLine);
                sb.Append("    ON RP_SUM.INVOICE_NO = T.NO " + Environment.NewLine);

                // 締グループ
                sb.Append("  LEFT JOIN M_CONDITION AS CN" + Environment.NewLine);
                sb.Append("    ON CN.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND CN.DISPLAY_FLG = 1 " + Environment.NewLine);
                sb.Append("   AND CN.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
                sb.Append("   AND CN.CONDITION_DIVISION_ID = 1" + Environment.NewLine);
                sb.Append("   AND CN.ID = T.SUMMING_UP_GROUP_ID" + Environment.NewLine);

                // 回収(入金)区分
                sb.Append("  LEFT JOIN SYS_M_RECEIPT_DIVISION AS RD" + Environment.NewLine);
                sb.Append("    ON RD.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND RD.DISPLAY_FLG = 1 " + Environment.NewLine);
                sb.Append("   AND RD.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
                sb.Append("   AND RD.ID = CS.RECEIPT_DIVISION_ID" + Environment.NewLine);

                #endregion

                sb.Append(" WHERE T.COMPANY_ID = " + companyId + Environment.NewLine);
                sb.Append("   AND T.GROUP_ID = " + groupId + Environment.NewLine);
                sb.Append("   AND T.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND T.NO = " + no.ToString() + Environment.NewLine);
                sb.Append(" LIMIT 0, 1");

                #endregion

                dt = db.GetDataTable(sb.ToString());

                if (dt.DefaultView.Count > 0)
                {
                    entity = new EntityInvoiceReceipt();

                    #region 請求繰越済チェック

                    string _invoice_yyyymmdd = ExCast.zDateNullToDefault(dt.DefaultView[0]["INVOICE_YYYYMMDD"]);
                    if (!string.IsNullOrEmpty(_invoice_yyyymmdd) && ExCast.zCInt(dt.DefaultView[0]["INVOICE_KBN"]) == 0)
                    {
                        #region SQL

                        sb.Length = 0;
                        sb.Append("SELECT T.NO " + Environment.NewLine);
                        sb.Append("      ,date_format(T.INVOICE_YYYYMMDD , " + ExEscape.SQL_YMD + ") AS INVOICE_YYYYMMDD" + Environment.NewLine);
                        sb.Append("  FROM T_INVOICE AS T" + Environment.NewLine);
                        sb.Append(" WHERE T.COMPANY_ID = " + companyId + Environment.NewLine);
                        sb.Append("   AND T.GROUP_ID = " + groupId + Environment.NewLine);
                        sb.Append("   AND T.DELETE_FLG = 0 " + Environment.NewLine);
                        sb.Append("   AND T.INVOICE_KBN = 0 " + Environment.NewLine);   // 締請求のみ
                        sb.Append("   AND T.INVOICE_ID = " + ExEscape.zRepStr(ExCast.zCStr(dt.DefaultView[0]["INVOICE_ID"])) + Environment.NewLine);
                        sb.Append("   AND T.INVOICE_YYYYMMDD > " + ExEscape.zRepStr(_invoice_yyyymmdd) + Environment.NewLine);
                        sb.Append(" ORDER BY T.INVOICE_YYYYMMDD DESC");
                        sb.Append(" LIMIT 0, 1");

                        #endregion

                        dt2 = db.GetDataTable(sb.ToString());

                        if (dt2.DefaultView.Count > 0)
                        {
                            entity.MESSAGE = "[請求書番号 " + no.ToString() + "] [請求先 " + ExCast.zCStr(dt.DefaultView[0]["INVOICE_ID"]) + "：" + ExCast.zCStr(dt.DefaultView[0]["INVOICE_NAME"]) + 
                                             "] は繰越済の為、選択できません。" + Environment.NewLine +
                                             "([繰越請求番号 " + ExCast.zCLng(dt2.DefaultView[0]["NO"]).ToString() + "] [繰越請求締日" + ExCast.zDateNullToDefault(dt.DefaultView[0]["INVOICE_YYYYMMDD"]) + "])";
                            return entity;
                        }

                    }

                    #endregion

                    // 排他制御
                    DataPgLock.geLovkFlg lockFlg = DataPgLock.geLovkFlg.UnLock;
                    string strErr = DataPgLock.SetLockPg(companyId, userId, DataPgEvidence.PGName.Receipt.ReceiptInp, "invoice-receipt-" + groupId + "-" + no, ipAdress, db, out lockFlg);
                    if (strErr != "")
                    {
                        entity.MESSAGE = CLASS_NM + ".GetPaymentCashIn : 排他制御(ロック情報取得)に失敗しました。" + Environment.NewLine + strErr;
                    }

                    #region Set Entity

                    entity.no = ExCast.zCLng(dt.DefaultView[0]["NO"]);
                    entity.invoice_id = ExCast.zFormatForID(dt.DefaultView[0]["INVOICE_ID"], idFigureCustomer);
                    entity.invoice_nm = ExCast.zCStr(dt.DefaultView[0]["INVOICE_NAME"]);
                    entity.invoice_kbn = ExCast.zCInt(dt.DefaultView[0]["INVOICE_KBN"]);
                    entity.invoice_kbn_nm = ExCast.zCStr(dt.DefaultView[0]["INVOICE_KBN_NM"]);
                    entity.summing_up_group_id = ExCast.zCStr(dt.DefaultView[0]["SUMMING_UP_GROUP_ID"]);
                    entity.summing_up_group_nm = ExCast.zCStr(dt.DefaultView[0]["SUMMING_UP_GROUP_NM"]);
                    entity.invoice_yyyymmdd = ExCast.zDateNullToDefault(dt.DefaultView[0]["INVOICE_YYYYMMDD"]);
                    entity.collect_plan_day = ExCast.zDateNullToDefault(dt.DefaultView[0]["COLLECT_PLAN_DAY"]);
                    entity.invoice_price = ExCast.zCDbl(dt.DefaultView[0]["INVOICE_PRICE"]);
                    entity.before_receipt_price = ExCast.zCDbl(dt.DefaultView[0]["BEFORE_RECEIPT_PRICE"]);
                    entity.credit_price = ExCast.zCDbl(dt.DefaultView[0]["SALES_CREDIT_PRICE"]);
                    entity.receipt_division_id = ExCast.zCStr(dt.DefaultView[0]["RECEIPT_DIVISION_ID"]);
                    entity.receipt_division_nm = ExCast.zCStr(dt.DefaultView[0]["RECEIPT_DIVISION_NAME"]);

                    entity.lock_flg = (int)lockFlg;

                    #endregion

                }
                else
                {
                    entity = null;
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetPaymentCashIn", ex);
                entity = new EntityInvoiceReceipt();
                entity.MESSAGE = CLASS_NM + ".GetPaymentCashIn : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message.ToString();
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
                                       DataPgEvidence.PGName.Receipt.ReceiptInp,
                                       DataPgEvidence.geOperationType.Select,
                                       "INVOICE NO:" + no);

            return entity;


        }

        #endregion

        #region データ更新

        /// <summary>
        /// データ更新
        /// </summary>
        /// <param name="random">セッションランダム文字列</param>
        /// <param name="entity">更新データ</param>
        /// <returns></returns>
        [OperationContract()]
        [WebMethod(EnableSession = true)]
        public string UpdateInvoice(string random, int type, List<EntityInvoiceClose> entity)
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
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateInvoice(認証処理)", ex);
                return CLASS_NM + ".UpdateInvoice : 認証処理に失敗しました。" + Environment.NewLine + ex.Message.ToString();
            }

            #endregion

            #region Field

            StringBuilder sb = new StringBuilder();
            DataTable dt;
            ExMySQLData db = null;
            string _Id = "";
            int _classKbn = 0;

            #endregion

            #region Databese Open

            try
            {
                db = new ExMySQLData(ExCast.zCStr(HttpContext.Current.Session[ExSession.DB_CONNECTION_STR]));
                db.DbOpen();
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateInvoice(DbOpen)", ex);
                return CLASS_NM + ".UpdateInvoice(DbOpen) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
            }

            #endregion

            #region BeginTransaction

            try
            {
                db.ExBeginTransaction();
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateInvoice(BeginTransaction)", ex);
                return CLASS_NM + ".UpdateInvoice(BeginTransaction) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
            }

            #endregion

            #region Delete

            if (type == 2)
            {
                try
                {
                    for (int i = 0; i <= entity.Count - 1; i++)
                    {
                        if (entity[i].exec_flg == true && entity[i].no != "未")
                        {
                            #region 請求データ存在確認

                            #region SQL

                            sb.Length = 0;
                            sb.Append("SELECT T.NO " + Environment.NewLine);
                            sb.Append("  FROM T_INVOICE AS T" + Environment.NewLine);
                            sb.Append(" WHERE T.COMPANY_ID = " + companyId + Environment.NewLine);
                            sb.Append("   AND T.GROUP_ID = " + groupId + Environment.NewLine);
                            sb.Append("   AND T.DELETE_FLG = 0 " + Environment.NewLine);
                            sb.Append("   AND T.INVOICE_KBN = 0 " + Environment.NewLine);       // 締請求分のみ
                            sb.Append("   AND T.INVOICE_YYYYMMDD > " + ExEscape.zRepStr(entity[i].invoice_yyyymmdd) + Environment.NewLine);
                            sb.Append("   AND T.INVOICE_ID = " + ExEscape.zRepStr(ExCast.zNumZeroNothingFormat(entity[i].invoice_id)) + Environment.NewLine);
                            sb.Append(" LIMIT 0, 1");

                            #endregion

                            dt = db.GetDataTable(sb.ToString());

                            if (dt.DefaultView.Count > 0)
                            {
                                db.ExRollbackTransaction();
                                return "請求先 " + entity[i].invoice_id + "：" + entity[i].invoice_nm + Environment.NewLine +
                                       "請求番号 : " + entity[i].no + "は" + Environment.NewLine  + 
                                       "請求締日 : " + ExCast.zFormatToDate_YYYYMM(entity[0].invoice_yyyymmdd) + " 以降のデータが存在する為、削除できません。";
                            }

                            #endregion

                            #region 入金データ存在確認

                            //#region SQL

                            //sb.Length = 0;
                            //sb.Append("SELECT T.ID " + Environment.NewLine);
                            //sb.Append("  FROM T_RECEIPT_H AS T" + Environment.NewLine);
                            //sb.Append(" WHERE T.COMPANY_ID = " + companyId + Environment.NewLine);
                            //sb.Append("   AND T.GROUP_ID = " + groupId + Environment.NewLine);
                            //sb.Append("   AND T.DELETE_FLG = 0 " + Environment.NewLine);
                            //sb.Append("   AND T.INVOICE_ID = " + ExEscape.zRepStr(ExCast.zNumZeroNothingFormat(entity[i].invoice_id)) + Environment.NewLine);
                            //sb.Append("   AND T.RECEIPT_YMD <= " + ExEscape.zRepStr(entity[i].invoice_yyyymmdd) + Environment.NewLine);

                            //#endregion

                            //dt = db.GetDataTable(sb.ToString());
                            //if (dt.DefaultView.Count > 0)
                            //{
                            //    db.ExRollbackTransaction();
                            //    return "請求先 " + entity[i].invoice_id + "：" + entity[i].invoice_nm +
                            //           " 請求締日 : " + entity[0].invoice_yyyymmdd + " 以前の入金データが存在する為、削除できません。";
                            //}

                            #endregion

                            #region 入金消込データ存在確認

                            #region SQL

                            sb.Length = 0;
                            sb.Append("SELECT T.ID " + Environment.NewLine);
                            sb.Append("  FROM T_RECEIPT_H AS T" + Environment.NewLine);
                            sb.Append(" WHERE T.COMPANY_ID = " + companyId + Environment.NewLine);
                            sb.Append("   AND T.GROUP_ID = " + groupId + Environment.NewLine);
                            sb.Append("   AND T.DELETE_FLG = 0 " + Environment.NewLine);
                            sb.Append("   AND T.INVOICE_NO = " + ExEscape.zRepStr(ExCast.zNumZeroNothingFormat(entity[i].no)) + Environment.NewLine);

                            #endregion

                            dt = db.GetDataTable(sb.ToString());
                            if (dt.DefaultView.Count > 0)
                            {
                                db.ExRollbackTransaction();
                                return "請求先 " + entity[i].invoice_id + "：" + entity[i].invoice_nm + Environment.NewLine +
                                       "請求番号 : " + entity[i].no + "は入金消込済の為、削除できません。";
                            }

                            #endregion

                            #region Delete SQL

                            sb.Length = 0;
                            sb.Append("DELETE FROM T_INVOICE " + Environment.NewLine);
                            sb.Append(" WHERE COMPANY_ID = " + companyId + Environment.NewLine);
                            sb.Append("   AND GROUP_ID = " + groupId + Environment.NewLine);
                            sb.Append("   AND INVOICE_KBN = 0 " + Environment.NewLine);       // 締請求分のみ
                            sb.Append("   AND NO = " + ExCast.zCLng(entity[i].no) + Environment.NewLine);

                            #endregion

                            db.ExecuteSQL(sb.ToString(), false);

                            #region Update Sales

                            sb.Length = 0;
                            sb.Append("UPDATE T_SALES_H " + Environment.NewLine);
                            sb.Append("   SET INVOICE_NO = 0" + Environment.NewLine);
                            sb.Append(" WHERE DELETE_FLG = 0 " + Environment.NewLine);
                            sb.Append("   AND COMPANY_ID = " + companyId + Environment.NewLine);
                            sb.Append("   AND GROUP_ID = " + groupId + Environment.NewLine);
                            sb.Append("   AND BUSINESS_DIVISION_ID = 1 " + Environment.NewLine);        // 掛売上のみ
                            sb.Append("   AND SALES_YMD <= " + ExEscape.zRepStr(entity[i].invoice_yyyymmdd) + Environment.NewLine);
                            sb.Append("   AND INVOICE_ID = " + ExEscape.zRepStr(entity[i].invoice_id) + Environment.NewLine);

                            #endregion

                            db.ExecuteSQL(sb.ToString(), false);


                        }
                    }
                }
                catch (Exception ex)
                {
                    db.ExRollbackTransaction();
                    CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateInvoice(Delete)", ex);
                    return CLASS_NM + ".UpdateInvoice(Delete) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
                }
            }

            #endregion

            #region Insert

            if (type == 1)
            {
                #region Get Max Invoice No

                long no = 0;
                try
                {
                    no = DataSlipNo.GetMaxSlipNo(companyId, groupId, db, "T_INVOICE", "NO");
                }
                catch (Exception ex)
                {
                    CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateInvoice(Get Max No)", ex);
                    return CLASS_NM + ".UpdateInvoice(BeginTransaction) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
                }

                #endregion

                try
                {
                    for (int i = 0; i <= entity.Count - 1; i++)
                    {
                        if (entity[i].exec_flg == true)
                        {
                            #region 請求データ存在確認

                            #region SQL

                            sb.Length = 0;
                            sb.Append("SELECT T.NO " + Environment.NewLine);
                            sb.Append("  FROM T_INVOICE AS T" + Environment.NewLine);
                            sb.Append(" WHERE T.COMPANY_ID = " + companyId + Environment.NewLine);
                            sb.Append("   AND T.GROUP_ID = " + groupId + Environment.NewLine);
                            sb.Append("   AND T.DELETE_FLG = 0 " + Environment.NewLine);
                            sb.Append("   AND T.INVOICE_KBN = 0 " + Environment.NewLine);       // 締請求分のみ
                            sb.Append("   AND T.INVOICE_YYYYMMDD > " + ExEscape.zRepStr(entity[i].invoice_yyyymmdd) + Environment.NewLine);
                            sb.Append("   AND T.INVOICE_ID = " + ExEscape.zRepStr(ExCast.zNumZeroNothingFormat(entity[i].invoice_id)) + Environment.NewLine);
                            sb.Append(" LIMIT 0, 1");

                            #endregion

                            dt = db.GetDataTable(sb.ToString());

                            if (dt.DefaultView.Count > 0)
                            {
                                db.ExRollbackTransaction();
                                return "請求先 " + entity[i].invoice_id + "：" + entity[i].invoice_nm + "は" + Environment.NewLine +
                                       "請求締日 : " + ExCast.zFormatToDate_YYYYMM(entity[0].invoice_yyyymmdd) + " 以降のデータが存在する為、締切できません。";
                            }

                            #endregion

                            if (entity[i].no != "未")
                            {
                                #region 入金消込データ存在確認

                                #region SQL

                                sb.Length = 0;
                                sb.Append("SELECT T.ID " + Environment.NewLine);
                                sb.Append("  FROM T_RECEIPT_H AS T" + Environment.NewLine);
                                sb.Append(" WHERE T.COMPANY_ID = " + companyId + Environment.NewLine);
                                sb.Append("   AND T.GROUP_ID = " + groupId + Environment.NewLine);
                                sb.Append("   AND T.DELETE_FLG = 0 " + Environment.NewLine);
                                sb.Append("   AND T.INVOICE_NO = " + ExEscape.zRepStr(ExCast.zNumZeroNothingFormat(entity[i].no)) + Environment.NewLine);

                                #endregion

                                dt = db.GetDataTable(sb.ToString());
                                if (dt.DefaultView.Count > 0)
                                {
                                    db.ExRollbackTransaction();
                                    return "請求先 " + entity[i].invoice_id + "：" + entity[i].invoice_nm + Environment.NewLine +
                                           "請求番号 : " + entity[i].no + "は入金消込済の為、締切できません。";
                                }

                                #endregion

                                #region Delete SQL

                                sb.Length = 0;
                                sb.Append("DELETE FROM T_INVOICE " + Environment.NewLine);
                                sb.Append(" WHERE COMPANY_ID = " + companyId + Environment.NewLine);
                                sb.Append("   AND GROUP_ID = " + groupId + Environment.NewLine);
                                sb.Append("   AND INVOICE_KBN = 0 " + Environment.NewLine);       // 締請求分のみ
                                sb.Append("   AND NO = " + ExCast.zCLng(entity[i].no) + Environment.NewLine);

                                #endregion

                                db.ExecuteSQL(sb.ToString(), false);
                            }

                            #region Insert SQL

                            sb.Length = 0;
                            sb.Append("INSERT INTO T_INVOICE " + Environment.NewLine);
                            sb.Append("       ( COMPANY_ID" + Environment.NewLine);
                            sb.Append("       , GROUP_ID" + Environment.NewLine);
                            sb.Append("       , NO" + Environment.NewLine);
                            sb.Append("       , INVOICE_ID" + Environment.NewLine);
                            sb.Append("       , INVOICE_YYYYMMDD" + Environment.NewLine);
                            sb.Append("       , SUMMING_UP_GROUP_ID" + Environment.NewLine);
                            sb.Append("       , BEFORE_INVOICE_YYYYMMDD" + Environment.NewLine);
                            sb.Append("       , PERSON_ID" + Environment.NewLine);
                            sb.Append("       , COLLECT_CYCLE_ID" + Environment.NewLine);
                            sb.Append("       , COLLECT_DAY" + Environment.NewLine);
                            sb.Append("       , COLLECT_PLAN_DAY" + Environment.NewLine);
                            sb.Append("       , BEFORE_INVOICE_PRICE" + Environment.NewLine);
                            sb.Append("       , RECEIPT_PRICE" + Environment.NewLine);
                            sb.Append("       , TRANSFER_PRICE" + Environment.NewLine);
                            sb.Append("       , SALES_PRICE" + Environment.NewLine);
                            sb.Append("       , NO_TAX_SALES_PRICE" + Environment.NewLine);
                            sb.Append("       , TAX" + Environment.NewLine);
                            sb.Append("       , INVOICE_PRICE" + Environment.NewLine);
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
                            sb.Append("SELECT  " + companyId + Environment.NewLine);                                                // COMPANY_ID
                            sb.Append("       ," + groupId + Environment.NewLine);                                                  // GROUP_ID

                            if (entity[i].no != "未")
                            {
                                sb.Append("       ," + ExCast.zCLng(entity[i].no) + Environment.NewLine);                                         // NO
                            }
                            else
                            {
                                sb.Append("       ," + no.ToString() + Environment.NewLine);                                        // NO
                            }

                            sb.Append("       ," + ExEscape.zRepStr(ExCast.zNumZeroNothingFormat(entity[i].invoice_id)) + Environment.NewLine);     // INVOICE_ID
                            sb.Append("       ," + ExEscape.zRepStr(entity[i].invoice_yyyymmdd) + Environment.NewLine);                             // INVOICE_YYYYMMDD
                            sb.Append("       ," + ExEscape.zRepStr(entity[i].summing_up_group_id) + Environment.NewLine);                          // SUMMING_UP_DAY
                            sb.Append("       ," + ExEscape.zRepStr(entity[i].before_invoice_yyyymmdd) + Environment.NewLine);                      // BEFORE_INVOICE_YYYYMMDD
                            sb.Append("       ," + entity[i].person_id + Environment.NewLine);                                                      // PERSON_ID
                            sb.Append("       ," + entity[i].collect_cycle_id + Environment.NewLine);                                               // COLLECT_CYCLE_ID
                            sb.Append("       ," + entity[i].collect_day + Environment.NewLine);                                                    // COLLECT_DAY
                            sb.Append("       ," + ExEscape.zRepStr(entity[i].collect_plan_day) + Environment.NewLine);                             // COLLECT_PLAN_DAY
                            sb.Append("       ," + entity[i].before_invoice_price + Environment.NewLine);                                           // BEFORE_INVOICE_PRICE
                            sb.Append("       ," + entity[i].receipt_price + Environment.NewLine);                                                  // RECEIPT_PRICE
                            sb.Append("       ," + entity[i].transfer_price + Environment.NewLine);                                                 // TRANSFER_PRICE
                            sb.Append("       ," + entity[i].sales_price + Environment.NewLine);                                                    // SALES_PRICE
                            sb.Append("       ," + entity[i].no_tax_sales_price + Environment.NewLine);                                             // NO_TAX_SALES_PRICE
                            sb.Append("       ," + entity[i].tax + Environment.NewLine);                                                            // TAX
                            sb.Append("       ," + entity[i].invoice_price + Environment.NewLine);                                                  // INVOICE_PRICE
                            sb.Append("       ," + ExEscape.zRepStr(entity[i].memo) + Environment.NewLine);                                         // MEMO
                            sb.Append(CommonUtl.GetInsSQLCommonColums(CommonUtl.UpdKbn.Ins,
                                                                        PG_NM,
                                                                        "T_INVOICE",
                                                                        ExCast.zCInt(personId),
                                                                        "",
                                                                        ipAdress,
                                                                        userId));

                            #endregion

                            db.ExecuteSQL(sb.ToString(), false);

                            #region Update Sales

                            sb.Length = 0;
                            sb.Append("UPDATE T_SALES_H " + Environment.NewLine);

                            if (entity[i].no != "未")
                            {
                                sb.Append("   SET INVOICE_NO = " + ExCast.zCLng(entity[i].no) + Environment.NewLine);
                            }
                            else
                            {
                                sb.Append("   SET INVOICE_NO = " + no.ToString() + Environment.NewLine);
                            }

                            sb.Append(" WHERE DELETE_FLG = 0 " + Environment.NewLine);
                            sb.Append("   AND COMPANY_ID = " + companyId + Environment.NewLine);
                            sb.Append("   AND GROUP_ID = " + groupId + Environment.NewLine);
                            sb.Append("   AND BUSINESS_DIVISION_ID = 1 " + Environment.NewLine);        // 掛売上のみ
                            sb.Append("   AND SALES_YMD <= " + ExEscape.zRepStr(entity[i].invoice_yyyymmdd) + Environment.NewLine);
                            sb.Append("   AND INVOICE_ID = " + ExEscape.zRepStr(entity[i].invoice_id) + Environment.NewLine);

                            #endregion

                            db.ExecuteSQL(sb.ToString(), false);

                            no += 1;
                        }
                    }
                }
                catch (Exception ex)
                {
                    db.ExRollbackTransaction();
                    CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateInvoice(Insert)", ex);
                    return CLASS_NM + ".UpdateInvoice(Insert) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
                }

            }

            #endregion

            #region PG排他制御

            //try
            //{
            //    DataPgLock.DelLockPg(companyId, userId, PG_NM, "", ipAdress, false, db);
            //}
            //catch (Exception ex)
            //{
            //    db.ExRollbackTransaction();
            //    CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateInvoice(DelLockPg)", ex);
            //    return CLASS_NM + ".UpdateInvoice(DelLockPg) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
            //}

            #endregion

            #region CommitTransaction

            try
            {
                db.ExCommitTransaction();
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateInvoice(CommitTransaction)", ex);
                return CLASS_NM + ".UpdateInvoice(CommitTransaction) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
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
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateInvoice(DbClose)", ex);
                return CLASS_NM + ".UpdateInvoice(DbClose) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
            }
            finally
            {
                db = null;
            }

            #endregion

            #region Add Evidence

            try
            {
                svcPgEvidence.gAddEvidence(ExCast.zCInt(HttpContext.Current.Session[ExSession.EVIDENCE_SAVE_FLG]),
                                           companyId,
                                           userId,
                                           ipAdress,
                                           sessionString,
                                           PG_NM,
                                           DataPgEvidence.geOperationType.DeleteAndInsert,
                                           "");
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateInvoice(Add Evidence)", ex);
                return CLASS_NM + ".UpdateInvoice(Add Evidence) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
            }

            #endregion

            if (type == 2)
            {
                return "削除しました。";
            }
            else if (type == 1)
            {
                return "請求締切を実行しました。";
            }
            else
            {
                return "";
            }

        }

        /// <summary>
        /// データ更新
        /// </summary>
        /// <param name="random">セッションランダム文字列</param>
        /// <param name="entity">更新データ</param>
        /// <returns></returns>
        [OperationContract()]
        [WebMethod(EnableSession = true)]
        public void UpdateInvoicePrint(string random, int type, List<EntityInvoiceClose> entity)
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
                    CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateInvoicePrint(認証処理)" + _message);
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateInvoicePrint(認証処理)", ex);
            }

            #endregion

            #region Field

            StringBuilder sb = new StringBuilder();
            DataTable dt;
            ExMySQLData db = null;
            string _Id = "";
            int _classKbn = 0;

            #endregion

            #region Databese Open

            try
            {
                db = new ExMySQLData(ExCast.zCStr(HttpContext.Current.Session[ExSession.DB_CONNECTION_STR]));
                db.DbOpen();
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateInvoicePrint(DbOpen)", ex);
            }

            #endregion

            #region BeginTransaction

            try
            {
                db.ExBeginTransaction();
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateInvoicePrint(BeginTransaction)", ex);
            }

            #endregion

            #region Update (発行区分更新)

            try
            {
                for (int i = 0; i <= entity.Count - 1; i++)
                {
                    #region Update Sales

                    sb.Length = 0;
                    sb.Append("UPDATE T_INVOICE " + Environment.NewLine);
                    sb.Append("   SET INVOICE_PRINT_FLG = 1" + Environment.NewLine);
                    sb.Append(" WHERE COMPANY_ID = " + companyId + Environment.NewLine);
                    sb.Append("   AND GROUP_ID = " + groupId + Environment.NewLine);
                    sb.Append("   AND DELETE_FLG = 0 " + Environment.NewLine);
                    sb.Append("   AND NO = " + ExCast.zCLng(entity[i].no) + Environment.NewLine);

                    #endregion

                    db.ExecuteSQL(sb.ToString(), false);
                }
            }
            catch (Exception ex)
            {
                db.ExRollbackTransaction();
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateInvoicePrint(Update)", ex);
            }

            #endregion

            #region CommitTransaction

            try
            {
                db.ExCommitTransaction();
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateInvoicePrint(CommitTransaction)", ex);
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
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateInvoicePrint(DbClose)", ex);
            }
            finally
            {
                db = null;
            }

            #endregion

            #region Add Evidence

            try
            {
                svcPgEvidence.gAddEvidence(ExCast.zCInt(HttpContext.Current.Session[ExSession.EVIDENCE_SAVE_FLG]),
                                           companyId,
                                           userId,
                                           ipAdress,
                                           sessionString,
                                           PG_NM,
                                           DataPgEvidence.geOperationType.DeleteAndInsert,
                                           "");
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateInvoicePrint(Add Evidence)", ex);
            }

            #endregion
        }

        #endregion

    }
}
