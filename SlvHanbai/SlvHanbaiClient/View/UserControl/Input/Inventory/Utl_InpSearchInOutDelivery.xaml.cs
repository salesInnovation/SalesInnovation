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
using SlvHanbaiClient.svcInOutDelivery;
using SlvHanbaiClient.View.Dlg;
using SlvHanbaiClient.View.Dlg.Report;
using SlvHanbaiClient.View.UserControl.Custom;
using SlvHanbaiClient.View.UserControl.Report;

namespace SlvHanbaiClient.View.UserControl.Input.Inventory
{
    public partial class Utl_InpSearchInOutDelivery : ExUserControl
    {

        #region Filed Const

        private const String CLASS_NM = "Utl_InpSearchInOutDelivery";
        private ObservableCollection<EntityInOutDelivery> entityList;
        private Control activeControl;

        private Utl_FunctionKey utlFKey = null;
        private Utl_Report utlReport = new Utl_Report();

        private List<DisplayList> _lst = new List<DisplayList>();
        public List<DisplayList> lst { set { this._lst = value; } get { return this._lst; } }

        private EntitySearch entity = new EntitySearch();        

        private long _no;
        public long no { set { this._no = value; } get { return this._no; } }

        private bool _DialogResult;
        public bool DialogResult { set { this._DialogResult = value; } get { return this._DialogResult; } }

        private ExWebService.geWebServiceCallKbn _WebServiceCallKbn = ExWebService.geWebServiceCallKbn.GetInOutDeliveryList;
        public ExWebService.geWebServiceCallKbn WebServiceCallKbn { set { this._WebServiceCallKbn = value; } get { return this._WebServiceCallKbn; } }

        private Dlg_ReportSetting dlgReportSetting = new Dlg_ReportSetting();

        private bool searchBtnFlg = false;

        private enum eProcKbn { Search = 0, Report, ReportDetail };
        private eProcKbn ProcKbn;

        private string UpdPrintNo = "";

        #endregion

        #region Constructor

        public Utl_InpSearchInOutDelivery()
        {
            InitializeComponent();
            this.Tag = "Main";      // ファンクションキーを受けつけ用
        }

        #endregion

        #region Page Events

        private void ExUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.utlInOutDeliveryKbn.ID_TextChanged -= this.InOutDeliveryToKbn_TextChanged;
            this.utlInOutDeliveryKbn.ID_TextChanged += this.InOutDeliveryToKbn_TextChanged;
            this.utlInOutDeliveryToKbn.ID_TextChanged -= this.InOutDeliveryToKbn_TextChanged;
            this.utlInOutDeliveryToKbn.ID_TextChanged += this.InOutDeliveryToKbn_TextChanged;

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
                    DataInitWhere.InitDate(DataInitWhere.geDateKbn.Month, ref this.datInOutDeliveryYmd_F, ref this.datInOutDeliveryYmd_T);

                    // DataGrid設定
                    this.GridDetail.Visibility = System.Windows.Visibility.Visible;
                    this.GridDetailSelect.Visibility = System.Windows.Visibility.Collapsed;

                    this.lblCauseNo.Visibility = System.Windows.Visibility.Collapsed;
                    this.utlCauseNo_F.Visibility = System.Windows.Visibility.Collapsed;
                    this.utlCauseNo_T.Visibility = System.Windows.Visibility.Collapsed;
                    this.borInOutDeliveryProcKbn.Visibility = System.Windows.Visibility.Collapsed;

                    break;
                case Common.geWinGroupType.InpListReport:
                    // 出力対象日初期設定
                    DataInitWhere.InitDate(DataInitWhere.geDateKbn.Month, ref this.datInOutDeliveryYmd_F, ref this.datInOutDeliveryYmd_T);

                    // DataGrid設定
                    this.GridDetail.Visibility = System.Windows.Visibility.Collapsed;
                    this.GridDetailSelect.Visibility = System.Windows.Visibility.Visible;

