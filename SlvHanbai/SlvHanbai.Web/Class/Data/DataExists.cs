using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Data;
using SlvHanbai.Web.Class;
using SlvHanbai.Web.Class.DB;
using SlvHanbai.Web.Class.Entity;
using SlvHanbai.Web.Class.Utility;

namespace SlvHanbai.Web.Class.Data
{
    public class DataExists
    {
        private const string CLASS_NM = "DataExists";

        #region 存在チェック

        public static bool IsExistData(ExMySQLData db, 
                                       string companyId,
                                       string groupId,
                                       string tblName, 
                                       string col1Name, 
                                       string col1Value,
                                       CommonUtl.geStrOrNumKbn col1Type)
        {
            StringBuilder sb;
            DataTable dt;

            try
            {
                sb = new StringBuilder();

                #region SQL

                sb.Append("SELECT TBL.* " + Environment.NewLine);
                sb.Append("  FROM " + tblName + " AS TBL" + Environment.NewLine);

                sb.Append(" WHERE TBL.DELETE_FLG = 0 " + Environment.NewLine);
                if (companyId != "")
                {
                    sb.Append("   AND TBL.COMPANY_ID = " + companyId + Environment.NewLine);
                }
                if (groupId != "")
                {
                    sb.Append("   AND TBL.GROUP_ID = " + groupId + Environment.NewLine);
                }
                if (col1Type == CommonUtl.geStrOrNumKbn.String)
                {
                    sb.Append("   AND TBL." + col1Name + " = " + ExEscape.zRepStr(col1Value) + Environment.NewLine);
                }
                else
                {
                    sb.Append("   AND TBL." + col1Name + " = " + ExCast.zCDbl(col1Value) + Environment.NewLine);
                }
                sb.Append(" LIMIT 0, 1" + Environment.NewLine);

                #endregion

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
                CommonUtl.ExLogger.Error(CLASS_NM + ".IsExistData", ex);
                throw;
            }
            finally
            {
            }
        }

        public static bool IsExistData(ExMySQLData db,
                               string companyId,
                               string groupId,
                               string tblName,
                               string col1Name,
                               string col1Value,
                               string col2Name,
                               string col2Value,
                               CommonUtl.geStrOrNumKbn col1Type)
        {
            StringBuilder sb;
            DataTable dt;

            try
            {
                sb = new StringBuilder();

                #region SQL

                sb.Append("SELECT TBL.* " + Environment.NewLine);
                sb.Append("  FROM " + tblName + " AS TBL" + Environment.NewLine);

                sb.Append(" WHERE TBL.DELETE_FLG = 0 " + Environment.NewLine);
                if (companyId != "")
                {
                    sb.Append("   AND TBL.COMPANY_ID = " + companyId + Environment.NewLine);
                }
                if (groupId != "")
                {
                    sb.Append("   AND TBL.GROUP_ID = " + groupId + Environment.NewLine);
                }
                if (col1Type == CommonUtl.geStrOrNumKbn.String)
                {
                    sb.Append("   AND TBL." + col1Name + " = " + ExEscape.zRepStr(col1Value) + Environment.NewLine);
                    sb.Append("   AND TBL." + col2Name + " <> " + ExEscape.zRepStr(col2Value) + Environment.NewLine);
                }
                else
                {
                    sb.Append("   AND TBL." + col1Name + " = " + ExCast.zCDbl(col1Value) + Environment.NewLine);
                    sb.Append("   AND TBL." + col2Name + " <> " + ExCast.zCDbl(col2Value) + Environment.NewLine);
                }
                sb.Append(" LIMIT 0, 1" + Environment.NewLine);

                #endregion

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
                CommonUtl.ExLogger.Error(CLASS_NM + ".IsExistData", ex);
                throw;
            }
            finally
            {
            }
        }

