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
    public class svcInvoiceBalance
    {
        private const string CLASS_NM = "svcInvoiceBalance";
        private readonly string PG_NM = DataPgEvidence.PGName.SalesManagement.InvoiceBalance;

        #region データ取得

        /// <summary>　
        /// リスト取得
        /// </summary>
        /// <param name="strWhereSql"></param>
        /// <param name="strOrderBySql"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebMethod(EnableSession = true)]
        public List<EntityInvoiceBalance> GetInvoiceBalanceList(string random, string strWhereSql, string strOrderBySql)
        {
            List<EntityInvoiceBalance> entityList = new List<EntityInvoiceBalance>();

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
                    EntityInvoiceBalance entity = new EntityInvoiceBalance();
                    entity.MESSAGE = _message;
                    entityList.Add(entity);
                    return entityList;
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetInvoiceBalanceList(認証処理)", ex);
                EntityInvoiceBalance entity = new EntityInvoiceBalance();
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
                sb.Append(rptMgr.GetInvoiceBalanceListReportSQL(companyId, groupId, strWhereSql, strOrderBySql));

                dt = db.GetDataTable(sb.ToString());

                if (dt.DefaultView.Count > 0)
                {
                    for (int i = 0; i <= dt.DefaultView.Count - 1; i++)
                    {
                        #region Set Entity

                        EntityInvoiceBalance entity = new EntityInvoiceBalance();
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

                        entity.invoice_kbn = ExCast.zCInt(dt.DefaultView[i]["INVOICE_KBN"]);
                        entity.invoice_kbn_nm = ExCast.zCStr(dt.DefaultView[i]["INVOICE_KBN_NM"]);

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
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetInvoiceBalanceList", ex);
                entityList.Clear();
                EntityInvoiceBalance entity = new EntityInvoiceBalance();
                entity.MESSAGE = CLASS_NM + ".GetInvoiceBalanceList : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message.ToString();
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
        public string UpdateInvoiceBalance(string random, int type, List<EntityInvoiceBalance> entity)
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
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateInvoiceBalance(認証処理)", ex);
                return CLASS_NM + ".UpdateInvoiceBalance : 認証処理に失敗しました。" + Environment.NewLine + ex.Message.ToString();
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
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateInvoiceBalance(DbOpen)", ex);
                return CLASS_NM + ".UpdateInvoiceBalance(DbOpen) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
            }

            #endregion

            #region BeginTransaction

            try
            {
                db.ExBeginTransaction();
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateInvoiceBalance(BeginTransaction)", ex);
                return CLASS_NM + ".UpdateInvoiceBalance(BeginTransaction) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
            }

            #endregion

            #region Update

            if (type == 0)
            {
                try
                {
                    for (int i = 0; i <= entity.Count - 1; i++)
                    {
                        if (entity[i].exec_flg == true)
                        {

                            double upd_balance = entity[i].before_invoice_price_upd - entity[i].before_invoice_price;

                            #region Update Invoice Balance

                            try
                            {
                                sb.Length = 0;
                                sb.Append("UPDATE T_INVOICE " + Environment.NewLine);

                                sb.Append(CommonUtl.GetUpdSQLCommonColums(PG_NM,
                                                                          ExCast.zCInt(personId),
                                                                          ipAdress,
                                                                          userId,
                                                                          0));

                                sb.Append("      ,BEFORE_INVOICE_PRICE = BEFORE_INVOICE_PRICE + " + upd_balance + Environment.NewLine);
                                sb.Append("      ,TRANSFER_PRICE = TRANSFER_PRICE + " + upd_balance + Environment.NewLine);
                                sb.Append("      ,INVOICE_PRICE = INVOICE_PRICE + " + upd_balance + Environment.NewLine);

                                sb.Append(" WHERE DELETE_FLG = 0 " + Environment.NewLine);
                                sb.Append("   AND COMPANY_ID = " + companyId + Environment.NewLine);
                                sb.Append("   AND GROUP_ID = " + groupId + Environment.NewLine);
                                sb.Append("   AND INVOICE_KBN = 0 " + Environment.NewLine);
                                sb.Append("   AND INVOICE_ID = " + ExEscape.zRepStr(ExCast.zNumZeroNothingFormat(entity[i].invoice_id)) + Environment.NewLine);

                                db.ExecuteSQL(sb.ToString(), false);
                            }
                            catch (Exception ex)
                            {
                                db.ExRollbackTransaction();
                                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateInvoiceBalance(Update Invoice Balance)", ex);
                                return CLASS_NM + ".UpdateInvoiceBalance(Update Invoice Balance) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
                            }

                            #endregion

                        }
                    }
                }
                catch (Exception ex)
                {
                    db.ExRollbackTransaction();
                    CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateInvoiceBalance(Insert)", ex);
                    return CLASS_NM + ".UpdateInvoiceBalance(Insert) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
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
            //    CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateInvoiceBalance(DelLockPg)", ex);
            //    return CLASS_NM + ".UpdateInvoiceBalance(DelLockPg) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
            //}

            #endregion

            #region CommitTransaction

            try
            {
                db.ExCommitTransaction();
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateInvoiceBalance(CommitTransaction)", ex);
                return CLASS_NM + ".UpdateInvoiceBalance(CommitTransaction) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
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
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateInvoiceBalance(DbClose)", ex);
                return CLASS_NM + ".UpdateInvoiceBalance(DbClose) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
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
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateInvoiceBalance(Add Evidence)", ex);
                return CLASS_NM + ".UpdateInvoiceBalance(Add Evidence) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
            }

            #endregion

            return "";

        }

        #endregion

    }
}
