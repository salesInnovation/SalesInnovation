﻿#pragma checksum "C:\Users\Owner\SkyDrive\SlvHanbai\SlvHanbaiClient\View\Dlg\MessageBox\Dlg_MessagBox.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "6B659DE6178DAD0563E18987966E04AA"
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


namespace SlvHanbaiClient.View.Dlg.MessageBox {
    
    
    public partial class Dlg_MessagBox : SlvHanbaiClient.Class.UI.ExChildWindow {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.ScrollViewer scrollViewer1;
        
        internal System.Windows.Controls.TextBlock txtMsg;
        
        internal SlvHanbaiClient.Class.UI.ExTextBox txtMsg3;
        
        internal System.Windows.Controls.Button btnYes;
        
        internal System.Windows.Controls.Button btnNo;
        
        internal System.Windows.Controls.Button btnCancel;
        
        internal System.Windows.Controls.Image imgIcon;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/SlvHanbaiClient;component/View/Dlg/MessageBox/Dlg_MessagBox.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.scrollViewer1 = ((System.Windows.Controls.ScrollViewer)(this.FindName("scrollViewer1")));
            this.txtMsg = ((System.Windows.Controls.TextBlock)(this.FindName("txtMsg")));
            this.txtMsg3 = ((SlvHanbaiClient.Class.UI.ExTextBox)(this.FindName("txtMsg3")));
            this.btnYes = ((System.Windows.Controls.Button)(this.FindName("btnYes")));
            this.btnNo = ((System.Windows.Controls.Button)(this.FindName("btnNo")));
            this.btnCancel = ((System.Windows.Controls.Button)(this.FindName("btnCancel")));
            this.imgIcon = ((System.Windows.Controls.Image)(this.FindName("imgIcon")));
        }
    }
}

