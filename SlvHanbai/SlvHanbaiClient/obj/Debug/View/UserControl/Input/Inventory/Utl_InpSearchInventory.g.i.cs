﻿#pragma checksum "C:\Users\chikugo\Documents\Visual Studio 2010\Projects\SlvHanbai\SlvHanbaiClient\View\UserControl\Input\Inventory\Utl_InpSearchInventory.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "0E73242509F371DF8DCFD06731AF419A"
//------------------------------------------------------------------------------
// <auto-generated>
//     このコードはツールによって生成されました。
//     ランタイム バージョン:4.0.30319.239
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


namespace SlvHanbaiClient.View.UserControl.Input.Inventory {
    
    
    public partial class Utl_InpSearchInventory : SlvHanbaiClient.Class.UI.ExUserControl {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.TextBox txtDummy;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblNokiYmd;
        
        internal SlvHanbaiClient.Class.UI.ExDatePicker datYm;
        
        internal SlvHanbaiClient.View.UserControl.Custom.Utl_MstText utlCommodity_F;
        
        internal SlvHanbaiClient.View.UserControl.Custom.Utl_MstText utlCommodity_T;
        
        internal SlvHanbaiClient.Class.UI.ExCheckBox chkSalesCredit0_Yes;
        
        internal SlvHanbaiClient.Class.UI.ExCheckBox chkSalesCredit0_No;
        
        internal SlvHanbaiClient.Class.UI.ExCheckBox chkBussinesNo;
        
        internal SlvHanbaiClient.Class.UI.ExCheckBox chkBussinesYes;
        
        internal System.Windows.Controls.Grid GridDetail;
        
        internal SlvHanbaiClient.Class.UI.ExDataGrid dgPrint;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/SlvHanbaiClient;component/View/UserControl/Input/Inventory/Utl_InpSearchInventor" +
                        "y.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.txtDummy = ((System.Windows.Controls.TextBox)(this.FindName("txtDummy")));
            this.lblNokiYmd = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblNokiYmd")));
            this.datYm = ((SlvHanbaiClient.Class.UI.ExDatePicker)(this.FindName("datYm")));
            this.utlCommodity_F = ((SlvHanbaiClient.View.UserControl.Custom.Utl_MstText)(this.FindName("utlCommodity_F")));
            this.utlCommodity_T = ((SlvHanbaiClient.View.UserControl.Custom.Utl_MstText)(this.FindName("utlCommodity_T")));
            this.chkSalesCredit0_Yes = ((SlvHanbaiClient.Class.UI.ExCheckBox)(this.FindName("chkSalesCredit0_Yes")));
            this.chkSalesCredit0_No = ((SlvHanbaiClient.Class.UI.ExCheckBox)(this.FindName("chkSalesCredit0_No")));
            this.chkBussinesNo = ((SlvHanbaiClient.Class.UI.ExCheckBox)(this.FindName("chkBussinesNo")));
            this.chkBussinesYes = ((SlvHanbaiClient.Class.UI.ExCheckBox)(this.FindName("chkBussinesYes")));
            this.GridDetail = ((System.Windows.Controls.Grid)(this.FindName("GridDetail")));
            this.dgPrint = ((SlvHanbaiClient.Class.UI.ExDataGrid)(this.FindName("dgPrint")));
        }
    }
}

