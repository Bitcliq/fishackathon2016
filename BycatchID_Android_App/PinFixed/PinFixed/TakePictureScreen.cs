using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XLabs.Forms;
using XLabs.Forms.Controls;
using XLabs.Ioc;
using XLabs.Platform.Device;
using XLabs.Platform.Services.Media;
using Xamarin.Forms.Maps;
using PinFixed.BL;
using System.IO;
//using Refractored.Xam.Settings;
using XLabs.Enums;
using Plugin.Settings;
namespace PinFixed
{
    public class TakePictureScreen : ContentPage
    {
        /// <summary>
        /// The device
        /// </summary>
        private IDevice _device;

        /// <summary>
        /// The media picker
        /// </summary>
        private IMediaPicker _mediaPicker;

        /// <summary>
        /// The scheduler
        /// </summary>
        private readonly TaskScheduler _scheduler = TaskScheduler.FromCurrentSynchronizationContext();




        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        public string Status
        {
            get;
            set;
        }


        private MediaFile mediaFile;

        //private ImageSource _imageSource;
        private RoundButton buttonTakePicture;
        private RoundButton buttonSelectPicture;
        private GPSUtils _gps;

        private byte[] resizedImage;


        double[] lat;
        string latRef;
        double[] lon;
        string lonRef;


        protected override bool OnBackButtonPressed()
        {
            buttonTakePicture.IsEnabled = true;
            buttonSelectPicture.IsEnabled = true;
            return base.OnBackButtonPressed();
        }


        ActivityIndicator activityIndicator;




