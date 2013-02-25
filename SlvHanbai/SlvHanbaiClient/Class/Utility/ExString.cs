using System;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using SlvHanbaiClient.Class.Utility;

namespace SlvHanbaiClient.Class.Utility
{
    public class ExString
    {
        public static int LenB(string stTarget)
        {
            if (string.IsNullOrEmpty(stTarget)) return 0;
            if (stTarget.Length == 0) return 0;


            int sumlength = 0;
            for (int i = 0; i <= stTarget.Length - 1; i++)
            {
                string str = stTarget.Substring(i, 1);
                byte[] bytes = ExSjisEncoding.ucstojms(str);
                sumlength += bytes.Length;
            }

            return sumlength;
        }

        public static bool IsFullString(string stTarget)
        {
            if (stTarget.Length == 0) return false;
            if (stTarget.Length != 1) return false;

            string str = stTarget.Substring(0, 1);
            byte[] bytes = ExSjisEncoding.ucstojms(str);
            if (bytes.Length == 2)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

    }
}
