using Bitcliq.BIR.Logs;
using Bitcliq.BIR.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitcliq.BIR.Platform
{
    public class Reporter
    {
        #region PRIVATE
        private int _ID;
        private string _Email;
        private string _Name;
        private DateTime _CreatedDate;
        private string _IPAddress;
        private string _Password;
        private bool _Deleted;

        private string _PhoneNumber;

        #endregion

        #region PUBLIC
        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        public string Email
        {
            get { return _Email; }
            set { _Email = value; }
        }
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
        public DateTime CreatedDate
        {
            get { return _CreatedDate; }
            set { _CreatedDate = value; }
        }
        public string IPAddress
        {
            get { return _IPAddress; }
            set { _IPAddress = value; }
        }
        public string Password
        {
            get { return _Password; }
            set { _Password = value; }
        }
        public bool Deleted
        {
            get { return _Deleted; }
            set { _Deleted = value; }
        }

        public string PhoneNumber
        {
            get { return _PhoneNumber; }
            set { _PhoneNumber = value; }
        }
        #endregion

        #region CONSTRUCTORS
        public Reporter()
        {
            CleanValues();
        }

        public Reporter(int p_ID)
        {
            Get(p_ID);
        }

        public Reporter(DataRow dr)
        {
            FillValues(dr);
        }
        #endregion

        #region GET
        private void Get(int p_ID)
        {
            try
            {
                if (p_ID > 0)
                {
                    SqlDataObject sql = new SqlDataObject(StaticKeys.ConnStr, "Reporter_ListByID");
                    sql.AddInputParameter(false, "@ID", p_ID);

                    DataSet ds = sql.ExecuteDataSet();

                    if (UtilMethods.DataSetHasData(ds))
                    {

                        FillValues(ds.Tables[0].Rows[0]);
                    }
                    else
                    {
                        CleanValues();
                    }
                }
                else
                {
                    CleanValues();
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.LOGGER.Error(ex.Message, ex);
                CleanValues();
            }
        }
        #endregion

        #region INSERT
        protected bool Insert()
        {
            try
            {
                SqlDataObject sql = new SqlDataObject(StaticKeys.ConnStr, "Reporter_Insert");
                //sql.AddInputParameter(false, "@ID", _ID);
                sql.AddInputParameter(false, "@Email", _Email);
                sql.AddInputParameter(false, "@Name", _Name);
                //sql.AddInputParameter(false, "@CreatedDate", DateTime.Now);
                sql.AddInputParameter(false, "@IPAddress", _IPAddress);
                sql.AddInputParameter(false, "@Password", PasswordManager.GenerateMD5Password(_Password));
                //sql.AddInputParameter(false, "@Deleted", _Deleted);
                //sql.AddInputParameter(false, "@PhoneNumber", PhoneNumber);

                int ret = (int)UtilMethods.toInt(sql.ExecuteScalar(), 0);

                if (ret <= 0)
                {
                    return false;
                }

                _ID = ret;

                return true;
            }
            catch (Exception ex)
            {
                ErrorLogger.LOGGER.Error(ex.Message, ex);
                return false;
            }
        }
        #endregion

        #region UPDATE
        protected bool Update()
        {
            try
            {
                SqlDataObject sql = new SqlDataObject(StaticKeys.ConnStr, "Reporter_Update");
                sql.AddInputParameter(false, "@ID", _ID);
                sql.AddInputParameter(false, "@Email", _Email);
                sql.AddInputParameter(false, "@Name", _Name);
                sql.AddInputParameter(false, "@CreatedDate", _CreatedDate);
                sql.AddInputParameter(false, "@IPAddress", _IPAddress);
                sql.AddInputParameter(false, "@Password", PasswordManager.GenerateMD5Password(_Password));
                sql.AddInputParameter(false, "@Deleted", _Deleted);
                sql.AddInputParameter(false, "@PhoneNumber", PhoneNumber);

                int ret = (int)UtilMethods.toInt(sql.ExecuteScalar(), 0);

                if (ret <= 0)
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                ErrorLogger.LOGGER.Error(ex.Message, ex);
                return false;
            }
        }
        #endregion

        #region SAVE
        public bool Save()
        {
            if (_ID > 0)
            {
                return Update();
            }
            else
            {
                return Insert();
            }
        }
        #endregion

        #region DELETE
        public bool Delete()
        {
            try
            {
                SqlDataObject sql = new SqlDataObject(StaticKeys.ConnStr, "Reporter_Delete");
                sql.AddInputParameter(false, "@ID", _ID);
                sql.AddInputParameter(false, "@Email", _Email);
                sql.AddInputParameter(false, "@Name", _Name);
                sql.AddInputParameter(false, "@CreatedDate", _CreatedDate);
                sql.AddInputParameter(false, "@IPAddress", _IPAddress);
                sql.AddInputParameter(false, "@Password", _Password);
                sql.AddInputParameter(false, "@Deleted", _Deleted);

                int ret = (int)UtilMethods.toInt(sql.ExecuteScalar(), 0);

                if (ret <= 0)
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                ErrorLogger.LOGGER.Error(ex.Message, ex);
                return false;
            }
        }
        #endregion


        #region STATIC FOR WEB SERVICE

        public static Reporter Login(string Email, string Password)
        {

            try
            {
                SqlDataObject sql = new SqlDataObject(StaticKeys.ConnStr, "Reporter_Login");

                sql.AddInputParameter(false, "@Email", Email);
                sql.AddInputParameter(false, "@Password", PasswordManager.GenerateMD5Password(Password));
                


                DataSet ds = sql.ExecuteDataSet();


                if (UtilMethods.DataSetHasOnlyOneDataRow(ds))
                {
                    Reporter user = new Reporter(ds.Tables[0].Rows[0]);


                    return user;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                ErrorLogger.LOGGER.Error(ex.Message, ex);
                return null;
            }

        }



        public static int CountByEmail(string Email)
        {
            try
            {
                SqlDataObject sql = new SqlDataObject(StaticKeys.ConnStr, "Reporter_CountByEmail");
                sql.AddInputParameter(false, "@Email", Email);
                
                return  (int)UtilMethods.toInt(sql.ExecuteScalar(), 0);


            }
            catch (Exception ex)
            {
                ErrorLogger.LOGGER.Error(ex.Message, ex);
                return -1;
            }
        }


        public static int Register(string Name,  string Email, string Password, string PhoneNumber, string IPAddress)
        {
            try
            {

                try
                {
                    SqlDataObject sql = new SqlDataObject(StaticKeys.ConnStr, "Reporter_Insert");
                    
                    sql.AddInputParameter(false, "@Email", Email);
                    sql.AddInputParameter(false, "@Name", Name);
                    
                    sql.AddInputParameter(false, "@IPAddress", IPAddress);
                    sql.AddInputParameter(false, "@Password", PasswordManager.GenerateMD5Password(Password));
                    sql.AddInputParameter(false, "@PhoneNumber", PhoneNumber);

                    return  (int)UtilMethods.toInt(sql.ExecuteScalar(), 0);

                    
                }
                catch (Exception ex)
                {
                    ErrorLogger.LOGGER.Error(ex.Message, ex);
                    return -1;
                }

            }
            catch (Exception ex)
            {
                ErrorLogger.LOGGER.Error(ex.Message, ex);
                return -1;
            }
        }


        #endregion


        public DataSet GetIssues(int? AccountID, int? TypeID, int? Priority)
        {
            try
            {
                SqlDataObject sql = new SqlDataObject(StaticKeys.ConnStr, "Issue_GetByReporterID");

                sql.AddInputParameter(false, "@ReporterID", _ID);
                sql.AddInputParameter(false, "@AccountID", AccountID);
                if (TypeID > 0)
                    sql.AddInputParameter(false, "@TypeID", TypeID);
                if (Priority > 0)
                    sql.AddInputParameter(false, "@Priority", Priority);


                return sql.ExecuteDataSet();
            }
            catch (Exception ex)
            {
                ErrorLogger.LOGGER.Error(ex.Message, ex);
                return null;
            }

        }

        public DataSet ListIssuesToDataSet(int p_AccountID, string p_Filters, int p_StartIndex, int p_NumRecords, string p_Order)
        {
            try
            {

                string whereclause = null;

                try
                {
                    if (p_Filters + "" != "")
                        whereclause = WhereClauseGenerator.ParseFilter(JsonConvert.DeserializeObject<Filter>(p_Filters)).ToString();
                }
                catch (Exception)
                {
                }

                SqlDataObject sql = new SqlDataObject(StaticKeys.ConnStr, "Reporter_ListIssuesToDataSet");

                sql.AddInputParameter(false, "@ReporterID", _ID);
                sql.AddInputParameter(false, "@AccountID", p_AccountID);
                sql.AddInputParameter(false, "@p_Si", p_StartIndex);
                sql.AddInputParameter(false, "@p_iPage", p_NumRecords);
                if (p_Order.Trim() == "")
                    p_Order = "ID desc";
                sql.AddInputParameter(false, "@p_SortColumnName", p_Order);

                if (p_Filters + "" != "")
                    sql.AddInputParameter(false, "@p_filters", p_Filters);
                else
                    sql.AddInputParameter(false, "@p_filters", null);

                return sql.ExecuteDataSet();
            }
            catch (Exception ex)
            {
                ErrorLogger.LOGGER.Error(ex.Message, ex);
                return null;
            }

        }


        #region DATASET - FOR JSON
        public DataSet ListToDataSet(int AccountID, string p_Filters, int p_StartIndex, int p_NumRecords, string p_Order)
        {
            try
            {


                string whereclause = null;

                try
                {
                    if (p_Filters + "" != "")
                        whereclause = WhereClauseGenerator.ParseFilter(JsonConvert.DeserializeObject<Filter>(p_Filters)).ToString();
                }
                catch (Exception)
                {
                }

                SqlDataObject sql = new SqlDataObject(StaticKeys.ConnStr, "Reporter_ListToDataSet");


                sql.AddInputParameter(false, "@AccountID", AccountID);
                sql.AddInputParameter(false, "@p_Si", p_StartIndex);
                sql.AddInputParameter(false, "@p_iPage", p_NumRecords);

                if (p_Order.Trim() == "")
                    p_Order = "ID desc";
                sql.AddInputParameter(false, "@p_SortColumnName", p_Order);

                if (p_Filters + "" != "")
                    sql.AddInputParameter(false, "@p_filters", p_Filters);
                else
                    sql.AddInputParameter(false, "@p_filters", null);


                return sql.ExecuteDataSet();




            }
            catch (Exception ex)
            {

                ErrorLogger.LOGGER.Error(ex.Message, ex);
                return null;
            }
        }
        #endregion


        #region CLEAN VALUES
        private void CleanValues()
        {
            _ID = 0;
            _Email = "";
            _Name = null;
            _CreatedDate = DateTime.MinValue;
            _IPAddress = null;
            _Password = null;
            _Deleted = false;
            _PhoneNumber = null;
        }
        #endregion

        #region FILL VALUES
        private void FillValues(DataRow dr)
        {
            _ID = (int)UtilMethods.toInt(dr["ID"], 0);
            _Email = dr["Email"] + "";
            _Name = dr["Name"] + "";// ? null : dr["Name"] + "";
            _CreatedDate = (DateTime)UtilMethods.toDateTime(dr["CreatedDate"], DateTime.MinValue);
            _IPAddress = dr["IPAddress"] + "" == "" ? null : dr["IPAddress"] + "";
            _Password = dr["Password"] + "" == "" ? null : dr["Password"] + "";
            _Deleted = (bool)UtilMethods.toBool(dr["Deleted"], false);
            _PhoneNumber = dr["PhoneNumber"] + "";// == "" ? null : dr["PhoneNumber"] + "";
        }
        #endregion

    }
}
