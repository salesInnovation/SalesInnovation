﻿#pragma checksum "C:\Users\chikugo\Documents\Visual Studio 2010\Projects\SlvHanbai\SlvHanbaiClient\View\UserControl\Master\Utl_MstUser.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "31C747C997F77D7061BDDBA5E9FDDBEF"
//------------------------------------------------------------------------------
// <auto-generated>
//     このコードはツールによって生成されました。
//     ランタイム バージョン:4.0.30319.237
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
    
    
    public partial class Utl_MstUser : SlvHanbaiClient.Class.UI.ExUserControl {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.TextBox txtDummy;
        
        internal SlvHanbaiClient.View.UserControl.Custom.Utl_FunctionKey utlFunctionKey;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblTitle;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblId;
        
        internal SlvHanbaiClient.Class.UI.ExComboBox cmbLoginId;
        
        internal SlvHanbaiClient.View.UserControl.Custom.Utl_Mode utlMode;
        
        internal System.Windows.Controls.StackPanel stpInput;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblLoginId;
        
        internal SlvHanbaiClient.Class.UI.ExTextBox txtLoginId;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblLoginPassword;
        
        internal System.Windows.Controls.PasswordBox txtLoginPassword;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblLoginPasswordConfirm;
        
        internal System.Windows.Controls.PasswordBox txtLoginPasswordConfirm;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblName;
        
        internal SlvHanbaiClient.Class.UI.ExTextBox txtName;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblCompanyGroup;
        
        internal SlvHanbaiClient.View.UserControl.Custom.Utl_MstText utlCompanyGroup;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblPerson;
        
        internal SlvHanbaiClient.View.UserControl.Custom.Utl_MstText utlPerson;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblDisplay;
        
        internal SlvHanbaiClient.View.UserControl.Custom.Utl_MeiText utlDisplay;
        
        internal SlvHanbaiClient.Class.UI.ExLabel lblRemark;
        
        internal SlvHanbaiClient.Class.UI.ExTextBox txtMemo;
        
        internal SlvHanbaiClient.Class.UI.ExUserControl utlDummy;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/SlvHanbaiClient;component/View/UserControl/Master/Utl_MstUser.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.txtDummy = ((System.Windows.Controls.TextBox)(this.FindName("txtDummy")));
            this.utlFunctionKey = ((SlvHanbaiClient.View.UserControl.Custom.Utl_FunctionKey)(this.FindName("utlFunctionKey")));
            this.lblTitle = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblTitle")));
            this.lblId = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblId")));
            this.cmbLoginId = ((SlvHanbaiClient.Class.UI.ExComboBox)(this.FindName("cmbLoginId")));
            this.utlMode = ((SlvHanbaiClient.View.UserControl.Custom.Utl_Mode)(this.FindName("utlMode")));
            this.stpInput = ((System.Windows.Controls.StackPanel)(this.FindName("stpInput")));
            this.lblLoginId = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblLoginId")));
            this.txtLoginId = ((SlvHanbaiClient.Class.UI.ExTextBox)(this.FindName("txtLoginId")));
            this.lblLoginPassword = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblLoginPassword")));
            this.txtLoginPassword = ((System.Windows.Controls.PasswordBox)(this.FindName("txtLoginPassword")));
            this.lblLoginPasswordConfirm = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblLoginPasswordConfirm")));
            this.txtLoginPasswordConfirm = ((System.Windows.Controls.PasswordBox)(this.FindName("txtLoginPasswordConfirm")));
            this.lblName = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblName")));
            this.txtName = ((SlvHanbaiClient.Class.UI.ExTextBox)(this.FindName("txtName")));
            this.lblCompanyGroup = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblCompanyGroup")));
            this.utlCompanyGroup = ((SlvHanbaiClient.View.UserControl.Custom.Utl_MstText)(this.FindName("utlCompanyGroup")));
            this.lblPerson = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblPerson")));
            this.utlPerson = ((SlvHanbaiClient.View.UserControl.Custom.Utl_MstText)(this.FindName("utlPerson")));
            this.lblDisplay = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblDisplay")));
            this.utlDisplay = ((SlvHanbaiClient.View.UserControl.Custom.Utl_MeiText)(this.FindName("utlDisplay")));
            this.lblRemark = ((SlvHanbaiClient.Class.UI.ExLabel)(this.FindName("lblRemark")));
            this.txtMemo = ((SlvHanbaiClient.Class.UI.ExTextBox)(this.FindName("txtMemo")));
            this.utlDummy = ((SlvHanbaiClient.Class.UI.ExUserControl)(this.FindName("utlDummy")));
        }
    }
}
