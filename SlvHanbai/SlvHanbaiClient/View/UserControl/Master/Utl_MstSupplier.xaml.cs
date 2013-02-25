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
using SlvHanbaiClient.svcSupplier;
using SlvHanbaiClient.svcCustomer;
using SlvHanbaiClient.View.UserControl.Custom;
using SlvHanbaiClient.View.Dlg;

namespace SlvHanbaiClient.View.UserControl.Master
{
    public partial class Utl_MstSupplier : ExUserControl
    {

        #region Filed Const

        private const String CLASS_NM = "Utl_MstSupplier";
        private const String PG_NM = DataPgEvidence.PGName.Mst.Supplier;
        private const Common.geWinMsterType _WinMsterType = Common.geWinMsterType.Supplier;
        private const ExWebService.geWebServiceCallKbn _GetWebServiceCallKbn = ExWebService.geWebServiceCallKbn.GetSupplier;
        private const ExWebService.geWebServiceCallKbn _UpdWebServiceCallKbn = ExWebService.geWebServiceCallKbn.UpdateSupplier;
        private Class.Data.MstData.geMDataKbn MstKbn = Class.Data.MstData.geMDataKbn.Supplier;
        private EntitySupplier _entity = new EntitySupplier();
        private readonly string tableName = "M_SUPPLIER";
        private Control activeControl;
        private Dlg_Copying searchDlg;
        private Common.geWinGroupType beforeWinGroupType;

        #endregion

        #region Constructor

        public Utl_MstSupplier()
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
            this.utlFunctionKey.gFunctionKeyEnable = Utl_FunctionKey.geFunctionKeyEnable.InitKbn;
            this.utlFunctionKey.Init();

            // バインド設定
            SetBinding();

            ExBackgroundWorker.DoWork_FocusForLoad(this.utlCustomer.txtID);
            ExVisualTreeHelper.SetEnabled(this.MainDetail, false);
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

            this.utlFunctionKey.gFunctionKeyEnable = Utl_FunctionKey.geFunctionKeyEnable.InitKbn;
            ExVisualTreeHelper.SetEnabled(this.MainDetail, false);

            this.utlID.txtID_IsReadOnly = false;
            this.utlID.txtID.Text = "";
            this.utlCustomer.txtID_IsReadOnly = false;
            ExBackgroundWorker.DoWork_Focus(this.utlCustomer.txtID, 10);

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
                case "utlZip":
                    Utl_Zip zip = (Utl_Zip)activeControl;
                    zip.ShowList();
                    break;
                case "utlID":
                case "utlCustomer":
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
                case "utlCustomer":
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

        private void utlCustomer_LostFocus(object sender, RoutedEventArgs e)
        {
            //if (!string.IsNullOrEmpty(this.utlCustomer.txtID.Text.Trim()) && !string.IsNullOrEmpty(this.utlCustomer.txtNm.Text.Trim()))
            //{
            //    string _id = this.utlID.txtID.Text.Trim();
            //    if (ExCast.IsNumeric(_id))
            //    {
            //        _id = ExCast.zCDbl(_id).ToString();
            //    }
            //    ExVisualTreeHelper.SetEnabled(this.MainDetail, true);
            //    this.utlCustomer.txtID_IsReadOnly = true;
            //}

        }

        #endregion

        #region Binding Setting

        private void SetBinding()
        {
            bool is_null = false;

            if (_entity == null)
            {
                _entity = new EntitySupplier();
                is_null = true;
            }

            // マスタコントロールPropertyChanged
            _entity.PropertyChanged += this.utlCustomer.MstID_Changed;
            _entity.PropertyChanged += this.utlZip.MstID_Changed;

            NumberConverter nmConvDecm0 = new NumberConverter();

            #region Bind

            string _customerId = ExCast.zNumZeroNothingFormat(this.utlCustomer.txtID.Text.Trim());
            string _customerNm = this.utlCustomer.txtNm.Text.Trim();

            // バインド
            Binding BindingCustomerId = new Binding("_customer_id");
            BindingCustomerId.Mode = BindingMode.TwoWay;
            BindingCustomerId.Source = _entity;
            this.utlCustomer.txtID.SetBinding(TextBox.TextProperty, BindingCustomerId);
            this.utlID.txtID2.SetBinding(TextBox.TextProperty, BindingCustomerId);

            Binding BindingCustomerName = new Binding("_customer_nm");
            BindingCustomerName.Mode = BindingMode.TwoWay;
            BindingCustomerName.Source = _entity;
            this.utlCustomer.txtNm.SetBinding(TextBox.TextProperty, BindingCustomerName);

            if (is_null == false)
            {
                this.utlCustomer.txtID.Text = _customerId;
                this.utlCustomer.txtNm.Text = _customerNm;
            }

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

            if (ExCast.IsNumeric(this.utlID.txtID.Text.Trim()))
            {
                this.utlID.txtID.SetZeroToNullString();
            }
            this.utlID.txtID.FormatToID();            
            this.utlTitle.txtID.SetZeroToNullString();
            this.utlCustomer.txtID.SetZeroToNullString();

            this.utlCustomer.txtID.FormatToID();

            if (ExCast.zCInt(_entity._id) == 0)
            {
                _entity._divide_permission_id = 2;
                _entity._display_division_id = 1;
            }
        }

        #endregion

        #region Data Select

        #region データ取得

