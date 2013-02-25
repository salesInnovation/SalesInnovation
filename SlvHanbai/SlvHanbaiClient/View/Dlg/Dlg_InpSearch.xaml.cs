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
using SlvHanbaiClient.Class.Data;
using SlvHanbaiClient.Class.UI;
using SlvHanbaiClient.Class.Data;
using SlvHanbaiClient.Class.WebService;
using SlvHanbaiClient.svcOrder;
using SlvHanbaiClient.View.UserControl;
using SlvHanbaiClient.View.UserControl.Input;
using SlvHanbaiClient.View.UserControl.Input.Sales;
using SlvHanbaiClient.View.UserControl.Input.Purchase;
using SlvHanbaiClient.View.UserControl.Input.Inventory;
using SlvHanbaiClient.View.UserControl.Custom;

namespace SlvHanbaiClient.View.Dlg
{
    public partial class Dlg_InpSearch : ExChildWindow
    {

        #region Filed Const

        private const String CLASS_NM = "Dlg_InpSearch";
        private ObservableCollection<EntityOrder> objOrderList;

        private ExUserControl utl = null;

        // リターンID
        public long no;

        // リターンリスト
        private object _lst;
        public object lst { set { this._lst = value; } get { return this._lst; } }

        #endregion

        #region Constructor

        public Dlg_InpSearch()
        {
            InitializeComponent();

            Init();
        }

        #endregion

        #region Page Events

        private void ExChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Load時の証跡を保存
            DataPgEvidence.SaveLoadOrUnLoadEvidence(Common.gWinGroupType, Common.gWinType, DataPgEvidence.geOperationType.Start);

            Init_Load();

            this.lst = null;
            this.listDisplayTabIndex = ExVisualTreeHelper.GetDisplayTabIndex(this.GridMain); // Tab Index 保持
        }

