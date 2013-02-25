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
using System.Collections.ObjectModel;
using SlvHanbaiClient.Class;
using SlvHanbaiClient.Class.UI;
using SlvHanbaiClient.Class.Data;
using SlvHanbaiClient.Class.WebService;
using SlvHanbaiClient.svcOrder;
using SlvHanbaiClient.View.UserControl.Custom;

#endregion

namespace SlvHanbaiClient.View.Dlg
{
    public partial class Dlg_Login : ExChildWindow
    {
        public Dlg_Login()
        {
            InitializeComponent();
            this.SetWindowsResource();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void ExChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.listDisplayTabIndex = ExVisualTreeHelper.GetDisplayTabIndex(this.LayoutRoot); // Tab Index 保持
            this.utlLoginMain.btnLogin.Focus();
        }

    }
}

