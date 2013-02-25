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
using SlvHanbaiClient.svcReceipt;
using SlvHanbaiClient.svcInvoiceClose;
using SlvHanbaiClient.svcMstData;
using SlvHanbaiClient.View.UserControl.Custom;
using SlvHanbaiClient.View.Dlg;

namespace SlvHanbaiClient.View.UserControl.Input.Sales
{
    public partial class Utl_InpReceipt : ExUserControl
    {

        #region Filed Const

        private const String CLASS_NM = "Utl_InpReceipt";
        private const String PG_NM = DataPgEvidence.PGName.Receipt.ReceiptInp;
        private Common.gePageType _PageType = Common.gePageType.InpReceipt;

        private Utl_FunctionKey utlFunctionKey = null;

        private const ExWebService.geWebServiceCallKbn _GetHeadWebServiceCallKbn = ExWebService.geWebServiceCallKbn.GetReceiptListH;
        private const ExWebService.geWebServiceCallKbn _GetDetailWebServiceCallKbn = ExWebService.geWebServiceCallKbn.GetReceiptListD;
        private const ExWebService.geWebServiceCallKbn _UpdWebServiceCallKbn = ExWebService.geWebServiceCallKbn.UpdateReceipt;

        private EntityReceiptH _entityH = new EntityReceiptH();
        private ObservableCollection<EntityReceiptD> _entityListD = new ObservableCollection<EntityReceiptD>();

        private EntityReceiptH _before_entityH = new EntityReceiptH();

        private EntityInvoiceReceipt _entityInvoiceReceipt = new EntityInvoiceReceipt();

        private readonly string tableName = "T_RECEIPT_H";
        private Control activeControl;

        private Common.geWinType WinType = Common.geWinType.ListReceipt;

        private Dlg_DataForm dataForm;
        private Common.geDataFormType DataFormType = Common.geDataFormType.ReceiptDetail;

        private Dlg_Calender calenderDlg;
        private Dlg_MstSearch masterDlg;
        private Dlg_Copying copyDlg;
        private Common.geWinGroupType beforeWinGroupType;

        private string beforeValue = "";                // DataGrid編集チェック用
        private int beforeSelectedIndex = -1;           // DataGrid編集前 行
        private int _selectIndex = 0;                   // データグリッド現在行保持用
        private int _selectColumn = 0;                  // データグリッド現在列保持用

        #endregion

        #region Constructor

        public Utl_InpReceipt()
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

            // 画面初期化
            this.utlFunctionKey = ExVisualTreeHelper.GetUtlFunctionKey(this.LayoutRoot);
            ExVisualTreeHelper.initDisplay(this.LayoutRoot);

            // ファンクションキー初期設定
            this.utlFunctionKey.gFunctionKeyEnable = Utl_FunctionKey.geFunctionKeyEnable.Init;
            SetDatePickerNotEnabled();

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
            SetDatePickerNotEnabled();

            // 先頭選択
            this.dg.SelectedFirst();

            // ヘッダ初期化
            _entityH = null;
            SetBinding();

            // 明細初期化
            _entityListD = null;
            _entityListD = new ObservableCollection<EntityReceiptD>();

            // 明細追加
            Common.gblnDesynchronizeLock = false;
            this.btnF7_Click(null, null);

            this.dg.ItemsSource = _entityListD;

            this.utlFunctionKey.gFunctionKeyEnable = Utl_FunctionKey.geFunctionKeyEnable.Init;
            SetDatePickerNotEnabled();

            this.utlNo.txtID_IsReadOnly = false;
            //this.utlNo.IsEnabled = true;
            this.utlNo.txtID.Text = "";
            ExBackgroundWorker.DoWork_Focus(this.utlNo.txtID, 10);

            // ロック解除
            DataPgLock.gLockPg(PG_NM, "", (int)DataPgLock.geLockType.UnLock);

            Common.gblnDesynchronizeLock = false;
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

