﻿#pragma checksum "C:\Users\chikugo\Documents\Visual Studio 2010\Projects\SlvHanbai\SlvHanbaiClient\View\Dlg\Dlg_SupportHistory.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "AB84ACE8B4CF0364FC2715FA9924411A"
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


namespace SlvHanbaiClient.View.Dlg {
    
    
    public partial class Dlg_SupportHistory : SlvHanbaiClient.Class.UI.ExChildWindow {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal SlvHanbaiClient.Class.UI.ExDataGrid dg;
        
        internal System.Windows.Controls.TextBlock tblbtnF1;
        
        internal System.Windows.Controls.Button btnF1;
        
        internal System.Windows.Controls.Button btnF2;
        
        internal System.Windows.Controls.Button btnF12;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/SlvHanbaiClient;component/View/Dlg/Dlg_SupportHistory.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.dg = ((SlvHanbaiClient.Class.UI.ExDataGrid)(this.FindName("dg")));
            this.tblbtnF1 = ((System.Windows.Controls.TextBlock)(this.FindName("tblbtnF1")));
            this.btnF1 = ((System.Windows.Controls.Button)(this.FindName("btnF1")));
            this.btnF2 = ((System.Windows.Controls.Button)(this.FindName("btnF2")));
            this.btnF12 = ((System.Windows.Controls.Button)(this.FindName("btnF12")));
        }
    }
}
