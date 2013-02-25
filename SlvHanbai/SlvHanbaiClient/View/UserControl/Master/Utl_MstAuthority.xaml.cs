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
using SlvHanbaiClient.svcAuthority;
using SlvHanbaiClient.svcMstData;
using SlvHanbaiClient.View.UserControl.Custom;
using SlvHanbaiClient.View.Dlg;

namespace SlvHanbaiClient.View.UserControl.Master
{
    public partial class Utl_MstAuthority : ExUserControl
    {

        #region Filed Const

        private const String CLASS_NM = "Utl_MstAuthority";
        private const String PG_NM = DataPgEvidence.PGName.Mst.Authority;
        private const Common.geWinMsterType _WinMsterType = Common.geWinMsterType.Authority;
        private const ExWebService.geWebServiceCallKbn _GetWebServiceCallKbn = ExWebService.geWebServiceCallKbn.GetAuthority;
        private const ExWebService.geWebServiceCallKbn _UpdWebServiceCallKbn = ExWebService.geWebServiceCallKbn.UpdateAuthority;
        private ObservableCollection<EntityAuthority> _entity = new ObservableCollection<EntityAuthority>();
        private readonly string tableName = "M_AUTHORITY";
        private Control activeControl;
        private Common.geWinGroupType beforeWinGroupType;

        private ObservableCollection<EntityMstList> _entityUserList = null;

        private Brush _brush_background;

        #endregion

        #region Constructor

        public Utl_MstAuthority()
        {
            InitializeComponent();
            this.Tag = "Main";      // ファンクションキーを受けつけ用
        }

        #endregion

        #region Page Events

        private void ExUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (Common.gblnLogin == false) return;

            _brush_background = borAll.Background;

            // 画面初期化
            ExVisualTreeHelper.initDisplay(this.LayoutRoot);

            // ファンクションキー初期設定
            this.utlFunctionKey.gFunctionKeyEnable = Utl_FunctionKey.geFunctionKeyEnable.Init;
            this.utlFunctionKey.Init();

            this.utlDummy.evtDataSelect -= this._evtDataSelect;
            this.utlDummy.evtDataSelect += this._evtDataSelect;

            Init();
        }

        private void Init()
        {
            InitCheck();
            GetUserList();
            IsEnabledCheck(false);            
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
            this.utlFunctionKey.gFunctionKeyEnable = Utl_FunctionKey.geFunctionKeyEnable.Init;

            // 初期化
            _entity = null;
            this.cmbUser.IsEnabled = true;
            InitCheck();
            IsEnabledCheck(false);
            ExBackgroundWorker.DoWork_Focus(this.cmbUser, 10);

            // ロック解除
            DataPgLock.gLockPg(PG_NM, "", (int)DataPgLock.geLockType.UnLock);
        }

        // F3ボタン(検索) クリック
        public override void btnF3_Click(object sender, RoutedEventArgs e)
        {
            InitCheck();
            GetMstData();
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
        }

        #endregion

        #region LostFocus Events

        #endregion

        #region Binding Setting

        private void SetBinding()
        {
        }

        #endregion

        #region Data Select

        #region 権限データ取得

        private void GetMstData()
        {
            object[] prm = new object[2];

            int id = 0;
            for (int i = 0; i <= this._entityUserList.Count - 1; i++)
            {
                if (ExCast.zCStr(this.cmbUser.SelectedValue) == _entityUserList[i].name)
                {
                    id = ExCast.zCInt(_entityUserList[i].attribute1);
                }
            }
            prm[0] = id;

            webService.objPerent = this;
            webService.CallWebService(_GetWebServiceCallKbn,
                                      ExWebService.geDialogDisplayFlg.Yes,
                                      ExWebService.geDialogCloseFlg.Yes,
                                      prm);
        }

        #endregion

        #region 権限データ取得コールバック呼出(ExWebServiceクラスより呼出)

