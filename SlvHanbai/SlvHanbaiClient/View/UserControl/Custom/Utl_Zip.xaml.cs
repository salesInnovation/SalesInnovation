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
using SlvHanbaiClient.svcMstData;
using SlvHanbaiClient.Class.WebService;

namespace SlvHanbaiClient.View.UserControl.Custom
{
    public partial class Utl_Zip : ExUserControl
    {

        #region Filed Const

        private const String CLASS_NM = "Utl_Zip";
        
        private Dlg_MstSearch searchDlg = null;
        private bool IsSearchDlgOpen = false;

        public double adress_Width
        {
            set
            {
                this.txtAdress1.Width = value;
                this.txtAdress2.Width = value;
                this.Width = value;
            }
            get { return this.Width; }
        }

        public bool is_zip_from_first_flg = false;
        public bool is_zip_to_first_flg = false;
        private bool Is_Zip_Upd = false;

        private string _predecture_Name;
        public string predecture_Name
        {
            set
            {
                this._predecture_Name = value;
            }
            get
            {
                return this._predecture_Name;
            }
        }

        private string _city_Name;
        public string city_Name
        {
            set
            {
                this._city_Name = value;
            }
            get
            {
                return this._city_Name;
            }
        }

        private string _town_Name;
        public string town_Name
        {
            set
            {
                this._town_Name = value;
            }
            get
            {
                return this._town_Name;
            }
        }

        #endregion

        #region DependencyProperty

        #region UserControlAdress1Property

        public static readonly DependencyProperty UserControlAdress1Property =
            DependencyProperty.Register("UserControlAdress1",
                                        typeof(string),
                                        typeof(Utl_Zip),
                                        new PropertyMetadata("UserControlAdress1", new PropertyChangedCallback(Utl_Zip.OnUserControlAdress1PropertyChanged)));

        public string UserControlAdress1
        {
            get
            {
                return (string)GetValue(UserControlAdress1Property);
            }
            set
            {
                SetValue(UserControlAdress1Property, value);
            }
        }

