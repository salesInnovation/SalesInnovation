#region using

using System;
using System.Collections.Generic;
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
using System.Windows.Browser;
using System.Collections.ObjectModel;
using SlvHanbaiClient.svcDuties;
using SlvHanbaiClient.Class;
using SlvHanbaiClient.Class.UI;
using SlvHanbaiClient.Class.Data;
using SlvHanbaiClient.Class.Utility;
using SlvHanbaiClient.Class.WebService;
using SlvHanbaiClient.View.Dlg;
using SlvHanbaiClient.View.Dlg.Support;
using SlvHanbaiClient.View.Dlg.Duties;
using SlvHanbaiClient.View.Dlg.SystemInf;

#endregion

namespace SlvHanbaiClient.View.UserControl
{
    public partial class Utl_Menu : ExUserControl
    {

        #region Filed Const

        private const String CLASS_NM = "Utl_Menu";
        public Page naviPage = null;
        //UA_Main pgMainPage = new UA_Main();

        private double before_height = 0;

        #endregion

        #region Constructor

        public Utl_Menu()
        {
            InitializeComponent();
        }

        #endregion

        #region Page Events

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            txtConfirmBusiness.Background = new SolidColorBrush(Colors.White);
            txtConfirmSystem.Background = new SolidColorBrush(Colors.White);
            GetDutiesList();
            GetSystemInfList();
        }


        #endregion

        #region メニュー表示制御

        public void DisplayChange()
        {
            GetDutiesList();
            GetSystemInfList();

            // VisualTree確定後の状態を取得する為、tabMainMenu_SizeChangedイベントを起こさせる
            this.before_height = this.tabMainMenu.Height;
            this.tabMainMenu.Height = this.tabMainMenu.Height + 1;
        }

        private void DisplayChangeExec()
        {
            // 権限によるメニュー表示制御
            if (Common.gAuthorityList != null)
            {
                foreach (DependencyObject ob in ExVisualTreeHelper.FindVisualChildrenNoType(this.stpSetting))
                {
                    SettingVisibleForAuthority(ob);
                }

                foreach (DependencyObject ob in ExVisualTreeHelper.FindVisualChildrenNoType(this.stpSales))
                {
                    SettingVisibleForAuthority(ob);
                }

                foreach (DependencyObject ob in ExVisualTreeHelper.FindVisualChildrenNoType(this.stpSalesReport))
                {
                    SettingVisibleForAuthority(ob);
                }

                foreach (DependencyObject ob in ExVisualTreeHelper.FindVisualChildrenNoType(this.stpPurchase))
                {
                    SettingVisibleForAuthority(ob);
                }

                foreach (DependencyObject ob in ExVisualTreeHelper.FindVisualChildrenNoType(this.stpInventory))
                {
                    SettingVisibleForAuthority(ob);
                }

                foreach (DependencyObject ob in ExVisualTreeHelper.FindVisualChildrenNoType(this.stpPurchaseReport))
                {
                    SettingVisibleForAuthority(ob);
                }

                foreach (DependencyObject ob in ExVisualTreeHelper.FindVisualChildrenNoType(this.stpSupport))
                {
                    SettingVisibleForAuthority(ob);
                }
            }
        }

