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
using System.Windows.Data;
using System.Globalization;
using System.Collections.Generic;
using SlvHanbaiClient.Class.Data;
using SlvHanbaiClient.Class.Utility;


namespace SlvHanbaiClient.Class.Provider
{
    public class taxDivisionProvider
    {
        public List<string> TaxDivisionList
        {
            get
            {
                return MeiNameList.GetListMei(MeiNameList.geNameKbn.TAX_DIVISION_ID);
            }
        }
    }
}
