﻿#pragma checksum "C:\Users\chikugo\Documents\Visual Studio 2010\Projects\SlvHanbai\SlvHanbaiClient\View\UserControl\DataForm\Purchase\Utl_DataFormSales.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "C49A1DC636C8E9FA8AEB8460C5F4F67F"
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


namespace SlvHanbaiClient.View.UserControl.DataForm.Sales {
    
    
    public partial class Utl_DataFormSales : SlvHanbaiClient.Class.UI.ExUserControl {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal SlvHanbaiClient.Class.UI.ExDataForm DataForm;
        
        internal SlvHanbaiClient.View.UserControl.Custom.Utl_FunctionKey utlFunctionKey;
        
        internal System.Windows.Controls.TextBlock txtBefore;
        
        internal System.Windows.Controls.TextBlock txtNext;
        
        internal System.Windows.Shapes.Rectangle recAdd;
        
        internal System.Windows.Shapes.Rectangle recDel;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/SlvHanbaiClient;component/View/UserControl/DataForm/Purchase/Utl_DataFormSales.x" +
                        "aml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.DataForm = ((SlvHanbaiClient.Class.UI.ExDataForm)(this.FindName("DataForm")));
            this.utlFunctionKey = ((SlvHanbaiClient.View.UserControl.Custom.Utl_FunctionKey)(this.FindName("utlFunctionKey")));
            this.txtBefore = ((System.Windows.Controls.TextBlock)(this.FindName("txtBefore")));
            this.txtNext = ((System.Windows.Controls.TextBlock)(this.FindName("txtNext")));
            this.recAdd = ((System.Windows.Shapes.Rectangle)(this.FindName("recAdd")));
            this.recDel = ((System.Windows.Shapes.Rectangle)(this.FindName("recDel")));
        }
    }
}

