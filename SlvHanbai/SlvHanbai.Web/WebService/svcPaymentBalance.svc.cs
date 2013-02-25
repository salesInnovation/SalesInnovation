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
    public class svcPaymentBalance
    {
        private const string CLASS_NM = "svcPaymentBalance";
        private readonly string PG_NM = DataPgEvidence.PGName.PaymentManagement.PaymentBalance;

        #region データ取得

        /// <summary>　
        /// リスト取得
        /// </summary>
        /// <param name="strWhereSql"></param>
        /// <param name="strOrderBySql"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebMethod(EnableSession = true)]
        public List<EntityPaymentBalance> GetPaymentBalanceList(string random, string strWhereSql, string strOrderBySql)
        {
            List<EntityPaymentBalance> entityList = new List<EntityPaymentBalance>();

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
                    EntityPaymentBalance entity = new EntityPaymentBalance();
                    entity.MESSAGE = _message;
                    entityList.Add(entity);
                    return entityList;
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetPaymentBalanceList(認証処理)", ex);
                EntityPaymentBalance entity = new EntityPaymentBalance();
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
                sb.Append(rptMgr.GetPaymentBalanceListReportSQL(companyId, groupId, strWhereSql, strOrderBySql));

                dt = db.GetDataTable(sb.ToString());

                if (dt.DefaultView.Count > 0)
                {
                    for (int i = 0; i <= dt.DefaultView.Count - 1; i++)
                    {
                        #region Set Entity

                        EntityPaymentBalance entity = new EntityPaymentBalance();
                        entity.no = ExCast.zCStr(dt.DefaultView[i]["NO"]);
                        entity.purchase_id = ExCast.zCStr(dt.DefaultView[i]["PURCHASE_ID"]);
                        entity.purchase_nm = ExCast.zCStr(dt.DefaultView[i]["PURCHASE_NM"]);
                        entity.payment_yyyymmdd = ExCast.zDateNullToDefault(dt.DefaultView[i]["PAYMENT_YYYYMMDD"]);
                        entity.summing_up_group_id = ExCast.zCStr(dt.DefaultView[i]["SUMMING_UP_GROUP_ID"]);
                        entity.summing_up_group_nm = ExCast.zCStr(dt.DefaultView[i]["SUMMING_UP_GROUP_NM"]);
                        entity.person_id = ExCast.zCInt(dt.DefaultView[i]["INPUT_PERSON"]);
                        entity.person_nm = ExCast.zCStr(dt.DefaultView[i]["INPUT_PERSON_NM"]);

                        entity.payment_plan_day = ExCast.zDateNullToDefault(dt.DefaultView[i]["PAYMENT_PLAN_DAY"]);
                        entity.payment_day = ExCast.zCInt(dt.DefaultView[i]["PAYMENT_DAY"]);

                        entity.before_payment_yyyymmdd = ExCast.zDateNullToDefault(dt.DefaultView[i]["BEFORE_PAYMENT_YYYYMMDD"]);

                        entity.before_payment_price = ExCast.zCDbl(dt.DefaultView[i]["BEFORE_PAYMENT_PRICE"]);
                        entity.before_payment_price_upd = ExCast.zCDbl(dt.DefaultView[i]["BEFORE_PAYMENT_PRICE"]);
                        entity.payment_cash_price = ExCast.zCDbl(dt.DefaultView[i]["PAYMENT_CASH_PRICE"]);
                        entity.transfer_price = ExCast.zCDbl(dt.DefaultView[i]["TRANSFER_PRICE"]);
                        entity.purchase_price = ExCast.zCDbl(dt.DefaultView[i]["PURCHASE_PRICE"]);
                        entity.no_tax_purchase_price = ExCast.zCDbl(dt.DefaultView[i]["NO_TAX_PURCHASE_PRICE"]);
                        entity.tax = ExCast.zCDbl(dt.DefaultView[i]["TAX"]);

                        entity.payment_price = ExCast.zCDbl(dt.DefaultView[i]["PAYMENT_PRICE"]);

                        entity.payment_kbn = ExCast.zCInt(dt.DefaultView[i]["PAYMENT_KBN"]);
                        //entity.payment_kbn_nm = ExCast.zCStr(dt.DefaultView[i]["PAYMENT_KBN_NM"]);

                        entity.memo = ExCast.zCStr(dt.DefaultView[i]["MEMO"]);

                        entity.payment_exists_flg = 0;
                        entity.exec_flg = false;

                        entity.lock_flg = 0;

                        entityList.Add(entity);

                        #endregion

                    }
                }

            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetPaymentBalanceList", ex);
                entityList.Clear();
                EntityPaymentBalance entity = new EntityPaymentBalance();
                entity.MESSAGE = CLASS_NM + ".GetPaymentBalanceList : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message.ToString();
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
        public string UpdatePaymentBalance(string random, int type, List<EntityPaymentBalance> entity)
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
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdatePaymentBalance(認証処理)", ex);
                return CLASS_NM + ".UpdatePaymentBalance : 認証処理に失敗しました。" + Environment.NewLine + ex.Message.ToString();
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
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdatePaymentBalance(DbOpen)", ex);
                return CLASS_NM + ".UpdatePaymentBalance(DbOpen) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
            }

            #endregion

            #region BeginTransaction

            try
            {
                db.ExBeginTransaction();
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdatePaymentBalance(BeginTransaction)", ex);
                return CLASS_NM + ".UpdatePaymentBalance(BeginTransaction) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
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

                            double upd_balance = entity[i].before_payment_price_upd - entity[i].before_payment_price;

                            #region Update Payment Balance

                            try
                            {
                                sb.Length = 0;
                                sb.Append("UPDATE T_PAYMENT " + Environment.NewLine);

                                sb.Append(CommonUtl.GetUpdSQLCommonColums(PG_NM,
                                                                          ExCast.zCInt(personId),
                                                                          ipAdress,
                                                                          userId,
                                                                          0));

                                sb.Append("      ,BEFORE_PAYMENT_PRICE = BEFORE_PAYMENT_PRICE + " + upd_balance + Environment.NewLine);
                                sb.Append("      ,TRANSFER_PRICE = TRANSFER_PRICE + " + upd_balance + Environment.NewLine);
                                sb.Append("      ,PAYMENT_PRICE = PAYMENT_PRICE + " + upd_balance + Environment.NewLine);

                                sb.Append(" WHERE DELETE_FLG = 0 " + Environment.NewLine);
                                sb.Append("   AND COMPANY_ID = " + companyId + Environment.NewLine);
                                sb.Append("   AND GROUP_ID = " + groupId + Environment.NewLine);
                                sb.Append("   AND PAYMENT_KBN = 0 " + Environment.NewLine);
                                sb.Append("   AND PURCHASE_ID = " + ExEscape.zRepStr(ExCast.zNumZeroNothingFormat(entity[i].purchase_id)) + Environment.NewLine);

                                db.ExecuteSQL(sb.ToString(), false);
                            }
                            catch (Exception ex)
                            {
                                db.ExRollbackTransaction();
                                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdatePaymentBalance(Update Payment Balance)", ex);
                                return CLASS_NM + ".UpdatePaymentBalance(Update Payment Balance) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
                            }

                            #endregion

                        }
                    }
                }
                catch (Exception ex)
                {
                    db.ExRollbackTransaction();
                    CommonUtl.ExLogger.Error(CLASS_NM + ".UpdatePaymentBalance(Insert)", ex);
                    return CLASS_NM + ".UpdatePaymentBalance(Insert) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
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
            //    CommonUtl.ExLogger.Error(CLASS_NM + ".UpdatePaymentBalance(DelLockPg)", ex);
            //    return CLASS_NM + ".UpdatePaymentBalance(DelLockPg) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
            //}

            #endregion

            #region CommitTransaction

            try
            {
                db.ExCommitTransaction();
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdatePaymentBalance(CommitTransaction)", ex);
                return CLASS_NM + ".UpdatePaymentBalance(CommitTransaction) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
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
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdatePaymentBalance(DbClose)", ex);
                return CLASS_NM + ".UpdatePaymentBalance(DbClose) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
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
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdatePaymentBalance(Add Evidence)", ex);
                return CLASS_NM + ".UpdatePaymentBalance(Add Evidence) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
            }

            #endregion

            return "";

        }

        #endregion

    }
}
