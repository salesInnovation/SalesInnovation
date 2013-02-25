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
using SlvHanbaiClient.svcPaymentCreditBalance;
using SlvHanbaiClient.View.Dlg;
using SlvHanbaiClient.View.Dlg.Report;
using SlvHanbaiClient.View.UserControl.Custom;
using SlvHanbaiClient.View.UserControl.Report;

namespace SlvHanbaiClient.View.UserControl.Input.Purchase
{
    public partial class Utl_InpPaymentCreditBalance : ExUserControl
    {

        #region Filed Const

        private const String CLASS_NM = "Utl_InpPaymentCreditBalance"; 
        private ObservableCollection<EntityPaymentCreditBalance> entityList;
        private Control activeControl;

        private Utl_FunctionKey utlFKey = null;
        private Utl_Report utlReport = new Utl_Report();

        private EntitySearch entity = new EntitySearch();

        private long _no;
        public long no { set { this._no = value; } get { return this._no; } }

        private bool _DialogResult;
        public bool DialogResult { set { this._DialogResult = value; } get { return this._DialogResult; } }

        private ExWebService.geWebServiceCallKbn _WebServiceCallKbn = ExWebService.geWebServiceCallKbn.GetPaymentCreditBalanceList;
        public ExWebService.geWebServiceCallKbn WebServiceCallKbn { set { this._WebServiceCallKbn = value; } get { return this._WebServiceCallKbn; } }

        private enum eProcKbn { Search = 0, Report, ReportDetail, Update };
        private eProcKbn ProcKbn;

        private Dlg_ReportSetting dlgReportSetting = new Dlg_ReportSetting();

        #endregion

        #region Constructor

        public Utl_InpPaymentCreditBalance()
        {
            InitializeComponent();
            this.Tag = "Main";      // ファンクションキーを受けつけ用
        }

        #endregion

        #region Page Events

        private void ExUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            System.Globalization.CultureInfo Culture = new System.Globalization.CultureInfo(Thread.CurrentThread.CurrentCulture.Name);
            // SelectedDateFormat=Shortのパターン
            Culture.DateTimeFormat.ShortDatePattern = "yyyy/MM";
            // SelectedDateFormat=Longのパターン
            Culture.DateTimeFormat.LongDatePattern = "ggy'年'M'月'";
            Thread.CurrentThread.CurrentCulture = Culture;

            // 処理対象年月初期設定
            DataInitWhere.InitDateYm(ref this.datYm);

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

            // 処理対象年月初期設定
            DataInitWhere.InitDateYm(ref this.datYm);

            this.dgPrint.ItemsSource = null;
            this.dgUpdate.ItemsSource = null;

            // ファンクションキー初期設定
            utlFKey = ExVisualTreeHelper.GetUtlFunctionKey(this);
            this.utlFKey.Init();

            this.utlReport.gPageType = Common.gPageType;
            this.utlReport.gWinMsterType = Common.geWinMsterType.None;
            this.utlReport.parentUtl = this;
            this.utlReport.Init();

            switch (Common.gWinGroupType)
            {
                case Common.geWinGroupType.InpListUpd:
                    // DataGrid設定
                    this.GridDetail.Visibility = System.Windows.Visibility.Collapsed;
                    this.GridDetailUpdate.Visibility = System.Windows.Visibility.Visible;
                    break;
                case Common.geWinGroupType.InpListReport:
                    // DataGrid設定
                    this.GridDetail.Visibility = System.Windows.Visibility.Collapsed;
                    this.GridDetailUpdate.Visibility = System.Windows.Visibility.Collapsed;

                    this.utlFKey.btnF6.Content = "     F6     " + Environment.NewLine;
                    this.utlFKey.btnF6.IsEnabled = false;

                    break;
            }

            SetBinding();

            //GetListData();
        }

        private void ExUserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            System.Globalization.CultureInfo Culture = new System.Globalization.CultureInfo(Thread.CurrentThread.CurrentCulture.Name);
            // SelectedDateFormat=Shortのパターン
            Culture.DateTimeFormat.ShortDatePattern = "yyyy/MM/dd";
            // SelectedDateFormat=Longのパターン
            Culture.DateTimeFormat.LongDatePattern = "ggy'年'M'月'd'日'";
            Thread.CurrentThread.CurrentCulture = Culture;
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
            entity.PropertyChanged += this.utlPurchase_F.MstID_Changed;
            entity.PropertyChanged += this.utlPurchase_T.MstID_Changed;

            #region Bind

            // バインド
            Binding BindingInvoiceId_F = new Binding("purchase_id_from");
            BindingInvoiceId_F.Mode = BindingMode.TwoWay;
            BindingInvoiceId_F.Source = entity;
            this.utlPurchase_F.txtID.SetBinding(TextBox.TextProperty, BindingInvoiceId_F);

