﻿#pragma checksum "C:\Users\Owner\SkyDrive\SlvHanbai\SlvHanbaiClient\View\UserControl\Input\Purchase\Utl_InpPurchaseOrder.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "C1D2893752E0B7B02A8BCD7C7E863A4E"
//------------------------------------------------------------------------------
// <auto-generated>
//     このコードはツールによって生成されました。
//     ランタイム バージョン:4.0.30319.18033
//
//     このファイルへの変更は、以下の状況下で不正な動作の原因になったり、
//     コードが再生成されるときに損失したりします。
// </auto-generated>
//------------------------------------------------------------------------------

using SlvHanbaiClient.Class.UI;
using SlvHanbaiClient.View.UserControl.Custom;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace SlvHanbaiClient.View.UserControl.Input.Purchase {
    
    
    public partial class Utl_InpPurchaseOrder : SlvHanbaiClient.Class.UI.ExUserControl {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.TextBox txtDummy;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblTitle;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblNo;
        
        internal SlvHanbaiClient.View.UserControl.Custom.Utl_InpNoText utlNo;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblOrderYmd;
        
        internal SlvHanbaiClient.Class.UI.ExDatePicker datPurchaseOrderYmd;
        
        internal SlvHanbaiClient.View.UserControl.Custom.Utl_Mode utlMode;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblInpPerson;
        
        internal SlvHanbaiClient.View.UserControl.Custom.Utl_MstText utlPerson;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblNokiYmd;
        
        internal SlvHanbaiClient.Class.UI.ExDatePicker datNokiYmd;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblCustomer;
        
        internal SlvHanbaiClient.View.UserControl.Custom.Utl_MstText utlPurchase;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblSalesLimitPrice;
        
        internal SlvHanbaiClient.Class.UI.ExTextBox txtPaymentLimitPrice;
        
        internal System.Windows.Controls.Button btnSalesBalance;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblExchange;
        
        internal SlvHanbaiClient.View.UserControl.Custom.Utl_MeiText utlBusiness;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblTax;
        
        internal SlvHanbaiClient.View.UserControl.Custom.Utl_MeiText utlTax;
        
        internal SlvHanbaiClient.View.UserControl.Custom.Utl_MeiText utlSendKbn;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblSupply;
        
        internal SlvHanbaiClient.View.UserControl.Custom.Utl_MstText utlCustomer;
        
        internal SlvHanbaiClient.View.UserControl.Custom.Utl_MstText utlSupplier;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblRemark;
        
        internal SlvHanbaiClient.Class.UI.ExTextBox txtMemo;
        
        internal SlvHanbaiClient.Class.UI.ExDataGrid dg;
        
        internal System.Windows.Controls.Button btnInventory;
        
        internal SlvHanbaiClient.Class.UI.ExTextBox txtEnterNumber;
        
        internal SlvHanbaiClient.Class.UI.ExTextBox txtCaseNumber;
        
        internal SlvHanbaiClient.Class.UI.ExTextBox txtNumber;
        
        internal SlvHanbaiClient.Class.UI.ExTextBox txtUnitPrice;
        
        internal SlvHanbaiClient.Class.UI.ExTextBox txtPrice;
        
        internal SlvHanbaiClient.Class.UI.ExTextBox txtInventory;
        
        internal SlvHanbaiClient.Class.UI.ExTextBox txtTax;
        
        internal SlvHanbaiClient.Class.UI.ExTextBox txtTaxNoPrice;
        
        internal SlvHanbaiClient.Class.UI.ExTextBox txtSumPrice;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/SlvHanbaiClient;component/View/UserControl/Input/Purchase/Utl_InpPurchaseOrder.x" +
                        "aml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.txtDummy = ((System.Windows.Controls.TextBox)(this.FindName("txtDummy")));
            this.lblTitle = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblTitle")));
            this.lblNo = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblNo")));
            this.utlNo = ((SlvHanbaiClient.View.UserControl.Custom.Utl_InpNoText)(this.FindName("utlNo")));
            this.lblOrderYmd = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblOrderYmd")));
            this.datPurchaseOrderYmd = ((SlvHanbaiClient.Class.UI.ExDatePicker)(this.FindName("datPurchaseOrderYmd")));
            this.utlMode = ((SlvHanbaiClient.View.UserControl.Custom.Utl_Mode)(this.FindName("utlMode")));
            this.lblInpPerson = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblInpPerson")));
            this.utlPerson = ((SlvHanbaiClient.View.UserControl.Custom.Utl_MstText)(this.FindName("utlPerson")));
            this.lblNokiYmd = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblNokiYmd")));
            this.datNokiYmd = ((SlvHanbaiClient.Class.UI.ExDatePicker)(this.FindName("datNokiYmd")));
            this.lblCustomer = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblCustomer")));
            this.utlPurchase = ((SlvHanbaiClient.View.UserControl.Custom.Utl_MstText)(this.FindName("utlPurchase")));
            this.lblSalesLimitPrice = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblSalesLimitPrice")));
            this.txtPaymentLimitPrice = ((SlvHanbaiClient.Class.UI.ExTextBox)(this.FindName("txtPaymentLimitPrice")));
            this.btnSalesBalance = ((System.Windows.Controls.Button)(this.FindName("btnSalesBalance")));
            this.lblExchange = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblExchange")));
            this.utlBusiness = ((SlvHanbaiClient.View.UserControl.Custom.Utl_MeiText)(this.FindName("utlBusiness")));
            this.lblTax = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblTax")));
            this.utlTax = ((SlvHanbaiClient.View.UserControl.Custom.Utl_MeiText)(this.FindName("utlTax")));
            this.utlSendKbn = ((SlvHanbaiClient.View.UserControl.Custom.Utl_MeiText)(this.FindName("utlSendKbn")));
            this.lblSupply = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblSupply")));
            this.utlCustomer = ((SlvHanbaiClient.View.UserControl.Custom.Utl_MstText)(this.FindName("utlCustomer")));
            this.utlSupplier = ((SlvHanbaiClient.View.UserControl.Custom.Utl_MstText)(this.FindName("utlSupplier")));
            this.lblRemark = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblRemark")));
            this.txtMemo = ((SlvHanbaiClient.Class.UI.ExTextBox)(this.FindName("txtMemo")));
            this.dg = ((SlvHanbaiClient.Class.UI.ExDataGrid)(this.FindName("dg")));
            this.btnInventory = ((System.Windows.Controls.Button)(this.FindName("btnInventory")));
            this.txtEnterNumber = ((SlvHanbaiClient.Class.UI.ExTextBox)(this.FindName("txtEnterNumber")));
            this.txtCaseNumber = ((SlvHanbaiClient.Class.UI.ExTextBox)(this.FindName("txtCaseNumber")));
            this.txtNumber = ((SlvHanbaiClient.Class.UI.ExTextBox)(this.FindName("txtNumber")));
            this.txtUnitPrice = ((SlvHanbaiClient.Class.UI.ExTextBox)(this.FindName("txtUnitPrice")));
            this.txtPrice = ((SlvHanbaiClient.Class.UI.ExTextBox)(this.FindName("txtPrice")));
            this.txtInventory = ((SlvHanbaiClient.Class.UI.ExTextBox)(this.FindName("txtInventory")));
            this.txtTax = ((SlvHanbaiClient.Class.UI.ExTextBox)(this.FindName("txtTax")));
            this.txtTaxNoPrice = ((SlvHanbaiClient.Class.UI.ExTextBox)(this.FindName("txtTaxNoPrice")));
            this.txtSumPrice = ((SlvHanbaiClient.Class.UI.ExTextBox)(this.FindName("txtSumPrice")));
        }
    }
}

