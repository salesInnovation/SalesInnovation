﻿#pragma checksum "C:\Users\Owner\SkyDrive\SlvHanbai\SlvHanbaiClient\View\UserControl\Input\Purchase\Utl_InpPaymentCash.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "490EEEF0FD08705A81178D4F6A2D4FEB"
//------------------------------------------------------------------------------
// <auto-generated>
//     このコードはツールによって生成されました。
//     ランタイム バージョン:4.0.30319.18034
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
    
    
    public partial class Utl_InpPaymentCash : SlvHanbaiClient.Class.UI.ExUserControl {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.TextBox txtDummy;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblTitle;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblNo;
        
        internal SlvHanbaiClient.View.UserControl.Custom.Utl_InpNoText utlNo;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblInpPerson;
        
        internal SlvHanbaiClient.View.UserControl.Custom.Utl_MstText utlPerson;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblReceiptYmd;
        
        internal SlvHanbaiClient.Class.UI.ExDatePicker datPaymentCashYmd;
        
        internal SlvHanbaiClient.View.UserControl.Custom.Utl_Mode utlMode;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblInvoiceNo;
        
        internal SlvHanbaiClient.View.UserControl.Custom.Utl_InpNoText utlPaymentNo;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblInvoiceCloseYmd;
        
        internal SlvHanbaiClient.Class.UI.ExDatePicker datPaymentCloseYmd;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblCollectPlanYmd;
        
        internal SlvHanbaiClient.Class.UI.ExDatePicker datPaymentPlanYmd;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblInvoice;
        
        internal SlvHanbaiClient.View.UserControl.Custom.Utl_MstText utlPurchase;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblReceiptDivision;
        
        internal SlvHanbaiClient.View.UserControl.Custom.Utl_MstText utlPaymentDivision;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblSummingUp;
        
        internal SlvHanbaiClient.View.UserControl.Custom.Utl_MstText utlSummingUp;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblRemark;
        
        internal SlvHanbaiClient.Class.UI.ExTextBox txtMemo;
        
        internal SlvHanbaiClient.Class.UI.ExDataGrid dg;
        
        internal SlvHanbaiClient.Class.UI.ExTextBox txtPaymentPrice;
        
        internal SlvHanbaiClient.Class.UI.ExTextBox txtPaymentCashBeforePrice;
        
        internal SlvHanbaiClient.Class.UI.ExTextBox txtPrice;
        
        internal SlvHanbaiClient.Class.UI.ExTextBox txtPaymentZanPrice;
        
        internal SlvHanbaiClient.Class.UI.ExTextBox txtCreditPrice;
        
        internal System.Windows.Controls.Button btnSalesBalance;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/SlvHanbaiClient;component/View/UserControl/Input/Purchase/Utl_InpPaymentCash.xam" +
                        "l", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.txtDummy = ((System.Windows.Controls.TextBox)(this.FindName("txtDummy")));
            this.lblTitle = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblTitle")));
            this.lblNo = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblNo")));
            this.utlNo = ((SlvHanbaiClient.View.UserControl.Custom.Utl_InpNoText)(this.FindName("utlNo")));
            this.lblInpPerson = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblInpPerson")));
            this.utlPerson = ((SlvHanbaiClient.View.UserControl.Custom.Utl_MstText)(this.FindName("utlPerson")));
            this.lblReceiptYmd = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblReceiptYmd")));
            this.datPaymentCashYmd = ((SlvHanbaiClient.Class.UI.ExDatePicker)(this.FindName("datPaymentCashYmd")));
            this.utlMode = ((SlvHanbaiClient.View.UserControl.Custom.Utl_Mode)(this.FindName("utlMode")));
            this.lblInvoiceNo = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblInvoiceNo")));
            this.utlPaymentNo = ((SlvHanbaiClient.View.UserControl.Custom.Utl_InpNoText)(this.FindName("utlPaymentNo")));
            this.lblInvoiceCloseYmd = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblInvoiceCloseYmd")));
            this.datPaymentCloseYmd = ((SlvHanbaiClient.Class.UI.ExDatePicker)(this.FindName("datPaymentCloseYmd")));
            this.lblCollectPlanYmd = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblCollectPlanYmd")));
            this.datPaymentPlanYmd = ((SlvHanbaiClient.Class.UI.ExDatePicker)(this.FindName("datPaymentPlanYmd")));
            this.lblInvoice = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblInvoice")));
            this.utlPurchase = ((SlvHanbaiClient.View.UserControl.Custom.Utl_MstText)(this.FindName("utlPurchase")));
            this.lblReceiptDivision = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblReceiptDivision")));
            this.utlPaymentDivision = ((SlvHanbaiClient.View.UserControl.Custom.Utl_MstText)(this.FindName("utlPaymentDivision")));
            this.lblSummingUp = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblSummingUp")));
            this.utlSummingUp = ((SlvHanbaiClient.View.UserControl.Custom.Utl_MstText)(this.FindName("utlSummingUp")));
            this.lblRemark = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblRemark")));
            this.txtMemo = ((SlvHanbaiClient.Class.UI.ExTextBox)(this.FindName("txtMemo")));
            this.dg = ((SlvHanbaiClient.Class.UI.ExDataGrid)(this.FindName("dg")));
            this.txtPaymentPrice = ((SlvHanbaiClient.Class.UI.ExTextBox)(this.FindName("txtPaymentPrice")));
            this.txtPaymentCashBeforePrice = ((SlvHanbaiClient.Class.UI.ExTextBox)(this.FindName("txtPaymentCashBeforePrice")));
            this.txtPrice = ((SlvHanbaiClient.Class.UI.ExTextBox)(this.FindName("txtPrice")));
            this.txtPaymentZanPrice = ((SlvHanbaiClient.Class.UI.ExTextBox)(this.FindName("txtPaymentZanPrice")));
            this.txtCreditPrice = ((SlvHanbaiClient.Class.UI.ExTextBox)(this.FindName("txtCreditPrice")));
            this.btnSalesBalance = ((System.Windows.Controls.Button)(this.FindName("btnSalesBalance")));
        }
    }
}

