using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Printing;
using System.Collections.ObjectModel;
using SlvHanbaiClient.Class.Utility;
using SlvHanbaiClient.Class.Data;
using SlvHanbaiClient.View.Dlg;
using SlvHanbaiClient.View.Dlg.Report;
using SlvHanbaiClient.Class;
using SlvHanbaiClient.Class.UI;
using SlvHanbaiClient.Class.Entity;
using SlvHanbaiClient.svcReport;
using SlvHanbaiClient.svcSysLogin;
using SlvHanbaiClient.svcInvoiceClose;
using SlvHanbaiClient.svcPaymentClose;
using SlvHanbaiClient.Class.WebService;
using SlvHanbaiClient.View.UserControl.Custom;

namespace SlvHanbaiClient.View.UserControl.Report
{
    public partial class Utl_Report : ExUserControl
    {

        #region Filed Const

        private const String CLASS_NM = "Utl_Report";

        PrintDocument pd;

        public Common.gePageType gPageType = Common.gePageType.None;
        public Common.geWinMsterType gWinMsterType = Common.geWinMsterType.None;

        public DataReport.geReportKbn rptKbn;
        private Control activeControl;
        public string pgId = "";

        public Utl_FunctionKey utlParentFKey = null;

        public string sqlWhere = "";
        public string sqlOrderBy = "";
        
        public bool blClose = false;

        public ExUserControl parentUtl = null;

        private Dlg_ReportView reportView = null;

        public object updPrintNo = null;

        public MstData.geMGroupKbn MstGroupKbn = MstData.geMGroupKbn.None;

        #endregion

        #region Constructor

        public Utl_Report()
        {
            InitializeComponent();
        }

        #endregion

        #region Page Events

        private void ExUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Init();

            // 画面初期化
            ExVisualTreeHelper.initDisplay(this.LayoutRoot);

            ExBackgroundWorker.DoWork_Focus(this.utlID_F.txtID, 100);
            ExBackgroundWorker.DoWork_Focus(this.utlID_F.txtID, 200);
            ExBackgroundWorker.DoWork_Focus(this.utlID_F.txtID, 300);
            ExBackgroundWorker.DoWork_Focus(this.utlID_F.txtID, 400);
            ExBackgroundWorker.DoWork_Focus(this.utlID_F.txtID, 500);

            this.utlParentFKey = this.utlFunctionKey;

            this.blClose = false;
        }

        private void ExUserControl_Unloaded(object sender, RoutedEventArgs e)
        {
        }

        private void ExUserControl_GotFocus(object sender, RoutedEventArgs e)
        {
        }

