using Bitcliq.BIR.Logs;
using Bitcliq.BIR.Utils;
using Microsoft.SqlServer.Server;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitcliq.BIR.Platform
{
    public class Type 
    {
        #region PRIVATE

        private int _ID;
        private string _Name;
        private int _CreatedBy;
        private DateTime _CreatedDate;
        private int? _ModifiedBy;
        private DateTime? _ModifiedDate;
        private int _AccountID;
        private int? _ParentID;
        private string _ParentName = "";

        #endregion

        #region PUBLIC

        public int ID { get { return _ID; } set { _ID = value; } }
        public string Name { get { return _Name; } set { _Name = value; } }
        public int CreatedBy { get { return _CreatedBy; } set { _CreatedBy = value; } }
        public DateTime CreatedDate { get { return _CreatedDate; } set { _CreatedDate = value; } }
        public int? ModifiedBy { get { return _ModifiedBy; } set { _ModifiedBy = value; } }
        public DateTime? ModifiedDate { get { return _ModifiedDate; } set { _ModifiedDate = value; } }
        public int AccountID { get { return _AccountID; } set { _AccountID = value; } }
        public int? ParentID { get { return _ParentID; } set { _ParentID = value; } }



        public string ParentName { get { return _ParentName; } set { _ParentName = value; } }

        #endregion

        #region CONSTRUCTORS
        public Type()
        {
            CleanValues();
        }


        public Type(int p_ID)
            
        {
            Get(p_ID);
        }

        public Type(DataRow dr)
        {
            FillValues(dr);
        }



    
        #endregion


        #region ################## PRIVATE METHODS

       

        #region GET
        private void Get(int p_ID)
        {
            try
            {
                if (p_ID > 0)
                {
                    SqlDataObject sql = new SqlDataObject(StaticKeys.ConnStr, "Type_ListByID");
                    sql.AddInputParameter(false, "@ID", p_ID);

                    DataSet ds = sql.ExecuteDataSet();

                    if (UtilMethods.DataSetHasOnlyOneDataRow(ds))
                    {
                        DataRow dr = ds.Tables[0].Rows[0];

                        FillValues(dr);
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
        private bool Insert(int? p_UserID)
        {
            try
            {
                SqlDataObject sql = new SqlDataObject(StaticKeys.ConnStr, "Type_Insert");

                sql.AddInputParameter(false, "@Name", _Name);
                sql.AddInputParameter(false, "@CreatedBy", p_UserID);
                sql.AddInputParameter(false, "@AccountID", _AccountID);
                sql.AddInputParameter(false, "@ParentID", _ParentID);

                int ret = (int)UtilMethods.toInt(sql.ExecuteScalar(), 0);

                if (ret <= 0)
                {
                    return false;
                }

                ID = ret;



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
        private bool Update(int? p_UserID)
        {
            try
            {
                SqlDataObject sql = new SqlDataObject(StaticKeys.ConnStr, "Type_Update");

                sql.AddInputParameter(false, "@ID", ID);
                sql.AddInputParameter(false, "@Name", _Name);
                sql.AddInputParameter(false, "@ModifiedBy", p_UserID);
                sql.AddInputParameter(false, "@ParentID", _ParentID);


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


        #endregion

        #region ################## PUBLIC METHODS


        #region SAVE
        public bool Save(int p_User)
        {
            if (ID > 0)
                return Update(p_User);
            else
                return Insert(p_User);



        }
        #endregion

        #region DELETE
        public bool Delete()
        {
            try
            {
                SqlDataObject sql = new SqlDataObject(StaticKeys.ConnStr, "Type_Delete");

                sql.AddInputParameter(false, "@ID", ID);

                sql.ExecuteNoQuery();

                return true;
            }
            catch (Exception ex)
            {
                ErrorLogger.LOGGER.Error(ex.Message, ex);
                return false;
            }
        }
        #endregion

     
        #endregion

        #region ################## PUBLIC STATIC METHODS

        #region LIST
        public static List<Type> List(string p_Name, int p_AccountID)
        {
            try
            {
                SqlDataObject sql = new SqlDataObject(StaticKeys.ConnStr, "Type_List");

                if (p_Name + "" != "")
                    sql.AddInputParameter(false, "@Name", p_Name);

                sql.AddInputParameter(false, "@AccountID", p_AccountID);

                DataSet ds = sql.ExecuteDataSet();


                List<Type> pl = new List<Type>();

                if (UtilMethods.DataSetHasData(ds))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                        pl.Add(new Type(dr));
                }
                return pl;

            }
            catch (Exception ex)
            {
                ErrorLogger.LOGGER.Error(ex.Message, ex);
                return new List<Type>();
            }
        }


        public static List<Type> ListByUserID(List<SqlDataRecord> list, string p_Name, int p_AccountID, int p_UserID, bool p_ISAdmin)
        {
            try
            {
                SqlDataObject sql = new SqlDataObject(StaticKeys.ConnStr, "Type_ListByUserID");

                if(list.Count >0 )
                    sql.AddInputParameterTable("@TypeIDs", "TypesTable", list);
                //else
                //    sql.AddInputParameterTable("@TypeIDs", "TypesTable", DBNull.Value);

                if (p_Name + "" != "")
                    sql.AddInputParameter(false, "@Name", p_Name);

                sql.AddInputParameter(false, "@AccountID", p_AccountID);
                sql.AddInputParameter(false, "@UserID", p_UserID);
                sql.AddInputParameter(false, "@ISAdmin", p_ISAdmin);

                DataSet ds = sql.ExecuteDataSet();


                List<Type> pl = new List<Type>();

                if (UtilMethods.DataSetHasData(ds))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                        pl.Add(new Type(dr));
                }
                return pl;

            }
            catch (Exception ex)
            {
                ErrorLogger.LOGGER.Error(ex.Message, ex);
                return new List<Type>();
            }
        }

        public static List<Type> ListByUserIDWithoutParent(List<SqlDataRecord> list, string p_Name, int p_AccountID, int p_UserID, bool p_ISAdmin)
        {
            try
            {
                SqlDataObject sql = new SqlDataObject(StaticKeys.ConnStr, "Type_ListByUserIDWithoutParent");

                if (list.Count > 0)
                    sql.AddInputParameterTable("@TypeIDs", "TypesTable", list);
                //else
                //    sql.AddInputParameterTable("@TypeIDs", "TypesTable", DBNull.Value);

                if (p_Name + "" != "")
                    sql.AddInputParameter(false, "@Name", p_Name);

                sql.AddInputParameter(false, "@AccountID", p_AccountID);
                sql.AddInputParameter(false, "@UserID", p_UserID);
                sql.AddInputParameter(false, "@ISAdmin", p_ISAdmin);

                DataSet ds = sql.ExecuteDataSet();


                List<Type> pl = new List<Type>();

                if (UtilMethods.DataSetHasData(ds))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                        pl.Add(new Type(dr));
                }
                return pl;

            }
            catch (Exception ex)
            {
                ErrorLogger.LOGGER.Error(ex.Message, ex);
                return new List<Type>();
            }
        }

        public static List<User> ListUsersByTypes(List<SqlDataRecord> list)
        {
            try
            {
                SqlDataObject sql = new SqlDataObject(StaticKeys.ConnStr, "User_ListUsersByTypes");

                if (list.Count > 0)
                    sql.AddInputParameterTable("@TypeIDs", "TypesTable", list);

                DataSet ds = sql.ExecuteDataSet();


                List<User> pl = new List<User>();

                if (UtilMethods.DataSetHasData(ds))
                {

                    foreach (DataRow dr in ds.Tables[0].Rows)
                        pl.Add(new User(dr));
                }
                return pl;

            }
            catch (Exception ex)
            {
                ErrorLogger.LOGGER.Error(ex.Message, ex);
                return new List<User>();
            }
        }


        public static List<User> ListUsersByTypeID(int TypeID)
        {
            try
            {
                SqlDataObject sql = new SqlDataObject(StaticKeys.ConnStr, "User_ListUsersByTypeID");

                sql.AddInputParameter(false, "@TypeID", TypeID);

                DataSet ds = sql.ExecuteDataSet();


                List<User> pl = new List<User>();

                if (UtilMethods.DataSetHasData(ds))
                {

                    foreach (DataRow dr in ds.Tables[0].Rows)
                        pl.Add(new User(dr));
                }
                return pl;

            }
            catch (Exception ex)
            {
                ErrorLogger.LOGGER.Error(ex.Message, ex);
                return new List<User>();
            }
        }

        public static List<User> ListDecisorsByTypeID(int TypeID)
        {
            try
            {
                SqlDataObject sql = new SqlDataObject(StaticKeys.ConnStr, "User_ListDecisorsByTypeID");

                sql.AddInputParameter(false, "@TypeID", TypeID);

                DataSet ds = sql.ExecuteDataSet();


                List<User> pl = new List<User>();

                if (UtilMethods.DataSetHasData(ds))
                {

                    foreach (DataRow dr in ds.Tables[0].Rows)
                        pl.Add(new User(dr));
                }
                return pl;

            }
            catch (Exception ex)
            {
                ErrorLogger.LOGGER.Error(ex.Message, ex);
                return new List<User>();
            }
        }
        public static List<Type> ListWithoutParent(string p_Name, int p_AccountID)
        {
            try
            {
                SqlDataObject sql = new SqlDataObject(StaticKeys.ConnStr, "Type_ListWithoutParent");

                if (p_Name + "" != "")
                    sql.AddInputParameter(false, "@Name", p_Name);

                sql.AddInputParameter(false, "@AccountID", p_AccountID);

                DataSet ds = sql.ExecuteDataSet();


                List<Type> pl = new List<Type>();

                if (UtilMethods.DataSetHasData(ds))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                        pl.Add(new Type(dr));
                }
                return pl;

            }
            catch (Exception ex)
            {
                ErrorLogger.LOGGER.Error(ex.Message, ex);
                return new List<Type>();
            }
        }

        public static List<Type> ListWithParent(string p_Name, int p_AccountID)
        {
            try
            {
                SqlDataObject sql = new SqlDataObject(StaticKeys.ConnStr, "Type_ListWithParent");

                if (p_Name + "" != "")
                    sql.AddInputParameter(false, "@Name", p_Name);

                sql.AddInputParameter(false, "@AccountID", p_AccountID);

                DataSet ds = sql.ExecuteDataSet();


                List<Type> pl = new List<Type>();

                if (UtilMethods.DataSetHasData(ds))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                        pl.Add(new Type(dr));
                }
                return pl;

            }
            catch (Exception ex)
            {
                ErrorLogger.LOGGER.Error(ex.Message, ex);
                return new List<Type>();
            }
        }
        public static List<Type> ListByParentID(string p_Name, int p_AccountID, int p_ParentID)
        {
            try
            {
                SqlDataObject sql = new SqlDataObject(StaticKeys.ConnStr, "Type_ListByParentID");

                if (p_Name + "" != "")
                    sql.AddInputParameter(false, "@Name", p_Name);

                sql.AddInputParameter(false, "@AccountID", p_AccountID);
                sql.AddInputParameter(false, "@ParentID", p_ParentID);

                DataSet ds = sql.ExecuteDataSet();


                List<Type> pl = new List<Type>();

                if (UtilMethods.DataSetHasData(ds))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                        pl.Add(new Type(dr));
                }
                return pl;

            }
            catch (Exception ex)
            {
                ErrorLogger.LOGGER.Error(ex.Message, ex);
                return new List<Type>();
            }
        }


        // gets only from that parent
        public static List<Type> ListByParentID2(string p_Name, int p_AccountID, int p_ParentID)
        {
            try
            {
                SqlDataObject sql = new SqlDataObject(StaticKeys.ConnStr, "Type_ListByParentID2");

                if (p_Name + "" != "")
                    sql.AddInputParameter(false, "@Name", p_Name);

                sql.AddInputParameter(false, "@AccountID", p_AccountID);
                sql.AddInputParameter(false, "@ParentID", p_ParentID);

                DataSet ds = sql.ExecuteDataSet();


                List<Type> pl = new List<Type>();

                if (UtilMethods.DataSetHasData(ds))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                        pl.Add(new Type(dr));
                }
                return pl;

            }
            catch (Exception ex)
            {
                ErrorLogger.LOGGER.Error(ex.Message, ex);
                return new List<Type>();
            }
        }
        #endregion

        #endregion



        #region CLEAN VALUES
        private void CleanValues()
        {
            ID = 0;
            _Name = "";
            _CreatedBy = 0;
            _CreatedDate = DateTime.MinValue;
            _ModifiedBy = null;
            _ModifiedDate = null;
            _ParentID = null;

        }
        #endregion


        #region FILL VALUES
        private void FillValues(DataRow dr)
        {
            ID = (int)UtilMethods.toInt(dr["ID"], 0);
            _Name = dr["Name"] + "";

            _CreatedBy = (int)UtilMethods.toInt(dr["CreatedBy"], 0);
            _CreatedDate = (DateTime)UtilMethods.toDateTime(dr["CreatedDate"], DateTime.MinValue);
            _ModifiedBy = UtilMethods.toInt(dr["ModifiedBy"], null);
            _ModifiedDate = UtilMethods.toDateTime(dr["ModifiedDate"], null);

            _ParentID = UtilMethods.toInt(dr["ParentID"], null);
            _AccountID = (int)UtilMethods.toInt(dr["AccountID"], 0);

            try
            {
                _ParentName = dr["ParentName"] + "";
            }
            catch (Exception)
            {
                // this exception is for procedures thar don't have ParentName yet @@TODO
            }
        }
        #endregion


   

        public int CountInType()
        {
            try
            {
                SqlDataObject sql = new SqlDataObject(StaticKeys.ConnStr, "Type_CheckCanDelete");

                sql.AddInputParameter(false, "@ID", ID);

                return (int)UtilMethods.toInt(sql.ExecuteScalar(), 0);

            }
            catch (Exception ex)
            {
                ErrorLogger.LOGGER.Error(ex.Message, ex);
                return -1;
            }
        }


        public static int CountByName(int? p_ID, string p_Name, int? p_ParentID)
        {
            try
            {
                SqlDataObject sql = new SqlDataObject(StaticKeys.ConnStr, "Type_CountByName");

                sql.AddInputParameter(false, "@ID", p_ID);
                sql.AddInputParameter(false, "@Name", p_Name);
                sql.AddInputParameter(false, "@ParentID", p_ParentID);

                return (int)UtilMethods.toInt(sql.ExecuteScalar(), 0);

            }
            catch (Exception ex)
            {
                ErrorLogger.LOGGER.Error(ex.Message, ex);
                return -1;
            }
        }


        #region DATASET - FOR JSON
        public DataSet ListToDataSetWithoutParent(int userID, string p_Filters, int p_StartIndex, int p_NumRecords, string p_Order)
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

                SqlDataObject sql = new SqlDataObject(StaticKeys.ConnStr, "Type_ListToDatasetWithoutParent");


                sql.AddInputParameter(false, "@UserID", userID);
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

        public DataSet ListToDataSetWithParent(int userID, string p_Filters, int p_StartIndex, int p_NumRecords, string p_Order)
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

                SqlDataObject sql = new SqlDataObject(StaticKeys.ConnStr, "Type_ListToDatasetWithParent");


                sql.AddInputParameter(false, "@UserID", userID);
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


        public DataSet ListToDataSet(int userID, string p_Filters, int p_StartIndex, int p_NumRecords, string p_Order)
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

                SqlDataObject sql = new SqlDataObject(StaticKeys.ConnStr, "Type_ListToDataset");


                sql.AddInputParameter(false, "@UserID", userID);
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
    }
}
