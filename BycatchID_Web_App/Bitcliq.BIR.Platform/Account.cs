using Bitcliq.BIR.Logs;
using Bitcliq.BIR.Utils;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitcliq.BIR.Platform
{
    public class Account
    {
        #region PRIVATE
        private int _ID;
        private string _Name;
        private string _Email;
        private DateTime _CreatedDate;
        private DateTime? _ModifiedDate;
        private int? _CreatedBy;
        private int? _ModifiedBy;
        #endregion

        #region PUBLIC
        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
        public string Email
        {
            get { return _Email; }
            set { _Email = value; }
        }
        public DateTime CreatedDate
        {
            get { return _CreatedDate; }
            set { _CreatedDate = value; }
        }
        public DateTime? ModifiedDate
        {
            get { return _ModifiedDate; }
            set { _ModifiedDate = value; }
        }
        public int? CreatedBy
        {
            get { return _CreatedBy; }
            set { _CreatedBy = value; }
        }
        public int? ModifiedBy
        {
            get { return _ModifiedBy; }
            set { _ModifiedBy = value; }
        }
        #endregion

        #region CONSTRUCTORS
        public Account()
        {
            CleanValues();
        }

        public Account(int p_ID)
        {
            Get(p_ID);
        }

        public Account(DataRow dr)
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
                    SqlDataObject sql = new SqlDataObject(StaticKeys.ConnStr, "Account_ListByID");
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
                SqlDataObject sql = new SqlDataObject(StaticKeys.ConnStr, "Account_Insert");
                sql.AddInputParameter(false, "@Name", _Name);
                sql.AddInputParameter(false, "@Email", _Email);
                sql.AddInputParameter(false, "@CreatedBy", _CreatedBy);

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
                SqlDataObject sql = new SqlDataObject(StaticKeys.ConnStr, "Account_Update");
                sql.AddInputParameter(false, "@ID", _ID);
                sql.AddInputParameter(false, "@Name", _Name);
                sql.AddInputParameter(false, "@Email", _Email);
                sql.AddInputParameter(false, "@ModifiedBy", _ModifiedBy);

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
                SqlDataObject sql = new SqlDataObject(StaticKeys.ConnStr, "Account_Delete");
                sql.AddInputParameter(false, "@ID", _ID);


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

        #region CLEAN VALUES
        private void CleanValues()
        {
            _ID = 0;
            _Name = "";
            _Email = "";
            _CreatedDate = DateTime.MinValue;
            _ModifiedDate = null;
            _CreatedBy = null;
            _ModifiedBy = null;
        }
        #endregion

        #region FILL VALUES
        private void FillValues(DataRow dr)
        {
            _ID = (int)UtilMethods.toInt(dr["ID"], 0);
            _Name = dr["Name"] + "";
            _Email = dr["Email"] + "";
            _CreatedDate = (DateTime)UtilMethods.toDateTime(dr["CreatedDate"], DateTime.MinValue);
            _ModifiedDate = UtilMethods.toDateTime(dr["ModifiedDate"], null);
            _CreatedBy = UtilMethods.toInt(dr["CreatedBy"], null);
            _ModifiedBy = UtilMethods.toInt(dr["ModifiedBy"], null);

            
        }
        #endregion

        #region GET ISSUES
        public DataSet GetIssues(int? TypeID, int? Priority, int? State)
        {
            try
            {
                SqlDataObject sql = new SqlDataObject(StaticKeys.ConnStr, "Issue_GetByAccountID");
                sql.AddInputParameter(false, "@AccountID", _ID);
                if(TypeID > 0)
                    sql.AddInputParameter(false, "@TypeID", TypeID);
                if(Priority > 0)
                    sql.AddInputParameter(false, "@Priority", Priority);

                if (State > 0)
                    sql.AddInputParameter(false, "@State", State);

                return sql.ExecuteDataSet();
            }
            catch (Exception ex)
            {
                ErrorLogger.LOGGER.Error(ex.Message, ex);
                return null;
            }

        }



        public DataSet GetIssues(List<SqlDataRecord> list, int? Priority, int? State)
        {
            try
            {
                SqlDataObject sql = new SqlDataObject(StaticKeys.ConnStr, "Issue_GetByAccountIDWithTable");
                sql.AddInputParameter(false, "@AccountID", _ID);
                
                
                //sql.AddInputParameterTable("@TypeIDs", "TypesTable", list);
                if (list.Count > 0)
                    sql.AddInputParameterTable("@TypeIDs", "TypesTable", list);

                if (Priority > 0)
                    sql.AddInputParameter(false, "@Priority", Priority);

                if (State > 0)
                    sql.AddInputParameter(false, "@State", State);

                return sql.ExecuteDataSet();
            }
            catch (Exception ex)
            {
                ErrorLogger.LOGGER.Error(ex.Message, ex);
                return null;
            }

        }

        public DataSet GetArchivedIssues(int? TypeID, int? Priority)
        {
            try
            {
                SqlDataObject sql = new SqlDataObject(StaticKeys.ConnStr, "Issue_GetArchivedByAccountID");
                sql.AddInputParameter(false, "@AccountID", _ID);
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


        public DataSet GetArchivedIssues(List<SqlDataRecord> list, int? Priority, int? State, int StartIndex, int NumRecords)
        {
            try
            {
                SqlDataObject sql = new SqlDataObject(StaticKeys.ConnStr, "Issue_GetArchivedByAccountIDPaginatedWithTable");
                sql.AddInputParameter(false, "@AccountID", _ID);

                if (list.Count > 0)
                    sql.AddInputParameterTable("@TypeIDs", "TypesTable", list);
                if (Priority > 0)
                    sql.AddInputParameter(false, "@Priority", Priority);

                if (State > 0)
                    sql.AddInputParameter(false, "@State", State);

                sql.AddInputParameter(false, "@StartIndex", StartIndex);
                sql.AddInputParameter(false, "@NumRecords", NumRecords);


                return sql.ExecuteDataSet();
            }
            catch (Exception ex)
            {
                ErrorLogger.LOGGER.Error(ex.Message, ex);
                return null;
            }

        }


        public DataSet GetArchivedIssues(List<SqlDataRecord> list, int? Priority, int? State)
        {
            try
            {
                SqlDataObject sql = new SqlDataObject(StaticKeys.ConnStr, "Issue_GetArchivedByAccountIDWithTable");
                sql.AddInputParameter(false, "@AccountID", _ID);

                if (list.Count > 0)
                {
                    sql.AddInputParameterTable("@TypeIDs", "TypesTable", list);
                   
                }
                if (Priority > 0)
                    sql.AddInputParameter(false, "@Priority", Priority);

                if (State > 0)
                    sql.AddInputParameter(false, "@State", State);

                return sql.ExecuteDataSet();
            }
            catch (Exception ex)
            {
                ErrorLogger.LOGGER.Error(ex.Message, ex);
                return null;
            }

        }
        #endregion


        #region PAGINATED 

        public DataSet GetIssues(int? TypeID, int? Priority, int? State, int StartIndex, int NumRecords)
        {
            try
            {
                SqlDataObject sql = new SqlDataObject(StaticKeys.ConnStr, "Issue_GetByAccountIDPaginated");
                sql.AddInputParameter(false, "@AccountID", _ID);
                if (TypeID > 0)
                    sql.AddInputParameter(false, "@TypeID", TypeID);
                if (Priority > 0)
                    sql.AddInputParameter(false, "@Priority", Priority);

                if (State > 0)
                    sql.AddInputParameter(false, "@State", State);

                sql.AddInputParameter(false, "@StartIndex", StartIndex);
                sql.AddInputParameter(false, "@NumRecords", NumRecords);

                return sql.ExecuteDataSet();
            }
            catch (Exception ex)
            {
                ErrorLogger.LOGGER.Error(ex.Message, ex);
                return null;
            }

        }


        //news
        public DataSet GetIssues(List<SqlDataRecord>  list, int? Priority, int? State, int StartIndex, int NumRecords)
        {
            try
            {
                SqlDataObject sql = new SqlDataObject(StaticKeys.ConnStr, "Issue_GetByAccountIDPaginatedWithTable");
                sql.AddInputParameter(false, "@AccountID", _ID);

                if (list.Count > 0)
                {
                    sql.AddInputParameterTable("@TypeIDs", "TypesTable", list);
                    //foreach (SqlDataRecord dr in list)
                    //for (int i = 0; i < list.Count; i++)
                    //{
                    //    ErrorLogger.LOGGER.Error(list[i], new Exception("eeee"));
                    //}
                }
                if (Priority > 0)
                    sql.AddInputParameter(false, "@Priority", Priority);

                if (State > 0)
                    sql.AddInputParameter(false, "@State", State);

                sql.AddInputParameter(false, "@StartIndex", StartIndex);
                sql.AddInputParameter(false, "@NumRecords", NumRecords);

                return sql.ExecuteDataSet();
            }
            catch (Exception ex)
            {
                ErrorLogger.LOGGER.Error(ex.Message, ex);
                return null;
            }

        }


        #endregion
    }
}
