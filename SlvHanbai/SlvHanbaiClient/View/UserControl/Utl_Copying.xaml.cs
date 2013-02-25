using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using SlvHanbaiClient.Class.Utility;
using SlvHanbaiClient.Class.Data;
using SlvHanbaiClient.View.Dlg;
using SlvHanbaiClient.Class.UI;
using SlvHanbaiClient.Class;
using SlvHanbaiClient.svcCopying;
using SlvHanbaiClient.Class.WebService;

namespace SlvHanbaiClient.View.UserControl
{
    public partial class Utl_Copying : ExUserControl
    {

        #region Filed Const

        private const String CLASS_NM = "Utl_Copying";

        public Common.gePageType gPageType = Common.gePageType.None;
        public Common.geWinMsterType gWinMsterType = Common.geWinMsterType.None;

        private MstData.geMDataKbn _MstKbn;
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

        private bool _ExistsData;
        public bool ExistsData
        {
            set
            {
                this._ExistsData = value;
            }
            get
            {
                return this._ExistsData;
            }
        }

        private string _copy_id;
        public string copy_id
        {
            set
            {
                this._copy_id = value;
            }
            get
            {
                return this._copy_id;
            }
        }

        private string _before_id;
        public string before_id
        {
            set
            {
                this._before_id = value;
            }
            get
            {
                return this._before_id;
            }
        }

        private ExTextBox _this_txtID;
        private ExTextBox this_txtID
        {
            set
            {
                this._this_txtID = value;
            }
            get
            {
                return this._this_txtID;
            }
        }

        public string tableName = "";

        #endregion

        #region Constructor

        public Utl_Copying()
        {
            InitializeComponent();
        }

        #endregion

        #region Page Events

        private void ExUserControl_GotFocus(object sender, RoutedEventArgs e)
        {
        }

        private void ExUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.gPageType != Common.gePageType.None)
            {
                this.utlID.Visibility = System.Windows.Visibility.Visible;
                this.utlMstID.Visibility = System.Windows.Visibility.Collapsed;
                this.utlMstID.IsTabStop = false;
                this_txtID = this.utlID.txtID;
            }
            else
            {
                this.utlID.Visibility = System.Windows.Visibility.Collapsed;
                this.utlID.IsTabStop = false;
                this.utlMstID.Visibility = System.Windows.Visibility.Visible;
                this_txtID = this.utlMstID.txtID;
            }

            // 画面初期化
            ExVisualTreeHelper.initDisplay(this.LayoutRoot);

            this.rdoAri.IsChecked = true;
            this.rdoAri_Checked(null, null);

            ExBackgroundWorker.DoWork_FocusForLoad(this.this_txtID);
        }

        #endregion

        #region Function Key Button Method

        // F1ボタン(OK) クリック
        public override void btnF1_Click(object sender, RoutedEventArgs e)
        {
            if (this.btnF1.IsEnabled == false) return;

            if (this.rdoAri.IsChecked == true && this_txtID.Text.Trim() == "")
            {
                ExMessageBox.Show("IDが指定されていません。");
                return;
            }

            if (ExCast.IsNumeric(this_txtID.Text.Trim()))
            {
                if (ExCast.zCDbl(this.before_id) == ExCast.zCDbl(this_txtID.Text.Trim()))
                {
                    ExMessageBox.Show("複写元と複写先IDが同じです。");
                    return;
                }
            }
            else
            {
                if (this.before_id == this_txtID.Text.Trim())
                {
                    ExMessageBox.Show("複写元と複写先IDが同じです。");
                    return;
                }
            }

            if (this.rdoAri.IsChecked == true)
            {
                this.copy_id = this_txtID.Text.Trim();
                if (ExCast.IsNumeric(this.copy_id))
                {
                    this.copy_id = ExCast.zCDbl(this.copy_id).ToString();
                }
                OnCopyCheck();
            }
            else
            {
                this.copy_id = "";
                this.DialogResult = true;
                Dlg_Copying win = (Dlg_Copying)ExVisualTreeHelper.FindPerentChildWindow(this);
                win.Close();
            }

        }

        // F5ボタン(検索) クリック
        public override void btnF5_Click(object sender, RoutedEventArgs e)
        {
            if (this.btnF5.IsEnabled == false) return;
            if (this.rdoAri.IsChecked == false) return;

            if (this.gPageType != Common.gePageType.None)
            {
                this.utlID.ShowList();
            }
            else
            {
                this.utlMstID.ShowList();
            }
        }

        // F12ボタン(キャンセル) クリック
        public override void btnF12_Click(object sender, RoutedEventArgs e)
        {
            this.copy_id = "";
            this.DialogResult = false;

            Dlg_Copying win = (Dlg_Copying)ExVisualTreeHelper.FindPerentChildWindow(this);
            win.Close();
        }

        #endregion

        #region RadioButton Checked

        private void rdoAri_Checked(object sender, RoutedEventArgs e)
        {
            this.btnF5.IsEnabled = true;
            this_txtID.IsEnabled = true;
            this_txtID.IsReadOnly = false;
            this_txtID.IsTabStop = true;
        }

        private void rdoAuto_Checked(object sender, RoutedEventArgs e)
        {
            this.btnF5.IsEnabled = false;
            this_txtID.IsEnabled = false;
            this_txtID.IsReadOnly = true;
            this_txtID.IsTabStop = false;
        }

        #endregion

        #region Copy Check

        private void OnCopyCheck()
        {
            object[] prm = new object[2];
            prm[0] = this.tableName;
            prm[1] = this.copy_id;
            webService.objPerent = this;
            webService.CallWebService(ExWebService.geWebServiceCallKbn.CopyCheck,
                                      ExWebService.geDialogDisplayFlg.Yes,
                                      ExWebService.geDialogCloseFlg.Yes,
                                      prm);
        }

        public override void CopyCheck(object entity)
        {
            if (entity != null)
            {
                EntityCopying _entity = (EntityCopying)entity;

                if (_entity._message != "" || _entity._is_lock_success == false)
                {
                    return;
                }
                else
                {
                    this.DialogResult = true;
                    this.ExistsData = _entity._is_exists_data;
                    Dlg_Copying win = (Dlg_Copying)ExVisualTreeHelper.FindPerentChildWindow(this);
                    win.Close();
                    return;
                }
            }
            // 失敗
            else
            {
                ExMessageBox.Show("複写情報の取得で予期せぬエラーが発生しました。");
                return;
            }
        }

        #endregion

        private void ExUserControl_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.F1: this.btnF1_Click(this.btnF1, e); break;
                case Key.F5: this.btnF5_Click(this.btnF5, e); break;
                case Key.F12: this.btnF12_Click(this.btnF12, e); break;
                default: break;
            }

        }
    }
}