        public TakePictureScreen(GPSUtils gps)
        {

            var grid = new Grid();

            grid.VerticalOptions = LayoutOptions.FillAndExpand;
            grid.HorizontalOptions = LayoutOptions.FillAndExpand;



            activityIndicator = new ActivityIndicator
            {

                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Color = Color.White,
                IsVisible = false
            };



            if (Device.OS == TargetPlatform.Android)
            {
                activityIndicator.HorizontalOptions = LayoutOptions.Center;
                activityIndicator.VerticalOptions = LayoutOptions.Center;
            }



            this.BackgroundImage = "bg_home.png";

            _gps = gps;

            NavigationPage.SetHasNavigationBar(this, false);
            var relativeLayout = new RelativeLayout();

            //relativeLayout.Padding = new Thickness(0, Device.OnPlatform(20, 0, 0), 0, 0);


            // relativeLayout.BackgroundColor = Color.Red;
            #region BUTTON BACK

            if (Device.OS == TargetPlatform.iOS)
            {
                ImageButton b = new ImageButton();
                b.Text = "   ";
                b.Orientation = ImageOrientation.ImageToLeft;


                b.ImageHeightRequest = 50;
                b.ImageWidthRequest = 50;


                b.Clicked += (sender, args) =>
                {
                    Navigation.PushAsync(new StartPage(gps));
                };

                b.Image = "back.png";
                b.Source = ImageSource.FromFile("back.png");

                relativeLayout.Children.Add(b, Constraint.RelativeToParent((parent) =>
                {
                    return 0;
                }));

            }
            else
            {
                var homeTapRecognizer = new TapGestureRecognizer
                {
                    Command = new Command(() =>
                    {
                        Navigation.PushAsync(new StartPage(gps));
                        //Navigation.PopAsync();
                    }),
                    NumberOfTapsRequired = 1
                };

                var homeImg = new Image();
                homeImg.Source = "back.png";




                homeImg.GestureRecognizers.Add(homeTapRecognizer);
                relativeLayout.Children.Add(homeImg, Constraint.RelativeToParent((parent) =>
                {
                    return 0;
                }), Constraint.Constant(10));

            }
            #endregion



            int idAux = CrossSettings.Current.GetValueOrDefault(StaticKeys.APP_PREFIX + "repid", 0);

            #region BUTTON LISTA


            if (Device.OS == TargetPlatform.iOS)
            {
                ImageButton b = new ImageButton();
                b.Text = "   ";
                b.Orientation = ImageOrientation.ImageToLeft;


                b.ImageHeightRequest = 50;
                b.ImageWidthRequest = 50;


                b.Clicked += (sender, args) =>
                {
                    if (idAux > 0)
                    {
                        GetDataAndGotoList();
                    }
                    else
                    {
                        DisplayAlert(StaticKeys.INFO_TITLE, StaticKeys.NO_REPORTER_DATA, StaticKeys.OK_BTN_TITLE);
                    }
                };

                b.Image = "btn_lista.png";
                b.Source = ImageSource.FromFile("btn_lista.png");
                Func<RelativeLayout, double> getWitdh = (parent) => b.GetSizeRequest(relativeLayout.Width, relativeLayout.Height).Request.Width;
                relativeLayout.Children.Add(b,
                    Constraint.RelativeToParent((r) => r.Width - getWitdh(r)),
                    Constraint.Constant(0));

            }
            else
            {
                var profileTapRecognizer = new TapGestureRecognizer
                {
                    Command = new Command(() =>
                    {
                        if (idAux > 0)
                        {
                            GetDataAndGotoList();
                        }
                        else
                        {
                            DisplayAlert(StaticKeys.INFO_TITLE, StaticKeys.NO_REPORTER_DATA, StaticKeys.OK_BTN_TITLE);
                        }
                    }),
                    NumberOfTapsRequired = 1
                };

                Image img = new Image();
                img.Source = "btn_lista.png";

                img.GestureRecognizers.Add(profileTapRecognizer);


                if (Device.OS == TargetPlatform.iOS)
                {
                    img.WidthRequest = 35;
                    img.HeightRequest = 35;

                }

                Func<RelativeLayout, double> getWitdh = (parent) => img.GetSizeRequest(relativeLayout.Width, relativeLayout.Height).Request.Width;
                relativeLayout.Children.Add(img,
                    Constraint.RelativeToParent((r) => r.Width - getWitdh(r)),
                    Constraint.Constant(10));

            }
            #endregion










            #region BUTTON NEAR BY

            double lat1 = 0, lon1 = 0;
            bool canGetData = false;
            if (_gps != null)
            {
                if (_gps.PositionLatitude + "" != "")
                {
                    canGetData = true;


                    try
                    {
                        lat1 = Convert.ToDouble(_gps.PositionLatitude);
                    }
                    catch
                    {
                    }


                    try
                    {
                        lon1 = Convert.ToDouble(_gps.PositionLongitude);

                    }
                    catch
                    {
                    }

                }
                else
                    _gps.GetPosition();
            }


            #region ANDROID / IOS
            if (Device.OS == TargetPlatform.iOS)
            {
                ImageButton b = new ImageButton();
                b.Text = "   ";
                b.Orientation = ImageOrientation.ImageToLeft;


                b.ImageHeightRequest = 50;
                b.ImageWidthRequest = 50;


                b.Clicked += (sender, args) =>
                {



                    if (canGetData)
                    {

                        Navigation.PushAsync(new ListNearByIssues(_gps, lat1, lon1));
                    }
                    else
                        DisplayAlert(StaticKeys.INFO_TITLE, StaticKeys.NO_LOCATION, StaticKeys.OK_BTN_TITLE);
                };

                b.Image = "icone_perto_de_mim.png";
                b.Source = ImageSource.FromFile("icone_perto_de_mim.png");




                Func<RelativeLayout, double> getImgWidth = (parent) => b.GetSizeRequest(relativeLayout.Width, relativeLayout.Height).Request.Width;
                relativeLayout.Children.Add(b,
                    Constraint.RelativeToParent((r) => r.Width - getImgWidth(r) - getImgWidth(r) - 10),
                    Constraint.Constant(0));



            }
            else
            {
                var profileTapRecognizer = new TapGestureRecognizer
                {
                    Command = new Command(() =>
                    {
                        if (canGetData)
                        {

                            Navigation.PushAsync(new ListNearByIssues(_gps, lat1, lon1));
                        }
                        else
                            DisplayAlert(StaticKeys.INFO_TITLE, StaticKeys.NO_LOCATION, StaticKeys.OK_BTN_TITLE);
                    }),
                    NumberOfTapsRequired = 1
                };

                Image img = new Image();
                img.Source = "icone_perto_de_mim.png";

                img.GestureRecognizers.Add(profileTapRecognizer);






                Func<RelativeLayout, double> getImgWidth = (parent) => img.GetSizeRequest(relativeLayout.Width, relativeLayout.Height).Request.Width;
                relativeLayout.Children.Add(img,
                    Constraint.RelativeToParent((r) => r.Width - getImgWidth(r) - getImgWidth(r) - 10),
                    Constraint.Constant(10));
            }

            #endregion


            #endregion


            //Padding = new Thickness(0, 0, 0, 20);
            var stack = new StackLayout();
            stack.Padding = new Thickness(0, Device.OnPlatform(20, 0, 0), 0, 0);
            stack.VerticalOptions = LayoutOptions.FillAndExpand;
            stack.HorizontalOptions = LayoutOptions.FillAndExpand;

            //stack.Padding = Padding;
            stack.Spacing = 20;
            Status = "Ready";
            // stack.BackgroundColor = Color.Aqua;
            //stack.Children.Add(relativeLayout);
            _mediaPicker = DependencyService.Get<IMediaPicker>();


            #region BUTTON TAKE PICTURE

            buttonTakePicture = new RoundButton
            {
                Text = "Take Picture",

                WidthRequest = Device.OnPlatform(120, 200, 200),
                HeightRequest = Device.OnPlatform(50, 50, 80),
                Stroke = Color.Black,
                StrokeThickness = 10,
                CornerRadius = 1,
                FontSize = 20,
                TextColor = StaticKeys.BUTTON_TEXT_COLOR,
                BackgroundColor = StaticKeys.BUTTON_BACKGROUND_COLOR
            };

            buttonTakePicture.VerticalOptions = LayoutOptions.Center;
            buttonTakePicture.HorizontalOptions = LayoutOptions.Center;


            buttonTakePicture.Clicked += async (sender, args) =>
            {



                await TakePicture();
                //System.Diagnostics.Debug.WriteLine("lat - " + gps.PositionLatitude);
                //System.Diagnostics.Debug.WriteLine("lon - " + gps.PositionLongitude);

                if (this.resizedImage != null)
                {

                    //Xamarin.Forms.Maps.Position pos = new Xamarin.Forms.Maps.Position(Convert.ToDouble(issue.Latitude.Replace(".", ",")), Convert.ToDouble(issue.Longitude.Replace(".", ",")));

                    mediaFile.Dispose();
                    await gps.GetPosition();


                    activityIndicator.IsRunning = false;
                    activityIndicator.IsVisible = false;
                    await Navigation.PushAsync(new MapPage(resizedImage, gps, null, null));




                }
            };
            #endregion

            stack.Children.Add(buttonTakePicture);


            #region BTN SELECT PICTURE


            buttonSelectPicture = new RoundButton
            {
                Text = "Select from Library",
                //WidthRequest = 120,
                //HeightRequest = 50,
                WidthRequest = Device.OnPlatform(120, 200, 200),
                HeightRequest = Device.OnPlatform(50, 50, 80),
                Stroke = Color.Black,
                StrokeThickness = 10,
                CornerRadius = 1,
                FontSize = 20,
                TextColor = StaticKeys.BUTTON_TEXT_COLOR,
                BackgroundColor = StaticKeys.BUTTON_BACKGROUND_COLOR
            };

            buttonSelectPicture.VerticalOptions = LayoutOptions.Center;
            buttonSelectPicture.HorizontalOptions = LayoutOptions.Center;


            buttonSelectPicture.Clicked += async (sender, args) =>
            {



                await SelectPicture();


                if (resizedImage != null)
                {


                    if (lat[0] == 0 && lat[1] == 0 && lat[2] == 0 && lon[0] == 0 && lon[1] == 0 && lon[2] == 0)
                    {
                        try
                        {
                            mediaFile.Dispose();
                            await gps.GetPosition();




                            //if (gps.PositionLatitude + "" != "" && gps.PositionLongitude + "" != "")
                            //{
                            //    // Navigation.PushAsync(new MapPage(resizedImage, gps, pos));
                            //    await Navigation.PushAsync(new ReportScreen(resizedImage, null, gps, null, null));
                            //}
                            //else



                            activityIndicator.IsRunning = false;
                            activityIndicator.IsVisible = false;
                            await Navigation.PushAsync(new MapPage(resizedImage, gps, null, null));


                        }
                        catch (Exception ex)
                        {
                            //throw (ex);

                            //DisplayAlert("Erro2", ex.Message, "OK");

                        }
                    }
                    else
                    {

                        var la = (lat[0] + lat[1] / 60 + lat[2] / 3600) * (latRef == "North" ? 1 : -1);
                        var lo = (lon[0] + lon[1] / 60 + lon[2] / 3600) * (lonRef == "West" ? -1 : 1);
                        mediaFile.Dispose();




                        Xamarin.Forms.Maps.Position pos = new Xamarin.Forms.Maps.Position(la, lo);

                        activityIndicator.IsRunning = false;
                        activityIndicator.IsVisible = false;

                        Navigation.PushAsync(new MapPage(resizedImage, null, null, pos));


                    }
                }


                // Navigation.PushAsync(new ReportScreen(mediaFile, null, gps));
            };
            #endregion

            stack.Children.Add(buttonSelectPicture);


            //Func<RelativeLayout, double> getHeight = (parent) => img.GetSizeRequest(relativeLayout.Width, relativeLayout.Height).Request.Height;

            //double d = homeImg.GetSizeRequest(relativeLayout.Width, relativeLayout.Height).Request.Height;
            var constraintY = Constraint.RelativeToParent(parent => parent.Y + 80 + 20);

            relativeLayout.Children.Add(stack,

                Constraint.Constant(0),
                //Constraint.Constant(0));
                constraintY,

                widthConstraint: Constraint.RelativeToParent((parent) =>
                {
                    return parent.Width;
                }),
                heightConstraint: Constraint.RelativeToParent((parent) =>
                {
                    return parent.Height;
                }));


            // ,
            //Constraint.Constant(relativeLayout.Width),
            // Constraint.Constant(relativeLayout.Height));


            //relativeLayout.Children.Add(stack,
            //       Constraint.Constant(0),
            //       Constraint.Constant(0));
            // Content = relativeLayout;


            //Content = new StackLayout
            //{

            //        Children = 
            //        {
            //            relativeLayout
            //        }
            //};


            grid.Children.Add(relativeLayout);
            grid.Children.Add(activityIndicator);


            this.Content = grid;

            this.Padding = new Thickness(Device.OnPlatform(10, 0, 0), Device.OnPlatform(30, 0, 0), Device.OnPlatform(10, 0, 0), 0);


        }

