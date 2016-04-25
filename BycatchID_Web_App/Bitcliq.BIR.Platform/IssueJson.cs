using Bitcliq.BIR.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Drawing;
using Bitcliq.BIR.Logs;
using System.Data;
using Newtonsoft.Json;
using System.Collections;

namespace Bitcliq.BIR.Platform
{
    public class IssueJson
    {
        private int _ID;
        private string _Subject;
        private string _Message;
        private object _GPS;
        private string _PhotoUrl;
        private string _PhotoThumbnailUrl;
        private string _BlobName;
        private int? _Priority;
        private DateTime? _DateReported;
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

        ArrayList usersArray = new ArrayList();
        private int? _ReportedBy_UserID;

        private bool _HasPhoto;

        private int? _PropertyID;
        private string _PropertyName;


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


        public int? ParentID { get { return _ParentID; } set { _ParentID = value; } }
        public string ParentName { get { return _ParentName; } set { _ParentName = value; } }


        public ArrayList UsersAssigned
        {
            get { return usersArray; }
            set { usersArray = value; }
        }

        public int? ReportedBy_UserID
        {
            get { return _ReportedBy_UserID; }
            set { _ReportedBy_UserID = value; }
        }

        public bool HasPhoto
        {
            get { return _HasPhoto; }
            set { _HasPhoto = value; }
        }
        #endregion


        public IssueJson(Issue issue, bool forWS)
        {
            this._ID = issue.ID;
            this._Subject = issue.Subject;
            this._Message = issue.Message;
            if (issue.Latitude + "" != "")
                this._Latitude = issue.Latitude.Replace(",", ".");
            if (issue.Longitude + "" != "")
                this.Longitude = issue.Longitude.Replace(",", ".");
            this._TypeID = issue.TypeID;
            this._Priority = issue.Priority;
            this._ReportedBy = issue.ReportedBy;
            this.AccountID = issue.AccountID;
            this.DateReported = issue.DateReported;
            this._State = issue.State;
            this._SortIndex = issue.SortIndex;
            this._StateName = issue.StateName;
            this.TypeName = issue.TypeName;
            this.AssignedTo = issue.AssignedTo;
            this._ParentName = issue.ParentName;
            this._ParentID = issue.ParentID;

            this._ReportedBy_UserID = issue.ReportedBy_UserID;

            this._PropertyID = issue.PropertyID;
            this._PropertyName = issue.PropertyName;



            if (issue.ReportedBy > 0)
            {
                Reporter r = new Reporter((int)issue.ReportedBy);

                _ReporterName = r.Name;
                _ReporterEmail = r.Email;

            }
            else if (issue.ReportedBy_UserID > 0)
            {
                User r = new User((int)issue.ReportedBy_UserID);

                _ReporterName = r.Name;
                _ReporterEmail = r.Email;
            }


            // save image
            #region IMAGE
            if (issue.Blob != null)
            {
                try
                {

                    string name = "img" + issue.ID + "_" + issue.BlobName;
                    string fName = StaticKeys.BackofficeTempPath + name;

                    if (!File.Exists(fName))
                    {
                        UtilMethods.CreateImage(issue.Blob, fName, issue.ImageRotation);
                    }

                    this._PhotoUrl = StaticKeys.BackofficeTempUrl + name;


                    // create thumbnail


                    byte[] arr = File.ReadAllBytes(fName);
                    name = "imgThumb" + issue.ID + "_" + issue.BlobName;
                    fName = StaticKeys.BackofficeTempPath + name;

                    if (!File.Exists(fName))
                    {

                        int Img_PreferredHeight = 230;
                        int Img_PreferredWidth = 180;

                        try
                        {
                            Img_PreferredWidth = Convert.ToInt32(StaticKeys.Img_PreferredWidth);
                        }
                        catch (Exception)
                        {
                        }

                        try
                        {
                            Img_PreferredHeight = Convert.ToInt32(StaticKeys.Img_PreferredHeight);
                        }
                        catch (Exception)
                        {
                        }




                        Bitmap b = UtilMethods.ResizeImageWithouAdapt(arr, Img_PreferredWidth, Img_PreferredHeight);

                        b.Save(fName);
                        b.Dispose();



                    }
                    this._PhotoThumbnailUrl = StaticKeys.BackofficeTempUrl + name;



                }

                catch (Exception ex)
                {
                    ErrorLogger.LOGGER.Error(ex.Message, ex);
                }
                _HasPhoto = true;
            }
            else
            {
                this._PhotoThumbnailUrl = StaticKeys.BackofficeUrl + "img/no_photo_200x200.jpg";

                this._PhotoUrl = StaticKeys.BackofficeUrl + "img/no_photo_400x400.jpg";
                _HasPhoto = false;
            }
            #endregion

            #region GET ASSIGNED USERS

            DataSet ds = Issue_AssignedUsers.ListAssignedUsers(issue.ID);

            if (ds != null)
            {

                foreach (DataRow dr in ds.Tables[0].Rows)
                    usersArray.Add(dr["AssignedTo"] + "");
            }


            #endregion

            _ParentID = issue.ParentID;
            _ParentName = issue.ParentName;







        }

    }
}
