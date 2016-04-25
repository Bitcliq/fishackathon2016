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
    public class State
    {
        #region PRIVATE
        private int _ID;
        private string _Name;
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
        #endregion

        #region CONSTRUCTORS
        public State()
        {
            CleanValues();
        }

        public State(int p_ID)
        {
            Get(p_ID);
        }

        public State(DataRow dr)
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
                    SqlDataObject sql = new SqlDataObject(StaticKeys.ConnStr, "State_ListByID");
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



        #region ################## PUBLIC STATIC METHODS

        public static List<State> List()
        {
            try
            {
                SqlDataObject sql = new SqlDataObject(StaticKeys.ConnStr, "State_List");

                DataSet ds = sql.ExecuteDataSet();

                List<State> pl = new List<State>();

                if (UtilMethods.DataSetHasData(ds))
                {
                    DataRow[] drArr = ds.Tables[0].Select();

                    foreach (DataRow dr in drArr)
                        pl.Add(new State(dr));
                }
                return pl;

            }
            catch (Exception ex)
            {
                ErrorLogger.LOGGER.Error(ex.Message, ex);
                return new List<State>();
            }
        }


        #endregion



        #region CLEAN VALUES
        private void CleanValues()
        {
            _ID = 0;
            _Name = "";
        }
        #endregion

        #region FILL VALUES
        private void FillValues(DataRow dr)
        {
            _ID = (int)UtilMethods.toInt(dr["ID"], 0);
            _Name = dr["Name"] + "";
        }
        #endregion
     

    }
}