        public void Init()
        {
            if (this.gPageType != Common.gePageType.None)
            {
                switch (this.gPageType)
                {
                    case Common.gePageType.InpOrder:
                        this.pgId = DataPgEvidence.PGName.Order.OrderListPrint;
                        break;
                }
            }
            else
            {
                EntitySearch　entity = new EntitySearch();
                entity.PropertyChanged -= this.utlID_F.MstID_Changed;
                entity.PropertyChanged += this.utlID_F.MstID_Changed;
                entity.PropertyChanged -= this.utlID_T.MstID_Changed;
                entity.PropertyChanged += this.utlID_T.MstID_Changed;

                switch (this.gWinMsterType)
                {
                    #region 得意先

                    case Common.geWinMsterType.Customer:
                        this.lblID.Content = "得意先";
                        this.utlID_F.MstKbn = MstData.geMDataKbn.Customer_F;
                        this.utlID_T.MstKbn = MstData.geMDataKbn.Customer_T;

                        this.utlID_F.txtID.InputMode = ExTextBox.geInputMode.ID;
                        this.utlID_F.txtID.MaxLengthB = Common.gintidFigureCustomer;
                        this.utlID_T.txtID.InputMode = ExTextBox.geInputMode.ID;
                        this.utlID_T.txtID.MaxLengthB = Common.gintidFigureCustomer;

                        this.cmbOrder.Items.Clear();
                        this.cmbOrder.Items.Add("ID順");
                        this.cmbOrder.Items.Add("得意先分類順");
                        this.cmbOrder.Items.Add("フリガナ順");
                        this.cmbOrder.SelectedIndex = 0;

                        this.pgId = DataPgEvidence.PGName.Mst.Customer;

                        Binding BindingCustomerF = new Binding("customer_id_from");
                        BindingCustomerF.Mode = BindingMode.TwoWay;
                        BindingCustomerF.Source = entity;
                        this.utlID_F.txtID.SetBinding(TextBox.TextProperty, BindingCustomerF);

                        Binding BindingCustomerT = new Binding("customer_id_to");
                        BindingCustomerT.Mode = BindingMode.TwoWay;
                        BindingCustomerT.Source = entity;
                        this.utlID_T.txtID.SetBinding(TextBox.TextProperty, BindingCustomerT);

                        break;

                    #endregion

                    #region 納入先

                    case Common.geWinMsterType.Supplier:
                        this.lblID.Content = "得意先";
                        this.utlID_F.MstKbn = MstData.geMDataKbn.Customer_F;
                        this.utlID_T.MstKbn = MstData.geMDataKbn.Customer_T;

                        this.utlID_F.txtID.InputMode = ExTextBox.geInputMode.ID;
                        this.utlID_F.txtID.MaxLengthB = Common.gintidFigureCustomer;
                        this.utlID_T.txtID.InputMode = ExTextBox.geInputMode.ID;
                        this.utlID_T.txtID.MaxLengthB = Common.gintidFigureCustomer;

                        this.cmbOrder.Items.Clear();
                        this.cmbOrder.Items.Add("得意先ID");
                        this.cmbOrder.Items.Add("納入先ID順");
                        this.cmbOrder.Items.Add("フリガナ順");
                        this.cmbOrder.SelectedIndex = 0;

                        this.pgId = DataPgEvidence.PGName.Mst.Supplier;

                        Binding BindingSupplierF = new Binding("customer_id_from");
                        BindingSupplierF.Mode = BindingMode.TwoWay;
                        BindingSupplierF.Source = entity;
                        this.utlID_F.txtID.SetBinding(TextBox.TextProperty, BindingSupplierF);

                        Binding BindingSupplierT = new Binding("customer_id_to");
                        BindingSupplierT.Mode = BindingMode.TwoWay;
                        BindingSupplierT.Source = entity;
                        this.utlID_T.txtID.SetBinding(TextBox.TextProperty, BindingSupplierT);

                        break;

                    #endregion

                    #region 仕入先

                    case Common.geWinMsterType.Purchase:
                        this.lblID.Content = "仕入先";
                        this.utlID_F.MstKbn = MstData.geMDataKbn.Purchase_F;
                        this.utlID_T.MstKbn = MstData.geMDataKbn.Purchase_T;

                        this.utlID_F.txtID.InputMode = ExTextBox.geInputMode.ID;
                        this.utlID_F.txtID.MaxLengthB = Common.gintidFigurePurchase;
                        this.utlID_T.txtID.InputMode = ExTextBox.geInputMode.ID;
                        this.utlID_T.txtID.MaxLengthB = Common.gintidFigurePurchase;

                        this.cmbOrder.Items.Clear();
                        this.cmbOrder.Items.Add("ID順");
                        this.cmbOrder.Items.Add("仕入先分類順");
                        this.cmbOrder.Items.Add("フリガナ順");
                        this.cmbOrder.SelectedIndex = 0;

                        this.pgId = DataPgEvidence.PGName.Mst.Purchase;

                        Binding BindingPurchaseF = new Binding("purchase_id_from");
                        BindingPurchaseF.Mode = BindingMode.TwoWay;
                        BindingPurchaseF.Source = entity;
                        this.utlID_F.txtID.SetBinding(TextBox.TextProperty, BindingPurchaseF);

                        Binding BindingPurchaseT = new Binding("purchase_id_to");
                        BindingPurchaseT.Mode = BindingMode.TwoWay;
                        BindingPurchaseT.Source = entity;
                        this.utlID_T.txtID.SetBinding(TextBox.TextProperty, BindingPurchaseT);

                        break;

                    #endregion

                    #region 担当者

                    case Common.geWinMsterType.Person:
                        this.lblID.Content = "担当者";
                        this.utlID_F.MstKbn = MstData.geMDataKbn.Person_F;
                        this.utlID_T.MstKbn = MstData.geMDataKbn.Person_T;

                        this.utlID_F.txtID.InputMode = ExTextBox.geInputMode.Number;
                        this.utlID_F.txtID.MinNumber = 1;
                        this.utlID_F.txtID.MaxNumber = 999;
                        this.utlID_F.txtID.FormatString = "000";
                        this.utlID_T.txtID.InputMode = ExTextBox.geInputMode.Number;
                        this.utlID_T.txtID.MinNumber = 1;
                        this.utlID_T.txtID.MaxNumber = 999;
                        this.utlID_T.txtID.FormatString = "000";

                        this.cmbOrder.Items.Clear();
                        this.cmbOrder.Items.Add("ID順");
                        this.cmbOrder.Items.Add("グループID順");
                        this.cmbOrder.SelectedIndex = 0;

                        this.pgId = DataPgEvidence.PGName.Mst.Person;

                        Binding BindingPersonF = new Binding("person_id_from");
                        BindingPersonF.Mode = BindingMode.TwoWay;
                        BindingPersonF.Source = entity;
                        this.utlID_F.txtID.SetBinding(TextBox.TextProperty, BindingPersonF);

                        Binding BindingPersonT = new Binding("person_id_to");
                        BindingPersonT.Mode = BindingMode.TwoWay;
                        BindingPersonT.Source = entity;
                        this.utlID_T.txtID.SetBinding(TextBox.TextProperty, BindingPersonT);

                        break;

                    #endregion

                    #region 商品

                    case Common.geWinMsterType.Commodity:
                        this.lblID.Content = "商品";
                        this.utlID_F.MstKbn = MstData.geMDataKbn.Commodity_F;
                        this.utlID_T.MstKbn = MstData.geMDataKbn.Commodity_T;

                        this.utlID_F.txtID.InputMode = ExTextBox.geInputMode.ID;
                        this.utlID_F.txtID.MaxLengthB = Common.gintidFigureCommodity;
                        this.utlID_T.txtID.InputMode = ExTextBox.geInputMode.ID;
                        this.utlID_T.txtID.MaxLengthB = Common.gintidFigureCommodity;

                        this.cmbOrder.Items.Clear();
                        this.cmbOrder.Items.Add("ID順");
                        this.cmbOrder.Items.Add("商品分類順");
                        this.cmbOrder.Items.Add("フリガナ順");
                        this.cmbOrder.SelectedIndex = 0;

                        this.pgId = DataPgEvidence.PGName.Mst.Commodity;

                        Binding BindingCommodityF = new Binding("commodity_id_from");
                        BindingCommodityF.Mode = BindingMode.TwoWay;
                        BindingCommodityF.Source = entity;
                        this.utlID_F.txtID.SetBinding(TextBox.TextProperty, BindingCommodityF);

                        Binding BindingCommodityT = new Binding("commodity_id_to");
                        BindingCommodityT.Mode = BindingMode.TwoWay;
                        BindingCommodityT.Source = entity;
                        this.utlID_T.txtID.SetBinding(TextBox.TextProperty, BindingCommodityT);

                        break;

                    #endregion

                    #region 締区分

                    case Common.geWinMsterType.Condition:
                        this.lblID.Content = "締区分";
                        this.utlID_F.MstKbn = MstData.geMDataKbn.Condition_F;
                        this.utlID_T.MstKbn = MstData.geMDataKbn.Condition_T;

                        this.utlID_F.txtID.InputMode = ExTextBox.geInputMode.Number;
                        this.utlID_F.txtID.MinNumber = 1;
                        this.utlID_F.txtID.MaxNumber = 99;
                        this.utlID_F.txtID.FormatString = "00";
                        this.utlID_T.txtID.InputMode = ExTextBox.geInputMode.Number;
                        this.utlID_T.txtID.MinNumber = 1;
                        this.utlID_T.txtID.MaxNumber = 99;
                        this.utlID_T.txtID.FormatString = "00";

                        this.cmbOrder.Items.Clear();
                        this.cmbOrder.Items.Add("ID順");
                        this.cmbOrder.SelectedIndex = 0;

                        this.pgId = DataPgEvidence.PGName.Mst.Condition;

                        Binding BindingConditionF = new Binding("condition_id_from");
                        BindingConditionF.Mode = BindingMode.TwoWay;
                        BindingConditionF.Source = entity;
                        this.utlID_F.txtID.SetBinding(TextBox.TextProperty, BindingConditionF);

                        Binding BindingConditionT = new Binding("condition_id_to");
                        BindingConditionT.Mode = BindingMode.TwoWay;
                        BindingConditionT.Source = entity;
                        this.utlID_T.txtID.SetBinding(TextBox.TextProperty, BindingConditionT);

                        break;

                    #endregion

                    #region 分類

                    case Common.geWinMsterType.Class:
                        this.lblID.Content = "分類";
                        this.utlID_F.MstKbn = MstData.geMDataKbn.Group_F;
                        this.utlID_T.MstKbn = MstData.geMDataKbn.Group_T;
                        this.utlID_F.MstGroupKbn = this.MstGroupKbn;
                        this.utlID_T.MstGroupKbn = this.MstGroupKbn;

                        this.utlID_F.txtID.InputMode = ExTextBox.geInputMode.Number;
                        this.utlID_F.txtID.MinNumber = 1;
                        this.utlID_F.txtID.MaxNumber = 999;
                        this.utlID_F.txtID.FormatString = "000";
                        this.utlID_T.txtID.InputMode = ExTextBox.geInputMode.Number;
                        this.utlID_T.txtID.MinNumber = 1;
                        this.utlID_T.txtID.MaxNumber = 999;
                        this.utlID_T.txtID.FormatString = "000";

                        this.cmbOrder.Items.Clear();
                        this.cmbOrder.Items.Add("ID順");
                        this.cmbOrder.SelectedIndex = 0;

                        this.pgId = DataPgEvidence.PGName.Mst.Class;

                        Binding BindingClassF = new Binding("class_id_from");
                        BindingClassF.Mode = BindingMode.TwoWay;
                        BindingClassF.Source = entity;
                        this.utlID_F.txtID.SetBinding(TextBox.TextProperty, BindingClassF);

                        Binding BindingClassT = new Binding("class_id_to");
                        BindingClassT.Mode = BindingMode.TwoWay;
                        BindingClassT.Source = entity;
                        this.utlID_T.txtID.SetBinding(TextBox.TextProperty, BindingClassT);

                        break;

                    #endregion
                }
            }
        }

