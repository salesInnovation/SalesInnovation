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
    public class svcSetCommodity
    {
        private const string CLASS_NM = "svcSetCommodity";
        private readonly string PG_NM = DataPgEvidence.PGName.Mst.SetCommodity;

        //#region データ取得

        ///// <summary> 
        ///// データ取得
        ///// </summary>
        ///// <param name="Id"></param>
        ///// <returns></returns>
        //[OperationContract()]
        //[WebMethod(EnableSession = true)]
        //public EntitySetCommodity GetSetCommodity(string random, string Id)
        //{

        //    EntitySetCommodity entity;

        //    #region 認証処理

        //    string companyId = "";
        //    string groupId = "";
        //    string userId = "";
        //    string ipAdress = "";
        //    string sessionString = "";

        //    try
        //    {
        //        companyId = ExCast.zCStr(HttpContext.Current.Session[ExSession.COMPANY_ID]);
        //        groupId = ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_ID]);
        //        userId = ExCast.zCStr(HttpContext.Current.Session[ExSession.USER_ID]);
        //        ipAdress = ExCast.zCStr(HttpContext.Current.Session[ExSession.IP_ADRESS]);
        //        sessionString = ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]);

        //        string _message = ExSession.SessionUserUniqueCheck(random, ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]), ExCast.zCInt(HttpContext.Current.Session[ExSession.USER_ID]));
        //        if (_message != "")
        //        {
        //            entity = new EntitySetCommodity();
        //            entity.MESSAGE = _message;
        //            return entity;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        CommonUtl.ExLogger.Error(CLASS_NM + ".GetCommodity(認証処理)", ex);
        //        entity = new EntitySetCommodity();
        //        entity.MESSAGE = CLASS_NM + ".GetCommodity : 認証処理に失敗しました。" + Environment.NewLine + ex.Message.ToString(); ;
        //        return entity;
        //    }


        //    #endregion

        //    StringBuilder sb;
        //    DataTable dt;
        //    ExMySQLData db;

        //    try
        //    {
        //        db = ExSession.GetSessionDb(ExCast.zCInt(HttpContext.Current.Session[ExSession.USER_ID]),
        //                                    ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]));

        //        sb = new StringBuilder();

        //        #region SQL

        //        sb.Append("SELECT MT.* " + Environment.NewLine);
        //        sb.Append("      ,PU.NAME AS MAIN_PURCHASE_NAME " + Environment.NewLine);
        //        sb.Append("      ,NM1.DESCRIPTION AS UNIT_NAME " + Environment.NewLine);
        //        sb.Append("      ,NM2.DESCRIPTION AS TAXATION_DIVISION_NAME " + Environment.NewLine);
        //        sb.Append("      ,NM3.DESCRIPTION AS INVENTORY_MANAGEMENT_DIVISION_NAME " + Environment.NewLine);
        //        sb.Append("      ,NM4.DESCRIPTION AS INVENTORY_EVALUATION_NAME " + Environment.NewLine);
        //        sb.Append("      ,CL1.NAME AS GROUP1_NAME " + Environment.NewLine);
        //        sb.Append("      ,CL2.NAME AS GROUP2_NAME " + Environment.NewLine);
        //        sb.Append("      ,CL3.NAME AS GROUP3_NAME " + Environment.NewLine);
        //        sb.Append("      ,NM.DESCRIPTION AS DISPLAY_DIVISION_NAME " + Environment.NewLine);
        //        sb.Append("  FROM M_COMMODITY AS MT" + Environment.NewLine);

        //        #region Join

        //        // 仕入先
        //        sb.Append("  LEFT JOIN M_PURCHASE AS PU" + Environment.NewLine);
        //        sb.Append("    ON PU.DELETE_FLG = 0 " + Environment.NewLine);
        //        sb.Append("   AND PU.DISPLAY_FLG = 1 " + Environment.NewLine);
        //        sb.Append("   AND PU.COMPANY_ID = MT.COMPANY_ID" + Environment.NewLine);
        //        sb.Append("   AND PU.ID = MT.MAIN_PURCHASE_ID" + Environment.NewLine);

        //        // 単位
        //        sb.Append("  LEFT JOIN SYS_M_NAME AS NM1" + Environment.NewLine);
        //        sb.Append("    ON NM1.DELETE_FLG = 0 " + Environment.NewLine);
        //        sb.Append("   AND NM1.DISPLAY_FLG = 1 " + Environment.NewLine);
        //        sb.Append("   AND NM1.DIVISION_ID = " + (int)CommonUtl.geNameKbn.UNIT_ID + Environment.NewLine);
        //        sb.Append("   AND NM1.ID = MT.UNIT_ID" + Environment.NewLine);

        //        // 課税区分
        //        sb.Append("  LEFT JOIN SYS_M_NAME AS NM2" + Environment.NewLine);
        //        sb.Append("    ON NM2.DELETE_FLG = 0 " + Environment.NewLine);
        //        sb.Append("   AND NM2.DISPLAY_FLG = 1 " + Environment.NewLine);
        //        sb.Append("   AND NM2.DIVISION_ID = " + (int)CommonUtl.geNameKbn.TAX_DIVISION_ID + Environment.NewLine);
        //        sb.Append("   AND NM2.ID = MT.TAXATION_DIVISION_ID" + Environment.NewLine);

        //        // 在庫管理区分
        //        sb.Append("  LEFT JOIN SYS_M_NAME AS NM3" + Environment.NewLine);
        //        sb.Append("    ON NM3.DELETE_FLG = 0 " + Environment.NewLine);
        //        sb.Append("   AND NM3.DISPLAY_FLG = 1 " + Environment.NewLine);
        //        sb.Append("   AND NM3.DIVISION_ID = " + (int)CommonUtl.geNameKbn.INVENTORY_DIVISION_ID + Environment.NewLine);
        //        sb.Append("   AND NM3.ID = MT.INVENTORY_MANAGEMENT_DIVISION_ID" + Environment.NewLine);

        //        // 在庫評価
        //        sb.Append("  LEFT JOIN SYS_M_NAME AS NM4" + Environment.NewLine);
        //        sb.Append("    ON NM4.DELETE_FLG = 0 " + Environment.NewLine);
        //        sb.Append("   AND NM4.DISPLAY_FLG = 1 " + Environment.NewLine);
        //        sb.Append("   AND NM4.DIVISION_ID = " + (int)CommonUtl.geNameKbn.INVENTORY_DIVISION_ID + Environment.NewLine);
        //        sb.Append("   AND NM4.ID = MT.INVENTORY_EVALUATION_ID" + Environment.NewLine);

        //        // 商品分類1
        //        sb.Append("  LEFT JOIN M_CLASS AS CL1" + Environment.NewLine);
        //        sb.Append("    ON CL1.DELETE_FLG = 0 " + Environment.NewLine);
        //        sb.Append("   AND CL1.DISPLAY_FLG = 1 " + Environment.NewLine);
        //        sb.Append("   AND CL1.COMPANY_ID = MT.COMPANY_ID" + Environment.NewLine);
        //        sb.Append("   AND CL1.CLASS_DIVISION_ID = " + (int)CommonUtl.geMGroupKbn.CommodityGrouop1 + Environment.NewLine);
        //        sb.Append("   AND CL1.ID = MT.GROUP1_ID" + Environment.NewLine);

        //        // 商品分類2
        //        sb.Append("  LEFT JOIN M_CLASS AS CL2" + Environment.NewLine);
        //        sb.Append("    ON CL2.DELETE_FLG = 0 " + Environment.NewLine);
        //        sb.Append("   AND CL2.DISPLAY_FLG = 1 " + Environment.NewLine);
        //        sb.Append("   AND CL2.COMPANY_ID = MT.COMPANY_ID" + Environment.NewLine);
        //        sb.Append("   AND CL2.CLASS_DIVISION_ID = " + (int)CommonUtl.geMGroupKbn.CommodityGrouop2 + Environment.NewLine);
        //        sb.Append("   AND CL2.ID = MT.GROUP2_ID" + Environment.NewLine);

        //        // 商品分類3
        //        sb.Append("  LEFT JOIN M_CLASS AS CL3" + Environment.NewLine);
        //        sb.Append("    ON CL3.DELETE_FLG = 0 " + Environment.NewLine);
        //        sb.Append("   AND CL3.DISPLAY_FLG = 1 " + Environment.NewLine);
        //        sb.Append("   AND CL3.COMPANY_ID = MT.COMPANY_ID" + Environment.NewLine);
        //        sb.Append("   AND CL3.CLASS_DIVISION_ID = " + (int)CommonUtl.geMGroupKbn.CommodityGrouop3 + Environment.NewLine);
        //        sb.Append("   AND CL3.ID = MT.GROUP3_ID" + Environment.NewLine);

        //        // 表示区分
        //        sb.Append("  LEFT JOIN SYS_M_NAME AS NM" + Environment.NewLine);
        //        sb.Append("    ON NM.DELETE_FLG = 0 " + Environment.NewLine);
        //        sb.Append("   AND NM.DISPLAY_FLG = 1 " + Environment.NewLine);
        //        sb.Append("   AND NM.DIVISION_ID = " + (int)CommonUtl.geNameKbn.DISPLAY_DIVISION_ID + Environment.NewLine);
        //        sb.Append("   AND NM.ID = MT.DISPLAY_FLG" + Environment.NewLine);

        //        #endregion

        //        sb.Append(" WHERE MT.COMPANY_ID = " + companyId + Environment.NewLine);
        //        sb.Append("   AND MT.DELETE_FLG = 0 " + Environment.NewLine);
        //        sb.Append("   AND MT.SET_COMMODITY_FLG = 0 " + Environment.NewLine);
        //        sb.Append("   AND MT.ID = " + ExEscape.zRepStr(ExCast.zNumZeroNothingFormat(Id)) + Environment.NewLine);

        //        sb.Append(" ORDER BY MT.COMPANY_ID " + Environment.NewLine);
        //        sb.Append("         ,MT.ID " + Environment.NewLine);

        //        #endregion

        //        dt = db.GetDataTable(sb.ToString());

        //        if (dt.DefaultView.Count > 0)
        //        {
        //            entity = new EntitySetCommodity();

        //            // 排他制御
        //            DataPgLock.geLovkFlg lockFlg;
        //            string strErr = DataPgLock.SetLockPg(companyId, userId, PG_NM, ExCast.zNumZeroNothingFormat(Id), ipAdress, db, out lockFlg);
        //            if (strErr != "")
        //            {
        //                entity.MESSAGE = CLASS_NM + ".GetCommodity : 排他制御(ロック情報取得)に失敗しました。" + Environment.NewLine + strErr;
        //            }

        //            #region Set Entity

        //            entity.id = ExCast.zCStr(dt.DefaultView[0]["ID"]);
        //            entity.name = ExCast.zCStr(dt.DefaultView[0]["NAME"]);

        //            entity.enter_number = ExCast.zCDbl(dt.DefaultView[0]["ENTER_NUMBER"]);


        //            entity.purchase_lot = ExCast.zCDbl(dt.DefaultView[0]["PURCHASE_LOT"]);
        //            entity.lead_time = ExCast.zCInt(dt.DefaultView[0]["LEAD_TIME"]);
        //            entity.just_inventory_number = ExCast.zCDbl(dt.DefaultView[0]["JUST_INVENTORY_NUMBER"]);
        //            entity.inventory_number = ExCast.zCDbl(dt.DefaultView[0]["INVENTORY_NUMBER"]);

        //            entity.inventory_evaluation_id = ExCast.zCInt(dt.DefaultView[0]["INVENTORY_EVALUATION_ID"]);
        //            entity.inventory_evaluation_nm = ExCast.zCStr(dt.DefaultView[0]["INVENTORY_EVALUATION_NAME"]);

        //            entity.main_purchase_id = ExCast.zCStr(dt.DefaultView[0]["MAIN_PURCHASE_ID"]);
        //            entity.main_purchase_nm = ExCast.zCStr(dt.DefaultView[0]["MAIN_PURCHASE_NAME"]);

        //            entity.retail_price_skip_tax = ExCast.zCDbl(dt.DefaultView[0]["RETAIL_PRICE_SKIP_TAX"]);
        //            entity.retail_price_before_tax = ExCast.zCDbl(dt.DefaultView[0]["RETAIL_PRICE_BEFORE_TAX"]);
        //            entity.sales_unit_price_skip_tax = ExCast.zCDbl(dt.DefaultView[0]["SALES_UNIT_PRICE_SKIP_TAX"]);
        //            entity.sales_unit_price_before_tax = ExCast.zCDbl(dt.DefaultView[0]["SALES_UNIT_PRICE_BEFORE_TAX"]);
        //            entity.sales_cost_price_skip_tax = ExCast.zCDbl(dt.DefaultView[0]["SALES_COST_PRICE_SKIP_TAX"]);
        //            entity.sales_cost_price_before_tax = ExCast.zCDbl(dt.DefaultView[0]["SALES_COST_PRICE_BEFORE_TAX"]);
        //            entity.purchase_unit_price_skip_tax = ExCast.zCDbl(dt.DefaultView[0]["PURCHASE_UNIT_PRICE_SKIP_TAX"]);
        //            entity.purchase_unit_price_before_tax = ExCast.zCDbl(dt.DefaultView[0]["PURCHASE_UNIT_PRICE_BEFORE_TAX"]);

        //            entity.group1_id = ExCast.zCStr(dt.DefaultView[0]["GROUP1_ID"]);
        //            entity.group1_nm = ExCast.zCStr(dt.DefaultView[0]["GROUP1_NAME"]);

        //            entity.group2_id = ExCast.zCStr(dt.DefaultView[0]["GROUP2_ID"]);
        //            entity.group2_nm = ExCast.zCStr(dt.DefaultView[0]["GROUP2_NAME"]);

        //            entity.group3_id = ExCast.zCStr(dt.DefaultView[0]["GROUP3_ID"]);
        //            entity.group3_nm = ExCast.zCStr(dt.DefaultView[0]["GROUP3_NAME"]);

        //            entity.display_division_id = ExCast.zCInt(dt.DefaultView[0]["DISPLAY_FLG"]);
        //            entity.display_division_nm = ExCast.zCStr(dt.DefaultView[0]["DISPLAY_DIVISION_NAME"]);

        //            entity.memo = ExCast.zCStr(dt.DefaultView[0]["MEMO"]);

        //            entity.lock_flg = (int)lockFlg;

        //            #endregion

        //        }
        //        else
        //        {
        //            entity = null;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        CommonUtl.ExLogger.Error(CLASS_NM + ".GetCommodity", ex);
        //        entity = new EntitySetCommodity();
        //        entity.MESSAGE = CLASS_NM + ".GetCommodity : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message.ToString();
        //    }
        //    finally
        //    {
        //        db = null;
        //    }

        //    svcPgEvidence.gAddEvidence(ExCast.zCInt(HttpContext.Current.Session[ExSession.EVIDENCE_SAVE_FLG]),
        //                   companyId,
        //                   userId,
        //                   ipAdress,
        //                   sessionString,
        //                   PG_NM,
        //                   DataPgEvidence.geOperationType.Select,
        //                   "ID:" + Id.ToString());

        //    return entity;

        //}

        //#endregion

        //#region データ更新

        ///// <summary>
        ///// データ更新
        ///// </summary>
        ///// <param name="random">セッションランダム文字列</param>
        ///// <param name="type">0:Update 1:Insert 2:Delete</param>
        ///// <param name="Id">Id</param>
        ///// <param name="entity">更新データ</param>
        ///// <returns></returns>
        //[OperationContract()]
        //[WebMethod(EnableSession = true)]
        //public string UpdateCommodity(string random, int type, string Id, EntitySetCommodity entity)
        //{

        //    #region 認証処理

        //    string companyId = "";
        //    string groupId = "";
        //    string userId = "";
        //    string ipAdress = "";
        //    string sessionString = "";
        //    string personId = "";

        //    try
        //    {
        //        companyId = ExCast.zCStr(HttpContext.Current.Session[ExSession.COMPANY_ID]);
        //        groupId = ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_ID]);
        //        userId = ExCast.zCStr(HttpContext.Current.Session[ExSession.USER_ID]);
        //        ipAdress = ExCast.zCStr(HttpContext.Current.Session[ExSession.IP_ADRESS]);
        //        sessionString = ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]);
        //        personId = ExCast.zCStr(HttpContext.Current.Session[ExSession.PERSON_ID]);

        //        string _message = ExSession.SessionUserUniqueCheck(random, ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]), ExCast.zCInt(HttpContext.Current.Session[ExSession.USER_ID]));
        //        if (_message != "")
        //        {
        //            return _message;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateCommodity(認証処理)", ex);
        //        return CLASS_NM + ".UpdateCommodity : 認証処理に失敗しました。" + Environment.NewLine + ex.Message.ToString();
        //    }

        //    #endregion

        //    #region Field

        //    StringBuilder sb = new StringBuilder();
        //    DataTable dt;
        //    ExMySQLData db = null;
        //    string _Id = "";

        //    #endregion

        //    #region Databese Open

        //    try
        //    {
        //        db = new ExMySQLData(ExCast.zCStr(HttpContext.Current.Session[ExSession.DB_CONNECTION_STR]));
        //        db.DbOpen();
        //    }
        //    catch (Exception ex)
        //    {
        //        CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateCommodity(DbOpen)", ex);
        //        return CLASS_NM + ".UpdateCommodity(DbOpen) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
        //    }

        //    #endregion

        //    #region BeginTransaction

        //    try
        //    {
        //        db.ExBeginTransaction();
        //    }
        //    catch (Exception ex)
        //    {
        //        CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateCommodity(BeginTransaction)", ex);
        //        return CLASS_NM + ".UpdateCommodity(BeginTransaction) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
        //    }

        //    #endregion

        //    #region Get Max Master ID

        //    if (type == 1 && (Id == "" || Id == "0"))
        //    {
        //        try
        //        {
        //            DataMasterId.GetMaxMasterId(companyId,
        //                                        "",
        //                                        db,
        //                                        DataMasterId.geMasterMaxIdKbn.Commodity,
        //                                        out _Id);

        //            if (_Id == "")
        //            {
        //                return "ID取得に失敗しました。";
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateCommodity(GetMaxMasterId)", ex);
        //            return CLASS_NM + ".UpdateCommodity(GetMaxMasterId) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
        //        }
        //    }
        //    else
        //    {
        //        _Id = Id;
        //    }

        //    #endregion

        //    #region Insert

        //    if (type == 1)
        //    {
        //        try
        //        {
        //            #region Delete SQL

        //            sb.Length = 0;
        //            sb.Append("DELETE FROM M_COMMODITY " + Environment.NewLine);
        //            sb.Append(" WHERE DELETE_FLG = 1 " + Environment.NewLine);
        //            sb.Append("   AND COMPANY_ID = " + companyId + Environment.NewLine);
        //            sb.Append("   AND ID = " + ExEscape.zRepStr(ExCast.zNumZeroNothingFormat(_Id)) + Environment.NewLine);

        //            #endregion

        //            db.ExecuteSQL(sb.ToString(), false);

        //            #region Insert SQL

        //            string _main_purchase_id = entity.main_purchase_id;
        //            if (ExCast.IsNumeric(_main_purchase_id)) _main_purchase_id = ExCast.zCDbl(_main_purchase_id).ToString();

        //            sb.Length = 0;
        //            sb.Append("INSERT INTO M_COMMODITY " + Environment.NewLine);
        //            sb.Append("       ( COMPANY_ID" + Environment.NewLine);
        //            sb.Append("       , ID" + Environment.NewLine);
        //            sb.Append("       , ID2" + Environment.NewLine);
        //            sb.Append("       , NAME" + Environment.NewLine);
        //            sb.Append("       , KANA" + Environment.NewLine);
        //            sb.Append("       , UNIT_ID" + Environment.NewLine);
        //            sb.Append("       , ENTER_NUMBER" + Environment.NewLine);
        //            sb.Append("       , NUMBER_DECIMAL_DIGIT" + Environment.NewLine);
        //            sb.Append("       , UNIT_DECIMAL_DIGIT" + Environment.NewLine);
        //            sb.Append("       , TAXATION_DIVISION_ID" + Environment.NewLine);
        //            sb.Append("       , INVENTORY_MANAGEMENT_DIVISION_ID" + Environment.NewLine);
        //            sb.Append("       , PURCHASE_LOT" + Environment.NewLine);
        //            sb.Append("       , LEAD_TIME" + Environment.NewLine);
        //            sb.Append("       , JUST_INVENTORY_NUMBER" + Environment.NewLine);
        //            sb.Append("       , INVENTORY_NUMBER" + Environment.NewLine);
        //            sb.Append("       , INVENTORY_EVALUATION_ID" + Environment.NewLine);
        //            sb.Append("       , MAIN_PURCHASE_ID" + Environment.NewLine);
        //            sb.Append("       , RETAIL_PRICE_SKIP_TAX" + Environment.NewLine);
        //            sb.Append("       , RETAIL_PRICE_BEFORE_TAX" + Environment.NewLine);
        //            sb.Append("       , SALES_UNIT_PRICE_SKIP_TAX" + Environment.NewLine);
        //            sb.Append("       , SALES_UNIT_PRICE_BEFORE_TAX" + Environment.NewLine);
        //            sb.Append("       , SALES_COST_PRICE_SKIP_TAX" + Environment.NewLine);
        //            sb.Append("       , SALES_COST_PRICE_BEFORE_TAX" + Environment.NewLine);
        //            sb.Append("       , PURCHASE_UNIT_PRICE_SKIP_TAX" + Environment.NewLine);
        //            sb.Append("       , PURCHASE_UNIT_PRICE_BEFORE_TAX" + Environment.NewLine);
        //            sb.Append("       , GROUP1_ID" + Environment.NewLine);
        //            sb.Append("       , GROUP2_ID" + Environment.NewLine);
        //            sb.Append("       , GROUP3_ID" + Environment.NewLine);
        //            sb.Append("       , MEMO" + Environment.NewLine);
        //            sb.Append("       , DISPLAY_FLG" + Environment.NewLine);
        //            sb.Append("       , UPDATE_FLG" + Environment.NewLine);
        //            sb.Append("       , DELETE_FLG" + Environment.NewLine);
        //            sb.Append("       , CREATE_PG_ID" + Environment.NewLine);
        //            sb.Append("       , CREATE_ADRESS" + Environment.NewLine);
        //            sb.Append("       , CREATE_USER_ID" + Environment.NewLine);
        //            sb.Append("       , CREATE_PERSON_ID" + Environment.NewLine);
        //            sb.Append("       , CREATE_DATE" + Environment.NewLine);
        //            sb.Append("       , CREATE_TIME" + Environment.NewLine);
        //            sb.Append("       , UPDATE_PG_ID" + Environment.NewLine);
        //            sb.Append("       , UPDATE_ADRESS" + Environment.NewLine);
        //            sb.Append("       , UPDATE_USER_ID" + Environment.NewLine);
        //            sb.Append("       , UPDATE_PERSON_ID" + Environment.NewLine);
        //            sb.Append("       , UPDATE_DATE" + Environment.NewLine);
        //            sb.Append("       , UPDATE_TIME" + Environment.NewLine);
        //            sb.Append(")" + Environment.NewLine);
        //            sb.Append("SELECT  " + companyId + Environment.NewLine);                                                // COMPANY_ID
        //            sb.Append("       ," + ExEscape.zRepStr(ExCast.zNumZeroNothingFormat(_Id)) + Environment.NewLine);      // ID
        //            sb.Append("       ," + ExCast.zIdForNumIndex(_Id) + Environment.NewLine);                               // ID2
        //            sb.Append("       ," + ExEscape.zRepStr(entity.name) + Environment.NewLine);                            // NAME
        //            sb.Append("       ," + ExEscape.zRepStr(entity.kana) + Environment.NewLine);                            // KANA
        //            sb.Append("       ," + entity.unit_id + Environment.NewLine);                                           // UNIT_ID
        //            sb.Append("       ," + entity.enter_number + Environment.NewLine);                                      // ENTER_NUMBER
        //            sb.Append("       ," + entity.number_decimal_digit + Environment.NewLine);                              // NUMBER_DECIMAL_DIGIT
        //            sb.Append("       ," + entity.unit_decimal_digit + Environment.NewLine);                                // UNIT_DECIMAL_DIGIT
        //            sb.Append("       ," + entity.taxation_divition_id + Environment.NewLine);                              // TAXATION_DIVISION_ID
        //            sb.Append("       ," + entity.inventory_management_division_id + Environment.NewLine);                  // INVENTORY_MANAGEMENT_DIVISION_ID
        //            sb.Append("       ," + entity.purchase_lot + Environment.NewLine);                                      // PURCHASE_LOT
        //            sb.Append("       ," + entity.lead_time + Environment.NewLine);                                         // LEAD_TIME
        //            sb.Append("       ," + entity.just_inventory_number + Environment.NewLine);                             // JUST_INVENTORY_NUMBER
        //            sb.Append("       ," + entity.inventory_number + Environment.NewLine);                                  // INVENTORY_NUMBER
        //            sb.Append("       ," + entity.inventory_evaluation_id + Environment.NewLine);                           // INVENTORY_EVALUATION_ID
        //            sb.Append("       ," + ExEscape.zRepStr(_main_purchase_id) + Environment.NewLine);                      // MAIN_PURCHASE_ID
        //            sb.Append("       ," + entity.retail_price_skip_tax + Environment.NewLine);                             // RETAIL_PRICE_SKIP_TAX
        //            sb.Append("       ," + entity.retail_price_before_tax + Environment.NewLine);                           // RETAIL_PRICE_BEFORE_TAX
        //            sb.Append("       ," + entity.sales_unit_price_skip_tax + Environment.NewLine);                         // SALES_UNIT_PRICE_SKIP_TAX
        //            sb.Append("       ," + entity.sales_unit_price_before_tax + Environment.NewLine);                       // SALES_UNIT_PRICE_BEFORE_TAX
        //            sb.Append("       ," + entity.sales_cost_price_skip_tax + Environment.NewLine);                         // SALES_COST_PRICE_SKIP_TAX
        //            sb.Append("       ," + entity.sales_cost_price_before_tax + Environment.NewLine);                       // SALES_COST_PRICE_BEFORE_TAX
        //            sb.Append("       ," + entity.purchase_unit_price_skip_tax + Environment.NewLine);                      // PURCHASE_UNIT_PRICE_SKIP_TAX
        //            sb.Append("       ," + entity.purchase_unit_price_before_tax + Environment.NewLine);                    // PURCHASE_UNIT_PRICE_BEFORE_TAX
        //            sb.Append("       ," + ExEscape.zRepStr(entity.group1_id) + Environment.NewLine);                       // GROUP1_ID
        //            sb.Append("       ," + ExEscape.zRepStr(entity.group2_id) + Environment.NewLine);                       // GROUP2_ID
        //            sb.Append("       ," + ExEscape.zRepStr(entity.group3_id) + Environment.NewLine);                       // GROUP3_ID
        //            sb.Append("       ," + ExEscape.zRepStr(entity.memo) + Environment.NewLine);                            // MEMO
        //            sb.Append("       ," + entity.display_division_id + Environment.NewLine);                               // DISPLAY_FLG
        //            sb.Append(CommonUtl.GetInsSQLCommonColums(CommonUtl.UpdKbn.Ins,
        //                                                        PG_NM,
        //                                                        "M_COMMODITY",
        //                                                        ExCast.zCInt(personId),
        //                                                        _Id,
        //                                                        ipAdress,
        //                                                        userId));

        //            #endregion

        //            db.ExecuteSQL(sb.ToString(), false);

        //        }
        //        catch (Exception ex)
        //        {
        //            db.ExRollbackTransaction();
        //            CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateCommodity(Insert)", ex);
        //            return CLASS_NM + ".UpdateCommodity(Insert) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
        //        }

        //    }

        //    #endregion

        //    #region Update

        //    if (type == 0)
        //    {
        //        try
        //        {
        //            #region SQL

        //            string _main_purchase_id = entity.main_purchase_id;
        //            if (ExCast.IsNumeric(_main_purchase_id)) _main_purchase_id = ExCast.zCDbl(_main_purchase_id).ToString();

        //            sb.Length = 0;
        //            sb.Append("UPDATE M_COMMODITY " + Environment.NewLine);
        //            sb.Append(CommonUtl.GetUpdSQLCommonColums(PG_NM,
        //                                                       ExCast.zCInt(personId),
        //                                                       ipAdress,
        //                                                       userId,
        //                                                       0));
        //            sb.Append("      ,NAME = " + ExEscape.zRepStr(entity.name) + Environment.NewLine);
        //            sb.Append("      ,KANA = " + ExEscape.zRepStr(entity.kana) + Environment.NewLine);
        //            sb.Append("      ,UNIT_ID = " + entity.unit_id + Environment.NewLine);
        //            sb.Append("      ,ENTER_NUMBER = " + entity.enter_number + Environment.NewLine);
        //            sb.Append("      ,NUMBER_DECIMAL_DIGIT = " + entity.number_decimal_digit + Environment.NewLine);
        //            sb.Append("      ,UNIT_DECIMAL_DIGIT = " + entity.unit_decimal_digit + Environment.NewLine);
        //            sb.Append("      ,TAXATION_DIVISION_ID = " + entity.taxation_divition_id + Environment.NewLine);
        //            sb.Append("      ,INVENTORY_MANAGEMENT_DIVISION_ID = " + entity.inventory_management_division_id + Environment.NewLine);
        //            sb.Append("      ,PURCHASE_LOT = " + entity.purchase_lot + Environment.NewLine);
        //            sb.Append("      ,LEAD_TIME = " + entity.lead_time + Environment.NewLine);
        //            sb.Append("      ,JUST_INVENTORY_NUMBER = " + entity.just_inventory_number + Environment.NewLine);
        //            sb.Append("      ,INVENTORY_NUMBER = " + entity.inventory_number + Environment.NewLine);
        //            sb.Append("      ,INVENTORY_EVALUATION_ID = " + entity.inventory_evaluation_id + Environment.NewLine);
        //            sb.Append("      ,MAIN_PURCHASE_ID = " + ExEscape.zRepStr(_main_purchase_id) + Environment.NewLine);
        //            sb.Append("      ,RETAIL_PRICE_SKIP_TAX = " + entity.retail_price_skip_tax + Environment.NewLine);
        //            sb.Append("      ,RETAIL_PRICE_BEFORE_TAX = " + entity.retail_price_before_tax + Environment.NewLine);
        //            sb.Append("      ,SALES_UNIT_PRICE_SKIP_TAX = " + entity.sales_unit_price_skip_tax + Environment.NewLine);
        //            sb.Append("      ,SALES_UNIT_PRICE_BEFORE_TAX = " + entity.sales_unit_price_before_tax + Environment.NewLine);
        //            sb.Append("      ,SALES_COST_PRICE_SKIP_TAX = " + entity.sales_cost_price_skip_tax + Environment.NewLine);
        //            sb.Append("      ,SALES_COST_PRICE_BEFORE_TAX = " + entity.sales_cost_price_before_tax + Environment.NewLine);
        //            sb.Append("      ,PURCHASE_UNIT_PRICE_SKIP_TAX = " + entity.purchase_unit_price_skip_tax + Environment.NewLine);
        //            sb.Append("      ,PURCHASE_UNIT_PRICE_BEFORE_TAX = " + entity.purchase_unit_price_before_tax + Environment.NewLine);
        //            sb.Append("      ,GROUP1_ID = " + ExEscape.zRepStr(entity.group1_id) + Environment.NewLine);
        //            sb.Append("      ,GROUP2_ID = " + ExEscape.zRepStr(entity.group2_id) + Environment.NewLine);
        //            sb.Append("      ,GROUP3_ID = " + ExEscape.zRepStr(entity.group3_id) + Environment.NewLine);
        //            sb.Append("      ,MEMO = " + ExEscape.zRepStr(entity.memo) + Environment.NewLine);
        //            sb.Append("      ,DISPLAY_FLG = " + entity.display_division_id + Environment.NewLine);
        //            sb.Append(" WHERE COMPANY_ID = " + companyId + Environment.NewLine);        // COMPANY_ID
        //            sb.Append("   AND ID = " + ExEscape.zRepStr(ExCast.zNumZeroNothingFormat(Id)) + Environment.NewLine);               // ID

        //            #endregion

        //            db.ExecuteSQL(sb.ToString(), false);

        //        }
        //        catch (Exception ex)
        //        {
        //            db.ExRollbackTransaction();
        //            CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateCommodity(Update)", ex);
        //            return CLASS_NM + ".UpdateCommodity(Insert) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
        //        }
        //    }

        //    #endregion

        //    #region Delete

        //    if (type == 2)
        //    {
        //        #region Exist Data

        //        try
        //        {
        //            bool _ret = false;

        //            _ret = DataExists.IsExistData(db, companyId, groupId, "T_ESTIMATE_D", "COMMODITY_ID", ExCast.zNumZeroNothingFormat(Id), CommonUtl.geStrOrNumKbn.String);
        //            if (_ret == true)
        //            {
        //                return "商品ID : " + Id + " は見積データに使用されている為、削除できません。";
        //            }

        //            _ret = DataExists.IsExistData(db, companyId, groupId, "T_ORDER_D", "COMMODITY_ID", ExCast.zNumZeroNothingFormat(Id), CommonUtl.geStrOrNumKbn.String);
        //            if (_ret == true)
        //            {
        //                return "商品ID : " + Id + " は受注データに使用されている為、削除できません。";
        //            }

        //            _ret = DataExists.IsExistData(db, companyId, groupId, "T_SALES_D", "COMMODITY_ID", ExCast.zNumZeroNothingFormat(Id), CommonUtl.geStrOrNumKbn.String);
        //            if (_ret == true)
        //            {
        //                return "商品ID : " + Id + " は売上データに使用されている為、削除できません。";
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            db.ExRollbackTransaction();
        //            CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateCommodity(Exist Data)", ex);
        //            return CLASS_NM + ".UpdateCommodity(Exist Data) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
        //        }

        //        #endregion

        //        #region Update

        //        try
        //        {
        //            sb.Length = 0;
        //            sb.Append("UPDATE M_COMMODITY " + Environment.NewLine);
        //            sb.Append(CommonUtl.GetUpdSQLCommonColums(PG_NM,
        //                                                       ExCast.zCInt(personId),
        //                                                       ipAdress,
        //                                                       userId,
        //                                                       1));
        //            sb.Append(" WHERE COMPANY_ID = " + companyId + Environment.NewLine);    // COMPANY_ID
        //            sb.Append("   AND ID = " + ExEscape.zRepStr(ExCast.zNumZeroNothingFormat(Id)) + Environment.NewLine);            // ID

        //            db.ExecuteSQL(sb.ToString(), false);

        //        }
        //        catch (Exception ex)
        //        {
        //            db.ExRollbackTransaction();
        //            CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateCommodity(Delete)", ex);
        //            return CLASS_NM + ".UpdateCommodity(Delete) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
        //        }

        //        #endregion
        //    }

        //    #endregion

        //    #region PG排他制御

        //    if (type == 0 || type == 2)
        //    {
        //        try
        //        {
        //            DataPgLock.DelLockPg(companyId, userId, PG_NM, "", ipAdress, false, db);
        //        }
        //        catch (Exception ex)
        //        {
        //            db.ExRollbackTransaction();
        //            CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateCommodity(DelLockPg)", ex);
        //            return CLASS_NM + ".UpdateCommodity(DelLockPg) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
        //        }
        //    }

        //    #endregion

        //    #region CommitTransaction

        //    try
        //    {
        //        db.ExCommitTransaction();
        //    }
        //    catch (Exception ex)
        //    {
        //        CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateCommodity(CommitTransaction)", ex);
        //        return CLASS_NM + ".UpdateCommodity(CommitTransaction) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
        //    }

        //    #endregion

        //    #region Database Close

        //    try
        //    {
        //        db.DbClose();
        //    }
        //    catch (Exception ex)
        //    {
        //        db.ExRollbackTransaction();
        //        CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateCommodity(DbClose)", ex);
        //        return CLASS_NM + ".UpdateCommodity(DbClose) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
        //    }
        //    finally
        //    {
        //        db = null;
        //    }

        //    #endregion

        //    #region Add Evidence

        //    try
        //    {
        //        switch (type)
        //        {
        //            case 0:
        //                svcPgEvidence.gAddEvidence(ExCast.zCInt(HttpContext.Current.Session[ExSession.EVIDENCE_SAVE_FLG]),
        //                                           companyId,
        //                                           userId,
        //                                           ipAdress,
        //                                           sessionString,
        //                                           PG_NM,
        //                                           DataPgEvidence.geOperationType.Update,
        //                                           "ID:" + Id.ToString());
        //                break;
        //            case 1:
        //                svcPgEvidence.gAddEvidence(ExCast.zCInt(HttpContext.Current.Session[ExSession.EVIDENCE_SAVE_FLG]),
        //                                           companyId,
        //                                           userId,
        //                                           ipAdress,
        //                                           sessionString,
        //                                           PG_NM,
        //                                           DataPgEvidence.geOperationType.Insert,
        //                                           "ID:" + _Id.ToString());
        //                break;
        //            case 2:
        //                svcPgEvidence.gAddEvidence(ExCast.zCInt(HttpContext.Current.Session[ExSession.EVIDENCE_SAVE_FLG]),
        //                                           companyId,
        //                                           userId,
        //                                           ipAdress,
        //                                           sessionString,
        //                                           PG_NM,
        //                                           DataPgEvidence.geOperationType.Delete,
        //                                           "ID:" + Id.ToString());
        //                break;
        //            default:
        //                break;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateCommodity(Add Evidence)", ex);
        //        return CLASS_NM + ".UpdateCommodity(Add Evidence) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
        //    }

        //    #endregion

        //    #region Return

        //    if (type == 1 && (Id == "0" || Id == ""))
        //    {
        //        return "Auto Insert success : " + "ID : " + _Id.ToString() + "で登録しました。";
        //    }
        //    else
        //    {
        //        return "";
        //    }

        //    #endregion
        //}

        //#endregion

    }
}
