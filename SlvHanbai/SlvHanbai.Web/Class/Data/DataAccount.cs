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
    public class DataAccount
    {
        private const string CLASS_NM = "DataAccount";

        public static string GetAccountPeriod(string accountBeginPeriod, string ymd)
        {
            if (accountBeginPeriod.Length != 4 || ymd.Length != 10)
            {
                return "";
            }

            string _ymd = ymd.Replace("/", "");
            string _YYYY = _ymd.Substring(0, 4);
            string _MMDD = _ymd.Substring(4, 4);

            if (ExCast.zCInt(accountBeginPeriod) > ExCast.zCInt(_MMDD))
            {
                return (ExCast.zCInt(_YYYY) - 1).ToString();
            }
            else
            {
                return _YYYY;
            }
        }
    }
}