        #region GOTO REPORTER ISSUES SCREEN
        private void GetDataAndGotoList()
        {
            int idAux = CrossSettings.Current.GetValueOrDefault(StaticKeys.APP_PREFIX + "repid", 0);
            if (idAux > 0)
            {
                var id = Convert.ToInt32(idAux);
                Navigation.PushAsync(new ListIssuesScreen(_gps, id));
            }
        }
        #endregion

        #region CAMERA

        /// <summary>
        /// Setups this instance.
        /// </summary>
        private void SetupCamera()
        {
            if (_mediaPicker != null)
            {
                return;
            }

            _device = Resolver.Resolve<IDevice>();

            ////RM: hack for working on windows phone? 
            _mediaPicker = DependencyService.Get<IMediaPicker>() ?? _device.MediaPicker;
        }



        /// <summary>
        /// Takes the picture. This is a variation from the CameraPage example.

        /// </summary>
        /// <returns>Take Picture Task.</returns>
        private async Task TakePicture()
        {
            activityIndicator.IsRunning = true;
            activityIndicator.IsVisible = true;

            //if (Device.OS == TargetPlatform.iOS)
            //{
            //    Navigation.PushAsync(new PictureFromCam());
            //}
            //else
            //{

            SetupCamera();

            await _mediaPicker.TakePhotoAsync(new CameraMediaStorageOptions { DefaultCamera = CameraDevice.Rear, MaxPixelDimension = 1000 }).ContinueWith(t =>
           {
               activityIndicator.IsRunning = false;
               activityIndicator.IsVisible = false;
               if (t.IsFaulted)
               {
                   Status = t.Exception.InnerException.ToString();
               }
               else if (t.IsCanceled)
               {
                   Status = "Canceled";


               }
               else
               {
                   mediaFile = t.Result;




                   lat = mediaFile.Exif.GpsLatitude;
                   latRef = mediaFile.Exif.GpsLatitudeRef + "";
                   lon = mediaFile.Exif.GpsLongitude;
                   lonRef = mediaFile.Exif.GpsLongitudeRef + "";


                   byte[] file = StaticMethods.ReadFully(mediaFile.Source);


                   var resizer = DependencyService.Get<IImageResizer>();
                   resizedImage = resizer.ResizeImage(mediaFile.Path, file, 1000, 1000);





                   //image.Source = ImageSource.FromStream(() => new MemoryStream(resizedBytes));


                   Status = "OK";//string.Format("Path: {0}", _path);
                   ////_imageSource = ImageSource.FromStream(() => mediaFile.Source);

                   //// Stream s = mediaFile.Source;
                   //byte[] file = StaticMethods.ReadFully(mediaFile.Source);

                   //// var resizedBytes = resizer.ResizeImage(file, 400, 400);
                   ////Stream stream = new MemoryStream(resizedBytes);
                   ////image.Source = ImageSource.FromStream(stream);

                   //var resizer = DependencyService.Get<IImageResizer>();
                   //var resizedBytes = resizer.ResizeImage(file, 400, 400);

                   //return mediaFile;



               }

               //return null;
           }, _scheduler);
            //}
        }


