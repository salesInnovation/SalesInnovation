using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.VisualBasic;
using SlvHanbai.Web.Class;
using SlvHanbai.Web.Class.DB;
using SlvHanbai.Web.Class.Utility;
using SlvHanbai.Web.Class.Reports;
using SlvHanbai.Web.Class.Data;

namespace SlvHanbai.Web.Page
{
    public partial class FrmReportDowmLoad : System.Web.UI.Page
    {
        private const string CLASS_NM = "FrmReportDowmLoad";

        protected void Page_Load(object sender, EventArgs e)
        {
            string errMsg = "";

            #region Parameter Get

            string rptKbn = "";
            string pgId = "";
            string random = "";
            string _prm = "";
            string[] prm;
            string downLoadFileName = "";
            string downLoadFilePath = "";

            try
            {
                rptKbn = Request.QueryString["rptKbn"];
                pgId = Request.QueryString["pgId"];
                random = Request.QueryString["random"];
                downLoadFileName = Request.QueryString["downLoadFileName"];
                downLoadFilePath = Request.QueryString["downLoadFilePath"];
                downLoadFilePath = downLoadFilePath.Replace("@AAB@", @"\").Replace("@AAD@", "/");
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".Page_Load(Parameter Get)", ex);
                errMsg = CLASS_NM + ".Page_Load(Parameter Get) : 予期せぬエラーが発生しました" + Environment.NewLine + ex.Message;
            }

            #endregion

            #region 認証処理

            string companyId = "";
            string groupId = "";
            string userId = "";
            string ipAdress = "";
            string sessionString = "";

            if (string.IsNullOrEmpty(errMsg))
            {
                companyId = ExCast.zCStr(HttpContext.Current.Session[ExSession.COMPANY_ID]);
                groupId = ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_ID]);
                userId = ExCast.zCStr(HttpContext.Current.Session[ExSession.USER_ID]);
                ipAdress = ExCast.zCStr(HttpContext.Current.Session[ExSession.IP_ADRESS]);
                sessionString = ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]);

                string _message = ExSession.SessionUserUniqueCheck(random, ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]), ExCast.zCInt(HttpContext.Current.Session[ExSession.USER_ID]));
                if (_message != "")
                {
                    CommonUtl.ExLogger.Error(CLASS_NM + ".Page_Load(認証処理) : 認証に失敗しました。" + Environment.NewLine + _message);
                    return;
                }
            }

            #endregion

            #region Response

            try
            {
                if (errMsg != "")
                {
                    Response.ContentType = "application/octet-stream";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + downLoadFileName);
                    Response.Flush();
                    string ret_errMsg = "error message start ==>" + errMsg;
                    if (ExCast.LenB(ret_errMsg) < 200)
                    {
                        ret_errMsg += Microsoft.VisualBasic.Strings.Space(201 - ExCast.LenB(ret_errMsg));
                    }
                    Response.Write(ret_errMsg);
                    Response.Flush();
                    Response.Close();
                }
                else
                {
                    Response.ContentType = "application/octet-stream";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + downLoadFileName);
                    Response.TransmitFile(downLoadFilePath);
                    Response.Flush();
                    Response.Close();
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".Page_Load(Response)", ex);
            }

            #endregion

        }
    }
}