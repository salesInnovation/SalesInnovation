﻿#pragma checksum "C:\Users\chikugo\Documents\Visual Studio 2010\Projects\SlvHanbai\SlvHanbaiClient\View\UserControl\Input\Sales\Utl_InpSearchReceipt.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "39442DA9930408F4A9897166DC44F75A"
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
    
    
    public partial class Utl_InpSearchReceipt : SlvHanbaiClient.Class.UI.ExUserControl {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.TextBox txtDummy;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblReceiptYmd;
        
        internal SlvHanbaiClient.Class.UI.ExDatePicker datReceiptYmd_F;
        
        internal SlvHanbaiClient.Class.UI.ExDatePicker datReceiptYmd_T;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblCustomer;
        
        internal SlvHanbaiClient.View.UserControl.Custom.Utl_MstText utlInvoice;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblOrderNo;
        
        internal SlvHanbaiClient.View.UserControl.Custom.Utl_InpNoText utlReceiptNo_F;
        
        internal SlvHanbaiClient.View.UserControl.Custom.Utl_InpNoText utlReceiptNo_T;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblInpPerson;
        
        internal SlvHanbaiClient.View.UserControl.Custom.Utl_MstText utlPerson_F;
        
        internal SlvHanbaiClient.View.UserControl.Custom.Utl_MstText utlPerson_T;
        
        internal System.Windows.Controls.Grid GridDetail;
        
        internal SlvHanbaiClient.Class.UI.ExDataGrid dg;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/SlvHanbaiClient;component/View/UserControl/Input/Sales/Utl_InpSearchReceipt.xaml" +
                        "", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.txtDummy = ((System.Windows.Controls.TextBox)(this.FindName("txtDummy")));
            this.lblReceiptYmd = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblReceiptYmd")));
            this.datReceiptYmd_F = ((SlvHanbaiClient.Class.UI.ExDatePicker)(this.FindName("datReceiptYmd_F")));
            this.datReceiptYmd_T = ((SlvHanbaiClient.Class.UI.ExDatePicker)(this.FindName("datReceiptYmd_T")));
            this.lblCustomer = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblCustomer")));
            this.utlInvoice = ((SlvHanbaiClient.View.UserControl.Custom.Utl_MstText)(this.FindName("utlInvoice")));
            this.lblOrderNo = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblOrderNo")));
            this.utlReceiptNo_F = ((SlvHanbaiClient.View.UserControl.Custom.Utl_InpNoText)(this.FindName("utlReceiptNo_F")));
            this.utlReceiptNo_T = ((SlvHanbaiClient.View.UserControl.Custom.Utl_InpNoText)(this.FindName("utlReceiptNo_T")));
            this.lblInpPerson = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblInpPerson")));
            this.utlPerson_F = ((SlvHanbaiClient.View.UserControl.Custom.Utl_MstText)(this.FindName("utlPerson_F")));
            this.utlPerson_T = ((SlvHanbaiClient.View.UserControl.Custom.Utl_MstText)(this.FindName("utlPerson_T")));
            this.GridDetail = ((System.Windows.Controls.Grid)(this.FindName("GridDetail")));
            this.dg = ((SlvHanbaiClient.Class.UI.ExDataGrid)(this.FindName("dg")));
        }
    }
}

