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
using System.Windows.Data;
using System.Collections.ObjectModel;  // ObservableCollectionを利用するために必要   
using System.ServiceModel.DomainServices.Client;
using SlvHanbaiClient.Class;
using SlvHanbaiClient.Class.Data;
using SlvHanbaiClient.Class.Utility;
using SlvHanbaiClient.Class.UI;
using SlvHanbaiClient.Class.Data;
using SlvHanbaiClient.Class.WebService;
using SlvHanbaiClient.svcSysLogin;
using SlvHanbaiClient.svcAuthority;
using SlvHanbaiClient.View.Dlg;
using SlvHanbaiClient.View.UserControl.Custom;

#endregion

namespace SlvHanbaiClient.View.UserControl
{
    public partial class Utl_LoginMain : ExUserControl
    {

        #region Filed Const

        private const String CLASS_NM = "Utl_Login";
        private bool blnUpdate = false;
        Dlg_Progress win;
        private bool _StartUpMode = true;
        public bool StartUpMode { set { this._StartUpMode = value; } get { return this._StartUpMode; } }

        #endregion

        #region Constructor

        public Utl_LoginMain()
        {
            InitializeComponent();

            // 更新終了時のイベントハンドラーを登録 
            Application.Current.CheckAndDownloadUpdateCompleted += Current_CheckAndDownloadUpdateCompleted; 
        }

        #endregion

        #region Page Events

        private void ExUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            // 画面初期化
            ExVisualTreeHelper.initDisplay(this.LayoutRoot);

            ExBackgroundWorker.DoWork_Focus(this.txtLoginID, 100);
            ExBackgroundWorker.DoWork_Focus(this.txtLoginID, 200);
            ExBackgroundWorker.DoWork_Focus(this.txtLoginID, 300);
            ExBackgroundWorker.DoWork_Focus(this.txtLoginID, 400);
            ExBackgroundWorker.DoWork_Focus(this.txtLoginID, 500);
        }

        private void ExUserControl_Unloaded(object sender, RoutedEventArgs e)
        {
        }

        #endregion

        #region Method

        #region Input Check

        private bool InputCheck()
        {
            if (this.txtLoginID.Text.Trim() == "")
            {
                ExMessageBox.Show(this, this.txtLoginID, "ログインIDを入力して下さい。");
                return false;
            }
            if (this.txtPass.Password.Trim() == "")
            {
                ExMessageBox.Show(this, this.txtPass, "ログインパスワードを入力して下さい。");
                return false;
            }
            return true;
        }

        public override void ResultMessageBox(Control _errCtl)
        {
            ExBackgroundWorker.DoWork_Focus(_errCtl, 10);
        }

        #endregion

        #region Update

        public void UpdateCheck()
        {
            ExBackgroundWorker.DoWork_Focus(this.txtLoginID, 100);

            // test
            return;

            // ネットワーク接続確認
            //if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable() == false)
            //{
            //    win.Close();
            //    ExMessageBox.Show("ネットワークに接続されていない為、プログラムの最新バージョンを確認できませんでした。" + Environment.NewLine);
            //    blnUpdate = true;
            //    return;
            //}

            //win = new Dlg_Progress();
            //win.Title = "更新プログラム確認中";
            //win.Show();

            ////更新プログラム確認
            //Application.Current.CheckAndDownloadUpdateAsync(); 

        }

        private void Current_CheckAndDownloadUpdateCompleted(object sender, CheckAndDownloadUpdateCompletedEventArgs e)
        {
            if (e.UpdateAvailable)
            {
                ExMessageBox.Show("最新アプリケーションを更新しました、再起動してください。");
                Application.Current.MainWindow.Close();
            }
            else if (e.Error != null && e.Error is PlatformNotSupportedException)
            {
                ExMessageBox.Show("最新Silverligtバージョンを更新してください。");
                blnUpdate = true;
            }
            else
            {
                if (e.Error == null)
                {
                }
                else
                {
                    ExMessageBox.Show(e.Error.Message);
                }
            }

            ExBackgroundWorker.DoWork_Close(win, 2000);
            ExBackgroundWorker.DoWork_Focus(this.txtLoginID, 2100);
        }

        #endregion

        #region ログイン処理

