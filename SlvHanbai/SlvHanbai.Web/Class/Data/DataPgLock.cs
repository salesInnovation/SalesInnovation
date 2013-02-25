using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Data;
using SlvHanbai.Web.Class.DB;
using SlvHanbai.Web.Class.Utility;
namespace SlvHanbai.Web.Class.Data
{
    public class DataPgLock
    {
        private const string CLASS_NM = "DataPgLock";

        public enum geLovkFlg
        {
            UnLock = 0,
            Lock
        }

        public static string SetLockPg(string companyId,
                                     string userId,
                                     string pgId,
                                     string lockId,
                                     string ipAdress,
                                     ExMySQLData _db,
                                     out geLovkFlg lockFlg)
        {
            StringBuilder sb;
            DataTable dt;
            ExMySQLData db = null;

            string ret = "err";
            lockFlg = geLovkFlg.UnLock;

            if (CommonUtl.gDemoKbn == 1)
            {
                lockFlg = DataPgLock.geLovkFlg.UnLock;
                return "";
            }

            #region Databese Open

            try
            {
                db = new ExMySQLData(ExCast.zCStr(HttpContext.Current.Session[ExSession.DB_CONNECTION_STR]));
                db.DbOpen();
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".SetLockPg(DbOpen)", ex);
                return CLASS_NM + ".SetLockPg(DbOpen) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
            }

            #endregion

            #region BeginTransaction

            try
            {
                db.ExBeginTransaction();
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".SetLockPg(BeginTransaction)", ex);
                return CLASS_NM + ".SetLockPg(BeginTransaction) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
            }

            #endregion

            try
            {
                sb = new StringBuilder();
                sb.Length = 0;

                ret = DataPgLock.DelLockPg(companyId, userId, pgId, "", ipAdress, false, db);
                if (ret != "") return ret;

                #region SQL

                // 存在確認
                sb.Append("SELECT TBL.* " + Environment.NewLine);
                sb.Append("  FROM S_PG_LOCK AS TBL" + Environment.NewLine);
                sb.Append(" WHERE TBL.COMPANY_ID = " + companyId + Environment.NewLine);
                sb.Append("   AND TBL.PG_ID = " + ExEscape.zRepStr(pgId) + Environment.NewLine);
                sb.Append("   AND TBL.LOCK_ID = " + ExEscape.zRepStr(lockId) + Environment.NewLine);
                sb.Append(" LIMIT 0, 1" + Environment.NewLine);
                sb.Append(" FOR UPDATE");

                #endregion

                dt = db.GetDataTable(sb.ToString());

                if (dt.DefaultView.Count > 0)
                {
                    if (!string.IsNullOrEmpty(userId))
                    {
                        if (userId != ExCast.zCStr(dt.DefaultView[0]["USER_ID"]))
                        {
                            lockFlg = DataPgLock.geLovkFlg.Lock;
                        }
                        else
                        {
                            lockFlg = DataPgLock.geLovkFlg.UnLock;
                        }
                    }
                    else
                    {
                        lockFlg = DataPgLock.geLovkFlg.Lock;
                    }
                }
                else
                {
                    #region SQL

                    sb.Length = 0;
                    sb.Append("INSERT INTO S_PG_LOCK " + Environment.NewLine);
                    sb.Append("       ( COMPANY_ID" + Environment.NewLine);
                    sb.Append("       , USER_ID" + Environment.NewLine);
                    sb.Append("       , PG_ID" + Environment.NewLine);
                    sb.Append("       , LOCK_ID" + Environment.NewLine);
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
                    sb.Append("VALUES (" + Environment.NewLine);
                    sb.Append("        " + companyId + Environment.NewLine);            // COMPANY_ID
                    sb.Append("       ," + userId + Environment.NewLine);               // USER_ID
                    sb.Append("       ," + ExEscape.zRepStr(pgId) + Environment.NewLine);          // PG_ID
                    sb.Append("       ," + ExEscape.zRepStr(lockId) + Environment.NewLine);        // LOCK_ID
                    sb.Append(CommonUtl.GetInsSQLCommonColums(CommonUtl.UpdKbn.Ins, "pgId", "S_PG_LOCK", 9999, "0", ipAdress, userId));
                    sb.Append(")" + Environment.NewLine);

                    #endregion

                    db.ExecuteSQL(sb.ToString(), false);

                    lockFlg = DataPgLock.geLovkFlg.UnLock;
                }

                ret = "";
            }
            catch (Exception ex)
            {
                db.ExRollbackTransaction();
                ret = CLASS_NM + ".SetLockPg(Insert) : " + ex.Message;
                CommonUtl.ExLogger.Error(CLASS_NM + ".SetLockPg(Insert)", ex);
                throw;
            }

            #region CommitTransaction

            try
            {
                db.ExCommitTransaction();
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".SetLockPg(CommitTransaction)", ex);
                return CLASS_NM + ".SetLockPg(CommitTransaction) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
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
                CommonUtl.ExLogger.Error(CLASS_NM + ".SetLockPg(DbClose)", ex);
                return CLASS_NM + ".SetLockPg(DbClose) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
            }
            finally
            {
                db = null;
            }

            #endregion

            return ret;
        }

        public static string DelLockPg(string companyId,
                                       string userId,
                                       string pgId,
                                       string lockId,
                                       string ipAdress,
                                       bool commitFlg,
                                       ExMySQLData _db)
        {
            StringBuilder sb;
            DataTable dt;
            ExMySQLData db = null;

            string ret = "err";

            #region Databese Open

            try
            {
                db = new ExMySQLData(ExCast.zCStr(HttpContext.Current.Session[ExSession.DB_CONNECTION_STR]));
                db.DbOpen();
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".DelLockPg(DbOpen)", ex);
                return CLASS_NM + ".DelLockPg(DbOpen) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
            }

            #endregion

            #region BeginTransaction

            try
            {
                db.ExBeginTransaction();
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".DelLockPg", ex);
                return CLASS_NM + ".DelLockPg(BeginTransaction) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
            }

            #endregion

            try
            {
                sb = new StringBuilder();
                sb.Length = 0;

                sb.Append("DELETE FROM S_PG_LOCK " + Environment.NewLine);
                sb.Append(" WHERE COMPANY_ID = " + companyId + Environment.NewLine);
                sb.Append("   AND USER_ID = " + userId + Environment.NewLine);
                if (pgId != "")
                {
                    sb.Append("   AND PG_ID = " + ExEscape.zRepStr(pgId) + Environment.NewLine);
                }
                if (lockId != "")
                {
                    sb.Append("   AND LOCK_ID = " + ExEscape.zRepStr(lockId) + Environment.NewLine);
                }

                db.ExecuteSQL(sb.ToString(), false);

                ret = "";
            }
            catch (Exception ex)
            {
                db.ExRollbackTransaction();
                ret = CLASS_NM + ".DelLockPg : " + ex.Message;
                CommonUtl.ExLogger.Error(CLASS_NM + ".DelLockPg", ex);
            }

            #region CommitTransaction

            try
            {
                if (commitFlg == true) db.ExCommitTransaction();
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".DelLockPg(CommitTransaction)", ex);
                return CLASS_NM + ".DelLockPg(CommitTransaction) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
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
                CommonUtl.ExLogger.Error(CLASS_NM + ".DelLockPg(DbClose)", ex);
                return CLASS_NM + ".DelLockPg(DbClose) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
            }
            finally
            {
                db = null;
            }

            #endregion

            return ret;
        }    
    }
}