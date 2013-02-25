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
using SlvHanbaiClient.svcUser;
using SlvHanbaiClient.svcMstData;
using SlvHanbaiClient.View.UserControl.Custom;
using SlvHanbaiClient.View.Dlg;
using SlvHanbaiClient.View.UserControl.Report;

namespace SlvHanbaiClient.View.UserControl.Master
{
    public partial class Utl_MstUser : ExUserControl
    {

        #region Filed Const

        private const String CLASS_NM = "Utl_MstUser";
        private const String PG_NM = DataPgEvidence.PGName.Mst.User;
        private readonly Common.geWinMsterType _WinMsterType = Common.geWinMsterType.User;
        private const ExWebService.geWebServiceCallKbn _GetWebServiceCallKbn = ExWebService.geWebServiceCallKbn.GetUser;
        private const ExWebService.geWebServiceCallKbn _UpdWebServiceCallKbn = ExWebService.geWebServiceCallKbn.UpdateUser;
        private Class.Data.MstData.geMDataKbn MstKbn = Class.Data.MstData.geMDataKbn.User;
        private EntityUser _entity = new EntityUser();
        private readonly string tableName = "SYS_M_USER";
        private Control activeControl;
        private Dlg_Copying searchDlg;
        private Common.geWinGroupType beforeWinGroupType;

        private ObservableCollection<EntityMstList> _entityUserList = null;

        private Utl_Report utlReport = new Utl_Report();
        private bool flg_relogin = false;
        
        #endregion

        #region Constructor

        public Utl_MstUser()
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
            ExVisualTreeHelper.SetEnabled(this.stpInput, false);
            this.cmbLoginId.IsEnabled = true;
            this.utlFunctionKey.Init();

            this.utlDummy.evtDataSelect -= this._evtDataSelect;
            this.utlDummy.evtDataSelect += this._evtDataSelect;

            this.lblCompanyGroup.Content = Common.gstrGroupDisplayNm;

            // バインド設定
            SetBinding();

            GetUserList();
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
            ExVisualTreeHelper.SetEnabled(this.stpInput, false);

            GetUserList();

            // ロック解除
            DataPgLock.gLockPg(PG_NM, "", (int)DataPgLock.geLockType.UnLock);
        }

        // F3ボタン(複写) クリック
        public override void btnF3_Click(object sender, RoutedEventArgs e)
        {
            if (_entity == null) return;

        }

        // F4ボタン(削除) クリック
        public override void btnF4_Click(object sender, RoutedEventArgs e)
        {
        }
        
        // F5ボタン(参照) クリック
        public override void btnF5_Click(object sender, RoutedEventArgs e)
        {
            if (activeControl == null) return;
            switch (activeControl.Name)
            {
                case "utlID":
                case "utlCompanyGroup":
                case "utlPerson":                    
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
                case "utlCompanyGroup":
                case "utlDisplay":
                case "utlPerson":                    
                    ExVisualTreeHelper.SetFunctionKeyEnabled("F5", true, this);
                    activeControl = ctl;
                    break;
                case "txtName":
                case "cmbLoginId":
                case "txtLoginId":
                case "txtLoginPassword":
                case "txtLoginPasswordConfirm":
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

        #endregion

        #region Combobox SelectionChanged

        private void cmbLoginId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.cmbLoginId.SelectedIndex == -1) return;
            GetMstData();
        }

        #endregion

        #region Binding Setting

