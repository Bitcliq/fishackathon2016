
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;

namespace PinFixed.Droid
{
    [Activity(MainLauncher = true,   NoHistory = true, Theme = "@style/Theme.Splash", 
		ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            //SetContentView(Resource.Layout.Splash);

            //Thread.Sleep(10000); // Simulate a long loading process on app startup.
            //this.StartActivity(typeof(MainActivity));
            var intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);
            Finish();



        }
    }
 
}