using System;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SlvHanbaiClient.Class;
using SlvHanbaiClient.Class.Data;
using SlvHanbaiClient.Class.UI;
using SlvHanbaiClient.Class.Data;
using SlvHanbaiClient.Class.WebService;
using SlvHanbaiClient.Class.Entity;
using SlvHanbaiClient.Class.Utility;
using SlvHanbaiClient.svcReceipt;
using SlvHanbaiClient.View.UserControl.Custom;
using SlvHanbaiClient.View.Dlg;

namespace SlvHanbaiClient.View.UserControl.DataForm.Sales
{
    public partial class Utl_DataFormReceipt : ExUserControl
    {

        #region Filed Const

        private const String CLASS_NM = "Utl_DataFormOrder";

        public EntityReceiptH _entityH = new EntityReceiptH();
        public ObservableCollection<EntityReceiptD> _entityListD = new ObservableCollection<EntityReceiptD>();
        public ObservableCollection<EntityDataFormReceiptD> objDataFormReceiptD = new ObservableCollection<EntityDataFormReceiptD>();
        public object objBeforeReceiptListD;
        private Control activeControl;

        private string beforeValue = "";
        private int beforeSelectedIndex = -1;           // 編集前 行
        private string beforeValueDlg = "";

        private bool IsKeyDown = false;

        #region Control

        private Utl_FunctionKey utlFncKey;
        private ExTextBox _txtNo = null;
        private ExTextBox _txtReceiptDivisionId = null;
        private ExTextBox _txtReceiptDivisionNm = null;
        private ExDatePicker _txtBillSiteDay = null;
        private ExTextBox _txtPrice = null;
        private ExTextBox _txtDetailMemo = null;
        private bool IsFocusOn = false;

        #endregion

        #endregion

        #region Constructor

        public Utl_DataFormReceipt()
        {
            InitializeComponent();
            this.Tag = "Main";      // ファンクションキーを受けつけ用
        }

        #endregion

        #region Page Events

        private void ExUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ExVisualTreeHelper.initDisplay(this.LayoutRoot);
            Init();
        }

        #endregion

        #region DataForm Events

        private void DataForm_ContentLoaded(object sender, DataFormContentLoadEventArgs e)
        {
            if (this.DataForm.CurrentIndex >= 1)
            {
                this.txtBefore.Foreground = new SolidColorBrush(Colors.Black);
            }
            else
            {
                this.txtBefore.Foreground = new SolidColorBrush(Colors.DarkGray);
            }
            if (this.DataForm.CurrentIndex < _entityListD.Count - 1)
            {
                this.txtNext.Foreground = new SolidColorBrush(Colors.Black);
            }
            else
            {
                this.txtNext.Foreground = new SolidColorBrush(Colors.DarkGray);
            }

            switch (GetUserControlFKey().gFunctionKeyEnable)
            {
                case Utl_FunctionKey.geFunctionKeyEnable.New:
                    // 行番号設定
                    _entityListD[DataForm.CurrentIndex]._rec_no = DataForm.CurrentIndex + 1;
                    _entityListD[DataForm.CurrentIndex]._receipt_division_id = this._entityH._receipt_division_id;
                    _entityListD[DataForm.CurrentIndex]._receipt_division_nm = this._entityH._receipt_division_nm;
                    break;
            }

            InitControl();
        }

        private void DataForm_AddingNewItem(object sender, DataFormAddingNewItemEventArgs e)
        {
            GetUserControlFKey().gFunctionKeyEnable = Utl_FunctionKey.geFunctionKeyEnable.New;
        }

        private void DataForm_DeletingItem(object sender, CancelEventArgs e)
        {
            GetUserControlFKey().gFunctionKeyEnable = Utl_FunctionKey.geFunctionKeyEnable.Upd;
        }

        #endregion

        #region Function Key Events

        // F1ボタン(OK) クリック
        public override void btnF1_Click(object sender, RoutedEventArgs e)
        {
            if (GetUserControlFKey().btnF1.IsEnabled == false) return;

            // 変更を反映
            this.DataForm.CommitEdit();

            Dlg_DataForm win = (Dlg_DataForm)ExVisualTreeHelper.FindPerentChildWindow(this);
            win.Close();
        }

