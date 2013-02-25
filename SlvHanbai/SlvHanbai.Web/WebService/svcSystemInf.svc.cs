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
    public class svcSystemInf
    {
        private const string CLASS_NM = "svcSystemInf";
        private readonly string PG_NM = DataPgEvidence.PGName.System;

        #region データ取得

        /// <summary> 
        /// データ取得
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebMethod(EnableSession = true)]
        public List<EntityDuties> GetSystemInf(string random, long no, int kbn)
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
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetSystemInf(認証処理)", ex);
                EntityDuties entity = new EntityDuties();
                entity.MESSAGE = CLASS_NM + ".GetSystemInf : 認証処理に失敗しました。" + Environment.NewLine + ex.Message.ToString(); ;
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
                sb.Append("  FROM T_SYSTEM_COMMUNICATION AS T" + Environment.NewLine);

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

                        //// 排他制御
                        //DataPgLock.geLovkFlg lockFlg = DataPgLock.geLovkFlg.UnLock;
                        //if (no != 0)    // 返答時
                        //{
                        //    string strErr = DataPgLock.SetLockPg(companyId, userId, PG_NM, no.ToString(), ipAdress, db, out lockFlg);
                        //    if (strErr != "")
                        //    {
                        //        entity.MESSAGE = CLASS_NM + ".GetDuties : 排他制御(ロック情報取得)に失敗しました。" + Environment.NewLine + strErr;
                        //    }
                        //}

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
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetSystemInf", ex);
                entityList.Clear();
                EntityDuties entity = new EntityDuties();
                entity.MESSAGE = CLASS_NM + ".GetSystemInf : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message.ToString(); ;
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

    }
}
