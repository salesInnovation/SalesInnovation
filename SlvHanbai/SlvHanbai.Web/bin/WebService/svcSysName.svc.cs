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
using SlvHanbai.Web.Class.Utility;
using SlvHanbai.Web.Class.Entity;

namespace SlvHanbai.Web.WebService
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class svcSysName
    {
        private const string CLASS_NM = "svcSysName";

        #region データ取得

        // 名称マスタリスト取得
        [OperationContract()]
        [WebMethod(EnableSession = true)]
        public List<EntityName> GetNameList(string random, int divisionIDFrom, int divisionIDTo, int idFrom, int idTo)
        {
            List<EntityName> objList = new List<EntityName>();

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
                    EntityName entity = new EntityName();
                    entity.MESSAGE = _message;
                    objList.Add(entity);
                    return objList;
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetNameList(認証処理)", ex);
                EntityName entity = new EntityName();
                entity.MESSAGE = "認証処理に失敗しました。" + Environment.NewLine + ex.Message.ToString();
                objList.Add(entity);
                return objList;
            }

            #endregion

            StringBuilder sb;
            DataTable dt;

            try
            {
                sb = new StringBuilder();
                sb.Append("SELECT NM.* " + Environment.NewLine);
                sb.Append("  FROM SYS_M_NAME AS NM" + Environment.NewLine);
                sb.Append(" WHERE NM.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND NM.DIVISION_ID >= " + divisionIDFrom.ToString() + Environment.NewLine);
                sb.Append("   AND NM.DIVISION_ID <= " + divisionIDTo.ToString() + Environment.NewLine);
                sb.Append("   AND NM.ID >= " + idFrom.ToString() + Environment.NewLine);
                sb.Append("   AND NM.ID <= " + idTo.ToString() + Environment.NewLine);
                sb.Append(" ORDER BY NM.DIVISION_ID ");
                sb.Append("         ,NM.ID ");

                dt = CommonUtl.gMySqlDt.GetDataTable(sb.ToString());

                if (dt.DefaultView.Count > 0)
                {
                    for (int i = 0; i <= dt.DefaultView.Count - 1; i++)
                    {
                        objList.Add(new EntityName(ExCast.zCInt(dt.DefaultView[i]["DIVISION_ID"]),
                                                   ExCast.zCInt(dt.DefaultView[i]["ID"]),
                                                   ExCast.zCStr(dt.DefaultView[i]["DESCRIPTION"])));
                    }
                }

            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetNameList", ex);
            }

            return objList;

        }

        // 名称マスタリスト取得
        [OperationContract()]
        [WebMethod(EnableSession = true)]
        public List<EntityName> GetNameListAll(string random)
        {
            List<EntityName> objList = new List<EntityName>();

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
                    EntityName entity = new EntityName();
                    entity.MESSAGE = _message;
                    objList.Add(entity);
                    return objList;
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetNameListAll(認証処理)", ex);
                EntityName entity = new EntityName();
                entity.MESSAGE = "認証処理に失敗しました。" + Environment.NewLine + ex.Message.ToString();
                objList.Add(entity);
                return objList;
            }

            #endregion

            StringBuilder sb;
            DataTable dt;

            try
            {
                sb = new StringBuilder();
                sb.Append("SELECT NM.* " + Environment.NewLine);
                sb.Append("  FROM SYS_M_NAME AS NM" + Environment.NewLine);
                sb.Append(" WHERE NM.DELETE_FLG = 0 ");
                sb.Append(" ORDER BY NM.DIVISION_ID ");
                sb.Append("         ,NM.ID ");

                dt = CommonUtl.gMySqlDt.GetDataTable(sb.ToString());

                if (dt.DefaultView.Count > 0)
                {
                    for (int i = 0; i <= dt.DefaultView.Count - 1; i++)
                    {
                        objList.Add(new EntityName(ExCast.zCInt(dt.DefaultView[i]["DIVISION_ID"]),
                                                   ExCast.zCInt(dt.DefaultView[i]["ID"]),
                                                   ExCast.zCStr(dt.DefaultView[i]["DESCRIPTION"])));
                    }
                }

            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetNameList", ex);
            }

            return objList;
        }

        #endregion
    }
}