        #endregion

        #region Function Key Button Method

        // F1ボタン(出力) クリック
        public override void btnF1_Click(object sender, RoutedEventArgs e)
        {
            this.utlParentFKey.IsEnabled = false;

            // ボタン押下時非同期入力チェックON
            Common.gblnBtnDesynchronizeLock = true;

            this.rptKbn = DataReport.geReportKbn.OutPut;

            // 入力確定の為、BackgroundWorker経由で入力チェックを呼出
            ExBackgroundInputCheckWk bk = new ExBackgroundInputCheckWk();
            bk.utl = this;
            this.txtDummy.IsTabStop = true;
            bk.focusCtl = this.txtDummy;
            bk.bw.RunWorkerAsync();
        }

        // F2ボタン(ダウンロード) クリック
        public override void btnF2_Click(object sender, RoutedEventArgs e)
        {
            this.utlParentFKey.IsEnabled = false;

            // ボタン押下時非同期入力チェックON
            Common.gblnBtnDesynchronizeLock = true;

            this.rptKbn = DataReport.geReportKbn.Download;

            // 入力確定の為、BackgroundWorker経由で入力チェックを呼出
            ExBackgroundInputCheckWk bk = new ExBackgroundInputCheckWk();
            bk.utl = this;
            this.txtDummy.IsTabStop = true;
            bk.focusCtl = this.txtDummy;
            bk.bw.RunWorkerAsync();
        }