        private void SettingVisibleForAuthority(DependencyObject ob)
        {
            Button btn = null;
            TextBlock tb = null;

            if (ob is Button)
            {
                try
                {
                    btn = (Button)ob;
                }
                catch
                {
                }

                if (btn != null)
                {
                    if (!string.IsNullOrEmpty(ExCast.zCStr(btn.Tag)))
                    {
                        for (int i = 0; i <= Common.gAuthorityList.Count - 1; i++)
                        {                            
                            if (Common.gAuthorityList[i]._pg_id == ExCast.zCStr(btn.Tag))
                            {
                                if (Common.gAuthorityList[i]._authority_kbn <= 1)
                                {
                                    btn.Visibility = System.Windows.Visibility.Collapsed;
                                }
                                else
                                {
                                    btn.Visibility = System.Windows.Visibility.Visible;
                                }
                            }
                        }
                    }
                }
            }

            if (ob is TextBlock)
            {
                try
                {
                    tb = (TextBlock)ob;
                }
                catch
                {
                }

                if (tb != null)
                {
                    if (!string.IsNullOrEmpty(ExCast.zCStr(tb.Tag)))
                    {
                        for (int i = 0; i <= Common.gAuthorityList.Count - 1; i++)
                        {
                            if (Common.gAuthorityList[i]._pg_id == ExCast.zCStr(tb.Tag))
                            {
                                if (Common.gAuthorityList[i]._authority_kbn <= 1)
                                {
                                    tb.Visibility = System.Windows.Visibility.Collapsed;
                                }
                                else
                                {
                                    tb.Visibility = System.Windows.Visibility.Visible;
                                }
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region Method

        private void searchDlg_Closed(object sender, EventArgs e)
        {
            if (sender is Dlg_Login)
            {
                UA_Main main = (UA_Main)ExVisualTreeHelper.FindPerentPage(this);
                if (main != null) main.SetHeaderInf();
            }

            if (Common.gWinMsterType == Common.geWinMsterType.Authority && Common.gWinGroupType == Common.geWinGroupType.InpMaster)
            {
                DisplayChange();
            }

            if (sender is Dlg_DutiesHistory)
            {
                GetDutiesList();
            }
        }

        #endregion

        #region Button Event

        private void btnMenu_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            ExChildWindow _win = null;
            UA_Main pg = null;

            switch (btn.Tag.ToString())
            {
                #region 全般設定

                case "CompanyMst":
                    Common.gWinGroupType = Common.geWinGroupType.InpMaster;
                    Common.gWinMsterType = Common.geWinMsterType.Company;
                    _win = new Dlg_InpMaster();
                    break;
                case "CompanyGroupMst":
                    Common.gWinGroupType = Common.geWinGroupType.InpMaster;
                    Common.gWinMsterType = Common.geWinMsterType.CompanyGroup;
                    _win = new Dlg_InpMaster();
                    break;
                case "UserMst":
                    Common.gWinGroupType = Common.geWinGroupType.InpMaster;
                    Common.gWinMsterType = Common.geWinMsterType.User;
                    _win = new Dlg_InpMaster();
                    break;
                case "AuthorityMst":
                    Common.gWinGroupType = Common.geWinGroupType.InpMaster;
                    Common.gWinMsterType = Common.geWinMsterType.Authority;
                    _win = new Dlg_InpMaster();
                    break;
                case "PersonMst":
                    Common.gWinGroupType = Common.geWinGroupType.InpMaster;
                    Common.gWinMsterType = Common.geWinMsterType.Person;
                    _win = new Dlg_InpMaster();
                    break;
                case "ClassMst":
                    Common.gWinGroupType = Common.geWinGroupType.InpMasterDetail;
                    Common.gWinMsterType = Common.geWinMsterType.Class;
                    _win = new Dlg_InpMaster();
                    break;
                case "CommodityMst":
                    Common.gWinGroupType = Common.geWinGroupType.InpMaster;
                    Common.gWinMsterType = Common.geWinMsterType.Commodity;
                    _win = new Dlg_InpMaster();
                    break;
                case "ConditionMst":
                    Common.gWinGroupType = Common.geWinGroupType.InpMasterDetail;
                    Common.gWinMsterType = Common.geWinMsterType.Condition;
                    _win = new Dlg_InpMaster();
                    break;
                case "Duties":
                    _win = new Dlg_DutiesHistory();
                    //ChildWindow child = new ChildWindow1();
                    _win.Show();
                    break;

                #endregion

                #region 売上系

                case "EstimateInp":
                    pg = (UA_Main)ExVisualTreeHelper.FindPerentPage(this.Parent);
                    Common.gPageGroupType = Common.gePageGroupType.Inp;
                    Common.gPageType = Common.gePageType.InpEstimate;
                    pg.ChangePage();
                    break;
                case "OrderInp":
                    pg = (UA_Main)ExVisualTreeHelper.FindPerentPage(this.Parent);
                    Common.gPageGroupType = Common.gePageGroupType.Inp;
                    Common.gPageType = Common.gePageType.InpOrder;
                    pg.ChangePage();
                    break;
                case "SalesInp":
                    pg = (UA_Main)ExVisualTreeHelper.FindPerentPage(this.Parent);
                    Common.gPageGroupType = Common.gePageGroupType.Inp;
                    Common.gPageType = Common.gePageType.InpSales;
                    pg.ChangePage();
                    break;
                case "InvoiceClose":
                    pg = (UA_Main)ExVisualTreeHelper.FindPerentPage(this.Parent);
                    Common.gPageGroupType = Common.gePageGroupType.Inp;
                    Common.gPageType = Common.gePageType.InpInvoiceClose;
                    pg.ChangePage();
                    break;
                case "InvoicePrint":
                    Common.gWinGroupType = Common.geWinGroupType.InpListReport;
                    Common.gWinMsterType = Common.geWinMsterType.None;
                    Common.gWinType = Common.geWinType.ListInvoice;
                    _win = new Dlg_InpSearch();
                    break;
                case "ReceiptInp":
                    pg = (UA_Main)ExVisualTreeHelper.FindPerentPage(this.Parent);
                    Common.gPageGroupType = Common.gePageGroupType.Inp;
                    Common.gPageType = Common.gePageType.InpReceipt;
                    pg.ChangePage();
                    break;
                case "CustomerMst":
                    Common.gWinGroupType = Common.geWinGroupType.InpMaster;
                    Common.gWinMsterType = Common.geWinMsterType.Customer;
                    _win = new Dlg_InpMaster();
                    break;
                case "SupplierMst":
                    Common.gWinGroupType = Common.geWinGroupType.InpMaster;
                    Common.gWinMsterType = Common.geWinMsterType.Supplier;
                    _win = new Dlg_InpMaster();
                    break;
                case "SalesBalance":
                    Common.gWinGroupType = Common.geWinGroupType.InpListUpd;
                    Common.gWinMsterType = Common.geWinMsterType.None;
                    Common.gWinType = Common.geWinType.ListSalesCreditBalance;
                    _win = new Dlg_InpSearch();
                    break;
                case "InvoiceBalance":
                    Common.gWinGroupType = Common.geWinGroupType.InpListUpd;
                    Common.gWinMsterType = Common.geWinMsterType.None;
                    Common.gWinType = Common.geWinType.ListInvoiceBalance;
                    _win = new Dlg_InpSearch();
                    break;

                #endregion

                #region 仕入系

                case "PurchaseOrderInp":
                    pg = (UA_Main)ExVisualTreeHelper.FindPerentPage(this.Parent);
                    Common.gPageGroupType = Common.gePageGroupType.Inp;
                    Common.gPageType = Common.gePageType.InpPurchaseOrder;
                    pg.ChangePage();
                    break;

                case "PurchaseInp":
                    pg = (UA_Main)ExVisualTreeHelper.FindPerentPage(this.Parent);
                    Common.gPageGroupType = Common.gePageGroupType.Inp;
                    Common.gPageType = Common.gePageType.InpPurchase;
                    pg.ChangePage();
                    break;

                case "PaymentClose":
                    pg = (UA_Main)ExVisualTreeHelper.FindPerentPage(this.Parent);
                    Common.gPageGroupType = Common.gePageGroupType.Inp;
                    Common.gPageType = Common.gePageType.InpPaymentClose;
                    pg.ChangePage();
                    break;

                case "PaymentPrint":
                    Common.gWinGroupType = Common.geWinGroupType.InpListReport;
                    Common.gWinMsterType = Common.geWinMsterType.None;
                    Common.gWinType = Common.geWinType.ListPayment;
                    _win = new Dlg_InpSearch();
                    break;

                case "PaymentCashInp":
                    pg = (UA_Main)ExVisualTreeHelper.FindPerentPage(this.Parent);
                    Common.gPageGroupType = Common.gePageGroupType.Inp;
                    Common.gPageType = Common.gePageType.InpPaymentCash;
                    pg.ChangePage();
                    break;

                case "PurchaseMst":
                    Common.gWinGroupType = Common.geWinGroupType.InpMaster;
                    Common.gWinMsterType = Common.geWinMsterType.Purchase;
                    _win = new Dlg_InpMaster();
                    break;

                case "PaymentCreditBalance":
                    Common.gWinGroupType = Common.geWinGroupType.InpListUpd;
                    Common.gWinMsterType = Common.geWinMsterType.None;
                    Common.gWinType = Common.geWinType.ListPaymentCreditBalance;
                    _win = new Dlg_InpSearch();
                    break;

                case "PaymentBalance":
                    Common.gWinGroupType = Common.geWinGroupType.InpListUpd;
                    Common.gWinMsterType = Common.geWinMsterType.None;
                    Common.gWinType = Common.geWinType.ListPaymentBalance;
                    _win = new Dlg_InpSearch();
                    break;

                #endregion

                #region 入出庫系

                case "InOutDeliveryInp":
                    pg = (UA_Main)ExVisualTreeHelper.FindPerentPage(this.Parent);
                    Common.gPageGroupType = Common.gePageGroupType.Inp;
                    Common.gPageType = Common.gePageType.InpInOutDelivery;
                    pg.ChangePage();
                    break;

                case "StockInventoryInp":
                    pg = (UA_Main)ExVisualTreeHelper.FindPerentPage(this.Parent);
                    Common.gPageGroupType = Common.gePageGroupType.Inp;
                    Common.gPageType = Common.gePageType.InpStockInventory;
                    pg.ChangePage();
                    break;

                #endregion

                #region 売上レポート系

                case "EstimateDPrint":
                    Common.gWinGroupType = Common.geWinGroupType.InpDetailReport;
                    Common.gWinMsterType = Common.geWinMsterType.None;
                    Common.gWinType = Common.geWinType.ListEstimat;
                    _win = Common.InpSearchEstimate;
                    break;
                case "OrderDPrint":
                    Common.gWinGroupType = Common.geWinGroupType.InpDetailReport;
                    Common.gWinMsterType = Common.geWinMsterType.None;
                    Common.gWinType = Common.geWinType.ListOrder;
                    _win = Common.InpSearchOrder;
                    break;
                case "SalesDPrint":
                    Common.gWinGroupType = Common.geWinGroupType.InpDetailReport;
                    Common.gWinMsterType = Common.geWinMsterType.None;
                    Common.gWinType = Common.geWinType.ListSales;
                    _win = Common.InpSearchSales;
                    break;
                case "SalesDayPrint":
                    Common.gWinGroupType = Common.geWinGroupType.InpDetailReport;
                    Common.gWinMsterType = Common.geWinMsterType.None;
                    Common.gWinType = Common.geWinType.ListSalesDay;
                    _win = Common.InpSearchSales;
                    break;
                case "SalesMonthPrint":
                    Common.gWinGroupType = Common.geWinGroupType.InpDetailReport;
                    Common.gWinMsterType = Common.geWinMsterType.None;
                    Common.gWinType = Common.geWinType.ListSalesMonth;
                    _win = Common.InpSearchSales;
                    break;
                case "SalesChangePrint":
                    Common.gWinGroupType = Common.geWinGroupType.InpDetailReport;
                    Common.gWinMsterType = Common.geWinMsterType.None;
                    Common.gWinType = Common.geWinType.ListSalesChange;
                    _win = Common.InpSearchSales;
                    break;
                case "ReceiptDPrint":
                    Common.gWinGroupType = Common.geWinGroupType.InpDetailReport;
                    Common.gWinMsterType = Common.geWinMsterType.None;
                    Common.gWinType = Common.geWinType.ListReceipt;
                    _win = Common.InpSearchReceipt;
                    break;
                case "ReceiptDayPrint":
                    Common.gWinGroupType = Common.geWinGroupType.InpDetailReport;
                    Common.gWinMsterType = Common.geWinMsterType.None;
                    Common.gWinType = Common.geWinType.ListReceiptDay;
                    _win = Common.InpSearchReceipt;
                    break;
                case "ReceiptMonthPrint":
                    Common.gWinGroupType = Common.geWinGroupType.InpDetailReport;
                    Common.gWinMsterType = Common.geWinMsterType.None;
                    Common.gWinType = Common.geWinType.ListReceiptMonth;
                    _win = Common.InpSearchReceipt;
                    break;
                case "CollectPlanPrint":
                    Common.gWinGroupType = Common.geWinGroupType.InpDetailReport;
                    Common.gWinMsterType = Common.geWinMsterType.None;
                    Common.gWinType = Common.geWinType.ListCollectPlan;
                    _win = Common.InpSearchPlan;
                    break;
                case "ReceiptPlanPrint":
                    //Common.gWinGroupType = Common.geWinGroupType.InpMaster;
                    //Common.gWinMsterType = Common.geWinMsterType.Person;
                    //_win = new Dlg_InpMaster();
                    break;
                case "SalesBalancePrint":
                    Common.gWinGroupType = Common.geWinGroupType.InpListReport;
                    Common.gWinMsterType = Common.geWinMsterType.None;
                    Common.gWinType = Common.geWinType.ListSalesCreditBalance;
                    _win = new Dlg_InpSearch();
                    break;
                case "InvoiceBalancePrint":
                    Common.gWinGroupType = Common.geWinGroupType.InpListReport;
                    Common.gWinMsterType = Common.geWinMsterType.None;
                    Common.gWinType = Common.geWinType.ListInvoiceBalance;
                    _win = new Dlg_InpSearch();
                    break;

                #endregion

                #region 在庫/仕入レポート系

                case "PurchaseOrderDPrint":
                    Common.gWinGroupType = Common.geWinGroupType.InpDetailReport;
                    Common.gWinMsterType = Common.geWinMsterType.None;
                    Common.gWinType = Common.geWinType.ListPurchaseOrder;
                    _win = Common.InpSearchPurchaseOrder;
                    break;

                case "PurchaseDPrint":
                    Common.gWinGroupType = Common.geWinGroupType.InpDetailReport;
                    Common.gWinMsterType = Common.geWinMsterType.None;
                    Common.gWinType = Common.geWinType.ListPurchase;
                    _win = Common.InpSearchPurchase;
                    break;

                case "PurchaseDayPrint":
                    Common.gWinGroupType = Common.geWinGroupType.InpDetailReport;
                    Common.gWinMsterType = Common.geWinMsterType.None;
                    Common.gWinType = Common.geWinType.ListPurchaseDay;
                    _win = Common.InpSearchPurchase;
                    break;

                case "PurchaseMonthPrint":
                    Common.gWinGroupType = Common.geWinGroupType.InpDetailReport;
                    Common.gWinMsterType = Common.geWinMsterType.None;
                    Common.gWinType = Common.geWinType.ListPurchaseMonth;
                    _win = Common.InpSearchPurchase;
                    break;

                case "PurchaseChangePrint":
                    Common.gWinGroupType = Common.geWinGroupType.InpDetailReport;
                    Common.gWinMsterType = Common.geWinMsterType.None;
                    Common.gWinType = Common.geWinType.ListPurchaseChange;
                    _win = Common.InpSearchPurchase;
                    break;

                case "PaymentCashDPrint":
                    Common.gWinGroupType = Common.geWinGroupType.InpDetailReport;
                    Common.gWinMsterType = Common.geWinMsterType.None;
                    Common.gWinType = Common.geWinType.ListPaymentCashDay;
                    _win = Common.InpSearchPaymentCash;
                    break;

                case "PaymentCashDayPrint":
                    Common.gWinGroupType = Common.geWinGroupType.InpDetailReport;
                    Common.gWinMsterType = Common.geWinMsterType.None;
                    Common.gWinType = Common.geWinType.ListPaymentCashDay;
                    _win = Common.InpSearchPaymentCash;
                    break;

                case "PaymentCashMonthPrint":
                    Common.gWinGroupType = Common.geWinGroupType.InpDetailReport;
                    Common.gWinMsterType = Common.geWinMsterType.None;
                    Common.gWinType = Common.geWinType.ListPaymentCashMonth;
                    _win = Common.InpSearchPaymentCash;
                    break;

                case "PaymentPlanPrint":
                    Common.gWinGroupType = Common.geWinGroupType.InpDetailReport;
                    Common.gWinMsterType = Common.geWinMsterType.None;
                    Common.gWinType = Common.geWinType.ListPaymentPlan;
                    _win = Common.InpSearchPlan;
                    break;

                case "InOutDeliverDPrint":
                    Common.gWinGroupType = Common.geWinGroupType.InpDetailReport;
                    Common.gWinMsterType = Common.geWinMsterType.None;
                    Common.gWinType = Common.geWinType.ListInOutDelivery;
                    _win = Common.InpSearchInOutDelivery;
                    break;

                case "InventoryPrint":
                    Common.gWinGroupType = Common.geWinGroupType.InpListReport;
                    Common.gWinMsterType = Common.geWinMsterType.None;
                    Common.gWinType = Common.geWinType.ListInventory;
                    _win = Common.InpSearchInventory;
                    break;

                case "PaymentCreditBalancePrint":
                    Common.gWinGroupType = Common.geWinGroupType.InpListReport;
                    Common.gWinMsterType = Common.geWinMsterType.None;
                    Common.gWinType = Common.geWinType.ListPaymentCreditBalance;
                    _win = new Dlg_InpSearch();
                    break;

                case "PaymentBalancePrint":
                    Common.gWinGroupType = Common.geWinGroupType.InpListReport;
                    Common.gWinMsterType = Common.geWinMsterType.None;
                    Common.gWinType = Common.geWinType.ListPaymentBalance;
                    _win = new Dlg_InpSearch();
                    break;

                #endregion

                #region サポート系

                case "InquiryInp":
                    _win = new Dlg_SupportHistory();
                    break;

                case "Mamual":
                    ExHyperlinkButton _link = new ExHyperlinkButton(Common.gstrManualUrl);
                    _link.ClickMe();
                    return;

                #endregion

                default:
                    break;
            }

            if (_win != null)
            {
                _win.Closed -= searchDlg_Closed;
                _win.Closed += searchDlg_Closed;
                _win.Show();
            }

        }

        private void btnConfirmBusiness_Click(object sender, RoutedEventArgs e)
        {
            ExChildWindow _win = new Dlg_DutiesList();
            _win.Closed -= searchDlg_Closed;
            _win.Closed += searchDlg_Closed;
            _win.Show();
        }

        private void btnConfirmSystem_Click(object sender, RoutedEventArgs e)
        {
            ExChildWindow _win = new Dlg_SystemInfList();
            _win.Closed -= searchDlg_Closed;
            _win.Closed += searchDlg_Closed;
            _win.Show();
        }

        #endregion

        #region TabMenu Events

        private void tabMainMenu_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this.tabMainMenu.Height == this.before_height)
            {
                DisplayChangeExec();
            }
            else
            {
                this.tabMainMenu.Height = this.before_height;
            }
        }

        private void tabItem7_SizeChanged(object sender, SizeChangedEventArgs e)
        {
        }

        #endregion

        #region Data Select

        #region 一覧データ取得

        private void GetDutiesList()
        {
            if (Common.gblnLogin == false) return;

            object[] prm = new object[2];
            prm[0] = 0;
            prm[1] = 1;
            webService.objPerent = this;
            webService.CallWebService(ExWebService.geWebServiceCallKbn.GetDuties,
                                      ExWebService.geDialogDisplayFlg.No,
                                      ExWebService.geDialogCloseFlg.No,
                                      prm);
        }

        private void GetSystemInfList()
        {
            if (Common.gblnLogin == false) return;

            object[] prm = new object[2];
            prm[0] = 0;
            prm[1] = 1;
            webService.objPerent = this;
            webService.CallWebService(ExWebService.geWebServiceCallKbn.GetSystemInf,
                                      ExWebService.geDialogDisplayFlg.No,
                                      ExWebService.geDialogCloseFlg.No,
                                      prm);
        }

        #endregion

        #region 一覧データ取得コールバック呼出(ExWebServiceクラスより呼出)

        public override void DataSelect(int intKbn, object objList)
        {
            if (objList == null) return;

            switch ((ExWebService.geWebServiceCallKbn)intKbn)
            { 
                case ExWebService.geWebServiceCallKbn.GetDuties:
                    ObservableCollection<svcDuties.EntityDuties> _entityDuties = (ObservableCollection<svcDuties.EntityDuties>)objList;
                    if (_entityDuties.Count > 0)
                    {
                        this.txtConfirmBusiness.Text = "";
                        this.txtConfirmBusiness.Text += "[" + _entityDuties[0]._duties_date_time + "]" + Environment.NewLine;
                        this.txtConfirmBusiness.Text += "[連絡者：" + _entityDuties[0]._person_nm + "]" + Environment.NewLine;
                        this.txtConfirmBusiness.Text += "[件名：" + _entityDuties[0]._title + "]" + Environment.NewLine;
                        this.txtConfirmBusiness.Text += "-----------------------------------------------" + Environment.NewLine;
                        this.txtConfirmBusiness.Text += _entityDuties[0]._content + Environment.NewLine;

                        if (_entityDuties[0]._duties_level_id == 1)
                        {
                            tbkConfirmBusiness.Text = "重要度 " + _entityDuties[0]._duties_level_nm;
                            //tbkConfirmBusiness.Foreground = new SolidColorBrush(Colors.Red);
                            tbkConfirmBusiness.Foreground = new SolidColorBrush(Colors.Black);
                        }
                        else
                        {
                            tbkConfirmBusiness.Text = "";
                            tbkConfirmBusiness.Foreground = new SolidColorBrush(Colors.Black);
                        }
                    }
                    break;
                case ExWebService.geWebServiceCallKbn.GetSystemInf:
                    ObservableCollection<svcSystemInf.EntityDuties> _entityDuties2 = (ObservableCollection<svcSystemInf.EntityDuties>)objList;
                    if (_entityDuties2.Count > 0)
                    {
                        this.txtConfirmSystem.Text = "";
                        this.txtConfirmSystem.Text += "[" + _entityDuties2[0]._duties_date_time + "]" + Environment.NewLine;
                        this.txtConfirmSystem.Text += "[件名：" + _entityDuties2[0]._title + "]" + Environment.NewLine;
                        this.txtConfirmSystem.Text += "-----------------------------------------------" + Environment.NewLine;
                        this.txtConfirmSystem.Text += _entityDuties2[0]._content + Environment.NewLine;

                        if (_entityDuties2[0]._duties_level_id == 1)
                        {
                            tbkConfirmSystem.Text = "重要度 " + _entityDuties2[0]._duties_level_nm;
                            //tbkConfirmSystem.Foreground = new SolidColorBrush(Colors.Red);
                            tbkConfirmSystem.Foreground = new SolidColorBrush(Colors.Black);
                        }
                        else
                        {
                            tbkConfirmSystem.Text = "";
                            tbkConfirmSystem.Foreground = new SolidColorBrush(Colors.Black);
                        }
                    }
                    break;
            }
        }

        #endregion

        #endregion
        
        //Application.Current.Host.Content.IsFullScreen = !Application.Current.Host.Content.IsFullScreen;
    }
}
