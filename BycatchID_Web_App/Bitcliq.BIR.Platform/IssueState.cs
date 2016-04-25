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
    public class IssueState
    {
        #region PRIVATE
        private int _ID;
        private int _IssueID;
        private DateTime _StateDate;
        private int? _UserID;
        private int _StateID;
        private string _StateName;
        private string _Message;
        private object _GPS;
        private byte[] _Blob;
        private string _BlobName;
        private string _BlobType;
        private int? _BlobLen;
        private string _Latitude;
        private string _Longitude;
        private string _InternalNotes;
        private string _By;

        private int? _ReporterIDClosed;
        

        #endregion

        #region PUBLIC
        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        public int IssueID
        {
            get { return _IssueID; }
            set { _IssueID = value; }
        }

        public DateTime StateDate
        {
            get { return _StateDate; }
            set { _StateDate = value; }
        }

        public int? UserID
        {
            get { return _UserID; }
            set { _UserID = value; }
        }


        public int? ReporterIDClosed
        {
            get { return _ReporterIDClosed; }
            set { _ReporterIDClosed = value; }
        }

        public int StateID
        {
            get { return _StateID; }
            set { _StateID = value; }
        }
        public string StateName
        {
            get { return _StateName; }
            set { _StateName = value; }
        }

        public string Message
        {
            get { return _Message; }
            set { _Message = value; }
        }
        public object GPS
        {
            get { return _GPS; }
            set { _GPS = value; }
        }
        public byte[] Blob
        {
            get { return _Blob; }
            set { _Blob = value; }
        }
        public string BlobName
        {
            get { return _BlobName; }
            set { _BlobName = value; }
        }
        public string BlobType
        {
            get { return _BlobType; }
            set { _BlobType = value; }
        }
        public int? BlobLen
        {
            get { return _BlobLen; }
            set { _BlobLen = value; }
        }



        public string Latitude { get { return _Latitude; } set { _Latitude = value; } }
        public string Longitude { get { return _Longitude; } set { _Longitude = value; } }

        public string InternalNotes { get { return _InternalNotes; } set { _InternalNotes = value; } }
        public string By { get { return _By; } set { _By = value; } }



        public IssueState(DataRow dr)
        {
            FillValues(dr);
        }

        public IssueState()
        {
        }

        public bool Fix()
        {
            try
            {
                SqlDataObject sql = new SqlDataObject(StaticKeys.ConnStr, "Issue_Fix");
                sql.AddInputParameter(false, "@IssueID", _IssueID);
                sql.AddInputParameter(false, "@UserID", _UserID);
                sql.AddInputParameter(false, "@StateID", 4);
                sql.AddInputParameter(false, "@InternalNotes", _InternalNotes);
                sql.AddInputParameter(false, "@GPS", _GPS);
                sql.AddInputParameter(false, "@Blob", _Blob);
                sql.AddInputParameter(false, "@BlobName", _BlobName);
                sql.AddInputParameter(false, "@BlobType", _BlobType);
                sql.AddInputParameter(false, "@BlobLen", _BlobLen);
                sql.AddInputParameter(false, "@ReporterIDClosed", _ReporterIDClosed);
                

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

        public static List<IssueState> GetIssueHistory(int IssueID)
        {
            try
            {
                SqlDataObject sql = new SqlDataObject(StaticKeys.ConnStr, "Issue_GetHistory");

                sql.AddInputParameter(false, "@ID", IssueID);

                DataSet ds = sql.ExecuteDataSet();

                List<IssueState> list = new List<IssueState>();
                if (UtilMethods.DataSetHasData(ds))
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        list.Add(new IssueState(dr));

                    }
                }

                return list;
            }
            catch (Exception ex)
            {
                ErrorLogger.LOGGER.Error(ex.Message, ex);
                return new List<IssueState>();
            }
        }



        public static IssueState GetFix(int IssueID)
        {
            try
            {
                SqlDataObject sql = new SqlDataObject(StaticKeys.ConnStr, "IssueStates_GetIssueFix");

                sql.AddInputParameter(false, "@IssueID", IssueID);

                DataSet ds = sql.ExecuteDataSet();

                List<IssueState> list = new List<IssueState>();
                if (UtilMethods.DataSetHasOnlyOneDataRow(ds))
                {
                    DataRow dr = ds.Tables[0].Rows[0];

                    return new IssueState(dr);


                }

                return null;
            }
            catch (Exception ex)
            {
                ErrorLogger.LOGGER.Error(ex.Message, ex);
                return null;
            }
        }


        #region FILL VALUES
        private void FillValues(DataRow dr)
        {


            _ID = (int)UtilMethods.toInt(dr["ID"], 0);
            _IssueID = (int)UtilMethods.toInt(dr["IssueID"], 0);
            _StateID = (int)UtilMethods.toInt(dr["StateID"], 0);
            _Message = dr["Message"] + "";
            _InternalNotes = dr["InternalNotes"] + "";

            _GPS = dr["GPS"] + "";
            _Blob = UtilMethods.toByteArray(dr["Blob"], null);
            _BlobName = dr["BlobName"] + "" == "" ? null : dr["BlobName"] + "";
            _BlobType = dr["BlobType"] + "" == "" ? null : dr["BlobType"] + "";
            _BlobLen = UtilMethods.toInt(dr["BlobLen"], null);
            _StateDate = (DateTime)UtilMethods.toDateTime(dr["StateDate"], DateTime.MaxValue);
            _UserID = UtilMethods.toInt(dr["UserID"], null);
            _IssueID = (int)UtilMethods.toInt(dr["IssueID"], 0);

            _By = dr["By"] + "";
            _ReporterIDClosed = UtilMethods.toInt(dr["ReporterIDClosed"], null);
            
            _StateName = dr["StateName"] + "" == "" ? null : dr["StateName"] + "";

            try
            {
                _Latitude = dr["Latitude"] + "";
                _Longitude = dr["Longitude"] + "";
            }
            catch (Exception)
            {
            }
        }
        #endregion



        #endregion

    }
}
