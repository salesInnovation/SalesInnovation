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
    public class svcStockInventory
    {
        private const string CLASS_NM = "svcStockInventory";
        private readonly string PG_NM = DataPgEvidence.PGName.StockInventory.StockInventoryInp;

        #region データ取得

        /// <summary>　
        /// リスト取得
        /// </summary>
        /// <param name="strWhereSql"></param>
        /// <param name="strOrderBySql"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebMethod(EnableSession = true)]
        public List<EntityStockInventory> GetStockInventoryList(string random, string strWhereSql, string strOrderBySql)
        {
            List<EntityStockInventory> entityList = new List<EntityStockInventory>();

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
                    EntityStockInventory entity = new EntityStockInventory();
                    entity.MESSAGE = _message;
                    entityList.Add(entity);
                    return entityList;
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetStockInventoryList(認証処理)", ex);
                EntityStockInventory entity = new EntityStockInventory();
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
                sb.Append(rptMgr.GetStockInventoryListReportSQL(companyId, groupId, strWhereSql, strOrderBySql));

                dt = db.GetDataTable(sb.ToString());

                if (dt.DefaultView.Count > 0)
                {
                    for (int i = 0; i <= dt.DefaultView.Count - 1; i++)
                    {
                        #region Set Entity

                        EntityStockInventory entity = new EntityStockInventory();
                        entity.commodity_id = ExCast.zCStr(dt.DefaultView[i]["COMMODITY_ID"]);
                        entity.commodity_name = ExCast.zCStr(dt.DefaultView[i]["COMMODITY_NAME"]);
                        entity.account_inventory_number = ExCast.zCDbl(dt.DefaultView[i]["INVENTORY_NUMBER"]);
                        entity.practice_inventory_number = ExCast.zCDbl(dt.DefaultView[i]["INVENTORY_NUMBER"]);
                        entity.diff_number = 0;

                        entity.exec_flg = false;

                        entity.lock_flg = 0;

                        entityList.Add(entity);

                        #endregion

                    }
                }

            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetStockInventoryList", ex);
                entityList.Clear();
                EntityStockInventory entity = new EntityStockInventory();
                entity.MESSAGE = CLASS_NM + ".GetStockInventoryList : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message.ToString();
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
                                       DataPgEvidence.PGName.StockInventory.StockInventoryInp,
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
        public string UpdateStockInventory(string random, int type, string ymd, List<EntityStockInventory> entity)
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
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateStockInventory(認証処理)", ex);
                return CLASS_NM + ".UpdateStockInventory : 認証処理に失敗しました。" + Environment.NewLine + ex.Message.ToString();
            }

            #endregion

            #region Field

            StringBuilder sb = new StringBuilder();
            DataTable dt;
            ExMySQLData db = null;
            string _Id = "";
            int _classKbn = 0;

            long rec_cnt = 0;
            string str_message = "";

            EntityInOutDeliveryH _entityInOutDeliveryH = new EntityInOutDeliveryH();
            List<EntityInOutDeliveryD> _entityInOutDeliveryListD_Plus = new List<EntityInOutDeliveryD>();
            List<EntityInOutDeliveryD> _entityInOutDeliveryListD_Minus = new List<EntityInOutDeliveryD>();

            #endregion

            #region Databese Open

            try
            {
                db = new ExMySQLData(ExCast.zCStr(HttpContext.Current.Session[ExSession.DB_CONNECTION_STR]));
                db.DbOpen();
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateStockInventory(DbOpen)", ex);
                return CLASS_NM + ".UpdateStockInventory(DbOpen) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
            }

            #endregion

            #region BeginTransaction

            try
            {
                db.ExBeginTransaction();
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateStockInventory(BeginTransaction)", ex);
                return CLASS_NM + ".UpdateStockInventory(BeginTransaction) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
            }

            #endregion

            #region Update

            if (type == 0)
            {
                try
                {
                    for (int i = 0; i <= entity.Count - 1; i++)
                    {
                        if (entity[i].exec_flg == true && entity[i].diff_number != 0)
                        {
                            #region Update Commodity Inventory

                            try
                            {
                                DataCommodityInventory.UpdCommodityInventory(companyId,
                                                                             groupId,
                                                                             db,
                                                                             ExCast.zNumZeroNothingFormat(entity[i].commodity_id),
                                                                             entity[i].diff_number * -1,
                                                                             PG_NM,
                                                                             ExCast.zCInt(personId),
                                                                             ipAdress,
                                                                             userId);
                            }
                            catch (Exception ex)
                            {
                                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateSales(Update Commodity Inventory)", ex);
                                return "UpdateSales(Update Commodity Inventory) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
                            }

                            #endregion

                            #region Set Entity InOutDelivery

                            rec_cnt += 1;

                            EntityInOutDeliveryD _entityInOutDeliveryD = new EntityInOutDeliveryD();
                            _entityInOutDeliveryD.rec_no = rec_cnt;
                            _entityInOutDeliveryD.commodity_id = entity[i].commodity_id;
                            _entityInOutDeliveryD.commodity_name = entity[i].commodity_name;
                            _entityInOutDeliveryD.unit_id = 0;
                            _entityInOutDeliveryD.enter_number = 0;
                            _entityInOutDeliveryD.case_number = 0;

                            if (entity[i].diff_number > 0)
                            {
                                _entityInOutDeliveryD.number = entity[i].diff_number;
                                _entityInOutDeliveryListD_Minus.Add(_entityInOutDeliveryD);
                            }
                            else
                            {
                                _entityInOutDeliveryD.number = entity[i].diff_number * - 1;
                                _entityInOutDeliveryListD_Plus.Add(_entityInOutDeliveryD);
                            }

                            #endregion

                        }
                    }

                    #region Update InOutDelivery

                    try
                    {
                        
                        svcInOutDelivery _svcInOutDelivery = new svcInOutDelivery();

                        if (_entityInOutDeliveryListD_Minus.Count > 0)
                        {
                            _entityInOutDeliveryH.in_out_delivery_ymd = ymd;
                            _entityInOutDeliveryH.in_out_delivery_kbn = 2;          // 入出庫区分：出庫
                            _entityInOutDeliveryH.in_out_delivery_proc_kbn = 4;     // 入出庫処理区分：棚卸
                            _entityInOutDeliveryH.in_out_delivery_to_kbn = 1;       // 入出庫先区分：グループ
                            _entityInOutDeliveryH.update_person_id = ExCast.zCInt(personId);
                            _entityInOutDeliveryH.group_id_to = groupId;

                            for (int i = 0; i <= _entityInOutDeliveryListD_Minus.Count - 1; i++)
                            {
                                _entityInOutDeliveryH.sum_enter_number += _entityInOutDeliveryListD_Minus[i].enter_number;
                                _entityInOutDeliveryH.sum_case_number += _entityInOutDeliveryListD_Minus[i].case_number;
                                _entityInOutDeliveryH.sum_number += _entityInOutDeliveryListD_Minus[i].number;
                            }

                            // random
                            // update type     1:Insert
                            // procKbn         4:棚卸
                            // InOutDeliveryNo 入出庫番号
                            // CauseNo         元伝票番号
                            str_message = _svcInOutDelivery.UpdateInOutDeliveryExcExc(random,
                                                                                      1,
                                                                                      4,
                                                                                      0,
                                                                                      0,
                                                                                      _entityInOutDeliveryH,
                                                                                      _entityInOutDeliveryListD_Minus,
                                                                                      null,
                                                                                      null);
                            if (str_message.IndexOf("Auto Insert success : ") == -1 && !string.IsNullOrEmpty(str_message))
                            {
                                return "UpdateStockInventory(Update InOutDelivery) : " + str_message;
                            }
                        }

                        _svcInOutDelivery = new svcInOutDelivery();

                        _entityInOutDeliveryH = null;
                        _entityInOutDeliveryH = new EntityInOutDeliveryH();
                        if (_entityInOutDeliveryListD_Plus.Count > 0)
                        {
                            _entityInOutDeliveryH.in_out_delivery_ymd = ymd;
                            _entityInOutDeliveryH.in_out_delivery_kbn = 1;          // 入出庫区分：入庫
                            _entityInOutDeliveryH.in_out_delivery_proc_kbn = 4;     // 入出庫処理区分：棚卸
                            _entityInOutDeliveryH.in_out_delivery_to_kbn = 1;       // 入出庫先区分：グループ
                            _entityInOutDeliveryH.update_person_id = ExCast.zCInt(personId);
                            _entityInOutDeliveryH.group_id_to = groupId;

                            for (int i = 0; i <= _entityInOutDeliveryListD_Plus.Count - 1; i++)
                            {
                                _entityInOutDeliveryH.sum_enter_number += _entityInOutDeliveryListD_Plus[i].enter_number;
                                _entityInOutDeliveryH.sum_case_number += _entityInOutDeliveryListD_Plus[i].case_number;
                                _entityInOutDeliveryH.sum_number += _entityInOutDeliveryListD_Plus[i].number;
                            }

                            // random
                            // update type     1:Insert
                            // procKbn         4:棚卸
                            // InOutDeliveryNo 入出庫番号
                            // CauseNo         元伝票番号
                            str_message = _svcInOutDelivery.UpdateInOutDeliveryExcExc(random,
                                                                                      1,
                                                                                      4,
                                                                                      0,
                                                                                      0,
                                                                                      _entityInOutDeliveryH,
                                                                                      _entityInOutDeliveryListD_Plus,
                                                                                      null,
                                                                                      null);
                            if (str_message.IndexOf("Auto Insert success : ") == -1 && !string.IsNullOrEmpty(str_message))
                            {
                                return "UpdateStockInventory(Update InOutDelivery) : " + str_message;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        db.ExRollbackTransaction();
                        CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateStockInventory(Update InOutDelivery)", ex);
                        return "UpdateStockInventory(Update InOutDelivery) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
                    }

                    #endregion


                }
                catch (Exception ex)
                {
                    db.ExRollbackTransaction();
                    CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateStockInventory(Insert)", ex);
                    return CLASS_NM + ".UpdateStockInventory(Insert) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
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
            //    CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateStockInventory(DelLockPg)", ex);
            //    return CLASS_NM + ".UpdateStockInventory(DelLockPg) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
            //}

            #endregion

            #region CommitTransaction

            try
            {
                db.ExCommitTransaction();
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateStockInventory(CommitTransaction)", ex);
                return CLASS_NM + ".UpdateStockInventory(CommitTransaction) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
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
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateStockInventory(DbClose)", ex);
                return CLASS_NM + ".UpdateStockInventory(DbClose) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
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
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateStockInventory(Add Evidence)", ex);
                return CLASS_NM + ".UpdateStockInventory(Add Evidence) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
            }

            #endregion

            return "";

        }

        #endregion

    }
}