        // F3ボタン(CSV) クリック
        public override void btnF3_Click(object sender, RoutedEventArgs e)
        {
            this.utlParentFKey.IsEnabled = false;

            // ボタン押下時非同期入力チェックON
            Common.gblnBtnDesynchronizeLock = true;

            this.rptKbn = DataReport.geReportKbn.Csv;

            // 入力確定の為、BackgroundWorker経由で入力チェックを呼出
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
                case "utlID_F":
                    this.utlID_F.ShowList();
                    break;
                case "utlID_T":
                    this.utlID_T.ShowList();
                    break;
                default:
                    break;
            }
        }

        // F11ボタン(出力設定) クリック
        public override void btnF11_Click(object sender, RoutedEventArgs e)
        {
        }

        // F12ボタン(キャンセル) クリック
        public override void btnF12_Click(object sender, RoutedEventArgs e)
        {
            Dlg_Report win = (Dlg_Report)ExVisualTreeHelper.FindPerentChildWindow(this);
            if (this.utlParentFKey != null) this.utlParentFKey.IsEnabled = true;
            win.Close();
        }

        #endregion

        #region Report

        public void ReportStart()
        {
            try
            {
                if (this.utlParentFKey != null) this.utlParentFKey.IsEnabled = false;

                ExWebService.geDialogDisplayFlg DialogDisplayFlg = ExWebService.geDialogDisplayFlg.Yes;
                if (this.blClose == true || this.rptKbn == DataReport.geReportKbn.OutPut)
                {
                    DialogDisplayFlg = ExWebService.geDialogDisplayFlg.No;
                }

                switch (this.rptKbn)
                {
                    case DataReport.geReportKbn.Download:
                        Common.gstrProgressDialogTitle = "PDFファイル作成中";
                        break;
                    case DataReport.geReportKbn.Csv:
                        Common.gstrProgressDialogTitle = "CSVファイル作成中";
                        break;
                }

                object[] prm = new object[3];
                prm[0] = (int)this.rptKbn;
                prm[1] = pgId;

                if (this.gWinMsterType != Common.geWinMsterType.None)
                {
                    if (this.gWinMsterType == Common.geWinMsterType.Class)
                    {
                        prm[2] = ExCast.zNumNoFormat(ExCast.zCStr((int)this.MstGroupKbn) + "<<@escape_comma@>>" + this.utlID_F.txtID.Text.Trim()) + "," +
                                 ExCast.zNumNoFormat(ExCast.zCStr((int)this.MstGroupKbn) + "<<@escape_comma@>>" + this.utlID_T.txtID.Text.Trim()) + "," +
                                 ExCast.zCStr(this.datUpdateYmd.SelectedDate) + "," +
                                 ExCast.zCStr(this.cmbOrder.SelectedIndex);
                    }
                    else
                    {
                        prm[2] = ExCast.zNumNoFormat(this.utlID_F.txtID.Text.Trim()) + "," +
                                 ExCast.zNumNoFormat(this.utlID_T.txtID.Text.Trim()) + "," +
                                 ExCast.zCStr(this.datUpdateYmd.SelectedDate) + "," +
                                 ExCast.zCStr(this.cmbOrder.SelectedIndex);
                    }
                }
                else
                {
                    prm[2] = this.sqlWhere + "," +
                             this.sqlOrderBy;
                }
                webService.objPerent = this;

                switch (this.rptKbn)
                {
                    case DataReport.geReportKbn.OutPut:
                        reportView = new Dlg_ReportView();
                        reportView.pgId = pgId;
                        reportView.rptKbn = this.rptKbn;
                        reportView.Closed -= dlg_Closed;
                        reportView.Closed += dlg_Closed;
                        reportView.Show();
                        break;
                }

                webService.CallWebService(ExWebService.geWebServiceCallKbn.ReportOut,
                                          DialogDisplayFlg,
                                          ExWebService.geDialogCloseFlg.No,
                                          prm);
            }
            catch
            {
                if (this.utlParentFKey != null) this.utlParentFKey.IsEnabled = true;
            }
        }

        public override void ReportOut(object entity)
        {
            string _msg = "";
            ExWebClient wc = null;

            try
            {
                if (entity != null)
                {
                    EntityReport _entity = (EntityReport)entity;

                    // 失敗
                    if (_entity._message != "" || _entity._ret == false)
                    {
                        reportView.Close();
                        if (this.utlParentFKey != null) this.utlParentFKey.IsEnabled = true;
                        return;
                    }

                    switch (this.rptKbn)
                    {
                        case DataReport.geReportKbn.OutPut:
                            //ExHyperlinkButton link = new ExHyperlinkButton(_entity._downLoadUrl);
                            //link.ClickMe();
                            reportView.reportUrl = _entity._downLoadUrl;
                            reportView.reportFileName = _entity._downLoadFileName;
                            reportView.reportFilePath = _entity._downLoadFilePath;
                            reportView.ViewReport();
                            break;
                        case DataReport.geReportKbn.Download:
                        case DataReport.geReportKbn.Csv:
                            string[] prm = new string[4];
                            prm[0] = _entity._downLoadFileName;
                            prm[1] = _entity._downLoadFilePath;
                            wc = new ExWebClient();
                            wc.utlParentFKey = this.utlParentFKey;
                            webService.ProcessingDlgClose();
                            wc.FileDownLoad(this.rptKbn, pgId, prm);
                            break;
                    }

                    #region 印刷発行済セット

                    switch (pgId)
                    {
                        case DataPgEvidence.PGName.Estimate.EstimatePrint:
                            if (this.updPrintNo != null)
                            {
                                string _no = (string)this.updPrintNo;
                                object[] prm = new object[2];
                                prm[0] = (int)Common.geUpdateType.Update;
                                prm[1] = _no;
                                webService.objPerent = this;
                                webService.CallWebService(ExWebService.geWebServiceCallKbn.UpdateEstimatePrint,
                                                          ExWebService.geDialogDisplayFlg.No,
                                                          ExWebService.geDialogCloseFlg.No,
                                                          prm);
                            }
                            break;
                        case DataPgEvidence.PGName.Order.OrderPrint:
                            if (this.updPrintNo != null)
                            {
                                string _no = (string)this.updPrintNo;
                                object[] prm = new object[2];
                                prm[0] = (int)Common.geUpdateType.Update;
                                prm[1] = _no;
                                webService.objPerent = this;
                                webService.CallWebService(ExWebService.geWebServiceCallKbn.UpdateOrderPrint,
                                                          ExWebService.geDialogDisplayFlg.No,
                                                          ExWebService.geDialogCloseFlg.No,
                                                          prm);
                            }
                            break;
                        case DataPgEvidence.PGName.Sales.SalesPrint:
                            if (this.updPrintNo != null)
                            {
                                string _no = (string)this.updPrintNo;
                                object[] prm = new object[2];
                                prm[0] = (int)Common.geUpdateType.Update;
                                prm[1] = _no;
                                webService.objPerent = this;
                                webService.CallWebService(ExWebService.geWebServiceCallKbn.UpdateSalesPrint,
                                                          ExWebService.geDialogDisplayFlg.No,
                                                          ExWebService.geDialogCloseFlg.No,
                                                          prm);
                            }
                            break;
                        case DataPgEvidence.PGName.Invoice.InvoicePrint:
                            if (this.updPrintNo != null)
                            {
                                ObservableCollection<EntityInvoiceClose> lstPrm = (ObservableCollection<EntityInvoiceClose>)this.updPrintNo;
                                object[] prm = new object[2];
                                prm[0] = (int)Common.geUpdateType.Update;
                                prm[1] = lstPrm;
                                webService.objPerent = this;
                                webService.CallWebService(ExWebService.geWebServiceCallKbn.UpdateInvoicePrint,
                                                          ExWebService.geDialogDisplayFlg.No,
                                                          ExWebService.geDialogCloseFlg.No,
                                                          prm);
                            }
                            break;
                        case DataPgEvidence.PGName.PurchaseOrder.PurchaseOrderPrint:
                            if (this.updPrintNo != null)
                            {
                                string _no = (string)this.updPrintNo;
                                object[] prm = new object[2];
                                prm[0] = (int)Common.geUpdateType.Update;
                                prm[1] = _no;
                                webService.objPerent = this;
                                webService.CallWebService(ExWebService.geWebServiceCallKbn.UpdatePurchaseOrderPrint,
                                                          ExWebService.geDialogDisplayFlg.No,
                                                          ExWebService.geDialogCloseFlg.No,
                                                          prm);
                            }
                            break;
                        case DataPgEvidence.PGName.Payment.PaymentPrint:
                            if (this.updPrintNo != null)
                            {
                                ObservableCollection<EntityPaymentClose> lstPrm = (ObservableCollection<EntityPaymentClose>)this.updPrintNo;
                                object[] prm = new object[2];
                                prm[0] = (int)Common.geUpdateType.Update;
                                prm[1] = lstPrm;
                                webService.objPerent = this;
                                webService.CallWebService(ExWebService.geWebServiceCallKbn.UpdatePaymentPrint,
                                                          ExWebService.geDialogDisplayFlg.No,
                                                          ExWebService.geDialogCloseFlg.No,
                                                          prm);
                            }
                            break;

                    }

                    #endregion

                }
                else
                {
                    // 失敗
                    switch (this.rptKbn)
                    {
                        case DataReport.geReportKbn.OutPut:
                            _msg = "レポート出力";
                            break;
                        case DataReport.geReportKbn.Download:
                            _msg = "レポートダウンロード";
                            break;
                        case DataReport.geReportKbn.Csv:
                            _msg = "CSVダウンロード";
                            break;
                    }
                    if (_msg != "") ExMessageBox.Show(_msg + "で予期せぬエラーが発生しました。");
                    if (this.utlParentFKey != null) this.utlParentFKey.IsEnabled = true;
                    return;
                }
            }
            catch
            {
                webService.ProcessingDlgClose();
                if (this.utlParentFKey != null) this.utlParentFKey.IsEnabled = true;
            }
        }

        private void dlg_Closed(object sender, EventArgs e)
        {
            try
            {
                if (parentUtl == null) return;
                parentUtl.ReportViewClose();
                ExBackgroundWorker.DoWork_Focus(this.utlID_F.txtID, 10);
            }
            finally
            {
                if (this.utlParentFKey != null) this.utlParentFKey.IsEnabled = true;
            }
        }

        #endregion

        #region GotFocus

        private void utlID_F_GotFocus(object sender, RoutedEventArgs e)
        {
            ExVisualTreeHelper.GetUtlFunctionKey(this.LayoutRoot).SetFunctionKeyEnable("F5", true);
            activeControl = this.utlID_F;
        }

        private void utlID_T_GotFocus(object sender, RoutedEventArgs e)
        {
            ExVisualTreeHelper.GetUtlFunctionKey(this.LayoutRoot).SetFunctionKeyEnable("F5", true);
            activeControl = this.utlID_T;
        }

        private void datUpdateYmd_GotFocus(object sender, RoutedEventArgs e)
        {
            ExVisualTreeHelper.GetUtlFunctionKey(this.LayoutRoot).SetFunctionKeyEnable("F5", false);
            activeControl = null;
        }

        private void cmbOrder_GotFocus(object sender, RoutedEventArgs e)
        {
            ExVisualTreeHelper.GetUtlFunctionKey(this.LayoutRoot).SetFunctionKeyEnable("F5", false);
            activeControl = null;
        }

        #endregion

        #region Login

        // ログイン
        //public void Login()
        //{
        //    try
        //    {
        //        ExWebService.geDialogDisplayFlg DialogDisplayFlg = ExWebService.geDialogDisplayFlg.Yes;
        //        if (this.blClose == true || this.rptKbn == DataReport.geReportKbn.OutPut)
        //        { 
        //            DialogDisplayFlg = ExWebService.geDialogDisplayFlg.No;
        //        }

        //        switch (this.rptKbn)
        //        {
        //            case DataReport.geReportKbn.Download:
        //                Common.gstrProgressDialogTitle = "PDFファイル作成中";
        //                break;
        //            case DataReport.geReportKbn.Csv:
        //                Common.gstrProgressDialogTitle = "CSVファイル作成中";
        //                break;
        //        }

        //        object[] prm = new object[3];
        //        prm[0] = Common.gstrLoginUserID;
        //        prm[1] = Common.gstrLoginPassword;
        //        prm[2] = 0;
        //        webService.objPerent = this;
        //        webService.CallWebService(ExWebService.geWebServiceCallKbn.Login,
        //                                  DialogDisplayFlg,
        //                                  ExWebService.geDialogCloseFlg.No,
        //                                  prm);
        //    }
        //    catch
        //    {
        //        if (this.utlParentFKey != null) this.utlParentFKey.IsEnabled = true;
        //    }
        //}

        //public override void DataSelect(int intKbn, object objList)
        //{
        //    try
        //    {
        //        switch ((ExWebService.geWebServiceCallKbn)intKbn)
        //        {
        //            case ExWebService.geWebServiceCallKbn.Login:
        //                EntitySysLogin entity = (EntitySysLogin)objList;
        //                switch (entity._login_flg)
        //                {
        //                    case 0:     // 正常ログイン
        //                        // システム情報設定
        //                        Common.gintCompanyId = entity._company_id;
        //                        Common.gstrCompanyNm = entity._company_nm;
        //                        Common.gintGroupId = entity._group_id;
        //                        Common.gstrGroupNm = entity._group_nm;
        //                        Common.gintDefaultPersonId = entity._defult_person_id;
        //                        Common.gstrDefaultPersonNm = entity._defult_person_nm;
        //                        Common.gstrGroupDisplayNm = entity._group_display_name;
        //                        Common.gintEvidenceFlg = entity._evidence_flg;
        //                        Common.gintidFigureSlipNo = entity._idFigureSlipNo;
        //                        Common.gintidFigureCustomer = entity._idFigureCustomer;
        //                        Common.gintidFigurePurchase = entity._idFigurePurchase;
        //                        Common.gintidFigureCommodity = entity._idFigureGoods;
        //                        Common.gstrSessionString = entity._session_string;
        //                        Common.gblnLogin = true;
        //                        break;
        //                    case 1:     // 同一ユーザーログイン
        //                        break;
        //                    default:    // ログイン失敗
        //                        if (this.utlParentFKey != null) this.utlParentFKey.IsEnabled = true;
        //                        return;
        //                }
        //                break;
        //        }

        //        if (this.blClose == true)
        //        {
        //            if (this.parentUtl != null)
        //            {
        //                this.blClose = false;
        //                if (this.utlParentFKey != null) this.utlParentFKey.IsEnabled = true;
        //                return;
        //            }
        //            Dlg_Report win = (Dlg_Report)ExVisualTreeHelper.FindPerentChildWindow(this);
        //            if (this.utlParentFKey != null) this.utlParentFKey.IsEnabled = true;
        //            win.Close();
        //        }
        //        else
        //        {
        //            Report();
        //        }
        //    }
        //    catch
        //    {
        //        if (this.utlParentFKey != null) this.utlParentFKey.IsEnabled = true;
        //    }
        //}

        #endregion

        #region InputCheck

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
                #region 入力チェック

                #region 必須チェック

                #endregion

                #region 入力チェック

                if (!string.IsNullOrEmpty(utlID_F.txtID.Text.Trim()) && string.IsNullOrEmpty(utlID_F.txtNm.Text.Trim()))
                {
                    errMessage += this.lblID.Content + "が適切に入力(選択)されていません。" + Environment.NewLine;
                    if (errCtl == null) errCtl = this.utlID_F.txtID;
                }

                if (!string.IsNullOrEmpty(utlID_T.txtID.Text.Trim()) && string.IsNullOrEmpty(utlID_T.txtNm.Text.Trim()))
                {
                    errMessage += this.lblID.Content + "が適切に入力(選択)されていません。" + Environment.NewLine;
                    if (errCtl == null) errCtl = this.utlID_T.txtID;
                }

                #endregion

                #region 日付チェック

                //// 受注日
                //if (string.IsNullOrEmpty(_entityH._order_ymd) == false)
                //{
                //    if (ExCast.IsDate(_entityH._order_ymd) == false)
                //    {
                //        errMessage += "受注日の形式が不正です。(yyyy/mm/dd形式で入力(選択)して下さい)" + Environment.NewLine;
                //        if (errCtl == null) errCtl = this.datOrderYmd;
                //    }
                //}

                //// 納入指定日
                //if (string.IsNullOrEmpty(_entityH._supply_ymd) == false)
                //{
                //    if (ExCast.IsDate(_entityH._supply_ymd) == false)
                //    {
                //        errMessage += "納入指定日の形式が不正です。(yyyy/mm/dd形式で入力(選択)して下さい)" + Environment.NewLine;
                //        if (errCtl == null) errCtl = this.datNokiYmd;
                //    }
                //}

                #endregion

                #region 日付変換

                //// 受注日
                //if (string.IsNullOrEmpty(_entityH._order_ymd) == false)
                //{
                //    _entityH._order_ymd = ExCast.zConvertToDate(_entityH._order_ymd).ToString("yyyy/MM/dd");

                //}

                //// 納入指定日
                //if (string.IsNullOrEmpty(_entityH._supply_ymd) == false)
                //{
                //    _entityH._supply_ymd = ExCast.zConvertToDate(_entityH._supply_ymd).ToString("yyyy/MM/dd");
                //}

                #endregion

                #region 正数チェック

                //if (ExCast.zCLng(_entityH._no) < 0)
                //{
                //    errMessage += "受注番号には正の整数を入力して下さい。" + Environment.NewLine;
                //}

                //if (ExCast.zCLng(_entityH._estimateno) < 0)
                //{
                //    errMessage += "見積番号には正の整数を入力して下さい。" + Environment.NewLine;
                //    if (errCtl == null) errCtl = this.utlEstimateNo.txtID;
                //}

                #endregion

                #region 範囲チェック

                if (!string.IsNullOrEmpty(this.utlID_F.txtID.Text.Trim()) && !string.IsNullOrEmpty(this.utlID_T.txtID.Text.Trim()) &&
                    ExCast.IsNumeric(this.utlID_F.txtID.Text.Trim()) && ExCast.IsNumeric(this.utlID_F.txtID.Text.Trim()))
                {
                    if (ExCast.zCDbl(this.utlID_F.txtID.Text.Trim()) > ExCast.zCLng(this.utlID_T.txtID.Text.Trim()))
                    {
                        errMessage += this.lblID.Content + "の範囲指定が不正です。" + Environment.NewLine;
                        if (errCtl == null) errCtl = this.utlID_F.txtID;
                    }
                }

                //if (ExCast.zCLng(_entityH._no) > 999999999999999)
                //{
                //    errMessage += "受注番号には15桁以内の正の整数を入力して下さい。" + Environment.NewLine;
                //}

                //if (ExCast.zCLng(_entityH._estimateno) > 999999999999999)
                //{
                //    errMessage += "見積番号には15桁以内の正の整数を入力して下さい。" + Environment.NewLine;
                //    if (errCtl == null) errCtl = this.utlEstimateNo.txtID;
                //}

                //if (ExString.LenB(_entityH._memo) > 32)
                //{
                //    errMessage += "摘要には全角16桁文字以内(半角32桁文字以内)を入力して下さい。" + Environment.NewLine;
                //    if (errCtl == null) errCtl = this.txtMemo;
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

                this.txtDummy.IsTabStop = false;

                if (flg == false)
                {
                    if (errCtl != null)
                    {
                        ExBackgroundWorker.DoWork_Focus(errCtl, 10);
                    }
                    if (this.utlParentFKey != null) this.utlParentFKey.IsEnabled = true;
                    return;
                }

                #endregion

                ReportStart();
            }
            catch
            {
                if (this.utlParentFKey != null) this.utlParentFKey.IsEnabled = true;
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
