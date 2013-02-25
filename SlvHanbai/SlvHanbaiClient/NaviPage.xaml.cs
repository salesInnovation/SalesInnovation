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
using System.Windows.Navigation;
using SlvHanbaiClient.View;
using SlvHanbaiClient.View.Dlg;
using SlvHanbaiClient.Class;
using SlvHanbaiClient.Class.UI;
using SlvHanbaiClient.Class.Utility;

#endregion

namespace SlvHanbaiClient
{
    public partial class NaviPage : Page
    {
        #region Field

        private List<UserControl> _PageStack = new List<UserControl>();

        #endregion

        #region Constructor

        public NaviPage()
        {
            InitializeComponent();

            if (Application.Current.IsRunningOutOfBrowser && Common.gblnStartSettingDlg == false)
            {
                Common.gblnStartSettingDlg = true;

                // 起動時間短縮の為に画面情報を保持しておく
                Dlg_InpSearch InpSearch = null;
                Common.dataForm = new View.Dlg.Dlg_DataForm();
                Common.report = new View.Dlg.Report.Dlg_Report();
                //InpSearch = Common.InpSearchEstimate;
                //InpSearch = Common.InpSearchOrder;
                InpSearch = Common.InpSearchSales;
                //InpSearch = Common.InpSearchReceipt;
                //InpSearch = Common.InpSearchEstimateRpt;
                //InpSearch = Common.InpSearchOrderRpt;
                //InpSearch = Common.InpSearchSalesRpt;
                //InpSearch = Common.InpSearchReceiptRpt;
            }

            Common.gPageGroupType = Common.gePageGroupType.StartUp;
            if (Application.Current.InstallState == InstallState.NotInstalled)
            {
                Common.gPageType = Common.gePageType.Install;
            }
            else
            {
                if (Application.Current.IsRunningOutOfBrowser)
                {
                    Common.gPageType = Common.gePageType.Login;
                }
                else 
                {
                    Common.gPageType = Common.gePageType.Install;
                }
            }
            UA_Main _main = new UA_Main();
            this.Content = _main;
            Common._main = _main;
        }

        #endregion

        #region PageNavigation

        // ユーザーがこのページに移動したときに実行されます。
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        #endregion

        #region Method

        public void PageForward(UserControl Page)
        {
            _PageStack.Add((UserControl)this.Content);
            this.Content=Page;
        }

        public void PageBack()
        {
            int intCnt = this._PageStack.Count -1;
            this.Content = this._PageStack[intCnt];
            this._PageStack.RemoveAt(intCnt);
            GC.Collect();
        }

        #endregion

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {

        }

    }
}
