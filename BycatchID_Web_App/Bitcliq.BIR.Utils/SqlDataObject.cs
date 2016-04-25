using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace Bitcliq.BIR.Utils
{
    public class SqlDataObject
    {
        #region Definitions
        protected SqlConnection Conn;
        protected SqlCommand Cmd;
        #endregion

        #region Constructor
   


        public SqlDataObject(string p_ConnStr, string p_Text)
        {
            Conn = new SqlConnection(p_ConnStr);
            Cmd = new SqlCommand();

            Cmd.CommandType = CommandType.StoredProcedure;
            Cmd.CommandText = p_Text;
            Cmd.Connection = Conn;
            Cmd.CommandTimeout = 300000;
            Cmd.Prepare();
        }
        #endregion

        #region Open/Close Methods
        public void Close()
        {
            if (Conn.State != ConnectionState.Closed)
            {
                Conn.Close();
                Conn.Dispose();
            }

            Cmd.Dispose();
        }

        public void Open()
        {
            if (Conn.State == ConnectionState.Closed)
                Conn.Open();
        }
        #endregion

        #region Add Parameters Methods
        public void AddInputParameterNullBinary(string p_Name)
        {
            Cmd.Parameters.Add(p_Name, SqlDbType.Binary);
            Cmd.Parameters[p_Name].Value = DBNull.Value;
        }

        public void AddInputParameter(bool p_isBinary, string p_Name, object p_Value)
        {
            if (p_isBinary)
            {
                Cmd.Parameters.Add(p_Name, SqlDbType.Binary);

                if (p_Value == null)
                    Cmd.Parameters[p_Name].Value = DBNull.Value;
                else
                    Cmd.Parameters[p_Name].Value = p_Value;
            }
            else
            {
                if (p_Value == null)
                    Cmd.Parameters.AddWithValue(p_Name, DBNull.Value);
                else
                    Cmd.Parameters.AddWithValue(p_Name, p_Value);
            }
        }

        public void AddInputParameterTable(string p_Name, string p_TypeName, object p_Value)
        {



            if (p_Value == null)
                Cmd.Parameters[p_Name].Value = DBNull.Value;
            else
            {

                Cmd.Parameters.Add(p_Name, SqlDbType.Structured);
                Cmd.Parameters[p_Name].Direction = ParameterDirection.Input;

                // The name of the table type.
                Cmd.Parameters[p_Name].TypeName = p_TypeName;

                // And the value actually we pass; the List with albums.
                Cmd.Parameters[p_Name].Value = p_Value;
            }


        }
        #endregion

        #region ExecuteNoQuery Method
        public void ExecuteNoQuery()
        {
            try
            {
                Open();
                Cmd.ExecuteNonQuery();
            }
            finally
            {
                Close();
            }
        }
        #endregion

        #region ExecuteScalar Method
        public object ExecuteScalar()
        {
            object ret = null;

            try
            {
                Open();
                ret = Cmd.ExecuteScalar();
            }
            finally
            {
                Close();
            }

            return ret;
        }
        #endregion

        #region Execute DataSet Method
        public DataSet ExecuteDataSet()
        {
            DataSet ds = new DataSet();

            try
            {
                Open();

                SqlDataAdapter da = new SqlDataAdapter(Cmd);
                da.Fill(ds);
            }
            finally
            {
                Close();
            }

            return ds;
        }
        #endregion
    }
}
