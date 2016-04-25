using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinFixed.BL
{
    public interface IImageResizer
    {
        byte[] ResizeImage(string filePath, byte[] imageData, float width, float height);


    }
}
