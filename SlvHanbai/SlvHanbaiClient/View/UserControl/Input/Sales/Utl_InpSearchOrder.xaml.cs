using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using SlvHanbaiClient.Class;
using SlvHanbaiClient.Class.Utility;
using SlvHanbaiClient.Class.UI;
using SlvHanbaiClient.Class.Data;
using SlvHanbaiClient.Class.Entity;
using SlvHanbaiClient.Class.WebService;
using SlvHanbaiClient.svcOrder;
using SlvHanbaiClient.View.Dlg;
using SlvHanbaiClient.View.Dlg.Report;
using SlvHanbaiClient.View.UserControl.Custom;
using SlvHanbaiClient.View.UserControl.Report;

namespace SlvHanbaiClient.View.UserControl.Input.Sales
{
    public partial class Utl_InpSearchOrder : ExUserControl
    {

        #region Filed Const

        private const String CLASS_NM = "Utl_InpSearch";
        private ObservableCollection<EntityOrder> entityList;
        private Control activeControl;

        private Utl_FunctionKey utlFKey = null;
        private Utl_Report utlReport = new Utl_Report();

        private List<DisplayOrderList> _lst = new List<DisplayOrderList>();
        public List<DisplayOrderList> lst { set { this._lst = value; } get { return this._lst; } }

        private EntitySearch entity = new EntitySearch();        

        private long _no;
        public long no { set { this._no = value; } get { return this._no; } }

        private bool _DialogResult;
        public bool DialogResult { set { this._DialogResult = value; } get { return this._DialogResult; } }

        private ExWebService.geWebServiceCallKbn _WebServiceCallKbn = ExWebService.geWebServiceCallKbn.GetOrderList;
        public ExWebService.geWebServiceCallKbn WebServiceCallKbn { set { this._WebServiceCallKbn = value; } get { return this._WebServiceCallKbn; } }

        private Dlg_ReportSetting dlgReportSetting = new Dlg_ReportSetting();

        private bool searchBtnFlg = false;

        private enum eProcKbn { Search = 0, Report, ReportDetail };
        private eProcKbn ProcKbn;

        private string UpdPrintNo = "";

        #endregion

        #region Constructor

        public Utl_InpSearchOrder()
        {
            InitializeComponent();
            this.Tag = "Main";      // ファンクションキーを受けつけ用
        }

        #endregion

        #region Page Events

        private void ExUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (Common.gWinGroupType == Common.geWinGroupType.InpDetailReport && this.utlFKey != null)
            {
                this.utlFKey.btnF6.Content = "     F6     " + Environment.NewLine;
                this.utlFKey.btnF6.IsEnabled = false;
            }
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

            this.lst.Clear();
            this.dg.ItemsSource = null;
            this.dg.ItemsSource = lst;

            // ファンクションキー初期設定
            utlFKey = ExVisualTreeHelper.GetUtlFunctionKey(this);
            this.utlFKey.Init();

            this.utlReport.gPageType = Common.gPageType;
            this.utlReport.gWinMsterType = Common.geWinMsterType.None;
            this.utlReport.parentUtl = this;
            this.utlReport.Init();

            switch (Common.gWinGroupType)
            {
                case Common.geWinGroupType.InpList:
                    // 出力対象日初期設定
                    DataInitWhere.InitDate(DataInitWhere.geDateKbn.Month, ref this.datOrderYmd_F, ref this.datOrderYmd_T);

                    // DataGrid設定
                    this.GridDetail.Visibility = System.Windows.Visibility.Visible;
                    this.GridDetailSelect.Visibility = System.Windows.Visibility.Collapsed;

                    // 発行区分表示設定
                    this.borPrint.Visibility = System.Windows.Visibility.Collapsed;

                    break;
                case Common.geWinGroupType.InpListReport:
                    // 出力対象日初期設定
                    DataInitWhere.InitDate(DataInitWhere.geDateKbn.Today, ref this.datOrderYmd_F, ref this.datOrderYmd_T);

                    // DataGrid設定
                    this.GridDetail.Visibility = System.Windows.Visibility.Collapsed;
                    this.GridDetailSelect.Visibility = System.Windows.Visibility.Visible;

                    // 発行区分表示設定
                    this.borPrint.Visibility = System.Windows.Visibility.Visible;

                    this.chkPrintNo.IsChecked = true;
                    this.chkPrintYes.IsChecked = false;

                    break;
                case Common.geWinGroupType.InpDetailReport:
                    // 出力対象日初期設定
                    DataInitWhere.InitDate(DataInitWhere.geDateKbn.Today, ref this.datOrderYmd_F, ref this.datOrderYmd_T);

                    // DataGrid設定
                    this.GridDetail.Visibility = System.Windows.Visibility.Visible;
                    this.GridDetailSelect.Visibility = System.Windows.Visibility.Collapsed;
                    this.dg.ItemsSource = null;
                    this.dg.ItemsSource = lst;

                    // 発行区分表示設定
                    this.borPrint.Visibility = System.Windows.Visibility.Collapsed;

                    this.utlFKey.btnF6.Content = "     F6     " + Environment.NewLine;
                    this.utlFKey.btnF6.IsEnabled = false;

                    break;
            }

            SetBinding();

            GetListData();
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
            entity.PropertyChanged += this.utlPerson_F.MstID_Changed;
            entity.PropertyChanged += this.utlPerson_T.MstID_Changed;
            entity.PropertyChanged += this.utlCustomer.MstID_Changed;
            entity.PropertyChanged += this.utlSupply.MstID_Changed;
            entity.PropertyChanged += this.utlCommodity.MstID_Changed;

            #region Bind

