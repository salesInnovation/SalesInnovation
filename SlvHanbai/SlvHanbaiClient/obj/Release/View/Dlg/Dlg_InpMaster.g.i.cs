﻿#pragma checksum "C:\Users\Owner\SkyDrive\SlvHanbai\SlvHanbaiClient\View\Dlg\Dlg_InpMaster.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "C531A7F322ACB1C4DE9F3CC400AE6B74"
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
using SlvHanbaiClient.View.Dlg.Theme;
using SlvHanbaiClient.View.UserControl.Master;
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
    
    
    public partial class Dlg_InpMaster : SlvHanbaiClient.Class.UI.ExChildWindow {
        
        internal SlvHanbaiClient.Class.UI.ExGridLayoutRoot LayoutRoot;
        
        internal System.Windows.Controls.Grid GridMaster;
        
        internal SlvHanbaiClient.View.UserControl.Master.Utl_MstPerson UtlMaster;
        
        internal SlvHanbaiClient.View.Dlg.Theme.Dlg_ThemeShinyBlue dlgThemeShinyBlue;
        
        internal SlvHanbaiClient.View.Dlg.Theme.Dlg_ThemeTwilightBlue dlgThemeTwilightBlue;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/SlvHanbaiClient;component/View/Dlg/Dlg_InpMaster.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((SlvHanbaiClient.Class.UI.ExGridLayoutRoot)(this.FindName("LayoutRoot")));
            this.GridMaster = ((System.Windows.Controls.Grid)(this.FindName("GridMaster")));
            this.UtlMaster = ((SlvHanbaiClient.View.UserControl.Master.Utl_MstPerson)(this.FindName("UtlMaster")));
            this.dlgThemeShinyBlue = ((SlvHanbaiClient.View.Dlg.Theme.Dlg_ThemeShinyBlue)(this.FindName("dlgThemeShinyBlue")));
            this.dlgThemeTwilightBlue = ((SlvHanbaiClient.View.Dlg.Theme.Dlg_ThemeTwilightBlue)(this.FindName("dlgThemeTwilightBlue")));
        }
    }
}

