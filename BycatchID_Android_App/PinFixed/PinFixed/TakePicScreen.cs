using PinFixed.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace PinFixed
{
    public class TakePicScreen : ContentPage
    {
        private GPSUtils _gps;
        public TakePicScreen(GPSUtils gps)
        {
            _gps = gps;

        }

        public async void SetCoordinatesAndImage(byte[] img, string latStr, string latRef, string lonStr, string longRef)
        {

            //if (newIssue.Latitude != null && newIssue.Longitude != null)
            //{
            //    string latStr = newIssue.Latitude + "";

            //    string lonStr = newIssue.Longitude + "";
            decimal? lat = null;
            decimal? ln = null;
            try
            {
                lat = Convert.ToDecimal(latStr.Replace(".", ","));
                ln = Convert.ToDecimal(lonStr.Replace(".", ","));

            }
            catch(Exception)
            {
                

            }


            if (latRef == "S")
            {
                if (lat != null)
                    lat = -lat;

            }
            if (longRef == "W")
            {
                if (ln != null)
                    ln = -ln;

            }



            if (_gps != null)
                await _gps.GetPosition();
            else
            {
                _gps = new GPSUtils();
                await _gps.GetPosition();
            }
                

            if (latStr + "" != "" && lonStr + "" != "")
            {
                Position pos = new Position(Convert.ToDouble(lat), Convert.ToDouble(ln));


               await Navigation.PushAsync(new MapPage(img, _gps, null, pos));

                //Navigation.PushAsync(new ReportScreen(img, null, null, Convert.ToDouble(lat), Convert.ToDouble(ln)));

            }
            else
            {

               await Navigation.PushAsync(new MapPage(img, _gps, null, null));
            }

        }
    }

    
}
