using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinFixed.BL
{
    public class TypesWS
    {
        public int ID;
        public string Name;
        public int? ParentID;
        public string ParentName;


        public TypesWS(int id, string name, int? parentID, string parentName)
        {
            ID = id;
            Name = name;
            ParentID = parentID;
            parentName = ParentName;

        }
    }


    public class JSONObjectList
    {
        [JsonProperty("DATA")]
        public List<TypesWS> DT { get; set; }
    }

}
