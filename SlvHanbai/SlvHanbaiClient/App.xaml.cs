using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using SlvHanbaiClient.Class;
using SlvHanbaiClient.Class.UI;
using SlvHanbaiClient.Class.WebService;
using SlvHanbaiClient.Class.Data;
using SlvHanbaiClient.Themes;
using SlvHanbaiClient.svcSysLogin;

namespace SlvHanbaiClient
{
    public partial class App : Application
    {

        public App()
        {
            this.Startup += this.Application_Startup;
            this.Exit += this.Application_Exit;
            this.UnhandledException += this.Application_UnhandledException;

            InitializeComponent();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine("1:" + DateTime.Now.ToString("hh:mm:ss") + " " + DateTime.Now.Millisecond.ToString());
            Common.gblnAppStart = true;
            MargedResourceDictionary.gThemeType = MargedResourceDictionary.geThemeType.ShinyBlue;
            Application.Current.Resources = new SlvHanbaiClient.Themes.MargedResourceDictionary();
            this.RootVisual = new NaviPage();
            //System.Windows.Interop.SilverlightHost host = Application.Current.Host;
            //System.Windows.Interop.Settings settings = host.Settings;
            //System.Diagnostics.Debug.WriteLine(settings.Windowless.ToString());

        }

        private void Application_Exit(object sender, EventArgs e)
        {
        }

        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            string errorMessage = String.Empty; 
            if (e.ExceptionObject.InnerException != null && e.ExceptionObject.InnerException is System.ServiceModel.FaultException)
            {
                //wcf exception.
                FaultException exc = e.ExceptionObject.InnerException as FaultException; 
                errorMessage = exc.Reason.ToString();
                ExMessageBox.Show(errorMessage);

                // WCFエラーの場合はアプリケーションを終了する
                if (Application.Current.IsRunningOutOfBrowser)
                {
                    Application.Current.MainWindow.Close();
                }
            }
            else
            {
                //silverlight exception.
                errorMessage = e.ExceptionObject.Message;
                ExMessageBox.Show(errorMessage);

                // 処理を続行する
                e.Handled = true; 
                Deployment.Current.Dispatcher.BeginInvoke(delegate { ReportErrorToDOM(e); }); //redirect to error page.
            }


            //handle exception so app doesn't crash.
            
            // アプリケーションがデバッガーの外側で実行されている場合、ブラウザーの
            // 例外メカニズムによって例外が報告されます。これにより、IE ではステータス バーに
            // 黄色の通知アイコンが表示され、Firefox にはスクリプト エラーが表示されます。
            //if (!System.Diagnostics.Debugger.IsAttached)
            //{

            //    // メモ : これにより、アプリケーションは例外がスローされた後も実行され続け、例外は
            //    // ハンドルされません。 
            //    // 実稼動アプリケーションでは、このエラー処理は、Web サイトにエラーを報告し、
            //    // アプリケーションを停止させるものに置換される必要があります。
            //    e.Handled = true;
            //    Deployment.Current.Dispatcher.BeginInvoke(delegate { ReportErrorToDOM(e); });
            //}
        }

        private void ReportErrorToDOM(ApplicationUnhandledExceptionEventArgs e)
        {
            try
            {
                string errorMsg = e.ExceptionObject.Message + e.ExceptionObject.StackTrace;
                errorMsg = errorMsg.Replace('"', '\'').Replace("\r\n", @"\n");

                System.Windows.Browser.HtmlPage.Window.Eval("throw new Error(\"Unhandled Error in Silverlight Application " + errorMsg + "\");");
            }
            catch (Exception)
            {
            }
        }

    }
}
