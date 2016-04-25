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
    public class Issue
    {
        #region PRIVATE
        private int _ID;
        private string _Subject;
        private string _Message;
        private object _GPS;
        private byte[] _Blob;
        private string _BlobName;
        private string _BlobType;
        private int? _BlobLen;
        private int? _Priority;
        private DateTime? _DateReported;
        private int? _ReportedBy;
        private int? _TypeID;
        private int _AccountID;
        private bool _Deleted;
        private DateTime? _DateTaken;

        private string _Latitude;
        private string _Longitude;

        private int _SortIndex;
        private int _State;
        private string _StateName;
        private string _TypeName;

       

        private int? _ImageRotation;
        private int? _AssignedTo;

     
        private string _Device;
        private int? _ParentID;
        private string _ParentName = "";

        private int? _ReportedBy_UserID;

        private int? _PropertyID;
        private string _PropertyName;

        #endregion

        #region PUBLIC
        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        public string Subject
        {
            get { return _Subject; }
            set { _Subject = value; }
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
        public int? Priority
        {
            get { return _Priority; }
            set { _Priority = value; }
        }
        public DateTime? DateReported
        {
            get { return _DateReported; }
            set { _DateReported = value; }
        }
        public int? ReportedBy
        {
            get { return _ReportedBy; }
            set { _ReportedBy = value; }
        }
        public int? TypeID
        {
            get { return _TypeID; }
            set { _TypeID = value; }
        }
        public int AccountID
        {
            get { return _AccountID; }
            set { _AccountID = value; }
        }
        public bool Deleted
        {
            get { return _Deleted; }
            set { _Deleted = value; }
        }

        public DateTime? DateTaken
        {
            get { return _DateTaken; }
            set { _DateTaken = value; }
        }

        public string StateName
        {
            get { return _StateName; }
            set { _StateName = value; }
        }

        public string Latitude { get { return _Latitude; } set { _Latitude = value; } }
        public string Longitude { get { return _Longitude; } set { _Longitude = value; } }

        public string TypeName
        {
            get { return _TypeName; }
            set { _TypeName = value; }
        }

        public int SortIndex
        {
            get { return _SortIndex; }
            set { _SortIndex = value; }
        }

        public int State
        {
            get { return _State; }
            set { _State = value; }
        }

        public int? ImageRotation
        {
            get { return _ImageRotation; }
            set { _ImageRotation = value; }
        }

        public int? AssignedTo
        {
            get { return _AssignedTo; }
            set { _AssignedTo = value; }
        }


       
        public int? ReportedBy_UserID
        {
            get { return _ReportedBy_UserID; }
            set { _ReportedBy_UserID = value; }
        }

        public string Device
        {
            get { return _Device; }
            set { _Device = value; }
        }

        public int? ParentID { get { return _ParentID; } set { _ParentID = value; } }

        public string ParentName { get { return _ParentName; } set { _ParentName = value; } }


        public int? PropertyID
        {
            get { return _PropertyID; }
            set { _PropertyID = value; }
        }


        public string PropertyName
        {
            get { return _PropertyName; }
            set { _PropertyName = value; }
        }
        #endregion

        #region CONSTRUCTORS
        public Issue()
        {
            CleanValues();
        }

        public Issue(int p_ID)
        {
            Get(p_ID);
        }

        public Issue(DataRow dr)
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
                    SqlDataObject sql = new SqlDataObject(StaticKeys.ConnStr, "Issue_ListByID");
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
                SqlDataObject sql = new SqlDataObject(StaticKeys.ConnStr, "Issue_Insert");
                sql.AddInputParameter(false, "@Subject", _Subject);
                sql.AddInputParameter(false, "@Message", _Message);
                sql.AddInputParameter(false, "@GPS", _GPS);
                sql.AddInputParameter(true, "@Blob", _Blob);
                sql.AddInputParameter(false, "@BlobName", _BlobName);
                sql.AddInputParameter(false, "@BlobType", _BlobType);
                sql.AddInputParameter(false, "@BlobLen", _BlobLen);
                sql.AddInputParameter(false, "@Priority", _Priority);
                sql.AddInputParameter(false, "@ReportedBy", _ReportedBy);
                sql.AddInputParameter(false, "@TypeID", _TypeID);
                sql.AddInputParameter(false, "@AccountID", _AccountID);
                sql.AddInputParameter(false, "@DateTaken", _DateTaken);
                sql.AddInputParameter(false, "@ImageRotation", _ImageRotation);
                sql.AddInputParameter(false, "@Device", _Device);
                sql.AddInputParameter(false, "@ReportedBy_UserID", _ReportedBy_UserID);

                sql.AddInputParameter(false, "@PropertyID", _PropertyID);


                if (_PropertyName + "" != "")
                    sql.AddInputParameter(false, "@PropertyName", _PropertyName);


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

        //#region UPDATE
        //protected bool Update()
        //{
        //    try
        //    {
        //        SqlDataObject sql = new SqlDataObject(StaticKeys.ConnStr, "Issue_Update");
        //        sql.AddInputParameter(false, "@ID", _ID);
        //        sql.AddInputParameter(false, "@Subject", _Subject);
        //        sql.AddInputParameter(false, "@Message", _Message);
        //        sql.AddInputParameter(false, "@GPS", _GPS);
        //        sql.AddInputParameter(false, "@Blob", _Blob);
        //        sql.AddInputParameter(false, "@BlobName", _BlobName);
        //        sql.AddInputParameter(false, "@BlobType", _BlobType);
        //        sql.AddInputParameter(false, "@BlobLen", _BlobLen);
        //        sql.AddInputParameter(false, "@Priority", _Priority);
        //        sql.AddInputParameter(false, "@DateReported", _DateReported);
        //        sql.AddInputParameter(false, "@ReportedBy", _ReportedBy);
        //        sql.AddInputParameter(false, "@TypeID", _TypeID);
        //        sql.AddInputParameter(false, "@AccountID", _AccountID);
        //        sql.AddInputParameter(false, "@Deleted", _Deleted);
        //        sql.AddInputParameter(false, "@DateTaken", _DateTaken);
        //        int ret = (int)UtilMethods.toInt(sql.ExecuteScalar(), 0);

        //        if (ret <= 0)
        //        {
        //            return false;
        //        }

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorLogger.LOGGER.Error(ex.Message, ex);
        //        return false;
        //    }
        //}
        //#endregion


        public bool UpdatePriorityAndType(int UserID, string Message, string Notes)
        {
            try
            {
                SqlDataObject sql = new SqlDataObject(StaticKeys.ConnStr, "Issue_UpdatePriorityAndType");
                sql.AddInputParameter(false, "@ID", _ID);

                sql.AddInputParameter(false, "@Priority", _Priority);
                if (_TypeID > 0)
                    sql.AddInputParameter(false, "@TypeID", _TypeID);

                sql.AddInputParameter(false, "@State", _State);
                sql.AddInputParameter(false, "@UserID", UserID);
                sql.AddInputParameter(false, "@Message", Message);
                sql.AddInputParameter(false, "@Notes", Notes);
                sql.AddInputParameter(false, "@AssignedTo", _AssignedTo);
                if(PropertyID > 0)
                    sql.AddInputParameter(false, "@PropertyID", _PropertyID);

              



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

        public bool Reopen(int UserID, string Message, string Notes)
        {
            try
            {
                SqlDataObject sql = new SqlDataObject(StaticKeys.ConnStr, "IssueStates_Insert");




                sql.AddInputParameter(false, "@IssueID", _ID);



                sql.AddInputParameter(false, "@StateID", _State);
                sql.AddInputParameter(false, "@StateDate", DateTime.Now);
                sql.AddInputParameter(false, "@UserID", UserID);
                sql.AddInputParameter(false, "@Message", Message);
                sql.AddInputParameter(false, "@InternalNotes", Notes);
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

        #region SAVE
        public bool Save()
        {
            //if (_ID > 0)
            //{
            //    return Update();
            //}
            //else
            //{
            //    return Insert();
            //}

            return Insert();

        }
        #endregion

        #region DELETE
        public bool Delete()
        {
            try
            {
                SqlDataObject sql = new SqlDataObject(StaticKeys.ConnStr, "Issue_Delete");
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

        #region UPDATE ORDER
        public static bool Reorder(string IssuesList)
        {
            try
            {
                SqlDataObject sql = new SqlDataObject(StaticKeys.ConnStr, "Issue_Reorder");
                sql.AddInputParameter(false, "@issuesList", IssuesList);

                //int ret = (int)UtilMethods.toInt(sql.ExecuteScalar(), 0);

                //if (ret <= 0)
                //{
                //    return false;
                //}

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

        #region STATISTICS
        public static int Count(int AccountID)
        {
            try
            {
                SqlDataObject sql = new SqlDataObject(StaticKeys.ConnStr, "Issue_Count");
                sql.AddInputParameter(false, "@AccountID", AccountID);

                return (int)UtilMethods.toInt(sql.ExecuteScalar(), 0);
            }
            catch (Exception ex)
            {
                ErrorLogger.LOGGER.Error(ex.Message, ex);
                return -1;
            }
        }



        public static int CountResolved(int AccountID)
        {
            try
            {
                SqlDataObject sql = new SqlDataObject(StaticKeys.ConnStr, "Issue_CountResolved");
                sql.AddInputParameter(false, "@AccountID", AccountID);

                sql.AddInputParameter(false, "@StateID", StaticKeys.StateResolvedID);

                return (int)UtilMethods.toInt(sql.ExecuteScalar(), 0);
            }
            catch (Exception ex)
            {
                ErrorLogger.LOGGER.Error(ex.Message, ex);
                return -1;
            }
        }



        #region GRAPHS
        public static DataSet GetByPriority(int AccountID)
        {
            try
            {
                SqlDataObject sql = new SqlDataObject(StaticKeys.ConnStr, "Issue_GetByPriority");
                sql.AddInputParameter(false, "@AccountID", AccountID);


                return sql.ExecuteDataSet();
            }
            catch (Exception ex)
            {
                ErrorLogger.LOGGER.Error(ex.Message, ex);
                return null;
            }
        }


        public static DataSet GetReportedVsResolved(int AccountID)
        {
            try
            {
                SqlDataObject sql = new SqlDataObject(StaticKeys.ConnStr, "Issue_GetReportedVsResolved");
                sql.AddInputParameter(false, "@AccountID", AccountID);
                sql.AddInputParameter(false, "@StateID", StaticKeys.StateResolvedID);

                return sql.ExecuteDataSet();
            }
            catch (Exception ex)
            {
                ErrorLogger.LOGGER.Error(ex.Message, ex);
                return null;
            }
        }



        #region  BY USER TYPES

        public static DataSet GetByPriorityAndUserTypes(List<SqlDataRecord> list, int AccountID)
        {
            try
            {
                SqlDataObject sql = new SqlDataObject(StaticKeys.ConnStr, "Issue_GetByPriorityAndUserTypes");
                sql.AddInputParameter(false, "@AccountID", AccountID);

                if (list.Count > 0)
                    sql.AddInputParameterTable("@TypeIDs", "TypesTable", list);

                return sql.ExecuteDataSet();
            }
            catch (Exception ex)
            {
                ErrorLogger.LOGGER.Error(ex.Message, ex);
                return null;
            }
        }


        public static DataSet GetReportedVsResolvedAndUserTypes(List<SqlDataRecord> list, int AccountID)
        {
            try
            {
                SqlDataObject sql = new SqlDataObject(StaticKeys.ConnStr, "Issue_GetReportedVsResolvedAndUserTypes");
                sql.AddInputParameter(false, "@AccountID", AccountID);
                sql.AddInputParameter(false, "@StateID", StaticKeys.StateResolvedID);


                if (list.Count > 0)
                    sql.AddInputParameterTable("@TypeIDs", "TypesTable", list);

                return sql.ExecuteDataSet();
            }
            catch (Exception ex)
            {
                ErrorLogger.LOGGER.Error(ex.Message, ex);
                return null;
            }
        }

        #endregion

        #endregion

        #endregion



        #region CLEAN VALUES
        private void CleanValues()
        {
            _ID = 0;
            _Subject = "";
            _Message = "";
            _GPS = null;
            _Blob = null;
            _BlobName = null;
            _BlobType = null;
            _BlobLen = null;
            _Priority = null;
            _DateReported = null;
            _ReportedBy = null;
            _TypeID = null;
            _AccountID = 0;
            _Deleted = false;
            _ImageRotation = null;
            
            _Device = null;
            _ReportedBy_UserID = null;

        }
        #endregion

        #region FILL VALUES
        private void FillValues(DataRow dr)
        {
            _ID = (int)UtilMethods.toInt(dr["ID"], 0);
            _Subject = dr["Subject"] + "";
            _Message = dr["Message"] + "";
            _GPS = dr["GPS"] + "";
            _Blob = UtilMethods.toByteArray(dr["Blob"], null);
            _BlobName = dr["BlobName"] + "" == "" ? null : dr["BlobName"] + "";
            _BlobType = dr["BlobType"] + "" == "" ? null : dr["BlobType"] + "";
            _BlobLen = UtilMethods.toInt(dr["BlobLen"], null);
            _Priority = UtilMethods.toInt(dr["Priority"], null);
            _DateReported = UtilMethods.toDateTime(dr["DateReported"], null);
            _ReportedBy = UtilMethods.toInt(dr["ReportedBy"], null);
            _TypeID = UtilMethods.toInt(dr["TypeID"], null);
            _AccountID = (int)UtilMethods.toInt(dr["AccountID"], 0);
            //_Deleted = (bool)UtilMethods.toBool(dr["Deleted"], false);
            _DateTaken = UtilMethods.toDateTime(dr["DateTaken"], null);

            _Latitude = dr["Latitude"] + "";
            _Longitude = dr["Longitude"] + "";
            _SortIndex = (int)UtilMethods.toInt(dr["SortIndex"], 1);
            _State = (int)UtilMethods.toInt(dr["State"], 1);
            _StateName = dr["StateName"] + "" == "" ? null : dr["StateName"] + "";

            _ImageRotation = UtilMethods.toInt(dr["ImageRotation"], null);
            _AssignedTo = UtilMethods.toInt(dr["AssignedTo"], null);

            _ReportedBy_UserID = UtilMethods.toInt(dr["ReportedBy_UserID"], null);
            try
            {
                _Device = dr["Device"] + "" == "" ? null : dr["Device"] + "";
            }
            catch (Exception)
            {
            }

            try
            {
                _TypeName = dr["TypeName"] + "";
            }
            catch (Exception)
            {
            }


            _ParentID = UtilMethods.toInt(dr["ParentID"], null);

            try
            {
                _ParentName = dr["ParentName"] + "";
            }
            catch (Exception)
            {
                // this exception is for procedures thar don't have ParentName yet @@TODO
            }

            _PropertyID = UtilMethods.toInt(dr["PropertyID"], null);


            try
            {
                _PropertyName = dr["PropertyName"] + "";
            }
            catch (Exception)
            {
                // this exception is for procedures thar don't have ParentName yet @@TODO
            }


        }
        #endregion



        #region GET NEAREST
        public static DataSet GetNearestIssues(int AccountID, int? Radius, string GPS)
        {

            try
            {
                SqlDataObject sql = new SqlDataObject(StaticKeys.ConnStr, "Issues_GetNearest");


                sql.AddInputParameter(false, "@AccountID", AccountID);
                sql.AddInputParameter(false, "@Radius", Radius);
                sql.AddInputParameter(false, "@GPS", GPS);
               


                return sql.ExecuteDataSet();
            }
            catch (Exception ex)
            {
                ErrorLogger.LOGGER.Error(ex.Message, ex);
                return null;
            }

        }


        public static int CountNearestIssues(int AccountID, int? Radius, string GPS)
        {

            try
            {
                SqlDataObject sql = new SqlDataObject(StaticKeys.ConnStr, "Issues_CountNearest");


                sql.AddInputParameter(false, "@AccountID", AccountID);
                sql.AddInputParameter(false, "@Radius", Radius);
                sql.AddInputParameter(false, "@GPS", GPS);



                return Convert.ToInt32( sql.ExecuteScalar());
            }
            catch (Exception ex)
            {
                ErrorLogger.LOGGER.Error(ex.Message, ex);
                return -1;
            }

        }
        #endregion


    }
}


