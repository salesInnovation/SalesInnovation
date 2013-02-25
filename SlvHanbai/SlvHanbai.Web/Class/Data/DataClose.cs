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
    public class DataClose
    {
        private const string CLASS_NM = "DataClose";

        /// <summary>
        /// 売掛締チェック
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="db"></param>
        /// <param name="customerId"></param>
        /// <param name="ipAdress"></param>
        /// <param name="userId"></param>
        public static bool IsInvoiceClose(string companyId, 
                                          ExMySQLData db, 
                                          string invoiceId,
                                          string ymd)
        {                              
            StringBuilder sb = new StringBuilder();
            DataTable dt;

            if (ymd.Length != 10) throw new Exception();
            if (!ExCast.IsDate(ymd)) throw new Exception();

            try
            {
                sb.Length = 0;
                sb.Append("SELECT T.NO " + Environment.NewLine);
                sb.Append("  FROM T_INVOICE AS T" + Environment.NewLine);
                sb.Append(" WHERE T.COMPANY_ID = " + companyId + Environment.NewLine);
                sb.Append("   AND T.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND T.INVOICE_KBN = 0 " + Environment.NewLine);       // 請求区分:締処理
                sb.Append("   AND T.INVOICE_YYYYMMDD >= " + ExEscape.zRepStr(ymd) + Environment.NewLine);
                if (invoiceId != "")
                {
                    sb.Append("   AND T.INVOICE_ID = " + ExEscape.zRepStr(ExCast.zNumZeroNothingFormat(invoiceId)) + Environment.NewLine);
                }
                sb.Append(" LIMIT 0, 1");

                dt = db.GetDataTable(sb.ToString());
                if (dt.DefaultView.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".IsInvoiceClose", ex);
                throw;
            }
        }

        /// <summary>
        /// 支払締チェック
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="db"></param>
        /// <param name="customerId"></param>
        /// <param name="ipAdress"></param>
        /// <param name="userId"></param>
        public static bool IsPaymentClose(string companyId,
                                          ExMySQLData db,
                                          string purchaseId,
                                          string ymd)
        {
            StringBuilder sb = new StringBuilder();
            DataTable dt;

            if (ymd.Length != 10) throw new Exception();
            if (!ExCast.IsDate(ymd)) throw new Exception();

            try
            {
                sb.Length = 0;
                sb.Append("SELECT T.NO " + Environment.NewLine);
                sb.Append("  FROM T_PAYMENT AS T" + Environment.NewLine);
                sb.Append(" WHERE T.COMPANY_ID = " + companyId + Environment.NewLine);
                sb.Append("   AND T.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND T.PAYMENT_KBN = 0 " + Environment.NewLine);       // 支払区分:締処理
                sb.Append("   AND T.PAYMENT_YYYYMMDD >= " + ExEscape.zRepStr(ymd) + Environment.NewLine);
                if (purchaseId != "")
                {
                    sb.Append("   AND T.PURCHASE_ID = " + ExEscape.zRepStr(ExCast.zNumZeroNothingFormat(purchaseId)) + Environment.NewLine);
                }
                sb.Append(" LIMIT 0, 1");

                dt = db.GetDataTable(sb.ToString());
                if (dt.DefaultView.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".IsPaymentClose", ex);
                throw;
            }
        }

   }
}