                    this.lblCauseNo.Visibility = System.Windows.Visibility.Visible;
                    this.utlCauseNo_F.Visibility = System.Windows.Visibility.Visible;
                    this.utlCauseNo_T.Visibility = System.Windows.Visibility.Visible;
                    this.borInOutDeliveryProcKbn.Visibility = System.Windows.Visibility.Visible;

                    break;
                case Common.geWinGroupType.InpDetailReport:
                    // 出力対象日初期設定
                    DataInitWhere.InitDate(DataInitWhere.geDateKbn.Today, ref this.datInOutDeliveryYmd_F, ref this.datInOutDeliveryYmd_T);

                    // DataGrid設定
                    this.GridDetail.Visibility = System.Windows.Visibility.Visible;
                    this.GridDetailSelect.Visibility = System.Windows.Visibility.Collapsed;
                    this.dg.ItemsSource = null;
                    this.dg.ItemsSource = lst;

                    this.lblCauseNo.Visibility = System.Windows.Visibility.Visible;
                    this.utlCauseNo_F.Visibility = System.Windows.Visibility.Visible;
                    this.utlCauseNo_T.Visibility = System.Windows.Visibility.Visible;
                    this.borInOutDeliveryProcKbn.Visibility = System.Windows.Visibility.Visible;

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
            entity.PropertyChanged += this.utlCompanyGroup.MstID_Changed;
            entity.PropertyChanged += this.utlCustomer.MstID_Changed;
            entity.PropertyChanged += this.utlPurchase.MstID_Changed;
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

            Binding BindingGroup = new Binding("group_id");
            BindingGroup.Mode = BindingMode.TwoWay;
            BindingGroup.Source = entity;
            this.utlCompanyGroup.txtID.SetBinding(TextBox.TextProperty, BindingGroup);

            Binding BindingCustomer = new Binding("customer_id");
            BindingCustomer.Mode = BindingMode.TwoWay;
            BindingCustomer.Source = entity;
            this.utlCustomer.txtID.SetBinding(TextBox.TextProperty, BindingCustomer);

            Binding BindingPurchase = new Binding("purchase_id");
            BindingPurchase.Mode = BindingMode.TwoWay;
            BindingPurchase.Source = entity;
            this.utlPurchase.txtID.SetBinding(TextBox.TextProperty, BindingPurchase);

            Binding BindingCommodity = new Binding("commodity_id");
            BindingCommodity.Mode = BindingMode.TwoWay;
            BindingCommodity.Source = entity;
            this.utlCommodity.txtID.SetBinding(TextBox.TextProperty, BindingCommodity);

            #endregion

            this.utlPerson_F.txtID.OnFormatString();
            this.utlPerson_T.txtID.SetZeroToNullString();
            this.utlCompanyGroup.txtID.SetZeroToNullString();
            this.utlCustomer.txtID.SetZeroToNullString();
            this.utlPurchase.txtID.SetZeroToNullString();
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
                DataInitWhere.InitDate(DataInitWhere.geDateKbn.Month, ref this.datInOutDeliveryYmd_F, ref this.datInOutDeliveryYmd_T);
            }
            else
            {
                DataInitWhere.InitDate(DataInitWhere.geDateKbn.Today, ref this.datInOutDeliveryYmd_F, ref this.datInOutDeliveryYmd_T);
            }

            // 入出庫処理区分
            this.chkInOutDelivery.IsChecked = false;
            this.chkSales.IsChecked = false;
            this.chkPurchase.IsChecked = false;

