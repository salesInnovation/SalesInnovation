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
    public class DataInitWhere
    {
        public enum geDateKbn { Today = 0, Month };

        public static void InitDate(DataInitWhere.geDateKbn DateKbn, ref ExDatePicker dpFrom, ref ExDatePicker dpTo)
        {
            DateTime dtNow = DateTime.Now;
            string strNow = DateTime.Now.ToString("yyyy/MM") + "/01";
            DateTime datStart = ExCast.zConvertToDate(strNow);

            dpFrom.SelectedDate = null;
            dpTo.SelectedDate = null;

            switch (DateKbn)
            {
                case DataInitWhere.geDateKbn.Month:
                    dpFrom.SelectedDate = datStart;
                    dpFrom.Text = datStart.ToString("yyyy/MM/dd");
                    dpTo.SelectedDate = datStart.AddMonths(1).AddDays(-1);
                    dpTo.Text = datStart.AddMonths(1).AddDays(-1).ToString("yyyy/MM/dd");
                    break;
                case DataInitWhere.geDateKbn.Today:
                    dpFrom.SelectedDate = dtNow;
                    dpFrom.Text = dtNow.ToString("yyyy/MM/dd");
                    dpTo.SelectedDate = dtNow;
                    dpTo.Text = dtNow.ToString("yyyy/MM/dd");
                    break;
            }
        }

        public static void InitDateYm(ref ExDatePicker dp)
        {
            DateTime dtNow = DateTime.Now;
            DateTime dt = DateTime.Now.AddMonths(-1);
            dp.SelectedDate = null;
            dp.SelectedDate = dt;
            dp.Text = dt.ToString("yyyy/MM");
        }

    }
}
