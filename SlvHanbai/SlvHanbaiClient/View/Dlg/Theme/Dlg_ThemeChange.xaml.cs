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
    public partial class Dlg_ThemeChange : ExChildWindow
    {

        #region Filed Const

        private const String CLASS_NM = "Dlg_ThemeChange";

        // 名称
        private string _description;
        public string description { set { this._description = value; } get { return this._description; } }

        // 名称リスト
        public static List<ListData> lst;

        #endregion

        #region Constructor

        public Dlg_ThemeChange()
        {
            InitializeComponent();
            this.SetWindowsResource();
        }

        #endregion

        #region Page Events

        private void ExChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.borBureauBlack.BorderThickness = new Thickness(2);
            this.borBureauBlack.BorderBrush = new SolidColorBrush(Colors.Yellow);
            imgBureauBlack.Margin = new Thickness(0);
        }

        #endregion

        #region Method

        private void img_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Image img = (Image)sender;
            switch (img.Name)
            {
                case "imgBureauBlack":
                    MargedResourceDictionary.gThemeType = MargedResourceDictionary.geThemeType.BureauBlack; 
                    break;
                case "imgShinyRed":
                    MargedResourceDictionary.gThemeType = MargedResourceDictionary.geThemeType.ShinyRed; 
                    break;
                case "imgDefault":
                    MargedResourceDictionary.gThemeType = MargedResourceDictionary.geThemeType.None; 
                    break;
                case "imgWhistlerBlue":
                    MargedResourceDictionary.gThemeType = MargedResourceDictionary.geThemeType.WhistlerBlue; 
                    break;
                case "imgTwilightBlue":
                    MargedResourceDictionary.gThemeType = MargedResourceDictionary.geThemeType.TwilightBlue; 
                    break;
                case "imgExpressionDark":
                    MargedResourceDictionary.gThemeType = MargedResourceDictionary.geThemeType.ExpressionDark; 
                    break;
                case "imgBubbleCreme":
                    MargedResourceDictionary.gThemeType = MargedResourceDictionary.geThemeType.BubbleCreme; 
                    break;
                case "imgBureauBlue":
                    MargedResourceDictionary.gThemeType = MargedResourceDictionary.geThemeType.BureauBlue; 
                    break;
                case "imgExpressionLight":
                    MargedResourceDictionary.gThemeType = MargedResourceDictionary.geThemeType.ExpressionLight; 
                    break;
                case "imgRainerOrange":
                    MargedResourceDictionary.gThemeType = MargedResourceDictionary.geThemeType.RainerOrange; 
                    break;
                case "imgRainerPurple":
                    MargedResourceDictionary.gThemeType = MargedResourceDictionary.geThemeType.RainerPurple; 
                    break;
                case "imgShinyBlue":
                    MargedResourceDictionary.gThemeType = MargedResourceDictionary.geThemeType.ShinyBlue; 
                    break;
            }
            this.DialogResult = true;
            //this.Close();
        }

        #endregion

        

    }
}

