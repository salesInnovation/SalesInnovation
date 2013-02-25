using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
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
using SlvHanbaiClient.Class.WebService;
using SlvHanbaiClient.Class.Entity;
using SlvHanbaiClient.svcSales;
using SlvHanbaiClient.View.Dlg;
using SlvHanbaiClient.View.Dlg.Report;
using SlvHanbaiClient.View.UserControl.Custom;
using SlvHanbaiClient.View.UserControl.Report;

namespace SlvHanbaiClient.View.UserControl.Input.Sales
{
    public partial class Utl_InpSearchPlan : ExUserControl
    {

        #region Filed Const

        private const String CLASS_NM = "Utl_InpSearchPlan";
        private ObservableCollection<EntitySales> entityList;
        private Control activeControl;

        private Utl_FunctionKey utlFKey = null;
        private Utl_Report utlReport = new Utl_Report();

        private EntitySearch entity = new EntitySearch();

        private long _no;
        public long no { set { this._no = value; } get { return this._no; } }

        private bool _DialogResult;
        public bool DialogResult { set { this._DialogResult = value; } get { return this._DialogResult; } }

        private ExWebService.geWebServiceCallKbn _WebServiceCallKbn = ExWebService.geWebServiceCallKbn.GetSalesList;
        public ExWebService.geWebServiceCallKbn WebServiceCallKbn { set { this._WebServiceCallKbn = value; } get { return this._WebServiceCallKbn; } }

        private bool searchBtnFlg = false;

        private enum eProcKbn { Search = 0, Report, ReportDetail };
        private eProcKbn ProcKbn;

        private string UpdPrintNo = "";

        private Dlg_ReportSetting dlgReportSetting = new Dlg_ReportSetting();

        #endregion

        #region Constructor

        public Utl_InpSearchPlan()
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

