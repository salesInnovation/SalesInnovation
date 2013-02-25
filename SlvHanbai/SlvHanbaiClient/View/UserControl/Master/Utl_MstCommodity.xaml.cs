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
using SlvHanbaiClient.svcCommodity;
using SlvHanbaiClient.View.UserControl.Custom;
using SlvHanbaiClient.View.Dlg;

namespace SlvHanbaiClient.View.UserControl.Master
{
    public partial class Utl_MstCommodity : ExUserControl
    {

        #region Filed Const

        private const String CLASS_NM = "Utl_MstCommodity";
        private const String PG_NM = DataPgEvidence.PGName.Mst.Commodity;
        private const Common.geWinMsterType _WinMsterType = Common.geWinMsterType.Commodity;
        private const ExWebService.geWebServiceCallKbn _GetWebServiceCallKbn = ExWebService.geWebServiceCallKbn.GetCommodity;
        private const ExWebService.geWebServiceCallKbn _UpdWebServiceCallKbn = ExWebService.geWebServiceCallKbn.UpdateCommodity;
        private Class.Data.MstData.geMDataKbn MstKbn = Class.Data.MstData.geMDataKbn.Commodity;
        private EntityCommodity _entity = new EntityCommodity();
        private readonly string tableName = "M_COMMODITY";
        private Control activeControl;
        private Dlg_Copying searchDlg;
        private Common.geWinGroupType beforeWinGroupType;

        #endregion

        #region Constructor

        public Utl_MstCommodity()
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

            searchDlg = new Dlg_Copying();

            searchDlg.utlCopying.gPageType = Common.gePageType.None;
            searchDlg.utlCopying.gWinMsterType = _WinMsterType;
            searchDlg.utlCopying.utlMstID.MstKbn = this.MstKbn;
            searchDlg.utlCopying.tableName = this.tableName;

            searchDlg.Show();
            searchDlg.Closed += searchDlg_Closed;
        }

        private void searchDlg_Closed(object sender, EventArgs e)
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
                case "utlID":
                case "utlMainPurchaseId":
                case "utlGroup1":
                    Utl_MstText mst = (Utl_MstText)activeControl;
                    mst.ShowList();
                    break;
                case "utlUnit":
                case "utlTaxationDivisionID":
                case "utlInventoryDivisionId":
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

        #region Button Click Events

        private void btnInventory_Click(object sender, RoutedEventArgs e)
        {
            if (ExCast.zCStr(utlID.txtID.Text.Trim()) != "")
            {
            }
            else
            {
                ExMessageBox.Show("商品を選択して下さい。");
                return;
            }

            Dlg_MstSearch searchDlg = new Dlg_MstSearch(MstData.geMDataKbn.Inventory);
            searchDlg.MstKbn = Class.Data.MstData.geMDataKbn.Inventory;
            searchDlg.MstGroupKbn = Class.Data.MstData.geMGroupKbn.None;
            searchDlg.txtCode.Text = ExCast.zNumZeroNothingFormat(ExCast.zCStr(utlID.txtID.Text.Trim()));
            searchDlg.Show();
        }

        #endregion

        #region GotFocus Events

        private void txt_GotFocus(object sender, RoutedEventArgs e)
        {
            Control ctl = (Control)sender;
            switch (ctl.Name)
            {
                case "utlID":
                case "utlUnit":
                case "utlTaxationDivisionID":
                case "utlInventoryDivisionId":
                case "utlDisplay":
                case "utlMainPurchaseId":
                case "utlGroup1":
                    ExVisualTreeHelper.SetFunctionKeyEnabled("F5", true, this);
                    activeControl = ctl;
                    break;
                case "txtName":
                case "txtKana":
                case "txtEnterNumver":
                case "txtNumverDecimalDigit":
                case "txtUnitDecimalDigit":
                case "txtPurchaseLot":
                case "txtJustInventoryNumber":
                case "txtInventoryNumber":
                case "txtLeadTime":
                case "txtRetailPriceSkipTax":
                case "txtRetailPriceBeforeTax":
                case "txtSalesUnitSkipTax":
                case "txtSalesUnitBeforeTax":
                case "txtSalesCostSkipTax":
                case "txtSalesCostBeforeTax":
                case "txtPurchaseUnitSkipTax":
                case "txtPurchaseUnitBeforeTax":
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
                GetMstData(_id);
            }
        }

