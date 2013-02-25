using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using SlvHanbai.Web.Class;
using SlvHanbai.Web.Class.Utility;
using SlvHanbai.Web.Class.DB;

namespace SlvHanbai.Web
{
    public class Global : System.Web.HttpApplication
    {

        void Application_Start(object sender, EventArgs e)
        {
            // アプリケーションのスタートアップで実行するコードです
            //log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(@HttpContext.Current.Server.MapPath(@"\\EW20121725\Sales.system-innovation.net\wwwroot\log4net.xml")));
            //log4net.Config.XmlConfigurator.Configure();

            CommonUtl.gSysDbKbn = ExCast.zCInt(System.Configuration.ConfigurationManager.AppSettings["SysDbKbn"]);
            CommonUtl.gDemoKbn = ExCast.zCInt(System.Configuration.ConfigurationManager.AppSettings["DemoKbn"]);

            CommonUtl.gConnectionString1 = System.Configuration.ConfigurationManager.ConnectionStrings["ConCmTestDt"].ConnectionString;

            CommonUtl.gMySqlDt = new ExMySQLData();
        }

        void Application_End(object sender, EventArgs e)
        {
            //  アプリケーションのシャットダウンで実行するコードです
        }

        void Application_Error(object sender, EventArgs e)
        {
            // ハンドルされていないエラーが発生したときに実行するコードです
        }

        void Session_Start(object sender, EventArgs e)
        {
            // 新規セッションを開始したときに実行するコードです
            Session["MySqlDt"] = null;
        }

        void Session_End(object sender, EventArgs e)
        {
            // セッションが終了したときに実行するコードです 
            // メモ: Web.config ファイル内で sessionstate モードが InProc に設定されているときのみ、
            // Session_End イベントが発生します。 session モードが StateServer か、または 
            // SQLServer に設定されている場合、イベントは発生しません。

        }

    }
}
