using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using System.IO;
using Android.Util;
using PinFixed.BL;
using Android.Media;

namespace PinFixed.Droid
{
    public class ImageServiceDroid : IImageResizer
    {
       // byte[] ResizeImage(byteguste[] imageData, double width, double height);


        public  byte[] ResizeImage(string fileName, byte[] imageData, float width, float height)
        {
            // Load the bitmap

            BitmapFactory.Options options = new BitmapFactory.Options();// Create object of bitmapfactory's option method for further option use
            options.InPurgeable = true;
            options.InDither = false;
            options.InInputShareable = true;
            options.InPreferredConfig = Bitmap.Config.Rgb565;

            

            Bitmap originalImage = BitmapFactory.DecodeByteArray(imageData, 0, imageData.Length, options);

            Bitmap resizedImage = Bitmap.CreateScaledBitmap(originalImage, (int)width, (int)height, false);
            
            // smasung case
            Matrix mtx = new Matrix();
            ExifInterface exif = new ExifInterface(fileName);
            string orientation = exif.GetAttribute(ExifInterface.TagOrientation);


            switch (orientation)
            {

                case "0": // ORIENTATION_UNDEFINED - Nexus 7 landscape...

                    break;

                case "1": // landscape - ORIENTATION_NORMAL

                    break;

                case "2": // ORIENTATION_FLIP_HORIZONTAL

                    break;

                case "3": // ORIENTATION_ROTATE_180

                    mtx.PreRotate(180);

                    resizedImage = Bitmap.CreateBitmap(resizedImage, 0, 0, resizedImage.Width, resizedImage.Height, mtx, false);

                    mtx.Dispose();

                    mtx = null;

                    break;

                case "4": // ORIENTATION_FLIP_VERTICAL

                    break;

                case "5": //ORIENTATION_TRANSPOSE

                    break;

                case "6": // portrait - ORIENTATION_ROTATE_90

                    mtx.PreRotate(90);

                    resizedImage = Bitmap.CreateBitmap(resizedImage, 0, 0, resizedImage.Width, resizedImage.Height, mtx, false);

                    mtx.Dispose();

                    mtx = null;

                    break;

                case "7": // ORIENTATION_TRANSVERSE

                    break;

                case "8": // ORIENTATION_ROTATE_270 - might need to flip horizontally too...

                    mtx.PreRotate(270);

                    resizedImage = Bitmap.CreateBitmap(resizedImage, 0, 0, resizedImage.Width, resizedImage.Height, mtx, false);

                    mtx.Dispose();

                    mtx = null;

                    break;

                default:

                    mtx.PreRotate(90);

                    resizedImage = Bitmap.CreateBitmap(resizedImage, 0, 0, resizedImage.Width, resizedImage.Height, mtx, false);

                    mtx.Dispose();

                    mtx = null;

                    break;
            }
            using (MemoryStream ms = new MemoryStream())
            {
                resizedImage.Compress(Bitmap.CompressFormat.Jpeg, 80, ms);
                resizedImage.Recycle();
                originalImage.Recycle();
                originalImage.Dispose();
                resizedImage.Dispose();

                try
                {
                    resizedImage = null;
                    originalImage = null;
                }
                catch(Exception ex)
                {
                    
                }

                System.GC.Collect();

                return ms.ToArray();
            }
        }

        //public void ResizeImage(string sourceFile, string targetFile, float maxWidth, float maxHeight)
        //{
        //    if (!File.Exists(targetFile) && File.Exists(sourceFile))
        //    {
        //        // First decode with inJustDecodeBounds=true to check dimensions
        //        var options = new BitmapFactory.Options()
        //        {
        //            InJustDecodeBounds = false,
        //            InPurgeable = true,
        //        };

        //        using (var image = BitmapFactory.DecodeFile(sourceFile, options))
        //        {
        //            if (image != null)
        //            {
        //                var sourceSize = new Size((int)image.GetBitmapInfo().Height, (int)image.GetBitmapInfo().Width);

        //                var maxResizeFactor = Math.Min(maxWidth / sourceSize.Width, maxHeight / sourceSize.Height);

        //                string targetDir = System.IO.Path.GetDirectoryName(targetFile);
        //                if (!Directory.Exists(targetDir))
        //                    Directory.CreateDirectory(targetDir);

        //                if (maxResizeFactor > 0.9)
        //                {
        //                    File.Copy(sourceFile, targetFile);
        //                }
        //                else
        //                {
        //                    var width = (int)(maxResizeFactor * sourceSize.Width);
        //                    var height = (int)(maxResizeFactor * sourceSize.Height);

        //                    using (var bitmapScaled = Bitmap.CreateScaledBitmap(image, height, width, true))
        //                    {
        //                        using (Stream outStream = File.Create(targetFile))
        //                        {
        //                            if (targetFile.ToLower().EndsWith("png"))
        //                                bitmapScaled.Compress(Bitmap.CompressFormat.Png, 100, outStream);
        //                            else
        //                                bitmapScaled.Compress(Bitmap.CompressFormat.Jpeg, 95, outStream);
        //                        }
        //                        bitmapScaled.Recycle();
        //                    }
        //                }

        //                image.Recycle();
        //            }
        //            //else
        //            //    Log.E("Image scaling failed: " + sourceFile);
        //        }
        //    }
        //}
    }
}