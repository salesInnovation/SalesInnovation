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
using SlvHanbaiClient.Class;
using SlvHanbaiClient.Class.UI;
using SlvHanbaiClient.Class.Data;
using SlvHanbaiClient.Class.WebService;
using SlvHanbaiClient.View.Dlg;
using SlvHanbaiClient.View.UserControl;
using SlvHanbaiClient.View.UserControl.Custom;
using SlvHanbaiClient.View.UserControl.Input;
using SlvHanbaiClient.View.UserControl.Input.Sales;
using SlvHanbaiClient.View.UserControl.Input.Purchase;
using SlvHanbaiClient.View.UserControl.Input.Inventory;

#endregion

namespace SlvHanbaiClient.View
{
    public partial class UA_Main : ExPage
    {
        #region Filed Const

        private const String CLASS_NM = "UA_Main";
        private ExUserControl _page = null;
        public Page naviPage = null;
        private Common.gePageGroupType beforePageGroupType;
        private Common.gePageType beforePageType;

        #endregion

        #region Constructor

        public UA_Main()
        {
            InitializeComponent();
        }

        #endregion

        #region Page Events

        // 画面ロード時
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.UserCtlFooter.perentPage = this;

            ChangePage();

            ExVisualTreeHelper.SetResource(this.LayoutRoot);
        }

