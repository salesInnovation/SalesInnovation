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
using System.Windows.Controls.Primitives;
using SlvHanbaiClient.Class.Utility;
using SlvHanbaiClient.Class.Data;
using SlvHanbaiClient.View.Dlg;
using SlvHanbaiClient.Class.UI;
using SlvHanbaiClient.Class;

namespace SlvHanbaiClient.View.UserControl.Custom
{
    public partial class Utl_State : ExUserControl
    {
        #region Filed Const

        private const String CLASS_NM = "Utl_State";

        private Utl_FunctionKey.geFunctionKeyEnable _mode = Utl_FunctionKey.geFunctionKeyEnable.Init;
        public Utl_FunctionKey.geFunctionKeyEnable Mode { get { return this._mode; } set { this._mode = value; SetMode(); } }

        public int StateValue { get { return ExCast.zCInt(GetValue(StateValueProperty)); } set { SetValue(StateValueProperty, value); SetMode(); } }

        public enum geState
        {
            StillApproval = 0,
            Approval,
            Resevation,
            Rejetion
        }

        #endregion

        #region DependencyProperty

        #region DecimalNumProperty

        public static readonly DependencyProperty StateValueProperty =
            DependencyProperty.Register("StateValue",
                                        typeof(int),
                                        typeof(Utl_State),
                                        new PropertyMetadata(new PropertyChangedCallback(Utl_State.OnStateValuePropertyChanged)));

        private static void OnStateValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }

        #endregion

        #endregion
        
        public Utl_State()
        {
            InitializeComponent();
            this.Tag = "State";
            SetMode();
        }

        public void SetMode()
        {
            switch (_mode)
            {
                case Utl_FunctionKey.geFunctionKeyEnable.InitKbn:
                    this.IsEnabled = false;
                    break;
                case Utl_FunctionKey.geFunctionKeyEnable.Init:
                    this.IsEnabled = false;
                    break;
                case Utl_FunctionKey.geFunctionKeyEnable.New:
                    this.IsEnabled = false;
                    break;
                case Utl_FunctionKey.geFunctionKeyEnable.Upd:
                    this.IsEnabled = true;
                    break;
                case Utl_FunctionKey.geFunctionKeyEnable.Sel:
                    this.IsEnabled = false;
                    break;
            }
            SetState();
        }

        private void SetState()
        {
            switch ((geState)this.StateValue)
            {
                case geState.StillApproval:
                    this.tglStillApproval.IsChecked = false;
                    this.tglApproval.IsChecked = true;
                    this.tglResevation.IsChecked = true;
                    this.tglRejetion.IsChecked = true;
                    break;
                case geState.Approval:
                    this.tglStillApproval.IsChecked = true;
                    this.tglApproval.IsChecked = false;
                    this.tglResevation.IsChecked = true;
                    this.tglRejetion.IsChecked = true;
                    break;
                case geState.Resevation:
                    this.tglStillApproval.IsChecked = true;
                    this.tglApproval.IsChecked = true;
                    this.tglResevation.IsChecked = false;
                    this.tglRejetion.IsChecked = true;
                    break;
                case geState.Rejetion:
                    this.tglStillApproval.IsChecked = true;
                    this.tglApproval.IsChecked = true;
                    this.tglResevation.IsChecked = true;
                    this.tglRejetion.IsChecked = false;
                    break;
            }
        }

        private void tblState_Click(object sender, RoutedEventArgs e)
        {
            ToggleButton tgl = (ToggleButton)sender;

            if (this.tglStillApproval.IsChecked == true &&
                this.tglApproval.IsChecked == true &&
                this.tglResevation.IsChecked == true &&
                this.tglRejetion.IsChecked == true)
            {
                this.StateValue = (int)geState.StillApproval;
            }
            else if (this.tglStillApproval.IsChecked == false)
            {
                this.StateValue = (int)geState.StillApproval;
            }
            else if (this.tglApproval.IsChecked == false)
            {
                this.StateValue = (int)geState.Approval;
            }
            else if (this.tglResevation.IsChecked == false)
            {
                this.StateValue = (int)geState.Resevation;
            }
            else if (this.tglRejetion.IsChecked == false)
            {
                this.StateValue = (int)geState.Rejetion;
            }
        }

        private void tblState_Checked(object sender, RoutedEventArgs e)
        {
        }

        private void tblState_UnChecked(object sender, RoutedEventArgs e)
        {
            ToggleButton tgl = (ToggleButton)sender;

            switch (tgl.Name)
            {
                case "tglStillApproval":
                    this.tglApproval.IsChecked = true;
                    this.tglResevation.IsChecked = true;
                    this.tglRejetion.IsChecked = true;
                    break;
                case "tglApproval":
                    this.tglStillApproval.IsChecked = true;
                    this.tglResevation.IsChecked = true;
                    this.tglRejetion.IsChecked = true;
                    break;
                case "tglResevation":
                    this.tglStillApproval.IsChecked = true;
                    this.tglApproval.IsChecked = true;
                    this.tglRejetion.IsChecked = true;
                    break;
                case "tglRejetion":
                    this.tglStillApproval.IsChecked = true;
                    this.tglApproval.IsChecked = true;
                    this.tglResevation.IsChecked = true;
                    break;
            }

        }

    }
}
