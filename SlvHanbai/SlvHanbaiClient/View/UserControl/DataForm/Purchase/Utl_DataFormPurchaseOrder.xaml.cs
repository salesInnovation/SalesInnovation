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
using SlvHanbaiClient.svcOrder;
using SlvHanbaiClient.svcPurchaseOrder;
using SlvHanbaiClient.View.UserControl.Custom;
using SlvHanbaiClient.View.Dlg;

namespace SlvHanbaiClient.View.UserControl.DataForm.Purchase
{
    public partial class Utl_DataFormPurchaseOrder : ExUserControl
    {

        #region Filed Const

        private const String CLASS_NM = "Utl_DataFormPurchaseOrder";

        public EntityPurchaseOrderH _entityH = new EntityPurchaseOrderH();
        public ObservableCollection< EntityPurchaseOrderD> _entityListD = new ObservableCollection<EntityPurchaseOrderD>();
        public ObservableCollection<EntityDataFormOrderD> objDataFormOrderD = new ObservableCollection<EntityDataFormOrderD>();
        public object objBeforeOrderListD;
        private Control activeControl;

        private string beforeValue = "";
        private int beforeSelectedIndex = -1;           // 編集前 行
        private string beforeValueDlg = "";

        #region Control

        private Utl_FunctionKey utlFncKey;
        private ExTextBox _txtNo = null;
        private ComboBox _cmbBreakdown = null;
        private ExTextBox _txtDeliverDivision = null;
        private ExTextBox _txtGoodsId = null;
        private ExTextBox _txtGoodsNm = null;
        private ComboBox _cboUnit = null;
        private ExTextBox _txtEnterNum = null;
        private ExTextBox _txtCaseNum = null;
        private ExTextBox _txtNumber = null;
        private ExTextBox _txtUnitPrice = null;
        private ExTextBox _txtPrice = null;
        private ComboBox _cboTaxDivision = null;
        private ExTextBox _txtDetailMemo = null;
        private bool IsFocusOn = false;

        #endregion

        #endregion

        #region Constructor

        public Utl_DataFormPurchaseOrder()
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

                    // コンボボックス初期選択
                    List<string> lst;
                    lst = MeiNameList.GetListMei(MeiNameList.geNameKbn.BREAKDOWN_ID);
                    _entityListD[DataForm.CurrentIndex]._breakdown_nm = lst[0];
                    _entityListD[DataForm.CurrentIndex]._breakdown_id = MeiNameList.GetID(MeiNameList.geNameKbn.BREAKDOWN_ID, lst[0]);

                    lst = MeiNameList.GetListMei(MeiNameList.geNameKbn.DELIVER_DIVISION_ID);
                    _entityListD[DataForm.CurrentIndex]._deliver_division_nm = lst[0];
                    _entityListD[DataForm.CurrentIndex]._deliver_division_id = MeiNameList.GetID(MeiNameList.geNameKbn.DELIVER_DIVISION_ID, lst[0]);

                    lst = MeiNameList.GetListMei(MeiNameList.geNameKbn.UNIT_ID);
                    _entityListD[DataForm.CurrentIndex]._unit_nm = lst[0];
                    _entityListD[DataForm.CurrentIndex]._unit_id = MeiNameList.GetID(MeiNameList.geNameKbn.UNIT_ID, lst[0]);

                    lst = MeiNameList.GetListMei(MeiNameList.geNameKbn.TAX_DIVISION_ID);
                    _entityListD[DataForm.CurrentIndex]._tax_division_nm = lst[0];
                    _entityListD[DataForm.CurrentIndex]._tax_division_id = MeiNameList.GetID(MeiNameList.geNameKbn.TAX_DIVISION_ID, lst[0]);

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

            //this.DataForm.CurrentIndex = this.DataForm.CurrentIndex + 1;
            _entityListD.Add(new EntityPurchaseOrderD());
            this.DataForm.CurrentIndex = _entityListD.Count - 1;
            _entityListD[_entityListD.Count - 1]._rec_no = DataForm.CurrentIndex + 1;