            // バインド
            Binding BindingPersonF = new Binding("person_id_from");
            BindingPersonF.Mode = BindingMode.TwoWay;
            BindingPersonF.Source = entity;
            this.utlPerson_F.txtID.SetBinding(TextBox.TextProperty, BindingPersonF);

            Binding BindingPersonT = new Binding("person_id_to");
            BindingPersonT.Mode = BindingMode.TwoWay;
            BindingPersonT.Source = entity;
            this.utlPerson_T.txtID.SetBinding(TextBox.TextProperty, BindingPersonT);

            Binding BindingCustomer = new Binding("customer_id");
            BindingCustomer.Mode = BindingMode.TwoWay;
            BindingCustomer.Source = entity;
            this.utlCustomer.txtID.SetBinding(TextBox.TextProperty, BindingCustomer);

            Binding BindingSupply = new Binding("supplier_id");
            BindingSupply.Mode = BindingMode.TwoWay;
            BindingSupply.Source = entity;
            this.utlSupply.txtID.SetBinding(TextBox.TextProperty, BindingSupply);
            this.utlSupply.txtID2.SetBinding(TextBox.TextProperty, BindingCustomer);

            Binding BindingCommodity = new Binding("commodity_id");
            BindingCommodity.Mode = BindingMode.TwoWay;
            BindingCommodity.Source = entity;
            this.utlCommodity.txtID.SetBinding(TextBox.TextProperty, BindingCommodity);

            #endregion

            this.utlPerson_F.txtID.OnFormatString();
            this.utlPerson_T.txtID.SetZeroToNullString();
            this.utlCustomer.txtID.SetZeroToNullString();
            this.utlSupply.txtID.SetZeroToNullString();
            this.utlCommodity.txtID.SetZeroToNullString();
        }

        #endregion

        #region Function Key Button Events

        // F1ボタン(OK / 出力) クリック
        public override void btnF1_Click(object sender, RoutedEventArgs e)
        {
            // OK
            if (Common.gWinGroupType == Common.geWinGroupType.InpList)
            {
                if (entityList == null)
                {
                    ExMessageBox.Show("データが検索されていません。");
                    return;
                }
                if (entityList.Count == 0)
                {
                    ExMessageBox.Show("データが検索されていません。");
                    return;
                }

                int intIndex = this.dg.SelectedIndex;
                if (intIndex < 0)
                {
                    ExMessageBox.Show("行が選択されていません。");
                    return;
                }

                this.no = this.lst[this.dg.SelectedIndex].no;
                this.DialogResult = true;

                Dlg_InpSearch win = (Dlg_InpSearch)ExVisualTreeHelper.FindPerentChildWindow(this);
                win.Close();

                // 直接設定すると何故か画面がフリーズする為、コメントアウト
                //win.no = this.lst[this.dg.SelectedIndex].no;
                //win.DialogResult = true;
            }
            // 出力
            else
            {
                if (Common.gWinGroupType == Common.geWinGroupType.InpListReport) this.ProcKbn = eProcKbn.Report;
                else this.ProcKbn = eProcKbn.ReportDetail;

                // ボタン押下時非同期入力チェックON
                Common.gblnBtnDesynchronizeLock = true;

                this.utlReport.rptKbn = DataReport.geReportKbn.OutPut;
                ExBackgroundInputCheckWk bk = new ExBackgroundInputCheckWk();
                bk.utl = this;
                bk.waitTime = 500;
                this.txtDummy.IsTabStop = true;
                bk.focusCtl = this.txtDummy;
                bk.bw.RunWorkerAsync();
            }

        }

        // F2ボタン(条件クリア) クリック
        public override void btnF2_Click(object sender, RoutedEventArgs e)
        {
            // 画面初期化
            ExVisualTreeHelper.initDisplay(this.LayoutRoot);
            this.lst.Clear();
            this.dg.ItemsSource = null;
            if (Common.gWinGroupType == Common.geWinGroupType.InpList)
            {
                DataInitWhere.InitDate(DataInitWhere.geDateKbn.Month, ref this.datOrderYmd_F, ref this.datOrderYmd_T);
            }
            else
            {
                DataInitWhere.InitDate(DataInitWhere.geDateKbn.Today, ref this.datOrderYmd_F, ref this.datOrderYmd_T);
            }

            // 取引区分
            this.chkKake.IsChecked = false;
            this.chkCash.IsChecked = false;
            // 完納区分
            this.chkUnDelivary.IsChecked = false;
            this.chkPartDelivary.IsChecked = false;
            this.chkFullDelivary.IsChecked = false;
            this.chkTorikeshi.IsChecked = false;

            ExBackgroundWorker.DoWork_Focus(this.datOrderYmd_F, 10);
        }

        // F3ボタン(ﾀﾞｳﾝﾛｰﾄﾞ) クリック
        public override void btnF3_Click(object sender, RoutedEventArgs e)
        {
            if (Common.gWinGroupType == Common.geWinGroupType.InpList) return;

            if (Common.gWinGroupType == Common.geWinGroupType.InpListReport) this.ProcKbn = eProcKbn.Report;
            else this.ProcKbn = eProcKbn.ReportDetail;

            // ボタン押下時非同期入力チェックON
            Common.gblnBtnDesynchronizeLock = true;

            this.utlReport.rptKbn = DataReport.geReportKbn.Download;
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
            if (Common.gWinGroupType == Common.geWinGroupType.InpList) return;

            // ボタン押下時非同期入力チェックON
            Common.gblnBtnDesynchronizeLock = true;

            this.utlReport.rptKbn = DataReport.geReportKbn.Csv;
            ExBackgroundInputCheckWk bk = new ExBackgroundInputCheckWk();
            bk.utl = this;
            bk.waitTime = 500;
            this.txtDummy.IsTabStop = true;
            bk.focusCtl = this.txtDummy;
            bk.bw.RunWorkerAsync();
        }