        private void SetBinding()
        {
            if (_entity == null)
            {
                _entity = new EntityUser();
            }

            // マスタコントロールPropertyChanged
            _entity.PropertyChanged += this.utlCompanyGroup.MstID_Changed;
            _entity.PropertyChanged += this.utlPerson.MstID_Changed;

            NumberConverter nmConvDecm0 = new NumberConverter();

            // バインド
            Binding BindingAfterLoginId = new Binding("_after_login_id");
            BindingAfterLoginId.Mode = BindingMode.TwoWay;
            BindingAfterLoginId.Source = _entity;
            this.txtLoginId.SetBinding(TextBox.TextProperty, BindingAfterLoginId);

            Binding BindingLoginPassword = new Binding("_login_password");
            BindingLoginPassword.Mode = BindingMode.TwoWay;
            BindingLoginPassword.Source = _entity;
            this.txtLoginPassword.SetBinding(PasswordBox.PasswordProperty, BindingLoginPassword);
            this.txtLoginPasswordConfirm.Password = "";

            Binding BindingName = new Binding("_name");
            BindingName.Mode = BindingMode.TwoWay;
            BindingName.Source = _entity;
            this.txtName.SetBinding(TextBox.TextProperty, BindingName);

            Binding BindingGroupId = new Binding("_group_id");
            BindingGroupId.Mode = BindingMode.TwoWay;
            BindingGroupId.Source = _entity;
            this.utlCompanyGroup.txtID.SetBinding(TextBox.TextProperty, BindingGroupId);

            Binding BindingGroupName = new Binding("_group_nm");
            BindingGroupName.Mode = BindingMode.TwoWay;
            BindingGroupName.Source = _entity;
            this.utlCompanyGroup.txtNm.SetBinding(TextBox.TextProperty, BindingGroupName);

            Binding BindingPersonId = new Binding("_person_id");
            BindingPersonId.Mode = BindingMode.TwoWay;
            BindingPersonId.Source = _entity;
            this.utlPerson.txtID.SetBinding(TextBox.TextProperty, BindingPersonId);

            Binding BindingPersonNm = new Binding("_person_nm");
            BindingPersonNm.Mode = BindingMode.TwoWay;
            BindingPersonNm.Source = _entity;
            this.utlPerson.txtNm.SetBinding(TextBox.TextProperty, BindingPersonNm);

            Binding BindingDisplayDivisionId = new Binding("_display_division_id");
            BindingDisplayDivisionId.Mode = BindingMode.TwoWay;
            BindingDisplayDivisionId.Source = _entity;
            this.utlDisplay.txtID.SetBinding(TextBox.TextProperty, BindingDisplayDivisionId);

            Binding BindingDisplayDivisionNm = new Binding("_display_division_nm");
            BindingDisplayDivisionNm.Mode = BindingMode.TwoWay;
            BindingDisplayDivisionNm.Source = _entity;
            this.utlDisplay.txtNm.SetBinding(TextBox.TextProperty, BindingDisplayDivisionNm);

            Binding BindingMemo = new Binding("_memo");
            BindingMemo.Mode = BindingMode.TwoWay;
            BindingMemo.Source = _entity;
            this.txtMemo.SetBinding(TextBox.TextProperty, BindingMemo);

            this.utlCompanyGroup.txtID.OnFormatString();
            this.utlPerson.txtID.OnFormatString();

            if (ExCast.zCInt(_entity._id) == 0)
            {
                _entity._display_division_id = 1;
            }

        }

        #endregion

        #region Data Select

        #region データ取得

        private void GetMstData()
        {
            object[] prm = new object[1];

            prm[0] = "";
            for (int i = 0; i <= _entityUserList.Count - 1; i++)
            {
                if (ExCast.zCStr(this.cmbLoginId.SelectedValue) == _entityUserList[i].id)
                {
                    prm[0] = ExCast.zCInt(_entityUserList[i].attribute1);
                }
            }
            if (ExCast.zCStr(prm[0]) == "") return;

            webService.objPerent = this;
            webService.CallWebService(_GetWebServiceCallKbn,
                                      ExWebService.geDialogDisplayFlg.Yes,
                                      ExWebService.geDialogCloseFlg.Yes,
                                      prm);
        }

