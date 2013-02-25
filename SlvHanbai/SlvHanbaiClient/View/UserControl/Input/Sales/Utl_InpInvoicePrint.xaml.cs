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
using SlvHanbaiClient.svcInvoiceClose;
using SlvHanbaiClient.View.Dlg;
using SlvHanbaiClient.View.UserControl.Custom;
using SlvHanbaiClient.View.UserControl.Report;

namespace SlvHanbaiClient.View.UserControl.Input.Sales
{
    public partial class Utl_InpInvoicePrint : ExUserControl
    {

        #region Filed Const

        private const String CLASS_NM = "Utl_InpInvoicePrint"; 
        private ObservableCollection<EntityInvoiceClose> entityList;
        private Control activeControl;

        private Utl_FunctionKey utlFKey = null;
        private Utl_Report utlReport = new Utl_Report();

        private List<DisplayInvoiceList> _lst = new List<DisplayInvoiceList>();
        public List<DisplayInvoiceList> lst { set { this._lst = value; } get { return this._lst; } }

        private EntitySearch entity = new EntitySearch();

        private long _no;
        public long no { set { this._no = value; } get { return this._no; } }

        private bool _DialogResult;
        public bool DialogResult { set { this._DialogResult = value; } get { return this._DialogResult; } }

        private ExWebService.geWebServiceCallKbn _WebServiceCallKbn = ExWebService.geWebServiceCallKbn.GetInvoiceList;
        public ExWebService.geWebServiceCallKbn WebServiceCallKbn { set { this._WebServiceCallKbn = value; } get { return this._WebServiceCallKbn; } }

        private enum eProcKbn { Search = 0, Report, ReportDetail };
        private eProcKbn ProcKbn;

        private ObservableCollection<EntityInvoiceClose> lstUpd = new ObservableCollection<EntityInvoiceClose>();

        #endregion

        #region Constructor

        public Utl_InpInvoicePrint()
        {
            InitializeComponent();
            this.Tag = "Main";      // ファンクションキーを受けつけ用
        }

        #endregion

        #region Page Events

        private void ExUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            // 画面初期化
            ExVisualTreeHelper.initDisplay(this.LayoutRoot);

            this.lst.Clear();
            this.dgSelect.ItemsSource = null;
            this.dgSelect.ItemsSource = lst;

            // ファンクションキー初期設定
            utlFKey = ExVisualTreeHelper.GetUtlFunctionKey(this);
            this.utlFKey.Init();

            this.utlReport.gPageType = Common.gPageType;
            this.utlReport.gWinMsterType = Common.geWinMsterType.None;
            this.utlReport.parentUtl = this;
            this.utlReport.Init();

            this.datIssueYmd.SelectedDate = DateTime.Now;
            this.chkPrintNo.IsChecked = true;

