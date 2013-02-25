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
using SlvHanbaiClient.Class.WebService;
using SlvHanbaiClient.View.UserControl.Custom;
using SlvHanbaiClient.svcMstData;

#endregion

namespace SlvHanbaiClient.Class.UI
{
    public class ExUserControl : UserControl
    {

        #region Field

        protected bool _loaded = false;
        protected ExWebService webService = new ExWebService();
        protected IEnumerable<Control> CtlsTabList;
        public static ExTextBox txt = new ExTextBox();

        private bool _IsGetFKey = true;
        public bool IsGetFKey
        {
            set
            {
                this._IsGetFKey = value;
            }
            get { return this._IsGetFKey; }
        }

        #endregion

        #region Constructor

        public ExUserControl()

            : base()
        {
            // デバック用
            // test
            //this.Resources = new SlvHanbaiClient.Themes.MargedResourceDictionary();
            this.IsTabStop = true;
        }

        #endregion

        #region Evnets

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);

            if (this.IsGetFKey == false) return;

            Utl_FunctionKey utlFunctionKey = null;
            switch (e.Key)
            {
                case Key.F1:
                case Key.F2:
                case Key.F3:
                case Key.F4:
                case Key.F5:
                case Key.F6:
                case Key.F7:
                case Key.F8:
                case Key.F9:
                case Key.F11:
                case Key.F12:
                    utlFunctionKey = ExVisualTreeHelper.GetUtlFunctionKey(this);
                    break;
            }

            switch (e.Key)
            {
                case Key.F1: this.FunctionKey_Click(utlFunctionKey.btnF1, e); break;
                case Key.F2: this.FunctionKey_Click(utlFunctionKey.btnF2, e); break;
                case Key.F3: this.FunctionKey_Click(utlFunctionKey.btnF3, e); break;
                case Key.F4: this.FunctionKey_Click(utlFunctionKey.btnF4, e); break;
                case Key.F5: this.FunctionKey_Click(utlFunctionKey.btnF5, e); break;
                case Key.F6: this.FunctionKey_Click(utlFunctionKey.btnF6, e); break;
                case Key.F7: this.FunctionKey_Click(utlFunctionKey.btnF7, e); break;
                case Key.F8: this.FunctionKey_Click(utlFunctionKey.btnF8, e); break;
                case Key.F9: this.FunctionKey_Click(utlFunctionKey.btnF9, e); break;
                //case Key.F10: btnF10_Click(null, e); break;
                case Key.F11: this.FunctionKey_Click(utlFunctionKey.btnF11, e); break;
                case Key.F12: this.FunctionKey_Click(utlFunctionKey.btnF12, e); break;
                default: break;
            }
        }

        #endregion

        #region Method

        #region FunctionKey Method

        public void FunctionKey_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;

            if (btn.IsEnabled == true)
            {
                if (btn.Name == "btnF12")
                {
                    Common.gblnDesynchronizeLock = false;
                    Common.gblnBtnDesynchronizeLock = false;
                    Common.gblnBtnProcLock = false;
                }
                else
                {
                    if (Common.gblnBtnProcLock) return;
                    Common.gblnBtnProcLock = true;
                }
                
                try
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
                finally
                {
                    // ボタン押下時非同期入力チェックOFF時
                    if (Common.gblnBtnDesynchronizeLock == false)
                    {
                        // ボタン押下排他制御OFF
                        Common.gblnBtnProcLock = false;
                    }
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

        public delegate void DataSelectEventHandler(int intKbn, object sender);
        public event DataSelectEventHandler evtDataSelect;
        public virtual void DataSelect(int intKbn, object objList) 
        {
            this.evtDataSelect(intKbn, objList);
        }

        public event EventHandler evtDataUpdate;
        public virtual void DataUpdate(int intKbn, string errMessage) 
        {
            this.evtDataUpdate(errMessage, new EventArgs());
        }

        //public virtual void DataUpdate(int intKbn, object obj) { }
        public virtual void DataInsert(int intKbn) { }
        public virtual void DataInsert(int intKbn, object obj) { }
        public virtual void DataDelete(int intKbn) { }
        public virtual void DataDelete(int intKbn, object obj) { }
        public virtual void CopyCheck(object objList) { }
        public virtual void ReportOut(object objList) { }
        public virtual void ReportViewClose() { }

        public virtual void MstDataSelect(ExWebServiceMst.geWebServiceMstNmCallKbn intKbn, EntityMstData mst) { }

        public virtual void InputCheckUpdate() { }
        public virtual void RecordAdd() { }

        public virtual void ResultMessageBox(MessageBoxResult _MessageBoxResult, Control _errCtl) { }
        public virtual void ResultMessageBox(Control _errCtl) { }

        public virtual void ResultMessageBoxClr(MessageBoxResult _MessageBoxResult, Control _errCtl) { }

        public virtual void Init_SearchDisplay() { }

        #endregion

    }
}
