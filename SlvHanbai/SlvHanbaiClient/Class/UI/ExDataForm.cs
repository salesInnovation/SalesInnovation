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
    public class ExDataForm : DataForm
    {
        #region Constructor

        public ExDataForm()

            : base()
        {
        }

        #endregion

        public void _OnAddingNewItem()
        {
            DataFormAddingNewItemEventArgs e = new DataFormAddingNewItemEventArgs();
            OnAddingNewItem(e);
        }

        protected override void OnAddingNewItem(DataFormAddingNewItemEventArgs e)
        {
            base.OnAddingNewItem(e);
        }

    }
}
