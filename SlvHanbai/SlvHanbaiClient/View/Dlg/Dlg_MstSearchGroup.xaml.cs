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
using System.Collections.ObjectModel;
using SlvHanbaiClient.Class;
using SlvHanbaiClient.Class.UI;
using SlvHanbaiClient.Class.Data;
using SlvHanbaiClient.Class.WebService;
using SlvHanbaiClient.Class.Utility;
using SlvHanbaiClient.svcMstData;
using SlvHanbaiClient.svcClass;
using SlvHanbaiClient.View.Dlg;
using SlvHanbaiClient.View.UserControl.Custom;

#endregion

namespace SlvHanbaiClient.View.Dlg
{
    public partial class Dlg_MstSearchGroup : ExChildWindow
    {

        #region Filed Const

        private const String CLASS_NM = "Dlg_MstSearchGroup";

        private Dlg_InpMaster inpDlg;
        private static ExWebServiceMst webServiceMst = new ExWebServiceMst();

        private ObservableCollection<EntityMstList> objMstList;
        private ObservableCollection<EntityMstList> objClassList = null;

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

        #region 画面フリーズする為、削除

        //public string id;       // リターンID
        //public string name;     // リターン名称
        //public string id2;      // ID2
        //private string _attribute1;
        //public string attribute1
        //{
        //    set
        //    {
        //        this._attribute1 = value;
        //    }
        //    get
        //    {
        //        return this._attribute1;
        //    }
        //}
        //private string _attribute2;
        //public string attribute2
        //{
        //    set
        //    {
        //        this._attribute2 = value;
        //    }
        //    get
        //    {
        //        return this._attribute2;
        //    }
        //}
        //private string _attribute3;
        //public string attribute3
        //{
        //    set
        //    {
        //        this._attribute3 = value;
        //    }
        //    get
        //    {
        //        return this._attribute3;
        //    }
        //}

        #endregion

        public static string this_id;           // リターンID
        public static string this_name;         // リターン名称
        public static string this_id2;
        public static string this_attribute1;
        public static string this_attribute2;
        public static string this_attribute3;
        public static bool this_DialogResult;

        public bool IsKanaUse = false;

        #endregion

        #region Constructor

        public Dlg_MstSearchGroup()
        {
            App.Current.Resources.Remove("ColumnString1");
            App.Current.Resources.Add("ColumnString1", "ID");
            App.Current.Resources.Remove("ColumnString2");
            App.Current.Resources.Add("ColumnString2", "名称");

            InitializeComponent();
            this.SetWindowsResource();
            this.Tag = "Main";      // ファンクションキーを受けつけ用

            this.tblbtnF1.Width = 440;
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

        private void ExChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // 画面初期化
            ExVisualTreeHelper.initDisplay(this.LayoutRoot);

            this.utlDummy.evtDataSelect -= this.evtDataSelect;
            this.utlDummy.evtDataSelect += this.evtDataSelect;

            switch (this.MstKbn)
            {
                case MstData.geMDataKbn.Customer:
                case MstData.geMDataKbn.Customer_F:
                case MstData.geMDataKbn.Customer_T:
                    this.lblCode.Content = "得意先ID";
                    this.lblName.Content = "得意先名";
                    this.lblKana.Content = "得意先カナ";
                    this.lblGroup1.Content = "得意先分類";
                    this.Title = "得意先一覧";
                    this.MstGroupKbn = MstData.geMGroupKbn.CustomerGrouop1;
                    break;
                case MstData.geMDataKbn.Invoice:
                case MstData.geMDataKbn.Invoice_F:
                case MstData.geMDataKbn.Invoice_T:
                    this.lblCode.Content = "請求先ID";
                    this.lblName.Content = "請求先名";
                    this.lblKana.Content = "請求先カナ";
                    this.lblGroup1.Content = "請求先分類";
                    this.Title = "請求先一覧";
                    this.MstGroupKbn = MstData.geMGroupKbn.CustomerGrouop1;
                    break;
                case MstData.geMDataKbn.Commodity:
                case MstData.geMDataKbn.Commodity_F:
                case MstData.geMDataKbn.Commodity_T:
                    this.lblCode.Content = "商品ID";
                    this.lblName.Content = "商品名";
                    this.lblKana.Content = "商品カナ";
                    this.lblGroup1.Content = "商品分類";
                    this.MstGroupKbn = MstData.geMGroupKbn.CommodityGrouop1;
                    this.Title = "商品一覧";
                    break;
                case MstData.geMDataKbn.Purchase:
                case MstData.geMDataKbn.Purchase_F:
                case MstData.geMDataKbn.Purchase_T:
                    this.lblCode.Content = "仕入先ID";
                    this.lblName.Content = "仕入先名";
                    this.lblKana.Content = "仕入先カナ";
                    this.lblGroup1.Content = "仕入先分類";
                    this.Title = "仕入先一覧";
                    this.MstGroupKbn = MstData.geMGroupKbn.PurchaseGrouop1;
                    break;
                default:
                    break;
            }

            GetClassList();
            GetMstList(ExWebService.geDialogDisplayFlg.No);
        }