        public override void DataSelect(int intKbn, object objList)
        {
            switch ((ExWebService.geWebServiceCallKbn)intKbn)
            {
                case _GetWebServiceCallKbn:
                    if (objList != null)
                    {
                        _entity = (ObservableCollection<EntityAuthority>)objList;
                        if (_entity.Count == 0) return;

                        if (_entity[0]._lock_flg == 0)
                        {
                            this.utlFunctionKey.gFunctionKeyEnable = Utl_FunctionKey.geFunctionKeyEnable.Upd;
                        }
                        else
                        {
                            this.utlFunctionKey.gFunctionKeyEnable = Utl_FunctionKey.geFunctionKeyEnable.Sel;
                        }
                        this.cmbUser.IsEnabled = false;

                        IsEnabledCheck(true);
                        InitCheck();

                        // チェックセット
                        for (int i = 0; i <= this._entity.Count - 1; i++)
                        {
                            foreach (CheckBox chk in ExVisualTreeHelper.FindVisualChildren<CheckBox>(this.stpAll))
                            {
                                if (ExCast.zCStr(chk.Tag) == _entity[i]._pg_id)
                                {
                                    if (_entity[i]._authority_kbn == 0)
                                    {
                                        chk.IsChecked = false;
                                    }
                                    else
                                    {
                                        chk.IsChecked = true;
                                    }
                                }
                                else if (_entity[i]._pg_id == "EstimateInp" && ExCast.zCStr(chk.Tag) == "EstimateApproval" && _entity[i]._authority_kbn >= 3)
                                {
                                    chk.IsChecked = true;
                                }
                            }
                        }
                    }

                    break;
                default:
                    break;
            }
        }

        #endregion

        #region ユーザーデータ取得

        private void GetUserList()
        {
            object[] prm = new object[3];
            prm[0] = "";
            prm[1] = "";
            ExWebServiceMst webServiceMst = new ExWebServiceMst();
            webServiceMst.objPerent = this.utlDummy;
            webServiceMst.CallWebServiceMst(ExWebServiceMst.geWebServiceMstNmCallKbn.GetUserList,
                                            ExWebService.geDialogDisplayFlg.No,
                                            ExWebService.geDialogCloseFlg.No,
                                            prm);
        }

        #endregion

        #region データ取得コールバック呼出(ExWebServiceクラスより呼出)

        private void _evtDataSelect(int intKbn, object objList)
        {
            if (objList != null)
            {
                _entityUserList = (ObservableCollection<EntityMstList>)objList;
                for (int i = 0; i <= _entityUserList.Count - 1; i++)
                {
                    this.cmbUser.Items.Add(_entityUserList[i].name);
                }
                this.cmbUser.SelectedIndex = 0;
            }

            ExBackgroundWorker.DoWork_Focus(this.cmbUser, 100);
            ExBackgroundWorker.DoWork_Focus(this.cmbUser, 200);
            ExBackgroundWorker.DoWork_Focus(this.cmbUser, 300);
        }

        #endregion

        #endregion

        #region Data Update

