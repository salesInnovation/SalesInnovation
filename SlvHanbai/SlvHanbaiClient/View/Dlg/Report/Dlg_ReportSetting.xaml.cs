using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Printing;
using SlvHanbaiClient.Class.Utility;
using SlvHanbaiClient.Class.Data;
using SlvHanbaiClient.View.Dlg;
using SlvHanbaiClient.View.Dlg.Report;
using SlvHanbaiClient.Class.UI;
using SlvHanbaiClient.Class;
using SlvHanbaiClient.Class.Converter;
using SlvHanbaiClient.svcReport;
using SlvHanbaiClient.Class.WebService;
using SlvHanbaiClient.View.UserControl.Custom;

namespace SlvHanbaiClient.View.Dlg.Report
{
    public partial class Dlg_ReportSetting : ExChildWindow
    {
        #region Filed Const

        private const String CLASS_NM = "Dlg_ReportSetting";
        private const String PG_NM = DataPgEvidence.PGName.Mst.ReportSetting;

        private EntityReportSetting _entity = new EntityReportSetting();

        private Control activeControl;

        private Common.geWinGroupType beforeWinGroupType;

        public string pg_id = "";

        public bool IsReportRange = false;
        public bool IsGroupTotal = false;
        public bool IsTotalKbn = false;
        public bool IsInvoiceKbn = false;
        public bool IsPurchaseKbn = false;

        #endregion

        #region Constructor

        public Dlg_ReportSetting()
        {
            InitializeComponent();

            this.cmbReportSize.Items.Add("デフォルト");
            this.cmbReportSize.Items.Add("A3");
            this.cmbReportSize.Items.Add("A4");
            this.cmbReportSize.Items.Add("A5");

            this.cmbTotalKbn.Items.Add("デフォルト");
            this.cmbTotalKbn.Items.Add("得意先別");
            this.cmbTotalKbn.Items.Add("担当別");
            this.cmbTotalKbn.Items.Add("商品別");
        }

        #endregion

        #region Page Events

        private void ExChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            beforeWinGroupType = Common.gWinGroupType;
            Common.gWinGroupType = Common.geWinGroupType.ReportSetting;

            this.utlDummy.evtDataSelect -= this._evtDataSelect;
            this.utlDummy.evtDataSelect += this._evtDataSelect;
            this.utlDummy.evtDataUpdate -= this._evtDataUpdate;
            this.utlDummy.evtDataUpdate += this._evtDataUpdate;

            if (this.IsPurchaseKbn)
            {
                this.cmbTotalKbn.Items.Clear();
                this.cmbTotalKbn.Items.Add("デフォルト");
                this.cmbTotalKbn.Items.Add("仕入先別");
                this.cmbTotalKbn.Items.Add("担当別");
            }
            else if (this.IsInvoiceKbn)
            {
                this.cmbTotalKbn.Items.Clear();
                this.cmbTotalKbn.Items.Add("デフォルト");
                this.cmbTotalKbn.Items.Add("請求先別");
                this.cmbTotalKbn.Items.Add("担当別");
            }

            // 画面初期化
            ExVisualTreeHelper.initDisplay(this.LayoutRoot);
            this.utlFunctionKey.Init();

            Init();

            //SetBinding();

            GetReportSettingData();
        }

        private void ExChildWindow_Closed(object sender, EventArgs e)
        {
            Common.gWinGroupType = beforeWinGroupType;
        }

