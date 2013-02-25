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
using SlvHanbaiClient.svcCustomer;
using SlvHanbaiClient.View.UserControl.Custom;
using SlvHanbaiClient.View.Dlg;

namespace SlvHanbaiClient.View.UserControl.Master
{
    public partial class Utl_MstCustomer : ExUserControl
    {

        #region Filed Const

        private const String CLASS_NM = "Utl_MstCustomer";
        private const String PG_NM = DataPgEvidence.PGName.Mst.Customer;
        private const Common.geWinMsterType _WinMsterType = Common.geWinMsterType.Customer;
        private const ExWebService.geWebServiceCallKbn _GetWebServiceCallKbn = ExWebService.geWebServiceCallKbn.GetCustomer;
        private const ExWebService.geWebServiceCallKbn _UpdWebServiceCallKbn = ExWebService.geWebServiceCallKbn.UpdateCustomer;
        private Class.Data.MstData.geMDataKbn MstKbn = Class.Data.MstData.geMDataKbn.Customer;
        private EntityCustomer _entity = new EntityCustomer();
        private readonly string tableName = "M_CUSTOMER";
        private Control activeControl;
        private Dlg_Copying copyDlg;
        private Common.geWinGroupType beforeWinGroupType;

        #endregion

        #region Constructor

        public Utl_MstCustomer()
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
            this.utlFunctionKey.gFunctionKeyEnable = Utl_FunctionKey.geFunctionKeyEnable.Init;
            this.utlFunctionKey.Init();

            // バインド設定
            SetBinding();

            ExBackgroundWorker.DoWork_FocusForLoad(this.utlID);
        }

        private void ExUserControl_Unloaded(object sender, RoutedEventArgs e)
        {
        }

        #endregion

        #region Function Key Button Events

        // F1ボタン(登録) クリック
        public override void btnF1_Click(object sender, RoutedEventArgs e)
        {
            // ボタン押下時非同期入力チェックON
            Common.gblnBtnDesynchronizeLock = true;

            // 入力確定の為、BackgroundWorker経由で入力チェックを呼出
            ExBackgroundInputCheckWk bk = new ExBackgroundInputCheckWk();
            bk.utl = this;
            this.txtDummy.IsTabStop = true;
            bk.focusCtl = this.txtDummy;
            bk.bw.RunWorkerAsync();
        }

        // F2ボタン(クリア) クリック
        public override void btnF2_Click(object sender, RoutedEventArgs e)
        {
            // 初期化
            _entity = null;
            SetBinding();

            this.utlFunctionKey.gFunctionKeyEnable = Utl_FunctionKey.geFunctionKeyEnable.Init;

            this.utlID.txtID_IsReadOnly = false;
            this.utlID.txtID.Text = "";
            ExBackgroundWorker.DoWork_Focus(this.utlID, 10);

            // ロック解除
            DataPgLock.gLockPg(PG_NM, "", (int)DataPgLock.geLockType.UnLock);
        }

        // F3ボタン(複写) クリック
        public override void btnF3_Click(object sender, RoutedEventArgs e)
        {
            if (_entity == null) return;

            copyDlg = new Dlg_Copying();

            copyDlg.utlCopying.gPageType = Common.gePageType.None;
            copyDlg.utlCopying.gWinMsterType = _WinMsterType;
            copyDlg.utlCopying.utlMstID.MstKbn = this.MstKbn;
            copyDlg.utlCopying.tableName = this.tableName;

            copyDlg.Show();
            copyDlg.Closed += copyDlg_Closed;
        }

        private void copyDlg_Closed(object sender, EventArgs e)
        {
            Dlg_Copying dlg = (Dlg_Copying)sender;
            if (dlg.utlCopying.DialogResult == true)
            {
                // ロック解除
                DataPgLock.gLockPg(PG_NM, _entity._id, (int)DataPgLock.geLockType.UnLock);

                if (dlg.utlCopying.copy_id == "")
                {
                    this.utlFunctionKey.gFunctionKeyEnable = Utl_FunctionKey.geFunctionKeyEnable.Init;
                    this.utlID.txtID_IsReadOnly = false;
                    this.utlID.txtID.Text = "";
                }
                else
                {
                    if (dlg.utlCopying.ExistsData == true)
                    {
                        this.utlFunctionKey.gFunctionKeyEnable = Utl_FunctionKey.geFunctionKeyEnable.Upd;
                    }
                    else
                    {
                        this.utlFunctionKey.gFunctionKeyEnable = Utl_FunctionKey.geFunctionKeyEnable.New;
                    }
                    string _id = dlg.utlCopying.copy_id;
                    if (ExCast.IsNumeric(dlg.utlCopying.copy_id))
                    {
                        _id = ExCast.zCDbl(_id).ToString();
                        _entity._id = _id;
                        this.utlID.txtID.Text = _id;
                        this.utlID.txtID.FormatToID();
                    }
                    else
                    {
                        _entity._id = _id;
                        this.utlID.txtID.Text = _id;
                    }

                    this.utlID.txtID_IsReadOnly = true;
                    ExBackgroundWorker.DoWork_Focus(this.txtName, 10);
                }
            }
        }

        // F4ボタン(削除) クリック
        public override void btnF4_Click(object sender, RoutedEventArgs e)
        {
            // ボタン押下時非同期入力チェックON
            Common.gblnBtnDesynchronizeLock = true;

            InputCheckDelete();
        }
        
        // F5ボタン(参照) クリック
        public override void btnF5_Click(object sender, RoutedEventArgs e)
        {
            if (activeControl == null) return;
            switch (activeControl.Name)
            {
                case "utlZip":
                    Utl_Zip zip = (Utl_Zip)activeControl;
                    zip.ShowList();
                    break;
                case "utlID":
                case "utlInvoice":
                case "utlSummingUp":
                case "utlReceiptDivision":
                case "utlGroup1":
                    Utl_MstText mst = (Utl_MstText)activeControl;
                    mst.ShowList();
                    break;
                case "utlTitle":
                case "utlBusiness":
                case "utlUnitKind":
                case "utlTaxChange":
                case "utlCollectCycle":
                case "utlPriceFractionProc":
                case "utlTaxFractionProc":
                case "utlDisplay":
                    Utl_MeiText mei = (Utl_MeiText)activeControl;
                    mei.ShowList();
                    break;
                default:
                    break;
            }
        }