        // データ更新
        private void UpdateData()
        {
            object[] prm = new object[2];

            prm[0] = _entity;
            prm[1] = _entity[0]._user_id;
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
                    if (_entity[0]._user_id == Common.gintUserID)
                    {
                        Common.gAuthorityList = _entity;
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

            int chk_cnt = 0;
            string errMessage = "";
            string warnMessage = "";
            Control errCtl = null;

            #endregion

            try
            {
                foreach (CheckBox chk in ExVisualTreeHelper.FindVisualChildren<CheckBox>(this.stpAll))
                {
                    if (chk.IsChecked == true)
                    {
                        chk_cnt += 1;
                    }
                }

                #region 必須チェック

                if (chk_cnt == 0)
                {
                    errMessage += "権限が1件も選択されていません。" + Environment.NewLine;
                }

                #endregion

                #region チェックセット

                if (chk_cnt > 0)
                {
                    foreach (CheckBox chk in ExVisualTreeHelper.FindVisualChildren<CheckBox>(this.stpAll))
                    {
                        for (int i = 0; i <= this._entity.Count - 1; i++)
                        {
                            if (ExCast.zCStr(chk.Tag) == _entity[i]._pg_id)
                            {
                                if (chk.IsChecked == true)
                                {
                                    _entity[i]._authority_kbn = 2;
                                }
                                else
                                {
                                    _entity[i]._authority_kbn = 0;
                                }
                            }
                            else if (ExCast.zCStr(chk.Tag) == "EstimateApproval" && _entity[i]._pg_id == "EstimateInp")
                            {
                                if (chk.IsChecked == true)
                                {
                                    _entity[i]._authority_kbn = 9;
                                }
                            }
                        }
                    }
                }

                #endregion

                #region エラー or 警告時処理

                bool flg = true;

                if (!string.IsNullOrEmpty(errMessage))
                {
                    ExMessageBox.Show(this, errCtl, errMessage, Dlg.MessageBox.MessageBoxIcon.Error);
                    flg = false;
                }

                if (!string.IsNullOrEmpty(warnMessage))
                {
                    warnMessage += "このまま登録を続行してもよろしいですか？" + Environment.NewLine;
                    ExMessageBox.ResultShow(this, errCtl, warnMessage);
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

                #region 更新処理

                UpdateData();

                #endregion

            }
            finally
            {
                Common.gblnBtnProcLock = false;
                Common.gblnBtnDesynchronizeLock = false;
            }
        }

        public override void ResultMessageBox(Control _errCtl)
        {
            if (_errCtl != null)
            {
                ExBackgroundWorker.DoWork_Focus(_errCtl, 10);
            }
        }

        public override void ResultMessageBox(MessageBoxResult _MessageBoxResult, Control _errCtl)
        {
            if (_MessageBoxResult == MessageBoxResult.OK)
            {
                #region 更新処理

                UpdateData();

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

        #region Check Init

        private void InitCheck()
        {
            foreach (CheckBox chk in ExVisualTreeHelper.FindVisualChildren<CheckBox>(this.stpAll))
            {
                chk.IsChecked = false;
            }
        }

        private void CheckRange(StackPanel stp, bool flg)
        {
            foreach (CheckBox chk in ExVisualTreeHelper.FindVisualChildren<CheckBox>(stp))
            {
                chk.IsChecked = flg;
            }
        }

        private void IsEnabledCheck(bool flg)
        {
            if (flg == true)
            {
                this.borAll.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                this.borAll.Visibility = System.Windows.Visibility.Collapsed;
            }

            foreach (CheckBox chk in ExVisualTreeHelper.FindVisualChildren<CheckBox>(this.stpAll))
            {
                chk.IsEnabled = flg;
            }
        }

        #endregion

        #region CheckBox Checked Events

        private void chkAll_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            CheckRange(this.stpAll, (bool)chk.IsChecked);
        }

        private void chkSales_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            CheckRange(this.stpSales, (bool)chk.IsChecked);
        }

        private void chkSalesReport_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            CheckRange(this.stpSalesReport, (bool)chk.IsChecked);
        }

        private void chkPurchase_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            CheckRange(this.stpPurchase, (bool)chk.IsChecked);
        }

        private void chkPurcharseReport_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            CheckRange(this.stpPurchaseReport, (bool)chk.IsChecked);
        }

        private void chkSetting_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            CheckRange(this.stpSetting, (bool)chk.IsChecked);
        }

        private void chkSupport_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            CheckRange(this.stpSupport, (bool)chk.IsChecked);
        }

        private void chkReport_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            CheckRange(this.stpReport, (bool)chk.IsChecked);
        }

        private void chkEstimateApproval_Checked(object sender, RoutedEventArgs e)
        {
            if (this.chkEstimateApproval.IsChecked == true)
            {
                this.chkEstimateInp.IsChecked = true;
            }
        }

        #endregion

    }

}
