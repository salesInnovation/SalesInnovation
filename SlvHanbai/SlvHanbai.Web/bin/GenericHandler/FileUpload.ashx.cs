using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using Microsoft.VisualBasic;
using SlvHanbai.Web.Class;
using SlvHanbai.Web.Class.DB;
using SlvHanbai.Web.Class.Utility;
using SlvHanbai.Web.Class.Reports;
using SlvHanbai.Web.Class.Data;

namespace SlvHanbai.Web.GenericHandler
{
    /// <summary>
    /// FileUpload の概要の説明
    /// </summary>
    public class FileUpload : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {

        private const string CLASS_NM = "FileUpload";

        public void ProcessRequest(HttpContext context)
        {
            string errMsg = "";

            #region Parameter Get

            string random = "";
            string upLoadFileName = "";
            int upLoadFileKbn = 0;

            try
            {
                random = context.Request.QueryString["random"];
                upLoadFileName = context.Request.QueryString["upLoadFileName"];
                upLoadFileKbn = ExCast.zCInt(context.Request.QueryString["upLoadFileKbn"]);
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

            string dir = "";
            try
            {
                if (string.IsNullOrEmpty(errMsg))
                {
                    string date = "";
                    string time = "";
                    string millisecond = "";
                    DateTime now = DateTime.Now;
                    date = now.ToString("yyyyMMdd");
                    time = now.ToString("HHmmss");
                    millisecond = now.Millisecond.ToString();
                    dir = date + time + millisecond;

                    Byte[] buffer = null; 
                    buffer = new Byte[context.Request.InputStream.Length];
                    context.Request.InputStream.Read(buffer, 0, buffer.Length);

                    if (upLoadFileKbn == 0)
                    {
                        if (!System.IO.Directory.Exists(CommonUtl.gstrFileUpLoadDir + dir))
                        {
                            System.IO.Directory.CreateDirectory(CommonUtl.gstrFileUpLoadDir + dir);
                        }

                        using (FileStream fs = new FileStream(CommonUtl.gstrFileUpLoadDir + dir + "\\" + upLoadFileName, FileMode.Create))
                        {
                            fs.Write(buffer, 0, buffer.Length);
                            fs.Close();
                        }
                    }
                    else if (upLoadFileKbn == 1)
                    {
                        if (!System.IO.Directory.Exists(CommonUtl.gstrFileUpLoadDutiesDir + dir))
                        {
                            System.IO.Directory.CreateDirectory(CommonUtl.gstrFileUpLoadDutiesDir + dir);
                        }

                        using (FileStream fs = new FileStream(CommonUtl.gstrFileUpLoadDutiesDir + dir + "\\" + upLoadFileName, FileMode.Create))
                        {
                            fs.Write(buffer, 0, buffer.Length);
                            fs.Close();
                        }
                    }
                    else
                    {
                        errMsg = CLASS_NM + ".Page_Load(UpLoad File Save) : upLoadFileKbnが不正です。";
                    }
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
                context.Response.ContentType = "application/octet-stream";
                context.Response.AppendHeader("Content-Disposition", "attachment; filename=");
                context.Response.Flush();
                if (errMsg != "")
                {
                    ret_Msg = "error message start ==>" + errMsg;
                }
                else
                {
                    ret_Msg = "success file upload ==>" + dir;
                }

                context.Response.Write(ret_Msg);
                context.Response.Flush();
                context.Response.Close();

            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".Page_Load(Response)", ex);
            }

            #endregion

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}