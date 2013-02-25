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
using SlvHanbaiClient.svcPerson;
using SlvHanbaiClient.View.UserControl.Custom;
using SlvHanbaiClient.View.Dlg;

namespace SlvHanbaiClient.View.UserControl.Master
{
    public partial class Utl_MstPerson : ExUserControl
    {

        #region Filed Const

        private const String CLASS_NM = "Utl_MstPerson";
        private const String PG_NM = DataPgEvidence.PGName.Mst.Person;
        private readonly Common.geWinMsterType _WinMsterType = Common.geWinMsterType.Person;
        private const ExWebService.geWebServiceCallKbn _GetWebServiceCallKbn = ExWebService.geWebServiceCallKbn.GetPerson;
        private const ExWebService.geWebServiceCallKbn _UpdWebServiceCallKbn = ExWebService.geWebServiceCallKbn.UpdatePerson;
        private Class.Data.MstData.geMDataKbn MstKbn = Class.Data.MstData.geMDataKbn.Person;
        private EntityPerson _entity = new EntityPerson();
        private readonly string tableName = "M_PERSON";
        private Control activeControl;
        private Dlg_Copying searchDlg;
        private Common.geWinGroupType beforeWinGroupType;

        private bool flg_relogin = false;

        #endregion

        #region Constructor

        public Utl_MstPerson()
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

            this.lblCompanyGroup.Content = Common.gstrGroupDisplayNm;

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
                        _id = ExCast.zCDbl(_id).ToString();
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
                case "utlID":
                case "utlCompanyGroup":
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
                case "utlCompanyGroup":
                case "utlDisplay":
                    ExVisualTreeHelper.SetFunctionKeyEnabled("F5", true, this);
                    activeControl = ctl;
                    break;
                case "txtName":
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
                GetMstData(this.utlID.txtID.Text.Trim());
            }
        }

        #endregion

        #region Binding Setting

        private void SetBinding()
        {
            if (_entity == null)
            {
                _entity = new EntityPerson();
            }

            // マスタコントロールPropertyChanged
            _entity.PropertyChanged += this.utlCompanyGroup.MstID_Changed;

            NumberConverter nmConvDecm0 = new NumberConverter();

            // バインド
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

            this.utlID.txtID.SetZeroToNullString();

            // 初期値設定
            if (ExCast.zCInt(this.utlCompanyGroup.txtID.Text.Trim()) == 0)
            {
                this.utlCompanyGroup.MstID_Changed(null, new PropertyChangedEventArgs("_group_id"));
            }
            if (ExCast.zCInt(_entity._id) == 0)
            {
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
                        _entity = (EntityPerson)objList;

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
                        _entity = new EntityPerson();
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
            prm[1] = ExCast.zCLng(this.utlID.txtID.Text.Trim());
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
                    if (_entity._id == Common.gintDefaultPersonId)
                    {
                        Common.gstrDefaultPersonNm = _entity._name;
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

                //// 会社グループ
                //if (string.IsNullOrEmpty(ExCast.zCStr(_entity._group_id)))
                //{
                //    errMessage += "会社グループが入力(選択)されていません。" + Environment.NewLine;
                //    if (errCtl == null) errCtl = this.utlCompanyGroup.txtID;
                //}

                // 表示区分
                if (string.IsNullOrEmpty(ExCast.zCStr(_entity._display_division_id)))
                {
                    errMessage += "表示区分が入力(選択)されていません。" + Environment.NewLine;
                    if (errCtl == null) errCtl = this.utlDisplay.txtID;
                }

                #endregion

                #region 適正値入力チェック

                // 会社グループ
                if (string.IsNullOrEmpty(_entity._group_id) == false && string.IsNullOrEmpty(_entity._group_nm))
                {
                    errMessage += "会社グループが適切に入力(選択)されていません。" + Environment.NewLine;
                    if (errCtl == null) errCtl = this.utlCompanyGroup.txtID;
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

                if (this.utlMode.Mode != Utl_FunctionKey.geFunctionKeyEnable.Init)
                {
                    if (ExCast.IsNumeric(this.utlID.txtID.Text.Trim()) == false)
                    {
                        errMessage += "IDには数値を入力して下さい。" + Environment.NewLine;
                    }
                }

                #endregion

                #region 正数チェック

                if (this.utlMode.Mode != Utl_FunctionKey.geFunctionKeyEnable.Init)
                {
                    if (ExCast.zCLng(this.utlID.txtID.Text.Trim()) < 0)
                    {
                        errMessage += "IDには正の整数を入力して下さい。" + Environment.NewLine;
                    }
                }

                #endregion

                #region 範囲チェック

                if (ExCast.zCLng(this.utlID.txtID.Text.Trim()) > 9999)
                {
                    errMessage += "IDには4桁の正の整数を入力して下さい。" + Environment.NewLine;
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

    }

}