        // F11ボタン(印刷) クリック
        public override void btnF11_Click(object sender, RoutedEventArgs e)
        {
            Common.report.utlReport.gPageType = Common.gePageType.None;
            Common.report.utlReport.gWinMsterType = _WinMsterType;

            beforeWinGroupType = Common.gWinGroupType;
            Common.gWinGroupType = Common.geWinGroupType.Report;

            Common.report.Closed -= reportDlg_Closed;
            Common.report.Closed += reportDlg_Closed;
            Common.report.Show();
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
            Dlg_InpMaster win = (Dlg_InpMaster)ExVisualTreeHelper.FindPerentChildWindow(this);
            win.Close();
        }

        #endregion

        #region GotFocus Events

        private void txt_GotFocus(object sender, RoutedEventArgs e)
        {
            Control ctl = (Control)sender;
            switch (ctl.Name)
            {
                case "utlID":
                //case "utlZip":
                case "utlTitle":
                case "utlInvoice":
                case "utlBusiness":
                case "utlUnitKind":
                case "utlTaxChange":
                case "utlSummingUp":
                case "utlReceiptDivision":
                case "utlCollectCycle":
                case "utlPriceFractionProc":
                case "utlTaxFractionProc":
                case "utlGroup1":
                case "utlDisplay":
                //case "utlZip.Name":
                    ExVisualTreeHelper.SetFunctionKeyEnabled("F5", true, this);
                    activeControl = ctl;
                    break;
                case "txtName":
                case "txtKana":
                case "txtAdoutName":
                case "txtStationName":
                case "txtPersonName":
                case "txtPostName":
                case "txtTel":
                case "txtFax":
                case "txtMail":
                case "txtCollectDay":
                case "txtBillSite":
                case "txtCreditLimitPrice":
                case "txtSalesLimitPrice":
                case "txtCreditRate":
                case "txtMemo":
                    ExVisualTreeHelper.SetFunctionKeyEnabled("F5", false, this);
                    activeControl = null;
                    break;
                default:
                    ExVisualTreeHelper.SetFunctionKeyEnabled("F5", false, this);
                    activeControl = null;
                    break;
            }
        }

        #endregion

        #region LostFocus Events

        private void utlID_LostFocus(object sender, RoutedEventArgs e)
        {
            if (this.utlID.txtID.Text.Trim() != "")
            {
                string _id = this.utlID.txtID.Text.Trim();
                if (ExCast.IsNumeric(_id))
                {
                    _id = ExCast.zCDbl(_id).ToString();
                }
                GetMstData(ExCast.zNumZeroNothingFormat(this.utlID.txtID.Text.Trim()));
            }
        }

        private void utlInvoice_LostFocus(object sender, RoutedEventArgs e)
        {
            if (this.utlInvoice.txtID.Text.Trim() == "")
            {
                this.utlTaxChange.IsEnabled = true;
                this.utlSummingUp.IsEnabled = true;
                this.utlReceiptDivision.IsEnabled = true;
                this.utlCollectCycle.IsEnabled = true;
                this.txtCollectDay.IsEnabled = true;
                this.txtBillSite.IsEnabled = true;
                this.utlPriceFractionProc.IsEnabled = true;
                this.utlTaxFractionProc.IsEnabled = true;
                this.txtCreditLimitPrice.IsEnabled = true;
                this.txtSalesLimitPrice.IsEnabled = true;
                //ExBackgroundWorker.DoWork_Focus(this.utlTaxChange, 100);
            }
        }

        #endregion

        #region Button Click Events

