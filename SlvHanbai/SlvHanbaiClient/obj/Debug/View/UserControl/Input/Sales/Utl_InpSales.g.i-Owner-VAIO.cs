﻿#pragma checksum "C:\Users\chikugo\Documents\Visual Studio 2010\Projects\SlvHanbai\SlvHanbaiClient\View\UserControl\Input\Sales\Utl_InpSales.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "0DA2B7D09F232EF85AE2F7336DA9F43F"
//------------------------------------------------------------------------------
// <auto-generated>
//     このコードはツールによって生成されました。
//     ランタイム バージョン:4.0.30319.237
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


namespace SlvHanbaiClient.View.UserControl.Input.Sales {
    
    
    public partial class Utl_InpSales : SlvHanbaiClient.Class.UI.ExUserControl {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.TextBox txtDummy;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblTitle;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblNo;
        
        internal SlvHanbaiClient.View.UserControl.Custom.Utl_InpNoText utlNo;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblInpPerson;
        
        internal SlvHanbaiClient.View.UserControl.Custom.Utl_MstText utlPerson;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblOrderYmd;
        
        internal SlvHanbaiClient.Class.UI.ExDatePicker datSalesYmd;
        
        internal SlvHanbaiClient.View.UserControl.Custom.Utl_Mode utlMode;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblEstimateNo;
        
        internal SlvHanbaiClient.View.UserControl.Custom.Utl_InpNoText utlEstimateNo;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblOrderNo;
        
        internal SlvHanbaiClient.View.UserControl.Custom.Utl_InpNoText utlOrderNo;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblNokiYmd;
        
        internal SlvHanbaiClient.Class.UI.ExDatePicker datNokiYmd;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblCustomer;
        
        internal SlvHanbaiClient.View.UserControl.Custom.Utl_MstText utlCustomer;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblInvoice;
        
        internal SlvHanbaiClient.View.UserControl.Custom.Utl_MstText utlInvoice;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblSupply;
        
        internal SlvHanbaiClient.View.UserControl.Custom.Utl_MstText utlSupply;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblInvoiceNo;
        
        internal SlvHanbaiClient.Class.UI.ExTextBox txtInvoiceNo;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblInvoiceState;
        
        internal SlvHanbaiClient.Class.UI.ExTextBox txtInvoiceState;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblTax;
        
        internal SlvHanbaiClient.View.UserControl.Custom.Utl_MeiText utlTax;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblExchange;
        
        internal SlvHanbaiClient.View.UserControl.Custom.Utl_MeiText utlBusiness;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblCreditLimitPrice;
        
        internal SlvHanbaiClient.Class.UI.ExTextBox txtCreditLimitPrice;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblReceiptState;
        
        internal SlvHanbaiClient.Class.UI.ExTextBox txtReceiptState;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblUnitKind;
        
        internal SlvHanbaiClient.View.UserControl.Custom.Utl_MeiText utlUnitKind;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblCreditRate;
        
        internal SlvHanbaiClient.Class.UI.ExTextBox txtCreditRate;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblSalesLimitPrice;
        
        internal SlvHanbaiClient.Class.UI.ExTextBox txtSalesLimitPrice;
        
        internal System.Windows.Controls.Button btnSalesBalance;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblRemark;
        
        internal SlvHanbaiClient.Class.UI.ExTextBox txtMemo;
        
        internal SlvHanbaiClient.Class.UI.ExDataGrid dg;
        
        internal System.Windows.Controls.Button btnInventory;
        
        internal SlvHanbaiClient.Class.UI.ExTextBox txtOrderStillNumber;
        
        internal SlvHanbaiClient.Class.UI.ExTextBox txtEnterNumber;
        
        internal SlvHanbaiClient.Class.UI.ExTextBox txtCaseNumber;
        
        internal SlvHanbaiClient.Class.UI.ExTextBox txtNumber;
        
        internal SlvHanbaiClient.Class.UI.ExTextBox txtUnitPrice;
        
        internal SlvHanbaiClient.Class.UI.ExTextBox txtPrice;
        
        internal SlvHanbaiClient.Class.UI.ExTextBox txtInventory;
        
        internal SlvHanbaiClient.Class.UI.ExTextBox txtProfits;
        
