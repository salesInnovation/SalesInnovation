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
using SlvHanbaiClient.Class.UI;
using SlvHanbaiClient.Class.Data;

#endregion

namespace SlvHanbaiClient.View.Dlg
{
    public partial class Dlg_Copying : ExChildWindow
    {

        #region Filed Const

        private const String CLASS_NM = "Dlg_Copying";

        #endregion

        #region Constructor

        public Dlg_Copying()
        {
            InitializeComponent();
            //this.SetWindowsResource();
            this.Tag = "Main";      // ファンクションキーを受けつけ用
        }

        #endregion

        #region Page Events

        private void ExChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.listDisplayTabIndex = ExVisualTreeHelper.GetDisplayTabIndex(this.LayoutRoot); // Tab Index 保持
        }

        #endregion

    }
}