        // F5ボタン(参照) クリック
        public override void btnF5_Click(object sender, RoutedEventArgs e)
        {
            if (activeControl == null) return;
            switch (activeControl.Name)
            {
                case "utlEstimateNo_F":
                case "utlEstimateNo_T":
                    Utl_InpNoText inp = (Utl_InpNoText)activeControl;
                    inp.ShowList();
                    break;
                case "datOrderYmd_F":
                case "datOrderYmd_T":
                case "datNokiYmd_F":
                case "datNokiYmd_T":
                    ExDatePicker dt = (ExDatePicker)activeControl;
                    dt.ShowCalender();
                    break;
                case "utlPerson_F":
                case "utlPerson_T":
                case "utlCustomer":
                case "utlSupply":
                case "utlCommodity":
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
            if (Common.gWinGroupType == Common.geWinGroupType.InpDetailReport) return;

            this.ProcKbn = eProcKbn.Search;

            this.utlReport.rptKbn = DataReport.geReportKbn.None;

            // ボタン押下時非同期入力チェックON
            Common.gblnBtnDesynchronizeLock = true;

            // 入力確定の為、BackgroundWorker経由で入力チェックを呼出
            ExBackgroundInputCheckWk bk = new ExBackgroundInputCheckWk();
            bk.utl = this;
            bk.waitTime = 500;
            this.txtDummy.IsTabStop = true;
            bk.focusCtl = this.txtDummy;
            bk.bw.RunWorkerAsync();

            searchBtnFlg = true;
        }

        // F11ボタン(出力設定) クリック
        public override void btnF11_Click(object sender, RoutedEventArgs e)
        {
            if (Common.gWinGroupType == Common.geWinGroupType.InpList) return;

            switch (Common.gWinGroupType)
            {
                case Common.geWinGroupType.InpListReport:
                    this.dlgReportSetting.pg_id = DataPgEvidence.PGName.Order.OrderPrint;
                    this.dlgReportSetting.IsReportRange = true;
                    this.dlgReportSetting.IsTotalKbn = false;
                    this.dlgReportSetting.IsGroupTotal = false;
                    break;
                case Common.geWinGroupType.InpDetailReport:
                    this.dlgReportSetting.pg_id = DataPgEvidence.PGName.Order.OrderDPrint;
                    this.dlgReportSetting.IsReportRange = true;
                    this.dlgReportSetting.IsTotalKbn = true;
                    this.dlgReportSetting.IsGroupTotal = true;
                    break;
            }
            this.dlgReportSetting.IsGroupTotal = true;
            this.dlgReportSetting.IsReportRange = true;
            this.dlgReportSetting.IsTotalKbn = true;
            this.dlgReportSetting.Show();
        }

        // F12ボタン(キャンセル) クリック
        public override void btnF12_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            Dlg_InpSearch win = (Dlg_InpSearch)ExVisualTreeHelper.FindPerentChildWindow(this);
            win.Close();
        }

        #endregion

        #region TextBox Events

        #region GotFocus

        private void txt_GotFocus(object sender, RoutedEventArgs e)
        {
            Control ctl = (Control)sender;

            switch (ctl.Name)
            {
                case "utlOrderNo_F":
                case "utlOrderNo_T":
                case "accBisiness":
                case "accFull":
                case "dg":
                case "chkKake":
                case "chkCash":
                case "chkInvoice":
                case "chkSample":
                case "chkUnDelivary":
                case "chkPartDelivary":
                case "chkFullDelivary":
                    ExVisualTreeHelper.SetFunctionKeyEnabled("F5", false, this);
                    activeControl = null;
                    break;
                case "datOrderYmd_F":
                case "datOrderYmd_T":
                case "utlEstimateNo_F":
                case "utlEstimateNo_T":
                case "utlCustomer":
                case "utlSupply":
                case "utlCommodity":
                case "utlPerson_F":
                case "utlPerson_T":
                    ExVisualTreeHelper.SetFunctionKeyEnabled("F5", true, this);
                    activeControl = ctl;
                    break;
                default:
                    break;
            }
        }

        #endregion

        #endregion

        #region DataGrid Events

        private void dg_DoubleClick(object sender, EventArgs e)
        {
            if (Common.gWinGroupType == Common.geWinGroupType.InpList)
            {
                btnF1_Click(null, null);
            }
        }

        private void dg_KeyUp(object sender, KeyEventArgs e)
        {
            if (Common.gWinGroupType != Common.geWinGroupType.InpList) return;

            switch (e.Key)
            {
                case Key.Enter: this.btnF1_Click(null, null); break;
                default: break;
            }
        }

        #endregion

        #region Button Click Evnets

