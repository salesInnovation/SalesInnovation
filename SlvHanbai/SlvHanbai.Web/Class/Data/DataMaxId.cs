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
    public class DataMaxId
    {
        private const string CLASS_NM = "DataMaxId";

        // マスタ最大ID取得区分
        public enum geMasterMaxIdKbn
        {
            Person = 1,        // 担当
            Customer,          // 得意先
            Supplier,          // 納入先
            Commodity          // 商品
        };

        /// <summary>
        /// 最大ID取得
        /// </summary>
        /// <param name="db"></param>
        /// <param name="id">ID</param>
        /// <param name="ipAdress"></param>
        /// <param name="userId"></param>
        public static void GetMaxId(ExMySQLData db,
                                    string tbl_name,
                                    string id_col_name,
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
                sb.Append("SELECT " + id_col_name + " AS MAX_ID " + Environment.NewLine);
                sb.Append("  FROM " + tbl_name + Environment.NewLine);
                sb.Append(" ORDER BY " + id_col_name + " DESC " + Environment.NewLine);
                sb.Append(" LIMIT 0, 1 " + Environment.NewLine);

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
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetMaxId", ex);
                id = "";
                throw;
            }
        }

    }
}