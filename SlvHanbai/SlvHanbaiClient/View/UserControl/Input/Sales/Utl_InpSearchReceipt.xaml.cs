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
using SlvHanbaiClient.Class.Entity;
using SlvHanbaiClient.Class.WebService;
using SlvHanbaiClient.svcReceipt;
using SlvHanbaiClient.View.Dlg;
using SlvHanbaiClient.View.Dlg.Report;
using SlvHanbaiClient.View.UserControl.Custom;
using SlvHanbaiClient.View.UserControl.Report;

namespace SlvHanbaiClient.View.UserControl.Input.Sales
{
    public partial class Utl_InpSearchReceipt : ExUserControl
    {

        #region Filed Const

        private const String CLASS_NM = "Utl_InpSearchReceipt";
        private ObservableCollection<EntityReceipt> entityList;
        private Control activeControl;

        private Utl_FunctionKey utlFKey = null;
        private Utl_Report utlReport = new Utl_Report();

        private List<DisplayReceiptList> _lst = new List<DisplayReceiptList>();
        public List<DisplayReceiptList> lst { set { this._lst = value; } get { return this._lst; } }

        private EntitySearch entity = new EntitySearch();

        private long _no;
        public long no { set { this._no = value; } get { return this._no; } }

        private bool _DialogResult;
        public bool DialogResult { set { this._DialogResult = value; } get { return this._DialogResult; } }

        private ExWebService.geWebServiceCallKbn _WebServiceCallKbn = ExWebService.geWebServiceCallKbn.GetReceiptList;
        public ExWebService.geWebServiceCallKbn WebServiceCallKbn { set { this._WebServiceCallKbn = value; } get { return this._WebServiceCallKbn; } }

        private bool searchBtnFlg = false;

        private Dlg_ReportSetting dlgReportSetting = new Dlg_ReportSetting();

        #endregion

        #region Constructor

        public Utl_InpSearchReceipt()
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

            if (Common.gWinType == Common.geWinType.ListReceiptMonth)
            {
                lblReceiptYmd.Content = "入金年月";
                System.Globalization.CultureInfo Culture = new System.Globalization.CultureInfo(Thread.CurrentThread.CurrentCulture.Name);
                // SelectedDateFormat=Shortのパターン
                Culture.DateTimeFormat.ShortDatePattern = "yyyy/MM";
                // SelectedDateFormat=Longのパターン
                Culture.DateTimeFormat.LongDatePattern = "ggy'年'M'月'";
                Thread.CurrentThread.CurrentCulture = Culture;
            }
        }

        private void ExUserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            if (Common.gWinType == Common.geWinType.ListReceiptMonth)
            {
                lblReceiptYmd.Content = "入金日";
                System.Globalization.CultureInfo Culture = new System.Globalization.CultureInfo(Thread.CurrentThread.CurrentCulture.Name);
                // SelectedDateFormat=Shortのパターン
                Culture.DateTimeFormat.ShortDatePattern = "yyyy/MM/dd";
                // SelectedDateFormat=Longのパターン
                Culture.DateTimeFormat.LongDatePattern = "ggy'年'M'月'd'日'";
                Thread.CurrentThread.CurrentCulture = Culture;
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

            if (Common.gWinType == Common.geWinType.ListReceiptMonth)
            {
                lblReceiptYmd.Content = "入金年月";
                System.Globalization.CultureInfo Culture = new System.Globalization.CultureInfo(Thread.CurrentThread.CurrentCulture.Name);
                // SelectedDateFormat=Shortのパターン
                Culture.DateTimeFormat.ShortDatePattern = "yyyy/MM";
                // SelectedDateFormat=Longのパターン
                Culture.DateTimeFormat.LongDatePattern = "ggy'年'M'月'";
                Thread.CurrentThread.CurrentCulture = Culture;
            }

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

            if (Common.gWinGroupType == Common.geWinGroupType.InpList)
            {
                DataInitWhere.InitDate(DataInitWhere.geDateKbn.Month, ref this.datReceiptYmd_F, ref this.datReceiptYmd_T);
            }
            else
            {
                DataInitWhere.InitDate(DataInitWhere.geDateKbn.Today, ref this.datReceiptYmd_F, ref this.datReceiptYmd_T);
            }

            if (Common.gWinGroupType != Common.geWinGroupType.InpList)
            {
                this.utlFKey.btnF6.Content = "     F6     " + Environment.NewLine;
                this.utlFKey.btnF6.IsEnabled = false;
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
            entity.PropertyChanged += this.utlInvoice.MstID_Changed;

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
            this.utlInvoice.txtID.SetBinding(TextBox.TextProperty, BindingCustomer);

            #endregion

            this.utlPerson_F.txtID.OnFormatString();
            this.utlPerson_T.txtID.SetZeroToNullString();
            this.utlInvoice.txtID.SetZeroToNullString();
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
                DataInitWhere.InitDate(DataInitWhere.geDateKbn.Month, ref this.datReceiptYmd_F, ref this.datReceiptYmd_T);
            }
            else
            {
                DataInitWhere.InitDate(DataInitWhere.geDateKbn.Today, ref this.datReceiptYmd_F, ref this.datReceiptYmd_T);
            }

            ExBackgroundWorker.DoWork_Focus(this.datReceiptYmd_F, 10);
        }

