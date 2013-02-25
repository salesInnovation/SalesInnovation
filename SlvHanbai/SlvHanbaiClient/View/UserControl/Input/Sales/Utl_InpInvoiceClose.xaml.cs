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
using SlvHanbaiClient.svcMstData;
using SlvHanbaiClient.svcClass;
using SlvHanbaiClient.svcInvoiceClose;
using SlvHanbaiClient.View.UserControl.Custom;
using SlvHanbaiClient.View.Dlg;

namespace SlvHanbaiClient.View.UserControl.Input.Sales
{
    public partial class Utl_InpInvoiceClose : ExUserControl
    {

        #region Filed Const

        private const String CLASS_NM = "Utl_InpInvoiceClose";
        private const String PG_NM = DataPgEvidence.PGName.Invoice.InvoiceClose;
        private Common.gePageType _PageType = Common.gePageType.InpInvoiceClose;

        private const ExWebService.geWebServiceCallKbn _GetWebServiceCallKbn = ExWebService.geWebServiceCallKbn.GetInvoiceTotal;
        private const ExWebService.geWebServiceCallKbn _UpdWebServiceCallKbn = ExWebService.geWebServiceCallKbn.UpdateInvoice;

        private ObservableCollection<EntityInvoiceClose> _entity = new ObservableCollection<EntityInvoiceClose>();
        private EntityInvoiceClosePrm _entityPrm = new EntityInvoiceClosePrm();

        private readonly string tableName = "T_INVOICE";
        private Control activeControl;

        private Common.geWinType WinType = Common.geWinType.ListInvoice;

        private Common.geWinGroupType beforeWinGroupType;

        private string beforeValue = "";                // DataGrid編集チェック用
        private int beforeSelectedIndex = -1;           // DataGrid編集前 行
        private int _selectIndex = 0;                   // データグリッド現在行保持用
        private int _selectColumn = 0;                  // データグリッド現在列保持用

        private Utl_FunctionKey utlFunctionKey;

        private eProccessKbn proceeKbn = eProccessKbn.None;
        private enum eProccessKbn
        { 
            None = 0,
            Close,
            Delete,
            Total
        }

        #endregion

        #region Constructor

        public Utl_InpInvoiceClose()
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
            utlFunctionKey = ExVisualTreeHelper.GetUtlFunctionKey(this.LayoutRoot);
            utlFunctionKey.gFunctionKeyEnable = Utl_FunctionKey.geFunctionKeyEnable.Init;
            utlFunctionKey.Init();

            // バインド設定
            SetBinding();

            //System.Globalization.CultureInfo Culture = new System.Globalization.CultureInfo(Thread.CurrentThread.CurrentCulture.Name);
            //// SelectedDateFormat=Shortのパターン
            //Culture.DateTimeFormat.ShortDatePattern = "yyyy/MM";
            //// SelectedDateFormat=Longのパターン
            //Culture.DateTimeFormat.LongDatePattern = "ggy'年'M'月'";
            //Thread.CurrentThread.CurrentCulture = Culture;

            ExBackgroundWorker.DoWork_Focus(this.utlInvoice.txtID, 100);
        }