            // バインド
            Binding BindingInvoiceId_T = new Binding("purchase_id_to");
            BindingInvoiceId_T.Mode = BindingMode.TwoWay;
            BindingInvoiceId_T.Source = entity;
            this.utlPurchase_T.txtID.SetBinding(TextBox.TextProperty, BindingInvoiceId_T);

            #endregion

            this.utlPurchase_F.txtID.OnFormatString();
            this.utlPurchase_T.txtID.OnFormatString();
        }

        #endregion

        #region Function Key Button Events

        // F1ボタン() クリック
        public override void btnF1_Click(object sender, RoutedEventArgs e)
        {
            if (Common.gWinGroupType == Common.geWinGroupType.InpListReport) this.ProcKbn = eProcKbn.Report;
            else this.ProcKbn = eProcKbn.Update;

            this.utlReport.rptKbn = DataReport.geReportKbn.OutPut;

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
            ExVisualTreeHelper.initDisplay(this.LayoutRoot);
            this.entityList = null;
            this.dgUpdate.ItemsSource = null;
            DataInitWhere.InitDateYm(ref this.datYm);

            // 取引区分
            //this.chkSime.IsChecked = false;
            //this.chkKake.IsChecked = false;

            ExBackgroundWorker.DoWork_Focus(this.datYm, 10);
        }

        // F3ボタン(ﾀﾞｳﾝﾛｰﾄﾞ) クリック
        public override void btnF3_Click(object sender, RoutedEventArgs e)
        {
            if (Common.gWinGroupType != Common.geWinGroupType.InpListReport) return;

            this.ProcKbn = eProcKbn.Report;

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
                //case "utlPurchase_F":
                //case "utlEstimateNo_T":
                //    Utl_InpNoText inp = (Utl_InpNoText)activeControl;
                //    inp.ShowList();
                //    break;
                case "datPaymentCloseYmd_F":
                case "datPaymentCloseYmd_T":
                case "datIssueYmd":
                    ExDatePicker dt = (ExDatePicker)activeControl;
                    dt.ShowCalender();
                    break;
                case "utlPurchase_F":
                case "utlPurchase_T":
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

        // F11ボタン(出力設定) クリック
        public override void btnF11_Click(object sender, RoutedEventArgs e)
        {
            if (Common.gWinGroupType == Common.geWinGroupType.InpList) return;

            switch (Common.gWinGroupType)
            {
                case Common.geWinGroupType.InpListReport:
                    this.dlgReportSetting.pg_id = DataPgEvidence.PGName.PaymentManagement.PaymentCreditBalancePrint;
                    this.dlgReportSetting.IsReportRange = true;
                    this.dlgReportSetting.IsTotalKbn = false;
                    this.dlgReportSetting.IsGroupTotal = true;
                    break;
                default:
                    return;
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
                case "chkSalesCredit0_Yes":
                case "chkSalesCredit0_No":
                case "chkBussinesNo":
                case "chkBussinesYes":
                    ExVisualTreeHelper.SetFunctionKeyEnabled("F5", false, this);
                    activeControl = null;
                    break;
                case "datYm":
                case "utlPurchase_F":
                case "utlPurchase_T":
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

        private void dgUpdate_CellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
        {
            EntityPaymentCreditBalance entity = (EntityPaymentCreditBalance)e.Row.DataContext;

            // 明細計算
            switch (e.Column.DisplayIndex)
            {
                case 3:
                    double price = entity._before_payment_credit_balacne_upd;

                    for (int i = 0; i <= this.entityList.Count - 1; i++)
                    {
                        if (entity._purchase_id == this.entityList[i]._purchase_id)
                        {
                            if (this.dgUpdate.SelectedIndex != i)
                            {
                                this.entityList[i]._before_payment_credit_balacne_upd += price;
                            }
                            this.entityList[i]._this_payment_credit_balance = this.entityList[i]._before_payment_credit_balacne_upd -
                                                                            this.entityList[i]._this_payment_cash_price + this.entityList[i]._this_purchase_price + this.entityList[i]._this_tax;
                        }
                    }
                    entity._before_payment_credit_balacne_upd = price;
                    entity._this_payment_credit_balance = entity._before_payment_credit_balacne_upd - entity._this_payment_cash_price + entity._this_purchase_price + entity._this_tax;

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

        #region Check Box Checked

        private void chkSalesCredit_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            if (chk.Name == "chkSalesCredit0_Yes")
            {
                if (this.chkSalesCredit0_No.IsChecked == true)
                {
                    this.chkSalesCredit0_No.IsChecked = false;
                }
            }
            else if (chk.Name == "chkSalesCredit0_No")
            {
                if (this.chkSalesCredit0_Yes.IsChecked == true)
                {
                    this.chkSalesCredit0_Yes.IsChecked = false;
                }
            }
        }

        private void chkBussines_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            if (chk.Name == "chkBussinesNo")
            {
                if (this.chkBussinesYes.IsChecked == true)
                {
                    this.chkBussinesYes.IsChecked = false;
                }
            }
            else if (chk.Name == "chkBussinesYes")
            {
                if (this.chkBussinesNo.IsChecked == true)
                {
                    this.chkBussinesNo.IsChecked = false;
                }
            }
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

            // 対象年月
            if (this.datYm.Text.Trim() != "")
            {
                //strWhrer += "   AND T.PAYMENT_YYYYMMDD = " + ExEscape.zRepStr(this.datYm.Text.Trim()) + Environment.NewLine;
                strWhrerString1 += "[対象年月 " + this.datYm.Text.Trim();
            }
            else
            {
                strWhrerString1 += "[対象年月 未指定";
            }

            // 仕入先
            if (this.utlPurchase_F.txtID.Text.Trim() != "")
            {
                strWhrer += "   AND CM.PURCHASE_ID >= " + ExCast.zCLng(this.utlPurchase_F.txtID.Text.Trim()) + Environment.NewLine;
                strWhrerString1 += "] [仕入先 " + this.utlPurchase_F.txtID.Text.Trim() + "～";
            }
            else
            {
                strWhrerString1 += "] [仕入先 未指定～";
            }
            if (this.utlPurchase_T.txtID.Text.Trim() != "")
            {
                strWhrer += "   AND CM.PURCHASE_ID <= " + ExCast.zCLng(this.utlPurchase_T.txtID.Text.Trim()) + Environment.NewLine;
                strWhrerString1 += this.utlPurchase_T.txtID.Text.Trim();
            }
            else
            {
                strWhrerString1 += "未指定";
            }

            // 買掛残高
            string _buf = "";
            if (this.chkSalesCredit0_Yes.IsChecked == true)
            {
                strWhrer += "   AND (IFNULL(SB.PAYMENT_CREDIT_INIT_PRICE, 0) + (IFNULL(SH_BEFORE.SUM_NO_TAX_PRICE, 0) + IFNULL(SH_BEFORE.SUM_TAX, 0)) - (IFNULL(RP_BEFORE.SUM_PRICE, 0))) - (IFNULL(RP_THIS.SUM_PRICE, 0)) + IFNULL(SH_THIS.SUM_TAX, 0) = 0";
                _buf += "] [買掛残高 残高0";
            }
            if (this.chkSalesCredit0_No.IsChecked == true)
            {
                strWhrer += "   AND (IFNULL(SB.PAYMENT_CREDIT_INIT_PRICE, 0) + (IFNULL(SH_BEFORE.SUM_NO_TAX_PRICE, 0) + IFNULL(SH_BEFORE.SUM_TAX, 0)) - (IFNULL(RP_BEFORE.SUM_PRICE, 0))) - (IFNULL(RP_THIS.SUM_PRICE, 0)) + IFNULL(SH_THIS.SUM_TAX, 0) <> 0";
                if (_buf == "")
                {
                    _buf += "] [買掛残高 残高0以外";
                }
                else
                {
                    _buf += " 残高0以外";
                }
            }
            if (_buf == "")
            {
                _buf += "] [買掛残高 指定無し]";
            }
            else
            {
                _buf += "]";
            }
            strWhrerString1 += _buf;
            _buf = "";

            // 期間内取引
            if (this.chkBussinesNo.IsChecked == true)
            {
                strWhrer += "   AND (IFNULL(SH_THIS.COMPANY_ID, 'No') = 'No' AND IFNULL(RP_THIS.COMPANY_ID, 'No') = 'No') ";
                _buf += " [期間内取引 無し";
            }
            if (this.chkBussinesYes.IsChecked == true)
            {
                strWhrer += "   AND (IFNULL(SH_THIS.COMPANY_ID, 'No') <> 'No' OR IFNULL(RP_THIS.COMPANY_ID, 'No') <> 'No') ";
                if (_buf == "")
                {
                    _buf += " [期間内取引 有り";
                }
                else
                {
                    _buf += " 有り";
                }
            }
            if (_buf == "")
            {
                _buf += " [期間内取引 指定無し]";
            }
            else
            {
                _buf += "]";
            }
            strWhrerString1 += _buf;

            _buf = "";

            if (datYm.SelectedDate != null)
            {
                strWhrer += "<proc ym>" + ((DateTime)this.datYm.SelectedDate).ToString("yyyy/MM");
            }

            strWhrer = strWhrer.Replace(",", "<<@escape_comma@>>");
            strWhrer = strWhrer.Replace("'", "<<@escape_single_quotation@>>");

            return strWhrer + "WhereString =>" + strWhrerString1 + ";" + strWhrerString2;
        }

        // ソート句SQL設定
        private string GetSQLOrderBy()
        {
            string strOrderBy = "";
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
                    entityList = (ObservableCollection<EntityPaymentCreditBalance>)objList;

                    this.dgPrint.Focus();
                    this.dgPrint.ItemsSource = null;
                    this.dgPrint.ItemsSource = entityList;
                    this.dgUpdate.ItemsSource = null;
                    this.dgUpdate.ItemsSource = entityList;
                    ExBackgroundWorker.DoWork_Focus(this.dgUpdate, 10);
                }
                else
                {
                    entityList = null;
                    this.dgPrint.ItemsSource = null;
                    this.dgUpdate.ItemsSource = null;
                    ExBackgroundWorker.DoWork_Focus(this.datYm, 10);
                }
            }
        }

        #endregion

        #endregion

        #region Data Update

        // データ追加・更新・削除
        private void UpdateData(Common.geUpdateType upd)
        {
            object[] prm = new object[2];

            prm[0] = (int)upd;
            prm[1] = this.entityList;
            webService.objPerent = this;
            webService.CallWebService(ExWebService.geWebServiceCallKbn.UpdatePaymentCreditBalance,
                                      ExWebService.geDialogDisplayFlg.Yes,
                                      ExWebService.geDialogCloseFlg.Yes,
                                      prm);
        }

        public override void DataUpdate(int intKbn, string errMessage)
        {
            if ((ExWebService.geWebServiceCallKbn)intKbn == ExWebService.geWebServiceCallKbn.UpdatePaymentCreditBalance)
            {
                if (string.IsNullOrEmpty(errMessage))
                {
                    ExMessageBox.Show("登録しました。");
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

                        // 対象年月
                        if (string.IsNullOrEmpty(this.datYm.Text.Trim()))
                        {
                            errMessage += "対象年月を入力(選択)して下さい" + Environment.NewLine;
                            if (errCtl == null) errCtl = this.datYm;
                        }

                        #endregion

                        #region 入力チェック

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

                        #endregion

                        #region 範囲チェック

                        // 仕入先
                        if (this.utlPurchase_F.txtID.Text.Trim() != "" && string.IsNullOrEmpty(this.utlPurchase_F.txtID.Text.Trim()))
                        {
                            errMessage += "仕入先が適切に入力(選択)されていません。" + Environment.NewLine;
                            if (errCtl == null) errCtl = this.utlPurchase_F.txtID;
                        }
                        if (this.utlPurchase_T.txtID.Text.Trim() != "" && string.IsNullOrEmpty(this.utlPurchase_T.txtID.Text.Trim()))
                        {
                            errMessage += "仕入先が適切に入力(選択)されていません。" + Environment.NewLine;
                            if (errCtl == null) errCtl = this.utlPurchase_T.txtID;
                        }

                        #endregion

                        #region 日付チェック

                        // 対象年月
                        if (string.IsNullOrEmpty(this.datYm.Text.Trim()) == false)
                        {
                            if (ExCast.IsDateYm(this.datYm.Text.Trim()) == false)
                            {
                                errMessage += "対象年月の形式が不正です。(yyyy/mm形式で入力(選択)して下さい)" + Environment.NewLine;
                                if (errCtl == null) errCtl = this.datYm;
                            }
                        }

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

                        #endregion

                        #region 選択チェック

                        if (this.entityList == null)
                        {
                            errMessage += "表示データがありません。" + Environment.NewLine;
                            if (errCtl == null) errCtl = this.datYm;
                        }
                        if (this.entityList.Count == 0)
                        {
                            errMessage += "表示データがありません。" + Environment.NewLine;
                            if (errCtl == null) errCtl = this.datYm;
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
                            if (errCtl == null) errCtl = this.datYm;
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
                    if (this.ProcKbn == eProcKbn.Update)
                    {
                        UpdateData(Common.geUpdateType.Update);
                        return;
                    }

                    this.utlReport.utlParentFKey = this.utlFKey;

                    switch (Common.gWinGroupType)
                    {
                        case Common.geWinGroupType.InpListReport:
                            this.utlReport.pgId = DataPgEvidence.PGName.PaymentManagement.PaymentCreditBalancePrint;
                            break;
                        default:
                            break;
                    }

                    this.utlReport.sqlWhere = GetSQLWhere();
                    this.utlReport.sqlOrderBy = GetSQLOrderBy();
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

    }

}
