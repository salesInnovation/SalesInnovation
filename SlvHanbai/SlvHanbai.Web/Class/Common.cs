using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Diagnostics;
using System.IO;
using SlvHanbai.Web.Class.DB;
using SlvHanbai.Web.Class.Utility;

namespace SlvHanbai.Web.Class
{
    public class CommonUtl
    {
        private const string CLASS_NM = "CommonUtl";

        public enum UpdKbn { Ins = 0, Upd };
        public static ExMySQLData gMySqlDt = null;
        public static string gConnectionString1;
        public static string gstrErrMsg;
        public static int gSysDbKbn;        // 0:sysdbと会社使用db違う 1:sysdbと会社使用db同じ
        public static int gDemoKbn;         // 0:Demoでない 1:Demo
        public static int gCommandTimeOut;
        //public static string gstrMainUrl = "http://123.108.6.202/SlvHanbai";
        //public static string gstrMainUrl = "https://system-innovation.biz/SlvHanbai";
        //public static string gstrReportTemp = @"C:\inetpub\wwwroot\SlvHanbai\temp\";
        //public static string gstrFileUpLoadDir = @"C:\inetpub\wwwroot\SlvHanbai\Support\Upload_File\";
        //public static string gstrFileUpLoadDutiesDir = @"C:\inetpub\wwwroot\SlvHanbai\Duties\Upload_File\";
        public static string gstrMainUrl = "https://system-innovation.biz/SlvHanbai_demo";
        public static string gstrReportTemp = @"C:\inetpub\wwwroot\SlvHanbai_demo\temp\";
        public static string gstrFileUpLoadDir = @"C:\inetpub\wwwroot\SlvHanbai_demo\Support\Upload_File\";
        public static string gstrFileUpLoadDutiesDir = @"C:\inetpub\wwwroot\SlvHanbai_demo\Duties\Upload_File\";

        public static ExSession gSession = new ExSession();

        //public static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // 分類グループ区分
        public enum geMGroupKbn
        {
            None = 0,
            CustomerGrouop1,            // 得意先分類1
            CustomerGrouop2,            // 得意先分類2
            CustomerGrouop3,            // 得意先分類2
            CommodityGrouop1,           // 商品分類1
            CommodityGrouop2,           // 商品分類2
            CommodityGrouop3,           // 商品分類2
            PurchaseGrouop1,            // 仕入先分類1
            PurchaseGrouop2,            // 仕入先分類2
            PurchaseGrouop3             // 仕入先分類2
        };

        // (入金・支払)条件マスタ区分
        public enum geMConditionKbn
        {
            RECEIPT = 1,                // 入金
            PAYMENT                     // 支払
        };

        // 名称マスタ取得区分
        public enum geNameKbn
        {
            NONE = 0,
            TAX_CHANGE_ID = 1,              //  1:税転換ID
            BUSINESS_DIVISION_ID,           //  2:取引区分ID(売上)
            BREAKDOWN_ID,                   //  3:内訳ID
            DELIVER_DIVISION_ID,            //  4:納品区分ID
            UNIT_ID,				        //  5:単位ID
            TAX_DIVISION_ID,                //  6:課税区分ID
            INVENTORY_DIVISION_ID,          //  7:在庫管理区分ID
            UNIT_PRICE_DIVISION_ID,         //  8:単価区分ID
            DISPLAY_DIVISION_ID,            //  9:表示区分ID
            TITLE_ID,                       // 10:敬称ID
            FRACTION_PROC_ID,               // 11:端数処理ID
            COLLECT_CYCLE_ID,               // 12:回収サイクルID
            CLASS,                          // 13:分類区分ID
            DIVIDE_PERMISSION_ID,           // 14:分納許可ID
            INQUIRY_DIVISION_ID,            // 15:問い合わせ区分ID
            LEVEL_ID,                       // 16:レベルID
            INQUIRY_STATE_ID,               // 17:問い合わせ状態ID
            APPROVAL_STATE_ID,              // 18:承認状態ID
            ACCOUNT_KBN,                    // 19:預金種別
            OPEN_CLOSE_STATE_ID,            // 20:状態ID
            BUSINESS_DIVISION_PU_ID,        // 21:取引区分ID(仕入)
            SEND_KBN,                       // 22:発送区分
            TAX_CHANGE_PU_ID,               // 23:税転換ID(仕入)
            UNIT_PRICE_DIVISION_PU_ID,      // 24:単価区分ID(仕入)
            IN_OUT_DELIVERY_KBN,            // 25:入出庫区分
            IN_OUT_DELIVERY_PROC_KBN,       // 25:入出庫処理区分
            IN_OUT_DELIVERY_TO_KBN          // 26:入出庫先区分
        };

