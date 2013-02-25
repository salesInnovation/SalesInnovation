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
    public class svcPaymentCreditBalance
    {
        private const string CLASS_NM = "svcPaymentCreditBalance";
        private readonly string PG_NM = DataPgEvidence.PGName.PaymentManagement.PaymentCreditBalance;

        #region データ取得

        /// <summary>　
        /// リスト取得
        /// </summary>
        /// <param name="strWhereSql"></param>
        /// <param name="strOrderBySql"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebMethod(EnableSession = true)]
        public List<EntityPaymentCreditBalance> GetPaymentCreditBalanceList(string random, string strWhereSql, string strOrderBySql)
        {
            List<EntityPaymentCreditBalance> entityList = new List<EntityPaymentCreditBalance>();

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
                    EntityPaymentCreditBalance entity = new EntityPaymentCreditBalance();
                    entity.MESSAGE = _message;
                    entityList.Add(entity);
                    return entityList;
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetPaymentCreditBalanceList(認証処理)", ex);
                EntityPaymentCreditBalance entity = new EntityPaymentCreditBalance();
                entity.MESSAGE = "認証処理に失敗しました。" + Environment.NewLine + ex.Message.ToString(); ;
                entityList.Add(entity);
                return entityList;
            }
            #endregion

            StringBuilder sb;
            DataTable dt;
            ExMySQLData db;
            long rec_no = 0;

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
                sb.Append(rptMgr.GetPaymentCreditBalanaceListReportSQL(companyId, groupId, strWhereSql, strOrderBySql));

                dt = db.GetDataTable(sb.ToString());

                if (dt.DefaultView.Count > 0)
                {
                    for (int i = 0; i <= dt.DefaultView.Count - 1; i++)
                    {
                        #region Set Entity

                        rec_no += 1;

                        EntityPaymentCreditBalance entity = new EntityPaymentCreditBalance();
                        entity.rec_no = rec_no;
                        entity.ym = ExCast.zCStr(dt.DefaultView[i]["YM"]);
                        entity.purchase_id = ExCast.zCStr(dt.DefaultView[i]["PURCHASE_ID"]);
                        entity.purchase_nm = ExCast.zCStr(dt.DefaultView[i]["PURCHASE_NAME"]);
                        entity.before_payment_credit_balacne = ExCast.zCDbl(dt.DefaultView[i]["BEFORE_PAYMENT_CREDIT_BALANCE"]);
                        entity.before_payment_credit_balacne_upd = ExCast.zCDbl(dt.DefaultView[i]["BEFORE_PAYMENT_CREDIT_BALANCE"]);
                        entity.this_payment_cash_price = ExCast.zCDbl(dt.DefaultView[i]["THIS_PAYMENT_CASH_PRICE"]);
                        entity.this_payment_cash_percent = ExCast.zCDbl(dt.DefaultView[i]["THIS_PAYMENT_CASH_PERCENT"]);
                        entity.this_purchase_price = ExCast.zCDbl(dt.DefaultView[i]["THIS_PURCHASE_PRICE"]);
                        entity.this_tax = ExCast.zCDbl(dt.DefaultView[i]["THIS_PURCHASE_TAX"]);
                        entity.this_payment_credit_balance = ExCast.zCDbl(dt.DefaultView[i]["THIS_PAYMENT_CREDIT_BALANCE"]);

                        entity.exec_flg = false;

                        entity.lock_flg = 0;

                        entityList.Add(entity);

                        #endregion

                    }
                }

            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetPaymentCreditBalanceList", ex);
                entityList.Clear();
                EntityPaymentCreditBalance entity = new EntityPaymentCreditBalance();
                entity.MESSAGE = CLASS_NM + ".GetPaymentCreditBalanceList : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message.ToString();
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
        public string UpdatePaymentCreditBalance(string random, int type, List<EntityPaymentCreditBalance> entity)
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
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdatePaymentCreditBalance(認証処理)", ex);
                return CLASS_NM + ".UpdatePaymentCreditBalance : 認証処理に失敗しました。" + Environment.NewLine + ex.Message.ToString();
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
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdatePaymentCreditBalance(DbOpen)", ex);
                return CLASS_NM + ".UpdatePaymentCreditBalance(DbOpen) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
            }

            #endregion

            #region BeginTransaction

            try
            {
                db.ExBeginTransaction();
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdatePaymentCreditBalance(BeginTransaction)", ex);
                return CLASS_NM + ".UpdatePaymentCreditBalance(BeginTransaction) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
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

                            double upd_balance = entity[i].before_payment_credit_balacne_upd - entity[i].before_payment_credit_balacne;

                            #region Update Payment Credit Balance

                            try
                            {
                                sb.Length = 0;
                                sb.Append("UPDATE M_PAYMENT_CREDIT_BALANCE " + Environment.NewLine);

                                sb.Append(CommonUtl.GetUpdSQLCommonColums(PG_NM,
                                                                          ExCast.zCInt(personId),
                                                                          ipAdress,
                                                                          userId,
                                                                          0));

                                sb.Append("      ,PAYMENT_CREDIT_INIT_PRICE = PAYMENT_CREDIT_INIT_PRICE + " + upd_balance + Environment.NewLine);
                                sb.Append("      ,PAYMENT_CREDIT_PRICE = PAYMENT_CREDIT_PRICE + " + upd_balance + Environment.NewLine);

                                sb.Append(" WHERE DELETE_FLG = 0 " + Environment.NewLine);
                                sb.Append("   AND COMPANY_ID = " + companyId + Environment.NewLine);
                                sb.Append("   AND GROUP_ID = " + groupId + Environment.NewLine);
                                sb.Append("   AND ID = " + ExEscape.zRepStr(ExCast.zNumZeroNothingFormat(entity[i].purchase_id)) + Environment.NewLine);

                                db.ExecuteSQL(sb.ToString(), false);


                            }
                            catch (Exception ex)
                            {
                                db.ExRollbackTransaction();
                                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdatePaymentCreditBalance(Update Payment Credit Balance)", ex);
                                return CLASS_NM + ".UpdatePaymentCreditBalance(Update Payment Credit Balance) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
                            }

                            #endregion

                            #region Update Payment Credit Balance (Purchase)

                            try
                            {
                                sb.Length = 0;
                                sb.Append("UPDATE M_PURCHASE " + Environment.NewLine);

                                sb.Append(CommonUtl.GetUpdSQLCommonColums(PG_NM,
                                                                          ExCast.zCInt(personId),
                                                                          ipAdress,
                                                                          userId,
                                                                          0));

                                sb.Append("      ,PAYMENT_CREDIT_PRICE = PAYMENT_CREDIT_PRICE + " + upd_balance + Environment.NewLine);

                                sb.Append(" WHERE DELETE_FLG = 0 " + Environment.NewLine);
                                sb.Append("   AND COMPANY_ID = " + companyId + Environment.NewLine);
                                sb.Append("   AND ID = " + ExEscape.zRepStr(ExCast.zNumZeroNothingFormat(entity[i].purchase_id)) + Environment.NewLine);

                                db.ExecuteSQL(sb.ToString(), false);


                            }
                            catch (Exception ex)
                            {
                                db.ExRollbackTransaction();
                                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdatePaymentCreditBalance(Update Payment Credit Balance (Purchase))", ex);
                                return CLASS_NM + ".UpdatePaymentCreditBalance(Update Payment Credit Balance (Purchase)) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
                            }

                            #endregion

                        }
                    }
                }
                catch (Exception ex)
                {
                    db.ExRollbackTransaction();
                    CommonUtl.ExLogger.Error(CLASS_NM + ".UpdatePaymentCreditBalance(Insert)", ex);
                    return CLASS_NM + ".UpdatePaymentCreditBalance(Insert) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
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
            //    CommonUtl.ExLogger.Error(CLASS_NM + ".UpdatePaymentCreditBalance(DelLockPg)", ex);
            //    return CLASS_NM + ".UpdatePaymentCreditBalance(DelLockPg) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
            //}

            #endregion

            #region CommitTransaction

            try
            {
                db.ExCommitTransaction();
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdatePaymentCreditBalance(CommitTransaction)", ex);
                return CLASS_NM + ".UpdatePaymentCreditBalance(CommitTransaction) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
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
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdatePaymentCreditBalance(DbClose)", ex);
                return CLASS_NM + ".UpdatePaymentCreditBalance(DbClose) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
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
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdatePaymentCreditBalance(Add Evidence)", ex);
                return CLASS_NM + ".UpdatePaymentCreditBalance(Add Evidence) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
            }

            #endregion

            return "";

        }

        #endregion

    }
}
