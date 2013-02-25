using System;
using System.Net;

namespace SlvHanbai.Web.Class.Utility
{
    public class ExCast
    {
        public static long zCLng(object Expression)
        {
            long n;
            try
            {
                if (Expression != null)
                {
                    n = long.Parse(Expression.ToString());
                }
                else
                {
                    n = 0;
                }
            }
            catch
            {
                n = 0;
            }
            return n;
        }

        public static int zCInt(object Expression)
        {
            int n;
            try
            {
                if (Expression != null)
                {
                    n = int.Parse(Expression.ToString());
                }
                else
                {
                    n = 0;
                }
            }
            catch
            {
                n = 0;
            }
            return n;
        }

        public static double zCDbl(object Expression)
        {
            double n;
            try
            {
                if (Expression != null)
                {
                    n = double.Parse(Expression.ToString());
                }
                else
                {
                    n = 0;
                }
                if (Double.IsNaN(n))
                {
                    n = 0;
                }
            }
            catch
            {
                n = 0;
            }
            return n;
        }

        public static string zCStr(object Expression)
        {
            string n;
            try
            {
                if (Expression != null)
                {
                    n = Expression.ToString();
                }
                else
                {
                    n = "";
                }
            }
            catch
            {
                n = "";
            }
            return n;
        }

        public static string zDateNullToDefault(object Expression)
        {
            string n;
            try
            {
                if (Expression != null)
                {
                    n = Expression.ToString();
                }
                else
                {
                    n = "";
                }
            }
            catch
            {
                n = "";
            }

            if (n == "0001/01/01") n = "";
            if (n == "0000/00/00") n = "";            

            return n;
        }

        public static string zTimeNullToDefault(string Expression)
        {
            string s;
            long l;
            DateTime d = new DateTime();
            try
            {
                if (Expression == null)
                {
                    return "00:00:00";
                }

                if (Expression == "")
                {
                    return "00:00:00";
                }
            }
            catch
            {
            }
            return Expression;
        }

        public static DateTime zConvertToDate(object Expression)
        {
            string s;
            long l;
            DateTime d = new DateTime();
            try
            {
                if (Expression != null)
                {
                    s = Expression.ToString();
                    d = DateTime.Parse(s);
                }
            }
            catch
            {
            }
            return d;
        }

        public static string zNumZeroFormat(string formatZero, object Expression)
        {
            string ret = "";

            try
            {
                if (IsNumeric(Expression))
                {
                    ret = string.Format(formatZero, zCDbl(Expression));
                }
                else
                {
                    ret = zCStr(Expression);
                }
            }
            catch
            {
                ret = zCStr(Expression);
            }

            return ret;
        }

        public static string zNumZeroNothingFormat(object Expression)
        {
            string _id = "";

            try
            {
                _id = zCStr(Expression);
                if (ExCast.IsNumeric(_id))
                {
                    _id = ExCast.zCDbl(_id).ToString();
                }
            }
            catch
            {
                _id = zCStr(Expression);
            }

            return _id;
        }

        public static string zIdForNumIndex(object Expression)
        {
            string _id = "";

            try
            {
                _id = zCStr(Expression);
                if (ExCast.IsNumeric(_id))
                {
                    _id = ExCast.zCDbl(_id).ToString();
                }
                else
                {
                    _id = "999999999999999";
                }
            }
            catch
            {
                _id = zCStr(Expression);
            }

            return _id;
        }

        public static string zNullToZero(object Expression)
        {
            string _id = "";

            try
            {
                _id = zCStr(Expression);
                if (string.IsNullOrEmpty(_id))
                {
                    _id = "0";
                }
            }
            catch
            {
                _id = zCStr(Expression);
            }

            return _id;
        }

        public static string zFormatToDate_YYYYMMDD(object Expression)
        {
            string str = "";
            try
            {
                str = ExCast.zCStr(Expression);
                if (string.IsNullOrEmpty(str)) return "";
                if (str.Length != 8) return str;

                str = str.Substring(0, 4) + "/" +
                      str.Substring(4, 2) + "/" +
                      str.Substring(6, 2);
                return str;
            }
            catch
            {
                return str;
            }
        }

        public static string zFormatToDate_YYYYMM(object Expression)
        {
            string str = "";
            try
            {
                str = ExCast.zCStr(Expression);
                if (string.IsNullOrEmpty(str)) return "";
                if (str.Length != 6) return str;

                str = str.Substring(0, 4) + "/" +
                      str.Substring(4, 2);
                return str;
            }
            catch
            {
                return str;
            }
        }

        public static string zFormatForID(object Expression, int maxLengthB)
        {
            string str = "";
            try
            {
                str = ExCast.zCStr(Expression);
                if (string.IsNullOrEmpty(str)) return "";
                if (maxLengthB == 0) return "";
                if (ExCast.IsNumeric(str) == false) return str;

                string str0 = "";
                for (int i = 1; i <= maxLengthB; i++)
                {
                    str0 += "0";
                }
                return ExCast.zCDbl(str).ToString(str0);
            }
            catch
            {
                return str;
            }
        }

        public static bool IsNumeric(object Expression)
        {
            double dNullable;
            bool ret = false;
            try
            {
                ret = double.TryParse(
                    zCStr(Expression),
                    System.Globalization.NumberStyles.Any,
                    null,
                    out dNullable
                );
            }
            catch
            {
            }
            return ret;
        }

        public static bool IsDate(object Expression)
        {
            DateTime dt;
            bool ret = false;
            try
            {
                DateTime.TryParse(zCStr(Expression), out dt);
                ret = true;
            }
            catch
            {
            }
            return ret;
        }

        public static int LenB(string stTarget)
        {
            return System.Text.Encoding.GetEncoding("Shift_JIS").GetByteCount(stTarget);
        }
    }
}