            ExBackgroundWorker.DoWork_Focus(this.datInOutDeliveryYmd_F, 10);
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
                case "utlCauseNo_F":
                case "utlCauseNo_T":
                    Utl_InpNoText inp = (Utl_InpNoText)activeControl;
                    inp.ShowList();
                    break;
                case "datInOutDeliveryYmd_F":
                case "datInOutDeliveryYmd_T":
                    ExDatePicker dt = (ExDatePicker)activeControl;
                    dt.ShowCalender();
                    break;
                case "utlPerson_F":
                case "utlPerson_T":
                case "utlCustomer":
                case "utlPurchase":
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
                    return;
                case Common.geWinGroupType.InpDetailReport:
                    this.dlgReportSetting.pg_id = DataPgEvidence.PGName.InOutDeliver.InOutDeliverDPrint;
                    this.dlgReportSetting.IsReportRange = true;
                    this.dlgReportSetting.IsTotalKbn = false;
                    this.dlgReportSetting.IsGroupTotal = true;
                    break;
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
                case "utlNo_F":
                case "utlNo_T":
                case "utlCauseNo_F":
                case "utlCauseNo_T":
                case "dg":
                case "chkInOutDelivery":
                case "chkSales":
                case "chkPurchase":
                    ExVisualTreeHelper.SetFunctionKeyEnabled("F5", false, this);
                    activeControl = null;
                    break;
                case "datInOutDeliveryYmd_F":
                case "datInOutDeliveryYmd_T":
                case "utlCompanyGroup":
                case "utlCustomer":
                case "utlPurchase":
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

            if (Common.gWinGroupType == Common.geWinGroupType.InpList)
            {
                strWhrer += "   AND T.IN_OUT_DELIVERY_PROC_KBN = 1 " + Environment.NewLine;
            }

            // 入出庫日
            if (this.datInOutDeliveryYmd_F.Text.Trim() != "")
            {
                strWhrer += "   AND T.IN_OUT_DELIVERY_YMD >= " + ExEscape.zRepStr(this.datInOutDeliveryYmd_F.Text.Trim()) + Environment.NewLine;
                strWhrerString1 += "[入出庫日 " + ExEscape.zRepStrNoQuota(this.datInOutDeliveryYmd_F.Text.Trim()) + "～";
            }
            else
            {
                strWhrerString1 += "[入出庫日 未指定～";
            }
            if (this.datInOutDeliveryYmd_T.Text.Trim() != "")
            {
                strWhrer += "   AND T.IN_OUT_DELIVERY_YMD <= " + ExEscape.zRepStr(this.datInOutDeliveryYmd_T.Text.Trim()) + Environment.NewLine;
                strWhrerString1 += ExEscape.zRepStrNoQuota(this.datInOutDeliveryYmd_T.Text.Trim());
            }
            else
            {
                strWhrerString1 += "未指定";
            }

            // 入出庫番号
            if (this.utlNo_F.txtID.Text.Trim() != "")
            {
                strWhrer += "   AND T.NO >= " + ExCast.zCLng(this.utlNo_F.txtID.Text.Trim()) + Environment.NewLine;
                strWhrerString1 += "] [入出庫番号 " + this.utlNo_F.txtID.Text.Trim() + "～";
            }
            else
            {
                strWhrerString1 += "] [入出庫番号 未指定～";
            }
            if (this.utlNo_T.txtID.Text.Trim() != "")
            {
                strWhrer += "   AND T.NO <= " + ExCast.zCLng(this.utlNo_T.txtID.Text.Trim()) + Environment.NewLine;
                strWhrerString1 += this.utlNo_T.txtID.Text.Trim();
            }
            else
            {
                strWhrerString1 += "未指定";
            }

