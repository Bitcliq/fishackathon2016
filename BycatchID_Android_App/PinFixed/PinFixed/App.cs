using PinFixed.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using XLabs.Forms.Controls;


namespace PinFixed
{
    public class App : Application
    {

        public static INavigation Navigation { get; set; }

        public App()
        {

         
               
                GPSUtils gps = new GPSUtils();
          
               
                MainPage = new NavigationPage(new StartPage(gps));
                Navigation = MainPage.Navigation;
     
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            try
            {
                // Handle when your app resumes
                base.OnResume();
            }
            catch (Exception ex)
            {
                //throw (ex);
            }
        }

        
    }
}