        private void Init()
        {
            this.tbkGroup.Text = Common.gstrGroupDisplayNm;
            this.tbkGroupTotal.Text = "＜" + Common.gstrGroupDisplayNm + "別集計＞";

            if (DataAuthority.IsReportTotal())
            {
                this.utlCompanyGroup_F.IsEnabled = true;
                this.utlCompanyGroup_T.IsEnabled = true;
                this.utlCompanyGroup_F.txtID_IsReadOnly = false;
                this.utlCompanyGroup_T.txtID_IsReadOnly = false;
            }
            else
            {
                this.utlCompanyGroup_F.IsEnabled = false;
                this.utlCompanyGroup_T.IsEnabled = false;
                this.utlCompanyGroup_F.txtID_IsReadOnly = true;
                this.utlCompanyGroup_T.txtID_IsReadOnly = true;
                this.utlCompanyGroup_F.txtID.Text = "";
                this.utlCompanyGroup_F.txtNm.Text = "";
                this.utlCompanyGroup_T.txtID.Text = "";
                this.utlCompanyGroup_T.txtNm.Text = "";
            }

            if (this.IsReportRange == false) this.stpReportRange.Visibility = System.Windows.Visibility.Collapsed;
            else this.stpReportRange.Visibility = System.Windows.Visibility.Visible;

            if (this.IsGroupTotal == false) this.stpGroupTotal.Visibility = System.Windows.Visibility.Collapsed;
            else this.stpGroupTotal.Visibility = System.Windows.Visibility.Visible;

            if (this.IsTotalKbn == false) this.stpTotalKbn.Visibility = System.Windows.Visibility.Collapsed;
            else this.stpTotalKbn.Visibility = System.Windows.Visibility.Visible;
        }

        #endregion

        #region Function Key Button Method

        // F1ボタン(OK) クリック
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

        // F2ボタン(クリア) クリック
        public override void btnF2_Click(object sender, RoutedEventArgs e)
        {
            GetReportSettingData();
        }

        // F5ボタン(参照) クリック
        public override void btnF5_Click(object sender, RoutedEventArgs e)
        {
            if (activeControl == null) return;
            switch (activeControl.Name)
            {
                case "utlCompanyGroup_F":
                    this.utlCompanyGroup_F.ShowList();
                    break;
                case "utlCompanyGroup_T":
                    this.utlCompanyGroup_T.ShowList();
                    break;
                default:
                    break;
            }
        }

        // F12ボタン(キャンセル) クリック
        public override void btnF12_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        #endregion

        #region GotFocus Events

        private void txt_GotFocus(object sender, RoutedEventArgs e)
        {
            Control ctl = (Control)sender;
            switch (ctl.Name)
            {
                case "utlCompanyGroup_F":
                case "utlCompanyGroup_T":
                    this.utlFunctionKey.SetFunctionKeyEnable("F5", true);
                    activeControl = ctl;
                    break;
                case "cmbReportSize":
                case "rdoVertical":
                case "rdoHorizontal":
                case "rdoOriDefault":
                case "txtLeftMargin":
                case "txtRightMargin":
                case "txtTopMargin":
                case "txtButtomMargin":
                case "rdoGroupTotalKbnNasi":
                case "rdoGroupTotalKbnAri":
                case "cmbTotalKbn":
                    this.utlFunctionKey.SetFunctionKeyEnable("F5", false);
                    activeControl = null;
                    break;
                default:
                    this.utlFunctionKey.SetFunctionKeyEnable("F5", false);
                    activeControl = null;
                    break;
            }
        }

        #endregion

        #region Binding Setting

