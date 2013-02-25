using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Navigation;
using System.Windows.Data;
using System.Threading;
using System.Collections.ObjectModel;  // ObservableCollectionを利用するために必要   
using System.ServiceModel.DomainServices.Client;
using SlvHanbaiClient.Class;
using SlvHanbaiClient.Class.Converter;
using SlvHanbaiClient.Class.Utility;
using SlvHanbaiClient.Class.UI;
using SlvHanbaiClient.Class.Data;
using SlvHanbaiClient.Class.Data;
using SlvHanbaiClient.Class.WebService;
using SlvHanbaiClient.svcCondition;
using SlvHanbaiClient.View.UserControl.Custom;
using SlvHanbaiClient.View.Dlg;

namespace SlvHanbaiClient.View.UserControl.Master
{
    public partial class Utl_MstCondition : ExUserControl
    {

        #region Filed Const

        private const String CLASS_NM = "Utl_MstCondition";
        private const String PG_NM = DataPgEvidence.PGName.Mst.Condition;
        private const Common.geWinMsterType _WinMsterType = Common.geWinMsterType.Condition;
        private const ExWebService.geWebServiceCallKbn _GetWebServiceCallKbn = ExWebService.geWebServiceCallKbn.GetCondition;
        private const ExWebService.geWebServiceCallKbn _UpdWebServiceCallKbn = ExWebService.geWebServiceCallKbn.UpdateCondition;
        private Class.Data.MstData.geMDataKbn MstKbn = Class.Data.MstData.geMDataKbn.Condition;
        private ObservableCollection<EntityCondition> _entity = new ObservableCollection<EntityCondition>();
        private readonly string tableName = "M_CONDITION";
        //private Control activeControl;
        //private Dlg_Copying searchDlg;
        private Common.geWinGroupType beforeWinGroupType;
        private int _selectIndex = 0;                   // データグリッド現在行保持用
        private int _selectColumn = 0;                  // データグリッド現在列保持用

        #endregion

        #region Constructor

        public Utl_MstCondition()
        {
            InitializeComponent();
            this.Tag = "Main";      // ファンクションキーを受けつけ用
        }

        #endregion

        #region Page Events

        private void ExUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (Common.gblnLogin == false) return;

            // 画面初期化
            ExVisualTreeHelper.initDisplay(this.LayoutRoot);

            // ファンクションキー初期設定
            this.utlFunctionKey.gFunctionKeyEnable = Utl_FunctionKey.geFunctionKeyEnable.Upd;
            this.utlFunctionKey.Init();

            // バインド設定
            SetBinding();

            GetMstData();
        }

        private void ExUserControl_Unloaded(object sender, RoutedEventArgs e)
        {
        }

        #endregion

        #region Function Key Button Events

        // F1ボタン(登録) クリック
        public override void btnF1_Click(object sender, RoutedEventArgs e)
        {
            dg.CommitEdit();

            // ボタン押下時非同期入力チェックON
            Common.gblnBtnDesynchronizeLock = true;

            // 入力確定の為、BackgroundWorker経由で入力チェックを呼出
            ExBackgroundInputCheckWk bk = new ExBackgroundInputCheckWk();
            bk.utl = this;
            this.txtDummy.IsTabStop = true;
            bk.focusCtl = this.txtDummy;
            bk.bw.RunWorkerAsync();
        }

        // F2ボタン(クリア) クリック
        public override void btnF2_Click(object sender, RoutedEventArgs e)
        {
            this.txtDummy.Focus();
            
            // 初期化
            _entity = null;
            GetMstData();
        }

        // F3ボタン(行挿入) クリック
        public override void btnF3_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // 明細初期設定
                EntityCondition entity = new EntityCondition();
                InitDetail(ref entity);

                int _selectedIndex = 0;
                int _displayIndex = 0;