            // コンボボックス初期選択
            List<string> lst;
            lst = MeiNameList.GetListMei(MeiNameList.geNameKbn.BREAKDOWN_ID);
            _entityListD[_entityListD.Count - 1]._breakdown_nm = lst[0];
            lst = MeiNameList.GetListMei(MeiNameList.geNameKbn.UNIT_ID);
            _entityListD[_entityListD.Count - 1]._unit_nm = lst[0];
            lst = MeiNameList.GetListMei(MeiNameList.geNameKbn.TAX_DIVISION_ID);
            _entityListD[_entityListD.Count - 1]._tax_division_nm = lst[0];

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
                case "txtGoodsId":
                    txtGoodsId_MouseDoubleClick(null, null);
                    break;
                case "txtUnitPrice":
                    txtUnitPrice_MouseDoubleClick(null, null);
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
                EntityPurchaseOrderD entity = new EntityPurchaseOrderD();
                entity._rec_no = 1;

                // コンボボックス初期選択
                List<string> lst;
                lst = MeiNameList.GetListMei(MeiNameList.geNameKbn.BREAKDOWN_ID);
                entity._breakdown_nm = lst[0];
                lst = MeiNameList.GetListMei(MeiNameList.geNameKbn.UNIT_ID);
                entity._unit_nm = lst[0];
                lst = MeiNameList.GetListMei(MeiNameList.geNameKbn.TAX_DIVISION_ID);
                entity._tax_division_nm = lst[0];

                _entityListD = new ObservableCollection<EntityPurchaseOrderD>();
                _entityListD.Add(entity);

                return;
            }

