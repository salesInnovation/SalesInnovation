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
    public class svcReceipt
    {
        private const string CLASS_NM = "svcReceipt";
        private readonly string PG_NM = DataPgEvidence.PGName.Receipt.ReceiptInp;

        #region データ取得

        /// <summary> 
        /// ヘッダリスト取得
        /// </summary>
        /// <param name="ReceiptIDFrom"></param>
        /// <param name="ReceiptIDTo"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebMethod(EnableSession = true)]
        public EntityReceiptH GetReceiptH(string random, long receipt_no)
        {

            EntityReceiptH entity = null;

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
                    entity = new EntityReceiptH();
                    entity.message = _message;
                    return entity;
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetReceiptH(認証処理)", ex);
                entity = new EntityReceiptH();
                entity.message = CLASS_NM + ".GetReceiptH : 認証処理に失敗しました。" + Environment.NewLine + ex.Message.ToString();
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

                sb.Append("SELECT T.COMPANY_ID " + Environment.NewLine);
                sb.Append("      ,T.ID " + Environment.NewLine);
                sb.Append("      ,lpad(CAST(T.NO as char), 10, '0') AS NO" + Environment.NewLine);
                sb.Append("      ,T.INVOICE_ID " + Environment.NewLine);
                sb.Append("      ,CS.NAME AS INVOICE_NAME " + Environment.NewLine);
                sb.Append("      ,CS.SUMMING_UP_GROUP_ID " + Environment.NewLine);
                sb.Append("      ,CN.NAME AS SUMMING_UP_GROUP_NM " + Environment.NewLine);
                sb.Append("      ,date_format(T.INVOICE_YYYYMMDD , " + ExEscape.SQL_YMD + ") AS INVOICE_YYYYMMDD" + Environment.NewLine);
                sb.Append("      ,T.PERSON_ID " + Environment.NewLine);
                sb.Append("      ,PS.NAME AS PERSON_NAME " + Environment.NewLine);
                sb.Append("      ,date_format(T.RECEIPT_YMD , " + ExEscape.SQL_YMD + ") AS RECEIPT_YMD" + Environment.NewLine);
                sb.Append("      ,T.SUM_PRICE " + Environment.NewLine);
                sb.Append("      ,CS.SALES_CREDIT_PRICE " + Environment.NewLine);
                sb.Append("      ,T.INVOICE_NO " + Environment.NewLine);
                sb.Append("      ,CS.RECEIPT_DIVISION_ID " + Environment.NewLine);
                sb.Append("      ,RD.DESCRIPTION AS RECEIPT_DIVISION_NAME " + Environment.NewLine);
                sb.Append("      ,T.MEMO " + Environment.NewLine);

                sb.Append("  FROM T_RECEIPT_H AS T" + Environment.NewLine);

                #region Join

                // 入力担当者
                sb.Append("  LEFT JOIN M_PERSON AS PS" + Environment.NewLine);
                sb.Append("    ON PS.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND PS.DISPLAY_FLG = 1 " + Environment.NewLine);
                sb.Append("   AND PS.COMPANY_ID = " + companyId + Environment.NewLine);
                sb.Append("   AND PS.GROUP_ID = " + groupId + Environment.NewLine);
                sb.Append("   AND PS.ID = T.PERSON_ID" + Environment.NewLine);

                // 請求先
                sb.Append("  LEFT JOIN M_CUSTOMER AS CS" + Environment.NewLine);
                sb.Append("    ON CS.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND CS.DISPLAY_FLG = 1 " + Environment.NewLine);
                sb.Append("   AND CS.COMPANY_ID = " + companyId + Environment.NewLine);
                sb.Append("   AND CS.ID = T.INVOICE_ID" + Environment.NewLine);

                // 締グループ
                sb.Append("  LEFT JOIN M_CONDITION AS CN" + Environment.NewLine);
                sb.Append("    ON CN.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND CN.DISPLAY_FLG = 1 " + Environment.NewLine);
                sb.Append("   AND CN.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
                sb.Append("   AND CN.CONDITION_DIVISION_ID = 1" + Environment.NewLine);
                sb.Append("   AND CN.ID = CS.SUMMING_UP_GROUP_ID" + Environment.NewLine);

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
                sb.Append("   AND T.NO = " + receipt_no.ToString() + Environment.NewLine);

                sb.Append(" ORDER BY T.COMPANY_ID " + Environment.NewLine);
                sb.Append("         ,T.GROUP_ID " + Environment.NewLine);
                sb.Append("         ,T.RECEIPT_YMD DESC " + Environment.NewLine);
                sb.Append("         ,T.NO DESC " + Environment.NewLine);
                sb.Append(" LIMIT 0, 1");

                #endregion

                dt = db.GetDataTable(sb.ToString());

                if (dt.DefaultView.Count > 0)
                {
                    entity = new EntityReceiptH();

                    // 排他制御
                    DataPgLock.geLovkFlg lockFlg = DataPgLock.geLovkFlg.UnLock;
                    string strErr = DataPgLock.SetLockPg(companyId, userId, PG_NM, groupId + "-" + receipt_no.ToString(), ipAdress, db, out lockFlg);
                    if (strErr != "")
                    {
                        entity.message = CLASS_NM + ".GetReceiptH : 排他制御(ロック情報取得)に失敗しました。" + Environment.NewLine + strErr;
                        return entity;
                    }

                    #region Set Entity

                    entity.id = ExCast.zCLng(dt.DefaultView[0]["ID"]);
                    entity.no = ExCast.zCLng(dt.DefaultView[0]["NO"]);
                    entity.invoice_no = ExCast.zCLng(dt.DefaultView[0]["INVOICE_NO"]);
                    entity.invoice_id = ExCast.zFormatForID(dt.DefaultView[0]["INVOICE_ID"], idFigureCustomer);
                    entity.invoice_name = ExCast.zCStr(dt.DefaultView[0]["INVOICE_NAME"]);
                    entity.summing_up_group_id = ExCast.zCStr(dt.DefaultView[0]["SUMMING_UP_GROUP_ID"]);
                    entity.summing_up_group_nm = ExCast.zCStr(dt.DefaultView[0]["SUMMING_UP_GROUP_NM"]);
                    entity.invoice_yyyymmdd = ExCast.zDateNullToDefault(dt.DefaultView[0]["INVOICE_YYYYMMDD"]);
                    entity.person_id = ExCast.zCInt(dt.DefaultView[0]["PERSON_ID"]);
                    entity.person_name = ExCast.zCStr(dt.DefaultView[0]["PERSON_NAME"]);
                    entity.receipt_ymd = ExCast.zDateNullToDefault(dt.DefaultView[0]["RECEIPT_YMD"]);
                    entity.sum_price = ExCast.zCDbl(dt.DefaultView[0]["SUM_PRICE"]);
                    entity.credit_price = ExCast.zCDbl(dt.DefaultView[0]["SALES_CREDIT_PRICE"]);
                    entity.before_credit_price = entity.credit_price + entity.sum_price;
                    entity.receipt_division_id = ExCast.zCStr(dt.DefaultView[0]["RECEIPT_DIVISION_ID"]);
                    entity.receipt_division_nm = ExCast.zCStr(dt.DefaultView[0]["RECEIPT_DIVISION_NAME"]);
                    entity.memo = ExCast.zCStr(dt.DefaultView[0]["MEMO"]);

                    // 請求締切済チェック
                    if (DataClose.IsInvoiceClose(companyId, db, entity.invoice_id, entity.receipt_ymd))
                    {
                        entity.invoice_close_flg = 1;
                    }

                    entity.lock_flg = (int)lockFlg;

                    #endregion

                    #region 請求入金取得

                    if (ExCast.zCLng(dt.DefaultView[0]["INVOICE_NO"]) != 0)
                    {
                        #region SQL

                        sb.Length = 0;
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
                        sb.Append("  FROM T_INVOICE AS T" + Environment.NewLine);

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
                        sb.Append("                AND RP.NO <> " + ExCast.zCLng(dt.DefaultView[0]["NO"]) + Environment.NewLine);
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

                        sb.Append(" WHERE T.COMPANY_ID = " + companyId + Environment.NewLine);
                        sb.Append("   AND T.GROUP_ID = " + groupId + Environment.NewLine);
                        sb.Append("   AND T.DELETE_FLG = 0 " + Environment.NewLine);
                        sb.Append("   AND T.NO = " + ExCast.zCLng(dt.DefaultView[0]["INVOICE_NO"]) + Environment.NewLine);
                        sb.Append(" LIMIT 0, 1");

                        #endregion

                        dt2 = db.GetDataTable(sb.ToString());

                        if (dt2.DefaultView.Count > 0)
                        {
                            entity.no = ExCast.zCLng(dt.DefaultView[0]["INVOICE_NO"]);
                            entity.invoice_kbn = ExCast.zCInt(dt2.DefaultView[0]["INVOICE_KBN"]);
                            entity.invoice_kbn_nm = ExCast.zCStr(dt2.DefaultView[0]["INVOICE_KBN_NM"]);

                            entity.summing_up_group_id = ExCast.zCStr(dt2.DefaultView[0]["SUMMING_UP_GROUP_ID"]);
                            entity.summing_up_group_nm = ExCast.zCStr(dt2.DefaultView[0]["SUMMING_UP_GROUP_NM"]);
                            entity.invoice_yyyymmdd = ExCast.zDateNullToDefault(dt2.DefaultView[0]["INVOICE_YYYYMMDD"]);
                            entity.collect_plan_day = ExCast.zDateNullToDefault(dt2.DefaultView[0]["COLLECT_PLAN_DAY"]);
                            entity.invoice_price = ExCast.zCDbl(dt2.DefaultView[0]["INVOICE_PRICE"]);
                            entity.before_receipt_price = ExCast.zCDbl(dt2.DefaultView[0]["BEFORE_RECEIPT_PRICE"]);
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
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetReceiptH", ex);
                entity = new EntityReceiptH();
                entity.message = CLASS_NM + ".GetReceiptH : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message.ToString();
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
                                       "NO:" + receipt_no.ToString());

            return entity;


        }

        /// <summary> 
        /// 明細リスト取得
        /// </summary>
        /// <param name="ReceiptIDFrom"></param>
        /// <param name="ReceiptIDTo"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebMethod(EnableSession = true)]
        public List<EntityReceiptD> GetReceiptListD(string random, long RECEIPT_ID)
        {
            List<EntityReceiptD> entityList = new List<EntityReceiptD>();

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
                    EntityReceiptD entity = new EntityReceiptD();
                    entity.message = _message;
                    entityList.Add(entity);
                    return entityList;
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetReceiptListD(認証処理)", ex);
                EntityReceiptD entity = new EntityReceiptD();
                entity.message = CLASS_NM + ".GetReceiptListD : 認証処理に失敗しました。" + Environment.NewLine + ex.Message.ToString(); ;
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

                sb.Append("SELECT T.RECEIPT_ID " + Environment.NewLine);
                sb.Append("      ,T.REC_NO " + Environment.NewLine);
                sb.Append("      ,T.RECEIPT_DIVISION_ID " + Environment.NewLine);
                sb.Append("      ,T.DESCRIPTION " + Environment.NewLine);
                sb.Append("      ,date_format(T.BILL_DUE_DATE , " + ExEscape.SQL_YMD + ") AS BILL_DUE_DATE" + Environment.NewLine);
                sb.Append("      ,T.PRICE " + Environment.NewLine);
                sb.Append("      ,T.MEMO " + Environment.NewLine);
                sb.Append("  FROM T_RECEIPT_D AS T" + Environment.NewLine);

                #region Join

                #endregion

                sb.Append(" WHERE T.COMPANY_ID = " + companyId + Environment.NewLine);
                sb.Append("   AND T.GROUP_ID = " + groupId + Environment.NewLine);
                sb.Append("   AND T.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND T.RECEIPT_ID = " + RECEIPT_ID + Environment.NewLine);

                sb.Append(" ORDER BY T.COMPANY_ID " + Environment.NewLine);
                sb.Append("         ,T.GROUP_ID " + Environment.NewLine);
                sb.Append("         ,T.REC_NO " + Environment.NewLine);
                sb.Append(" LIMIT 0, 100");

                #endregion

                dt = db.GetDataTable(sb.ToString());

                if (dt.DefaultView.Count > 0)
                {
                    for (int i = 0; i <= dt.DefaultView.Count - 1; i++)
                    {
                        EntityReceiptD entityD = new EntityReceiptD();

                        #region Set Entity

                        entityD.id = ExCast.zCLng(dt.DefaultView[i]["RECEIPT_ID"]);
                        entityD.rec_no = ExCast.zCInt(dt.DefaultView[i]["REC_NO"]);
                        entityD.receipt_division_id = ExCast.zCStr(dt.DefaultView[i]["RECEIPT_DIVISION_ID"]);
                        entityD.receipt_division_nm = ExCast.zCStr(dt.DefaultView[i]["DESCRIPTION"]);
                        entityD.bill_site_day = ExCast.zDateNullToDefault(dt.DefaultView[i]["BILL_DUE_DATE"]);
                        entityD.price = ExCast.zCDbl(dt.DefaultView[i]["PRICE"]);
                        entityD.memo = ExCast.zCStr(dt.DefaultView[i]["MEMO"]);

                        entityList.Add(entityD);

                        #endregion
                    }
                }

                return entityList;

            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetReceiptListD", ex);
                entityList.Clear();
                EntityReceiptD entityD = new EntityReceiptD();
                entityD.message = CLASS_NM + ".GetReceiptH : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message.ToString();
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
        /// <param name="strOrderBySql"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebMethod(EnableSession = true)]
        public List<EntityReceipt> GetReceiptList(string random, string strWhereSql, string strOrderBySql)
        {
            List<EntityReceipt> entityList = new List<EntityReceipt>();

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
                    EntityReceipt entity = new EntityReceipt();
                    entity.MESSAGE = _message;
                    entityList.Add(entity);
                    return entityList;
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetReceiptList(認証処理)", ex);
                EntityReceipt entity = new EntityReceipt();
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
                sb.Append(rptMgr.GetReceiptListReportSQL(companyId, groupId, strWhereSql, strOrderBySql));

                dt = db.GetDataTable(sb.ToString());

                if (dt.DefaultView.Count > 0)
                {
                    for (int i = 0; i <= dt.DefaultView.Count - 1; i++)
                    {
                        EntityReceipt _entity = new EntityReceipt();
                        _entity.ID = ExCast.zCLng(dt.DefaultView[i]["ID"]);
                        _entity.NO = ExCast.zCLng(dt.DefaultView[i]["NO"]);
                        _entity.RECEIPT_YMD = ExCast.zDateNullToDefault(dt.DefaultView[i]["RECEIPT_YMD"]);
                        _entity.INPUT_PERSON = ExCast.zCInt(dt.DefaultView[i]["INPUT_PERSON"]);
                        _entity.INPUT_PERSON_NM = ExCast.zCStr(dt.DefaultView[i]["INPUT_PERSON_NM"]);
                        _entity.INVOICE_ID = ExCast.zFormatForID(dt.DefaultView[0]["INVOICE_ID"], idFigureCustomer);
                        _entity.INVOICE_NM = ExCast.zCStr(dt.DefaultView[i]["INVOICE_NM"]);
                        _entity.SUM_PRICE = ExCast.zCDbl(dt.DefaultView[i]["SUM_PRICE"]);
                        _entity.MEMO = ExCast.zCStr(dt.DefaultView[i]["MEMO"]);
                        _entity.UPDATE_PERSON_ID = ExCast.zCInt(dt.DefaultView[i]["UPDATE_PERSON_ID"]);
                        _entity.UPDATE_PERSON_NM = ExCast.zCStr(dt.DefaultView[i]["UPDATE_PERSON_NM"]);
                        _entity.REC_NO = ExCast.zCInt(dt.DefaultView[i]["REC_NO"]);
                        _entity.RECEIPT_DIVISION_ID = ExCast.zCInt(dt.DefaultView[i]["RECEIPT_DIVISION_ID"]);
                        _entity.RECEIPT_DIVISION_NM = ExCast.zCStr(dt.DefaultView[i]["DESCRIPTION"]);
                        _entity.PRICE = ExCast.zCDbl(dt.DefaultView[i]["PRICE"]);
                        _entity.BILL_SITE_DAY = ExCast.zDateNullToDefault(dt.DefaultView[i]["BILL_DUE_DATE"]);
                        _entity.D_MEMO = ExCast.zCStr(dt.DefaultView[i]["D_MEMO"]);
                        entityList.Add(_entity);
                    }
                }

            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetReceiptList", ex);
                entityList.Clear();
                EntityReceipt entity = new EntityReceipt();
                entity.MEMO = CLASS_NM + ".GetReceiptList : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message.ToString();
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
                                       DataPgEvidence.PGName.Receipt.ReceiptList,
                                       DataPgEvidence.geOperationType.Select,
                                       "Where:" + strWhereSql + ",Orderby:" + strOrderBySql);

            return entityList;

        }

        #endregion

        #region データ更新

        /// <summary>
        /// データ更新
        /// </summary>
        /// <param name="random">セッションランダム文字列</param>
        /// <param name="type">0:Update 1:Insert 2:Delete</param>
        /// <param name="ReceiptNo">伝票番号</param>
        /// <param name="entityH">ヘッダデータ</param>
        /// <param name="entityD">明細データ</param>
        /// <returns></returns>
        [OperationContract()]
        [WebMethod(EnableSession = true)]
        public string UpdateReceipt(string random, int type, long ReceiptNo, EntityReceiptH entityH, List<EntityReceiptD> entityD, EntityReceiptH before_entityH)
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
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateReceipt(認証処理)", ex);
                return CLASS_NM + ".認証処理に失敗しました。" + Environment.NewLine + ex.Message.ToString();
            }

            #endregion

            #region Field

            StringBuilder sb = new StringBuilder();
            DataTable dt;
            ExMySQLData db = null;

            string accountPeriod = "";
            long id = 0;
            long no = ReceiptNo;

            #endregion

            #region Databese Open

            try
            {
                db = new ExMySQLData(ExCast.zCStr(HttpContext.Current.Session[ExSession.DB_CONNECTION_STR]));
                db.DbOpen();
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateReceipt(DbOpen)", ex);
                return "UpdateReceipt(DbOpen) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
            }

            #endregion

            #region BeginTransaction

            try
            {
                db.ExBeginTransaction();
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateReceipt(BeginTransaction)", ex);
                return "UpdateReceipt(BeginTransaction) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
            }

            #endregion

            #region Invoice Close Check

            try
            {
                if (type <= 1)
                {
                    // 請求締切済チェック
                    if (DataClose.IsInvoiceClose(companyId, db, entityH.invoice_id, entityH.receipt_ymd))
                    {
                        db.DbClose();
                        return "入金日 : " + entityH.receipt_ymd + " は請求締切済の為、計上できません。";
                    }
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateReceipt(Invoice Close Check)", ex);
                return "UpdateReceipt(Invoice Close Check) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
            }

            #endregion

            #region Update or Insert

            if (type <= 1)
            {
                #region Get Accout Period

                try
                {
                    accountPeriod = DataAccount.GetAccountPeriod(ExCast.zCStr(HttpContext.Current.Session[ExSession.ACCOUNT_BEGIN_PERIOD]), entityH.receipt_ymd);
                    if (accountPeriod == "")
                    {
                        return "会計年の取得に失敗しました。(期首月日 : " + ExCast.zCStr(HttpContext.Current.Session[ExSession.ACCOUNT_BEGIN_PERIOD]) +
                                                             " 入金日 : " + entityH.receipt_ymd + ")";
                    }
                }
                catch (Exception ex)
                {
                    CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateReceipt(GetAccountPeriod)", ex);
                    return "UpdateReceipt(GetAccountPeriod) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
                }

                #endregion

                #region Get Slip No

                try
                {
                    DataSlipNo.GetSlipNo(companyId,
                                         groupId,
                                         db,
                                         DataSlipNo.geSlipKbn.Receipt,
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
                    CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateReceipt(GetSlipNo)", ex);
                    return "UpdateReceipt(GetSlipNo) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
                }

                #endregion

                #region Head Insert

                try
                {
                    #region SQL

                    sb.Length = 0;
                    sb.Append("INSERT INTO T_RECEIPT_H " + Environment.NewLine);
                    sb.Append("       ( COMPANY_ID" + Environment.NewLine);
                    sb.Append("       , GROUP_ID" + Environment.NewLine);
                    sb.Append("       , ID" + Environment.NewLine);
                    sb.Append("       , NO" + Environment.NewLine);
                    sb.Append("       , INVOICE_ID" + Environment.NewLine);
                    sb.Append("       , INVOICE_YYYYMMDD" + Environment.NewLine);
                    sb.Append("       , PERSON_ID" + Environment.NewLine);
                    sb.Append("       , RECEIPT_YMD" + Environment.NewLine);
                    sb.Append("       , SUM_PRICE" + Environment.NewLine);
                    sb.Append("       , INVOICE_NO" + Environment.NewLine);
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
                    sb.Append("       ," + groupId + Environment.NewLine);                                                                  // COMPANY_ID
                    sb.Append("       ," + id.ToString() + Environment.NewLine);                                                            // ID
                    sb.Append("       ," + no.ToString() + Environment.NewLine);                                                            // NO
                    sb.Append("       ," + ExEscape.zRepStr(ExCast.zNumZeroNothingFormat(entityH.invoice_id)) + Environment.NewLine);       // INVOICE_ID
                    sb.Append("       ," + ExEscape.zRepStr(entityH.invoice_yyyymmdd) + Environment.NewLine);                               // INVOICE_YYYYMM
                    sb.Append("       ," + entityH.person_id + Environment.NewLine);                                                        // PERSON_ID
                    sb.Append("       ," + ExEscape.zRepStr(ExCast.zDateNullToDefault(entityH.receipt_ymd)) + Environment.NewLine);         // RECEIPT_YMD
                    sb.Append("       ," + entityH.sum_price + Environment.NewLine);                                                        // SUM_PRICE
                    sb.Append("       ," + entityH.invoice_no + Environment.NewLine);                                                       // INVOICE_NO
                    sb.Append("       ," + ExEscape.zRepStr(entityH.memo) + Environment.NewLine);                                           // MEMO
                    if (type == 0)
                    {
                        sb.Append(CommonUtl.GetInsSQLCommonColums(CommonUtl.UpdKbn.Upd,
                                                                  PG_NM,
                                                                  "T_RECEIPT_H",
                                                                  entityH.update_person_id,
                                                                  ReceiptNo.ToString(),
                                                                  ipAdress,
                                                                  userId));
                    }
                    else
                    {
                        sb.Append(CommonUtl.GetInsSQLCommonColums(CommonUtl.UpdKbn.Ins,
                                                                  PG_NM,
                                                                  "T_RECEIPT_H",
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
                    CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateReceipt(Head Insert)", ex);
                    return "UpdateReceipt(Head Insert) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
                }

                #endregion

                #region Detail Insert

                try
                {
                    for (int i = 0; i <= entityD.Count - 1; i++)
                    {
                        if ((!string.IsNullOrEmpty(entityD[i].receipt_division_id)) || (!string.IsNullOrEmpty(entityD[i].receipt_division_nm)) ||
                             (entityD[i].price != 0) || (!string.IsNullOrEmpty(entityD[i].bill_site_day)) || (!string.IsNullOrEmpty(entityD[i].memo)))
                        {
                            #region SQL

                            sb.Length = 0;
                            sb.Append("INSERT INTO T_RECEIPT_D " + Environment.NewLine);
                            sb.Append("       ( COMPANY_ID" + Environment.NewLine);
                            sb.Append("       , GROUP_ID" + Environment.NewLine);
                            sb.Append("       , RECEIPT_ID" + Environment.NewLine);
                            sb.Append("       , REC_NO" + Environment.NewLine);
                            sb.Append("       , RECEIPT_DIVISION_ID" + Environment.NewLine);
                            sb.Append("       , DESCRIPTION" + Environment.NewLine);
                            sb.Append("       , BILL_DUE_DATE" + Environment.NewLine);
                            sb.Append("       , PRICE" + Environment.NewLine);
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
                            sb.Append("       ," + groupId + Environment.NewLine);                                                          // COMPANY_ID
                            sb.Append("       ," + id.ToString() + Environment.NewLine);                                                    // RECEIPT_ID
                            sb.Append("       ," + entityD[i].rec_no + Environment.NewLine);                                                // REC_NO
                            sb.Append("       ," + ExEscape.zRepStr(entityD[i].receipt_division_id) + Environment.NewLine);                                   // RECEIPT_DIVISION_ID
                            sb.Append("       ," + ExEscape.zRepStr(entityD[i].receipt_division_nm) + Environment.NewLine);                                   // DESCRIPTION
                            sb.Append("       ," + ExEscape.zRepStr(ExCast.zNumZeroNothingFormat(entityD[i].bill_site_day)) + Environment.NewLine);    // BILL_DUE_DATE
                            sb.Append("       ," + entityD[i].price + Environment.NewLine);                                                 // PRICE
                            sb.Append("       ," + ExEscape.zRepStr(entityD[i].memo) + Environment.NewLine);                                           // MEMO

                            if (type == 0)
                            {
                                sb.Append(CommonUtl.GetInsSQLCommonColums(CommonUtl.UpdKbn.Upd,
                                                                          PG_NM,
                                                                          "T_RECEIPT_D",
                                                                          entityH.update_person_id,
                                                                          ExCast.zCStr(entityH.id),
                                                                          ipAdress,
                                                                          userId));
                            }
                            else
                            {
                                sb.Append(CommonUtl.GetInsSQLCommonColums(CommonUtl.UpdKbn.Ins,
                                                                          PG_NM,
                                                                          "T_RECEIPT_D",
                                                                          entityH.update_person_id,
                                                                          "0",
                                                                          ipAdress,
                                                                          userId));
                            }

                            #endregion

                            db.ExecuteSQL(sb.ToString(), false);
                        }
                    }
                }
                catch (Exception ex)
                {
                    db.ExRollbackTransaction();
                    CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateReceipt(Detail Insert)", ex);
                    return "UpdateReceipt(Detail Insert) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
                }

                #endregion

                #region Head Update

                if (type == 0)
                {
                    try
                    {
                        sb.Length = 0;
                        sb.Append("UPDATE T_RECEIPT_H " + Environment.NewLine);
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
                        CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateReceipt(Head Update)", ex);
                        return "UpdateReceipt(Head Update) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
                    }
                }

                #endregion

                #region Detail Update

                if (type == 0)
                {
                    try
                    {
                        sb.Length = 0;
                        sb.Append("UPDATE T_RECEIPT_D " + Environment.NewLine);
                        sb.Append(CommonUtl.GetUpdSQLCommonColums(PG_NM,
                                                                  entityH.update_person_id,
                                                                  ipAdress,
                                                                  userId,
                                                                  1));
                        sb.Append(" WHERE COMPANY_ID = " + companyId + Environment.NewLine);
                        sb.Append("   AND GROUP_ID = " + groupId + Environment.NewLine);
                        sb.Append("   AND RECEIPT_ID = " + entityH.id.ToString() + Environment.NewLine);

                        db.ExecuteSQL(sb.ToString(), false);

                    }
                    catch (Exception ex)
                    {
                        db.ExRollbackTransaction();
                        CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateReceipt(Detail Update)", ex);
                        return "UpdateReceipt(Detail Update) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
                    }
                }

                #endregion

                #region Update Slip No

                try
                {
                    if (type == 0)
                    {
                        DataSlipNo.UpdateSlipNo(companyId, groupId, db, DataSlipNo.geSlipKbn.Receipt, accountPeriod, 0, id);
                    }
                    else
                    {
                        DataSlipNo.UpdateSlipNo(companyId, groupId, db, DataSlipNo.geSlipKbn.Receipt, accountPeriod, no, id);
                    }

                    if (no == 0 || id == 0)
                    {
                        return "伝票番号の更新に失敗しました。(会計年 : " + accountPeriod + ")";
                    }
                }
                catch (Exception ex)
                {
                    CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateReceipt(UpdateSlipNo)", ex);
                    return "UpdateReceipt(UpdateSlipNo) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
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
                    sb.Append("UPDATE T_RECEIPT_H " + Environment.NewLine);
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
                    CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateReceipt(Head Delete)", ex);
                    return "UpdateReceipt(Head Update) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
                }

                #endregion

                #region Detail Delete

                try
                {
                    sb.Length = 0;
                    sb.Append("UPDATE T_RECEIPT_D " + Environment.NewLine);
                    sb.Append(CommonUtl.GetUpdSQLCommonColums(PG_NM,
                                                              entityH.update_person_id,
                                                              ipAdress,
                                                              userId,
                                                              1));
                    sb.Append(" WHERE COMPANY_ID = " + companyId + Environment.NewLine);
                    sb.Append("   AND GROUP_ID = " + groupId + Environment.NewLine);
                    sb.Append("   AND RECEIPT_ID = " + entityH.id.ToString() + Environment.NewLine);

                    db.ExecuteSQL(sb.ToString(), false);

                }
                catch (Exception ex)
                {
                    db.ExRollbackTransaction();
                    CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateReceipt(Detail Delete)", ex);
                    return "UpdateReceipt(Detail Update) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
                }

                #endregion
            }

            #endregion

            #region Update Sales Credit

            try
            {
                double _price = 0;
                switch (type)
                {
                    case 0:         // Update
                        _price = before_entityH.sum_price;
                        DataCredit.UpdSalesCredit(companyId, groupId, db, before_entityH.invoice_id, _price, PG_NM, ExCast.zCInt(entityH.update_person_id), ipAdress, userId);
                        _price = entityH.sum_price * -1;
                        DataCredit.UpdSalesCredit(companyId, groupId, db, entityH.invoice_id, _price, PG_NM, ExCast.zCInt(entityH.update_person_id), ipAdress, userId);
                        break;
                    case 1:         // Insert
                        _price = entityH.sum_price * -1;
                        DataCredit.UpdSalesCredit(companyId, groupId, db, entityH.invoice_id, _price, PG_NM, ExCast.zCInt(entityH.update_person_id), ipAdress, userId);
                        break;
                    case 2:         // Delete
                        _price = before_entityH.sum_price;
                        DataCredit.UpdSalesCredit(companyId, groupId, db, before_entityH.invoice_id, _price, PG_NM, ExCast.zCInt(entityH.update_person_id), ipAdress, userId);
                        break;
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateReceipt(UpdateSalesCredit)", ex);
                return "UpdateReceipt(UpdateSalesCredit) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
            }

            #endregion

            #region CommitTransaction

            try
            {
                db.ExCommitTransaction();
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateReceipt(CommitTransaction)", ex);
                return "UpdateReceipt(CommitTransaction) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
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
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateReceipt(DbClose)", ex);
                return "UpdateReceipt(DbClose) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
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
                                                   DataPgEvidence.PGName.Receipt.ReceiptInp,
                                                   DataPgEvidence.geOperationType.Update,
                                                   "NO:" + no.ToString() + ",ID:" + id.ToString());
                        break;
                    case 1:
                        svcPgEvidence.gAddEvidence(ExCast.zCInt(HttpContext.Current.Session[ExSession.EVIDENCE_SAVE_FLG]),
                                                   companyId,
                                                   userId,
                                                   ipAdress,
                                                   sessionString,
                                                   DataPgEvidence.PGName.Receipt.ReceiptInp,
                                                   DataPgEvidence.geOperationType.Insert,
                                                   "NO:" + no.ToString() + ",ID:" + id.ToString());
                        break;
                    case 2:
                        svcPgEvidence.gAddEvidence(ExCast.zCInt(HttpContext.Current.Session[ExSession.EVIDENCE_SAVE_FLG]),
                                                   companyId,
                                                   userId,
                                                   ipAdress,
                                                   sessionString,
                                                   DataPgEvidence.PGName.Receipt.ReceiptInp,
                                                   DataPgEvidence.geOperationType.Delete,
                                                   "NO:" + no.ToString() + ",ID:" + id.ToString());
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateReceipt(Add Evidence)", ex);
                return "UpdateReceipt(Add Evidence) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
            }

            #endregion

            #region Return

            if (type == 1 && ReceiptNo == 0)
            {
                return "Auto Insert success : " + "入金番号 : " + no.ToString().ToString() + "で登録しました。";
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