        private void GetMstData(string id)
        {
            object[] prm = new object[2];
            prm[0] = ExCast.zNumZeroNothingFormat(this.utlCustomer.txtID.Text.Trim());
            prm[1] = id;
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
                        _entity = (EntitySupplier)objList;

                        if (_entity.message != "" && _entity.message != null)
                        {
                            this.utlID.txtID.Text = "";
                            ExBackgroundWorker.DoWork_Focus(this.utlID, 10);
                            return;
                        }
                        else
                        {
                            ExVisualTreeHelper.SetEnabled(this.MainDetail, true);

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
                        _entity = new EntitySupplier();

                        ExVisualTreeHelper.SetEnabled(this.MainDetail, true);
                        SetBinding();
                        this.utlFunctionKey.gFunctionKeyEnable = Utl_FunctionKey.geFunctionKeyEnable.New;
                    }
                    this.utlID.txtID_IsReadOnly = true;
                    this.utlCustomer.txtID_IsReadOnly = true;
                    ExBackgroundWorker.DoWork_Focus(this.txtName, 10);
                    break;
                case ExWebService.geWebServiceCallKbn.GetCustomer:
                    // 更新
                    if (objList != null)
                    {
                        EntityCustomer entityCustomer = (EntityCustomer)objList;

                        if (string.IsNullOrEmpty(entityCustomer.message))
                        {
                            string _Id = ExCast.zNumZeroNothingFormat(this.utlID.txtID.Text.Trim());
                            string _Nm = this.utlID.txtNm.Text.Trim();
                            string _customerId = ExCast.zNumZeroNothingFormat(this.utlCustomer.txtID.Text.Trim());
                            string _customerNm = this.utlCustomer.txtNm.Text.Trim();

                            _entity = new EntitySupplier();
                            _entity._id = _Id;
                            _entity._customer_id = _customerId;
                            _entity._customer_nm = _customerNm;
                            _entity._name = entityCustomer._name;
                            _entity._kana = entityCustomer._kana;
                            _entity._about_name = entityCustomer._about_name;
                            //_entity._zip_code = entityCustomer._zip_code;
                            _entity._adress_city = entityCustomer._adress_city;
                            _entity._adress_town = entityCustomer._adress_town;
                            _entity._adress1 = entityCustomer._adress1;
                            _entity._adress2 = entityCustomer._adress2;
                            _entity._station_name = entityCustomer._station_name;
                            _entity._post_name = entityCustomer._post_name;
                            _entity._person_name = entityCustomer._person_name;
                            _entity._title_id = entityCustomer._title_id;
                            _entity._title_name = entityCustomer._title_name;
                            _entity._tel = entityCustomer._tel;
                            _entity._fax = entityCustomer._fax;
                            _entity._mail_adress = entityCustomer._mail_adress;
                            _entity._mobile_tel = entityCustomer._mobile_tel;
                            _entity._mobile_adress = entityCustomer._mobile_adress;
                            _entity._url = entityCustomer._url;

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

        private void btnCustomerCopy_Click(object sender, RoutedEventArgs e)
        {
            object[] prm = new object[1];
            prm[0] = ExCast.zNumZeroNothingFormat(this.utlCustomer.txtID.Text.Trim());
            webService.objPerent = this;
            webService.CallWebService(ExWebService.geWebServiceCallKbn.GetCustomer,
                                      ExWebService.geDialogDisplayFlg.Yes,
                                      ExWebService.geDialogCloseFlg.Yes,
                                      prm);
        }

        #endregion

        #region Master DataSelect

        public override void MstDataSelect(ExWebServiceMst.geWebServiceMstNmCallKbn intKbn, svcMstData.EntityMstData mst)
        {
            switch (intKbn)
            {
                case ExWebServiceMst.geWebServiceMstNmCallKbn.GetCustomer:
                    if (!string.IsNullOrEmpty(this.utlCustomer.txtID.Text.Trim()) && !string.IsNullOrEmpty(this.utlCustomer.txtNm.Text.Trim()))
                    {
                        if (this.utlFunctionKey.gFunctionKeyEnable == Utl_FunctionKey.geFunctionKeyEnable.InitKbn)
                        {
                            ExVisualTreeHelper.SetEnabled(this.MainDetail, true);
                            this.utlCustomer.txtID_IsReadOnly = true;
                            ExBackgroundWorker.DoWork_Focus(this.utlID.txtID, 10);
                            this.utlFunctionKey.gFunctionKeyEnable = Utl_FunctionKey.geFunctionKeyEnable.Init;
                        }
                    }
                    break;
            }
        }

        #endregion

        #region Data Update

        // データ追加・更新・削除
        private void UpdateData(Common.geUpdateType upd)
        {
            object[] prm = new object[4];

            prm[0] = (int)upd;
            prm[1] = ExCast.zNumZeroNothingFormat(this.utlCustomer.txtID.Text.Trim());
            prm[2] = ExCast.zNumZeroNothingFormat(this.utlID.txtID.Text.Trim());
            prm[3] = _entity;
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

                // 請求先
                if (ExCast.zCStr(_entity._customer_id) != "" && string.IsNullOrEmpty(_entity._customer_nm))
                {
                    errMessage += "請求先が適切に入力(選択)されていません。" + Environment.NewLine;
                    if (errCtl == null) errCtl = this.utlCustomer.txtID;
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