        internal SlvHanbaiClient.Class.UI.ExTextBox txtProfitsPercent;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/SlvHanbaiClient;component/View/UserControl/Input/Sales/Utl_InpSales.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.txtDummy = ((System.Windows.Controls.TextBox)(this.FindName("txtDummy")));
            this.lblTitle = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblTitle")));
            this.lblNo = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblNo")));
            this.utlNo = ((SlvHanbaiClient.View.UserControl.Custom.Utl_InpNoText)(this.FindName("utlNo")));
            this.lblInpPerson = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblInpPerson")));
            this.utlPerson = ((SlvHanbaiClient.View.UserControl.Custom.Utl_MstText)(this.FindName("utlPerson")));
            this.lblOrderYmd = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblOrderYmd")));
            this.datSalesYmd = ((SlvHanbaiClient.Class.UI.ExDatePicker)(this.FindName("datSalesYmd")));
            this.utlMode = ((SlvHanbaiClient.View.UserControl.Custom.Utl_Mode)(this.FindName("utlMode")));
            this.lblEstimateNo = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblEstimateNo")));
            this.utlEstimateNo = ((SlvHanbaiClient.View.UserControl.Custom.Utl_InpNoText)(this.FindName("utlEstimateNo")));
            this.lblOrderNo = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblOrderNo")));
            this.utlOrderNo = ((SlvHanbaiClient.View.UserControl.Custom.Utl_InpNoText)(this.FindName("utlOrderNo")));
            this.lblNokiYmd = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblNokiYmd")));
            this.datNokiYmd = ((SlvHanbaiClient.Class.UI.ExDatePicker)(this.FindName("datNokiYmd")));
            this.lblCustomer = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblCustomer")));
            this.utlCustomer = ((SlvHanbaiClient.View.UserControl.Custom.Utl_MstText)(this.FindName("utlCustomer")));
            this.lblInvoice = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblInvoice")));
            this.utlInvoice = ((SlvHanbaiClient.View.UserControl.Custom.Utl_MstText)(this.FindName("utlInvoice")));
            this.lblSupply = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblSupply")));
            this.utlSupply = ((SlvHanbaiClient.View.UserControl.Custom.Utl_MstText)(this.FindName("utlSupply")));
            this.lblInvoiceNo = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblInvoiceNo")));
            this.txtInvoiceNo = ((SlvHanbaiClient.Class.UI.ExTextBox)(this.FindName("txtInvoiceNo")));
            this.lblInvoiceState = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblInvoiceState")));
            this.txtInvoiceState = ((SlvHanbaiClient.Class.UI.ExTextBox)(this.FindName("txtInvoiceState")));
            this.lblTax = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblTax")));
            this.utlTax = ((SlvHanbaiClient.View.UserControl.Custom.Utl_MeiText)(this.FindName("utlTax")));
            this.lblExchange = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblExchange")));
            this.utlBusiness = ((SlvHanbaiClient.View.UserControl.Custom.Utl_MeiText)(this.FindName("utlBusiness")));
            this.lblCreditLimitPrice = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblCreditLimitPrice")));
            this.txtCreditLimitPrice = ((SlvHanbaiClient.Class.UI.ExTextBox)(this.FindName("txtCreditLimitPrice")));
            this.lblReceiptState = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblReceiptState")));
            this.txtReceiptState = ((SlvHanbaiClient.Class.UI.ExTextBox)(this.FindName("txtReceiptState")));
            this.lblUnitKind = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblUnitKind")));
            this.utlUnitKind = ((SlvHanbaiClient.View.UserControl.Custom.Utl_MeiText)(this.FindName("utlUnitKind")));
            this.lblCreditRate = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblCreditRate")));
            this.txtCreditRate = ((SlvHanbaiClient.Class.UI.ExTextBox)(this.FindName("txtCreditRate")));
            this.lblSalesLimitPrice = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblSalesLimitPrice")));
            this.txtSalesLimitPrice = ((SlvHanbaiClient.Class.UI.ExTextBox)(this.FindName("txtSalesLimitPrice")));
            this.btnSalesBalance = ((System.Windows.Controls.Button)(this.FindName("btnSalesBalance")));
            this.lblRemark = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblRemark")));
            this.txtMemo = ((SlvHanbaiClient.Class.UI.ExTextBox)(this.FindName("txtMemo")));
            this.dg = ((SlvHanbaiClient.Class.UI.ExDataGrid)(this.FindName("dg")));
            this.btnInventory = ((System.Windows.Controls.Button)(this.FindName("btnInventory")));
            this.txtOrderStillNumber = ((SlvHanbaiClient.Class.UI.ExTextBox)(this.FindName("txtOrderStillNumber")));
            this.txtEnterNumber = ((SlvHanbaiClient.Class.UI.ExTextBox)(this.FindName("txtEnterNumber")));
            this.txtCaseNumber = ((SlvHanbaiClient.Class.UI.ExTextBox)(this.FindName("txtCaseNumber")));
            this.txtNumber = ((SlvHanbaiClient.Class.UI.ExTextBox)(this.FindName("txtNumber")));
            this.txtUnitPrice = ((SlvHanbaiClient.Class.UI.ExTextBox)(this.FindName("txtUnitPrice")));
            this.txtPrice = ((SlvHanbaiClient.Class.UI.ExTextBox)(this.FindName("txtPrice")));
            this.txtInventory = ((SlvHanbaiClient.Class.UI.ExTextBox)(this.FindName("txtInventory")));
            this.txtProfits = ((SlvHanbaiClient.Class.UI.ExTextBox)(this.FindName("txtProfits")));
            this.txtProfitsPercent = ((SlvHanbaiClient.Class.UI.ExTextBox)(this.FindName("txtProfitsPercent")));
            this.txtTax = ((SlvHanbaiClient.Class.UI.ExTextBox)(this.FindName("txtTax")));
            this.txtTaxNoPrice = ((SlvHanbaiClient.Class.UI.ExTextBox)(this.FindName("txtTaxNoPrice")));
            this.txtSumPrice = ((SlvHanbaiClient.Class.UI.ExTextBox)(this.FindName("txtSumPrice")));
        }
    }
}

