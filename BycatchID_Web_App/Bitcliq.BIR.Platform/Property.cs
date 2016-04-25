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
    public class Property
    {
        #region PRIVATE
        private int _ID;
        private string _Name;
        private int? _CreatedBy_User;
        private int? _CreatedBy_Reporter;
        private DateTime? _CreatedDate;
        private int? _ModifiedBy;
        private DateTime? _ModifiedDate;
        private bool _Active;
        private bool _Deleted;
        private int _AccountID;
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
        public int? CreatedBy_User
        {
            get { return _CreatedBy_User; }
            set { _CreatedBy_User = value; }
        }
        public int? CreatedBy_Reporter
        {
            get { return _CreatedBy_Reporter; }
            set { _CreatedBy_Reporter = value; }
        }
        public DateTime? CreatedDate
        {
            get { return _CreatedDate; }
            set { _CreatedDate = value; }
        }
        public int? ModifiedBy
        {
            get { return _ModifiedBy; }
            set { _ModifiedBy = value; }
        }
        public DateTime? ModifiedDate
        {
            get { return _ModifiedDate; }
            set { _ModifiedDate = value; }
        }
        public bool Active
        {
            get { return _Active; }
            set { _Active = value; }
        }
        public bool Deleted
        {
            get { return _Deleted; }
            set { _Deleted = value; }
        }
        public int AccountID
        {
            get { return _AccountID; }
            set { _AccountID = value; }
        }
        #endregion

        #region CONSTRUCTORS
        public Property()
        {
            CleanValues();
        }

        public Property(int p_ID)
        {
            Get(p_ID);
        }

        public Property(DataRow ds)
        {
            FillValues(ds);
        }
        #endregion

        #region GET
        private void Get(int p_ID)
        {
            try
            {
                if (p_ID > 0)
                {
                    SqlDataObject sql = new SqlDataObject(StaticKeys.ConnStr, "Property_ListByID");
                    sql.AddInputParameter(false, "@ID", p_ID);

                    DataSet ds = sql.ExecuteDataSet();

                    if (UtilMethods.DataSetHasOnlyOneDataRow(ds))
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
                SqlDataObject sql = new SqlDataObject(StaticKeys.ConnStr, "Property_Insert");
                

                sql.AddInputParameter(false, "@Name", _Name);
                sql.AddInputParameter(false, "@CreatedBy_User", _CreatedBy_User);
                sql.AddInputParameter(false, "@CreatedBy_Reporter", _CreatedBy_Reporter);
                sql.AddInputParameter(false, "@AccountID", _AccountID);

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
                SqlDataObject sql = new SqlDataObject(StaticKeys.ConnStr, "Property_Update");
                sql.AddInputParameter(false, "@ID", _ID);
                sql.AddInputParameter(false, "@Name", _Name);
               

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
                SqlDataObject sql = new SqlDataObject(StaticKeys.ConnStr, "Property_Delete");
                sql.AddInputParameter(false, "@ID", _ID);
             

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

        #region CLEAN VALUES
        private void CleanValues()
        {
            _ID = 0;
            _Name = "";
            _CreatedBy_User = null;
            _CreatedBy_Reporter = null;
            _CreatedDate = null;
            _ModifiedBy = null;
            _ModifiedDate = null;
            _Active = false;
            _Deleted = false;
            _AccountID = 0;
        }
        #endregion

        #region FILL VALUES
        private void FillValues(DataRow dr)
        {
            _ID = (int)UtilMethods.toInt(dr["ID"], 0);
            _Name = dr["Name"] + "";
            _CreatedBy_User = UtilMethods.toInt(dr["CreatedBy_User"], null);
            _CreatedBy_Reporter = UtilMethods.toInt(dr["CreatedBy_Reporter"], null);
            _CreatedDate = UtilMethods.toDateTime(dr["CreatedDate"], null);
            _ModifiedBy = UtilMethods.toInt(dr["ModifiedBy"], null);
            _ModifiedDate = UtilMethods.toDateTime(dr["ModifiedDate"], null);
            _Active = (bool)UtilMethods.toBool(dr["Active"], false);
            _Deleted = (bool)UtilMethods.toBool(dr["Deleted"], false);
            _AccountID = (int)UtilMethods.toInt(dr["AccountID"], 0);
        }
        #endregion




        public static int CountByName(string Name, int? ID, int AccountID)
        {
            try
            {
                SqlDataObject sql = new SqlDataObject(StaticKeys.ConnStr, "Property_CountByName");
                sql.AddInputParameter(false, "@ID", ID);
                sql.AddInputParameter(false, "@Name", Name);
                sql.AddInputParameter(false, "@AccountID", AccountID);


                int ret = (int)UtilMethods.toInt(sql.ExecuteScalar(), 0);

                return ret;
            }
            catch (Exception ex)
            {
                ErrorLogger.LOGGER.Error(ex.Message, ex);
                return -1;
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

                SqlDataObject sql = new SqlDataObject(StaticKeys.ConnStr, "Property_ListToDataSet");


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



        #region LIST
        public static List<Property> List(int AccountID)
        {
            try
            {
                SqlDataObject sql = new SqlDataObject(StaticKeys.ConnStr, "Property_List");

                sql.AddInputParameter(false, "@AccountID", AccountID);
               
                
                DataSet ds = sql.ExecuteDataSet();


                List<Property> pl = new List<Property>();

                if (UtilMethods.DataSetHasData(ds))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                        pl.Add(new Property(dr));
                }
                return pl;


            }
            catch (Exception ex)
            {

                ErrorLogger.LOGGER.Error(ex.Message, ex);
                return new List<Property>();
            }
        }



        #endregion

    }
}
