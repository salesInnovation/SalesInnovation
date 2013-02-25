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
    public class DataDeliverDivision
    {
        private const string CLASS_NM = "DataDeliverDivision";

        public enum gUpdDeliverDivisionKbn { Order, Purchase }

        /// <summary>
        /// 納品区分更新
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="db"></param>
        /// <param name="customerId"></param>
        /// <param name="ipAdress"></param>
        /// <param name="userId"></param>
        public static void UpdDeliverDivision(gUpdDeliverDivisionKbn kbn,
                                              string companyId,
                                              string groupId,
                                              long Id,
                                              int recNo,
                                              ExMySQLData db, 
                                              int deliverDivisionId,
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

                switch (kbn)
                { 
                    case gUpdDeliverDivisionKbn.Order:
                        sb.Append("UPDATE T_ORDER_D " + Environment.NewLine);
                        sb.Append(CommonUtl.GetUpdSQLCommonColums(pg_nm,
                                                                  update_person_id,
                                                                  ipAdress,
                                                                  userId,
                                                                  0));
                        sb.Append("      ,DELIVER_DIVISION_ID = " + deliverDivisionId.ToString() + Environment.NewLine);
                        sb.Append(" WHERE COMPANY_ID = " + companyId + Environment.NewLine);
                        sb.Append("   AND GROUP_ID = " + groupId + Environment.NewLine);
                        sb.Append("   AND ORDER_ID = " + Id + Environment.NewLine);
                        sb.Append("   AND REC_NO = " + recNo + Environment.NewLine);
                        break;
                    case gUpdDeliverDivisionKbn.Purchase:
                        sb.Append("UPDATE T_PURCHASE_ORDER_D " + Environment.NewLine);
                        sb.Append(CommonUtl.GetUpdSQLCommonColums(pg_nm,
                                                                  update_person_id,
                                                                  ipAdress,
                                                                  userId,
                                                                  0));
                        sb.Append("      ,DELIVER_DIVISION_ID = " + deliverDivisionId.ToString() + Environment.NewLine);
                        sb.Append(" WHERE COMPANY_ID = " + companyId + Environment.NewLine);
                        sb.Append("   AND GROUP_ID = " + groupId + Environment.NewLine);
                        sb.Append("   AND PURCHASE_ORDER_ID = " + Id + Environment.NewLine);
                        sb.Append("   AND REC_NO = " + recNo + Environment.NewLine);
                        break;
                }


                db.ExecuteSQL(sb.ToString(), false);

            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdDeliverDivision", ex);
                throw;
            }
        }
   }
}