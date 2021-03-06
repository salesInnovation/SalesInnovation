﻿#region using

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
using SlvHanbaiClient.svcInquiry;
using SlvHanbaiClient.View.Dlg;
using SlvHanbaiClient.View.UserControl.Custom;

#endregion

namespace SlvHanbaiClient.View.Dlg.Support
{
    public partial class Dlg_SupportRegist : ExChildWindow
    {

        #region Filed Const

        private const String CLASS_NM = "Dlg_SupportRegist";

        private ObservableCollection<EntityMstList> _entityPersonList = null;
        private EntityInquiry _entityUpdate = new EntityInquiry();
        private Dlg_Progress dlgProc = new Dlg_Progress();

        ExWebClient wc = null;

        #endregion

        #region Constructor

        public Dlg_SupportRegist()
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

            this.utlDummy.evtDataSelect -= this.evtDataSelect;
            this.utlDummy.evtDataSelect += this.evtDataSelect;

            Init();
        }

        private void Init()
        {
            for (int i = 0; i <= MeiNameList.glstInquiryDivision.Count - 1; i++)
            {
                cmbKbn.Items.Add(MeiNameList.glstInquiryDivision[i].DESCRIPTION);
            }

            for (int i = 0; i <= MeiNameList.glstLevel.Count - 1; i++)
            {
                cmbLevel.Items.Add(MeiNameList.glstLevel[i].DESCRIPTION);
            }

            GetPersonList();
        }

        private void ExChildWindow_Closed(object sender, EventArgs e)
        {
        }

        #endregion

        #region LostFocus Events

        #endregion

        #region Data Select

        #region データ取得

        private void GetPersonList()
        {
            object[] prm = new object[3];
            prm[0] = "";
            prm[1] = "";
            ExWebServiceMst webServiceMst = new ExWebServiceMst();
            webServiceMst.objPerent = this.utlDummy;
            webServiceMst.CallWebServiceMst(ExWebServiceMst.geWebServiceMstNmCallKbn.GetPersonList,
                                            ExWebService.geDialogDisplayFlg.No,
                                            ExWebService.geDialogCloseFlg.No,
                                            prm);
        }

        #endregion

        #region データ取得コールバック呼出(ExWebServiceクラスより呼出)

        private void evtDataSelect(int intKbn, object objList)
        {
            if (objList != null)
            {
                _entityPersonList = (ObservableCollection<EntityMstList>)objList;
                for (int i = 0; i <= _entityPersonList.Count - 1; i++)
                {
                    this.cmbPerson.Items.Add(_entityPersonList[i].name);
                }
            }

            ExBackgroundWorker.DoWork_Focus(this.txtTitle, 100);
            ExBackgroundWorker.DoWork_Focus(this.txtTitle, 200);
            ExBackgroundWorker.DoWork_Focus(this.txtTitle, 300);
        }

        #endregion

        #endregion

        #region Text Events

        #endregion

        #region Function Key Button Method

        // F1ボタン(登録) クリック
        public override void btnF1_Click(object sender, RoutedEventArgs e)
        {
            // 入力確定の為、BackgroundWorker経由で入力チェックを呼出
            ExBackgroundInputCheckWk bk = new ExBackgroundInputCheckWk();
            bk.utl = null;
            bk.win = this;
            this.txtDummy.IsTabStop = true;
            bk.focusCtl = this.txtDummy;
            bk.bw.RunWorkerAsync();
        }

        // F12ボタン(キャンセル) クリック
        public override void btnF12_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        #endregion

        #region Data Update

        // データ追加・更新・削除
        private void UpdateData(Common.geUpdateType upd)
        {
            object[] prm = new object[3];
            prm[0] = (int)upd;
            prm[1] = 0;

            _entityUpdate._title = this.txtTitle.Text;

            for (int i = 0; i <= _entityPersonList.Count - 1; i++)
            {
                if (ExCast.zCStr(this.cmbPerson.SelectedValue) == _entityPersonList[i].name)
                {
                    _entityUpdate._person_id = ExCast.zCInt(_entityPersonList[i].id);
                    _entityUpdate._d_person_id = ExCast.zCInt(_entityPersonList[i].id);
                }
            }

            if (this.cmbKbn.SelectedValue != null)
            {
                _entityUpdate._inquiry_division_id = MeiNameList.GetID(MeiNameList.geNameKbn.INQUIRY_DIVISION_ID, ExCast.zCStr(this.cmbKbn.SelectedValue));
            }

            if (this.cmbLevel.SelectedValue != null)
            {
                _entityUpdate._inquiry_level_id = MeiNameList.GetID(MeiNameList.geNameKbn.LEVEL_ID, ExCast.zCStr(this.cmbLevel.SelectedValue));
            }

            _entityUpdate._inquiry_level_state_id = 1;    // オープン
            _entityUpdate._title = this.txtTitle.Text;
            _entityUpdate._content = this.txtContent.Text;
            _entityUpdate._rec_no = 1;
            _entityUpdate._kbn = 0;       // ユーザー

            prm[2] = _entityUpdate;
            webService.objWindow = this;
            webService.CallWebService(ExWebService.geWebServiceCallKbn.UpdateInquiry,
                                      ExWebService.geDialogDisplayFlg.Yes,
                                      ExWebService.geDialogCloseFlg.Yes,
                                      prm);
        }

