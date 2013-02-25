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
using SlvHanbaiClient.Class;
using SlvHanbaiClient.Class.UI;
using SlvHanbaiClient.Class.Utility;

#endregion

namespace SlvHanbaiClient.View.Dlg
{
    public partial class Dlg_Progress : ExChildWindow
    {
        #region Constructor

        public Dlg_Progress()
        {
            InitializeComponent();
            //this.SetWindowsResource();
        }

        #endregion

        private void ExChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (Common.gstrProgressDialogTitle != "")
            {
                this.Title = Common.gstrProgressDialogTitle;
            }
            else
            {
                this.Title = "処理中";
            }

            if (Common.gstrProgressDialogMsg != "")
            {
                this.tbkMsg.Text = Common.gstrProgressDialogMsg;
            }
            else
            {
                this.tbkMsg.Text = "しばらくお待ちください";
            }

            Common.gstrProgressDialogTitle = "";
            Common.gstrProgressDialogMsg = "";
        }

        private void ExChildWindow_Unloaded(object sender, RoutedEventArgs e)
        {
            Common.gstrProgressDialogTitle = "";
            Common.gstrProgressDialogMsg = "";
        }

    }
}