        #endregion

        #region Binding Setting

        private void SetBinding()
        {
            if (_entity == null)
            {
                _entity = new EntityCommodity();
            }

            // マスタコントロールPropertyChanged
            _entity.PropertyChanged += this.utlMainPurchaseId.MstID_Changed;
            _entity.PropertyChanged += this.utlGroup1.MstID_Changed;

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

            Binding BindigUnitId = new Binding("_unit_id");
            BindigUnitId.Mode = BindingMode.TwoWay;
            BindigUnitId.Source = _entity;
            this.utlUnit.txtID.SetBinding(TextBox.TextProperty, BindigUnitId);

            Binding BindigUnitNm = new Binding("_unit_nm");
            BindigUnitNm.Mode = BindingMode.TwoWay;
            BindigUnitNm.Source = _entity;
            this.utlUnit.txtNm.SetBinding(TextBox.TextProperty, BindigUnitNm);

            Binding BindigEnterNumber = new Binding("_enter_number");
            BindigEnterNumber.Mode = BindingMode.TwoWay;
            BindigEnterNumber.Source = _entity;
            this.txtEnterNumver.SetBinding(TextBox.TextProperty, BindigEnterNumber);

            Binding BindigNumberDecimalDigit = new Binding("_number_decimal_digit");
            BindigNumberDecimalDigit.Mode = BindingMode.TwoWay;
            BindigNumberDecimalDigit.Source = _entity;
            this.txtNumverDecimalDigit.SetBinding(TextBox.TextProperty, BindigNumberDecimalDigit);

            Binding BindigUnitDecimalDigit = new Binding("_unit_decimal_digit");
            BindigUnitDecimalDigit.Mode = BindingMode.TwoWay;
            BindigUnitDecimalDigit.Source = _entity;
            this.txtUnitDecimalDigit.SetBinding(TextBox.TextProperty, BindigUnitDecimalDigit);

            Binding BindigTaxationDivitionId = new Binding("_taxation_divition_id");
            BindigTaxationDivitionId.Mode = BindingMode.TwoWay;
            BindigTaxationDivitionId.Source = _entity;
            this.utlTaxationDivisionID.txtID.SetBinding(TextBox.TextProperty, BindigTaxationDivitionId);

            Binding BindigTaxationDivitionNm = new Binding("_taxation_divition_nm");
            BindigTaxationDivitionNm.Mode = BindingMode.TwoWay;
            BindigTaxationDivitionNm.Source = _entity;
            this.utlTaxationDivisionID.txtNm.SetBinding(TextBox.TextProperty, BindigTaxationDivitionNm);

            Binding BindigInventoryManagementDivisionId = new Binding("_inventory_management_division_id");
            BindigInventoryManagementDivisionId.Mode = BindingMode.TwoWay;
            BindigInventoryManagementDivisionId.Source = _entity;
            this.utlInventoryDivisionId.txtID.SetBinding(TextBox.TextProperty, BindigInventoryManagementDivisionId);

            Binding BindigInventoryManagementDivisionNm = new Binding("_inventory_management_division_nm");
            BindigInventoryManagementDivisionNm.Mode = BindingMode.TwoWay;
            BindigInventoryManagementDivisionNm.Source = _entity;
            this.utlInventoryDivisionId.txtNm.SetBinding(TextBox.TextProperty, BindigInventoryManagementDivisionNm);

            Binding BindigPurchaselot = new Binding("_purchase_lot");
            BindigPurchaselot.Mode = BindingMode.TwoWay;
            BindigPurchaselot.Source = _entity;
            this.txtPurchaseLot.SetBinding(TextBox.TextProperty, BindigPurchaselot);

            Binding BindigLeadTime = new Binding("_lead_time");
            BindigLeadTime.Mode = BindingMode.TwoWay;
            BindigLeadTime.Source = _entity;
            this.txtLeadTime.SetBinding(TextBox.TextProperty, BindigLeadTime);

            Binding BindigJustInventoryNumver = new Binding("_just_inventory_number");
            BindigJustInventoryNumver.Mode = BindingMode.TwoWay;
            BindigJustInventoryNumver.Source = _entity;
            this.txtJustInventoryNumber.SetBinding(TextBox.TextProperty, BindigJustInventoryNumver);

            Binding BindigInventoryNumver = new Binding("_inventory_number");
            BindigInventoryNumver.Mode = BindingMode.TwoWay;
            BindigInventoryNumver.Source = _entity;
            this.txtInventoryNumber.SetBinding(TextBox.TextProperty, BindigInventoryNumver);

            Binding BindigMainPurchaseId = new Binding("_main_purchase_id");
            BindigMainPurchaseId.Mode = BindingMode.TwoWay;
            BindigMainPurchaseId.Source = _entity;
            this.utlMainPurchaseId.txtID.SetBinding(TextBox.TextProperty, BindigMainPurchaseId);

            Binding BindigMainPurchaseNm = new Binding("_main_purchase_nm");
            BindigMainPurchaseNm.Mode = BindingMode.TwoWay;
            BindigMainPurchaseNm.Source = _entity;
            this.utlMainPurchaseId.txtNm.SetBinding(TextBox.TextProperty, BindigMainPurchaseNm);

            Binding BindigRetailPriceSkipTax = new Binding("_retail_price_skip_tax");
            BindigRetailPriceSkipTax.Mode = BindingMode.TwoWay;
            BindigRetailPriceSkipTax.Source = _entity;
            this.txtRetailPriceSkipTax.SetBinding(TextBox.TextProperty, BindigRetailPriceSkipTax);

            Binding BindigRetailPriceBeforeTax = new Binding("_retail_price_before_tax");
            BindigRetailPriceBeforeTax.Mode = BindingMode.TwoWay;
            BindigRetailPriceBeforeTax.Source = _entity;
            this.txtRetailPriceBeforeTax.SetBinding(TextBox.TextProperty, BindigRetailPriceBeforeTax);

            Binding BindigSalesUnitPriceSkipTax = new Binding("_sales_unit_price_skip_tax");
            BindigSalesUnitPriceSkipTax.Mode = BindingMode.TwoWay;
            BindigSalesUnitPriceSkipTax.Source = _entity;
            this.txtSalesUnitSkipTax.SetBinding(TextBox.TextProperty, BindigSalesUnitPriceSkipTax);

            Binding BindigSalesUnitPriceBeforeTax = new Binding("_sales_unit_price_before_tax");
            BindigSalesUnitPriceBeforeTax.Mode = BindingMode.TwoWay;
            BindigSalesUnitPriceBeforeTax.Source = _entity;
            this.txtSalesUnitBeforeTax.SetBinding(TextBox.TextProperty, BindigSalesUnitPriceBeforeTax);

            Binding BindigSalesCostPriceSkipTax = new Binding("_sales_cost_price_skip_tax");
            BindigSalesCostPriceSkipTax.Mode = BindingMode.TwoWay;
            BindigSalesCostPriceSkipTax.Source = _entity;
            this.txtSalesCostSkipTax.SetBinding(TextBox.TextProperty, BindigSalesCostPriceSkipTax);

            Binding BindigSalesCostPriceBeforeTax = new Binding("_sales_cost_price_before_tax");
            BindigSalesCostPriceBeforeTax.Mode = BindingMode.TwoWay;
            BindigSalesCostPriceBeforeTax.Source = _entity;
            this.txtSalesCostBeforeTax.SetBinding(TextBox.TextProperty, BindigSalesCostPriceBeforeTax);

            Binding BindigPPurcharseUnitPriceSkipTax = new Binding("_purchase_unit_price_skip_tax");
            BindigPPurcharseUnitPriceSkipTax.Mode = BindingMode.TwoWay;
            BindigPPurcharseUnitPriceSkipTax.Source = _entity;
            this.txtPurchaseUnitSkipTax.SetBinding(TextBox.TextProperty, BindigPPurcharseUnitPriceSkipTax);

            Binding BindigPPurcharseUnitPriceBeforeTax = new Binding("_purchase_unit_price_before_tax");
            BindigPPurcharseUnitPriceBeforeTax.Mode = BindingMode.TwoWay;
            BindigPPurcharseUnitPriceBeforeTax.Source = _entity;
            this.txtPurchaseUnitBeforeTax.SetBinding(TextBox.TextProperty, BindigPPurcharseUnitPriceBeforeTax);

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
            this.utlUnit.txtID.SetZeroToNullString();
            this.utlTaxationDivisionID.txtID.SetZeroToNullString();
            this.utlInventoryDivisionId.txtID.SetZeroToNullString();
            this.utlGroup1.txtID.SetZeroToNullString();

            this.utlMainPurchaseId.txtID.FormatToID();
            this.txtEnterNumver.OnFormatString();
            this.txtPurchaseLot.OnFormatString();
            this.txtJustInventoryNumber.OnFormatString();
            this.txtInventoryNumber.OnFormatString();
            this.txtRetailPriceSkipTax.OnFormatString();
            this.txtRetailPriceBeforeTax.OnFormatString();
            this.txtSalesUnitSkipTax.OnFormatString();
            this.txtSalesUnitBeforeTax.OnFormatString();
            this.txtSalesCostSkipTax.OnFormatString();
            this.txtSalesCostBeforeTax.OnFormatString();
            this.txtPurchaseUnitSkipTax.OnFormatString();
            this.txtPurchaseUnitBeforeTax.OnFormatString();

            if (ExCast.zCInt(_entity._id) == 0)
            {
                _entity._unit_id = 1;                               // 単位 1:個
                _entity._enter_number = 1;                          // 入数
                _entity._taxation_divition_id = 1;                  // 課税区分 1:課税
                _entity._inventory_management_division_id = 1;      // 在庫管理区分 1:在庫管理する
                _entity._display_division_id = 1;
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
                        _entity = (EntityCommodity)objList;

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
                        _entity = new EntityCommodity();
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

                // 表示区分
                if (string.IsNullOrEmpty(ExCast.zCStr(_entity._display_division_nm)))
                {
                    errMessage += "表示区分が入力(選択)されていません。" + Environment.NewLine;
                    if (errCtl == null) errCtl = this.utlDisplay.txtID;
                }

                // 単位
                if (ExCast.zCInt(_entity._unit_id) == 0)
                {
                    errMessage += "単位が入力(選択)されていません。" + Environment.NewLine;
                    if (errCtl == null) errCtl = this.utlUnit.txtID;
                }

                // 課税区分
                if (ExCast.zCInt(_entity._taxation_divition_id) == 0)
                {
                    errMessage += "課税区分が入力(選択)されていません。" + Environment.NewLine;
                    if (errCtl == null) errCtl = this.utlTaxationDivisionID.txtID;
                }

                // 在庫管理区分
                if (ExCast.zCInt(_entity._inventory_management_division_id) == 0)
                {
                    errMessage += "在庫管理区分が入力(選択)されていません。" + Environment.NewLine;
                    if (errCtl == null) errCtl = this.utlInventoryDivisionId.txtID;
                }

                #endregion

                #region 適正値入力チェック

                // 主仕入先
                if (ExCast.zCStr(_entity._main_purchase_id) != "" && string.IsNullOrEmpty(_entity._main_purchase_nm))
                {
                    errMessage += "主仕入先が適切に入力(選択)されていません。" + Environment.NewLine;
                    if (errCtl == null) errCtl = this.utlMainPurchaseId.txtID;
                }

                // 単位
                if (ExCast.zCInt(_entity._unit_id) != 0 && string.IsNullOrEmpty(_entity._unit_nm))
                {
                    errMessage += "単位が適切に入力(選択)されていません。" + Environment.NewLine;
                    if (errCtl == null) errCtl = this.utlUnit.txtID;
                }

                // 課税区分
                if (ExCast.zCInt(_entity._taxation_divition_id) != 0 && string.IsNullOrEmpty(_entity._taxation_divition_nm))
                {
                    errMessage += "課税区分が適切に入力(選択)されていません。" + Environment.NewLine;
                    if (errCtl == null) errCtl = this.utlTaxationDivisionID.txtID;
                }

                // 在庫管理区分
                if (ExCast.zCInt(_entity._inventory_management_division_id) != 0 && string.IsNullOrEmpty(_entity._inventory_management_division_nm))
                {
                    errMessage += "在庫管理区分が適切に入力(選択)されていません。" + Environment.NewLine;
                    if (errCtl == null) errCtl = this.utlInventoryDivisionId.txtID;
                }

                // 商品分類
                if (ExCast.zCStr(_entity._group1_id) != "" && string.IsNullOrEmpty(_entity._group1_nm))
                {
                    errMessage += "商品分類が適切に入力(選択)されていません。" + Environment.NewLine;
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


    }

}
