using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;


using XLabs.Ioc;
using XLabs.Platform.Device;
using XLabs.Platform.Services;
using XLabs.Platform.Services.Geolocation;
using Xamarin.Forms;

namespace PinFixed.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //

        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            //DependencyService.Register<Geolocator>();
            //DependencyService.Register<ImageServiceIOS>();

            //#region Resolver Init
            //if (!Resolver.IsSet)
            //{
            //    SimpleContainer container = new SimpleContainer();
            //    container.Register<IDevice>(t => AppleDevice.CurrentDevice);
            //    container.Register<IDisplay>(t => t.Resolve<IDevice>().Display);
            //    container.Register<INetwork>(t => t.Resolve<IDevice>().Network);
            //    container.Register<Geolocator>(t => t.Resolve<Geolocator>());
            //    Resolver.SetResolver(container.GetResolver());
            //}
            //#endregion

            if (!Resolver.IsSet)
            {
               

                DependencyService.Register<Geolocator>();
                DependencyService.Register<ImageServiceIOS>();

                SimpleContainer container = new SimpleContainer();
                container.Register<IDevice>(t => AppleDevice.CurrentDevice);
                container.Register<IDisplay>(t => t.Resolve<IDevice>().Display);
                container.Register<INetwork>(t => t.Resolve<IDevice>().Network);

                container.Register<Geolocator>(t => t.Resolve<Geolocator>());

                //container.Register<IGeolocator, global::XLabs.Platform.Services.Geolocation.Geolocator>();
                Resolver.SetResolver(container.GetResolver());
            }

            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }
    }
}
