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
    public class ExHyperlinkButton : HyperlinkButton
    {
        #region Constructor

        public ExHyperlinkButton(string navigateUri)

            : base()
        {
            base.NavigateUri = new Uri(navigateUri);   
            TargetName = "_blank";
        }

        #endregion

        public void ClickMe()  
        {  
            base.OnClick();  
        }  
    }
}
