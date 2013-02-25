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
using SlvHanbaiClient.Class.UI;
using SlvHanbaiClient.View.Dlg;
using SlvHanbaiClient.View.Dlg.Theme;
using SlvHanbaiClient.Themes;
using SlvHanbaiClient.Class;

namespace SlvHanbaiClient.View.UserControl.Custom
{
    public partial class Utl_FooterBar : ExUserControl
    {

        #region Filed Const

        private UA_Main _perentPage;
        public UA_Main perentPage
        {
            set { this._perentPage = value; }
            get { return this._perentPage; }
        }

        private bool IsSearchDlgOpen = false;

        private DispatcherTimer timer = new DispatcherTimer();
 
        #endregion

        public Utl_FooterBar()
        {
            InitializeComponent();

            timer.Interval = new TimeSpan(0, 0, 0, 0, 30000);
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
        }

        private void imgTheme_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (this.IsSearchDlgOpen == true) return;

            Dlg_ThemeChange win = new Dlg_ThemeChange();
            win.Show();
            this.IsSearchDlgOpen = true;
            win.Closed += win_Closed;
        }

        private void win_Closed(object sender, EventArgs e)
        {
            try
            {
                string url;
                switch (MargedResourceDictionary.gThemeType)
                {
                    case MargedResourceDictionary.geThemeType.BureauBlack:
                        url = "/SlvHanbaiClient;component/Image/Theme/bureauBlack.png";
                        break;
                    case MargedResourceDictionary.geThemeType.ShinyRed:
                        url = "/SlvHanbaiClient;component/Image/Theme/shinyRed.png";
                        break;
                    case MargedResourceDictionary.geThemeType.None:
                        url = "/SlvHanbaiClient;component/Image/Theme/default.png";
                        break;
                    case MargedResourceDictionary.geThemeType.WhistlerBlue:
                        url = "/SlvHanbaiClient;component/Image/Theme/whistlerBlue.png";
                        break;
                    case MargedResourceDictionary.geThemeType.TwilightBlue:
                        url = "/SlvHanbaiClient;component/Image/Theme/twilightBlue.png";
                        break;
                    case MargedResourceDictionary.geThemeType.ExpressionDark:
                        url = "/SlvHanbaiClient;component/Image/Theme/expressionDark.png";
                        break;
                    case MargedResourceDictionary.geThemeType.BubbleCreme:
                        url = "/SlvHanbaiClient;component/Image/Theme/bubbleCreme.png";
                        break;
                    case MargedResourceDictionary.geThemeType.BureauBlue:
                        url = "/SlvHanbaiClient;component/Image/Theme/bureauBlue.png";
                        break;
                    case MargedResourceDictionary.geThemeType.ExpressionLight:
                        url = "/SlvHanbaiClient;component/Image/Theme/expressionLight.png";
                        break;
                    case MargedResourceDictionary.geThemeType.RainerOrange:
                        url = "/SlvHanbaiClient;component/Image/Theme/rainerOrange.png";
                        break;
                    case MargedResourceDictionary.geThemeType.RainerPurple:
                        url = "/SlvHanbaiClient;component/Image/Theme/rainerPurple.png";
                        break;
                    case MargedResourceDictionary.geThemeType.ShinyBlue:
                        url = "/SlvHanbaiClient;component/Image/Theme/shinyBlue.png";
                        break;
                    default:
                        url = "/SlvHanbaiClient;component/Image/Theme/shinyBlue.png";
                        break;
                }

                Uri uriSource = new Uri(url, UriKind.RelativeOrAbsolute);
                this.imgTheme.Source = new System.Windows.Media.Imaging.BitmapImage(uriSource);

                perentPage.SetRecource();
                ExVisualTreeHelper.SetResource(perentPage.LayoutRoot);
            }
            finally
            {
                this.IsSearchDlgOpen = false;
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            //if (Common.gblnStartSettingDlg == false)
            //{
            //    Common.gblnStartSettingDlg = true;

            //    // 起動時間短縮の為に画面情報を保持しておく
            //    Dlg_InpSearch InpSearch = null;
            //    Common.dataForm = new View.Dlg.Dlg_DataForm();
            //    Common.report = new View.Dlg.Dlg_Report();
            //    Common.reportView = new View.Dlg.Dlg_ReportView();
            //    InpSearch = Common.InpSearchEstimate;
            //    InpSearch = Common.InpSearchOrder;
            //    InpSearch = Common.InpSearchSales;
            //    InpSearch = Common.InpSearchReceipt;
            //    InpSearch = Common.InpSearchEstimateRpt;
            //    InpSearch = Common.InpSearchOrderRpt;
            //    InpSearch = Common.InpSearchSalesRpt;
            //    InpSearch = Common.InpSearchReceiptRpt;
            //}

            this.txtTimer.Text = DateTime.Now.ToString("MM/dd HH:mm");
        }

        private void ExUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.txtTimer.Text = DateTime.Now.ToString("MM/dd HH:mm");
        }
 
    }
}
