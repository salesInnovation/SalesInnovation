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
    public class svcInOutDelivery
    {
        private const string CLASS_NM = "svcInOutDelivery";
        private readonly string PG_NM = DataPgEvidence.PGName.InOutDeliver.InOutDeliverInp;

        #region データ取得

        /// <summary> 
        /// ヘッダリスト取得
        /// </summary>
        /// <param name="IDFrom"></param>
        /// <param name="IDTo"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebMethod(EnableSession = true)]
        public EntityInOutDeliveryH GetInOutDeliveryH(string random, long IDFrom, long IDTo)
        {

            EntityInOutDeliveryH entity = null;

            #region 認証処理

            string companyId = "";
            string groupId = "";
            string userId = "";
            string ipAdress = "";
            string sessionString = "";
            int idFigureCustomer = 0;
            int idFigurePurchase = 0;
            try
            {
                companyId = ExCast.zCStr(HttpContext.Current.Session[ExSession.COMPANY_ID]);
                groupId = ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_ID]);
                userId = ExCast.zCStr(HttpContext.Current.Session[ExSession.USER_ID]);
                ipAdress = ExCast.zCStr(HttpContext.Current.Session[ExSession.IP_ADRESS]);
                sessionString = ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]);
                idFigureCustomer = ExCast.zCInt(HttpContext.Current.Session[ExSession.ID_FIGURE_CUSTOMER]);
                idFigurePurchase = ExCast.zCInt(HttpContext.Current.Session[ExSession.ID_FIGURE_PURCHASE]);

                string _message = ExSession.SessionUserUniqueCheck(random, ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]), ExCast.zCInt(HttpContext.Current.Session[ExSession.USER_ID]));
                if (_message != "")
                {
                    entity = new EntityInOutDeliveryH();
                    entity.message = _message;
                    return entity;
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetInOutDeliveryH(認証処理)", ex);
                entity = new EntityInOutDeliveryH();
                entity.message = CLASS_NM + ".GetInOutDeliveryH : 認証処理に失敗しました。" + Environment.NewLine + ex.Message.ToString();
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
                sb.Append("      ,date_format(T.IN_OUT_DELIVERY_YMD , " + ExEscape.SQL_YMD + ") AS IN_OUT_DELIVERY_YMD" + Environment.NewLine);

                sb.Append("      ,T.IN_OUT_DELIVERY_KBN " + Environment.NewLine);
                sb.Append("      ,NM1.DESCRIPTION AS IN_OUT_DELIVERY_KBN_NM " + Environment.NewLine);

                sb.Append("      ,T.IN_OUT_DELIVERY_PROC_KBN " + Environment.NewLine);
                sb.Append("      ,NM2.DESCRIPTION AS IN_OUT_DELIVERY_PROC_KBN_NM " + Environment.NewLine);

                sb.Append("      ,T.CAUSE_NO " + Environment.NewLine);

                sb.Append("      ,T.PERSON_ID " + Environment.NewLine);
                sb.Append("      ,PS.NAME AS UPDATE_PERSON_NM " + Environment.NewLine);

                sb.Append("      ,T.IN_OUT_DELIVERY_TO_KBN " + Environment.NewLine);
                sb.Append("      ,NM3.DESCRIPTION AS IN_OUT_DELIVERY_TO_KBN_NM " + Environment.NewLine);

                sb.Append("      ,T.GROUP_ID_TO " + Environment.NewLine);
                sb.Append("      ,SG.NAME AS GROUP_ID_TO_NM " + Environment.NewLine);

                sb.Append("      ,T.CUSTOMER_ID " + Environment.NewLine);
                sb.Append("      ,CS.NAME AS CUSTOMER_NM " + Environment.NewLine);

                sb.Append("      ,T.PURCHASE_ID " + Environment.NewLine);
                sb.Append("      ,PU.NAME AS PURCHASE_NM " + Environment.NewLine);

                sb.Append("      ,T.SUM_ENTER_NUMBER " + Environment.NewLine);
                sb.Append("      ,T.SUM_CASE_NUMBER " + Environment.NewLine);
                sb.Append("      ,T.SUM_NUMBER " + Environment.NewLine);

                sb.Append("      ,T.MEMO " + Environment.NewLine);
                sb.Append("      ,date_format(T.UPDATE_DATE , " + ExEscape.SQL_YMD + ") AS UPDATE_DATE" + Environment.NewLine);
                sb.Append("      ,T.UPDATE_TIME " + Environment.NewLine);

                sb.Append("  FROM T_IN_OUT_DELIVERY_H AS T" + Environment.NewLine);

                #region Join

                // 更新担当者
                sb.Append("  LEFT JOIN M_PERSON AS PS" + Environment.NewLine);
                sb.Append("    ON PS.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND PS.DISPLAY_FLG = 1 " + Environment.NewLine);
                sb.Append("   AND PS.COMPANY_ID = " + companyId + Environment.NewLine);
                sb.Append("   AND PS.GROUP_ID = " + groupId + Environment.NewLine);
                sb.Append("   AND PS.ID = T.PERSON_ID" + Environment.NewLine);

                // グループ
                sb.Append("  LEFT JOIN SYS_M_COMPANY_GROUP AS SG" + Environment.NewLine);
                sb.Append("    ON SG.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND SG.DISPLAY_FLG = 1 " + Environment.NewLine);
                sb.Append("   AND SG.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
                sb.Append("   AND SG.ID = T.GROUP_ID_TO" + Environment.NewLine);

                // 得意先
                sb.Append("  LEFT JOIN M_CUSTOMER AS CS" + Environment.NewLine);
                sb.Append("    ON CS.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND CS.DISPLAY_FLG = 1 " + Environment.NewLine);
                sb.Append("   AND CS.COMPANY_ID = " + companyId + Environment.NewLine);
                sb.Append("   AND T.CUSTOMER_ID = CS.ID" + Environment.NewLine);

                // 仕入先
                sb.Append("  LEFT JOIN M_PURCHASE AS PU" + Environment.NewLine);
                sb.Append("    ON PU.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND PU.DISPLAY_FLG = 1 " + Environment.NewLine);
                sb.Append("   AND PU.COMPANY_ID = " + companyId + Environment.NewLine);
                sb.Append("   AND T.PURCHASE_ID = PU.ID" + Environment.NewLine);

                // 入出庫区分
                sb.Append("  LEFT JOIN SYS_M_NAME AS NM1" + Environment.NewLine);
                sb.Append("    ON NM1.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND NM1.DISPLAY_FLG = 1 " + Environment.NewLine);
                sb.Append("   AND NM1.DIVISION_ID = " + (int)CommonUtl.geNameKbn.IN_OUT_DELIVERY_KBN + Environment.NewLine);
                sb.Append("   AND T.IN_OUT_DELIVERY_KBN = NM1.ID" + Environment.NewLine);

                // 入出庫処理区分
                sb.Append("  LEFT JOIN SYS_M_NAME AS NM2" + Environment.NewLine);
                sb.Append("    ON NM2.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND NM2.DISPLAY_FLG = 1 " + Environment.NewLine);
                sb.Append("   AND NM2.DIVISION_ID = " + (int)CommonUtl.geNameKbn.IN_OUT_DELIVERY_PROC_KBN + Environment.NewLine);
                sb.Append("   AND T.IN_OUT_DELIVERY_PROC_KBN = NM2.ID" + Environment.NewLine);

                // 入出庫先区分
                sb.Append("  LEFT JOIN SYS_M_NAME AS NM3" + Environment.NewLine);
                sb.Append("    ON NM3.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND NM3.DISPLAY_FLG = 1 " + Environment.NewLine);
                sb.Append("   AND NM3.DIVISION_ID = " + (int)CommonUtl.geNameKbn.IN_OUT_DELIVERY_TO_KBN + Environment.NewLine);
                sb.Append("   AND T.IN_OUT_DELIVERY_TO_KBN = NM3.ID" + Environment.NewLine);

                #endregion

                sb.Append(" WHERE T.COMPANY_ID = " + companyId + Environment.NewLine);
                sb.Append("   AND T.GROUP_ID = " + groupId + Environment.NewLine);
                sb.Append("   AND T.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND T.NO >= " + IDFrom.ToString() + Environment.NewLine);
                sb.Append("   AND T.NO <= " + IDTo.ToString() + Environment.NewLine);

                sb.Append(" ORDER BY T.COMPANY_ID " + Environment.NewLine);
                sb.Append("         ,T.GROUP_ID " + Environment.NewLine);
                sb.Append("         ,T.IN_OUT_DELIVERY_YMD DESC " + Environment.NewLine);
                sb.Append("         ,T.NO DESC " + Environment.NewLine);
                sb.Append(" LIMIT 0, 1");

                #endregion

                dt = db.GetDataTable(sb.ToString());

                if (dt.DefaultView.Count > 0)
                {
                    entity = new EntityInOutDeliveryH();

                    // 排他制御
                    DataPgLock.geLovkFlg lockFlg = DataPgLock.geLovkFlg.UnLock;
                    string strErr = DataPgLock.SetLockPg(companyId, userId, PG_NM, groupId + "-" + ExCast.zNumZeroNothingFormat(IDFrom.ToString()), ipAdress, db, out lockFlg);
                    if (strErr != "")
                    {
                        entity.message = CLASS_NM + ".GetInOutDeliveryH : 排他制御(ロック情報取得)に失敗しました。" + Environment.NewLine + strErr;
                    }

                    #region Set Entity

                    entity.id = ExCast.zCLng(dt.DefaultView[0]["ID"]);
                    entity.no = ExCast.zCLng(dt.DefaultView[0]["NO"]);
                    entity.in_out_delivery_ymd = ExCast.zDateNullToDefault(dt.DefaultView[0]["IN_OUT_DELIVERY_YMD"]);
                    entity.in_out_delivery_kbn = ExCast.zCInt(dt.DefaultView[0]["IN_OUT_DELIVERY_KBN"]);
                    entity.in_out_delivery_kbn_nm = ExCast.zCStr(dt.DefaultView[0]["IN_OUT_DELIVERY_KBN_NM"]);

                    entity.in_out_delivery_proc_kbn = ExCast.zCInt(dt.DefaultView[0]["IN_OUT_DELIVERY_PROC_KBN"]);
                    entity.in_out_delivery_proc_kbn_nm = ExCast.zCStr(dt.DefaultView[0]["IN_OUT_DELIVERY_PROC_KBN_NM"]);

                    entity.cause_no = ExCast.zCLng(dt.DefaultView[0]["CAUSE_NO"]);

                    entity.update_person_id = ExCast.zCInt(dt.DefaultView[0]["PERSON_ID"]);
                    entity.update_person_nm = ExCast.zCStr(dt.DefaultView[0]["UPDATE_PERSON_NM"]);

                    entity.in_out_delivery_to_kbn = ExCast.zCInt(dt.DefaultView[0]["IN_OUT_DELIVERY_TO_KBN"]);
                    entity.in_out_delivery_to_kbn_nm = ExCast.zCStr(dt.DefaultView[0]["IN_OUT_DELIVERY_TO_KBN_NM"]);

                    entity.group_id_to = string.Format("{0:000}", ExCast.zCInt(dt.DefaultView[0]["GROUP_ID_TO"]));
                    entity.group_id_to_nm = ExCast.zCStr(dt.DefaultView[0]["GROUP_ID_TO_NM"]);

                    entity.customer_id = ExCast.zFormatForID(dt.DefaultView[0]["CUSTOMER_ID"], idFigureCustomer);
                    entity.customer_name = ExCast.zCStr(dt.DefaultView[0]["CUSTOMER_NM"]);

                    entity.purchase_id = ExCast.zFormatForID(dt.DefaultView[0]["PURCHASE_ID"], idFigurePurchase);
                    entity.purchase_name = ExCast.zCStr(dt.DefaultView[0]["PURCHASE_NM"]);

                    entity.sum_enter_number = ExCast.zCDbl(dt.DefaultView[0]["SUM_ENTER_NUMBER"]);
                    entity.sum_case_number = ExCast.zCDbl(dt.DefaultView[0]["SUM_CASE_NUMBER"]);
                    entity.sum_number = ExCast.zCDbl(dt.DefaultView[0]["SUM_NUMBER"]);

                    entity.memo = ExCast.zCStr(dt.DefaultView[0]["MEMO"]);

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
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetInOutDeliveryH", ex);
                entity = new EntityInOutDeliveryH();
                entity.message = CLASS_NM + ".GetInOutDeliveryH : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message.ToString();
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
                                       DataPgEvidence.PGName.InOutDeliver.InOutDeliverInp,
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
        public List<EntityInOutDeliveryD> GetInOutDeliveryListD(string random, long IDFrom, long IDTo)
        {
            List<EntityInOutDeliveryD> entityList = new List<EntityInOutDeliveryD>();

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
                    EntityInOutDeliveryD entity = new EntityInOutDeliveryD();
                    entity.message = _message;
                    entityList.Add(entity);
                    return entityList;
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetInOutDeliveryListD(認証処理)", ex);
                EntityInOutDeliveryD entity = new EntityInOutDeliveryD();
                entity.message = CLASS_NM + ".GetInOutDeliveryListD : 認証処理に失敗しました。" + Environment.NewLine + ex.Message.ToString(); ;
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

                sb.Append("SELECT OD.IN_OUT_DELIVERY_ID " + Environment.NewLine);
                sb.Append("      ,OD.REC_NO " + Environment.NewLine);
                sb.Append("      ,OD.COMMODITY_ID " + Environment.NewLine);
                sb.Append("      ,OD.COMMODITY_NAME " + Environment.NewLine);
                sb.Append("      ,OD.UNIT_ID " + Environment.NewLine);
                sb.Append("      ,NM3.DESCRIPTION AS UNIT_NM " + Environment.NewLine);
                sb.Append("      ,OD.ENTER_NUMBER " + Environment.NewLine);
                sb.Append("      ,OD.CASE_NUMBER " + Environment.NewLine);
                sb.Append("      ,OD.NUMBER " + Environment.NewLine);
                sb.Append("      ,OD.MEMO " + Environment.NewLine);
                sb.Append("      ,CT.INVENTORY_NUMBER " + Environment.NewLine);
                sb.Append("      ,CT.INVENTORY_MANAGEMENT_DIVISION_ID " + Environment.NewLine);
                sb.Append("      ,CT.NUMBER_DECIMAL_DIGIT " + Environment.NewLine);
                sb.Append("  FROM T_IN_OUT_DELIVERY_D AS OD" + Environment.NewLine);

                #region Join

                // 現在庫など
                sb.Append("  LEFT JOIN M_COMMODITY AS CT" + Environment.NewLine);
                sb.Append("    ON CT.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND CT.DISPLAY_FLG = 1 " + Environment.NewLine);
                sb.Append("   AND CT.COMPANY_ID = " + companyId + Environment.NewLine);
                sb.Append("   AND CT.ID = OD.COMMODITY_ID" + Environment.NewLine);

                // 単位
                sb.Append("  LEFT JOIN SYS_M_NAME AS NM3" + Environment.NewLine);
                sb.Append("    ON NM3.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND NM3.DISPLAY_FLG = 1 " + Environment.NewLine);
                sb.Append("   AND NM3.DIVISION_ID = " + (int)CommonUtl.geNameKbn.UNIT_ID + Environment.NewLine);
                sb.Append("   AND OD.UNIT_ID = NM3.ID" + Environment.NewLine);

                #endregion

                sb.Append(" WHERE OD.COMPANY_ID = " + companyId + Environment.NewLine);
                sb.Append("   AND OD.GROUP_ID = " + groupId + Environment.NewLine);
                sb.Append("   AND OD.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND OD.IN_OUT_DELIVERY_ID >= " + IDFrom.ToString() + Environment.NewLine);
                sb.Append("   AND OD.IN_OUT_DELIVERY_ID <= " + IDTo.ToString() + Environment.NewLine);

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
                        EntityInOutDeliveryD entityD = new EntityInOutDeliveryD();

                        #region Set Entity

                        entityD.id = ExCast.zCLng(dt.DefaultView[i]["IN_OUT_DELIVERY_ID"]);
                        entityD.rec_no = ExCast.zCInt(dt.DefaultView[i]["REC_NO"]);
                        entityD.commodity_id = ExCast.zFormatForID(dt.DefaultView[i]["COMMODITY_ID"], idFigureGoods);
                        entityD.commodity_name = ExCast.zCStr(dt.DefaultView[i]["COMMODITY_NAME"]);
                        entityD.unit_id = ExCast.zCInt(dt.DefaultView[i]["UNIT_ID"]);
                        entityD.unit_nm = ExCast.zCStr(dt.DefaultView[i]["UNIT_NM"]);
                        entityD.enter_number = ExCast.zCDbl(dt.DefaultView[i]["ENTER_NUMBER"]);
                        entityD.case_number = ExCast.zCDbl(dt.DefaultView[i]["CASE_NUMBER"]);
                        entityD.number = ExCast.zCDbl(dt.DefaultView[i]["NUMBER"]);
                        entityD.memo = ExCast.zCStr(dt.DefaultView[i]["MEMO"]);
                        entityD.inventory_number = ExCast.zCDbl(dt.DefaultView[i]["INVENTORY_NUMBER"]);
                        entityD.inventory_management_division_id = ExCast.zCInt(dt.DefaultView[i]["INVENTORY_MANAGEMENT_DIVISION_ID"]);
                        entityD.number_decimal_digit = ExCast.zCInt(dt.DefaultView[i]["NUMBER_DECIMAL_DIGIT"]);
                        entityList.Add(entityD);

                        #endregion
                    }
                }

                return entityList;

            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetInOutDeliveryListD", ex);
                entityList.Clear();
                EntityInOutDeliveryD entityD = new EntityInOutDeliveryD();
                entityD.message = CLASS_NM + ".GetInOutDeliveryListD : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message.ToString();
                entityList.Add(entityD);
                return entityList;
            }
            finally
            {
                db = null;
            }

        }

        /// <summary>　
        /// 入出庫リスト取得
        /// </summary>
        /// <param name="strWhereSql"></param>
        /// <param name="strOrderBySql"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebMethod(EnableSession = true)]
        public List<EntityInOutDelivery> GetInOutDeliveryList(string random, string strWhereSql, string strOrderBySql)
        {
            List<EntityInOutDelivery> entityList = new List<EntityInOutDelivery>();

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
                    EntityInOutDelivery entity = new EntityInOutDelivery();
                    entity.MESSAGE = _message;
                    entityList.Add(entity);
                    return entityList;
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetInOutDeliveryList(認証処理)", ex);
                EntityInOutDelivery entity = new EntityInOutDelivery();
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

                sb.Append(rptMgr.GetInOutDeliveryListReportSQL(companyId, groupId, strWhereSql, strOrderBySql));

                dt = db.GetDataTable(sb.ToString());

                if (dt.DefaultView.Count > 0)
                {
                    for (int i = 0; i <= dt.DefaultView.Count - 1; i++)
                    {
                        #region Set Entity

                        EntityInOutDelivery _entityInOutDelivery = new EntityInOutDelivery();
                        _entityInOutDelivery.ID = ExCast.zCLng(dt.DefaultView[i]["ID"]);
                        _entityInOutDelivery.NO = ExCast.zCLng(dt.DefaultView[i]["NO"]);
                        _entityInOutDelivery.CAUSE_NO = ExCast.zCLng(dt.DefaultView[i]["CAUSE_NO"]);
                        _entityInOutDelivery.IN_OUT_DELIVERY_YMD = ExCast.zDateNullToDefault(dt.DefaultView[i]["IN_OUT_DELIVERY_YMD"]);
                        _entityInOutDelivery.IN_OUT_DELIVERY_KBN = ExCast.zCInt(dt.DefaultView[i]["IN_OUT_DELIVERY_KBN"]);
                        _entityInOutDelivery.IN_OUT_DELIVERY_KBN_NM = ExCast.zCStr(dt.DefaultView[i]["IN_OUT_DELIVERY_KBN_NM"]);
                        _entityInOutDelivery.IN_OUT_DELIVERY_PROC_KBN = ExCast.zCInt(dt.DefaultView[i]["IN_OUT_DELIVERY_PROC_KBN"]);
                        _entityInOutDelivery.IN_OUT_DELIVERY_PROC_KBN_NM = ExCast.zCStr(dt.DefaultView[i]["IN_OUT_DELIVERY_PROC_KBN_NM"]);
                        _entityInOutDelivery.IN_OUT_DELIVERY_TO_KBN = ExCast.zCInt(dt.DefaultView[i]["IN_OUT_DELIVERY_TO_KBN"]);
                        _entityInOutDelivery.IN_OUT_DELIVERY_TO_KBN_NM = ExCast.zCStr(dt.DefaultView[i]["IN_OUT_DELIVERY_TO_KBN_NM"]);
                        _entityInOutDelivery.INPUT_PERSON = ExCast.zCInt(dt.DefaultView[i]["INPUT_PERSON"]);
                        _entityInOutDelivery.INPUT_PERSON_NM = ExCast.zCStr(dt.DefaultView[i]["INPUT_PERSON_NM"]);
                        _entityInOutDelivery.INPUT_PERSON = ExCast.zCInt(dt.DefaultView[i]["CUSTOMER_ID"]);
                        _entityInOutDelivery.INPUT_PERSON_NM = ExCast.zCStr(dt.DefaultView[i]["INPUT_PERSON_NM"]);

                        // 入出庫区分
                        switch (_entityInOutDelivery.IN_OUT_DELIVERY_KBN)
                        {
                            case 1:     // 入庫
                                // 入出庫先区分
                                switch (_entityInOutDelivery.IN_OUT_DELIVERY_TO_KBN)
                                {
                                    case 1:     // グループ
                                        _entityInOutDelivery.IN_OUT_DELIVERY_TO_ID = ExCast.zCStr(dt.DefaultView[i]["GROUP_ID_TO"]);
                                        _entityInOutDelivery.IN_OUT_DELIVERY_TO_NM = ExCast.zCStr(dt.DefaultView[i]["GROUP_ID_TO_NM"]);
                                        break;
                                    case 2:     // 仕入先
                                        _entityInOutDelivery.IN_OUT_DELIVERY_TO_ID = ExCast.zFormatForID(dt.DefaultView[0]["PURCHASE_ID"], idFigurePurchase);
                                        _entityInOutDelivery.IN_OUT_DELIVERY_TO_NM = ExCast.zCStr(dt.DefaultView[i]["PURCHASE_NM"]);
                                        break;
                                    default:
                                        _entityInOutDelivery.IN_OUT_DELIVERY_TO_ID = "";
                                        _entityInOutDelivery.IN_OUT_DELIVERY_TO_NM = "";
                                        break;
                                }
                                break;
                            case 2:     // 出庫
                                // 入出庫先区分
                                switch (_entityInOutDelivery.IN_OUT_DELIVERY_TO_KBN)
                                {
                                    case 1:     // グループ
                                        _entityInOutDelivery.IN_OUT_DELIVERY_TO_ID = ExCast.zCStr(dt.DefaultView[i]["GROUP_ID_TO"]);
                                        _entityInOutDelivery.IN_OUT_DELIVERY_TO_NM = ExCast.zCStr(dt.DefaultView[i]["GROUP_ID_TO_NM"]);
                                        break;
                                    case 2:     // 得意先
                                        _entityInOutDelivery.IN_OUT_DELIVERY_TO_ID = ExCast.zFormatForID(dt.DefaultView[0]["CUSTOMER_ID"], idFigureCustomer);
                                        _entityInOutDelivery.IN_OUT_DELIVERY_TO_NM = ExCast.zCStr(dt.DefaultView[i]["CUSTOMER_NM"]);
                                        break;
                                    default:
                                        _entityInOutDelivery.IN_OUT_DELIVERY_TO_ID = "";
                                        _entityInOutDelivery.IN_OUT_DELIVERY_TO_NM = "";
                                        break;
                                }
                                break;
                            default:
                                _entityInOutDelivery.IN_OUT_DELIVERY_TO_ID = "";
                                _entityInOutDelivery.IN_OUT_DELIVERY_TO_NM = "";
                                break;
                        }

                        _entityInOutDelivery.GROUP_ID_TO = ExCast.zCInt(dt.DefaultView[i]["GROUP_ID_TO"]);
                        _entityInOutDelivery.GROUP_ID_TO_NM = ExCast.zCStr(dt.DefaultView[i]["GROUP_ID_TO_NM"]);
                        _entityInOutDelivery.CUSTOMER_ID = ExCast.zFormatForID(dt.DefaultView[0]["CUSTOMER_ID"], idFigureCustomer);
                        _entityInOutDelivery.CUSTOMER_NM = ExCast.zCStr(dt.DefaultView[i]["CUSTOMER_NM"]);
                        _entityInOutDelivery.PURCHASE_ID = ExCast.zFormatForID(dt.DefaultView[0]["PURCHASE_ID"], idFigurePurchase);
                        _entityInOutDelivery.PURCHASE_NAME = ExCast.zCStr(dt.DefaultView[i]["PURCHASE_NM"]);
                        _entityInOutDelivery.SUM_ENTER_NUMBER = ExCast.zCInt(dt.DefaultView[i]["SUM_ENTER_NUMBER"]);
                        _entityInOutDelivery.SUM_CASE_NUMBER = ExCast.zCDbl(dt.DefaultView[i]["SUM_CASE_NUMBER"]);
                        _entityInOutDelivery.SUM_NUMBER = ExCast.zCDbl(dt.DefaultView[i]["SUM_NUMBER"]);
                        _entityInOutDelivery.MEMO = ExCast.zCStr(dt.DefaultView[i]["MEMO"]);
                        _entityInOutDelivery.REC_NO = ExCast.zCInt(dt.DefaultView[i]["REC_NO"]);
                        _entityInOutDelivery.COMMODITY_ID = ExCast.zCStr(dt.DefaultView[i]["COMMODITY_ID"]);
                        _entityInOutDelivery.COMMODITY_NAME = ExCast.zCStr(dt.DefaultView[i]["COMMODITY_NAME"]);
                        _entityInOutDelivery.UNIT_ID = ExCast.zCInt(dt.DefaultView[i]["UNIT_ID"]);
                        _entityInOutDelivery.UNIT_NM = ExCast.zCStr(dt.DefaultView[i]["UNIT_NM"]);
                        _entityInOutDelivery.ENTER_NUMBER = ExCast.zCInt(dt.DefaultView[i]["ENTER_NUMBER"]);
                        _entityInOutDelivery.CASE_NUMBER = ExCast.zCDbl(dt.DefaultView[i]["CASE_NUMBER"]);
                        _entityInOutDelivery.NUMBER = ExCast.zCDbl(dt.DefaultView[i]["NUMBER"]);
                        _entityInOutDelivery.D_MEMO = ExCast.zCStr(dt.DefaultView[i]["D_MEMO"]);

                        entityList.Add(_entityInOutDelivery);

                        #endregion
                    }
                }

            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetInOutDeliveryList", ex);
                entityList.Clear();
                EntityInOutDelivery entity = new EntityInOutDelivery();
                entity.MESSAGE = CLASS_NM + ".GetInOutDeliveryList : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message.ToString();
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
                                       DataPgEvidence.PGName.InOutDeliver.InOutDeliverList,
                                       DataPgEvidence.geOperationType.Select,
                                       "Where:" + strWhereSql + ",Orderby:" + strOrderBySql);

            return entityList;

        }

        #endregion

        #region データ更新

        /// <summary>
        /// 入出庫更新
        /// </summary>
        /// <param name="random">セッションランダム文字列</param>
        /// <param name="type">0:Update 1:Insert 2:Delete</param>
        /// <param name="InOutDeliveryNo">伝票番号</param>
        /// <param name="entityH">ヘッダデータ</param>
        /// <param name="entityD">明細データ</param>
        /// <returns></returns>
        [OperationContract()]
        [WebMethod(EnableSession = true)]
        public string UpdateInOutDelivery(string random, int type, long InOutDeliveryNo, EntityInOutDeliveryH entityH, List<EntityInOutDeliveryD> entityD, EntityInOutDeliveryH before_entityH, List<EntityInOutDeliveryD> before_entityD)
        {

            try
            {
                return UpdateInOutDeliveryExcExc(random, type, 1, InOutDeliveryNo, 0, entityH, entityD, before_entityH, before_entityD);
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateInOutDelivery(UpdateInOutDeliveryExcExc)", ex);
                return "UpdateInOutDelivery(UpdateInOutDeliveryExcExc) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
            }
        }

        [WebMethod(EnableSession = true)]
        public string UpdateInOutDeliveryExcExc(string random, 
                                                int type, 
                                                int procKbn,
                                                long InOutDeliveryNo,
                                                long CauseNo,
                                                EntityInOutDeliveryH entityH,
                                                List<EntityInOutDeliveryD> entityD,
                                                EntityInOutDeliveryH before_entityH,
                                                List<EntityInOutDeliveryD> before_entityD)
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
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateInOutDeliveryExc(認証処理)", ex);
                return CLASS_NM + ".認証処理に失敗しました。" + Environment.NewLine + ex.Message.ToString();
            }

            #endregion

            #region Field

            StringBuilder sb = new StringBuilder();
            DataTable dt;
            ExMySQLData db = null;

            string accountPeriod = "";
            long id = 0;
            long no = InOutDeliveryNo;

            #endregion

            #region Databese Open

            try
            {
                db = new ExMySQLData(ExCast.zCStr(HttpContext.Current.Session[ExSession.DB_CONNECTION_STR]));
                db.DbOpen();
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateInOutDeliveryExc(DbOpen)", ex);
                return "UpdateInOutDeliveryExc(DbOpen) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
            }

            #endregion

            #region BeginTransaction

            try
            {
                db.ExBeginTransaction();
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateInOutDeliveryExc(BeginTransaction)", ex);
                return "UpdateInOutDeliveryExc(BeginTransaction) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
            }

            #endregion

            #region Update or Insert

            if (type <= 1)
            {
                #region Get Accout Period

                try
                {
                    accountPeriod = DataAccount.GetAccountPeriod(ExCast.zCStr(HttpContext.Current.Session[ExSession.ACCOUNT_BEGIN_PERIOD]), entityH.in_out_delivery_ymd);
                    if (accountPeriod == "")
                    {
                        return "会計年の取得に失敗しました。(期首月日 : " + ExCast.zCStr(HttpContext.Current.Session[ExSession.ACCOUNT_BEGIN_PERIOD]) +
                                                             " 入出庫日 : " + entityH.in_out_delivery_ymd + ")";
                    }
                }
                catch (Exception ex)
                {
                    CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateInOutDeliveryExc(GetAccountPeriod)", ex);
                    return "UpdateInOutDeliveryExc(GetAccountPeriod) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
                }

                #endregion

                #region Get Slip No

                try
                {
                    DataSlipNo.GetSlipNo(companyId,
                                         groupId,
                                         db,
                                         DataSlipNo.geSlipKbn.InOutDelivery,
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
                    CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateInOutDeliveryExc(GetSlipNo)", ex);
                    return "UpdateInOutDeliveryExc(GetSlipNo) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
                }

                #endregion

                #region Head Insert

                try
                {
                    #region SQL

                    sb.Length = 0;
                    sb.Append("INSERT INTO T_IN_OUT_DELIVERY_H " + Environment.NewLine);
                    sb.Append("       ( COMPANY_ID" + Environment.NewLine);
                    sb.Append("       , GROUP_ID" + Environment.NewLine);
                    sb.Append("       , ID" + Environment.NewLine);
                    sb.Append("       , NO" + Environment.NewLine);
                    sb.Append("       , CAUSE_NO" + Environment.NewLine);
                    sb.Append("       , IN_OUT_DELIVERY_YMD" + Environment.NewLine);
                    sb.Append("       , IN_OUT_DELIVERY_KBN" + Environment.NewLine);
                    sb.Append("       , IN_OUT_DELIVERY_PROC_KBN" + Environment.NewLine);
                    sb.Append("       , PERSON_ID" + Environment.NewLine);
                    sb.Append("       , IN_OUT_DELIVERY_TO_KBN" + Environment.NewLine);
                    sb.Append("       , GROUP_ID_TO" + Environment.NewLine);
                    sb.Append("       , CUSTOMER_ID" + Environment.NewLine);
                    sb.Append("       , PURCHASE_ID" + Environment.NewLine);
                    sb.Append("       , SUM_ENTER_NUMBER" + Environment.NewLine);
                    sb.Append("       , SUM_CASE_NUMBER" + Environment.NewLine);
                    sb.Append("       , SUM_NUMBER" + Environment.NewLine);
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
                    sb.Append("       ," + CauseNo.ToString() + Environment.NewLine);                                           // CAUSE_NO
                    sb.Append("       ," + ExEscape.zRepStr(ExCast.zDateNullToDefault(entityH.in_out_delivery_ymd)) + Environment.NewLine); // IN_OUT_DELIVERY_YMD
                    sb.Append("       ," + entityH.in_out_delivery_kbn + Environment.NewLine);                                              // IN_OUT_DELIVERY_KBN
                    sb.Append("       ," + procKbn.ToString() + Environment.NewLine);                                                       // IN_OUT_DELIVERY_PROC_KBN
                    sb.Append("       ," + entityH.update_person_id + Environment.NewLine);                                                 // PERSON_ID
                    sb.Append("       ," + entityH.in_out_delivery_to_kbn + Environment.NewLine);                                           // IN_OUT_DELIVERY_TO_KBN
                    sb.Append("       ," + ExCast.zCInt(entityH.group_id_to) + Environment.NewLine);                                        // GROUP_ID_TO
                    sb.Append("       ," + ExEscape.zRepStr(ExCast.zNumZeroNothingFormat(entityH.customer_id)) + Environment.NewLine);      // CUSTOMER_ID
                    sb.Append("       ," + ExEscape.zRepStr(ExCast.zNumZeroNothingFormat(entityH.purchase_id)) + Environment.NewLine);      // PURCHASE_ID
                    sb.Append("       ," + ExCast.zCDbl(entityH.sum_enter_number) + Environment.NewLine);                                   // SUM_ENTER_NUMBER
                    sb.Append("       ," + ExCast.zCDbl(entityH.sum_case_number) + Environment.NewLine);                                    // SUM_CASE_NUMBER
                    sb.Append("       ," + ExCast.zCDbl(entityH.sum_number) + Environment.NewLine);                                         // SUM_NUMBER
                    sb.Append("       ," + ExEscape.zRepStr(entityH.memo) + Environment.NewLine);                                           // MEMO
                    if (type == 0)
                    {
                        sb.Append(CommonUtl.GetInsSQLCommonColums(CommonUtl.UpdKbn.Upd,
                                                                  PG_NM,
                                                                  "T_IN_OUT_DELIVERY_H",
                                                                  entityH.update_person_id,
                                                                  InOutDeliveryNo.ToString(),
                                                                  ipAdress,
                                                                  userId));
                    }
                    else
                    {
                        sb.Append(CommonUtl.GetInsSQLCommonColums(CommonUtl.UpdKbn.Ins,
                                                                  PG_NM,
                                                                  "T_IN_OUT_DELIVERY_H",
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
                    CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateInOutDeliveryExc(Head Insert)", ex);
                    return "UpdateInOutDeliveryExc(Head Insert) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
                }

                #endregion

                #region Detail Insert

                try
                {
                    for (int i = 0; i <= entityD.Count - 1; i++)
                    {
                        if (!string.IsNullOrEmpty(entityD[i].commodity_id))
                        {
                            #region SQL

                            sb.Length = 0;
                            sb.Append("INSERT INTO T_IN_OUT_DELIVERY_D " + Environment.NewLine);
                            sb.Append("       ( COMPANY_ID" + Environment.NewLine);
                            sb.Append("       , GROUP_ID" + Environment.NewLine);
                            sb.Append("       , IN_OUT_DELIVERY_ID" + Environment.NewLine);
                            sb.Append("       , REC_NO" + Environment.NewLine);
                            sb.Append("       , COMMODITY_ID" + Environment.NewLine);
                            sb.Append("       , COMMODITY_NAME" + Environment.NewLine);
                            sb.Append("       , UNIT_ID" + Environment.NewLine);
                            sb.Append("       , ENTER_NUMBER" + Environment.NewLine);
                            sb.Append("       , CASE_NUMBER" + Environment.NewLine);
                            sb.Append("       , NUMBER" + Environment.NewLine);
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
                            sb.Append("       ," + id.ToString() + Environment.NewLine);                                                    // IN_OUT_DELIVERY_ID
                            sb.Append("       ," + entityD[i].rec_no + Environment.NewLine);                                                // REC_NO
                            sb.Append("       ," + ExEscape.zRepStr(ExCast.zNumZeroNothingFormat(entityD[i].commodity_id)) + Environment.NewLine);     // COMMODITY_ID
                            sb.Append("       ," + ExEscape.zRepStr(entityD[i].commodity_name) + Environment.NewLine);                                 // COMMODITY_NAME
                            sb.Append("       ," + entityD[i].unit_id + Environment.NewLine);                                               // UNIT_ID
                            sb.Append("       ," + entityD[i].enter_number + Environment.NewLine);                                          // ENTER_NUMBER
                            sb.Append("       ," + entityD[i].case_number + Environment.NewLine);                                           // CASE_NUMBER
                            sb.Append("       ," + entityD[i].number + Environment.NewLine);                                                // NUMBER
                            sb.Append("       ," + ExEscape.zRepStr(entityD[i].memo) + Environment.NewLine);                                // MEMO

                            if (type == 0)
                            {
                                sb.Append(CommonUtl.GetInsSQLCommonColums(CommonUtl.UpdKbn.Upd,
                                                                          PG_NM,
                                                                          "T_IN_OUT_DELIVERY_D",
                                                                          entityH.update_person_id,
                                                                          ExCast.zCStr(entityH.id),
                                                                          ipAdress,
                                                                          userId));
                            }
                            else
                            {
                                sb.Append(CommonUtl.GetInsSQLCommonColums(CommonUtl.UpdKbn.Ins,
                                                                          PG_NM,
                                                                          "T_IN_OUT_DELIVERY_D",
                                                                          entityH.update_person_id,
                                                                          "0",
                                                                          ipAdress,
                                                                          userId));
                            }

                            #endregion

                            db.ExecuteSQL(sb.ToString(), false);

                            #region Update Commodity Inventory

                            // 入出庫時
                            if (procKbn == 1)
                            {
                                try
                                {
                                    double _number = entityD[i].number;
                                    if (entityH.in_out_delivery_kbn == 2)
                                    {
                                        // 出庫時
                                        _number = _number * -1;
                                    }
                                    if (entityD[i].inventory_management_division_id == 1)
                                    {
                                        DataCommodityInventory.UpdCommodityInventory(companyId,
                                                                                     groupId,
                                                                                     db,
                                                                                     ExCast.zNumZeroNothingFormat(entityD[i].commodity_id),
                                                                                     _number,
                                                                                     PG_NM,
                                                                                     ExCast.zCInt(entityH.update_person_id),
                                                                                     ipAdress,
                                                                                     userId);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateInOutDeliveryExc(Update Commodity Inventory)", ex);
                                    return "UpdateInOutDeliveryExc(Update Commodity Inventory) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
                                }
                            }

                            #endregion
                        }
                    }
                }
                catch (Exception ex)
                {
                    db.ExRollbackTransaction();
                    CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateInOutDeliveryExc(Detail Insert)", ex);
                    return "UpdateInOutDeliveryExc(Detail Insert) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
                }

                #endregion

                #region Head Update

                if (type == 0)
                {
                    try
                    {
                        sb.Length = 0;
                        sb.Append("UPDATE T_IN_OUT_DELIVERY_H " + Environment.NewLine);
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
                        CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateInOutDeliveryExc(Head Update)", ex);
                        return "UpdateInOutDeliveryExc(Head Update) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
                    }
                }

                #endregion

                #region Detail Update

                if (type == 0)
                {
                    try
                    {
                        sb.Length = 0;
                        sb.Append("UPDATE T_IN_OUT_DELIVERY_D " + Environment.NewLine);
                        sb.Append(CommonUtl.GetUpdSQLCommonColums(PG_NM,
                                                                  entityH.update_person_id,
                                                                  ipAdress,
                                                                  userId,
                                                                  1));
                        sb.Append(" WHERE COMPANY_ID = " + companyId + Environment.NewLine);
                        sb.Append("   AND GROUP_ID = " + groupId + Environment.NewLine);
                        sb.Append("   AND IN_OUT_DELIVERY_ID = " + entityH.id.ToString() + Environment.NewLine);

                        db.ExecuteSQL(sb.ToString(), false);

                    }
                    catch (Exception ex)
                    {
                        db.ExRollbackTransaction();
                        CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateInOutDeliveryExc(Detail Update)", ex);
                        return "UpdateInOutDeliveryExc(Detail Update) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
                    }
                }

                #endregion

                #region Update Slip No

                try
                {
                    if (type == 0)
                    {
                        DataSlipNo.UpdateSlipNo(companyId, groupId, db, DataSlipNo.geSlipKbn.InOutDelivery, accountPeriod, 0, id);
                    }
                    else
                    {
                        DataSlipNo.UpdateSlipNo(companyId, groupId, db, DataSlipNo.geSlipKbn.InOutDelivery, accountPeriod, no, id);
                    }

                    if (no == 0 || id == 0)
                    {
                        return "伝票番号の更新に失敗しました。(会計年 : " + accountPeriod + ")";
                    }
                }
                catch (Exception ex)
                {
                    CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateInOutDeliveryExc(UpdateSlipNo)", ex);
                    return "UpdateInOutDeliveryExc(UpdateSlipNo) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
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
                    sb.Append("UPDATE T_IN_OUT_DELIVERY_H " + Environment.NewLine);
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
                    CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateInOutDeliveryExc(Head Delete)", ex);
                    return "UpdateInOutDeliveryExc(Head Update) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
                }

                #endregion

                #region Detail Delete

                try
                {
                    sb.Length = 0;
                    sb.Append("UPDATE T_IN_OUT_DELIVERY_D " + Environment.NewLine);
                    sb.Append(CommonUtl.GetUpdSQLCommonColums(PG_NM,
                                                              entityH.update_person_id,
                                                              ipAdress,
                                                              userId,
                                                              1));
                    sb.Append(" WHERE COMPANY_ID = " + companyId + Environment.NewLine);
                    sb.Append("   AND GROUP_ID = " + groupId + Environment.NewLine);
                    sb.Append("   AND IN_OUT_DELIVERY_ID = " + entityH.id.ToString() + Environment.NewLine);

                    db.ExecuteSQL(sb.ToString(), false);

                }
                catch (Exception ex)
                {
                    db.ExRollbackTransaction();
                    CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateInOutDeliveryExc(Detail Delete)", ex);
                    return "UpdateInOutDeliveryExc(Detail Update) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
                }

                #endregion
            }

            #endregion

            #region Update Commodity Inventory

            // 入出庫時
            if (procKbn == 1)
            {
                try
                {
                    // Update or Delete
                    if (type == 0 || type == 2)
                    {

                        for (int i = 0; i <= before_entityD.Count - 1; i++)
                        {
                            double _number = before_entityD[i].number;
                            if (before_entityH.in_out_delivery_kbn == 1)
                            {
                                // 入庫時
                                _number = _number * -1;
                            }
                            if (before_entityD[i].inventory_management_division_id == 1)
                            {
                                DataCommodityInventory.UpdCommodityInventory(companyId,
                                                                             groupId,
                                                                             db,
                                                                             ExCast.zNumZeroNothingFormat(before_entityD[i].commodity_id),
                                                                             _number,
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
                    CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateInOutDeliveryExc(UpdateCommodityInventory)", ex);
                    return "UpdateInOutDeliveryExc(UpdateCommodityInventory) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
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
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateInOutDeliveryExc(CommitTransaction)", ex);
                return "UpdateInOutDeliveryExc(CommitTransaction) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
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
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateInOutDeliveryExc(DbClose)", ex);
                return "UpdateInOutDeliveryExc(DbClose) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
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
                                                   DataPgEvidence.PGName.InOutDeliver.InOutDeliverInp,
                                                   DataPgEvidence.geOperationType.Update,
                                                   "NO:" + no.ToString() + ",ID:" + id.ToString());
                        break;
                    case 1:
                        svcPgEvidence.gAddEvidence(ExCast.zCInt(HttpContext.Current.Session[ExSession.EVIDENCE_SAVE_FLG]),
                                                   companyId,
                                                   userId,
                                                   ipAdress,
                                                   sessionString,
                                                   DataPgEvidence.PGName.InOutDeliver.InOutDeliverInp,
                                                   DataPgEvidence.geOperationType.Insert,
                                                   "NO:" + no.ToString() + ",ID:" + id.ToString());
                        break;
                    case 2:
                        svcPgEvidence.gAddEvidence(ExCast.zCInt(HttpContext.Current.Session[ExSession.EVIDENCE_SAVE_FLG]),
                                                   companyId,
                                                   userId,
                                                   ipAdress,
                                                   sessionString,
                                                   DataPgEvidence.PGName.InOutDeliver.InOutDeliverInp,
                                                   DataPgEvidence.geOperationType.Delete,
                                                   "NO:" + no.ToString() + ",ID:" + id.ToString());
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateInOutDeliveryExc(Add Evidence)", ex);
                return "UpdateInOutDeliveryExc(Add Evidence) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
            }

            #endregion

            #region Return

            if (type == 1 && InOutDeliveryNo == 0)
            {
                return "Auto Insert success : " + "入出庫番号 : " + no.ToString().ToString() + "で登録しました。";
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
