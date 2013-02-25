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
using SlvHanbaiClient.View.UserControl.Custom;
using SlvHanbaiClient.View.Dlg;
using SlvHanbaiClient.View.UserControl.Report;

namespace SlvHanbaiClient.View.UserControl.Master
{
    public partial class Utl_MstCompany : ExUserControl
    {

        #region Filed Const

        private const String CLASS_NM = "Utl_MstCompany";
        private const String PG_NM = DataPgEvidence.PGName.Mst.Company;
        private const Common.geWinMsterType _WinMsterType = Common.geWinMsterType.Company;
        private const ExWebService.geWebServiceCallKbn _GetWebServiceCallKbn = ExWebService.geWebServiceCallKbn.GetCompany;
        private const ExWebService.geWebServiceCallKbn _UpdWebServiceCallKbn = ExWebService.geWebServiceCallKbn.UpdateCompany;
        private Class.Data.MstData.geMDataKbn MstKbn = Class.Data.MstData.geMDataKbn.Company;
        private EntityCompany _entity = new EntityCompany();
        private readonly string tableName = "M_CUSTOMER";
        private Control activeControl;
        private Dlg_Copying copyDlg;
        private Common.geWinGroupType beforeWinGroupType;

        private Utl_Report utlReport = new Utl_Report();
        private bool flg_relogin = false;

        private int before_intidFigureSlipNo = 0;
        private int before_intidFigureCustomer = 0;
        private int before_intidFigurePurchase = 0;
        private int before_intidFigureCommodity = 0;

        #endregion

        #region Constructor

        public Utl_MstCompany()
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
            ExVisualTreeHelper.initDisplayNoReadOnly(this.LayoutRoot);

            // ファンクションキー初期設定
            this.utlFunctionKey.gFunctionKeyEnable = Utl_FunctionKey.geFunctionKeyEnable.Init;
            this.utlFunctionKey.Init();

            GetMstData();
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
            GetMstData();
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