        private void SetBinding()
        {
            if (_entity == null)
            {
                _entity = new EntityReportSetting();
            }

            _entity.PropertyChanged += this.utlCompanyGroup_F.MstID_Changed;
            _entity.PropertyChanged += this.utlCompanyGroup_T.MstID_Changed;

            // マスタコントロールPropertyChanged
            //_entity.PropertyChanged += this.utlInvoice.MstID_Changed;

            NumberConverter nmConvDecm0 = new NumberConverter();

            nmConvDecm0.IsMaxMinCheck = true;
            nmConvDecm0.MaxNumber = 350;
            nmConvDecm0.MinNumber = -350;

            NumberConverter nmConvDecm2 = new NumberConverter();
            nmConvDecm2.FormatString = "000";

            #region Bind

            switch (_entity._size)
            {
                case 0:      // デフォルト
                    this.cmbReportSize.SelectedIndex = 0;
                    break;
                case 1:      // A3
                    this.cmbReportSize.SelectedIndex = 1;
                    break;
                case 2:      // A4
                    this.cmbReportSize.SelectedIndex = 2;
                    break;
                case 3:      // A5
                    this.cmbReportSize.SelectedIndex = 3;
                    break;
                default:
                    this.cmbReportSize.SelectedIndex = 0;
                    break;
            }

            switch (_entity._orientation)
            {
                case 0:      // 指定無し
                    this.rdoVertical.IsChecked = false;
                    this.rdoHorizontal.IsChecked = false;
                    this.rdoOriDefault.IsChecked = true;
                    break;
                case 1:      // 縦
                    this.rdoVertical.IsChecked = true;
                    this.rdoHorizontal.IsChecked = false;
                    this.rdoOriDefault.IsChecked = false;
                    break;
                case 2:      // 横
                    this.rdoVertical.IsChecked = false;
                    this.rdoHorizontal.IsChecked = true;
                    this.rdoOriDefault.IsChecked = false;
                    break;
                default:
                    this.rdoVertical.IsChecked = false;
                    this.rdoHorizontal.IsChecked = false;
                    this.rdoOriDefault.IsChecked = true;
                    break;
            }

            switch (_entity._group_total)
            {
                case 0:      // 無し
                    this.rdoGroupTotalKbnNasi.IsChecked = true;
                    this.rdoGroupTotalKbnAri.IsChecked = false;
                    break;
                case 1:      // 有り
                    this.rdoGroupTotalKbnNasi.IsChecked = false;
                    this.rdoGroupTotalKbnAri.IsChecked = true;
                    break;
                default:
                    this.rdoGroupTotalKbnNasi.IsChecked = true;
                    this.rdoGroupTotalKbnAri.IsChecked = false;
                    break;
            }

            switch (_entity._total_kbn)
            {
                case 0:      // 無し
                    this.cmbTotalKbn.SelectedIndex = 0;
                    break;
                case 1:      // 得意先別
                    this.cmbTotalKbn.SelectedIndex = 1;
                    break;
                case 2:      // 商品別
                    this.cmbTotalKbn.SelectedIndex = 2;
                    break;
                case 3:      // 担当別
                    this.cmbTotalKbn.SelectedIndex = 3;
                    break;
                default:
                    this.cmbTotalKbn.SelectedIndex = 0;
                    break;
            }

            // バインド
            Binding BindingLeftMargin = new Binding("_left_margin");
            BindingLeftMargin.Mode = BindingMode.TwoWay;
            BindingLeftMargin.Converter = nmConvDecm0;
            BindingLeftMargin.Source = _entity;
            this.txtLeftMargin.SetBinding(TextBox.TextProperty, BindingLeftMargin);

            Binding BindingRightMargin = new Binding("_right_margin");
            BindingRightMargin.Mode = BindingMode.TwoWay;
            BindingRightMargin.Converter = nmConvDecm0;
            BindingRightMargin.Source = _entity;
            this.txtRightMargin.SetBinding(TextBox.TextProperty, BindingRightMargin);

            Binding BindingTopMargin = new Binding("_top_margin");
            BindingTopMargin.Mode = BindingMode.TwoWay;
            BindingTopMargin.Converter = nmConvDecm0;
            BindingTopMargin.Source = _entity;
            this.txtTopMargin.SetBinding(TextBox.TextProperty, BindingTopMargin);

            Binding BindingBottomMargin = new Binding("_bottom_margin");
            BindingBottomMargin.Mode = BindingMode.TwoWay;
            BindingBottomMargin.Converter = nmConvDecm0;
            BindingBottomMargin.Source = _entity;
            this.txtButtomMargin.SetBinding(TextBox.TextProperty, BindingBottomMargin);

            if (DataAuthority.IsReportTotal())
            {
                Binding BindingGroupIdFrom = new Binding("_group_id_from");
                BindingGroupIdFrom.Mode = BindingMode.TwoWay;
                //BindingGroupIdFrom.Converter = nmConvDecm2;
                BindingGroupIdFrom.Source = _entity;
                this.utlCompanyGroup_F.txtID.SetBinding(TextBox.TextProperty, BindingGroupIdFrom);

                Binding BindingGroupNmFrom = new Binding("_group_nm_from");
                BindingGroupNmFrom.Mode = BindingMode.TwoWay;
                BindingGroupNmFrom.Source = _entity;
                this.utlCompanyGroup_F.txtNm.SetBinding(TextBox.TextProperty, BindingGroupNmFrom);

                Binding BindingGroupIdTo = new Binding("_group_id_to");
                BindingGroupIdTo.Mode = BindingMode.TwoWay;
                //BindingGroupIdTo.Converter = nmConvDecm2;
                BindingGroupIdTo.Source = _entity;
                this.utlCompanyGroup_T.txtID.SetBinding(TextBox.TextProperty, BindingGroupIdTo);

                Binding BindingGroupNmTo = new Binding("_group_nm_to");
                BindingGroupNmTo.Mode = BindingMode.TwoWay;
                BindingGroupNmTo.Source = _entity;
                this.utlCompanyGroup_T.txtNm.SetBinding(TextBox.TextProperty, BindingGroupNmTo);

                if (ExCast.zCInt(_entity._group_id_from) == 0 && ExCast.zCInt(_entity._group_id_to) == 0)
                {
                    this.utlCompanyGroup_F.txtID.Text = string.Format("{0:000}", Common.gintGroupId);
                    //_entity._group_id_from = string.Format("{0:000}", Common.gintGroupId);

                    //_entity._group_nm_from = Common.gstrGroupNm;
                    //this.utlCompanyGroup_F.txtID.Text = Common.gintGroupId.ToString();

                    this.utlCompanyGroup_T.txtID.Text = string.Format("{0:000}", Common.gintGroupId);
                    //_entity._group_id_to = string.Format("{0:000}", Common.gintGroupId);
                    //_entity._group_nm_to = Common.gstrGroupNm;
                    //this.utlCompanyGroup_T.txtID.Text = Common.gintGroupId.ToString();

                    this.utlCompanyGroup_F.MstID_Changed(null, new PropertyChangedEventArgs("_group_id_from"));
                    this.utlCompanyGroup_T.MstID_Changed(null, new PropertyChangedEventArgs("_group_id_to"));
                }
            }
            
            #endregion

            this.txtLeftMargin.OnFormatString();
            this.txtRightMargin.OnFormatString();
            this.txtTopMargin.OnFormatString();
            this.txtButtomMargin.OnFormatString();

            //this.utlCompanyGroup_F.txtID.OnFormatString();
            //this.utlCompanyGroup_T.txtID.OnFormatString();
        }