        private async Task SelectPicture()
        {

            activityIndicator.IsRunning = true;
            activityIndicator.IsVisible = true;

            SetupCamera();
            if (Device.OS == TargetPlatform.iOS)
            {
                await _gps.GetPosition();

                _gps = null;
                await Navigation.PushAsync(new TakePicScreen(_gps));
            }
            else
            {

                try
                {
                    mediaFile = await this._mediaPicker.SelectPhotoAsync(new CameraMediaStorageOptions
                   {
                       DefaultCamera = CameraDevice.Front,
                       MaxPixelDimension = 1000
                   });

                    // _imageSource = ImageSource.FromStream(() => mediaFile.Source);
                    //mediaFile = ImageSource.FromStream(() => mediaFile.Source);
                    buttonTakePicture.IsEnabled = true;
                    lat = mediaFile.Exif.GpsLatitude;
                    latRef = mediaFile.Exif.GpsLatitudeRef + "";
                    lon = mediaFile.Exif.GpsLongitude;
                    lonRef = mediaFile.Exif.GpsLongitudeRef + "";


                    byte[] file = StaticMethods.ReadFully(mediaFile.Source);


                    var resizer = DependencyService.Get<IImageResizer>();
                    resizedImage = resizer.ResizeImage(mediaFile.Path, file, 1000, 1000);




                }
                catch (System.Exception ex)
                {
                    Status = ex.Message;

                    //DisplayAlert("Erro1", ex.Message, "OK");

                }
            }


        }
        #endregion

    }
}