        // 画面キーアップ時
        private void Page_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {                    
                // ファンクションキーを受け取る
                //case Key.F1: this.UserCtlFKey.btnFKey_Click(this.UserCtlFKey.btnF1, null); break;
                //case Key.F2: this.UserCtlFKey.btnFKey_Click(this.UserCtlFKey.btnF2, null); break;
                //case Key.F3: this.UserCtlFKey.btnFKey_Click(this.UserCtlFKey.btnF3, null); break;
                //case Key.F4: this.UserCtlFKey.btnFKey_Click(this.UserCtlFKey.btnF4, null); break;
                //case Key.F5: this.UserCtlFKey.btnFKey_Click(this.UserCtlFKey.btnF5, null); break;
                //case Key.F6: this.UserCtlFKey.btnFKey_Click(this.UserCtlFKey.btnF6, null); break;
                //case Key.F7: this.UserCtlFKey.btnFKey_Click(this.UserCtlFKey.btnF7, null); break;
                //case Key.F8: this.UserCtlFKey.btnFKey_Click(this.UserCtlFKey.btnF8, null); break;
                //case Key.F9: this.UserCtlFKey.btnFKey_Click(this.UserCtlFKey.btnF9, null); break;
                //case Key.F11: this.UserCtlFKey.btnFKey_Click(this.UserCtlFKey.btnF11, null); break;
                //case Key.F12: this.UserCtlFKey.btnFKey_Click(this.UserCtlFKey.btnF12, null); break;
                default: break;
            }
        }

        private void ExPage_Unloaded(object sender, RoutedEventArgs e)
        {
            if (Common.gblnLogin == true)
            {
                // System終了時の証跡を保存(System開始時の証跡はログイン時)
                DataPgEvidence.SaveLoadOrUnLoadEvidence(DataPgEvidence.PGName.System, DataPgEvidence.geOperationType.End);
            }
        }

        // ユーザーがこのページに移動したときに実行されます。
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        #endregion

        #region Method
                
        // ページ切り替え
        public void ChangePage()
        {
            this.GirdMenu.Visibility = Visibility.Collapsed;
            this.GirdMain.Visibility = Visibility.Collapsed;
            this.GirdStartUp.Visibility = Visibility.Collapsed;

            // ヘッダー部
            this.txtLoginInf.Visibility = Visibility.Visible;
            this.lblLogOff.Visibility = Visibility.Visible;
            this.lblEnd.Visibility = Visibility.Visible;

            for (int i = 0; i <= this.GirdStartUp.Children.Count - 1; i++)
            {
                this.GirdStartUp.Children.RemoveAt(0);
            }

            switch (Common.gPageGroupType)
            { 
                case Common.gePageGroupType.StartUp:
                    // ヘッダー部非表示
                    this.txtLoginInf.Visibility = Visibility.Collapsed;
                    this.lblLogOff.Visibility = Visibility.Collapsed;
                    this.lblEnd.Visibility = Visibility.Collapsed;

                    this.GirdStartUp.Visibility = Visibility.Visible;
                    for (int i = 0; i <= this.GirdStartUp.Children.Count - 1; i++)
                    {
                        this.GirdStartUp.Children.RemoveAt(0);
                    }
                    switch (Common.gPageType)
                    {
                        case Common.gePageType.Install:
                            // test
                            //this.GirdStartUp.Children.Add(new Utl_Login());
                            this.GirdStartUp.Children.Add(new Utl_Install());
                            this.listDisplayTabIndex = ExVisualTreeHelper.GetDisplayTabIndex(this.GirdStartUp); // Tab Index 保持
                            break;
                        case Common.gePageType.Login:
                            this.GirdStartUp.Children.Add(new Utl_Login());
                            this.listDisplayTabIndex = ExVisualTreeHelper.GetDisplayTabIndex(this.GirdStartUp); // Tab Index 保持
                            break;
                        default:
                            break;
                    }
                    break;
                case Common.gePageGroupType.Menu:
                    // ヘッダー情報設定
                    SetHeaderInf();

                    // Page UnLoad時の証跡を保存
                    DataPgEvidence.SaveLoadOrUnLoadEvidence(beforePageGroupType, beforePageType, DataPgEvidence.geOperationType.End);

                    this.GirdMenu.Visibility = Visibility.Visible;
                    this.GirdMenu.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                    this.utlMenu.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                    this.utlMenu.DisplayChange();
                    this.listDisplayTabIndex = ExVisualTreeHelper.GetDisplayTabIndex(this.GirdMenu); // Tab Index 保持
                    break;
                case Common.gePageGroupType.Inp:
                    this.GirdMain.Visibility = Visibility.Visible;

                    for (int i = 0; i <= this.utlInpMain.InpDetail.Children.Count - 1; i++)
                    {
                        this.utlInpMain.InpDetail.Children.RemoveAt(0);
                    }

                    switch (Common.gPageType)
                    {
                        #region 売上入力系

                        case Common.gePageType.InpSales:
                            Utl_InpSales utlSales = new Utl_InpSales();
                            utlSales.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                            this.utlInpMain.InpDetail.Children.Add(utlSales);
                            this.utlInpMain.UserCtlFKey.Init();
                            break;
                        case Common.gePageType.InpOrder:
                            Utl_InpOrder utlOrder = new Utl_InpOrder();
                            utlOrder.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                            this.utlInpMain.InpDetail.Children.Add(utlOrder);
                            this.utlInpMain.UserCtlFKey.Init();
                            break;
                        case Common.gePageType.InpEstimate:
                            Utl_InpEstimate utlEstimate = new Utl_InpEstimate();
                            utlEstimate.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                            this.utlInpMain.InpDetail.Children.Add(utlEstimate);
                            this.utlInpMain.UserCtlFKey.Init();
                            break;
                        case Common.gePageType.InpInvoiceClose:
                            Utl_InpInvoiceClose utlInvoiceClose = new Utl_InpInvoiceClose();
                            utlInvoiceClose.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                            this.utlInpMain.InpDetail.Children.Add(utlInvoiceClose);
                            this.utlInpMain.UserCtlFKey.Init();
                            break;
                        case Common.gePageType.InpReceipt:
                            Utl_InpReceipt utlReceipt = new Utl_InpReceipt();
                            utlReceipt.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                            this.utlInpMain.InpDetail.Children.Add(utlReceipt);
                            this.utlInpMain.UserCtlFKey.Init();
                            break;

                        #endregion

                        #region 仕入入力系

                        case Common.gePageType.InpPurchaseOrder:
                            Utl_InpPurchaseOrder utlPurchaseOrder = new Utl_InpPurchaseOrder();
                            utlPurchaseOrder.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                            this.utlInpMain.InpDetail.Children.Add(utlPurchaseOrder);
                            this.utlInpMain.UserCtlFKey.Init();
                            break;
                        case Common.gePageType.InpPurchase:
                            Utl_InpPurchase utlPurchase = new Utl_InpPurchase();
                            utlPurchase.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                            this.utlInpMain.InpDetail.Children.Add(utlPurchase);
                            this.utlInpMain.UserCtlFKey.Init();
                            break;
                        case Common.gePageType.InpPaymentClose:
                            Utl_InpPaymentClose utlPaymentClose = new Utl_InpPaymentClose();
                            utlPaymentClose.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                            this.utlInpMain.InpDetail.Children.Add(utlPaymentClose);
                            this.utlInpMain.UserCtlFKey.Init();
                            break;
                        case Common.gePageType.InpPaymentCash:
                            Utl_InpPaymentCash utlPaymentCash = new Utl_InpPaymentCash();
                            utlPaymentCash.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                            this.utlInpMain.InpDetail.Children.Add(utlPaymentCash);
                            this.utlInpMain.UserCtlFKey.Init();
                            break;

                        #endregion

                        #region 入出庫入力系

                        case Common.gePageType.InpInOutDelivery:
                            Utl_InpInOutDelivery utlInOutDelivery = new Utl_InpInOutDelivery();
                            utlInOutDelivery.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                            this.utlInpMain.InpDetail.Children.Add(utlInOutDelivery);
                            this.utlInpMain.UserCtlFKey.Init();
                            break;

                        case Common.gePageType.InpStockInventory:
                            Utl_InpStockInventory utlStockInventory = new Utl_InpStockInventory();
                            utlStockInventory.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                            this.utlInpMain.InpDetail.Children.Add(utlStockInventory);
                            this.utlInpMain.UserCtlFKey.Init();
                            break;

                        #endregion

                        default:
                            break;
                    }
                    this.listDisplayTabIndex = ExVisualTreeHelper.GetDisplayTabIndex(this.GirdMain); // Tab Index 保持
                    break;
                default:
                    break;
            }
            GC.Collect();

            // 前回ページ情報を保持する
            beforePageGroupType = Common.gPageGroupType;
            beforePageType = Common.gPageType;

            // Page Load時の証跡を保存
            DataPgEvidence.SaveLoadOrUnLoadEvidence(Common.gPageGroupType, Common.gPageType, DataPgEvidence.geOperationType.Start);
        }

        public void SetHeaderInf()
        {
            this.tbkVer.Text = "Ver." + Common.gstrClinetVer;

            this.txtLoginInf.Text = "[" + Common.gstrGroupDisplayNm + "：" + Common.gstrGroupNm + "]  [ﾛｸﾞｲﾝﾕｰｻﾞ：" + Common.gstrUserNm + "]";
            //this.txtLoginInf.Text = "[会社：" + Common.gstrCompanyNm + "]  [" + Common.gstrGroupDisplayNm + "：" + Common.gstrGroupNm + "]  [ﾛｸﾞｲﾝﾕｰｻﾞ：" + Common.gstrUserNm + "]";

            if (Common.gintDemoFlg == 0) this.tbkDemo.Visibility = System.Windows.Visibility.Collapsed;
            else this.tbkDemo.Visibility = System.Windows.Visibility.Visible;
        }

        #endregion

        #region Menu Button Events

        private void lblMenu_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            NaviPage naviPage = (NaviPage)this.Parent;
            naviPage.PageBack();
        }

        public override void btnF12_Click(object sender, RoutedEventArgs e)
        {
            ChangePage();
            //NaviPage naviPage = (NaviPage)this.Parent;
            //naviPage.PageBack();
        }

        #endregion

        #region Header Events

        private void GirdHeader_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Application.Current.MainWindow.DragMove();
        }

        private void lblLogOff_MouseEnter(object sender, MouseEventArgs e)
        {
            this.lblLogOff.BorderBrush = new SolidColorBrush(Colors.Yellow);
            this.lblLogOff.Foreground = new SolidColorBrush(Colors.Yellow);
        }

        private void lblLogOff_MouseLeave(object sender, MouseEventArgs e)
        {
            this.lblLogOff.BorderBrush = new SolidColorBrush(Colors.White);
            this.lblLogOff.Foreground = new SolidColorBrush(Colors.White);
        }

        private void lblLogOff_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Dlg_Login win = new Dlg_Login();
            win.Closed -= searchDlg_Closed;
            win.Closed += searchDlg_Closed;
            win.Show();
        }

        private void searchDlg_Closed(object sender, EventArgs e)
        {
            if (sender is Dlg_Login)
            {
                Dlg_Login dlg = (Dlg_Login)sender;
                dlg.Closed -= searchDlg_Closed;

                SetHeaderInf();
                Utl_Menu utl = (Utl_Menu)ExVisualTreeHelper.FindUserControl(this, "utlMenu");
                if (utl != null) utl.DisplayChange();
            }
        }

        private void lblEnd_MouseEnter(object sender, MouseEventArgs e)
        {
            this.lblEnd.BorderBrush = new SolidColorBrush(Colors.Yellow);
            this.lblEnd.Foreground = new SolidColorBrush(Colors.Yellow);
        }

        private void lblEnd_MouseLeave(object sender, MouseEventArgs e)
        {
            this.lblEnd.BorderBrush = new SolidColorBrush(Colors.White);
            this.lblEnd.Foreground = new SolidColorBrush(Colors.White);
        }

        private void lblEnd_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ExMessageBox.ResultShow(this, null, "アプリケーションを終了します。" + Environment.NewLine + "よろしいですか？");

            //if (ExMessageBox.ResultShow("アプリケーションを終了します。" + Environment.NewLine + "よろしいですか？") == MessageBoxResult.OK)
            //{
            //    // ログイン済の場合
            //    if (Common.gstrSessionString != "")
            //    {
            //        // System終了時の証跡を保存(System開始時の証跡はログイン時)
            //        DataPgEvidence.SaveLoadOrUnLoadEvidence(DataPgEvidence.PGName.System, DataPgEvidence.geOperationType.End);

            //        // ログオフ
            //        object[] prm = new object[1];
            //        prm[0] = "";
            //        ExWebService webService = new ExWebService();
            //        webService.objMenu = this;
            //        webService.CallWebService(ExWebService.geWebServiceCallKbn.Logoff,
            //                                  ExWebService.geDialogDisplayFlg.No,
            //                                  ExWebService.geDialogCloseFlg.No,
            //                                  prm);
            //    }
            //}
        }

        public override void DataInsert(int intKbn)
        {
            if ((ExWebService.geWebServiceCallKbn)intKbn == ExWebService.geWebServiceCallKbn.Logoff)
            {
                if (Application.Current.IsRunningOutOfBrowser)
                {
                    Application.Current.MainWindow.Close();
                } 
            }
        }

        public override void ResultMessageBox(MessageBoxResult _MessageBoxResult, Control _errCtl)
        {
            if (_MessageBoxResult == MessageBoxResult.OK)
            {
                // ログイン済の場合
                if (Common.gstrSessionString != "")
                {
                    Common.gblnAppEnd = true;

                    // System終了時の証跡を保存(System開始時の証跡はログイン時)
                    DataPgEvidence.SaveLoadOrUnLoadEvidence(DataPgEvidence.PGName.System, DataPgEvidence.geOperationType.End);

                    // ログオフ
                    object[] prm = new object[1];
                    prm[0] = "";
                    ExWebService webService = new ExWebService();
                    webService.objMenu = this;
                    webService.CallWebService(ExWebService.geWebServiceCallKbn.Logoff,
                                              ExWebService.geDialogDisplayFlg.Yes,
                                              ExWebService.geDialogCloseFlg.Yes,
                                              prm);
                }
                else
                {
                    if (Application.Current.IsRunningOutOfBrowser)
                    {
                        Application.Current.MainWindow.Close();
                    } 
                }
            }
        }

        #endregion

    }
}
