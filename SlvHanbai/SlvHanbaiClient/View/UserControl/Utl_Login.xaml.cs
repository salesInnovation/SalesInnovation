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
using System.Windows.Data;
using System.Collections.ObjectModel;  // ObservableCollectionを利用するために必要   
using System.ServiceModel.DomainServices.Client;
using SlvHanbaiClient.Class.Utility;
using SlvHanbaiClient.Class.UI;
using SlvHanbaiClient.View.Dlg;
using SlvHanbaiClient.Class.Data;
using SlvHanbaiClient.Class.WebService;
using SlvHanbaiClient.svcOrder;
using SlvHanbaiClient.View.UserControl.Custom;
using SlvHanbaiClient.Class;

#endregion

namespace SlvHanbaiClient.View.UserControl
{
    public partial class Utl_Login : ExUserControl
    {

        #region Filed Const

        private const String CLASS_NM = "Utl_Login";
        private bool blnUpdate = false;
        Dlg_Progress win;
        
        #endregion

        #region Constructor

        public Utl_Login()
        {
            InitializeComponent();
        }

        #endregion

        #region Page Events

        private void ExUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //
            // 画面初期化
            //ExVisualTreeHelper.initDisplay(this.LayoutRoot);

            // 更新プログラム確認
            this.utlLoginMain.UpdateCheck();

        }

        private void ExUserControl_Unloaded(object sender, RoutedEventArgs e)
        {
        }

        #endregion

    }
}