            for (int i = 0; i <= _entityListD.Count - 1; i++)
            {
                EntityDataFormOrderD _entityDataFormOrderD = new EntityDataFormOrderD();
                _entityDataFormOrderD.id = _entityListD[i]._id;
                _entityDataFormOrderD.rec_no = _entityListD[i]._rec_no;
                _entityDataFormOrderD.breakdown_id = _entityListD[i]._breakdown_id;
                _entityDataFormOrderD.breakdown_nm = _entityListD[i]._breakdown_nm;
                _entityDataFormOrderD.deliver_division_id = _entityListD[i]._deliver_division_id;
                _entityDataFormOrderD.deliver_division_nm = _entityListD[i]._deliver_division_nm;
                _entityDataFormOrderD.commodity_id = _entityListD[i]._commodity_id;
                _entityDataFormOrderD.commodity_name = _entityListD[i]._commodity_name;
                _entityDataFormOrderD.unit_id = _entityListD[i]._unit_id;
                _entityDataFormOrderD.unit_nm = _entityListD[i]._unit_nm;
                _entityDataFormOrderD.enter_number = _entityListD[i]._enter_number;
                _entityDataFormOrderD.case_number = _entityListD[i]._case_number;
                _entityDataFormOrderD.number = _entityListD[i]._number;
                _entityDataFormOrderD.unit_price = _entityListD[i]._unit_price;
                //_entityDataFormOrderD.sales_cost = _entityListD[i]._sales_cost;
                _entityDataFormOrderD.tax = _entityListD[i]._tax;
                _entityDataFormOrderD.no_tax_price = _entityListD[i]._no_tax_price;
                _entityDataFormOrderD.price = _entityListD[i]._price;
                //_entityDataFormOrderD.profits = _entityListD[i]._profits;
                //_entityDataFormOrderD.profits_percent = _entityListD[i]._profits_percent;
                _entityDataFormOrderD.memo = _entityListD[i]._memo;
                _entityDataFormOrderD.tax_division_id = _entityListD[i]._tax_division_id;
                _entityDataFormOrderD.tax_division_nm = _entityListD[i]._tax_division_nm;
                _entityDataFormOrderD.tax_percent = _entityListD[i]._tax_percent;
                _entityDataFormOrderD.inventory_management_division_id = _entityListD[i]._inventory_management_division_id;
                _entityDataFormOrderD.inventory_number = _entityListD[i]._inventory_number;
                _entityDataFormOrderD.retail_price_skip_tax = _entityListD[i]._retail_price_skip_tax;
                _entityDataFormOrderD.retail_price_before_tax = _entityListD[i]._retail_price_before_tax;
                _entityDataFormOrderD.sales_unit_price_skip_tax = _entityListD[i]._sales_unit_price_skip_tax;
                _entityDataFormOrderD.sales_unit_price_before_tax = _entityListD[i]._sales_unit_price_before_tax;
                _entityDataFormOrderD.sales_cost_price_skip_tax = _entityListD[i]._sales_cost_price_skip_tax;
                _entityDataFormOrderD.sales_cost_price_before_tax = _entityListD[i]._sales_cost_price_before_tax;
                _entityDataFormOrderD.purchase_unit_price_skip_tax = _entityListD[i]._purchase_unit_price_skip_tax;
                _entityDataFormOrderD.purchase_unit_price_before_tax = _entityListD[i]._purchase_unit_price_before_tax;
                _entityDataFormOrderD.number_decimal_digit = _entityListD[i]._number_decimal_digit;
                _entityDataFormOrderD.unit_decimal_digit = _entityListD[i]._unit_decimal_digit;

                objDataFormOrderD.Add(_entityDataFormOrderD);
            }
        }

        private void ConvertDataFormToDetail()
        {
            this.DataForm.CommitEdit();
            if (_entityListD != null) _entityListD.Clear();

            for (int i = 0; i <= objDataFormOrderD.Count - 1; i++)
            {
                EntityPurchaseOrderD entity = new EntityPurchaseOrderD();

                entity._id = objDataFormOrderD[i].id;
                entity._rec_no = objDataFormOrderD[i].rec_no;
                entity._breakdown_id = objDataFormOrderD[i].breakdown_id;
                entity._breakdown_nm = objDataFormOrderD[i].breakdown_nm;
                entity._deliver_division_id = objDataFormOrderD[i].deliver_division_id;
                entity._deliver_division_nm = objDataFormOrderD[i].deliver_division_nm;
                entity._commodity_id = objDataFormOrderD[i].commodity_id;
                entity._commodity_name = objDataFormOrderD[i].commodity_name;
                entity._unit_id = objDataFormOrderD[i].unit_id;
                entity._unit_nm = objDataFormOrderD[i].unit_nm;
                entity._enter_number = objDataFormOrderD[i].enter_number;
                entity._case_number = objDataFormOrderD[i].case_number;
                entity._number = objDataFormOrderD[i].number;
                entity._unit_price = objDataFormOrderD[i].unit_price;
                //entity._sales_cost = objDataFormOrderD[i].sales_cost;
                entity._tax = objDataFormOrderD[i].tax;
                entity._no_tax_price = objDataFormOrderD[i].no_tax_price;
                entity._price = objDataFormOrderD[i].price;
                //entity._profits = objDataFormOrderD[i].profits;
                //entity._profits_percent = objDataFormOrderD[i].profits_percent;
                entity._memo = objDataFormOrderD[i].memo;
                entity._tax_division_id = objDataFormOrderD[i].tax_division_id;
                entity._tax_division_nm = objDataFormOrderD[i].tax_division_nm;
                entity._tax_percent = objDataFormOrderD[i].tax_percent;
                entity._inventory_management_division_id = objDataFormOrderD[i].inventory_management_division_id;
                entity._inventory_number = objDataFormOrderD[i].inventory_number;
                entity._retail_price_skip_tax = objDataFormOrderD[i].retail_price_skip_tax;
                entity._retail_price_before_tax = objDataFormOrderD[i].retail_price_before_tax;
                entity._sales_unit_price_skip_tax = objDataFormOrderD[i].sales_unit_price_skip_tax;
                entity._sales_unit_price_before_tax = objDataFormOrderD[i].sales_unit_price_before_tax;
                entity._sales_cost_price_skip_tax = objDataFormOrderD[i].sales_cost_price_skip_tax;
                entity._sales_cost_price_before_tax = objDataFormOrderD[i].sales_cost_price_before_tax;
                entity._purchase_unit_price_skip_tax = objDataFormOrderD[i].purchase_unit_price_skip_tax;
                entity._purchase_unit_price_before_tax = objDataFormOrderD[i].purchase_unit_price_before_tax;
                entity._number_decimal_digit = objDataFormOrderD[i].number_decimal_digit;
                entity._unit_decimal_digit = objDataFormOrderD[i].unit_decimal_digit;

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
                case "txtGoodsId":
                case "txtUnitPrice":
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
                case "cmbBreakdown":
                    beforeValue = ExCast.zCStr(_entityListD[beforeSelectedIndex]._breakdown_nm); break;
                case "txtDeliverDivision":
                    beforeValue = ExCast.zCStr(_entityListD[beforeSelectedIndex]._tax_division_nm); break;
                case "txtGoodsId":
                    beforeValue = ExCast.zCStr(_entityListD[beforeSelectedIndex]._commodity_id); break;
                case "txtGoodsNm":
                    beforeValue = ExCast.zCStr(_entityListD[beforeSelectedIndex]._commodity_name); break;
                case "cboUnit":
                    beforeValue = ExCast.zCStr(_entityListD[beforeSelectedIndex]._unit_nm); break;
                case "txtCaseNum":
                    beforeValue = ExCast.zCStr(_entityListD[beforeSelectedIndex]._case_number); break;
                case "txtEnterNum":
                    beforeValue = ExCast.zCStr(_entityListD[beforeSelectedIndex]._enter_number); break;
                case "txtNumber":
                    beforeValue = ExCast.zCStr(_entityListD[beforeSelectedIndex]._number); break;
                case "txtUnitPrice":
                    beforeValue = ExCast.zCStr(_entityListD[beforeSelectedIndex]._unit_price); break;
                case "txtPrice":
                    beforeValue = ExCast.zCStr(_entityListD[beforeSelectedIndex]._price); break;
                case "cboTaxDivision":
                    beforeValue = ExCast.zCStr(_entityListD[beforeSelectedIndex]._breakdown_nm); break;
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
            ComboBox cmb = null;
            int i = beforeSelectedIndex;

            switch (ctl.Name)
            {
                case "cmbBreakdown":
                    cmb = (ComboBox)sender;
                    if (_entityListD.Count > i && i != -1)
                    {
                        // コンボボックスID連携
                        _entityListD[i]._breakdown_id = MeiNameList.GetID(MeiNameList.geNameKbn.BREAKDOWN_ID, ExCast.zCStr(cmb.SelectedValue));
                        // 消費税
                        if (_entityListD[i]._breakdown_id == 5)
                        {
                            _entityListD[i]._tax_division_nm = "非課税";
                            _entityListD[i]._tax_division_id = MeiNameList.GetID(MeiNameList.geNameKbn.TAX_DIVISION_ID, ExCast.zCStr(_entityListD[i]._tax_division_nm));
                        }
                    }
                    break;
                case "txtGoodsId":
                    txt = (ExTextBox)sender;
                    if (beforeValue == ExCast.zCStr(txt.Text)) return;
                    MstData _mstData = new MstData();
                    _mstData.GetMData(MstData.geMDataKbn.Commodity, new string[] { ExCast.zCStr(txt.Text), ExCast.zCStr(i) }, this);
                    break;
                case "cboUnit":
                    cmb = (ComboBox)sender;
                    if (_entityListD.Count > i && i != -1)
                    {
                        // コンボボックスID連携
                        _entityListD[i]._unit_id = MeiNameList.GetID(MeiNameList.geNameKbn.UNIT_ID, ExCast.zCStr(cmb.SelectedValue));
                    }
                    break;
                case "txtEnterNum":
                    // 明細入数計算
                    txt = (ExTextBox)sender;
                    if (_entityListD.Count > i && i != -1)
                    {
                        if (beforeValue == ExCast.zCStr(txt.Text)) return;
                        _entityListD[i]._enter_number = ExCast.zCDbl(txt.Text);
                        DataDetail.CalcDetailNumber(i, _entityH, _entityListD);
                    }
                    break;
                case "txtCaseNum":
                    // 明細ケース数計算
                    txt = (ExTextBox)sender;
                    if (_entityListD.Count > i && i != -1)
                    {
                        if (beforeValue == ExCast.zCStr(txt.Text)) return;
                        _entityListD[i]._case_number = ExCast.zCDbl(txt.Text);
                        DataDetail.CalcDetailNumber(i, _entityH, _entityListD);
                    }
                    break;
                case "txtNumber":
                    // 明細数量計算
                    txt = (ExTextBox)sender;
                    if (_entityListD.Count > i && i != -1)
                    {
                        if (beforeValue == ExCast.zCStr(txt.Text)) return;
                        //OrderDetailData.CalcDetailNumber(i, _entityH, _entityListD);
                        _entityListD[i]._number = ExCast.zCDbl(txt.Text);
                        DataDetail.CalcDetail(i, _entityH, _entityListD);
                    }
                    break;
                case "txtUnitPrice":
                    // 明細計算
                    txt = (ExTextBox)sender;
                    if (_entityListD.Count > i && i != -1)
                    {
                        if (beforeValue == ExCast.zCStr(txt.Text)) return;
                        _entityListD[i]._unit_price = ExCast.zCDbl(txt.Text);
                        DataDetail.CalcDetail(i, _entityH, _entityListD);
                    }
                    break;
                case "txtPrice":
                    // 明細計算
                    txt = (ExTextBox)sender;
                    if (_entityListD.Count > i && i != -1)
                    {
                        if (beforeValue == ExCast.zCStr(txt.Text)) return;
                        _entityListD[i]._price = ExCast.zCDbl(txt.Text);
                        DataDetail.IsCalcPrice = false;
                        DataDetail.CalcDetail(i, _entityH, _entityListD);
                        DataDetail.IsCalcPrice = true;
                    }
                    break;
                case "cboTaxDivision":
                    cmb = (ComboBox)sender;
                    if (_entityListD.Count > i && i != -1)
                    {
                        // コンボボックスID連携
                        _entityListD[i]._tax_division_id = MeiNameList.GetID(MeiNameList.geNameKbn.TAX_DIVISION_ID, ExCast.zCStr(cmb.SelectedValue));

                        // 課税区分が課税で内訳が消費税の場合
                        if (_entityListD[i]._tax_division_id == 1 && _entityListD[i]._breakdown_id == 5)
                        {
                            ExMessageBox.Show("内訳が消費税の場合、課税区分に課税を選択できません。");
                            _entityListD[i]._tax_division_nm = "非課税";
                            _entityListD[i]._tax_division_id = MeiNameList.GetID(MeiNameList.geNameKbn.TAX_DIVISION_ID, ExCast.zCStr(_entityListD[i]._tax_division_nm));
                        }

                        // 明細計算
                        DataDetail.CalcDetail(i, _entityH, _entityListD);
                    }
                    break;
                case "txtGoodsNm":
                case "txtDetailMemo":
                    break;
            }
        }

        #endregion

        #region DoubleClick

        private void txtGoodsId_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            beforeValueDlg = "";
            if (_entityListD.Count >= DataForm.CurrentIndex)
            {
                beforeValueDlg = _entityListD[DataForm.CurrentIndex]._commodity_id;
            }

            if (_txtGoodsId == null)
            {
                _txtGoodsId = ExVisualTreeHelper.FindTextBox(this.DataForm, "txtGoodsId");
            }

            Dlg_MstSearch searchDlg = new Dlg_MstSearch();
            searchDlg.MstKbn = MstData.geMDataKbn.Commodity;
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
                    _entityListD[DataForm.CurrentIndex]._commodity_id = Dlg_MstSearch.this_id;
                    _entityListD[DataForm.CurrentIndex]._commodity_name = Dlg_MstSearch.this_name;
                }
            }
            if (_entityListD.Count > DataForm.CurrentIndex && DataForm.CurrentIndex != -1)
            {
                if (beforeValueDlg != _entityListD[DataForm.CurrentIndex]._commodity_id)
                {
                    MstData _mstData = new MstData();
                    _mstData.GetMData(MstData.geMDataKbn.Commodity, new string[] { _entityListD[DataForm.CurrentIndex]._commodity_id }, this);
                    this.Focus();
                    _txtGoodsId.Focus();
                    _txtGoodsId = null;
                }
            }
        }

        private void txtUnitPrice_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            beforeValueDlg = "";
            if (_entityListD.Count >= DataForm.CurrentIndex)
            {
                beforeValueDlg = ExCast.zCStr(_entityListD[DataForm.CurrentIndex]._unit_price);
            }

            if (_txtUnitPrice == null)
            {
                _txtUnitPrice = ExVisualTreeHelper.FindTextBox(this.DataForm, "txtUnitPrice");
            }

            Dlg_UnitPriceSetting unitPriceDlg = new Dlg_UnitPriceSetting();

            // 税転換が内税で明細課税有りの場合
            if ((_entityH._tax_change_id == 4 || _entityH._tax_change_id == 5 || _entityH._tax_change_id == 6) && _entityListD[DataForm.CurrentIndex]._tax_division_id == 1)
            {
                unitPriceDlg.retail_price = _entityListD[DataForm.CurrentIndex]._retail_price_before_tax;
                unitPriceDlg.sales_unit_price = _entityListD[DataForm.CurrentIndex]._purchase_unit_price_before_tax;
                unitPriceDlg.sales_cost_price = _entityListD[DataForm.CurrentIndex]._sales_cost_price_before_tax;
            }
            else
            {
                unitPriceDlg.retail_price = _entityListD[DataForm.CurrentIndex]._retail_price_skip_tax;
                unitPriceDlg.sales_unit_price = _entityListD[DataForm.CurrentIndex]._purchase_unit_price_skip_tax;
                unitPriceDlg.sales_cost_price = _entityListD[DataForm.CurrentIndex]._sales_cost_price_skip_tax;
            }
            unitPriceDlg.unit_decimal_digit = _entityListD[DataForm.CurrentIndex]._unit_decimal_digit;
            unitPriceDlg.credit_rate = _entityH._credit_rate;
            unitPriceDlg.Closed += unitPriceDlg_Closed;
            unitPriceDlg.Show();
        }

        private void unitPriceDlg_Closed(object sender, EventArgs e)
        {
            Dlg_UnitPriceSetting unitPriceDlg = (Dlg_UnitPriceSetting)sender;
            if (unitPriceDlg.DialogResult == true)
            {
                if (_entityListD.Count > DataForm.CurrentIndex && DataForm.CurrentIndex != -1)
                {
                    _entityListD[DataForm.CurrentIndex]._unit_price = unitPriceDlg.return_unit_price;
                }
            }
            if (beforeValue != ExCast.zCStr(_entityListD[DataForm.CurrentIndex]._unit_price))
            {
                DataDetail.CalcDetail(DataForm.CurrentIndex, _entityH, _entityListD);
                this.Focus();
                _txtUnitPrice.Focus();
                _txtUnitPrice = null;
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
                    case ExWebServiceMst.geWebServiceMstNmCallKbn.GetCommodity:
                        if (DataForm.CurrentIndex == -1) return;

                        if (_mst == null) return;

                        // attribute20に選択された行が設定されてくる
                        if (string.IsNullOrEmpty(_mst.attribute20))
                        {
                            // 商品から明細へ
                            DataDetail.SetCommodityToDetail(DataForm.CurrentIndex, this._entityH, this._entityListD, _mst);
                            OnFormatAll();
                        }
                        else
                        {
                            if (_entityListD.Count >= ExCast.zCInt(_mst.attribute20) + 1)
                            {
                                // 商品から明細へ
                                DataDetail.SetCommodityToDetail(ExCast.zCInt(_mst.attribute20), _entityH, _entityListD, _mst);
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
        }

        private void txt_KeyUp(object sender, KeyEventArgs e)
        {
            //if (Application.Current.IsRunningOutOfBrowser == false) return;

            Control ctl = null;

            switch (e.Key)
            {
                case Key.Down:
                    ctl = (Control)sender;
                    if (ctl is ExTextBox)
                    {
                        this.OnNextControl();
                    }
                    break;
                case Key.Up:
                    ctl = (Control)sender;
                    if (ctl is ExTextBox)
                    {
                        this.OnBeforeControl();
                    }
                    break;
            }
        }


        public void OnBeforeControl()
        {
            switch (activeControl.Name)
            {
                case "txtNo": break;
                case "cmbBreakdown": this._txtNo.Focus(); break;
                //case "txtDeliverDivision": this._cmbBreakdown.Focus(); break;
                case "txtGoodsId": this._cmbBreakdown.Focus(); break;
                case "txtGoodsNm": this._txtGoodsId.Focus(); break;
                case "cboUnit": this._txtGoodsNm.Focus(); break;
                case "txtEnterNum": this._cboUnit.Focus(); break;
                case "txtCaseNum": this._txtEnterNum.Focus(); break;
                case "txtNumber": this._txtCaseNum.Focus(); break;
                case "txtUnitPrice": this._txtNumber.Focus(); break;
                case "txtPrice": this._txtUnitPrice.Focus(); break;
                case "cboTaxDivision": this._txtPrice.Focus(); break;
                case "txtDetailMemo": this._cboTaxDivision.Focus(); break;
            }
        }

        public void OnNextControl()
        {
            switch (activeControl.Name)
            {
                case "txtNo": this._cmbBreakdown.Focus(); break;
                case "cmbBreakdown": this._txtGoodsId.Focus(); break;
                //case "txtDeliverDivision": this._txtGoodsId.Focus(); break;
                case "txtGoodsId": this._txtGoodsNm.Focus(); break;
                case "txtGoodsNm": this._cboUnit.Focus(); break;
                case "cboUnit": this._txtEnterNum.Focus(); break;
                case "txtEnterNum": this._txtCaseNum.Focus(); break;
                case "txtCaseNum": this._txtNumber.Focus(); break;
                case "txtNumber": this._txtUnitPrice.Focus(); break;
                case "txtUnitPrice": this._txtPrice.Focus(); break;
                case "txtPrice": this._cboTaxDivision.Focus(); break;
                case "cboTaxDivision": this._txtDetailMemo.Focus(); break;
                case "txtDetailMemo": break;
            }
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
                this._cmbBreakdown != null &&
                this._txtDeliverDivision != null &&
                this._txtGoodsId != null &&
                this._txtGoodsNm != null &&
                this._cboUnit != null &&
                this._txtEnterNum != null &&
                this._txtCaseNum != null &&
                this._txtNumber != null &&
                this._txtUnitPrice != null &&
                this._txtPrice != null &&
                this._cboTaxDivision != null &&
                this._txtDetailMemo != null)
            {
                SetFocusOn();
            }

            string[] _name = new string[13];
            _name[0] = "txtNo";
            _name[1] = "cmbBreakdown";
            _name[2] = "txtDeliverDivision";
            _name[3] = "txtGoodsId";
            _name[4] = "txtGoodsNm";
            _name[5] = "cboUnit";
            _name[6] = "txtCaseNum";
            _name[7] = "txtEnterNum";
            _name[8] = "txtNumber";
            _name[9] = "txtUnitPrice";
            _name[10] = "txtPrice";
            _name[11] = "cboTaxDivision";
            _name[12] = "txtDetailMemo";
            List<SlvHanbaiClient.Class.UI.ListControl> lst = ExVisualTreeHelper.FindControlList(this.DataForm, _name);

            for (int i = 0; i < lst.Count; i++)
            {
                switch (lst[i].ctlName)
                {
                    case "txtNo":
                        if (this._txtNo == null) this._txtNo = (ExTextBox)lst[i].ctl;
                        break;
                    case "cmbBreakdown":
                        if (this._cmbBreakdown == null) this._cmbBreakdown = (ComboBox)lst[i].ctl;
                        break;
                    case "txtDeliverDivision":
                        if (this._txtDeliverDivision == null) this._txtDeliverDivision = (ExTextBox)lst[i].ctl;
                        break;
                    case "txtGoodsId":
                        if (this._txtGoodsId == null) this._txtGoodsId = (ExTextBox)lst[i].ctl;
                        break;
                    case "txtGoodsNm":
                        if (this._txtGoodsNm == null) this._txtGoodsNm = (ExTextBox)lst[i].ctl;
                        break;
                    case "cboUnit":
                        if (this._cboUnit == null) this._cboUnit = (ComboBox)lst[i].ctl;
                        break;
                    case "txtCaseNum":
                        if (this._txtCaseNum == null) this._txtCaseNum = (ExTextBox)lst[i].ctl;
                        break;
                    case "txtEnterNum":
                        if (this._txtEnterNum == null) this._txtEnterNum = (ExTextBox)lst[i].ctl;
                        break;
                    case "txtNumber":
                        if (this._txtNumber == null) this._txtNumber = (ExTextBox)lst[i].ctl;
                        break;
                    case "txtUnitPrice":
                        if (this._txtUnitPrice == null) this._txtUnitPrice = (ExTextBox)lst[i].ctl;
                        break;
                    case "txtPrice":
                        if (this._txtPrice == null) this._txtPrice = (ExTextBox)lst[i].ctl;
                        break;
                    case "cboTaxDivision":
                        if (this._cboTaxDivision == null) this._cboTaxDivision = (ComboBox)lst[i].ctl;
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
                this._txtDetailMemo.Focus();
                this._cboTaxDivision.Focus();
                this._txtPrice.Focus();
                this._txtUnitPrice.Focus();
                this._txtNumber.Focus();
                this._txtCaseNum.Focus();
                this._txtEnterNum.Focus();
                this._cboUnit.Focus();
                this._txtGoodsNm.Focus();
                this._txtGoodsId.Focus();
                this._cmbBreakdown.Focus();

                this._txtPrice.OnFormatString();
                this._txtUnitPrice.OnFormatString();
                this._txtNumber.OnFormatString();
                this._txtCaseNum.OnFormatString();
                this._txtPrice.OnFormatString();
                this._txtEnterNum.OnFormatString();

                // 前のコントロールにフォーカス移動
                if (_ctl == null)
                {
                    ExBackgroundWorker.DoWork_FocusForLoad(this._txtGoodsId);
                }
                else
                {
                    switch (_ctl.Name)
                    {
                        case "cmbBreakdown": ExBackgroundWorker.DoWork_FocusForLoad(this._cmbBreakdown); break;
                        case "txtGoodsId": ExBackgroundWorker.DoWork_FocusForLoad(this._txtGoodsId); break;
                        case "txtGoodsNm": ExBackgroundWorker.DoWork_FocusForLoad(this._txtGoodsNm); break;
                        case "cboUnit": ExBackgroundWorker.DoWork_FocusForLoad(this._cboUnit); break;
                        case "txtEnterNum": ExBackgroundWorker.DoWork_FocusForLoad(this._txtEnterNum); break;
                        case "txtCaseNum": ExBackgroundWorker.DoWork_FocusForLoad(this._txtCaseNum); break;
                        case "txtNumber": ExBackgroundWorker.DoWork_FocusForLoad(this._txtNumber); break;
                        case "txtUnitPrice": ExBackgroundWorker.DoWork_FocusForLoad(this._txtUnitPrice); break;
                        case "txtPrice": ExBackgroundWorker.DoWork_FocusForLoad(this._txtPrice); break;
                        case "cboTaxDivision": ExBackgroundWorker.DoWork_FocusForLoad(this._cboTaxDivision); break;
                        case "txtDetailMemo": ExBackgroundWorker.DoWork_FocusForLoad(this._txtDetailMemo); break;
                    }
                }
            }
        }

        private void OnFormatAll()
        {
            if (this._txtEnterNum != null) this._txtEnterNum.OnFormatString();
            if (this._txtCaseNum != null) this._txtCaseNum.OnFormatString();
            if (this._txtNumber != null) this._txtNumber.OnFormatString();
            if (this._txtUnitPrice != null) this._txtUnitPrice.OnFormatString();
            if (this._txtPrice != null) this._txtPrice.OnFormatString();
        }

        private void InitControl()
        {
            this.IsFocusOn = false;

            this._txtNo = null;
            this._cmbBreakdown = null;
            this._txtDeliverDivision = null;
            this._txtGoodsId = null;
            this._txtGoodsNm = null;
            this._cboUnit = null;
            this._txtEnterNum = null;
            this._txtCaseNum = null;
            this._txtNumber = null;
            this._txtUnitPrice = null;
            this._txtPrice = null;
            this._cboTaxDivision = null;
            this._txtDetailMemo = null;

            SetControl();
        }

        #endregion

    }
}