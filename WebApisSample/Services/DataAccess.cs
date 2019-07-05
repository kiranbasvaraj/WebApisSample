using System;
using System.Collections;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Xml.XPath;
using System.Security.Cryptography;

namespace WebApisSample.Services
{
    public class DataAccess
    {
        #region "property"

        private string _CommandTypes;
        public string CommandTypes
        {
            get
            {
                return _CommandTypes;
            }
            set
            {
                _CommandTypes = value;
            }
        }
        String strpath = System.IO.Path.GetTempPath();
        public string LoginKey
        {
            get { return (string)System.Web.HttpContext.Current.Session["UserKey"]; }
        }
        public string LoginUser
        {
            get { return (string)System.Web.HttpContext.Current.Session["UserName"]; }
        }
        public string UserType
        {
            get { return (string)System.Web.HttpContext.Current.Session["UserType"]; }
        }

        #endregion

        #region "Declarations"

        private SqlConnection conDB;
        private SqlCommand cmdDB;
        private SqlDataAdapter dapDB;
        private DataSet dstDB;
        private SqlDataReader drdDB;
        private SqlTransaction tranDB;
        private SqlParameter[] SP_OledbParameter;
        private ArrayList arlSample = new ArrayList();
        public TripleDESCryptoServiceProvider TripleDes = new TripleDESCryptoServiceProvider();
        private string strReturn;
        OleDbCommand cmdHrm;
        OleDbDataAdapter dapHrm;
        OleDbDataReader drdHrm;
        OleDbTransaction tranHrm;
        DataSet dstHrm;
        #endregion

        #region "Database Methods"
        //public string ExecuteSP(string ConnectionString, string SpName, string OutputParam)
        //{
        //    string strRetvalue = "";
        //    try
        //    {
        //        conDB = new SqlConnection(ConfigurationManager.ConnectionStrings[ConnectionString].ConnectionString);
        //        if (conDB.State == ConnectionState.Closed | conDB.State == ConnectionState.Broken)
        //            conDB.Open();
        //        cmdDB = new SqlCommand();
        //        cmdDB.CommandType = CommandType.StoredProcedure;
        //        cmdDB.Connection = conDB;
        //        cmdDB.CommandTimeout = 300;
        //        tranDB = conDB.BeginTransaction();
        //        cmdDB.CommandText = SpName;
        //        cmdDB.Transaction = tranDB;
        //        foreach (SqlParameter oleDbParam in SP_OledbParameter)
        //        {
        //            cmdDB.Parameters.Add(oleDbParam);
        //        }
        //        cmdDB.ExecuteNonQuery();
        //        strRetvalue = cmdDB.Parameters[OutputParam].Value.ToString();
        //        tranDB.Commit();
        //        SP_OledbParameter = null;
        //        return strRetvalue;
        //    }
        //    catch (Exception ex)
        //    {
        //        tranDB.Rollback();
        //        throw (ex);
        //    }
        //    finally
        //    {
        //        if (conDB.State == ConnectionState.Open)
        //            conDB.Close();
        //    }
        //}

        public void InputParameter(string ParameterName, int FieldSize, object ParameterValue, object ParameterType)
        {
            if (SP_OledbParameter == null)
                Array.Resize<SqlParameter>(ref SP_OledbParameter, 1);
            else
                Array.Resize<SqlParameter>(ref SP_OledbParameter, SP_OledbParameter.Length + 1);

            SP_OledbParameter[SP_OledbParameter.Length - 1] = new SqlParameter();
            SP_OledbParameter[SP_OledbParameter.Length - 1].ParameterName = ParameterName;
            SP_OledbParameter[SP_OledbParameter.Length - 1].Size = FieldSize;
            SP_OledbParameter[SP_OledbParameter.Length - 1].Value = ParameterValue;
            SP_OledbParameter[SP_OledbParameter.Length - 1].SqlDbType = (SqlDbType)ParameterType;
            SP_OledbParameter[SP_OledbParameter.Length - 1].Direction = ParameterDirection.Input;
        }

