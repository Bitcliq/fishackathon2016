using Bitcliq.BIR.Logs;
using Bitcliq.BIR.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitcliq.BIR.Platform
{
    public  class Profile
    { 
        #region PRIVATE

        private int _ID;
        private string _Name;
        private bool _isAdmin;
     

        #endregion

        #region PUBLIC

        public int ID { get { return _ID; } set { _ID = value; } }
        public string Name { get { return _Name; } set { _Name = value; } }
        public bool isAdmin { get { return _isAdmin; } set { _isAdmin = value; } }

        #endregion

         #region CONSTRUCTORS
        public Profile()
        {
            CleanValues();
        }

        public Profile(int p_ID)
        {
            Get(p_ID);
        }

        public Profile(DataRow dr)
        {
            FillValues(dr);
        }
        #endregion


        #region CLEAN VALUES
        private void CleanValues()
        {
            ID = 0;
            _Name = "";
            _isAdmin = false;

        }
        #endregion

        #region GET
        private void Get(int p_ID)
        {
            try
            {
                if (p_ID > 0)
                {
                    SqlDataObject sql = new SqlDataObject(StaticKeys.ConnStr, "Type_ListByID");
                    sql.AddInputParameter(false, "@ID", ID);

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

        public static List<Profile> List()
        {
            try
            {
                SqlDataObject sql = new SqlDataObject(StaticKeys.ConnStr, "Profile_List");

                
                DataSet ds = sql.ExecuteDataSet();


                List<Profile> pl = new List<Profile>();

                if (UtilMethods.DataSetHasData(ds))
                {
                    DataRow[] drArr = ds.Tables[0].Select();

                    foreach (DataRow dr in drArr)
                        pl.Add(new Profile(dr));
                }
                return pl;

            }
            catch (Exception ex)
            {
                ErrorLogger.LOGGER.Error(ex.Message, ex);
                return new List<Profile>();
            }
        }



        #region FILL VALUES
        private void FillValues(DataRow dr)
        {
            ID = (int)UtilMethods.toInt(dr["ID"], 0);
            _Name = dr["Name"] + "";

            _isAdmin = (bool)UtilMethods.toBool(dr["ISAdmin"], false);
            
        }
        #endregion
    }
}
