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
    public class svcClass
    {
        private const string CLASS_NM = "svcClass";
        private readonly string PG_NM = DataPgEvidence.PGName.Mst.Class;

        #region データ取得

        /// <summary> 
        /// データ取得
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebMethod(EnableSession = true)]
        public List<EntityClass> GetClass(string random, int classKbn)
        {

            EntityClass entity;
            List<EntityClass> entityList = new List<EntityClass>();

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
                    entity = new EntityClass();
                    entity.MESSAGE = _message;
                    entityList.Add(entity);
                    return entityList;
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetClass(認証処理)", ex);
                entity = new EntityClass();
                entity.MESSAGE = CLASS_NM + ".GetClass : 認証処理に失敗しました。" + Environment.NewLine + ex.Message.ToString(); ;
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

                int _classKbn = ((classKbn - 1)) * 3 + 1;

                sb.Append("SELECT MT.* " + Environment.NewLine);
                sb.Append("      ,NM.DESCRIPTION AS DISPLAY_DIVISION_NAME " + Environment.NewLine);
                sb.Append("  FROM M_CLASS AS MT" + Environment.NewLine);

                #region Join

                // 表示区分
                sb.Append("  LEFT JOIN SYS_M_NAME AS NM" + Environment.NewLine);
                sb.Append("    ON NM.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND NM.DISPLAY_FLG = 1 " + Environment.NewLine);
                sb.Append("   AND NM.DIVISION_ID = " + (int)CommonUtl.geNameKbn.DISPLAY_DIVISION_ID + Environment.NewLine);
                sb.Append("   AND NM.ID = MT.DISPLAY_FLG" + Environment.NewLine);

                #endregion

                sb.Append(" WHERE MT.COMPANY_ID = " + companyId + Environment.NewLine);
                sb.Append("   AND MT.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND MT.CLASS_DIVISION_ID = " + _classKbn.ToString() + Environment.NewLine);

                sb.Append(" ORDER BY MT.COMPANY_ID " + Environment.NewLine);
                sb.Append("         ,MT.ID " + Environment.NewLine);

                #endregion

                dt = db.GetDataTable(sb.ToString());

                // 排他制御
                DataPgLock.geLovkFlg lockFlg;
                string strErr = DataPgLock.SetLockPg(companyId, userId, PG_NM, _classKbn.ToString(), ipAdress, db, out lockFlg);

                if (strErr != "")
                {
                    entity = new EntityClass();
                    entity.MESSAGE = CLASS_NM + ".GetCommodity : 排他制御(ロック情報取得)に失敗しました。" + Environment.NewLine + strErr;
                    entityList.Add(entity);
                    return entityList;
                }

                if (dt.DefaultView.Count > 0)
                {
                    for (int i = 0; i <= dt.DefaultView.Count - 1; i++)
                    {
                        #region Set Entity

                        entity = new EntityClass();
                        entity.id = ExCast.zCStr(dt.DefaultView[i]["ID"]);
                        entity.class_divition_id = ExCast.zCInt(dt.DefaultView[i]["CLASS_DIVISION_ID"]);
                        entity.name = ExCast.zCStr(dt.DefaultView[i]["NAME"]);

                        entity.display_division_id = ExCast.zCInt(dt.DefaultView[i]["DISPLAY_FLG"]);
                        entity.display_division_nm = ExCast.zCStr(dt.DefaultView[i]["DISPLAY_DIVISION_NAME"]);

                        entity.memo = ExCast.zCStr(dt.DefaultView[i]["MEMO"]);

                        entity.lock_flg = (int)lockFlg;

                        entityList.Add(entity);

                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetClass", ex);
                entityList.Clear();
                entity = new EntityClass();
                entity.MESSAGE = CLASS_NM + ".GetClass : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message.ToString();
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

        /// <summary> 
        /// データ存在チェック
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebMethod(EnableSession = true)]
        public string ChkClassExist(string random, int classKbn, string Id)
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
                CommonUtl.ExLogger.Error(CLASS_NM + ".ChkClassExist(認証処理)", ex);
                return CLASS_NM + ".ChkClassExist : 認証処理に失敗しました。" + Environment.NewLine + ex.Message.ToString(); ;
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

                int _classKbn = ((classKbn - 1)) * 3 + 1;

                #region Exist Data

                bool _ret = false;

                switch (_classKbn)
                { 
                    case 1:     // 得意先分類
                        _ret = DataExists.IsExistData(db, companyId, "", "M_CUSTOMER", "GROUP1_ID", string.Format("{0:000",ExCast.zCInt(Id)), CommonUtl.geStrOrNumKbn.String);
                        if (_ret == true)
                        {
                            return "得意先分類ID : " + Id + " は得意先マスタで使用されている為、削除できません。";
                        }
                        break;
                    case 2:     // 商品分類
                        _ret = DataExists.IsExistData(db, companyId, "", "M_COMMODITY", "GROUP1_ID", string.Format("{0:000", ExCast.zCInt(Id)), CommonUtl.geStrOrNumKbn.String);
                        if (_ret == true)
                        {
                            return "商品分類ID : " + Id + " は商品マスタで使用されている為、削除できません。";
                        }
                        break;
                    case 3:     // 仕入先分類
                        _ret = DataExists.IsExistData(db, companyId, "", "M_PURCHASE", "GROUP1_ID", string.Format("{0:000", ExCast.zCInt(Id)), CommonUtl.geStrOrNumKbn.String);
                        if (_ret == true)
                        {
                            return "仕入先分類ID : " + Id + " は仕入先マスタで使用されている為、削除できません。";
                        }
                        break;
                }

                #endregion
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".ChkClassExist", ex);
                return CLASS_NM + ".ChkClassExist : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message.ToString();
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

            return "";

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
        public string UpdateClass(string random, List<EntityClass> entity, int classKbn)
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
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateClass(認証処理)", ex);
                return CLASS_NM + ".UpdateClass : 認証処理に失敗しました。" + Environment.NewLine + ex.Message.ToString();
            }

            #endregion

            #region Field

            StringBuilder sb = new StringBuilder();
            DataTable dt;
            ExMySQLData db = null;
            string _Id = "";
            int _classKbn = 0;

            #endregion

            #region データ存在チェック

            sb = new StringBuilder();

            #region SQL

            _classKbn = ((classKbn - 1)) * 3 + 1;

            sb.Length = 0;
            sb.Append("SELECT MT.* " + Environment.NewLine);
            sb.Append("  FROM M_CLASS AS MT" + Environment.NewLine);
            sb.Append(" WHERE MT.COMPANY_ID = " + companyId + Environment.NewLine);
            sb.Append("   AND MT.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND MT.CLASS_DIVISION_ID = " + _classKbn.ToString() + Environment.NewLine);
            sb.Append(" ORDER BY MT.COMPANY_ID " + Environment.NewLine);
            sb.Append("         ,MT.ID " + Environment.NewLine);

            #endregion

            db = ExSession.GetSessionDb(ExCast.zCInt(HttpContext.Current.Session[ExSession.USER_ID]),
                                        ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]));
            dt = db.GetDataTable(sb.ToString());

            string ret_msg = "";
            if (dt.DefaultView.Count > 0)
            {
                for (int i = 0; i <= dt.DefaultView.Count - 1; i++)
                {
                    long lngClassId = ExCast.zCLng(dt.DefaultView[i]["ID"]);
                    bool exists_flg = false;
                    for (int i_ = 0; i_ <= entity.Count - 1; i_++)
                    {
                        if (ExCast.zCLng(entity[i_].id) == lngClassId)
                        {
                            exists_flg = true;
                        }
                    }

                    string strClassId = string.Format("{0:000}", ExCast.zCDbl(lngClassId));

                    if (exists_flg == false)
                    {
                        bool _ret = false;

                        switch (classKbn)
                        {
                            case 1:     // 得意先分類
                                _ret = DataExists.IsExistData(db, companyId, "", "M_CUSTOMER", "GROUP1_ID", strClassId, CommonUtl.geStrOrNumKbn.String);
                                if (_ret == true)
                                {
                                    ret_msg += "得意先分類ID : " + strClassId + " は得意先マスタで使用されている為、削除できません。" + Environment.NewLine;
                                }
                                break;
                            case 2:     // 商品分類
                                _ret = DataExists.IsExistData(db, companyId, "", "M_COMMODITY", "GROUP1_ID", strClassId, CommonUtl.geStrOrNumKbn.String);
                                if (_ret == true)
                                {
                                    ret_msg += "商品分類ID : " + strClassId + " は商品マスタで使用されている為、削除できません。" + Environment.NewLine;
                                }
                                break;
                            case 3:     // 仕入先分類
                                _ret = DataExists.IsExistData(db, companyId, "", "M_PURCHASE", "GROUP1_ID", strClassId, CommonUtl.geStrOrNumKbn.String);
                                if (_ret == true)
                                {
                                    ret_msg += "仕入先分類ID : " + strClassId + " は仕入先マスタで使用されている為、削除できません。" + Environment.NewLine;
                                }
                                break;
                        }
                    }
                }

                if (!string.IsNullOrEmpty(ret_msg))
                {
                    return ret_msg;
                }
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
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateClass(DbOpen)", ex);
                return CLASS_NM + ".UpdateClass(DbOpen) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
            }

            #endregion

            #region BeginTransaction

            try
            {
                db.ExBeginTransaction();
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateClass(BeginTransaction)", ex);
                return CLASS_NM + ".UpdateClass(BeginTransaction) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
            }

            #endregion

            #region Delete

            try
            {
                #region Delete SQL

                _classKbn = ((classKbn - 1)) * 3 + 1;

                sb.Length = 0;
                sb.Append("DELETE FROM M_CLASS " + Environment.NewLine);
                sb.Append(" WHERE COMPANY_ID = " + companyId + Environment.NewLine);
                sb.Append("   AND CLASS_DIVISION_ID = " + _classKbn.ToString() + Environment.NewLine);

                #endregion

                db.ExecuteSQL(sb.ToString(), false);

            }
            catch (Exception ex)
            {
                db.ExRollbackTransaction();
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateClass(Delete)", ex);
                return CLASS_NM + ".UpdateClass(Delete) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
            }

            #endregion

            #region Insert

            try
            {
                for (int i = 0; i <= entity.Count - 1; i++)
                {
                    // 空行は無視する
                    if (!string.IsNullOrEmpty(entity[i].id))
                    {
                        #region Insert SQL

                        sb.Length = 0;
                        sb.Append("INSERT INTO M_CLASS " + Environment.NewLine);
                        sb.Append("       ( COMPANY_ID" + Environment.NewLine);
                        sb.Append("       , CLASS_DIVISION_ID" + Environment.NewLine);
                        sb.Append("       , ID" + Environment.NewLine);
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
                        sb.Append("SELECT  " + companyId + Environment.NewLine);                                    // COMPANY_ID
                        sb.Append("       ," + _classKbn.ToString() + Environment.NewLine);                         // Class_DIVISION_ID
                        sb.Append("       ," + ExEscape.zRepStr(ExCast.zNumZeroFormat("{0:000}", entity[i].id)) + Environment.NewLine);                          // ID
                        sb.Append("       ," + ExEscape.zRepStr(entity[i].name) + Environment.NewLine);             // NAME
                        sb.Append("       ," + ExEscape.zRepStr(entity[i].memo) + Environment.NewLine);             // MEMO
                        sb.Append("       ," + entity[i].display_division_id + Environment.NewLine);                // DISPLAY_FLG
                        sb.Append(CommonUtl.GetInsSQLCommonColums(CommonUtl.UpdKbn.Ins,
                                                                    PG_NM,
                                                                    "M_Class",
                                                                    ExCast.zCInt(personId),
                                                                    _Id,
                                                                    ipAdress,
                                                                    userId));

                        #endregion

                        db.ExecuteSQL(sb.ToString(), false);
                    }
                }
            }
            catch (Exception ex)
            {
                db.ExRollbackTransaction();
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateClass(Insert)", ex);
                return CLASS_NM + ".UpdateClass(Insert) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
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
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateClass(DelLockPg)", ex);
                return CLASS_NM + ".UpdateClass(DelLockPg) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
            }

            #endregion

            #region CommitTransaction

            try
            {
                db.ExCommitTransaction();
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateClass(CommitTransaction)", ex);
                return CLASS_NM + ".UpdateClass(CommitTransaction) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
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
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateClass(DbClose)", ex);
                return CLASS_NM + ".UpdateClass(DbClose) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
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
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateClass(Add Evidence)", ex);
                return CLASS_NM + ".UpdateClass(Add Evidence) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
            }

            #endregion

            return "";

        }

        #endregion

    }
}