        public void OutputParameter(string ParameterName, int FieldSize, object ParameterType)
        {

            if (SP_OledbParameter == null)
                Array.Resize<SqlParameter>(ref SP_OledbParameter, 1);
            else
                Array.Resize<SqlParameter>(ref SP_OledbParameter, SP_OledbParameter.Length + 1);

            SP_OledbParameter[SP_OledbParameter.Length - 1] = new SqlParameter();
            SP_OledbParameter[SP_OledbParameter.Length - 1].ParameterName = ParameterName;
            SP_OledbParameter[SP_OledbParameter.Length - 1].Size = FieldSize;
            SP_OledbParameter[SP_OledbParameter.Length - 1].SqlDbType = (SqlDbType)ParameterType;
            if (CommandType.Text.ToString().ToUpper() == "FUNCTION")
                SP_OledbParameter[SP_OledbParameter.Length - 1].Direction = ParameterDirection.ReturnValue;
            else
                SP_OledbParameter[SP_OledbParameter.Length - 1].Direction = ParameterDirection.Output;
        }
             
        #endregion

        #region New Methods
        public string GetSingleValueForCCS(string ConnectionString, string QueryId)
        {
            try
            {
                string strVal = null;
                conDB = new SqlConnection(ConfigurationManager.ConnectionStrings[ConnectionString].ConnectionString);
                if (conDB.State == ConnectionState.Closed | conDB.State == ConnectionState.Broken)
                    conDB.Open();
                cmdDB = new SqlCommand();
                cmdDB.Connection = conDB;
                cmdDB.CommandText = QueryId;
                drdDB = cmdDB.ExecuteReader();
                while (drdDB.Read())
                {
                    if (drdDB[0] != null)
                        strVal = drdDB[0].ToString();
                }
                return strVal;
            }
            catch (Exception Ex)
            {
                throw (Ex);
            }
            finally
            {
                if (conDB.State == ConnectionState.Open)
                    conDB.Close();
            }
        }
        public DataSet GetDatasetForCCS(string ConnectionString, string QueryId)
        {
            try
            {
                conDB = new SqlConnection(ConfigurationManager.ConnectionStrings[ConnectionString].ConnectionString);
                if (conDB.State == ConnectionState.Closed | conDB.State == ConnectionState.Broken)
                    conDB.Open();
                cmdDB = new SqlCommand();
                cmdDB.Connection = conDB;
                cmdDB.CommandText = QueryId;
                dapDB = new SqlDataAdapter(cmdDB);
                dstDB = new DataSet();

                dapDB.Fill(dstDB);
                return dstDB;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (conDB.State == ConnectionState.Open)
                    conDB.Close();
            }
        }

        public string ExecuteSPForCCS(string ConnectionString, string SpName, string OutputParam)
        {
            string strRetvalue = "";
            try
            {
                conDB = new SqlConnection(ConfigurationManager.ConnectionStrings[ConnectionString].ConnectionString);
                if (conDB.State == ConnectionState.Closed | conDB.State == ConnectionState.Broken)
                    conDB.Open();
                cmdDB = new SqlCommand();
                cmdDB.CommandType = CommandType.StoredProcedure;
                cmdDB.Connection = conDB;
                cmdDB.CommandTimeout = 300;
                tranDB = conDB.BeginTransaction();
                cmdDB.CommandText = SpName;
                cmdDB.Transaction = tranDB;
                foreach (SqlParameter oleDbParam in SP_OledbParameter)
                {
                    cmdDB.Parameters.Add(oleDbParam);
                }
                cmdDB.ExecuteNonQuery();
                strRetvalue = cmdDB.Parameters[OutputParam].Value.ToString();
                tranDB.Commit();
                SP_OledbParameter = null;
                return strRetvalue;
            }
            catch (Exception ex)
            {
                tranDB.Rollback();
                throw (ex);
            }
            finally
            {
                if (conDB.State == ConnectionState.Open)
                    conDB.Close();
            }
        }

        #endregion
    }
}