        private void btnSelectAll_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i <= lst.Count - 1; i++)
            {
                lst[i].exec_flg = true;
            }
            this.dgSelect.ItemsSource = null;
            this.dgSelect.ItemsSource = lst;
        }

        private void btnNoSelectAll_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i <= lst.Count - 1; i++)
            {
                lst[i].exec_flg = false;
            }
            this.dgSelect.ItemsSource = null;
            this.dgSelect.ItemsSource = lst;
        }

        #endregion

        #region Data Select

        #region データ取得

        // データ取得
        private void GetListData()
        {
            if (Common.gWinGroupType == Common.geWinGroupType.InpDetailReport) return;

            object[] prm = new object[2];
            string str = "";
            prm[0] = GetSQLWhere();
            prm[1] = "";
            webService.objPerent = this;

            if (searchBtnFlg == true)
            {
                webService.CallWebService(this.WebServiceCallKbn,
                                          ExWebService.geDialogDisplayFlg.Yes,
                                          ExWebService.geDialogCloseFlg.Yes,
                                          prm);
            }
            else
            {
                webService.CallWebService(this.WebServiceCallKbn,
                                          ExWebService.geDialogDisplayFlg.No,
                                          ExWebService.geDialogCloseFlg.No,
                                          prm);
            }
        }

        // 条件句SQL設定
        private string GetSQLWhere()
        {
            string strWhrer = "";
            string strWhrerString1 = "";
            string strWhrerString2 = "";

            // 受注日
            if (this.datOrderYmd_F.Text.Trim() != "")
            {
                strWhrer += "   AND T.ORDER_YMD >= " + ExEscape.zRepStr(this.datOrderYmd_F.Text.Trim()) + Environment.NewLine;
                strWhrerString1 += "[受注日 " + ExEscape.zRepStrNoQuota(this.datOrderYmd_F.Text.Trim()) + "～";
            }
            else
            {
                strWhrerString1 += "[受注日 未指定～";
            }
            if (this.datOrderYmd_T.Text.Trim() != "")
            {
                strWhrer += "   AND T.ORDER_YMD <= " + ExEscape.zRepStr(this.datOrderYmd_T.Text.Trim()) + Environment.NewLine;
                strWhrerString1 += ExEscape.zRepStrNoQuota(this.datOrderYmd_T.Text.Trim());
            }
            else
            {
                strWhrerString1 += "未指定";
            }

            // 受注番号
            if (this.utlOrderNo_F.txtID.Text.Trim() != "")
            {
                strWhrer += "   AND T.NO >= " + ExCast.zCLng(this.utlOrderNo_F.txtID.Text.Trim()) + Environment.NewLine;
                strWhrerString1 += "] [受注番号 " + this.utlOrderNo_F.txtID.Text.Trim() + "～";
            }
            else
            {
                strWhrerString1 += "] [受注番号 未指定～";
            }
            if (this.utlOrderNo_T.txtID.Text.Trim() != "")
            {
                strWhrer += "   AND T.NO <= " + ExCast.zCLng(this.utlOrderNo_T.txtID.Text.Trim()) + Environment.NewLine;
                strWhrerString1 += this.utlOrderNo_T.txtID.Text.Trim();
            }
            else
            {
                strWhrerString1 += "未指定";
            }

            // 納入指定日
            if (this.datNokiYmd_F.Text.Trim() != "")
            {
                strWhrer += "   AND T.SUPPLY_YMD >= " + ExEscape.zRepStr(this.datNokiYmd_F.Text.Trim()) + Environment.NewLine;
                strWhrerString1 += "] [納入指定日 " + ExEscape.zRepStrNoQuota(this.datNokiYmd_F.Text.Trim()) + "～";
            }
            else
            {
                strWhrerString1 += "] [納入指定日 未指定～";
            }
            if (this.datNokiYmd_T.Text.Trim() != "")
            {
                strWhrer += "   AND T.SUPPLY_YMD <= " + ExEscape.zRepStr(this.datNokiYmd_T.Text.Trim()) + Environment.NewLine;
                strWhrerString1 += ExEscape.zRepStrNoQuota(this.datNokiYmd_T.Text.Trim());
            }
            else
            {
                strWhrerString1 += "未指定";
            }

            // 見積番号
            if (this.utlEstimateNo_F.txtID.Text.Trim() != "")
            {
                strWhrer += "   AND T.ESTIMATENO >= " + ExCast.zCLng(this.utlEstimateNo_F.txtID.Text.Trim()) + Environment.NewLine;
                strWhrerString1 += "] [見積番号 " + this.utlEstimateNo_F.txtID.Text.Trim() + "～";
            }
            else
            {
                strWhrerString1 += "] [見積番号 未指定～";
            }
            if (this.utlEstimateNo_T.txtID.Text.Trim() != "")
            {
                strWhrer += "   AND T.ESTIMATENO <= " + ExCast.zCLng(this.utlEstimateNo_T.txtID.Text.Trim()) + Environment.NewLine;
                strWhrerString1 += this.utlEstimateNo_T.txtID.Text.Trim() + "]";
            }
            else
            {
                strWhrerString1 += "未指定]";
            }

            // 入力担当者
            if (this.utlPerson_F.txtID.Text.Trim() != "")
            {
                strWhrer += "   AND T.PERSON_ID >= " + ExCast.zCLng(this.utlPerson_F.txtID.Text.Trim()) + Environment.NewLine;
                strWhrerString2 += "[入力担当者 " + this.utlPerson_F.txtID.Text.Trim() + "：" + ExEscape.zRepStrNoQuota(this.utlPerson_F.txtNm.Text.Trim()) + "～";
            }
            else
            {
                strWhrerString2 += "[入力担当者 未指定～";
            }
            if (this.utlPerson_T.txtID.Text.Trim() != "")
            {
                strWhrer += "   AND T.PERSON_ID <= " + ExCast.zCLng(this.utlPerson_T.txtID.Text.Trim()) + Environment.NewLine;
                strWhrerString2 += this.utlPerson_T.txtID.Text.Trim() + "：" + ExEscape.zRepStrNoQuota(this.utlPerson_T.txtNm.Text.Trim());
            }
            else
            {
                strWhrerString2 += "未指定";
            }

            // 得意先
            if (this.utlCustomer.txtID.Text.Trim() != "")
            {
                strWhrer += "   AND T.CUSTOMER_ID = " + ExEscape.zRepStr(ExCast.zNumZeroNothingFormat(this.utlCustomer.txtID.Text.Trim())) + Environment.NewLine;
                strWhrerString2 += "] [得意先 " + ExEscape.zRepStrNoQuota(this.utlCustomer.txtID.Text.Trim()) + "：" + ExEscape.zRepStrNoQuota(this.utlCustomer.txtNm.Text.Trim());
            }
            else
            {
                strWhrerString2 += "] [得意先 未指定";
            }

            // 納入先
            if (this.utlSupply.txtID.Text.Trim() != "")
            {
                strWhrer += "   AND T.SUPPLIER_ID = " + ExEscape.zRepStr(ExCast.zNumZeroNothingFormat(this.utlSupply.txtID.Text.Trim())) + Environment.NewLine;
                strWhrerString2 += "] [納入先 " + ExEscape.zRepStrNoQuota(this.utlSupply.txtID.Text.Trim()) + "：" + ExEscape.zRepStrNoQuota(this.utlSupply.txtNm.Text.Trim());
            }
            else
            {
                strWhrerString2 += "] [納入先 未指定";
            }

            // 商品コード
            if (this.utlCommodity.txtID.Text.Trim() != "")
            {
                strWhrer += "   AND EXISTS (SELECT 1 " + Environment.NewLine;
                strWhrer += "                 FROM T_ORDER_D AS OOD " + Environment.NewLine;
                strWhrer += "                WHERE OOD.COMPANY_ID = T.COMPANY_ID " + Environment.NewLine;
                strWhrer += "                  AND OOD.GROUP_ID = T.GROUP_ID " + Environment.NewLine;
                strWhrer += "                  AND OOD.ORDER_ID = T.ID " + Environment.NewLine;
                strWhrer += "                  AND OOD.COMMODITY_ID = " + ExEscape.zRepStr(ExCast.zNumZeroNothingFormat(this.utlCommodity.txtID.Text.Trim())) + Environment.NewLine;
                strWhrer += "                  ) " + Environment.NewLine;
                strWhrerString2 += "] [商品コード " + ExEscape.zRepStrNoQuota(this.utlCommodity.txtID.Text.Trim()) + "：" + ExEscape.zRepStrNoQuota(this.utlCommodity.txtNm.Text.Trim()) + "]";
            }
            else
            {
                strWhrerString2 += "] [商品コード 未指定" + "]";
            }

            // 取引区分
            string _buf = "";
            if (this.chkKake.IsChecked == true)
            {
                // 掛売上
                strWhrer += "   AND (T.BUSINESS_DIVISION_ID = 1";
                _buf += " [取引区分 掛売上";
            }
            if (this.chkCash.IsChecked == true)
            {
                // 現金
                if (_buf == "")
                {
                    strWhrer += "   AND (T.BUSINESS_DIVISION_ID = 2";
                    _buf += " [取引区分 現金";
                }
                else
                {
                    strWhrer += " OR T.BUSINESS_DIVISION_ID = 2";
                    _buf += " 現金";
                }
            }
            if (_buf == "")
            {
                _buf += " [取引区分 指定無し]";
            }
            else
            {
                strWhrer += ")" + Environment.NewLine;
                _buf += "]";
            }
            strWhrerString2 += _buf;
            _buf = "";

            // 完納区分
            if (this.chkUnDelivary.IsChecked == true)
            {
                // 未納
                strWhrer += "   AND (EXISTS (SELECT 1 " + Environment.NewLine;
                strWhrer += "                  FROM T_ORDER_D AS OOD " + Environment.NewLine;
                strWhrer += "                 WHERE OOD.COMPANY_ID = T.COMPANY_ID " + Environment.NewLine;
                strWhrer += "                   AND OOD.GROUP_ID = T.GROUP_ID " + Environment.NewLine;
                strWhrer += "                   AND OOD.ORDER_ID = T.ID " + Environment.NewLine;
                strWhrer += "                   AND OOD.DELIVER_DIVISION_ID = 1 " + Environment.NewLine;
                strWhrer += "                   ) " + Environment.NewLine;
                _buf += " [完納区分 未納";
            }
            if (this.chkPartDelivary.IsChecked == true)
            {
                // 一部納品
                if (_buf == "")
                {
                    strWhrer += "   AND (EXISTS (SELECT 1 " + Environment.NewLine;
                    strWhrer += "                  FROM T_ORDER_D AS OOD " + Environment.NewLine;
                    strWhrer += "                 WHERE OOD.COMPANY_ID = T.COMPANY_ID " + Environment.NewLine;
                    strWhrer += "                   AND OOD.GROUP_ID = T.GROUP_ID " + Environment.NewLine;
                    strWhrer += "                   AND OOD.ORDER_ID = T.ID " + Environment.NewLine;
                    strWhrer += "                   AND OOD.DELIVER_DIVISION_ID = 2 " + Environment.NewLine;
                    strWhrer += "                   ) " + Environment.NewLine;
                    _buf += " [完納区分 分納";
                }
                else
                {
                    strWhrer += "OR EXISTS (SELECT 1 " + Environment.NewLine;
                    strWhrer += "             FROM T_ORDER_D AS OOD " + Environment.NewLine;
                    strWhrer += "            WHERE OOD.COMPANY_ID = T.COMPANY_ID " + Environment.NewLine;
                    strWhrer += "              AND OOD.GROUP_ID = T.GROUP_ID " + Environment.NewLine;
                    strWhrer += "              AND OOD.ORDER_ID = T.ID " + Environment.NewLine;
                    strWhrer += "              AND OOD.DELIVER_DIVISION_ID = 2 " + Environment.NewLine;
                    strWhrer += "              ) " + Environment.NewLine;
                    _buf += " 分納";
                }

            }
            if (this.chkFullDelivary.IsChecked == true)
            {
                // 完納
                if (_buf == "")
                {
                    strWhrer += "   AND (EXISTS (SELECT 1 " + Environment.NewLine;
                    strWhrer += "                  FROM T_ORDER_D AS OOD " + Environment.NewLine;
                    strWhrer += "                 WHERE OOD.COMPANY_ID = T.COMPANY_ID " + Environment.NewLine;
                    strWhrer += "                   AND OOD.GROUP_ID = T.GROUP_ID " + Environment.NewLine;
                    strWhrer += "                   AND OOD.ORDER_ID = T.ID " + Environment.NewLine;
                    strWhrer += "                   AND OOD.DELIVER_DIVISION_ID = 3 " + Environment.NewLine;
                    strWhrer += "                   ) " + Environment.NewLine;
                    _buf += " [完納区分 完納";
                }
                else
                {
                    strWhrer += "OR EXISTS (SELECT 1 " + Environment.NewLine;
                    strWhrer += "             FROM T_ORDER_D AS OOD " + Environment.NewLine;
                    strWhrer += "            WHERE OOD.COMPANY_ID = T.COMPANY_ID " + Environment.NewLine;
                    strWhrer += "              AND OOD.GROUP_ID = T.GROUP_ID " + Environment.NewLine;
                    strWhrer += "              AND OOD.ORDER_ID = T.ID " + Environment.NewLine;
                    strWhrer += "              AND OOD.DELIVER_DIVISION_ID = 3 " + Environment.NewLine;
                    strWhrer += "              ) " + Environment.NewLine;
                    _buf += " 完納";
                }
            }
            if (this.chkTorikeshi.IsChecked == true)
            {
                // 取消
                if (_buf == "")
                {
                    strWhrer += "   AND (EXISTS (SELECT 1 " + Environment.NewLine;
                    strWhrer += "                  FROM T_ORDER_D AS OOD " + Environment.NewLine;
                    strWhrer += "                 WHERE OOD.COMPANY_ID = T.COMPANY_ID " + Environment.NewLine;
                    strWhrer += "                   AND OOD.GROUP_ID = T.GROUP_ID " + Environment.NewLine;
                    strWhrer += "                   AND OOD.ORDER_ID = T.ID " + Environment.NewLine;
                    strWhrer += "                   AND OOD.DELIVER_DIVISION_ID = 4 " + Environment.NewLine;
                    strWhrer += "                   ) " + Environment.NewLine;
                    _buf += " [完納区分 取消";
                }
                else
                {
                    strWhrer += "OR EXISTS (SELECT 1 " + Environment.NewLine;
                    strWhrer += "             FROM T_ORDER_D AS OOD " + Environment.NewLine;
                    strWhrer += "            WHERE OOD.COMPANY_ID = T.COMPANY_ID " + Environment.NewLine;
                    strWhrer += "              AND OOD.GROUP_ID = T.GROUP_ID " + Environment.NewLine;
                    strWhrer += "              AND OOD.ORDER_ID = T.ID " + Environment.NewLine;
                    strWhrer += "              AND OOD.DELIVER_DIVISION_ID = 4 " + Environment.NewLine;
                    strWhrer += "              ) " + Environment.NewLine;
                    _buf += " 取消";
                }
            }
            if (_buf == "")
            {
                _buf += " [完納区分 指定無し]";
            }
            else
            {
                strWhrer += ")" + Environment.NewLine;
                _buf += "]";
            }
            strWhrerString2 += _buf;

            _buf = "";

            // 注文請書発行
            if (this.borPrint.Visibility == System.Windows.Visibility.Visible)
            {
                if (this.chkPrintNo.IsChecked == true)
                {
                    strWhrer += "   AND (T.ORDER_PRINT_FLG = 0";
                    _buf += " [注文請書発行 発行未";
                }
                if (this.chkPrintYes.IsChecked == true)
                {
                    if (_buf == "")
                    {
                        strWhrer += "   AND (T.ORDER_PRINT_FLG = 1";
                        _buf += " [注文請書発行 発行済";
                    }
                    else
                    {
                        strWhrer += " OR T.ORDER_PRINT_FLG = 1";
                        _buf += " 発行済";
                    }
                }
                if (_buf == "")
                {
                    _buf += " [注文請書発行 指定無し]";
                }
                else
                {
                    strWhrer += ")" + Environment.NewLine;
                    _buf += "]";
                }
                strWhrerString2 += _buf;
                _buf = "";
            }

            UpdPrintNo = "";
            if (this.ProcKbn == eProcKbn.Report)
            {
                strWhrer = "";
                strWhrerString1 = "";
                strWhrerString2 = "";

                string _no = "";
                for (int i = 0; i <= lst.Count - 1; i++)
                {
                    if (lst[i].exec_flg == true)
                    {
                        if (string.IsNullOrEmpty(_no))
                        {
                            _no += ExCast.zCLng(this.lst[i].no).ToString();
                        }
                        else
                        {
                            _no += "<<@escape_comma@>>" + ExCast.zCLng(this.lst[i].no).ToString();
                        }
                    }
                }
                if (!string.IsNullOrEmpty(_no))
                {
                    UpdPrintNo = _no;
                    strWhrer += "   AND T.NO IN (" + _no + ")" + Environment.NewLine;
                }
            }

            return strWhrer + "WhereString =>" + strWhrerString1 + ";" + strWhrerString2;
        }

        #endregion

        #region 受注データ取得コールバック呼出(ExWebServiceクラスより呼出)

        // 受注リスト取得コールバック呼出
        public override void DataSelect(int intKbn, object objList)
        {
            if ((ExWebService.geWebServiceCallKbn)intKbn == this.WebServiceCallKbn)
            {
                if (objList != null)
                {
                    entityList = (ObservableCollection<EntityOrder>)objList;
                    var records =
                         (from n in entityList
                          orderby n.order_ymd descending, n.no descending
                          select new { n.no, n.order_ymd, n.estimateno, n.customer_nm, n.supplier_nm, n.supply_ymd, n.business_division_nm, n.deliver_division_nm }).Distinct();

                    this.lst.Clear();
                    foreach (var rec in records)
                    {
                        string _no = ExCast.zFormatForID(rec.no, Common.gintidFigureSlipNo);
                        string _estimate_no = ExCast.zFormatForID(rec.estimateno, Common.gintidFigureSlipNo);
                        if (ExCast.zCLng(_estimate_no) == 0) _estimate_no = "";

                        lst.Add(new DisplayOrderList(rec.no,
                                                     _no,
                                                     rec.order_ymd,
                                                     rec.estimateno,
                                                     _estimate_no,
                                                     rec.customer_nm,
                                                     rec.supplier_nm,
                                                     rec.supply_ymd,
                                                     rec.business_division_nm,
                                                     rec.deliver_division_nm));

                    }

                    this.dg.Focus();
                    this.dg.ItemsSource = null;
                    this.dg.ItemsSource = lst;
                    this.dgSelect.ItemsSource = null;
                    this.dgSelect.ItemsSource = lst;
                    if (lst.Count > 0)
                    {
                        this.dg.SelectedIndex = 0;
                    }
                    ExBackgroundWorker.DoWork_Focus(dg, 10);
                }
                else
                {
                    entityList = null;
                    this.lst.Clear();
                    this.dg.ItemsSource = null;
                    this.dgSelect.ItemsSource = null;
                    ExBackgroundWorker.DoWork_Focus(this.datOrderYmd_F, 10);
                }
            }
        }

        #endregion

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
                    case eProcKbn.Report:

                        #region 伝票レポート出力時チェック

                        #region 選択チェック

                        if (this.lst == null)
                        {
                            errMessage += "表示データがありません。" + Environment.NewLine;
                        }
                        if (this.lst.Count == 0)
                        {
                            errMessage += "表示データがありません。" + Environment.NewLine;
                        }

                        bool _exec_flg = false;
                        for (int i = 0; i <= lst.Count - 1; i++)
                        {
                            if (lst[i].exec_flg == true)
                            {
                                _exec_flg = true;
                            }
                        }
                        if (_exec_flg == false)
                        {
                            errMessage += "レポート対象データを選択して下さい。" + Environment.NewLine;
                            if (errCtl == null) errCtl = this.dgSelect;
                        }

                        #endregion

                        #endregion

                        break;
                    case eProcKbn.Search:
                    case eProcKbn.ReportDetail:

                        #region 入力チェック

                        // 入力担当者
                        if (this.utlPerson_F.txtID.Text.Trim() != "" && string.IsNullOrEmpty(this.utlPerson_F.txtNm.Text.Trim()))
                        {
                            errMessage += "入力担当者(開始)が適切に入力(選択)されていません。" + Environment.NewLine;
                            if (errCtl == null) errCtl = this.utlPerson_F.txtID;
                        }

                        // 入力担当者
                        if (this.utlPerson_T.txtID.Text.Trim() != "" && string.IsNullOrEmpty(this.utlPerson_T.txtNm.Text.Trim()))
                        {
                            errMessage += "入力担当者(終了)が適切に入力(選択)されていません。" + Environment.NewLine;
                            if (errCtl == null) errCtl = this.utlPerson_T.txtID;
                        }

                        // 得意先
                        if (this.utlCustomer.txtID.Text.Trim() != "" && string.IsNullOrEmpty(this.utlCustomer.txtNm.Text.Trim()))
                        {
                            errMessage += "得意先が適切に入力(選択)されていません。" + Environment.NewLine;
                            if (errCtl == null) errCtl = this.utlCustomer.txtID;
                        }

                        // 納入先
                        if (this.utlSupply.txtID.Text.Trim() != "" && string.IsNullOrEmpty(this.utlSupply.txtNm.Text.Trim()))
                        {
                            errMessage += "納入先が適切に入力(選択)されていません。" + Environment.NewLine;
                            if (errCtl == null) errCtl = this.utlSupply.txtID;
                        }

                        // 商品先
                        if (this.utlCommodity.txtID.Text.Trim() != "" && string.IsNullOrEmpty(this.utlCommodity.txtNm.Text.Trim()))
                        {
                            errMessage += "商品先が適切に入力(選択)されていません。" + Environment.NewLine;
                            if (errCtl == null) errCtl = this.utlCommodity.txtID;
                        }

                        #endregion

                        #region 範囲チェック

                        // 受注番号
                        if (!string.IsNullOrEmpty(this.utlOrderNo_F.txtID.Text.Trim()) && !string.IsNullOrEmpty(this.utlOrderNo_T.txtID.Text.Trim()))
                        {
                            if (ExCast.zCLng(this.utlOrderNo_F.txtID.Text.Trim()) > ExCast.zCLng(this.utlOrderNo_T.txtID.Text.Trim()))
                            {
                                errMessage += "受注番号の範囲指定が不正です。" + Environment.NewLine;
                                if (errCtl == null) errCtl = this.utlOrderNo_F.txtID;
                            }
                        }
                        // 受注日
                        if (!string.IsNullOrEmpty(this.datOrderYmd_F.Text.Trim()) && !string.IsNullOrEmpty(this.datOrderYmd_T.Text.Trim()))
                        {
                            if (ExCast.zCLng(this.datOrderYmd_F.Text.Replace("/", "")) > ExCast.zCLng(this.datOrderYmd_T.Text.Replace("/", "")))
                            {
                                errMessage += "受注日の範囲指定が不正です。" + Environment.NewLine;
                                if (errCtl == null) errCtl = this.datOrderYmd_F;
                            }
                        }
                        // 見積番号
                        if (!string.IsNullOrEmpty(this.utlEstimateNo_F.txtID.Text.Trim()) && !string.IsNullOrEmpty(this.utlEstimateNo_T.txtID.Text.Trim()))
                        {
                            if (ExCast.zCLng(this.utlEstimateNo_F.txtID.Text.Trim()) > ExCast.zCLng(this.utlEstimateNo_T.txtID.Text.Trim()))
                            {
                                errMessage += "見積番号の範囲指定が不正です。" + Environment.NewLine;
                                if (errCtl == null) errCtl = this.utlEstimateNo_F.txtID;
                            }
                        }
                        // 納入指定日
                        if (!string.IsNullOrEmpty(this.datNokiYmd_F.Text.Trim()) && !string.IsNullOrEmpty(this.datNokiYmd_T.Text.Trim()))
                        {
                            if (ExCast.zCLng(this.datNokiYmd_F.Text.Replace("/", "")) > ExCast.zCLng(this.datNokiYmd_T.Text.Replace("/", "")))
                            {
                                errMessage += "納入指定日の範囲指定が不正です。" + Environment.NewLine;
                                if (errCtl == null) errCtl = this.datNokiYmd_F;
                            }
                        }
                        // 入力担当者
                        if (!string.IsNullOrEmpty(this.utlPerson_F.txtID.Text.Trim()) && !string.IsNullOrEmpty(this.utlPerson_T.txtID.Text.Trim()))
                        {
                            if (ExCast.zCLng(this.utlPerson_F.txtID.Text.Trim()) > ExCast.zCLng(this.utlPerson_T.txtID.Text.Trim()))
                            {
                                errMessage += "入力担当者の範囲指定が不正です。" + Environment.NewLine;
                                if (errCtl == null) errCtl = this.utlPerson_F.txtID;
                            }
                        }

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

                if (this.utlReport.rptKbn == DataReport.geReportKbn.None)
                {
                    GetListData();
                }
                else
                {
                    this.utlReport.utlParentFKey = this.utlFKey;

                    switch (Common.gWinGroupType)
                    {
                        case Common.geWinGroupType.InpListReport:
                            this.utlReport.pgId = DataPgEvidence.PGName.Order.OrderPrint;
                            break;
                        case Common.geWinGroupType.InpDetailReport:
                            this.utlReport.pgId = DataPgEvidence.PGName.Order.OrderDPrint;
                            break;
                    }

                    this.utlReport.sqlWhere = GetSQLWhere();
                    this.utlReport.sqlOrderBy = "";
                    this.utlReport.updPrintNo = this.UpdPrintNo;
                    this.utlReport.ReportStart();
                }
            }
            finally
            {
                searchBtnFlg = false;
                Common.gblnBtnProcLock = false;
                Common.gblnBtnDesynchronizeLock = false;
            }
        }

        #endregion
        
        public class DisplayOrderList
        {
            private bool _exec_flg = false;
            public bool exec_flg { set { this._exec_flg = value; } get { return this._exec_flg; } }

            private long _no = 0;
            public long no { set { this._no = value; } get { return this._no; } }

            private string _str_no = "";
            public string str_no { set { this._str_no = value; } get { return this._str_no; } }

            private string _order_ymd = "";
            public string order_ymd { set { this._order_ymd = value; } get { return this._order_ymd; } }

            private long _estimateno = 0;
            public long estimateno { set { this._estimateno = value; } get { return this._estimateno; } }

            private string _str_estimate_no = "";
            public string str_estimate_no { set { this._str_estimate_no = value; } get { return this._str_estimate_no; } }

            private string _customer_nm = "";
            public string customer_nm { set { this._customer_nm = value; } get { return this._customer_nm; } }

            private string _supplier_nm = "";
            public string supplier_nm { set { this._supplier_nm = value; } get { return this._supplier_nm; } }

            private string _supply_ymd = "";
            public string supply_ymd { set { this._supply_ymd = value; } get { return this._supply_ymd; } }

            private string _business_division_nm = "";
            public string business_division_nm { set { this._business_division_nm = value; } get { return this._business_division_nm; } }

            private string _deliver_division_nm = "";
            public string deliver_division_nm { set { this._deliver_division_nm = value; } get { return this._deliver_division_nm; } }

            public DisplayOrderList(long no
                                  , string str_no
                                  , string order_ymd
                                  , long estimateno
                                  , string str_estimate_no
                                  , string customer_nm
                                  , string supplier_nm
                                  , string supply_ymd
                                  , string business_division_nm
                                  , string deliver_division_nm)
            {
                this.no = no;
                this.str_no = str_no;
                this.order_ymd = order_ymd;
                this.estimateno = estimateno;
                this.str_estimate_no = str_estimate_no;
                this.customer_nm = customer_nm;
                this.supplier_nm = supplier_nm;
                this.supply_ymd = supply_ymd;
                this.business_division_nm = business_division_nm;
                this.deliver_division_nm = deliver_division_nm;
            }
        }

    }

}
