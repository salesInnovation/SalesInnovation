﻿using System;
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
    public class breakdownProviderNoReturn
    {
        public List<string> BreakDownList
        {
            get
            {
                List<string> lst = MeiNameList.GetListMei(MeiNameList.geNameKbn.BREAKDOWN_ID);
                lst.RemoveAt(lst.Count - 1);
                return lst;
            }
        }
    }
}
