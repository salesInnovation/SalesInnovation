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
using SlvHanbaiClient.svcInquiry;
using SlvHanbaiClient.svcDuties;
using SlvHanbaiClient.View.Dlg;
using SlvHanbaiClient.View.UserControl.Custom;

#endregion

namespace SlvHanbaiClient.View.Dlg.Duties
{
    public partial class Dlg_DutiesRegist : ExChildWindow
    {

        #region Filed Const

        private const String CLASS_NM = "Dlg_DutiesRegist";

        private ObservableCollection<EntityMstList> _entityPersonList = null;
        private ObservableCollection<EntityMstList> _entityGroupList = null;
        private EntityDuties _entityUpdate = new EntityDuties();
        private Dlg_Progress dlgProc = new Dlg_Progress();
        public enum eProcKbn { New, Update }
        public eProcKbn ProcKbn = eProcKbn.New;

        public EntityDuties _entity = null;

        ExWebClient wc = null;

        private bool IsGetPersonList = false;
        private bool IsGetCompanyGroupList = false;
        private bool IsDataSet = false;

        #endregion

        #region Constructor

        public Dlg_DutiesRegist()
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

            this.tbkGroup.Text = "公開" + Common.gstrGroupDisplayNm;

            this.utlDummy.evtDataSelect -= this.evtDataSelect;
            this.utlDummy.evtDataSelect += this.evtDataSelect;

            Init();
        }

        private void Init()
        {
            for (int i = 0; i <= MeiNameList.glstLevel.Count - 1; i++)
            {
                cmbLevel.Items.Add(MeiNameList.glstLevel[i].DESCRIPTION);
            }

            for (int i = 0; i <= MeiNameList.glstDisplayDivision.Count - 1; i++)
            {
                cmbState.Items.Add(MeiNameList.glstDisplayDivision[i].DESCRIPTION);
            }

            GetPersonList();
            GetCompanyGroupList();
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

        private void GetCompanyGroupList()
        {
            object[] prm = new object[3];
            prm[0] = "";
            prm[1] = "";
            ExWebServiceMst webServiceMst = new ExWebServiceMst();
            webServiceMst.objPerent = this.utlDummy;
            webServiceMst.CallWebServiceMst(ExWebServiceMst.geWebServiceMstNmCallKbn.GetCompanyGroupList,
                                            ExWebService.geDialogDisplayFlg.No,
                                            ExWebService.geDialogCloseFlg.No,
                                            prm);
        }

        #endregion

        #region データ取得コールバック呼出(ExWebServiceクラスより呼出)

        private void evtDataSelect(int intKbn, object objList)
        {
            if (objList == null)
            {
                ExMessageBox.Show("初期データの取得に失敗しました。");
                this.DialogResult = false;
            }

            switch ((ExWebServiceMst.geWebServiceMstNmCallKbn)intKbn)
            { 
                case ExWebServiceMst.geWebServiceMstNmCallKbn.GetPersonList:
                    _entityPersonList = (ObservableCollection<EntityMstList>)objList;
                    for (int i = 0; i <= _entityPersonList.Count - 1; i++)
                    {
                        this.cmbPerson.Items.Add(_entityPersonList[i].name);
                    }
                    this.IsGetPersonList = true;
                    break;
                case ExWebServiceMst.geWebServiceMstNmCallKbn.GetCompanyGroupList:
                    this.cmbGroup.Items.Add("全" + Common.gstrGroupDisplayNm);

                    _entityGroupList = (ObservableCollection<EntityMstList>)objList;
                    for (int i = 0; i <= _entityGroupList.Count - 1; i++)
                    {
                        this.cmbGroup.Items.Add(_entityGroupList[i].name);
                    }
                    this.IsGetCompanyGroupList = true;
                    break;
            }
            InitDataSet();

            //ExBackgroundWorker.DoWork_Focus(this.txtTitle, 100);
            //ExBackgroundWorker.DoWork_Focus(this.txtTitle, 200);
            //ExBackgroundWorker.DoWork_Focus(this.txtTitle, 300);
        }

        #endregion

        #endregion

        #region Init Data Set

        private void InitDataSet()
        {
            try
            {
                if (this.ProcKbn == eProcKbn.Update)
                {
                    if (this.IsGetCompanyGroupList == true && this.IsGetPersonList == true)
                    {
                        this.txtTitle.Text = this._entity._title;

                        if (this._entity._gropu_id == 0)
                        {
                            cmbGroup.SelectedIndex = 0;
                        }
                        else
                        {
                            for (int i = 0; i <= cmbGroup.Items.Count - 1; i++)
                            {
                                if (ExCast.zCStr(this.cmbGroup.Items[i].ToString()) == this._entity._gropu_nm)
                                {
                                    cmbGroup.SelectedIndex = i;
                                }
                            }
                        }
                        for (int i = 0; i <= cmbPerson.Items.Count - 1; i++)
                        {
                            if (ExCast.zCStr(this.cmbPerson.Items[i].ToString()) == this._entity._person_nm)
                            {
                                cmbPerson.SelectedIndex = i;
                            }
                        }
                        for (int i = 0; i <= cmbLevel.Items.Count - 1; i++)
                        {
                            if (ExCast.zCStr(this.cmbLevel.Items[i].ToString()) == this._entity._duties_level_nm)
                            {
                                cmbLevel.SelectedIndex = i;
                            }
                        }
                        for (int i = 0; i <= cmbState.Items.Count - 1; i++)
                        {
                            if (ExCast.zCStr(this.cmbState.Items[i].ToString()) == this._entity._duties_state_nm)
                            {
                                cmbState.SelectedIndex = i;
                            }
                        }
                        this.txtPath.Text = this._entity._upload_file_name1;
                        this.txtContent.Text = this._entity._content;

                        ExBackgroundWorker.DoWork_Focus(this.txtTitle, 100);
                    }
                }
            }
            catch (Exception ex)
            {
                ExMessageBox.Show(CLASS_NM + ".InitDataSet 初期データの設定に失敗しました。" + Environment.NewLine + ex.ToString(), "エラー確認");
                this.DialogResult = false;
            }
        }

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

            string upload_filePath = _entityUpdate._upload_file_path1;
            string upload_fileName = _entityUpdate._upload_file_name1;
            _entityUpdate = new EntityDuties();
            _entityUpdate._upload_file_path1 = upload_filePath;
            _entityUpdate._upload_file_name1 = upload_fileName;

            if (this.ProcKbn == eProcKbn.Update)
            {
                prm[1] = this._entity._no;
                _entityUpdate._no = this._entity._no;
            }
            else
            {
                prm[1] = 0;
                _entityUpdate._no = 0;
            }
            _entityUpdate._title = this.txtTitle.Text;

            if (ExCast.zCStr(this.cmbGroup.SelectedValue) == "全グループ")
            {
                _entityUpdate._gropu_id = 0;
            }
            else
            {
                for (int i = 0; i <= _entityGroupList.Count - 1; i++)
                {
                    if (ExCast.zCStr(this.cmbGroup.SelectedValue) == _entityGroupList[i].name)
                    {
                        _entityUpdate._gropu_id = ExCast.zCInt(_entityGroupList[i].id);
                    }
                }
            }

            for (int i = 0; i <= _entityPersonList.Count - 1; i++)
            {
                if (ExCast.zCStr(this.cmbPerson.SelectedValue) == _entityPersonList[i].name)
                {
                    _entityUpdate._person_id = ExCast.zCInt(_entityPersonList[i].id);
                }
            }

            if (this.cmbLevel.SelectedValue != null)
            {
                _entityUpdate._duties_level_id = MeiNameList.GetID(MeiNameList.geNameKbn.LEVEL_ID, ExCast.zCStr(this.cmbLevel.SelectedValue));
            }

            if (this.cmbState.SelectedValue != null)
            {
                _entityUpdate._duties_state_id = MeiNameList.GetID(MeiNameList.geNameKbn.DISPLAY_DIVISION_ID, ExCast.zCStr(this.cmbState.SelectedValue)) - 1;
            }

            _entityUpdate._content = this.txtContent.Text;

            prm[2] = _entityUpdate;
            webService.objWindow = this;
            webService.CallWebService(ExWebService.geWebServiceCallKbn.UpdateDuties,
                                      ExWebService.geDialogDisplayFlg.Yes,
                                      ExWebService.geDialogCloseFlg.Yes,
                                      prm);
        }