        // F12ボタン(メニュー) クリック
        public override void btnF12_Click(object sender, RoutedEventArgs e)
        {
            if (this.flg_relogin == true)
            {
                Common.ReLogin(ExWebService.geDialogDisplayFlg.No, ExWebService.geDialogCloseFlg.No);
            }

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
                //case "utlZip":
                //case "utlZip.Name":
                //    ExVisualTreeHelper.SetFunctionKeyEnabled("F5", true, this);
                //    activeControl = ctl;
                //    break;
                case "txtName":
                case "txtKana":
                case "txtTel":
                case "txtFax":
                case "txtMail":
                case "txtUrl":
                case "txtMemo":
                case "txtCustomerIdFg":
                case "txtCommdityIdFg":
                case "txtPurchaseIdFg":
                case "txtDenNoFg":
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

        #endregion

        #region Binding Setting

        private void SetBinding()
        {
            if (_entity == null)
            {
                _entity = new EntityCompany();
            }

            // マスタコントロールPropertyChanged
            _entity.PropertyChanged += this.utlZip.MstID_Changed;

            NumberConverter nmConvDecm0 = new NumberConverter();
            nmConvDecm0.IsMaxMinCheck = true;
            nmConvDecm0.MinNumber = 4;
            nmConvDecm0.MaxNumber = 15;

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

            Binding BindingGroupDisplayNm = new Binding("_group_display_name");
            BindingGroupDisplayNm.Mode = BindingMode.TwoWay;
            BindingGroupDisplayNm.Source = _entity;
            this.txtGroupDisplayNm.SetBinding(TextBox.TextProperty, BindingGroupDisplayNm);

            Binding BindingIdFigureSlipNo = new Binding("_id_figure_slip_no");
            BindingIdFigureSlipNo.Converter = nmConvDecm0;
            BindingIdFigureSlipNo.Mode = BindingMode.TwoWay;
            BindingIdFigureSlipNo.Source = _entity;
            this.txtDenNoFg.SetBinding(TextBox.TextProperty, BindingIdFigureSlipNo);

            Binding BindingIdFigureCustomer = new Binding("_id_figure_customer");
            BindingIdFigureCustomer.Converter = nmConvDecm0;
            BindingIdFigureCustomer.Mode = BindingMode.TwoWay;
            BindingIdFigureCustomer.Source = _entity;
            this.txtCustomerIdFg.SetBinding(TextBox.TextProperty, BindingIdFigureCustomer);

            Binding BindingIdFigurePurchase = new Binding("_id_figure_purchase");
            BindingIdFigurePurchase.Converter = nmConvDecm0;
            BindingIdFigurePurchase.Mode = BindingMode.TwoWay;
            BindingIdFigurePurchase.Source = _entity;
            this.txtPurchaseIdFg.SetBinding(TextBox.TextProperty, BindingIdFigurePurchase);

            Binding BindingIdFigureCommodity = new Binding("_id_figure_commodity");
            BindingIdFigureCommodity.Converter = nmConvDecm0;
            BindingIdFigureCommodity.Mode = BindingMode.TwoWay;
            BindingIdFigureCommodity.Source = _entity;
            this.txtCommdityIdFg.SetBinding(TextBox.TextProperty, BindingIdFigureCommodity);

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

            #endregion

            this.txtEstimateCnt.OnFormatString();
            this.txtOrderCnt.OnFormatString();
            this.txtSalesCnt.OnFormatString();
            this.txtReceiptCnt.OnFormatString();
            this.txtPurchaseOrderCnt.OnFormatString();
            this.txtPurchaseCnt.OnFormatString();
            this.txtCashPaymentCnt.OnFormatString();
            this.txtProduceCnt.OnFormatString();
            this.txtShipCnt.OnFormatString();
        }

        #endregion

        #region Data Select

        #region データ取得

        private void GetMstData()
        {
            // 初期化
            before_intidFigureSlipNo = 0;
            before_intidFigureCustomer = 0;
            before_intidFigurePurchase = 0;
            before_intidFigureCommodity = 0;

            object[] prm = new object[1];
            prm[0] = "";
            webService.objPerent = this;
            webService.CallWebService(_GetWebServiceCallKbn,
                                      ExWebService.geDialogDisplayFlg.No,
                                      ExWebService.geDialogCloseFlg.No,
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
                        _entity = (EntityCompany)objList;

                        if (_entity.message != "" && _entity.message != null)
                        {
                            //btnF12_Click(null, null);
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

                            before_intidFigureSlipNo = _entity._id_figure_slip_no;
                            before_intidFigureCustomer = _entity._id_figure_customer;
                            before_intidFigurePurchase = _entity._id_figure_purchase;
                            before_intidFigureCommodity = _entity._id_figure_commodity;
                        }
                    }
                    else
                    {
                        //btnF12_Click(null, null);
                    }
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
            object[] prm = new object[2];
            prm[0] = (int)upd;
            prm[1] = _entity;
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
                    this.flg_relogin = true;

                    Common.gintidFigureCustomer = _entity._id_figure_customer;
                    Common.gintidFigureCommodity = _entity._id_figure_commodity;
                    Common.gintidFigurePurchase = _entity._id_figure_purchase;
                    Common.gintidFigureSlipNo = _entity._id_figure_slip_no;
                    Common.gstrGroupDisplayNm = _entity._group_display_name;
                    Common.gstrCompanyNm = _entity._name;

                    ExMessageBox.Show("登録しました。");
                    this.btnF12_Click(null, null);
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

                // 名称
                if (string.IsNullOrEmpty(_entity._name))
                {
                    errMessage += "会社名が入力されていません。" + Environment.NewLine;
                    if (errCtl == null) errCtl = this.txtName;
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

                #region 変更チェック
                
                if (before_intidFigureCustomer > _entity._id_figure_customer)
                {
                    warnMessage += "得意先/納入先ID桁数が" + before_intidFigureCustomer.ToString() + "から" + _entity._id_figure_customer + "に変更されています。" + Environment.NewLine;
                    warnMessage += "得意先/納入先ID桁数" + (_entity._id_figure_customer + 1).ToString() + "桁以上のデータの登録・参照ができなくなります。" + Environment.NewLine;
                    if (errCtl == null) errCtl = this.txtCustomerIdFg;
                }

                if (before_intidFigureCommodity > _entity._id_figure_commodity)
                {
                    warnMessage += "商品ID桁数が" + before_intidFigureCommodity.ToString() + "から" + _entity._id_figure_commodity + "に変更されています。" + Environment.NewLine;
                    warnMessage += "商品ID桁数" + (_entity._id_figure_commodity + 1).ToString() + "桁以上のデータの登録・参照ができなくなります。" + Environment.NewLine;
                    if (errCtl == null) errCtl = this.txtCommdityIdFg;
                }

                if (before_intidFigurePurchase > _entity._id_figure_purchase)
                {
                    warnMessage += "仕入先ID桁数が" + before_intidFigurePurchase.ToString() + "から" + _entity._id_figure_purchase + "に変更されています。" + Environment.NewLine;
                    warnMessage += "仕入先ID桁数" + (_entity._id_figure_purchase + 1).ToString() + "桁以上のデータの登録・参照ができなくなります。" + Environment.NewLine;
                    if (errCtl == null) errCtl = this.txtPurchaseIdFg;
                }

                if (before_intidFigureSlipNo > _entity._id_figure_slip_no)
                {
                    warnMessage += "伝票No桁数が" + before_intidFigureSlipNo.ToString() + "から" + _entity._id_figure_slip_no + "に変更されています。" + Environment.NewLine;
                    warnMessage += "伝票No桁数" + (_entity._id_figure_slip_no + 1).ToString() + "桁以上のデータの登録・参照ができなくなります。" + Environment.NewLine;
                    if (errCtl == null) errCtl = this.txtDenNoFg;
                }

                #endregion

                #region 範囲チェック

                if (!(ExCast.zCInt(this.txtCustomerIdFg.Text.Trim()) >= 4 && ExCast.zCInt(this.txtCustomerIdFg.Text.Trim()) <= 15))
                {
                    errMessage += "得意先/納入先ID桁数には4から15の整数を入力して下さい。" + Environment.NewLine;
                    if (errCtl == null) errCtl = this.txtCustomerIdFg;
                }

                if (!(ExCast.zCInt(this.txtCommdityIdFg.Text.Trim()) >= 4 && ExCast.zCInt(this.txtCommdityIdFg.Text.Trim()) <= 15))
                {
                    errMessage += "商品ID桁数には4から15の整数を入力して下さい。" + Environment.NewLine;
                    if (errCtl == null) errCtl = this.txtCommdityIdFg;
                }

                if (!(ExCast.zCInt(this.txtPurchaseIdFg.Text.Trim()) >= 4 && ExCast.zCInt(this.txtPurchaseIdFg.Text.Trim()) <= 15))
                {
                    errMessage += "仕入先ID桁数には4から15の整数を入力して下さい。" + Environment.NewLine;
                    if (errCtl == null) errCtl = this.txtPurchaseIdFg;
                }

                if (!(ExCast.zCInt(this.txtDenNoFg.Text.Trim()) >= 4 && ExCast.zCInt(this.txtDenNoFg.Text.Trim()) <= 15))
                {
                    errMessage += "伝票No桁数には4から15の整数を入力して下さい。" + Environment.NewLine;
                    if (errCtl == null) errCtl = this.txtDenNoFg;
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

                UpdateData(Common.geUpdateType.Update);

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

                UpdateData(Common.geUpdateType.Update);

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

        #endregion

        #region PropertyChanged

        //private void _PropertyChanged(object sender, PropertyChangedEventArgs e)
        //{
        //    string _prop = e.PropertyName;
        //    if (_prop == null) _prop = "";
        //    if (_prop.Length > 0)
        //    {
        //        if (_prop.Substring(0, 1) == "_")
        //        {
        //            _prop = _prop.Substring(1, _prop.Length - 1);
        //        }
        //    }

        //    switch (_prop)
        //    {
        //        case "invoice_nm":
        //            break;
        //        default:
        //            break;
        //    }
        //}

        #endregion

    }

}
