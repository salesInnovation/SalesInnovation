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
using SlvHanbaiClient.Class.Utility;
using SlvHanbaiClient.Class.UI;
using SlvHanbaiClient.Class.Data;
using SlvHanbaiClient.Class.WebService;
using SlvHanbaiClient.Class.Entity;
using SlvHanbaiClient.svcStockInventory;
using SlvHanbaiClient.View.Dlg;
using SlvHanbaiClient.View.UserControl.Custom;
using SlvHanbaiClient.View.UserControl.Report;

namespace SlvHanbaiClient.View.UserControl.Input.Inventory
{
    public partial class Utl_InpStockInventory : ExUserControl
    {

        #region Filed Const

        private const String CLASS_NM = "Utl_InpStockInventory"; 
        private ObservableCollection<EntityStockInventory> entityList;
        private Control activeControl;

        private Utl_FunctionKey utlFKey = null;
        private Utl_Report utlReport = new Utl_Report();

        private EntitySearch entity = new EntitySearch();

        private long _no;
        public long no { set { this._no = value; } get { return this._no; } }

        private bool _DialogResult;
        public bool DialogResult { set { this._DialogResult = value; } get { return this._DialogResult; } }

        private ExWebService.geWebServiceCallKbn _WebServiceCallKbn = ExWebService.geWebServiceCallKbn.GetStockInventoryList;
        public ExWebService.geWebServiceCallKbn WebServiceCallKbn { set { this._WebServiceCallKbn = value; } get { return this._WebServiceCallKbn; } }

        private enum eProcKbn { Search = 0, Report, ReportDetail, Update };
        private eProcKbn ProcKbn;

        #endregion

        #region Constructor

        public Utl_InpStockInventory()
        {
            InitializeComponent();
            this.Tag = "Main";      // ファンクションキーを受けつけ用
        }

        #endregion

        #region Page Events

        private void ExUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //System.Globalization.CultureInfo Culture = new System.Globalization.CultureInfo(Thread.CurrentThread.CurrentCulture.Name);
            //// SelectedDateFormat=Shortのパターン
            //Culture.DateTimeFormat.ShortDatePattern = "yyyy/MM";
            //// SelectedDateFormat=Longのパターン
            //Culture.DateTimeFormat.LongDatePattern = "ggy'年'M'月'";
            //Thread.CurrentThread.CurrentCulture = Culture;

            this.utlFKey = ExVisualTreeHelper.GetUtlFunctionKey(this.LayoutRoot);
            this.utlFKey.gFunctionKeyEnable = Utl_FunctionKey.geFunctionKeyEnable.Init;

            // 処理棚卸日初期設定
            DataInitWhere.InitDate(DataInitWhere.geDateKbn.Today, ref this.datYmd, ref this.datYmd);

            //if (Common.gWinGroupType == Common.geWinGroupType.InpDetailReport && this.utlFKey != null)
            //{
            //    this.utlFKey.btnF6.Content = "     F6     " + Environment.NewLine;
            //    this.utlFKey.btnF6.IsEnabled = false;
            //}
            this.utlFKey.btnF6.Content = "     F6     " + Environment.NewLine;
            this.utlFKey.btnF6.IsEnabled = false;

            Init_SearchDisplay();
        }

        public override void Init_SearchDisplay()
        {
            Dispatcher.BeginInvoke(
               () =>
               {
                   _Init_SearchDisplay();
               }
            );
        }

