using Bitcliq.BIR.Logs;
using Bitcliq.BIR.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitcliq.BIR.Platform
{
    public class IssueStateJson
    {
        private int _ID;
        private int _IssueID;
        private string _InternalNotes;
        private object _GPS;
        private string _PhotoUrl;
        private string _BlobName;
        private string _StateDate;
       
        private string _Latitude;
        private string _Longitude;
        private int? _UserID;

        private string _FixedBy;
        private int? _ReporterIDClosed;

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
        public string InternalNotes
        {
            get { return _InternalNotes; }
            set { _InternalNotes = value; }
        }
        public object GPS
        {
            get { return _GPS; }
            set { _GPS = value; }
        }
       
        public string BlobName
        {
            get { return _BlobName; }
            set { _BlobName = value; }
        }
      
       
        public string StateDate
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
        public string PhotoUrl
        {
            get { return _PhotoUrl; }
            set { _PhotoUrl = value; }
        }
        public string FixedBy
        {
            get { return _FixedBy; }
            set { _FixedBy = value; }
        }
        public string Latitude { get { return _Latitude; } set { _Latitude = value; } }
        public string Longitude { get { return _Longitude; } set { _Longitude = value; } }


       
        #endregion


        public IssueStateJson(IssueState issue)
        {
            this._ID = issue.ID;

            this.UserID = issue.UserID;
            this._InternalNotes = issue.InternalNotes;
            
            if(issue.Latitude + "" != "")
                this._Latitude = issue.Latitude.Replace(",", ".");
            if (issue.Longitude + "" != "")
                this.Longitude = issue.Longitude.Replace(",", ".");
            this._StateDate = issue.StateDate + "";

            this._ReporterIDClosed = issue.ReporterIDClosed;
            if (issue.UserID > 0)
            {
                User r = new User((int)issue.UserID);

                _FixedBy = r.Name;
            }

            int? imgRot = null;

            if (issue.IssueID > 0)
            {
                Issue i = new Issue((int)issue.IssueID);

                imgRot = i.ImageRotation;
                _IssueID = i.ID;
            }
            // save image

            if (issue.Blob != null)
            {

                try
                {

                    string name = "img" + issue.ID + "_" + issue.BlobName;
                    string fName = StaticKeys.BackofficeTempPath + issue.ID + name;

                    if (!File.Exists(fName))
                    {
                        UtilMethods.CreateImage(issue.Blob, fName, imgRot);
                    }

                    this._PhotoUrl = StaticKeys.BackofficeTempUrl + issue.ID + name;



                }
                catch (Exception ex)
                {
                    ErrorLogger.LOGGER.Error(ex.Message, ex);
                }
            }
            else
                this._PhotoUrl = "";

        }



        

    }
}
