#region using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;
using SlvHanbaiClient.Class;
using SlvHanbaiClient.Class.UI;
using SlvHanbaiClient.Class.Utility;
using SlvHanbaiClient.Class.Data;
using SlvHanbaiClient.Class.WebService;
using SlvHanbaiClient.svcSysLogin;

#endregion

namespace SlvHanbaiClient.View.Dlg.Report
{
    public partial class Dlg_ReportView : ExChildWindow
    {

        #region Filed Const

        private const String CLASS_NM = "Dlg_ReportView";

        public string reportUrl = "";
        public string reportFileName = "";
        public string reportFilePath = "";

        public DataReport.geReportKbn rptKbn;
        public string pgId;

        private ExBackgroundReportViewWk bk = new ExBackgroundReportViewWk();

        #endregion

        #region Constructor

        public Dlg_ReportView()
        {
            InitializeComponent();
            //this.SetWindowsResource();
            this.Tag = "Main";      // ファンクションキーを受けつけ用
        }

        #endregion

        #region Page Events

        private void ExChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.stlProcess.Visibility = Visibility.Visible;
            this.utlReortView.webBrowser.Source = null;
        }

        private void ExChildWindow_Closed(object sender, EventArgs e)
        {
            this.utlReortView.webBrowser.Source = null;
            this.stlProcess.Visibility = Visibility.Collapsed;
            this.utlReortView.Visibility = Visibility.Collapsed;
        }

        #endregion

        #region Method

        public void ViewReport()
        {
            bk.reportView = this;
            string _url = reportUrl.Replace(this.reportFileName, "") + System.Windows.Browser.HttpUtility.UrlEncode(this.reportFileName);
            _url = Common.gstrReportViewUrl;

            string viewFileName = System.Windows.Browser.HttpUtility.UrlEncode(this.reportFileName);
            string viewFilePath = this.reportFilePath.Replace(this.reportFileName, System.Windows.Browser.HttpUtility.UrlEncode(this.reportFileName)).Replace(@"\", "@AAB@").Replace("/", "@AAD@");
            var requestUri = string.Format("{0}?random={1}&viewFileName={2}&viewFilePath={3}", Common.gstrReportViewUrl, Common.gstrSessionString, viewFileName, viewFilePath);

            bk.uri = new Uri(requestUri);
            bk.bw.RunWorkerAsync();
        }

        #endregion

    }
}

