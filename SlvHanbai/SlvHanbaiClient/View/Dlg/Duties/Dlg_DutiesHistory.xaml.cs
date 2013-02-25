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
using SlvHanbaiClient.svcDuties;
using SlvHanbaiClient.View.Dlg;
using SlvHanbaiClient.View.UserControl.Custom;

#endregion

namespace SlvHanbaiClient.View.Dlg.Duties
{
    public partial class Dlg_DutiesHistory : ExChildWindow
    {

        #region Filed Const

        private const String CLASS_NM = "Dlg_DutiesHistory";
        private ObservableCollection<EntityDuties> _entity = new ObservableCollection<EntityDuties>();

        private int proc_kbn = 0;

        #endregion

        #region Constructor

        public Dlg_DutiesHistory()
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

            GetDutiesList(0);
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

        private void GetDutiesList(long no)
        {
            object[] prm = new object[1];
            prm[0] = no;
            webService.objWindow = this;
            webService.CallWebService(ExWebService.geWebServiceCallKbn.GetDuties,
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
                    ObservableCollection<EntityDuties> _entityList = (ObservableCollection<EntityDuties>)objList;
                    if (_entityList.Count == 0) return;
                    if (_entityList[0]._lock_flg == 1)
                    {
                        ExMessageBox.Show("他ユーザーにて排他処理中です。");
                        return;
                    }

                    Dlg_DutiesRegist registDlg = new Dlg_DutiesRegist();
                    registDlg._entity = _entityList[0];
                    registDlg.ProcKbn = Dlg_DutiesRegist.eProcKbn.Update;
                    registDlg.Closed += returnDlg_Closed;
                    registDlg.Show();
                }
                return;
            }

            if (objList != null)
            {
                _entity = (ObservableCollection<EntityDuties>)objList;
            }
            else
            {
                _entity = null;
            }
            this.dg.ItemsSource = null;
            this.dg.ItemsSource = _entity;
            this.dg.SelectedIndex = 0;
            ExBackgroundWorker.DoWork_Focus(dg, 10);
        }

        private void returnDlg_Closed(object sender, EventArgs e)
        {
            Dlg_DutiesRegist dlg = (Dlg_DutiesRegist)sender;
            if (dlg.DialogResult == true)
            {
                GetDutiesList(0);
            }
        }

        #endregion

        #endregion

        #endregion

        #region Text Events

        #endregion

        #region Function Key Button Method

        // F1ボタン(新規) クリック
        public override void btnF1_Click(object sender, RoutedEventArgs e)
        {
            Dlg_DutiesRegist registDlg = new Dlg_DutiesRegist();
            registDlg.Closed += registDlg_Closed;
            registDlg.ProcKbn = Dlg_DutiesRegist.eProcKbn.New;
            registDlg.Show();
        }

        private void registDlg_Closed(object sender, EventArgs e)
        {
            Dlg_DutiesRegist dlg = (Dlg_DutiesRegist)sender;
            if (dlg.DialogResult == true)
            {
                GetDutiesList(0);
            }
        }

        // F2ボタン(更新) クリック
        public override void btnF2_Click(object sender, RoutedEventArgs e)
        {
            if (_entity == null)
            {
                ExMessageBox.Show("業務連絡履歴が存在しません。");
                return;
            }
            if (_entity.Count == 0)
            {
                ExMessageBox.Show("業務連絡履歴が存在しません。");
                return;
            }
            int intIndex = this.dg.SelectedIndex;
            if (intIndex < 0)
            {
                ExMessageBox.Show("行が選択されていません。");
                return;
            }

            GetDutiesList(_entity[intIndex]._no);
        }

        // F3ボタン() クリック
        public override void btnF3_Click(object sender, RoutedEventArgs e)
        {
        }

        // F4ボタン() クリック
        public override void btnF4_Click(object sender, RoutedEventArgs e)
        {

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

