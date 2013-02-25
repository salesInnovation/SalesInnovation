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
    public class svcAuthority
    {
        private const string Authority_NM = "svcAuthority";
        private readonly string PG_NM = DataPgEvidence.PGName.Mst.Authority;

        #region データ取得

        /// <summary> 
        /// データ取得
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebMethod(EnableSession = true)]
        public List<EntityAuthority> GetAuthority(string random, int _user_id)
        {

            EntityAuthority entity;
            List<EntityAuthority> entityList = new List<EntityAuthority>();

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
                    entity = new EntityAuthority();
                    entity.MESSAGE = _message;
                    entityList.Add(entity);
                    return entityList;
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(Authority_NM + ".GetAuthority(認証処理)", ex);
                entity = new EntityAuthority();
                entity.MESSAGE = Authority_NM + ".GetAuthority : 認証処理に失敗しました。" + Environment.NewLine + ex.Message.ToString(); ;
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

                sb.Append("SELECT IFNULL(AR.USER_ID, " + _user_id.ToString() + ") AS USER_ID" + Environment.NewLine);
                sb.Append("      ,MT.ID AS PG_ID " + Environment.NewLine);
                sb.Append("      ,IFNULL(AR.AUTHORITY_KBN, 0) AS AUTHORITY_KBN" + Environment.NewLine);
                sb.Append("      ,IFNULL(AR.MEMO, '') AS MEMO" + Environment.NewLine);
                sb.Append("      ,IFNULL(AR.DISPLAY_INDEX, 0) AS DISPLAY_INDEX" + Environment.NewLine);
                sb.Append("  FROM SYS_M_PG AS MT" + Environment.NewLine);

                #region Join

                sb.Append("  LEFT JOIN M_AUTHORITY AS AR" + Environment.NewLine);
                sb.Append("    ON AR.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND AR.DISPLAY_FLG = 1 " + Environment.NewLine);
                sb.Append("   AND AR.USER_ID = " + _user_id + Environment.NewLine);
                sb.Append("   AND AR.PG_ID = MT.ID " + Environment.NewLine);

                #endregion

                sb.Append(" WHERE MT.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND MT.DISPLAY_FLG = 1 " + Environment.NewLine);

                #endregion

                dt = db.GetDataTable(sb.ToString());

                // 排他制御
                DataPgLock.geLovkFlg lockFlg;
                string strErr = DataPgLock.SetLockPg(companyId, userId, PG_NM, _user_id.ToString(), ipAdress, db, out lockFlg);

                if (strErr != "")
                {
                    entity = new EntityAuthority();
                    entity.MESSAGE = Authority_NM + ".GetAuthority : 排他制御(ロック情報取得)に失敗しました。" + Environment.NewLine + strErr;
                    entityList.Add(entity);
                    return entityList;
                }

                if (dt.DefaultView.Count > 0)
                {
                    for (int i = 0; i <= dt.DefaultView.Count - 1; i++)
                    {
                        #region Set Entity

                        entity = new EntityAuthority();
                        entity.user_id = ExCast.zCInt(dt.DefaultView[i]["USER_ID"]);
                        entity.pg_id = ExCast.zCStr(dt.DefaultView[i]["PG_ID"]);
                        entity.authority_kbn = ExCast.zCInt(dt.DefaultView[i]["AUTHORITY_KBN"]);
                        entity.display_index = ExCast.zCInt(dt.DefaultView[i]["DISPLAY_INDEX"]);

                        entity.memo = ExCast.zCStr(dt.DefaultView[i]["MEMO"]);

                        entity.lock_flg = (int)lockFlg;

                        if (entity.pg_id == "ReportTotal" && _user_id == ExCast.zCInt(userId))
                        {
                            HttpContext.Current.Session[ExSession.REPORT_TOTAL_AUTHORITY_KBN] = entity.authority_kbn;
                        }

                        entityList.Add(entity);

                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(Authority_NM + ".GetAuthority", ex);
                entityList.Clear();
                entity = new EntityAuthority();
                entity.MESSAGE = Authority_NM + ".GetAuthority : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message.ToString();
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
        public string UpdateAuthority(string random, List<EntityAuthority> entity, int _user_id)
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
                CommonUtl.ExLogger.Error(Authority_NM + ".UpdateAuthority(認証処理)", ex);
                return Authority_NM + ".UpdateAuthority : 認証処理に失敗しました。" + Environment.NewLine + ex.Message.ToString();
            }

            #endregion

            #region Field

            StringBuilder sb = new StringBuilder();
            DataTable dt;
            ExMySQLData db = null;
            string str_authority_kbn = "";

            #endregion

            #region 権限不可ユーザ0件チェック

            try
            {
                bool _flg = true;
                for (int i = 0; i <= entity.Count - 1; i++)
                {
                    if (entity[i].authority_kbn < 2 && entity[i].pg_id == "AuthorityMst")
                    {
                        _flg = false;
                    }
                }
                if (_flg == false)
                {
                    ExMySQLData dbSelect = ExSession.GetSessionDb(ExCast.zCInt(HttpContext.Current.Session[ExSession.USER_ID]),
                                                                  ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]));

                    #region SQL

                    sb.Append("SELECT MT.USER_ID " + Environment.NewLine);
                    sb.Append("  FROM M_AUTHORITY AS MT" + Environment.NewLine);

                    #region Join

                    sb.Append("  INNER JOIN SYS_M_USER AS UR" + Environment.NewLine);
                    sb.Append("    ON UR.DELETE_FLG = 0 " + Environment.NewLine);
                    sb.Append("   AND UR.DISPLAY_FLG = 1 " + Environment.NewLine);
                    sb.Append("   AND UR.COMPANY_ID = " + companyId + Environment.NewLine);
                    sb.Append("   AND UR.ID = MT.USER_ID " + Environment.NewLine);
                    sb.Append("   AND UR.ID <> " + _user_id + Environment.NewLine);

                    #endregion

                    sb.Append(" WHERE MT.DELETE_FLG = 0 " + Environment.NewLine);
                    sb.Append("   AND MT.PG_ID = 'AuthorityMst' " + Environment.NewLine);
                    sb.Append("   AND MT.AUTHORITY_KBN >= 2 " + Environment.NewLine);

                    #endregion

                    dt = dbSelect.GetDataTable(sb.ToString());

                    if (dt.DefaultView.Count == 0)
                    {
                        return "権限を付加できるユーザが0件になる為、更新できません。";
                    }
                    
                    dbSelect = null;
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(Authority_NM + ".UpdateAuthority(Check)", ex);
                return Authority_NM + ".UpdateAuthority(Check) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
            }

            #endregion

            #region Databese Open

            try
            {
                db = new ExMySQLData(ExCast.zCStr(HttpContext.Current.Session[ExSession.DB_CONNECTION_STR]));
                db.DbOpen();
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(Authority_NM + ".UpdateAuthority(DbOpen)", ex);
                return Authority_NM + ".UpdateAuthority(DbOpen) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
            }

            #endregion

            #region BeginTransaction

            try
            {
                db.ExBeginTransaction();
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(Authority_NM + ".UpdateAuthority(BeginTransaction)", ex);
                return Authority_NM + ".UpdateAuthority(BeginTransaction) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
            }

            #endregion

            #region Delete

            try
            {
                #region Delete SQL

                sb.Length = 0;
                sb.Append("DELETE FROM M_AUTHORITY " + Environment.NewLine);
                sb.Append(" WHERE USER_ID = " + _user_id + Environment.NewLine);

                #endregion

                db.ExecuteSQL(sb.ToString(), false);

            }
            catch (Exception ex)
            {
                db.ExRollbackTransaction();
                CommonUtl.ExLogger.Error(Authority_NM + ".UpdateAuthority(Delete)", ex);
                return Authority_NM + ".UpdateAuthority(Delete) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
            }

            #endregion

            #region Insert

            try
            {
                for (int i = 0; i <= entity.Count - 1; i++)
                {
                    #region Insert SQL

                    sb.Length = 0;
                    sb.Append("INSERT INTO M_AUTHORITY " + Environment.NewLine);
                    sb.Append("       ( USER_ID" + Environment.NewLine);
                    sb.Append("       , PG_ID" + Environment.NewLine);
                    sb.Append("       , AUTHORITY_KBN" + Environment.NewLine);
                    sb.Append("       , MEMO" + Environment.NewLine);
                    sb.Append("       , DISPLAY_INDEX" + Environment.NewLine);
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
                    sb.Append("SELECT  " + _user_id + Environment.NewLine);                                 // USER_ID
                    sb.Append("       ," + ExEscape.zRepStr(entity[i].pg_id) + Environment.NewLine);        // PG_ID
                    sb.Append("       ," + entity[i].authority_kbn + Environment.NewLine);                  // AUTHORITY_KBN
                    sb.Append("       ," + ExEscape.zRepStr(entity[i].memo) + Environment.NewLine);         // MEMO
                    sb.Append("       ," + entity[i].display_index + Environment.NewLine);                  // DISPLAY_INDEX
                    sb.Append("       ,1" + Environment.NewLine);                                           // DISPLAY_FLG
                    sb.Append(CommonUtl.GetInsSQLCommonColums(CommonUtl.UpdKbn.Ins,
                                                                PG_NM,
                                                                "M_AUTHORITY",
                                                                ExCast.zCInt(personId),
                                                                _user_id.ToString(),
                                                                ipAdress,
                                                                userId));

                    #endregion

                    db.ExecuteSQL(sb.ToString(), false);

                    if (entity[i].pg_id == "ReportTotal" && _user_id == ExCast.zCInt(userId))
                    {
                        str_authority_kbn = ExCast.zCStr(entity[i].authority_kbn);
                        HttpContext.Current.Session[ExSession.REPORT_TOTAL_AUTHORITY_KBN] = entity[i].authority_kbn;
                    }
                }
            }
            catch (Exception ex)
            {
                db.ExRollbackTransaction();
                CommonUtl.ExLogger.Error(Authority_NM + ".UpdateAuthority(Insert)", ex);
                return Authority_NM + ".UpdateAuthority(Insert) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
            }

            #endregion

            #region PG排他制御

            try
            {
                DataPgLock.DelLockPg(companyId, userId, PG_NM, "", ipAdress, false, db);
            }
            catch (Exception ex)
            {
                db.ExRollbackTransaction();
                CommonUtl.ExLogger.Error(Authority_NM + ".UpdateAuthority(DelLockPg)", ex);
                return Authority_NM + ".UpdateAuthority(DelLockPg) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
            }

            #endregion

            #region CommitTransaction

            try
            {
                db.ExCommitTransaction();
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(Authority_NM + ".UpdateAuthority(CommitTransaction)", ex);
                return Authority_NM + ".UpdateAuthority(CommitTransaction) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
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
                CommonUtl.ExLogger.Error(Authority_NM + ".UpdateAuthority(DbClose)", ex);
                return Authority_NM + ".UpdateAuthority(DbClose) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
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
                CommonUtl.ExLogger.Error(Authority_NM + ".UpdateAuthority(Add Evidence)", ex);
                return Authority_NM + ".UpdateAuthority(Add Evidence) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
            }

            #endregion

            if (str_authority_kbn != "")
            {
                HttpContext.Current.Session[ExSession.REPORT_TOTAL_AUTHORITY_KBN] = ExCast.zCInt(str_authority_kbn);
            }

            return "";

        }

        #endregion

    }
}