        // F2ボタン(追加) クリック
        public override void btnF2_Click(object sender, RoutedEventArgs e)
        {
            if (GetUserControlFKey().btnF2.IsEnabled == false) return;
            EntityReceiptD __entity = new EntityReceiptD();
            __entity._receipt_division_id = this._entityH._receipt_division_id;
            __entity._receipt_division_nm = this._entityH._receipt_division_nm;
            _entityListD.Add(__entity);
            this.DataForm.CurrentIndex = _entityListD.Count - 1;
            _entityListD[_entityListD.Count - 1]._rec_no = DataForm.CurrentIndex + 1;

            GetUserControlFKey().gFunctionKeyEnable = Utl_FunctionKey.geFunctionKeyEnable.New;

            this.recAdd.Visibility = System.Windows.Visibility.Visible;
        }

        // F3ボタン(削除) クリック
        public override void btnF3_Click(object sender, RoutedEventArgs e)
        {
            if (GetUserControlFKey().btnF3.IsEnabled == false) return;

            // 一旦保存
            this.DataForm.CommitEdit();

            // 削除
            this.DataForm.DeleteItem();
        }

        // F4ボタン(クリア) クリック
        public override void btnF4_Click(object sender, RoutedEventArgs e)
        {
            if (GetUserControlFKey().btnF4.IsEnabled == false) return;

            switch (GetUserControlFKey().gFunctionKeyEnable)
            {
                case Utl_FunctionKey.geFunctionKeyEnable.New:
                    // 一旦保存
                    this.DataForm.CommitEdit();

                    // 削除
                    this.DataForm.DeleteItem();

                    break;
            }

            this.recAdd.Visibility = System.Windows.Visibility.Collapsed;
        }

        // F5ボタン(参照) クリック
        public override void btnF5_Click(object sender, RoutedEventArgs e)
        {
            if (GetUserControlFKey().btnF5.IsEnabled == false) return;

            if (activeControl == null)
            {
                return;
            }

            switch (activeControl.Name)
            {
                case "txtReceiptDivisionId":
                    txtReceiptDivisionId_MouseDoubleClick(null, null);
                    break;
                case "txtBillSiteDay":
                    txtBillSiteDay_MouseDoubleClick(null, null);
                    break;
                default:
                    break;
            }

        }

        // F6ボタン(保存) クリック
        public override void btnF6_Click(object sender, RoutedEventArgs e)
        {
            if (GetUserControlFKey().btnF6.IsEnabled == false) return;

            this.DataForm.CommitEdit();
            GetUserControlFKey().gFunctionKeyEnable = Utl_FunctionKey.geFunctionKeyEnable.Upd;

            this.recAdd.Visibility = System.Windows.Visibility.Collapsed;

        }

        // F7ボタン(前へ) クリック
        public override void btnF7_Click(object sender, RoutedEventArgs e)
        {
            if (this.DataForm.CurrentIndex >= 1)
                this.DataForm.CurrentIndex -= 1;
        }

        // F8ボタン(次へ) クリック
        public override void btnF8_Click(object sender, RoutedEventArgs e)
        {
            if (this.DataForm.CurrentIndex < _entityListD.Count - 1)
                this.DataForm.CurrentIndex += 1;
        }

        // F12ボタン(キャンセル) クリック
        public override void btnF12_Click(object sender, RoutedEventArgs e)
        {
            if (GetUserControlFKey().btnF12.IsEnabled == false) return;

            // 保持情報を戻す
            ConvertDataFormToDetail();

            Dlg_DataForm win = (Dlg_DataForm)ExVisualTreeHelper.FindPerentChildWindow(this);
            win.Close();
        }

        #endregion

        #region Convert Detail DataForm

