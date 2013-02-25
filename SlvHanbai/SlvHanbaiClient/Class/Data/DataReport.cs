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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SlvHanbaiClient.svcSysName;
using SlvHanbaiClient.Class;
using SlvHanbaiClient.Class.UI;
using SlvHanbaiClient.Class.WebService;

namespace SlvHanbaiClient.Class.Data
{
    public class DataReport
    {
        public enum geReportKbn
        {
            None = 0,
            OutPut,
            Download,
            Csv
        };
    }
}