        #endregion

        #region Data Select

        #region データ取得

        private void GetReportSettingData()
        {
            Common.winParemt = this;
            object[] prm = new object[1];
            prm[0] = this.pg_id;
            webService.objPerent = utlDummy;
            webService.CallWebService(ExWebService.geWebServiceCallKbn.GetReportSetting,
                                      ExWebService.geDialogDisplayFlg.No,
                                      ExWebService.geDialogCloseFlg.No,
                                      prm);
        }

        #endregion

        #region データ取得コールバック呼出(ExWebServiceクラスより呼出)

        private void _evtDataSelect(int intKbn, object objList)
        {
            EntityReportSetting entitySetting;

            if (objList != null)
            {
                entitySetting = (EntityReportSetting)objList;
                if (string.IsNullOrEmpty(entitySetting._message))
                {
                    _entity = entitySetting;
                }
            }
            else
            {
                _entity = null;
            }
            SetBinding();
        }

        #endregion

        #endregion

        #region Data Update

        // データ追加・更新・削除
        private void UpdateData(Common.geUpdateType upd)
        {
            object[] prm = new object[3];

            prm[0] = (int)upd;
            prm[1] = this.pg_id;
            prm[2] = _entity;
            webService.objPerent = this.utlDummy;
            webService.CallWebService(ExWebService.geWebServiceCallKbn.UpdateReportSetting,
                                      ExWebService.geDialogDisplayFlg.Yes,
                                      ExWebService.geDialogCloseFlg.Yes,
                                      prm);
        }

