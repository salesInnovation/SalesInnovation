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
using SlvHanbaiClient.svcCompany;
using SlvHanbaiClient.svcCompanyGroup;
using SlvHanbaiClient.View.UserControl.Custom;
using SlvHanbaiClient.View.Dlg;
using SlvHanbaiClient.View.UserControl.Report;

namespace SlvHanbaiClient.View.UserControl.Master
{
    public partial class Utl_MstCompanyGroup : ExUserControl
    {

        #region Filed Const

        private const String CLASS_NM = "Utl_MstCompanyGroup";
        private const String PG_NM = DataPgEvidence.PGName.Mst.CompanyGroup;
        private const Common.geWinMsterType _WinMsterType = Common.geWinMsterType.CompanyGroup;
        private const ExWebService.geWebServiceCallKbn _GetWebServiceCallKbn = ExWebService.geWebServiceCallKbn.GetCompanyGroup;
        private const ExWebService.geWebServiceCallKbn _UpdWebServiceCallKbn = ExWebService.geWebServiceCallKbn.UpdateCompanyGroup;
        private Class.Data.MstData.geMDataKbn MstKbn = Class.Data.MstData.geMDataKbn.CompanyGroup;
        private EntityCompanyGroup _entity = null;
        private readonly string tableName = "SYS_M_COMPANY_GROUP";
        private Control activeControl;
        private Dlg_Copying copyDlg;
        private Common.geWinGroupType beforeWinGroupType;

        private long before_EstimateNo = 0;
        private long before_OrderNo = 0;
        private long before_SalesNo = 0;
        private long before_ReceiptNo = 0;
        private long before_PurchaseOrderNo = 0;
        private long before_PurchaseNo = 0;
        private long before_CashPaymentNo = 0;
        private long before_ProduceNo = 0;
        private long before_ShipNo = 0;

        private Utl_Report utlReport = new Utl_Report();
        private bool flg_relogin = false;

        #endregion

        #region Constructor

        public Utl_MstCompanyGroup()
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