                if (dlg.utlCopying.copy_id == "")
                {
                    this.utlFunctionKey.gFunctionKeyEnable = Utl_FunctionKey.geFunctionKeyEnable.Init;
                    SetDatePickerNotEnabled();
                    this.utlNo.txtID_IsReadOnly = false;
                    this.utlNo.txtID.Text = "";
                }
                else
                {
                    if (dlg.utlCopying.ExistsData == true)
                    {
                        this.utlFunctionKey.gFunctionKeyEnable = Utl_FunctionKey.geFunctionKeyEnable.Upd;
                        SetDatePickerNotEnabled();
                    }
                    else
                    {
                        this.utlFunctionKey.gFunctionKeyEnable = Utl_FunctionKey.geFunctionKeyEnable.New;
                        SetDatePickerNotEnabled();
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
                    ExBackgroundWorker.DoWork_Focus(this.datReceiptYmd, 10);
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
            if (activeControl == null)
            {
                return;
            }

            switch (activeControl.Name)
            {
                case "utlNo":
                case "utlInvoiceNo":
                    Utl_InpNoText inp = (Utl_InpNoText)activeControl;
                    inp.ShowList();
                    break;
                case "datReceiptYmd":
                    ExDatePicker dt = (ExDatePicker)activeControl;
                    dt.ShowCalender();
                    break;
                case "utlPerson":
                case "utlInvoice":
                    Utl_MstText mst = (Utl_MstText)activeControl;
                    mst.ShowList();
                    break;
                case "DetailReceiptDivision":
                    txtReceiptDivisionId_MouseDoubleClick(null, null);
                    break;
                case "DetailBillSiteDay":
                    txtBillSiteDay_MouseDoubleClick(null, null);
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
            DetailSumPrice();

            this.dg.Focus();
        }

        // F7ボタン(明細追加) クリック
        public override void btnF7_Click(object sender, RoutedEventArgs e)
        {
            ExDataGridUtilty.zCommitEdit(this.dg);

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

            if (_entityListD == null) _entityListD = new ObservableCollection<EntityReceiptD>();

            EntityReceiptD entity = new EntityReceiptD();
            int cnt = 1;
            if (_entityListD != null) cnt = _entityListD.Count + 1;
            entity._rec_no = cnt;
            //entity._receipt_division_id = this._entityH._receipt_division_id;
            //entity._receipt_division_nm = this._entityH._receipt_division_nm;
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
            DetailSumPrice();
        }

        // F11ボタン(印刷) クリック
        public override void btnF11_Click(object sender, RoutedEventArgs e)
        {
            beforeWinGroupType = Common.gWinGroupType;
            Common.gWinMsterType = Common.geWinMsterType.None;
            Common.gWinGroupType = Common.geWinGroupType.InpListReport;
            Common.gWinType = this.WinType;

            Common.InpSearchReceipt.Closed -= reportDlg_Closed;
            Common.InpSearchReceipt.Closed += reportDlg_Closed;
            Common.InpSearchReceipt.Show();
        }

        private void reportDlg_Closed(object sender, EventArgs e)
        {
            Common.InpSearchReceipt.Closed -= reportDlg_Closed;
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
                ExMessageBox.Show("請求先を選択して下さい。");
                return;
            }

            if (string.IsNullOrEmpty(this._entityH._invoice_id) || this._entityH._invoice_id == "0")
            {
                ExMessageBox.Show("請求先を選択して下さい。");
                return;
            }

            Dlg_MstSearch searchDlg = new Dlg_MstSearch(MstData.geMDataKbn.SalesBalance);
            searchDlg.MstKbn = Class.Data.MstData.geMDataKbn.SalesBalance;
            searchDlg.MstGroupKbn = Class.Data.MstData.geMGroupKbn.None;
            searchDlg.txtCode.Text = ExCast.zNumZeroNothingFormat(this._entityH._invoice_id);
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
                case "datReceiptYmd":
                case "utlPerson":
                case "utlInvoice":
                case "utlInvoiceNo":
                    ExVisualTreeHelper.SetFunctionKeyEnabled("F5", true, this);
                    ExVisualTreeHelper.SetFunctionKeyEnabled("F8", false, this);
                    activeControl = ctl;
                    break;
                case "txtMemo":
                    ExVisualTreeHelper.SetFunctionKeyEnabled("F5", false, this);
                    ExVisualTreeHelper.SetFunctionKeyEnabled("F8", false, this);
                    activeControl = null;
                    break;

                #endregion

                #region Detail

                case "txtReceiptDivisionNm":
                case "txtPrice":
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
                case "txtReceiptDivisionId":
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
                    txt.Name = "DetailReceiptDivision";
                    activeControl = txt;
                    break;
                case "txtBillSiteDay":
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
                    txt.Name = "DetailBillSiteDay";
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

        private void utlInvoiceNo_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.utlInvoiceNo.txtID.Text.Trim()))
            { 
                _entityH._invoice_no = 0;
                _entityH._invoice_kbn = 0;
                _entityH._invoice_kbn_nm = "";
                _entityH._summing_up_group_id = "";
                _entityH._summing_up_group_nm = "";
                _entityH._invoice_yyyymmdd = "";
                _entityH._collect_plan_day = "";
                _entityH._invoice_price = 0;
                _entityH._before_receipt_price = 0;

                this.utlInvoice.txtID.IsEnabled = true;

                DetailSumPrice();

                return;
            }

            if (this.utlInvoiceNo.txtID.Text.Trim() != "" && this.utlInvoiceNo.txtID.UpdataFlg == true)
            {
                GetInvocieReceipt(ExCast.zCLng(this.utlInvoiceNo.txtID.Text.Trim()));
            }
        }