        private void ConvertDetailToDataForm()
        {
            if (_entityListD == null)
            {
                // 行番号
                EntityReceiptD entity = new EntityReceiptD();
                entity._rec_no = 1;

                _entityListD = new ObservableCollection<EntityReceiptD>();
                _entityListD.Add(entity);

                return;
            }

            for (int i = 0; i <= _entityListD.Count - 1; i++)
            {
                EntityDataFormReceiptD _entityD = new EntityDataFormReceiptD();
                _entityD.id = _entityListD[i]._id;
                _entityD.rec_no = _entityListD[i]._rec_no;
                _entityD.receipt_division_id = _entityListD[i]._receipt_division_id;
                _entityD.receipt_division_nm = _entityListD[i]._receipt_division_nm;
                _entityD.bill_site_day = _entityListD[i]._bill_site_day;
                _entityD.price = _entityListD[i]._price;
                _entityD.memo = _entityListD[i]._memo;
                objDataFormReceiptD.Add(_entityD);
            }
        }

        private void ConvertDataFormToDetail()
        {
            this.DataForm.CommitEdit();
            if (_entityListD != null) _entityListD.Clear();

            for (int i = 0; i <= objDataFormReceiptD.Count - 1; i++)
            {
                EntityReceiptD entity = new EntityReceiptD();
                entity._id = objDataFormReceiptD[i].id;
                entity._rec_no = objDataFormReceiptD[i].rec_no;
                entity._receipt_division_id = objDataFormReceiptD[i].receipt_division_id;
                entity._receipt_division_nm = objDataFormReceiptD[i].receipt_division_nm;
                entity._bill_site_day = objDataFormReceiptD[i].bill_site_day;
                entity._price = objDataFormReceiptD[i].price;
                entity._memo = objDataFormReceiptD[i].memo;
                _entityListD.Add(entity);
            }
        }

        #endregion

        #region GotFocus

        private void txt_GotFocus(object sender, RoutedEventArgs e)
        {
            GetUserControlFKey().SetFunctionKeyEnable("F5", false);
            activeControl = (Control)sender;
            beforeValue = "";
            SetControl();

            if ((_entityListD.Count > DataForm.CurrentIndex && DataForm.CurrentIndex != -1) == false) return;

            switch (activeControl.Name)
            {
                case "txtReceiptDivisionId":
                case "txtBillSiteDay":
                    GetUserControlFKey().SetFunctionKeyEnable("F5", true);
                    break;
                default:
                    GetUserControlFKey().SetFunctionKeyEnable("F5", false);
                    break;
            }

            beforeSelectedIndex = DataForm.CurrentIndex;

            switch (activeControl.Name)
            {
                case "txtNo":
                     beforeValue = ExCast.zCStr(_entityListD[beforeSelectedIndex]._rec_no); break;
                case "txtReceiptDivisionId":
                    beforeValue = ExCast.zCStr(_entityListD[beforeSelectedIndex]._receipt_division_id); break;
                case "txtReceiptDivisionNm":
                    beforeValue = ExCast.zCStr(_entityListD[beforeSelectedIndex]._receipt_division_nm); break;
                case "txtBillSiteDay":
                    beforeValue = ExCast.zCStr(_entityListD[beforeSelectedIndex]._bill_site_day); break;
                case "txtPrice":
                    beforeValue = ExCast.zCStr(_entityListD[beforeSelectedIndex]._price); break;
                case "txtDetailMemo":
                    beforeValue = ExCast.zCStr(_entityListD[beforeSelectedIndex]._memo); break;
            }
        }

        #endregion

        #region Lost Focus