        private void ExChildWindow_Closed(object sender, EventArgs e)
        {
            this.utlDummy.evtDataSelect -= this.evtDataSelect;
        }

        #endregion

        #region LostFocus Events

        #endregion

        #region Method

        #region Data Select

        #region マスタ一覧データ取得

        private void GetMstList(ExWebService.geDialogDisplayFlg flg)
        {
            object[] prm = new object[4];
            prm[0] = this.txtCode.Text.Trim();
            prm[1] = this.txtName.Text.Trim();

            if (this.stpKana.Visibility == System.Windows.Visibility.Visible)
            {
                prm[2] = this.txtKana.Text.Trim();
            }
            else
            {
                prm[2] = "";
            }

            prm[3] = "";
            if (objClassList != null)
            {
                for (int i = 0; i <= objClassList.Count - 1; i++)
                {
                    if (ExCast.zCStr(this.cmbGroup1.SelectedValue) == objClassList[i].name)
                    {
                        prm[3] = objClassList[i].id;
                    }
                }
            }

            if (this.MstKbn == MstData.geMDataKbn.Supplier)
            {
                prm[4] = ExCast.zNumZeroNothingFormat(Dlg_MstSearchGroup.this_id2);
            }
            webServiceMst.objPerent = this.utlDummy;

            ExWebServiceMst.geWebServiceMstNmCallKbn callKbn = ExWebServiceMst.geWebServiceMstNmCallKbn.GetCustomerList;
            switch (this.MstKbn)
            {
                case MstData.geMDataKbn.Customer:
                case MstData.geMDataKbn.Customer_F:
                case MstData.geMDataKbn.Customer_T:
                    callKbn = ExWebServiceMst.geWebServiceMstNmCallKbn.GetCustomerList;
                    break;
                case MstData.geMDataKbn.Supplier:
                case MstData.geMDataKbn.Supplier_F:
                case MstData.geMDataKbn.Supplier_T:
                    callKbn = ExWebServiceMst.geWebServiceMstNmCallKbn.GetSupplierList;
                    break;
                case MstData.geMDataKbn.Person:
                case MstData.geMDataKbn.Person_F:
                case MstData.geMDataKbn.Person_T:
                    callKbn = ExWebServiceMst.geWebServiceMstNmCallKbn.GetPersonList;
                    break;
                case MstData.geMDataKbn.Commodity:
                case MstData.geMDataKbn.Commodity_F:
                case MstData.geMDataKbn.Commodity_T:
                    callKbn = ExWebServiceMst.geWebServiceMstNmCallKbn.GetCommodityList;
                    break;
                case MstData.geMDataKbn.CompanyGroup:
                case MstData.geMDataKbn.CompanyGroup_F:
                case MstData.geMDataKbn.CompanyGroup_T:
                    callKbn = ExWebServiceMst.geWebServiceMstNmCallKbn.GetCompanyGroupList;
                    break;
                case MstData.geMDataKbn.Zip:
                    callKbn = ExWebServiceMst.geWebServiceMstNmCallKbn.GetZipList;
                    break;
                case MstData.geMDataKbn.Condition:
                case MstData.geMDataKbn.Condition_F:
                case MstData.geMDataKbn.Condition_T:
                    callKbn = ExWebServiceMst.geWebServiceMstNmCallKbn.GetConditionList;
                    break;
                case MstData.geMDataKbn.RecieptDivision:
                    callKbn = ExWebServiceMst.geWebServiceMstNmCallKbn.GetRecieptDivisionList;
                    break;
                case MstData.geMDataKbn.Group:
                case MstData.geMDataKbn.Group_F:
                case MstData.geMDataKbn.Group_T:
                    callKbn = ExWebServiceMst.geWebServiceMstNmCallKbn.GetGroupList;
                    webServiceMst.MstGroupKbn = this.MstGroupKbn;
                    break;
                case MstData.geMDataKbn.Purchase:
                case MstData.geMDataKbn.Purchase_F:
                case MstData.geMDataKbn.Purchase_T:
                    callKbn = ExWebServiceMst.geWebServiceMstNmCallKbn.GetPurchaseList;
                    webServiceMst.MstGroupKbn = this.MstGroupKbn;
                    break;
                case MstData.geMDataKbn.Inventory:
                    callKbn = ExWebServiceMst.geWebServiceMstNmCallKbn.GetInventoryList;
                    webServiceMst.MstGroupKbn = this.MstGroupKbn;
                    break;
                case MstData.geMDataKbn.SalesBalance:
                    callKbn = ExWebServiceMst.geWebServiceMstNmCallKbn.GetSalesBalanceList;
                    webServiceMst.MstGroupKbn = this.MstGroupKbn;
                    break;
                case MstData.geMDataKbn.PaymentBalance:
                    callKbn = ExWebServiceMst.geWebServiceMstNmCallKbn.GetPaymentBalanceList;
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

        private void GetClassList()
        {
            object[] prm = new object[3];
            prm[0] = "";
            prm[1] = "";
            ExWebServiceMst webServiceMst = new ExWebServiceMst();
            webServiceMst.MstGroupKbn = this.MstGroupKbn;
            webServiceMst.objPerent = this.utlDummy;
            webServiceMst.CallWebServiceMst(ExWebServiceMst.geWebServiceMstNmCallKbn.GetGroupList,
                                            ExWebService.geDialogDisplayFlg.No,
                                            ExWebService.geDialogCloseFlg.No,
                                            prm);
        }

        #endregion

        #region マスタ一覧データ取得コールバック呼出(ExWebServiceクラスより呼出)

        public override void DataSelect(int intKbn, object objList)
        {
            if ((ExWebServiceMst.geWebServiceMstNmCallKbn)intKbn == ExWebServiceMst.geWebServiceMstNmCallKbn.GetGroupList)
            {
                this.cmbGroup1.Items.Clear();

                if (objList != null)
                {
                    objClassList = (ObservableCollection<EntityMstList>)objList;
                    for (int i = 0; i <= objClassList.Count - 1; i++)
                    {
                        this.cmbGroup1.Items.Add(objClassList[i].name);
                    }
                }
                else
                {
                    objClassList = null;
                }
                return;
            }

            if (objList != null)
            {
                objMstList = (ObservableCollection<EntityMstList>)objList;
            }
            else
            {
                objMstList = null;
            }
            this.dg.ItemsSource = objMstList;
            this.dg.SelectedIndex = 0;
            ExBackgroundWorker.DoWork_Focus(dg, 10);

            switch (this.MstKbn)
            {
                case MstData.geMDataKbn.Zip:
                    //DataGridTextColumn col = ExVisualTreeHelper.FindDataGridTextColumn(this, "dgHeadCd");
                    //col.Header = "郵便番号";
                    //this.dgHeadNm.Header = "住所";
                    break;
            }
        }

        private void evtDataSelect(int intKbn, object objList)
        {
            if ((ExWebServiceMst.geWebServiceMstNmCallKbn)intKbn == ExWebServiceMst.geWebServiceMstNmCallKbn.GetGroupList)
            {
                this.cmbGroup1.Items.Clear();

                if (objList != null)
                {
                    this.cmbGroup1.Items.Add("選択無し");
                    objClassList = (ObservableCollection<EntityMstList>)objList;
                    for (int i = 0; i <= objClassList.Count - 1; i++)
                    {
                        this.cmbGroup1.Items.Add(objClassList[i].name);
                    }
                }
                else
                {
                    objClassList = null;
                }
                return;
            }

            if (objList != null)
            {
                objMstList = (ObservableCollection<EntityMstList>)objList;
            }
            else
            {
                objMstList = null;
            }
            this.dg.ItemsSource = objMstList;
            this.dg.SelectedIndex = 0;
            ExBackgroundWorker.DoWork_Focus(dg, 10);

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

        private void txtKana_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        #endregion

        #region Function Key Button Method

        // F1ボタン(OK) クリック
        public override void btnF1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.dg.CommitEdit();
            }
            catch
            { 
            }

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

            Dlg_MstSearchGroup.this_id = objMstList[intIndex].id;
            Dlg_MstSearchGroup.this_name = objMstList[intIndex].name;
            Dlg_MstSearchGroup.this_attribute1 = objMstList[intIndex].attribute1;
            Dlg_MstSearchGroup.this_attribute2 = objMstList[intIndex].attribute2;
            Dlg_MstSearchGroup.this_attribute3 = objMstList[intIndex].attribute3;
            Dlg_MstSearchGroup.this_DialogResult = true;
            this.DialogResult = true;
            //this.Close();
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

            Dlg_MstSearchGroup.this_id = objMstList[intIndex].id;
            Dlg_MstSearchGroup.this_name = objMstList[intIndex].name;
            Dlg_MstSearchGroup.this_attribute1 = objMstList[intIndex].attribute1;
            Dlg_MstSearchGroup.this_attribute2 = objMstList[intIndex].attribute2;
            Dlg_MstSearchGroup.this_attribute3 = objMstList[intIndex].attribute3;
            Dlg_MstSearchGroup.this_DialogResult = true;
        }

        // F4ボタン(参照) クリック
        public override void btnF4_Click(object sender, RoutedEventArgs e)
        {

        }

        // F5ボタン(検索) クリック
        public override void btnF5_Click(object sender, RoutedEventArgs e)
        {
            GetMstList(ExWebService.geDialogDisplayFlg.No);
        }

        // F12ボタン(キャンセル) クリック
        public override void btnF12_Click(object sender, RoutedEventArgs e)
        {
            Dlg_MstSearchGroup.this_id = "";
            Dlg_MstSearchGroup.this_name = "";
            Dlg_MstSearchGroup.this_attribute1 = "";
            Dlg_MstSearchGroup.this_attribute2 = "";
            Dlg_MstSearchGroup.this_attribute3 = "";
            Dlg_MstSearchGroup.this_DialogResult = false;
            this.DialogResult = false;
            //this.Close();
        }

        #endregion

        #region DataGrid Events

        private void dg_DoubleClick(object sender, EventArgs e)
        {
            btnF1_Click(null, null);
        }

        private void dg_KeyUp(object sender, KeyEventArgs e)
        {
            //switch (e.Key)
            //{
            //    case Key.Enter:
            //        this.btnF1_Click(this.btnF1, null);
            //        break;
            //    default: break;
            //}
        }

        #endregion

    }
}

