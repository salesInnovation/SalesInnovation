﻿#pragma checksum "C:\Users\Owner\SkyDrive\SlvHanbai\SlvHanbaiClient\View\UserControl\Input\Inventory\Utl_InpInOutDelivery.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "5C97D4E3DCADB2B0AA6AE84874516F79"
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


namespace SlvHanbaiClient.View.UserControl.Input.Inventory {
    
    
    public partial class Utl_InpInOutDelivery : SlvHanbaiClient.Class.UI.ExUserControl {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.TextBox txtDummy;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblTitle;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblNo;
        
        internal SlvHanbaiClient.View.UserControl.Custom.Utl_InpNoText utlNo;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblOrderYmd;
        
        internal SlvHanbaiClient.Class.UI.ExDatePicker datInOutDeliveryYmd;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblInpPerson;
        
        internal SlvHanbaiClient.View.UserControl.Custom.Utl_MstText utlPerson;
        
        internal SlvHanbaiClient.View.UserControl.Custom.Utl_Mode utlMode;
        
        internal SlvHanbaiClient.View.UserControl.Custom.Utl_MeiText utlInOutDeliveryKbn;
        
        internal SlvHanbaiClient.View.UserControl.Custom.Utl_MeiText utlInOutDeliveryToKbn;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblSupply;
        
        internal SlvHanbaiClient.View.UserControl.Custom.Utl_MstText utlCompanyGroup;
        
        internal SlvHanbaiClient.View.UserControl.Custom.Utl_MstText utlCustomer;
        
        internal SlvHanbaiClient.View.UserControl.Custom.Utl_MstText utlPurchase;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblRemark;
        
        internal SlvHanbaiClient.Class.UI.ExTextBox txtMemo;
        
        internal SlvHanbaiClient.Class.UI.ExDataGrid dg;
        
        internal SlvHanbaiClient.Class.UI.ExTextBox txtInventory;
        
        internal System.Windows.Controls.Button btnInventory;
        
        internal SlvHanbaiClient.Class.UI.ExTextBox txtEnterNumber;
        
        internal SlvHanbaiClient.Class.UI.ExTextBox txtCaseNumber;
        
        internal SlvHanbaiClient.Class.UI.ExTextBox txtNumber;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/SlvHanbaiClient;component/View/UserControl/Input/Inventory/Utl_InpInOutDelivery." +
                        "xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.txtDummy = ((System.Windows.Controls.TextBox)(this.FindName("txtDummy")));
            this.lblTitle = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblTitle")));
            this.lblNo = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblNo")));
            this.utlNo = ((SlvHanbaiClient.View.UserControl.Custom.Utl_InpNoText)(this.FindName("utlNo")));
            this.lblOrderYmd = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblOrderYmd")));
            this.datInOutDeliveryYmd = ((SlvHanbaiClient.Class.UI.ExDatePicker)(this.FindName("datInOutDeliveryYmd")));
            this.lblInpPerson = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblInpPerson")));
            this.utlPerson = ((SlvHanbaiClient.View.UserControl.Custom.Utl_MstText)(this.FindName("utlPerson")));
            this.utlMode = ((SlvHanbaiClient.View.UserControl.Custom.Utl_Mode)(this.FindName("utlMode")));
            this.utlInOutDeliveryKbn = ((SlvHanbaiClient.View.UserControl.Custom.Utl_MeiText)(this.FindName("utlInOutDeliveryKbn")));
            this.utlInOutDeliveryToKbn = ((SlvHanbaiClient.View.UserControl.Custom.Utl_MeiText)(this.FindName("utlInOutDeliveryToKbn")));
            this.lblSupply = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblSupply")));
            this.utlCompanyGroup = ((SlvHanbaiClient.View.UserControl.Custom.Utl_MstText)(this.FindName("utlCompanyGroup")));
            this.utlCustomer = ((SlvHanbaiClient.View.UserControl.Custom.Utl_MstText)(this.FindName("utlCustomer")));
            this.utlPurchase = ((SlvHanbaiClient.View.UserControl.Custom.Utl_MstText)(this.FindName("utlPurchase")));
            this.lblRemark = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblRemark")));
            this.txtMemo = ((SlvHanbaiClient.Class.UI.ExTextBox)(this.FindName("txtMemo")));
            this.dg = ((SlvHanbaiClient.Class.UI.ExDataGrid)(this.FindName("dg")));
            this.txtInventory = ((SlvHanbaiClient.Class.UI.ExTextBox)(this.FindName("txtInventory")));
            this.btnInventory = ((System.Windows.Controls.Button)(this.FindName("btnInventory")));
            this.txtEnterNumber = ((SlvHanbaiClient.Class.UI.ExTextBox)(this.FindName("txtEnterNumber")));
            this.txtCaseNumber = ((SlvHanbaiClient.Class.UI.ExTextBox)(this.FindName("txtCaseNumber")));
            this.txtNumber = ((SlvHanbaiClient.Class.UI.ExTextBox)(this.FindName("txtNumber")));
        }
    }
}