        public override void DataUpdate(int intKbn, string errMessage)
        {
            if ((ExWebService.geWebServiceCallKbn)intKbn == ExWebService.geWebServiceCallKbn.UpdateDuties)
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

            // 内容
            if (string.IsNullOrEmpty(this.txtContent.Text.Trim()))
            {
                errMessage += "業務連絡内容が入力されていません。" + Environment.NewLine;
                if (errCtl == null) errCtl = this.txtTitle;
            }

            // 公開グループ
            if (this.cmbGroup.SelectedIndex == -1)
            {
                errMessage += "公開" + Common.gstrGroupDisplayNm + "が選択されていません。" + Environment.NewLine;
                if (errCtl == null) errCtl = this.cmbGroup;
            }

            // 担当
            if (this.cmbPerson.SelectedIndex == -1)
            {
                errMessage += "担当が選択されていません。" + Environment.NewLine;
                if (errCtl == null) errCtl = this.cmbPerson;
            }

            // 重要度
            if (this.cmbLevel.SelectedIndex == -1)
            {
                errMessage += "重要度が選択されていません。" + Environment.NewLine;
                if (errCtl == null) errCtl = this.cmbLevel;
            }

            // 表示区分
            if (this.cmbState.SelectedIndex == -1)
            {
                errMessage += "表示区分が選択されていません。" + Environment.NewLine;
                if (errCtl == null) errCtl = this.cmbState;
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

            if (ExString.LenB(_entity._content) > 1000)
            {
                errMessage += "業務連絡内容には全角500桁文字以内(半角1000桁文字以内)を入力して下さい。" + Environment.NewLine;
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

            if (this.ProcKbn == eProcKbn.Update)
            {
                UpdateData(Common.geUpdateType.Update);
            }
            else
            {
                UpdateData(Common.geUpdateType.Insert);
            }

            #endregion
        }

        public override void ResultMessageBox(MessageBoxResult _MessageBoxResult, Control _errCtl)
        {
            if (_MessageBoxResult == MessageBoxResult.OK)
            {
                #region 更新処理

                if (this.ProcKbn == eProcKbn.Update)
                {
                    UpdateData(Common.geUpdateType.Update);
                }
                else
                {
                    UpdateData(Common.geUpdateType.Insert);
                }

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

        #region Button Events

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            this.tblUpload.Text = "";
            _entityUpdate._upload_file_path1 = "";

            this.wc = new ExWebClient();
            wc.win = this;
            wc.upLoadFileKbn = 1;
            wc.FileUpLoadFileSet();
            this.txtPath.Text = wc.uploadFileName;

            if (!string.IsNullOrEmpty(this.txtPath.Text))
            {
                Common.gstrProgressDialogTitle = "ファイルアップロード処理中";
                dlgProc.Show();
            }
        }

        #endregion

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
                _entityUpdate._upload_file_name1 = wc.uploadFileName;
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

