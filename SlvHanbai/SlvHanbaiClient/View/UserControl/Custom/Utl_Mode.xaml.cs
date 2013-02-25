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
    public partial class Utl_Mode : ExUserControl
    {
        #region Filed Const

        private const String CLASS_NM = "Utl_Mode";

        private Common.gePageType _PageType = Common.gePageType.None;
        public Common.gePageType PageType { get { return this._PageType; } set { this._PageType = value; } }

        private Utl_State _utlStat = null;
        public Utl_State utlStat
        {
            get
            {
                return this._utlStat;
            }
            set
            {
                this._utlStat = value;
            }
        }

        private Utl_FunctionKey.geFunctionKeyEnable _mode = Utl_FunctionKey.geFunctionKeyEnable.Init;
        public Utl_FunctionKey.geFunctionKeyEnable Mode
        {
            get
            {
                return this._mode;
            }
            set
            {
                this._mode = value;
                SetMode();
                if (utlStat != null)
                {
                    utlStat.Mode = this.Mode;
                    utlStat.SetMode();
                } 
            }
        }

        private bool _IsAutoNumMode = true;
        public bool IsAutoNumMode
        {
            get
            {
                return this._IsAutoNumMode;
            }
            set
            {
                this._IsAutoNumMode = value;
            }
        }

        #endregion

        public Utl_Mode()
        {
            InitializeComponent();
            this.Tag = "Mode";
            SetMode();
        }

        private void SetMode()
        {
            switch (this.PageType)
            { 
                case Common.gePageType.InpInvoiceClose:
                    switch (_mode)
                    {
                        case Utl_FunctionKey.geFunctionKeyEnable.Init:
                            this.borMode.Visibility = System.Windows.Visibility.Collapsed;
                            this.txtMode.Visibility = System.Windows.Visibility.Collapsed;
                            this.txtMode.Text = "";
                            break;
                        case Utl_FunctionKey.geFunctionKeyEnable.New:
                            this.borMode.Visibility = System.Windows.Visibility.Visible;
                            this.txtMode.Visibility = System.Windows.Visibility.Visible;
                            this.txtMode.Foreground = new SolidColorBrush(Colors.White);
                            this.borMode.BorderBrush = new SolidColorBrush(Colors.White);
                            //this.txtMode.Text = " 新規モード(締切未) ";
                            this.txtMode.Text = " 更新モード ";
                            break;
                        case Utl_FunctionKey.geFunctionKeyEnable.Upd:
                            this.borMode.Visibility = System.Windows.Visibility.Visible;
                            this.txtMode.Visibility = System.Windows.Visibility.Visible;
                            this.txtMode.Foreground = new SolidColorBrush(Colors.White);
                            this.borMode.BorderBrush = new SolidColorBrush(Colors.White);
                            //this.txtMode.Text = " 更新モード(締切済) ";
                            this.txtMode.Text = " 更新モード ";
                            break;
                        case Utl_FunctionKey.geFunctionKeyEnable.Sel:
                            this.borMode.Visibility = System.Windows.Visibility.Visible;
                            this.txtMode.Visibility = System.Windows.Visibility.Visible;
                            this.txtMode.Foreground = new SolidColorBrush(Colors.Red);
                            this.borMode.BorderBrush = new SolidColorBrush(Colors.Red);
                            this.txtMode.Text = " 参照モード ";
                            break;
                    }
                    break;
                default:
                    switch (_mode)
                    {
                        case Utl_FunctionKey.geFunctionKeyEnable.InitKbn:
                            this.borMode.Visibility = System.Windows.Visibility.Collapsed;
                            this.txtMode.Visibility = System.Windows.Visibility.Collapsed;
                            this.txtMode.Text = "";
                            break;
                        case Utl_FunctionKey.geFunctionKeyEnable.Init:

                            if (IsAutoNumMode == true)
                            {
                                this.borMode.Visibility = System.Windows.Visibility.Visible;
                                this.txtMode.Visibility = System.Windows.Visibility.Visible;
                                this.txtMode.Foreground = new SolidColorBrush(Colors.White);
                                this.borMode.BorderBrush = new SolidColorBrush(Colors.White);
                                this.txtMode.Text = " 新規モード(自動採番) ";
                            }
                            else
                            {
                                this.borMode.Visibility = System.Windows.Visibility.Collapsed;
                                this.txtMode.Visibility = System.Windows.Visibility.Collapsed;
                                this.txtMode.Text = "";
                            }
                            break;
                        case Utl_FunctionKey.geFunctionKeyEnable.New:
                            this.borMode.Visibility = System.Windows.Visibility.Visible;
                            this.txtMode.Visibility = System.Windows.Visibility.Visible;
                            this.txtMode.Foreground = new SolidColorBrush(Colors.White);
                            this.borMode.BorderBrush = new SolidColorBrush(Colors.White);
                            this.txtMode.Text = " 新規モード(ID指定) ";
                            break;
                        case Utl_FunctionKey.geFunctionKeyEnable.Upd:
                            this.borMode.Visibility = System.Windows.Visibility.Visible;
                            this.txtMode.Visibility = System.Windows.Visibility.Visible;
                            this.txtMode.Foreground = new SolidColorBrush(Colors.White);
                            this.borMode.BorderBrush = new SolidColorBrush(Colors.White);
                            this.txtMode.Text = " 更新モード ";
                            break;
                        case Utl_FunctionKey.geFunctionKeyEnable.Sel:
                            this.borMode.Visibility = System.Windows.Visibility.Visible;
                            this.txtMode.Visibility = System.Windows.Visibility.Visible;
                            this.txtMode.Foreground = new SolidColorBrush(Colors.Red);
                            this.borMode.BorderBrush = new SolidColorBrush(Colors.Red);
                            this.txtMode.Text = " 参照モード ";
                            break;
                    }
                    break;
            }
        }


    }
}
