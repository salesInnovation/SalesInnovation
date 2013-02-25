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
    public class DataSlipNo
    {
        private const string CLASS_NM = "DataSlipNo";

        // 伝票No取得区分
        public enum geSlipKbn
        {
            Estimate = 1,           // 見積
            Order,                  // 受注
            Sales,                  // 売上
            Invoice,                // 請求
            Receipt,                // 入金
            Purchase,               // 発注
            Vendor,				    // 仕入
            Payment,                // 支払
            PaymentCash,            // 出金
            Produce,                // 生産
            InOutDelivery           // 入出庫
        };

        public enum geInOutDeliverySlipKbn
        {
            Sales = 2,              // 売上
            Purchase                // 仕入
        };

        /// <summary>
        /// 伝票番号取得
        /// </summary>
        /// <param name="companyId">会社ID</param>
        /// <param name="groupId">グループID</param>
        /// <param name="db"></param>
        /// <param name="kbn">伝票区分</param>
        /// <param name="accountPeriod">会計年</param>
        /// <param name="no">伝票番号</param>
        /// <param name="id">伝票ID</param>
        /// <param name="ipAdress"></param>
        /// <param name="userId"></param>
        public static void GetSlipNo(string companyId, 
                                     string groupId, 
                                     ExMySQLData db, 
                                     geSlipKbn kbn, 
                                     string accountPeriod, 
                                     ref long no,
                                     out long id, 
                                     string ipAdress, 
                                     string userId)
        {
            StringBuilder sb;
            DataTable dt;

            bool exist = false;

            try
            {
                sb = new StringBuilder();
                sb.Length = 0;

                // 存在確認
                sb.Append("SELECT SM.* " + Environment.NewLine);
                sb.Append("  FROM M_SLIP_MANAGEMENT AS SM" + Environment.NewLine);
                sb.Append(" WHERE SM.COMPANY_ID = " + companyId + Environment.NewLine);
                sb.Append("   AND SM.GROUP_ID = " + groupId + Environment.NewLine);
                sb.Append("   AND SM.SLIP_DIVISION = " + ((int)kbn).ToString() + Environment.NewLine);
                sb.Append("   AND SM.YEAR = " + ExCast.zCInt(accountPeriod).ToString() + Environment.NewLine);
                sb.Append("   AND SM.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append(" LIMIT 0, 1" + Environment.NewLine);
                sb.Append(" FOR UPDATE");
                dt = db.GetDataTable(sb.ToString());

                if (dt.DefaultView.Count > 0)
                {
                    id = ExCast.zCLng(dt.DefaultView[0]["ID"]) + 1;
                    // 自動採番追加時
                    if (no == 0)
                    {
                        no = ExCast.zCLng(dt.DefaultView[0]["NO"]) + 1;
                    }
                }
                else
                {
                    if (no == 0) no = 1;
                    id = 1;

                    sb.Length = 0;
                    sb.Append("INSERT INTO M_SLIP_MANAGEMENT " + Environment.NewLine);
                    sb.Append("       ( COMPANY_ID" + Environment.NewLine);
                    sb.Append("       , GROUP_ID" + Environment.NewLine);
                    sb.Append("       , SLIP_DIVISION" + Environment.NewLine);
                    sb.Append("       , YEAR" + Environment.NewLine);
                    sb.Append("       , ID" + Environment.NewLine);
                    sb.Append("       , NO" + Environment.NewLine);
                    sb.Append("       , UPDATE_FLG" + Environment.NewLine);
                    sb.Append("       , DELETE_FLG" + Environment.NewLine);
                    sb.Append("       , CREATE_PG_ID" + Environment.NewLine);
                    sb.Append("       , CREATE_ADRESS" + Environment.NewLine);
                    sb.Append("       , CREATE_USER_ID" + Environment.NewLine);
                    sb.Append("       , CREATE_PERSON_ID" + Environment.NewLine);
                    sb.Append("       , CREATE_DATE" + Environment.NewLine);
                    sb.Append("       , CREATE_TIME" + Environment.NewLine);
                    sb.Append("       , UPDATE_PG_ID" + Environment.NewLine);
                    sb.Append("       , UPDATE_ADRESS" + Environment.NewLine);
                    sb.Append("       , UPDATE_USER_ID" + Environment.NewLine);
                    sb.Append("       , UPDATE_PERSON_ID" + Environment.NewLine);
                    sb.Append("       , UPDATE_DATE" + Environment.NewLine);
                    sb.Append("       , UPDATE_TIME" + Environment.NewLine);
                    sb.Append(")" + Environment.NewLine);
                    sb.Append("VALUES (" + Environment.NewLine);
                    sb.Append("        " + companyId + Environment.NewLine);                        // COMPANY_ID
                    sb.Append("       ," + groupId + Environment.NewLine);                          // GROUP_ID
                    sb.Append("       ," + ((int)kbn).ToString() + Environment.NewLine);            // SLIP_DIVISION
                    sb.Append("       ," + ExCast.zCInt(accountPeriod) + Environment.NewLine);      // YEAR
                    sb.Append("       ," + id.ToString() + Environment.NewLine);                    // ID
                    sb.Append("       ," + no.ToString() + Environment.NewLine);                    // NO
                    sb.Append(CommonUtl.GetInsSQLCommonColums(CommonUtl.UpdKbn.Ins, "OrderInp", "T_ORDER_H", 9999, "0", ipAdress, userId));
                    sb.Append(")" + Environment.NewLine);

                    db.ExecuteSQL(sb.ToString(), false);
                }

            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetSlipNo", ex);
                no = 0;
                id = 0;
                throw;
            }
        }

        public static void UpdateSlipNo(string companyId,
                                        string groupId,
                                        ExMySQLData db,
                                        geSlipKbn kbn,
                                        string accountPeriod,
                                        long no,
                                        long id)
        {
            StringBuilder sb;

            try
            {
                sb = new StringBuilder();

                sb.Append("UPDATE M_SLIP_MANAGEMENT " + Environment.NewLine);
                sb.Append("   SET ID = " + id.ToString() + Environment.NewLine);
                // 追加時
                if (no != 0)
                {
                    sb.Append("      ,NO = " + no.ToString() + Environment.NewLine);
                }
                sb.Append(" WHERE COMPANY_ID = " + companyId + Environment.NewLine);
                sb.Append("   AND GROUP_ID = " + groupId + Environment.NewLine);
                sb.Append("   AND SLIP_DIVISION = " + ((int)kbn).ToString() + Environment.NewLine);
                sb.Append("   AND YEAR = " + ExCast.zCInt(accountPeriod).ToString() + Environment.NewLine);
                sb.Append("   AND DELETE_FLG = 0 " + Environment.NewLine);

                db.ExecuteSQL(sb.ToString(), false);

            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateSlipNo", ex);
                no = 0;
                id = 0;
                throw;
            }
        }

        /// <summary>
        /// 伝票番号取得
        /// </summary>
        /// <param name="companyId">会社ID</param>
        /// <param name="groupId">グループID</param>
        /// <param name="db"></param>
        /// <param name="kbn">伝票区分</param>
        /// <param name="accountPeriod">会計年</param>
        /// <param name="no">伝票番号</param>
        /// <param name="id">伝票ID</param>
        /// <param name="ipAdress"></param>
        /// <param name="userId"></param>
        public static long GetMaxSlipNo(string companyId,
                                        string groupId,
                                        ExMySQLData db,
                                        string tableName,
                                        string columnName)
        {
            StringBuilder sb;
            DataTable dt;

            long no = 1;
            try
            {
                sb = new StringBuilder();
                sb.Length = 0;

                // 存在確認
                sb.Append("SELECT MAX(T." + columnName + ") AS MAX_NO " + Environment.NewLine);
                sb.Append("  FROM " + tableName + " AS T" + Environment.NewLine);
                sb.Append(" WHERE T.COMPANY_ID = " + companyId + Environment.NewLine);
                if (groupId != "")
                {
                    sb.Append("   AND T.GROUP_ID = " + groupId + Environment.NewLine);
                }
                sb.Append(" FOR UPDATE");

                dt = db.GetDataTable(sb.ToString());

                if (dt.DefaultView.Count > 0)
                {
                    no = ExCast.zCLng(dt.DefaultView[0]["MAX_NO"]) + 1;
                }

                return no;
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetMaxSlipNo", ex);
                throw;
            }
        }

        /// <summary>
        /// 伝票番号取得
        /// </summary>
        /// <param name="companyId">会社ID</param>
        /// <param name="groupId">グループID</param>
        /// <param name="db"></param>
        /// <param name="kbn">伝票区分</param>
        /// <param name="accountPeriod">会計年</param>
        /// <param name="no">伝票番号</param>
        /// <param name="id">伝票ID</param>
        /// <param name="ipAdress"></param>
        /// <param name="userId"></param>
        public static void GetInOutDeliveryNo(string companyId,
                                              string groupId,
                                              ExMySQLData db,
                                              geInOutDeliverySlipKbn kbn,
                                              long cause_no,
                                              ref long _no,
                                              ref long _id)
        {
            StringBuilder sb;
            DataTable dt;

            _no = 0;
            _id = 0;

            try
            {
                sb = new StringBuilder();
                sb.Length = 0;

                // 存在確認
                sb.Append("SELECT ID " + Environment.NewLine);
                sb.Append("      ,NO " + Environment.NewLine);
                sb.Append("  FROM T_IN_OUT_DELIVERY_H AS T" + Environment.NewLine);
                sb.Append(" WHERE T.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND T.COMPANY_ID = " + companyId + Environment.NewLine);
                sb.Append("   AND T.GROUP_ID = " + groupId + Environment.NewLine);
                sb.Append("   AND T.IN_OUT_DELIVERY_PROC_KBN = " + (int)kbn + Environment.NewLine);
                sb.Append("   AND T.CAUSE_NO = " + cause_no + Environment.NewLine);

                dt = db.GetDataTable(sb.ToString());

                if (dt.DefaultView.Count > 0)
                {
                    _no = ExCast.zCLng(dt.DefaultView[0]["NO"]);
                    _id = ExCast.zCLng(dt.DefaultView[0]["ID"]);
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetInOutDeliveryNo", ex);
                throw;
            }
        }

    }
}