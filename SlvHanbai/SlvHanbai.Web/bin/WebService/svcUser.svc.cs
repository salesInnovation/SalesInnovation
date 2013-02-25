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
    public class svcUser
    {
        private const string CLASS_NM = "svcUser";
        private readonly string PG_NM = DataPgEvidence.PGName.Mst.User;

        #region データ取得

        /// <summary> 
        /// データ取得
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebMethod(EnableSession = true)]
        public EntityUser GetUser(string random, int Id)
        {
            EntityUser entity = null;

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
                    entity = new EntityUser();
                    entity.MESSAGE = _message;
                    return entity;
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetUserList(認証処理)", ex);
                entity = new EntityUser();
                entity.MESSAGE = "認証処理に失敗しました。" + Environment.NewLine + ex.Message.ToString(); ;
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
                sb.Append("      ,NM.DESCRIPTION AS DISPLAY_DIVISION_NAME " + Environment.NewLine);
                sb.Append("      ,SG.NAME AS GROUP_NAME " + Environment.NewLine);
                sb.Append("      ,PS.NAME AS PERSON_NAME " + Environment.NewLine);
                sb.Append("  FROM SYS_M_USER AS MT" + Environment.NewLine);

                // 表示区分
                sb.Append("  LEFT JOIN SYS_M_NAME AS NM" + Environment.NewLine);
                sb.Append("    ON NM.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND NM.DISPLAY_FLG = 1 " + Environment.NewLine);
                sb.Append("   AND NM.DIVISION_ID = " + (int)CommonUtl.geNameKbn.DISPLAY_DIVISION_ID + Environment.NewLine);
                sb.Append("   AND MT.DISPLAY_FLG = NM.ID" + Environment.NewLine);

                // グループ
                sb.Append("  LEFT JOIN SYS_M_COMPANY_GROUP AS SG" + Environment.NewLine);
                sb.Append("    ON SG.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND SG.DISPLAY_FLG = 1 " + Environment.NewLine);
                sb.Append("   AND SG.COMPANY_ID = MT.COMPANY_ID" + Environment.NewLine);
                sb.Append("   AND SG.ID = MT.GROUP_ID" + Environment.NewLine);

                // 担当
                sb.Append("  LEFT JOIN M_PERSON AS PS" + Environment.NewLine);
                sb.Append("    ON PS.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND PS.DISPLAY_FLG = 1 " + Environment.NewLine);
                sb.Append("   AND PS.COMPANY_ID = MT.COMPANY_ID" + Environment.NewLine);
                sb.Append("   AND PS.ID = MT.PERSON_ID" + Environment.NewLine);

                sb.Append(" WHERE MT.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND MT.ID = " + Id.ToString() + Environment.NewLine);

                sb.Append(" ORDER BY MT.COMPANY_ID " + Environment.NewLine);
                sb.Append("         ,MT.ID " + Environment.NewLine);

                #endregion

                dt = db.GetDataTable(sb.ToString());

                if (dt.DefaultView.Count > 0)
                {
                    entity = new EntityUser();

                    // 排他制御
                    DataPgLock.geLovkFlg lockFlg;
                    string strErr = DataPgLock.SetLockPg(companyId, userId, PG_NM, Id.ToString(), ipAdress, db, out lockFlg);
                    if (strErr != "")
                    {
                        entity.MESSAGE = CLASS_NM + ".GetUser : 排他制御(ロック情報取得)に失敗しました。" + Environment.NewLine + strErr;
                    }

                    #region Set Entity

                    entity.id = ExCast.zCInt(dt.DefaultView[0]["ID"]);
                    entity.company_id = ExCast.zCInt(dt.DefaultView[0]["COMPANY_ID"]);
                    entity.group_id = ExCast.zCInt(dt.DefaultView[0]["GROUP_ID"]);
                    entity.group_nm = ExCast.zCStr(dt.DefaultView[0]["GROUP_NAME"]);
                    entity.login_id = ExCast.zCStr(dt.DefaultView[0]["LOGIN_ID"]);
                    entity.after_login_id = ExCast.zCStr(dt.DefaultView[0]["LOGIN_ID"]);
                    entity.name = ExCast.zCStr(dt.DefaultView[0]["NAME"]);
                    entity.login_password = ExCast.zCStr(dt.DefaultView[0]["PASSWORD"]);
                    entity.person_id = ExCast.zCInt(dt.DefaultView[0]["PERSON_ID"]);
                    entity.person_nm = ExCast.zCStr(dt.DefaultView[0]["PERSON_NAME"]);
                    entity.memo = ExCast.zCStr(dt.DefaultView[0]["MEMO"]);
                    entity.display_division_id = ExCast.zCInt(dt.DefaultView[0]["DISPLAY_FLG"]);
                    entity.display_division_nm = ExCast.zCStr(dt.DefaultView[0]["DISPLAY_DIVISION_NAME"]);
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
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetUser", ex);
                entity = new EntityUser();
                entity.MESSAGE = CLASS_NM + ".GetUser : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message.ToString();
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
        public string UpdateUser(string random, int type, long Id, EntityUser entity)
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
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateUser(認証処理)", ex);
                return CLASS_NM + ".UpdateUser : 認証処理に失敗しました。" + Environment.NewLine + ex.Message.ToString();
            }

            #endregion

            #region Field

            StringBuilder sb = new StringBuilder();
            DataTable dt;
            ExMySQLData db = null;
            string _Id = "";
            _Id = Id.ToString();

            #endregion

            #region Databese Open

            try
            {
                db = new ExMySQLData();
                db.DbOpen();
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateUser(DbOpen)", ex);
                return CLASS_NM + ".UpdateUser(DbOpen) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
            }

            #endregion

            #region BeginTransaction

            try
            {
                db.ExBeginTransaction();
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateUser(BeginTransaction)", ex);
                return CLASS_NM + ".UpdateUser(BeginTransaction) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
            }

            #endregion

            #region Update

            if (type == 0)
            {
                try
                {
                    #region SQL

                    sb.Length = 0;
                    sb.Append("UPDATE SYS_M_USER " + Environment.NewLine);
                    sb.Append(CommonUtl.GetUpdSQLCommonColums(PG_NM,
                                                               ExCast.zCInt(personId),
                                                               ipAdress,
                                                               userId,
                                                               0));
                    sb.Append("      ,GROUP_ID = " + entity.group_id + Environment.NewLine);
                    sb.Append("      ,LOGIN_ID = " + ExEscape.zRepStr(entity.after_login_id) + Environment.NewLine);
                    sb.Append("      ,NAME = " + ExEscape.zRepStr(entity.name) + Environment.NewLine);
                    sb.Append("      ,PASSWORD = " + ExEscape.zRepStr(entity.login_password) + Environment.NewLine);
                    sb.Append("      ,PERSON_ID = " + entity.person_id + Environment.NewLine);
                    sb.Append("      ,MEMO = " + ExEscape.zRepStr(entity.memo) + Environment.NewLine);
                    sb.Append("      ,DISPLAY_FLG = 1" + Environment.NewLine);
                    sb.Append(" WHERE ID = " + Id.ToString() + Environment.NewLine);            // ID

                    #endregion

                    db.ExecuteSQL(sb.ToString(), false);

                }
                catch (Exception ex)
                {
                    db.ExRollbackTransaction();
                    CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateUser(Update)", ex);
                    return CLASS_NM + ".UpdateUser(Update) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
                }
            }

            #endregion

            #region Delete

            if (type == 2)
            {
                //#region Exist Data

                //try
                //{
                //    bool _ret = false;
                //    _ret = DataExists.IsExistData(db, companyId, "", "T_ORDER_H", "User_ID", Id.ToString(), CommonUtl.geStrOrNumKbn.Number);
                //    if (_ret == true)
                //    {
                //        return "担当ID : " + Id + " は受注データの入力担当者に使用されている為、削除できません。";
                //    }
                //}
                //catch (Exception ex)
                //{
                //    db.ExRollbackTransaction();
                //    CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateUser(Exist Data)", ex);
                //    return CLASS_NM + ".UpdateUser(Exist Data) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
                //}

                //#endregion

                //#region Update

                //try
                //{
                //    sb.Length = 0;
                //    sb.Append("UPDATE SYS_M_USER " + Environment.NewLine);
                //    sb.Append(CommonUtl.GetUpdSQLCommonColums(PG_NM,
                //                                               ExCast.zCInt(personId),
                //                                               ipAdress,
                //                                               userId,
                //                                               1));
                //    sb.Append(" WHERE COMPANY_ID = " + companyId + Environment.NewLine);    // COMPANY_ID
                //    sb.Append("   AND ID = " + Id.ToString() + Environment.NewLine);        // ID

                //    db.ExecuteSQL(sb.ToString(), false);

                //}
                //catch (Exception ex)
                //{
                //    db.ExRollbackTransaction();
                //    CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateUser(Delete)", ex);
                //    return CLASS_NM + ".UpdateUser(Delete) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
                //}

                //#endregion
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
                    CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateUser(DelLockPg)", ex);
                    return CLASS_NM + ".UpdateUser(DelLockPg) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
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
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateUser(CommitTransaction)", ex);
                return CLASS_NM + ".UpdateUser(CommitTransaction) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
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
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateUser(DbClose)", ex);
                return CLASS_NM + ".UpdateUser(DbClose) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
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
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateUser(Add Evidence)", ex);
                return CLASS_NM + ".UpdateUser(Add Evidence) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
            }

            #endregion

            #region Return

            if (type == 1 && Id == 0)
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
