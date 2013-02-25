using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace SlvHanbai.Web.Class.DB
{
    public class ExMySQLData
    {
        private const String CLASS_NM = "ExMySQLData.";
        private MySqlDataAdapter mAdp = new MySqlDataAdapter();
        private MySqlConnection mCn = new MySqlConnection();
        private MySqlCommand mCmd = new MySqlCommand();
        private MySqlTransaction mTran = null;
        public string errMessage = "";

        public ExMySQLData()
        {
            mCn.ConnectionString = CommonUtl.gConnectionString1;
            mCmd.Connection = mCn;
            mCmd.CommandTimeout = CommonUtl.gCommandTimeOut;
        }

        public ExMySQLData(string srcCnString) 
        { 
            mCn.ConnectionString = srcCnString;
            mCmd.Connection = mCn;
            mCmd.CommandTimeout = CommonUtl.gCommandTimeOut;
        }

        public bool DbOpen()
        {
            try
            { 
                if (mCmd.Connection.State == System.Data.ConnectionState.Closed)
                {
                    mCmd.Connection.Open();
                }
                return true;
            }
            catch (Exception ex)
            { 
                CommonUtl.ExLogger.Error(CLASS_NM + "Open " + Environment.NewLine + mCn.ConnectionString, ex);
                errMessage = ex.ToString();
                throw;
            }
        }

        public bool DbClose()
        {
            try
            {
                if (mCmd.Connection.State == System.Data.ConnectionState.Open)
                {
                    mCmd.Connection.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + "Close " + Environment.NewLine + mCn.ConnectionString, ex);
                errMessage = ex.ToString();
                throw;
            }
        }

        public DataTable GetDataTable(string strSQL)
        {
            try
            {
                mCmd.CommandText = strSQL;
                mAdp.SelectCommand = mCmd;

                if (DbOpen() == false)
                {
                    return null;
                }
                DataSet ds = new DataSet();
                mAdp.Fill(ds);
                return ds.Tables[0];

            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + "GetDataTable " + Environment.NewLine + strSQL, ex);
                errMessage = ex.ToString();
                throw;
            }
        }

        public DataSet GetDataSet(string strSQL, string tblName)
        {
            try
            {
                mCmd.CommandText = strSQL;
                mAdp.SelectCommand = mCmd;

                if (DbOpen() == false)
                {
                    return null;
                }
                DataSet ds = new DataSet();
                mAdp.Fill(ds);
                ds.Tables[0].TableName = tblName;
                return ds;

            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + "GetDataTable " + Environment.NewLine + strSQL, ex);
                errMessage = ex.ToString();
                throw;
            }
        }

        public bool ExecuteSQL(string strSQL)
        {
            try
            {
                mCmd.CommandText = strSQL;

                if (DbOpen() == false)
                {
                    return false;
                }
                mCmd.Transaction = null;
                mCmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + "ExecuteSQL " + Environment.NewLine + strSQL, ex);
                errMessage = ex.ToString();
                throw;
            }
        }

        public bool ExecuteSQL(string strSQL, bool blnTran)
        {
            try
            {
                mCmd.CommandText = strSQL;

                if (DbOpen() == false)
                {
                    return false;
                }
                if (blnTran == true)
                {
                    mTran = mCmd.Connection.BeginTransaction();
                }
                mCmd.Transaction = mTran;
                mCmd.ExecuteNonQuery();
                if (blnTran == true)
                {
                    mTran.Commit();
                }
                return true;
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + "ExecuteSQL " + Environment.NewLine + strSQL, ex);
                errMessage = ex.ToString();
                throw;
            }
            finally
            {
                if (blnTran == true)
                {
                    mTran = null;
                    mCmd.Transaction = null;
                }
            }
        }

        public bool ExBeginTransaction()
        {
            try
            {
                if (DbOpen() == false)
                {
                    return false;
                }
                if (mTran == null || mCmd.Transaction == null)
                {
                    mTran = mCmd.Connection.BeginTransaction();
                    mCmd.Transaction = mTran;
                }
                return true;
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + "ExBeginTransaction ", ex);
                errMessage = ex.ToString();
                throw;
            }
        }

        public bool ExCommitTransaction()
        {
            try
            {
                if (DbOpen() == false)
                {
                    return false;
                }
                if (mTran == null || mCmd.Transaction == null)
                {
                    return false;
                }
                mTran.Commit();
                return true;
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + "ExCommitTransaction ", ex);
                errMessage = ex.ToString();
                throw;
            }
            finally
            {
                mTran = null;
                mCmd.Transaction = null;
            }
        }

        public bool ExRollbackTransaction()
        {
            try
            {
                if (DbOpen() == false)
                {
                    return false;
                }
                if (mTran == null || mCmd.Transaction == null)
                {
                    return false;
                }
                mTran.Rollback();
                return true;
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + "ExCommitTransaction ", ex);
                errMessage = ex.ToString();
                return false;
            }
            finally
            {
                mTran = null;
                mCmd.Transaction = null;
            }
        }

    }
}
