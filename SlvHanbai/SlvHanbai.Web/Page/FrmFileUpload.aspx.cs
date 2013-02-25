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
    public partial class FrmFileUpload : System.Web.UI.Page
    {
        private const string CLASS_NM = "FrmFileUpload";

        protected void Page_Load(object sender, EventArgs e)
        {
            string errMsg = "";

            #region Parameter Get

            string random = "";
            string upLoadFileName = "";

            try
            {
                random = Request.QueryString["random"];
                upLoadFileName = Request.QueryString["upLoadFileName"];
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

            #region UpLoad File Save

            HttpPostedFile posted = Request.Files[0];

            string dir = "";
            string date = "";
            string time = "";
            string millisecond = "";
            DateTime now = DateTime.Now;
            date = now.ToString("yyyyMMdd");
            time = now.ToString("HHmmss");
            millisecond = now.Millisecond.ToString();
            dir = date + time + millisecond;

            try
            {
                if (string.IsNullOrEmpty(errMsg))
                {
                    posted.SaveAs(CommonUtl.gstrFileUpLoadDir + dir + "\\" + upLoadFileName);
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".Page_Load(UpLoad File Save)", ex);
                errMsg = CLASS_NM + ".Page_Load(UpLoad File Save) : 予期せぬエラーが発生しました" + Environment.NewLine + ex.Message;
            }

            #endregion

            #region Response

            string ret_Msg = "";
            try
            {
                Response.ContentType = "application/octet-stream";
                Response.AppendHeader("Content-Disposition", "attachment; filename=");
                Response.Flush();
                if (errMsg != "")
                {
                    ret_Msg = "error message start ==>" + errMsg;
                }
                else
                {
                    ret_Msg = "success file upload ==>" + dir;
                }
                Response.Write(ret_Msg);
                Response.Flush();
                Response.Close();

            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".Page_Load(Response)", ex);
            }

            #endregion
        }
    }
}