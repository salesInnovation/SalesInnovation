using System;
using System.Net;

namespace SlvHanbai.Web.Class.Utility
{
    public class ExEscape
    {
        public static readonly string SQL_YMD = "'%Y/%m/%d'";
        public static readonly string SQL_YM = "'%Y/%m'";
        public static readonly string SQL_Y = "'%Y'";

        public static string zRepStr(object Expression)
        {
            string n;
            try
            {
                if (Expression != null)
                {
                    n = ExCast.zCStr(Expression);
                    n = n.Replace("'", "");
                    n = n.Replace("\"", "");
                    n = n.Replace(@"\", @"\\");
                    n = n.Replace(";", "");
                    n = n.Replace(" * ", "");
                    n = n.Replace(",", "");
                    return "'" + n + "'";
                }
                else
                {
                    return "''";
                }
            }
            catch
            {
                throw;
            }
        }

        public static string zRepStrNoQuota(object Expression)
        {
            string n;
            try
            {
                if (Expression != null)
                {
                    n = ExCast.zCStr(Expression);
                    n = n.Replace("'", "");
                    n = n.Replace("\"", "");
                    n = n.Replace(@"\", @"\\");
                    n = n.Replace(";", "");
                    n = n.Replace(" * ", "");
                    n = n.Replace(",", "");
                    return n;
                }
                else
                {
                    return "";
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
