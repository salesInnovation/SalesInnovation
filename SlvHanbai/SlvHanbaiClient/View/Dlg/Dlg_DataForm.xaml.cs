#region using

using System;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SlvHanbaiClient.Class;
using SlvHanbaiClient.Class.Data;
using SlvHanbaiClient.Class.UI;
using SlvHanbaiClient.Class.Data;
using SlvHanbaiClient.Class.WebService;
using SlvHanbaiClient.Class.Entity;
using SlvHanbaiClient.Class.Utility;
using SlvHanbaiClient.svcOrder;
using SlvHanbaiClient.svcEstimate;
using SlvHanbaiClient.svcSales;
using SlvHanbaiClient.svcReceipt;
using SlvHanbaiClient.svcPurchaseOrder;
using SlvHanbaiClient.svcPurchase;
using SlvHanbaiClient.svcPaymentCash;
using SlvHanbaiClient.View.UserControl.Custom;
using SlvHanbaiClient.View.UserControl.DataForm;
using SlvHanbaiClient.View.UserControl.DataForm.Sales;
using SlvHanbaiClient.View.UserControl.DataForm.Purchase;

#endregion

namespace SlvHanbaiClient.View.Dlg
{
    public partial class Dlg_DataForm : ExChildWindow
    {

        #region Filed Const

        private const String CLASS_NM = "Dlg_DataForm";
        public object _entityH;
        public object _entityListD;

        #endregion

        #region Constructor

        public Dlg_DataForm()
        {
            InitializeComponent();
            //this.SetWindowsResource();
            this.IsTabDefualMove = true;
        }

        #endregion

        #region Page Events

        private void ExChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.LayoutRoot.Children.Clear();

            switch (Common.gDataFormType)
            {
                case Common.geDataFormType.OrderDetail:
                    Utl_DataFormOrder utlOrder = new Utl_DataFormOrder();
                    utlOrder._entityH = (EntityOrderH)this._entityH;
                    utlOrder._entityListD = (ObservableCollection<EntityOrderD>)this._entityListD;
                    this.LayoutRoot.Children.Add(utlOrder);
                    break;
                case Common.geDataFormType.EstimateDetail:
                    Utl_DataFormEstimate utlEstimate = new Utl_DataFormEstimate();
                    utlEstimate._entityH = (EntityEstimateH)this._entityH;
                    utlEstimate._entityListD = (ObservableCollection<EntityEstimateD>)this._entityListD;
                    this.LayoutRoot.Children.Add(utlEstimate);
                    break;
                case Common.geDataFormType.SalesDetail:
                    Utl_DataFormSales utlSales = new Utl_DataFormSales();
                    utlSales._entityH = (EntitySalesH)this._entityH;
                    utlSales._entityListD = (ObservableCollection<EntitySalesD>)this._entityListD;
                    this.LayoutRoot.Children.Add(utlSales);
                    break;
                case Common.geDataFormType.ReceiptDetail:
                    Utl_DataFormReceipt utlReceipt = new Utl_DataFormReceipt();
                    utlReceipt._entityH = (EntityReceiptH)this._entityH;
                    utlReceipt._entityListD = (ObservableCollection<EntityReceiptD>)this._entityListD;
                    this.LayoutRoot.Children.Add(utlReceipt);
                    break;
                case Common.geDataFormType.PurchaseOrderDetail:
                    Utl_DataFormPurchaseOrder utlPurchaseOrder = new Utl_DataFormPurchaseOrder();
                    utlPurchaseOrder._entityH = (EntityPurchaseOrderH)this._entityH;
                    utlPurchaseOrder._entityListD = (ObservableCollection<EntityPurchaseOrderD>)this._entityListD;
                    this.LayoutRoot.Children.Add(utlPurchaseOrder);
                    break;
                case Common.geDataFormType.PurchaseDetail:
                    Utl_DataFormPurchase utlPurchase = new Utl_DataFormPurchase();
                    utlPurchase._entityH = (EntityPurchaseH)this._entityH;
                    utlPurchase._entityListD = (ObservableCollection<EntityPurchaseD>)this._entityListD;
                    this.LayoutRoot.Children.Add(utlPurchase);
                    break;
                case Common.geDataFormType.PaymentCashDetail:
                    Utl_DataFormPaymentCash utlPaymentCash = new Utl_DataFormPaymentCash();
                    utlPaymentCash._entityH = (EntityPaymentCashH)this._entityH;
                    utlPaymentCash._entityListD = (ObservableCollection<EntityPaymentCashD>)this._entityListD;
                    this.LayoutRoot.Children.Add(utlPaymentCash);
                    break;
                default:
                    break;
            }

            //this.listDisplayTabIndex = ExVisualTreeHelper.GetDisplayTabIndex(this.GridMaster); // Tab Index 保持

        }

        private void ExChildWindow_Unloaded(object sender, RoutedEventArgs e)
        {
        }

        private void ExChildWindow_KeyUp(object sender, KeyEventArgs e)
        {
        }

        #endregion

    }



}

