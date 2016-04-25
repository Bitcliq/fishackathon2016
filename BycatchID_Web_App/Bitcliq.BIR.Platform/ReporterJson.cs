using Bitcliq.BIR.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitcliq.BIR.Platform
{
    public class ReporterJson
    {
        private int _ID;
        private string _Name;
        private string _Email;
        private object _PhoneNumber;
       
        
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
        public string Email
        {
            get { return _Email; }
            set { _Email = value; }
        }
        public object PhoneNumber
        {
            get { return _PhoneNumber; }
            set { _PhoneNumber = value; }
        }
       
     




        #endregion


        public ReporterJson(Reporter rep)
        {
            this._ID = rep.ID;
            this._Name = rep.Name;
            this._Email = rep.Email;
            this._PhoneNumber = rep.PhoneNumber;

        }


        public ReporterJson(User rep)
        {
            this._ID = rep.UserID;
            this._Name = rep.Name;
            this._Email = rep.Email;
            this._PhoneNumber = "";

        }


    }
}