                if (_entity == null)
                {
                    _entity = new ObservableCollection<EntityCondition>();
                    _entity.Add(entity);
                    SetDetailRecNo();
                }
                else
                {
                    if (this.dg.ConfirmEditEnd() == false) return;

                    _selectedIndex = dg.SelectedIndex;
                    _displayIndex = this.dg.CurrentColumn.DisplayIndex;

                    _entity.Insert(_selectedIndex + 1, entity);
                    SetDetailRecNo();

                    dg.SelectedIndex = _selectedIndex;
                    this.dg.CurrentColumn = this.dg.Columns[_displayIndex];
                }
            }
            catch (Exception ex)
            {
                ExMessageBox.Show(CLASS_NM + ".btnF3_Click : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message);
            }
        }

        // F4ボタン(行削除) クリック
        public override void btnF4_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // 選択有りか、データ有りか確認
                if (this.dg.SelectedIndex < 0) return;
                if (this.dg.ItemsSource == null) return;
                if (_entity == null) return;
                if (_entity.Count == 0) return;
                if (_entity.Count == 1 && this.dg.SelectedIndex == _entity.Count - 1) return;

                int _selectedIndex = dg.SelectedIndex;
                DataGridColumn dgCol = this.dg.CurrentColumn;

                // 行削除
                _entity.RemoveAt(this.dg.SelectedIndex);
                SetDetailRecNo();

                // 行削除後の選択
                if (this.dg.SelectedIndex != 0)
                {
                    if (_selectedIndex <= _entity.Count - 1)
                    {
                        this.dg.SelectedIndex = this.dg.SelectedIndex;
                        this.dg.CurrentColumn = dgCol;
                    }
                    else
                    {
                        this.dg.SelectedIndex = this.dg.SelectedIndex - 1;
                        this.dg.CurrentColumn = dgCol;
                    }
                }

                // データ1件もない場合、デフォルト行の追加
                if (_entity.Count == 0) btnF3_Click(null, null);

            }
            catch (Exception ex)
            {
                ExMessageBox.Show(CLASS_NM + ".btnF4_Click : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message);
            }
        }

        // F5ボタン(参照) クリック
        public override void btnF5_Click(object sender, RoutedEventArgs e)
        {
        }
        
        // F11ボタン(印刷) クリック
        public override void btnF11_Click(object sender, RoutedEventArgs e)
        {
            Common.report.utlReport.gPageType = Common.gePageType.None;
            Common.report.utlReport.gWinMsterType = _WinMsterType;

            beforeWinGroupType = Common.gWinGroupType;
            Common.gWinGroupType = Common.geWinGroupType.Report;

            Common.report.Show();
            Common.report.Closed -= reportDlg_Closed;
            Common.report.Closed += reportDlg_Closed;
        }

        private void reportDlg_Closed(object sender, EventArgs e)
        {
            Common.report.Closed -= reportDlg_Closed;
            Common.gWinGroupType = beforeWinGroupType;
        }

        // F12ボタン(メニュー) クリック
        public override void btnF12_Click(object sender, RoutedEventArgs e)
        {
            UA_Main pg = (UA_Main)ExVisualTreeHelper.FindPerentPage(this.Parent);
            Dlg_InpMaster win = (Dlg_InpMaster)ExVisualTreeHelper.FindPerentChildWindow(this);
            win.Close();
        }

        #endregion

        #region GotFocus Events

        private void dg_GotFocus(object sender, RoutedEventArgs e)
        {
        }

        #endregion

        #region LostFocus Events

        #endregion

        #region DataGrid Events

        private void dg_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            //try
            //{
            //    beforeValue = "";
            //    EntityOrderD entity = (EntityOrderD)e.Row.DataContext;
            //    switch (e.Column.DisplayIndex)
            //    {
            //        case 1:         // 内訳
            //            beforeValue = entity.breakdown_nm;
            //            break;
            //        case 3:         // 商品コード
            //            beforeValue = entity.commodity_id;
            //            break;
            //        case 4:         // 商品名
            //            beforeValue = entity.commodity_name;
            //            break;
            //        case 5:         // 単位
            //            beforeValue = entity.unit_nm;
            //            break;
            //        case 6:         // 入数
            //            beforeValue = ExCast.zCStr(entity.enter_number);
            //            break;
            //        case 7:         // ケース数
            //            beforeValue = ExCast.zCStr(entity.case_number);
            //            break;
            //        case 8:         // 数量
            //            beforeValue = ExCast.zCStr(entity.number);
            //            break;
            //        case 9:         // 単価
            //            beforeValue = ExCast.zCStr(entity.unit_price);
            //            break;
            //        case 10:         // 金額
            //            beforeValue = ExCast.zCStr(entity.price);
            //            break;
            //        case 11:         // 課税区分
            //            beforeValue = ExCast.zCStr(entity.tax_division_nm);
            //            break;
            //        case 12:         // 備考
            //            beforeValue = ExCast.zCStr(entity.memo);
            //            break;
            //    }
            //}
            //catch
            //{
            //}
        }

        private void dg_CellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
        {
            EntityCondition entity = (EntityCondition)e.Row.DataContext;

            // コンボボックスID連動
            switch (e.Column.DisplayIndex)
            {
                case 2:         // 表示区分
                    if (_entity == null) return;
                    if (_entity.Count >= dg.SelectedIndex && dg.SelectedIndex != -1)
                        _entity[dg.SelectedIndex]._display_division_id = MeiNameList.GetID(MeiNameList.geNameKbn.DISPLAY_DIVISION_ID, ExCast.zCStr(entity._display_division_nm)) - 1;
                    break;
            }

        }

        private void dg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (objOrderListD == null) return;
            //if (objOrderListD.Count >= dg.SelectedIndex && dg.SelectedIndex != -1)
            //{
            //    if (ExCast.zCStr(objOrderListD[dg.SelectedIndex].commodity_id) != "")
            //    {
            //        txtInventory.Text = ExCast.zCStr(objOrderListD[dg.SelectedIndex].inventory_number);
            //    }
            //}
        }

        private void dg_LayoutUpdated(object sender, EventArgs e)
        {
            if (this.utlFunctionKey != null)
            {
                if (this.utlFunctionKey.gFunctionKeyEnable == Utl_FunctionKey.geFunctionKeyEnable.Sel)
                {
                    ExVisualTreeHelper.SetEnabled(this.dg, false);
                }
            }
        }

        #endregion

        #region Binding Setting

        private void SetBinding()
        {
        }

        #endregion

        #region Data Select

        #region データ取得

        private void GetMstData()
        {
            object[] prm = new object[1];
            prm[0] = "";
            webService.objPerent = this;
            webService.CallWebService(_GetWebServiceCallKbn,
                                      ExWebService.geDialogDisplayFlg.Yes,
                                      ExWebService.geDialogCloseFlg.Yes,
                                      prm);
        }

        #endregion

        #region データ取得コールバック呼出(ExWebServiceクラスより呼出)

        public override void DataSelect(int intKbn, object objList)
        {
            switch ((ExWebService.geWebServiceCallKbn)intKbn)
            {
                case _GetWebServiceCallKbn:
                    if (objList != null)
                    {
                        _entity = (ObservableCollection<EntityCondition>)objList;

                        EntityCondition entity = new EntityCondition();

                        // 明細初期設定
                        InitDetail(ref entity);

                        _entity.Add(entity);
                        SetDetailRecNo();

                        this.dg.ItemsSource = _entity;

                        if (ExCast.zCInt(_entity[0]._lock_flg) == 0)
                        {
                            this.utlFunctionKey.gFunctionKeyEnable = Utl_FunctionKey.geFunctionKeyEnable.Upd;
                            ExBackgroundWorker.DoWork_Focus(this.dg, 100);
                            ExBackgroundWorker.DoWork_SelectedIndex(this.dg, 400, 0);
                            ExBackgroundWorker.DoWork_DataGridColum(this.dg, 500, dg.Columns[0]);
                        }
                        else
                        {
                            this.utlFunctionKey.gFunctionKeyEnable = Utl_FunctionKey.geFunctionKeyEnable.Sel;
                        }

                    }
                    else
                    {
                        _entity = new ObservableCollection<EntityCondition>();

                        EntityCondition entity = new EntityCondition();

                        // 明細初期設定
                        InitDetail(ref entity);

                        _entity.Add(entity);
                        SetDetailRecNo();

                        this.dg.ItemsSource = _entity;

                        this.utlFunctionKey.gFunctionKeyEnable = Utl_FunctionKey.geFunctionKeyEnable.Upd;
                        ExBackgroundWorker.DoWork_Focus(this.dg, 100);
                        ExBackgroundWorker.DoWork_SelectedIndex(this.dg, 400, 0);
                        ExBackgroundWorker.DoWork_DataGridColum(this.dg, 500, dg.Columns[0]);
                    }

                    break;
                default:
                    break;
            }
        }

        #endregion

        #endregion

        #region Data Update

        // データ更新
        private void UpdateData()
        {
            object[] prm = new object[1];

            prm[0] = this._entity;
            webService.objPerent = this;
            webService.CallWebService(_UpdWebServiceCallKbn,
                                      ExWebService.geDialogDisplayFlg.Yes,
                                      ExWebService.geDialogCloseFlg.Yes,
                                      prm);
        }

        public override void DataUpdate(int intKbn, string errMessage)
        {
            if ((ExWebService.geWebServiceCallKbn)intKbn == _UpdWebServiceCallKbn)
            {
                if (string.IsNullOrEmpty(errMessage))
                {
                    btnF2_Click(null, null);
                }
            }
        }

        #endregion

        #region Input Check

        // 入力チェック(更新時)
        public override void InputCheckUpdate()
        {
            #region Field

            string errMessage = "";
            string warnMessage = "";
            Control errCtl = null;

            #endregion

            try
            {
                for (int i = 0; i <= _entity.Count - 1; i++)
                {
                    // IDまたは名称の入力がある場合(空行は無視)
                    if (!string.IsNullOrEmpty(_entity[i]._id) || !string.IsNullOrEmpty(_entity[i]._name))
                    {
                        #region 入力チェック

                        #region 必須チェック

                        // ID
                        if (string.IsNullOrEmpty(_entity[i]._id))
                        {
                            errMessage += (i + 1) + "行目のIDが入力されていません。" + Environment.NewLine;
                            if (errCtl == null)
                            {
                                errCtl = this.dg;
                                _selectIndex = i;
                                _selectColumn = 0;
                            }
                        }

                        // 名称
                        if (string.IsNullOrEmpty(_entity[i]._name))
                        {
                            errMessage += (i + 1) + "行目の名称が入力されていません。" + Environment.NewLine;
                            if (errCtl == null)
                            {
                                errCtl = this.dg;
                                _selectIndex = i;
                                _selectColumn = 1;
                            }
                        }

                        #endregion

                        #region 適正値入力チェック

                        if (!string.IsNullOrEmpty(_entity[i]._id))
                        {
                            if (ExCast.zCInt(_entity[i]._id) == 0)
                            {
                                errMessage += (i + 1) + "行目のIDに「0」以外を入力して下さい。" + Environment.NewLine;
                                if (errCtl == null)
                                {
                                    errCtl = this.dg;
                                    _selectIndex = i;
                                    _selectColumn = 0;
                                }
                            }
                        }

                        //// 主仕入先
                        //if (ExCast.zCStr(_entity._main_purchase_id) != "" && string.IsNullOrEmpty(_entity._main_purchase_nm))
                        //{
                        //    errMessage += "主仕入先が適切に入力(選択)されていません。" + Environment.NewLine;
                        //    if (errCtl == null) errCtl = this.utlMainPurchaseId.txtID;
                        //}

                        #endregion

                        #region 同一IDチェック

                        if (!string.IsNullOrEmpty(_entity[i]._id))
                        {
                            for (int _i = 0; _i <= _entity.Count - 1; _i++)
                            {
                                if (_i != i && ExCast.zCInt(_entity[i]._id) == ExCast.zCInt(_entity[_i]._id) && !string.IsNullOrEmpty(_entity[_i]._id) && ExCast.zCInt(_entity[i]._id) != 0)
                                {
                                    string _msg = "ID : " + ExCast.zCInt(_entity[i]._id) + " が重複して入力されています。" + Environment.NewLine;
                                    if (errMessage.IndexOf(_msg) == -1)
                                    {
                                        errMessage += _msg;
                                        if (errCtl == null)
                                        {
                                            errCtl = this.dg;
                                            _selectIndex = i;
                                            _selectColumn = 0;
                                        }
                                    }
                                }
                            }
                        }

                        #endregion

                        #region 日付チェック

                        //// 納入指定日
                        //if (string.IsNullOrEmpty(_entity.supply_ymd) == false)
                        //{
                        //    if (ExCast.IsDate(_entity.supply_ymd) == false)
                        //    {
                        //        errMessage += "納入指定日の形式が不正です。(yyyy/mm/dd形式で入力(選択)して下さい)" + Environment.NewLine;
                        //        if (errCtl == null) errCtl = this.datNokiYmd;
                        //    }
                        //}

                        #endregion

                        #region 日付変換

                        // 受注日
                        //if (string.IsNullOrEmpty(_entity.order_ymd) == false)
                        //{
                        //    _entity.order_ymd = ExCast.zConvertToDate(_entity.order_ymd).ToString("yyyy/MM/dd");

                        //}

                        #endregion

                        #region 数値チェック

                        // ID
                        if (!string.IsNullOrEmpty(_entity[i]._id))
                        {
                            if (!ExCast.IsNumeric(_entity[i]._id))
                            {
                                errMessage += (i + 1) + "行目のIDに数値が入力されていません。" + Environment.NewLine;
                                if (errCtl == null)
                                {
                                    errCtl = this.dg;
                                    _selectIndex = i;
                                    _selectColumn = 0;
                                }
                            }
                        }

                        #endregion

                        #region 正数チェック

                        //if (this.utlMode.Mode != Utl_FunctionKey.geFunctionKeyEnable.Init)
                        //{
                        //    if (ExCast.zCLng(this.utlID.txtID.Text.Trim()) < 0)
                        //    {
                        //        errMessage += "IDには正の整数を入力して下さい。" + Environment.NewLine;
                        //    }
                        //}

                        #endregion

                        #region 範囲チェック

                        //if (ExCast.zCLng(this.utlID.txtID.Text.Trim()) > 9999)
                        //{
                        //    errMessage += "IDには4桁の正の整数を入力して下さい。" + Environment.NewLine;
                        //}

                        //if (ExString.LenB(_entity._memo) > 32)
                        //{
                        //    errMessage += "備考には全角16桁文字以内(半角32桁文字以内)を入力して下さい。" + Environment.NewLine;
                        //    if (errCtl == null) errCtl = this.txtMemo;
                        //}

                        #endregion

                        #endregion

                        _entity[i]._display_division_id = MeiNameList.GetID(MeiNameList.geNameKbn.DISPLAY_DIVISION_ID, ExCast.zCStr(_entity[i]._display_division_nm)) - 1;
                    }
                }

                #region エラー or 警告時処理

                bool flg = true;

                if (!string.IsNullOrEmpty(errMessage))
                {
                    ExMessageBox.Show(this, errCtl, errMessage, Dlg.MessageBox.MessageBoxIcon.Error);
                    flg = false;
                }

                if (!string.IsNullOrEmpty(warnMessage))
                {
                    warnMessage += "このまま登録を続行してもよろしいですか？" + Environment.NewLine;
                    ExMessageBox.ResultShow(this, errCtl, warnMessage);
                    flg = false;
                    //if (ExMessageBox.ResultShow(warnMessage) == MessageBoxResult.No)
                    //{
                    //    flg = false;
                    //}
                }

                this.txtDummy.IsTabStop = false;

                if (flg == false)
                {
                    if (errCtl != null)
                    {
                        switch (errCtl.Name)
                        {
                            case "dg":
                                errCtl.Focus();
                                this.dg.SelectedIndex = _selectIndex;
                                dg.CurrentColumn = dg.Columns[_selectColumn];
                                ExBackgroundWorker.DoWork_Focus(errCtl, 10);
                                break;
                            default:
                                ExBackgroundWorker.DoWork_Focus(errCtl, 10);
                                break;
                        }
                    }
                    return;
                }

                #endregion

                #region 更新処理

                UpdateData();

                #endregion
            }
            finally
            {
                Common.gblnBtnProcLock = false;
                Common.gblnBtnDesynchronizeLock = false;
            }
        }

        public override void ResultMessageBox(Control _errCtl)
        {
            if (_errCtl != null)
            {
                switch (_errCtl.Name)
                {
                    case "dg":
                        ExBackgroundWorker.DoWork_DataGridSelectCell(dg, _selectIndex, _selectColumn);
                        break;
                    default:
                        ExBackgroundWorker.DoWork_Focus(_errCtl, 10);
                        break;
                }
            }
        }

        public override void ResultMessageBox(MessageBoxResult _MessageBoxResult, Control _errCtl)
        {
            if (_MessageBoxResult == MessageBoxResult.OK)
            {
                #region 更新処理

                UpdateData();

                #endregion
            }
            else
            {
                switch (_errCtl.Name)
                {
                    case "dg":
                        _errCtl.Focus();
                        this.dg.SelectedIndex = _selectIndex;
                        dg.CurrentColumn = dg.Columns[_selectColumn];
                        ExBackgroundWorker.DoWork_Focus(_errCtl, 10);
                        break;
                    default:
                        ExBackgroundWorker.DoWork_Focus(_errCtl, 10);
                        break;
                }
            }
        }

        #endregion

        #region InitDetail

        private void InitDetail(ref EntityCondition entity)
        {
            entity._condition_divition_id = 1;
            SetInitCombo(ref entity);
        }

        private void SetInitCombo(ref EntityCondition entity)
        {
            // コンボボックス初期選択
            List<string> lst;
            lst = MeiNameList.GetListMei(MeiNameList.geNameKbn.DISPLAY_DIVISION_ID);
            entity._display_division_nm = lst[1];
            entity._display_division_id = MeiNameList.GetID(MeiNameList.geNameKbn.BREAKDOWN_ID, lst[1]);
        }

        #endregion

        #region Set Detail RecNo

        private void SetDetailRecNo()
        {
            if (_entity == null) return;
            if (_entity.Count == 0) return;

            for (int i = 0; i <= _entity.Count - 1; i++)
            {
                _entity[i]._rec_no = i + 1;
            }
        }

        #endregion

    }

}