        private void txt_LostFocus(object sender, RoutedEventArgs e)
        {
            Control ctl = (Control)sender;
            ExTextBox txt = null;
            ExDatePicker dap = null;
            ComboBox cmb = null;
            int i = beforeSelectedIndex;

            switch (ctl.Name)
            {
                case "txtReceiptDivisionId":
                    txt = (ExTextBox)sender;
                    if (beforeValue == ExCast.zCStr(txt.Text)) return;
                    MstData _mstData = new MstData();
                    _mstData.GetMData(MstData.geMDataKbn.RecieptDivision, new string[] { ExCast.zCStr(txt.Text), ExCast.zCStr(i) }, this);
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region DoubleClick

        private void txtReceiptDivisionId_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            beforeValueDlg = "";
            if (_entityListD.Count >= DataForm.CurrentIndex)
            {
                beforeValueDlg = _entityListD[DataForm.CurrentIndex]._receipt_division_id;
            }

            if (_txtReceiptDivisionId == null)
            {
                _txtReceiptDivisionId = ExVisualTreeHelper.FindTextBox(this.DataForm, "txtReceiptDivisionId");
            }

            Dlg_MstSearch searchDlg = new Dlg_MstSearch();
            searchDlg.MstKbn = MstData.geMDataKbn.RecieptDivision;
            searchDlg.Show();
            searchDlg.Closed += searchDlg_Closed;
        }

        private void searchDlg_Closed(object sender, EventArgs e)
        {
            Dlg_MstSearch dlg = (Dlg_MstSearch)sender;
            if (Dlg_MstSearch.this_DialogResult == true)
            {
                if (_entityListD.Count > DataForm.CurrentIndex && DataForm.CurrentIndex != -1)
                {
                    _entityListD[DataForm.CurrentIndex]._receipt_division_id = Dlg_MstSearch.this_id;
                    _entityListD[DataForm.CurrentIndex]._receipt_division_nm = Dlg_MstSearch.this_name;
                }
            }
            if (_entityListD.Count > DataForm.CurrentIndex && DataForm.CurrentIndex != -1)
            {
                if (beforeValueDlg != _entityListD[DataForm.CurrentIndex]._receipt_division_id)
                {
                    MstData _mstData = new MstData();
                    _mstData.GetMData(MstData.geMDataKbn.RecieptDivision, new string[] { _entityListD[DataForm.CurrentIndex]._receipt_division_id }, this);
                    this.Focus();
                    _txtReceiptDivisionNm.Focus();
                    _txtReceiptDivisionId = null;
                }
            }
        }

        private void txtBillSiteDay_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            beforeValueDlg = "";
            if (_entityListD.Count >= DataForm.CurrentIndex)
            {
                beforeValueDlg = _entityListD[DataForm.CurrentIndex]._bill_site_day;
            }

            if (_txtBillSiteDay == null)
            {
                _txtBillSiteDay = ExVisualTreeHelper.FindDatePicker(this.DataForm, "txtBillSiteDay");
            }

            _txtBillSiteDay.ShowCalender();

            //Dlg_Calender calenderDlg = new Dlg_Calender();
            //calenderDlg = new Dlg_Calender();
            //calenderDlg.Show();
            //calenderDlg.Closed += calenderDlg_Closed;
        }

        private void calenderDlg_Closed(object sender, EventArgs e)
        {
            Dlg_Calender dlg = (Dlg_Calender)sender;
            if (dlg.DialogResult == true)
            {
                if (_entityListD.Count > DataForm.CurrentIndex && DataForm.CurrentIndex != -1)
                {
                    _entityListD[DataForm.CurrentIndex]._receipt_division_id = Dlg_MstSearch.this_id;
                    _entityListD[DataForm.CurrentIndex]._receipt_division_nm = Dlg_MstSearch.this_name;
                }
            }
            if (_entityListD.Count > DataForm.CurrentIndex && DataForm.CurrentIndex != -1)
            {
                if (beforeValueDlg != _entityListD[DataForm.CurrentIndex]._receipt_division_id)
                {
                    MstData _mstData = new MstData();
                    _mstData.GetMData(MstData.geMDataKbn.RecieptDivision, new string[] { _entityListD[DataForm.CurrentIndex]._receipt_division_id }, this);
                    _txtBillSiteDay.Focus();
                    _txtBillSiteDay = null;
                }
            }
        }

        #endregion

        #region Master Name Select

        public override void MstDataSelect(ExWebServiceMst.geWebServiceMstNmCallKbn intKbn, svcMstData.EntityMstData mst)
        {
            try
            {
                svcMstData.EntityMstData _mst = null;
                _mst = mst;

                switch (intKbn)
                {
                    case ExWebServiceMst.geWebServiceMstNmCallKbn.GetRecieptDivision:
                        if (DataForm.CurrentIndex == -1) return;

                        if (_mst == null) return;

                        // attribute20に選択された行が設定されてくる
                        if (string.IsNullOrEmpty(_mst.attribute20))
                        {
                            _entityListD[ExCast.zCInt(_mst.attribute20) + 1]._receipt_division_nm = "";
                            OnFormatAll();
                        }
                        else
                        {
                            if (_entityListD.Count >= ExCast.zCInt(_mst.attribute20) + 1)
                            {
                                _entityListD[ExCast.zCInt(_mst.attribute20)]._receipt_division_nm = mst.name;
                                OnFormatAll();
                            }
                        }
                        break;
                }
            }
            catch
            { 
            }

        }

