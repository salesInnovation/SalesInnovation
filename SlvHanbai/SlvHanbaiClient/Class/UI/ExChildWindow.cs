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
using SlvHanbaiClient.Themes;
using SlvHanbaiClient.View.Dlg.Theme;

#endregion

namespace SlvHanbaiClient.Class.UI
{
    public class ExChildWindow : ChildWindow
    {

        #region Field

        private bool IsEnterKeyDown = false;
        protected ExWebService webService = new ExWebService();
        protected IEnumerable<Control> CtlsTabList;
        protected List<ListDisplayTabIndex> listDisplayTabIndex = new List<ListDisplayTabIndex>();
        private bool _IsTabDefualMove = true;
        public bool IsTabDefualMove { set { this._IsTabDefualMove = value; } get { return this._IsTabDefualMove; } }

        #endregion

        #region Constructor

        public ExChildWindow()
            : base()
        {
            //SetRecource();
        }

        #endregion

        #region Events

        protected override void OnKeyDown(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    //if (Application.Current.IsRunningOutOfBrowser == false) break;

                    // Enterキー フォーカス移動
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
                    if (IsTabDefualMove == true) break;

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
                    if (IsTabDefualMove == true) break;

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

        #region Resource

        public void SetRecource()
        {
            this.Resources.Clear();
            this.Resources = new SlvHanbaiClient.Themes.MargedResourceDictionary();
        }

        public void SetWindowsResource()
        {
            System.Windows.Style windowStyle = null;

            switch (MargedResourceDictionary.gThemeType)
            {
                case MargedResourceDictionary.geThemeType.ShinyBlue:
                    //Dlg_ThemeShinyBlue dlgThemeShinyBlue = (Dlg_ThemeShinyBlue)ExVisualTreeHelper.FindChildWindowForTheme(this, "dlgThemeShinyBlue");
                    //if (dlgThemeShinyBlue != null) 
                    //{
                    //    windowStyle = dlgThemeShinyBlue.Resources["ThemeStyle"] as Style;
                    //}
                    Dlg_ThemeShinyBlue dlgThemeShinyBlue = (Dlg_ThemeShinyBlue)this.FindName("dlgThemeShinyBlue");
                    if (dlgThemeShinyBlue != null) 
                    {
                        windowStyle = dlgThemeShinyBlue.Resources["ThemeStyle"] as Style;
                    }
                    break;
                case MargedResourceDictionary.geThemeType.ShinyRed:
                    break;
                case MargedResourceDictionary.geThemeType.TwilightBlue:
                    Dlg_ThemeTwilightBlue dlgThemeTwilightBlue = (Dlg_ThemeTwilightBlue)this.FindName("dlgThemeTwilightBlue");
                    if (dlgThemeTwilightBlue != null) 
                    {
                        windowStyle = dlgThemeTwilightBlue.Resources["ThemeStyle"] as Style;
                    }
                    break;
                case MargedResourceDictionary.geThemeType.BubbleCreme:
                    break;
                case MargedResourceDictionary.geThemeType.BureauBlack:
                    break;
                case MargedResourceDictionary.geThemeType.BureauBlue:
                    break;
                case MargedResourceDictionary.geThemeType.ExpressionDark:
                    break;
                case MargedResourceDictionary.geThemeType.ExpressionLight:
                    break;
                case MargedResourceDictionary.geThemeType.RainerOrange:
                    break;
                default:
                    break;
            }

            if (windowStyle != null)
            {
                this.Style = windowStyle;
            }
        }

        #endregion

        #region Method

        //public void ShowModalDialog() 
        //{
        //    System.Threading.AutoResetEvent waitHandle = new System.Threading.AutoResetEvent(false); 
        //    Dispatcher.BeginInvoke(() => 
        //    { 
        //        ExChildWindow cw = this; 
        //        cw.Content = "Modal Dialog"; 
        //        cw.Closed += (s, e) => waitHandle.Set(); cw.Show(); 
        //    }
        //    ); 
        //    waitHandle.WaitOne(); 
        //}

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

                // 次のコントロールを取得
                Control ctlNext = ExVisualTreeHelper.GetNextDisplayTabIndexCtl(this.listDisplayTabIndex, ctl.Name);
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

        public virtual void DataSelect(int intKbn, object objList) { }
        public virtual void DataUpdate(int intKbn, string errMessage) { }
        public virtual void DataInsert(int intKbn) { }
        public virtual void DataInsert(int intKbn, object obj) { }
        public virtual void DataDelete(int intKbn) { }
        public virtual void DataDelete(int intKbn, object obj) { }

        public virtual void OnFileUploadCompleted(string dir) { }

        public virtual void InputCheckUpdate() { }

        public virtual void MstDataSelect(ExWebServiceMst.geWebServiceMstNmCallKbn intKbn, string name) { }

        public virtual void ResultMessageBox(MessageBoxResult _MessageBoxResult, Control _errCtl) { }
        public virtual void ResultMessageBox(Control _errCtl) { }

        public virtual void ResultMessageBoxClr(MessageBoxResult _MessageBoxResult, Control _errCtl) { }

        #endregion

    }
}
