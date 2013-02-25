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
using SlvHanbaiClient.svcMstData;
using SlvHanbaiClient.Class;
using SlvHanbaiClient.Class.UI;
using SlvHanbaiClient.Class.Data;
using SlvHanbaiClient.Class.WebService;
using SlvHanbaiClient.Class.Utility;
using SlvHanbaiClient.svcDuties;
using SlvHanbaiClient.View.Dlg;
using SlvHanbaiClient.View.UserControl.Custom;

#endregion

namespace SlvHanbaiClient.View.Dlg.Duties
{
    public partial class Dlg_DutiesList : ExChildWindow
    {

        #region Filed Const

        private const String CLASS_NM = "Dlg_SupportReturn";

        public ObservableCollection<EntityDuties> _entityList = new ObservableCollection<EntityDuties>();
        private Dlg_Progress dlgProc = new Dlg_Progress();

        ExWebClient wc = null;

        #endregion

        #region Constructor

        public Dlg_DutiesList()
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

            GetDutiesList();
        }

        private void ExChildWindow_Closed(object sender, EventArgs e)
        {
        }

        #endregion

        #region LostFocus Events

        #endregion

        #region Data Select

        #region 一覧データ取得

        private void GetDutiesList()
        {
            object[] prm = new object[2];
            prm[0] = 0;
            prm[1] = 1;
            webService.objWindow = this;
            webService.CallWebService(ExWebService.geWebServiceCallKbn.GetDuties,
                                      ExWebService.geDialogDisplayFlg.No,
                                      ExWebService.geDialogCloseFlg.No,
                                      prm);

        }

        #endregion

        #region 一覧データ取得コールバック呼出(ExWebServiceクラスより呼出)

        public override void DataSelect(int intKbn, object objList)
        {
            if (objList != null)
            {
                _entityList = (ObservableCollection<EntityDuties>)objList;
            }
            else
            {
                _entityList = null;
            }
            Init();
        }

        private void Init()
        {
            if (_entityList == null)
            {
                TextBlock tbk = new TextBlock();
                tbk.Text = "表示するデータはありません。";
                tbk.Height = 50;
                this.stpHistory.Children.Add(tbk);
                return;
            }

            for (int i = 0; i <= _entityList.Count - 1; i++)
            {
                TextBlock tbk4 = new TextBlock();
                if (_entityList[i]._duties_level_id == 1)
                {
                    tbk4.Foreground = new SolidColorBrush(Colors.Red);
                }
                else
                {
                    tbk4.Foreground = new SolidColorBrush(Colors.Black);
                }
                tbk4.Text = "重要度：" + _entityList[i]._duties_level_nm;
                this.stpHistory.Children.Add(tbk4);

                TextBox txt = new TextBox();
                txt.IsReadOnly = true;
                txt.AcceptsReturn = true;
                txt.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
                txt.Text += "[" + _entityList[i]._duties_date_time + "  連絡者：" + _entityList[i]._person_nm + "  No：" + _entityList[i]._no + "]" + Environment.NewLine;
                txt.Text += "[件名：" + _entityList[i]._title + "]" + Environment.NewLine;
                txt.Background = new SolidColorBrush(Colors.White);
                txt.Text += "---------------------------------------------------------------------------------------------------------------------" + Environment.NewLine;
                txt.Text += _entityList[i]._content + Environment.NewLine;
                this.stpHistory.Children.Add(txt);

                TextBlock tbk = new TextBlock();
                tbk.Height = 5;
                this.stpHistory.Children.Add(tbk);

                if (!string.IsNullOrEmpty(_entityList[i]._upload_file_name1))
                {
                    StackPanel stp = new StackPanel();
                    stp.Orientation = Orientation.Horizontal;
                    TextBox txt2 = new TextBox();
                    txt2.Width = 250;
                    txt2.IsReadOnly = true;
                    txt2.Text = _entityList[i]._upload_file_name1;
                    stp.Children.Add(txt2);
                    TextBlock tbk2 = new TextBlock();
                    tbk2.Width = 10;
                    stp.Children.Add(tbk2);
                    Button btn = new Button();
                    btn.Width = 80;
                    btn.Content = "ﾀﾞｳﾝﾛｰﾄﾞ";
                    btn.Name = "btn" + _entityList[i]._no.ToString();
                    btn.Click += this.btnDownLoad_Click;
                    stp.Children.Add(btn);
                    this.stpHistory.Children.Add(stp);
                }

                TextBlock tbk3 = new TextBlock();
                tbk3.Height = 25;
                this.stpHistory.Children.Add(tbk3);
            }
        }

        #endregion

        #endregion

        #region Text Events

        #endregion

        #region Function Key Button Method

        // F12ボタン(キャンセル) クリック
        public override void btnF12_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        #endregion

        #region 

        private void btnDownLoad_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            string no = btn.Name.Replace("btn", "");
            for (int i = 0; i <= _entityList.Count - 1; i++)
            {
                if (ExCast.zCLng(no) == _entityList[i]._no)
                {
                    string[] prm = new string[4];
                    prm[0] = _entityList[i]._upload_file_name1;
                    prm[1] = _entityList[i]._upload_file_path1;
                    wc = new ExWebClient();
                    wc.utlParentFKey = null;
                    wc.FileDownLoad(DataReport.geReportKbn.None, CLASS_NM, prm);
                }
            }
        }

        #endregion

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            //this.tblUpload.Text = "";
            //_entityUpdate._upload_file_path1 = "";

            //this.wc = new ExWebClient();
            //wc.win = this;
            //wc.FileUpLoadFileSet();
            //this.txtPath.Text = wc.uploadFileName;

            //if (!string.IsNullOrEmpty(this.txtPath.Text))
            //{
            //    Common.gstrProgressDialogTitle = "ファイルアップロード処理中";
            //    dlgProc.Show();
            //}
        }

        #region File Upload

        public override void OnFileUploadCompleted(string dir)
        {
            dlgProc.Dispatcher.BeginInvoke(() =>
            {
                dlgProc.Close();
            });

            //// dir未設定はエラーなので無視する
            //if (!string.IsNullOrEmpty(dir))
            //{
            //    this.tblUpload.Dispatcher.BeginInvoke(() =>
            //    {
            //        this.tblUpload.Text = "";
            //    });

            //    _entityUpdate._upload_file_path1 = dir;
            //}
            //else
            //{
            //    this.tblUpload.Dispatcher.BeginInvoke(() =>
            //    {
            //        this.tblUpload.Text = "※ファイルアップロードに失敗しました。";
            //    });
            //}
        }

        #endregion


    }
}

