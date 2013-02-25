using System;
using System.Collections.Generic;
using System.Linq;
using SlvHanbaiClient.Class.WebService;

namespace SlvHanbaiClient.Class.Data
{
    public class DataPgLock
    {
        public enum geLockType
        {
            UnLock = 0,
            Lock
        }


        // PG証跡追加
        public static void gLockPg(string pgId, string lockId, int type)
        {
            object[] prm = new object[3];
            prm[0] = pgId;
            prm[1] = lockId;
            prm[2] = type.ToString();
            ExWebService webService = new ExWebService();
            webService.CallWebService(ExWebService.geWebServiceCallKbn.LockPg,
                                      ExWebService.geDialogDisplayFlg.No,
                                      ExWebService.geDialogCloseFlg.No,
                                      prm);
        }

    }


}