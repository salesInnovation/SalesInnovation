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
using SlvHanbaiClient.Class.Utility;
using SlvHanbaiClient.Class.Data;
using SlvHanbaiClient.View.Dlg;
using SlvHanbaiClient.Class.UI;
using SlvHanbaiClient.View.UserControl.Input;
using SlvHanbaiClient.View.UserControl.Input.Sales;
using SlvHanbaiClient.View.UserControl.Input.Purchase;
using SlvHanbaiClient.View.UserControl.Input.Inventory;

namespace SlvHanbaiClient.View.UserControl.Custom
{
    public partial class Utl_InpNoText : ExUserControl
    {

        #region Filed Const

        private const String CLASS_NM = "Utl_InpNoText";

        private geInpSearchKbn _InpSearchKbn;
        public geInpSearchKbn InpSearchKbn { set { this._InpSearchKbn = value; } get { return this._InpSearchKbn; } }
        public enum geInpSearchKbn 
        { 
            Order = 0, 
            Estimate,
            Sales,
            Receipt,
            Invoice,
            Purchase,
            PurchaseOrder,
            PaymentCash,
            Payment,
            InOutDelivery
        }

        public bool txtID_IsReadOnly
        {
            set
            {
                this.txtID.IsReadOnly = value;
                if (value == true)
                {
                    this.txtID.IsTabStop = false;
                    this.IsTabStop = false;
                    this.txtID.Background = new SolidColorBrush(Colors.LightGray);
                }
                else
                {
                    this.txtID.IsTabStop = true;
                    this.IsTabStop = true;
                    this.txtID.Background = new SolidColorBrush(Colors.White);
                }

            }
            get
            {
                return this.txtID.IsReadOnly;
            }
        }

        private Dlg_InpSearch searchDlg = null;
        private bool IsSearchDlgOpen = false;

        private Common.geWinGroupType _beforeWinGroupType;

        private bool _IsDobleClick = true;
        public bool IsDobleClick { set { this._IsDobleClick = value; } get { return this._IsDobleClick; } }

        public event RoutedEventHandler _ExUserControl_LostFocus;

        #endregion

        #region Constructor

        public Utl_InpNoText()
        {
            InitializeComponent();
        }

        #endregion

        #region Page Events

        private void ExUserControl_GotFocus(object sender, RoutedEventArgs e)
        {
            this.txtID.Focus();
        }

        private void ExUserControl_LostFocus(object sender, RoutedEventArgs e)
        {
            if (this._ExUserControl_LostFocus != null)
            {
                this._ExUserControl_LostFocus(sender, e);
            }
        }

        private void ExUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.txtID.InputMode = ExTextBox.geInputMode.ID;
            this.txtID.MaxLengthB = Common.gintidFigureSlipNo;
        }

        #endregion

        #region TextBox Events

