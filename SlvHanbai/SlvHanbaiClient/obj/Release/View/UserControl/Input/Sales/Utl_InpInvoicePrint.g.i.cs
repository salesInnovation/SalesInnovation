﻿#pragma checksum "C:\Users\Owner\SkyDrive\SlvHanbai\SlvHanbaiClient\View\UserControl\Input\Sales\Utl_InpInvoicePrint.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "FB4A175BA65DA8B960C0D280ACF2A3FB"
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


namespace SlvHanbaiClient.View.UserControl.Input.Sales {
    
    
    public partial class Utl_InpInvoicePrint : SlvHanbaiClient.Class.UI.ExUserControl {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.TextBox txtDummy;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblInvoiceCloseYmd;
        
        internal SlvHanbaiClient.Class.UI.ExDatePicker datInvoiceCloseYmd_F;
        
        internal System.Windows.Controls.TextBlock tbkInvoiceCloseYmd;
        
        internal SlvHanbaiClient.Class.UI.ExDatePicker datInvoiceCloseYmd_T;
        
        internal System.Windows.Controls.TextBlock tbkInvoiceCloseYmd2;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblSalesNo;
        
        internal SlvHanbaiClient.View.UserControl.Custom.Utl_InpNoText utlInvoiceNo_F;
        
        internal SlvHanbaiClient.View.UserControl.Custom.Utl_InpNoText utlInvoiceNo_T;
        
        internal SlvHanbaiClient.View.UserControl.Custom.Utl_MstText utlInvoice;
        
        internal SlvHanbaiClient.View.UserControl.Custom.Utl_MstText utlSummingUp;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblIssueYmd;
        
        internal SlvHanbaiClient.Class.UI.ExDatePicker datIssueYmd;
        
        internal SlvHanbaiClient.Class.UI.ExCheckBox chkSime;
        
        internal SlvHanbaiClient.Class.UI.ExCheckBox chkKake;
        
        internal System.Windows.Controls.Border borReceipt;
        
        internal SlvHanbaiClient.Class.UI.ExCheckBox chkReceiptNo;
        
        internal SlvHanbaiClient.Class.UI.ExCheckBox chkReceiptPlace;
        
        internal SlvHanbaiClient.Class.UI.ExCheckBox chkReceiptYes;
        
        internal SlvHanbaiClient.Class.UI.ExCheckBox chkReceiptOver;
        
        internal System.Windows.Controls.Border borPrint;
        
        internal SlvHanbaiClient.Class.UI.ExCheckBox chkPrintNo;
        
        internal SlvHanbaiClient.Class.UI.ExCheckBox chkPrintYes;
        
        internal System.Windows.Controls.Grid GridDetail;
        
        internal SlvHanbaiClient.Class.UI.ExDataGrid dgPrint;
        
        internal System.Windows.Controls.Grid GridDetailSelect;
        
        internal System.Windows.Controls.Button btnSelectAll;
        
        internal System.Windows.Controls.Button btnNoSelectAll;
        
        internal SlvHanbaiClient.Class.UI.ExDataGrid dgSelect;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/SlvHanbaiClient;component/View/UserControl/Input/Sales/Utl_InpInvoicePrint.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.txtDummy = ((System.Windows.Controls.TextBox)(this.FindName("txtDummy")));
            this.lblInvoiceCloseYmd = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblInvoiceCloseYmd")));
            this.datInvoiceCloseYmd_F = ((SlvHanbaiClient.Class.UI.ExDatePicker)(this.FindName("datInvoiceCloseYmd_F")));
            this.tbkInvoiceCloseYmd = ((System.Windows.Controls.TextBlock)(this.FindName("tbkInvoiceCloseYmd")));
            this.datInvoiceCloseYmd_T = ((SlvHanbaiClient.Class.UI.ExDatePicker)(this.FindName("datInvoiceCloseYmd_T")));
            this.tbkInvoiceCloseYmd2 = ((System.Windows.Controls.TextBlock)(this.FindName("tbkInvoiceCloseYmd2")));
            this.lblSalesNo = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblSalesNo")));
            this.utlInvoiceNo_F = ((SlvHanbaiClient.View.UserControl.Custom.Utl_InpNoText)(this.FindName("utlInvoiceNo_F")));
            this.utlInvoiceNo_T = ((SlvHanbaiClient.View.UserControl.Custom.Utl_InpNoText)(this.FindName("utlInvoiceNo_T")));
            this.utlInvoice = ((SlvHanbaiClient.View.UserControl.Custom.Utl_MstText)(this.FindName("utlInvoice")));
            this.utlSummingUp = ((SlvHanbaiClient.View.UserControl.Custom.Utl_MstText)(this.FindName("utlSummingUp")));
            this.lblIssueYmd = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblIssueYmd")));
            this.datIssueYmd = ((SlvHanbaiClient.Class.UI.ExDatePicker)(this.FindName("datIssueYmd")));
            this.chkSime = ((SlvHanbaiClient.Class.UI.ExCheckBox)(this.FindName("chkSime")));
            this.chkKake = ((SlvHanbaiClient.Class.UI.ExCheckBox)(this.FindName("chkKake")));
            this.borReceipt = ((System.Windows.Controls.Border)(this.FindName("borReceipt")));
            this.chkReceiptNo = ((SlvHanbaiClient.Class.UI.ExCheckBox)(this.FindName("chkReceiptNo")));
            this.chkReceiptPlace = ((SlvHanbaiClient.Class.UI.ExCheckBox)(this.FindName("chkReceiptPlace")));
            this.chkReceiptYes = ((SlvHanbaiClient.Class.UI.ExCheckBox)(this.FindName("chkReceiptYes")));
            this.chkReceiptOver = ((SlvHanbaiClient.Class.UI.ExCheckBox)(this.FindName("chkReceiptOver")));
            this.borPrint = ((System.Windows.Controls.Border)(this.FindName("borPrint")));
            this.chkPrintNo = ((SlvHanbaiClient.Class.UI.ExCheckBox)(this.FindName("chkPrintNo")));
            this.chkPrintYes = ((SlvHanbaiClient.Class.UI.ExCheckBox)(this.FindName("chkPrintYes")));
            this.GridDetail = ((System.Windows.Controls.Grid)(this.FindName("GridDetail")));
            this.dgPrint = ((SlvHanbaiClient.Class.UI.ExDataGrid)(this.FindName("dgPrint")));
            this.GridDetailSelect = ((System.Windows.Controls.Grid)(this.FindName("GridDetailSelect")));
            this.btnSelectAll = ((System.Windows.Controls.Button)(this.FindName("btnSelectAll")));
            this.btnNoSelectAll = ((System.Windows.Controls.Button)(this.FindName("btnNoSelectAll")));
            this.dgSelect = ((SlvHanbaiClient.Class.UI.ExDataGrid)(this.FindName("dgSelect")));
            this.dataG_HeaderSelect = ((System.Windows.Controls.DataGridCheckBoxColumn)(this.FindName("dataG_HeaderSelect")));
        }
    }
}

