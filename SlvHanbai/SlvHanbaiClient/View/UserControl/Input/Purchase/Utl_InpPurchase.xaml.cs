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
using SlvHanbaiClient.svcOrder;
using SlvHanbaiClient.svcPurchaseOrder;
using SlvHanbaiClient.svcPurchase;
using SlvHanbaiClient.svcMstData;
using SlvHanbaiClient.View.UserControl.Custom;
using SlvHanbaiClient.View.Dlg;

namespace SlvHanbaiClient.View.UserControl.Input.Purchase
{
    public partial class Utl_InpPurchase : ExUserControl
    {

        #region Filed Const

        private const String CLASS_NM = "Utl_InpPurchase";
        private const String PG_NM = DataPgEvidence.PGName.Purchase.PurchaseInp;
        private Common.gePageType _PageType = Common.gePageType.InpPurchase;

        private Utl_FunctionKey utlFunctionKey = null;

        private const ExWebService.geWebServiceCallKbn _GetHeadWebServiceCallKbn = ExWebService.geWebServiceCallKbn.GetPurchaseListH;
        private const ExWebService.geWebServiceCallKbn _GetDetailWebServiceCallKbn = ExWebService.geWebServiceCallKbn.GetPurchaseListD;
        private const ExWebService.geWebServiceCallKbn _UpdWebServiceCallKbn = ExWebService.geWebServiceCallKbn.UpdatePurchase;

        private EntityPurchaseH _entityH = new EntityPurchaseH();
        private ObservableCollection<EntityPurchaseD> _entityListD = new ObservableCollection<EntityPurchaseD>();

        private EntityPurchaseOrderH _EntityPurchaseOrderH = new EntityPurchaseOrderH();
        private ObservableCollection<EntityPurchaseOrderD> _entityListOrderD = new ObservableCollection<EntityPurchaseOrderD>();

        private EntityPurchaseH _before_entityH = new EntityPurchaseH();
        private ObservableCollection<EntityPurchaseD> _before_entityListD = new ObservableCollection<EntityPurchaseD>();

        // 赤伝発行保持用
        private EntityPurchaseH _entityRedH = new EntityPurchaseH();
        private ObservableCollection<EntityPurchaseD> _entityListRedD = new ObservableCollection<EntityPurchaseD>();

        private readonly string tableName = "T_PURCHASE_H";
        private Control activeControl;

        private Common.geWinType WinType = Common.geWinType.ListPurchase;

        private Dlg_DataForm dataForm;
        private Common.geDataFormType DataFormType = Common.geDataFormType.PurchaseDetail;

        private Dlg_MstSearch masterDlg;
        private Dlg_Copying copyDlg;
        private Common.geWinGroupType beforeWinGroupType;

        private string beforeValue = "";                // DataGrid編集チェック用
        private int beforeSelectedIndex = -1;           // DataGrid編集前 行
        private int _selectIndex = 0;                   // データグリッド現在行保持用
        private int _selectColumn = 0;                  // データグリッド現在列保持用

        #endregion

        #region Constructor

        public Utl_InpPurchase()
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
            this.utlPurchaseOrderNo._ExUserControl_LostFocus -= this.utlPurchaseOrderNo_LostFocus;
            this.utlPurchaseOrderNo._ExUserControl_LostFocus += this.utlPurchaseOrderNo_LostFocus;

            this.utlSendKbn.ID_TextChanged -= this.sendKbn_TextChanged;
            this.utlSendKbn.ID_TextChanged += this.sendKbn_TextChanged;

            // 画面初期化
            ExVisualTreeHelper.initDisplay(this.LayoutRoot);

            // ファンクションキー初期設定
            this.utlFunctionKey = ExVisualTreeHelper.GetUtlFunctionKey(this.LayoutRoot);
            this.utlFunctionKey.gFunctionKeyEnable = Utl_FunctionKey.geFunctionKeyEnable.Init;

            // バインド設定
            this.utlPurchase.utlSupplier = this.utlSupplier;
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
            Common.gblnDesynchronizeLock = false;

            // 先頭選択
            this.dg.SelectedFirst();

            // ヘッダ初期化
            _entityH = null;
            SetBinding();

            // 明細初期化
            _entityListD = null;
            _entityListD = new ObservableCollection<EntityPurchaseD>();

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
            if (_entityH == null || _entityListD == null) return;

            copyDlg = new Dlg_Copying();

            copyDlg.utlCopying.gPageType = _PageType;
            copyDlg.utlCopying.gWinMsterType = Common.geWinMsterType.None;
            copyDlg.utlCopying.utlMstID.MstKbn = Class.Data.MstData.geMDataKbn.None;
            copyDlg.utlCopying.tableName = this.tableName;
            copyDlg.utlCopying.before_id = this.utlNo.txtID.Text.Trim();

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
                _entityH._purchase_order_no = 0;
                _entityH._payment_close_flg = 0;
                _entityH._payment_no = 0;
                _entityH._payment_state = "";
                _entityH._payment_cash_receivable_kbn = 0;
                _entityH._payment_cash_receivable_kbn_nm = "消込未";
                _entityH._lock_flg = 0;

                for (int i = 1; i <= _entityListD.Count; i++)
                {
                    _entityListD[i - 1]._purchase_order_id = 0;
                    _entityListD[i - 1]._purchase_order_stay_number = 0;
                }

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
                    ExBackgroundWorker.DoWork_Focus(this.utlPerson.txtID, 10);
                }

                // バインド反映
                SetBinding();

                // 前回情報保持
                ConvertBeforeData(_entityH, _entityListD, _before_entityH, _before_entityListD);
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
            if (activeControl == null)
            {
                return;
            }

            switch (activeControl.Name)
            {
                case "utlNo":
                case "utlPurchaseOrderNo":
                    Utl_InpNoText inp = (Utl_InpNoText)activeControl;
                    inp.ShowList();
                    break;
                case "datPurchaseYmd":
                case "datNokiYmd":
                    ExDatePicker dt = (ExDatePicker)activeControl;
                    dt.ShowCalender();
                    break;
                case "utlTax":
                case "utlBusiness":
                case "utlSendKbn":
                    Utl_MeiText mei = (Utl_MeiText)activeControl;
                    mei.ShowList();
                    break;
                case "utlPerson":
                case "utlPurchase":
                case "utlCustomer":
                case "utlSupplier":
                    Utl_MstText mst = (Utl_MstText)activeControl;
                    mst.ShowList();
                    break;
                case "DetailGoods":
                    txtGoodsID_MouseDoubleClick(null, null);
                    break;
                case "DetailUnitPrice":
                    txtUnitPrice_MouseDoubleClick(null, null);
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

            if (_entityListD == null) _entityListD = new ObservableCollection<EntityPurchaseD>();

            EntityPurchaseD entity = new EntityPurchaseD();
            int cnt = 1;
            if (_entityListD != null) cnt = _entityListD.Count + 1;
            entity._rec_no = cnt;
            SetInitCombo(ref entity);   // コンボボックス初期選択
            _entityListD.Add(entity);
            dg.SelectedIndex = entity._rec_no - 1;
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
                case "cboBreakDown":
                case "cboDeliver":
                case "txtGoodsName":
                case "cboUnit":
                case "txtEnterNum":
                case "txtNumber":
                case "txtUnitPrice":
                case "txtPrice":
                case "txtCaseNum":
                case "cboTaxDivision":
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

            if (_entityListD[this.dg.SelectedIndex]._purchase_order_id != 0)
            {
                ExMessageBox.Show("発注データからの転記分の為、削除できません。" + Environment.NewLine + "対象行データを取消する場合、納品区分を取消に変更して下さい。");
                return;
            }

            // 行削除
            _entityListD.RemoveAt(this.dg.SelectedIndex);

            // 行削除後の選択
            if (this.dg.SelectedIndex != 0) this.dg.SelectedIndex = this.dg.SelectedIndex - 1;

            // データ1件もない場合、デフォルト行の追加
            if (_entityListD.Count == 0) btnF7_Click(null, null);

            // 行番号振り直し
            for (int i = 1; i <= _entityListD.Count; i++)
            {
                _entityListD[i -1]._rec_no = i;
            }

            // 明細合計計算
            DataDetail.CalcSumDetail(this._entityH, this._entityListD);
        }

        // F9ボタン(赤伝発行) クリック
        public override void btnF9_Click(object sender, RoutedEventArgs e)
        {
            SetRedData();
            SetBinding();
            this.utlFunctionKey.gFunctionKeyEnable = Utl_FunctionKey.geFunctionKeyEnable.Init;
            this.utlNo.txtID_IsReadOnly = false;
            this.utlNo.txtID.Text = "";
            ExBackgroundWorker.DoWork_Focus(this.utlPerson.txtID, 10);
        }

        // F11ボタン(印刷) クリック
        public override void btnF11_Click(object sender, RoutedEventArgs e)
        {
            beforeWinGroupType = Common.gWinGroupType;
            Common.gWinMsterType = Common.geWinMsterType.None;
            Common.gWinGroupType = Common.geWinGroupType.InpListReport;
            Common.gWinType = this.WinType;

            Common.InpSearchSales.Closed -= reportDlg_Closed;
            Common.InpSearchSales.Closed += reportDlg_Closed;
            Common.InpSearchSales.Show();
        }

        private void reportDlg_Closed(object sender, EventArgs e)
        {
            Common.InpSearchSales.Closed -= reportDlg_Closed;
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

        private void btnSalesBalance_Click(object sender, RoutedEventArgs e)
        {
            if (this._entityH == null)
            {
                ExMessageBox.Show("仕入先を選択して下さい。");
                return;
            }

            if (string.IsNullOrEmpty(this._entityH._purchase_id) || this._entityH._purchase_id == "0")
            {
                ExMessageBox.Show("仕入先を選択して下さい。");
                return;
            }

            Dlg_MstSearch searchDlg = new Dlg_MstSearch(MstData.geMDataKbn.PaymentBalance);
            searchDlg.MstKbn = Class.Data.MstData.geMDataKbn.PaymentBalance;
            searchDlg.MstGroupKbn = Class.Data.MstData.geMGroupKbn.None;
            searchDlg.txtCode.Text = ExCast.zNumZeroNothingFormat(this._entityH._purchase_id);
            searchDlg.Show();

        }

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

            switch (ctl.Name)
            {
                #region Header

                case "utlNo":
                case "utlPurchaseOrderNo":
                case "datPurchaseYmd":
                case "utlPerson":
                case "utlPurchase":
                case "utlSendKbn":
                case "utlCustomer":
                case "utlSupplier":
                case "utlTax":
                case "utlBusiness":
                case "datNokiYmd":
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

                case "cboBreakDown":
                case "cboDeliverDivison":
                case "txtGoodsName":
                case "cboUnit":
                case "txtEnterNum":
                case "txtNumber":
                case "txtPrice":
                case "txtCaseNum":
                case "cboTaxDivision":
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
                    ExTextBox txt = new ExTextBox();
                    txt.Name = "DetailGoods";
                    activeControl = txt;
                    break;
                case "txtUnitPrice":
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
                    txt.Name = "DetailUnitPrice";
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

        private void utlPurchaseOrderNo_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.utlPurchaseOrderNo.txtID.Text.Trim()))
            {
                if (this._entityH != null) this._entityH._purchase_order_no = 0;
                return;
            }