        private void utlInvoice_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.utlInvoice.txtID.Text.Trim()))
            {
                if (this._entityH != null)
                {
                    this._entityH._credit_price = 0;
                    this._entityH._before_credit_price = 0;
                }
            }

        }

        #endregion

        #region Binding Setting

        private void SetBinding()
        {
            if (_entityH == null)
            {
                _entityH = new EntityReceiptH();
            }

            if (_entityListD == null)
            {
                _entityListD = new ObservableCollection<EntityReceiptD>();
            }

            // マスタコントロールPropertyChanged
            _entityH.PropertyChanged += this.utlInvoice.MstID_Changed;
            _entityH.PropertyChanged += this.utlPerson.MstID_Changed;
            _entityH.PropertyChanged += this.utlReceiptDivision.MstID_Changed;
            _entityH.PropertyChanged += this.utlSummingUp.MstID_Changed;
            _entityH.PropertyChanged += this._PropertyChanged;
            this.utlInvoice.ParentData = _entityH;
            this.utlPerson.ParentData = _entityH;

            NumberConverter nmConvDecm0 = new NumberConverter();
            NumberConverter nmConvDecm2 = new NumberConverter();
            nmConvDecm2.DecimalPlaces = 2;

            #region Bind

            // バインド
            Binding BindingReceiptYmd = new Binding("_receipt_ymd");
            BindingReceiptYmd.Mode = BindingMode.TwoWay;
            BindingReceiptYmd.Source = _entityH;
            this.datReceiptYmd.SetBinding(DatePicker.SelectedDateProperty, BindingReceiptYmd);

            if (string.IsNullOrEmpty(_entityH._receipt_ymd))
            {
                _entityH._receipt_ymd = DateTime.Now.ToString("yyyy/MM/dd");
            }
            
            Binding BindingPersonId = new Binding("_person_id");
            BindingPersonId.Mode = BindingMode.TwoWay;
            BindingPersonId.Source = _entityH;
            this.utlPerson.txtID.SetBinding(TextBox.TextProperty, BindingPersonId);

            Binding BindingPersonNm = new Binding("_person_name");
            BindingPersonNm.Mode = BindingMode.TwoWay;
            BindingPersonNm.Source = _entityH;
            this.utlPerson.txtNm.SetBinding(TextBox.TextProperty, BindingPersonNm);

            if (_entityH._person_id == 0)
            {
                // デフォルト担当の設定
                this.utlPerson.txtID.Text = Common.gintDefaultPersonId.ToString();
                _entityH._person_id = Common.gintDefaultPersonId;
                this.utlPerson.MstID_Changed(null, new PropertyChangedEventArgs("_person_id"));
            }

            Binding BindingInvoiceNo = new Binding("_invoice_no");
            BindingInvoiceNo.Mode = BindingMode.TwoWay;
            BindingInvoiceNo.Source = _entityH;
            this.utlInvoiceNo.txtID.SetBinding(TextBox.TextProperty, BindingInvoiceNo);

            Binding BindingCollectPlanYmd = new Binding("_collect_plan_day");
            BindingCollectPlanYmd.Mode = BindingMode.TwoWay;
            BindingCollectPlanYmd.Source = _entityH;
            this.datCollectPlanYmd.SetBinding(DatePicker.SelectedDateProperty, BindingCollectPlanYmd);

            Binding BindingInvoiceCloseYmd = new Binding("_invoice_yyyymmdd");
            BindingInvoiceCloseYmd.Mode = BindingMode.TwoWay;
            BindingInvoiceCloseYmd.Source = _entityH;
            this.datInvoiceCloseYmd.SetBinding(DatePicker.SelectedDateProperty, BindingInvoiceCloseYmd);

            Binding BindingInvoiceKbnNm = new Binding("_invoice_kbn_nm");
            BindingInvoiceKbnNm.Mode = BindingMode.TwoWay;
            BindingInvoiceKbnNm.Source = _entityH;
            this.txtInvoiceKbn.SetBinding(TextBox.TextProperty, BindingInvoiceKbnNm);

            Binding BindingInvoiceId = new Binding("_invoice_id");
            BindingInvoiceId.Mode = BindingMode.TwoWay;
            BindingInvoiceId.Source = _entityH;
            this.utlInvoice.txtID.SetBinding(TextBox.TextProperty, BindingInvoiceId);

            Binding BindingInvoiceNm = new Binding("_invoice_name");
            BindingInvoiceNm.Mode = BindingMode.TwoWay;
            BindingInvoiceNm.Source = _entityH;
            this.utlInvoice.txtNm.SetBinding(TextBox.TextProperty, BindingInvoiceNm);

            Binding BindingReceiptDivisionId = new Binding("_receipt_division_id");
            BindingReceiptDivisionId.Mode = BindingMode.TwoWay;
            BindingReceiptDivisionId.Source = _entityH;
            this.utlReceiptDivision.txtID.SetBinding(TextBox.TextProperty, BindingReceiptDivisionId);

            Binding BindingReceiptDivisionNm = new Binding("_receipt_division_nm");
            BindingReceiptDivisionNm.Mode = BindingMode.TwoWay;
            BindingReceiptDivisionNm.Source = _entityH;
            this.utlReceiptDivision.txtNm.SetBinding(TextBox.TextProperty, BindingReceiptDivisionNm);

            Binding BindingSummingUpId = new Binding("_summing_up_group_id");
            BindingSummingUpId.Mode = BindingMode.TwoWay;
            BindingSummingUpId.Source = _entityH;
            this.utlSummingUp.txtID.SetBinding(TextBox.TextProperty, BindingSummingUpId);

            Binding BindingSummingUpNm = new Binding("_summing_up_group_nm");
            BindingSummingUpNm.Mode = BindingMode.TwoWay;
            BindingSummingUpNm.Source = _entityH;
            this.utlSummingUp.txtNm.SetBinding(TextBox.TextProperty, BindingSummingUpNm);

            Binding BindingMemo = new Binding("_memo");
            BindingMemo.Mode = BindingMode.TwoWay;
            BindingMemo.Source = _entityH;
            this.txtMemo.SetBinding(TextBox.TextProperty, BindingMemo);

            Binding BindingInvoicePrice = new Binding("_invoice_price");
            BindingInvoicePrice.Mode = BindingMode.TwoWay;
            BindingInvoicePrice.Source = _entityH;
            this.txtInvoicePrice.SetBinding(TextBox.TextProperty, BindingInvoicePrice);

            Binding BindingBeforeReceipPrice = new Binding("_before_receipt_price");
            BindingBeforeReceipPrice.Mode = BindingMode.TwoWay;
            BindingBeforeReceipPrice.Source = _entityH;
            this.txtReceipBeforePrice.SetBinding(TextBox.TextProperty, BindingBeforeReceipPrice);

            Binding BindingSumPrice = new Binding("_sum_price");
            BindingSumPrice.Mode = BindingMode.TwoWay;
            BindingSumPrice.Source = _entityH;
            this.txtPrice.SetBinding(TextBox.TextProperty, BindingSumPrice);

            Binding BindingCreditPrice = new Binding("_credit_price");
            BindingCreditPrice.Mode = BindingMode.TwoWay;
            BindingCreditPrice.Source = _entityH;
            this.txtCreditPrice.SetBinding(TextBox.TextProperty, BindingCreditPrice);

            DetailSumInvoicePrice();

            #endregion

            this.utlInvoiceNo.txtID.SetZeroToNullString();
            this.utlInvoice.txtID.SetZeroToNullString();
            this.utlPerson.txtID.SetZeroToNullString();

            this.utlInvoiceNo.txtID.OnFormatString();
            this.utlInvoice.txtID.OnFormatString();
            this.utlReceiptDivision.txtID.OnFormatString();
            this.utlSummingUp.txtID.OnFormatString();
        }

        #endregion

        #region Data Select

        #region ヘッダデータ取得

        // ヘッダデータ取得
        private void GetHeadData(long _No)
        {
            object[] prm = new object[1];
            prm[0] = _No.ToString();
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

        #region 請求入金データ取得

        private void GetInvocieReceipt(long _No)
        {
            _entityInvoiceReceipt = null;

            object[] prm = new object[2];
            prm[0] = _No.ToString();
            prm[1] = ExCast.zCLng(this.utlNo.txtID.Text.Trim());
            webService.objPerent = this;
            webService.CallWebService(ExWebService.geWebServiceCallKbn.GetInvocieReceipt,
                                      ExWebService.geDialogDisplayFlg.Yes,
                                      ExWebService.geDialogCloseFlg.Yes,
                                      prm);
        }

        #endregion

        #endregion

        #region データ取得コールバック呼出(ExWebServiceクラスより呼出)

        // データ取得コールバック呼出
        public override void DataSelect(int intKbn, object objList)
        {
            switch ((ExWebService.geWebServiceCallKbn)intKbn)
            {
                #region 入金

                // ヘッダ
                case _GetHeadWebServiceCallKbn:
                    // 更新
                    if (objList != null)    
                    {
                        _entityH = (EntityReceiptH)objList;

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
                        //webService.ProcessingDlgClose();
                        //this.utlFunctionKey.gFunctionKeyEnable = Utl_FunctionKey.geFunctionKeyEnable.New;
                        //this.utlNo.txtID_IsReadOnly = true;

                        webService.ProcessingDlgClose();
                        this.utlFunctionKey.gFunctionKeyEnable = Utl_FunctionKey.geFunctionKeyEnable.New;
                        SetDatePickerNotEnabled();
                        this.utlNo.txtID_IsReadOnly = false;
                    }
                    ExBackgroundWorker.DoWork_Focus(this.datReceiptYmd, 10);
                    break;
                // 明細
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
                        _entityListD = (ObservableCollection<EntityReceiptD>)objList;
                    }
                    else 
                    {
                        _entityListD = null;
                    }

                    // 明細追加
                    this.btnF7_Click(null, null);

                    this.dg.ItemsSource = _entityListD;

                    // 前回情報保持
                    ConvertBeforeData(_entityH, _before_entityH);

                    if (_entityH._lock_flg == 0)
                    {
                        this.utlFunctionKey.gFunctionKeyEnable = Utl_FunctionKey.geFunctionKeyEnable.Upd;
                        SetDatePickerNotEnabled();
                    }
                    else
                    {
                        this.utlFunctionKey.gFunctionKeyEnable = Utl_FunctionKey.geFunctionKeyEnable.Sel;
                        SetDatePickerNotEnabled();
                    }

                    // 明細再計算
                    DetailSumPrice();

                    // 請求番号指定時、請求先は変更不可
                    if (!string.IsNullOrEmpty(this.utlInvoiceNo.txtID.Text.Trim()))
                    {
                        this.utlInvoice.txtID.IsEnabled = false;
                    }

                    ExBackgroundWorker.DoWork_Focus(this.datReceiptYmd, 10);
                    this.utlNo.txtID_IsReadOnly = true;
                    //this.utlNo.IsEnabled = false;
                    break;

                #endregion

                #region 請求入金

                // 請求入金
                case ExWebService.geWebServiceCallKbn.GetInvocieReceipt:
                    // 更新
                    if (objList != null)
                    {
                        _entityInvoiceReceipt = (EntityInvoiceReceipt)objList;

                        // エラー発生時
                        if (_entityInvoiceReceipt.message != "" && _entityInvoiceReceipt.message != null)
                        {
                            webService.ProcessingDlgClose();
                            this.utlInvoiceNo.txtID.Text = "";

                            _entityH._invoice_no = 0;
                            _entityH._invoice_kbn = 0;
                            _entityH._invoice_kbn_nm = "";
                            _entityH._summing_up_group_id = "";
                            _entityH._summing_up_group_nm = "";
                            _entityH._invoice_yyyymmdd = "";
                            _entityH._collect_plan_day = "";
                            _entityH._invoice_price = 0;
                            _entityH._before_receipt_price = 0;

                            DetailSumPrice();

                            this.utlInvoice.txtID.IsEnabled = true;
                            ExBackgroundWorker.DoWork_Focus(this.utlInvoiceNo.txtID, 10);
                            return;
                        }

                        // 2回設定がかかりエラーになる為
                        try
                        {
                            this.dg.ItemsSource = null;
                        }
                        catch
                        {
                            return;
                        }

                        _entityH._invoice_no = _entityInvoiceReceipt._no;

                        this.utlInvoice.txtID.Text = _entityInvoiceReceipt._invoice_id;
                        _entityH._invoice_id = _entityInvoiceReceipt._invoice_id;
                        _entityH._invoice_name = _entityInvoiceReceipt._invoice_nm;
                        _entityH._invoice_kbn = _entityInvoiceReceipt._invoice_kbn;
                        _entityH._invoice_kbn_nm = _entityInvoiceReceipt._invoice_kbn_nm;
                        _entityH._summing_up_group_id = _entityInvoiceReceipt._summing_up_group_id;
                        _entityH._summing_up_group_nm = _entityInvoiceReceipt._summing_up_group_nm;
                        _entityH._invoice_yyyymmdd = _entityInvoiceReceipt._invoice_yyyymmdd;
                        _entityH._collect_plan_day = _entityInvoiceReceipt._collect_plan_day;
                        _entityH._invoice_price = _entityInvoiceReceipt._invoice_price;
                        _entityH._before_receipt_price = _entityInvoiceReceipt._before_receipt_price;
                        _entityH._before_credit_price = _entityInvoiceReceipt._credit_price;
                        _entityH._credit_price = _entityInvoiceReceipt._credit_price;
                        _entityH._receipt_division_id = _entityInvoiceReceipt._receipt_division_id;
                        _entityH._receipt_division_nm = _entityInvoiceReceipt._receipt_division_nm;

                        // バインド反映
                        SetBinding();

                        // 前回情報保持
                        ConvertBeforeData(_entityH, _before_entityH);

                        _entityListD = null;
                        _entityListD = new ObservableCollection<EntityReceiptD>();
                        EntityReceiptD entity = new EntityReceiptD();
                        entity._rec_no = 1;
                        entity._receipt_division_id = this._entityH._receipt_division_id;
                        entity._receipt_division_nm = this._entityH._receipt_division_nm;
                        _entityListD.Add(entity);

                        DetailSumPrice();
                        this._entityListD[0]._price = ExCast.zCDbl(this.txtInvoiceZanPrice.Text);
                        this._entityListD[0]._receipt_division_id = _entityH._receipt_division_id;
                        this._entityListD[0]._receipt_division_nm = _entityH._receipt_division_nm;
                        this.dg.ItemsSource = null;
                        this.dg.ItemsSource = this._entityListD;
                        DetailSumPrice();

                        // 請求番号指定時、請求先は変更不可
                        this.utlInvoice.txtID.IsEnabled = false;

                        ExBackgroundWorker.DoWork_Focus(this.txtMemo, 10);


                    }
                    else
                    {
                        MessageBox.Show("請求番号：" + this.utlInvoiceNo.txtID.Text + " は存在しません。");
                        webService.ProcessingDlgClose();
                        this.utlInvoiceNo.txtID.Text = "";

                        _entityH._invoice_no = 0;
                        _entityH._invoice_kbn = 0;
                        _entityH._invoice_kbn_nm = "";
                        _entityH._summing_up_group_id = "";
                        _entityH._summing_up_group_nm = "";
                        _entityH._invoice_yyyymmdd = "";
                        _entityH._collect_plan_day = "";
                        _entityH._invoice_price = 0;
                        _entityH._before_receipt_price = 0;

                        DetailSumPrice();

                        ExBackgroundWorker.DoWork_Focus(this.utlInvoiceNo.txtID, 10);
                        this.utlInvoice.txtID.IsEnabled = true;
                    }
                    break;

                #endregion

                default:
                    break;
            }
        }

        #endregion

        #region Data Update

        // データ追加・更新・削除
        private void UpdateData(Common.geUpdateType upd)
        {
            object[] prm = new object[5];

            prm[0] = (int)upd;
            prm[1] = ExCast.zCLng(this.utlNo.txtID.Text.Trim());
            prm[2] = _entityH;
            prm[3] = _entityListD;
            prm[4] = _before_entityH;
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

                #region 必須チェック

                // 入金日
                if (string.IsNullOrEmpty(_entityH._receipt_ymd))
                {
                    errMessage += "入金日が入力されていません。" + Environment.NewLine;
                    if (errCtl == null) errCtl = this.datReceiptYmd;
                }

                // 入力担当者
                if (_entityH._person_id == 0)
                {
                    errMessage += "入力担当者が入力(選択)されていません。" + Environment.NewLine;
                    if (errCtl == null) errCtl = this.utlPerson.txtID;
                }

                // 請求先
                if (string.IsNullOrEmpty(_entityH._invoice_id))
                {
                    errMessage += "請求先が入力(選択)されていません。" + Environment.NewLine;
                    if (errCtl == null) errCtl = this.utlInvoice.txtID;
                }

                #endregion

                #region 入力チェック

                // 入力担当
                if (_entityH._person_id != 0 && string.IsNullOrEmpty(this.utlPerson.txtNm.Text.Trim()))
                {
                    errMessage += "入力担当者が適切に入力(選択)されていません。" + Environment.NewLine;
                    if (errCtl == null) errCtl = this.utlPerson.txtID;
                }

                // 請求先
                if (_entityH._invoice_id != "" && string.IsNullOrEmpty(this.utlInvoice.txtNm.Text.Trim()))
                {
                    errMessage += "請求先が適切に入力(選択)されていません。" + Environment.NewLine;
                    if (errCtl == null) errCtl = this.utlInvoice.txtID;
                }

                #endregion

                #region 日付チェック

                // 入金日
                if (string.IsNullOrEmpty(_entityH._receipt_ymd) == false)
                {
                    if (ExCast.IsDate(_entityH._receipt_ymd) == false)
                    {
                        errMessage += "入金日の形式が不正です。(yyyy/mm/dd形式で入力(選択)して下さい)" + Environment.NewLine;
                        if (errCtl == null) errCtl = this.datReceiptYmd;
                    }
                }

                #endregion

                #region 日付変換

                // 入金日
                if (string.IsNullOrEmpty(_entityH._receipt_ymd) == false)
                {
                    _entityH._receipt_ymd = ExCast.zConvertToDate(_entityH._receipt_ymd).ToString("yyyy/MM/dd");

                }

                #endregion

                #region 正数チェック

                if (ExCast.zCLng(_entityH._no) < 0)
                {
                    errMessage += "入金番号には正の整数を入力して下さい。" + Environment.NewLine;
                }

                #endregion

                #region 範囲チェック

                if (ExCast.zCLng(_entityH._no) > 999999999999999)
                {
                    errMessage += "入金番号には15桁以内の正の整数を入力して下さい。" + Environment.NewLine;
                }

                if (ExString.LenB(_entityH._memo) > 94)
                {
                    errMessage += "摘要には全角47桁文字以内(半角94桁文字以内)を入力して下さい。" + Environment.NewLine;
                    if (errCtl == null) errCtl = this.txtMemo;
                }

                #endregion

                #endregion

                #region 明細チェック

                #region 明細合計計算

                // 明細合計計算
                DetailSumPrice();

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
                    if (string.IsNullOrEmpty(_entityListD[i]._receipt_division_id))
                    {
                        #region 入金区分未選択チェック

                        // 入金区分未選択は登録されない
                        if (!string.IsNullOrEmpty(_entityListD[i]._receipt_division_nm) ||
                            _entityListD[i]._bill_site_day != "" ||
                            _entityListD[i]._price != 0 ||
                            !string.IsNullOrEmpty(_entityListD[i]._memo))
                        {
                            // なんらかの入力がされている場合、警告を出す
                            warnMessage += (i + 1).ToString() + "行目は入金区分が選択されていない為、登録されません。" + Environment.NewLine;
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

                        #endregion

                        #region 数値チェック

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

                        //if (ExCast.zCDbl(_entityListD[i]._enter_number) < 0)
                        //{
                        //    errMessage += (i + 1).ToString() + "行目の入数に正の数値を入力して下さい)" + Environment.NewLine;
                        //    if (errCtl == null)
                        //    {
                        //        errCtl = this.dg;
                        //        _selectIndex = i;
                        //        _selectColumn = 6;
                        //    }
                        //}

                        #endregion

                        #region 範囲チェック

                        // 入金内容
                        if (ExString.LenB(_entityListD[i]._receipt_division_nm) > 40)
                        {
                            errMessage += (i + 1).ToString() + "行目の入金内容に全角20桁文字以内(半角40桁文字以内)を入力して下さい。" + Environment.NewLine;
                            if (errCtl == null)
                            {
                                errCtl = this.dg;
                                _selectIndex = i;
                                _selectColumn = 12;
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
                        if (ExString.LenB(_entityListD[i]._memo) > 30)
                        {
                            errMessage += (i + 1).ToString() + "行目の備考に全角15桁文字以内(半角30桁文字以内)を入力して下さい。" + Environment.NewLine;
                            if (errCtl == null)
                            {
                                errCtl = this.dg;
                                _selectIndex = i;
                                _selectColumn = 12;
                            }
                        }

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
                                this.dg.Focus();
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
                            this.dg.Focus();
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
                    ExMessageBox.Show("入金データが選択されていません。");
                    return;
                }

                if (this._entityH == null)
                {
                    ExMessageBox.Show("入金データが選択されていません。");
                    return;
                }

                if (this._entityH._id == 0)
                {
                    ExMessageBox.Show("入金データが選択されていません。");
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

                EntityReceiptD entity = (EntityReceiptD)e.Row.DataContext;
                switch (e.Column.DisplayIndex)
                {
                    case 1:         // 入金区分
                        beforeValue = entity._receipt_division_id;
                        break;
                    case 2:         // 入金内容
                        beforeValue = entity._receipt_division_nm;
                        break;
                    case 3:         // 金額
                        beforeValue = ExCast.zCStr(entity._price);
                        break;
                    case 4:         // 手形期日
                        beforeValue = entity._bill_site_day;
                        break;
                    case 5:         // 備考
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
            EntityReceiptD entity = (EntityReceiptD)e.Row.DataContext;

            if (beforeSelectedIndex == -1) return;

            // 明細計算
            switch (e.Column.DisplayIndex)
            {
                case 1:         // 入金区分
                    if (beforeValue == entity._receipt_division_id) return;
                    if (Common.gblnDesynchronizeLock == true) return;
                    Common.gblnDesynchronizeLock = true;
                    MstData _mstData = new MstData();
                    _mstData.GetMData(MstData.geMDataKbn.RecieptDivision, new string[] { entity._receipt_division_id, ExCast.zCStr(beforeSelectedIndex) }, this);
                    break;
                case 3:        // 金額
                    if (beforeValue == ExCast.zCStr(entity._price)) return;
                    DetailSumPrice();
                    break;
            }
        }

        private void dg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void txtBillSiteDay_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            _selectIndex = dg.SelectedIndex;
            _selectColumn = dg.CurrentColumn.DisplayIndex;
            beforeValue = "";
            if (_entityListD == null) return;
            if (_entityListD.Count >= _selectIndex)
            {
                beforeValue = _entityListD[_selectIndex]._bill_site_day;
            }

            calenderDlg = new Dlg_Calender();
            calenderDlg.Show();
            calenderDlg.Closed += calenderDlg_Closed;
        }

        private void calenderDlg_Closed(object sender, EventArgs e)
        {
            Dlg_Calender dlg = (Dlg_Calender)sender;
            if (_entityListD == null) return;
            if (_entityListD.Count >= _selectIndex)
            {
                if (dlg._selectedDate == null)
                {
                    _entityListD[_selectIndex]._bill_site_day = dlg._selectedDate.ToString("yyyy/MM/dd");
                }
            }
            dg.Focus();
            dg.SelectedIndex = _selectIndex;
            dg.CurrentColumn = dg.Columns[_selectColumn];
        }

        private void txtReceiptDivisionId_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            _selectIndex = dg.SelectedIndex;
            _selectColumn = dg.CurrentColumn.DisplayIndex;
            beforeValue = "";
            if (_entityListD == null) return;
            if (_entityListD.Count >= _selectIndex)
            {
                beforeValue = _entityListD[_selectIndex]._receipt_division_id;
            }

            masterDlg = new Dlg_MstSearch();
            masterDlg.MstKbn = MstData.geMDataKbn.RecieptDivision;
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
                    _entityListD[_selectIndex]._receipt_division_id = Dlg_MstSearch.this_id;
                    _entityListD[_selectIndex]._receipt_division_nm = Dlg_MstSearch.this_name;
                }
            }
            if (beforeValue != _entityListD[_selectIndex]._receipt_division_id)
            {
                MstData _mstData = new MstData();
                _mstData.GetMData(MstData.geMDataKbn.RecieptDivision, new string[] { _entityListD[_selectIndex]._receipt_division_id, ExCast.zCStr(_selectIndex) }, this);
            }

            dg.Focus();
            dg.SelectedIndex = _selectIndex;
            dg.CurrentColumn = dg.Columns[_selectColumn];
        }

        private void txtUnitPrice_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
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
                        break;
                    case ExWebServiceMst.geWebServiceMstNmCallKbn.GetRecieptDivision:
                        if (Common.gblnDesynchronizeLock == false) return;
                        if (_mst == null) return;

                        if (string.IsNullOrEmpty(_mst.attribute20))
                        {
                            try
                            {
                                _entityListD[dg.SelectedIndex]._receipt_division_nm = _mst.name;
                            }
                            catch
                            { 
                            }
                        }
                        else
                        {
                            try
                            {
                                _entityListD[ExCast.zCInt(_mst.attribute20)]._receipt_division_nm = _mst.name;
                            }
                            catch
                            {
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

        public void DetailSumPrice()
        {
            double price = 0;

            for (int i = 0; i <= _entityListD.Count -1; i++)
            {
                price += _entityListD[i]._price;
            }
            this.txtPrice.Text = price.ToString();

            switch (this.utlFunctionKey.gFunctionKeyEnable)
            {
                case Utl_FunctionKey.geFunctionKeyEnable.Upd:
                    double _before_credit_price = _before_entityH._credit_price + _before_entityH._sum_price;
                    this.txtCreditPrice.Text = ExCast.zCStr(_before_credit_price - price);
                    break;
                default:
                    this.txtCreditPrice.Text = ExCast.zCStr(_entityH._before_credit_price - price);
                    break;
            }

            this.txtPrice.OnFormatString();
            this.txtCreditPrice.OnFormatString();

            DetailSumInvoicePrice();
        }

        private void DetailSumInvoicePrice()
        {
            if (ExCast.zCLng(_entityH._invoice_no) != 0)
            {
                this.txtInvoiceZanPrice.Text = ExCast.zCDbl(_entityH._invoice_price - _entityH._before_receipt_price - _entityH._sum_price).ToString();
                this.txtInvoicePrice.OnFormatString();
                this.txtReceipBeforePrice.OnFormatString();
                this.txtInvoiceZanPrice.OnFormatString();
            }
            else
            {
                this.txtInvoicePrice.Text = "-";
                this.txtReceipBeforePrice.Text = "-";
                this.txtInvoiceZanPrice.Text = "-";
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
                case "invoice_name":
                    Common.gblnDesynchronizeLock = false;
                    break;
                case "receipt_division_name":
                    if (Common.gblnDesynchronizeLock == false) return;

                    // 明細再計算
                    Common.gblnDesynchronizeLock = false;
                    break;
                case "sum_tax":
                case "sum_no_tax_price":
                    //this.txtSumPrice.Text = ExCast.zCStr(ExCast.zCDbl(_entityH._sum_no_tax_price) + ExCast.zCDbl(_entityH._sum_tax));
                    //this.txtSumPrice.OnFormatString();
                    break;
                case "_before_credit_price":
                    DetailSumPrice();
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region Set DatePicker Enabled

        private void SetDatePickerNotEnabled()
        {
            this.datInvoiceCloseYmd.IsEnabled = false;
            this.datCollectPlanYmd.IsEnabled = false;
        }

        #endregion

        #region ConvertBeforeDate

        private void ConvertBeforeData(EntityReceiptH _prm_entityH,
                                       EntityReceiptH _prm_before_entityH)
        {
            #region Set Entity Detail

            //for (int i = 0; i <= _prm_entityListD.Count - 1; i++)
            //{
            //    _prm_before_entityListD.Clear();

            //    EntityReceiptD _entityD = new EntityReceiptD();

            //    _entityD._id = _prm_entityListD[i]._id;
            //    _entityD._rec_no = _prm_entityListD[i]._rec_no;
            //    _entityD._receipt_division_id = _prm_entityListD[i]._receipt_division_id;
            //    _entityD._receipt_division_nm = _prm_entityListD[i]._receipt_division_nm;
            //    _entityD._bill_site_day = _prm_entityListD[i]._bill_site_day;
            //    _entityD._price = _prm_entityListD[i]._price;
            //    _entityD._memo = _prm_entityListD[i]._memo;
            //    _entityD._lock_flg = _prm_entityListD[i]._lock_flg;
            //    _entityD._message = _prm_entityListD[i]._message;

            //    _prm_before_entityListD.Add(_entityD);
            //}

            #endregion

            #region Set Entity Head

            _prm_before_entityH._id = _prm_entityH._id;
            _prm_before_entityH._no = _prm_entityH._no;
            _prm_before_entityH._invoice_id = _prm_entityH._invoice_id;
            _prm_before_entityH._invoice_name = _prm_entityH._invoice_name;
            _prm_before_entityH._invoice_yyyymmdd = _prm_entityH._invoice_yyyymmdd;
            _prm_before_entityH._summing_up_day = _prm_entityH._summing_up_day;
            _prm_before_entityH._person_id = _prm_entityH._person_id;
            _prm_before_entityH._person_name = _prm_entityH._person_name;
            _prm_before_entityH._receipt_ymd = _prm_entityH._receipt_ymd;
            _prm_before_entityH._sum_price = _prm_entityH._sum_price;
            _prm_before_entityH._credit_price = _prm_entityH._credit_price;
            _prm_before_entityH._before_credit_price = _prm_entityH._before_credit_price;
            _prm_before_entityH._invoice_close_flg = _prm_entityH._invoice_close_flg;
            _prm_before_entityH._memo = _prm_entityH._memo;
            _prm_before_entityH._update_flg = _prm_entityH._update_flg;
            _prm_before_entityH._update_person_id = _prm_entityH._update_person_id;
            _prm_before_entityH._update_person_nm = _prm_entityH._update_person_nm;
            _prm_before_entityH._update_date = _prm_entityH._update_date;
            _prm_before_entityH._update_time = _prm_entityH._update_time;
            _prm_before_entityH._lock_flg = _prm_entityH._lock_flg;
            _prm_before_entityH._message = _prm_entityH._message;

            #endregion
        }

        #endregion

    }

}