        public override void DataUpdate(int intKbn, string errMessage)
        {
            if ((ExWebService.geWebServiceCallKbn)intKbn == ExWebService.geWebServiceCallKbn.UpdateInquiry)
            {
                if (string.IsNullOrEmpty(errMessage))
                {
                    this.DialogResult = true;
                }
            }
        }

        #endregion

        #region Input Check

        // 入力チェック
        public override void InputCheckUpdate()
        {
            #region Field

            string errMessage = "";
            string warnMessage = "";
            int _selectIndex = 0;
            int _selectColumn = 0;
            bool IsDetailExists = false;
            Control errCtl = null;
            List<string> list_warn_commodity = new List<string>();

            #endregion

            #region 必須チェック

            // タイトル
            if (string.IsNullOrEmpty(this.txtTitle.Text.Trim()))
            {
                errMessage += "件名が入力されていません。" + Environment.NewLine;
                if (errCtl == null) errCtl = this.txtTitle;
            }

            // 問い合わせ内容
            if (string.IsNullOrEmpty(this.txtContent.Text.Trim()))
            {
                errMessage += "問い合わせ内容が入力されていません。" + Environment.NewLine;
                if (errCtl == null) errCtl = this.txtTitle;
            }

            // 担当
            if (this.cmbPerson.SelectedIndex == -1)
            {
                errMessage += "担当が選択されていません。" + Environment.NewLine;
                if (errCtl == null) errCtl = this.cmbPerson;
            }

            // 問い合わせ区分
            if (this.cmbKbn.SelectedIndex == -1)
            {
                errMessage += "問い合わせ区分が選択されていません。" + Environment.NewLine;
                if (errCtl == null) errCtl = this.cmbKbn;
            }

            // 緊急度
            if (this.cmbLevel.SelectedIndex == -1)
            {
                errMessage += "緊急度が選択されていません。" + Environment.NewLine;
                if (errCtl == null) errCtl = this.cmbLevel;
            }

            #endregion

            #region 範囲チェック

            //if (ExCast.zCLng(_entityH._no) > 999999999999999)
            //{
            //    errMessage += "受注番号には15桁以内の正の整数を入力して下さい。" + Environment.NewLine;
            //}

            //if (ExCast.zCLng(_entityH._estimateno) > 999999999999999)
            //{
            //    errMessage += "見積番号には15桁以内の正の整数を入力して下さい。" + Environment.NewLine;
            //    if (errCtl == null) errCtl = this.utlEstimateNo.txtID;
            //}

            if (ExString.LenB(this.txtContent.Text) > 1000)
            {
                errMessage += "問い合わせ内容には全角500桁文字以内(半角1000桁文字以内)を入力して下さい。" + Environment.NewLine;
                if (errCtl == null) errCtl = this.txtContent;
            }

            #endregion

            #region アップロードチェック

            if (this.tblUpload.Text == "※ファイルアップロードに失敗しました。")
            {
                warnMessage += "ファイルアップロードに失敗しています。" + Environment.NewLine;
            }

            #endregion

            #region エラー or 警告時処理

            bool flg = true;

            if (!string.IsNullOrEmpty(errMessage))
            {
                ExMessageBox.Show(errMessage, Dlg.MessageBox.MessageBoxIcon.Error);
                flg = false;
            }
            else
            {
                if (!string.IsNullOrEmpty(warnMessage))
                {
                    warnMessage += "このまま登録を続行してもよろしいですか？" + Environment.NewLine;
                    ExMessageBox.ResultShow(this, errCtl, warnMessage);
                    flg = false;    // ResultMessageBoxにてResult処理
                }
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

            UpdateData(Common.geUpdateType.Insert);

            #endregion
        }

        public override void ResultMessageBox(MessageBoxResult _MessageBoxResult, Control _errCtl)
        {
            if (_MessageBoxResult == MessageBoxResult.OK)
            {
                #region 更新処理

                UpdateData(Common.geUpdateType.Insert);

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

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            this.tblUpload.Text = "";
            _entityUpdate._upload_file_path1 = "";

            this.wc = new ExWebClient();
            wc.win = this;
            wc.FileUpLoadFileSet();
            this.txtPath.Text = wc.uploadFileName;

            if (!string.IsNullOrEmpty(this.txtPath.Text))
            {
                Common.gstrProgressDialogTitle = "ファイルアップロード処理中";
                dlgProc.Show();
            }
        }

        #region File Upload

        public override void OnFileUploadCompleted(string dir)
        {
            dlgProc.Dispatcher.BeginInvoke(() =>
            {
                dlgProc.Close();
            });

            // dir未設定はエラーなので無視する
            if (!string.IsNullOrEmpty(dir))
            {
                this.tblUpload.Dispatcher.BeginInvoke(() =>
                {
                    this.tblUpload.Text = "";
                });

                _entityUpdate._upload_file_path1 = dir;
            }
            else
            {
                this.tblUpload.Dispatcher.BeginInvoke(() =>
                {
                    this.tblUpload.Text = "※ファイルアップロードに失敗しました。";
                });
            }
        }

        #endregion

    }
}

