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
using SlvHanbaiClient.Class.Data;
using SlvHanbaiClient.Themes;

#endregion

namespace SlvHanbaiClient.View.Dlg.Theme
{
    public partial class Dlg_ThemeShinyBlue : ExChildWindow
    {

        #region Constructor

        public Dlg_ThemeShinyBlue()
        {
            InitializeComponent();
            this.Tag = "dlgThemeShinyBlue";
        }

        #endregion

    }
}