        private void txtID_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ExCast.zCInt(txtID.Text.Trim()) == 0)
                txtID.Text = "";
        }

        private void txtNo_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (this.IsDobleClick == false) return;
            if (this.IsSearchDlgOpen == true) return;

            _beforeWinGroupType = Common.gWinGroupType;
            Common.gWinGroupType = Common.geWinGroupType.InpList;

            switch (InpSearchKbn)
            {
                case geInpSearchKbn.Order:
                    Common.gWinType = Common.geWinType.ListOrder;
                    Common.InpSearchOrder.Closed -= searchDlg_Closed;
                    Common.InpSearchOrder.Closed += searchDlg_Closed;
                    Common.InpSearchOrder.Show();
                    break;
                case geInpSearchKbn.Estimate:
                    Common.gWinType = Common.geWinType.ListEstimat;
                    Common.InpSearchEstimate.Closed -= searchDlg_Closed;
                    Common.InpSearchEstimate.Closed += searchDlg_Closed;
                    Common.InpSearchEstimate.Show();
                    break;
                case geInpSearchKbn.Sales:
                    Common.gWinType = Common.geWinType.ListSales;
                    Common.InpSearchSales.Closed -= searchDlg_Closed;
                    Common.InpSearchSales.Closed += searchDlg_Closed;
                    Common.InpSearchSales.Show();
                    break;
                case geInpSearchKbn.Receipt:
                    Common.gWinType = Common.geWinType.ListReceipt;
                    Common.InpSearchReceipt.Closed -= searchDlg_Closed;
                    Common.InpSearchReceipt.Closed += searchDlg_Closed;
                    Common.InpSearchReceipt.Show();
                    break;
                case geInpSearchKbn.Invoice:
                    Common.gWinType = Common.geWinType.ListInvoice;
                    Common.InpSearchInvoice.Closed -= searchDlg_Closed;
                    Common.InpSearchInvoice.Closed += searchDlg_Closed;
                    Common.InpSearchInvoice.Show();
                    break;
                case geInpSearchKbn.PurchaseOrder:
                    Common.gWinType = Common.geWinType.ListPurchaseOrder;
                    Common.InpSearchPurchaseOrder.Closed -= searchDlg_Closed;
                    Common.InpSearchPurchaseOrder.Closed += searchDlg_Closed;
                    Common.InpSearchPurchaseOrder.Show();
                    break;
                case geInpSearchKbn.Purchase:
                    Common.gWinType = Common.geWinType.ListPurchase;
                    Common.InpSearchPurchase.Closed -= searchDlg_Closed;
                    Common.InpSearchPurchase.Closed += searchDlg_Closed;
                    Common.InpSearchPurchase.Show();
                    break;
                case geInpSearchKbn.PaymentCash:
                    Common.gWinType = Common.geWinType.ListPaymentCash;
                    Common.InpSearchPaymentCash.Closed -= searchDlg_Closed;
                    Common.InpSearchPaymentCash.Closed += searchDlg_Closed;
                    Common.InpSearchPaymentCash.Show();
                    break;
                case geInpSearchKbn.Payment:
                    Common.gWinType = Common.geWinType.ListPayment;
                    Common.InpSearchPayment.Closed -= searchDlg_Closed;
                    Common.InpSearchPayment.Closed += searchDlg_Closed;
                    Common.InpSearchPayment.Show();
                    break;
                case geInpSearchKbn.InOutDelivery:
                    Common.gWinType = Common.geWinType.ListInOutDelivery;
                    Common.InpSearchInOutDelivery.Closed -= searchDlg_Closed;
                    Common.InpSearchInOutDelivery.Closed += searchDlg_Closed;
                    Common.InpSearchInOutDelivery.Show();
                    break;
            }

            this.IsSearchDlgOpen = true;
        }

        private void searchDlg_Closed(object sender, EventArgs e)
        {
            try
            {
                long _no = 0;
                bool _DialogResult = false;

                Common.gWinGroupType = _beforeWinGroupType;

                Dlg_InpSearch dlg = (Dlg_InpSearch)sender;
                ExUserControl utl = (ExUserControl)ExVisualTreeHelper.FindUserControl(dlg, "utlInpSearch");
                switch (Common.gWinType)
                {
                    case Common.geWinType.ListOrder:                // 受注一覧
                        Common.InpSearchOrder.Closed -= searchDlg_Closed;
                        Utl_InpSearchOrder utlInpOrder = (Utl_InpSearchOrder)utl;
                        _no = utlInpOrder.no;
                        _DialogResult = utlInpOrder.DialogResult;
                        break;
                    case Common.geWinType.ListEstimat:              // 見積一覧
                        Common.InpSearchEstimate.Closed -= searchDlg_Closed;
                        Utl_InpSearchEstimate utlInpEstimate = (Utl_InpSearchEstimate)utl;
                        _no = utlInpEstimate.no;
                        _DialogResult = utlInpEstimate.DialogResult;
                        break;
                    case Common.geWinType.ListSales:                // 売上一覧
                        Common.InpSearchSales.Closed -= searchDlg_Closed;
                        Utl_InpSearchSales utlInpSales = (Utl_InpSearchSales)utl;
                        _no = utlInpSales.no;
                        _DialogResult = utlInpSales.DialogResult;
                        break;
                    case Common.geWinType.ListReceipt:              // 入金一覧
                        Common.InpSearchReceipt.Closed -= searchDlg_Closed;
                        Utl_InpSearchReceipt utlInpReceipt = (Utl_InpSearchReceipt)utl;
                        _no = utlInpReceipt.no;
                        _DialogResult = utlInpReceipt.DialogResult;
                        break;
                    case Common.geWinType.ListInvoice:              // 請求一覧
                        Common.InpSearchInvoice.Closed -= searchDlg_Closed;
                        Utl_InpInvoicePrint utlInpInvoice = (Utl_InpInvoicePrint)utl;
                        _no = utlInpInvoice.no;
                        _DialogResult = utlInpInvoice.DialogResult;
                        break;
                    case Common.geWinType.ListPurchaseOrder:        // 発注一覧
                        Common.InpSearchPurchaseOrder.Closed -= searchDlg_Closed;
                        Utl_InpSearchPurchaseOrder utlInpSearchPurchaseOrder = (Utl_InpSearchPurchaseOrder)utl;
                        _no = utlInpSearchPurchaseOrder.no;
                        _DialogResult = utlInpSearchPurchaseOrder.DialogResult;
                        break;
                    case Common.geWinType.ListPurchase:             // 仕入一覧
                        Common.InpSearchPurchase.Closed -= searchDlg_Closed;
                        Utl_InpSearchPurchase utlInpSearchPurchase = (Utl_InpSearchPurchase)utl;
                        _no = utlInpSearchPurchase.no;
                        _DialogResult = utlInpSearchPurchase.DialogResult;
                        break;
                    case Common.geWinType.ListPaymentCash:          // 出金一覧
                        Common.InpSearchPaymentCash.Closed -= searchDlg_Closed;
                        Utl_InpSearchPaymentCash utlInpSearchPaymentCash = (Utl_InpSearchPaymentCash)utl;
                        _no = utlInpSearchPaymentCash.no;
                        _DialogResult = utlInpSearchPaymentCash.DialogResult;
                        break;
                    case Common.geWinType.ListPayment:              // 支払一覧
                        Common.InpSearchPayment.Closed -= searchDlg_Closed;
                        Utl_InpPaymentPrint utlInpSearchPayment = (Utl_InpPaymentPrint)utl;
                        _no = utlInpSearchPayment.no;
                        _DialogResult = utlInpSearchPayment.DialogResult;
                        break;
                    case Common.geWinType.ListInOutDelivery:        // 入出庫一覧
                        Common.InpSearchInOutDelivery.Closed -= searchDlg_Closed;
                        Utl_InpSearchInOutDelivery utlInpSearchInOutDelivery = (Utl_InpSearchInOutDelivery)utl;
                        _no = utlInpSearchInOutDelivery.no;
                        _DialogResult = utlInpSearchInOutDelivery.DialogResult;
                        break;
                    default:
                        break;
                }

                if (_DialogResult == true)
                {
                    string _str = ExCast.zFormatForID(_no, Common.gintidFigureSlipNo);
                    this.txtID.Text = _str;
                    this.txtID.Text = _str;

                    // 次コントロールフォーカスセット
                    //ExVisualTreeHelper.FoucsNextControlNoFocus(this);
                    this.Focus();
                    this.txtID.UpdataFlg = true;
                    this.ExUserControl_LostFocus(null, null);
                }
                else
                {
                    this.txtID.Focus();
                }
            }
            finally
            {
                this.IsSearchDlgOpen = false;
            }
        }

        #endregion

        #region Method

        public void ShowList()
        {
            txtNo_MouseDoubleClick(this.txtID, null);
        }

        #endregion

    }
}
