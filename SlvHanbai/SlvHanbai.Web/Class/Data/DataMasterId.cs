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
    public class DataMasterId
    {
        private const string CLASS_NM = "DataMasterId";

        // マスタ最大ID取得区分
        public enum geMasterMaxIdKbn
        {
            CompanyGroup = 1,   // 会社グループ
            Person,             // 担当
            Customer,           // 得意先
            Supplier,           // 納入先
            Commodity,          // 商品
            Purchase            // 仕入先
        };

        /// <summary>
        /// 最大ID取得
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
        public static void GetMaxMasterId(string companyId, 
                                 　　     string groupId, 
                                     　　 ExMySQLData db,
                                          geMasterMaxIdKbn kbn, 
                                     　　 out string id)
        {
            StringBuilder sb;
            DataTable dt;

            bool exist = false;

            try
            {
                sb = new StringBuilder();
                sb.Length = 0;

                // 存在確認
                sb.Append("SELECT MT.ID AS MAX_ID " + Environment.NewLine);

                switch (kbn)
                {
                    case geMasterMaxIdKbn.CompanyGroup:
                        sb.Append("  FROM SYS_M_COMPANY_GROUP AS MT" + Environment.NewLine);
                        break;
                    case geMasterMaxIdKbn.Person:
                        sb.Append("  FROM M_PERSON AS MT" + Environment.NewLine);
                        break;
                    case geMasterMaxIdKbn.Customer:
                        sb.Append("  FROM M_CUSTOMER AS MT" + Environment.NewLine);
                        break;
                    case geMasterMaxIdKbn.Commodity:
                        sb.Append("  FROM M_COMMODITY AS MT" + Environment.NewLine);
                        break;
                    case geMasterMaxIdKbn.Supplier:
                        sb.Append("  FROM M_SUPPLIER AS MT" + Environment.NewLine);
                        break;
                    case geMasterMaxIdKbn.Purchase:
                        sb.Append("  FROM M_PURCHASE AS MT" + Environment.NewLine);
                        break;
                    default:
                        break;
                }

                sb.Append(" WHERE MT.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND MT.COMPANY_ID = " + companyId + Environment.NewLine);

                switch (kbn)
                {
                    case geMasterMaxIdKbn.Supplier:
                        sb.Append("   AND MT.CUSTOMER_ID = " + ExEscape.zRepStr(groupId) + Environment.NewLine);
                        break;
                    default:
                        if (groupId != "")
                        {
                            sb.Append("   AND MT.GROUP_ID = " + groupId + Environment.NewLine);
                        }
                        break;
                }

                switch (kbn)
                {
                    case geMasterMaxIdKbn.Customer:
                    case geMasterMaxIdKbn.Commodity:
                    case geMasterMaxIdKbn.Supplier:
                    case geMasterMaxIdKbn.Purchase:
                        sb.Append("   AND CAST(MT.ID AS SIGNED) > 0 " + Environment.NewLine);
                        sb.Append(" ORDER BY CAST(MT.ID AS SIGNED) DESC " + Environment.NewLine);
                        sb.Append(" LIMIT 0, 1 " + Environment.NewLine);
                        break;
                    default:
                        sb.Append(" ORDER BY MT.ID DESC " + Environment.NewLine);
                        sb.Append(" LIMIT 0, 1 " + Environment.NewLine);
                        break;
                }

                dt = db.GetDataTable(sb.ToString());

                if (dt.DefaultView.Count > 0)
                {
                    id = (ExCast.zCLng(dt.DefaultView[0]["MAX_ID"]) + 1).ToString();
                }
                else
                    id = "1";
                }

            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetMaxMasterId", ex);
                id = "";
                throw;
            }
        }

    }
}