        public enum geStrOrNumKbn
        {
            String = 0,
            Number
        };

        // (入金・支払)条件マスタ区分ID取得
        public static int GetMConditionKbnID(geMConditionKbn conditionkbn)
        {
            switch (conditionkbn)
            {
                case geMConditionKbn.RECEIPT:
                    return 1;
                case geMConditionKbn.PAYMENT:
                    return 2;
                default:
                    return 0;
            }
        }

        // 更新区分名取得
        public static string GetUpdateKbnNm(int kbn)
        {
            switch (kbn)
            {
                case 0:
                    return "更新未";
                case 1:
                    return "更新済";
                default:
                    return "";
            }
        }

        #region Common SQL

        /// <summary>
        /// 共通追加SQL(WHO COLUMS)取得
        /// </summary>
        /// <param name="intKbn"></param>
        /// <param name="pgid"></param>
        /// <param name="tblName"></param>
        /// <param name="person_id"></param>
        /// <param name="denNo"></param>
        /// <param name="ipAdress"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static string GetInsSQLCommonColums(UpdKbn intKbn, string pgid, string tblName, int person_id, string denNo, string ipAdress, string userId)
        {
            // 日時取得
            DateTime now = DateTime.Now;
            string date = now.ToString("yyyy/MM/dd");
            string time = now.ToString("HH:mm:ss");

            StringBuilder sb = new StringBuilder();
            if (intKbn == UpdKbn.Ins)
            {
                // insert
                sb.Append("       ,0" + Environment.NewLine);                                                                           // UPDATE_FLG
                sb.Append("       ,0" + Environment.NewLine);                                                                           // DELETE_FLG
                sb.Append("       ," + ExEscape.zRepStr(pgid) + Environment.NewLine);                                                              // CREATE_PG_ID
                sb.Append("       ," + ExEscape.zRepStr(ipAdress) + Environment.NewLine);                                                          // CREATE_ADRESS
                sb.Append("       ," + ExEscape.zRepStr(userId) + Environment.NewLine);                                                            // CREATE_USER_ID
                sb.Append("       ," + ExEscape.zRepStr(person_id.ToString()) + Environment.NewLine);                                              // CREATE_PERSON_ID
                sb.Append("       ," + ExEscape.zRepStr(date) + Environment.NewLine);                                                              // CREATE_DATE
                sb.Append("       ," + ExEscape.zRepStr(time) + Environment.NewLine);                                                              // CREATE_TIME
                sb.Append("       ," + ExEscape.zRepStr(pgid) + Environment.NewLine);                                                              // UPDATE_PG_ID
                sb.Append("       ," + ExEscape.zRepStr(ipAdress) + Environment.NewLine);                                                          // UPDATE_ADRESS
                sb.Append("       ," + ExEscape.zRepStr(userId) + Environment.NewLine);                                                            // UPDATE_USER_ID
                sb.Append("       ," + ExEscape.zRepStr(person_id) + Environment.NewLine);                                              // UPDATE_PERSON_ID
                sb.Append("       ," + ExEscape.zRepStr(date) + Environment.NewLine);                                                              // UPDATE_DATE
                sb.Append("       ," + ExEscape.zRepStr(time) + Environment.NewLine);                                                              // UPDATE_TIME

            }
            else
            {
                // Update
                string _pgId = "";
                string _adress = "";
                string _userId = "";
                string _personId = "";
                string _date = "";
                string _time = "";

                GetSQLCommonUpdate(denNo, tblName, ref _pgId, ref _adress, ref _userId, ref _personId, ref _date, ref _time);
                sb.Append("       ,1" + Environment.NewLine);                                                                           // UPDATE_FLG
                sb.Append("       ,0" + Environment.NewLine);                                                                           // DELETE_FLG
                sb.Append("       ," + ExEscape.zRepStr(_pgId) + Environment.NewLine);                                                             // CREATE_PG_ID
                sb.Append("       ," + ExEscape.zRepStr(_adress) + Environment.NewLine);                                                           // CREATE_ADRESS
                sb.Append("       ," + ExEscape.zRepStr(_userId) + Environment.NewLine);                                                           // CREATE_USER_ID
                sb.Append("       ," + ExEscape.zRepStr(_personId) + Environment.NewLine);                                                         // CREATE_PERSON_ID
                sb.Append("       ," + ExEscape.zRepStr(ExCast.zDateNullToDefault(_date)) + Environment.NewLine);                                  // CREATE_DATE
                sb.Append("       ," + ExEscape.zRepStr(ExCast.zTimeNullToDefault(_time)) + Environment.NewLine);                                  // CREATE_TIME
                sb.Append("       ," + ExEscape.zRepStr(pgid) + Environment.NewLine);                                                              // UPDATE_PG_ID
                sb.Append("       ," + ExEscape.zRepStr(ipAdress) + Environment.NewLine);                                                          // UPDATE_ADRESS
                sb.Append("       ," + ExEscape.zRepStr(userId) + Environment.NewLine);                                                            // UPDATE_USER_ID
                sb.Append("       ," + ExEscape.zRepStr(person_id) + Environment.NewLine);                                              // UPDATE_PERSON_ID
                sb.Append("       ," + ExEscape.zRepStr(date) + Environment.NewLine);                                                              // UPDATE_DATE
                sb.Append("       ," + ExEscape.zRepStr(time) + Environment.NewLine);                                                              // UPDATE_TIME
            }

            return sb.ToString();

        }