            if (Common.gWinType == Common.geWinType.ListCollectPlan)
            {
                this.lblCollectPlanYmd.Content = "回収予定日";
                this.utlInvoice_F.Visibility = System.Windows.Visibility.Visible;
                this.utlInvoice_T.Visibility = System.Windows.Visibility.Visible;
                this.utlPurchase_F.Visibility = System.Windows.Visibility.Collapsed;
                this.utlPurchase_T.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                this.lblCollectPlanYmd.Content = "支払予定日";
                this.utlInvoice_F.Visibility = System.Windows.Visibility.Collapsed;
                this.utlInvoice_T.Visibility = System.Windows.Visibility.Collapsed;
                this.utlPurchase_F.Visibility = System.Windows.Visibility.Visible;
                this.utlPurchase_T.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void ExUserControl_Unloaded(object sender, RoutedEventArgs e)
        {
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

            if (Common.gWinType == Common.geWinType.ListCollectPlan)
            {
                this.lblCollectPlanYmd.Content = "回収予定日";
                this.utlInvoice_F.Visibility = System.Windows.Visibility.Visible;
                this.utlInvoice_T.Visibility = System.Windows.Visibility.Visible;
                this.utlPurchase_F.Visibility = System.Windows.Visibility.Collapsed;
                this.utlPurchase_T.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                this.lblCollectPlanYmd.Content = "支払予定日";
                this.utlInvoice_F.Visibility = System.Windows.Visibility.Collapsed;
                this.utlInvoice_T.Visibility = System.Windows.Visibility.Collapsed;
                this.utlPurchase_F.Visibility = System.Windows.Visibility.Visible;
                this.utlPurchase_T.Visibility = System.Windows.Visibility.Visible;
            }

            // ファンクションキー初期設定
            utlFKey = ExVisualTreeHelper.GetUtlFunctionKey(this);
            this.utlFKey.Init();

            // レポート初期設定
            this.utlReport.gPageType = Common.gPageType;
            this.utlReport.gWinMsterType = Common.geWinMsterType.None;
            this.utlReport.parentUtl = this;
            this.utlReport.Init();

            entityList = null;
            this.dg.ItemsSource = null;
            this.dg.ItemsSource = entityList;

            switch (Common.gWinGroupType)
            {
                case Common.geWinGroupType.InpList:
                    // 出力対象日初期設定
                    DataInitWhere.InitDate(DataInitWhere.geDateKbn.Month, ref this.datCollectPlanYmd_F, ref this.datCollectPlanYmd_T);

                    // DataGrid設定
                    this.GridDetail.Visibility = System.Windows.Visibility.Visible;

                    break;
                case Common.geWinGroupType.InpListReport:
                    // 出力対象日初期設定
                    DataInitWhere.InitDate(DataInitWhere.geDateKbn.Today, ref this.datCollectPlanYmd_F, ref this.datCollectPlanYmd_T);

                    // DataGrid設定
                    this.GridDetail.Visibility = System.Windows.Visibility.Collapsed;

                    break;
                case Common.geWinGroupType.InpDetailReport:
                    // 出力対象日初期設定
                    DataInitWhere.InitDate(DataInitWhere.geDateKbn.Today, ref this.datCollectPlanYmd_F, ref this.datCollectPlanYmd_T);

                    // DataGrid設定
                    this.GridDetail.Visibility = System.Windows.Visibility.Visible;
                    this.dg.ItemsSource = null;
                    this.dg.ItemsSource = entityList;

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
            //entity.PropertyChanged += this.utlPerson_F.MstID_Changed;
            //entity.PropertyChanged += this.utlPerson_T.MstID_Changed;
            //entity.PropertyChanged += this.utlSupply.MstID_Changed;
            //entity.PropertyChanged += this.utlCommodity.MstID_Changed;

            #region Bind

            // バインド
            //Binding BindingPersonF = new Binding("person_id_from");
            //BindingPersonF.Mode = BindingMode.TwoWay;
            //BindingPersonF.Source = entity;
            //this.utlPerson_F.txtID.SetBinding(TextBox.TextProperty, BindingPersonF);

            //Binding BindingPersonT = new Binding("person_id_to");
            //BindingPersonT.Mode = BindingMode.TwoWay;
            //BindingPersonT.Source = entity;
            //this.utlPerson_T.txtID.SetBinding(TextBox.TextProperty, BindingPersonT);

            if (Common.gWinType == Common.geWinType.ListCollectPlan)
            {
                entity.PropertyChanged -= this.utlInvoice_F.MstID_Changed;
                entity.PropertyChanged -= this.utlInvoice_T.MstID_Changed;
                entity.PropertyChanged += this.utlInvoice_F.MstID_Changed;
                entity.PropertyChanged += this.utlInvoice_T.MstID_Changed;

                Binding BindingInvoiceF = new Binding("invoice_id_from");
                BindingInvoiceF.Mode = BindingMode.TwoWay;
                BindingInvoiceF.Source = entity;
                this.utlInvoice_F.txtID.SetBinding(TextBox.TextProperty, BindingInvoiceF);

                Binding BindingInvoiceT = new Binding("invoice_id_to");
                BindingInvoiceT.Mode = BindingMode.TwoWay;
                BindingInvoiceT.Source = entity;
                this.utlInvoice_T.txtID.SetBinding(TextBox.TextProperty, BindingInvoiceT);

                this.utlInvoice_F.txtID.SetZeroToNullString();
                this.utlInvoice_T.txtID.SetZeroToNullString();
            }
            else
            {
                entity.PropertyChanged -= this.utlPurchase_F.MstID_Changed;
                entity.PropertyChanged -= this.utlPurchase_T.MstID_Changed;
                entity.PropertyChanged += this.utlPurchase_F.MstID_Changed;
                entity.PropertyChanged += this.utlPurchase_T.MstID_Changed;

                Binding BindingPurchaseF = new Binding("purchase_id_from");
                BindingPurchaseF.Mode = BindingMode.TwoWay;
                BindingPurchaseF.Source = entity;
                this.utlPurchase_F.txtID.SetBinding(TextBox.TextProperty, BindingPurchaseF);

                Binding BindingPurchaseT = new Binding("purchase_id_to");
                BindingPurchaseT.Mode = BindingMode.TwoWay;
                BindingPurchaseT.Source = entity;
                this.utlPurchase_T.txtID.SetBinding(TextBox.TextProperty, BindingPurchaseT);

                this.utlPurchase_F.txtID.SetZeroToNullString();
                this.utlPurchase_T.txtID.SetZeroToNullString();
            }


            //Binding BindingSupply = new Binding("supplier_id");
            //BindingSupply.Mode = BindingMode.TwoWay;
            //BindingSupply.Source = entity;
            //this.utlSupply.txtID.SetBinding(TextBox.TextProperty, BindingSupply);
            //this.utlSupply.txtID2.SetBinding(TextBox.TextProperty, BindingCustomer);

            //Binding BindingCommodity = new Binding("commodity_id");
            //BindingCommodity.Mode = BindingMode.TwoWay;
            //BindingCommodity.Source = entity;
            //this.utlCommodity.txtID.SetBinding(TextBox.TextProperty, BindingCommodity);

            #endregion

            //this.utlPerson_F.txtID.OnFormatString();
            //this.utlPerson_T.txtID.SetZeroToNullString();
            //this.utlSupply.txtID.SetZeroToNullString();
            //this.utlCommodity.txtID.SetZeroToNullString();
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

                this.no = this.entityList[this.dg.SelectedIndex].no;
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
            this.entityList.Clear();
            this.dg.ItemsSource = null;

            if (Common.gWinGroupType == Common.geWinGroupType.InpList)
            {
                DataInitWhere.InitDate(DataInitWhere.geDateKbn.Month, ref this.datCollectPlanYmd_F, ref this.datCollectPlanYmd_T);
            }
            else
            {
                DataInitWhere.InitDate(DataInitWhere.geDateKbn.Today, ref this.datCollectPlanYmd_F, ref this.datCollectPlanYmd_T);
            }

            ExBackgroundWorker.DoWork_Focus(this.datCollectPlanYmd_F, 10);
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
                case "datCollectPlanYmd_F":
                case "datCollectPlanYmd_T":
                    ExDatePicker dt = (ExDatePicker)activeControl;
                    dt.ShowCalender();
                    break;
                case "utlInvoice_F":
                case "utlInvoice_T":
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

            searchBtnFlg = true;
        }

        // F11ボタン(出力設定) クリック
        public override void btnF11_Click(object sender, RoutedEventArgs e)
        {
            if (Common.gWinGroupType == Common.geWinGroupType.InpList) return;

            if (Common.gWinType == Common.geWinType.ListCollectPlan)
            {
                this.dlgReportSetting.pg_id = DataPgEvidence.PGName.Plan.CollectPlanPrint;
                this.dlgReportSetting.IsReportRange = true;
                this.dlgReportSetting.IsTotalKbn = false;
                this.dlgReportSetting.IsGroupTotal = true;
            }
            else if (Common.gWinType == Common.geWinType.ListPaymentPlan)
            {
                this.dlgReportSetting.pg_id = DataPgEvidence.PGName.Plan.PaymentPlanPrint;
                this.dlgReportSetting.IsReportRange = true;
                this.dlgReportSetting.IsTotalKbn = false;
                this.dlgReportSetting.IsGroupTotal = true;
            }

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
                case "datCollectPlanYmd_F":
                case "datCollectPlanYmd_T":
                case "utlInvoice_F":
                case "utlInvoice_T":
                    ExVisualTreeHelper.SetFunctionKeyEnabled("F5", true, this);
                    activeControl = ctl;
                    break;
                default:
                    ExVisualTreeHelper.SetFunctionKeyEnabled("F5", false, this);
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
            //for (int i = 0; i <= entityList.Count - 1; i++)
            //{
            //    entityList[i].exec_flg = true;
            //}
            //this.dgSelect.ItemsSource = null;
            //this.dgSelect.ItemsSource = entityList;
        }

        private void btnNoSelectAll_Click(object sender, RoutedEventArgs e)
        {
            //for (int i = 0; i <= entityList.Count - 1; i++)
            //{
            //    entityList[i].exec_flg = false;
            //}
            //this.dgSelect.ItemsSource = null;
            //this.dgSelect.ItemsSource = entityList;
        }

        #endregion

        #region Data Select

        #region データ取得

        // データ取得
        private void GetListData()
        {
            if (Common.gWinGroupType == Common.geWinGroupType.InpDetailReport) return;

            object[] prm = new object[2];
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

            

            if (Common.gWinType == Common.geWinType.ListCollectPlan)
            {

                #region 回収予定日

                if (this.datCollectPlanYmd_F.Text.Trim() != "")
                {
                    strWhrer += "   AND T.COLLECT_PLAN_DAY >= " + ExEscape.zRepStr(this.datCollectPlanYmd_F.Text.Trim()) + Environment.NewLine;
                    strWhrerString1 += "[回収予定日 " + ExEscape.zRepStrNoQuota(this.datCollectPlanYmd_F.Text.Trim()) + "～";
                }
                else
                {
                    strWhrerString1 += "[回収予定日 未指定～";
                }
                if (this.datCollectPlanYmd_T.Text.Trim() != "")
                {
                    strWhrer += "   AND T.COLLECT_PLAN_DAY <= " + ExEscape.zRepStr(this.datCollectPlanYmd_T.Text.Trim()) + Environment.NewLine;
                    strWhrerString1 += ExEscape.zRepStrNoQuota(this.datCollectPlanYmd_T.Text.Trim()) + "]";
                }
                else
                {
                    strWhrerString1 += "未指定]";
                }

                #endregion

                #region 請求先

                if (this.utlInvoice_F.txtID.Text.Trim() != "")
                {
                    strWhrer += "   AND T.INVOICE_ID >= " + ExEscape.zRepStr(ExCast.zNumZeroNothingFormat(this.utlInvoice_F.txtID.Text.Trim())) + Environment.NewLine;
                    strWhrerString1 += "] [請求先 " + ExEscape.zRepStrNoQuota(this.utlInvoice_F.txtID.Text.Trim()) + "：" + ExEscape.zRepStrNoQuota(this.utlInvoice_F.txtNm.Text.Trim()) + "～";
                }
                else
                {
                    strWhrerString1 += "] [請求先 未指定～";
                }
                if (this.utlInvoice_T.txtID.Text.Trim() != "")
                {
                    strWhrer += "   AND T.INVOICE_ID >= " + ExEscape.zRepStr(ExCast.zNumZeroNothingFormat(this.utlInvoice_T.txtID.Text.Trim())) + Environment.NewLine;
                    strWhrerString1 += ExEscape.zRepStrNoQuota(this.utlInvoice_T.txtID.Text.Trim()) + "：" + ExEscape.zRepStrNoQuota(this.utlInvoice_T.txtNm.Text.Trim()) + "]";
                }
                else
                {
                    strWhrerString1 += "未指定]";
                }

                #endregion

            }
            else if (Common.gWinType == Common.geWinType.ListPaymentPlan)
            {

                #region 支払予定日

                if (this.datCollectPlanYmd_F.Text.Trim() != "")
                {
                    strWhrer += "   AND T.PAYMENT_PLAN_DAY >= " + ExEscape.zRepStr(this.datCollectPlanYmd_F.Text.Trim()) + Environment.NewLine;
                    strWhrerString1 += "[支払予定日 " + ExEscape.zRepStrNoQuota(this.datCollectPlanYmd_F.Text.Trim()) + "～";
                }
                else
                {
                    strWhrerString1 += "[支払予定日 未指定～";
                }
                if (this.datCollectPlanYmd_T.Text.Trim() != "")
                {
                    strWhrer += "   AND T.PAYMENT_PLAN_DAY <= " + ExEscape.zRepStr(this.datCollectPlanYmd_T.Text.Trim()) + Environment.NewLine;
                    strWhrerString1 += ExEscape.zRepStrNoQuota(this.datCollectPlanYmd_T.Text.Trim()) + "]";
                }
                else
                {
                    strWhrerString1 += "未指定]";
                }

                #endregion

                #region 仕入先

                if (this.utlPurchase_F.txtID.Text.Trim() != "")
                {
                    strWhrer += "   AND T.PURCHASE_ID >= " + ExEscape.zRepStr(ExCast.zNumZeroNothingFormat(this.utlPurchase_F.txtID.Text.Trim())) + Environment.NewLine;
                    strWhrerString1 += "] [仕入先 " + ExEscape.zRepStrNoQuota(this.utlPurchase_F.txtID.Text.Trim()) + "：" + ExEscape.zRepStrNoQuota(this.utlPurchase_F.txtNm.Text.Trim()) + "～";
                }
                else
                {
                    strWhrerString1 += "] [仕入先 未指定～";
                }
                if (this.utlPurchase_T.txtID.Text.Trim() != "")
                {
                    strWhrer += "   AND T.PURCHASE_ID >= " + ExEscape.zRepStr(ExCast.zNumZeroNothingFormat(this.utlPurchase_T.txtID.Text.Trim())) + Environment.NewLine;
                    strWhrerString1 += ExEscape.zRepStrNoQuota(this.utlPurchase_T.txtID.Text.Trim()) + "：" + ExEscape.zRepStrNoQuota(this.utlPurchase_T.txtNm.Text.Trim()) + "]";
                }
                else
                {
                    strWhrerString1 += "未指定]";
                }

                #endregion

            }
            
            string _buf = "";
            
            //if (Common.gWinType == Common.geWinType.ListSalesDay)
            //{
            //    strWhrer += "<group kbn>1";
            //}
            //else if (Common.gWinType == Common.geWinType.ListSalesMonth)
            //{
            //    strWhrer += "<group kbn>2";
            //}
            //else if (Common.gWinType == Common.geWinType.ListSalesChange)
            //{
            //    strWhrer += "<group kbn>3";
            //}

            strWhrer = strWhrer.Replace(",", "<<@escape_comma@>>");

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
                    entityList = (ObservableCollection<EntitySales>)objList;
                    this.dg.Focus();
                    this.dg.ItemsSource = null;
                    this.dg.ItemsSource = entityList;
                    ExBackgroundWorker.DoWork_Focus(dg, 10);
                }
                else
                {
                    entityList = null;
                    this.dg.ItemsSource = null;
                    ExBackgroundWorker.DoWork_Focus(this.datCollectPlanYmd_F, 10);
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

                        #region 必須チェック

                        //if (this.datIssueYmd.SelectedDate == null)
                        //{
                        //    errMessage += "出力発行日が入力されていません。" + Environment.NewLine;
                        //    if (errCtl == null) errCtl = this.datIssueYmd;
                        //}

                        #endregion

                        #region 選択チェック

                        //if (this.lst == null)
                        //{
                        //    errMessage += "表示データがありません。" + Environment.NewLine;
                        //    if (errCtl == null) errCtl = this.datIssueYmd;
                        //}
                        //if (this.lst.Count == 0)
                        //{
                        //    errMessage += "表示データがありません。" + Environment.NewLine;
                        //    if (errCtl == null) errCtl = this.datIssueYmd;
                        //}

                        //bool _exec_flg = false;
                        //for (int i = 0; i <= lst.Count - 1; i++)
                        //{
                        //    if (lst[i].exec_flg == true)
                        //    {
                        //        _exec_flg = true;
                        //    }
                        //}
                        //if (_exec_flg == false)
                        //{
                        //    errMessage += "レポート対象データを選択して下さい。" + Environment.NewLine;
                        //    if (errCtl == null) errCtl = this.dgSelect;
                        //}

                        #endregion

                        #endregion

                        break;
                    case eProcKbn.Search:
                    case eProcKbn.ReportDetail:

                        #region 必須チェック

                        //// 発行日
                        //if (Common.gWinGroupType == Common.geWinGroupType.InpListReport)
                        //{
                        //    if (this.datIssueYmd.SelectedDate == null)
                        //    {
                        //        errMessage += "出力発行日が入力されていません。" + Environment.NewLine;
                        //        if (errCtl == null) errCtl = this.datIssueYmd;
                        //    }
                        //}

                        #endregion

                        #region 入力チェック

                        if (Common.gWinType == Common.geWinType.ListCollectPlan)
                        {
                            // 請求先
                            if (this.utlInvoice_F.txtID.Text.Trim() != "" && string.IsNullOrEmpty(this.utlInvoice_F.txtNm.Text.Trim()))
                            {
                                errMessage += "請求先が適切に入力(選択)されていません。" + Environment.NewLine;
                                if (errCtl == null) errCtl = this.utlInvoice_F.txtID;
                            }
                            if (this.utlInvoice_T.txtID.Text.Trim() != "" && string.IsNullOrEmpty(this.utlInvoice_T.txtNm.Text.Trim()))
                            {
                                errMessage += "請求先が適切に入力(選択)されていません。" + Environment.NewLine;
                                if (errCtl == null) errCtl = this.utlInvoice_T.txtID;
                            }
                        }
                        else if (Common.gWinType == Common.geWinType.ListPaymentPlan)
                        {
                            // 仕入先
                            if (this.utlPurchase_F.txtID.Text.Trim() != "" && string.IsNullOrEmpty(this.utlPurchase_F.txtNm.Text.Trim()))
                            {
                                errMessage += "仕入先が適切に入力(選択)されていません。" + Environment.NewLine;
                                if (errCtl == null) errCtl = this.utlPurchase_F.txtID;
                            }
                            if (this.utlPurchase_T.txtID.Text.Trim() != "" && string.IsNullOrEmpty(this.utlPurchase_T.txtNm.Text.Trim()))
                            {
                                errMessage += "仕入先が適切に入力(選択)されていません。" + Environment.NewLine;
                                if (errCtl == null) errCtl = this.utlPurchase_T.txtID;
                            }
                        }

                        #endregion

                        #region 範囲チェック

                        // 回収予定日
                        if (!string.IsNullOrEmpty(this.datCollectPlanYmd_F.Text.Trim()) && !string.IsNullOrEmpty(this.datCollectPlanYmd_T.Text.Trim()))
                        {
                            if (ExCast.zCLng(this.datCollectPlanYmd_F.Text.Replace("/", "")) > ExCast.zCLng(this.datCollectPlanYmd_T.Text.Replace("/", "")))
                            {
                                if (Common.gWinType == Common.geWinType.ListCollectPlan)
                                {
                                    errMessage += "回収予定日の範囲指定が不正です。" + Environment.NewLine;
                                }
                                else if (Common.gWinType == Common.geWinType.ListPaymentPlan)
                                {
                                    errMessage += "支払予定日の範囲指定が不正です。" + Environment.NewLine;
                                }
                                if (errCtl == null) errCtl = this.datCollectPlanYmd_F;
                            }
                        }

                        if (Common.gWinType == Common.geWinType.ListCollectPlan)
                        {
                            // 請求先
                            if (!string.IsNullOrEmpty(this.utlInvoice_F.txtID.Text.Trim()) && !string.IsNullOrEmpty(this.utlInvoice_T.txtID.Text.Trim()))
                            {
                                if (ExCast.zCLng(this.utlInvoice_F.txtID.Text.Trim()) > ExCast.zCLng(this.utlInvoice_T.txtID.Text.Trim()))
                                {
                                    errMessage += "請求先の範囲指定が不正です。" + Environment.NewLine;
                                    if (errCtl == null) errCtl = this.utlInvoice_T.txtID;
                                }
                            }
                        }
                        else if (Common.gWinType == Common.geWinType.ListPaymentPlan)
                        {
                            // 仕入先
                            if (!string.IsNullOrEmpty(this.utlPurchase_F.txtID.Text.Trim()) && !string.IsNullOrEmpty(this.utlPurchase_T.txtID.Text.Trim()))
                            {
                                if (ExCast.zCLng(this.utlPurchase_F.txtID.Text.Trim()) > ExCast.zCLng(this.utlPurchase_T.txtID.Text.Trim()))
                                {
                                    errMessage += "仕入先の範囲指定が不正です。" + Environment.NewLine;
                                    if (errCtl == null) errCtl = this.utlPurchase_T.txtID;
                                }
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

                    if (Common.gWinType == Common.geWinType.ListCollectPlan)
                    {
                        this.utlReport.pgId = DataPgEvidence.PGName.Plan.CollectPlanPrint;
                    }
                    else if (Common.gWinType == Common.geWinType.ListPaymentPlan)
                    {
                        this.utlReport.pgId = DataPgEvidence.PGName.Plan.PaymentPlanPrint;
                    }

                    this.utlReport.sqlWhere = GetSQLWhere();
                    this.utlReport.sqlOrderBy = GetSQLOrderBy();
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

            private string _sales_ymd = "";
            public string sales_ymd { set { this._sales_ymd = value; } get { return this._sales_ymd; } }

            private long _estimateno = 0;
            public long estimateno { set { this._estimateno = value; } get { return this._estimateno; } }

            private string _str_estimate_no = "";
            public string str_estimate_no { set { this._str_estimate_no = value; } get { return this._str_estimate_no; } }

            private long _order_no = 0;
            public long order_no { set { this._order_no = value; } get { return this._order_no; } }

            private string _str_order_no = "";
            public string str_order_no { set { this._str_order_no = value; } get { return this._str_order_no; } }

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
                                  , string sales_ymd
                                  , long estimateno
                                  , string str_estimate_no
                                  , long order_no
                                  , string str_order_no
                                  , string customer_nm
                                  , string supplier_nm
                                  , string supply_ymd
                                  , string business_division_nm
                                  , string deliver_division_nm)
            {
                this.no = no;
                this.str_no = str_no;
                this.sales_ymd = sales_ymd;
                this.estimateno = estimateno;
                this.str_estimate_no = str_estimate_no;
                this.order_no = order_no;
                this.str_order_no = str_order_no;
                this.customer_nm = customer_nm;
                this.supplier_nm = supplier_nm;
                this.supply_ymd = supply_ymd;
                this.business_division_nm = business_division_nm;
                this.deliver_division_nm = deliver_division_nm;
            }
        }

    }

}
