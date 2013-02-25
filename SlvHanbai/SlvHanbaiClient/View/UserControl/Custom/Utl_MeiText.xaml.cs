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
using SlvHanbaiClient.Class.Utility;
using SlvHanbaiClient.Class.Data;
using SlvHanbaiClient.View.Dlg;
using SlvHanbaiClient.Class.UI;
using SlvHanbaiClient.Class;

namespace SlvHanbaiClient.View.UserControl.Custom
{
    public partial class Utl_MeiText : ExUserControl
    {

        #region Filed Const

        private const String CLASS_NM = "Utl_MeiText";

        public event RoutedEventHandler txt_LostFocus;

        private string _before = "";

        private MeiNameList.geNameKbn _NameKbn;
        public MeiNameList.geNameKbn NameKbn 
        { 
            set 
            {
                this._NameKbn = value; 
            } 
            get 
            {
                return this._NameKbn; 
            } 
        }

        private Dlg_MeiSearch searchDlg = null;
        private bool IsSearchDlgOpen = false;

        public double id_Width
        {
            set
            {
                this.txtBindID.Width = value;
                this.Width = this.txtBindID.Width + txtNm.Width;
            }
            get { return this.txtBindID.Width; }
        }
        public bool id_IsReadOnly
        {
            set
            {
                this.txtBindID.IsReadOnly = value;
                if (value == true)
                {
                    this.txtBindID.IsTabStop = false;
                    this.Tag = "";
                }
                else
                {
                    this.txtBindID.IsTabStop = true;
                    this.Tag = "TabSearchOn";
                }
            }
            get { return this.txtBindID.IsReadOnly; }
        }
        public double nm_Width
        {
            set
            {
                this.txtNm.Width = value;
                this.Width = this.txtBindID.Width + txtNm.Width;
            }
            get { return this.txtNm.Width; }
        }
        public int nm_MaxLengthB
        {
            set
            {
                this.txtNm.MaxLengthB = value;
            }
            get { return this.txtNm.MaxLengthB; }
        }

        private bool _nm_IsEnabledIdNull = false;
        public bool nm_IsEnabledIdNull
        {
            set
            {
                this._nm_IsEnabledIdNull = value;
            }
            get { return this._nm_IsEnabledIdNull; }
        }

        private bool _IsPaymentCycle = false;
        public bool IsPaymentCycle
        {
            set
            {
                this._IsPaymentCycle = value;
            }
            get { return this._IsPaymentCycle; }
        }

        private Common.geWinGroupType _beforeWinGroupType;

        private bool IsLostFocus = false;
        private bool IstxtNmGotFocus = false;

        public event TextChangedEventHandler ID_TextChanged;

        #endregion

        #region Constructor

        public Utl_MeiText()
        {
            InitializeComponent();
        }

        #endregion

        #region Page Events

        private void ExUserControl_GotFocus(object sender, RoutedEventArgs e)
        {
            if (this.IsLostFocus == true && this.IstxtNmGotFocus == false)
            {
                this.txtBindID.Focus();
            }
            // 現在フォーカスがあるコントロールを取得
            //Control ctl = (Control)FocusManager.GetFocusedElement();
            //if (ctl == null) return;

            //if (ctl.Name.ToString() != this.txtBindID.Name && ctl.Name.ToString() != this.txtNm.Name)
            //{
                
            //}  

            SetTxtNmEnabled();
        }

        private void ExUserControl_LostFocus(object sender, RoutedEventArgs e)
        {
            this.IsLostFocus = true;
        }

        private void ExUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.IsLostFocus = true;

            switch (this.NameKbn)
            {
                // ID:0 データ有りケース
                case MeiNameList.geNameKbn.DISPLAY_DIVISION_ID:
                    this.txtBindID.ZeroToNull = false;
                    break;
                // ID:0 データ無しケース
                default:
                    this.txtBindID.ZeroToNull = true;
                    break;
            }
            SetTxtNmEnabled();
        }

        #endregion

        #region TextBox Events

        private void txtBindID_GotFocus(object sender, RoutedEventArgs e)
        {
            this.IsLostFocus = false;
            MeiNameList.gNameKbn = this.NameKbn;
            MeiNameList.gIsPaymentCycle = this.IsPaymentCycle;
            _before = this.txtBindID.Text.Trim();
        }

