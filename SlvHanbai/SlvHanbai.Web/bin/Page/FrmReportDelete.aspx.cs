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
    public partial class FrmReportDelete : System.Web.UI.Page
    {
        private const string CLASS_NM = "FrmReportDelete";

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
                downLoadFileName = Request.QueryString["deleteFileName"];
                downLoadFilePath = Request.QueryString["deleteFilePath"];

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

            #region Report Delete

            try
            {
                if (string.IsNullOrEmpty(errMsg))
                {
                    if (!ExFile.IsFileOpenTime(downLoadFilePath, 100))
                    {
                        errMsg = CLASS_NM + ".Page_Load(Report Delete) : 別のプロセスで使用中の為、ファイルを削除できません。";
                    }
                    else
                    {
                        string _directory = downLoadFilePath.Replace(downLoadFileName, "");
                        if (System.IO.Directory.Exists(_directory))
                        {
                            System.IO.Directory.Delete(_directory, true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".Page_Load(Report Delete)", ex);
                errMsg = CLASS_NM + ".Page_Load(Report Delete) : 予期せぬエラーが発生しました" + Environment.NewLine + ex.Message;
            }

            #endregion

            #region Response

            string ret_Msg = "";
            try
            {
                Response.ContentType = "application/octet-stream";
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + downLoadFileName);
                Response.Flush();
                if (errMsg != "")
                {
                    ret_Msg = "error message start ==>" + errMsg;
                }
                else
                {
                    ret_Msg = "success file delete";
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