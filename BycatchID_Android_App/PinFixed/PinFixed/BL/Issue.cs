using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinFixed.BL
{
    public class Issue
    {
        private int _ID;
        private string _Subject;
        private string _Message;
        private object _GPS;
        private string _PhotoUrl;
        private string _PhotoThumbnailUrl;
        private string _BlobName;
        private int? _Priority;
        private DateTime _DateReported;
        private int? _ReportedBy;
        private int? _TypeID;
        private int _AccountID;
        private DateTime? _DateTaken;

        private string _ReporterName;
        private string _ReporterEmail;
        private string _Latitude;
        private string _Longitude;
        private int _SortIndex;
        private int _State;
        private string _StateName;
        private string _TypeName;
        private int? _AssignedTo;

        private int? _ParentID;
        private string _ParentName = "";

        private string _PropertyID;
        private string _PropertyName;



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

        public string BlobName
        {
            get { return _BlobName; }
            set { _BlobName = value; }
        }

        public int? Priority
        {
            get { return _Priority; }
            set { _Priority = value; }
        }
        public DateTime DateReported
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


        public DateTime? DateTaken
        {
            get { return _DateTaken; }
            set { _DateTaken = value; }
        }

        public string PhotoUrl
        {
            get { return _PhotoUrl; }
            set { _PhotoUrl = value; }
        }



        public string PhotoThumbnailUrl
        {
            get { return _PhotoThumbnailUrl; }
            set { _PhotoThumbnailUrl = value; }
        }

        public string Latitude { get { return _Latitude; } set { _Latitude = value; } }
        public string Longitude { get { return _Longitude; } set { _Longitude = value; } }

        public int? ParentID { get { return _ParentID; } set { _ParentID = value; } }
        public string ParentName { get { return _ParentName; } set { _ParentName = value; } }
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
        public string StateName
        {
            get { return _StateName; }
            set { _StateName = value; }
        }
        public string TypeName
        {
            get { return _TypeName; }
            set { _TypeName = value; }
        }
        public string ReporterName { get { return _ReporterName; } set { _ReporterName = value; } }
        public string ReporterEmail { get { return _ReporterEmail; } set { _ReporterEmail = value; } }


        public int? AssignedTo
        {
            get { return _AssignedTo; }
            set { _AssignedTo = value; }
        }


        public string DateFormated
        {
            get { return _DateReported.ToString("dd-MM-yyyy HH:mm"); }
        }


        public string SubjectFormatted
        {
            get {

                if (_ParentName + "" != "")
                    return _Subject + " (" + _ParentName + ")";
                else if (_TypeName + "" != "")
                    return _Subject + " (" + _TypeName + ")";
                else
                    return _Subject;
            }
        }


        public string TypeNameFormatted
        {
            get
            {

                if (_ParentName + "" != "")
                    return _ParentName ;
                else if (_TypeName + "" != "")
                    return _TypeName;
                else
                    return _Subject;
            }
        }
        public string PropertyID
        {
            get { return _PropertyID; }
            set { _PropertyID = value; }
        }


        public string PropertyName
        {
            get { return _PropertyName; }
            set { _PropertyName = value; }
        }



        public Issue(int id, string photoThumbnailUrl, string subject, string state, string message, string type, DateTime dateReported, string area, string latitude, string longitude, 
            string photoUrl, string PropertyID, string PropertyName)
        {
            ID = id;
            _PhotoThumbnailUrl = photoThumbnailUrl;
            _Subject = subject;
            _StateName = state;
            _Message = message;
            _TypeName = type;
            _DateReported = dateReported;

            _Latitude = latitude;
            _Longitude = longitude;
          
             _ParentName = area;
             _PhotoUrl = photoUrl;
             _PropertyID = PropertyID;
             _PropertyName = PropertyName;

        }
    }
}
