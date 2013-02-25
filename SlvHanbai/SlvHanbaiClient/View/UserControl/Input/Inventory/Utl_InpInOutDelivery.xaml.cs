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
using SlvHanbaiClient.svcInOutDelivery;
using SlvHanbaiClient.svcMstData;
using SlvHanbaiClient.View.UserControl.Custom;
using SlvHanbaiClient.View.Dlg;

namespace SlvHanbaiClient.View.UserControl.Input.Inventory
{
    public partial class Utl_InpInOutDelivery : ExUserControl
    {

        #region Filed Const

        private const String CLASS_NM = "Utl_InpInOutDelivery";
        private const String PG_NM = DataPgEvidence.PGName.InOutDeliver.InOutDeliverInp;
        private Common.gePageType _PageType = Common.gePageType.InpInOutDelivery;

        private Utl_FunctionKey utlFunctionKey = null;

        private const ExWebService.geWebServiceCallKbn _GetHeadWebServiceCallKbn = ExWebService.geWebServiceCallKbn.GetInOutDeliveryListH;
        private const ExWebService.geWebServiceCallKbn _GetDetailWebServiceCallKbn = ExWebService.geWebServiceCallKbn.GetInOutDeliveryListD;
        private const ExWebService.geWebServiceCallKbn _UpdWebServiceCallKbn = ExWebService.geWebServiceCallKbn.UpdateInOutDelivery;

        private EntityInOutDeliveryH _entityH = new EntityInOutDeliveryH();
        private ObservableCollection<EntityInOutDeliveryD> _entityListD = new ObservableCollection<EntityInOutDeliveryD>();

        private EntityInOutDeliveryH _before_entityH = new EntityInOutDeliveryH();
        private ObservableCollection<EntityInOutDeliveryD> _before_entityListD = new ObservableCollection<EntityInOutDeliveryD>();

        private readonly string tableName = "T_IN_OUT_DELIVERY_H";
        private Control activeControl;

        private Common.geWinType WinType = Common.geWinType.ListInOutDelivery;

        private Dlg_DataForm dataForm;
        private Common.geDataFormType DataFormType = Common.geDataFormType.InOutDeliveryDetail;

        private Dlg_MstSearch masterDlg;
        private Dlg_Copying copyDlg;
        private Common.geWinGroupType beforeWinGroupType;

        private string beforeValue = "";                // DataGrid編集チェック用
        private int beforeSelectedIndex = -1;           // DataGrid編集前 行
        private int _selectIndex = 0;                   // データグリッド現在行保持用
        private int _selectColumn = 0;                  // データグリッド現在列保持用

        #endregion

        #region Constructor

        public Utl_InpInOutDelivery()
        {
            InitializeComponent();
            this.Tag = "Main";      // ファンクションキーを受けつけ用
        }

        #endregion

        #region Page Events

        private void ExUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (Common.gblnLogin == false) return;

            this.utlNo._ExUserControl_LostFocus -= this.utlNo_LostFocus;
            this.utlNo._ExUserControl_LostFocus += this.utlNo_LostFocus;

            this.utlInOutDeliveryKbn.ID_TextChanged -= this.InOutDeliveryToKbn_TextChanged;
            this.utlInOutDeliveryKbn.ID_TextChanged += this.InOutDeliveryToKbn_TextChanged;
            this.utlInOutDeliveryToKbn.ID_TextChanged -= this.InOutDeliveryToKbn_TextChanged;
            this.utlInOutDeliveryToKbn.ID_TextChanged += this.InOutDeliveryToKbn_TextChanged;

            // 画面初期化
            ExVisualTreeHelper.initDisplay(this.LayoutRoot);

            // ファンクションキー初期設定
            this.utlFunctionKey = ExVisualTreeHelper.GetUtlFunctionKey(this.LayoutRoot);
            this.utlFunctionKey.gFunctionKeyEnable = Utl_FunctionKey.geFunctionKeyEnable.Init;

            // バインド設定
            SetBinding();

            // 明細追加
            this.RecordAdd();

            dg.ItemsSource = _entityListD;

            ExBackgroundWorker.DoWork_Focus(this.utlNo.txtID, 10);
        }