        private void txtBindID_LostFocus(object sender, RoutedEventArgs e)
        {
            this.IsLostFocus = true;
            if (_before != this.txtBindID.Text.Trim())
            {
                if (this.txt_LostFocus != null)
                {
                    this.txt_LostFocus(sender, e);
                }
            }
        }

        private void txtBindID_TextChanged(object sender, TextChangedEventArgs e)
        {
            switch (this.NameKbn)
            {
                // ID:0 データ有りケース
                case MeiNameList.geNameKbn.DISPLAY_DIVISION_ID:
                    this.txtID.Text = this.txtBindID.Text;
                    if (this.txtBindID.Text == "")
                    {
                        this.txtNm.Text = "";
                    }
                    else
                    {
                        this.txtNm.Text = MeiNameList.GetName(this.NameKbn, ExCast.zCInt(this.txtBindID.Text));
                    }
                    break;
                case MeiNameList.geNameKbn.TITLE_ID:
                    this.txtID.Text = ExCast.zCInt(this.txtBindID.Text).ToString();
                    if (ExCast.zCInt(this.txtBindID.Text) == 0)
                    {
                        this.txtBindID.Text = "";
                    }
                    else
                    {
                        this.txtNm.Text = MeiNameList.GetName(this.NameKbn, ExCast.zCInt(this.txtBindID.Text));
                    }
                    break;
                // ID:0 データ無しケース
                default:
                    this.txtID.Text = ExCast.zCInt(this.txtBindID.Text).ToString();
                    if (ExCast.zCInt(this.txtBindID.Text) == 0)
                    {
                        this.txtBindID.Text = "";
                        this.txtNm.Text = "";
                    }
                    else
                    {
                        this.txtNm.Text = MeiNameList.GetName(this.NameKbn, ExCast.zCInt(this.txtBindID.Text));
                    }
                    break;
            }

            SetTxtNmEnabled();

            if (this.ID_TextChanged != null)
            {
                this.ID_TextChanged(sender, e);
            }
        }

        private void txtID_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.txtBindID.Text = this.txtID.Text;
            this.txtBindID_TextChanged(sender, e);

            //switch (this.NameKbn)
            //{ 
            //    case MeiNameList.geNameKbn.CLASS:
            //        this.txt_Changed(sender, e);
            //        break;
            //}
        }

        private void txtBindID_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (this.IsSearchDlgOpen == true) return;

            _beforeWinGroupType = Common.gWinGroupType;
            Common.gWinGroupType = Common.geWinGroupType.NameList;
            
            searchDlg = new Dlg_MeiSearch();
            searchDlg.Show();
            this.IsSearchDlgOpen = true;
            searchDlg.Closed += searchDlg_Closed;
        }

        private void searchDlg_Closed(object sender, EventArgs e)
        {
            try
            {
                Common.gWinGroupType = _beforeWinGroupType;

                Dlg_MeiSearch dlg = (Dlg_MeiSearch)sender;
                if (dlg.DialogResult == true)
                {
                    this.txtBindID.Text = dlg.id.ToString();
                    this.txtNm.Text = dlg.description;

                    //if (_before != this.txtBindID.Text.Trim())
                    //{
                    //    if (this.txt_LostFocus != null) this.txt_LostFocus(null, null);
                    //}

                    // 次コントロールフォーカスセット
                    ExVisualTreeHelper.FoucsNextControl(this);
                }
                else
                {
                    this.txtBindID.Focus();
                }
            }
            finally
            {
                this.IsSearchDlgOpen = false;
            }
        }

        private void txtNm_GotFocus(object sender, RoutedEventArgs e)
        {
            if (this.nm_IsEnabledIdNull == true)
            {
                this.IstxtNmGotFocus = true;
            }
        }

        private void txtNm_LostFocus(object sender, RoutedEventArgs e)
        {
            this.IstxtNmGotFocus = false;
        }

        #endregion

        #region Method

        public void ShowList()
        {
            txtBindID_MouseDoubleClick(this.txtBindID, null);
        }

        public void SetTxtNmEnabled()
        {
            if (this.nm_IsEnabledIdNull == true)
            {
                if (this.txtBindID.Text.Trim() == "")
                {
                    this.txtNm.IsEnabled = true;
                    this.txtNm.IsReadOnly = false;
                    this.txtNm.IsTabStop = true;
                }
                else
                {
                    //this.txtNm.IsEnabled = false;
                    this.txtNm.IsReadOnly = true;
                    this.txtNm.IsTabStop = false;
                }
            } 
        }

        #endregion

    }
}