        // ログイン
        private void Login()
        {
            Common.gstrMsgSessionError = "";

            object[] prm = new object[3];
            prm[0] = this.txtLoginID.Text.Trim();
            prm[1] = this.txtPass.Password.Trim();
            prm[2] = 1;
            webService.objPerent = this;
            webService.CallWebService(ExWebService.geWebServiceCallKbn.Login,
                                      ExWebService.geDialogDisplayFlg.Yes,
                                      ExWebService.geDialogCloseFlg.No,
                                      prm);
        }

        #endregion

        #region ログインコールバック呼出(ExWebServiceクラスより呼出)

        public override void DataSelect(int intKbn, object objList)
        {
            Common.gstrMsgSessionError = "";

            ExPage pg;
            Dlg_Login login;
            object[] prm;

            switch ((ExWebService.geWebServiceCallKbn)intKbn)
            { 
                case ExWebService.geWebServiceCallKbn.Login:
                    EntitySysLogin entity = (EntitySysLogin)objList;
                    switch (entity._login_flg)
                    { 
                        case 0:     // 正常ログイン
                            if (Common.gblnStartSettingDlg == false)
                            {
                                Common.gblnStartSettingDlg = true;

                                ExBackgroundWorker.DoWork_StartUpInstanceSet();

                                // 起動時間短縮の為に画面情報を保持しておく
                                Dlg_InpSearch InpSearch = null;
                                Common.dataForm = new View.Dlg.Dlg_DataForm();
                                Common.report = new View.Dlg.Report.Dlg_Report();
                                Common.reportView = new View.Dlg.Report.Dlg_ReportView();
                                InpSearch = Common.InpSearchEstimate;
                                InpSearch = Common.InpSearchOrder;
                                InpSearch = Common.InpSearchSales;
                                InpSearch = Common.InpSearchReceipt;
                                InpSearch = Common.InpSearchPlan;
                                InpSearch = Common.InpSearchInvoice;
                                InpSearch = Common.InpSearchPurchaseOrder;
                                InpSearch = Common.InpSearchPurchase;
                                InpSearch = Common.InpSearchPaymentCash;
                                InpSearch = Common.InpSearchPayment;
                                InpSearch = Common.InpSearchInOutDelivery;
                            }

                            // システム情報設定
                            Common.gintCompanyId = entity._company_id;
                            Common.gstrCompanyNm = entity._company_nm;
                            Common.gintGroupId = entity._group_id;
                            Common.gstrGroupNm = entity._group_nm;
                            Common.gintDefaultPersonId = entity._defult_person_id;
                            Common.gstrDefaultPersonNm = entity._defult_person_nm;
                            Common.gstrGroupDisplayNm = entity._group_display_name;
                            Common.gintEvidenceFlg = entity._evidence_flg;
                            Common.gintidFigureSlipNo = entity._idFigureSlipNo;
                            Common.gintidFigureCustomer = entity._idFigureCustomer;
                            Common.gintidFigurePurchase = entity._idFigurePurchase;
                            Common.gintidFigureCommodity = entity._idFigureGoods;
                            Common.gintEstimateApprovalFlg = entity._estimate_approval_flg;
                            Common.gintReceiptAccountInvoicePringFlg = entity._receipt_account_invoice_print_flg;
                            Common.gstrSessionString = entity._session_string;
                            Common.gintUserID = entity._user_id;
                            Common.gstrUserNm = entity._user_nm;
                            Common.gstrSessionString = entity._session_string;
                            Common.gstrLoginUserID = this.txtLoginID.Text.Trim();;
                            Common.gstrLoginPassword = this.txtPass.Password.Trim();
                            Common.gintDemoFlg = entity._demo_flg;
                            Common.gstrSystemVer = entity._sys_ver;
                            Common.gblnLogin = true;

                            // System開始時の証跡を保存
                            DataPgEvidence.SaveLoadOrUnLoadEvidence(DataPgEvidence.PGName.System, DataPgEvidence.geOperationType.Start);

                            // 名称リスト取得
                            prm = new object[1];
                            prm[0] = "";
                            webService.CallWebService(ExWebService.geWebServiceCallKbn.GetNameList,
                                                      ExWebService.geDialogDisplayFlg.No,
                                                      ExWebService.geDialogCloseFlg.No,
                                                      prm);
                            break;
                        case 1:     // 同一ユーザーログイン
                            // 権限リスト取得
                            prm = new object[1];
                            prm[0] = Common.gintUserID;
                            webService.CallWebService(ExWebService.geWebServiceCallKbn.GetAuthority,
                                                      ExWebService.geDialogDisplayFlg.No,
                                                      ExWebService.geDialogCloseFlg.Yes,
                                                      prm);
                            //webService.ProcessingDlgClose();
                            //pg = (ExPage)ExVisualTreeHelper.FindPerentPage(this);
                            //if (pg != null)
                            //{
                            //    UA_Main main = (UA_Main)pg;
                            //    Common.gPageGroupType = Common.gePageGroupType.Menu;
                            //    Common.gPageType = Common.gePageType.Menu;
                            //    main.ChangePage();
                            //    return;
                            //}
                            //login = (Dlg_Login)ExVisualTreeHelper.FindPerentChildWindow(this);
                            //login.Close();
                            break;
                        default:    // ログイン失敗
                            webService.ProcessingDlgClose();
                            ExMessageBox.Show(this, this.txtLoginID, entity._login_message);
                            return;
                    }

                    break;
                case ExWebService.geWebServiceCallKbn.GetNameList:
                    Common.gNameList = new MeiNameList(objList);

                    // 権限リスト取得
                    prm = new object[1];
                    prm[0] = Common.gintUserID;
                    webService.CallWebService(ExWebService.geWebServiceCallKbn.GetAuthority,
                                              ExWebService.geDialogDisplayFlg.No,
                                              ExWebService.geDialogCloseFlg.Yes,
                                              prm);
                    break;
                case ExWebService.geWebServiceCallKbn.GetAuthority:
                    Common.gAuthorityList = (ObservableCollection<EntityAuthority>)objList;

                    pg = (ExPage)ExVisualTreeHelper.FindPerentPage(this);
                    if (pg != null)
                    {
                        if (Common.gstrSystemVer != "" && Common.gstrSystemVer != Common.gstrClinetVer)
                        {
                            ExMessageBox.Show("システムのバージョン Ver." + Common.gstrSystemVer + " がリリースされています。" + Environment.NewLine + 
                                              "(現在 Ver." + Common.gstrClinetVer + ")" + Environment.NewLine + 
                                              "一旦アンインストールして再インストールして下さい。");
                        }

                        UA_Main main = (UA_Main)pg;
                        Common.gPageGroupType = Common.gePageGroupType.Menu;
                        Common.gPageType = Common.gePageType.Menu;
                        main.ChangePage();
                        return;
                    }
                    login = (Dlg_Login)ExVisualTreeHelper.FindPerentChildWindow(this);
                    login.Close();
                    break;
            }
        }

