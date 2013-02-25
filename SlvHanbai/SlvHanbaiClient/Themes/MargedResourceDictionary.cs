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

namespace SlvHanbaiClient.Themes
{
    public class MargedResourceDictionary : ResourceDictionary
    {
        private bool _contentLoaded;
        public enum geThemeType
        { 
            None = 0,
            ShinyBlue,
            ShinyRed,
            TwilightBlue,
            WhistlerBlue,
            BubbleCreme,
            BureauBlack,
            BureauBlue,
            ExpressionDark,
            ExpressionLight,
            RainerOrange,
            RainerPurple
        };
        public static geThemeType gThemeType = geThemeType.ShinyBlue;
        
        public MargedResourceDictionary()
            {
                InitializeComponent();
            }
        public void InitializeComponent()
        {
            if (_contentLoaded)
            {
                return;
            }

            _contentLoaded = true;
            switch (gThemeType)
            { 
                case geThemeType.ShinyBlue:
                    System.Windows.Application.LoadComponent(this, new System.Uri(
                            "/SlvHanbaiClient;component/Themes/ShinyBlue.xaml",
                            System.UriKind.Relative));
                    break;
                case geThemeType.ShinyRed:
                    System.Windows.Application.LoadComponent(this, new System.Uri(
                            "/SlvHanbaiClient;component/Themes/ShinyRed.xaml",
                            System.UriKind.Relative));
                    break;
                case geThemeType.TwilightBlue:
                    System.Windows.Application.LoadComponent(this, new System.Uri(
                            "/SlvHanbaiClient;component/Themes/TwilightBlue.xaml",
                            System.UriKind.Relative));
                    break;
                case geThemeType.BubbleCreme:
                    System.Windows.Application.LoadComponent(this, new System.Uri(
                            "/SlvHanbaiClient;component/Themes/BubbleCreme.xaml",
                            System.UriKind.Relative));
                    break;
                case geThemeType.BureauBlack:
                    System.Windows.Application.LoadComponent(this, new System.Uri(
                            "/SlvHanbaiClient;component/Themes/BureauBlack.xaml",
                            System.UriKind.Relative));
                    break;
                case geThemeType.BureauBlue:
                    System.Windows.Application.LoadComponent(this, new System.Uri(
                            "/SlvHanbaiClient;component/Themes/BureauBlue.xaml",
                            System.UriKind.Relative));
                    break;
                case geThemeType.ExpressionDark:
                    System.Windows.Application.LoadComponent(this, new System.Uri(
                            "/SlvHanbaiClient;component/Themes/ExpressionDark.xaml",
                            System.UriKind.Relative));
                    break;
                case geThemeType.ExpressionLight:
                    System.Windows.Application.LoadComponent(this, new System.Uri(
                            "/SlvHanbaiClient;component/Themes/ExpressionLight.xaml",
                            System.UriKind.Relative));
                    break;
                case geThemeType.RainerOrange:
                    System.Windows.Application.LoadComponent(this, new System.Uri(
                            "/SlvHanbaiClient;component/Themes/RainerOrange.xaml",
                            System.UriKind.Relative));
                    break;
                default:
                    break;
            }
        } 
    }
}
