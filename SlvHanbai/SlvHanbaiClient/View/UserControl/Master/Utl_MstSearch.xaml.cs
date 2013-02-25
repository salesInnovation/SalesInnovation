#region using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using SlvHanbaiClient.Class;
using SlvHanbaiClient.Class.UI;
using SlvHanbaiClient.Class.Data;
using SlvHanbaiClient.Class.WebService;
using SlvHanbaiClient.Class.Utility;
using SlvHanbaiClient.svcMstData;
using SlvHanbaiClient.View.Dlg;
using SlvHanbaiClient.View.UserControl.Custom;

#endregion

namespace SlvHanbaiClient.View.UserControl.Master
{
    public partial class Utl_MstSearch : ExUserControl
    {

        #region Filed Const

        private const String CLASS_NM = "Utl_MstSearch";

        private Dlg_InpMaster inpDlg;
        private static ExWebServiceMst webServiceMst = new ExWebServiceMst();

        private ObservableCollection<EntityMstList> objMstList;

        private MstData.geMDataKbn _MstKbn = MstData.geMDataKbn.Customer;
        public MstData.geMDataKbn MstKbn
        {
            set
            {
                this._MstKbn = value;
            }
            get
            {
                return this._MstKbn;
            }
        }

        private MstData.geMGroupKbn _MstGroupKbn = MstData.geMGroupKbn.None;
        public MstData.geMGroupKbn MstGroupKbn
        {
            set
            {
                this._MstGroupKbn = value;
            }
            get
            {
                return this._MstGroupKbn;
            }
        }

        private bool _DialogResult;
        public bool DialogResult
        {
            set
            {
                this._DialogResult = value;
            }
            get
            {
                return this._DialogResult;
            }
        }

        private string _Id2;
        public string Id2
        {
            set
            {
                this._Id2 = value;
            }
            get
            {
                return this._Id2;
            }
        }

        private string _attribute1;
        public string attribute1
        {
            set
            {
                this._attribute1 = value;
            }
            get
            {
                return this._attribute1;
            }
        }

        private string _attribute2;
        public string attribute2
        {
            set
            {
                this._attribute2 = value;
            }
            get
            {
                return this._attribute2;
            }
        }

        private string _attribute3;
        public string attribute3
        {
            set
            {
                this._attribute3 = value;
            }
            get
            {
                return this._attribute3;
            }
        }

        // リターンCD
        private string _Id;
        public string Id { set { this._Id = value; } get { return this._Id; } }

        // リターン名称
        private string _name;
        public string name { set { this._name = value; } get { return this._name; } }

        #endregion

        #region Constructor

        public Utl_MstSearch()
        {
            InitializeComponent();
            this.Tag = "Main";      // ファンクションキーを受けつけ用

            this.tblbtnF1.Width = 180;
            this.btnF2.IsEnabled = false;
            this.btnF2.IsTabStop = false;
            this.btnF2.Visibility = System.Windows.Visibility.Collapsed;
            this.tblbtnF2.Visibility = System.Windows.Visibility.Collapsed;
            this.btnF3.IsEnabled = false;
            this.btnF3.IsTabStop = false;
            this.btnF3.Visibility = System.Windows.Visibility.Collapsed;
            this.tblbtnF3.Visibility = System.Windows.Visibility.Collapsed;
            this.btnF4.IsEnabled = false;
            this.btnF4.IsTabStop = false;
            this.btnF4.Visibility = System.Windows.Visibility.Collapsed;
            this.tblbtnF4.Visibility = System.Windows.Visibility.Collapsed;
        }

        #endregion

        #region Page Events

        private void ExUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            // 画面初期化
            ExVisualTreeHelper.initDisplay(this.LayoutRoot);

            GetMstList(ExWebService.geDialogDisplayFlg.Yes);
        }

        #endregion

        #region LostFocus Events

        #endregion

        #region Method

        #region Data Select

        #region マスタ一覧データ取得