        public static bool IsExistDataDouble(ExMySQLData db,
                       string companyId,
                       string groupId,
                       string tblName,
                       string col1Name,
                       string col1Value,
                       string col2Name,
                       string col2Value,
                       CommonUtl.geStrOrNumKbn col1Type)
        {
            StringBuilder sb;
            DataTable dt;

            try
            {
                sb = new StringBuilder();

                #region SQL

                sb.Append("SELECT TBL.* " + Environment.NewLine);
                sb.Append("  FROM " + tblName + " AS TBL" + Environment.NewLine);

                sb.Append(" WHERE TBL.DELETE_FLG = 0 " + Environment.NewLine);
                if (companyId != "")
                {
                    sb.Append("   AND TBL.COMPANY_ID = " + companyId + Environment.NewLine);
                }
                if (groupId != "")
                {
                    sb.Append("   AND TBL.GROUP_ID = " + groupId + Environment.NewLine);
                }
                if (col1Type == CommonUtl.geStrOrNumKbn.String)
                {
                    sb.Append("   AND TBL." + col1Name + " = " + ExEscape.zRepStr(col1Value) + Environment.NewLine);
                    sb.Append("   AND TBL." + col2Name + " = " + ExEscape.zRepStr(col2Value) + Environment.NewLine);
                }
                else
                {
                    sb.Append("   AND TBL." + col1Name + " = " + ExCast.zCDbl(col1Value) + Environment.NewLine);
                    sb.Append("   AND TBL." + col2Name + " = " + ExCast.zCDbl(col2Value) + Environment.NewLine);
                }
                sb.Append(" LIMIT 0, 1" + Environment.NewLine);

                #endregion

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
                CommonUtl.ExLogger.Error(CLASS_NM + ".IsExistDataDouble", ex);
                throw;
            }
            finally
            {
            }
        }
        #endregion

        #region 指定データ取得

        public static void GetData(ExMySQLData db,
                                   string companyId,
                                   string groupId,
                                   string tblName,
                                   string col1Name,
                                   string col1Value,
                                   CommonUtl.geStrOrNumKbn col1Type,
                                   string get_col1Name,
                                   string get_col2Name,
                                   string get_col3Name,
                                   string get_col4Name,
                                   string get_col5Name,
                                   string get_another_col1Name,
                                   string get_another_col2Name,
                                   string get_another_col3Name,
                                   string get_another_col4Name,
                                   string get_another_col5Name,
                                   ref string get_col1Value,
                                   ref string get_col2Value,
                                   ref string get_col3Value,
                                   ref string get_col4Value,
                                   ref string get_col5Value)
        {
            StringBuilder sb;
            DataTable dt;

            try
            {
                sb = new StringBuilder();

                #region SQL

                sb.Append("SELECT " + get_col1Name + Environment.NewLine);
                if (!string.IsNullOrEmpty(get_col2Name)) sb.Append("      ," + get_col2Name + Environment.NewLine);
                if (!string.IsNullOrEmpty(get_col3Name)) sb.Append("      ," + get_col3Name + Environment.NewLine);
                if (!string.IsNullOrEmpty(get_col4Name)) sb.Append("      ," + get_col4Name + Environment.NewLine);
                if (!string.IsNullOrEmpty(get_col5Name)) sb.Append("      ," + get_col5Name + Environment.NewLine);
                sb.Append("  FROM " + tblName + " AS TBL" + Environment.NewLine);

                sb.Append(" WHERE TBL.DELETE_FLG = 0 " + Environment.NewLine);
                if (companyId != "")
                {
                    sb.Append("   AND TBL.COMPANY_ID = " + companyId + Environment.NewLine);
                }
                if (groupId != "")
                {
                    sb.Append("   AND TBL.GROUP_ID = " + groupId + Environment.NewLine);
                }
                if (col1Type == CommonUtl.geStrOrNumKbn.String)
                {
                    sb.Append("   AND TBL." + col1Name + " = " + ExEscape.zRepStr(col1Value) + Environment.NewLine);
                }
                else
                {
                    sb.Append("   AND TBL." + col1Name + " = " + ExCast.zCDbl(col1Value) + Environment.NewLine);
                }
                sb.Append(" LIMIT 0, 1" + Environment.NewLine);

                #endregion

                dt = db.GetDataTable(sb.ToString());

                if (dt.DefaultView.Count > 0)
                {
                    get_col1Value = ExCast.zCStr(dt.DefaultView[0][get_another_col1Name]);
                    if (!string.IsNullOrEmpty(get_col2Name)) get_col2Value = ExCast.zCStr(dt.DefaultView[0][get_another_col2Name]);
                    if (!string.IsNullOrEmpty(get_col3Name)) get_col3Value = ExCast.zCStr(dt.DefaultView[0][get_another_col3Name]);
                    if (!string.IsNullOrEmpty(get_col4Name)) get_col4Value = ExCast.zCStr(dt.DefaultView[0][get_another_col4Name]);
                    if (!string.IsNullOrEmpty(get_col5Name)) get_col5Value = ExCast.zCStr(dt.DefaultView[0][get_another_col5Name]);
                }
                else
                {
                    get_col1Value = "";
                    get_col2Value = "";
                    get_col3Value = "";
                    get_col4Value = "";
                    get_col5Value = "";
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetData", ex);
                throw;
            }
            finally
            {
            }
        }

        #endregion
    }
}