        // F3ボタン(ﾀﾞｳﾝﾛｰﾄﾞ) クリック
        public override void btnF3_Click(object sender, RoutedEventArgs e)
        {
            if (Common.gWinGroupType == Common.geWinGroupType.InpList)

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
            if (Common.gWinGroupType == Common.geWinGroupType.InpList)

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
                case "datReceiptYmd_F":
                case "datReceiptYmd_T":
                    ExDatePicker dt = (ExDatePicker)activeControl;
                    dt.ShowCalender();
                    break;
                case "utlPerson_F":
                case "utlPerson_T":
                case "utlInvoice":
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
            if (Common.gWinGroupType != Common.geWinGroupType.InpList) return;

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

            if (Common.gWinType == Common.geWinType.ListReceiptDay)
            {
                this.dlgReportSetting.pg_id = DataPgEvidence.PGName.Receipt.ReceiptDayPrint;
                this.dlgReportSetting.IsReportRange = true;
                this.dlgReportSetting.IsTotalKbn = true;
                this.dlgReportSetting.IsGroupTotal = true;
            }
            else if (Common.gWinType == Common.geWinType.ListReceiptMonth)
            {
                this.dlgReportSetting.pg_id = DataPgEvidence.PGName.Receipt.ReceiptMonthPrint;
                this.dlgReportSetting.IsReportRange = true;
                this.dlgReportSetting.IsTotalKbn = true;
                this.dlgReportSetting.IsGroupTotal = true;
            }
            else
            {
                switch (Common.gWinGroupType)
                {
                    case Common.geWinGroupType.InpDetailReport:
                        this.dlgReportSetting.pg_id = DataPgEvidence.PGName.Receipt.ReceiptDPrint;
                        this.dlgReportSetting.IsReportRange = true;
                        this.dlgReportSetting.IsTotalKbn = true;
                        this.dlgReportSetting.IsGroupTotal = true;
                        break;
                }
            }

            this.dlgReportSetting.IsInvoiceKbn = true;

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
                case "utlReceiptNo_F":
                case "utlReceiptNo_T":
                case "dg":
                    ExVisualTreeHelper.SetFunctionKeyEnabled("F5", false, this);
                    activeControl = null;
                    break;
                case "datReceiptYmd_F":
                case "datReceiptYmd_T":
                case "utlInvoice":
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

        #region Data Select

        #region データ取得

        // データ取得
        private void GetListData()
        {
            if (Common.gWinGroupType != Common.geWinGroupType.InpList) return;

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

            #region 入金番号

            if (this.utlReceiptNo_F.txtID.Text.Trim() != "")
            {
                strWhrer += "   AND T.NO >= " + ExCast.zCLng(this.utlReceiptNo_F.txtID.Text.Trim()) + Environment.NewLine;
                strWhrerString1 += "[入金番号 " + ExEscape.zRepStrNoQuota(this.utlReceiptNo_F.txtID.Text.Trim()) + "～";
            }
            else
            {
                strWhrerString1 += "[入金番号 未指定～";
            }
            if (this.utlReceiptNo_T.txtID.Text.Trim() != "")
            {
                strWhrer += "   AND T.NO <= " + ExCast.zCLng(this.utlReceiptNo_T.txtID.Text.Trim()) + Environment.NewLine;
                strWhrerString1 += ExEscape.zRepStrNoQuota(this.utlReceiptNo_T.txtID.Text.Trim());
            }
            else
            {
                strWhrerString1 += "未指定";
            }

            #endregion

            #region 入金日

            if (Common.gWinType == Common.geWinType.ListReceiptMonth)
            {
                if (this.datReceiptYmd_F.Text.Trim() != "")
                {
                    strWhrer += "   AND replace(date_format(T.RECEIPT_YMD , " + ExEscape.SQL_YM + "), '0000/00', '') >= " + ExEscape.zRepStr(this.datReceiptYmd_F.Text.Trim()) + Environment.NewLine;
                    strWhrerString1 += "] [入金年月 " + ExEscape.zRepStrNoQuota(this.datReceiptYmd_F.Text.Trim()) + "～";
                }
                else
                {
                    strWhrerString1 += "] [入金年月 未指定～";
                }
                if (this.datReceiptYmd_T.Text.Trim() != "")
                {
                    strWhrer += "   AND replace(date_format(T.RECEIPT_YMD , " + ExEscape.SQL_YM + "), '0000/00', '') <= " + ExEscape.zRepStr(this.datReceiptYmd_T.Text.Trim()) + Environment.NewLine;
                    strWhrerString1 += ExEscape.zRepStrNoQuota(this.datReceiptYmd_T.Text.Trim()) + "]";
                }
                else
                {
                    strWhrerString1 += "未指定]";
                }
            }
            else
            {
                if (this.datReceiptYmd_F.Text.Trim() != "")
                {
                    strWhrer += "   AND T.RECEIPT_YMD >= " + ExEscape.zRepStr(this.datReceiptYmd_F.Text.Trim()) + Environment.NewLine;
                    strWhrerString1 += "] [入金日 " + ExEscape.zRepStrNoQuota(this.datReceiptYmd_F.Text.Trim()) + "～";
                }
                else
                {
                    strWhrerString1 += "] [入金日 未指定～";
                }
                if (this.datReceiptYmd_T.Text.Trim() != "")
                {
                    strWhrer += "   AND T.RECEIPT_YMD <= " + ExEscape.zRepStr(this.datReceiptYmd_T.Text.Trim()) + Environment.NewLine;
                    strWhrerString1 += ExEscape.zRepStrNoQuota(this.datReceiptYmd_T.Text.Trim()) + "]";
                }
                else
                {
                    strWhrerString1 += "未指定]";
                }
            }

            #endregion

            #region 入力担当者

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

            #endregion

            #region 請求先

            if (this.utlInvoice.txtID.Text.Trim() != "")
            {
                strWhrer += "   AND T.INVOICE_ID = " + ExEscape.zRepStr(ExCast.zNumZeroNothingFormat(this.utlInvoice.txtID.Text.Trim())) + Environment.NewLine;
                strWhrerString2 += "] [請求先 " + ExEscape.zRepStrNoQuota(this.utlInvoice.txtID.Text.Trim()) + "：" + ExEscape.zRepStrNoQuota(this.utlInvoice.txtNm.Text.Trim()) + "]";
            }
            else
            {
                strWhrerString2 += "] [請求先 未指定]";
            }

            #endregion

            if (Common.gWinType == Common.geWinType.ListReceiptDay)
            {
                strWhrer += "<group kbn>1";
            }
            else if (Common.gWinType == Common.geWinType.ListReceiptMonth)
            {
                strWhrer += "<group kbn>2";
            }

            strWhrer = strWhrer.Replace(",", "<<@escape_comma@>>");

            return strWhrer + "WhereString =>" + strWhrerString1 + ";" + strWhrerString2;
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
                    entityList = (ObservableCollection<EntityReceipt>)objList;
                    var records =
                         (from n in entityList
                          orderby n.receipt_ymd descending, n.no descending
                          select new { n.no, n.receipt_ymd, n.invoice_id, n.invoice_nm, n.sum_price }).Distinct();

                    this.lst.Clear();
                    foreach (var rec in records)
                    {
                        string _no = ExCast.zFormatForID(rec.no, Common.gintidFigureSlipNo);

                        lst.Add(new DisplayReceiptList(rec.no,
                                                       _no,
                                                       rec.receipt_ymd,
                                                       rec.invoice_id,
                                                       rec.invoice_nm,
                                                       rec.sum_price.ToString("#,##0")));
                    }

                    this.dg.Focus();
                    this.dg.ItemsSource = null;
                    this.dg.ItemsSource = lst;
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
                    ExBackgroundWorker.DoWork_Focus(this.datReceiptYmd_F, 10);
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

                // 請求先
                if (this.utlInvoice.txtID.Text.Trim() != "" && string.IsNullOrEmpty(this.utlInvoice.txtNm.Text.Trim()))
                {
                    errMessage += "請求先が適切に入力(選択)されていません。" + Environment.NewLine;
                    if (errCtl == null) errCtl = this.utlInvoice.txtID;
                }

                #endregion

                #region 範囲チェック

                // 入金番号
                if (!string.IsNullOrEmpty(this.utlReceiptNo_F.txtID.Text.Trim()) && !string.IsNullOrEmpty(this.utlReceiptNo_T.txtID.Text.Trim()))
                {
                    if (ExCast.zCLng(this.utlReceiptNo_F.txtID.Text.Trim()) > ExCast.zCLng(this.utlReceiptNo_T.txtID.Text.Trim()))
                    {
                        errMessage += "入金番号の範囲指定が不正です。" + Environment.NewLine;
                        if (errCtl == null) errCtl = this.utlReceiptNo_F.txtID;
                    }
                }
                // 入金日
                if (!string.IsNullOrEmpty(this.datReceiptYmd_F.Text.Trim()) && !string.IsNullOrEmpty(this.datReceiptYmd_T.Text.Trim()))
                {
                    if (ExCast.zCLng(this.datReceiptYmd_F.Text.Replace("/", "")) > ExCast.zCLng(this.datReceiptYmd_T.Text.Replace("/", "")))
                    {
                        errMessage += "入金日の範囲指定が不正です。" + Environment.NewLine;
                        if (errCtl == null) errCtl = this.datReceiptYmd_F;
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

                    if (Common.gWinType == Common.geWinType.ListReceiptDay)
                    {
                        this.utlReport.pgId = DataPgEvidence.PGName.Receipt.ReceiptDayPrint;
                    }
                    else if (Common.gWinType == Common.geWinType.ListReceiptMonth)
                    {
                        this.utlReport.pgId = DataPgEvidence.PGName.Receipt.ReceiptMonthPrint;
                    }
                    else
                    {
                        switch (Common.gWinGroupType)
                        {
                            case Common.geWinGroupType.InpListReport:
                                return;
                            case Common.geWinGroupType.InpDetailReport:
                                this.utlReport.pgId = DataPgEvidence.PGName.Receipt.ReceiptDPrint;
                                break;
                        }
                    }

                    this.utlReport.sqlWhere = GetSQLWhere();
                    this.utlReport.sqlOrderBy = "";
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

        public class DisplayReceiptList
        {
            private long _no = 0;
            public long no { set { this._no = value; } get { return this._no; } }

            private string _str_no = "";
            public string str_no { set { this._str_no = value; } get { return this._str_no; } }

            private string _receipt_ymd = "";
            public string receipt_ymd { set { this._receipt_ymd = value; } get { return this._receipt_ymd; } }

            private string _invoice_id = "";
            public string invoice_id { set { this._invoice_id = value; } get { return this._invoice_id; } }

            private string _invoice_nm = "";
            public string invoice_nm { set { this._invoice_nm = value; } get { return this._invoice_nm; } }

            private string _price = "";
            public string price { set { this._price = value; } get { return this._price; } }

            public DisplayReceiptList(long no
                                    , string str_no
                                    , string receipt_ymd
                                    , string invoice_id
                                    , string invoice_nm
                                    , string price)
            {
                this.no = no;
                this.str_no = str_no;
                this.receipt_ymd = receipt_ymd;
                this.invoice_id = invoice_id;
                this.invoice_nm = invoice_nm;
                this.price = price;
            }
        }

    }

}
