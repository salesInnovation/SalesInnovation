#region using

using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using SlvHanbaiClient.Class.Utility;
using SlvHanbaiClient.Class.WebService;

#endregion

namespace SlvHanbaiClient.Class.UI
{
    public class ExPage : Page
    {

        #region Field

        private bool IsEnterKeyDown = false;
        protected ExWebService webService = new ExWebService();
        protected IEnumerable<Control> CtlsTabList;
        protected List<ListDisplayTabIndex> listDisplayTabIndex = new List<ListDisplayTabIndex>();

        #endregion

        #region Constructor

        public ExPage()

            : base()
        {
            //this.Resources = new SlvHanbaiClient.Themes.MargedResourceDictionary();
        }

        #endregion

        #region Evnets

        protected override void OnKeyDown(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    // Enterキー フォーカス移動
                    //if (Application.Current.IsRunningOutOfBrowser == false) break;

                    if (Keyboard.Modifiers == ModifierKeys.Shift)
                    {
                        OnBeforeControl();
                    }
                    else
                    {
                        OnNextControl();
                    }
                    break;

                case Key.Tab:
                    //if (Application.Current.IsRunningOutOfBrowser == false) break;

                    if (Keyboard.Modifiers == ModifierKeys.Shift)
                    {
                        OnBeforeControl();
                    }
                    else
                    {
                        OnNextControl();
                    }
                    e.Handled = true;
                    return;
                default: break;
            }
            base.OnKeyDown(e);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            Control ctl = null;
            switch (e.Key)
            {
                case Key.F1: btnF1_Click(null, e); break;
                case Key.F2: btnF2_Click(null, e); break;
                case Key.F3: btnF3_Click(null, e); break;
                case Key.F4: btnF4_Click(null, e); break;
                case Key.F5: btnF5_Click(null, e); break;
                case Key.F6: btnF6_Click(null, e); break;
                case Key.F7: btnF7_Click(null, e); break;
                case Key.F8: btnF8_Click(null, e); break;
                case Key.F9: btnF9_Click(null, e); break;
                //case Key.F10: btnF10_Click(null, e); break;
                case Key.F11: btnF11_Click(null, e); break;
                case Key.F12: btnF12_Click(null, e); break;
                case Key.Down:
                    //if (Application.Current.IsRunningOutOfBrowser == false) break;
                    OnNextControl();
                    break;
                case Key.Up:
                    //if (Application.Current.IsRunningOutOfBrowser == false) break;
                    OnBeforeControl();
                    break;
                case Key.Enter:
                    //if (Application.Current.IsRunningOutOfBrowser == false) break;

                    // ２つ移動されないように
                    if (IsEnterKeyDown == true)
                    {
                        IsEnterKeyDown = false;
                        return;
                    }

                    // 現在フォーカスがあるコントロールを取得
                    ctl = (Control)FocusManager.GetFocusedElement();
                    if (ctl == null) return;

                    // DatePickerTextBoxの場合、KeyUpでしかEnterキーがとれない為
                    if (ctl is System.Windows.Controls.Primitives.DatePickerTextBox)
                    {
                        // Enterキー フォーカス移動
                        if (Keyboard.Modifiers == ModifierKeys.Shift)
                        {
                            OnBeforeControl();
                        }
                        else
                        {
                            OnNextControl();
                        }
                    }
                    break;
                case Key.Tab:
                    //if (Application.Current.IsRunningOutOfBrowser == false) break;

                    // ２つ移動されないように
                    if (IsEnterKeyDown == true)
                    {
                        IsEnterKeyDown = false;
                        return;
                    }

                    // 現在フォーカスがあるコントロールを取得
                    ctl = (Control)FocusManager.GetFocusedElement();
                    if (ctl == null) return;

                    // DatePickerTextBoxの場合、KeyUpでしかEnterキーがとれない為
                    if (ctl is System.Windows.Controls.Primitives.DatePickerTextBox)
                    {
                        // Enterキー フォーカス移動
                        if (Keyboard.Modifiers == ModifierKeys.Shift)
                        {
                            OnBeforeControl();
                        }
                        else
                        {
                            OnNextControl();
                        }
                    }
                    e.Handled = true;
                    return;
                default: break;
            }
            base.OnKeyUp(e);

        }

        #endregion

        #region Method

        // 前コントロールフォーカス移動
        public void OnBeforeControl()
        {
            try
            {
                if (this.listDisplayTabIndex == null) return;
                if (this.listDisplayTabIndex.Count == 0) return;

                // 現在フォーカスがあるコントロールを取得
                Control ctl = (Control)FocusManager.GetFocusedElement();
                if (ctl == null) return;

                // DatePickerTextBoxの場合、親を取得
                if (ctl is System.Windows.Controls.Primitives.DatePickerTextBox)
                {
                    try
                    {
                        DependencyObject obj = VisualTreeHelper.GetParent(ctl);
                        Control ctl2 = (Control)VisualTreeHelper.GetParent(obj);
                        if (ctl2 is System.Windows.Controls.DatePicker) ctl = ctl2;
                    }
                    catch
                    {
                    }
                }

                // UserControlタイプの場合
                if (ctl.Name == "txtID" || ctl.Name == "txtBindID")
                {
                    DependencyObject obj = (DependencyObject)ctl;
                    ExUserControl utl = (ExUserControl)ExVisualTreeHelper.FindPerentUserControl((DependencyObject)ctl);

                    if (utl == null) return;
                    if (utl.Tag == null) return;
                    if (utl.Tag.ToString().Length == 0) return;
                    if (utl.Tag.ToString().IndexOf("TabSearchOn") == -1) return;

                    ctl = utl;
                }

                // 前のコントロールを取得
                Control ctlNext = ExVisualTreeHelper.GetBeforeDisplayTabIndexCtl(this.listDisplayTabIndex, ctl.Name);
                if (ctlNext == null) return;
                if (ctlNext.IsTabStop == false) return;

                ExBackgroundWorker.DoWork_Focus(ctlNext, 10);

                //ctlNext.Focus();

                IsEnterKeyDown = true;
            }
            catch
            {
            }
        }

