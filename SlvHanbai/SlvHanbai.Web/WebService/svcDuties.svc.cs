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
    public class svcDuties
    {
        private const string CLASS_NM = "svcDuties";
        private readonly string PG_NM = DataPgEvidence.PGName.Duties.DutiesInp;

        #region データ取得

        /// <summary> 
        /// データ取得
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebMethod(EnableSession = true)]
        public List<EntityDuties> GetDuties(string random, long no, int kbn)
        {
            List<EntityDuties> entityList = new List<EntityDuties>();

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
                    EntityDuties entity = new EntityDuties();
                    entity.MESSAGE = _message;
                    entityList.Add(entity);
                    return entityList;
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetDuties(認証処理)", ex);
                EntityDuties entity = new EntityDuties();
                entity.MESSAGE = CLASS_NM + ".GetDuties : 認証処理に失敗しました。" + Environment.NewLine + ex.Message.ToString(); ;
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

                // 返答時
                sb.Append("SELECT T.COMPANY_ID " + Environment.NewLine);
                sb.Append("      ,CP.NAME AS COMPANY_NAME " + Environment.NewLine);
                sb.Append("      ,T.NO " + Environment.NewLine);
                sb.Append("      ,T.GROUP_ID " + Environment.NewLine);
                sb.Append("      ,GP.NAME AS GROUP_NAME " + Environment.NewLine);
                sb.Append("      ,T.DATE " + Environment.NewLine);
                sb.Append("      ,T.TIME " + Environment.NewLine);
                sb.Append("      ,T.PERSON_ID " + Environment.NewLine);
                sb.Append("      ,PS.NAME AS PERSON_NAME " + Environment.NewLine);
                sb.Append("      ,T.DUTIES_LEVEL_ID " + Environment.NewLine);
                sb.Append("      ,NM1.DESCRIPTION AS DUTIES_LEVEL_NAME " + Environment.NewLine);
                sb.Append("      ,T.DUTIES_STATE_ID " + Environment.NewLine);
                sb.Append("      ,NM2.DESCRIPTION AS DUTIES_STATE_NAME " + Environment.NewLine);
                sb.Append("      ,T.TITLE " + Environment.NewLine);
                sb.Append("      ,T.CONTENT " + Environment.NewLine);
                sb.Append("      ,T.UPLOAD_FILE_PATH1 " + Environment.NewLine);
                sb.Append("      ,T.UPLOAD_FILE_PATH2 " + Environment.NewLine);
                sb.Append("      ,T.UPLOAD_FILE_PATH3 " + Environment.NewLine);
                sb.Append("      ,T.UPLOAD_FILE_PATH4 " + Environment.NewLine);
                sb.Append("      ,T.UPLOAD_FILE_PATH5 " + Environment.NewLine);
                sb.Append("      ,T.MEMO " + Environment.NewLine);
                sb.Append("  FROM T_DUTIES_COMMUNICATION AS T" + Environment.NewLine);

                #region Join

                // 会社
                sb.Append("  LEFT JOIN SYS_M_COMPANY AS CP" + Environment.NewLine);
                sb.Append("    ON CP.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND CP.DISPLAY_FLG = 1 " + Environment.NewLine);
                sb.Append("   AND CP.ID = T.COMPANY_ID " + Environment.NewLine);

                // グループ
                sb.Append("  LEFT JOIN SYS_M_COMPANY_GROUP AS GP" + Environment.NewLine);
                sb.Append("    ON GP.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND GP.DISPLAY_FLG = 1 " + Environment.NewLine);
                sb.Append("   AND GP.ID = T.GROUP_ID " + Environment.NewLine);

                // 担当者
                sb.Append("  LEFT JOIN M_PERSON AS PS" + Environment.NewLine);
                sb.Append("    ON PS.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND PS.DISPLAY_FLG = 1 " + Environment.NewLine);
                sb.Append("   AND PS.COMPANY_ID = T.COMPANY_ID " + Environment.NewLine);
                sb.Append("   AND PS.ID = T.PERSON_ID" + Environment.NewLine);

                // レベルID
                sb.Append("  LEFT JOIN SYS_M_NAME AS NM1" + Environment.NewLine);
                sb.Append("    ON NM1.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND NM1.DISPLAY_FLG = 1 " + Environment.NewLine);
                sb.Append("   AND NM1.DIVISION_ID = " + (int)CommonUtl.geNameKbn.LEVEL_ID + Environment.NewLine);
                sb.Append("   AND NM1.ID = T.DUTIES_LEVEL_ID" + Environment.NewLine);

                // 状態ID
                sb.Append("  LEFT JOIN SYS_M_NAME AS NM2" + Environment.NewLine);
                sb.Append("    ON NM2.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND NM2.DISPLAY_FLG = 1 " + Environment.NewLine);
                sb.Append("   AND NM2.DIVISION_ID = " + (int)CommonUtl.geNameKbn.DISPLAY_DIVISION_ID + Environment.NewLine);
                sb.Append("   AND NM2.ID = T.DUTIES_STATE_ID" + Environment.NewLine);

                #endregion

                sb.Append(" WHERE T.COMPANY_ID = " + companyId + Environment.NewLine);
                sb.Append("   AND T.DELETE_FLG = 0 " + Environment.NewLine);
                if (no != 0)
                {
                    sb.Append("   AND T.NO = " + no + Environment.NewLine);
                }

                // 参照確認時
                if (kbn == 1)
                {
                    sb.Append("   AND T.DUTIES_STATE_ID = 1 " + Environment.NewLine);       // 表示区分 1:表示するのみ
                    sb.Append("   AND (T.GROUP_ID = " + groupId + " OR T.GROUP_ID = 0)" + Environment.NewLine);
                }

                sb.Append(" ORDER BY T.DATE DESC " + Environment.NewLine);
                sb.Append("         ,T.TIME DESC " + Environment.NewLine);
                sb.Append("         ,T.NO DESC " + Environment.NewLine);

                #endregion

                dt = db.GetDataTable(sb.ToString());

                if (dt.DefaultView.Count > 0)
                {
                    for (int i = 0; i <= dt.DefaultView.Count - 1; i++)
                    {
                        EntityDuties entity = new EntityDuties();

                        // 排他制御
                        DataPgLock.geLovkFlg lockFlg = DataPgLock.geLovkFlg.UnLock;
                        if (no != 0)    // 返答時
                        {
                            string strErr = DataPgLock.SetLockPg(companyId, userId, PG_NM, no.ToString(), ipAdress, db, out lockFlg);
                            if (strErr != "")
                            {
                                entity.MESSAGE = CLASS_NM + ".GetDuties : 排他制御(ロック情報取得)に失敗しました。" + Environment.NewLine + strErr;
                            }
                        }

                        #region Set Entity

                        entity.company_id = ExCast.zCInt(dt.DefaultView[i]["COMPANY_ID"]);
                        entity.company_nm = ExCast.zCStr(dt.DefaultView[i]["COMPANY_NAME"]);
                        entity.no = ExCast.zCLng(dt.DefaultView[i]["NO"]);
                        entity.gropu_id = ExCast.zCInt(dt.DefaultView[i]["GROUP_ID"]);
                        entity.gropu_nm = ExCast.zCStr(dt.DefaultView[i]["GROUP_NAME"]);
                        if (entity.gropu_id == 0)
                        {
                            entity.gropu_nm = "全グループ";
                        }

                        entity.duties_ymd = ExCast.zDateNullToDefault(dt.DefaultView[i]["DATE"]);
                        entity.duties_time = ExCast.zCStr(dt.DefaultView[i]["TIME"]);
                        entity.duties_date_time = ExCast.zDateNullToDefault(dt.DefaultView[i]["DATE"]) + " " + ExCast.zCStr(dt.DefaultView[i]["TIME"]);
                        entity.person_id = ExCast.zCInt(dt.DefaultView[i]["PERSON_ID"]);
                        entity.person_nm = ExCast.zCStr(dt.DefaultView[i]["PERSON_NAME"]);
                        entity.duties_level_id = ExCast.zCInt(dt.DefaultView[i]["DUTIES_LEVEL_ID"]);
                        entity.duties_level_nm = ExCast.zCStr(dt.DefaultView[i]["DUTIES_LEVEL_NAME"]);
                        entity.duties_state_id = ExCast.zCInt(dt.DefaultView[i]["DUTIES_STATE_ID"]);
                        entity.duties_state_nm = ExCast.zCStr(dt.DefaultView[i]["DUTIES_STATE_NAME"]);
                        entity.title = ExCast.zCStr(dt.DefaultView[i]["TITLE"]);
                        entity.content = ExCast.zCStr(dt.DefaultView[i]["CONTENT"]);

                        entity.upload_file_path1 = ExCast.zCStr(dt.DefaultView[i]["UPLOAD_FILE_PATH1"]);
                        entity.upload_file_path2 = ExCast.zCStr(dt.DefaultView[i]["UPLOAD_FILE_PATH2"]);
                        entity.upload_file_path3 = ExCast.zCStr(dt.DefaultView[i]["UPLOAD_FILE_PATH3"]);
                        entity.upload_file_path4 = ExCast.zCStr(dt.DefaultView[i]["UPLOAD_FILE_PATH4"]);
                        entity.upload_file_path5 = ExCast.zCStr(dt.DefaultView[i]["UPLOAD_FILE_PATH5"]);

                        if (!string.IsNullOrEmpty(entity.upload_file_path1)) entity.upload_file_name1 = System.IO.Path.GetFileName(entity.upload_file_path1);
                        if (!string.IsNullOrEmpty(entity.upload_file_path2)) entity.upload_file_name2 = System.IO.Path.GetFileName(entity.upload_file_path2);
                        if (!string.IsNullOrEmpty(entity.upload_file_path3)) entity.upload_file_name3 = System.IO.Path.GetFileName(entity.upload_file_path3);
                        if (!string.IsNullOrEmpty(entity.upload_file_path4)) entity.upload_file_name4 = System.IO.Path.GetFileName(entity.upload_file_path4);
                        if (!string.IsNullOrEmpty(entity.upload_file_path5)) entity.upload_file_name5 = System.IO.Path.GetFileName(entity.upload_file_path5);

                        entity.memo = ExCast.zCStr(dt.DefaultView[i]["MEMO"]);

                        #endregion

                        entityList.Add(entity);

                    }
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetDuties", ex);
                entityList.Clear();
                EntityDuties entity = new EntityDuties();
                entity.MESSAGE = CLASS_NM + ".GetDuties : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message.ToString(); ;
                entityList.Add(entity);
                return entityList;
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
                           "NO:" + no.ToString());

            return entityList;

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
        public string UpdateDuties(string random, int type, long no, EntityDuties entity)
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
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateDuties(認証処理)", ex);
                return CLASS_NM + ".UpdateDuties : 認証処理に失敗しました。" + Environment.NewLine + ex.Message.ToString();
            }

            #endregion

            #region Field

            StringBuilder sb = new StringBuilder();
            DataTable dt;
            ExMySQLData db = null;
            string _no = "";

            DateTime now = DateTime.Now;
            string date = now.ToString("yyyy/MM/dd");
            string time = now.ToString("HH:mm:ss");

            #endregion

            #region Databese Open

            try
            {
                db = new ExMySQLData(ExCast.zCStr(HttpContext.Current.Session[ExSession.DB_CONNECTION_STR]));
                db.DbOpen();
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateDuties(DbOpen)", ex);
                return CLASS_NM + ".UpdateDuties(DbOpen) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
            }

            #endregion

            #region BeginTransaction

            try
            {
                db.ExBeginTransaction();
                if (CommonUtl.gSysDbKbn == 0) db.ExBeginTransaction();
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateDuties(BeginTransaction)", ex);
                return CLASS_NM + ".UpdateDuties(BeginTransaction) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
            }

            #endregion

            #region Get Max ID

            if (type == 1)
            {
                try
                {
                    DataMaxId.GetMaxId(db,
                                       "T_DUTIES_COMMUNICATION",
                                       "NO",
                                       out _no);

                    if (_no == "")
                    {
                        return "業務連絡NOの取得に失敗しました。";
                    }
                }
                catch (Exception ex)
                {
                    db.ExRollbackTransaction();
                    CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateDuties(GetMaxId)", ex);
                    return CLASS_NM + ".UpdateDuties(GetMaxId) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
                }
            }
            else
            {
                _no = no.ToString();
            }

            #endregion

            #region Upload File Set

            if (type <= 1)
            {
                if (!string.IsNullOrEmpty(entity.upload_file_path1))
                {
                    if (System.IO.Directory.Exists(CommonUtl.gstrFileUpLoadDutiesDir + entity.upload_file_path1) && !System.IO.Directory.Exists(CommonUtl.gstrFileUpLoadDutiesDir + _no + "-" + date.Replace("/", "") + time.Replace(":", "")))
                    {
                        System.IO.Directory.Move(CommonUtl.gstrFileUpLoadDutiesDir + entity.upload_file_path1, CommonUtl.gstrFileUpLoadDutiesDir + _no + "-" + date.Replace("/", "") + time.Replace(":", ""));
                        entity.upload_file_path1 = CommonUtl.gstrFileUpLoadDutiesDir + _no + "-" + date.Replace("/", "") + time.Replace(":", "") + "\\" + entity.upload_file_name1;
                    }
                }
            }

            #endregion

            #region Insert

            if (type == 1)
            {
                try
                {
                    #region Insert SQL

                    sb.Length = 0;
                    sb.Append("INSERT INTO T_DUTIES_COMMUNICATION " + Environment.NewLine);
                    sb.Append("       ( COMPANY_ID" + Environment.NewLine);
                    sb.Append("       , NO" + Environment.NewLine);
                    sb.Append("       , GROUP_ID" + Environment.NewLine);
                    sb.Append("       , DATE" + Environment.NewLine);
                    sb.Append("       , TIME" + Environment.NewLine);
                    sb.Append("       , PERSON_ID" + Environment.NewLine);
                    sb.Append("       , DUTIES_LEVEL_ID" + Environment.NewLine);
                    sb.Append("       , DUTIES_STATE_ID" + Environment.NewLine);
                    sb.Append("       , TITLE" + Environment.NewLine);
                    sb.Append("       , CONTENT" + Environment.NewLine);
                    sb.Append("       , UPLOAD_FILE_PATH1" + Environment.NewLine);
                    sb.Append("       , UPLOAD_FILE_PATH2" + Environment.NewLine);
                    sb.Append("       , UPLOAD_FILE_PATH3" + Environment.NewLine);
                    sb.Append("       , UPLOAD_FILE_PATH4" + Environment.NewLine);
                    sb.Append("       , UPLOAD_FILE_PATH5" + Environment.NewLine);
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
                    sb.Append("SELECT  " + companyId + Environment.NewLine);                                        // COMPANY_ID
                    sb.Append("       ," + _no + Environment.NewLine);                                              // NO
                    sb.Append("       ," + entity.gropu_id + Environment.NewLine);                                  // GROUP_ID
                    sb.Append("       ," + ExEscape.zRepStr(date) + Environment.NewLine);                           // DATE
                    sb.Append("       ," + ExEscape.zRepStr(time) + Environment.NewLine);                           // TIME
                    sb.Append("       ," + entity.person_id + Environment.NewLine);                                 // PERSON_ID
                    sb.Append("       ," + entity.duties_level_id + Environment.NewLine);                           // DUTIES_LEVEL_ID
                    sb.Append("       ," + entity.duties_state_id + Environment.NewLine);                           // DUTIES_STATE_ID
                    sb.Append("       ," + ExEscape.zRepStr(entity.title) + Environment.NewLine);                   // TITLE
                    sb.Append("       ," + ExEscape.zRepStr(entity.content) + Environment.NewLine);                 // CONTENT
                    sb.Append("       ," + ExEscape.zRepStr(entity.upload_file_path1) + Environment.NewLine);       // UPLOAD_FILE_PATH1
                    sb.Append("       ," + ExEscape.zRepStr(entity.upload_file_path2) + Environment.NewLine);       // UPLOAD_FILE_PATH2
                    sb.Append("       ," + ExEscape.zRepStr(entity.upload_file_path3) + Environment.NewLine);       // UPLOAD_FILE_PATH3
                    sb.Append("       ," + ExEscape.zRepStr(entity.upload_file_path4) + Environment.NewLine);       // UPLOAD_FILE_PATH4
                    sb.Append("       ," + ExEscape.zRepStr(entity.upload_file_path5) + Environment.NewLine);       // UPLOAD_FILE_PATH5
                    sb.Append("       ," + ExEscape.zRepStr(entity.memo) + Environment.NewLine);                    // MEMO
                    sb.Append(CommonUtl.GetInsSQLCommonColums(CommonUtl.UpdKbn.Ins,
                                                                PG_NM,
                                                                "T_DUTIES_COMMUNICATION",
                                                                ExCast.zCInt(personId),
                                                                _no,
                                                                ipAdress,
                                                                userId));

                    #endregion

                    db.ExecuteSQL(sb.ToString(), false);
                    if (CommonUtl.gSysDbKbn == 0) db.ExecuteSQL(sb.ToString(), false);

                }
                catch (Exception ex)
                {
                    db.ExRollbackTransaction();
                    CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateDuties(Insert)", ex);
                    return CLASS_NM + ".UpdateDuties(Insert) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
                }
            }

            #endregion

            #region Update

            if (type == 0)
            {
                #region Update

                try
                {
                    #region SQL

                    sb.Length = 0;
                    sb.Append("UPDATE T_DUTIES_COMMUNICATION " + Environment.NewLine);
                    sb.Append(CommonUtl.GetUpdSQLCommonColums(PG_NM,
                                                               ExCast.zCInt(personId),
                                                               ipAdress,
                                                               userId,
                                                               0));
                    sb.Append("      ,GROUP_ID = " + entity.gropu_id + Environment.NewLine);
                    sb.Append("      ,DATE = " + ExEscape.zRepStr(date) + Environment.NewLine);
                    sb.Append("      ,TIME = " + ExEscape.zRepStr(time) + Environment.NewLine);
                    sb.Append("      ,PERSON_ID = " + entity.person_id + Environment.NewLine);
                    sb.Append("      ,DUTIES_LEVEL_ID = " + entity.duties_level_id + Environment.NewLine);
                    sb.Append("      ,DUTIES_STATE_ID = " + entity.duties_state_id + Environment.NewLine);
                    sb.Append("      ,TITLE = " + ExEscape.zRepStr(entity.title) + Environment.NewLine);
                    sb.Append("      ,CONTENT = " + ExEscape.zRepStr(entity.content) + Environment.NewLine);
                    sb.Append("      ,UPLOAD_FILE_PATH1 = " + ExEscape.zRepStr(entity.upload_file_path1) + Environment.NewLine);
                    sb.Append("      ,UPLOAD_FILE_PATH2 = " + ExEscape.zRepStr(entity.upload_file_path2) + Environment.NewLine);
                    sb.Append("      ,UPLOAD_FILE_PATH3 = " + ExEscape.zRepStr(entity.upload_file_path3) + Environment.NewLine);
                    sb.Append("      ,UPLOAD_FILE_PATH4 = " + ExEscape.zRepStr(entity.upload_file_path4) + Environment.NewLine);
                    sb.Append("      ,UPLOAD_FILE_PATH5 = " + ExEscape.zRepStr(entity.upload_file_path5) + Environment.NewLine);
                    sb.Append("      ,MEMO = " + ExEscape.zRepStr(entity.memo) + Environment.NewLine);
                    sb.Append(" WHERE NO = " + _no + Environment.NewLine);

                    #endregion

                    db.ExecuteSQL(sb.ToString(), false);
                    if (CommonUtl.gSysDbKbn == 0) db.ExecuteSQL(sb.ToString(), false);

                }
                catch (Exception ex)
                {
                    db.ExRollbackTransaction();
                    CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateDuties(Update Head)", ex);
                    return CLASS_NM + ".UpdateDuties(Update Head) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
                }

                #endregion
            }

            #endregion

            #region Delete

            if (type == 2)
            {
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
                    CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateDuties(DelLockPg)", ex);
                    return CLASS_NM + ".UpdateDuties(DelLockPg) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
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
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateDuties(CommitTransaction)", ex);
                return CLASS_NM + ".UpdateDuties(CommitTransaction) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
            }

            #endregion

            #region Database Close

            try
            {
                db.DbClose();
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateDuties(DbClose)", ex);
                return CLASS_NM + ".UpdateDuties(DbClose) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
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
                                                   "NO:" + _no);
                        break;
                    case 1:
                        svcPgEvidence.gAddEvidence(ExCast.zCInt(HttpContext.Current.Session[ExSession.EVIDENCE_SAVE_FLG]),
                                                   companyId,
                                                   userId,
                                                   ipAdress,
                                                   sessionString,
                                                   PG_NM,
                                                   DataPgEvidence.geOperationType.Insert,
                                                   "NO:" + _no);
                        break;
                    case 2:
                        svcPgEvidence.gAddEvidence(ExCast.zCInt(HttpContext.Current.Session[ExSession.EVIDENCE_SAVE_FLG]),
                                                   companyId,
                                                   userId,
                                                   ipAdress,
                                                   sessionString,
                                                   PG_NM,
                                                   DataPgEvidence.geOperationType.Delete,
                                                   "NO:" + _no);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateDuties(Add Evidence)", ex);
                return CLASS_NM + ".UpdateDuties(Add Evidence) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
            }

            #endregion

            #region Return

            if (type == 1)
            {
                return "Auto Insert success : " + "NO : " + _no + "で登録しました。";
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