        private void ExUserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            //System.Globalization.CultureInfo Culture = new System.Globalization.CultureInfo(Thread.CurrentThread.CurrentCulture.Name);
            //// SelectedDateFormat=Shortのパターン
            //Culture.DateTimeFormat.ShortDatePattern = "yyyy/MM/dd";
            //// SelectedDateFormat=Longのパターン
            //Culture.DateTimeFormat.LongDatePattern = "ggy'年'M'月'd'日'";
            //Thread.CurrentThread.CurrentCulture = Culture;
        }

        #endregion

        #region Function Key Button Events

        // F1ボタン(締切) クリック
        public override void btnF1_Click(object sender, RoutedEventArgs e)
        {
            // ボタン押下時非同期入力チェックON
            Common.gblnBtnDesynchronizeLock = true;

            this.proceeKbn = eProccessKbn.Close;

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
            // 初期化
            _entity = null;
            this.dg.ItemsSource = _entity;

            this.utlInvoice.IsEnabled = true;
            this.utlSummingUp.IsEnabled = true;
            this.datInvoiceYmd.IsEnabled = true;
            this.datCollectPlanDay.IsEnabled = true;

            _entityPrm = null;
            SetBinding();

            ExBackgroundWorker.DoWork_Focus(this.utlInvoice.txtID, 10);

            utlFunctionKey.gFunctionKeyEnable = Utl_FunctionKey.geFunctionKeyEnable.Init;

            // ロック解除
            DataPgLock.gLockPg(PG_NM, "", (int)DataPgLock.geLockType.UnLock);
        }

        // F3ボタン(集計) クリック
        public override void btnF3_Click(object sender, RoutedEventArgs e)
        {
            // ボタン押下時非同期入力チェックON
            Common.gblnBtnDesynchronizeLock = true;

            this.proceeKbn = eProccessKbn.Total;

            // 入力確定の為、BackgroundWorker経由で入力チェックを呼出
            ExBackgroundInputCheckWk bk = new ExBackgroundInputCheckWk();
            bk.utl = this;
            bk.waitTime = 500;
            this.txtDummy.IsTabStop = true;
            bk.focusCtl = this.txtDummy;
            bk.bw.RunWorkerAsync();
        }

        // F4ボタン(削除) クリック
        public override void btnF4_Click(object sender, RoutedEventArgs e)
        {
            // ボタン押下時非同期入力チェックON
            Common.gblnBtnDesynchronizeLock = true;

            this.proceeKbn = eProccessKbn.Delete;

            // 入力確定の為、BackgroundWorker経由で入力チェックを呼出
            ExBackgroundInputCheckWk bk = new ExBackgroundInputCheckWk();
            bk.utl = this;
            bk.waitTime = 500;
            this.txtDummy.IsTabStop = true;
            bk.focusCtl = this.txtDummy;
            bk.bw.RunWorkerAsync();

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
                case "utlInvoice":
                case "utlSummingUp":
                    Utl_MstText mst = (Utl_MstText)activeControl;
                    mst.ShowList();
                    break;
                case "datInvoiceYmd":
                case "datCollectPlanDay":
                    ExDatePicker dt = (ExDatePicker)activeControl;
                    dt.ShowCalender();
                    break;
                default:
                    break;
            }
        }

        // F11ボタン(印刷) クリック
        public override void btnF11_Click(object sender, RoutedEventArgs e)
        {
            //Common.report.utlReport.gPageType = Common.gePageType.None;
            ////reportDlg.utlReport.gWinMsterType = _WinMsterType;

            //beforeWinGroupType = Common.gWinGroupType;
            //Common.gWinGroupType = Common.geWinGroupType.Report;

            //Common.report.Closed -= reportDlg_Closed;
            //Common.report.Closed += reportDlg_Closed;
            //Common.report.Show();
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
            Common.gPageGroupType = Common.gePageGroupType.Menu;
            Common.gPageType = Common.gePageType.Menu;
            pg.ChangePage();
        }

        #endregion

        #region GotFocus Events

        private void txt_GotFocus(object sender, RoutedEventArgs e)
        {
            Control ctl = (Control)sender;
            switch (ctl.Name)
            {

                case "utlInvoice":
                case "utlSummingUp":
                case "datInvoiceYmd":
                case "datCollectPlanDay":
                    ExVisualTreeHelper.SetFunctionKeyEnabled("F5", true, this);
                    activeControl = ctl;
                    break;
                default:
                    ExVisualTreeHelper.SetFunctionKeyEnabled("F5", false, this);
                    activeControl = null;
                    break;
            }
        }

        private void dg_GotFocus(object sender, RoutedEventArgs e)
        {
            ExVisualTreeHelper.SetFunctionKeyEnabled("F5", false, this);
            activeControl = null;
        }

        #endregion

        #region LostFocus Events

        private void utlSummingUp_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(utlSummingUp.txtID.Text.Trim())) return;

            string yyyymm = "";
            string now_yyyymmdd = DateTime.Now.ToString("yyyy/MM/dd");

            if (this.datInvoiceYmd.SelectedDate == null)
            {
                this.datInvoiceYmd.SelectedDate = ExCast.zConvertToDate(now_yyyymmdd);
            }
            yyyymm = ExCast.zConvertToDate(this.datInvoiceYmd.SelectedDate).ToString("yyyy/MM");

            int _id = ExCast.zCInt(utlSummingUp.txtID.Text.Trim());
            switch (_id)
            { 
                case 5:
                    _entityPrm._invoice_yyyymmdd = "";
                    this.datInvoiceYmd.SelectedDate = null;
                    _entityPrm._invoice_yyyymmdd = yyyymm + "/05";
                    this.datInvoiceYmd.DisplayDate = ExCast.zConvertToDate(yyyymm + "/05");
                    break;
                case 15:
                    _entityPrm._invoice_yyyymmdd = "";
                    this.datInvoiceYmd.SelectedDate = null;
                    _entityPrm._invoice_yyyymmdd = yyyymm + "/15";
                    this.datInvoiceYmd.DisplayDate = ExCast.zConvertToDate(yyyymm + "/15");
                    break;
                case 20:
                    _entityPrm._invoice_yyyymmdd = "";
                    this.datInvoiceYmd.SelectedDate = null;
                    _entityPrm._invoice_yyyymmdd = yyyymm + "/20";
                    this.datInvoiceYmd.DisplayDate = ExCast.zConvertToDate(yyyymm + "/20");
                    break;
                case 25:
                    _entityPrm._invoice_yyyymmdd = "";
                    this.datInvoiceYmd.SelectedDate = null;
                    _entityPrm._invoice_yyyymmdd = yyyymm + "/25";
                    this.datInvoiceYmd.DisplayDate = ExCast.zConvertToDate(yyyymm + "/25");
                    break;
                case 31:
                    _entityPrm._invoice_yyyymmdd = "";
                    this.datInvoiceYmd.SelectedDate = null;
                    DateTime dt = ExCast.zConvertToDate(yyyymm + "/01");
                    this.datInvoiceYmd.SelectedDate = dt.AddMonths(1).AddDays(-1);
                    this.datInvoiceYmd.DisplayDate = (DateTime)this.datInvoiceYmd.SelectedDate;
                    break;
                default:
                    break;
            }
        }

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
            //EntityClass entity = (EntityClass)e.Row.DataContext;

            //// コンボボックスID連動
            //switch (e.Column.DisplayIndex)
            //{
            //    case 2:         // 表示区分
            //        if (_entity == null) return;
            //        if (_entity.Count >= dg.SelectedIndex && dg.SelectedIndex != -1)
            //            _entity[dg.SelectedIndex]._display_division_id = MeiNameList.GetID(MeiNameList.geNameKbn.DISPLAY_DIVISION_ID, ExCast.zCStr(entity._display_division_nm)) - 1;
            //        break;
            //}
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

        #endregion

        #region Button Click Evnets

        private void btnSelectAll_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i <= _entity.Count - 1; i++)
            {
                _entity[i]._exec_flg = true;
            }
            dg.ItemsSource = null;
            dg.ItemsSource = _entity;
        }

        private void btnNoSelectAll_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i <= _entity.Count - 1; i++)
            {
                _entity[i]._exec_flg = false;
            }
            dg.ItemsSource = null;
            dg.ItemsSource = _entity;
        }

        #endregion

        #region Binding Setting

        private void SetBinding()
        {
            if (_entityPrm == null)
            {
                _entityPrm = new EntityInvoiceClosePrm();
            }

            // マスタコントロールPropertyChanged
            _entityPrm.PropertyChanged += this.utlInvoice.MstID_Changed;
            _entityPrm.PropertyChanged += this.utlSummingUp.MstID_Changed;

            #region Bind

            // バインド
            Binding BindingInvoiceId = new Binding("_invoice_id");
            BindingInvoiceId.Mode = BindingMode.TwoWay;
            BindingInvoiceId.Source = _entityPrm;
            this.utlInvoice.txtID.SetBinding(TextBox.TextProperty, BindingInvoiceId);

            Binding BindingInvoiceNm = new Binding("_invoice_nm");
            BindingInvoiceNm.Mode = BindingMode.TwoWay;
            BindingInvoiceNm.Source = _entityPrm;
            this.utlInvoice.txtNm.SetBinding(TextBox.TextProperty, BindingInvoiceNm);

            Binding BindingSummingUpGroupId = new Binding("_summing_up_group_id");
            BindingSummingUpGroupId.Mode = BindingMode.TwoWay;
            BindingSummingUpGroupId.Source = _entityPrm;
            this.utlSummingUp.txtID.SetBinding(TextBox.TextProperty, BindingSummingUpGroupId);

            Binding BindingSummingUpGroupNm = new Binding("_summing_up_group_nm");
            BindingSummingUpGroupNm.Mode = BindingMode.TwoWay;
            BindingSummingUpGroupNm.Source = _entityPrm;
            this.utlSummingUp.txtNm.SetBinding(TextBox.TextProperty, BindingSummingUpGroupNm);

            Binding BindingInvoiceYmd = new Binding("_invoice_yyyymmdd");
            BindingInvoiceYmd.Mode = BindingMode.TwoWay;
            BindingInvoiceYmd.Source = _entityPrm;
            this.datInvoiceYmd.SetBinding(DatePicker.SelectedDateProperty, BindingInvoiceYmd);

            Binding BindingCollectPlanYmd = new Binding("_collect_plan_yyyymmdd");
            BindingCollectPlanYmd.Mode = BindingMode.TwoWay;
            BindingCollectPlanYmd.Source = _entityPrm;
            this.datCollectPlanDay.SetBinding(DatePicker.SelectedDateProperty, BindingCollectPlanYmd);

            #endregion
        }

        #endregion

        #region Data Select

        #region データ取得

        private void GetData()
        {
            object[] prm = new object[1];
            prm[0] = _entityPrm;
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
                        _entity = (ObservableCollection<EntityInvoiceClose>)objList;
                        utlFunctionKey.gFunctionKeyEnable = Utl_FunctionKey.geFunctionKeyEnable.New;
                        //if (_entity[0]._invoice_exists_flg == 0)
                        //{
                        //    utlFunctionKey.gFunctionKeyEnable = Utl_FunctionKey.geFunctionKeyEnable.New;
                        //}
                        //else
                        //{
                        //    utlFunctionKey.gFunctionKeyEnable = Utl_FunctionKey.geFunctionKeyEnable.Upd;
                        //}

                        this.utlInvoice.IsEnabled = false;
                        this.utlSummingUp.IsEnabled = false;
                        this.datInvoiceYmd.IsEnabled = false;
                        this.datCollectPlanDay.IsEnabled = false;
                        
                        this.dg.ItemsSource = _entity;
                    }
                    else
                    {
                        this.dg.ItemsSource = null;
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
        private void UpdateData(Common.geUpdateType upd)
        {
            object[] prm = new object[2];

            prm[0] = (int)upd;
            prm[1] = this._entity;
            //prm[1] = this.utlClass.txtBindID.Text.Trim();
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
            bool _flg = false;

            #endregion

            try
            {
                #region チェック処理

                switch (this.proceeKbn)
                {
                    case eProccessKbn.None:
                        return;

                    case eProccessKbn.Delete:

                        #region 削除チェック

                        if (this._entity == null)
                        {
                            ExMessageBox.Show("削除対象データがありません。");
                            return;
                        }

                        if (this._entity.Count == 0)
                        {
                            ExMessageBox.Show("削除対象データがありません。");
                            return;
                        }

                        _flg = false;
                        for (int i = 0; i <= _entity.Count - 1; i++)
                        {
                            if (_entity[i]._exec_flg == true)
                            {
                                _flg = true;
                            }
                        }
                        if (_flg == false)
                        {
                            ExMessageBox.Show("削除対象データが選択されていません。");
                            return;
                        }

                        _flg = false;
                        for (int i = 0; i <= _entity.Count - 1; i++)
                        {
                            if (_entity[i]._exec_flg == true && _entity[i]._no != "未")
                            {
                                _flg = true;
                            }
                        }
                        if (_flg == false)
                        {
                            ExMessageBox.Show("削除対象データは締切がされていません。");
                            return;
                        }

                        UpdateData(Common.geUpdateType.Delete);
                        return;

                        #endregion

                    case eProccessKbn.Total:

                        #region 必須チェック

                        // 請求締日
                        if (string.IsNullOrEmpty(this._entityPrm._invoice_yyyymmdd))
                        {
                            errMessage += "請求締日が入力されていません。" + Environment.NewLine;
                            if (errCtl == null) errCtl = this.datInvoiceYmd;
                        }

                        #endregion

                        #region 入力チェック

                        // 締区分
                        if ((!string.IsNullOrEmpty(this._entityPrm._summing_up_group_id)) && string.IsNullOrEmpty(this._entityPrm._summing_up_group_nm))
                        {
                            errMessage += "締区分が適切に入力(選択)されていません。" + Environment.NewLine;
                            if (errCtl == null) errCtl = this.utlSummingUp.txtID;
                        }

                        // 請求先
                        if ((!string.IsNullOrEmpty(this._entityPrm._invoice_id)) && string.IsNullOrEmpty(this._entityPrm._invoice_nm))
                        {
                            errMessage += "請求先が適切に入力(選択)されていません。" + Environment.NewLine;
                            if (errCtl == null) errCtl = this.utlInvoice.txtID;
                        }

                        #endregion

                        #region 日付チェック

                        // 請求締日
                        if (string.IsNullOrEmpty(_entityPrm._invoice_yyyymmdd) == false)
                        {
                            if (ExCast.IsDate(_entityPrm._invoice_yyyymmdd) == false)
                            {
                                errMessage += "請求締日の形式が不正です。(yyyy/mm/dd形式で入力(選択)して下さい)" + Environment.NewLine;
                                if (errCtl == null) errCtl = this.datInvoiceYmd;
                            }
                        }

                        // 回収予定日
                        if (string.IsNullOrEmpty(_entityPrm._collect_plan_yyyymmdd) == false)
                        {
                            if (ExCast.IsDate(_entityPrm._collect_plan_yyyymmdd) == false)
                            {
                                errMessage += "回収予定日の形式が不正です。(yyyy/mm/dd形式で入力(選択)して下さい)" + Environment.NewLine;
                                if (errCtl == null) errCtl = this.datCollectPlanDay;
                            }
                        }

                        #endregion

                        #region 日付変換

                        // 請求締日
                        if (string.IsNullOrEmpty(_entityPrm._invoice_yyyymmdd) == false)
                        {
                            _entityPrm._invoice_yyyymmdd = ExCast.zConvertToDate(_entityPrm._invoice_yyyymmdd).ToString("yyyy/MM/dd");
                        }

                        // 回収予定日
                        if (string.IsNullOrEmpty(_entityPrm._collect_plan_yyyymmdd) == false)
                        {
                            _entityPrm._collect_plan_yyyymmdd = ExCast.zConvertToDate(_entityPrm._collect_plan_yyyymmdd).ToString("yyyy/MM/dd");
                        }

                        #endregion

                        #region 正数チェック

                        //if (ExCast.zCLng(this.txtSummingUpDay.Text.Trim()) < 0)
                        //{
                        //    errMessage += "締日には正の整数を入力して下さい。" + Environment.NewLine;
                        //}

                        #endregion

                        #region 範囲チェック

                        //if (this.datInvoiceYmd.SelectedDate != null && !string.IsNullOrEmpty(this.txtSummingUpDay.Text.Trim()))
                        //{
                        //    string _ym = this.datInvoiceYmd.SelectedDate.ToString();
                        //    _ym = _ym.Substring(0, 7);
                        //    _ym = _ym.Replace("/", "");

                        //    string yyyymm = "";
                        //    string _now = DateTime.Now.ToString("yyyyMMdd");
                        //    int _now_day = ExCast.zCInt(_now.Substring(5, 2));

                        //    if (ExCast.zCInt(_ym) != 0)
                        //    {
                        //        string _invoice_yyyymm = _ym + ExCast.zCInt(this.txtSummingUpDay.Text.Trim()).ToString("00");
                        //        if (ExCast.zCInt(_now) < ExCast.zCInt(_invoice_yyyymm))
                        //        {
                        //            errMessage += "請求締日に明日以降は指定できません。" + Environment.NewLine;
                        //        }
                        //    }
                        //}


                        //if (!(ExCast.zCLng(this.txtSummingUpDay.Text.Trim()) <= 31 && ExCast.zCLng(this.txtSummingUpDay.Text.Trim()) >= 1))
                        //{
                        //    errMessage += "締日には1日から31日を入力して下さい。" + Environment.NewLine;
                        //}

                        #endregion

                        break;

                    case eProccessKbn.Close:

                        #region データ存在チェック

                        if (this._entity == null)
                        {
                            errMessage += "締切対象データが存在しません。" + Environment.NewLine;
                            if (errCtl == null) errCtl = this.utlSummingUp.txtID;
                        }

                        if (this._entity.Count == 0)
                        {
                            errMessage += "締切対象データが存在しません。" + Environment.NewLine;
                            if (errCtl == null) errCtl = this.utlSummingUp.txtID;
                        }

                        _flg = false;
                        for (int i = 0; i <= _entity.Count - 1; i++)
                        {
                            if (_entity[i]._exec_flg == true)
                            {
                                _flg = true;
                            }
                        }
                        if (_flg == false)
                        {
                            errMessage += "締切対象データが選択されていません。" + Environment.NewLine;
                            if (errCtl == null) errCtl = this.utlSummingUp.txtID;
                        }

                        #endregion

                        break;

                }

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
                    //if (!string.IsNullOrEmpty(warnMessage))
                    //{
                    //    warnMessage += "このまま登録を続行してもよろしいですか？" + Environment.NewLine;
                    //    if (ExMessageBox.ResultShow(warnMessage) == MessageBoxResult.Cancel)
                    //    {
                    //        flg = false;
                    //    }
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
                                break;
                            default:
                                ExBackgroundWorker.DoWork_Focus(errCtl, 10);
                                break;
                        }
                    }
                    return;
                }

                #endregion

                #region 各処理

                switch (this.proceeKbn)
                {
                    case eProccessKbn.Total:
                        GetData();
                        break;
                    case eProccessKbn.Close:
                        UpdateData(Common.geUpdateType.Insert);
                        break;
                    case eProccessKbn.None:
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

            }
            finally
            {
                Common.gblnBtnProcLock = false;
                Common.gblnBtnDesynchronizeLock = false;
            }
        }

        #endregion

        #region InitDetail

        private void InitDetail(ref EntityClass entity)
        {
            //entity._condition_divition_id = 1;
            SetInitCombo(ref entity);
        }

        private void SetInitCombo(ref EntityClass entity)
        {
            // コンボボックス初期選択
            List<string> lst;
            lst = MeiNameList.GetListMei(MeiNameList.geNameKbn.DISPLAY_DIVISION_ID);
            entity._display_division_nm = lst[1];
            entity._display_division_id = MeiNameList.GetID(MeiNameList.geNameKbn.BREAKDOWN_ID, lst[1]);
        }

        #endregion

        #region Outer Text Changed Events

        private void txt_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                ExTextBox txt = (ExTextBox)sender;
                //if (!string.IsNullOrEmpty(this.utlClass.txtBindID.Text.Trim()) && !string.IsNullOrEmpty(this.utlClass.txtNm.Text.Trim()))
                //{
                //    GetMstData();
                //}
                //else
                //{
                //    btnF2_Click(null, null);
                //}
            }
            catch
            {
            }
        }

        #endregion

    }

}
