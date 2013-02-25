using System;
using System.Net;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;  // ObservableCollectionを利用するために必要   
using SlvHanbaiClient.Class.Utility;
using SlvHanbaiClient.Class.UI;
using SlvHanbaiClient.svcSales;
using SlvHanbaiClient.svcOrder;
using SlvHanbaiClient.svcEstimate;

namespace SlvHanbaiClient.Class.Data
{
    public class DataAuthority
    {
        public static bool IsReportTotal()
        {
            bool ret = false;
            for (int i = 0; i <= Common.gAuthorityList.Count - 1; i++)
            {
                if (Common.gAuthorityList[i]._pg_id == "ReportTotal" && Common.gAuthorityList[i]._authority_kbn >= 2)
                {
                    ret = true;
                }
            }
            return ret;
        }
    }
}
