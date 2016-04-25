using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using PinFixed.BL;
using CoreGraphics;
using System.Drawing;

namespace PinFixed.iOS
{
    public class ImageServiceIOS : IImageResizer
    {
        public byte[] ResizeImage(string fileName, byte[] imageData, float width, float height)
        {
            UIImage originalImage = ImageFromByteArray(imageData);

            //switch (originalImage.Orientation)
            //{
            //    case UIImageOrientation.Left:
            //        bitmap.RotateCTM((float)Math.PI / 2);
            //        bitmap.TranslateCTM(0, -height);
            //        break;
            //    case UIImageOrientation.Right:
            //        bitmap.RotateCTM(-((float)Math.PI / 2));
            //        bitmap.TranslateCTM(-width, 0);
            //        break;
            //    case UIImageOrientation.Up:
            //        break;
            //    case UIImageOrientation.Down:
            //        bitmap.TranslateCTM(width, height);
            //        bitmap.RotateCTM(-(float)Math.PI);
            //        break;
            //}




            using (CGImage imageRef = originalImage.CGImage)
            {
                CGImageAlphaInfo alphaInfo = imageRef.AlphaInfo;
                CGColorSpace colorSpaceInfo = CGColorSpace.CreateDeviceRGB();
                if (alphaInfo == CGImageAlphaInfo.None)
                {
                    alphaInfo = CGImageAlphaInfo.NoneSkipLast;
                }


                int maxSize = (int)width;

                width = imageRef.Width;
                height = imageRef.Height;


                if (height >= width)
                {
                    width = (int)Math.Floor((double)width * ((double)maxSize / (double)height));
                    height = maxSize;
                }
                else
                {
                    height = (int)Math.Floor((double)height * ((double)maxSize / (double)width));
                    width = maxSize;
                }


                CGBitmapContext bitmap;

                if (originalImage.Orientation == UIImageOrientation.Up || originalImage.Orientation == UIImageOrientation.Down)
                {
                    bitmap = new CGBitmapContext(IntPtr.Zero, (int)width, (int)height, imageRef.BitsPerComponent, imageRef.BytesPerRow, colorSpaceInfo, alphaInfo);
                }
                else
                {
                    bitmap = new CGBitmapContext(IntPtr.Zero, (int)height, (int)width, imageRef.BitsPerComponent, imageRef.BytesPerRow, colorSpaceInfo, alphaInfo);
                }

                try
                {
                    switch (originalImage.Orientation)
                    {
                        case UIImageOrientation.Left:
                            bitmap.RotateCTM((float)Math.PI / 2);
                            bitmap.TranslateCTM(0, -height);
                            break;
                        case UIImageOrientation.Right:
                            bitmap.RotateCTM(-((float)Math.PI / 2));
                            bitmap.TranslateCTM(-width, 0);
                            break;
                        case UIImageOrientation.Up:
                            break;
                        case UIImageOrientation.Down:
                            bitmap.TranslateCTM(width, height);
                            bitmap.RotateCTM(-(float)Math.PI);
                            break;
                    }
                }
                catch (Exception)
                {
                }

                bitmap.DrawImage(new Rectangle(0, 0, (int)width, (int)height), imageRef);



                UIImage res = UIImage.FromImage(bitmap.ToImage());
                bitmap = null;

                return res.AsJPEG().ToArray();

              

            }



            ////create a 24bit RGB image
            //using (CGBitmapContext context = new CGBitmapContext(IntPtr.Zero,
            //    (int)width, (int)height, 8,
            //    (int)(4 * width), CGColorSpace.CreateDeviceRGB(),
            //    CGImageAlphaInfo.PremultipliedFirst))
            //{

            //    RectangleF imageRect = new RectangleF(0, 0, width, height);

            //    // draw the image
            //    context.DrawImage(imageRect, originalImage.CGImage);

            //    UIKit.UIImage resizedImage = UIKit.UIImage.FromImage(context.ToImage());

            //    // save the image as a jpeg
            //    return resizedImage.AsJPEG().ToArray();
            //}
        }

        public static UIKit.UIImage ImageFromByteArray(byte[] data)
        {
            if (data == null)
            {
                return null;
            }

            UIKit.UIImage image;
            try
            {
                image = new UIKit.UIImage(Foundation.NSData.FromArray(data));
            }
            catch (Exception e)
            {
                //Console.WriteLine("Image load failed: " + e.Message);
                return null;
            }
            return image;
        }

    }
}