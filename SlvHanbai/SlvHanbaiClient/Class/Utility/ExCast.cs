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

namespace SlvHanbaiClient.Class.Utility
{
    public class ExCast
    {
        public static long zCLng(object Expression)
        {
            long n = 0;
            try
            {
                if (Expression != null)
                {
                    if (Expression.ToString() != "")
                    {
                        n = long.Parse(Expression.ToString());
                    }
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
            int n = 0;
            try
            {
                if (Expression != null)
                {
                    if (Expression.ToString() != "")
                    {
                        n = int.Parse(Expression.ToString());
                    }
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
            double n = 0;
            try
            {
                if (Expression != null)
                {
                    if (Expression.ToString() != "")
                    {
                        n = double.Parse(Expression.ToString());
                    }
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

        public static DateTime zConvertToDate(object Expression)
        {
            string s;
            long l;
            DateTime dt = new DateTime();
            DateTime d = new DateTime();
            try
            {
                if (Expression != null)
                {
                    s = Expression.ToString();
                    //l = long.Parse(s);
                    DateTime.TryParse(s, out dt);
                }
            }
            catch
            { 
            }
            return dt;
        }

        public static DateTime zConvertToDate(object Expression, string strFormat)
        {
            string s;
            long l;
            DateTime d = new DateTime();
            try
            {
                if (Expression != null)
                {
                    s = Expression.ToString();
                    //l = long.Parse(s);
                    System.Globalization.DateTimeFormatInfo dtfi = new System.Globalization.DateTimeFormatInfo();
                    d = DateTime.ParseExact(s, strFormat, dtfi);
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
                if (zCStr(Expression).Length != 10) ret = false;

                ret = true;
            }
            catch
            {
            }
            return ret;
        }

        public static bool IsDateYm(object Expression)
        {
            DateTime dt;
            bool ret = false;
            try
            {
                DateTime.TryParse(zCStr(Expression), out dt);
                if (zCStr(Expression).Length != 7) ret = false;

                ret = true;
            }
            catch
            {
            }
            return ret;
        }

        public static string zNumNoFormat(object Expression)
        {
            string n;
            try
            {
                n = zCStr(Expression);
                if (IsNumeric(n))
                {
                    n = zCDbl(n).ToString();
                }
            }
            catch
            {
                n = "";
            }
            return n;
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

    }
}
