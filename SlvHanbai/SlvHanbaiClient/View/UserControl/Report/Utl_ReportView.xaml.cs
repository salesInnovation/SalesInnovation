using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using SlvHanbaiClient.Class.Utility;
using SlvHanbaiClient.Class.Data;
using SlvHanbaiClient.View.Dlg;
using SlvHanbaiClient.View.Dlg.Report;
using SlvHanbaiClient.Class.UI;
using SlvHanbaiClient.Class;
using SlvHanbaiClient.svcReport;
using SlvHanbaiClient.svcSysLogin;
using SlvHanbaiClient.Class.WebService;

namespace SlvHanbaiClient.View.UserControl.Report
{
    public partial class Utl_ReportView : ExUserControl
    {

        #region Filed Const

        private const String CLASS_NM = "Utl_ReportView";

        #endregion

        #region Constructor

        public Utl_ReportView()
        {
            InitializeComponent();
            //this.btnF12.Content = "     F12    " + Environment.NewLine + "   Close";

        }

        #endregion

        #region Function Key Button Method

        // F12ボタン(キャンセル) クリック
        public override void btnF12_Click(object sender, RoutedEventArgs e)
        {
            Dlg_ReportView win = (Dlg_ReportView)ExVisualTreeHelper.FindPerentChildWindow(this);
            win.Close();
        }

        #endregion

        private void ExUserControl_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.F12: this.btnF12_Click(null, e); break;
                default: break;
            }
        }

        private void LayoutRoot_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.F12: this.btnF12_Click(null, e); break;
                default: break;
            }
        }

        private void webBrowser_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.F12: this.btnF12_Click(null, e); break;
                default: break;
            }
        }

    }
}
