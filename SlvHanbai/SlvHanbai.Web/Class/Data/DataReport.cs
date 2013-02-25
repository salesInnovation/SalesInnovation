using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Data;
using SlvHanbai.Web.Class.DB;
using SlvHanbai.Web.Class.Utility;

namespace SlvHanbai.Web.Class.Data
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
