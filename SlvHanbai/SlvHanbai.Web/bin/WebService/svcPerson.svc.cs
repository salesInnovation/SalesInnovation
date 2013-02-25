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
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class svcPerson
    {
        private const string CLASS_NM = "svcPerson";
        private readonly string PG_NM = DataPgEvidence.PGName.Mst.Person;

        #region データ取得

        /// <summary> 
        /// 担当データ取得
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebMethod(EnableSession = true)]
        public EntityPerson GetPerson(string random, int Id)
        {
            EntityPerson entity = null;

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
                    entity = new EntityPerson();
                    entity.MESSAGE = _message;
                    return entity;
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetPersonList(認証処理)", ex);
                entity = new EntityPerson();
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

                sb.Append("SELECT PS.* " + Environment.NewLine);
                sb.Append("      ,NM.DESCRIPTION AS DISPLAY_DIVISION_NAME " + Environment.NewLine);
                sb.Append("      ,SG.NAME AS GROUP_NAME " + Environment.NewLine);
                sb.Append("  FROM M_PERSON AS PS" + Environment.NewLine);

                // 表示区分
                sb.Append("  LEFT JOIN SYS_M_NAME AS NM" + Environment.NewLine);
                sb.Append("    ON NM.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND NM.DISPLAY_FLG = 1 " + Environment.NewLine);
                sb.Append("   AND NM.DIVISION_ID = " + (int)CommonUtl.geNameKbn.DISPLAY_DIVISION_ID + Environment.NewLine);
                sb.Append("   AND PS.DISPLAY_FLG = NM.ID" + Environment.NewLine);

                // グループ
                sb.Append("  LEFT JOIN SYS_M_COMPANY_GROUP AS SG" + Environment.NewLine);
                sb.Append("    ON SG.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND SG.DISPLAY_FLG = 1 " + Environment.NewLine);
                sb.Append("   AND SG.COMPANY_ID = PS.COMPANY_ID" + Environment.NewLine);
                sb.Append("   AND SG.ID = PS.GROUP_ID" + Environment.NewLine);

                sb.Append(" WHERE PS.COMPANY_ID = " + companyId + Environment.NewLine);
                sb.Append("   AND PS.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND PS.ID = " + Id.ToString() + Environment.NewLine);

                sb.Append(" ORDER BY PS.COMPANY_ID " + Environment.NewLine);
                sb.Append("         ,PS.ID " + Environment.NewLine);

                #endregion

                dt = db.GetDataTable(sb.ToString());

                if (dt.DefaultView.Count > 0)
                {
                    entity = new EntityPerson();

                    // 排他制御
                    DataPgLock.geLovkFlg lockFlg;
                    string strErr = DataPgLock.SetLockPg(companyId, userId, PG_NM, Id.ToString(), ipAdress, db, out lockFlg);
                    if (strErr != "")
                    {
                        entity.MESSAGE = CLASS_NM + ".GetPerson : 排他制御(ロック情報取得)に失敗しました。" + Environment.NewLine + strErr;
                    }

                    int _group_id = ExCast.zCInt(dt.DefaultView[0]["GROUP_ID"]);
                    string _group_nm = "";
                    if (_group_id == 0)
                    {
                        _group_nm = "全て";
                    }
                    else
                    {
                        _group_nm = ExCast.zCStr(dt.DefaultView[0]["GROUP_NAME"]);
                    }

                    #region Set Entity

                    entity.id = ExCast.zCInt(dt.DefaultView[0]["ID"]);
                    entity.name = ExCast.zCStr(dt.DefaultView[0]["NAME"]);
                    entity.group_id = string.Format("{0:000}", _group_id);
                    entity.group_nm = _group_nm;
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
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetPerson", ex);
                entity = new EntityPerson();
                entity.MESSAGE = CLASS_NM + ".GetPerson : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message.ToString();
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
        public string UpdatePerson(string random, int type, long Id, EntityPerson entity)
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
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdatePerson(認証処理)", ex);
                return CLASS_NM + ".UpdatePerson : 認証処理に失敗しました。" + Environment.NewLine + ex.Message.ToString();
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
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdatePerson(DbOpen)", ex);
                return CLASS_NM + ".UpdatePerson(DbOpen) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
            }

            #endregion

            #region BeginTransaction

            try
            {
                db.ExBeginTransaction();
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdatePerson(BeginTransaction)", ex);
                return CLASS_NM + ".UpdatePerson(BeginTransaction) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
            }

            #endregion

            #region Get Max Master ID

            if (type == 1 && Id == 0)
            {
                try
                {
                    DataMasterId.GetMaxMasterId(companyId,
                                                "",
                                                db,
                                                DataMasterId.geMasterMaxIdKbn.Person,
                                                out _Id);
                    if (_Id == "")
                    {
                        return "ID取得に失敗しました。";
                    }
                }
                catch (Exception ex)
                {
                    CommonUtl.ExLogger.Error(CLASS_NM + ".UpdatePerson(GetMaxMasterId)", ex);
                    return CLASS_NM + ".UpdatePerson(GetMaxMasterId) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
                }
            }
            else
            {
                _Id = Id.ToString();
            }

            #endregion

            #region Insert

            if (type == 1)
            {
                try
                {
                    #region Delete SQL

                    sb.Length = 0;
                    sb.Append("DELETE FROM M_PERSON " + Environment.NewLine);
                    sb.Append(" WHERE DELETE_FLG = 1 " + Environment.NewLine);
                    sb.Append("   AND COMPANY_ID = " + companyId + Environment.NewLine);
                    sb.Append("   AND ID = " + _Id.ToString() + Environment.NewLine);

                    #endregion

                    db.ExecuteSQL(sb.ToString(), false);

                    #region Insert SQL

                    sb.Length = 0;
                    sb.Append("INSERT INTO M_PERSON " + Environment.NewLine);
                    sb.Append("       ( COMPANY_ID" + Environment.NewLine);
                    sb.Append("       , ID" + Environment.NewLine);
                    sb.Append("       , GROUP_ID" + Environment.NewLine);
                    sb.Append("       , NAME" + Environment.NewLine);
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
                    sb.Append("SELECT  " + companyId + Environment.NewLine);                                                // COMPANY_ID
                    sb.Append("       ," + _Id.ToString() + Environment.NewLine);                                            // ID
                    sb.Append("       ," + entity.group_id + Environment.NewLine);                                          // GROUP_ID
                    sb.Append("       ," + ExEscape.zRepStr(entity.name) + Environment.NewLine);                                       // NAME
                    sb.Append("       ," + ExEscape.zRepStr(entity.memo) + Environment.NewLine);                                       // MEMO
                    sb.Append("       ," + entity.display_division_id + Environment.NewLine);                               // DISPLAY_FLG
                    sb.Append(CommonUtl.GetInsSQLCommonColums(CommonUtl.UpdKbn.Ins,
                                                                PG_NM,
                                                                "M_PERSON",
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
                    CommonUtl.ExLogger.Error(CLASS_NM + ".UpdatePerson(Insert)", ex);
                    return CLASS_NM + ".UpdatePerson(Insert) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
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
                    sb.Append("UPDATE M_PERSON " + Environment.NewLine);
                    sb.Append(CommonUtl.GetUpdSQLCommonColums(PG_NM,
                                                               ExCast.zCInt(personId),
                                                               ipAdress,
                                                               userId,
                                                               0));
                    sb.Append("      ,GROUP_ID = " + entity.group_id + Environment.NewLine);
                    sb.Append("      ,NAME = " + ExEscape.zRepStr(entity.name) + Environment.NewLine);
                    sb.Append("      ,MEMO = " + ExEscape.zRepStr(entity.memo) + Environment.NewLine);
                    sb.Append("      ,DISPLAY_FLG = " + entity.display_division_id + Environment.NewLine);
                    sb.Append(" WHERE COMPANY_ID = " + companyId + Environment.NewLine);        // COMPANY_ID
                    sb.Append("   AND ID = " + Id.ToString() + Environment.NewLine);            // ID

                    #endregion

                    db.ExecuteSQL(sb.ToString(), false);

                }
                catch (Exception ex)
                {
                    db.ExRollbackTransaction();
                    CommonUtl.ExLogger.Error(CLASS_NM + ".UpdatePerson(Update)", ex);
                    return CLASS_NM + ".UpdatePerson(Update) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
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

                    _ret = DataExists.IsExistData(db, companyId, groupId, "T_ESTIMATE_H", "PERSON_ID", Id.ToString(), CommonUtl.geStrOrNumKbn.Number);
                    if (_ret == true)
                    {
                        return "担当ID : " + string.Format("{0:000}", Id) + " は見積データの入力担当者に使用されている為、削除できません。";
                    }

                    _ret = DataExists.IsExistData(db, companyId, groupId, "T_ORDER_H", "PERSON_ID", Id.ToString(), CommonUtl.geStrOrNumKbn.Number);
                    if (_ret == true)
                    {
                        return "担当ID : " + string.Format("{0:000}", Id) + " は受注データの入力担当者に使用されている為、削除できません。";
                    }

                    _ret = DataExists.IsExistData(db, companyId, groupId, "T_SALES_H", "PERSON_ID", Id.ToString(), CommonUtl.geStrOrNumKbn.Number);
                    if (_ret == true)
                    {
                        return "担当ID : " + string.Format("{0:000}", Id) + " は売上データの入力担当者に使用されている為、削除できません。";
                    }

                    _ret = DataExists.IsExistData(db, companyId, groupId, "T_RECEIPT_H", "PERSON_ID", Id.ToString(), CommonUtl.geStrOrNumKbn.Number);
                    if (_ret == true)
                    {
                        return "担当ID : " + string.Format("{0:000}", Id) + " は入金データの入力担当者に使用されている為、削除できません。";
                    }
                }
                catch (Exception ex)
                {
                    db.ExRollbackTransaction();
                    CommonUtl.ExLogger.Error(CLASS_NM + ".UpdatePerson(Exist Data)", ex);
                    return CLASS_NM + ".UpdatePerson(Exist Data) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
                }

                #endregion

                #region Update

                try
                {
                    sb.Length = 0;
                    sb.Append("UPDATE M_PERSON " + Environment.NewLine);
                    sb.Append(CommonUtl.GetUpdSQLCommonColums(PG_NM,
                                                               ExCast.zCInt(personId),
                                                               ipAdress,
                                                               userId,
                                                               1));
                    sb.Append(" WHERE COMPANY_ID = " + companyId + Environment.NewLine);    // COMPANY_ID
                    sb.Append("   AND ID = " + Id.ToString() + Environment.NewLine);        // ID

                    db.ExecuteSQL(sb.ToString(), false);

                }
                catch (Exception ex)
                {
                    db.ExRollbackTransaction();
                    CommonUtl.ExLogger.Error(CLASS_NM + ".UpdatePerson(Delete)", ex);
                    return CLASS_NM + ".UpdatePerson(Delete) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
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
                    CommonUtl.ExLogger.Error(CLASS_NM + ".UpdatePerson(DelLockPg)", ex);
                    return CLASS_NM + ".UpdatePerson(DelLockPg) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
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
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdatePerson(CommitTransaction)", ex);
                return CLASS_NM + ".UpdatePerson(CommitTransaction) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
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
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdatePerson(DbClose)", ex);
                return CLASS_NM + ".UpdatePerson(DbClose) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
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
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdatePerson(Add Evidence)", ex);
                return CLASS_NM + ".UpdatePerson(Add Evidence) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
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
