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
using SlvHanbaiClient.Class.Data;

#endregion

namespace SlvHanbaiClient.View.Dlg.Report
{
    public partial class Dlg_Report : ExChildWindow
    {

        #region Filed Const

        private const String CLASS_NM = "Dlg_Report";

        #endregion

        #region Constructor

        public Dlg_Report()
        {
            InitializeComponent();
            this.SetWindowsResource();
            this.Tag = "Main";      // ファンクションキーを受けつけ用
        }

        #endregion

        #region Page Events

        private void ExChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.listDisplayTabIndex = ExVisualTreeHelper.GetDisplayTabIndex(this.LayoutRoot); // Tab Index 保持

            if (this.utlReport.gPageType != Common.gePageType.None)
            {
                switch (this.utlReport.gPageType)
                {
                    case Common.gePageType.InpOrder:
                        break;
                }
            }
            else
            {
                switch (this.utlReport.gWinMsterType)
                {
                    case Common.geWinMsterType.Customer:
                        this.Title = "得意先マスタ出力";
                        break;
                    case Common.geWinMsterType.Supplier:
                        this.Title = "納入先マスタ出力";
                        break;
                    case Common.geWinMsterType.Purchase:
                        this.Title = "仕入先マスタ出力";
                        break;
                    case Common.geWinMsterType.Person:
                        this.Title = "担当マスタ出力";
                        break;
                    case Common.geWinMsterType.Commodity:
                        this.Title = "商品マスタ出力";
                        break;
                    case Common.geWinMsterType.Condition:
                        this.Title = "締区分マスタ出力";
                        break;
                    case Common.geWinMsterType.Class:
                        this.Title = "分類マスタ出力";
                        break;
                }
            }
        }

        #endregion

    }
}

