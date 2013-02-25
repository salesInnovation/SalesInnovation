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
    public class svcSupplier
    {
        private const string CLASS_NM = "svcSupplier";
        private readonly string PG_NM = DataPgEvidence.PGName.Mst.Supplier;

        #region データ取得

        /// <summary> 
        /// データ取得
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebMethod(EnableSession = true)]
        public EntitySupplier GetSupplier(string random, string CustomerId, string Id)
        {

            EntitySupplier entity;

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
                    entity = new EntitySupplier();
                    entity.MESSAGE = _message;
                    return entity;
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetSupplier(認証処理)", ex);
                entity = new EntitySupplier();
                entity.MESSAGE = CLASS_NM + ".GetSupplier : 認証処理に失敗しました。" + Environment.NewLine + ex.Message.ToString(); ;
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

                sb.Append("SELECT MT.* " + Environment.NewLine);
                sb.Append("      ,CM.NAME AS CUSTOMER_NAME " + Environment.NewLine);
                sb.Append("      ,NM1.DESCRIPTION AS DIVIDE_PERMISSION_NAME " + Environment.NewLine);
                sb.Append("      ,NM.DESCRIPTION AS DISPLAY_DIVISION_NAME " + Environment.NewLine);
                sb.Append("  FROM M_SUPPLIER AS MT" + Environment.NewLine);

                #region Join

                // 得意先
                sb.Append("  LEFT JOIN M_CUSTOMER AS CM" + Environment.NewLine);
                sb.Append("    ON CM.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND CM.DISPLAY_FLG = 1 " + Environment.NewLine);
                sb.Append("   AND CM.COMPANY_ID = MT.COMPANY_ID" + Environment.NewLine);
                sb.Append("   AND CM.ID = MT.CUSTOMER_ID" + Environment.NewLine);

                // 分納許可
                sb.Append("  LEFT JOIN SYS_M_NAME AS NM1" + Environment.NewLine);
                sb.Append("    ON NM1.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND NM1.DISPLAY_FLG = 1 " + Environment.NewLine);
                sb.Append("   AND NM1.DIVISION_ID = " + (int)CommonUtl.geNameKbn.DIVIDE_PERMISSION_ID + Environment.NewLine);
                sb.Append("   AND NM1.ID = MT.DIVIDE_PERMISSION_ID" + Environment.NewLine);

                // 表示区分
                sb.Append("  LEFT JOIN SYS_M_NAME AS NM" + Environment.NewLine);
                sb.Append("    ON NM.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND NM.DISPLAY_FLG = 1 " + Environment.NewLine);
                sb.Append("   AND NM.DIVISION_ID = " + (int)CommonUtl.geNameKbn.DISPLAY_DIVISION_ID + Environment.NewLine);
                sb.Append("   AND NM.ID = MT.DISPLAY_FLG" + Environment.NewLine);

                #endregion

                sb.Append(" WHERE MT.COMPANY_ID = " + companyId + Environment.NewLine);
                sb.Append("   AND MT.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND MT.CUSTOMER_ID = " + ExEscape.zRepStr(ExCast.zNumZeroNothingFormat(CustomerId)) + Environment.NewLine);
                sb.Append("   AND MT.ID = " + ExEscape.zRepStr(ExCast.zNumZeroNothingFormat(Id)) + Environment.NewLine);

                sb.Append(" ORDER BY MT.COMPANY_ID " + Environment.NewLine);
                sb.Append("         ,MT.ID " + Environment.NewLine);

                #endregion

                dt = db.GetDataTable(sb.ToString());

                if (dt.DefaultView.Count > 0)
                {
                    entity = new EntitySupplier();

                    // 排他制御
                    DataPgLock.geLovkFlg lockFlg;
                    string strErr = DataPgLock.SetLockPg(companyId, userId, PG_NM, ExCast.zNumZeroNothingFormat(CustomerId) + "-" + ExCast.zNumZeroNothingFormat(Id), ipAdress, db, out lockFlg);
                    if (strErr != "")
                    {
                        entity.MESSAGE = CLASS_NM + ".GetSupplier : 排他制御(ロック情報取得)に失敗しました。" + Environment.NewLine + strErr;
                    }

                    #region Set Entity

                    entity.id = ExCast.zCStr(dt.DefaultView[0]["ID"]);
                    entity.name = ExCast.zCStr(dt.DefaultView[0]["NAME"]);
                    entity.kana = ExCast.zCStr(dt.DefaultView[0]["KANA"]);
                    entity.about_name = ExCast.zCStr(dt.DefaultView[0]["ABOUT_NAME"]);

                    string _zip = ExCast.zCStr(dt.DefaultView[0]["ZIP_CODE"]);
                    if (!string.IsNullOrEmpty(_zip) && ExCast.zCStr(_zip) != "0")
                    {
                        _zip = string.Format("{0:0000000}", ExCast.zCDbl(_zip));
                        entity.zip_code_from = _zip.Substring(0, 3);
                        entity.zip_code_to = _zip.Substring(3, 4);
                    }
                    else
                    {
                        entity.zip_code_from = "";
                        entity.zip_code_to = "";
                    }

                    entity.prefecture_id = ExCast.zCInt(dt.DefaultView[0]["PREFECTURE_ID"]);
                    entity.city_id = ExCast.zCInt(dt.DefaultView[0]["CITY_ID"]);
                    entity.town_id = ExCast.zCInt(dt.DefaultView[0]["TOWN_ID"]);
                    entity.adress_city = ExCast.zCStr(dt.DefaultView[0]["ADRESS_CITY"]);
                    entity.adress_town = ExCast.zCStr(dt.DefaultView[0]["ADRESS_TOWN"]);
                    entity.adress1 = ExCast.zCStr(dt.DefaultView[0]["ADRESS1"]);
                    entity.adress2 = ExCast.zCStr(dt.DefaultView[0]["ADRESS2"]);
                    entity.station_name = ExCast.zCStr(dt.DefaultView[0]["STATION_NAME"]);
                    entity.post_name = ExCast.zCStr(dt.DefaultView[0]["POST_NAME"]);
                    entity.person_name = ExCast.zCStr(dt.DefaultView[0]["PERSON_NAME"]);
                    entity.title_id = ExCast.zCInt(dt.DefaultView[0]["TITLE_ID"]);
                    entity.title_name = ExCast.zCStr(dt.DefaultView[0]["TITLE_NAME"]);
                    entity.tel = ExCast.zCStr(dt.DefaultView[0]["TEL"]);
                    entity.fax = ExCast.zCStr(dt.DefaultView[0]["FAX"]);
                    entity.mail_adress = ExCast.zCStr(dt.DefaultView[0]["MAIL_ADRESS"]);
                    entity.mobile_tel = ExCast.zCStr(dt.DefaultView[0]["MOBILE_TEL"]);
                    entity.mobile_adress = ExCast.zCStr(dt.DefaultView[0]["MOBILE_ADRESS"]);
                    entity.url = ExCast.zCStr(dt.DefaultView[0]["URL"]);

                    entity.customer_id = ExCast.zCStr(dt.DefaultView[0]["CUSTOMER_ID"]);
                    entity.customer_nm = ExCast.zCStr(dt.DefaultView[0]["CUSTOMER_NAME"]);

                    entity.divide_permission_id = ExCast.zCInt(dt.DefaultView[0]["DISPLAY_FLG"]);
                    entity.divide_permission_nm = ExCast.zCStr(dt.DefaultView[0]["DISPLAY_DIVISION_NAME"]);

                    entity.display_division_id = ExCast.zCInt(dt.DefaultView[0]["DISPLAY_FLG"]);
                    entity.display_division_nm = ExCast.zCStr(dt.DefaultView[0]["DISPLAY_DIVISION_NAME"]);

                    entity.lock_flg = (int)lockFlg;
                    entity.memo = ExCast.zCStr(dt.DefaultView[0]["MEMO"]);

                    #endregion

                }
                else
                {
                    entity = null;
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetSupplier", ex);
                entity = new EntitySupplier();
                entity.MESSAGE = CLASS_NM + ".GetSupplier : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message.ToString();
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
                           "ID:" + Id.ToString());

            return entity;

        }

        #endregion

        #region データ更新

        /// <summary>
        /// データ更新
        /// </summary>
        /// <param name="random">セッションランダム文字列</param>
        /// <param name="type">0:Update 1:Insert 2:Delete</param>
        /// <param name="Id">Id</param>
        /// <param name="entity">更新データ</param>
        /// <returns></returns>
        [OperationContract()]
        [WebMethod(EnableSession = true)]
        public string UpdateSupplier(string random, int type, string CustomerId, string Id, EntitySupplier entity)
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
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateSupplier(認証処理)", ex);
                return CLASS_NM + ".UpdateSupplier : 認証処理に失敗しました。" + Environment.NewLine + ex.Message.ToString();
            }

            #endregion

            #region Field

            StringBuilder sb = new StringBuilder();
            DataTable dt;
            ExMySQLData db = null;
            string _Id = "";

            #endregion

            #region Databese Open

            try
            {
                db = new ExMySQLData(ExCast.zCStr(HttpContext.Current.Session[ExSession.DB_CONNECTION_STR]));
                db.DbOpen();
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateSupplier(DbOpen)", ex);
                return CLASS_NM + ".UpdateSupplier(DbOpen) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
            }

            #endregion

            #region BeginTransaction

            try
            {
                db.ExBeginTransaction();
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateSupplier(BeginTransaction)", ex);
                return CLASS_NM + ".UpdateSupplier(BeginTransaction) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
            }

            #endregion

            #region Get Max Master ID

            if (type == 1 && (Id == "" || Id == "0"))
            {
                try
                {
                    DataMasterId.GetMaxMasterId(companyId,
                                                ExCast.zNumZeroNothingFormat(CustomerId),
                                                db,
                                                DataMasterId.geMasterMaxIdKbn.Supplier,
                                                out _Id);

                    if (_Id == "")
                    {
                        return "ID取得に失敗しました。";
                    }
                }
                catch (Exception ex)
                {
                    CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateSupplier(GetMaxMasterId)", ex);
                    return CLASS_NM + ".UpdateSupplier(GetMaxMasterId) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
                }
            }
            else
            {
                _Id = Id;
            }

            #endregion

            #region Insert

            if (type == 1)
            {
                try
                {
                    #region Delete SQL

                    sb.Length = 0;
                    sb.Append("DELETE FROM M_SUPPLIER " + Environment.NewLine);
                    sb.Append(" WHERE DELETE_FLG = 1 " + Environment.NewLine);
                    sb.Append("   AND COMPANY_ID = " + companyId + Environment.NewLine);
                    sb.Append("   AND ID = " + ExEscape.zRepStr(ExCast.zNumZeroNothingFormat(_Id)) + Environment.NewLine);
                    sb.Append("   AND CUSTOMER_ID = " + ExEscape.zRepStr(ExCast.zNumZeroNothingFormat(CustomerId)) + Environment.NewLine);

                    #endregion

                    db.ExecuteSQL(sb.ToString(), false);

                    #region Insert SQL

                    sb.Length = 0;
                    sb.Append("INSERT INTO M_SUPPLIER " + Environment.NewLine);
                    sb.Append("       ( COMPANY_ID" + Environment.NewLine);
                    sb.Append("       , CUSTOMER_ID" + Environment.NewLine);
                    sb.Append("       , ID" + Environment.NewLine);
                    sb.Append("       , ID2" + Environment.NewLine);
                    sb.Append("       , NAME" + Environment.NewLine);
                    sb.Append("       , KANA" + Environment.NewLine);
                    sb.Append("       , ABOUT_NAME" + Environment.NewLine);
                    sb.Append("       , ZIP_CODE" + Environment.NewLine);
                    sb.Append("       , PREFECTURE_ID" + Environment.NewLine);
                    sb.Append("       , CITY_ID" + Environment.NewLine);
                    sb.Append("       , TOWN_ID" + Environment.NewLine);
                    sb.Append("       , ADRESS_CITY" + Environment.NewLine);
                    sb.Append("       , ADRESS_TOWN" + Environment.NewLine);
                    sb.Append("       , ADRESS1" + Environment.NewLine);
                    sb.Append("       , ADRESS2" + Environment.NewLine);
                    sb.Append("       , STATION_NAME" + Environment.NewLine);
                    sb.Append("       , POST_NAME" + Environment.NewLine);
                    sb.Append("       , PERSON_NAME" + Environment.NewLine);
                    sb.Append("       , TITLE_ID" + Environment.NewLine);
                    sb.Append("       , TITLE_NAME" + Environment.NewLine);
                    sb.Append("       , TEL" + Environment.NewLine);
                    sb.Append("       , FAX" + Environment.NewLine);
                    sb.Append("       , MAIL_ADRESS" + Environment.NewLine);
                    sb.Append("       , MOBILE_TEL" + Environment.NewLine);
                    sb.Append("       , MOBILE_ADRESS" + Environment.NewLine);
                    sb.Append("       , URL" + Environment.NewLine);
                    sb.Append("       , DIVIDE_PERMISSION_ID" + Environment.NewLine);
                    sb.Append("       , MEMO" + Environment.NewLine);
                    sb.Append("       , DISPLAY_FLG" + Environment.NewLine);
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
                    sb.Append("       ," + ExEscape.zRepStr(ExCast.zNumZeroNothingFormat(CustomerId)) + Environment.NewLine);       // CUSTOMER_ID
                    sb.Append("       ," + ExEscape.zRepStr(ExCast.zNumZeroNothingFormat(_Id)) + Environment.NewLine);              // ID
                    sb.Append("       ," + ExCast.zIdForNumIndex(_Id) + Environment.NewLine);                                       // ID2
                    sb.Append("       ," + ExEscape.zRepStr(entity.name) + Environment.NewLine);                                    // NAME
                    sb.Append("       ," + ExEscape.zRepStr(entity.kana) + Environment.NewLine);                                    // KANA
                    sb.Append("       ," + ExEscape.zRepStr(entity.about_name) + Environment.NewLine);                              // ABOUT_NAME
                    sb.Append("       ," + ExCast.zNullToZero(entity.zip_code_from + entity.zip_code_to) + Environment.NewLine);    // ZIP_CODE
                    sb.Append("       ," + entity.prefecture_id + Environment.NewLine);                                             // PREFECTURE_ID
                    sb.Append("       ," + entity.city_id + Environment.NewLine);                                                   // CITY_ID
                    sb.Append("       ," + entity.town_id + Environment.NewLine);                                                   // TOWN_ID
                    sb.Append("       ," + ExEscape.zRepStr(entity.adress_city) + Environment.NewLine);                             // ADRESS_CITY
                    sb.Append("       ," + ExEscape.zRepStr(entity.adress_town) + Environment.NewLine);                             // ADRESS_TOWN
                    sb.Append("       ," + ExEscape.zRepStr(entity.adress1) + Environment.NewLine);                                 // ADRESS1
                    sb.Append("       ," + ExEscape.zRepStr(entity.adress2) + Environment.NewLine);                                 // ADRESS2
                    sb.Append("       ," + ExEscape.zRepStr(entity.station_name) + Environment.NewLine);                            // STATION_NAME
                    sb.Append("       ," + ExEscape.zRepStr(entity.post_name) + Environment.NewLine);                               // POST_NAME
                    sb.Append("       ," + ExEscape.zRepStr(entity.person_name) + Environment.NewLine);                             // PERSON_NAME
                    sb.Append("       ," + entity.title_id + Environment.NewLine);                                                  // TITLE_ID
                    sb.Append("       ," + ExEscape.zRepStr(entity.title_name) + Environment.NewLine);                              // TITLE_NAME
                    sb.Append("       ," + ExEscape.zRepStr(entity.tel) + Environment.NewLine);                                     // TEL
                    sb.Append("       ," + ExEscape.zRepStr(entity.fax) + Environment.NewLine);                                     // FAX
                    sb.Append("       ," + ExEscape.zRepStr(entity.mail_adress) + Environment.NewLine);                             // MAIL_ADRESS
                    sb.Append("       ," + ExEscape.zRepStr(entity.mobile_tel) + Environment.NewLine);                              // MOBILE_TEL
                    sb.Append("       ," + ExEscape.zRepStr(entity.mobile_adress) + Environment.NewLine);                           // MOBILE_ADRESS
                    sb.Append("       ," + ExEscape.zRepStr(entity.url) + Environment.NewLine);                                     // URL
                    sb.Append("       ," + entity.divide_permission_id + Environment.NewLine);                                      // DIVIDE_PERMISSION_ID
                    sb.Append("       ," + ExEscape.zRepStr(entity.memo) + Environment.NewLine);                                    // MEMO
                    sb.Append("       ," + entity.display_division_id + Environment.NewLine);                                       // DISPLAY_FLG
                    sb.Append(CommonUtl.GetInsSQLCommonColums(CommonUtl.UpdKbn.Ins,
                                                                PG_NM,
                                                                "M_SUPPLIER",
                                                                ExCast.zCInt(personId),
                                                                _Id,
                                                                ipAdress,
                                                                userId));

                    #endregion

                    db.ExecuteSQL(sb.ToString(), false);

                }
                catch (Exception ex)
                {
                    db.ExRollbackTransaction();
                    CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateSupplier(Insert)", ex);
                    return CLASS_NM + ".UpdateSupplier(Insert) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
                }

            }

            #endregion

            #region Update

            if (type == 0)
            {
                try
                {
                    #region SQL

                    sb.Length = 0;
                    sb.Append("UPDATE M_SUPPLIER " + Environment.NewLine);
                    sb.Append(CommonUtl.GetUpdSQLCommonColums(PG_NM,
                                                               ExCast.zCInt(personId),
                                                               ipAdress,
                                                               userId,
                                                               0));
                    sb.Append("      ,NAME = " + ExEscape.zRepStr(entity.name) + Environment.NewLine);
                    sb.Append("      ,KANA = " + ExEscape.zRepStr(entity.kana) + Environment.NewLine);
                    sb.Append("      ,ABOUT_NAME = " + ExEscape.zRepStr(entity.about_name) + Environment.NewLine);
                    sb.Append("      ,ZIP_CODE = " + ExCast.zNullToZero(entity.zip_code_from + entity.zip_code_to) + Environment.NewLine);
                    sb.Append("      ,PREFECTURE_ID = " + entity.prefecture_id + Environment.NewLine);
                    sb.Append("      ,CITY_ID = " + entity.city_id + Environment.NewLine);
                    sb.Append("      ,TOWN_ID = " + entity.town_id + Environment.NewLine);
                    sb.Append("      ,ADRESS_CITY = " + ExEscape.zRepStr(entity.adress_city) + Environment.NewLine);
                    sb.Append("      ,ADRESS_TOWN = " + ExEscape.zRepStr(entity.adress_town) + Environment.NewLine);
                    sb.Append("      ,ADRESS1 = " + ExEscape.zRepStr(entity.adress1) + Environment.NewLine);
                    sb.Append("      ,ADRESS2 = " + ExEscape.zRepStr(entity.adress2) + Environment.NewLine);
                    sb.Append("      ,STATION_NAME = " + ExEscape.zRepStr(entity.station_name) + Environment.NewLine);
                    sb.Append("      ,POST_NAME = " + ExEscape.zRepStr(entity.post_name) + Environment.NewLine);
                    sb.Append("      ,PERSON_NAME = " + ExEscape.zRepStr(entity.person_name) + Environment.NewLine);
                    sb.Append("      ,TITLE_ID = " + entity.title_id + Environment.NewLine);
                    sb.Append("      ,TITLE_NAME = " + ExEscape.zRepStr(entity.title_name) + Environment.NewLine);
                    sb.Append("      ,TEL = " + ExEscape.zRepStr(entity.tel) + Environment.NewLine);
                    sb.Append("      ,FAX = " + ExEscape.zRepStr(entity.fax) + Environment.NewLine);
                    sb.Append("      ,MAIL_ADRESS = " + ExEscape.zRepStr(entity.mail_adress) + Environment.NewLine);
                    sb.Append("      ,MOBILE_TEL = " + ExEscape.zRepStr(entity.mobile_tel) + Environment.NewLine);
                    sb.Append("      ,MOBILE_ADRESS = " + ExEscape.zRepStr(entity.mobile_adress) + Environment.NewLine);
                    sb.Append("      ,URL = " + ExEscape.zRepStr(entity.url) + Environment.NewLine);
                    sb.Append("      ,DIVIDE_PERMISSION_ID = " + entity.divide_permission_id + Environment.NewLine);
                    sb.Append("      ,MEMO = " + ExEscape.zRepStr(entity.memo) + Environment.NewLine);
                    sb.Append("      ,DISPLAY_FLG = " + entity.display_division_id + Environment.NewLine);
                    sb.Append(" WHERE COMPANY_ID = " + companyId + Environment.NewLine);                            // COMPANY_ID
                    sb.Append("   AND ID = " + ExEscape.zRepStr(ExCast.zNumZeroNothingFormat(Id)) + Environment.NewLine);      // ID
                    sb.Append("   AND CUSTOMER_ID = " + ExEscape.zRepStr(ExCast.zNumZeroNothingFormat(CustomerId)) + Environment.NewLine);

                    #endregion

                    db.ExecuteSQL(sb.ToString(), false);

                }
                catch (Exception ex)
                {
                    db.ExRollbackTransaction();
                    CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateSupplier(Update)", ex);
                    return CLASS_NM + ".UpdateSupplier(Insert) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
                }
            }

            #endregion

            #region Delete

            if (type == 2)
            {
                #region Exist Data

                try
                {
                    bool _ret = false;
                    _ret = DataExists.IsExistDataDouble(db, companyId, "", "T_ESTIMATE_H", "SUPPLIER_ID", ExCast.zNumZeroNothingFormat(Id), "CUSTOMER_ID", ExCast.zNumZeroNothingFormat(CustomerId), CommonUtl.geStrOrNumKbn.String);
                    if (_ret == true)
                    {
                        return "ID : " + Id + " は見積データの納入先に使用されている為、削除できません。";
                    }

                    _ret = DataExists.IsExistDataDouble(db, companyId, "", "T_ORDER_H", "SUPPLIER_ID", ExCast.zNumZeroNothingFormat(Id), "CUSTOMER_ID", ExCast.zNumZeroNothingFormat(CustomerId), CommonUtl.geStrOrNumKbn.String);
                    if (_ret == true)
                    {
                        return "ID : " + Id + " は受注データの納入先に使用されている為、削除できません。";
                    }

                    _ret = DataExists.IsExistDataDouble(db, companyId, "", "T_SALES_H", "SUPPLIER_ID", ExCast.zNumZeroNothingFormat(Id), "CUSTOMER_ID", ExCast.zNumZeroNothingFormat(CustomerId), CommonUtl.geStrOrNumKbn.String);
                    if (_ret == true)
                    {
                        return "ID : " + Id + " は売上データの納入先に使用されている為、削除できません。";
                    }
                }
                catch (Exception ex)
                {
                    db.ExRollbackTransaction();
                    CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateSupplier(Exist Data)", ex);
                    return CLASS_NM + ".UpdateSupplier(Exist Data) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
                }

                #endregion

                #region Update

                try
                {
                    sb.Length = 0;
                    sb.Append("UPDATE M_SUPPLIER " + Environment.NewLine);
                    sb.Append(CommonUtl.GetUpdSQLCommonColums(PG_NM,
                                                               ExCast.zCInt(personId),
                                                               ipAdress,
                                                               userId,
                                                               1));
                    sb.Append(" WHERE COMPANY_ID = " + companyId + Environment.NewLine);    // COMPANY_ID
                    sb.Append("   AND ID = " + ExEscape.zRepStr(ExCast.zNumZeroNothingFormat(Id)) + Environment.NewLine);            // ID
                    sb.Append("   AND CUSTOMER_ID = " + ExEscape.zRepStr(ExCast.zNumZeroNothingFormat(CustomerId)) + Environment.NewLine);

                    db.ExecuteSQL(sb.ToString(), false);

                }
                catch (Exception ex)
                {
                    db.ExRollbackTransaction();
                    CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateSupplier(Delete)", ex);
                    return CLASS_NM + ".UpdateSupplier(Delete) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
                }

                #endregion
            }

            #endregion

            #region PG排他制御

            if (type == 0 || type == 2)
            {
                try
                {
                    DataPgLock.DelLockPg(companyId, userId, PG_NM, "", ipAdress, false, db);
                }
                catch (Exception ex)
                {
                    db.ExRollbackTransaction();
                    CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateSupplier(DelLockPg)", ex);
                    return CLASS_NM + ".UpdateSupplier(DelLockPg) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
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
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateSupplier(CommitTransaction)", ex);
                return CLASS_NM + ".UpdateSupplier(CommitTransaction) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
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
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateSupplier(DbClose)", ex);
                return CLASS_NM + ".UpdateSupplier(DbClose) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
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
                                                   PG_NM,
                                                   DataPgEvidence.geOperationType.Update,
                                                   "ID:" + Id.ToString());
                        break;
                    case 1:
                        svcPgEvidence.gAddEvidence(ExCast.zCInt(HttpContext.Current.Session[ExSession.EVIDENCE_SAVE_FLG]),
                                                   companyId,
                                                   userId,
                                                   ipAdress,
                                                   sessionString,
                                                   PG_NM,
                                                   DataPgEvidence.geOperationType.Insert,
                                                   "ID:" + _Id.ToString());
                        break;
                    case 2:
                        svcPgEvidence.gAddEvidence(ExCast.zCInt(HttpContext.Current.Session[ExSession.EVIDENCE_SAVE_FLG]),
                                                   companyId,
                                                   userId,
                                                   ipAdress,
                                                   sessionString,
                                                   PG_NM,
                                                   DataPgEvidence.geOperationType.Delete,
                                                   "ID:" + Id.ToString());
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateSupplier(Add Evidence)", ex);
                return CLASS_NM + ".UpdateSupplier(Add Evidence) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
            }

            #endregion

            #region Return

            if (type == 1 && (Id == "0" || Id == ""))
            {
                return "Auto Insert success : " + "ID : " + _Id.ToString() + "で登録しました。";
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
