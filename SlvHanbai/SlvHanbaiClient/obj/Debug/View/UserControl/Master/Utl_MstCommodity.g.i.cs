﻿#pragma checksum "C:\Users\chikugo\Documents\Visual Studio 2010\Projects\SlvHanbai\SlvHanbaiClient\View\UserControl\Master\Utl_MstCommodity.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "0B4FACE32A96E359774DF4A356F22736"
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


namespace SlvHanbaiClient.View.UserControl.Master {
    
    
    public partial class Utl_MstCommodity : SlvHanbaiClient.Class.UI.ExUserControl {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.TextBox txtDummy;
        
        internal SlvHanbaiClient.View.UserControl.Custom.Utl_FunctionKey utlFunctionKey;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblTitle;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblId;
        
        internal SlvHanbaiClient.View.UserControl.Custom.Utl_MstText utlID;
        
        internal SlvHanbaiClient.View.UserControl.Custom.Utl_Mode utlMode;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblName;
        
        internal SlvHanbaiClient.Class.UI.ExTextBox txtName;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblKana;
        
        internal SlvHanbaiClient.Class.UI.ExTextBox txtKana;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblUnit;
        
        internal SlvHanbaiClient.View.UserControl.Custom.Utl_MeiText utlUnit;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblEnterNumver;
        
        internal SlvHanbaiClient.Class.UI.ExTextBox txtEnterNumver;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblNumverDecimalDigit;
        
        internal SlvHanbaiClient.Class.UI.ExTextBox txtNumverDecimalDigit;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblUnitDecimalDigit;
        
        internal SlvHanbaiClient.Class.UI.ExTextBox txtUnitDecimalDigit;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblTaxationDivisionID;
        
        internal SlvHanbaiClient.View.UserControl.Custom.Utl_MeiText utlTaxationDivisionID;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblInventoryDivisionId;
        
        internal SlvHanbaiClient.View.UserControl.Custom.Utl_MeiText utlInventoryDivisionId;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblPurchaseLot;
        
        internal SlvHanbaiClient.Class.UI.ExTextBox txtPurchaseLot;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblJustInventoryNumber;
        
        internal SlvHanbaiClient.Class.UI.ExTextBox txtJustInventoryNumber;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblInventoryNumber;
        
        internal SlvHanbaiClient.Class.UI.ExTextBox txtInventoryNumber;
        
        internal System.Windows.Controls.Button btnInventory;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblMainPurchaseId;
        
        internal SlvHanbaiClient.View.UserControl.Custom.Utl_MstText utlMainPurchaseId;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblLeadTime;
        
        internal SlvHanbaiClient.Class.UI.ExTextBox txtLeadTime;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblUnitPrice;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblSkipTax;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblBeforeTax;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblRetailPrice;
        
        internal SlvHanbaiClient.Class.UI.ExTextBox txtRetailPriceSkipTax;
        
        internal SlvHanbaiClient.Class.UI.ExTextBox txtRetailPriceBeforeTax;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblSalesUnit;
        
        internal SlvHanbaiClient.Class.UI.ExTextBox txtSalesUnitSkipTax;
        
        internal SlvHanbaiClient.Class.UI.ExTextBox txtSalesUnitBeforeTax;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblSalesCost;
        
        internal SlvHanbaiClient.Class.UI.ExTextBox txtSalesCostSkipTax;
        
        internal SlvHanbaiClient.Class.UI.ExTextBox txtSalesCostBeforeTax;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblPurchaseUnit;
        
        internal SlvHanbaiClient.Class.UI.ExTextBox txtPurchaseUnitSkipTax;
        
        internal SlvHanbaiClient.Class.UI.ExTextBox txtPurchaseUnitBeforeTax;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblGroup1;
        
        internal SlvHanbaiClient.View.UserControl.Custom.Utl_MstText utlGroup1;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblDisplay;
        
        internal SlvHanbaiClient.View.UserControl.Custom.Utl_MeiText utlDisplay;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblRemark;
        