        private void ExUserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            this.utlNo._ExUserControl_LostFocus -= this.utlNo_LostFocus;
        }

        #endregion

        #region Function Key Button Events

        // F1ボタン(登録) クリック
        public override void btnF1_Click(object sender, RoutedEventArgs e)
        {
            ExDataGridUtilty.zCommitEdit(this.dg);
            if (Common.gblnDesynchronizeLock == true) return;

            // ボタン押下時非同期入力チェックON
            Common.gblnBtnDesynchronizeLock = true;

            // 入力確定の為、BackgroundWorker経由で入力チェックを呼出
            ExBackgroundInputCheckWk bk = new ExBackgroundInputCheckWk();
            bk.utl = this;
            bk.waitTime = 500;
            this.txtDummy.IsTabStop = true;
            bk.focusCtl = this.txtDummy;
            bk.bw.RunWorkerAsync();
        }

        // F2ボタン(クリア) クリック
        public override void btnF2_Click(object sender, RoutedEventArgs e)
        {
            this.utlFunctionKey.gFunctionKeyEnable = Utl_FunctionKey.geFunctionKeyEnable.Init;

            // 先頭選択
            this.dg.SelectedFirst();

            // ヘッダ初期化
            _entityH = null;
            SetBinding();

            // 明細初期化
            _entityListD = null;
            _entityListD = new ObservableCollection<EntityInOutDeliveryD>();

            // 明細追加
            Common.gblnDesynchronizeLock = false;
            this.btnF7_Click(null, null);

            this.dg.ItemsSource = _entityListD;

            this.utlFunctionKey.gFunctionKeyEnable = Utl_FunctionKey.geFunctionKeyEnable.Init;

            this.utlNo.txtID_IsReadOnly = false;
            //this.utlNo.IsEnabled = true;
            this.utlNo.txtID.Text = "";
            ExBackgroundWorker.DoWork_Focus(this.utlNo.txtID, 10);

            // ロック解除
            DataPgLock.gLockPg(PG_NM, "", (int)DataPgLock.geLockType.UnLock);
        }

        // F3ボタン(複写) クリック
        public override void btnF3_Click(object sender, RoutedEventArgs e)
        {
            copyDlg = new Dlg_Copying();

            copyDlg.utlCopying.gPageType = _PageType;
            copyDlg.utlCopying.gWinMsterType = Common.geWinMsterType.None;
            copyDlg.utlCopying.utlMstID.MstKbn = Class.Data.MstData.geMDataKbn.None;
            copyDlg.utlCopying.tableName = this.tableName;

            copyDlg.Show();
            copyDlg.Closed += copyDlg_Closed;
        }

        private void copyDlg_Closed(object sender, EventArgs e)
        {
            Dlg_Copying dlg = (Dlg_Copying)sender;
            if (dlg.utlCopying.DialogResult == true)
            {
                // ロック解除
                DataPgLock.gLockPg(PG_NM, ExCast.zCStr(_entityH._id), (int)DataPgLock.geLockType.UnLock);

                // 引き継いではいけない情報は初期化する
                _entityH._lock_flg = 0;

                if (dlg.utlCopying.copy_id == "")
                {
                    this.utlFunctionKey.gFunctionKeyEnable = Utl_FunctionKey.geFunctionKeyEnable.Init;
                    this.utlNo.txtID_IsReadOnly = false;
                    this.utlNo.txtID.Text = "";
                }
                else
                {
                    if (dlg.utlCopying.ExistsData == true)
                    {
                        this.utlFunctionKey.gFunctionKeyEnable = Utl_FunctionKey.geFunctionKeyEnable.Upd;
                    }
                    else
                    {
                        this.utlFunctionKey.gFunctionKeyEnable = Utl_FunctionKey.geFunctionKeyEnable.New;
                    }
                    string _id = dlg.utlCopying.copy_id;
                    if (ExCast.IsNumeric(dlg.utlCopying.copy_id))
                    {
                        _id = ExCast.zCDbl(_id).ToString();
                        _entityH._no = ExCast.zCLng(_id);
                        this.utlNo.txtID.Text = _id;
                        this.utlNo.txtID.FormatToID();
                    }
                    else
                    {
                        _entityH._no = ExCast.zCLng(_id);
                        this.utlNo.txtID.Text = _id;
                    }

                    this.utlNo.txtID_IsReadOnly = true;
                    ExBackgroundWorker.DoWork_Focus(this.datInOutDeliveryYmd, 10);

                    // 前回情報保持
                    ConvertBeforeData(_entityH, _entityListD, _before_entityH, _before_entityListD);
                }
            }
        }

        // F4ボタン(削除) クリック
        public override void btnF4_Click(object sender, RoutedEventArgs e)
        {
            // ボタン押下時非同期入力チェックON
            Common.gblnBtnDesynchronizeLock = true;

            InputCheckDelete();
        }
        
        // F5ボタン(参照) クリック
        public override void btnF5_Click(object sender, RoutedEventArgs e)
        {
            if (activeControl == null) return;
            switch (activeControl.Name)
            {
                case "utlNo":
                    Utl_InpNoText inp = (Utl_InpNoText)activeControl;
                    inp.ShowList();
                    break;
                case "datInOutDeliveryYmd":
                    ExDatePicker dt = (ExDatePicker)activeControl;
                    dt.ShowCalender();
                    break;
                case "utlInOutDeliveryToKbn":
                    Utl_MeiText mei = (Utl_MeiText)activeControl;
                    mei.ShowList();
                    break;
                case "utlPerson":
                case "utlCompanyGroup":
                case "utlCustomer":
                case "utlPurchase":
                    Utl_MstText mst = (Utl_MstText)activeControl;
                    mst.ShowList();
                    break;
                case "DetailGoods":
                    txtGoodsID_MouseDoubleClick(null, null);
                    break;
                default:
                    break;
            }
        }

        // F6ボタン(明細画面) クリック
        public override void btnF6_Click(object sender, RoutedEventArgs e)
        {
            ExDataGridUtilty.zCommitEdit(this.dg);
            if (Common.gblnDesynchronizeLock == true) return;

            Common.gDataFormType = DataFormType;
            Common.dataForm._entityH = this._entityH;
            Common.dataForm._entityListD = this._entityListD;
            Common.dataForm.Closed -= dlg_Closed;
            Common.dataForm.Closed += dlg_Closed;
            Common.dataForm.Show();
        }

        private void dlg_Closed(object sender, EventArgs e)
        {
            Common.dataForm.Closed -= dlg_Closed;

            // 明細合計計算
            DataDetail.CalcSumDetail(this._entityH, this._entityListD);

            this.dg.Focus();
        }

        // F7ボタン(明細追加) クリック
        public override void btnF7_Click(object sender, RoutedEventArgs e)
        {
            ExDataGridUtilty.zCommitEdit(this.dg);
            if (Common.gblnDesynchronizeLock == true) return;

            // 入力確定の為、BackgroundWorker経由で行挿入を呼出
            ExBackgroundRecordAddWk bk = new ExBackgroundRecordAddWk();
            bk.utl = this;
            this.txtDummy.IsTabStop = true;
            if (sender != null) bk.waitFlg = true;
            bk.focusCtl = this.txtDummy;
            bk.bw.RunWorkerAsync();
        }

        public override void RecordAdd()
        {
            if (Common.gblnDesynchronizeLock == true) return;

            if (_entityListD == null) _entityListD = new ObservableCollection<EntityInOutDeliveryD>();

            EntityInOutDeliveryD entity = new EntityInOutDeliveryD();
            int cnt = 1;
            if (_entityListD != null) cnt = _entityListD.Count + 1;
            entity._rec_no = cnt;
            SetInitCombo(ref entity);   // コンボボックス初期選択
            _entityListD.Add(entity);
            dg.SelectedIndex = ExCast.zCInt(entity._rec_no) - 1;
            dg.Focus();
            if (dg.CurrentColumn != null)
            {
                dg.ScrollIntoView(entity, dg.Columns[0]);
                dg.CommitEdit();
                dg.CurrentColumn = dg.Columns[0];
                dg.BeginEdit();
                dg.MoveNextCell();
            }

            // ファンクションキー設定
            string _activeCtlName = "";
            if (this.activeControl != null) _activeCtlName = ExCast.zCStr(this.activeControl.Name);
            switch (_activeCtlName)
            {
                case "txtGoodsName":
                case "cboUnit":
                case "txtEnterNum":
                case "txtNumber":
                case "txtCaseNum":
                case "txtGoodsID":
                    if (this._entityListD.Count > 1)
                    {
                        ExVisualTreeHelper.SetFunctionKeyEnabled("F8", true, this);
                    }
                    else
                    {
                        ExVisualTreeHelper.SetFunctionKeyEnabled("F8", false, this);
                    }
                    break;
            }
        }

        // F8ボタン(明細削除) クリック
        public override void btnF8_Click(object sender, RoutedEventArgs e)
        {
            ExDataGridUtilty.zCommitEdit(this.dg);
            if (Common.gblnDesynchronizeLock == true) return;

            // 選択有りか、データ有りか確認
            if (this.dg.SelectedIndex < 0) return;
            if (this.dg.ItemsSource == null) return;
            if (_entityListD == null) return;
            if (_entityListD.Count == 0) return;

            // 行削除
            _entityListD.RemoveAt(this.dg.SelectedIndex);

            // 行削除後の選択
            if (this.dg.SelectedIndex != 0) this.dg.SelectedIndex = this.dg.SelectedIndex - 1;

            // データ1件もない場合、デフォルト行の追加
            if (_entityListD.Count == 0)
            {
                this.btnF7_Click(null, null);
            }

            // 行番号振り直し
            for (int i = 1; i <= _entityListD.Count; i++)
            {
                _entityListD[i - 1]._rec_no = i;
            }

            // 明細合計計算
            DataDetail.CalcSumDetail(this._entityH, this._entityListD);
        }

        // F11ボタン(印刷) クリック
        public override void btnF11_Click(object sender, RoutedEventArgs e)
        {
            beforeWinGroupType = Common.gWinGroupType;
            Common.gWinMsterType = Common.geWinMsterType.None;
            Common.gWinGroupType = Common.geWinGroupType.InpListReport;
            Common.gWinType = this.WinType;


            Common.InpSearchOrder.Closed -= reportDlg_Closed;
            Common.InpSearchOrder.Closed += reportDlg_Closed;
            Common.InpSearchOrder.Show();
        }

        private void reportDlg_Closed(object sender, EventArgs e)
        {
            Common.InpSearchOrder.Closed -= reportDlg_Closed;
            Common.gWinGroupType = beforeWinGroupType;
        }

        // F12ボタン(メニュー) クリック
        public override void btnF12_Click(object sender, RoutedEventArgs e)
        {
            UA_Main pg = (UA_Main)ExVisualTreeHelper.FindPerentPage(this.Parent);
            Common.gPageGroupType = Common.gePageGroupType.Menu;
            Common.gPageType = Common.gePageType.Menu;
            pg.ChangePage();
        }

        #endregion

        #region Button Click Events

        private void btnInventory_Click(object sender, RoutedEventArgs e)
        {
            if (ExCast.zCStr(_entityListD[dg.SelectedIndex]._commodity_id) != "" && ExCast.zCInt(_entityListD[dg.SelectedIndex]._inventory_management_division_id) == 1)
            {
            }
            else
            {
                ExMessageBox.Show("商品を選択して下さい。");
                return;
            }

            Dlg_MstSearch searchDlg = new Dlg_MstSearch(MstData.geMDataKbn.Inventory);
            searchDlg.MstKbn = Class.Data.MstData.geMDataKbn.Inventory;
            searchDlg.MstGroupKbn = Class.Data.MstData.geMGroupKbn.None;
            searchDlg.txtCode.Text = ExCast.zNumZeroNothingFormat(ExCast.zCStr(_entityListD[dg.SelectedIndex]._commodity_id));
            searchDlg.Show();
        }

        #endregion

        #region GotFocus Events

        private void txt_GotFocus(object sender, RoutedEventArgs e)
        {
            Control ctl = (Control)sender;
            ExTextBox txt = null;

            switch (ctl.Name)
            {
                #region Header

                case "utlNo":
                case "datInOutDeliveryYmd":
                case "utlPerson":
                case "utlCustomer":
                case "utlSupplier":
                    ExVisualTreeHelper.SetFunctionKeyEnabled("F5", true, this);
                    ExVisualTreeHelper.SetFunctionKeyEnabled("F8", false, this);
                    activeControl = ctl;
                    break;
                case "txtLastUpdYmd":
                case "txtMemo":
                    ExVisualTreeHelper.SetFunctionKeyEnabled("F5", false, this);
                    ExVisualTreeHelper.SetFunctionKeyEnabled("F8", false, this);
                    activeControl = null;
                    break;

                #endregion

                #region Detail

                case "txtGoodsName":
                case "cboUnit":
                case "txtEnterNum":
                case "txtNumber":
                case "txtCaseNum":
                case "txtDetailMemo":
                    ExVisualTreeHelper.SetFunctionKeyEnabled("F5", false, this);
                    if (this._entityListD.Count > 1)
                    {
                        ExVisualTreeHelper.SetFunctionKeyEnabled("F8", true, this);
                    }
                    else
                    {
                        ExVisualTreeHelper.SetFunctionKeyEnabled("F8", false, this);
                    }
                    activeControl = ctl;
                    break;
                case "txtGoodsID":
                    ExVisualTreeHelper.SetFunctionKeyEnabled("F5", true, this);
                    if (this._entityListD.Count > 1)
                    {
                        ExVisualTreeHelper.SetFunctionKeyEnabled("F8", true, this);
                    }
                    else
                    {
                        ExVisualTreeHelper.SetFunctionKeyEnabled("F8", false, this);
                    }
                    txt = new ExTextBox();
                    txt.Name = "DetailGoods";
                    activeControl = txt;
                    break;

                #endregion

                default:
                    break;
            }
        }

        #endregion

        #region LostFocus Events

        private void utlNo_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.utlNo.txtID.Text.Trim()))
            {
                if (this._entityH != null) this._entityH._no = 0;
                return;
            }

            if (this.utlNo.txtID.Text.Trim() != "")
            {
                GetHeadData(ExCast.zCLng(this.utlNo.txtID.Text.Trim()));
            }
        }
        
        #endregion

        #region Text Changed

        private void InOutDeliveryToKbn_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ExCast.zCInt(this.utlInOutDeliveryKbn.txtID.Text.Trim()) == 0)
            {
                this.lblSupply.Visibility = System.Windows.Visibility.Collapsed;
                this.utlCompanyGroup.Visibility = System.Windows.Visibility.Collapsed;
                this.utlCustomer.Visibility = System.Windows.Visibility.Collapsed;
                this.utlPurchase.Visibility = System.Windows.Visibility.Collapsed;
                return;
            }

            switch (ExCast.zCInt(this.utlInOutDeliveryToKbn.txtID.Text.Trim()))
            {
                case 1: // グループ
                    this.lblSupply.Visibility = System.Windows.Visibility.Visible;
                    this.utlCompanyGroup.Visibility = System.Windows.Visibility.Visible;
                    this.utlCustomer.Visibility = System.Windows.Visibility.Collapsed;
                    this.utlPurchase.Visibility = System.Windows.Visibility.Collapsed;

                    if (ExCast.zCInt(this.utlInOutDeliveryKbn.txtID.Text.Trim()) == 1)
                    {
                        this.lblSupply.Content = "入庫先" + Common.gstrGroupDisplayNm;
                    }
                    else
                    {
                        this.lblSupply.Content = "出庫先" + Common.gstrGroupDisplayNm;
                    }
                    break;
                case 2: // 得意先/仕入先
                    this.lblSupply.Visibility = System.Windows.Visibility.Visible;
                    if (ExCast.zCInt(this.utlInOutDeliveryKbn.txtID.Text.Trim()) == 1)
                    {
                        // 入庫
                        this.lblSupply.Content = "入庫先(仕入先)";
                        this.utlCompanyGroup.Visibility = System.Windows.Visibility.Collapsed;
                        this.utlCustomer.Visibility = System.Windows.Visibility.Collapsed;
                        this.utlPurchase.Visibility = System.Windows.Visibility.Visible;
                    }
                    else
                    {
                        // 出庫
                        this.lblSupply.Content = "出庫先(得意先)";
                        this.utlCompanyGroup.Visibility = System.Windows.Visibility.Collapsed;
                        this.utlCustomer.Visibility = System.Windows.Visibility.Visible;
                        this.utlPurchase.Visibility = System.Windows.Visibility.Collapsed;
                    }
                    break;
                default:
                    this.lblSupply.Visibility = System.Windows.Visibility.Collapsed;
                    this.utlCompanyGroup.Visibility = System.Windows.Visibility.Collapsed;
                    this.utlCustomer.Visibility = System.Windows.Visibility.Collapsed;
                    this.utlPurchase.Visibility = System.Windows.Visibility.Collapsed;
                    break;
            }
        }

        #endregion

        #region Binding Setting

        private void SetBinding()
        {
            if (_entityH == null)
            {
                _entityH = new EntityInOutDeliveryH();
            }

            if (_entityListD == null)
            {
                _entityListD = new ObservableCollection<EntityInOutDeliveryD>();
            }

            // マスタコントロールPropertyChanged
            _entityH.PropertyChanged += this.utlCompanyGroup.MstID_Changed;
            _entityH.PropertyChanged += this.utlCustomer.MstID_Changed;
            _entityH.PropertyChanged += this.utlPurchase.MstID_Changed;
            _entityH.PropertyChanged += this.utlPerson.MstID_Changed;
            _entityH.PropertyChanged += this._PropertyChanged;
            this.utlCustomer.ParentData = _entityH;
            this.utlPurchase.ParentData = _entityH;
            this.utlPerson.ParentData = _entityH;

            NumberConverter nmConvDecm0 = new NumberConverter();
            NumberConverter nmConvDecm2 = new NumberConverter();
            nmConvDecm2.DecimalPlaces = 2;

            #region Bind

            // バインド
            Binding BindingInOutDeliveryYmd = new Binding("_in_out_delivery_ymd");
            BindingInOutDeliveryYmd.Mode = BindingMode.TwoWay;
            BindingInOutDeliveryYmd.Source = _entityH;
            this.datInOutDeliveryYmd.SetBinding(DatePicker.SelectedDateProperty, BindingInOutDeliveryYmd);

            if (string.IsNullOrEmpty(_entityH._in_out_delivery_ymd))
            {
                _entityH._in_out_delivery_ymd = DateTime.Now.ToString("yyyy/MM/dd");
            }
            
            Binding BindingInpPersonID = new Binding("_update_person_id");
            BindingInpPersonID.Mode = BindingMode.TwoWay;
            BindingInpPersonID.Source = _entityH;
            this.utlPerson.txtID.SetBinding(TextBox.TextProperty, BindingInpPersonID);

            Binding BindingInpPersonName = new Binding("_update_person_nm");
            BindingInpPersonName.Mode = BindingMode.TwoWay;
            BindingInpPersonName.Source = _entityH;
            this.utlPerson.txtNm.SetBinding(TextBox.TextProperty, BindingInpPersonName);

            if (_entityH._update_person_id == 0)
            {
                // デフォルト担当の設定
                this.utlPerson.txtID.Text = Common.gintDefaultPersonId.ToString();
                _entityH._update_person_id = Common.gintDefaultPersonId;
                this.utlPerson.MstID_Changed(null, new PropertyChangedEventArgs("_update_person_id"));
            }

            Binding BindingInOutDeliveryKbnId = new Binding("_in_out_delivery_kbn");
            BindingInOutDeliveryKbnId.Mode = BindingMode.TwoWay;
            BindingInOutDeliveryKbnId.Source = _entityH;
            this.utlInOutDeliveryKbn.txtID.SetBinding(TextBox.TextProperty, BindingInOutDeliveryKbnId);

            Binding BindingInOutDeliveryKbnName = new Binding("_in_out_delivery_kbn_nm");
            BindingInOutDeliveryKbnName.Mode = BindingMode.TwoWay;
            BindingInOutDeliveryKbnName.Source = _entityH;
            this.utlInOutDeliveryKbn.txtNm.SetBinding(TextBox.TextProperty, BindingInOutDeliveryKbnName);

            Binding BindingInOutDeliveryToKbnId = new Binding("_in_out_delivery_to_kbn");
            BindingInOutDeliveryToKbnId.Mode = BindingMode.TwoWay;
            BindingInOutDeliveryToKbnId.Source = _entityH;
            this.utlInOutDeliveryToKbn.txtID.SetBinding(TextBox.TextProperty, BindingInOutDeliveryToKbnId);

            Binding BindingInOutDeliveryToKbnName = new Binding("_in_out_delivery_to_kbn_nm");
            BindingInOutDeliveryToKbnName.Mode = BindingMode.TwoWay;
            BindingInOutDeliveryToKbnName.Source = _entityH;
            this.utlInOutDeliveryToKbn.txtNm.SetBinding(TextBox.TextProperty, BindingInOutDeliveryToKbnName);

            Binding BindingCompanyGroupId = new Binding("_group_id_to");
            BindingCompanyGroupId.Mode = BindingMode.TwoWay;
            BindingCompanyGroupId.Source = _entityH;
            this.utlCompanyGroup.txtID.SetBinding(TextBox.TextProperty, BindingCompanyGroupId);

            Binding BindingCompanyGroupName = new Binding("_group_id_to_nm");
            BindingCompanyGroupName.Mode = BindingMode.TwoWay;
            BindingCompanyGroupName.Source = _entityH;
            this.utlCompanyGroup.txtNm.SetBinding(TextBox.TextProperty, BindingCompanyGroupName);
            
            Binding BindingCustomeNo = new Binding("_customer_id");
            BindingCustomeNo.Mode = BindingMode.TwoWay;
            BindingCustomeNo.Source = _entityH;
            this.utlCustomer.txtID.SetBinding(TextBox.TextProperty, BindingCustomeNo);

            Binding BindingCustomeName = new Binding("_customer_name");
            BindingCustomeName.Mode = BindingMode.TwoWay;
            BindingCustomeName.Source = _entityH;
            this.utlCustomer.txtNm.SetBinding(TextBox.TextProperty, BindingCustomeName);

            Binding BindingPurchaseId = new Binding("_purchase_id");
            BindingPurchaseId.Mode = BindingMode.TwoWay;
            BindingPurchaseId.Source = _entityH;
            this.utlPurchase.txtID.SetBinding(TextBox.TextProperty, BindingPurchaseId);

            Binding BindingPurchaseName = new Binding("_purchase_name");
            BindingPurchaseName.Mode = BindingMode.TwoWay;
            BindingPurchaseName.Source = _entityH;
            this.utlPurchase.txtNm.SetBinding(TextBox.TextProperty, BindingPurchaseName);

            Binding BindingMemo = new Binding("_memo");
            BindingMemo.Mode = BindingMode.TwoWay;
            BindingMemo.Source = _entityH;
            this.txtMemo.SetBinding(TextBox.TextProperty, BindingMemo);

            // 入数計
            Binding BindingEnterNumber = new Binding("_sum_enter_number");
            BindingEnterNumber.Mode = BindingMode.TwoWay;
            BindingEnterNumber.Source = _entityH;
            BindingEnterNumber.Converter = nmConvDecm0;
            this.txtEnterNumber.SetBinding(TextBox.TextProperty, BindingEnterNumber);
            
            // ケース数計
            Binding BindingCaseNumber = new Binding("_sum_case_number");
            BindingCaseNumber.Mode = BindingMode.TwoWay;
            BindingCaseNumber.Source = _entityH;
            BindingCaseNumber.Converter = nmConvDecm0;
            this.txtCaseNumber.SetBinding(TextBox.TextProperty, BindingCaseNumber);
            
            // 数量計
            Binding BindingNumber = new Binding("_sum_number");
            BindingNumber.Mode = BindingMode.TwoWay;
            BindingNumber.Source = _entityH;
            BindingNumber.Converter = nmConvDecm2;
            this.txtNumber.SetBinding(TextBox.TextProperty, BindingNumber);
            
            #endregion

            this.utlCustomer.txtID.SetZeroToNullString();
            this.utlPurchase.txtID.SetZeroToNullString();
            this.utlCompanyGroup.txtID.SetZeroToNullString();
            this.utlPerson.txtID.SetZeroToNullString();
        }

        #endregion

        #region Data Select

        #region ヘッダデータ取得

        // ヘッダデータ取得
        private void GetHeadData(long _No)
        {
            object[] prm = new object[2];
            prm[0] = _No.ToString();
            prm[1] = _No.ToString();
            webService.objPerent = this;
            webService.CallWebService(_GetHeadWebServiceCallKbn,
                                      ExWebService.geDialogDisplayFlg.Yes,
                                      ExWebService.geDialogCloseFlg.No,
                                      prm);
        }

        #endregion

        #region 明細データ取得

        // 明細データ取得
        private void GetDetailData(long _No)
        {
            object[] prm = new object[1];
            prm[0] = _No.ToString();
            webService.objPerent = this;
            webService.CallWebService(_GetDetailWebServiceCallKbn,
                                      ExWebService.geDialogDisplayFlg.No,
                                      ExWebService.geDialogCloseFlg.Yes,
                                      prm);
        }

        #endregion

        #region データ取得コールバック呼出(ExWebServiceクラスより呼出)

        // データ取得コールバック呼出
        public override void DataSelect(int intKbn, object objList)
        {
            switch ((ExWebService.geWebServiceCallKbn)intKbn)
            {
                #region 入出庫

                // 入出庫ヘッダ
                case _GetHeadWebServiceCallKbn:
                    // 更新
                    if (objList != null)    
                    {
                        _entityH = (EntityInOutDeliveryH)objList;

                        // エラー発生時
                        if (_entityH._message != "" && _entityH._message != null)
                        {
                            webService.ProcessingDlgClose();
                            this.utlNo.txtID.Text = "";
                            return;
                        }

                        switch (_entityH._in_out_delivery_proc_kbn)
                        {
                            case 2:
                                webService.ProcessingDlgClose();
                                ExMessageBox.Show("売上計上分の為、修正できません。");
                                this.utlNo.txtID.Text = "";
                                ExBackgroundWorker.DoWork_Focus(this.utlNo, 10);
                                return;
                            case 3:
                                webService.ProcessingDlgClose();
                                ExMessageBox.Show("仕入計上分の為、修正できません。");
                                this.utlNo.txtID.Text = "";
                                ExBackgroundWorker.DoWork_Focus(this.utlNo, 10);
                                return;
                        }
                        
                        // バインド反映
                        SetBinding();

                        // 明細データ取得
                        GetDetailData(_entityH._id);
                    }
                    // 新規
                    else
                    {
                        //// 明細追加
                        //this.btnF7_Click(null, null);

                        //this.dg.ItemsSource = _entityListD;
                        //webService.ProcessingDlgClose();
                        //this.utlFunctionKey.gFunctionKeyEnable = Utl_FunctionKey.geFunctionKeyEnable.New;
                        //this.utlNo.txtID_IsReadOnly = true;

                        webService.ProcessingDlgClose();
                        this.utlFunctionKey.gFunctionKeyEnable = Utl_FunctionKey.geFunctionKeyEnable.New;
                        this.utlNo.txtID_IsReadOnly = false;
                    }
                    ExBackgroundWorker.DoWork_Focus(this.datInOutDeliveryYmd, 10);
                    break;
                // 入出庫明細
                case _GetDetailWebServiceCallKbn:
                    // 2回設定がかかりエラーになる為
                    try
                    {
                        this.dg.ItemsSource = null;
                    }
                    catch
                    {
                        return;
                    }

                    if (objList != null)
                    {
                        _entityListD = (ObservableCollection<EntityInOutDeliveryD>)objList;
                    }
                    else 
                    {
                        _entityListD = null;
                    }

                    // 明細追加
                    this.btnF7_Click(null, null);

                    // 明細再計算
                    DataDetail.IsCalcPrice = false;
                    DataDetail.ReCalcDetail(_entityH, _entityListD);
                    DataDetail.IsCalcPrice = true;

                    this.dg.ItemsSource = _entityListD;

                    // 前回情報保持
                    ConvertBeforeData(_entityH, _entityListD, _before_entityH, _before_entityListD);

                    if (_entityH._lock_flg == 0)
                    {
                        this.utlFunctionKey.gFunctionKeyEnable = Utl_FunctionKey.geFunctionKeyEnable.Upd;
                    }
                    else
                    {
                        this.utlFunctionKey.gFunctionKeyEnable = Utl_FunctionKey.geFunctionKeyEnable.Sel;
                    }

                    ExBackgroundWorker.DoWork_Focus(this.datInOutDeliveryYmd, 10);
                    this.utlNo.txtID_IsReadOnly = true;
                    //this.utlNo.IsEnabled = false;

                    break;

                #endregion

                default:
                    break;
            }
        }

        #endregion

        #endregion

        #region Data Update

        // データ追加・更新・削除
        private void UpdateData(Common.geUpdateType upd)
        {
            object[] prm = new object[6];

            prm[0] = (int)upd;
            prm[1] = ExCast.zCLng(this.utlNo.txtID.Text.Trim());
            prm[2] = _entityH;
            prm[3] = _entityListD;
            prm[4] = _before_entityH;
            prm[5] = _before_entityListD;
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

        // 入力チェック
        public override void InputCheckUpdate()
        {
            #region Field

            string errMessage = "";
            string warnMessage = "";
            int _selectIndex = 0;
            int _selectColumn = 0;
            bool IsDetailExists = false;
            Control errCtl = null;
            List<string> list_warn_commodity = new List<string>();

            #endregion

            try
            {
                #region 排他処理

                if (Common.gblnDesynchronizeLock == true)
                {
                    errMessage += "現在排他処理中です。再度実行してください。" + Environment.NewLine;
                }

                #endregion

                #region ヘッダチェック

                #region 入出庫先セット

                switch (ExCast.zCInt(this.utlInOutDeliveryKbn.txtID.Text.Trim()))
                {
                    case 1:     // 入庫
                        switch (ExCast.zCInt(this.utlInOutDeliveryToKbn.txtID.Text.Trim()))
                        {
                            case 1: // グループ
                                this.utlCustomer.txtID.Text = "";
                                this.utlPurchase.txtID.Text = "";
                                _entityH._customer_id = "";
                                _entityH._purchase_id = "";
                                break;
                            case 2: // 仕入先
                                this.utlCompanyGroup.txtID.Text = "";
                                this.utlCustomer.txtID.Text = "";
                                _entityH._group_id_to = "";
                                _entityH._customer_id = "";
                                break;
                            default:
                                this.utlCompanyGroup.txtID.Text = "";
                                this.utlCustomer.txtID.Text = "";
                                this.utlPurchase.txtID.Text = "";
                                _entityH._group_id_to = "";
                                _entityH._customer_id = "";
                                _entityH._purchase_id = "";
                                break;
                        }
                        break;
                    case 2:     // 出庫
                        switch (ExCast.zCInt(this.utlInOutDeliveryToKbn.txtID.Text.Trim()))
                        {
                            case 1: // グループ
                                this.utlCustomer.txtID.Text = "";
                                this.utlPurchase.txtID.Text = "";
                                _entityH._customer_id = "";
                                _entityH._purchase_id = "";
                                break;
                            case 2: // 得意先
                                this.utlCompanyGroup.txtID.Text = "";
                                this.utlPurchase.txtID.Text = "";
                                _entityH._group_id_to = "";
                                _entityH._purchase_id = "";
                                break;
                            default:
                                this.utlCompanyGroup.txtID.Text = "";
                                this.utlCustomer.txtID.Text = "";
                                this.utlPurchase.txtID.Text = "";
                                _entityH._group_id_to = "";
                                _entityH._customer_id = "";
                                _entityH._purchase_id = "";
                                break;
                        }
                        break;
                    default:
                        this.utlCompanyGroup.txtID.Text = "";
                        this.utlCustomer.txtID.Text = "";
                        this.utlPurchase.txtID.Text = "";
                        _entityH._group_id_to = "";
                        _entityH._customer_id = "";
                        _entityH._purchase_id = "";
                        break;

                }

                #endregion

                #region 必須チェック

                // 入出庫日
                if (string.IsNullOrEmpty(_entityH._in_out_delivery_ymd))
                {
                    errMessage += "入出庫日が入力されていません。" + Environment.NewLine;
                    if (errCtl == null) errCtl = this.datInOutDeliveryYmd;
                }

                // 入力担当者
                if (_entityH._update_person_id == 0)
                {
                    errMessage += "入力担当者が入力(選択)されていません。" + Environment.NewLine;
                    if (errCtl == null) errCtl = this.utlPerson.txtID;
                }
                
                // 入出庫区分
                if (ExCast.zCInt(_entityH._in_out_delivery_kbn) == 0)
                {
                    errMessage += "入出庫区分が入力(選択)されていません。" + Environment.NewLine;
                    if (errCtl == null) errCtl = this.utlInOutDeliveryKbn.txtID;
                }

                // 入出庫先区分
                if (ExCast.zCInt(_entityH._in_out_delivery_to_kbn) == 0)
                {
                    errMessage += "入出庫先区分が入力(選択)されていません。" + Environment.NewLine;
                    if (errCtl == null) errCtl = this.utlInOutDeliveryToKbn.txtID;
                }

                // 入出庫先
                if (ExCast.zCInt(_entityH._in_out_delivery_to_kbn) == 1)
                {
                    // グループ
                    if (ExCast.zCInt(_entityH._group_id_to) == 0)
                    {
                        errMessage += this.lblSupply.Content + "が入力(選択)されていません。" + Environment.NewLine;
                        if (errCtl == null) errCtl = this.utlCompanyGroup.txtID;
                    }
                }
                else if (ExCast.zCInt(_entityH._in_out_delivery_to_kbn) == 2)
                {
                    switch (ExCast.zCInt(_entityH._in_out_delivery_kbn))
                    {
                        case 1:     // 入庫
                            if (ExCast.zCInt(_entityH._purchase_id) == 0)
                            {
                                errMessage += this.lblSupply.Content + "が入力(選択)されていません。" + Environment.NewLine;
                                if (errCtl == null) errCtl = this.utlPurchase.txtID;
                            }
                            break;
                        case 2:     // 出庫
                            if (ExCast.zCInt(_entityH._customer_id) == 0)
                            {
                                errMessage += this.lblSupply.Content + "が入力(選択)されていません。" + Environment.NewLine;
                                if (errCtl == null) errCtl = this.utlCustomer.txtID;
                            }
                            break;
                        default:
                            break;
                    }
                }

                #endregion

                #region 入力チェック

                // 入力担当者
                if (_entityH._update_person_id != 0 && string.IsNullOrEmpty(this.utlPerson.txtNm.Text.Trim()))
                {
                    errMessage += "入力担当者が適切に入力(選択)されていません。" + Environment.NewLine;
                    if (errCtl == null) errCtl = this.utlPerson.txtID;
                }

                // 入出庫区分
                if (_entityH._in_out_delivery_kbn != 0 && string.IsNullOrEmpty(this.utlInOutDeliveryKbn.txtNm.Text.Trim()))
                {
                    errMessage += "入出庫区分が適切に入力(選択)されていません。" + Environment.NewLine;
                    if (errCtl == null) errCtl = this.utlInOutDeliveryKbn.txtID;
                }

                // 入出庫先区分
                if (_entityH._in_out_delivery_to_kbn != 0 && string.IsNullOrEmpty(this.utlInOutDeliveryToKbn.txtNm.Text.Trim()))
                {
                    errMessage += "入出庫先区分が適切に入力(選択)されていません。" + Environment.NewLine;
                    if (errCtl == null) errCtl = this.utlInOutDeliveryToKbn.txtID;
                }

                // 得意先
                if (_entityH._customer_id != "" && string.IsNullOrEmpty(this.utlCustomer.txtNm.Text.Trim()))
                {
                    errMessage += "得意先が適切に入力(選択)されていません。" + Environment.NewLine;
                    if (errCtl == null) errCtl = this.utlCustomer.txtID;
                }

                // 仕入先
                if (_entityH._purchase_id != "" && string.IsNullOrEmpty(this.utlPurchase.txtNm.Text.Trim()))
                {
                    errMessage += "仕入先が適切に入力(選択)されていません。" + Environment.NewLine;
                    if (errCtl == null) errCtl = this.utlPurchase.txtID;
                }

                // グループ
                if (_entityH._group_id_to != "" && string.IsNullOrEmpty(this.utlCompanyGroup.txtNm.Text.Trim()))
                {
                    errMessage += Common.gstrGroupDisplayNm + "が適切に入力(選択)されていません。" + Environment.NewLine;
                    if (errCtl == null) errCtl = this.utlCompanyGroup.txtID;
                }

                #endregion

                #region 日付チェック

                // 入出庫日
                if (string.IsNullOrEmpty(_entityH._in_out_delivery_ymd) == false)
                {
                    if (ExCast.IsDate(_entityH._in_out_delivery_ymd) == false)
                    {
                        errMessage += "入出庫日の形式が不正です。(yyyy/mm/dd形式で入力(選択)して下さい)" + Environment.NewLine;
                        if (errCtl == null) errCtl = this.datInOutDeliveryYmd;
                    }
                }

                #endregion

                #region 日付変換

                // 入出庫日
                if (string.IsNullOrEmpty(_entityH._in_out_delivery_ymd) == false)
                {
                    _entityH._in_out_delivery_ymd = ExCast.zConvertToDate(_entityH._in_out_delivery_ymd).ToString("yyyy/MM/dd");

                }

                #endregion

                #region 正数チェック

                if (ExCast.zCLng(_entityH._no) < 0)
                {
                    errMessage += "入出庫番号には正の整数を入力して下さい。" + Environment.NewLine;
                }

                #endregion

                #region 範囲チェック

                if (ExCast.zCLng(_entityH._no) > 999999999999999)
                {
                    errMessage += "入出庫番号には15桁以内の正の整数を入力して下さい。" + Environment.NewLine;
                }

                if (ExString.LenB(_entityH._memo) > 94)
                {
                    errMessage += "摘要には全角47桁文字以内(半角94桁文字以内)を入力して下さい。" + Environment.NewLine;
                    if (errCtl == null) errCtl = this.txtMemo;
                }

                #endregion

                #endregion

                #region 明細チェック

                #region 明細再計算

                // 明細再計算
                DataDetail.IsCalcPrice = false;
                DataDetail.ReCalcDetail(_entityH, _entityListD);
                DataDetail.IsCalcPrice = true;

                #endregion

                #region 明細入力チェック

                if (_entityListD == null)
                {
                    errMessage += "明細が入力されていません。" + Environment.NewLine;
                }

                if (_entityListD.Count == 0)
                {
                    errMessage += "明細が入力されていません。" + Environment.NewLine;
                }

                #endregion

                for (int i = 0; i <= _entityListD.Count - 1; i++)
                {
                    if (string.IsNullOrEmpty(_entityListD[i]._commodity_id))
                    {
                        #region 商品未選択チェック

                        // 商品未選択は登録されない
                        if (!string.IsNullOrEmpty(_entityListD[i]._commodity_name) ||
                            _entityListD[i]._unit_id != 1 ||
                            _entityListD[i]._enter_number != 0 ||
                            _entityListD[i]._case_number != 0 ||
                            _entityListD[i]._number != 0 ||
                            !string.IsNullOrEmpty(_entityListD[i]._memo))
                        {
                            // なんらかの入力がされている場合、警告を出す
                            warnMessage += (i + 1).ToString() + "行目は商品が選択されていない為、登録されません。" + Environment.NewLine;
                            if (errCtl == null)
                            {
                                errCtl = this.dg;
                                _selectIndex = i;
                                _selectColumn = 1;
                            }
                        }

                        #endregion
                    }
                    else
                    {
                        #region 必須チェック

                        // 数量
                        if (ExCast.zCDbl(_entityListD[i]._number) == 0)
                        {
                            errMessage += (i + 1).ToString() + "行目の数量が0です。" + Environment.NewLine;
                            if (errCtl == null)
                            {
                                errCtl = this.dg;
                                _selectIndex = i;
                                _selectColumn = 8;
                            }
                        }

                        #endregion

                        #region 数値チェック

                        // 入数
                        if (ExCast.IsNumeric(_entityListD[i]._enter_number) == false)
                        {
                            errMessage += (i + 1).ToString() + "行目の入数に数値以外が入力されています。" + Environment.NewLine;
                            if (errCtl == null)
                            {
                                errCtl = this.dg;
                                _selectIndex = i;
                                _selectColumn = 6;
                            }
                        }

                        // ケース数
                        if (ExCast.IsNumeric(_entityListD[i]._case_number) == false)
                        {
                            errMessage += (i + 1).ToString() + "行目のケース数に数値以外が入力されています。" + Environment.NewLine;
                            if (errCtl == null)
                            {
                                errCtl = this.dg;
                                _selectIndex = i;
                                _selectColumn = 7;
                            }
                        }
                        // 数量
                        if (ExCast.IsNumeric(_entityListD[i]._number) == false)
                        {
                            errMessage += (i + 1).ToString() + "行目の数量に数値以外が入力されています。" + Environment.NewLine;
                            if (errCtl == null)
                            {
                                errCtl = this.dg;
                                _selectIndex = i;
                                _selectColumn = 8;
                            }
                        }

                        #endregion

                        #region 正数チェック

                        if (ExCast.zCDbl(_entityListD[i]._enter_number) < 0)
                        {
                            errMessage += (i + 1).ToString() + "行目の入数に正の数値を入力して下さい)" + Environment.NewLine;
                            if (errCtl == null)
                            {
                                errCtl = this.dg;
                                _selectIndex = i;
                                _selectColumn = 6;
                            }
                        }

                        #endregion

                        #region 範囲チェック

                        // 入数
                        if (ExCast.zCDbl(_entityListD[i]._enter_number) > 9999)
                        {
                            errMessage += (i + 1).ToString() + "行目の入数に9,999以内の数値を入力して下さい。" + Environment.NewLine;
                            if (errCtl == null)
                            {
                                errCtl = this.dg;
                                _selectIndex = i;
                                _selectColumn = 6;
                            }
                        }

                        // ケース数
                        if (ExCast.zCDbl(_entityListD[i]._case_number) > 9999)
                        {
                            errMessage += (i + 1).ToString() + "行目のケース数に9,999以内の数値を入力して下さい。" + Environment.NewLine;
                            if (errCtl == null)
                            {
                                errCtl = this.dg;
                                _selectIndex = i;
                                _selectColumn = 7;
                            }
                        }
                        if (ExCast.zCDbl(_entityListD[i]._case_number) < -9999)
                        {
                            errMessage += (i + 1).ToString() + "行目のケース数に-9,999以上の数値を入力して下さい。" + Environment.NewLine;
                            if (errCtl == null)
                            {
                                errCtl = this.dg;
                                _selectIndex = i;
                                _selectColumn = 7;
                            }
                        }

                        // 数量
                        if (ExCast.zCDbl(_entityListD[i]._number) > 99999999)
                        {
                            errMessage += (i + 1).ToString() + "行目の数量に99,999,999以内の数値を入力して下さい。" + Environment.NewLine;
                            if (errCtl == null)
                            {
                                errCtl = this.dg;
                                _selectIndex = i;
                                _selectColumn = 8;
                            }
                        }
                        if (ExCast.zCDbl(_entityListD[i]._number) < -99999999)
                        {
                            errMessage += (i + 1).ToString() + "行目の数量に-99,999,999以上の数値を入力して下さい。" + Environment.NewLine;
                            if (errCtl == null)
                            {
                                errCtl = this.dg;
                                _selectIndex = i;
                                _selectColumn = 8;
                            }
                        }

                        // 備考
                        if (ExString.LenB(_entityListD[i]._memo) > 16)
                        {
                            errMessage += (i + 1).ToString() + "行目の備考に全角8桁文字以内(半角16桁文字以内)を入力して下さい。" + Environment.NewLine;
                            if (errCtl == null)
                            {
                                errCtl = this.dg;
                                _selectIndex = i;
                                _selectColumn = 12;
                            }
                        }

                        #endregion

                        #region 在庫チェック

                        //// 在庫
                        //if (ExCast.zCStr(_entityListD[i]._commodity_id) != "" && ExCast.zCInt(_entityListD[i]._inventory_management_division_id) == 1)
                        //{
                        //    double sum_number = 0;

                        //    for (int _i = 0; _i <= _entityListD.Count - 1; _i++)
                        //    {
                        //        if (ExCast.zCStr(_entityListD[i]._commodity_id) == ExCast.zCStr(_entityListD[_i]._commodity_id))
                        //        {
                        //            sum_number += ExCast.zCDbl(_entityListD[_i]._number);
                        //        }
                        //    }

                        //    if (ExCast.zCDbl(_entityListD[i]._inventory_number) - sum_number < 0)
                        //    {
                        //        bool _set = true;
                        //        foreach (string item in list_warn_commodity)
                        //        {
                        //            if (item == ExCast.zCStr(_entityListD[i]._commodity_id))
                        //            {
                        //                _set = false;
                        //            }
                        //        }

                        //        if (_set == true)
                        //        {
                        //            warnMessage += "商品コード：" + _entityListD[i]._commodity_id + "(" + _entityListD[i]._commodity_name + ") の在庫数がマイナスになります。" + Environment.NewLine;
                        //            if (errCtl == null)
                        //            {
                        //                errCtl = this.dg;
                        //                _selectIndex = i;
                        //                _selectColumn = 3;
                        //            }
                        //            list_warn_commodity.Add(ExCast.zCStr(_entityListD[i]._commodity_id));
                        //        }
                        //    }
                        //}
                        
                        #endregion

                        IsDetailExists = true;
                    }
                }


                #region 登録対象データ存在チェック

                if (IsDetailExists == false)
                {
                    errMessage += "明細の登録対象データがありません。" + Environment.NewLine;
                    if (errCtl == null)
                    {
                        errCtl = this.dg;
                        _selectIndex = 0;
                        _selectColumn = 1;
                    }
                }

                #endregion

                #endregion

                #region エラー or 警告時処理

                bool flg = true;

                if (!string.IsNullOrEmpty(errMessage))
                {
                    ExMessageBox.Show(errMessage, Dlg.MessageBox.MessageBoxIcon.Error);
                    flg = false;
                }
                else
                {
                    if (!string.IsNullOrEmpty(warnMessage))
                    {
                        warnMessage += "このまま登録を続行してもよろしいですか？" + Environment.NewLine;
                        ExMessageBox.ResultShow(this, errCtl, warnMessage);
                        flg = false;    // ResultMessageBoxにてResult処理
                        //if (ExMessageBox.ResultShow(warnMessage) == MessageBoxResult.Cancel)
                        //{
                        //    flg = false;
                        //}
                    }
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

                switch (this.utlFunctionKey.gFunctionKeyEnable)
                {
                    case Utl_FunctionKey.geFunctionKeyEnable.New:
                    case Utl_FunctionKey.geFunctionKeyEnable.Init:
                        UpdateData(Common.geUpdateType.Insert);
                        break;
                    case Utl_FunctionKey.geFunctionKeyEnable.Upd:
                        UpdateData(Common.geUpdateType.Update);
                        break;
                    default:
                        break;
                }

                #endregion
            }
            finally
            {
                Common.gblnBtnProcLock = false;
                Common.gblnBtnDesynchronizeLock = false;
            }
        }

        // 入力チェック
        private void InputCheckDelete()
        {
            try
            {
                if (this.utlNo.txtID.Text.Trim() == "")
                {
                    ExMessageBox.Show("入出庫データが選択されていません。");
                    return;
                }

                if (this._entityH == null)
                {
                    ExMessageBox.Show("入出庫データが選択されていません。");
                    return;
                }

                if (this._entityH._id == 0)
                {
                    ExMessageBox.Show("入出庫データが選択されていません。");
                    return;
                }

                UpdateData(Common.geUpdateType.Delete);
            }
            finally
            {
                Common.gblnBtnProcLock = false;
                Common.gblnBtnDesynchronizeLock = false;
            }
        }

        public override void ResultMessageBox(MessageBoxResult _MessageBoxResult, Control _errCtl)
        {
            if (_MessageBoxResult == MessageBoxResult.OK)
            {
                #region 更新処理

                switch (this.utlFunctionKey.gFunctionKeyEnable)
                {
                    case Utl_FunctionKey.geFunctionKeyEnable.New:
                    case Utl_FunctionKey.geFunctionKeyEnable.Init:
                        UpdateData(Common.geUpdateType.Insert);
                        break;
                    case Utl_FunctionKey.geFunctionKeyEnable.Upd:
                        UpdateData(Common.geUpdateType.Update);
                        break;
                    default:
                        break;
                }

                #endregion
            }
            else
            {
                if (_errCtl != null)
                {
                    switch (_errCtl.Name)
                    {
                        case "dg":
                            _errCtl.Focus();
                            this.dg.SelectedIndex = _selectIndex;
                            dg.CurrentColumn = dg.Columns[_selectColumn];
                            break;
                        default:
                            ExBackgroundWorker.DoWork_Focus(_errCtl, 10);
                            break;
                    }
                }
            }
        }

        #endregion

        #region DataGrid Events

        private void dg_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            try
            {
                beforeValue = "";
                beforeSelectedIndex = -1;
                beforeSelectedIndex = this.dg.SelectedIndex;

                EntityInOutDeliveryD entity = (EntityInOutDeliveryD)e.Row.DataContext;
                switch (e.Column.DisplayIndex)
                {
                    case 1:         // 商品コード
                        beforeValue = entity._commodity_id;
                        break;
                    case 2:         // 商品名
                        beforeValue = entity._commodity_name;
                        break;
                    case 3:         // 単位
                        beforeValue = entity._unit_nm;
                        break;
                    case 4:         // 入数
                        beforeValue = ExCast.zCStr(entity._enter_number);
                        break;
                    case 5:         // ケース数
                        beforeValue = ExCast.zCStr(entity._case_number);
                        break;
                    case 6:         // 数量
                        beforeValue = ExCast.zCStr(entity._number);
                        break;
                    case 7:         // 備考
                        beforeValue = ExCast.zCStr(entity._memo);
                        break;
                }
            }
            catch
            {
            }
        }

        private void dg_CellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
        {
            EntityInOutDeliveryD entity = (EntityInOutDeliveryD)e.Row.DataContext;

            // コンボボックスID連動
            switch (e.Column.DisplayIndex)
            {
                case 3:         // 単位
                    if (_entityListD == null) return;
                    if (_entityListD.Count >= dg.SelectedIndex && dg.SelectedIndex != -1)
                        _entityListD[dg.SelectedIndex]._unit_id = MeiNameList.GetID(MeiNameList.geNameKbn.UNIT_ID, ExCast.zCStr(entity._unit_nm));
                    break;
            }

            if (beforeSelectedIndex == -1) return;

            // 明細計算
            switch (e.Column.DisplayIndex)
            {
                case 1:         // 商品コード
                    if (beforeValue == entity._commodity_id) return;
                    if (Common.gblnDesynchronizeLock == true) return;
                    Common.gblnDesynchronizeLock = true;
                    entity._commodity_id = ExCast.zFormatForID(entity._commodity_id, Common.gintidFigureCommodity);
                    MstData _mstData = new MstData();
                    _mstData.GetMData(MstData.geMDataKbn.Commodity, new string[] { entity._commodity_id, ExCast.zCStr(beforeSelectedIndex) }, this);
                    break;
                case 4:         // 入数
                    if (beforeValue == ExCast.zCStr(entity._enter_number)) return;
                    DataDetail.CalcDetailNumber(beforeSelectedIndex, _entityH, _entityListD);   // 明細数量計算
                    break;
                case 5:         // ケース数
                    if (beforeValue == ExCast.zCStr(entity._case_number)) return;
                    DataDetail.CalcDetailNumber(beforeSelectedIndex, _entityH, _entityListD);   // 明細数量計算
                    break;
                case 6:         // 数量
                    if (beforeValue == ExCast.zCStr(entity._number)) return;
                    DataDetail.CalcDetail(beforeSelectedIndex, _entityH, _entityListD);       　// 明細計算
                    //OrderDetailData.CalcDetailNumber(dg.SelectedIndex, _entityH, _entityListD);   // 明細数量計算
                    break;
            }
        }

        private void dg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_entityListD == null)
            {
                txtInventory.Text = "";
                return;
            }

            if (_entityListD.Count >= dg.SelectedIndex && dg.SelectedIndex != -1)
            {
                if (ExCast.zCStr(_entityListD[dg.SelectedIndex]._commodity_id) != "" && ExCast.zCInt(_entityListD[dg.SelectedIndex]._inventory_management_division_id) == 1)
                {
                    txtInventory.Text = ExCast.zCStr(_entityListD[dg.SelectedIndex]._inventory_number);
                }
                else
                {
                    txtInventory.Text = "";
                }
            }
            else
            {
                txtInventory.Text = "";
            }
        }

        private void txtGoodsID_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            _selectIndex = dg.SelectedIndex;
            _selectColumn = dg.CurrentColumn.DisplayIndex;
            beforeValue = "";
            if (_entityListD == null) return;
            if (_entityListD.Count >= _selectIndex)
            {
                beforeValue = _entityListD[_selectIndex]._commodity_id;
            }

            masterDlg = new Dlg_MstSearch();
            masterDlg.MstKbn = MstData.geMDataKbn.Commodity;
            masterDlg.Show();
            masterDlg.Closed += masterDlg_Closed;
        }

        private void masterDlg_Closed(object sender, EventArgs e)
        {
            Dlg_MstSearch dlg = (Dlg_MstSearch)sender;
            if (Dlg_MstSearch.this_DialogResult == true)
            {
                if (_entityListD == null) return;
                if (_entityListD.Count >= _selectIndex)
                {
                    _entityListD[_selectIndex]._commodity_id = Dlg_MstSearch.this_id.ToString();
                    _entityListD[_selectIndex]._commodity_name = Dlg_MstSearch.this_name.ToString();
                }
            }
            if (beforeValue != _entityListD[_selectIndex]._commodity_id)
            {
                MstData _mstData = new MstData();
                _mstData.GetMData(MstData.geMDataKbn.Commodity, new string[] { _entityListD[_selectIndex]._commodity_id, ExCast.zCStr(_selectIndex) }, this);
            }

            dg.Focus();
            dg.SelectedIndex = _selectIndex;
            dg.CurrentColumn = dg.Columns[_selectColumn];
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

        #region Master Data Select

        public override void MstDataSelect(ExWebServiceMst.geWebServiceMstNmCallKbn intKbn, svcMstData.EntityMstData mst)
        {
            try
            {
                svcMstData.EntityMstData _mst = null;
                _mst = mst;

                switch (intKbn)
                {
                    case ExWebServiceMst.geWebServiceMstNmCallKbn.GetCustomer:
                        // _PropertyChangedにて設定
                        break;
                    case ExWebServiceMst.geWebServiceMstNmCallKbn.GetCommodity:
                        if (Common.gblnDesynchronizeLock == false) return;
                        if (_mst == null) return;

                        // attribute20に選択された行が設定されてくる
                        if (string.IsNullOrEmpty(_mst.attribute20))
                        {
                            // 商品から明細へ
                            DataDetail.SetCommodityToDetail(dg.SelectedIndex, _entityH, _entityListD, _mst);
                            // 明細再計算
                            DataDetail.ReCalcDetail(_entityH, _entityListD);
                            // 明細フッターセット
                            SetDetailFooter();
                        }
                        else
                        {
                            if (_entityListD.Count >= ExCast.zCInt(_mst.attribute20) + 1)
                            {
                                // 商品から明細へ
                                DataDetail.SetCommodityToDetail(ExCast.zCInt(_mst.attribute20), _entityH, _entityListD, _mst);
                                // 明細再計算
                                DataDetail.ReCalcDetail(_entityH, _entityListD);
                                // 明細フッターセット
                                SetDetailFooter();                                      
                            }
                        }
                        break;
                }
            }
            finally
            {
                Common.gblnDesynchronizeLock = false;
            }
        }

        #endregion

        #region Detail

        private void SetInitCombo(ref EntityInOutDeliveryD entityD)
        {
            // コンボボックス初期選択
            List<string> lst;
            lst = MeiNameList.GetListMei(MeiNameList.geNameKbn.UNIT_ID);
            entityD._unit_nm = lst[0];
            entityD._unit_id = MeiNameList.GetID(MeiNameList.geNameKbn.UNIT_ID, lst[0]);
        }

        // 明細フッターセット
        public void SetDetailFooter()
        {
            // 初期化
            txtInventory.Text = "";

            if (_entityListD == null) return;
            if (_entityListD.Count >= dg.SelectedIndex && dg.SelectedIndex != -1)
            {
                txtInventory.Text = ExCast.zCStr(_entityListD[dg.SelectedIndex]._inventory_number);
            }
        }

        #endregion

        #region PropertyChanged

        private void _PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            string _prop = e.PropertyName;
            if (_prop == null) _prop = "";
            if (_prop.Length > 0)
            {
                if (_prop.Substring(0, 1) == "_")
                {
                    _prop = _prop.Substring(1, _prop.Length - 1);
                }
            }

            switch (_prop)
            {
                case "customer_name":
                    if (Common.gblnDesynchronizeLock == false) return;

                    // 明細再計算
                    //DataDetail.ReCalcDetail(_entityH, _entityListD);
                    //Common.gblnDesynchronizeLock = false;
                    break;

                default:
                    break;
            }
        }

        #endregion

        #region ConvertBeforeDate

        private void ConvertBeforeData(EntityInOutDeliveryH _prm_entityH,
                                       ObservableCollection<EntityInOutDeliveryD> _prm_entityListD,
                                       EntityInOutDeliveryH _prm_before_entityH,
                                       ObservableCollection<EntityInOutDeliveryD> _prm_before_entityListD)
        {

            #region Set Entity Detail

            _prm_before_entityListD.Clear();

            for (int i = 0; i <= _prm_entityListD.Count - 1; i++)
            {

                EntityInOutDeliveryD _entityD = new EntityInOutDeliveryD();

                _entityD._id = _prm_entityListD[i]._id;
                _entityD._rec_no = _prm_entityListD[i]._rec_no;
                //_entityD._breakdown_id = _prm_entityListD[i]._breakdown_id;
                //_entityD._breakdown_nm = _prm_entityListD[i]._breakdown_nm;
                //_entityD._deliver_division_id = _prm_entityListD[i]._deliver_division_id;
                //_entityD._deliver_division_nm = _prm_entityListD[i]._deliver_division_nm;
                _entityD._commodity_id = _prm_entityListD[i]._commodity_id;
                _entityD._commodity_name = _prm_entityListD[i]._commodity_name;
                _entityD._unit_id = _prm_entityListD[i]._unit_id;
                _entityD._unit_nm = _prm_entityListD[i]._unit_nm;
                _entityD._enter_number = _prm_entityListD[i]._enter_number;
                _entityD._case_number = _prm_entityListD[i]._case_number;
                _entityD._number = _prm_entityListD[i]._number;
                //_entityD._unit_price = _prm_entityListD[i]._unit_price;
                //_entityD._sales_cost = _prm_entityListD[i]._sales_cost;
                //_entityD._tax = _prm_entityListD[i]._tax;
                //_entityD._no_tax_price = _prm_entityListD[i]._no_tax_price;
                //_entityD._price = _prm_entityListD[i]._price;
                //_entityD._profits = _prm_entityListD[i]._profits;
                //_entityD._profits_percent = _prm_entityListD[i]._profits_percent;
                //_entityD._tax_division_id = _prm_entityListD[i]._tax_division_id;
                //_entityD._tax_division_nm = _prm_entityListD[i]._tax_division_nm;
                //_entityD._tax_percent = _prm_entityListD[i]._tax_percent;
                _entityD._inventory_management_division_id = _prm_entityListD[i]._inventory_management_division_id;
                _entityD._inventory_number = _prm_entityListD[i]._inventory_number;
                //_entityD._retail_price_skip_tax = _prm_entityListD[i]._retail_price_skip_tax;
                //_entityD._retail_price_before_tax = _prm_entityListD[i]._retail_price_before_tax;
                //_entityD._sales_unit_price_skip_tax = _prm_entityListD[i]._sales_unit_price_skip_tax;
                //_entityD._sales_unit_price_before_tax = _prm_entityListD[i]._sales_unit_price_before_tax;
                //_entityD._sales_cost_price_skip_tax = _prm_entityListD[i]._sales_cost_price_skip_tax;
                //_entityD._sales_cost_price_before_tax = _prm_entityListD[i]._sales_cost_price_before_tax;
                _entityD._number_decimal_digit = _prm_entityListD[i]._number_decimal_digit;
                //_entityD._unit_decimal_digit = _prm_entityListD[i]._unit_decimal_digit;
                //_entityD._order_id = _prm_entityListD[i]._order_id;
                //_entityD._order_number = _prm_entityListD[i]._order_number;
                //_entityD._order_stay_number = _prm_entityListD[i]._order_stay_number;
                _entityD._memo = _prm_entityListD[i]._memo;
                _entityD._lock_flg = _prm_entityListD[i]._lock_flg;
                _entityD._message = _prm_entityListD[i]._message;

                _prm_before_entityListD.Add(_entityD);
            }

            #endregion

            #region Set Entity Head

            _prm_before_entityH._id = _prm_entityH._id;
            _prm_before_entityH._no = _prm_entityH._no;
            //_prm_before_entityH._red_before_no = _prm_entityH._red_before_no;
            //_prm_before_entityH._order_no = _prm_entityH._order_no;
            //_prm_before_entityH._estimateno = _prm_entityH._estimateno;
            _prm_before_entityH._in_out_delivery_ymd = _prm_entityH._in_out_delivery_ymd;

            _prm_before_entityH._in_out_delivery_kbn = _prm_entityH._in_out_delivery_kbn;
            _prm_before_entityH._in_out_delivery_kbn_nm = _prm_entityH._in_out_delivery_kbn_nm;
            _prm_before_entityH._in_out_delivery_to_kbn = _prm_entityH._in_out_delivery_to_kbn;
            _prm_before_entityH._in_out_delivery_to_kbn_nm = _prm_entityH._in_out_delivery_to_kbn_nm;
            _prm_before_entityH._in_out_delivery_proc_kbn = _prm_entityH._in_out_delivery_proc_kbn;
            _prm_before_entityH._in_out_delivery_proc_kbn_nm = _prm_entityH._in_out_delivery_proc_kbn_nm;

            _prm_before_entityH._cause_no = _prm_entityH._cause_no;

            _prm_before_entityH._group_id_to = _prm_entityH._group_id_to;
            _prm_before_entityH._group_id_to_nm = _prm_entityH._group_id_to_nm;
            _prm_before_entityH._customer_id = _prm_entityH._customer_id;
            _prm_before_entityH._customer_name = _prm_entityH._customer_name;
            _prm_before_entityH._purchase_id = _prm_entityH._purchase_id;
            _prm_before_entityH._purchase_name = _prm_entityH._purchase_name;
            //_prm_before_entityH._business_division_id = _prm_entityH._business_division_id;
            //_prm_before_entityH._business_division_name = _prm_entityH._business_division_name;
            //_prm_before_entityH._supplier_id = _prm_entityH._supplier_id;
            //_prm_before_entityH._supplier_name = _prm_entityH._supplier_name;
            //_prm_before_entityH._supply_ymd = _prm_entityH._supply_ymd;
            _prm_before_entityH._sum_enter_number = _prm_entityH._sum_enter_number;
            _prm_before_entityH._sum_case_number = _prm_entityH._sum_case_number;
            _prm_before_entityH._sum_number = _prm_entityH._sum_number;
            //_prm_before_entityH._sum_unit_price = _prm_entityH._sum_unit_price;
            //_prm_before_entityH._sum_sales_cost = _prm_entityH._sum_sales_cost;
            //_prm_before_entityH._sum_tax = _prm_entityH._sum_tax;
            //_prm_before_entityH._sum_no_tax_price = _prm_entityH._sum_no_tax_price;
            //_prm_before_entityH._sum_price = _prm_entityH._sum_price;
            //_prm_before_entityH._sum_profits = _prm_entityH._sum_profits;
            //_prm_before_entityH._profits_percent = _prm_entityH._profits_percent;
            //_prm_before_entityH._invoice_close_flg = _prm_entityH._invoice_close_flg;
            _prm_before_entityH._memo = _prm_entityH._memo;
            _prm_before_entityH._update_flg = _prm_entityH._update_flg;
            _prm_before_entityH._update_person_id = _prm_entityH._update_person_id;
            _prm_before_entityH._update_person_nm = _prm_entityH._update_person_nm;
            _prm_before_entityH._update_date = _prm_entityH._update_date;
            _prm_before_entityH._update_time = _prm_entityH._update_time;
            //_prm_before_entityH._price_fraction_proc_id = _prm_entityH._price_fraction_proc_id;
            //_prm_before_entityH._tax_fraction_proc_id = _prm_entityH._tax_fraction_proc_id;
            //_prm_before_entityH._unit_kind_id = _prm_entityH._unit_kind_id;
            //_prm_before_entityH._credit_limit_price = _prm_entityH._credit_limit_price;
            //_prm_before_entityH._sales_credit_price = _prm_entityH._sales_credit_price;
            //_prm_before_entityH._before_sales_credit_price = _prm_entityH._before_sales_credit_price;
            //_prm_before_entityH._invoice_id = _prm_entityH._invoice_id;
            //_prm_before_entityH._invoice_name = _prm_entityH._invoice_name;
            //_prm_before_entityH._invoice_yyyymmdd = _prm_entityH._invoice_yyyymmdd;
            //_prm_before_entityH._credit_rate = _prm_entityH._credit_rate;
            //_prm_before_entityH._collect_day = _prm_entityH._collect_day;
            //_prm_before_entityH._collect_cycle_id = _prm_entityH._collect_cycle_id;
            //_prm_before_entityH._invoice_no = _prm_entityH._invoice_no;
            //_prm_before_entityH._invoice_state = _prm_entityH._invoice_state;
            //_prm_before_entityH._receipt_receivable_kbn = _prm_entityH._receipt_receivable_kbn;
            //_prm_before_entityH._receipt_receivable_kbn_nm = _prm_entityH._receipt_receivable_kbn_nm;
            _prm_before_entityH._lock_flg = _prm_entityH._lock_flg;
            _prm_before_entityH._message = _prm_entityH._message;

            #endregion
        }

        #endregion

    }

}
