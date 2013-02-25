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
    public class DataCredit
    {
        private const string CLASS_NM = "DataCredit";

        /// <summary>
        /// 売掛残高更新
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="db"></param>
        /// <param name="customerId"></param>
        /// <param name="ipAdress"></param>
        /// <param name="userId"></param>
        public static void UpdSalesCredit(string companyId,
                                          string groupId,
                                          ExMySQLData db, 
                                          string customerId,
                                          double price,
                                          string pg_nm,
                                          int update_person_id,
                                          string ipAdress, 
                                          string userId)
        {
            StringBuilder sb;

            try
            {
                sb = new StringBuilder();

                sb.Length = 0;
                sb.Append("UPDATE M_CUSTOMER " + Environment.NewLine);
                sb.Append(CommonUtl.GetUpdSQLCommonColums(pg_nm,
                                                          update_person_id,
                                                          ipAdress,
                                                          userId,
                                                          0));
                sb.Append("      ,SALES_CREDIT_PRICE = SALES_CREDIT_PRICE + " + price.ToString() + Environment.NewLine);

                sb.Append(" WHERE COMPANY_ID = " + companyId + Environment.NewLine);                                // COMPANY_ID
                sb.Append("   AND ID = " + ExEscape.zRepStr(ExCast.zNumZeroNothingFormat(customerId)) + Environment.NewLine);  // ID

                db.ExecuteSQL(sb.ToString(), false);

                sb.Length = 0;
                sb.Append("UPDATE M_SALES_CREDIT_BALANCE " + Environment.NewLine);

                sb.Append(CommonUtl.GetUpdSQLCommonColums(pg_nm,
                                                          update_person_id,
                                                          ipAdress,
                                                          userId,
                                                          0));
                sb.Append("      ,SALES_CREDIT_PRICE = SALES_CREDIT_PRICE + " + price.ToString() + Environment.NewLine);

                sb.Append(" WHERE DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND COMPANY_ID = " + companyId + Environment.NewLine);
                sb.Append("   AND GROUP_ID = " + groupId + Environment.NewLine);
                sb.Append("   AND ID = " + ExEscape.zRepStr(ExCast.zNumZeroNothingFormat(customerId)) + Environment.NewLine);  // ID

                db.ExecuteSQL(sb.ToString(), false);

            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdSalesCredit", ex);
                throw;
            }
        }

        /// <summary>
        /// 買掛残高更新
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="db"></param>
        /// <param name="customerId"></param>
        /// <param name="ipAdress"></param>
        /// <param name="userId"></param>
        public static void UpdPaymentCredit(string companyId,
                                            string groupId,
                                            ExMySQLData db,
                                            string purchaseId,
                                            double price,
                                            string pg_nm,
                                            int update_person_id,
                                            string ipAdress,
                                            string userId)
        {
            StringBuilder sb;

            try
            {
                sb = new StringBuilder();
                sb.Length = 0;

                sb.Append("UPDATE M_PURCHASE " + Environment.NewLine);
                sb.Append(CommonUtl.GetUpdSQLCommonColums(pg_nm,
                                                          update_person_id,
                                                          ipAdress,
                                                          userId,
                                                          0));
                sb.Append("      ,PAYMENT_CREDIT_PRICE = PAYMENT_CREDIT_PRICE + " + price.ToString() + Environment.NewLine);

                sb.Append(" WHERE COMPANY_ID = " + companyId + Environment.NewLine);                                            // COMPANY_ID
                sb.Append("   AND ID = " + ExEscape.zRepStr(ExCast.zNumZeroNothingFormat(purchaseId)) + Environment.NewLine);   // ID

                db.ExecuteSQL(sb.ToString(), false);

                sb.Length = 0;
                sb.Append("UPDATE M_PAYMENT_CREDIT_BALANCE " + Environment.NewLine);

                sb.Append(CommonUtl.GetUpdSQLCommonColums(pg_nm,
                                                          update_person_id,
                                                          ipAdress,
                                                          userId,
                                                          0));
                sb.Append("      ,PAYMENT_CREDIT_PRICE = PAYMENT_CREDIT_PRICE + " + price.ToString() + Environment.NewLine);

                sb.Append(" WHERE DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND COMPANY_ID = " + companyId + Environment.NewLine);
                sb.Append("   AND GROUP_ID = " + groupId + Environment.NewLine);
                sb.Append("   AND ID = " + ExEscape.zRepStr(ExCast.zNumZeroNothingFormat(purchaseId)) + Environment.NewLine);  // ID

                db.ExecuteSQL(sb.ToString(), false);
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdPaymentCredit", ex);
                throw;
            }
        }

   }
}