        private static void GetSQLCommonUpdate(string denNo, string tblNmae, ref string pgId, ref string adress, ref string userId, ref string personId, ref string date, ref string time)
        {
            StringBuilder sb;
            DataTable dt;
            ExMySQLData db;
            string _id = "";

            try
            {
                db = ExSession.GetSessionDb(ExCast.zCInt(HttpContext.Current.Session[ExSession.USER_ID]),
                                            ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]));

                sb = new StringBuilder();
                sb.Length = 0;
                sb.Append("SELECT CREATE_PG_ID " + Environment.NewLine);
                sb.Append("      ,CREATE_ADRESS " + Environment.NewLine);
                sb.Append("      ,CREATE_USER_ID " + Environment.NewLine);
                sb.Append("      ,CREATE_PERSON_ID " + Environment.NewLine);
                sb.Append("      ,date_format(CREATE_DATE , " + ExEscape.SQL_YMD + ") AS CREATE_DATE" + Environment.NewLine);
                sb.Append("      ,CREATE_TIME " + Environment.NewLine);
                sb.Append("  FROM " + tblNmae + Environment.NewLine);
                sb.Append(" WHERE COMPANY_ID = " + ExCast.zCStr(HttpContext.Current.Session[ExSession.COMPANY_ID]) + Environment.NewLine);

                if (tblNmae.Substring(0, 2) == "T_")
                {
                    // 伝票入力
                    sb.Append("   AND GROUP_ID = " + ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_ID]) + Environment.NewLine);