            if (this.utlPurchaseOrderNo.txtID.Text.Trim() != "" && this.utlPurchaseOrderNo.txtID.UpdataFlg == true)
            {
                GetHeadPurchaseOrder(ExCast.zCLng(this.utlPurchaseOrderNo.txtID.Text.Trim()));
            }
        }

        #endregion

        #region Text Changed

        private void sendKbn_TextChanged(object sender, TextChangedEventArgs e)
        {
            switch (ExCast.zCInt(this.utlSendKbn.txtID.Text.Trim()))
            {
                case 2: // 得意先
                    this.lblSupply.Visibility = System.Windows.Visibility.Visible;
                    this.utlCustomer.Visibility = System.Windows.Visibility.Visible;
                    this.utlCustomer.nm_Width = 252;
                    this.utlSupplier.txtID.Text = "";
                    this.utlSupplier.Visibility = System.Windows.Visibility.Collapsed;
                    break;
                case 3: // 納入先
                    this.lblSupply.Visibility = System.Windows.Visibility.Visible;
                    this.utlCustomer.Visibility = System.Windows.Visibility.Visible;
                    this.utlCustomer.nm_Width = 0;
                    this.utlSupplier.Visibility = System.Windows.Visibility.Visible;
                    break;
                default:
                    this.utlCustomer.txtID.Text = "";
                    this.utlSupplier.txtID.Text = "";
                    this.lblSupply.Visibility = System.Windows.Visibility.Collapsed;
                    this.utlCustomer.Visibility = System.Windows.Visibility.Collapsed;
                    this.utlSupplier.Visibility = System.Windows.Visibility.Collapsed;
                    break;
            }
        }

        #endregion

        #region Binding Setting

        private void SetBinding()
        {
            if (_entityH == null)
            {
                _entityH = new EntityPurchaseH();
            }

            if (_entityListD == null)
            {
                _entityListD = new ObservableCollection<EntityPurchaseD>();
            }

            // マスタコントロールPropertyChanged
            _entityH.PropertyChanged += this.utlPurchase.MstID_Changed;
            _entityH.PropertyChanged += this.utlCustomer.MstID_Changed;
            _entityH.PropertyChanged += this.utlSupplier.MstID_Changed;
            _entityH.PropertyChanged += this.utlPerson.MstID_Changed;
            _entityH.PropertyChanged += this._PropertyChanged;
            this.utlPurchase.ParentData = _entityH;
            this.utlCustomer.ParentData = _entityH;
            this.utlSupplier.ParentData = _entityH;
            this.utlPerson.ParentData = _entityH;

            NumberConverter nmConvDecm0 = new NumberConverter();
            NumberConverter nmConvDecm2 = new NumberConverter();
            nmConvDecm2.DecimalPlaces = 2;

            #region Bind

            // バインド
            Binding BindingPurchaseOrderNo = new Binding("_purchase_order_no");
            BindingPurchaseOrderNo.Mode = BindingMode.TwoWay;
            BindingPurchaseOrderNo.Source = _entityH;
            this.utlPurchaseOrderNo.txtID.SetBinding(TextBox.TextProperty, BindingPurchaseOrderNo);

            Binding BindingPurchaseYmd = new Binding("_purchase_ymd");
            BindingPurchaseYmd.Mode = BindingMode.TwoWay;
            BindingPurchaseYmd.Source = _entityH;
            this.datPurchaseYmd.SetBinding(DatePicker.SelectedDateProperty, BindingPurchaseYmd);

            if (string.IsNullOrEmpty(_entityH._purchase_ymd))
            {
                _entityH._purchase_ymd = DateTime.Now.ToString("yyyy/MM/dd");
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

            Binding BindingPurchaseId = new Binding("_purchase_id");
            BindingPurchaseId.Mode = BindingMode.TwoWay;
            BindingPurchaseId.Source = _entityH;
            this.utlPurchase.txtID.SetBinding(TextBox.TextProperty, BindingPurchaseId);

            Binding BindingPurchaseName = new Binding("_purchase_name");
            BindingPurchaseName.Mode = BindingMode.TwoWay;
            BindingPurchaseName.Source = _entityH;
            this.utlPurchase.txtNm.SetBinding(TextBox.TextProperty, BindingPurchaseName);

            Binding BindingPaymentNo = new Binding("_payment_no");
            BindingPaymentNo.Mode = BindingMode.TwoWay;
            BindingPaymentNo.Source = _entityH;
            this.txtPaymentNo.SetBinding(TextBox.TextProperty, BindingPaymentNo);

            Binding BindingPaymentState = new Binding("_payment_state");
            BindingPaymentState.Mode = BindingMode.TwoWay;
            BindingPaymentState.Source = _entityH;
            this.txtPaymentState.SetBinding(TextBox.TextProperty, BindingPaymentState);

            Binding BindingPaymentCashtState = new Binding("_payment_cash_receivable_kbn");
            BindingPaymentCashtState.Mode = BindingMode.TwoWay;
            BindingPaymentCashtState.Source = _entityH;
            this.txtPaymentCashState.SetBinding(TextBox.TextProperty, BindingPaymentCashtState);

            Binding BindingSendId = new Binding("_send_kbn_id");
            BindingSendId.Mode = BindingMode.TwoWay;
            BindingSendId.Source = _entityH;
            this.utlSendKbn.txtID.SetBinding(TextBox.TextProperty, BindingSendId);

            Binding BindingSendName = new Binding("_send_kbn_nm");
            BindingSendName.Mode = BindingMode.TwoWay;
            BindingSendName.Source = _entityH;
            this.utlSendKbn.txtNm.SetBinding(TextBox.TextProperty, BindingSendName);

            if (_entityH._send_kbn_id == 0)
            {
                _entityH._send_kbn_id = 1;
            }

            Binding BindingCustomeNo = new Binding("_customer_id");
            BindingCustomeNo.Mode = BindingMode.TwoWay;
            BindingCustomeNo.Source = _entityH;
            this.utlCustomer.txtID.SetBinding(TextBox.TextProperty, BindingCustomeNo);

            Binding BindingCustomeName = new Binding("_customer_name");
            BindingCustomeName.Mode = BindingMode.TwoWay;
            BindingCustomeName.Source = _entityH;
            this.utlCustomer.txtNm.SetBinding(TextBox.TextProperty, BindingCustomeName);

            Binding BindingSupplyNo = new Binding("_supplier_id");
            BindingSupplyNo.Mode = BindingMode.TwoWay;
            BindingSupplyNo.Source = _entityH;
            this.utlSupplier.txtID.SetBinding(TextBox.TextProperty, BindingSupplyNo);
            this.utlSupplier.txtID2.SetBinding(TextBox.TextProperty, BindingCustomeNo);

            Binding BindingSupplyName = new Binding("_supplier_name");
            BindingSupplyName.Mode = BindingMode.TwoWay;
            BindingSupplyName.Source = _entityH;
            this.utlSupplier.txtNm.SetBinding(TextBox.TextProperty, BindingSupplyName);

            Binding BindingTax = new Binding("_tax_change_id");
            BindingTax.Mode = BindingMode.TwoWay;
            BindingTax.Source = _entityH;
            this.utlTax.txtID.SetBinding(TextBox.TextProperty, BindingTax);

            Binding BindingTaxName = new Binding("_tax_change_name");
            BindingTaxName.Mode = BindingMode.TwoWay;
            BindingTaxName.Source = _entityH;
            this.utlTax.txtNm.SetBinding(TextBox.TextProperty, BindingTaxName);

            Binding BindingBusiness = new Binding("_business_division_id");
            BindingBusiness.Mode = BindingMode.TwoWay;
            BindingBusiness.Source = _entityH;
            this.utlBusiness.txtID.SetBinding(TextBox.TextProperty, BindingBusiness);

            Binding BindingBusinessName = new Binding("_business_division_name");
            BindingBusinessName.Mode = BindingMode.TwoWay;
            BindingBusinessName.Source = _entityH;
            this.utlBusiness.txtNm.SetBinding(TextBox.TextProperty, BindingBusinessName);

            Binding BindingUnitKindId = new Binding("_unit_kind_id");
            BindingUnitKindId.Mode = BindingMode.TwoWay;
            BindingUnitKindId.Source = _entityH;
            this.utlUnitKind.txtID.SetBinding(TextBox.TextProperty, BindingUnitKindId);

            Binding BindingCreditRate = new Binding("_credit_rate");
            BindingCreditRate.Mode = BindingMode.TwoWay;
            BindingCreditRate.Source = _entityH;
            this.txtCreditRate.SetBinding(TextBox.TextProperty, BindingCreditRate);

            Binding BindingNokiYmd = new Binding("_supply_ymd");
            BindingNokiYmd.Mode = BindingMode.TwoWay;
            BindingNokiYmd.Source = _entityH;
            this.datNokiYmd.SetBinding(DatePicker.SelectedDateProperty, BindingNokiYmd);

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
            
            // 単価計
            Binding BindingUnitPrice = new Binding("_sum_unit_price");
            BindingUnitPrice.Mode = BindingMode.TwoWay;
            BindingUnitPrice.Source = _entityH;
            BindingUnitPrice.Converter = nmConvDecm2;
            this.txtUnitPrice.SetBinding(TextBox.TextProperty, BindingUnitPrice);
            
            // 消費税額計
            Binding BindingSumTax = new Binding("_sum_tax");
            BindingSumTax.Mode = BindingMode.TwoWay;
            BindingSumTax.Source = _entityH;
            BindingSumTax.Converter = nmConvDecm0;
            this.txtTax.SetBinding(TextBox.TextProperty, BindingSumTax);
            
            // 税抜金額計
            Binding BindingTaxNoPrice = new Binding("_sum_no_tax_price");
            BindingTaxNoPrice.Mode = BindingMode.TwoWay;
            BindingTaxNoPrice.Source = _entityH;
            BindingTaxNoPrice.Converter = nmConvDecm0;
            this.txtTaxNoPrice.SetBinding(TextBox.TextProperty, BindingTaxNoPrice);

            // 税込金額計
            this.txtSumPrice.Text = ExCast.zCStr(ExCast.zCDbl(_entityH._sum_no_tax_price) + ExCast.zCDbl(_entityH._sum_tax));
            this.txtSumPrice.OnFormatString();

            // 金額計
            Binding BindingPrice = new Binding("_sum_price");
            BindingPrice.Mode = BindingMode.TwoWay;
            BindingPrice.Source = _entityH;
            BindingPrice.Converter = nmConvDecm0;
            this.txtPrice.SetBinding(TextBox.TextProperty, BindingPrice);
            
            // 買掛残高
            Binding BindingPaymentCreditPrice = new Binding("_payment_credit_price");
            BindingPaymentCreditPrice.Mode = BindingMode.TwoWay;
            BindingPaymentCreditPrice.Source = _entityH;
            BindingPaymentCreditPrice.Converter = nmConvDecm0;
            this.txtPaymentLimitPrice.SetBinding(TextBox.TextProperty, BindingPaymentCreditPrice);

            #endregion

            this.utlPurchaseOrderNo.txtID.OnFormatString();
            this.utlPurchaseOrderNo.txtID.SetZeroToNullString();
            this.utlPurchase.txtID.SetZeroToNullString();
            this.utlCustomer.txtID.SetZeroToNullString();
            this.utlSupplier.txtID.SetZeroToNullString();
            this.utlPerson.txtID.SetZeroToNullString();
            this.utlTax.txtID.SetZeroToNullString();
            this.utlBusiness.txtID.SetZeroToNullString();

            this.txtPaymentNo.Text = ExCast.zFormatForID(this.txtPaymentNo.Text, Common.gintidFigureSlipNo);
            this.txtPaymentLimitPrice.OnFormatString();
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

        #region 発注ヘッダデータ取得

        // ヘッダデータ取得
        private void GetHeadPurchaseOrder(long _No)
        {
            object[] prm = new object[2];
            prm[0] = _No.ToString();
            prm[1] = _No.ToString();
            webService.objPerent = this;
            webService.CallWebService(ExWebService.geWebServiceCallKbn.GetPurchaseOrderListH,
                                      ExWebService.geDialogDisplayFlg.Yes,
                                      ExWebService.geDialogCloseFlg.No,
                                      prm);
        }

        #endregion

        #region 発注明細データ取得

        // 明細データ取得
        private void GetDetailOrder(long _No)
        {
            object[] prm = new object[1];
            prm[0] = _No.ToString();
            webService.objPerent = this;
            webService.CallWebService(ExWebService.geWebServiceCallKbn.GetPurchaseOrderListD,
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
                #region 仕入

                // 仕入ヘッダ
                case _GetHeadWebServiceCallKbn:
                    // 更新
                    if (objList != null)
                    {
                        _entityH = (EntityPurchaseH)objList;

                        // エラー発生時
                        if (_entityH._message != "" && _entityH._message != null)
                        {
                            webService.ProcessingDlgClose();
                            this.utlNo.txtID.Text = "";
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
                        webService.ProcessingDlgClose();
                        this.utlFunctionKey.gFunctionKeyEnable = Utl_FunctionKey.geFunctionKeyEnable.New;
                        //this.utlNo.txtID_IsReadOnly = true;
                        //this.utlNo.IsEnabled = false;
                        this.utlNo.txtID_IsReadOnly = false;
                    }
                    ExBackgroundWorker.DoWork_Focus(this.utlPerson.txtID, 10);
                    break;
                // 仕入明細
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
                        _entityListD = (ObservableCollection<EntityPurchaseD>)objList;
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

                    if (_entityH._payment_close_flg == 1)
                    {
                        this.utlFunctionKey.gFunctionKeyEnable = Utl_FunctionKey.geFunctionKeyEnable.Sel;
                        this.utlMode.txtMode.Text = " 参照モード(支払締切済) ";
                        return;
                    }

                    if (_entityH._payment_cash_receivable_kbn_nm == "消込未" || _entityH._payment_cash_receivable_kbn_nm == "")
                    {
                    }
                    else
                    {
                        this.utlFunctionKey.gFunctionKeyEnable = Utl_FunctionKey.geFunctionKeyEnable.Sel;
                        this.utlMode.txtMode.Text = " 参照モード(" + _entityH._payment_cash_receivable_kbn_nm + ") ";
                        return;
                    }

                    if (_entityH._lock_flg == 0)
                    {
                        this.utlFunctionKey.gFunctionKeyEnable = Utl_FunctionKey.geFunctionKeyEnable.Upd;
                        ConvertRedData(_entityH, _entityListD, _entityRedH, _entityListRedD);
                        _entityRedH._no = 0;
                        _entityRedH._id = 0;
                        //_entityRedH._red_before_no = _entityH._no;
                    }
                    else
                    {
                        this.utlFunctionKey.gFunctionKeyEnable = Utl_FunctionKey.geFunctionKeyEnable.Sel;
                    }

                    // 買掛再計算
                    CalcPaymentCredit();

                    ExBackgroundWorker.DoWork_Focus(this.utlPerson.txtID, 10);
                    this.utlNo.txtID_IsReadOnly = true;
                    //this.utlNo.IsEnabled = false;
                    break;

                #endregion

                #region 発注

                // 発注ヘッダ
                case ExWebService.geWebServiceCallKbn.GetOrderListH:
                    // 更新
                    if (objList != null)    
                    {
                        _EntityPurchaseOrderH = (EntityPurchaseOrderH)objList;

                        // エラー発生時
                        if (_entityH._message != "" && _entityH._message != null)
                        {
                            webService.ProcessingDlgClose();
                            this.utlPurchaseOrderNo.txtID.Text = "";
                            ExBackgroundWorker.DoWork_Focus(this.utlPurchaseOrderNo.txtID, 10);
                            return;
                        }

                        if (_EntityPurchaseOrderH._purchase_allocation_flg == 1)
                        {
                            MessageBox.Show("仕入計上済の為、発注番号：" + this.utlPurchaseOrderNo.txtID.Text + " は選択できません。");
                            webService.ProcessingDlgClose();
                            this.utlPurchaseOrderNo.txtID.Text = "";
                            ExBackgroundWorker.DoWork_Focus(this.utlPurchaseOrderNo.txtID, 10);
                            return;
                        }

                        // 明細データ取得
                        GetDetailOrder(_EntityPurchaseOrderH._id);
                    }
                    // 新規
                    else
                    {
                        MessageBox.Show("発注番号：" + this.utlPurchaseOrderNo.txtID.Text + " は存在しません。");
                        webService.ProcessingDlgClose();
                        this.utlPurchaseOrderNo.txtID.Text = "";
                        ExBackgroundWorker.DoWork_Focus(this.utlPurchaseOrderNo.txtID, 10);
                    }
                    break;
                // 発注明細
                case ExWebService.geWebServiceCallKbn.GetOrderListD:
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
                        _entityListOrderD = (ObservableCollection<EntityPurchaseOrderD>)objList;
                    }
                    else 
                    {
                        _entityListOrderD = null;
                    }

                    // バインド反映
                    SetBinding();

                    ConverPurchaseOrderToSales(this._entityH, this._entityListD, this._EntityPurchaseOrderH, this._entityListOrderD);

                    // 明細再計算
                    DataDetail.IsCalcPrice = false;
                    DataDetail.ReCalcDetail(_entityH, _entityListD);
                    DataDetail.IsCalcPrice = true;


                    // 買掛再計算
                    CalcPaymentCredit();

                    if (string.IsNullOrEmpty(_entityH._memo))
                    {
                        _entityH._memo = "発注No : " + ExCast.zFormatForID(_EntityPurchaseOrderH._no, Common.gintidFigureSlipNo);
                    }

                    this.dg.ItemsSource = _entityListD;
                    ExBackgroundWorker.DoWork_Focus(this.datNokiYmd, 10);

                    this.utlPurchaseOrderNo.txtID.OnFormatString();
                    this.utlPurchaseOrderNo.txtID.UpdataFlg = false;

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
                #region 排他チェック

                if (Common.gblnDesynchronizeLock == true)
                {
                    errMessage += "現在排他処理中です。再度実行してください。" + Environment.NewLine;
                }

                #endregion

                #region ヘッダチェック

                #region 必須チェック

                // 仕入日
                if (string.IsNullOrEmpty(_entityH._purchase_ymd))
                {
                    errMessage += "仕入日が入力されていません。" + Environment.NewLine;
                    if (errCtl == null) errCtl = this.datPurchaseYmd;
                }

                // 入力担当者
                if (_entityH._update_person_id == 0)
                {
                    errMessage += "入力担当者が入力(選択)されていません。" + Environment.NewLine;
                    if (errCtl == null) errCtl = this.utlPerson.txtID;
                }

                // 仕入先
                if (string.IsNullOrEmpty(_entityH._purchase_id))
                {
                    errMessage += "仕入先が入力(選択)されていません。" + Environment.NewLine;
                    if (errCtl == null) errCtl = this.utlPurchase.txtID;
                }

                // 税転換
                if (_entityH._tax_change_id == 0)
                {
                    errMessage += "税転換が入力(選択)されていません。" + Environment.NewLine;
                    if (errCtl == null) errCtl = this.utlTax.txtID;
                }

                // 取引区分
                if (_entityH._business_division_id == 0)
                {
                    errMessage += "取引区分が入力(選択)されていません。" + Environment.NewLine;
                    if (errCtl == null) errCtl = this.utlBusiness.txtID;
                }

                // 発送区分
                if (_entityH._send_kbn_id == 0)
                {
                    errMessage += "発送区分が入力(選択)されていません。" + Environment.NewLine;
                    if (errCtl == null) errCtl = this.utlSendKbn.txtID;
                }

                #endregion

                #region 入力チェック

                // 入力担当者
                if (_entityH._update_person_id != 0 && string.IsNullOrEmpty(this.utlPerson.txtNm.Text.Trim()))
                {
                    errMessage += "入力担当者が適切に入力(選択)されていません。" + Environment.NewLine;
                    if (errCtl == null) errCtl = this.utlPerson.txtID;
                }

                // 仕入先
                if (!string.IsNullOrEmpty(_entityH._purchase_id) && string.IsNullOrEmpty(this.utlPurchase.txtNm.Text.Trim()))
                {
                    errMessage += "仕入先が適切に入力(選択)されていません。" + Environment.NewLine;
                    if (errCtl == null) errCtl = this.utlPurchase.txtID;
                }

                // 発送区分
                if (_entityH._send_kbn_id != 0 && string.IsNullOrEmpty(this.utlSendKbn.txtNm.Text.Trim()))
                {
                    errMessage += "発送区分が適切に入力(選択)されていません。" + Environment.NewLine;
                    if (errCtl == null) errCtl = this.utlSendKbn.txtID;
                }

                // 得意先
                if (!string.IsNullOrEmpty(_entityH._customer_id) && string.IsNullOrEmpty(this.utlCustomer.txtNm.Text.Trim()))
                {
                    errMessage += "得意先が適切に入力(選択)されていません。" + Environment.NewLine;
                    if (errCtl == null) errCtl = this.utlCustomer.txtID;
                }

                // 納入先
                if (!string.IsNullOrEmpty(_entityH._supplier_id) && string.IsNullOrEmpty(this.utlSupplier.txtNm.Text.Trim()))
                {
                    errMessage += "納入先が適切に入力(選択)されていません。" + Environment.NewLine;
                    if (errCtl == null) errCtl = this.utlSupplier.txtID;
                }

                // 税転換
                if (_entityH._tax_change_id != 0 && string.IsNullOrEmpty(this.utlTax.txtNm.Text.Trim()))
                {
                    errMessage += "税転換が適切に入力(選択)されていません。" + Environment.NewLine;
                    if (errCtl == null) errCtl = this.utlTax.txtID;
                }

                // 取引区分
                if (_entityH._business_division_id != 0 && string.IsNullOrEmpty(this.utlBusiness.txtNm.Text.Trim()))
                {
                    errMessage += "取引区分が適切に入力(選択)されていません。" + Environment.NewLine;
                    if (errCtl == null) errCtl = this.utlBusiness.txtID;
                }

                #endregion

                #region 日付チェック

                // 仕入日
                if (string.IsNullOrEmpty(_entityH._purchase_ymd) == false)
                {
                    if (ExCast.IsDate(_entityH._purchase_ymd) == false)
                    {
                        errMessage += "仕入日の形式が不正です。(yyyy/mm/dd形式で入力(選択)して下さい)" + Environment.NewLine;
                        if (errCtl == null) errCtl = this.datPurchaseYmd;
                    }
                }

                // 納入指定日
                if (string.IsNullOrEmpty(_entityH._supply_ymd) == false)
                {
                    if (ExCast.IsDate(_entityH._supply_ymd) == false)
                    {
                        errMessage += "納入指定日の形式が不正です。(yyyy/mm/dd形式で入力(選択)して下さい)" + Environment.NewLine;
                        if (errCtl == null) errCtl = this.datNokiYmd;
                    }
                }

                #endregion

                #region 日付変換

                // 仕入日
                if (string.IsNullOrEmpty(_entityH._purchase_ymd) == false)
                {
                    _entityH._purchase_ymd = ExCast.zConvertToDate(_entityH._purchase_ymd).ToString("yyyy/MM/dd");

                }

                // 納入指定日
                if (string.IsNullOrEmpty(_entityH._supply_ymd) == false)
                {
                    _entityH._supply_ymd = ExCast.zConvertToDate(_entityH._supply_ymd).ToString("yyyy/MM/dd");
                }

                #endregion

                #region 正数チェック

                if (ExCast.zCLng(_entityH._no) < 0)
                {
                    errMessage += "仕入番号には正の整数を入力して下さい。" + Environment.NewLine;
                }

                #endregion

                #region 範囲チェック

                if (ExCast.zCLng(_entityH._no) > 999999999999999)
                {
                    errMessage += "仕入番号には15桁以内の正の整数を入力して下さい。" + Environment.NewLine;
                }

                if (ExCast.zCLng(_entityH._purchase_order_no) > 999999999999999)
                {
                    errMessage += "発注番号には15桁以内の正の整数を入力して下さい。" + Environment.NewLine;
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
                    if (_entityListD[i]._breakdown_id <= 3 && string.IsNullOrEmpty(_entityListD[i]._commodity_id))
                    {
                        #region 商品未選択チェック

                        // 摘要以外で商品未選択は登録されない
                        if (!string.IsNullOrEmpty(_entityListD[i]._commodity_name) ||
                            _entityListD[i]._unit_id != 1 ||
                            _entityListD[i]._enter_number != 0 ||
                            _entityListD[i]._case_number != 0 ||
                            _entityListD[i]._number != 0 ||
                            _entityListD[i]._unit_price != 0 ||
                            _entityListD[i]._price != 0 ||
                            _entityListD[i]._tax_division_id != 1 ||
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

                        // 内訳
                        if (string.IsNullOrEmpty(_entityListD[i]._breakdown_nm))
                        {
                            errMessage += (i + 1).ToString() + "行目の内訳を選択して下さい。" + Environment.NewLine;
                            if (errCtl == null)
                            {
                                errCtl = this.dg;
                                _selectIndex = i;
                                _selectColumn = 3;
                            }
                        }

                        switch (ExCast.zCStr(_entityListD[i]._breakdown_nm))
                        {
                            case "通常":       // 通常
                            case "値引":       // 値引
                            case "諸経費":     // 諸経費
                                // 単価
                                if (ExCast.zCDbl(_entityListD[i]._unit_price) == 0)
                                {
                                    errMessage += (i + 1).ToString() + "行目の単価が0です。" + Environment.NewLine;
                                    if (errCtl == null)
                                    {
                                        errCtl = this.dg;
                                        _selectIndex = i;
                                        _selectColumn = 9;
                                    }
                                }
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
                                // 金額
                                if (ExCast.zCDbl(_entityListD[i]._price) == 0)
                                {
                                    errMessage += (i + 1).ToString() + "行目の金額が0です。" + Environment.NewLine;
                                    if (errCtl == null)
                                    {
                                        errCtl = this.dg;
                                        _selectIndex = i;
                                        _selectColumn = 10;
                                    }
                                }
                                // 課税区分
                                if (ExCast.zCStr(_entityListD[i]._tax_division_nm) == "")
                                {
                                    errMessage += (i + 1).ToString() + "行目の課税区分を選択して下さい。" + Environment.NewLine;
                                    if (errCtl == null)
                                    {
                                        errCtl = this.dg;
                                        _selectIndex = i;
                                        _selectColumn = 11;
                                    }
                                }
                                break;
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
                        // 単価
                        if (ExCast.IsNumeric(_entityListD[i]._unit_price) == false)
                        {
                            errMessage += (i + 1).ToString() + "行目の単価に数値以外が入力されています。" + Environment.NewLine;
                            if (errCtl == null)
                            {
                                errCtl = this.dg;
                                _selectIndex = i;
                                _selectColumn = 9;
                            }
                        }
                        // 金額
                        if (ExCast.IsNumeric(_entityListD[i]._price) == false)
                        {
                            errMessage += (i + 1).ToString() + "行目の金額に数値以外が入力されています。" + Environment.NewLine;
                            if (errCtl == null)
                            {
                                errCtl = this.dg;
                                _selectIndex = i;
                                _selectColumn = 10;
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

                        // 単価
                        if (ExCast.zCDbl(_entityListD[i]._unit_price) > 99999999)
                        {
                            errMessage += (i + 1).ToString() + "行目の単価に99,999,999以内の数値を入力して下さい。" + Environment.NewLine;
                            if (errCtl == null)
                            {
                                errCtl = this.dg;
                                _selectIndex = i;
                                _selectColumn = 9;
                            }
                        }
                        if (ExCast.zCDbl(_entityListD[i]._unit_price) < -99999999)
                        {
                            errMessage += (i + 1).ToString() + "行目の単価に-99,999,999以上の数値を入力して下さい。" + Environment.NewLine;
                            if (errCtl == null)
                            {
                                errCtl = this.dg;
                                _selectIndex = i;
                                _selectColumn = 9;
                            }
                        }

                        // 金額
                        if (ExCast.zCDbl(_entityListD[i]._price) > 99999999999)
                        {
                            errMessage += (i + 1).ToString() + "行目の金額に99,999,999,999以内の数値を入力して下さい。" + Environment.NewLine;
                            if (errCtl == null)
                            {
                                errCtl = this.dg;
                                _selectIndex = i;
                                _selectColumn = 10;
                            }
                        }
                        if (ExCast.zCDbl(_entityListD[i]._price) < -99999999999)
                        {
                            errMessage += (i + 1).ToString() + "行目の金額に-99,999,999,999以上の数値を入力して下さい。" + Environment.NewLine;
                            if (errCtl == null)
                            {
                                errCtl = this.dg;
                                _selectIndex = i;
                                _selectColumn = 10;
                            }
                        }

                        // 備考
                        if (ExString.LenB(_entityListD[i]._memo) > 14)
                        {
                            errMessage += (i + 1).ToString() + "行目の備考に全角7桁文字以内(半角14桁文字以内)を入力して下さい。" + Environment.NewLine;
                            if (errCtl == null)
                            {
                                errCtl = this.dg;
                                _selectIndex = i;
                                _selectColumn = 12;
                            }
                        }

                        // 課税区分
                        if (ExCast.zCInt(_entityListD[i]._tax_division_id) == 1 && ExCast.zCStr(_entityListD[i]._tax_division_nm) != "課税")
                        {
                            errMessage += (i + 1).ToString() + "行目の課税区分が不正です。(課税で1以外)" + Environment.NewLine;
                            if (errCtl == null)
                            {
                                errCtl = this.dg;
                                _selectIndex = i;
                                _selectColumn = 12;
                            }
                        }

                        if (ExCast.zCInt(_entityListD[i]._tax_division_id) == 2 && ExCast.zCStr(_entityListD[i]._tax_division_nm) != "非課税")
                        {
                            errMessage += (i + 1).ToString() + "行目の課税区分が不正です。(非課税で2以外)" + Environment.NewLine;
                            if (errCtl == null)
                            {
                                errCtl = this.dg;
                                _selectIndex = i;
                                _selectColumn = 12;
                            }
                        }

                        #endregion

                        #region 在庫チェック

                        // 在庫
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
                    errMessage += "明細の登録対象データが選択されていません。" + Environment.NewLine;
                    if (errCtl == null)
                    {
                        errCtl = this.dg;
                        _selectIndex = 0;
                        _selectColumn = 1;
                    }
                }

                #endregion

                #region 掛仕入チェック

                //// 掛仕入 or 都度支払
                //if ((_entityH._business_division_id == 1 || _entityH._business_division_id == 3) && _entityH._credit_limit_price != 0)
                //{
                //    //if (_entityH._credit_limit_price < _entityH._sales_credit_price + _entityH._sum_no_tax_price + _entityH._sum_tax)
                //    if (_entityH._credit_limit_price < _entityH._sales_credit_price)
                //    {
                //        warnMessage += "買掛残高が与信限度額を超えます。" + Environment.NewLine;
                //        if (errCtl == null)
                //        {
                //            errCtl = this.dg;
                //            _selectIndex = 0;
                //            _selectColumn = 1;
                //        }
                //    }
                //}

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
                        flg = false;
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

        // 入力チェック
        private void InputCheckDelete()
        {
            try
            {
                if (this.utlNo.txtID.Text.Trim() == "")
                {
                    ExMessageBox.Show("仕入データが選択されていません。");
                    return;
                }

                if (this._entityH == null)
                {
                    ExMessageBox.Show("仕入データが選択されていません。");
                    return;
                }

                if (this._entityH._id == 0)
                {
                    ExMessageBox.Show("仕入データが選択されていません。");
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

        #endregion

        #region DataGrid Events

        private void dg_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            try
            {
                beforeValue = "";
                beforeSelectedIndex = -1;
                beforeSelectedIndex = this.dg.SelectedIndex;

                EntityPurchaseD entity = (EntityPurchaseD)e.Row.DataContext;
                switch (e.Column.DisplayIndex)
                {
                    case 1:         // 内訳
                        beforeValue = entity._breakdown_nm;
                        break;
                    case 2:         // 納品区分
                        beforeValue = entity._deliver_division_nm;
                        break;
                    case 3:         // 商品コード
                        beforeValue = entity._commodity_id;
                        break;
                    case 4:         // 商品名
                        beforeValue = entity._commodity_name;
                        break;
                    case 5:         // 単位
                        beforeValue = entity._unit_nm;
                        break;
                    case 6:         // 入数
                        beforeValue = ExCast.zCStr(entity._enter_number);
                        break;
                    case 7:         // ケース数
                        beforeValue = ExCast.zCStr(entity._case_number);
                        break;
                    case 8:         // 数量
                        beforeValue = ExCast.zCStr(entity._number);
                        break;
                    case 9:         // 単価
                        beforeValue = ExCast.zCStr(entity._unit_price);
                        break;
                    case 10:         // 金額
                        beforeValue = ExCast.zCStr(entity._price);
                        break;
                    case 11:         // 備考
                        beforeValue = ExCast.zCStr(entity._memo);
                        break;
                    case 12:         // 課税区分
                        beforeValue = ExCast.zCStr(entity._tax_division_nm);
                        break;
                }
            }
            catch
            {
            }
        }

        private void dg_CellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
        {
            EntityPurchaseD entity = (EntityPurchaseD)e.Row.DataContext;

            // コンボボックスID連動
            switch (e.Column.DisplayIndex)
            {
                case 1:         // 内訳
                    if (_entityListD == null) return;
                    if (_entityListD.Count >= dg.SelectedIndex && dg.SelectedIndex != -1)
                    {
                        _entityListD[dg.SelectedIndex]._breakdown_id = MeiNameList.GetID(MeiNameList.geNameKbn.BREAKDOWN_ID, ExCast.zCStr(entity._breakdown_nm));
                        // 消費税
                        if (_entityListD[dg.SelectedIndex]._breakdown_id == 5)
                        {
                            _entityListD[dg.SelectedIndex]._tax_division_nm = "非課税";
                            _entityListD[dg.SelectedIndex]._tax_division_id = MeiNameList.GetID(MeiNameList.geNameKbn.TAX_DIVISION_ID, ExCast.zCStr(_entityListD[dg.SelectedIndex]._tax_division_nm));
                        }
                        // 返品
                        else if (_entityListD[dg.SelectedIndex]._breakdown_id == 6)
                        {
                            DataDetail.CalRedcDetail(_entityListD[dg.SelectedIndex]);
                            DataDetail.CalcSumDetail(_entityH, _entityListD);
                        }
                    }
                    break;
                case 2:         // 納品区分
                    if (_entityListD == null) return;
                    if (_entityListD.Count >= dg.SelectedIndex && dg.SelectedIndex != -1)
                    {
                        _entityListD[dg.SelectedIndex]._deliver_division_id = MeiNameList.GetID(MeiNameList.geNameKbn.DELIVER_DIVISION_ID, ExCast.zCStr(entity._deliver_division_nm));
                        DataDetail.CalcSumDetail(_entityH, _entityListD);       　// 明細合計計算
                    }
                    break;
                case 5:         // 単価
                    if (_entityListD == null) return;
                    if (_entityListD.Count >= dg.SelectedIndex && dg.SelectedIndex != -1)
                        _entityListD[dg.SelectedIndex]._unit_id = MeiNameList.GetID(MeiNameList.geNameKbn.UNIT_ID, ExCast.zCStr(entity._unit_nm));
                    break;
                case 12:        // 課税区分
                    if (_entityListD == null) return;
                    if (_entityListD.Count >= dg.SelectedIndex && dg.SelectedIndex != -1)
                    {
                        _entityListD[dg.SelectedIndex]._tax_division_id = MeiNameList.GetID(MeiNameList.geNameKbn.TAX_DIVISION_ID, ExCast.zCStr(entity._tax_division_nm));
                        // 課税選択で内訳が消費税の場合
                        if (_entityListD[dg.SelectedIndex]._tax_division_id == 1 && _entityListD[dg.SelectedIndex]._breakdown_id == 5)
                        {
                            ExMessageBox.Show("内訳が消費税の場合、課税区分に課税を選択できません。");
                            _entityListD[dg.SelectedIndex]._tax_division_nm = "非課税";
                            _entityListD[dg.SelectedIndex]._tax_division_id = MeiNameList.GetID(MeiNameList.geNameKbn.TAX_DIVISION_ID, ExCast.zCStr(_entityListD[dg.SelectedIndex]._tax_division_nm));
                        }
                    }
                    break;
            }

            if (beforeSelectedIndex == -1) return;

            // 明細計算
            switch (e.Column.DisplayIndex)
            {
                case 3:         // 商品コード
                    if (beforeValue == entity._commodity_id) return;
                    if (Common.gblnDesynchronizeLock == true) return;
                    Common.gblnDesynchronizeLock = true;
                    entity._commodity_id = ExCast.zFormatForID(entity._commodity_id, Common.gintidFigureCommodity);
                    MstData _mstData = new MstData();
                    _mstData.GetMData(MstData.geMDataKbn.Commodity, new string[] { entity._commodity_id, ExCast.zCStr(beforeSelectedIndex) }, this);
                    break;
                case 6:         // 入数
                    if (beforeValue == ExCast.zCStr(entity._enter_number)) return;
                    DataDetail.CalcDetailNumber(beforeSelectedIndex, _entityH, _entityListD);   // 明細数量計算
                    break;
                case 7:         // ケース数
                    if (beforeValue == ExCast.zCStr(entity._case_number)) return;
                    DataDetail.CalcDetailNumber(beforeSelectedIndex, _entityH, _entityListD);   // 明細数量計算
                    break;
                case 8:         // 数量
                    if (beforeValue == ExCast.zCStr(entity._number)) return;
                    DataDetail.CalcDetail(beforeSelectedIndex, _entityH, _entityListD);       　// 明細計算
                    //OrderDetailData.CalcDetailNumber(dg.SelectedIndex, _entityH, _entityListD);   // 明細数量計算
                    break;
                case 9:         // 単価
                    if (beforeValue == ExCast.zCStr(entity._unit_price)) return;
                    DataDetail.CalcDetail(beforeSelectedIndex, _entityH, _entityListD);         // 明細計算
                    break;
                case 10:        // 金額
                    if (beforeValue == ExCast.zCStr(entity._price)) return;
                    DataDetail.IsCalcPrice = false;
                    DataDetail.CalcDetail(beforeSelectedIndex, _entityH, _entityListD);         // 明細計算
                    DataDetail.IsCalcPrice = true;
                    break;
                case 12:        // 課税区分
                    if (beforeValue == ExCast.zCStr(entity._tax_division_nm)) return;
                    DataDetail.CalcDetail(beforeSelectedIndex, _entityH, _entityListD);         // 明細計算
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

                if (ExCast.zCLng(this._entityH._purchase_order_no) != 0)
                {
                    this.txtOrderStillNumber.Text = ExCast.zCStr(_entityListD[dg.SelectedIndex]._purchase_order_stay_number);
                }
                else
                {
                    this.txtOrderStillNumber.Text = "";
                }
            }
            else
            {
                txtInventory.Text = "";
                this.txtOrderStillNumber.Text = "";
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

        private void txtUnitPrice_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            _selectIndex = dg.SelectedIndex;
            _selectColumn = dg.CurrentColumn.DisplayIndex;
            beforeValue = "";
            if (_entityListD == null) return;
            if (_entityListD.Count >= _selectIndex)
            {
                beforeValue = ExCast.zCStr(_entityListD[_selectIndex]._unit_price);
            }

            Dlg_UnitPriceSetting unitPriceDlg = new Dlg_UnitPriceSetting();
            unitPriceDlg.kbn = Dlg_UnitPriceSetting.eKbn.Purchase;

            // 税転換が内税で明細課税有りの場合
            if ((_entityH._tax_change_id == 4 || _entityH._tax_change_id == 5 || _entityH._tax_change_id == 6) && _entityListD[_selectIndex]._tax_division_id == 1)
            {
                unitPriceDlg.retail_price = _entityListD[_selectIndex]._retail_price_before_tax;
                unitPriceDlg.sales_unit_price = _entityListD[_selectIndex]._sales_unit_price_before_tax;
                unitPriceDlg.sales_cost_price = _entityListD[_selectIndex]._sales_cost_price_before_tax;
            }
            else
            {
                unitPriceDlg.retail_price = _entityListD[_selectIndex]._retail_price_skip_tax;
                unitPriceDlg.sales_unit_price = _entityListD[_selectIndex]._sales_unit_price_skip_tax;
                unitPriceDlg.sales_cost_price = _entityListD[_selectIndex]._sales_cost_price_skip_tax;
            }
            unitPriceDlg.unit_decimal_digit = _entityListD[_selectIndex]._unit_decimal_digit;
            unitPriceDlg.credit_rate = _entityH._credit_rate;
            unitPriceDlg.Closed += unitPriceDlg_Closed;
            unitPriceDlg.Show();
        }

        private void unitPriceDlg_Closed(object sender, EventArgs e)
        {
            Dlg_UnitPriceSetting unitPriceDlg = (Dlg_UnitPriceSetting)sender;
            if (unitPriceDlg.DialogResult == true)
            {
                if (_entityListD == null) return;
                if (_entityListD.Count >= _selectIndex)
                {
                    _entityListD[_selectIndex]._unit_price = unitPriceDlg.return_unit_price;
                }
            }
            if (beforeValue != ExCast.zCStr(_entityListD[_selectIndex]._unit_price))
            {
                DataDetail.CalcDetail(_selectIndex, _entityH, _entityListD);
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

        private void SetInitCombo(ref EntityPurchaseD entityD)
        {
            // コンボボックス初期選択
            List<string> lst;
            lst = MeiNameList.GetListMei(MeiNameList.geNameKbn.BREAKDOWN_ID);
            entityD._breakdown_nm = lst[0];
            entityD._breakdown_id = MeiNameList.GetID(MeiNameList.geNameKbn.BREAKDOWN_ID, lst[0]);

            lst = MeiNameList.GetListMei(MeiNameList.geNameKbn.DELIVER_DIVISION_ID);
            entityD._deliver_division_nm = lst[2];
            entityD._deliver_division_id = MeiNameList.GetID(MeiNameList.geNameKbn.DELIVER_DIVISION_ID, lst[2]);

            lst = MeiNameList.GetListMei(MeiNameList.geNameKbn.UNIT_ID);
            entityD._unit_nm = lst[0];
            entityD._unit_id = MeiNameList.GetID(MeiNameList.geNameKbn.UNIT_ID, lst[0]);

            lst = MeiNameList.GetListMei(MeiNameList.geNameKbn.TAX_DIVISION_ID);
            entityD._tax_division_nm = lst[0];
            entityD._tax_division_id = MeiNameList.GetID(MeiNameList.geNameKbn.TAX_DIVISION_ID, lst[0]);
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

                    string _before = _entityH._supplier_id;
                    _entityH._supplier_id = "";
                    _entityH._supplier_id = _before;

                    // 明細再計算
                    DataDetail.ReCalcDetail(_entityH, _entityListD);
                    Common.gblnDesynchronizeLock = false;
                    break;
                case "business_division_id":
                    // 現金仕入、都度支払時
                    if (_entityH._business_division_id == 2 || _entityH._business_division_id == 3)
                    {
                        switch (_entityH._tax_change_id)
                        {
                            case 0:
                            case 1:     // 外税伝票計
                            case 2:     // 外税明細単位
                            case 4:     // 内税伝票計
                            case 5:     // 内税伝票計
                            case 7:     // 非課税
                                break;
                            default:
                                if (_entityH._business_division_id == 2)
                                {
                                    ExMessageBox.Show("取引区分に「現金仕入」指定時は支払単位の税転換を指定できません。");
                                }
                                else
                                {
                                    ExMessageBox.Show("取引区分に「都度支払」指定時は支払単位の税転換を指定できません。");
                                }
                                _entityH._tax_change_id = 0;
                                break;
                        }
                    }

                    CalcPaymentCredit();

                    break;
                case "tax_change_id":
                    // 現金仕入、都度支払時
                    if (_entityH._business_division_id == 2 || _entityH._business_division_id == 3)
                    {
                        switch (_entityH._tax_change_id)
                        {
                            case 0:
                            case 1:     // 外税伝票計
                            case 2:     // 外税明細単位
                            case 4:     // 内税伝票計
                            case 5:     // 内税伝票計
                            case 7:     // 非課税
                                break;
                            default:
                                if (_entityH._business_division_id == 2)
                                {
                                    ExMessageBox.Show("取引区分に「現金仕入」指定時は支払単位の税転換を指定できません。");
                                }
                                else
                                {
                                    ExMessageBox.Show("取引区分に「都度支払」指定時は支払単位の税転換を指定できません。");
                                }
                                _entityH._tax_change_id = 0;
                                break;
                        }
                    }

                    if (_entityListD == null) return;
                    if (_entityListD.Count == 0) return;
                    if (dg.ItemsSource == null) return;

                    // 明細再計算
                    DataDetail.ReCalcDetail(_entityH, _entityListD);

                    break;
                case "sum_tax":
                case "sum_no_tax_price":
                    this.txtSumPrice.Text = ExCast.zCStr(ExCast.zCDbl(_entityH._sum_no_tax_price) + ExCast.zCDbl(_entityH._sum_tax));
                    this.txtSumPrice.OnFormatString();
                    CalcPaymentCredit();
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region Calc Payment Credit

        private void CalcPaymentCredit()
        {
            switch (this.utlFunctionKey.gFunctionKeyEnable)
            {
                case Utl_FunctionKey.geFunctionKeyEnable.Upd:
                    switch (_entityH._business_division_id)
                    {
                        case 1:
                        case 3:
                            double _before_credit_price = _before_entityH._payment_credit_price - (_before_entityH._sum_no_tax_price + _before_entityH._sum_tax);
                            this.txtPaymentLimitPrice.Text = ExCast.zCStr(_before_credit_price + ExCast.zCDbl(this.txtSumPrice.Text));
                            break;
                        default:
                            this.txtPaymentLimitPrice.Text = ExCast.zCStr(_entityH._before_payment_credit_price);
                            break;
                    }

                    break;
                default:
                    this.txtPaymentLimitPrice.Text = ExCast.zCStr(_entityH._before_payment_credit_price);
                    break;
            }
            this.txtPaymentLimitPrice.OnFormatString();
        }

        #endregion

        #region ConverPurchaseOrderToSales

        private void ConverPurchaseOrderToSales(EntityPurchaseH _EntityPurchaseH,
                                                ObservableCollection<EntityPurchaseD> _entitySalesListD,
                                                EntityPurchaseOrderH _EntityPurchaseOrderH,
                                                ObservableCollection<EntityPurchaseOrderD> _entityListOrderD)
        {
            #region Set Entity Head

            _EntityPurchaseH._purchase_order_no = _EntityPurchaseOrderH._no;
            //_EntityPurchaseH._sales_ymd = _EntityPurchaseOrderH.or;

            this.utlPurchase.Is_MstID_Changed = false;
            this.utlSupplier.Is_MstID_Changed = false;

            _EntityPurchaseH._customer_id = _EntityPurchaseOrderH._customer_id;
            _EntityPurchaseH._customer_name = _EntityPurchaseOrderH._customer_name;

            _EntityPurchaseH._supplier_id = _EntityPurchaseOrderH._supplier_id;
            _EntityPurchaseH._supplier_name = _EntityPurchaseOrderH._supplier_name;

            this.utlPurchase.Is_MstID_Changed = true;
            this.utlSupplier.Is_MstID_Changed = true;
            this.utlPurchase.MstID_Changed(_EntityPurchaseOrderH, new PropertyChangedEventArgs("customer_id"));
            this.utlSupplier.MstID_Changed(_EntityPurchaseOrderH, new PropertyChangedEventArgs("supplier_id"));

            _EntityPurchaseH._tax_change_id = _EntityPurchaseOrderH._tax_change_id;
            _EntityPurchaseH._tax_change_name = _EntityPurchaseOrderH._tax_change_name;

            _EntityPurchaseH._business_division_id = _EntityPurchaseOrderH._business_division_id;
            _EntityPurchaseH._business_division_name = _EntityPurchaseOrderH._business_division_name;

            _EntityPurchaseH._supply_ymd = _EntityPurchaseOrderH._supply_ymd;
            _EntityPurchaseH._sum_enter_number = _EntityPurchaseOrderH._sum_enter_number;
            _EntityPurchaseH._sum_case_number = _EntityPurchaseOrderH._sum_case_number;
            _EntityPurchaseH._sum_number = _EntityPurchaseOrderH._sum_number;
            _EntityPurchaseH._sum_unit_price = _EntityPurchaseOrderH._sum_unit_price;
            //_EntityPurchaseH._sum_sales_cost = _EntityPurchaseOrderH._sum_sales_cost;
            _EntityPurchaseH._sum_tax = _EntityPurchaseOrderH._sum_tax;
            _EntityPurchaseH._sum_no_tax_price = _EntityPurchaseOrderH._sum_no_tax_price;
            _EntityPurchaseH._sum_price = _EntityPurchaseOrderH._sum_price;
            //_EntityPurchaseH._sum_profits = _EntityPurchaseOrderH._sum_profits;
            //_EntityPurchaseH._profits_percent = _EntityPurchaseOrderH._profits_percent;
            //_EntityPurchaseH._credit_limit_price = _EntityPurchaseOrderH._credit_limit_price;
            _EntityPurchaseH._payment_credit_price = _EntityPurchaseOrderH._payment_credit_price;
            _EntityPurchaseH._before_payment_credit_price = _EntityPurchaseOrderH._payment_credit_price;

            _EntityPurchaseH._credit_rate = _EntityPurchaseOrderH._credit_rate;
            _EntityPurchaseH._unit_kind_id = _EntityPurchaseOrderH._unit_kind_id;

            _EntityPurchaseH._payment_state = "";
            _EntityPurchaseH._payment_cash_receivable_kbn = 1;
            _EntityPurchaseH._payment_cash_receivable_kbn_nm = "消込未";

            _EntityPurchaseH._memo = _EntityPurchaseOrderH._memo;

            //_EntityPurchaseH._update_person_id = ExCast.zCInt(dt.DefaultView[0]["UPDATE_PERSON_ID"]);
            //_EntityPurchaseH._update_person_nm = ExCast.zCStr(dt.DefaultView[0]["UPDATE_PERSON_NM"]);

            //_EntityPurchaseH._lock_flg = (int)lockFlg;

            #endregion

            #region Set Entity Detail

            if (_entitySalesListD == null) _entitySalesListD = new ObservableCollection<EntityPurchaseD>();
            _entitySalesListD.Clear();

            for (int i = 0; i <= _entityListOrderD.Count - 1; i++)
            {
                EntityPurchaseD _entityD = new EntityPurchaseD();
                _entityD._id = _EntityPurchaseH._id;
                _entityD._rec_no = _entityListOrderD[i]._rec_no;
                _entityD._breakdown_id = _entityListOrderD[i]._breakdown_id;
                _entityD._breakdown_nm = _entityListOrderD[i]._breakdown_nm;
                List<string> lst;
                lst = MeiNameList.GetListMei(MeiNameList.geNameKbn.DELIVER_DIVISION_ID);
                _entityD._deliver_division_nm = lst[2];
                _entityD._deliver_division_id = MeiNameList.GetID(MeiNameList.geNameKbn.DELIVER_DIVISION_ID, lst[2]);
                //_entityD._deliver_division_id = 2;
                //_entityD._deliver_division_nm = _entityListOrderD[i]._deliver_division_nm;
                _entityD._commodity_id = _entityListOrderD[i]._commodity_id;
                _entityD._commodity_name = _entityListOrderD[i]._commodity_name;
                _entityD._unit_id = _entityListOrderD[i]._unit_id;
                _entityD._unit_nm = _entityListOrderD[i]._unit_nm;
                _entityD._enter_number = _entityListOrderD[i]._enter_number;
                _entityD._case_number = _entityListOrderD[i]._case_number;
                _entityD._number = _entityListOrderD[i]._number;
                _entityD._purchase_order_id = _EntityPurchaseOrderH._id;
                _entityD._purchase_order_number = _entityListOrderD[i]._number;
                _entityD._purchase_order_stay_number = 0;
                _entityD._unit_price = _entityListOrderD[i]._unit_price;
                //_entityD._sales_cost = _entityListOrderD[i]._sales_cost;
                _entityD._tax = _entityListOrderD[i]._tax;
                _entityD._no_tax_price = _entityListOrderD[i]._no_tax_price;
                _entityD._price = _entityListOrderD[i]._price;
                //_entityD._profits = _entityListOrderD[i]._profits;
                //_entityD._profits_percent = _entityListOrderD[i]._profits_percent;
                _entityD._memo = _entityListOrderD[i]._memo;
                _entityD._tax_division_id = _entityListOrderD[i]._tax_division_id;
                _entityD._tax_division_nm = _entityListOrderD[i]._tax_division_nm;
                _entityD._tax_percent = _entityListOrderD[i]._tax_percent;
                _entityD._inventory_number = _entityListOrderD[i]._inventory_number;
                _entityD._inventory_management_division_id = _entityListOrderD[i]._inventory_management_division_id;
                _entityD._number_decimal_digit = _entityListOrderD[i]._number_decimal_digit;
                _entityD._unit_decimal_digit = _entityListOrderD[i]._unit_decimal_digit;
                _entitySalesListD.Add(_entityD);
            }

            #endregion
        }

        #endregion

        #region ConvertRedDate

        private void ConvertRedData(EntityPurchaseH _prm_entityH, 
                                    ObservableCollection<EntityPurchaseD> _prm_entityListD, 
                                    EntityPurchaseH _prm_entityRedH, 
                                    ObservableCollection<EntityPurchaseD> _prm_entityListRedD)
        {
            //for (int i = 0; i <= _prm_entityListD.Count - 1; i++)
            //{
            //    _prm_entityListRedD.Clear();

            //    #region Set Entity Detail

            //    EntityPurchaseD _entityD = new EntityPurchaseD();

            //    _entityD._id = _prm_entityListD[i]._id;
            //    _entityD._rec_no = _prm_entityListD[i]._rec_no;
            //    _entityD._breakdown_id = _prm_entityListD[i]._breakdown_id;
            //    _entityD._breakdown_nm = _prm_entityListD[i]._breakdown_nm;
            //    _entityD._deliver_division_id = _prm_entityListD[i]._deliver_division_id;
            //    _entityD._deliver_division_nm = _prm_entityListD[i]._deliver_division_nm;
            //    _entityD._commodity_id = _prm_entityListD[i]._commodity_id;
            //    _entityD._commodity_name = _prm_entityListD[i]._commodity_name;
            //    _entityD._unit_id = _prm_entityListD[i]._unit_id;
            //    _entityD._unit_nm = _prm_entityListD[i]._unit_nm;
            //    _entityD._enter_number = _prm_entityListD[i]._enter_number;
            //    _entityD._case_number = _prm_entityListD[i]._case_number;
            //    _entityD._number = _prm_entityListD[i]._number;
            //    _entityD._unit_price = _prm_entityListD[i]._unit_price;
            //    _entityD._sales_cost = _prm_entityListD[i]._sales_cost;
            //    _entityD._tax = _prm_entityListD[i]._tax;
            //    _entityD._no_tax_price = _prm_entityListD[i]._no_tax_price;
            //    _entityD._price = _prm_entityListD[i]._price;
            //    _entityD._profits = _prm_entityListD[i]._profits;
            //    _entityD._profits_percent = _prm_entityListD[i]._profits_percent;
            //    _entityD._tax_division_id = _prm_entityListD[i]._tax_division_id;
            //    _entityD._tax_division_nm = _prm_entityListD[i]._tax_division_nm;
            //    _entityD._tax_percent = _prm_entityListD[i]._tax_percent;
            //    _entityD._inventory_management_division_id = _prm_entityListD[i]._inventory_management_division_id;
            //    _entityD._inventory_number = _prm_entityListD[i]._inventory_number;
            //    _entityD._retail_price_skip_tax = _prm_entityListD[i]._retail_price_skip_tax;
            //    _entityD._retail_price_before_tax = _prm_entityListD[i]._retail_price_before_tax;
            //    _entityD._sales_unit_price_skip_tax = _prm_entityListD[i]._sales_unit_price_skip_tax;
            //    _entityD._sales_unit_price_before_tax = _prm_entityListD[i]._sales_unit_price_before_tax;
            //    _entityD._sales_cost_price_skip_tax = _prm_entityListD[i]._sales_cost_price_skip_tax;
            //    _entityD._sales_cost_price_before_tax = _prm_entityListD[i]._sales_cost_price_before_tax;
            //    _entityD._number_decimal_digit = _prm_entityListD[i]._number_decimal_digit;
            //    _entityD._unit_decimal_digit = _prm_entityListD[i]._unit_decimal_digit;
            //    _entityD._order_id = _prm_entityListD[i]._order_id;
            //    _entityD._order_number = _prm_entityListD[i]._order_number;
            //    _entityD._order_stay_number = _prm_entityListD[i]._order_stay_number;
            //    _entityD._memo = _prm_entityListD[i]._memo;
            //    _entityD._lock_flg = _prm_entityListD[i]._lock_flg;
            //    _entityD._message = _prm_entityListD[i]._message;

            //    #endregion

            //    _prm_entityListRedD.Add(_entityD);
            //}

            //#region Set Entity Head

            //_prm_entityRedH._id = _prm_entityH._id;
            //_prm_entityRedH._no = _prm_entityH._no;
            //_prm_entityRedH._red_before_no = _prm_entityH._red_before_no;
            //_prm_entityRedH._order_no = _prm_entityH._order_no;
            //_prm_entityRedH._estimateno = _prm_entityH._estimateno;
            //_prm_entityRedH._sales_ymd = _prm_entityH._sales_ymd;
            //_prm_entityRedH._customer_id = _prm_entityH._customer_id;
            //_prm_entityRedH._customer_name = _prm_entityH._customer_name;
            //_prm_entityRedH._tax_change_id = _prm_entityH._tax_change_id;
            //_prm_entityRedH._tax_change_name = _prm_entityH._tax_change_name;
            //_prm_entityRedH._business_division_id = _prm_entityH._business_division_id;
            //_prm_entityRedH._business_division_name = _prm_entityH._business_division_name;
            //_prm_entityRedH._supplier_id = _prm_entityH._supplier_id;
            //_prm_entityRedH._supplier_name = _prm_entityH._supplier_name;
            //_prm_entityRedH._supply_ymd = _prm_entityH._supply_ymd;
            //_prm_entityRedH._sum_enter_number = _prm_entityH._sum_enter_number;
            //_prm_entityRedH._sum_case_number = _prm_entityH._sum_case_number;
            //_prm_entityRedH._sum_number = _prm_entityH._sum_number;
            //_prm_entityRedH._sum_unit_price = _prm_entityH._sum_unit_price;
            //_prm_entityRedH._sum_sales_cost = _prm_entityH._sum_sales_cost;
            //_prm_entityRedH._sum_tax = _prm_entityH._sum_tax;
            //_prm_entityRedH._sum_no_tax_price = _prm_entityH._sum_no_tax_price;
            //_prm_entityRedH._sum_price = _prm_entityH._sum_price;
            //_prm_entityRedH._sum_profits = _prm_entityH._sum_profits;
            //_prm_entityRedH._profits_percent = _prm_entityH._profits_percent;
            //_prm_entityRedH._invoice_close_flg = _prm_entityH._invoice_close_flg;
            //_prm_entityRedH._memo = _prm_entityH._memo;
            //_prm_entityRedH._update_flg = _prm_entityH._update_flg;
            //_prm_entityRedH._update_person_id = _prm_entityH._update_person_id;
            //_prm_entityRedH._update_person_nm = _prm_entityH._update_person_nm;
            //_prm_entityRedH._update_date = _prm_entityH._update_date;
            //_prm_entityRedH._update_time = _prm_entityH._update_time;
            //_prm_entityRedH._price_fraction_proc_id = _prm_entityH._price_fraction_proc_id;
            //_prm_entityRedH._tax_fraction_proc_id = _prm_entityH._tax_fraction_proc_id;
            //_prm_entityRedH._unit_kind_id = _prm_entityH._unit_kind_id;
            //_prm_entityRedH._credit_limit_price = _prm_entityH._credit_limit_price;
            //_prm_entityRedH._sales_credit_price = _prm_entityH._sales_credit_price;
            //_prm_entityRedH._before_sales_credit_price = _prm_entityH._before_sales_credit_price;
            //_prm_entityRedH._invoice_id = _prm_entityH._invoice_id;
            //_prm_entityRedH._invoice_name = _prm_entityH._invoice_name;
            //_prm_entityRedH._invoice_yyyymmdd = _prm_entityH._invoice_yyyymmdd;
            //_prm_entityRedH._credit_rate = _prm_entityH._credit_rate;
            //_prm_entityRedH._collect_day = _prm_entityH._collect_day;
            //_prm_entityRedH._collect_cycle_id = _prm_entityH._collect_cycle_id;
            //_prm_entityRedH._invoice_no = _prm_entityH._invoice_no;
            //_prm_entityRedH._invoice_state = _prm_entityH._invoice_state;
            //_prm_entityRedH._receipt_receivable_kbn = _prm_entityH._receipt_receivable_kbn;
            //_prm_entityRedH._receipt_receivable_kbn_nm = _prm_entityH._receipt_receivable_kbn_nm;
            //_prm_entityRedH._lock_flg = _prm_entityH._lock_flg;
            //_prm_entityRedH._message = _prm_entityH._message;

            //#endregion
        }

        #endregion

        #region ConvertBeforeDate

        private void ConvertBeforeData(EntityPurchaseH _prm_entityH,
                                       ObservableCollection<EntityPurchaseD> _prm_entityListD,
                                       EntityPurchaseH _prm_before_entityH,
                                       ObservableCollection<EntityPurchaseD> _prm_before_entityListD)
        {

            #region Set Entity Detail
            
            _prm_before_entityListD.Clear();

            for (int i = 0; i <= _prm_entityListD.Count - 1; i++)
            {

                EntityPurchaseD _entityD = new EntityPurchaseD();

                _entityD._id = _prm_entityListD[i]._id;
                _entityD._rec_no = _prm_entityListD[i]._rec_no;
                _entityD._breakdown_id = _prm_entityListD[i]._breakdown_id;
                _entityD._breakdown_nm = _prm_entityListD[i]._breakdown_nm;
                _entityD._deliver_division_id = _prm_entityListD[i]._deliver_division_id;
                _entityD._deliver_division_nm = _prm_entityListD[i]._deliver_division_nm;
                _entityD._commodity_id = _prm_entityListD[i]._commodity_id;
                _entityD._commodity_name = _prm_entityListD[i]._commodity_name;
                _entityD._unit_id = _prm_entityListD[i]._unit_id;
                _entityD._unit_nm = _prm_entityListD[i]._unit_nm;
                _entityD._enter_number = _prm_entityListD[i]._enter_number;
                _entityD._case_number = _prm_entityListD[i]._case_number;
                _entityD._number = _prm_entityListD[i]._number;
                _entityD._unit_price = _prm_entityListD[i]._unit_price;
                //_entityD._sales_cost = _prm_entityListD[i]._sales_cost;
                _entityD._tax = _prm_entityListD[i]._tax;
                _entityD._no_tax_price = _prm_entityListD[i]._no_tax_price;
                _entityD._price = _prm_entityListD[i]._price;
                //_entityD._profits = _prm_entityListD[i]._profits;
                //_entityD._profits_percent = _prm_entityListD[i]._profits_percent;
                _entityD._tax_division_id = _prm_entityListD[i]._tax_division_id;
                _entityD._tax_division_nm = _prm_entityListD[i]._tax_division_nm;
                _entityD._tax_percent = _prm_entityListD[i]._tax_percent;
                _entityD._inventory_management_division_id = _prm_entityListD[i]._inventory_management_division_id;
                _entityD._inventory_number = _prm_entityListD[i]._inventory_number;
                _entityD._retail_price_skip_tax = _prm_entityListD[i]._retail_price_skip_tax;
                _entityD._retail_price_before_tax = _prm_entityListD[i]._retail_price_before_tax;
                _entityD._sales_unit_price_skip_tax = _prm_entityListD[i]._sales_unit_price_skip_tax;
                _entityD._sales_unit_price_before_tax = _prm_entityListD[i]._sales_unit_price_before_tax;
                _entityD._sales_cost_price_skip_tax = _prm_entityListD[i]._sales_cost_price_skip_tax;
                _entityD._sales_cost_price_before_tax = _prm_entityListD[i]._sales_cost_price_before_tax;
                _entityD._number_decimal_digit = _prm_entityListD[i]._number_decimal_digit;
                _entityD._unit_decimal_digit = _prm_entityListD[i]._unit_decimal_digit;
                _entityD._purchase_order_id = _prm_entityListD[i]._purchase_order_id;
                _entityD._purchase_order_number = _prm_entityListD[i]._purchase_order_number;
                _entityD._purchase_order_stay_number = _prm_entityListD[i]._purchase_order_stay_number;
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
            _prm_before_entityH._purchase_order_no = _prm_entityH._purchase_order_no;
            _prm_before_entityH._purchase_ymd = _prm_entityH._purchase_ymd;
            _prm_before_entityH._customer_id = _prm_entityH._customer_id;
            _prm_before_entityH._customer_name = _prm_entityH._customer_name;
            _prm_before_entityH._tax_change_id = _prm_entityH._tax_change_id;
            _prm_before_entityH._tax_change_name = _prm_entityH._tax_change_name;
            _prm_before_entityH._business_division_id = _prm_entityH._business_division_id;
            _prm_before_entityH._business_division_name = _prm_entityH._business_division_name;
            _prm_before_entityH._supplier_id = _prm_entityH._supplier_id;
            _prm_before_entityH._supplier_name = _prm_entityH._supplier_name;
            _prm_before_entityH._supply_ymd = _prm_entityH._supply_ymd;
            _prm_before_entityH._sum_enter_number = _prm_entityH._sum_enter_number;
            _prm_before_entityH._sum_case_number = _prm_entityH._sum_case_number;
            _prm_before_entityH._sum_number = _prm_entityH._sum_number;
            _prm_before_entityH._sum_unit_price = _prm_entityH._sum_unit_price;
            //_prm_before_entityH._sum_sales_cost = _prm_entityH._sum_sales_cost;
            _prm_before_entityH._sum_tax = _prm_entityH._sum_tax;
            _prm_before_entityH._sum_no_tax_price = _prm_entityH._sum_no_tax_price;
            _prm_before_entityH._sum_price = _prm_entityH._sum_price;
            //_prm_before_entityH._sum_profits = _prm_entityH._sum_profits;
            //_prm_before_entityH._profits_percent = _prm_entityH._profits_percent;
            _prm_before_entityH._payment_close_flg = _prm_entityH._payment_close_flg;
            _prm_before_entityH._memo = _prm_entityH._memo;
            _prm_before_entityH._update_flg = _prm_entityH._update_flg;
            _prm_before_entityH._update_person_id = _prm_entityH._update_person_id;
            _prm_before_entityH._update_person_nm = _prm_entityH._update_person_nm;
            _prm_before_entityH._update_date = _prm_entityH._update_date;
            _prm_before_entityH._update_time = _prm_entityH._update_time;
            _prm_before_entityH._price_fraction_proc_id = _prm_entityH._price_fraction_proc_id;
            _prm_before_entityH._tax_fraction_proc_id = _prm_entityH._tax_fraction_proc_id;
            _prm_before_entityH._unit_kind_id = _prm_entityH._unit_kind_id;
            //_prm_before_entityH._credit_limit_price = _prm_entityH._credit_limit_price;
            //_prm_before_entityH._sales_credit_price = _prm_entityH._sales_credit_price;
            //_prm_before_entityH._before_sales_credit_price = _prm_entityH._before_sales_credit_price;

            _prm_before_entityH._payment_credit_price = _prm_entityH._payment_credit_price;
            _prm_before_entityH._before_payment_credit_price = _prm_entityH._before_payment_credit_price;

            _prm_before_entityH._payment_yyyymmdd = _prm_entityH._payment_yyyymmdd;
            _prm_before_entityH._credit_rate = _prm_entityH._credit_rate;
            _prm_before_entityH._payment_day = _prm_entityH._payment_day;
            _prm_before_entityH._payment_cycle_id = _prm_entityH._payment_cycle_id;
            _prm_before_entityH._payment_no = _prm_entityH._payment_no;
            _prm_before_entityH._payment_state = _prm_entityH._payment_state;
            _prm_before_entityH._payment_cash_receivable_kbn = _prm_entityH._payment_cash_receivable_kbn;
            _prm_before_entityH._payment_cash_receivable_kbn_nm = _prm_entityH._payment_cash_receivable_kbn_nm;
            _prm_before_entityH._lock_flg = _prm_entityH._lock_flg;
            _prm_before_entityH._message = _prm_entityH._message;

            #endregion
        }

        #endregion

        #region Red Data Set

        private void SetRedData()
        {
            //_entityRedH._sum_case_number = _entityRedH._sum_case_number * -1;                                   // ケース数計
            //_entityRedH._sum_number = _entityRedH._sum_number * -1;                                             // 数量計
            //_entityRedH._sum_tax = _entityRedH._sum_tax * -1;                                                   // 消費税額計
            //_entityRedH._sum_no_tax_price = _entityRedH._sum_no_tax_price * -1;                                 // 税抜金額計
            //_entityRedH._sum_price = _entityRedH._sum_price * -1;                                               // 金額計
            //_entityRedH._sum_profits = _entityRedH._sum_profits * -1;                                           // 粗利計(仕出金額計-仕入原価計)
            //_entityRedH._sales_credit_price = _entityRedH._sales_credit_price - _entityH._sum_price;            // 買掛残高
            //_entityRedH._before_sales_credit_price = _entityRedH._sales_credit_price;                           // 買掛残高
            
            //for (int i = 0; i <= _entityListRedD.Count - 1; i++)
            //{
            //    DataDetail.CalRedcDetail(_entityListRedD[i]);
            //}

            //_entityH = _entityRedH;
            //_entityListD = _entityListRedD;
        }

        #endregion

    }

}
