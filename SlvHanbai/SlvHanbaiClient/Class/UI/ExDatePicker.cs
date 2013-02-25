using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using SlvHanbaiClient.View.Dlg;


namespace SlvHanbaiClient.Class.UI
{
    public class ExDatePicker : DatePicker
    {

        private TextBox txt = new TextBox();
        //public DateTime _selectedDate;

        #region Constructor

        public ExDatePicker()

            : base()
        {
            this.IsTabStop = true;
        }

        #endregion

        public void ShowCalender()
        {
            Dlg_Calender dlg = new Dlg_Calender();
            dlg.SetRecource();

            if (this.SelectedDate != null)
            {
                dlg._selectedDate = (DateTime)this.SelectedDate;
            }
            else
            {
                dlg._selectedDate = DateTime.Now;
            }

            dlg.Show();
            dlg.Closed -= dlg_Closed;
            dlg.Closed += dlg_Closed;
        }

        private void dlg_Closed(object sender, EventArgs e)
        {
            Dlg_Calender dlg = (Dlg_Calender)sender;
            if (dlg.DialogResult == true) this.SelectedDate = dlg._selectedDate;
            this.Focus();
            //ExVisualTreeHelper.FoucsNextControl(this);
            dlg = null;
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            //InputMethod.SetPreferredImeState(txt, InputMethodState.Off);
            //InputMethod.SetIsInputMethodEnabled(txt, false);

            base.OnGotFocus(e);
        }

    }
}
