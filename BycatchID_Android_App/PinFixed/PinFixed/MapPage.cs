using PinFixed.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using XLabs.Platform.Services.Media;

namespace PinFixed
{
    public class MapPage : ContentPage
    {
        //Map map;


       
        GPSUtils _gps;

        public GPSUtils GPS
        {
            get { return _gps; }
        }

        //MediaFile _mediaFile;
         Position? _pos;
         public Position? Pos
         {
             get { return _pos; }
         }


         Position? _posImageFromLibrary;
         public Position? PosImageFromLibrary
         {
             get { return _posImageFromLibrary; }
         }
        byte[] _resizedImage;
        public MapPage(byte[] resizedImage, GPSUtils gps, Position? pos, Position? posImageFromLibrary)
		{
            NavigationPage.SetHasNavigationBar(this, false);


            this._gps = gps;
            this._resizedImage = resizedImage;
            this._pos = pos;
            this._posImageFromLibrary = posImageFromLibrary;
           

		}


        public void SetCoords(double Latitude, double Longitude)
        {

            Navigation.PushAsync(new ReportScreen(_resizedImage, null, _gps, Latitude, Longitude));
        }


        public void GoHome()
        {

            Navigation.PushAsync(new StartPage(_gps));
        }

        public void GoBack()
        {

            Navigation.PushAsync(new TakePictureScreen(_gps));
        }
    }
       
}