        private void GetUserList()
        {
            object[] prm = new object[3];
            prm[0] = "";
            prm[1] = "";
            prm[2] = "";
            ExWebServiceMst webServiceMst = new ExWebServiceMst();
            webServiceMst.objPerent = this.utlDummy;
            webServiceMst.CallWebServiceMst(ExWebServiceMst.geWebServiceMstNmCallKbn.GetUserList,
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
                        _entity = (EntityUser)objList;

                        if (_entity.message != "" && _entity.message != null)
                        {
                            return;
                        }
                        else
                        {
                            // バインド反映
                            SetBinding();

                            if (_entity._lock_flg == 0)
                            {
                                this.utlFunctionKey.gFunctionKeyEnable = Utl_FunctionKey.geFunctionKeyEnable.Upd;
                                ExVisualTreeHelper.SetEnabled(this.stpInput, true);
                                this.cmbLoginId.IsEnabled = false;
                                ExBackgroundWorker.DoWork_Focus(this.txtLoginId, 10);
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
                    }
                    break;
                default:
                    break;
            }
        }

        private void _evtDataSelect(int intKbn, object objList)
        {
            this.cmbLoginId.Items.Clear();
            if (objList != null)
            {
                _entityUserList = (ObservableCollection<EntityMstList>)objList;
                for (int i = 0; i <= _entityUserList.Count - 1; i++)
                {
                    this.cmbLoginId.Items.Add(_entityUserList[i].id);
                }
                this.cmbLoginId.SelectedIndex = -1;
            }
            else
            {
                ExMessageBox.Show("ログインユーザーリストの取得に失敗しました。");
                this.btnF12_Click(null, null);
            }
            ExBackgroundWorker.DoWork_FocusForLoad(this.cmbLoginId);
        }

        #endregion

        #endregion

        #region Data Update

        // データ追加・更新・削除
        private void UpdateData(Common.geUpdateType upd)
        {
            object[] prm = new object[4];

            prm[0] = (int)upd;

            prm[1] = "";
            for (int i = 0; i <= _entityUserList.Count - 1; i++)
            {
                if (ExCast.zCStr(this.cmbLoginId.SelectedValue) == _entityUserList[i].id)
                {
                    prm[1] = ExCast.zCInt(_entityUserList[i].attribute1);
                }
            }
            if (ExCast.zCStr(prm[1]) == "") return;

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
                    if (Common.gintUserID == _entity._id)
                    {
                        Common.gstrLoginUserID = _entity._after_login_id;
                        Common.gstrLoginPassword = _entity._login_password;
                        Common.gstrUserNm = _entity._name;
                        Common.gintDefaultPersonId = _entity._person_id;
                        Common.gstrDefaultPersonNm = _entity._person_nm;
                        Common.gintGroupId = _entity._group_id;
                        Common.gstrGroupNm = _entity._group_nm;
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

                // ID
                if (this.cmbLoginId.SelectedIndex == -1)
                {
                    errMessage += "ログインIDが選択されていません。" + Environment.NewLine;
                    if (errCtl == null) errCtl = this.cmbLoginId;
                }

                // 変更ログインID
                if (string.IsNullOrEmpty(_entity._after_login_id))
                {
                    errMessage += "変更ログインIDが入力されていません。" + Environment.NewLine;
                    if (errCtl == null) errCtl = this.txtLoginId;
                }

                // ログインパスワード
                if (string.IsNullOrEmpty(_entity._login_password))
                {
                    errMessage += "ログインパスワードが入力されていません。" + Environment.NewLine;
                    if (errCtl == null) errCtl = this.txtLoginId;
                }

                // ログインパスワード確認
                if (string.IsNullOrEmpty(txtLoginPasswordConfirm.Password))
                {
                    errMessage += "ログインパスワード確認が入力されていません。" + Environment.NewLine;
                    if (errCtl == null) errCtl = this.txtLoginId;
                }

                // 名称
                if (string.IsNullOrEmpty(_entity._name))
                {
                    errMessage += "ユーザー名が入力されていません。" + Environment.NewLine;
                    if (errCtl == null) errCtl = this.txtName;
                }

                // 会社グループ
                if (string.IsNullOrEmpty(ExCast.zCStr(_entity._group_id)))
                {
                    errMessage += "グループが入力(選択)されていません。" + Environment.NewLine;
                    if (errCtl == null) errCtl = this.utlCompanyGroup.txtID;
                }

                // デフォルト入力担当
                if (string.IsNullOrEmpty(ExCast.zCStr(_entity._person_id)))
                {
                    errMessage += "デフォルト入力担当が入力(選択)されていません。" + Environment.NewLine;
                    if (errCtl == null) errCtl = this.utlPerson.txtID;
                }

                //// 表示区分
                //if (string.IsNullOrEmpty(ExCast.zCStr(_entity._display_division_id)))
                //{
                //    errMessage += "表示区分が入力(選択)されていません。" + Environment.NewLine;
                //    if (errCtl == null) errCtl = this.utlDisplay.txtID;
                //}

                #endregion

                #region 適正値入力チェック

                // パスワード
                if (this.txtLoginPassword.Password != this.txtLoginPasswordConfirm.Password)
                {
                    errMessage += "パスワードが一致しません。" + Environment.NewLine;
                    if (errCtl == null) errCtl = this.txtLoginPasswordConfirm;
                }

                // 会社グループ
                if (ExCast.zCInt(_entity._group_id) != 0 && string.IsNullOrEmpty(_entity._group_nm))
                {
                    errMessage += "会社グループが適切に入力(選択)されていません。" + Environment.NewLine;
                    if (errCtl == null) errCtl = this.utlCompanyGroup.txtID;
                }

                // 担当
                if (ExCast.zCInt(_entity._person_id) != 0 && string.IsNullOrEmpty(_entity._person_nm))
                {
                    errMessage += "デフォルト入力担当が適切に入力(選択)されていません。" + Environment.NewLine;
                    if (errCtl == null) errCtl = this.utlPerson.txtID;
                }

                // 表示区分
                if (ExCast.zCInt(_entity._display_division_id) != 0 && string.IsNullOrEmpty(_entity._display_division_nm))
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

                #endregion

                #region 正数チェック

                #endregion

                #region 範囲チェック

                if (ExString.LenB(_entity._after_login_id) < 4)
                {
                    errMessage += "変更ログインIDには半角英数4桁以上を入力して下さい。" + Environment.NewLine;
                    if (errCtl == null) errCtl = this.txtLoginId;
                }

                if (ExString.LenB(_entity._after_login_id) > 10)
                {
                    errMessage += "変更ログインIDには半角英数10桁以内を入力して下さい。" + Environment.NewLine;
                    if (errCtl == null) errCtl = this.txtLoginId;
                }

                if (ExString.LenB(_entity._login_password) < 4)
                {
                    errMessage += "ログインパスワードには半角英数4桁以上を入力して下さい。" + Environment.NewLine;
                    if (errCtl == null) errCtl = this.txtLoginPassword;
                }

                if (ExString.LenB(_entity._login_password) > 10)
                {
                    errMessage += "ログインパスワードには半角英数20桁以内を入力して下さい。" + Environment.NewLine;
                    if (errCtl == null) errCtl = this.txtLoginPassword;
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
                        //UpdateData(Common.geUpdateType.Insert);
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
                        //UpdateData(Common.geUpdateType.Insert);
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

        #endregion

    }

}
