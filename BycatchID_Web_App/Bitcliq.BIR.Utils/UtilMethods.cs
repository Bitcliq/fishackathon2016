using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace Bitcliq.BIR.Utils
{
    public class UtilMethods
    {


      

        #region DATABASE METHODS
        public static bool DataSetHasData(DataSet p_Ds)
        {
            bool ret = false;

            if (p_Ds != null)
            {
                if (p_Ds.Tables.Count > 0)
                {
                    if (p_Ds.Tables[0].Rows.Count > 0)
                    {
                        ret = true;
                    }
                }
            }


            return ret;
        }

        public static bool DataSetHasOnlyOneDataRow(DataSet p_Ds)
        {
            bool ret = false;

            if (p_Ds != null)
            {
                if (p_Ds.Tables.Count == 1)
                {
                    if (p_Ds.Tables[0].Rows.Count == 1)
                    {
                        ret = true;
                    }
                }
            }
            return ret;
        }


        #endregion

        #region CONVERTION METHODS
        public static int? toInt(object num, int? inError)
        {
            int? i = inError;

            try
            {
                i = Convert.ToInt32(num);
            }
            catch
            {
                i = inError;
            }

            return i;
        }

        public static int? toMiliseconds(int? num)
        {
            try
            {
                return num * 1000; ;
            }
            catch
            {
                return null;
            }
        }

        public static double? toDouble(object num, double? inError)
        {
            double? i = inError;

            try
            {
                i = Convert.ToDouble(num);
            }
            catch
            {
                i = inError;
            }

            return i;
        }




        public static string toString(object str, string inError)
        {
            string i = inError;

            try
            {
                i = str + "";
            }
            catch
            {
                i = inError;
            }

            return i;
        }

        #region DecimalToString
        /// <summary>
        /// Devolve String Formatada como um Double.
        /// </summary>
        /// <param name="Value">Valor a Formatar.</param>
        /// <param name="Decimals">Número de Casas Decimais.</param>
        /// <param name="ValueFormat">Formato na conversão de Double para String. Ex: ##0,00 ou 00,000 etc.</param>
        /// <param name="OutputFormat">Formato em que será devolvido o valor. Vazio para obter apenas o valor ou: "@%" ou "@ cm", onde o @ será substituido pelo valor. Caso seja vazio devolve simplesmente o valor.</param>
        /// <param name="ZeroFormat">Texto a ser devolvido no caso de o valor ser zero. Ex: "---". Caso seja vazio, devolve o valor zero formatado com o OutputFormat.</param>
        /// <returns>Valor Formatado</returns>
        public static string DecimalToString(string Value, int Decimals, string ValueFormat, string OutputFormat, string ZeroFormat)
        {
            decimal dbl = 0;
            string ret = "";

            try
            {
                dbl = Convert.ToDecimal(Value);
            }
            catch
            {
                dbl = 0;
            }

            if (Decimals > 0)
                dbl = Math.Round(dbl, Decimals);

            if (ValueFormat == "")
            {
                ret = dbl.ToString();
            }
            else
            {
                ret = dbl.ToString(ValueFormat);
            }

            if (dbl == 0)
            {
                if (ZeroFormat == "")
                {
                    if (OutputFormat != "" && OutputFormat.Contains("@"))
                    {
                        ret = OutputFormat.Replace("@", ret);
                    }
                }
                else
                {
                    ret = ZeroFormat;
                }
            }
            else
            {
                if (OutputFormat != "" && OutputFormat.Contains("@"))
                {
                    ret = OutputFormat.Replace("@", ret);
                }
            }

            return ret;
        }
        #endregion

        public static decimal? toDecimal(string Value, decimal? inError)
        {
            decimal? dbl = inError;

            char[] ch = Value.ToCharArray();
            string s = "";
            string ret = "";
            bool first = true;

            try
            {
                for (int i = ch.Length - 1; i >= 0; i--)
                {
                    s = ch[i].ToString();

                    if (s == "0" || s == "1" || s == "2" || s == "3" || s == "4" || s == "5" || s == "6" || s == "7" || s == "8" || s == "9" || s == "-")
                    {
                        ret = s + ret;
                    }
                    else
                    {
                        if (first)
                        {
                            ret = System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator + ret;
                            first = false;
                        }
                    }
                }

                dbl = Convert.ToDecimal(ret);
            }
            catch
            {
                dbl = inError;
            }

            return dbl;
        }

        public static string DecimalToDBString(string Value, decimal inError)
        {
            string ret = "";
            decimal dbl = 0;

            try
            {
                dbl = Convert.ToDecimal(Value);
            }
            catch
            {
                return inError.ToString();
            }

            if (dbl == 0)
            {
                ret = "0";
            }
            else
            {
                char[] ch = Value.ToCharArray();
                string s = "";
                bool first = true;

                for (int i = ch.Length - 1; i >= 0; i--)
                {
                    s = ch[i].ToString();

                    if (s == "0" || s == "1" || s == "2" || s == "3" || s == "4" || s == "5" || s == "6" || s == "7" || s == "8" || s == "9" || s == "-")
                    {
                        ret = s + ret;
                    }
                    else
                    {
                        if (first)
                        {
                            ret = "." + ret;
                            first = false;
                        }
                    }
                }
            }

            return ret;
        }

        public static DateTime? toDateTime(object dt, DateTime? inError)
        {
            DateTime? d = inError;

            try
            {
                d = Convert.ToDateTime(dt);
            }
            catch
            {
                d = inError;
            }

            return d;
        }

        public static bool? toBool(object bol, bool? inError)
        {
            bool? b = inError;

            try
            {
                b = Convert.ToBoolean(bol);
            }
            catch
            {
                b = inError;
            }

            return b;
        }

        public static byte[] toByteArray(object arr, byte[] inError)
        {
            byte[] ba = inError;

            try
            {
                ba = (byte[])arr;
            }
            catch
            {
                ba = inError;
            }

            return ba;
        }

        public static Guid toGuid(object guid, Guid inError)
        {
            Guid g = inError;

            try
            {
                g = new Guid(guid + "");
            }
            catch
            {
                g = inError;
            }

            return g;
        }
        #endregion






        #region FILES

        public static string ReadTextFile(string p_FileName)
        {
            string content = File.ReadAllText(p_FileName, System.Text.Encoding.GetEncoding("windows-1252"));
            return content;
        }



        public static void SaveTextFile(string p_FileName, string p_Content)
        {
            StreamWriter w = new StreamWriter(p_FileName, false, System.Text.Encoding.GetEncoding("windows-1252"));
            w.Write(p_Content);
            w.Close();
        }

        #endregion

        public static void CreateFile(byte[] p_Fich, string p_Caminho)
        {
            FileStream fs = new FileStream(p_Caminho, FileMode.Create);
            //FileStream fs = new FileStream(p_Caminho, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            fs.Write(p_Fich, 0, p_Fich.Length);
            fs.Close();
        }


        public static string CreateImage(byte[] p_Fich, string p_Caminho, int? Orientation)
        {
            FileStream fs = new FileStream(p_Caminho, FileMode.Create);
            //FileStream fs = new FileStream(p_Caminho, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            fs.Write(p_Fich, 0, p_Fich.Length);
            fs.Close();


            System.Drawing.Image img = null;
            try
            {

               
                img = System.Drawing.Image.FromFile(p_Caminho);

                switch(Orientation)
                {

                       

                    case 0:
                        //Rodar 180º para a direita;
                        //img.RotateFlip(RotateFlipType.Rotate180FlipNone);

                        //img.RotateFlip(RotateFlipType.Rotate180FlipX);
                        //img.RotateFlip(RotateFlipType.Rotate180FlipXY);
                        //img.RotateFlip(RotateFlipType.Rotate180FlipXY);
                    break;
                    case 1:
                        img.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    break;
                    case 2:
                       


                        img.RotateFlip(RotateFlipType.Rotate90FlipXY);

                        //img.Save(p_Caminho + "Rotate270FlipNone");
                        //img.Dispose();

                        //img = System.Drawing.Image.FromFile(p_Caminho);

                        //img.RotateFlip(RotateFlipType.Rotate270FlipX);

                        //img.Save(p_Caminho + "Rotate270FlipX");
                        //img.Dispose();

                        //img = System.Drawing.Image.FromFile(p_Caminho);

                        //img.RotateFlip(RotateFlipType.Rotate270FlipY);

                        //img.Save(p_Caminho + "Rotate270FlipY");
                        //img.Dispose();


                        //  img = System.Drawing.Image.FromFile(p_Caminho);

                        //img.RotateFlip(RotateFlipType.Rotate270FlipXY);

                        //img.Save(p_Caminho + "Rotate270FlipXY");
                        //img.Dispose();

                        //img = System.Drawing.Image.FromFile(p_Caminho);

                        //img.RotateFlip(RotateFlipType.Rotate180FlipNone);

                        //img.Save(p_Caminho + "Rotate180FlipNone");
                        //img.Dispose();


                        //img = System.Drawing.Image.FromFile(p_Caminho);

                        //img.RotateFlip(RotateFlipType.Rotate180FlipX);

                        //img.Save(p_Caminho + "Rotate180FlipX");
                        //img.Dispose();


                        // img = System.Drawing.Image.FromFile(p_Caminho);

                        //img.RotateFlip(RotateFlipType.Rotate180FlipY);

                        //img.Save(p_Caminho + "Rotate180FlipY");
                        //img.Dispose();

                        //img = System.Drawing.Image.FromFile(p_Caminho);

                        //img.RotateFlip(RotateFlipType.Rotate180FlipXY);

                        //img.Save(p_Caminho + "Rotate180FlipXY");
                        //img.Dispose();


                        //  img = System.Drawing.Image.FromFile(p_Caminho);

                        //img.RotateFlip(RotateFlipType.Rotate90FlipNone);

                        //img.Save(p_Caminho + "Rotate90FlipNone");
                        //img.Dispose();


                        //  img = System.Drawing.Image.FromFile(p_Caminho);

                        //img.RotateFlip(RotateFlipType.Rotate90FlipX);

                        //img.Save(p_Caminho + "Rotate90FlipX");
                        //img.Dispose();
                        //   img = System.Drawing.Image.FromFile(p_Caminho);

                        //img.RotateFlip(RotateFlipType.Rotate90FlipXY);

                        //img.Save(p_Caminho + "Rotate90FlipXY");
                        //img.Dispose();

                        
                        //img.RotateFlip(RotateFlipType.Rotate90FlipY);

                        //img.Save(p_Caminho + "Rotate90FlipY");
                        //img.Dispose();
                        //Rodar 90º para a esquerda;
                        //img.RotateFlip(RotateFlipType.Rotate270FlipNone);


                        //Rodar 90º para a direita;
                        //img.RotateFlip(RotateFlipType.Rotate180FlipNone);


                    break;
                    case 3:
                        //Rodar 90º para a direita;
                        img.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    break;
                    case null:
                    break;
                    default:
                    break;
                
                }

                ///save the image out to the file
                img.Save(p_Caminho);

                //release image file
                img.Dispose();

                return p_Caminho;
                //return fileName;
            }
            catch (Exception)
            {

                if (img != null)
                    img.Dispose();

                

                return "";
            }
            return "";
        }


        public static string RotateImage(string fileName)
        {
            System.Drawing.Image img = null;
            try
            {

                Uri uri = new Uri(fileName);

                string name = uri.Segments[uri.Segments.Count() - 1];

                //if (uri.IsFile)
                //{
                //    f = System.IO.Path.GetFileName(uri.LocalPath);
                //}
                //create an image object from the image in that path
                img = System.Drawing.Image.FromFile(StaticKeys.BackofficeTempPath + name);

                //rotate the image
                img.RotateFlip(RotateFlipType.Rotate90FlipXY);


                
                //save the image out to the file
                img.Save(StaticKeys.BackofficeTempPath + "rotate_" + name);

                //release image file
                img.Dispose();

                return StaticKeys.BackofficeTempUrl + "rotate_" + name;
                //return fileName;
            }
            catch (Exception ex)
            {
                if (img != null)
                    img.Dispose();

                return "ERROR";
            }
        }
        
        #region PERMITTED EXTENSIONS





        public static bool ImageExtensionIsPermitted(string p_Ext)
        {
            string[] exts = StaticKeys.ImageExtensionsSupported.Split(';');
            bool permitted = false;

            for (int i = 0; i < exts.Length; i++)
            {

                if (exts[i].ToLower() == p_Ext.ToLower())
                {
                    permitted = true;
                    break;
                }
            }

            return permitted;
        }

        //public static bool VideoExtensionIsPermited(string p_Ext)
        //{
        //    string[] exts = StaticKeys.VideoExtensionsSupported.Split(';');
        //    bool permitted = false;

        //    for (int i = 0; i < exts.Length; i++)
        //    {

        //        if (exts[i].ToLower() == p_Ext.ToLower())
        //        {
        //            permitted = true;
        //            break;
        //        }
        //    }


        //    return permitted;
        //}

        //public static bool AudioExtensionIsPermited(string p_Ext)
        //{
        //    string[] exts = StaticKeys.AudioExtensionsSupported.Split(';');
        //    bool permitted = false;

        //    for (int i = 0; i < exts.Length; i++)
        //    {

        //        if (exts[i].ToLower() == p_Ext.ToLower())
        //        {
        //            permitted = true;
        //            break;
        //        }
        //    }


        //    return permitted;
        //}
        //public static bool MbTilesExtensionIsPermited(string p_Ext)
        //{
        //    string[] exts = StaticKeys.MbTilesExtensionsSupported.Split(';');
        //    bool permitted = false;

        //    for (int i = 0; i < exts.Length; i++)
        //    {

        //        if (exts[i].ToLower() == p_Ext.ToLower())
        //        {
        //            permitted = true;
        //            break;
        //        }
        //    }


        //    return permitted;
        //}


        public static string GetMimeTypeFromExtension(string p_Ext)
        {
            string[] apps = StaticKeys.MultimediaMimeTypes.Split('|');

            for (int i = 0; i < apps.Length; i++)
            {
                string[] mt = apps[i].Split('=');

                if (mt[1].ToLower() == p_Ext.ToLower())
                {
                    return mt[0];
                }
            }

            return null;
        }

        #endregion


        #region STRING METHODS
        public static string ReplaceTag(string content, string tag, string valueToReplace)
        {
            return content.Replace("<!--@" + tag + "-->", valueToReplace);
        }
        #endregion


        #region QR CODE

        private static char[] alphabetArray = { 'a', 'b', 'c', 'd', 'e', 
											    'f', 'g', 'h', 'i', 'j', 
											    'k', 'l', 'm', 'n', 'o', 
											    'p', 'q', 'r', 's', 't', 
											    'u', 'v', 'w', 'y', 'z' };


        public static string GenerateRandomCode(int numChars, bool upperCase)
        {
            Random random = new Random();

            int n = 0;
            string input = "";

            for (int i = 0; i < numChars; i++)
            {
                n = random.Next(0, alphabetArray.Length - 1);

                if (upperCase)
                    input += alphabetArray[n].ToString().ToUpper();
                else
                    input += alphabetArray[n];

            }

            return input;
        }
        #endregion


        #region THUMBNAILS

        public static System.Drawing.Bitmap ResizeImageWithouAdapt(byte[] oldimg, int width, int height)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream(oldimg);

            System.Drawing.Image image = System.Drawing.Image.FromStream(ms);

            Size szReal = new Size(image.Width, image.Height);
            Size szMax = new Size(width, height);
            Size p = adaptProportionalSizeWidth(szMax, szReal);

            //a holder for the result 
            Bitmap result = new Bitmap(p.Width, p.Height);

            //use a graphics object to draw the resized image into the bitmap 
            using (Graphics graphics = Graphics.FromImage(result))
            {
                ////set the resize quality modes to high quality 
                //graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                //graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                //graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                var attribute = new ImageAttributes();

                attribute.SetWrapMode(WrapMode.TileFlipXY);


                //draw the image into the target bitmap 
                //graphics.DrawImage(image, 0, 0, result.Width, result.Height, GraphicsUnit.Pixel, attribute);

                graphics.DrawImage(image, new Rectangle(new Point(0, 0), p), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, attribute);


            }

            //return the resulting bitmap 
            return result;
        }

        #region ADAPT PORPORTINAL SIZE
        private static Size adaptProportionalSizeWidth(Size szMax, Size szReal)
        {
            int nWidth;
            int nHeight;
            double sMaxRatio;
            double sRealRatio;

            if (szMax.Width < 1 || szMax.Height < 1 || szReal.Width < 1 || szReal.Height < 1)
                return Size.Empty;

            sMaxRatio = (double)szMax.Width / (double)szMax.Height;
            sRealRatio = (double)szReal.Width / (double)szReal.Height;

            nWidth = Math.Min(szMax.Width, szReal.Width);
            nHeight = (int)Math.Round(nWidth / sRealRatio);


            return new Size(nWidth, nHeight);
        }
        #endregion


        //public static Bitmap ResizeBitmap(Bitmap originalBitmap, int requiredHeight, int requiredWidth)
        //{
        //    int[] heightWidthRequiredDimensions;

        //    // Pass dimensions to worker method depending on image type required 
        //    heightWidthRequiredDimensions = WorkDimensions(originalBitmap.Height, originalBitmap.Width, requiredHeight, requiredWidth);


        //    Bitmap resizedBitmap = new Bitmap(heightWidthRequiredDimensions[1],
        //                                       heightWidthRequiredDimensions[0]);

        //    const float resolution = 72;

        //    resizedBitmap.SetResolution(resolution, resolution);

        //    Graphics graphic = Graphics.FromImage((Image)resizedBitmap);

        //    graphic.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
        //    graphic.DrawImage(originalBitmap, 0, 0, resizedBitmap.Width, resizedBitmap.Height);

        //    graphic.Dispose();
        //    originalBitmap.Dispose();
        //    //resizedBitmap.Dispose(); // Still in use 


        //    return resizedBitmap;
        //}


        //private static int[] WorkDimensions(int originalHeight, int originalWidth, int requiredHeight, int requiredWidth)
        //{
        //    int imgHeight = 0;
        //    int imgWidth = 0;

        //    imgWidth = requiredHeight;
        //    imgHeight = requiredWidth;


        //    int requiredHeightLocal = originalHeight;
        //    int requiredWidthLocal = originalWidth;

        //    double ratio = 0;

        //    // Check height first 
        //    // If original height exceeds maximum, get new height and work ratio. 
        //    if (originalHeight > imgHeight)
        //    {
        //        ratio = double.Parse(((double)imgHeight / (double)originalHeight).ToString());
        //        requiredHeightLocal = imgHeight;
        //        requiredWidthLocal = (int)((decimal)originalWidth * (decimal)ratio);
        //    }

        //    // Check width second. It will most likely have been sized down enough 
        //    // in the previous if statement. If not, change both dimensions here by width. 
        //    // If new width exceeds maximum, get new width and height ratio. 
        //    if (requiredWidthLocal >= imgWidth)
        //    {
        //        ratio = double.Parse(((double)imgWidth / (double)originalWidth).ToString());
        //        requiredWidthLocal = imgWidth;
        //        requiredHeightLocal = (int)((double)originalHeight * (double)ratio);
        //    }

        //    int[] heightWidthDimensionArr = { requiredHeightLocal, requiredWidthLocal };

        //    return heightWidthDimensionArr;
        //}






        //public static void GetThumb2(string imagePath, string dest, int thumbWidth, int thumbHeight)
        //{
        //    System.Drawing.Image image = System.Drawing.Image.FromFile(imagePath);
        //    int srcWidth = image.Width;
        //    int srcHeight = image.Height;
        //    //int thumbHeight = (srcHeight / srcWidth) * thumbWidth;


        //    Size szMax = new Size(thumbWidth, thumbHeight);
        //    Size szReal = new Size(image.Width, image.Height);
        //    Size p = adaptProportionalSize(szMax, szReal);



        //    Bitmap bmp = new Bitmap(p.Width, p.Height);

        //    System.Drawing.Graphics gr = System.Drawing.Graphics.FromImage(bmp);
        //    gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
        //    gr.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
        //    gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

        //    System.Drawing.Rectangle rectDestination = new System.Drawing.Rectangle(0, 0, thumbWidth, thumbHeight);
        //    gr.DrawImage(image, rectDestination, 0, 0, srcWidth, srcHeight, GraphicsUnit.Pixel);

        //    bmp.Save(dest);

        //    bmp.Dispose();
        //    image.Dispose();
        //}


        //#region GET THUMBNAIL

        //public static byte[] GetThumb(byte[] oldimg, int newh, int neww)
        //{

        //    System.IO.MemoryStream ms = new System.IO.MemoryStream(oldimg);

        //    System.Drawing.Image img = System.Drawing.Image.FromStream(ms);

        //    Size szMax = new Size(neww, newh);
        //    Size szReal = new Size(img.Width, img.Height);
        //    Size p = adaptProportionalSize(szMax, szReal);


        //    System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(p.Width, p.Height);
        //    System.Drawing.Graphics graph = System.Drawing.Graphics.FromImage(bmp);
        //    graph.DrawImage(img, 0, 0, p.Width, p.Height);
        //    System.IO.MemoryStream msnew = new System.IO.MemoryStream();
        //    System.Drawing.Imaging.EncoderParameters encparams = new System.Drawing.Imaging.EncoderParameters(1);
        //    encparams.Param[0] = new System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 100L);




        //    System.Drawing.Imaging.ImageCodecInfo codecJpeg = GetEncoderInfo("image/jpeg");
        //    bmp.Save(msnew, codecJpeg, encparams);


        //    byte[] arr = msnew.ToArray();
        //    msnew.Dispose();
        //    bmp.Dispose();
        //    img.Dispose();
        //    return arr;



        //}
        //#endregion


        //#region ADAPT PORPORTINAL SIZE
        //private static Size adaptProportionalSize(Size szMax, Size szReal)
        //{
        //    int nWidth;
        //    int nHeight;
        //    double sMaxRatio;
        //    double sRealRatio;

        //    if (szMax.Width < 1 || szMax.Height < 1 || szReal.Width < 1 || szReal.Height < 1)
        //        return Size.Empty;

        //    sMaxRatio = (double)szMax.Width / (double)szMax.Height;
        //    sRealRatio = (double)szReal.Width / (double)szReal.Height;

        //    if (sMaxRatio < sRealRatio)
        //    {
        //        nWidth = Math.Min(szMax.Width, szReal.Width);
        //        nHeight = (int)Math.Round(nWidth / sRealRatio);
        //    }
        //    else
        //    {
        //        nHeight = Math.Min(szMax.Height, szReal.Height);
        //        nWidth = (int)Math.Round(nHeight * sRealRatio);
        //    }
        //    return new Size(nWidth, nHeight);
        //}
        //#endregion

        //#region GET ENCODER INFO
        //private static System.Drawing.Imaging.ImageCodecInfo GetEncoderInfo(string mimeType)
        //{
        //    // Get image codecs for all image formats 
        //    System.Drawing.Imaging.ImageCodecInfo[] codecs = System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders();

        //    // Find the correct image codec 
        //    for (int i = 0; i < codecs.Length; i++)
        //        if (codecs[i].MimeType == mimeType)
        //            return codecs[i];
        //    return null;
        //}
        //#endregion

        #endregion


        public static string ValidateEmail(string email)
        {
            if (String.IsNullOrEmpty(email))
            {
                return "";
            }

            //string strRegex = @"^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$";

            //string strRegex = @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
            //string strRegex = @"\b[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}\b";

            // define a regular expression for "normal" addresses
            string strRegex = @"^[a-z0-9_\+-]+(\.[a-z0-9_\+-]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*\.([a-z]{2,4})$";


            Regex re = new Regex(strRegex);

            if (!re.IsMatch(email))
            {
                return "";
            }

            return email;
        }
        public static string RemoveCharactersForUrl(string str)
        {
            if (String.IsNullOrEmpty(str))
            {
                return "";
            }

            str = str.Replace("/", "");
            str = str.Replace(@"\", "");
            str = str.Replace("/", "");
            str = str.Replace("<", "");
            str = str.Replace(">", "");
            str = str.Replace(" ", "_");
            str = str.Replace("\"", "");

            str = str.Replace("´", "");
            str = str.Replace("~", "");
            str = str.Replace("`", "");
            str = str.Replace("^", "");

            str = str.Replace("á", "a");
            str = str.Replace("Á", "A");
            str = str.Replace("à", "a");
            str = str.Replace("À", "A");
            str = str.Replace("ã", "a");
            str = str.Replace("Ã", "A");
            str = str.Replace("â", "a");
            str = str.Replace("Â", "A");

            str = str.Replace("é", "e");
            str = str.Replace("É", "E");
            str = str.Replace("è", "e");
            str = str.Replace("È", "E");
            str = str.Replace("ê", "e");
            str = str.Replace("Ê", "E");

            str = str.Replace("í", "i");
            str = str.Replace("Í", "I");
            str = str.Replace("ì", "i");
            str = str.Replace("Ì", "I");
            str = str.Replace("î", "i");
            str = str.Replace("Î", "I");

            str = str.Replace("ó", "o");
            str = str.Replace("Ó", "o");
            str = str.Replace("Ò", "O");
            str = str.Replace("ò", "o");
            str = str.Replace("õ", "o");
            str = str.Replace("Õ", "O");
            str = str.Replace("ô", "o");
            str = str.Replace("Ô", "O");

            str = str.Replace("ú", "u");
            str = str.Replace("Ú", "U");
            str = str.Replace("ù", "u");
            str = str.Replace("Ù", "u");
            str = str.Replace("û", "u");
            str = str.Replace("Û", "u");

            str = str.Replace("ç", "c");
            str = str.Replace("Ç", "C");
            str = str.Replace("-", "");
            str = str.Replace("&", "");
            str = str.Replace("<", "");
            str = str.Replace(">", "");
            str = str.Replace("/", "");
            str = str.Replace(@"\", "");
            str = str.Replace(":", "");
            str = str.Replace("#", "");
            str = str.Replace("$", "");
            str = str.Replace("º", "");
            str = str.Replace("ª", "");
            str = str.Replace(",", "");
            str = str.Replace(".", "");
            str = str.Replace("%", "");
            str = str.Replace("(", "");
            str = str.Replace(")", "");
            str = str.Replace("[", "");
            str = str.Replace("]", "");
            str = str.Replace("{", "");
            str = str.Replace("}", "");
            str = str.Replace("?", "");
            str = str.Replace("'", "");
            str = str.Replace("\"", "");
            str = str.Replace("&", "");
            str = str.Replace("=", "");
            str = str.Replace("«", "");
            str = str.Replace("»", "");
            str = str.Replace("~", "");
            str = str.Replace("^", "");
            str = str.Replace("+", "");
            str = str.Replace("*", "");
            str = str.Replace("!", "");
            str = str.Replace("|", "");
            str = str.Replace("€", "");
            str = str.Replace("'", "");
            str = str.Replace("@", "");
            str = str.Replace("$", "");
            str = str.Replace("*", "");
            str = str.Replace("+", "");
            str = str.Replace("«", "");
            str = str.Replace("»", "");
            str = str.Replace(@"\", "");
            str = str.Replace("/", "");
            str = str.Replace("@", "");

            string s = str;

            Match m = Regex.Match(str, @"[^\w]");

            while (m.Success)
            {
                s = s.Replace(m.Value, "");

                m = m.NextMatch();
            }

            return s;
        }


        public static string RemoveCharactersForFileName(string str)
        {
            if (String.IsNullOrEmpty(str))
            {
                return "";
            }

            str = str.Replace("/", "");
            str = str.Replace(@"\", "");
            str = str.Replace("/", "");
            str = str.Replace("<", "");
            str = str.Replace(">", "");
            str = str.Replace(" ", "_");
            str = str.Replace("\"", "");

            str = str.Replace("´", "");
            str = str.Replace("~", "");
            str = str.Replace("`", "");
            str = str.Replace("^", "");

            str = str.Replace("á", "a");
            str = str.Replace("Á", "A");
            str = str.Replace("à", "a");
            str = str.Replace("À", "A");
            str = str.Replace("ã", "a");
            str = str.Replace("Ã", "A");
            str = str.Replace("â", "a");
            str = str.Replace("Â", "A");

            str = str.Replace("é", "e");
            str = str.Replace("É", "E");
            str = str.Replace("è", "e");
            str = str.Replace("È", "E");
            str = str.Replace("ê", "e");
            str = str.Replace("Ê", "E");

            str = str.Replace("í", "i");
            str = str.Replace("Í", "I");
            str = str.Replace("ì", "i");
            str = str.Replace("Ì", "I");
            str = str.Replace("î", "i");
            str = str.Replace("Î", "I");

            str = str.Replace("ó", "o");
            str = str.Replace("Ó", "o");
            str = str.Replace("Ò", "O");
            str = str.Replace("ò", "o");
            str = str.Replace("õ", "o");
            str = str.Replace("Õ", "O");
            str = str.Replace("ô", "o");
            str = str.Replace("Ô", "O");

            str = str.Replace("ú", "u");
            str = str.Replace("Ú", "U");
            str = str.Replace("ù", "u");
            str = str.Replace("Ù", "u");
            str = str.Replace("û", "u");
            str = str.Replace("Û", "u");

            str = str.Replace("ç", "c");
            str = str.Replace("Ç", "C");
            str = str.Replace("-", "");
            str = str.Replace("&", "");
            str = str.Replace("<", "");
            str = str.Replace(">", "");
            str = str.Replace("/", "");
            str = str.Replace(@"\", "");
            str = str.Replace(":", "");
            str = str.Replace("#", "");
            str = str.Replace("$", "");
            str = str.Replace("º", "");
            str = str.Replace("ª", "");
            str = str.Replace(",", "");

            str = str.Replace("%", "");
            str = str.Replace("(", "");
            str = str.Replace(")", "");
            str = str.Replace("[", "");
            str = str.Replace("]", "");
            str = str.Replace("{", "");
            str = str.Replace("}", "");
            str = str.Replace("?", "");
            str = str.Replace("'", "");
            str = str.Replace("\"", "");
            str = str.Replace("&", "");
            str = str.Replace("=", "");
            str = str.Replace("«", "");
            str = str.Replace("»", "");
            str = str.Replace("~", "");
            str = str.Replace("^", "");
            str = str.Replace("+", "");
            str = str.Replace("*", "");
            str = str.Replace("!", "");
            str = str.Replace("|", "");
            str = str.Replace("€", "");
            str = str.Replace("'", "");
            str = str.Replace("@", "");
            str = str.Replace("$", "");
            str = str.Replace("*", "");
            str = str.Replace("+", "");
            str = str.Replace("«", "");
            str = str.Replace("»", "");
            str = str.Replace(@"\", "");
            str = str.Replace("/", "");
            str = str.Replace("@", "");

            //string s = str;

            //Match m = Regex.Match(str, @"[^\w]");

            //while (m.Success)
            //{
            //    s = s.Replace(m.Value, "");

            //    m = m.NextMatch();
            //}

            return str;
        }



        #region JSON

        // has to have total recosrds in firts datatable as param cont
        public static string JsonForDataTable(string echo, DataSet ds, int pageSize, int page, bool addLink)
        {

            int totalRecords = 0;
            if (UtilMethods.DataSetHasData(ds))
            {

                if (ds.Tables.Count > 1)
                {
                    totalRecords = Convert.ToInt32(ds.Tables[0].Rows[0]["cont"]);

                    #region JSON BUILDER
                    StringBuilder jsonBuilder = new StringBuilder();
                    DataTable dt = ds.Tables[1];

                    jsonBuilder.Append("{");
                    jsonBuilder.Append("\"sEcho\": " + echo + ", \"iTotalRecords\":" + totalRecords + ",\"iTotalDisplayRecords\":" + totalRecords + ",\"aaData\"");
                    jsonBuilder.Append(":[");
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            //jsonBuilder.Append("{\"i\":" + (i) + ",\"cell\":[");
                            jsonBuilder.Append("{");

                            jsonBuilder.Append("\"DT_RowId\":\"" + dt.Rows[i]["ID"].ToString() + "\", ");
                            for (int j = 0; j < dt.Columns.Count; j++)
                            {
                                jsonBuilder.Append("\"");
                                jsonBuilder.Append(dt.Columns[j].ColumnName);
                                jsonBuilder.Append("\"");
                                jsonBuilder.Append(":");
                                jsonBuilder.Append("\"");

                                string val = dt.Rows[i][j].ToString();
                                val = val.Replace("\r\n", " ");
                                val = val.Replace("\n", " ");
                                val = val.Replace("\r", " ");
                                val = val.Replace("\t", " ");
                                val = val.Replace("\"", "'");

                                jsonBuilder.Append(val);
                                jsonBuilder.Append("\",");
                            }


                            if (addLink)
                            {
                                jsonBuilder.Append("\"");
                                jsonBuilder.Append("Check");
                                jsonBuilder.Append("\"");
                                jsonBuilder.Append(":");
                                jsonBuilder.Append("\"");
                                // GARANTIR QUE A COLUNA SE CHAMA ID
                                jsonBuilder.Append(dt.Rows[i]["ID"] + "");

                                jsonBuilder.Append("\",");
                            }
                            jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
                            jsonBuilder.Append("},");
                        }
                    }
                    else
                    {
                        jsonBuilder.Append("[");
                    }
                    jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
                    jsonBuilder.Append("]");
                    jsonBuilder.Append("}");
                    return jsonBuilder.ToString();


                    #endregion

                }
                else
                    return "";
            }
            else
                return "";


        }

        #endregion
    }
        

}