        private void btnSalesBalance_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.utlInvoice.txtID.Text.Trim()))
            {
                ExMessageBox.Show("請求先を選択して下さい。");
                return;
            }

            if (!string.IsNullOrEmpty(this.utlInvoice.txtID.Text.Trim()) && string.IsNullOrEmpty(this.utlInvoice.txtNm.Text.Trim()))
            {
                ExMessageBox.Show("請求先が適切に選択(入力)されていません。");
                return;
            }

            Dlg_MstSearch searchDlg = new Dlg_MstSearch(MstData.geMDataKbn.SalesBalance);
            searchDlg.MstKbn = Class.Data.MstData.geMDataKbn.SalesBalance;
            searchDlg.MstGroupKbn = Class.Data.MstData.geMGroupKbn.None;
            searchDlg.txtCode.Text = this.utlInvoice.txtID.Text.Trim();
            searchDlg.Show();
        }

        #endregion

        #region Binding Setting

        private void SetBinding()
        {
            if (_entity == null)
            {
                _entity = new EntityCustomer();
            }

            // マスタコントロールPropertyChanged
            _entity.PropertyChanged += this.utlInvoice.MstID_Changed;
            _entity.PropertyChanged += this.utlZip.MstID_Changed;
            _entity.PropertyChanged += this.utlSummingUp.MstID_Changed;
            _entity.PropertyChanged += this.utlReceiptDivision.MstID_Changed;
            _entity.PropertyChanged += this.utlGroup1.MstID_Changed;
            _entity.PropertyChanged += this._PropertyChanged;

            NumberConverter nmConvDecm0 = new NumberConverter();

            #region Bind

            // バインド
            Binding BindingName = new Binding("_name");
            BindingName.Mode = BindingMode.TwoWay;
            BindingName.Source = _entity;
            this.txtName.SetBinding(TextBox.TextProperty, BindingName);

            Binding BindingKana = new Binding("_kana");
            BindingKana.Mode = BindingMode.TwoWay;
            BindingKana.Source = _entity;
            this.txtKana.SetBinding(TextBox.TextProperty, BindingKana);

            Binding BindingAboutName = new Binding("_about_name");
            BindingAboutName.Mode = BindingMode.TwoWay;
            BindingAboutName.Source = _entity;
            this.txtAdoutName.SetBinding(TextBox.TextProperty, BindingAboutName);

            Binding BindingZipCodeFrom = new Binding("_zip_code_from");
            BindingZipCodeFrom.Mode = BindingMode.TwoWay;
            BindingZipCodeFrom.Source = _entity;
            this.utlZip.txtZipNo1.SetBinding(TextBox.TextProperty, BindingZipCodeFrom);

            Binding BindingZipCodeTo = new Binding("_zip_code_to");
            BindingZipCodeTo.Mode = BindingMode.TwoWay;
            BindingZipCodeTo.Source = _entity;
            this.utlZip.txtZipNo2.SetBinding(TextBox.TextProperty, BindingZipCodeTo);

            this.utlZip.is_zip_from_first_flg = true;
            this.utlZip.is_zip_to_first_flg = true;

            Binding BindingAdress1 = new Binding("_adress1");
            BindingAdress1.Mode = BindingMode.TwoWay;
            BindingAdress1.Source = _entity;
            this.utlZip.SetBinding(Utl_Zip.UserControlAdress1Property, BindingAdress1);

            Binding BindingAdress2 = new Binding("_adress2");
            BindingAdress2.Mode = BindingMode.TwoWay;
            BindingAdress2.Source = _entity;
            this.utlZip.SetBinding(Utl_Zip.UserControlAdress2Property, BindingAdress2);

            Binding BindingStationName = new Binding("_station_name");
            BindingStationName.Mode = BindingMode.TwoWay;
            BindingStationName.Source = _entity;
            this.txtStationName.SetBinding(TextBox.TextProperty, BindingStationName);

            Binding BindingPostName = new Binding("_post_name");
            BindingPostName.Mode = BindingMode.TwoWay;
            BindingPostName.Source = _entity;
            this.txtPostName.SetBinding(TextBox.TextProperty, BindingPostName);

            Binding BindingPersonName = new Binding("_person_name");
            BindingPersonName.Mode = BindingMode.TwoWay;
            BindingPersonName.Source = _entity;
            this.txtPersonName.SetBinding(TextBox.TextProperty, BindingPersonName);

            Binding BindingTitleId = new Binding("_title_id");
            BindingTitleId.Mode = BindingMode.TwoWay;
            BindingTitleId.Source = _entity;
            this.utlTitle.txtID.SetBinding(TextBox.TextProperty, BindingTitleId);

            Binding BindingTitleName = new Binding("_title_name");
            BindingTitleName.Mode = BindingMode.TwoWay;
            BindingTitleName.Source = _entity;
            this.utlTitle.txtNm.SetBinding(TextBox.TextProperty, BindingTitleName);

            Binding BindingTel = new Binding("_tel");
            BindingTel.Mode = BindingMode.TwoWay;
            BindingTel.Source = _entity;
            this.txtTel.SetBinding(TextBox.TextProperty, BindingTel);

            Binding BindingFax = new Binding("_fax");
            BindingFax.Mode = BindingMode.TwoWay;
            BindingFax.Source = _entity;
            this.txtFax.SetBinding(TextBox.TextProperty, BindingFax);

            Binding BindingMailAdress = new Binding("_mail_adress");
            BindingMailAdress.Mode = BindingMode.TwoWay;
            BindingMailAdress.Source = _entity;
            this.txtMail.SetBinding(TextBox.TextProperty, BindingMailAdress);

            Binding BindingInvoiceId = new Binding("_invoice_id");
            BindingInvoiceId.Mode = BindingMode.TwoWay;
            BindingInvoiceId.Source = _entity;
            this.utlInvoice.txtID.SetBinding(TextBox.TextProperty, BindingInvoiceId);

            Binding BindingInvoiceName = new Binding("_invoice_nm");
            BindingInvoiceName.Mode = BindingMode.TwoWay;
            BindingInvoiceName.Source = _entity;
            this.utlInvoice.txtNm.SetBinding(TextBox.TextProperty, BindingInvoiceName);

            Binding BindingBusinessDivisionId = new Binding("_business_division_id");
            BindingBusinessDivisionId.Mode = BindingMode.TwoWay;
            BindingBusinessDivisionId.Source = _entity;
            this.utlBusiness.txtID.SetBinding(TextBox.TextProperty, BindingBusinessDivisionId);

            Binding BindingBusinessDivisionName = new Binding("_business_division_nm");
            BindingBusinessDivisionName.Mode = BindingMode.TwoWay;
            BindingBusinessDivisionName.Source = _entity;
            this.utlBusiness.txtNm.SetBinding(TextBox.TextProperty, BindingBusinessDivisionName);

            Binding BindingUnitKindId = new Binding("_unit_kind_id");
            BindingUnitKindId.Mode = BindingMode.TwoWay;
            BindingUnitKindId.Source = _entity;
            this.utlUnitKind.txtID.SetBinding(TextBox.TextProperty, BindingUnitKindId);

            Binding BindingUnitKindName = new Binding("_unit_kind_nm");
            BindingUnitKindName.Mode = BindingMode.TwoWay;
            BindingUnitKindName.Source = _entity;
            this.utlUnitKind.txtNm.SetBinding(TextBox.TextProperty, BindingUnitKindName);

            Binding BindingCreditRate = new Binding("_credit_rate");
            BindingCreditRate.Mode = BindingMode.TwoWay;
            BindingCreditRate.Source = _entity;
            this.txtCreditRate.SetBinding(TextBox.TextProperty, BindingCreditRate);

            Binding BindingTaxChangeId = new Binding("_tax_change_id");
            BindingTaxChangeId.Mode = BindingMode.TwoWay;
            BindingTaxChangeId.Source = _entity;
            this.utlTaxChange.txtID.SetBinding(TextBox.TextProperty, BindingTaxChangeId);

            Binding BindingTaxChangeName = new Binding("_tax_change_nm");
            BindingTaxChangeName.Mode = BindingMode.TwoWay;
            BindingTaxChangeName.Source = _entity;
            this.utlTaxChange.txtNm.SetBinding(TextBox.TextProperty, BindingTaxChangeName);

            Binding BindingSummingUpGroupId = new Binding("_summing_up_group_id");
            BindingSummingUpGroupId.Mode = BindingMode.TwoWay;
            BindingSummingUpGroupId.Source = _entity;
            this.utlSummingUp.txtID.SetBinding(TextBox.TextProperty, BindingSummingUpGroupId);

            Binding BindingSummingUpGroupName = new Binding("_summing_up_group_nm");
            BindingSummingUpGroupName.Mode = BindingMode.TwoWay;
            BindingSummingUpGroupName.Source = _entity;
            this.utlSummingUp.txtNm.SetBinding(TextBox.TextProperty, BindingSummingUpGroupName);

            Binding BindigPriceFractionProcId = new Binding("_price_fraction_proc_id");
            BindigPriceFractionProcId.Mode = BindingMode.TwoWay;
            BindigPriceFractionProcId.Source = _entity;
            this.utlPriceFractionProc.txtID.SetBinding(TextBox.TextProperty, BindigPriceFractionProcId);

            Binding BindigPriceFractionProcName = new Binding("_price_fraction_proc_nm");
            BindigPriceFractionProcName.Mode = BindingMode.TwoWay;
            BindigPriceFractionProcName.Source = _entity;
            this.utlPriceFractionProc.txtNm.SetBinding(TextBox.TextProperty, BindigPriceFractionProcName);

            Binding BindigTaxFractionProcId = new Binding("_tax_fraction_proc_id");
            BindigTaxFractionProcId.Mode = BindingMode.TwoWay;
            BindigTaxFractionProcId.Source = _entity;
            this.utlTaxFractionProc.txtID.SetBinding(TextBox.TextProperty, BindigTaxFractionProcId);

            Binding BindigTaxFractionProcName = new Binding("_tax_fraction_proc_nm");
            BindigTaxFractionProcName.Mode = BindingMode.TwoWay;
            BindigTaxFractionProcName.Source = _entity;
            this.utlTaxFractionProc.txtNm.SetBinding(TextBox.TextProperty, BindigTaxFractionProcName);

            Binding BindigCreditLimitPrice = new Binding("_credit_limit_price");
            BindigCreditLimitPrice.Mode = BindingMode.TwoWay;
            BindigCreditLimitPrice.Source = _entity;
            this.txtCreditLimitPrice.SetBinding(TextBox.TextProperty, BindigCreditLimitPrice);
            this.txtCreditLimitPrice.OnFormatString();

            Binding BindigSalesCreditPrice = new Binding("_sales_credit_price");
            BindigSalesCreditPrice.Mode = BindingMode.TwoWay;
            BindigSalesCreditPrice.Source = _entity;
            this.txtSalesLimitPrice.SetBinding(TextBox.TextProperty, BindigSalesCreditPrice);
            this.txtSalesLimitPrice.OnFormatString();

            Binding BindigReceiptDivisionId = new Binding("_receipt_division_id");
            BindigReceiptDivisionId.Mode = BindingMode.TwoWay;
            BindigReceiptDivisionId.Source = _entity;
            this.utlReceiptDivision.txtID.SetBinding(TextBox.TextProperty, BindigReceiptDivisionId);

            Binding BindigReceiptDivisionName = new Binding("_receipt_division_nm");
            BindigReceiptDivisionName.Mode = BindingMode.TwoWay;
            BindigReceiptDivisionName.Source = _entity;
            this.utlReceiptDivision.txtNm.SetBinding(TextBox.TextProperty, BindigReceiptDivisionName);

            Binding BindigCollectCycleId = new Binding("_collect_cycle_id");
            BindigCollectCycleId.Mode = BindingMode.TwoWay;
            BindigCollectCycleId.Source = _entity;
            this.utlCollectCycle.txtID.SetBinding(TextBox.TextProperty, BindigCollectCycleId);

            Binding BindigCollectCycleName = new Binding("_collect_cycle_nm");
            BindigCollectCycleName.Mode = BindingMode.TwoWay;
            BindigCollectCycleName.Source = _entity;
            this.utlCollectCycle.txtNm.SetBinding(TextBox.TextProperty, BindigCollectCycleName);

            Binding BindigCollectDay = new Binding("_collect_day");
            BindigCollectDay.Mode = BindingMode.TwoWay;
            BindigCollectDay.Source = _entity;
            this.txtCollectDay.SetBinding(TextBox.TextProperty, BindigCollectDay);

            Binding BindigBillSite = new Binding("_bill_site");
            BindigBillSite.Mode = BindingMode.TwoWay;
            BindigBillSite.Source = _entity;
            this.txtBillSite.SetBinding(TextBox.TextProperty, BindigBillSite);

            Binding BindigGroup1Id = new Binding("_group1_id");
            BindigGroup1Id.Mode = BindingMode.TwoWay;
            BindigGroup1Id.Source = _entity;
            this.utlGroup1.txtID.SetBinding(TextBox.TextProperty, BindigGroup1Id);

            Binding BindigGroup1Name = new Binding("_group1_nm");
            BindigGroup1Name.Mode = BindingMode.TwoWay;
            BindigGroup1Name.Source = _entity;
            this.utlGroup1.txtNm.SetBinding(TextBox.TextProperty, BindigGroup1Name);

            Binding BindigDiaplayDivisionId = new Binding("_display_division_id");
            BindigDiaplayDivisionId.Mode = BindingMode.TwoWay;
            BindigDiaplayDivisionId.Source = _entity;
            this.utlDisplay.txtID.SetBinding(TextBox.TextProperty, BindigDiaplayDivisionId);

            Binding BindigDiaplayDivisionNm = new Binding("_display_division_nm");
            BindigDiaplayDivisionNm.Mode = BindingMode.TwoWay;
            BindigDiaplayDivisionNm.Source = _entity;
            this.utlDisplay.txtNm.SetBinding(TextBox.TextProperty, BindigDiaplayDivisionNm);

            Binding BindigMemo = new Binding("_memo");
            BindigMemo.Mode = BindingMode.TwoWay;
            BindigMemo.Source = _entity;
            this.txtMemo.SetBinding(TextBox.TextProperty, BindigMemo);

            #endregion

            this.utlID.txtID.SetZeroToNullString();
            this.utlTitle.txtID.SetZeroToNullString();
            this.utlInvoice.txtID.SetZeroToNullString();
            this.utlBusiness.txtID.SetZeroToNullString();
            this.utlUnitKind.txtID.SetZeroToNullString();
            this.utlTaxFractionProc.txtID.SetZeroToNullString();
            this.utlSummingUp.txtID.SetZeroToNullString();
            this.utlReceiptDivision.txtID.SetZeroToNullString();
            this.utlCollectCycle.txtID.SetZeroToNullString();
            this.utlPriceFractionProc.txtID.SetZeroToNullString();
            this.utlTaxFractionProc.txtID.SetZeroToNullString();
            this.utlGroup1.txtID.SetZeroToNullString();

            this.utlInvoice.txtID.FormatToID();
            this.txtCreditLimitPrice.OnFormatString();
            this.txtSalesLimitPrice.OnFormatString();

            if (ExCast.zCInt(_entity._id) == 0)
            {
                _entity._business_division_id = 1;                              // 取引区分 1:掛売上
                _entity._unit_kind_id = 2;                                      // 単価種類 2:売上単価
                _entity._tax_change_id = (int)Common.geTaxChange.OUT_TAX_SUM;   // 税転換 1:外税/伝票計
                _entity._price_fraction_proc_id = 2;                            // 金額端数処理 2:切り上げ
                _entity._tax_fraction_proc_id = 2;                              // 税端数処理 2:切り上げ
                _entity._display_division_id = 1;
                _entity._credit_rate = 100;
            }
        }

        #endregion

        #region Data Select

        #region データ取得

        private void GetMstData(string id)
        {
            object[] prm = new object[1];
            prm[0] = id;
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
                    // 更新
                    if (objList != null)    
                    {
                        _entity = (EntityCustomer)objList;

                        if (_entity.message != "" && _entity.message != null)
                        {
                            this.utlID.txtID.Text = "";
                            ExBackgroundWorker.DoWork_Focus(this.utlID, 10);
                            return;
                        }
                        else
                        {
                            // バインド反映
                            SetBinding();

                            if (_entity._lock_flg == 0)
                            {
                                this.utlFunctionKey.gFunctionKeyEnable = Utl_FunctionKey.geFunctionKeyEnable.Upd;
                            }
                            else
                            {
                                this.utlFunctionKey.gFunctionKeyEnable = Utl_FunctionKey.geFunctionKeyEnable.Sel;
                            }
                        }
                    }
                    // 新規
                    else
                    {
                        _entity = new EntityCustomer();
                        SetBinding();
                        this.utlFunctionKey.gFunctionKeyEnable = Utl_FunctionKey.geFunctionKeyEnable.New;
                    }
                    this.utlID.txtID_IsReadOnly = true;
                    ExBackgroundWorker.DoWork_Focus(this.txtName, 10);
                    break;
                default:
                    break;
            }
        }

        #endregion

        #endregion

        #region Data Update

        // データ追加・更新・削除
        private void UpdateData(Common.geUpdateType upd)
        {
            object[] prm = new object[4];

            prm[0] = (int)upd;
            if (ExCast.IsNumeric(this.utlID.txtID.Text.Trim()))
            {
                prm[1] = ExCast.zCLng(this.utlID.txtID.Text.Trim());
            }
            else
            {
                prm[1] = this.utlID.txtID.Text.Trim();
            }
            prm[2] = _entity;
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

        // 入力チェック(更新時)
        public override void InputCheckUpdate()
        {
            #region Field

            string errMessage = "";
            string warnMessage = "";
            int _selectIndex = 0;
            int _selectColumn = 0;
            Control errCtl = null;

            #endregion

            try
            {
                #region 必須チェック

                if (this.utlMode.Mode != Utl_FunctionKey.geFunctionKeyEnable.Init)
                {
                    // ID
                    if (string.IsNullOrEmpty(this.utlID.txtID.Text.Trim()))
                    {
                        errMessage += "IDが入力されていません。" + Environment.NewLine;
                        if (errCtl == null) errCtl = this.utlID.txtID;
                    }
                }

                // 名称
                if (string.IsNullOrEmpty(_entity._name))
                {
                    errMessage += "名称が入力されていません。" + Environment.NewLine;
                    if (errCtl == null) errCtl = this.txtName;
                }

                // 請求先
                if (string.IsNullOrEmpty(ExCast.zCStr(_entity._invoice_id)))
                {
                    errMessage += "請求先が入力(選択)されていません。" + Environment.NewLine;
                    if (errCtl == null) errCtl = this.utlInvoice.txtID;
                }

                // 表示区分
                if (string.IsNullOrEmpty(ExCast.zCStr(_entity._display_division_nm)))
                {
                    errMessage += "表示区分が入力(選択)されていません。" + Environment.NewLine;
                    if (errCtl == null) errCtl = this.utlDisplay.txtID;
                }

                // 取引区分
                if (ExCast.zCInt(_entity._business_division_id) == 0)
                {
                    errMessage += "取引区分が入力(選択)されていません。" + Environment.NewLine;
                    if (errCtl == null) errCtl = this.utlBusiness.txtID;
                }

                // 単価種類
                if (ExCast.zCInt(_entity._unit_kind_id) == 0)
                {
                    errMessage += "単価種類が入力(選択)されていません。" + Environment.NewLine;
                    if (errCtl == null) errCtl = this.utlUnitKind.txtID;
                }

                // 税転換
                if (ExCast.zCInt(_entity._tax_change_id) == (int)Common.geTaxChange.None)
                {
                    errMessage += "税転換が入力(選択)されていません。" + Environment.NewLine;
                    if (errCtl == null) errCtl = this.utlTaxChange.txtID;
                }

                // 金額端数処理
                if (ExCast.zCInt(_entity._price_fraction_proc_id) == 0)
                {
                    errMessage += "金額端数処理が入力(選択)されていません。" + Environment.NewLine;
                    if (errCtl == null) errCtl = this.utlPriceFractionProc.txtID;
                }

                // 税端数処理
                if (ExCast.zCInt(_entity._tax_fraction_proc_id) == 0)
                {
                    errMessage += "税端数処理が入力(選択)されていません。" + Environment.NewLine;
                    if (errCtl == null) errCtl = this.utlTaxFractionProc.txtID;
                }

                // 取引区分が掛売上の場合
                if (ExCast.zCInt(_entity._business_division_id) == 1)
                {
                    // 掛率
                    if (ExCast.zCInt(_entity._credit_rate) == 0)
                    {
                        errMessage += "掛率が入力されていません。" + Environment.NewLine;
                        if (errCtl == null) errCtl = this.txtCreditRate;
                    }

                    // 締区分
                    if (ExCast.zCStr(_entity._summing_up_group_id) == "")
                    {
                        errMessage += "締区分が入力(選択)されていません。" + Environment.NewLine;
                        if (errCtl == null) errCtl = this.utlSummingUp.txtID;
                    }

                    // 入金区分
                    if (ExCast.zCStr(_entity._receipt_division_id) == "")
                    {
                        errMessage += "入金区分が入力(選択)されていません。" + Environment.NewLine;
                        if (errCtl == null) errCtl = this.utlReceiptDivision.txtID;
                    }

                    // 回収サイクル
                    if (ExCast.zCInt(_entity._collect_cycle_id) == 0)
                    {
                        errMessage += "回収サイクルが入力(選択)されていません。" + Environment.NewLine;
                        if (errCtl == null) errCtl = this.utlCollectCycle.txtID;
                    }

                    // 回収日
                    if (ExCast.zCInt(_entity._collect_day) == 0)
                    {
                        errMessage += "回収日が入力されていません。" + Environment.NewLine;
                        if (errCtl == null) errCtl = this.txtCollectDay;
                    }
                }

                // 入金区分が手形の場合
                if (ExCast.zCStr(_entity._receipt_division_id) == "401" || ExCast.zCStr(_entity._receipt_division_id) == "402" || ExCast.zCStr(_entity._receipt_division_nm).IndexOf("手形") != -1)
                {
                    // 手形サイト
                    if (ExCast.zCInt(_entity._bill_site) == 0)
                    {
                        errMessage += "手形サイトが入力されていません。" + Environment.NewLine;
                        if (errCtl == null) errCtl = this.txtBillSite;
                    }
                }

                #endregion

                #region 適正値入力チェック

                // 郵便番号上記3桁のみはNG
                if (!string.IsNullOrEmpty(_entity._zip_code_from) && string.IsNullOrEmpty(_entity._zip_code_to))
                {
                    errMessage += "郵便番号が適切に入力(選択)されていません。" + Environment.NewLine;
                    if (errCtl == null) errCtl = this.utlZip.txtZipNo1;
                }

                // 郵便番号下記4桁のみはNG
                if (string.IsNullOrEmpty(_entity._zip_code_from) && !string.IsNullOrEmpty(_entity._zip_code_to))
                {
                    errMessage += "郵便番号が適切に入力(選択)されていません。" + Environment.NewLine;
                    if (errCtl == null) errCtl = this.utlZip.txtZipNo2;
                }

                // 郵便番号のみはNG
                if ((!string.IsNullOrEmpty(_entity._zip_code_from) && !string.IsNullOrEmpty(_entity._zip_code_to)) && (string.IsNullOrEmpty(_entity._adress1)))
                {
                    errMessage += "郵便番号が適切に入力(選択)されていません。" + Environment.NewLine;
                    if (errCtl == null) errCtl = this.utlZip.txtAdress1;
                }

                // 敬称
                if (ExCast.zCInt(_entity._title_id) != 0 && string.IsNullOrEmpty(_entity._title_name))
                {
                    errMessage += "敬称が適切に入力(選択)されていません。" + Environment.NewLine;
                    if (errCtl == null) errCtl = this.utlTitle.txtID;
                }

                if (this.utlFunctionKey.gFunctionKeyEnable == Utl_FunctionKey.geFunctionKeyEnable.New)
                {
                    // 請求先
                    if (ExCast.zNumZeroNothingFormat(_entity._invoice_id) != ExCast.zNumZeroNothingFormat(this.utlID.txtID.Text.Trim()))
                    {
                        if (ExCast.zCStr(_entity._invoice_id) != "" && string.IsNullOrEmpty(_entity._invoice_nm))
                        {
                            errMessage += "請求先が適切に入力(選択)されていません。" + Environment.NewLine;
                            if (errCtl == null) errCtl = this.utlInvoice.txtID;
                        }
                    }
                }
                else
                {
                    // 請求先
                    if (ExCast.zCStr(_entity._invoice_id) != "" && string.IsNullOrEmpty(_entity._invoice_nm))
                    {
                        errMessage += "請求先が適切に入力(選択)されていません。" + Environment.NewLine;
                        if (errCtl == null) errCtl = this.utlInvoice.txtID;
                    }
                }

                // 取引区分
                if (ExCast.zCInt(_entity._business_division_id) != 0 && string.IsNullOrEmpty(_entity._business_division_nm))
                {
                    errMessage += "取引区分が適切に入力(選択)されていません。" + Environment.NewLine;
                    if (errCtl == null) errCtl = this.utlBusiness.txtID;
                }

                // 単価種類
                if (ExCast.zCInt(_entity._unit_kind_id) != 0 && string.IsNullOrEmpty(_entity._unit_kind_nm))
                {
                    errMessage += "単価種類が適切に入力(選択)されていません。" + Environment.NewLine;
                    if (errCtl == null) errCtl = this.utlUnitKind.txtID;
                }

                // 税転換
                if (ExCast.zCInt(_entity._tax_change_id) != (int)Common.geTaxChange.None && string.IsNullOrEmpty(_entity._tax_change_nm))
                {
                    errMessage += "税転換が適切に入力(選択)されていません。" + Environment.NewLine;
                    if (errCtl == null) errCtl = this.utlTaxChange.txtID;
                }

                // 締区分
                if (ExCast.zCStr(_entity._summing_up_group_id) != "" && string.IsNullOrEmpty(_entity._summing_up_group_nm))
                {
                    errMessage += "締区分が適切に入力(選択)されていません。" + Environment.NewLine;
                    if (errCtl == null) errCtl = this.utlSummingUp.txtID;
                }

                // 金額端数処理
                if (ExCast.zCInt(_entity._price_fraction_proc_id) != 0 && string.IsNullOrEmpty(_entity._price_fraction_proc_nm))
                {
                    errMessage += "金額端数処理が適切に入力(選択)されていません。" + Environment.NewLine;
                    if (errCtl == null) errCtl = this.utlPriceFractionProc.txtID;
                }

                // 税端数処理
                if (ExCast.zCInt(_entity._tax_fraction_proc_id) != 0 && string.IsNullOrEmpty(_entity._tax_fraction_proc_nm))
                {
                    errMessage += "税端数処理が適切に入力(選択)されていません。" + Environment.NewLine;
                    if (errCtl == null) errCtl = this.utlTaxFractionProc.txtID;
                }

                // 入金区分
                if (ExCast.zCStr(_entity._receipt_division_id) != "" && string.IsNullOrEmpty(_entity._receipt_division_nm))
                {
                    errMessage += "入金区分が適切に入力(選択)されていません。" + Environment.NewLine;
                    if (errCtl == null) errCtl = this.utlReceiptDivision.txtID;
                }

                // 回収サイクル
                if (ExCast.zCInt(_entity._collect_cycle_id) != 0 && string.IsNullOrEmpty(_entity._collect_cycle_nm))
                {
                    errMessage += "回収サイクルが適切に入力(選択)されていません。" + Environment.NewLine;
                    if (errCtl == null) errCtl = this.utlCollectCycle.txtID;
                }

                // 得意先分類
                if (ExCast.zCStr(_entity._group1_id) != "" && string.IsNullOrEmpty(_entity._group1_nm))
                {
                    errMessage += "得意先分類が適切に入力(選択)されていません。" + Environment.NewLine;
                    if (errCtl == null) errCtl = this.utlGroup1.txtID;
                }

                // 表示区分
                if (ExCast.zCInt(_entity._display_division_id) > 1 && string.IsNullOrEmpty(_entity._display_division_nm))
                {
                    errMessage += "表示区分が適切に入力(選択)されていません。" + Environment.NewLine;
                    if (errCtl == null) errCtl = this.utlDisplay.txtID;
                }

                #endregion

                #region 日付チェック

                //// 納入指定日
                //if (string.IsNullOrEmpty(_entity.supply_ymd) == false)
                //{
                //    if (ExCast.IsDate(_entity.supply_ymd) == false)
                //    {
                //        errMessage += "納入指定日の形式が不正です。(yyyy/mm/dd形式で入力(選択)して下さい)" + Environment.NewLine;
                //        if (errCtl == null) errCtl = this.datNokiYmd;
                //    }
                //}

                #endregion

                #region 日付変換

                // 受注日
                //if (string.IsNullOrEmpty(_entity.order_ymd) == false)
                //{
                //    _entity.order_ymd = ExCast.zConvertToDate(_entity.order_ymd).ToString("yyyy/MM/dd");

                //}

                #endregion

                #region 数値チェック

                //if (this.utlMode.Mode != Utl_FunctionKey.geFunctionKeyEnable.Init)
                //{
                //    if (ExCast.IsNumeric(this.utlID.txtID.Text.Trim()) == false)
                //    {
                //        errMessage += "IDには数値を入力して下さい。" + Environment.NewLine;
                //    }
                //}

                #endregion

                #region 正数チェック

                //if (this.utlMode.Mode != Utl_FunctionKey.geFunctionKeyEnable.Init)
                //{
                //    if (ExCast.zCLng(this.utlID.txtID.Text.Trim()) < 0)
                //    {
                //        errMessage += "IDには正の整数を入力して下さい。" + Environment.NewLine;
                //    }
                //}

                #endregion

                #region 範囲チェック

                //if (ExCast.zCLng(this.utlID.txtID.Text.Trim()) > 9999)
                //{
                //    errMessage += "IDには4桁の正の整数を入力して下さい。" + Environment.NewLine;
                //}

                //if (ExString.LenB(_entity._memo) > 32)
                //{
                //    errMessage += "備考には全角16桁文字以内(半角32桁文字以内)を入力して下さい。" + Environment.NewLine;
                //    if (errCtl == null) errCtl = this.txtMemo;
                //}

                if (ExString.LenB(_entity._title_name) > 10)
                {
                    errMessage += "備考には全角5桁文字以内(半角10桁文字以内)を入力して下さい。" + Environment.NewLine;
                    if (errCtl == null) errCtl = this.utlTitle.txtNm;
                }

                #endregion

                #region エラー or 警告時処理

                bool flg = true;

                if (!string.IsNullOrEmpty(errMessage))
                {
                    ExMessageBox.Show(errMessage, Dlg.MessageBox.MessageBoxIcon.Error);
                    flg = false;
                }

                if (!string.IsNullOrEmpty(warnMessage))
                {
                    warnMessage += "このまま登録を続行してもよろしいですか？" + Environment.NewLine;
                    ExMessageBox.ResultShow(this, errCtl, warnMessage);
                    flg = false;
                    //if (ExMessageBox.ResultShow(warnMessage) == MessageBoxResult.No)
                    //{
                    //    flg = false;
                    //}
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

                #region 更新処理

                switch (this.utlFunctionKey.gFunctionKeyEnable)
                {
                    case Utl_FunctionKey.geFunctionKeyEnable.New:
                    case Utl_FunctionKey.geFunctionKeyEnable.Init:
                        UpdateData(Common.geUpdateType.Insert);
                        break;
                    case Utl_FunctionKey.geFunctionKeyEnable.Upd:
                        UpdateData(Common.geUpdateType.Update);
                        break;
                    default:
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

        public override void ResultMessageBox(MessageBoxResult _MessageBoxResult, Control _errCtl)
        {
            if (_MessageBoxResult == MessageBoxResult.OK)
            {
                #region 更新処理

                switch (this.utlFunctionKey.gFunctionKeyEnable)
                {
                    case Utl_FunctionKey.geFunctionKeyEnable.New:
                    case Utl_FunctionKey.geFunctionKeyEnable.Init:
                        UpdateData(Common.geUpdateType.Insert);
                        break;
                    case Utl_FunctionKey.geFunctionKeyEnable.Upd:
                        UpdateData(Common.geUpdateType.Update);
                        break;
                    default:
                        break;
                }

                #endregion
            }
            else
            {
                if (_errCtl != null)
                {
                    ExBackgroundWorker.DoWork_Focus(_errCtl, 10);
                }
            }
        }

        // 入力チェック(削除時)
        private void InputCheckDelete()
        {
            try
            {
                if (this.utlID.txtID.Text.Trim() == "")
                {
                    ExMessageBox.Show("データが選択されていません。");
                    return;
                }

                if (this._entity == null)
                {
                    ExMessageBox.Show("データが選択されていません。");
                    return;
                }

                if (this._entity._id == "")
                {
                    ExMessageBox.Show("データが選択されていません。");
                    return;
                }

                UpdateData(Common.geUpdateType.Delete);
            }
            finally
            {
                Common.gblnBtnProcLock = false;
                Common.gblnBtnDesynchronizeLock = false;
            }
        }

        #endregion

        #region PropertyChanged

        private void _PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            string _prop = e.PropertyName;
            if (_prop == null) _prop = "";
            if (_prop.Length > 0)
            {
                if (_prop.Substring(0, 1) == "_")
                {
                    _prop = _prop.Substring(1, _prop.Length - 1);
                }
            }

            switch (_prop)
            {
                case "invoice_nm":
                    if (_entity._invoice_id == "")
                    {
                        this.utlTaxChange.IsEnabled = true;
                        this.utlSummingUp.IsEnabled = true;
                        this.utlReceiptDivision.IsEnabled = true;
                        this.utlCollectCycle.IsEnabled = true;
                        this.txtCollectDay.IsEnabled = true;
                        this.txtBillSite.IsEnabled = true;
                        this.utlPriceFractionProc.IsEnabled = true;
                        this.utlTaxFractionProc.IsEnabled = true;
                        this.txtCreditLimitPrice.IsEnabled = true;
                        this.txtSalesLimitPrice.IsEnabled = true;
                        ExBackgroundWorker.DoWork_Focus(this.utlTaxChange, 100);
                        break;
                    }

                    if (_entity._invoice_nm == "")
                    {
                        this.utlTaxChange.IsEnabled = true;
                        this.utlSummingUp.IsEnabled = true;
                        this.utlReceiptDivision.IsEnabled = true;
                        this.utlCollectCycle.IsEnabled = true;
                        this.txtCollectDay.IsEnabled = true;
                        this.txtBillSite.IsEnabled = true;
                        this.utlPriceFractionProc.IsEnabled = true;
                        this.utlTaxFractionProc.IsEnabled = true;
                        this.txtCreditLimitPrice.IsEnabled = true;
                        this.txtSalesLimitPrice.IsEnabled = true;
                        ExBackgroundWorker.DoWork_Focus(this.utlTaxChange, 100);
                        break;
                    }

                    if (ExCast.zNumZeroNothingFormat(_entity._id) != ExCast.zNumZeroNothingFormat(_entity._invoice_id))
                    {
                        if (this.utlInvoice.MasterData != null)
                        {
                            this.utlTaxChange.txtID.Text = ExCast.zCStr(this.utlInvoice.MasterData.attribute2);
                            this.utlSummingUp.txtID.Text = ExCast.zCStr(this.utlInvoice.MasterData.attribute10);
                            this.utlReceiptDivision.txtID.Text = ExCast.zCStr(this.utlInvoice.MasterData.attribute11);
                            this.utlCollectCycle.txtID.Text = ExCast.zCStr(this.utlInvoice.MasterData.attribute12);
                            this.txtCollectDay.Text = ExCast.zCStr(this.utlInvoice.MasterData.attribute13);
                            this.txtBillSite.Text = ExCast.zCStr(this.utlInvoice.MasterData.attribute14);
                            this.utlPriceFractionProc.txtID.Text = ExCast.zCStr(this.utlInvoice.MasterData.attribute15);
                            this.utlTaxFractionProc.txtID.Text = ExCast.zCStr(this.utlInvoice.MasterData.attribute16);
                            this.txtCreditLimitPrice.Text = ExCast.zCStr(this.utlInvoice.MasterData.attribute6);
                            this.txtSalesLimitPrice.Text = ExCast.zCStr(this.utlInvoice.MasterData.attribute7);
                        }

                        this.txtCreditLimitPrice.OnFormatString();
                        this.txtSalesLimitPrice.OnFormatString();

                        this.utlTaxChange.IsEnabled = false;
                        this.utlSummingUp.IsEnabled = false;
                        this.utlReceiptDivision.IsEnabled = false;
                        this.utlCollectCycle.IsEnabled = false;
                        this.txtCollectDay.IsEnabled = false;
                        this.txtBillSite.IsEnabled = false;
                        this.utlPriceFractionProc.IsEnabled = false;
                        this.utlTaxFractionProc.IsEnabled = false;
                        this.txtCreditLimitPrice.IsEnabled = false;
                        this.txtSalesLimitPrice.IsEnabled = false;
                        ExBackgroundWorker.DoWork_Focus(this.utlGroup1, 100);
                    }
                    else
                    {
                        this.utlTaxChange.IsEnabled = true;
                        this.utlSummingUp.IsEnabled = true;
                        this.utlReceiptDivision.IsEnabled = true;
                        this.utlCollectCycle.IsEnabled = true;
                        this.txtCollectDay.IsEnabled = true;
                        this.txtBillSite.IsEnabled = true;
                        this.utlPriceFractionProc.IsEnabled = true;
                        this.utlTaxFractionProc.IsEnabled = true;
                        this.txtCreditLimitPrice.IsEnabled = true;
                        this.txtSalesLimitPrice.IsEnabled = true;
                        ExBackgroundWorker.DoWork_Focus(this.utlTaxChange, 100);
                    }
                    break;
                default:
                    break;
            }
        }

        #endregion

    }

}