        #endregion

        #region Text KeyDown

        private void txt_KeyDown(object sender, KeyEventArgs e)
        {
            //if (Application.Current.IsRunningOutOfBrowser == false) return;

            switch (e.Key)
            {
                case Key.Down:
                case Key.Up:
                    Control ctl = (Control)sender;
                    if (ctl is ExTextBox)
                    {
                        e.Handled = true;
                    }
                    break;
                case Key.Enter:
                    if (Keyboard.Modifiers == ModifierKeys.Shift)
                    {
                        this.OnBeforeControl();
                    }
                    else
                    {
                        this.OnNextControl();
                    }
                    break;
            }
            this.IsKeyDown = true;
        }

        private void txt_KeyUp(object sender, KeyEventArgs e)
        {
            //if (Application.Current.IsRunningOutOfBrowser == false) return;

            Control ctl = null;

            switch (e.Key)
            {
                case Key.Enter:
                    if (this.IsKeyDown == true)
                    {
                        this.IsKeyDown = false;
                        return;
                    }
                    //Control _ctl = (Control)FocusManager.GetFocusedElement();
                    //if (_ctl != null)
                    //{
                    //    if (ctl is System.Windows.Controls.Primitives.DatePickerTextBox)
                    //    {
                    //        return;
                    //    }
                    //}

                    if (activeControl.Name == "txtBillSiteDay")
                    {
                        if (Keyboard.Modifiers == ModifierKeys.Shift)
                        {
                            this.OnBeforeControl();
                        }
                        else
                        {
                            this.OnNextControl();
                        }
                    }
                    break;
                case Key.Down:
                    ctl = (Control)sender;
                    if (ctl is ExTextBox || ctl is ExDatePicker)
                    {
                        this.OnNextControl();
                    }
                    break;
                case Key.Up:
                    ctl = (Control)sender;
                    if (ctl is ExTextBox || ctl is ExDatePicker)
                    {
                        this.OnBeforeControl();
                    }
                    break;
            }
            this.IsKeyDown = false;
        }
        
        #endregion

        #region Method

        public void Init()
        {
            ConvertDetailToDataForm();
            this.DataForm.ItemsSource = this._entityListD;
            this.utlFunctionKey.gFunctionKeyEnable = Utl_FunctionKey.geFunctionKeyEnable.Upd;
        }

        private Utl_FunctionKey GetUserControlFKey()
        {
            if (utlFncKey == null)
            {
                utlFncKey = ExVisualTreeHelper.GetUtlFunctionKey(this.LayoutRoot);
            }
            return utlFncKey;
        }

        // フォーカス・IME制御の為のフォーカス用コントロール設定
        private void SetControl()
        {
            if (this._txtNo != null &&
                this._txtReceiptDivisionId != null &&
                this._txtReceiptDivisionNm != null &&
                this._txtBillSiteDay != null &&
                this._txtPrice != null &&
                this._txtDetailMemo != null)
            {
                SetFocusOn();
            }

            string[] _name = new string[6];
            _name[0] = "txtNo";
            _name[1] = "txtReceiptDivisionId";
            _name[2] = "txtReceiptDivisionNm";
            _name[3] = "txtPrice";
            _name[4] = "txtBillSiteDay";
            _name[5] = "txtDetailMemo";
            List<SlvHanbaiClient.Class.UI.ListControl> lst = ExVisualTreeHelper.FindControlList(this.DataForm, _name);

            for (int i = 0; i < lst.Count; i++)
            {
                switch (lst[i].ctlName)
                {
                    case "txtNo":
                        if (this._txtNo == null) this._txtNo = (ExTextBox)lst[i].ctl;
                        break;
                    case "txtReceiptDivisionId":
                        if (this._txtReceiptDivisionId == null) this._txtReceiptDivisionId = (ExTextBox)lst[i].ctl;
                        break;
                    case "txtReceiptDivisionNm":
                        if (this._txtReceiptDivisionNm == null) this._txtReceiptDivisionNm = (ExTextBox)lst[i].ctl;
                        break;
                    case "txtBillSiteDay":
                        if (this._txtBillSiteDay == null) this._txtBillSiteDay = (ExDatePicker)lst[i].ctl;
                        break;
                    case "txtPrice":
                        if (this._txtPrice == null) this._txtPrice = (ExTextBox)lst[i].ctl;
                        break;
                    case "txtDetailMemo":
                        if (this._txtDetailMemo == null) this._txtDetailMemo = (ExTextBox)lst[i].ctl;
                        break;
                }
            }

            SetFocusOn();
        }