        private void ExChildWindow_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                // ファンクションキーを受け取る
                //case Key.F1: this.btnF1_Click(this.btnF1, null); break;
                //case Key.Enter: this.btnF1_Click(this.btnF1, null); break;
                //case Key.F12: this.btnF12_Click(this.btnF12, null); break;
                default: break;
            }
        }

        private void ExChildWindow_Unloaded(object sender, RoutedEventArgs e)
        {
            // UnLoad時の証跡を保存
            DataPgEvidence.SaveLoadOrUnLoadEvidence(Common.gWinGroupType, Common.gWinType, DataPgEvidence.geOperationType.End);
        }

        #endregion

        #region Method

        public void Init()
        {
            this.GridMain.Children.Clear();

            switch (Common.gWinGroupType)
            {
                case Common.geWinGroupType.InpList:
                    switch (Common.gWinType)
                    {
                        case Common.geWinType.ListEstimat:              // 見積一覧
                            this.Title = "見積一覧";
                            utl = new Utl_InpSearchEstimate();
                            break;
                        case Common.geWinType.ListOrder:                // 受注一覧
                            this.Title = "受注一覧";
                            utl = new Utl_InpSearchOrder();
                            break;
                        case Common.geWinType.ListSales:                // 売上一覧
                            this.Title = "売上一覧";
                            utl = new Utl_InpSearchSales();
                            break;
                        case Common.geWinType.ListReceipt:              // 入金一覧
                            this.Title = "入金一覧";
                            utl = new Utl_InpSearchReceipt();
                            break;
                        case Common.geWinType.ListInvoice:              // 請求一覧
                            this.Title = "請求一覧";
                            utl = new Utl_InpInvoicePrint();
                            break;
                        case Common.geWinType.ListPurchaseOrder:        // 発注一覧
                            this.Title = "発注一覧";
                            utl = new Utl_InpSearchPurchaseOrder();
                            break;
                        case Common.geWinType.ListPurchase:             // 仕入一覧
                            this.Title = "仕入一覧";
                            utl = new Utl_InpSearchPurchase();
                            break;
                        case Common.geWinType.ListPaymentCash:          // 出金一覧
                            this.Title = "出金一覧";
                            utl = new Utl_InpSearchPaymentCash();
                            break;
                        case Common.geWinType.ListPayment:              // 支払一覧
                            this.Title = "支払一覧";
                            utl = new Utl_InpPaymentPrint();
                            break;
                        case Common.geWinType.ListInOutDelivery:        // 入出庫一覧
                            this.Title = "入出庫一覧";
                            utl = new Utl_InpSearchInOutDelivery ();
                            break;
                        default:
                            break;
                    }
                    break;
                case Common.geWinGroupType.InpListUpd:
                    switch (Common.gWinType)
                    {
                        case Common.geWinType.ListInvoiceBalance:           // 請求残高
                            this.Title = "請求残高";
                            utl = new Utl_InpInvoiceBalance();
                            break;
                        case Common.geWinType.ListSalesCreditBalance:       // 売掛残高
                            this.Title = "売掛残高";
                            utl = new Utl_InpSalesCreditBalance();
                            break;
                        case Common.geWinType.ListPaymentBalance:           // 支払残高
                            this.Title = "支払残高";
                            utl = new Utl_InpPaymentBalance();
                            break;
                        case Common.geWinType.ListPaymentCreditBalance:     // 買掛残高
                            this.Title = "買掛残高";
                            utl = new Utl_InpPaymentCreditBalance();
                            break;
                        default:
                            break;
                    }
                    break;
                case Common.geWinGroupType.InpListReport:
                    //this.Height = 320;
                    //this.VerticalAlignment = System.Windows.VerticalAlignment.Top;

                    switch (Common.gWinType)
                    {
                        case Common.geWinType.ListEstimat:              // 見積一覧
                            this.Title = "レポート出力：見積書";
                            utl = new Utl_InpSearchEstimate();
                            break;
                        case Common.geWinType.ListOrder:                // 受注一覧
                            this.Title = "レポート出力：注文請書";
                            utl = new Utl_InpSearchOrder();
                            break;
                        case Common.geWinType.ListSales:                // 売上一覧
                            this.Title = "レポート出力：納品書";
                            utl = new Utl_InpSearchSales();
                            break;
                        case Common.geWinType.ListInvoice:              // 請求一覧
                            this.Title = "レポート出力：請求書";
                            utl = new Utl_InpInvoicePrint();
                            this.Height = 700;
                            break;
                        case Common.geWinType.ListReceipt:              // 入金一覧
                            this.Title = "レポート出力：入金書";
                            utl = new Utl_InpSearchReceipt();
                            break;
                        case Common.geWinType.ListSalesCreditBalance:   // 売掛残高一覧
                            this.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                            this.Title = "レポート出力：売掛残高一覧表";
                            this.Height = 320;
                            utl = new Utl_InpSalesCreditBalance();
                            break;
                        case Common.geWinType.ListInvoiceBalance:       // 請求残高一覧
                            this.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                            this.Title = "レポート出力：請求残高一覧表";
                            this.Height = 200;
                            utl = new Utl_InpInvoiceBalance();
                            break;
                        case Common.geWinType.ListPaymentCreditBalance:   // 買掛残高一覧
                            this.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                            this.Title = "レポート出力：買掛残高一覧表";
                            this.Height = 320;
                            utl = new Utl_InpPaymentCreditBalance();
                            break;
                        case Common.geWinType.ListPaymentBalance:       // 支払残高一覧
                            this.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                            this.Title = "レポート出力：支払残高一覧表";
                            this.Height = 200;
                            utl = new Utl_InpPaymentBalance();
                            break;
                        case Common.geWinType.ListPurchaseOrder:        // 発注一覧
                            this.Title = "レポート出力：注文書";
                            utl = new Utl_InpSearchPurchaseOrder();
                            break;
                        case Common.geWinType.ListPayment:              // 支払一覧
                            this.Title = "レポート出力：支払書";
                            utl = new Utl_InpPaymentPrint();
                            this.Height = 700;
                            break;
                        case Common.geWinType.ListInOutDelivery:        // 入出庫一覧
                            this.Title = "レポート出力：入出庫一覧表";
                            utl = new Utl_InpSearchInOutDelivery();
                            break;
                        case Common.geWinType.ListInventory:            // 在庫一覧
                            this.Title = "レポート出力：在庫一覧表";
                            utl = new Utl_InpSearchInventory();
                            break;
                        default:
                            break;
                    }
                    break;
                case Common.geWinGroupType.InpDetailReport:
                    this.Height = 320;
                    this.VerticalAlignment = System.Windows.VerticalAlignment.Top;

                    switch (Common.gWinType)
                    {
                        case Common.geWinType.ListEstimat:              // 見積一覧
                            this.Title = "レポート出力：見積明細書";
                            utl = new Utl_InpSearchEstimate();
                            break;
                        case Common.geWinType.ListOrder:                // 受注一覧
                            this.Title = "レポート出力：受注明細書";
                            utl = new Utl_InpSearchOrder();
                            break;
                        case Common.geWinType.ListSales:                // 売上一覧
                            this.Title = "レポート出力：売上明細書";
                            utl = new Utl_InpSearchSales();
                            break;
                        case Common.geWinType.ListReceipt:              // 入金一覧
                            this.Title = "レポート出力：入金明細書";
                            utl = new Utl_InpSearchReceipt();
                            break;
                        case Common.geWinType.ListCollectPlan:          // 回収予定表
                            this.Title = "レポート出力：回収予定表";
                            utl = new Utl_InpSearchPlan();
                            break;
                        case Common.geWinType.ListPurchaseOrder:        // 発注一覧
                            this.Title = "レポート出力：発注明細書";
                            utl = new Utl_InpSearchPurchaseOrder();
                            break;
                        case Common.geWinType.ListPurchase:             // 仕入一覧
                            this.Title = "レポート出力：仕入明細書";
                            utl = new Utl_InpSearchPurchase();
                            break;
                        case Common.geWinType.ListPaymentCash:          // 出金一覧
                            this.Title = "レポート出力：出金明細書";
                            utl = new Utl_InpSearchPaymentCash();
                            break;
                        case Common.geWinType.ListPaymentPlan:          // 支払予定表
                            this.Title = "レポート出力：支払予定表";
                            utl = new Utl_InpSearchPlan();
                            break;
                        default:
                            break;
                    }
                    break;
            }

            utl.Name = "utlInpSearch";
            this.GridMain.Children.Add(utl);
            this.SetWindowsResource();
        }

        public void Init_Load()
        {
            this.Height = 700;

            switch (Common.gWinGroupType)
            {
                case Common.geWinGroupType.InpList:
                    switch (Common.gWinType)
                    {
                        case Common.geWinType.ListEstimat:              // 見積一覧
                            this.Title = "見積一覧";
                            break;
                        case Common.geWinType.ListOrder:                // 受注一覧
                            this.Title = "受注一覧";
                            break;
                        case Common.geWinType.ListSales:                // 売上一覧
                            this.Title = "売上一覧";
                            break;
                        case Common.geWinType.ListReceipt:              // 入金一覧
                            this.Title = "入金一覧";
                            break;
                        case Common.geWinType.ListInvoice:              // 請求一覧
                            this.Title = "請求一覧";
                            break;
                        case Common.geWinType.ListPurchaseOrder:        // 発注一覧
                            this.Title = "発注一覧";
                            break;
                        case Common.geWinType.ListPurchase:             // 仕入一覧
                            this.Title = "仕入一覧";
                            break;
                        case Common.geWinType.ListPaymentCash:          // 出金一覧
                            this.Title = "出金一覧";
                            break;
                        case Common.geWinType.ListPayment:              // 支払一覧
                            this.Title = "支払一覧";
                            break;
                        case Common.geWinType.ListInOutDelivery:        // 入出庫一覧
                            this.Title = "入出庫一覧";
                            break;
                        default:
                            break;
                    }
                    break;
                case Common.geWinGroupType.InpListUpd:
                    switch (Common.gWinType)
                    {
                        case Common.geWinType.ListInvoiceBalance:           // 請求残高
                            this.Title = "請求残高";
                            break;
                        case Common.geWinType.ListSalesCreditBalance:       // 売掛残高
                            this.Title = "売掛残高";
                            break;
                        case Common.geWinType.ListPaymentBalance:           // 支払残高
                            this.Title = "支払残高";
                            break;
                        case Common.geWinType.ListPaymentCreditBalance:     // 買掛残高
                            this.Title = "買掛残高";
                            break;
                        default:
                            break;
                    }
                    break;
                case Common.geWinGroupType.InpListReport:
                    //this.Height = 320;
                    //this.VerticalAlignment = System.Windows.VerticalAlignment.Top;

                    switch (Common.gWinType)
                    {
                        case Common.geWinType.ListEstimat:                  // 見積一覧
                            this.Title = "レポート出力：見積書";
                            break;
                        case Common.geWinType.ListOrder:                    // 受注一覧
                            this.Title = "レポート出力：注文請書";
                            break;
                        case Common.geWinType.ListSales:                    // 売上一覧
                            this.Title = "レポート出力：納品書";
                            break;
                        case Common.geWinType.ListInvoice:                  // 請求一覧
                            this.Title = "レポート出力：請求書";
                            this.Height = 700;
                            break;
                        case Common.geWinType.ListReceipt:                  // 入金一覧
                            this.Title = "レポート出力：入金書";
                            break;
                        case Common.geWinType.ListSalesCreditBalance:       // 売掛残高一覧
                            this.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                            this.Title = "レポート出力：売掛残高一覧表";
                            this.Height = 250;
                            break;
                        case Common.geWinType.ListInvoiceBalance:           // 請求残高一覧
                            this.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                            this.Title = "レポート出力：請求残高一覧表";
                            this.Height = 200;
                            break;
                        case Common.geWinType.ListPaymentCreditBalance:     // 買掛残高一覧
                            this.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                            this.Title = "レポート出力：買掛残高一覧表";
                            this.Height = 250;
                            break;
                        case Common.geWinType.ListPaymentBalance:           // 支払残高一覧
                            this.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                            this.Title = "レポート出力：支払残高一覧表";
                            this.Height = 200;
                            break;
                        case Common.geWinType.ListPurchaseOrder:            // 発注一覧
                            this.Title = "レポート出力：注文書";
                            break;
                        case Common.geWinType.ListPayment:                  // 支払一覧
                            this.Title = "レポート出力：支払書";
                            this.Height = 700;
                            break;
                        case Common.geWinType.ListInOutDelivery:        // 入出庫一覧
                            this.Title = "レポート出力：入出庫一覧表";
                            break;
                        case Common.geWinType.ListInventory:            // 在庫一覧
                            this.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                            this.Title = "レポート出力：在庫一覧表";
                            this.Height = 250;
                            break;
                        default:
                            break;
                    }
                    break;
                case Common.geWinGroupType.InpDetailReport:
                    this.Height = 320;
                    this.VerticalAlignment = System.Windows.VerticalAlignment.Top;

                    switch (Common.gWinType)
                    {
                        case Common.geWinType.ListEstimat:              // 見積明細書
                            this.Title = "レポート出力：見積明細書";
                            this.Height = 300;
                            break;
                        case Common.geWinType.ListOrder:                // 受注明細書
                            this.Title = "レポート出力：受注明細書";
                            break;
                        case Common.geWinType.ListSales:                // 売上明細書
                            this.Title = "レポート出力：売上明細書";
                            break;
                        case Common.geWinType.ListSalesDay:             // 売上日報
                            this.Title = "レポート出力：売上日報";
                            break;
                        case Common.geWinType.ListSalesMonth:           // 売上月報
                            this.Title = "レポート出力：売上月報";
                            break;
                        case Common.geWinType.ListSalesChange:          // 売上推移表
                            this.Title = "レポート出力：売上推移表";
                            break;
                        case Common.geWinType.ListReceipt:              // 入金明細書
                            this.Title = "レポート出力：入金明細書";
                            this.Height = 200;
                            break;
                        case Common.geWinType.ListReceiptDay:           // 入金日報
                            this.Title = "レポート出力：入金日報";
                            this.Height = 200;
                            break;
                        case Common.geWinType.ListReceiptMonth:         // 入金月報
                            this.Title = "レポート出力：入金月報";
                            this.Height = 200;
                            break;
                        case Common.geWinType.ListCollectPlan:          // 回収予定表
                            this.Title = "レポート出力：回収予定表";
                            this.Height = 190;
                            break;
                        case Common.geWinType.ListPurchaseOrder:        // 発注明細書
                            this.Title = "レポート出力：発注明細書";
                            this.Height = 310;
                            break;
                        case Common.geWinType.ListPurchase:             // 仕入明細書
                            this.Title = "レポート出力：仕入明細書";
                            this.Height = 330;
                            break;
                        case Common.geWinType.ListPurchaseDay:          // 仕入日報
                            this.Title = "レポート出力：仕入日報";
                            this.Height = 330;
                            break;
                        case Common.geWinType.ListPurchaseMonth:        // 仕入月報
                            this.Title = "レポート出力：仕入月報";
                            this.Height = 330;
                            break;
                        case Common.geWinType.ListPurchaseChange:       // 仕入推移表
                            this.Title = "レポート出力：仕入推移表";
                            this.Height = 330;
                            break;
                        case Common.geWinType.ListPaymentCash:          // 出金明細書
                            this.Title = "レポート出力：出金明細書";
                            this.Height = 200;
                            break;
                        case Common.geWinType.ListPaymentCashDay:       // 出金日報
                            this.Title = "レポート出力：出金日報";
                            this.Height = 200;
                            break;
                        case Common.geWinType.ListPaymentCashMonth:     // 出金月報
                            this.Title = "レポート出力：出金月報";
                            this.Height = 200;
                            break;
                        case Common.geWinType.ListPaymentPlan:          // 支払予定表
                            this.Title = "レポート出力：支払予定表";
                            this.Height = 190;
                            break;
                        default:
                            break;
                    }
                    break;
            }

            utl.Name = "utlInpSearch";
            utl.Init_SearchDisplay();
        }

        #endregion

    }
}