        private void GetMstList(ExWebService.geDialogDisplayFlg flg)
        {
            object[] prm = new object[3];
            prm[0] = this.txtCode.Text.Trim();
            prm[1] = this.txtName.Text.Trim();

            if (this.MstKbn == MstData.geMDataKbn.Supplier)
            {
                prm[2] = ExCast.zNumZeroNothingFormat(this.Id2);
            }
            webServiceMst.objPerent = this;

            ExWebServiceMst.geWebServiceMstNmCallKbn callKbn = ExWebServiceMst.geWebServiceMstNmCallKbn.GetCustomerList;
            switch (this.MstKbn)
            { 
                case MstData.geMDataKbn.Customer:
                    callKbn = ExWebServiceMst.geWebServiceMstNmCallKbn.GetCustomerList;
                    break;
                case MstData.geMDataKbn.Supplier:
                    callKbn = ExWebServiceMst.geWebServiceMstNmCallKbn.GetSupplierList;
                    break;
                case MstData.geMDataKbn.Person:
                    callKbn = ExWebServiceMst.geWebServiceMstNmCallKbn.GetPersonList;
                    break;
                case MstData.geMDataKbn.Commodity:
                    callKbn = ExWebServiceMst.geWebServiceMstNmCallKbn.GetCommodityList;
                    break;
                case MstData.geMDataKbn.CompanyGroup:
                    callKbn = ExWebServiceMst.geWebServiceMstNmCallKbn.GetCompanyGroupList;
                    break;
                case MstData.geMDataKbn.Zip:
                    callKbn = ExWebServiceMst.geWebServiceMstNmCallKbn.GetZipList;
                    break;
                case MstData.geMDataKbn.Condition:
                    callKbn = ExWebServiceMst.geWebServiceMstNmCallKbn.GetConditionList;
                    break;
                case MstData.geMDataKbn.RecieptDivision:
                    callKbn = ExWebServiceMst.geWebServiceMstNmCallKbn.GetRecieptDivisionList;
                    break;
                case MstData.geMDataKbn.Group:
                    callKbn = ExWebServiceMst.geWebServiceMstNmCallKbn.GetGroupList;
                    webServiceMst.MstGroupKbn = this.MstGroupKbn;
                    break;
                case MstData.geMDataKbn.Purchase:
                    callKbn = ExWebServiceMst.geWebServiceMstNmCallKbn.GetPurchaseList;
                    webServiceMst.MstGroupKbn = this.MstGroupKbn;
                    break;
            }

            if (flg == ExWebService.geDialogDisplayFlg.Yes)
            {
                webServiceMst.CallWebServiceMst(callKbn,
                                                ExWebService.geDialogDisplayFlg.Yes,
                                                ExWebService.geDialogCloseFlg.Yes,
                                                prm);
            }
            else
            {
                webServiceMst.CallWebServiceMst(callKbn,
                                                ExWebService.geDialogDisplayFlg.No,
                                                ExWebService.geDialogCloseFlg.No,
                                                prm);
            }
        }

        #endregion

        #region マスタ一覧データ取得コールバック呼出(ExWebServiceクラスより呼出)

        public override void DataSelect(int intKbn, object objList)
        {
            if (objList != null)
            {
                objMstList = (ObservableCollection<EntityMstList>)objList;
            }
            else
            {
                objMstList = null;
            }
            this.dg.ItemsSource = objMstList;
            this.dg.Focus();
            this.dg.SelectedIndex = 0;

            switch (this.MstKbn)
            {
                case MstData.geMDataKbn.Zip:
                    //DataGridTextColumn col = ExVisualTreeHelper.FindDataGridTextColumn(this, "dgHeadCd");
                    //col.Header = "郵便番号";
                    //this.dgHeadNm.Header = "住所";
                    break;
            }
        }

        #endregion

        #endregion

        #endregion

        #region Text Events