        #endregion

        #endregion

        #region Button Events

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            // ネットワーク接続確認
            if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable() == false)
            {
                ExMessageBox.Show("ネットワークに接続されていない為、ログイン認証が実行できません。" + Environment.NewLine);
                return;
            }

            if (InputCheck() == false) return;

            // ログイン処理
            Login();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (this.StartUpMode == true)
            {
                ExMessageBox.ResultShow(this, null, "アプリケーションを終了します。" + Environment.NewLine + "よろしいですか？");
                //if (ExMessageBox.ResultShow("アプリケーションを終了します。" + Environment.NewLine + "よろしいですか？") == MessageBoxResult.OK)
                //{
                //    if (Application.Current.IsRunningOutOfBrowser)
                //        Application.Current.MainWindow.Close();
                //}
            }
            else
            {
                Dlg_Login win = (Dlg_Login)ExVisualTreeHelper.FindPerentChildWindow(this.Parent);
                if (win != null)
                {
                    win.Close();
                }
            }
        }

        public override void ResultMessageBox(MessageBoxResult _MessageBoxResult, Control _errCtl)
        {
            if (_MessageBoxResult == MessageBoxResult.OK)
            {
                if (Application.Current.IsRunningOutOfBrowser)
                    Application.Current.MainWindow.Close();
            }
        }

        #endregion

        private void txtLoginID_GotFocus(object sender, RoutedEventArgs e)
        {
            if (this.txtLoginID.Text.Trim().Length == 0) return;
            this.txtLoginID.SelectAll();
        }

        private void txtPass_GotFocus(object sender, RoutedEventArgs e)
        {
            if (this.txtPass.Password.Trim().Length == 0) return;
            this.txtPass.SelectAll();
        }

    }
}