            switch (Common.gWinGroupType)
            {
                case Common.geWinGroupType.InpList:
                    // 出力対象日初期設定
                    DataInitWhere.InitDate(DataInitWhere.geDateKbn.Month, ref this.datInvoiceCloseYmd_F, ref this.datInvoiceCloseYmd_T);

                    // 発行日表示設定
                    this.lblIssueYmd.Visibility = System.Windows.Visibility.Collapsed;
                    this.datIssueYmd.Visibility = System.Windows.Visibility.Collapsed;

                    // DataGrid設定
                    this.GridDetail.Visibility = System.Windows.Visibility.Visible;
                    this.GridDetailSelect.Visibility = System.Windows.Visibility.Collapsed;

                    // 入金消込区分表示設定
                    this.borReceipt.Visibility = System.Windows.Visibility.Visible;

                    // 発行区分表示設定
                    this.borPrint.Visibility = System.Windows.Visibility.Collapsed;

                    break;
                case Common.geWinGroupType.InpListReport:
                    // 出力対象日初期設定
                    DataInitWhere.InitDate(DataInitWhere.geDateKbn.Month, ref this.datInvoiceCloseYmd_F, ref this.datInvoiceCloseYmd_T);

                    // 発行日表示設定
                    this.lblIssueYmd.Visibility = System.Windows.Visibility.Visible;
                    this.datIssueYmd.Visibility = System.Windows.Visibility.Visible;
                    this.datIssueYmd.SelectedDate = DateTime.Now;

                    // DataGrid設定
                    this.GridDetail.Visibility = System.Windows.Visibility.Collapsed;
                    this.GridDetailSelect.Visibility = System.Windows.Visibility.Visible;

                    // 入金消込区分表示設定
                    this.borReceipt.Visibility = System.Windows.Visibility.Collapsed;

                    // 発行区分表示設定
                    this.borPrint.Visibility = System.Windows.Visibility.Visible;

                    this.chkPrintNo.IsChecked = true;
                    this.chkPrintYes.IsChecked = false;

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
            entity.PropertyChanged += this.utlInvoice.MstID_Changed;
            entity.PropertyChanged += this.utlSummingUp.MstID_Changed;

            #region Bind

            // バインド
            Binding BindingInvoiceId = new Binding("invoice_id");
            BindingInvoiceId.Mode = BindingMode.TwoWay;
            BindingInvoiceId.Source = entity;
            this.utlInvoice.txtID.SetBinding(TextBox.TextProperty, BindingInvoiceId);

            Binding BindingSummingUpGroupId = new Binding("summing_up_group_id");
            BindingSummingUpGroupId.Mode = BindingMode.TwoWay;
            BindingSummingUpGroupId.Source = entity;
            this.utlSummingUp.txtID.SetBinding(TextBox.TextProperty, BindingSummingUpGroupId);

            #endregion

            this.utlInvoice.txtID.OnFormatString();
            this.utlSummingUp.txtID.SetZeroToNullString();
        }

        #endregion

        #region Function Key Button Events

        // F1ボタン(出力) クリック
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

                int intIndex = this.dgPrint.SelectedIndex;
                if (intIndex < 0)
                {
                    ExMessageBox.Show("行が選択されていません。");
                    return;
                }

                this.no = ExCast.zCLng(this.lst[this.dgPrint.SelectedIndex].no);
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

                this.utlReport.rptKbn = DataReport.geReportKbn.OutPut;
                this.ProcKbn = eProcKbn.Report;

                // ボタン押下時非同期入力チェックON
                Common.gblnBtnDesynchronizeLock = true;

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
            this.dgSelect.ItemsSource = null;
            DataInitWhere.InitDate(DataInitWhere.geDateKbn.Month, ref this.datInvoiceCloseYmd_F, ref this.datInvoiceCloseYmd_T);
            this.datIssueYmd.SelectedDate = DateTime.Now;

            // 取引区分
            this.chkSime.IsChecked = false;
            this.chkKake.IsChecked = false;

            ExBackgroundWorker.DoWork_Focus(this.utlInvoiceNo_F.txtID, 10);
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
            //if (Common.gWinGroupType == Common.geWinGroupType.InpList) return;

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
                //case "utlInvoice":
                //case "utlEstimateNo_T":
                //    Utl_InpNoText inp = (Utl_InpNoText)activeControl;
                //    inp.ShowList();
                //    break;
                case "datInvoiceCloseYmd_F":
                case "datInvoiceCloseYmd_T":
                case "datIssueYmd":
                    ExDatePicker dt = (ExDatePicker)activeControl;
                    dt.ShowCalender();
                    break;
                case "utlInvoice":
                case "utlSummingUp":
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
                case "datInvoiceCloseYmd_F":
                case "datInvoiceCloseYmd_T":
                case "chkSime":
                case "chkKake":
                case "chkPrintNo":
                    ExVisualTreeHelper.SetFunctionKeyEnabled("F5", false, this);
                    activeControl = null;
                    break;
                case "utlInvoiceNo_F":
                case "utlInvoiceNo_T":
                case "utlInvoice":
                case "utlSummingUp":
                case "datIssueYmd":
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
            dgSelect.ItemsSource = null;
            dgSelect.ItemsSource = lst;
        }