        // 次コントロールフォーカス移動
        public void OnNextControl()
        {
            try
            {
                if (this.listDisplayTabIndex == null) return;
                if (this.listDisplayTabIndex.Count == 0) return;

                // 現在フォーカスがあるコントロールを取得
                Control ctl = (Control)FocusManager.GetFocusedElement();
                if (ctl == null) return;

                // DatePickerTextBoxの場合、親を取得
                if (ctl is System.Windows.Controls.Primitives.DatePickerTextBox)
                {
                    try
                    {
                        DependencyObject obj = VisualTreeHelper.GetParent(ctl);
                        Control ctl2 = (Control)VisualTreeHelper.GetParent(obj);
                        if (ctl2 is System.Windows.Controls.DatePicker) ctl = ctl2;
                    }
                    catch
                    {
                    }
                }

                // UserControlタイプの場合
                if (ctl.Name == "txtID" || ctl.Name == "txtBindID")
                {
                    DependencyObject obj = (DependencyObject)ctl;
                    ExUserControl utl = (ExUserControl)ExVisualTreeHelper.FindPerentUserControl((DependencyObject)ctl);

                    if (utl == null) return;
                    if (utl.Tag == null) return;
                    if (utl.Tag.ToString().Length == 0) return;
                    if (utl.Tag.ToString().IndexOf("TabSearchOn") == -1) return;

                    ctl = utl;
                }

                string tag = ExVisualTreeHelper.GetTagDisplayTabIndex(this.listDisplayTabIndex, ctl.Name);
                if (tag.IndexOf("End") == -1)
                {
                    // 次のコントロールを取得
                    Control ctlNext = null;
                    ctlNext = ExVisualTreeHelper.GetNextDisplayTabIndexCtl(this.listDisplayTabIndex, ctl.Name);
                    if (ctlNext == null) return;
                    if (ctlNext.IsTabStop == false) return;
                    ExBackgroundWorker.DoWork_Focus(ctlNext, 10);
                }
                else
                { 
                    // データグリッドを取得
                    ExDataGrid dg = null;
                    dg = ExVisualTreeHelper.FindDataGrid(this);
                    if (dg == null) return;
                    ExBackgroundWorker.DoWork_DataGridSelectedFirst(dg, 10);
                }

                //ctlNext.Focus();

                IsEnterKeyDown = true;
            }
            catch
            {
            }
        }

        #region FunctionKey Method

        public void FunctionKey_Click(object sender, RoutedEventArgs e) 
        {
            Button btn = (Button)sender;

            if (btn.IsEnabled == true)
            {
                switch (btn.Name)
                {
                    case "btnF1": btnF1_Click(sender, e); break;
                    case "btnF2": btnF2_Click(sender, e); break;
                    case "btnF3": btnF3_Click(sender, e); break;
                    case "btnF4": btnF4_Click(sender, e); break;
                    case "btnF5": btnF5_Click(sender, e); break;
                    case "btnF6": btnF6_Click(sender, e); break;
                    case "btnF7": btnF7_Click(sender, e); break;
                    case "btnF8": btnF8_Click(sender, e); break;
                    case "btnF9": btnF9_Click(sender, e); break;
                    //case "btnF10": btnF10_Click(sender, e); break;
                    case "btnF11": btnF11_Click(sender, e); break;
                    case "btnF12": btnF12_Click(sender, e); break;
                    default: break;
                }
            }
        }

        public virtual void btnF1_Click(object sender, RoutedEventArgs e) { }
        public virtual void btnF2_Click(object sender, RoutedEventArgs e) { }
        public virtual void btnF3_Click(object sender, RoutedEventArgs e) { }
        public virtual void btnF4_Click(object sender, RoutedEventArgs e) { }
        public virtual void btnF5_Click(object sender, RoutedEventArgs e) { }
        public virtual void btnF6_Click(object sender, RoutedEventArgs e) { }
        public virtual void btnF7_Click(object sender, RoutedEventArgs e) { }
        public virtual void btnF8_Click(object sender, RoutedEventArgs e) { }
        public virtual void btnF9_Click(object sender, RoutedEventArgs e) { }
        //public virtual void btnF10_Click(object sender, RoutedEventArgs e) { }
        public virtual void btnF11_Click(object sender, RoutedEventArgs e) { }
        public virtual void btnF12_Click(object sender, RoutedEventArgs e) { }

        #endregion

        #region Resource

        public void SetRecource()
        {
            this.Resources.Clear();
            this.Resources = new SlvHanbaiClient.Themes.MargedResourceDictionary();
        }

        #endregion

        public virtual void DataSelect(int intKbn, object objList) { }
        public virtual void DataUpdate(int intKbn) { }
        public virtual void DataInsert(int intKbn) { }
        public virtual void DataDelete(int intKbn) { }

        public virtual void MstDataSelect(ExWebServiceMst.geWebServiceMstNmCallKbn intKbn, string name) { }

        public virtual void ResultMessageBox(MessageBoxResult _MessageBoxResult, Control _errCtl) { }
        public virtual void ResultMessageBox(Control _errCtl) { }

        public virtual void ResultMessageBoxClr(MessageBoxResult _MessageBoxResult, Control _errCtl) { }

        #endregion

    }
}
