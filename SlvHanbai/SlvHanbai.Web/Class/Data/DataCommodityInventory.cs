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
    public class DataCommodityInventory
    {
        private const string CLASS_NM = "DataCommodityInventory";

        /// <summary>
        /// 商品在庫更新
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="db"></param>
        /// <param name="customerId"></param>
        /// <param name="ipAdress"></param>
        /// <param name="userId"></param>
        public static void UpdCommodityInventory(string companyId,
                                                 string groupId,
                                                 ExMySQLData db, 
                                                 string customerId,
                                                 double number,
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
                sb.Append("UPDATE M_COMMODITY " + Environment.NewLine);
                sb.Append(CommonUtl.GetUpdSQLCommonColums(pg_nm,
                                                          update_person_id,
                                                          ipAdress,
                                                          userId,
                                                          0));
                sb.Append("      ,INVENTORY_NUMBER = INVENTORY_NUMBER + " + number.ToString() + Environment.NewLine);

                sb.Append(" WHERE COMPANY_ID = " + companyId + Environment.NewLine);                                // COMPANY_ID
                sb.Append("   AND ID = " + ExEscape.zRepStr(ExCast.zNumZeroNothingFormat(customerId)) + Environment.NewLine);  // ID

                db.ExecuteSQL(sb.ToString(), false);

                sb.Length = 0;
                sb.Append("UPDATE M_COMMODITY_INVENTORY " + Environment.NewLine);
                sb.Append(CommonUtl.GetUpdSQLCommonColums(pg_nm,
                                                          update_person_id,
                                                          ipAdress,
                                                          userId,
                                                          0));
                sb.Append("      ,INVENTORY_NUMBER = INVENTORY_NUMBER + " + number.ToString() + Environment.NewLine);

                sb.Append(" WHERE COMPANY_ID = " + companyId + Environment.NewLine);                                           // COMPANY_ID
                sb.Append("   AND GROUP_ID = " + groupId + Environment.NewLine);
                sb.Append("   AND ID = " + ExEscape.zRepStr(ExCast.zNumZeroNothingFormat(customerId)) + Environment.NewLine);  // ID

                db.ExecuteSQL(sb.ToString(), false);

            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdCommodityInventory", ex);
                throw;
            }
        }
   }
}