        private void _evtDataUpdate(object errMessage, EventArgs e)
        {
            if (string.IsNullOrEmpty(ExCast.zCStr(errMessage)))
            {
                btnF12_Click(null, null);
            }
        }

        #endregion

        #region Input Check

        // 入力チェック(更新時)
        public override void InputCheckUpdate()
        {
            #region Field

            string errMessage = "";
            string warnMessage = "";
            int _selectIndex = 0;
            int _selectColumn = 0;
            Control errCtl = null;

            #endregion

            try
            {
                #region 必須チェック

                #endregion

                #region 適正値入力チェック

                #endregion

                #region 日付チェック

                //// 納入指定日
                //if (string.IsNullOrEmpty(_entity.supply_ymd) == false)
                //{
                //    if (ExCast.IsDate(_entity.supply_ymd) == false)
                //    {
                //        errMessage += "納入指定日の形式が不正です。(yyyy/mm/dd形式で入力(選択)して下さい)" + Environment.NewLine;
                //        if (errCtl == null) errCtl = this.datNokiYmd;
                //    }
                //}

                #endregion

                #region 日付変換

                // 受注日
                //if (string.IsNullOrEmpty(_entity.order_ymd) == false)
                //{
                //    _entity.order_ymd = ExCast.zConvertToDate(_entity.order_ymd).ToString("yyyy/MM/dd");

                //}

                #endregion

                #region 数値チェック

                #endregion

                #region 正数チェック

                #endregion

                #region 範囲チェック

                #endregion

                #region エラー or 警告時処理

                bool flg = true;

                if (!string.IsNullOrEmpty(errMessage))
                {
                    ExMessageBox.Show(errMessage, Dlg.MessageBox.MessageBoxIcon.Error);
                    flg = false;
                }

                if (!string.IsNullOrEmpty(warnMessage))
                {
                    warnMessage += "このまま登録を続行してもよろしいですか？" + Environment.NewLine;
                    ExMessageBox.ResultShow(this, errCtl, warnMessage);
                    flg = false;
                    //if (ExMessageBox.ResultShow(warnMessage) == MessageBoxResult.No)
                    //{
                    //    flg = false;
                    //}
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

                #region Entity Set 

                if (this.cmbReportSize.SelectedIndex == -1)
                {
                    _entity._size = 0;
                }
                else
                {
                    _entity._size = this.cmbReportSize.SelectedIndex;
                }

                if (this.rdoVertical.IsChecked == true)
                {
                    _entity._orientation = 1;
                }
                else if (this.rdoHorizontal.IsChecked == true)
                {
                    _entity._orientation = 2;
                }
                else
                {
                    _entity._orientation = 0;
                }

                if (this.rdoGroupTotalKbnNasi.IsChecked == true)
                {
                    _entity._group_total = 0;
                }
                else if (this.rdoGroupTotalKbnAri.IsChecked == true)
                {
                    _entity._group_total = 1;
                }
                else
                {
                    _entity._group_total = 0;
                }

                if (this.cmbTotalKbn.SelectedIndex == -1)
                {
                    _entity._total_kbn = 0;
                }
                else
                {
                    _entity._total_kbn = this.cmbTotalKbn.SelectedIndex;
                }

                #endregion

                #region 更新処理

                UpdateData(Common.geUpdateType.Update);

                #endregion
            }
            finally
            {
                Common.gblnBtnProcLock = false;
                Common.gblnBtnDesynchronizeLock = false;
            }
        }

        public override void ResultMessageBox(MessageBoxResult _MessageBoxResult, Control _errCtl)
        {
            if (_MessageBoxResult == MessageBoxResult.OK)
            {
                #region 更新処理

                UpdateData(Common.geUpdateType.Update);

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

    }
}
