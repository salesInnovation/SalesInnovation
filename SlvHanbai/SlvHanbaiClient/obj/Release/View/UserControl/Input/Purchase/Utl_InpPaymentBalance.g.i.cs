﻿#pragma checksum "C:\Users\Owner\SkyDrive\SlvHanbai\SlvHanbaiClient\View\UserControl\Input\Purchase\Utl_InpPaymentBalance.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "D31D4590E45C793F7BBC0D6854479C0C"
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
    
    
    public partial class Utl_InpPaymentBalance : SlvHanbaiClient.Class.UI.ExUserControl {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.TextBox txtDummy;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblNokiYmd;
        
        internal SlvHanbaiClient.Class.UI.ExDatePicker datYm;
        
        internal SlvHanbaiClient.View.UserControl.Custom.Utl_MstText utlPurchase_F;
        
        internal SlvHanbaiClient.View.UserControl.Custom.Utl_MstText utlPurchase_T;
        
        internal System.Windows.Controls.Grid GridDetail;
        
        internal SlvHanbaiClient.Class.UI.ExDataGrid dgPrint;
        
        internal System.Windows.Controls.Grid GridDetailUpdate;
        
        internal System.Windows.Controls.Button btnSelectAll;
        
        internal System.Windows.Controls.Button btnNoSelectAll;
        
        internal SlvHanbaiClient.Class.UI.ExDataGrid dgUpdate;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/SlvHanbaiClient;component/View/UserControl/Input/Purchase/Utl_InpPaymentBalance." +
                        "xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.txtDummy = ((System.Windows.Controls.TextBox)(this.FindName("txtDummy")));
            this.lblNokiYmd = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblNokiYmd")));
            this.datYm = ((SlvHanbaiClient.Class.UI.ExDatePicker)(this.FindName("datYm")));
            this.utlPurchase_F = ((SlvHanbaiClient.View.UserControl.Custom.Utl_MstText)(this.FindName("utlPurchase_F")));
            this.utlPurchase_T = ((SlvHanbaiClient.View.UserControl.Custom.Utl_MstText)(this.FindName("utlPurchase_T")));
            this.GridDetail = ((System.Windows.Controls.Grid)(this.FindName("GridDetail")));
            this.dgPrint = ((SlvHanbaiClient.Class.UI.ExDataGrid)(this.FindName("dgPrint")));
            this.GridDetailUpdate = ((System.Windows.Controls.Grid)(this.FindName("GridDetailUpdate")));
            this.btnSelectAll = ((System.Windows.Controls.Button)(this.FindName("btnSelectAll")));
            this.btnNoSelectAll = ((System.Windows.Controls.Button)(this.FindName("btnNoSelectAll")));
            this.dgUpdate = ((SlvHanbaiClient.Class.UI.ExDataGrid)(this.FindName("dgUpdate")));
            this.dataG_HeaderSelect = ((System.Windows.Controls.DataGridCheckBoxColumn)(this.FindName("dataG_HeaderSelect")));
        }
    }
}

