using PinFixed.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using Xamarin.Forms;
using XLabs.Forms.Mvvm;



namespace PinFixed
{
    public class Home : ContentPage
    {
        public Home(GPSUtils gps)
        {

            //ViewFactory.Register<GeolocatorPage, GeolocatorViewModel>();

            //Navigation.PushAsync(ViewFactory.CreatePage<GeolocatorViewModel, Page>() as Page);
            


            Title = "PinFix(ed)";
            var btn = new Button { Text = "Reportar Issue" };
            btn.Clicked += (s, e) => Navigation.PushAsync(new TakePictureScreen(gps));

            Content = new StackLayout
            {
                Children = {
					btn
				}
            };
        }
    }
}