                    switch (tblNmae)
                    {
                        case "T_ORDER_D":
                            _id = "ORDER_ID";
                            break;
                        case "T_SALES_D":
                            _id = "SALES_ID";
                            break;
                        case "T_RECEIPT_D":
                            _id = "RECEIPT_ID";
                            break;
                        case "T_ESTIMATE_D":
                            _id = "ESTIMATE_ID";
                            break;
                        default:
                            _id = "NO";
                            break;

                    }
                }
                else
                {
                    // マスタ
                    _id = "ID";
                }
                sb.Append("   AND " + _id + " = " + denNo.ToString() + Environment.NewLine);
                sb.Append(" ORDER BY " + _id + " DESC " + Environment.NewLine);
                sb.Append(" LIMIT 0, 1");

                dt = db.GetDataTable(sb.ToString());

                if (dt.DefaultView.Count > 0)
                {
                    pgId = ExCast.zCStr(dt.DefaultView[0]["CREATE_PG_ID"]);
                    adress = ExCast.zCStr(dt.DefaultView[0]["CREATE_ADRESS"]);
                    userId = ExCast.zCStr(dt.DefaultView[0]["CREATE_USER_ID"]);
                    personId = ExCast.zCStr(dt.DefaultView[0]["CREATE_PERSON_ID"]);
                    date = ExCast.zCStr(dt.DefaultView[0]["CREATE_DATE"]);
                    time = ExCast.zCStr(dt.DefaultView[0]["CREATE_TIME"]);
                }
            }
            catch (Exception ex)
            {
                //CommonUtl.ExLogger.Error(CLASS_NM + ".GetSQLCommonUpdate TableName:" + tblNmae + " ProgramId:" + pgId, ex);
            }
            finally
            {
                db = null;
            }

        }

        /// <summary>
        /// 共通更新SQL(WHO COLUMS)取得
        /// </summary>
        /// <param name="pgid"></param>
        /// <param name="person_id"></param>
        /// <param name="ipAdress"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static string GetUpdSQLCommonColums(string pgid, int person_id, string ipAdress, string userId, int delete_flg)
        {
            // 日時取得
            DateTime now = DateTime.Now;
            string date = now.ToString("yyyy/MM/dd");
            string time = now.ToString("HH:mm:ss");

            StringBuilder sb = new StringBuilder();

            sb.Append("   SET DELETE_FLG = " + delete_flg.ToString() + Environment.NewLine);
            sb.Append("      ,UPDATE_FLG = 1" + Environment.NewLine);
            sb.Append("      ,UPDATE_PG_ID = " + ExEscape.zRepStr(pgid) + Environment.NewLine);                        // UPDATE_PG_ID
            sb.Append("      ,UPDATE_ADRESS = " + ExEscape.zRepStr(ipAdress) + Environment.NewLine);                   // UPDATE_ADRESS
            sb.Append("      ,UPDATE_USER_ID = " + ExEscape.zRepStr(userId) + Environment.NewLine);                    // UPDATE_USER_ID
            sb.Append("      ,UPDATE_PERSON_ID = " + ExEscape.zRepStr(person_id) + Environment.NewLine);               // UPDATE_PERSON_ID
            sb.Append("      ,UPDATE_DATE = " + ExEscape.zRepStr(date) + Environment.NewLine);                         // UPDATE_DATE
            sb.Append("      ,UPDATE_TIME = " + ExEscape.zRepStr(time) + Environment.NewLine);                         // UPDATE_TIME

            return sb.ToString();

        }

        public static string GetCommonColumsSelectStr(string tbl)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("      ," + tbl + ".ATTRIBUTE1  AS D_ATTRIBUTE1" + Environment.NewLine);
            sb.Append("      ," + tbl + ".ATTRIBUTE2  AS D_ATTRIBUTE2" + Environment.NewLine);
            sb.Append("      ," + tbl + ".ATTRIBUTE3  AS D_ATTRIBUTE3" + Environment.NewLine);
            sb.Append("      ," + tbl + ".ATTRIBUTE4  AS D_ATTRIBUTE4" + Environment.NewLine);
            sb.Append("      ," + tbl + ".ATTRIBUTE5  AS D_ATTRIBUTE5" + Environment.NewLine);
            sb.Append("      ," + tbl + ".ATTRIBUTE6  AS D_ATTRIBUTE6" + Environment.NewLine);
            sb.Append("      ," + tbl + ".ATTRIBUTE7  AS D_ATTRIBUTE7" + Environment.NewLine);
            sb.Append("      ," + tbl + ".ATTRIBUTE8  AS D_ATTRIBUTE8" + Environment.NewLine);
            sb.Append("      ," + tbl + ".ATTRIBUTE9  AS D_ATTRIBUTE9" + Environment.NewLine);
            sb.Append("      ," + tbl + ".ATTRIBUTE10 AS D_ATTRIBUTE10" + Environment.NewLine);
            sb.Append("      ," + tbl + ".ATTRIBUTE11 AS D_ATTRIBUTE11" + Environment.NewLine);
            sb.Append("      ," + tbl + ".ATTRIBUTE12 AS D_ATTRIBUTE12" + Environment.NewLine);
            sb.Append("      ," + tbl + ".ATTRIBUTE13 AS D_ATTRIBUTE13" + Environment.NewLine);
            sb.Append("      ," + tbl + ".ATTRIBUTE14 AS D_ATTRIBUTE14" + Environment.NewLine);
            sb.Append("      ," + tbl + ".ATTRIBUTE15 AS D_ATTRIBUTE15" + Environment.NewLine);
            sb.Append("      ," + tbl + ".ATTRIBUTE16 AS D_ATTRIBUTE16" + Environment.NewLine);
            sb.Append("      ," + tbl + ".ATTRIBUTE17 AS D_ATTRIBUTE17" + Environment.NewLine);
            sb.Append("      ," + tbl + ".ATTRIBUTE18 AS D_ATTRIBUTE18" + Environment.NewLine);
            sb.Append("      ," + tbl + ".ATTRIBUTE19 AS D_ATTRIBUTE19" + Environment.NewLine);
            sb.Append("      ," + tbl + ".ATTRIBUTE20 AS D_ATTRIBUTE20" + Environment.NewLine);
            sb.Append("      ," + tbl + ".UPDATE_FLG     AS D_UPDATE_FLG" + Environment.NewLine);
            sb.Append("      ," + tbl + ".DELETE_FLG     AS D_DELETE_FLG" + Environment.NewLine);
            sb.Append("      ," + tbl + ".CREATE_PG_ID   AS D_CREATE_PG_ID" + Environment.NewLine);
            sb.Append("      ," + tbl + ".CREATE_ADRESS  AS D_CREATE_ADRESS" + Environment.NewLine);
            sb.Append("      ," + tbl + ".CREATE_USER_ID AS D_CREATE_USER_ID" + Environment.NewLine);
            sb.Append("      ," + tbl + ".CREATE_DATE    AS D_CREATE_DATE" + Environment.NewLine);
            sb.Append("      ," + tbl + ".CREATE_TIME    AS D_CREATE_TIME" + Environment.NewLine);
            sb.Append("      ," + tbl + ".UPDATE_PG_ID   AS D_UPDATE_PG_ID" + Environment.NewLine);
            sb.Append("      ," + tbl + ".UPDATE_ADRESS  AS D_UPDATE_ADRESS" + Environment.NewLine);
            sb.Append("      ," + tbl + ".UPDATE_USER_ID AS D_UPDATE_USER_ID" + Environment.NewLine);
            sb.Append("      ," + tbl + ".UPDATE_DATE    AS D_UPDATE_DATE" + Environment.NewLine);
            sb.Append("      ," + tbl + ".UPDATE_TIME    AS D_UPDATE_TIME" + Environment.NewLine);
            return sb.ToString();
        }

        #endregion

        public class ExLogger
        {
            public static void Info(string log)
            {
                logOut(log, null);
            }

            public static void Error(string log, Exception ex)
            {
                logOut(log, ex);
            }

            public static void Error(string log)
            {
                logOut(log, null);
            }

            private static void logOut(string log, Exception ex)
            {
                string _companyId = ExCast.zCStr(HttpContext.Current.Session[ExSession.COMPANY_ID]);
                string _userId = ExCast.zCStr(HttpContext.Current.Session[ExSession.USER_ID]);
                string _now = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss").Replace("i", "");

                try
                {
                    using (FileStream fileStream = new FileStream(@"F:\sitesroot\0\log\AppLog.txt", FileMode.Append))
                    using (Stream streamSync = Stream.Synchronized(fileStream))
                    using (BinaryWriter writer = new BinaryWriter(streamSync))
                    {
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        sb.Append(_now + " >>>>>>>> log start " + " [COMPANY ID : " + _companyId + "] [USER ID : " + _userId + "] -------------------- " + Environment.NewLine);
                        sb.Append(_now + " >>>>>>>> " + log + Environment.NewLine);
                        if (ex != null)
                        {
                            sb.Append(_now + " >>>>>>>> " + "--------------- Exception Message --------------------" + Environment.NewLine +
                                      _now + " >>>>>>>> " + ex.Message + Environment.NewLine);
                            sb.Append(_now + " >>>>>>>> " + "--------------- Exception HelpLink -------------------" + Environment.NewLine +
                                      _now + " >>>>>>>> " + ex.HelpLink + Environment.NewLine);
                            sb.Append(_now + " >>>>>>>> " + "--------------- Exception Source ---------------------" + Environment.NewLine +
                                      _now + " >>>>>>>> " + ex.Source + Environment.NewLine);
                            sb.Append(_now + " >>>>>>>> " + "--------------- Exception StackTrace -----------------" + Environment.NewLine +
                                      _now + " >>>>>>>> " + ex.StackTrace + Environment.NewLine);
                            sb.Append(_now + " >>>>>>>> " + "--------------- Exception TargetSite -----------------" + Environment.NewLine +
                                      _now + " >>>>>>>> " + ex.TargetSite + Environment.NewLine);
                            sb.Append(_now + " >>>>>>>> " + "------------------------------------------------------" + Environment.NewLine);
                        }
                        sb.Append(_now + " >>>>>>>> log end   " + " [COMPANY ID : " + _companyId + "] [USER ID : " + _userId + "] -------------------- " + Environment.NewLine + Environment.NewLine);

                        byte[] bytesData = System.Text.Encoding.GetEncoding("shift_jis").GetBytes(sb.ToString());
                        writer.Write(bytesData);
                        writer.Flush();
                    }
                }
                catch (Exception e)
                { 
                    Debug.Print(ex.Message.ToString());
                }
            }
        }
    }
}