            this.lblName.Content = Common.gstrGroupDisplayNm + "名";

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
                DataPgLock.gLockPg(PG_NM, ExCast.zCStr(_entity._id), (int)DataPgLock.geLockType.UnLock);

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
                        _entity._id = ExCast.zCInt(_id);
                        this.utlID.txtID.Text = _id;
                        this.utlID.txtID.FormatToID();
                    }
                    else
                    {
                        _entity._id = ExCast.zCInt(_id);
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
                    Utl_MstText mst = (Utl_MstText)activeControl;
                    mst.ShowList();
                    break;
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
            if (this.flg_relogin == true)
            {
                Common.ReLogin(ExWebService.geDialogDisplayFlg.No, ExWebService.geDialogCloseFlg.No);
            }

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
                case "utlDisplay":
                //case "utlZip.Name":
                    ExVisualTreeHelper.SetFunctionKeyEnabled("F5", true, this);
                    activeControl = ctl;
                    break;
                case "txtName":
                case "txtKana":
                case "txtAdoutName":
                case "txtTel":
                case "txtFax":
                case "txtMail":
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
                if (ExCast.zCInt(this.utlID.txtID.Text.Trim()) == 0)
                {
                    ExMessageBox.Show("グループIDには0以外を指定して下さい。");
                    this.utlID.txtID.Text = "";
                    this.utlID.txtID.Focus();
                    return;
                }
                GetMstData(ExCast.zCInt(this.utlID.txtID.Text.Trim()));
            }
        }

        #endregion

        #region Binding Setting

        private void SetBinding()
        {
            if (_entity == null)
            {
                _entity = new EntityCompanyGroup();
                _entity._estimate_approval_flg = 1;
                _entity._invoice_print_flg = 1;
            }

            // マスタコントロールPropertyChanged
            _entity.PropertyChanged += this.utlZip.MstID_Changed;

            NumberConverter nmConvDecm0 = new NumberConverter();

            #region Bind

            #region グループ情報

            // バインド
            Binding BindingName = new Binding("_name");
            BindingName.Mode = BindingMode.TwoWay;
            BindingName.Source = _entity;
            this.txtName.SetBinding(TextBox.TextProperty, BindingName);

            Binding BindingKana = new Binding("_kana");
            BindingKana.Mode = BindingMode.TwoWay;
            BindingKana.Source = _entity;
            this.txtKana.SetBinding(TextBox.TextProperty, BindingKana);

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

            #region グループ運用情報

            if (_entity._estimate_approval_flg == 0)
            {
                this.rdoApprovalAri.IsChecked = false;
                this.rdoApprovalNasi.IsChecked = true;
            }
            else
            {
                this.rdoApprovalAri.IsChecked = true;
                this.rdoApprovalNasi.IsChecked = false;
            }

            #endregion

            #region 入金口座情報

            Binding BindingBankName = new Binding("_bank_nm");
            BindingBankName.Mode = BindingMode.TwoWay;
            BindingBankName.Source = _entity;
            this.txtBankName.SetBinding(TextBox.TextProperty, BindingBankName);

            Binding BindingBankBranchName = new Binding("_bank_branch_nm");
            BindingBankBranchName.Mode = BindingMode.TwoWay;
            BindingBankBranchName.Source = _entity;
            this.txtBranchName.SetBinding(TextBox.TextProperty, BindingBankBranchName);

            Binding BindingBankAccountNo = new Binding("_bank_account_no");
            BindingBankAccountNo.Mode = BindingMode.TwoWay;
            BindingBankAccountNo.Source = _entity;
            this.txtAccountNo.SetBinding(TextBox.TextProperty, BindingBankAccountNo);

            Binding BindingBankAccountName = new Binding("_bank_account_nm");
            BindingBankAccountName.Mode = BindingMode.TwoWay;
            BindingBankAccountName.Source = _entity;
            this.txtAccountName.SetBinding(TextBox.TextProperty, BindingBankAccountName);

            Binding BindingBankAccountKana = new Binding("_bank_account_kana");
            BindingBankAccountKana.Mode = BindingMode.TwoWay;
            BindingBankAccountKana.Source = _entity;
            this.txtAccountKana.SetBinding(TextBox.TextProperty, BindingBankAccountKana);

            if (_entity._invoice_print_flg == 0)
            {
                this.rdoInvvoicePrintNasi.IsChecked = true;
                this.rdoInvvoicePrintAri.IsChecked = false;
            }
            else
            {
                this.rdoInvvoicePrintNasi.IsChecked = false;
                this.rdoInvvoicePrintAri.IsChecked = true;
            }

            #endregion

            #region 伝票情報

            Binding BindingEstimateYmd = new Binding("_estimate_ymd");
            BindingEstimateYmd.Mode = BindingMode.TwoWay;
            BindingEstimateYmd.Source = _entity;
            this.txtEstimateYmd.SetBinding(TextBox.TextProperty, BindingEstimateYmd);

            Binding BindingOrderYmd = new Binding("_order_ymd");
            BindingOrderYmd.Mode = BindingMode.TwoWay;
            BindingOrderYmd.Source = _entity;
            this.txtOrderYmd.SetBinding(TextBox.TextProperty, BindingOrderYmd);

            Binding BindingSalesYmd = new Binding("_sales_ymd");
            BindingSalesYmd.Mode = BindingMode.TwoWay;
            BindingSalesYmd.Source = _entity;
            this.txtSalesYmd.SetBinding(TextBox.TextProperty, BindingSalesYmd);

            Binding BindingReceiptYmd = new Binding("_receipt_ymd");
            BindingReceiptYmd.Mode = BindingMode.TwoWay;
            BindingReceiptYmd.Source = _entity;
            this.txtReceiptYmd.SetBinding(TextBox.TextProperty, BindingReceiptYmd);

            Binding BindingPurchaseOrderYmd = new Binding("_purchase_order_ymd");
            BindingPurchaseOrderYmd.Mode = BindingMode.TwoWay;
            BindingPurchaseOrderYmd.Source = _entity;
            this.txtPurchaseOrderYmd.SetBinding(TextBox.TextProperty, BindingPurchaseOrderYmd);

            Binding BindingPurchaseYmd = new Binding("_purchase_ymd");
            BindingPurchaseYmd.Mode = BindingMode.TwoWay;
            BindingPurchaseYmd.Source = _entity;
            this.txtPurchaseYmd.SetBinding(TextBox.TextProperty, BindingPurchaseYmd);

            Binding BindingCashPaymentYmd = new Binding("_cash_payment_ymd");
            BindingCashPaymentYmd.Mode = BindingMode.TwoWay;
            BindingCashPaymentYmd.Source = _entity;
            this.txtCashPaymentYmd.SetBinding(TextBox.TextProperty, BindingCashPaymentYmd);

            Binding BindingProduceYmd = new Binding("_produce_ymd");
            BindingProduceYmd.Mode = BindingMode.TwoWay;
            BindingProduceYmd.Source = _entity;
            this.txtProduceYmd.SetBinding(TextBox.TextProperty, BindingProduceYmd);

            Binding BindingShipYmd = new Binding("_ship_ymd");
            BindingShipYmd.Mode = BindingMode.TwoWay;
            BindingShipYmd.Source = _entity;
            this.txtShipYmd.SetBinding(TextBox.TextProperty, BindingShipYmd);

            Binding BindingEstimateCnt = new Binding("_estimate_cnt");
            BindingEstimateCnt.Mode = BindingMode.TwoWay;
            BindingEstimateCnt.Source = _entity;
            this.txtEstimateCnt.SetBinding(TextBox.TextProperty, BindingEstimateCnt);

            Binding BindingOrderCnt = new Binding("_order_cnt");
            BindingOrderCnt.Mode = BindingMode.TwoWay;
            BindingOrderCnt.Source = _entity;
            this.txtOrderCnt.SetBinding(TextBox.TextProperty, BindingOrderCnt);

            Binding BindingSalesCnt = new Binding("_sales_cnt");
            BindingSalesCnt.Mode = BindingMode.TwoWay;
            BindingSalesCnt.Source = _entity;
            this.txtSalesCnt.SetBinding(TextBox.TextProperty, BindingSalesCnt);

            Binding BindingReceiptCnt = new Binding("_receipt_cnt");
            BindingReceiptCnt.Mode = BindingMode.TwoWay;
            BindingReceiptCnt.Source = _entity;
            this.txtReceiptCnt.SetBinding(TextBox.TextProperty, BindingReceiptCnt);

            Binding BindingPurchaseOrderCnt = new Binding("_purchase_order_cnt");
            BindingPurchaseOrderCnt.Mode = BindingMode.TwoWay;
            BindingPurchaseOrderCnt.Source = _entity;
            this.txtPurchaseOrderCnt.SetBinding(TextBox.TextProperty, BindingPurchaseOrderCnt);

            Binding BindingPurchaseCnt = new Binding("_purchase_cnt");
            BindingPurchaseCnt.Mode = BindingMode.TwoWay;
            BindingPurchaseCnt.Source = _entity;
            this.txtPurchaseCnt.SetBinding(TextBox.TextProperty, BindingPurchaseCnt);

            Binding BindingCashPaymentCnt = new Binding("_cash_payment_cnt");
            BindingCashPaymentCnt.Mode = BindingMode.TwoWay;
            BindingCashPaymentCnt.Source = _entity;
            this.txtCashPaymentCnt.SetBinding(TextBox.TextProperty, BindingCashPaymentCnt);

            Binding BindingProduceCnt = new Binding("_produce_cnt");
            BindingProduceCnt.Mode = BindingMode.TwoWay;
            BindingProduceCnt.Source = _entity;
            this.txtProduceCnt.SetBinding(TextBox.TextProperty, BindingProduceCnt);

            Binding BindingShipCnt = new Binding("_ship_cnt");
            BindingShipCnt.Mode = BindingMode.TwoWay;
            BindingShipCnt.Source = _entity;
            this.txtShipCnt.SetBinding(TextBox.TextProperty, BindingShipCnt);

            Binding BindingEstimateNo = new Binding("_estimate_no");
            BindingEstimateNo.Mode = BindingMode.TwoWay;
            BindingEstimateNo.Source = _entity;
            this.utlEstimateNo.txtID.SetBinding(TextBox.TextProperty, BindingEstimateNo);

            Binding BindingOrderNo = new Binding("_order_no");
            BindingOrderNo.Mode = BindingMode.TwoWay;
            BindingOrderNo.Source = _entity;
            this.utlOrderNo.txtID.SetBinding(TextBox.TextProperty, BindingOrderNo);

            Binding BindingSalesNo = new Binding("_sales_no");
            BindingSalesNo.Mode = BindingMode.TwoWay;
            BindingSalesNo.Source = _entity;
            this.utlSalesNo.txtID.SetBinding(TextBox.TextProperty, BindingSalesNo);

            Binding BindingReceiptNo = new Binding("_receipt_no");
            BindingReceiptNo.Mode = BindingMode.TwoWay;
            BindingReceiptNo.Source = _entity;
            this.utlReceiptNo.txtID.SetBinding(TextBox.TextProperty, BindingReceiptNo);

            Binding BindingPurchaseOrderNo = new Binding("_purchase_order_no");
            BindingPurchaseOrderNo.Mode = BindingMode.TwoWay;
            BindingPurchaseOrderNo.Source = _entity;
            this.utlPurchaseOrderNo.txtID.SetBinding(TextBox.TextProperty, BindingPurchaseOrderNo);

            Binding BindingPurchaseNo = new Binding("_purchase_no");
            BindingPurchaseNo.Mode = BindingMode.TwoWay;
            BindingPurchaseNo.Source = _entity;
            this.utlPurchaseNo.txtID.SetBinding(TextBox.TextProperty, BindingPurchaseNo);

            Binding BindingCashPaymentNo = new Binding("_cash_payment_no");
            BindingCashPaymentNo.Mode = BindingMode.TwoWay;
            BindingCashPaymentNo.Source = _entity;
            this.utlCashPaymentNo.txtID.SetBinding(TextBox.TextProperty, BindingCashPaymentNo);

            Binding BindingProduceNo = new Binding("_produce_no");
            BindingProduceNo.Mode = BindingMode.TwoWay;
            BindingProduceNo.Source = _entity;
            this.utlProduceNo.txtID.SetBinding(TextBox.TextProperty, BindingProduceNo);

            Binding BindingShipNo = new Binding("_ship_no");
            BindingShipNo.Mode = BindingMode.TwoWay;
            BindingShipNo.Source = _entity;
            this.utlShipNo.txtID.SetBinding(TextBox.TextProperty, BindingShipNo);

            #endregion

            #endregion

            this.utlID.txtID.SetZeroToNullString();

            if (ExCast.zCInt(_entity._id) == 0)
            {
                _entity._display_division_id = 1;
            }

            this.txtEstimateCnt.OnFormatString();
            this.txtOrderCnt.OnFormatString();
            this.txtSalesCnt.OnFormatString();
            this.txtReceiptCnt.OnFormatString();
            this.txtPurchaseOrderCnt.OnFormatString();
            this.txtPurchaseCnt.OnFormatString();
            this.txtCashPaymentCnt.OnFormatString();
            this.txtProduceCnt.OnFormatString();
            this.txtShipCnt.OnFormatString();

            this.utlEstimateNo.txtID.OnFormatString();
            this.utlOrderNo.txtID.OnFormatString();
            this.utlSalesNo.txtID.OnFormatString();
            this.utlReceiptNo.txtID.OnFormatString();
            this.utlPurchaseOrderNo.txtID.OnFormatString();
            this.utlPurchaseNo.txtID.OnFormatString();
            this.utlCashPaymentNo.txtID.OnFormatString();
            this.utlProduceNo.txtID.OnFormatString();
            this.utlShipNo.txtID.OnFormatString();

            before_EstimateNo = _entity._estimate_no;
            before_OrderNo = _entity._order_no;
            before_SalesNo = _entity._sales_no;
            before_ReceiptNo = _entity._receipt_no;
            before_PurchaseOrderNo = _entity._purchase_order_no;
            before_PurchaseNo = _entity._purchase_no;
            before_CashPaymentNo = _entity._cash_payment_no;
            before_ProduceNo = _entity._produce_no;
            before_ShipNo = _entity._ship_no;
        }

        #endregion

        #region Data Select

        #region データ取得

        private void GetMstData(int id)
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
                        _entity = (EntityCompanyGroup)objList;

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
                        _entity = new EntityCompanyGroup();
                        SetBinding();
                        this.utlFunctionKey.gFunctionKeyEnable = Utl_FunctionKey.geFunctionKeyEnable.New;
                    }
                    this.utlID.txtID_IsReadOnly = true;
                    ExBackgroundWorker.DoWork_Focus(this.txtName, 10);
                    break;
                case ExWebService.geWebServiceCallKbn.GetCompany:
                    // 更新
                    if (objList != null)
                    {
                        EntityCompany entityCompany = (EntityCompany)objList;

                        if (string.IsNullOrEmpty(entityCompany.message))
                        {
                            _entity._name = entityCompany._name;
                            _entity._kana = entityCompany._kana;
                            _entity._zip_code_from = entityCompany._zip_code_from;
                            _entity._zip_code_to = entityCompany._zip_code_to;
                            _entity._adress_city = entityCompany._adress_city;
                            _entity._adress_town = entityCompany._adress_town;
                            _entity._adress1 = entityCompany._adress1;
                            _entity._adress2 = entityCompany._adress2;
                            _entity._tel = entityCompany._tel;
                            _entity._fax = entityCompany._fax;
                            _entity._mail_adress = entityCompany._mail_adress;
                            _entity._mobile_tel = entityCompany._mobile_tel;
                            _entity._mobile_adress = entityCompany._mobile_adress;
                            _entity._url = entityCompany._url;

                            SetBinding();

                            return;
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        #endregion

        private void btnCompanyCopy_Click(object sender, RoutedEventArgs e)
        {
            object[] prm = new object[1];
            prm[0] = "";
            webService.objPerent = this;
            webService.CallWebService(ExWebService.geWebServiceCallKbn.GetCompany,
                                      ExWebService.geDialogDisplayFlg.Yes,
                                      ExWebService.geDialogCloseFlg.Yes,
                                      prm);

        }

        #endregion

        #region Data Update

        // データ追加・更新・削除
        private void UpdateData(Common.geUpdateType upd)
        {
            object[] prm = new object[4];

            prm[0] = (int)upd;
            prm[1] = ExCast.zCInt(this.utlID.txtID.Text.Trim());
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
                    if (Common.gintGroupId == _entity._id)
                    {
                        Common.gstrGroupNm = _entity._name;
                        Common.gintEstimateApprovalFlg = _entity._estimate_approval_flg;
                        Common.gintReceiptAccountInvoicePringFlg = _entity._invoice_print_flg;
                        this.flg_relogin = true;
                    }

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

                // 表示区分
                if (string.IsNullOrEmpty(ExCast.zCStr(_entity._display_division_nm)))
                {
                    errMessage += "表示区分が入力(選択)されていません。" + Environment.NewLine;
                    if (errCtl == null) errCtl = this.utlDisplay.txtID;
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

                #endregion

                #region 値セット

                if (this.rdoApprovalAri.IsChecked == true)
                {
                    _entity._estimate_approval_flg = 1;
                }
                else
                {
                    _entity._estimate_approval_flg = 0;
                }

                if (this.rdoInvvoicePrintAri.IsChecked == true)
                {
                    _entity._invoice_print_flg = 1;
                }
                else
                {
                    _entity._invoice_print_flg = 0;
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

                if (this._entity._id == 0)
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
                    break;
                default:
                    break;
            }
        }

        #endregion

    }

}
