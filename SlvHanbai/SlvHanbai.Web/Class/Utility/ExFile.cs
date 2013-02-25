using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SlvHanbai.Web.Class;

namespace SlvHanbai.Web.Class.Utility
{
    public class ExFile
    {

        public static bool IsFileOpen(string path)
        {
            FileStream fs = null;
            try
            {
                fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
                fs.Close();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                if (fs == null) fs.Dispose(); fs = null;
            }
        }

        public static bool IsFileOpenTime(string path, int retryCnt)
        {
            for (int i = 1; i <= retryCnt; i++)
            {
                bool ret = ExFile.IsFileOpen(path);
                if (ret == true) return true;
                //System.Threading.Thread.Sleep(100);
            }
            return false;
        }

    }
}