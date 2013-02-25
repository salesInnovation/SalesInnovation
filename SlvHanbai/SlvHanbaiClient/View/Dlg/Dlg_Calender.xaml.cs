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
using SlvHanbaiClient.Class.UI;
using SlvHanbaiClient.Themes;

#endregion

namespace SlvHanbaiClient.View.Dlg
{
    public partial class Dlg_Calender : ExChildWindow
    {
        public DateTime _selectedDate;

        #region Constructor

        public Dlg_Calender()
        {
            InitializeComponent();
            //this.SetWindowsResource();
        }

        #endregion

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (this._selectedDate != null)
            {
                this.calendar.SelectedDate = (DateTime?)this._selectedDate;
            }
            else
            {
                this.calendar.SelectedDate = (DateTime?)DateTime.Now;
            }
        }

        private void calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            this._selectedDate = (DateTime)this.calendar.SelectedDate;
        }

        #region Function Key Button Method

        // F1ボタン(OK) クリック
        public override void btnF1_Click(object sender, RoutedEventArgs e)
        {
            this._selectedDate = (DateTime)this.calendar.SelectedDate;
            this.DialogResult = true;
            //this.Close();
        }

        // F12ボタン(キャンセル) クリック
        public override void btnF12_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            //this.Close();
        }

        #endregion

    }
}

