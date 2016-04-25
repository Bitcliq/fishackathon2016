using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinFixed.BL
{
    public class NewIssue
    {
        
        private string _Subject;
        private string _Message;
        private object _GPS;
        
        
        private string _BlobName;
        
        private int? _ReportedBy;
        private int? _TypeID;
        //private Stream _mediaFile;
        private byte[] _resizedImage;
        private string _fileName;
        private double? _Latitude;
        private double? _Longitude;
        private int? _Orientation;

        private string _Device;
        private string _PropertyID;
        private string _PropertyName;


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
    

       

        //public Stream MediaFile
        //{
        //    get { return _mediaFile; }
        //    set { _mediaFile = value; }
        //}


        public byte[] ResizedImage
        {
            get { return _resizedImage; }
            set { _resizedImage = value; }
        }
        public string FileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }

        public double? Latitude
        {
            get { return _Latitude; }
            set { _Latitude = value; }
        }

        public double? Longitude
        {
            get { return _Longitude; }
            set { _Longitude = value; }
        }

        public int? Orientation
        {
            get { return _Orientation; }
            set { _Orientation = value; }
        }

        public string Device
        {
            get { return _Device; }
            set { _Device = value; }
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

        public NewIssue(string subject, string message, byte[] media, string fileName, double? lat, double? lng,
                        int? typeID, int? orientation, string device, string PropertyID, string PropertyName)
        {
           
            _Subject = subject;
            _Message = message;
            //_mediaFile = media;
            _resizedImage = media;
            _fileName = fileName;
            _Latitude = lat;
            _Longitude = lng;
            _TypeID = typeID;
            _Orientation = orientation;
            _Device = device;
            _PropertyID = PropertyID;
            _PropertyName = PropertyName;

             
        }
    }
}