        private void txtCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            //GetMstList(ExWebService.geDialogDisplayFlg.No);
        }

        private void txtName_TextChanged(object sender, TextChangedEventArgs e)
        {
            //GetMstList(ExWebService.geDialogDisplayFlg.No);
        }

        #endregion

        #region Function Key Button Method

        // F1ボタン(OK) クリック
        public override void btnF1_Click(object sender, RoutedEventArgs e)
        {
            if (objMstList == null)
            {
                ExMessageBox.Show("データが登録されていません。");
                return;
            }
            if (objMstList.Count == 0)
            {
                ExMessageBox.Show("データが登録されていません。");
                return;
            }

            int intIndex = this.dg.SelectedIndex;
            if (intIndex < 0)
            {
                ExMessageBox.Show("行が選択されていません。");
                return;
            }

            this.Id = objMstList[intIndex].id;
            this.name = objMstList[intIndex].name;
            this.attribute1 = objMstList[intIndex].attribute1;
            this.attribute2 = objMstList[intIndex].attribute2;
            this.attribute3 = objMstList[intIndex].attribute3;
            this.DialogResult = true;

            Dlg_MstSearch win = (Dlg_MstSearch)ExVisualTreeHelper.FindPerentChildWindow(this);
            win.Close();
        }

        // F2ボタン(追加) クリック
        public override void btnF2_Click(object sender, RoutedEventArgs e)
        {

        }

        // F3ボタン(更新) クリック
        public override void btnF3_Click(object sender, RoutedEventArgs e)
        {
            if (objMstList == null)
            {
                ExMessageBox.Show("データが登録されていません。");
                return;
            }
            if (objMstList.Count == 0)
            {
                ExMessageBox.Show("データが登録されていません。");
                return;
            }

            int intIndex = this.dg.SelectedIndex;
            if (intIndex < 0)
            {
                ExMessageBox.Show("行が選択されていません。");
                return;
            }

            this.Id = objMstList[intIndex].id;
            this.name = objMstList[intIndex].name;
            this.attribute1 = objMstList[intIndex].attribute1;
            this.attribute2 = objMstList[intIndex].attribute2;
            this.attribute3 = objMstList[intIndex].attribute3;
            this.DialogResult = true;

        }

        // F4ボタン(参照) クリック
        public override void btnF4_Click(object sender, RoutedEventArgs e)
        {

        }

        // F5ボタン(検索) クリック
        public override void btnF5_Click(object sender, RoutedEventArgs e)
        {
            GetMstList(ExWebService.geDialogDisplayFlg.Yes);
        }

        // F12ボタン(キャンセル) クリック
        public override void btnF12_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            Dlg_MstSearch win = (Dlg_MstSearch)ExVisualTreeHelper.FindPerentChildWindow(this);
            win.Close();
        }

        #endregion

        private void openMstWindow()
        {
            inpDlg = new Dlg_InpMaster();
            inpDlg.IsSearchFlg = true;

            switch (MstKbn)
            {
                case MstData.geMDataKbn.Person:
                    Common.gWinGroupType = Common.geWinGroupType.InpMaster;
                    Common.gWinMsterType = Common.geWinMsterType.Person;
                    inpDlg.Show();
                    inpDlg.Closed += inpDlg_Closed;
                    break;
                case MstData.geMDataKbn.Customer:
                    Common.gWinGroupType = Common.geWinGroupType.InpMaster;
                    Common.gWinMsterType = Common.geWinMsterType.Comstomer;
                    inpDlg = new Dlg_InpMaster();
                    inpDlg.Show();
                    inpDlg.Closed += inpDlg_Closed;
                    break;
                case MstData.geMDataKbn.Commodity:
                    Common.gWinGroupType = Common.geWinGroupType.InpMaster;
                    Common.gWinMsterType = Common.geWinMsterType.Commodity;
                    inpDlg = new Dlg_InpMaster();
                    inpDlg.Show();
                    inpDlg.Closed += inpDlg_Closed;
                    break;
                case MstData.geMDataKbn.Condition:
                    Common.gWinGroupType = Common.geWinGroupType.InpMasterDetail;
                    Common.gWinMsterType = Common.geWinMsterType.Condition;
                    inpDlg = new Dlg_InpMaster();
                    inpDlg.Show();
                    inpDlg.Closed += inpDlg_Closed;
                    break;
                case MstData.geMDataKbn.Group:
                    Common.gWinGroupType = Common.geWinGroupType.InpMasterDetail;
                    Common.gWinMsterType = Common.geWinMsterType.Class;
                    inpDlg = new Dlg_InpMaster();
                    inpDlg.Show();
                    inpDlg.Closed += inpDlg_Closed;
                    break;
                case MstData.geMDataKbn.Supplier:
                    Common.gWinGroupType = Common.geWinGroupType.InpMaster;
                    Common.gWinMsterType = Common.geWinMsterType.Supplier;
                    inpDlg = new Dlg_InpMaster();
                    inpDlg.Show();
                    inpDlg.Closed += inpDlg_Closed;
                    break;
                default:
                    break;
            }
        }

        private void inpDlg_Closed(object sender, EventArgs e)
        {
            switch (Common.gDataFormType)
            {
                case Common.geDataFormType.Person:
                    break;
                default:
                    break;
            }
        }

        #region DataGrid Events

        private void dg_DoubleClick(object sender, EventArgs e)
        {
            //btnF1_Click(null, null);
        }

        private void dg_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter: this.btnF1_Click(this.btnF1, null); break;
                default: break;
            }
        }

        #endregion

    }

}