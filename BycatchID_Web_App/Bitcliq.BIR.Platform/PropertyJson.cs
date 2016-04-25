using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitcliq.BIR.Platform
{
    public class PropertyJson
    {

        private int _PropertyID;
        private string _PropertyName;

        public int PropertyID
        {
            get { return _PropertyID; }
            set { _PropertyID = value; }
        }


        public string PropertyName
        {
            get { return _PropertyName; }
            set { _PropertyName = value; }
        }


        public PropertyJson(Property p)
        {
            _PropertyID = p.ID;
            _PropertyName = p.Name;
        }

    }
}