        private void btnNoSelectAll_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i <= lst.Count - 1; i++)
            {
                lst[i].exec_flg = false;
            }
            dgSelect.ItemsSource = null;
            dgSelect.ItemsSource = lst;
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

            // 請求書番号
            if (this.utlInvoiceNo_F.txtID.Text.Trim() != "")
            {
                strWhrer += "   AND T.NO >= " + ExCast.zCLng(this.utlInvoiceNo_F.txtID.Text.Trim()) + Environment.NewLine;
                strWhrerString1 += "[請求書番号 " + ExCast.zCLng(this.utlInvoiceNo_F.txtID.Text.Trim()) + "～";
            }
            else
            {
                strWhrerString1 += "[請求書番号 未指定～";
            }
            if (this.utlInvoiceNo_T.txtID.Text.Trim() != "")
            {
                strWhrer += "   AND T.NO <= " + ExCast.zCLng(this.utlInvoiceNo_T.txtID.Text.Trim()) + Environment.NewLine;
                strWhrerString1 += ExCast.zCLng(this.utlInvoiceNo_T.txtID.Text.Trim());
            }
            else
            {
                strWhrerString1 += "未指定";
            }

            // 請求締日
            if (this.datInvoiceCloseYmd_F.Text.Trim() != "")
            {
                strWhrer += "   AND T.INVOICE_YYYYMMDD >= " + ExEscape.zRepStr(this.datInvoiceCloseYmd_F.Text.Trim()) + Environment.NewLine;
                strWhrerString1 += "] [請求締日 " + this.datInvoiceCloseYmd_F.Text.Trim() + "～";
            }
            else
            {
                strWhrerString1 += "] [請求締日 未指定～";
            }
            if (this.datInvoiceCloseYmd_T.Text.Trim() != "")
            {
                strWhrer += "   AND T.INVOICE_YYYYMMDD <= " + ExEscape.zRepStr(this.datInvoiceCloseYmd_T.Text.Trim()) + Environment.NewLine;
                strWhrerString1 += this.datInvoiceCloseYmd_T.Text.Trim();
            }
            else
            {
                strWhrerString1 += "未指定";
            }

            // 請求先
            if (this.utlInvoice.txtID.Text.Trim() != "")
            {
                strWhrer += "   AND T.INVOICE_ID = " + ExEscape.zRepStr(ExCast.zNumZeroNothingFormat(this.utlInvoice.txtID.Text.Trim())) + Environment.NewLine;
                strWhrerString2 += "[請求先 " + ExEscape.zRepStrNoQuota(this.utlInvoice.txtID.Text.Trim()) + "：" + ExEscape.zRepStrNoQuota(this.utlInvoice.txtNm.Text.Trim());
            }
            else
            {
                strWhrerString2 += "[請求先 未指定";
            }

            // 締区分
            if (this.utlSummingUp.txtID.Text.Trim() != "")
            {
                strWhrer += "   AND T.SUMMING_UP_GROUP_ID = " + ExEscape.zRepStr(ExCast.zNumZeroNothingFormat(this.utlSummingUp.txtID.Text.Trim())) + Environment.NewLine;
                strWhrerString2 += "] [締区分 " + ExEscape.zRepStrNoQuota(this.utlSummingUp.txtID.Text.Trim()) + "：" + ExEscape.zRepStrNoQuota(this.utlSummingUp.txtNm.Text.Trim());
            }
            else
            {
                strWhrerString2 += "] [締区分 未指定";
            }

            // 請求区分
            string _buf = "";
            if (this.chkSime.IsChecked == true)
            {
                // 締請求
                strWhrer += "   AND (T.INVOICE_KBN = 0";
                _buf += " [請求区分 締請求";
            }
            if (this.chkKake.IsChecked == true)
            {
                // 都度請求
                if (_buf == "")
                {
                    strWhrer += "   AND (T.INVOICE_KBN = 1";
                    _buf += " [請求区分 都度請求";
                }
                else
                {
                    strWhrer += " OR T.INVOICE_KBN = 1";
                    _buf += " 都度請求";
                }
            }
            if (_buf == "")
            {
                _buf += " [請求区分 指定無し]";
            }
            else
            {
                strWhrer += ")" + Environment.NewLine;
                _buf += "]";
            }
            strWhrerString2 += _buf;
            _buf = "";

            // 請求書発行
            if (borPrint.Visibility == System.Windows.Visibility.Visible)
            {
                if (this.chkPrintNo.IsChecked == true)
                {
                    // 発行済を除く
                    strWhrer += "   AND (T.INVOICE_PRINT_FLG = 0";
                    _buf += " [請求書発行 発行未";
                }
                if (this.chkPrintYes.IsChecked == true)
                {
                    // 都度請求
                    if (_buf == "")
                    {
                        strWhrer += "   AND (T.INVOICE_PRINT_FLG = 1";
                        _buf += " [請求書発行 発行済";
                    }
                    else
                    {
                        strWhrer += " OR T.INVOICE_PRINT_FLG = 1";
                        _buf += " 発行済";
                    }
                }
                if (_buf == "")
                {
                    _buf += " [請求書発行 指定無し]";
                }
                else
                {
                    strWhrer += ")" + Environment.NewLine;
                    _buf += "]";
                }
                strWhrerString2 += _buf;
            }

            _buf = "";

            // 入金消込
            if (borReceipt.Visibility == System.Windows.Visibility.Visible)
            {
                if (this.chkReceiptNo.IsChecked == true)
                {
                    strWhrer += "   AND (IFNULL(RP_SUM.SUM_PRICE, 0) = 0";
                    _buf += " [入金消込 消込未";
                }
                if (this.chkReceiptPlace.IsChecked == true)
                {
                    if (_buf == "")
                    {
                        strWhrer += "   AND (IFNULL(RP_SUM.SUM_PRICE, 0) < IFNULL(T.INVOICE_PRICE, 0)";
                        _buf += " [入金消込 一部消込";
                    }
                    else
                    {
                        strWhrer += " OR IFNULL(RP_SUM.SUM_PRICE, 0) < IFNULL(T.INVOICE_PRICE, 0)";
                        _buf += " 一部消込";
                    }
                }
                if (this.chkReceiptYes.IsChecked == true)
                {
                    if (_buf == "")
                    {
                        strWhrer += "   AND (IFNULL(RP_SUM.SUM_PRICE, 0) = IFNULL(T.INVOICE_PRICE, 0)";
                        _buf += " [入金消込 消込済";
                    }
                    else
                    {
                        strWhrer += " OR IFNULL(RP_SUM.SUM_PRICE, 0) = IFNULL(T.INVOICE_PRICE, 0)";
                        _buf += " 消込済";
                    }
                }
                if (this.chkReceiptOver.IsChecked == true)
                {
                    if (_buf == "")
                    {
                        strWhrer += "   AND (IFNULL(RP_SUM.SUM_PRICE, 0) > IFNULL(T.INVOICE_PRICE, 0)";
                        _buf += " [入金消込 超過消込";
                    }
                    else
                    {
                        strWhrer += " OR IFNULL(RP_SUM.SUM_PRICE, 0) > IFNULL(T.INVOICE_PRICE, 0)";
                        _buf += " 超過消込";
                    }
                }
                if (_buf == "")
                {
                    _buf += " [入金消込 指定無し]";
                }
                else
                {
                    strWhrer += ")" + Environment.NewLine;
                    _buf += "]";
                }
                strWhrerString2 += _buf;
            }

            _buf = "";

            lstUpd.Clear();
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
                        EntityInvoiceClose entityUpd = new EntityInvoiceClose();
                        entityUpd._no = lst[i].no;
                        lstUpd.Add(entityUpd);

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
                    strWhrer += "   AND T.NO IN (" + _no + ")" + Environment.NewLine;
                }

                if (datIssueYmd.SelectedDate != null)
                {
                    strWhrer += "<issue ymd>" + ((DateTime)datIssueYmd.SelectedDate).ToString("yyyy/MM/dd");
                }

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
                    entityList = (ObservableCollection<EntityInvoiceClose>)objList;
                    var records =
                         (from n in entityList
                          orderby n._invoice_yyyymmdd descending, n._no descending
                          select new { n._receipt_receivable_kbn_nm,
                                       n._invoice_print_flg_nm,
                                       n._invoice_kbn_nm,
                                       n._no,
                                       n._invoice_yyyymmdd, 
                                       n._invoice_id, 
                                       n._invoice_nm, 
                                       n._before_invoice_price, 
                                       n._receipt_price, 
                                       n._transfer_price,
                                       n._no_tax_sales_price, 
                                       n._tax, 
                                       n._invoice_price,
                                       n._this_receipt_price,
                                       n._invoice_zan_price}).Distinct();

                    this.lst.Clear();
                    foreach (var rec in records)
                    {
                        string _no = ExCast.zFormatForID(rec._no, Common.gintidFigureSlipNo);
                        string _invoice_id = ExCast.zFormatForID(rec._invoice_id, Common.gintidFigureCustomer);
                        if (ExCast.zCLng(_no) == 0) _no = "";

                        DisplayInvoiceList _entity = new DisplayInvoiceList();

                        _entity.exec_flg = false;
                        _entity.receipt_receivable_kbn_nm = rec._receipt_receivable_kbn_nm;
                        _entity.invoice_print_flg_nm = rec._invoice_print_flg_nm;
                        _entity.invoice_kbn_nm = rec._invoice_kbn_nm;
                        _entity.no = rec._no;
                        _entity.invoice_yyyymmdd = rec._invoice_yyyymmdd;
                        _entity.invoice_id = rec._invoice_id;
                        _entity.invoice_nm = rec._invoice_nm;
                        _entity.before_invoice_price = rec._before_invoice_price;
                        _entity.receipt_price = rec._receipt_price;
                        _entity.transfer_price = rec._transfer_price;
                        _entity.no_tax_sales_price = rec._no_tax_sales_price;
                        _entity.tax = rec._tax;
                        _entity.invoice_price = rec._invoice_price;
                        _entity.this_receipt_price = rec._this_receipt_price;
                        _entity.invoice_zan_price = rec._invoice_zan_price;

                        lst.Add(_entity);
                    }

                    this.dgPrint.Focus();
                    this.dgPrint.ItemsSource = null;
                    this.dgPrint.ItemsSource = lst;
                    this.dgSelect.ItemsSource = null;
                    this.dgSelect.ItemsSource = lst;
                    if (lst.Count > 0)
                    {
                        this.dgPrint.SelectedIndex = 0;
                    }
                    ExBackgroundWorker.DoWork_Focus(this.dgSelect, 10);
                }
                else
                {
                    entityList = null;
                    this.lst.Clear();
                    this.dgPrint.ItemsSource = null;
                    this.dgSelect.ItemsSource = null;
                    ExBackgroundWorker.DoWork_Focus(this.utlInvoiceNo_F.txtID, 10);
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
                    case eProcKbn.Search:
                    case eProcKbn.ReportDetail:

                        #region 検索時チェック

                        #region 入力チェック

                        // 請求先
                        if (this.utlInvoice.txtID.Text.Trim() != "" && string.IsNullOrEmpty(this.utlInvoice.txtNm.Text.Trim()))
                        {
                            errMessage += "請求先が適切に入力(選択)されていません。" + Environment.NewLine;
                            if (errCtl == null) errCtl = this.utlInvoice.txtID;
                        }

                        // 締区分
                        if (this.utlSummingUp.txtID.Text.Trim() != "" && string.IsNullOrEmpty(this.utlSummingUp.txtNm.Text.Trim()))
                        {
                            errMessage += "締区分が適切に入力(選択)されていません。" + Environment.NewLine;
                            if (errCtl == null) errCtl = this.utlSummingUp.txtID;
                        }

                        #endregion

                        #region 範囲チェック

                        // 請求番号
                        if (!string.IsNullOrEmpty(this.utlInvoiceNo_F.txtID.Text.Trim()) && !string.IsNullOrEmpty(this.utlInvoiceNo_T.txtID.Text.Trim()))
                        {
                            if (ExCast.zCLng(this.utlInvoiceNo_F.txtID.Text.Trim()) > ExCast.zCLng(this.utlInvoiceNo_T.txtID.Text.Trim()))
                            {
                                errMessage += "請求番号の範囲指定が不正です。" + Environment.NewLine;
                                if (errCtl == null) errCtl = this.utlInvoiceNo_F.txtID;
                            }
                        }
                        // 請求締日
                        if (!string.IsNullOrEmpty(this.datInvoiceCloseYmd_F.Text.Trim()) && !string.IsNullOrEmpty(this.datInvoiceCloseYmd_T.Text.Trim()))
                        {
                            if (ExCast.zCLng(this.datInvoiceCloseYmd_F.Text.Replace("/", "")) > ExCast.zCLng(this.datInvoiceCloseYmd_T.Text.Replace("/", "")))
                            {
                                errMessage += "請求締日の範囲指定が不正です。" + Environment.NewLine;
                                if (errCtl == null) errCtl = this.utlInvoiceNo_F.txtID;
                            }
                        }

                        #endregion

                        #endregion

                        break;
                    case eProcKbn.Report:

                        #region レポート出力時チェック

                        #region 必須チェック

                        if (this.datIssueYmd.SelectedDate == null)
                        {
                            errMessage += "出力発行日が入力されていません。" + Environment.NewLine;
                            if (errCtl == null) errCtl = this.datIssueYmd;
                        }

                        #endregion

                        #region 選択チェック

                        if (this.lst == null)
                        {
                            errMessage += "表示データがありません。" + Environment.NewLine;
                            if (errCtl == null) errCtl = this.datIssueYmd;
                        }
                        if (this.lst.Count == 0)
                        {
                            errMessage += "表示データがありません。" + Environment.NewLine;
                            if (errCtl == null) errCtl = this.datIssueYmd;
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
                            if (errCtl == null) errCtl = this.datIssueYmd;
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
                            this.utlReport.pgId = DataPgEvidence.PGName.Invoice.InvoicePrint;
                            break;
                        case Common.geWinGroupType.InpDetailReport:
                            //this.utlReport.pgId = DataPgEvidence.PGName.Sales.SalesDPrint;
                            break;
                    }

                    this.utlReport.sqlWhere = GetSQLWhere();
                    this.utlReport.sqlOrderBy = GetSQLOrderBy();
                    this.utlReport.updPrintNo = this.lstUpd;
                    this.utlReport.ReportStart();
                }
            }
            finally
            {
                Common.gblnBtnProcLock = false;
                Common.gblnBtnDesynchronizeLock = false;
            }
        }

        #endregion

        public class DisplayInvoiceList
        {
            private bool _exec_flg = false;
            public bool exec_flg { set { this._exec_flg = value; } get { return this._exec_flg; } }

            private string _receipt_receivable_kbn_nm = "";
            public string receipt_receivable_kbn_nm { set { this._receipt_receivable_kbn_nm = value; } get { return this._receipt_receivable_kbn_nm; } }

            private string _invoice_print_flg_nm = "";
            public string invoice_print_flg_nm { set { this._invoice_print_flg_nm = value; } get { return this._invoice_print_flg_nm; } }

            private string _invoice_kbn_nm = "";
            public string invoice_kbn_nm { set { this._invoice_kbn_nm = value; } get { return this._invoice_kbn_nm; } }

            private string _no = "";
            public string no { set { this._no = value; } get { return this._no; } }

            private string _invoice_yyyymmdd = "";
            public string invoice_yyyymmdd { set { this._invoice_yyyymmdd = value; } get { return this._invoice_yyyymmdd; } }

            private string _invoice_id = "";
            public string invoice_id { set { this._invoice_id = value; } get { return this._invoice_id; } }

            private string _invoice_nm = "";
            public string invoice_nm { set { this._invoice_nm = value; } get { return this._invoice_nm; } }

            private double _before_invoice_price = 0;
            public double before_invoice_price { set { this._before_invoice_price = value; } get { return this._before_invoice_price; } }

            private double _receipt_price = 0;
            public double receipt_price { set { this._receipt_price = value; } get { return this._receipt_price; } }

            private double _transfer_price = 0;
            public double transfer_price { set { this._transfer_price = value; } get { return this._transfer_price; } }

            private double _no_tax_sales_price = 0;
            public double no_tax_sales_price { set { this._no_tax_sales_price = value; } get { return this._no_tax_sales_price; } }

            private double _tax = 0;
            public double tax { set { this._tax = value; } get { return this._tax; } }

            private double _invoice_price = 0;
            public double invoice_price { set { this._invoice_price = value; } get { return this._invoice_price; } }

            private double _this_receipt_price = 0;
            public double this_receipt_price { set { this._this_receipt_price = value; } get { return this._this_receipt_price; } }

            private double _invoice_zan_price = 0;
            public double invoice_zan_price { set { this._invoice_zan_price = value; } get { return this._invoice_zan_price; } }

            public DisplayInvoiceList()
            {
            }
        }

    }

}
