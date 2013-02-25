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
using SlvHanbaiClient.svcInquiry;
using SlvHanbaiClient.View.Dlg;
using SlvHanbaiClient.View.UserControl.Custom;

#endregion

namespace SlvHanbaiClient.View.Dlg.Support
{
    public partial class Dlg_Help : ExChildWindow
    {

        #region Filed Const

        private const String CLASS_NM = "Dlg_Help";
        private ObservableCollection<EntityInquiry> _entity = new ObservableCollection<EntityInquiry>();

        private int proc_kbn = 0;

        #endregion

        #region Constructor

        public Dlg_Help()
        {
            InitializeComponent();
            this.SetWindowsResource();
            this.Tag = "Main";      // ファンクションキーを受けつけ用
        }

        #endregion

        #region Page Events

        private void ExChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // 画面初期化
            ExVisualTreeHelper.initDisplay(this.LayoutRoot);

            GetSupportList(0);
        }

        private void ExChildWindow_Closed(object sender, EventArgs e)
        {
        }

        #endregion

        #region LostFocus Events

        #endregion

        #region Method

        #region Data Select

        #region 一覧データ取得

        private void GetSupportList(long no)
        {
            object[] prm = new object[1];
            prm[0] = no;
            webService.objWindow = this;
            webService.CallWebService(ExWebService.geWebServiceCallKbn.GetInquiry,
                                      ExWebService.geDialogDisplayFlg.No,
                                      ExWebService.geDialogCloseFlg.No,
                                      prm);

            if (no == 0)
            {
                this.proc_kbn = 0;
            }
            else
            {
                this.proc_kbn = 1;
            }
        }

        #endregion

        #region 一覧データ取得コールバック呼出(ExWebServiceクラスより呼出)

        public override void DataSelect(int intKbn, object objList)
        {
            if (this.proc_kbn == 1)
            {
                if (objList != null)
                {
                    ObservableCollection<EntityInquiry> _entityList = (ObservableCollection<EntityInquiry>)objList;
                    if (_entityList.Count == 0) return;
                    if (_entityList[0]._lock_flg == 1)
                    {
                        ExMessageBox.Show("他ユーザーにて排他処理中です。");
                        return;
                    }

                    Dlg_SupportReturn returnDlg = new Dlg_SupportReturn();
                    returnDlg._entityList = _entityList;
                    returnDlg.Closed += returnDlg_Closed;
                    returnDlg.Show();
                }
                return;
            }

            if (objList != null)
            {
                _entity = (ObservableCollection<EntityInquiry>)objList;
            }
            else
            {
                _entity = null;
            }
        }

        private void returnDlg_Closed(object sender, EventArgs e)
        {
            Dlg_SupportReturn dlg = (Dlg_SupportReturn)sender;
            if (dlg.DialogResult == true)
            {
                GetSupportList(0);
            }
        }

        #endregion

        private void treeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var item = treeView.ItemContainerGenerator.ContainerFromItem(treeView.SelectedItem) as TreeViewItem;

        }

        #endregion

        #endregion

        #region Text Events

        #endregion

        #region Function Key Button Method

        //// F1ボタン(問い合わせ) クリック
        //public override void btnF1_Click(object sender, RoutedEventArgs e)
        //{
        //    Dlg_SupportRegist registDlg = new Dlg_SupportRegist();
        //    registDlg.Closed += registDlg_Closed;
        //    registDlg.Show();
        //}

        //private void registDlg_Closed(object sender, EventArgs e)
        //{
        //    Dlg_SupportRegist dlg = (Dlg_SupportRegist)sender;
        //    if (dlg.DialogResult == true)
        //    {
        //        GetSupportList(0);
        //    }
        //}

        //// F2ボタン(返答) クリック
        //public override void btnF2_Click(object sender, RoutedEventArgs e)
        //{
        //    if (_entity == null)
        //    {
        //        ExMessageBox.Show("問い合わせ履歴が存在しません。");
        //        return;
        //    }
        //    if (_entity.Count == 0)
        //    {
        //        ExMessageBox.Show("問い合わせ履歴が存在しません。");
        //        return;
        //    }
        //    //if (intIndex < 0)
        //    //{
        //    //    ExMessageBox.Show("行が選択されていません。");
        //    //    return;
        //    //}

        //    //GetSupportList(_entity[intIndex]._no);
        //}

        //// F3ボタン() クリック
        //public override void btnF3_Click(object sender, RoutedEventArgs e)
        //{
        //}

        //// F4ボタン() クリック
        //public override void btnF4_Click(object sender, RoutedEventArgs e)
        //{

        //}

        //// F12ボタン(キャンセル) クリック
        //public override void btnF12_Click(object sender, RoutedEventArgs e)
        //{
        //    this.DialogResult = false;
        //}

        #endregion


    }
}

