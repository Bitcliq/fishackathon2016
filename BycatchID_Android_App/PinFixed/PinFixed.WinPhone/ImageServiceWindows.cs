using PinFixed.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinFixed.WinPhone
{
    public class ImageServiceWindows : IImageResizer 
    {
        public byte[] ResizeImage(string fileName, byte[] imageData, float width, float height)
        {

            return imageData;
        }

    }
}