        private void _Init_SearchDisplay()
        {
            // 画面初期化
            ExVisualTreeHelper.initDisplay(this.LayoutRoot);

            // 処理棚卸日初期設定
            DataInitWhere.InitDate(DataInitWhere.geDateKbn.Today, ref this.datYmd, ref this.datYmd);

            //this.dgPrint.ItemsSource = null;
            this.dgUpdate.ItemsSource = null;

            // ファンクションキー初期設定
            utlFKey = ExVisualTreeHelper.GetUtlFunctionKey(this);
            this.utlFKey.Init();

            this.utlReport.gPageType = Common.gPageType;
            this.utlReport.gWinMsterType = Common.geWinMsterType.None;
            this.utlReport.parentUtl = this;
            this.utlReport.Init();

            // DataGrid設定
            this.GridDetail.Visibility = System.Windows.Visibility.Collapsed;
            this.GridDetailUpdate.Visibility = System.Windows.Visibility.Visible;

            //switch (Common.gWinGroupType)
            //{
            //    case Common.geWinGroupType.InpListUpd:
            //        // DataGrid設定
            //        this.GridDetail.Visibility = System.Windows.Visibility.Collapsed;
            //        this.GridDetailUpdate.Visibility = System.Windows.Visibility.Visible;
            //        break;
            //    case Common.geWinGroupType.InpListReport:
            //        // DataGrid設定
            //        this.GridDetail.Visibility = System.Windows.Visibility.Collapsed;
            //        this.GridDetailUpdate.Visibility = System.Windows.Visibility.Collapsed;

            //        this.utlFKey.btnF6.Content = "     F6     " + Environment.NewLine;
            //        this.utlFKey.btnF6.IsEnabled = false;

            //        break;
            //}

            SetBinding();

            //GetListData();
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

        #region Binding Setting

        private void SetBinding()
        {
            if (entity == null)
            {
                entity = new EntitySearch();
            }

            // マスタコントロールPropertyChanged
            entity.PropertyChanged += this.utlCommodity_F.MstID_Changed;
            entity.PropertyChanged += this.utlCommodity_T.MstID_Changed;

            #region Bind

            // バインド
            Binding BindingCommodityId_F = new Binding("commodity_id_from");
            BindingCommodityId_F.Mode = BindingMode.TwoWay;
            BindingCommodityId_F.Source = entity;
            this.utlCommodity_F.txtID.SetBinding(TextBox.TextProperty, BindingCommodityId_F);

            // バインド
            Binding BindingCommodityId_T = new Binding("commodity_id_to");
            BindingCommodityId_T.Mode = BindingMode.TwoWay;
            BindingCommodityId_T.Source = entity;
            this.utlCommodity_T.txtID.SetBinding(TextBox.TextProperty, BindingCommodityId_T);

            #endregion

            this.utlCommodity_F.txtID.OnFormatString();
            this.utlCommodity_T.txtID.OnFormatString();
        }

        #endregion

        #region Function Key Button Events

        // F1ボタン() クリック
        public override void btnF1_Click(object sender, RoutedEventArgs e)
        {
            //if (Common.gWinGroupType == Common.geWinGroupType.InpListReport) this.ProcKbn = eProcKbn.Report;
            //else this.ProcKbn = eProcKbn.Update;
            this.ProcKbn = eProcKbn.Update;

            //this.utlReport.rptKbn = DataReport.geReportKbn.OutPut;

            // ボタン押下時非同期入力チェックON
            Common.gblnBtnDesynchronizeLock = true;

            ExBackgroundInputCheckWk bk = new ExBackgroundInputCheckWk();
            bk.utl = this;
            bk.waitTime = 500;
            this.txtDummy.IsTabStop = true;
            bk.focusCtl = this.txtDummy;
            bk.bw.RunWorkerAsync();
        }

        // F2ボタン(条件クリア) クリック
        public override void btnF2_Click(object sender, RoutedEventArgs e)
        {
            // 画面初期化
            //ExVisualTreeHelper.initDisplay(this.LayoutRoot);
            this.utlFKey.gFunctionKeyEnable = Utl_FunctionKey.geFunctionKeyEnable.Init;

            this.entityList = null;
            this.dgUpdate.ItemsSource = null;
            //DataInitWhere.InitDate(DataInitWhere.geDateKbn.Today, ref this.datYmd, ref this.datYmd);

            // 取引区分
            //this.chkSime.IsChecked = false;
            //this.chkKake.IsChecked = false;

            ExBackgroundWorker.DoWork_Focus(this.datYmd, 10);
        }

        // F3ボタン(集計) クリック
        public override void btnF3_Click(object sender, RoutedEventArgs e)
        {
            //if (Common.gWinGroupType != Common.geWinGroupType.InpListReport) return;

            this.ProcKbn = eProcKbn.Search;

            // ボタン押下時非同期入力チェックON
            Common.gblnBtnDesynchronizeLock = true;

            //this.utlReport.rptKbn = DataReport.geReportKbn.Download;
            ExBackgroundInputCheckWk bk = new ExBackgroundInputCheckWk();
            bk.utl = this;
            bk.waitTime = 500;
            this.txtDummy.IsTabStop = true;
            bk.focusCtl = this.txtDummy;
            bk.bw.RunWorkerAsync();
        }

        // F4ボタン(CSV) クリック
        public override void btnF4_Click(object sender, RoutedEventArgs e)
        {
            //if (Common.gWinGroupType == Common.geWinGroupType.InpListUpd) return;

            //// ボタン押下時非同期入力チェックON
            //Common.gblnBtnDesynchronizeLock = true;

            //this.utlReport.rptKbn = DataReport.geReportKbn.Csv;
            //ExBackgroundInputCheckWk bk = new ExBackgroundInputCheckWk();
            //bk.utl = this;
            //bk.waitTime = 500;
            //this.txtDummy.IsTabStop = true;
            //bk.focusCtl = this.txtDummy;
            //bk.bw.RunWorkerAsync();
        }

        // F5ボタン(参照) クリック
        public override void btnF5_Click(object sender, RoutedEventArgs e)
        {
            if (activeControl == null) return;
            switch (activeControl.Name)
            {
                //case "utlCommodity_F":
                //case "utlEstimateNo_T":
                //    Utl_InpNoText inp = (Utl_InpNoText)activeControl;
                //    inp.ShowList();
                //    break;
                case "datYmd":
                    ExDatePicker dt = (ExDatePicker)activeControl;
                    dt.ShowCalender();
                    break;
                case "utlCommodity_F":
                case "utlCommodity_T":
                    Utl_MstText mst = (Utl_MstText)activeControl;
                    mst.ShowList();
                    break;
                default:
                    break;
            }
        }

        // F6ボタン(検索) クリック
        public override void btnF6_Click(object sender, RoutedEventArgs e)
        {
            //if (Common.gWinGroupType == Common.geWinGroupType.InpDetailReport) return;

            this.ProcKbn = eProcKbn.Search;

            // ボタン押下時非同期入力チェックON
            Common.gblnBtnDesynchronizeLock = true;

            this.utlReport.rptKbn = DataReport.geReportKbn.None;

            // 入力確定の為、BackgroundWorker経由で入力チェックを呼出
            ExBackgroundInputCheckWk bk = new ExBackgroundInputCheckWk();
            bk.utl = this;
            bk.waitTime = 500;
            this.txtDummy.IsTabStop = true;
            bk.focusCtl = this.txtDummy;
            bk.bw.RunWorkerAsync();
        }

        // F12ボタン(キャンセル) クリック
        public override void btnF12_Click(object sender, RoutedEventArgs e)
        {
            UA_Main pg = (UA_Main)ExVisualTreeHelper.FindPerentPage(this.Parent);
            Common.gPageGroupType = Common.gePageGroupType.Menu;
            Common.gPageType = Common.gePageType.Menu;
            pg.ChangePage();
        }

        #endregion

        #region TextBox Events

        #region GotFocus

        private void txt_GotFocus(object sender, RoutedEventArgs e)
        {
            Control ctl = (Control)sender;

            switch (ctl.Name)
            {
                case "datYmd":
                case "utlCommodity_F":
                case "utlCommodity_T":
                    ExVisualTreeHelper.SetFunctionKeyEnabled("F5", true, this);
                    activeControl = ctl;
                    break;
                default:
                    ExVisualTreeHelper.SetFunctionKeyEnabled("F5", false, this);
                    activeControl = null;
                    break;
            }
        }

        #endregion

        #endregion

        #region DataGrid Events

        private void dg_DoubleClick(object sender, EventArgs e)
        {
            //if (Common.gWinGroupType == Common.geWinGroupType.InpListUpd)
            //{
            //    btnF1_Click(null, null);
            //}
        }

        private void dg_KeyUp(object sender, KeyEventArgs e)
        {
            //switch (e.Key)
            //{
            //    case Key.Enter: this.btnF1_Click(null, null); break;
            //    default: break;
            //}
        }

        private void dg_CellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
        {
        }

        private void dgUpdate_CellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
        {
            EntityStockInventory entity = (EntityStockInventory)e.Row.DataContext;

            // 明細計算
            switch (e.Column.DisplayIndex)
            {
                case 4:
                    entity._diff_number = entity._account_inventory_number - entity._practice_inventory_number;

                    break;
            }
        }

        #endregion

        #region Button Click Evnets

        private void btnSelectAll_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i <= this.entityList.Count - 1; i++)
            {
                this.entityList[i]._exec_flg = true;
            }
            dgUpdate.ItemsSource = null;
            dgUpdate.ItemsSource = this.entityList;
        }

        private void btnNoSelectAll_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i <= this.entityList.Count - 1; i++)
            {
                this.entityList[i]._exec_flg = false;
            }
            dgUpdate.ItemsSource = null;
            dgUpdate.ItemsSource = this.entityList;
        }

        #endregion

        #region Data Select

        #region データ取得

        // データ取得
        private void GetListData()
        {
            //if (Common.gWinGroupType == Common.geWinGroupType.InpDetailReport) return;

            object[] prm = new object[2];
            prm[0] = GetSQLWhere();
            prm[1] = "";
            webService.objPerent = this;
            webService.CallWebService(this.WebServiceCallKbn,
                                      ExWebService.geDialogDisplayFlg.Yes,
                                      ExWebService.geDialogCloseFlg.Yes,
                                      prm);
        }

        // 条件句SQL設定
        private string GetSQLWhere()
        {
            string strWhrer = "";
            string strWhrerString1 = "";
            string strWhrerString2 = "";

            // 棚卸日
            string proc_ym = this.datYmd.Text.Trim();

            // 商品
            if (this.utlCommodity_F.txtID.Text.Trim() != "")
            {
                strWhrer += "   AND T.ID >= " + ExEscape.zRepStr(ExCast.zNumZeroNothingFormat(this.utlCommodity_F.txtID.Text.Trim())) + Environment.NewLine;
                strWhrerString1 += "] [商品 " + this.utlCommodity_F.txtID.Text.Trim() + "～";
            }
            else
            {
                strWhrerString1 += "] [商品 未指定～";
            }
            if (this.utlCommodity_T.txtID.Text.Trim() != "")
            {
                strWhrer += "   AND T.ID <= " + ExEscape.zRepStr(ExCast.zNumZeroNothingFormat(this.utlCommodity_T.txtID.Text.Trim())) + Environment.NewLine;
                strWhrerString1 += this.utlCommodity_T.txtID.Text.Trim();
            }
            else
            {
                strWhrerString1 += "未指定";
            }

            return strWhrer + "WhereString =>" + strWhrerString1 + ";" + strWhrerString2;
        }

        // ソート句SQL設定
        private string GetSQLOrderBy()
        {
            string strOrderBy = "";
            //if (Common.gWinGroupType == Common.geWinGroupType.InpListReport)
            //{
            //    strOrderBy += "         ,OD.NO " + Environment.NewLine;
            //}
            //else
            //{
            //    strOrderBy += "         ,OD.SALES_YMD " + Environment.NewLine;
            //    strOrderBy += "         ,OD.NO " + Environment.NewLine;
            //}
            return strOrderBy;
        }

        #endregion

        #region データ取得コールバック呼出(ExWebServiceクラスより呼出)

        // リスト取得コールバック呼出
        public override void DataSelect(int intKbn, object objList)
        {
            if ((ExWebService.geWebServiceCallKbn)intKbn == this.WebServiceCallKbn)
            {
                if (objList != null)
                {
                    entityList = (ObservableCollection<EntityStockInventory>)objList;

                    //this.dgPrint.Focus();
                    //this.dgPrint.ItemsSource = null;
                    //this.dgPrint.ItemsSource = entityList;
                    this.dgUpdate.ItemsSource = null;
                    this.dgUpdate.ItemsSource = entityList;
                    ExBackgroundWorker.DoWork_Focus(this.dgUpdate, 10);

                    this.utlFKey.gFunctionKeyEnable = Utl_FunctionKey.geFunctionKeyEnable.Upd;
                }
                else
                {
                    entityList = null;
                    //this.dgPrint.ItemsSource = null;
                    this.dgUpdate.ItemsSource = null;
                    ExBackgroundWorker.DoWork_Focus(this.datYmd, 10);

                    this.utlFKey.gFunctionKeyEnable = Utl_FunctionKey.geFunctionKeyEnable.New;
                }
            }
        }

        #endregion

        #endregion

        #region Data Update

        // データ追加・更新・削除
        private void UpdateData(Common.geUpdateType upd)
        {
            object[] prm = new object[3];

            prm[0] = (int)upd;
            prm[1] = this.datYmd.Text.Trim();
            prm[2] = this.entityList;
            webService.objPerent = this;
            webService.CallWebService(ExWebService.geWebServiceCallKbn.UpdateStockInventory,
                                      ExWebService.geDialogDisplayFlg.Yes,
                                      ExWebService.geDialogCloseFlg.Yes,
                                      prm);
        }

        public override void DataUpdate(int intKbn, string errMessage)
        {
            if ((ExWebService.geWebServiceCallKbn)intKbn == ExWebService.geWebServiceCallKbn.UpdateStockInventory)
            {
                if (string.IsNullOrEmpty(errMessage))
                {
                    ExMessageBox.Show("登録しました。");
                    GetListData();
                    //btnF2_Click(null, null);
                }
            }
        }

        #endregion

        #region Input Check

        // 入力チェック
        public override void InputCheckUpdate()
        {
            string errMessage = "";
            Control errCtl = null;

            try
            {
                switch (this.ProcKbn)
                {
                    case eProcKbn.Search:
                    case eProcKbn.Report:

                        #region 検索時チェック

                        #region 必須チェック

                        #endregion

                        #region 入力チェック

                        // 商品
                        if (this.utlCommodity_F.txtID.Text.Trim() != "" && string.IsNullOrEmpty(this.utlCommodity_F.txtNm.Text.Trim()))
                        {
                            errMessage += "商品が適切に入力(選択)されていません。" + Environment.NewLine;
                            if (errCtl == null) errCtl = this.utlCommodity_F.txtID;
                        }
                        if (this.utlCommodity_T.txtID.Text.Trim() != "" && string.IsNullOrEmpty(this.utlCommodity_T.txtNm.Text.Trim()))
                        {
                            errMessage += "商品が適切に入力(選択)されていません。" + Environment.NewLine;
                            if (errCtl == null) errCtl = this.utlCommodity_T.txtID;
                        }

                        #endregion

                        #region 範囲チェック

                        // 商品
                        if ((ExCast.IsNumeric(this.utlCommodity_F.txtID.Text.Trim()) && ExCast.IsNumeric(this.utlCommodity_T.txtID.Text.Trim())) &&
                            (!string.IsNullOrEmpty(this.utlCommodity_F.txtID.Text.Trim()) && !string.IsNullOrEmpty(this.utlCommodity_T.txtID.Text.Trim())))
                        {
                            if (ExCast.zCLng(this.utlCommodity_F.txtID.Text.Trim()) > ExCast.zCLng(this.utlCommodity_T.txtID.Text.Trim()))
                            {
                                errMessage += "商品の範囲指定が不正です。" + Environment.NewLine;
                                if (errCtl == null) errCtl = this.utlCommodity_F.txtID;
                            }
                        }

                        #endregion

                        #region 日付チェック

                        #endregion

                        #endregion

                        break;
                    case eProcKbn.Update:

                        #region 更新チェック

                        #region 必須チェック

                        //if (this.datIssueYmd.SelectedDate == null)
                        //{
                        //    errMessage += "出力発行日が入力されていません。" + Environment.NewLine;
                        //    if (errCtl == null) errCtl = this.datIssueYmd;
                        //}

                        // 棚卸日
                        if (string.IsNullOrEmpty(this.datYmd.Text.Trim()))
                        {
                            errMessage += "棚卸日を入力(選択)して下さい" + Environment.NewLine;
                            if (errCtl == null) errCtl = this.datYmd;
                        }

                        #endregion

                        #region 日付チェック

                        // 棚卸日
                        if (string.IsNullOrEmpty(this.datYmd.Text.Trim()) == false)
                        {
                            if (ExCast.IsDate(this.datYmd.Text.Trim()) == false)
                            {
                                errMessage += "棚卸日の形式が不正です。(yyyy/mm/dd形式で入力(選択)して下さい)" + Environment.NewLine;
                                if (errCtl == null) errCtl = this.datYmd;
                            }
                        }

                        #endregion

                        #region 選択チェック

                        if (this.entityList == null)
                        {
                            errMessage += "表示データがありません。" + Environment.NewLine;
                            if (errCtl == null) errCtl = this.datYmd;
                        }
                        if (this.entityList.Count == 0)
                        {
                            errMessage += "表示データがありません。" + Environment.NewLine;
                            if (errCtl == null) errCtl = this.datYmd;
                        }

                        bool _exec_flg = false;
                        for (int i = 0; i <= this.entityList.Count - 1; i++)
                        {
                            if (this.entityList[i]._exec_flg == true)
                            {
                                _exec_flg = true;
                            }
                        }
                        if (_exec_flg == false)
                        {
                            errMessage += "登録対象データを選択して下さい。" + Environment.NewLine;
                            if (errCtl == null) errCtl = this.datYmd;
                        }

                        if (_exec_flg == true)
                        {
                            _exec_flg = false;
                            for (int i = 0; i <= this.entityList.Count - 1; i++)
                            {
                                if (this.entityList[i]._exec_flg == true && this.entityList[i]._diff_number != 0)
                                {
                                    _exec_flg = true;
                                }
                            }
                            if (_exec_flg == false)
                            {
                                errMessage += "差異0以外のデータがありません。" + Environment.NewLine;
                                if (errCtl == null) errCtl = this.datYmd;
                            }
                        }

                        #endregion

                        #endregion

                        break;
                }

                #region エラー or 警告時処理

                bool flg = true;

                if (!string.IsNullOrEmpty(errMessage))
                {
                    ExMessageBox.Show(errMessage, Dlg.MessageBox.MessageBoxIcon.Error);
                    flg = false;
                }
                this.txtDummy.IsTabStop = false;

                if (flg == false)
                {
                    if (errCtl != null)
                    {
                        ExBackgroundWorker.DoWork_Focus(errCtl, 10);
                    }
                    return;
                }

                #endregion

                if (this.ProcKbn == eProcKbn.Search)
                {
                    GetListData();
                }
                else
                {
                    if (this.ProcKbn == eProcKbn.Update)
                    {
                        //ExMessageBox.ResultShow(this, null, "処理対象となる仕入先のすべての請求データの残高が更新されます。" + Environment.NewLine + "このまま登録を続行してもよろしいですか？");

                        #region 更新処理

                        UpdateData(Common.geUpdateType.Update);

                        #endregion

                        return;
                    }

                    this.utlReport.utlParentFKey = this.utlFKey;

                    //switch (Common.gWinGroupType)
                    //{
                    //    case Common.geWinGroupType.InpListReport:
                    //        this.utlReport.pgId = DataPgEvidence.PGName.SalesManagement.InvoiceBalancePrint;
                    //        break;
                    //    default:
                    //        break;
                    //}

                    //this.utlReport.sqlWhere = GetSQLWhere();
                    //this.utlReport.sqlOrderBy = GetSQLOrderBy();
                    //this.utlReport.ReportStart();
                }
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

                UpdateData(Common.geUpdateType.Update);

                #endregion
            }
            else
            {
            }
        }

        #endregion

    }

}
