using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitcliq.BIR.Utils
{
       

    public class ImgTemp
    {

        private byte[] _Audio;
        private string _AudioType;
        private int _AudioLength;
        private string _AudioExtension;
        private string _AudioName;


        private string _FullPath;

        public byte[] Audio { get { return _Audio; } set { _Audio = value; } }
        public string AudioType { get { return _AudioType; } set { _AudioType = value; } }
        public int AudioLength { get { return _AudioLength; } set { _AudioLength = value; } }
        public string AudioExtension { get { return _AudioExtension; } set { _AudioExtension = value; } }
       
        public string AudioName { get { return _AudioName; } set { _AudioName = value; } }

        public string FullPath { get { return _FullPath; } set { _FullPath = value; } }

        public ImgTemp(byte[] Audio, 
            string AudioName,  string AudioType, int AudioLength, string AudioExtension, string FullPath)
        {
            _Audio = Audio;
            _AudioName = AudioName;
            _AudioType = AudioType;
            _AudioLength = AudioLength;
            _AudioExtension = AudioExtension;
            _FullPath = FullPath;
         
        }

    }
}
