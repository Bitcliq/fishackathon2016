using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitcliq.BIR.Utils
{
    public class MarkerHelperResult
    {
        public MarkerHelperResult()
        {


        }


        string _id;
        string _label;
        string _lat;
        string _lng;
        string _icon;



        public string label
        {
            get { return _label; }
            set { _label = value; }
        }

        public string id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string lat
        {
            get { return _lat; }
            set { _lat = value; }
        }

        public string lng
        {
            get { return _lng; }
            set { _lng = value; }
        }

        public string icon
        {
            get { return _icon; }
            set { _icon = value; }
        }
    }
}
