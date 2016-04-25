using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using XLabs.Ioc;
using XLabs.Platform.Device;
using XLabs.Platform.Services;
//using PinFixed.Droid.fix.bitcliq.com;
using PinFixed;
using XLabs.Platform.Services.Geolocation;
using Xamarin.Forms;
using XLabs.Platform.Services.Media;
using XLabs.Platform.Services.IO;
using XLabs.Forms;
using Android.Gms.Maps;

namespace PinFixed.Droid
{
    [Activity(Label = "ByCatch ID", Icon = "@drawable/icon", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, WindowSoftInputMode = SoftInput.AdjustResize)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {

        public override void OnBackPressed()
        {
            Fragment fragment = this.FragmentManager.FindFragmentById(Resource.Id.mapTest);
            if (fragment != null)
            {
                if (fragment.IsResumed)
                {
                    this.FragmentManager.BeginTransaction().Remove(fragment).Commit();

                }
            }


            base.OnBackPressed();
           
        }
        
        protected override void OnCreate(Bundle bundle)
        {
            try
            {
                base.OnCreate(bundle);
               
                #region Resolver Init
                if (!Resolver.IsSet)
                {
                    Xamarin.FormsMaps.Init(this, bundle);
                    DependencyService.Register<Geolocator>();
                    DependencyService.Register<ImageServiceDroid>();

                    SimpleContainer container = new SimpleContainer();
                    container.Register<IDevice>(t => AndroidDevice.CurrentDevice);
                    container.Register<IDisplay>(t => t.Resolve<IDevice>().Display);
                    container.Register<INetwork>(t => t.Resolve<IDevice>().Network);

                    container.Register<Geolocator>(t => t.Resolve<Geolocator>());

                    //container.Register<IGeolocator, global::XLabs.Platform.Services.Geolocation.Geolocator>();
                    Resolver.SetResolver(container.GetResolver());
                }

                


                #endregion

                global::Xamarin.Forms.Forms.Init(this, bundle);
                LoadApplication(new App());
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }




        //protected override void OnPostResume()
        //{
        //    base.OnPostResume();

        //    Fragment fragment = this.FragmentManager.FindFragmentByTag("mp");
        //    if (fragment != null)
        //    {
        //        if (fragment.IsResumed)
        //        {
        //            this.FragmentManager.BeginTransaction().Remove(fragment).Commit();
        //        }
        //    }
        //    //Fragment fragment2 = this.FragmentManager.FindFragmentByTag("bt");
        //    //if (fragment2 != null)
        //    //{
        //    //    if (fragment2.IsResumed)
        //    //    {
        //    //        this.FragmentManager.BeginTransaction().Remove(fragment2).Commit();
        //    //    }
        //    //}


        //}

        //protected override void OnResume()
        //{
        //    base.OnResume();
        //}

        //protected override void OnDestroy()
        //{




        //    Fragment fragment = this.FragmentManager.FindFragmentByTag("mp");
        //    if (fragment != null)
        //    {
        //        if (fragment.IsResumed)
        //        {
        //            this.FragmentManager.BeginTransaction().Remove(fragment).Commit();
        //        }
        //    }
        //    Fragment fragment2 = this.FragmentManager.FindFragmentByTag("bt");
        //    if (fragment2 != null)
        //    {
        //        if (fragment2.IsResumed)
        //        {
        //            this.FragmentManager.BeginTransaction().Remove(fragment2).Commit();
        //        }
        //    }

        //    base.OnDestroy();
        //}

        //protected override void OnStop()
        //{
        //    base.OnStop();
        //}

        


    }
}