            if (Common.gWinGroupType != Common.geWinGroupType.InpList)
            {
                // 元伝票番号
                if (this.utlCauseNo_F.txtID.Text.Trim() != "")
                {
                    strWhrer += "   AND T.CAUSE_NO >= " + ExCast.zCLng(this.utlCauseNo_F.txtID.Text.Trim()) + Environment.NewLine;
                    strWhrerString1 += "] [元伝票番号 " + this.utlCauseNo_F.txtID.Text.Trim() + "～";
                }
                else
                {
                    strWhrerString1 += "] [元伝票番号 未指定～";
                }
                if (this.utlCauseNo_T.txtID.Text.Trim() != "")
                {
                    strWhrer += "   AND T.CAUSE_NO <= " + ExCast.zCLng(this.utlCauseNo_T.txtID.Text.Trim()) + Environment.NewLine;
                    strWhrerString1 += this.utlCauseNo_T.txtID.Text.Trim() + "]";
                }
                else
                {
                    strWhrerString1 += "未指定]";
                }
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

            // 入出庫区分
            if (this.utlInOutDeliveryKbn.txtID.Text.Trim() != "")
            {
                strWhrer += "   AND T.IN_OUT_DELIVERY_KBN = " + ExCast.zCLng(this.utlInOutDeliveryKbn.txtID.Text.Trim()) + Environment.NewLine;
                strWhrerString2 += "] [入出庫区分 " + ExCast.zCLng(this.utlInOutDeliveryKbn.txtID.Text.Trim()) + "：" + ExEscape.zRepStrNoQuota(this.utlInOutDeliveryKbn.txtNm.Text.Trim());
            }
            else
            {
                strWhrerString2 += "] [入出庫区分 未指定";
            }

            // 入出庫先区分
            if (this.utlInOutDeliveryToKbn.txtID.Text.Trim() != "")
            {
                strWhrerString2 += "] [入出庫先区分 " + ExCast.zCLng(this.utlInOutDeliveryToKbn.txtID.Text.Trim()) + "：" + ExEscape.zRepStrNoQuota(this.utlInOutDeliveryToKbn.txtNm.Text.Trim());
            }

            // 会社グループ
            if (this.utlCompanyGroup.txtID.Text.Trim() != "")
            {
                strWhrer += "   AND T.GROUP_ID_TO = " + ExCast.zCLng(this.utlCompanyGroup.txtID.Text.Trim()) + Environment.NewLine;
                strWhrerString2 += "] [会社グループ " + ExCast.zCLng(this.utlCompanyGroup.txtID.Text.Trim()) + "：" + ExEscape.zRepStrNoQuota(this.utlCompanyGroup.txtNm.Text.Trim());
            }
            else
            {
                strWhrerString2 += "] [会社グループ 未指定";
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

            // 仕入先
            if (this.utlPurchase.txtID.Text.Trim() != "")
            {
                strWhrer += "   AND T.PURCHASE_ID = " + ExEscape.zRepStr(ExCast.zNumZeroNothingFormat(this.utlPurchase.txtID.Text.Trim())) + Environment.NewLine;
                strWhrerString2 += "] [仕入先 " + ExEscape.zRepStrNoQuota(this.utlPurchase.txtID.Text.Trim()) + "：" + ExEscape.zRepStrNoQuota(this.utlPurchase.txtNm.Text.Trim());
            }
            else
            {
                strWhrerString2 += "] [仕入先 未指定";
            }

            // 商品コード
            if (this.utlCommodity.txtID.Text.Trim() != "")
            {
                strWhrer += "   AND EXISTS (SELECT 1 " + Environment.NewLine;
                strWhrer += "                 FROM T_IN_OUT_DELIVERY_D AS OOD " + Environment.NewLine;
                strWhrer += "                WHERE OOD.COMPANY_ID = T.COMPANY_ID " + Environment.NewLine;
                strWhrer += "                  AND OOD.GROUP_ID = T.GROUP_ID " + Environment.NewLine;
                strWhrer += "                  AND OOD.IN_OUT_DELIVERY_ID = T.ID " + Environment.NewLine;
                strWhrer += "                  AND OOD.COMMODITY_ID = " + ExEscape.zRepStr(ExCast.zNumZeroNothingFormat(this.utlCommodity.txtID.Text.Trim())) + Environment.NewLine;
                strWhrer += "                  ) " + Environment.NewLine;
                strWhrerString2 += "] [商品コード " + ExEscape.zRepStrNoQuota(this.utlCommodity.txtID.Text.Trim()) + "：" + ExEscape.zRepStrNoQuota(this.utlCommodity.txtNm.Text.Trim()) + "]";
            }
            else
            {
                strWhrerString2 += "] [商品コード 未指定" + "]";
            }

            string _buf = "";

            if (Common.gWinGroupType != Common.geWinGroupType.InpList)
            {
                // 入出庫処理区分
                if (this.chkInOutDelivery.IsChecked == true)
                {
                    // 入出庫
                    strWhrer += "   AND (T.IN_OUT_DELIVERY_PROC_KBN = 1";
                    _buf += " [入出庫処理区分 入出庫";
                }
                if (this.chkSales.IsChecked == true)
                {
                    // 売上
                    if (_buf == "")
                    {
                        strWhrer += "   AND (T.IN_OUT_DELIVERY_PROC_KBN = 2";
                        _buf += " [入出庫処理区分 売上";
                    }
                    else
                    {
                        strWhrer += " OR T.IN_OUT_DELIVERY_PROC_KBN = 2";
                        _buf += " 売上";
                    }

                }
                if (this.chkPurchase.IsChecked == true)
                {
                    // 仕入
                    if (_buf == "")
                    {
                        strWhrer += "   AND (T.IN_OUT_DELIVERY_PROC_KBN = 3";
                        _buf += " [入出庫処理区分 仕入";
                    }
                    else
                    {
                        strWhrer += " OR T.IN_OUT_DELIVERY_PROC_KBN = 3";
                        _buf += " 仕入";
                    }
                }
                if (_buf == "")
                {
                    _buf += " [入出庫処理区分 指定無し]";
                }
                else
                {
                    strWhrer += ")" + Environment.NewLine;
                    _buf += "]";
                }
                strWhrerString2 += _buf;
            }
            
            _buf = "";

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

        #region 入出庫データ取得コールバック呼出(ExWebServiceクラスより呼出)

        // 入出庫リスト取得コールバック呼出
        public override void DataSelect(int intKbn, object objList)
        {
            if ((ExWebService.geWebServiceCallKbn)intKbn == this.WebServiceCallKbn)
            {
                if (objList != null)
                {
                    entityList = (ObservableCollection<EntityInOutDelivery>)objList;
                    var records =
                         (from n in entityList
                          orderby n.in_out_delivery_ymd descending, n.no descending, n.in_out_delivery_kbn, n.in_out_delivery_proc_kbn
                          select new { n.no, 
                                       n.cause_no, 
                                       n.in_out_delivery_ymd, 
                                       n.in_out_delivery_kbn_nm, 
                                       n.in_out_delivery_proc_kbn_nm, 
                                       n.in_out_delivery_to_kbn_nm,
                                       n.in_out_delivery_to_nm,
                                       n.group_id_to_nm, 
                                       n.customer_nm, 
                                       n.purchase_name, 
                                       n.sum_number }).Distinct();

                    this.lst.Clear();
                    foreach (var rec in records)
                    {
                        string _no = ExCast.zFormatForID(rec.no, Common.gintidFigureSlipNo);
                        string _cause_no = ExCast.zFormatForID(rec.cause_no, Common.gintidFigureSlipNo);
                        if (ExCast.zCLng(_cause_no) == 0) _cause_no = "";

                        DisplayList _displayList = new DisplayList();
                        _displayList.no = rec.no;
                        _displayList.str_no = _no;
                        _displayList.str_cause_no = _cause_no;
                        _displayList.in_out_delivery_ymd = rec.in_out_delivery_ymd;
                        _displayList.in_out_delivery_kbn_nm = rec.in_out_delivery_kbn_nm;
                        _displayList.in_out_delivery_proc_kbn_nm = rec.in_out_delivery_proc_kbn_nm;
                        _displayList.in_out_delivery_to_kbn_nm = rec.in_out_delivery_to_kbn_nm;
                        _displayList.in_out_delivery_to_nm = rec.in_out_delivery_to_nm;
                        _displayList.group_nm = rec.group_id_to_nm;
                        _displayList.customer_nm = rec.customer_nm;
                        _displayList.purchase_nm = rec.purchase_name;
                        _displayList.sum_number = rec.sum_number;

                        lst.Add(_displayList);
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
                    ExBackgroundWorker.DoWork_Focus(this.datInOutDeliveryYmd_F, 10);
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
                            errMessage += "入力担当者が適切に入力(選択)されていません。" + Environment.NewLine;
                            if (errCtl == null) errCtl = this.utlPerson_F.txtID;
                        }

                        // 入力担当者
                        if (this.utlPerson_T.txtID.Text.Trim() != "" && string.IsNullOrEmpty(this.utlPerson_T.txtNm.Text.Trim()))
                        {
                            errMessage += "入力担当者が適切に入力(選択)されていません。" + Environment.NewLine;
                            if (errCtl == null) errCtl = this.utlPerson_T.txtID;
                        }

                        // 入出庫区分
                        if (ExCast.zCInt(this.utlInOutDeliveryKbn.txtID.Text.Trim()) != 0 && string.IsNullOrEmpty(this.utlInOutDeliveryKbn.txtNm.Text.Trim()))
                        {
                            errMessage += "入出庫区分が適切に入力(選択)されていません。" + Environment.NewLine;
                            if (errCtl == null) errCtl = this.utlInOutDeliveryKbn.txtID;
                        }

                        // 入出庫先区分
                        if (ExCast.zCInt(this.utlInOutDeliveryToKbn.txtID.Text.Trim()) != 0 && string.IsNullOrEmpty(this.utlInOutDeliveryToKbn.txtNm.Text.Trim()))
                        {
                            errMessage += "入出庫先区分が適切に入力(選択)されていません。" + Environment.NewLine;
                            if (errCtl == null) errCtl = this.utlInOutDeliveryToKbn.txtID;
                        }

                        // グループ
                        if (this.utlCompanyGroup.txtID.Text.Trim() != "" && string.IsNullOrEmpty(this.utlCompanyGroup.txtNm.Text.Trim()))
                        {
                            errMessage += this.lblSupply.Content + "が適切に入力(選択)されていません。" + Environment.NewLine;
                            if (errCtl == null) errCtl = this.utlCompanyGroup.txtID;
                        }

                        // 得意先
                        if (this.utlCustomer.txtID.Text.Trim() != "" && string.IsNullOrEmpty(this.utlCustomer.txtNm.Text.Trim()))
                        {
                            errMessage += "得意先が適切に入力(選択)されていません。" + Environment.NewLine;
                            if (errCtl == null) errCtl = this.utlCustomer.txtID;
                        }

                        // 仕入先
                        if (this.utlPurchase.txtID.Text.Trim() != "" && string.IsNullOrEmpty(this.utlPurchase.txtNm.Text.Trim()))
                        {
                            errMessage += "仕入先が適切に入力(選択)されていません。" + Environment.NewLine;
                            if (errCtl == null) errCtl = this.utlPurchase.txtID;
                        }

                        // 商品先
                        if (this.utlCommodity.txtID.Text.Trim() != "" && string.IsNullOrEmpty(this.utlCommodity.txtNm.Text.Trim()))
                        {
                            errMessage += "商品先が適切に入力(選択)されていません。" + Environment.NewLine;
                            if (errCtl == null) errCtl = this.utlCommodity.txtID;
                        }

                        #endregion

                        #region 範囲チェック

                        // 入出庫番号
                        if (!string.IsNullOrEmpty(this.utlNo_F.txtID.Text.Trim()) && !string.IsNullOrEmpty(this.utlNo_T.txtID.Text.Trim()))
                        {
                            if (ExCast.zCLng(this.utlNo_F.txtID.Text.Trim()) > ExCast.zCLng(this.utlNo_T.txtID.Text.Trim()))
                            {
                                errMessage += "入出庫番号の範囲指定が不正です。" + Environment.NewLine;
                                if (errCtl == null) errCtl = this.utlNo_F.txtID;
                            }
                        }
                        // 入出庫日
                        if (!string.IsNullOrEmpty(this.datInOutDeliveryYmd_F.Text.Trim()) && !string.IsNullOrEmpty(this.datInOutDeliveryYmd_T.Text.Trim()))
                        {
                            if (ExCast.zCLng(this.datInOutDeliveryYmd_F.Text.Replace("/", "")) > ExCast.zCLng(this.datInOutDeliveryYmd_T.Text.Replace("/", "")))
                            {
                                errMessage += "入出庫日の範囲指定が不正です。" + Environment.NewLine;
                                if (errCtl == null) errCtl = this.datInOutDeliveryYmd_F;
                            }
                        }
                        // 元伝票番号
                        if (!string.IsNullOrEmpty(this.utlCauseNo_F.txtID.Text.Trim()) && !string.IsNullOrEmpty(this.utlCauseNo_T.txtID.Text.Trim()))
                        {
                            if (ExCast.zCLng(this.utlCauseNo_F.txtID.Text.Trim()) > ExCast.zCLng(this.utlCauseNo_T.txtID.Text.Trim()))
                            {
                                errMessage += "元伝票番号の範囲指定が不正です。" + Environment.NewLine;
                                if (errCtl == null) errCtl = this.utlCauseNo_F.txtID;
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
                            this.utlReport.pgId = DataPgEvidence.PGName.InOutDeliver.InOutDeliverPrint;
                            break;
                        case Common.geWinGroupType.InpDetailReport:
                            this.utlReport.pgId = DataPgEvidence.PGName.InOutDeliver.InOutDeliverDPrint;
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
        
        public class DisplayList
        {
            private bool _exec_flg = false;
            public bool exec_flg { set { this._exec_flg = value; } get { return this._exec_flg; } }

            private long _no = 0;
            public long no { set { this._no = value; } get { return this._no; } }

            private string _str_no = "";
            public string str_no { set { this._str_no = value; } get { return this._str_no; } }

            private long _cause_no = 0;
            public long cause_no { set { this._cause_no = value; } get { return this._cause_no; } }

            private string _str_cause_no = "";
            public string str_cause_no { set { this._str_cause_no = value; } get { return this._str_cause_no; } }

            private string _in_out_delivery_ymd = "";
            public string in_out_delivery_ymd { set { this._in_out_delivery_ymd = value; } get { return this._in_out_delivery_ymd; } }

            private string _in_out_delivery_kbn_nm = "";
            public string in_out_delivery_kbn_nm { set { this._in_out_delivery_kbn_nm = value; } get { return this._in_out_delivery_kbn_nm; } }

            private string _in_out_delivery_proc_kbn_nm = "";
            public string in_out_delivery_proc_kbn_nm { set { this._in_out_delivery_proc_kbn_nm = value; } get { return this._in_out_delivery_proc_kbn_nm; } }

            private string _in_out_delivery_to_kbn_nm = "";
            public string in_out_delivery_to_kbn_nm { set { this._in_out_delivery_to_kbn_nm = value; } get { return this._in_out_delivery_to_kbn_nm; } }

            private string _in_out_delivery_to_nm = "";
            public string in_out_delivery_to_nm { set { this._in_out_delivery_to_nm = value; } get { return this._in_out_delivery_to_nm; } }

            private string _group_nm = "";
            public string group_nm { set { this._group_nm = value; } get { return this._group_nm; } }

            private string _customer_nm = "";
            public string customer_nm { set { this._customer_nm = value; } get { return this._customer_nm; } }

            private string _purchase_nm = "";
            public string purchase_nm { set { this._purchase_nm = value; } get { return this._purchase_nm; } }

            private double _sum_number = 0;
            public double sum_number { set { this._sum_number = value; } get { return this._sum_number; } }

            public DisplayList() {}
        }

    }

}
