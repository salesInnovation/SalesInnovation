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
using SlvHanbai.Web.Class.DB;
using SlvHanbai.Web.Class.Data;
using SlvHanbai.Web.Class.Utility;
using SlvHanbai.Web.Class.Entity;

namespace SlvHanbai.Web.WebService
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class svcPgEvidence
    {
        private const string CLASS_NM = "svcPgEvidence";

        [OperationContract]
        [WebMethod(EnableSession = true)]
        public void AddEvidence(string random, string pgId, int type, string memo)
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
                    return;
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".AddEvidence(認証処理)", ex);
                return;
            }

            #endregion

            gAddEvidence(1, companyId, userId, ipAdress, sessionString, pgId, (DataPgEvidence.geOperationType)type, memo);
        }

        public static void gAddEvidence(int saveFlg, string companyId, string userId, string ipAdress, string sessionString, string pgId, DataPgEvidence.geOperationType type, string memo)
        {
            if (saveFlg != 1) return; 

            StringBuilder sb = new StringBuilder();
            DataTable dt;

            ExMySQLData db = ExSession.GetSessionDb(ExCast.zCInt(userId), sessionString);

            // 日時取得
            string date = "";
            string time = "";
            int millisecond = 0;
            DateTime now = DateTime.Now;
            date = now.ToString("yyyy/MM/dd");
            time = now.ToString("HH:mm:ss");
            millisecond = now.Millisecond;

            // PG実行履歴登録
            try
            {
                sb.Length = 0;
                sb.Append("INSERT INTO H_PG_EXEC_HISTORY " + Environment.NewLine);
                sb.Append("       (COMPANY_ID" + Environment.NewLine);
                sb.Append("       ,USER_ID" + Environment.NewLine);
                sb.Append("       ,PG_ID" + Environment.NewLine);
                sb.Append("       ,OPERATION_DATE" + Environment.NewLine);
                sb.Append("       ,OPERATION_TIME" + Environment.NewLine);
                sb.Append("       ,OPERATION_MILLISECOND" + Environment.NewLine);
                sb.Append("       ,OPERATION_TYPE" + Environment.NewLine);
                sb.Append("       ,PG_NAME" + Environment.NewLine);
                sb.Append("       ,OPERATION_TYPE_NAME" + Environment.NewLine);
                sb.Append("       ,MEMO" + Environment.NewLine);
                sb.Append("       ,UPDATE_FLG" + Environment.NewLine);
                sb.Append("       ,DELETE_FLG" + Environment.NewLine);
                sb.Append("       ,CREATE_PG_ID" + Environment.NewLine);
                sb.Append("       ,CREATE_ADRESS" + Environment.NewLine);
                sb.Append("       ,CREATE_USER_ID" + Environment.NewLine);
                sb.Append("       ,CREATE_DATE" + Environment.NewLine);
                sb.Append("       ,CREATE_TIME" + Environment.NewLine);
                sb.Append("       ,UPDATE_PG_ID" + Environment.NewLine);
                sb.Append("       ,UPDATE_ADRESS" + Environment.NewLine);
                sb.Append("       ,UPDATE_USER_ID" + Environment.NewLine);
                sb.Append("       ,UPDATE_DATE" + Environment.NewLine);
                sb.Append("       ,UPDATE_TIME" + Environment.NewLine);
                sb.Append(")" + Environment.NewLine);
                sb.Append("VALUES (" + companyId + Environment.NewLine);                                            // COMPANY_ID
                sb.Append("       ," + userId + Environment.NewLine);                                               // USER_ID
                sb.Append("       ," + ExEscape.zRepStr(pgId) + Environment.NewLine);                                          // PG_ID
                sb.Append("       ," + ExEscape.zRepStr(date) + Environment.NewLine);                                          // OPERATION_DATE
                sb.Append("       ," + ExEscape.zRepStr(time) + Environment.NewLine);                                          // OPERATION_TIME
                sb.Append("       ," + millisecond.ToString() + Environment.NewLine);                               // OPERATION_MILLISECOND
                sb.Append("       ," + (int)type + Environment.NewLine);                                            // OPERATION_TYPE
                sb.Append("       ," + ExEscape.zRepStr(DataPgEvidence.GetPgName(pgId)) + Environment.NewLine);                // PG_NAME
                sb.Append("       ," + ExEscape.zRepStr(DataPgEvidence.GetOperationTypeName(type)) + Environment.NewLine);     // OPERATION_TYPE_NAME
                sb.Append("       ," + ExEscape.zRepStr(memo) + Environment.NewLine);                                          // MEMO
                sb.Append("       ,0" + Environment.NewLine);                                                       // UPDATE_FLG
                sb.Append("       ,0" + Environment.NewLine);                                                       // DELETE_FLG
                sb.Append("       ,'SYSTEM'" + Environment.NewLine);                                                // CREATE_PG_ID
                sb.Append("       ," + ExEscape.zRepStr(ipAdress) + Environment.NewLine);                                      // CREATE_ADRESS
                sb.Append("       ," + ExEscape.zRepStr(userId) + Environment.NewLine);                                        // CREATE_USER_ID
                sb.Append("       ," + ExEscape.zRepStr(date) + Environment.NewLine);                                          // CREATE_DATE
                sb.Append("       ," + ExEscape.zRepStr(time) + Environment.NewLine);                                          // CREATE_TIME
                sb.Append("       ,'SYSTEM'" + Environment.NewLine);                                                // UPDATE_PG_ID
                sb.Append("       ," + ExEscape.zRepStr(ipAdress) + Environment.NewLine);                                      // UPDATE_ADRESS
                sb.Append("       ," + ExEscape.zRepStr(userId) + Environment.NewLine);                                        // UPDATE_USER_ID
                sb.Append("       ," + ExEscape.zRepStr(date) + Environment.NewLine);                                          // UPDATE_DATE
                sb.Append("       ," + ExEscape.zRepStr(time) + Environment.NewLine);                                          // UPDATE_TIME
                sb.Append(")");

                if (db.ExecuteSQL(sb.ToString(), true) == false)
                {
                    CommonUtl.ExLogger.Error(CLASS_NM + ".gAddEvidence(ExecuteSQL) : " + Environment.NewLine + ExSession.GetSessionDb(ExCast.zCInt(userId), sessionString).errMessage);
                    return;
                }

                switch (type)
                { 
                    case DataPgEvidence.geOperationType.Start:
                    case DataPgEvidence.geOperationType.End:
                        if (pgId == DataPgEvidence.PGName.System)
                        {
                            DataPgLock.DelLockPg(companyId, userId, "", "", ipAdress, true, db);
                        }
                        else
                        {
                            DataPgLock.DelLockPg(companyId, userId, pgId, "", ipAdress, true, db);
                        }
                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".gAddEvidence(Insert)", ex);
                db.ExRollbackTransaction();
            }

        }

    }
}
