﻿#pragma checksum "C:\Users\chikugo\Documents\Visual Studio 2010\Projects\SlvHanbai\SlvHanbaiClient\View\UserControl\Input\Purchase\Utl_InpPaymentClose.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "51C3AE74DFB52A46D02A1232E18E2695"
//------------------------------------------------------------------------------
// <auto-generated>
//     このコードはツールによって生成されました。
//     ランタイム バージョン:4.0.30319.235
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
    
    
    public partial class Utl_InpPaymentClose : SlvHanbaiClient.Class.UI.ExUserControl {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.TextBox txtDummy;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblTitle;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblCustomer;
        
        internal SlvHanbaiClient.View.UserControl.Custom.Utl_MstText utlPurchase;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblSummingUp;
        
        internal SlvHanbaiClient.View.UserControl.Custom.Utl_MstText utlSummingUp;
        
        internal SlvHanbaiClient.View.UserControl.Custom.Utl_Mode utlMode;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblInvoiceYm;
        
        internal SlvHanbaiClient.Class.UI.ExDatePicker datPaymentYmd;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblCollectPlanDay;
        
        internal SlvHanbaiClient.Class.UI.ExDatePicker datPaymentPlanDay;
        
        internal System.Windows.Controls.Button btnSelectAll;
        
        internal System.Windows.Controls.Button btnNoSelectAll;
        
        internal SlvHanbaiClient.Class.UI.ExDataGrid dg;
        
        internal System.Windows.Controls.DataGridCheckBoxColumn dataG_HeaderSelect;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/SlvHanbaiClient;component/View/UserControl/Input/Purchase/Utl_InpPaymentClose.xa" +
                        "ml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.txtDummy = ((System.Windows.Controls.TextBox)(this.FindName("txtDummy")));
            this.lblTitle = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblTitle")));
            this.lblCustomer = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblCustomer")));
            this.utlPurchase = ((SlvHanbaiClient.View.UserControl.Custom.Utl_MstText)(this.FindName("utlPurchase")));
            this.lblSummingUp = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblSummingUp")));
            this.utlSummingUp = ((SlvHanbaiClient.View.UserControl.Custom.Utl_MstText)(this.FindName("utlSummingUp")));
            this.utlMode = ((SlvHanbaiClient.View.UserControl.Custom.Utl_Mode)(this.FindName("utlMode")));
            this.lblInvoiceYm = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblInvoiceYm")));
            this.datPaymentYmd = ((SlvHanbaiClient.Class.UI.ExDatePicker)(this.FindName("datPaymentYmd")));
            this.lblCollectPlanDay = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblCollectPlanDay")));
            this.datPaymentPlanDay = ((SlvHanbaiClient.Class.UI.ExDatePicker)(this.FindName("datPaymentPlanDay")));
            this.btnSelectAll = ((System.Windows.Controls.Button)(this.FindName("btnSelectAll")));
            this.btnNoSelectAll = ((System.Windows.Controls.Button)(this.FindName("btnNoSelectAll")));
            this.dg = ((SlvHanbaiClient.Class.UI.ExDataGrid)(this.FindName("dg")));
            this.dataG_HeaderSelect = ((System.Windows.Controls.DataGridCheckBoxColumn)(this.FindName("dataG_HeaderSelect")));
        }
    }
}