        private void SetFocusOn()
        {
            if (this.IsFocusOn == false)
            {
                this.IsFocusOn = true;

                Control _ctl = this.activeControl;

                // IME制御の為のフォーカス移動
                this._txtReceiptDivisionId.Focus();
                this._txtReceiptDivisionNm.Focus();
                this._txtBillSiteDay.Focus();
                this._txtPrice.Focus();
                this._txtDetailMemo.Focus();
                this._txtPrice.Focus();
                this._txtBillSiteDay.Focus();
                this._txtReceiptDivisionNm.Focus();
                this._txtReceiptDivisionId.Focus();

                this._txtReceiptDivisionId.OnFormatString();
                this._txtPrice.OnFormatString();

                // 前のコントロールにフォーカス移動
                if (_ctl == null)
                {
                    ExBackgroundWorker.DoWork_FocusForLoad(this._txtReceiptDivisionId);
                }
                else
                {
                    switch (_ctl.Name)
                    {
                        case "txtReceiptDivisionId": ExBackgroundWorker.DoWork_FocusForLoad(this._txtReceiptDivisionId); break;
                        case "txtReceiptDivisionNm": ExBackgroundWorker.DoWork_FocusForLoad(this._txtReceiptDivisionNm); break;
                        case "txtBillSiteDay": ExBackgroundWorker.DoWork_FocusForLoad(this._txtBillSiteDay); break;
                        case "txtPrice": ExBackgroundWorker.DoWork_FocusForLoad(this._txtPrice); break;
                        case "txtDetailMemo": ExBackgroundWorker.DoWork_FocusForLoad(this._txtDetailMemo); break;
                    }
                }
            }
        }

        private void OnFormatAll()
        {
            if (this._txtReceiptDivisionId != null) this._txtReceiptDivisionId.OnFormatString();
            if (this._txtPrice != null) this._txtPrice.OnFormatString();
        }

        private void InitControl()
        {
            this.IsFocusOn = false;

            this._txtNo = null;
            this._txtReceiptDivisionId = null;
            this._txtReceiptDivisionNm = null;
            this._txtBillSiteDay = null;
            this._txtPrice = null;
            this._txtDetailMemo = null;

            SetControl();
        }

        private void OnBeforeControl()
        {
            switch (activeControl.Name)
            {

                case "txtNo": break;
                case "txtReceiptDivisionId": this._txtNo.Focus(); break;
                case "txtReceiptDivisionNm": this._txtReceiptDivisionId.Focus(); break;
                case "txtPrice": this._txtReceiptDivisionNm.Focus(); break;
                case "txtBillSiteDay": this._txtPrice.Focus(); break;
                case "txtDetailMemo": this._txtBillSiteDay.Focus(); break;
            }
        }

        private void OnNextControl()
        {
            switch (activeControl.Name)
            {
                case "txtNo": this._txtReceiptDivisionId.Focus(); break;
                case "txtReceiptDivisionId": this._txtReceiptDivisionNm.Focus(); break;
                case "txtReceiptDivisionNm": this._txtPrice.Focus(); break;
                case "txtPrice": this._txtBillSiteDay.Focus(); break;
                case "txtBillSiteDay": this._txtDetailMemo.Focus(); break;
                case "txtDetailMemo": break;
            }
        }

        #endregion

    }
}