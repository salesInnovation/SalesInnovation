﻿#pragma checksum "C:\Users\chikugo\Documents\Visual Studio 2010\Projects\SlvHanbai\SlvHanbaiClient\View\UserControl\Input\Sales\Utl_InpSearchPlan.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "0747AD4449EA10AFCBFC387B7E659BD2"
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


namespace SlvHanbaiClient.View.UserControl.Input.Sales {
    
    
    public partial class Utl_InpSearchPlan : SlvHanbaiClient.Class.UI.ExUserControl {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.TextBox txtDummy;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblCollectPlanYmd;
        
        internal SlvHanbaiClient.Class.UI.ExDatePicker datCollectPlanYmd_F;
        
        internal SlvHanbaiClient.Class.UI.ExDatePicker datCollectPlanYmd_T;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblInvoice;
        
        internal SlvHanbaiClient.View.UserControl.Custom.Utl_MstText utlInvoice_F;
        
        internal SlvHanbaiClient.View.UserControl.Custom.Utl_MstText utlPurchase_F;
        
        internal SlvHanbaiClient.View.UserControl.Custom.Utl_MstText utlInvoice_T;
        
        internal SlvHanbaiClient.View.UserControl.Custom.Utl_MstText utlPurchase_T;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/SlvHanbaiClient;component/View/UserControl/Input/Sales/Utl_InpSearchPlan.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.txtDummy = ((System.Windows.Controls.TextBox)(this.FindName("txtDummy")));
            this.lblCollectPlanYmd = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblCollectPlanYmd")));
            this.datCollectPlanYmd_F = ((SlvHanbaiClient.Class.UI.ExDatePicker)(this.FindName("datCollectPlanYmd_F")));
            this.datCollectPlanYmd_T = ((SlvHanbaiClient.Class.UI.ExDatePicker)(this.FindName("datCollectPlanYmd_T")));
            this.lblInvoice = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblInvoice")));
            this.utlInvoice_F = ((SlvHanbaiClient.View.UserControl.Custom.Utl_MstText)(this.FindName("utlInvoice_F")));
            this.utlPurchase_F = ((SlvHanbaiClient.View.UserControl.Custom.Utl_MstText)(this.FindName("utlPurchase_F")));
            this.utlInvoice_T = ((SlvHanbaiClient.View.UserControl.Custom.Utl_MstText)(this.FindName("utlInvoice_T")));
            this.utlPurchase_T = ((SlvHanbaiClient.View.UserControl.Custom.Utl_MstText)(this.FindName("utlPurchase_T")));
            this.GridDetail = ((System.Windows.Controls.Grid)(this.FindName("GridDetail")));
            this.dg = ((SlvHanbaiClient.Class.UI.ExDataGrid)(this.FindName("dg")));
        }
    }
}
