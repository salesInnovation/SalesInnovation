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
using SlvHanbaiClient.View.Dlg;
using SlvHanbaiClient.View.UserControl.Custom;

#endregion

namespace SlvHanbaiClient.View.Dlg
{
    public partial class Dlg_MstSearchBank : ExChildWindow
    {

        #region Filed Const

        private const String CLASS_NM = "Dlg_MstSearchBank";

        private static ExWebServiceMst webServiceMst = new ExWebServiceMst();

        private ObservableCollection<EntityMstList> objMstList;

        public static string bank_id;
        public static string bank_name;
        public static string bank_branch_id;
        public static string this_id2;
        public static string this_attribute1;
        public static string this_attribute2;
        public static string this_attribute3;
        public static bool this_DialogResult;

        #endregion

        #region Constructor

        public Dlg_MstSearchBank()
        {
            InitializeComponent();
            this.SetWindowsResource();
            this.Tag = "Main";      // ファンクションキーを受けつけ用
        }

        #endregion

        #region Page Events

        private void ExChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.utlDummy.evtDataSelect -= this.evtDataSelect;
            this.utlDummy.evtDataSelect += this.evtDataSelect;

            // 画面初期化
            ExVisualTreeHelper.initDisplay(this.LayoutRoot);

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
            object[] prm = new object[3];
            prm[0] = this.txtCode.Text.Trim();
            prm[1] = this.txtName.Text.Trim();
            prm[2] = ExCast.zNumZeroNothingFormat(Dlg_MstSearch.this_id2);
            webServiceMst.objPerent = this.utlDummy;
            webServiceMst.CallWebServiceMst(ExWebServiceMst.geWebServiceMstNmCallKbn.GetCommodityList,
                                            ExWebService.geDialogDisplayFlg.No,
                                            ExWebService.geDialogCloseFlg.No,
                                            prm);
        }

        #endregion

        #region マスタ一覧データ取得コールバック呼出(ExWebServiceクラスより呼出)

        private void evtDataSelect(int intKbn, object objList)
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
            this.dg.SelectedIndex = 0;
            ExBackgroundWorker.DoWork_DataGridSelectedFirst(dg, 500);
            ExBackgroundWorker.DoWork_DataGridSelectedFirst(dg, 1000);
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

            //Dlg_MstSearch.this_id = objMstList[intIndex].id;
            //Dlg_MstSearch.this_name = objMstList[intIndex].name;
            //Dlg_MstSearch.this_attribute1 = objMstList[intIndex].attribute1;
            //Dlg_MstSearch.this_attribute2 = objMstList[intIndex].attribute2;
            //Dlg_MstSearch.this_attribute3 = objMstList[intIndex].attribute3;
            Dlg_MstSearch.this_DialogResult = true;
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
            this.DialogResult = false;
        }

        #endregion

        #region DataGrid Events

        private void dg_DoubleClick(object sender, EventArgs e)
        {
            btnF1_Click(null, null);
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

