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

namespace SlvHanbaiClient.Class.UI
{
    public class ExCheckBox : CheckBox
    {
        #region Constructor

        public ExCheckBox()

            : base()
        {
        }

        #endregion

        #region Event

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);
            //this.BorderThickness = new Thickness(2);
            //this.BorderBrush = new SolidColorBrush(Colors.Yellow);
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);
            //this.BorderThickness = new Thickness(0);
            //this.BorderBrush = new SolidColorBrush(Colors.Transparent);
        }

        #endregion

    }
}