        private static void OnUserControlAdress1PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Utl_Zip)d).OnUserControlAdress1Changed(e);
        }

        private void OnUserControlAdress1Changed(DependencyPropertyChangedEventArgs e)
        {
            if (this.txtAdress1 != null)
            {
                if (e.NewValue != null)
                {
                    this.txtAdress1.Text = e.NewValue.ToString();
                }
                else
                {
                    this.txtAdress1.Text = "";
                }
            }
        }

        #endregion

        #region UserControlAdress2Property

        public static readonly DependencyProperty UserControlAdress2Property =
            DependencyProperty.Register("UserControlAdress2",
                                        typeof(string),
                                        typeof(Utl_Zip),
                                        new PropertyMetadata("UserControlAdress1", new PropertyChangedCallback(Utl_Zip.OnUserControlAdress2PropertyChanged)));

        public string UserControlAdress2
        {
            get
            {
                return (string)GetValue(UserControlAdress2Property);
            }
            set
            {
                SetValue(UserControlAdress2Property, value);
            }
        }

        private static void OnUserControlAdress2PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Utl_Zip)d).OnUserControlAdress2Changed(e);
        }

        private void OnUserControlAdress2Changed(DependencyPropertyChangedEventArgs e)
        {
            if (this.txtAdress2 != null)
            {
                if (e.NewValue != null)
                {
                    this.txtAdress2.Text = e.NewValue.ToString();
                }
                else
                {
                    this.txtAdress2.Text = "";
                }
            }
        }

        #endregion

        #endregion

        #region Constructor

        public Utl_Zip()
        {
            InitializeComponent();
        }

        #endregion

        #region Page Events

        private void ExUserControl_GotFocus(object sender, RoutedEventArgs e)
        {
            //this.txtZipNo1.Focus();
            // 現在フォーカスがあるコントロールを取得
            Control ctl = (Control)FocusManager.GetFocusedElement();
            if (ctl == null) return;

            if (ctl.Name.ToString() != this.btnZip.Name &&
                ctl.Name.ToString() != this.txtZipNo.Name && 
                ctl.Name.ToString() != this.txtZipNo1.Name &&
                ctl.Name.ToString() != this.txtZipNo2.Name &&
                ctl.Name.ToString() != this.txtAdress1.Name &&
                ctl.Name.ToString() != this.txtAdress2.Name)
            {
                this.txtZipNo1.Focus();
            }

        }

        private void ExUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.tbkMessage.Text = "";
        }

        #endregion

        #region TextBox Events

        private void txtZipNo1_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.is_zip_from_first_flg == true)
            {
                this.is_zip_from_first_flg = false;
                return;
            }
            Is_Zip_Upd = true;
        }

        private void txtZipNo2_TextChanged(object sender, TextChangedEventArgs e)
        {
            //this.UserControlZipNoTo = this.txtZipNo2.Text.Trim();
            if (this.is_zip_to_first_flg == true)
            {
                this.is_zip_to_first_flg = false;
                return;
            }
            Is_Zip_Upd = true;
        }

        private void txtZipNo1_LostFocus(object sender, RoutedEventArgs e)
        {
            if (this.txtZipNo1.Text.Trim() == "")
            {
                this.UserControlAdress1 = "";
                this.UserControlAdress2 = "";
                return;
            }

            if (this.txtZipNo2.Text.Trim() == "")
            {
                this.UserControlAdress1 = "";
                this.UserControlAdress2 = "";
                return;
            }

            if (Is_Zip_Upd == false) return;

            string zip1 = this.txtZipNo1.Text.Trim();
            if (zip1 != "" && ExCast.IsNumeric(zip1))
            {
                zip1 = string.Format("{0:000}", ExCast.zCDbl(zip1));
            }

            string zip2 = this.txtZipNo2.Text.Trim();
            if (zip2 != "" && ExCast.IsNumeric(zip2))
            {
                zip2 = string.Format("{0:0000}", ExCast.zCDbl(zip2));
            }

            this.txtZipNo1.Text = zip1;
            this.txtZipNo2.Text = zip2;

            MstData _mstData = new MstData();
            _mstData.GetMData(MstData.geMDataKbn.Zip, new string[] { zip1, zip2 }, this);
        }

        private void txtZipNo2_LostFocus(object sender, RoutedEventArgs e)
        {
            if (this.txtZipNo1.Text.Trim() == "")
            {
                this.UserControlAdress1 = "";
                this.UserControlAdress2 = "";
                return;
            }

            if (this.txtZipNo2.Text.Trim() == "")
            {
                this.UserControlAdress1 = "";
                this.UserControlAdress2 = "";
                return;
            }

            if (Is_Zip_Upd == false) return;

            string zip1 = this.txtZipNo1.Text.Trim();
            if (zip1 != "" && ExCast.IsNumeric(zip1))
            {
                zip1 = string.Format("{0:000}", ExCast.zCDbl(zip1));
            }

            string zip2 = this.txtZipNo2.Text.Trim();
            if (zip2 != "" && ExCast.IsNumeric(zip2))
            {
                zip2 = string.Format("{0:0000}", ExCast.zCDbl(zip2));
            }

            this.txtZipNo1.Text = zip1;
            this.txtZipNo2.Text = zip2;

            MstData _mstData = new MstData();
            _mstData.GetMData(MstData.geMDataKbn.Zip, new string[] { zip1, zip2 }, this);
        }

        private void txtZipNo1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (this.IsSearchDlgOpen == true) return;

            searchDlg = new Dlg_MstSearch(MstData.geMDataKbn.Zip);
            searchDlg.MstKbn = MstData.geMDataKbn.Zip;
            searchDlg.Show();
            this.IsSearchDlgOpen = true;
            searchDlg.Closed += searchDlg_Closed;
        }

        private void txtZipNo2_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (this.IsSearchDlgOpen == true) return;

            searchDlg = new Dlg_MstSearch(MstData.geMDataKbn.Zip);
            searchDlg.MstKbn = MstData.geMDataKbn.Zip;
            searchDlg.Show();
            this.IsSearchDlgOpen = true;
            searchDlg.Closed += searchDlg_Closed;
        }

        private void searchDlg_Closed(object sender, EventArgs e)
        {
            try
            {
                Dlg_MstSearch dlg = (Dlg_MstSearch)sender;
                if (dlg.DialogResult == true)
                {
                    string zip = Dlg_MstSearch.this_id;
                    zip = String.Format("{0:0000000}", ExCast.zCDbl(zip));
                    this.txtZipNo1.Text = zip.Substring(0, 3);
                    this.txtZipNo2.Text = zip.Substring(3, 4);
                    this.txtAdress1.Text = Dlg_MstSearch.this_attribute1 + Dlg_MstSearch.this_attribute2 + Dlg_MstSearch.this_attribute3;
                    this.txtAdress2.Text = "";
                    this.predecture_Name = Dlg_MstSearch.this_attribute1;
                    this.city_Name = Dlg_MstSearch.this_attribute2;
                    this.town_Name = Dlg_MstSearch.this_attribute3;

                    this.UserControlAdress1 = this.txtAdress1.Text;
                    this.UserControlAdress2 = this.txtAdress2.Text;

                    // 次コントロールフォーカスセット
                    ExVisualTreeHelper.FoucsNextControl(this);
                }
                else
                {
                    this.txtZipNo1.Focus();
                }
            }
            finally
            {
                this.IsSearchDlgOpen = false;
            }
        }

        private void txtAdress1_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.UserControlAdress1 = this.txtAdress1.Text;
        }

        private void txtAdress2_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.UserControlAdress2 = this.txtAdress2.Text;
        }

        #endregion

        #region Method

        #region Parent DataPropety Change Events

        public void MstID_Changed(object sender, PropertyChangedEventArgs e)
        {
            string _propertyName = e.PropertyName;
            if (_propertyName.Length == 0) return;
            if (_propertyName.Substring(0, 1) == "_")
            {
                _propertyName = _propertyName.Substring(1, _propertyName.Length - 1);
            }

            switch (_propertyName)
            {
                case "zip_code":
                    txtZipNo1_LostFocus(null, null);
                    break;
                default:
                    break;
            }
        }

        #endregion

        public void ShowList()
        {
            txtZipNo1_MouseDoubleClick(this.txtZipNo1, null);
        }

        #endregion

        #region Master Name Set

        /// <summary>
        /// マスタ名称設定(ExWebServiceMstNameから非同期呼出)
        /// </summary>
        /// <param name="intKbn"></param>
        /// <param name="name"></param>
        public override void MstDataSelect(ExWebServiceMst.geWebServiceMstNmCallKbn intKbn, EntityMstData mst)
        {
            Is_Zip_Upd = false;

            if (mst == null)
            {
                this.txtAdress1.Text = "";
                this.txtAdress2.Text = "";
                this.predecture_Name = "";
                this.city_Name = "";
                this.town_Name = "";
                this.tbkMessage.Text = "  ※入力郵便番号は存在しません";

                this.UserControlAdress1 = this.txtAdress1.Text;
                this.UserControlAdress2 = this.txtAdress2.Text;
            }
            else
            {
                if (!string.IsNullOrEmpty(mst.attribute1)) this.txtZipNo1.Text = String.Format("{0:000}", ExCast.zCDbl(mst.attribute1));
                if (!string.IsNullOrEmpty(mst.attribute2)) this.txtZipNo2.Text = String.Format("{0:0000}", ExCast.zCDbl(mst.attribute2)); 

                if (this.txtAdress1.Text.Trim() == "" || this.txtAdress2.Text.Trim() == "")
                {
                    string adress = mst.attribute3 + mst.attribute4 + mst.attribute5;
                    if (adress.Length > 20)
                    {
                        this.txtAdress1.Text = adress.Substring(0, 20);
                        this.txtAdress2.Text = adress.Substring(20, adress.Length - 20);
                    }
                    else
                    {
                        this.txtAdress1.Text = adress;
                        this.txtAdress2.Text = "";
                    }
                }

                this.predecture_Name = mst.attribute3;
                this.city_Name = mst.attribute4;
                this.town_Name = mst.attribute5;
                this.tbkMessage.Text = "";

                this.UserControlAdress1 = this.txtAdress1.Text;
                this.UserControlAdress2 = this.txtAdress2.Text;
            }
        }

        #endregion

        private void btnZip_Click(object sender, RoutedEventArgs e)
        {
            ShowList();
        }

    }
}