        internal SlvHanbaiClient.Class.UI.ExTextBox txtMemo;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/SlvHanbaiClient;component/View/UserControl/Master/Utl_MstCommodity.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.txtDummy = ((System.Windows.Controls.TextBox)(this.FindName("txtDummy")));
            this.utlFunctionKey = ((SlvHanbaiClient.View.UserControl.Custom.Utl_FunctionKey)(this.FindName("utlFunctionKey")));
            this.lblTitle = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblTitle")));
            this.lblId = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblId")));
            this.utlID = ((SlvHanbaiClient.View.UserControl.Custom.Utl_MstText)(this.FindName("utlID")));
            this.utlMode = ((SlvHanbaiClient.View.UserControl.Custom.Utl_Mode)(this.FindName("utlMode")));
            this.lblName = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblName")));
            this.txtName = ((SlvHanbaiClient.Class.UI.ExTextBox)(this.FindName("txtName")));
            this.lblKana = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblKana")));
            this.txtKana = ((SlvHanbaiClient.Class.UI.ExTextBox)(this.FindName("txtKana")));
            this.lblUnit = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblUnit")));
            this.utlUnit = ((SlvHanbaiClient.View.UserControl.Custom.Utl_MeiText)(this.FindName("utlUnit")));
            this.lblEnterNumver = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblEnterNumver")));
            this.txtEnterNumver = ((SlvHanbaiClient.Class.UI.ExTextBox)(this.FindName("txtEnterNumver")));
            this.lblNumverDecimalDigit = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblNumverDecimalDigit")));
            this.txtNumverDecimalDigit = ((SlvHanbaiClient.Class.UI.ExTextBox)(this.FindName("txtNumverDecimalDigit")));
            this.lblUnitDecimalDigit = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblUnitDecimalDigit")));
            this.txtUnitDecimalDigit = ((SlvHanbaiClient.Class.UI.ExTextBox)(this.FindName("txtUnitDecimalDigit")));
            this.lblTaxationDivisionID = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblTaxationDivisionID")));
            this.utlTaxationDivisionID = ((SlvHanbaiClient.View.UserControl.Custom.Utl_MeiText)(this.FindName("utlTaxationDivisionID")));
            this.lblInventoryDivisionId = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblInventoryDivisionId")));
            this.utlInventoryDivisionId = ((SlvHanbaiClient.View.UserControl.Custom.Utl_MeiText)(this.FindName("utlInventoryDivisionId")));
            this.lblPurchaseLot = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblPurchaseLot")));
            this.txtPurchaseLot = ((SlvHanbaiClient.Class.UI.ExTextBox)(this.FindName("txtPurchaseLot")));
            this.lblJustInventoryNumber = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblJustInventoryNumber")));
            this.txtJustInventoryNumber = ((SlvHanbaiClient.Class.UI.ExTextBox)(this.FindName("txtJustInventoryNumber")));
            this.lblInventoryNumber = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblInventoryNumber")));
            this.txtInventoryNumber = ((SlvHanbaiClient.Class.UI.ExTextBox)(this.FindName("txtInventoryNumber")));
            this.btnInventory = ((System.Windows.Controls.Button)(this.FindName("btnInventory")));
            this.lblMainPurchaseId = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblMainPurchaseId")));
            this.utlMainPurchaseId = ((SlvHanbaiClient.View.UserControl.Custom.Utl_MstText)(this.FindName("utlMainPurchaseId")));
            this.lblLeadTime = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblLeadTime")));
            this.txtLeadTime = ((SlvHanbaiClient.Class.UI.ExTextBox)(this.FindName("txtLeadTime")));
            this.lblUnitPrice = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblUnitPrice")));
            this.lblSkipTax = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblSkipTax")));
            this.lblBeforeTax = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblBeforeTax")));
            this.lblRetailPrice = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblRetailPrice")));
            this.txtRetailPriceSkipTax = ((SlvHanbaiClient.Class.UI.ExTextBox)(this.FindName("txtRetailPriceSkipTax")));
            this.txtRetailPriceBeforeTax = ((SlvHanbaiClient.Class.UI.ExTextBox)(this.FindName("txtRetailPriceBeforeTax")));
            this.lblSalesUnit = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblSalesUnit")));
            this.txtSalesUnitSkipTax = ((SlvHanbaiClient.Class.UI.ExTextBox)(this.FindName("txtSalesUnitSkipTax")));
            this.txtSalesUnitBeforeTax = ((SlvHanbaiClient.Class.UI.ExTextBox)(this.FindName("txtSalesUnitBeforeTax")));
            this.lblSalesCost = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblSalesCost")));
            this.txtSalesCostSkipTax = ((SlvHanbaiClient.Class.UI.ExTextBox)(this.FindName("txtSalesCostSkipTax")));
            this.txtSalesCostBeforeTax = ((SlvHanbaiClient.Class.UI.ExTextBox)(this.FindName("txtSalesCostBeforeTax")));
            this.lblPurchaseUnit = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblPurchaseUnit")));
            this.txtPurchaseUnitSkipTax = ((SlvHanbaiClient.Class.UI.ExTextBox)(this.FindName("txtPurchaseUnitSkipTax")));
            this.txtPurchaseUnitBeforeTax = ((SlvHanbaiClient.Class.UI.ExTextBox)(this.FindName("txtPurchaseUnitBeforeTax")));
            this.lblGroup1 = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblGroup1")));
            this.utlGroup1 = ((SlvHanbaiClient.View.UserControl.Custom.Utl_MstText)(this.FindName("utlGroup1")));
            this.lblDisplay = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblDisplay")));
            this.utlDisplay = ((SlvHanbaiClient.View.UserControl.Custom.Utl_MeiText)(this.FindName("utlDisplay")));
            this.lblRemark = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblRemark")));
            this.txtMemo = ((SlvHanbaiClient.Class.UI.ExTextBox)(this.FindName("txtMemo")));
        }
    }
}

