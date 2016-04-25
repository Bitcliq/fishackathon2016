using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitcliq.BIR.Platform
{
    public class TypeJson
    {
        private int _ID;
        private string _Name;

        private int? _ParentID;
        private string _ParentName = "";

        #region PUBLIC

        public int ID { get { return _ID; } set { _ID = value; } }
        public string Name { get { return _Name; } set { _Name = value; } }
        public int? ParentID { get { return _ParentID; } set { _ParentID = value; } }
        public string ParentName { get { return _ParentName; } set { _ParentName = value; } }

        #endregion

        public TypeJson(Type issue)
        {
            this._ID = issue.ID;
            this.Name = issue.Name;
            this.ParentID = issue.ParentID;
            this.ParentName = issue.ParentName;
        }

    }
}
