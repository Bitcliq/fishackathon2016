using Newtonsoft.Json.Linq;
using PinFixed.BL;
using Plugin.Settings;
//using Refractored.Xam.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XLabs.Enums;
using XLabs.Forms.Controls;

namespace PinFixed
{
    public class ListNearByIssues : ContentPage
    {

        RelativeLayout relativeLayout;
        private GPSUtils _gps;
        Image img;

        ActivityIndicator activityIndicator;
        public ListView listView;

        private double _latitude;
        private double _longitude;
        private int? _radius;

        public GPSUtils GPS
        {
            get { return _gps; }
        }


        private int _reportedBy;
        public ListNearByIssues(GPSUtils gps, double latitude, double longitude)
        {

            _gps = gps;

            _latitude = latitude;
            _longitude = longitude;


            int rd = CrossSettings.Current.GetValueOrDefault(StaticKeys.APP_PREFIX + "Radius", 100);
            string type = CrossSettings.Current.GetValueOrDefault(StaticKeys.APP_PREFIX + "Measure", "m");



            if (rd != -1)
                _radius = rd;

            if (type != "m") // it is km
            {
                if (_radius != null)
                    _radius = _radius * 1000;
            }



            NavigationPage.SetHasNavigationBar(this, false);


            #region ACTIVITY INDICATOR
            activityIndicator = new ActivityIndicator
            {

                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Color = Color.Black,
                IsVisible = false
            };


            activityIndicator.IsRunning = true;
            activityIndicator.IsVisible = true;

            #endregion



            relativeLayout = new RelativeLayout();

            relativeLayout.HorizontalOptions = LayoutOptions.FillAndExpand;
            relativeLayout.VerticalOptions = LayoutOptions.Start;
            //relativeLayout.BackgroundColor = Color.FromHex("#000000");
            relativeLayout.BackgroundColor = StaticKeys.TOPBAR_BACKGROUND_COLOR;
            //relativeLayout.Padding = new Thickness(Device.OnPlatform(10, 10, 0), Device.OnPlatform(10, 10, 0), Device.OnPlatform(10, 10, 0), Device.OnPlatform(10, 10, 0));

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
                    Navigation.PopAsync();
                    //Navigation.PushAsync(new StartPage(gps));
                };

                b.Image = "back.png";
                b.Source = ImageSource.FromFile("back.png");

                relativeLayout.Children.Add(b,
                     Constraint.Constant(10),
                      Constraint.Constant(Device.OnPlatform(20, 10, 10)));

            }
            else
            {
                var backTapRecognizer = new TapGestureRecognizer
                {
                    Command = new Command(() =>
                    {
                        Navigation.PopAsync();
                        //Navigation.PushAsync(new StartPage(gps));

                        //Navigation.PopAsync();
                    }),
                    NumberOfTapsRequired = 1
                };

                var backImg = new Image();
                backImg.Source = "back.png";




                backImg.GestureRecognizers.Add(backTapRecognizer);
                relativeLayout.Children.Add(backImg,
                    Constraint.Constant(0),
                    Constraint.Constant(10));

            }

            #endregion





            //var referenceLabel = new Label { Text = "PERTO DE MIM", Opacity = 0 };
            //var titleLabel = new Label { Text = "PERTO DE MIM" };
            //titleLabel.TextColor = StaticKeys.LABEL_TEXT_COLOR;

            //var centerX = Constraint.RelativeToParent(parent => parent.Width / 2);


            //var centerY = Constraint.RelativeToParent(parent => relativeLayout.Height / 2);


            //relativeLayout.Children.Add(referenceLabel, centerX,  centerY);
            //relativeLayout.Children.Add(titleLabel,
            //    Constraint.RelativeToView(referenceLabel, (parent, sibling) => sibling.X - sibling.Width / 2), // X
            //    centerY);


            //#region BUTTON HOME


            //if (Device.OS == TargetPlatform.iOS)
            //{
            //    ImageButton b = new ImageButton();
            //    b.Text = "   ";
            //    b.Orientation = ImageOrientation.ImageToLeft;

            //    b.ImageHeightRequest = 50;
            //    b.ImageWidthRequest = 50;


            //    b.Clicked += (sender, args) =>
            //    {
            //        Navigation.PushAsync(new StartPage(gps));
            //    };

            //    b.Image = "btn_home.png";
            //    b.Source = ImageSource.FromFile("btn_home.png");

            //    Func<RelativeLayout, double> getBackWitdh = (parent) => b.GetSizeRequest(relativeLayout.Width, relativeLayout.Height).Request.Width;
            //    relativeLayout.Children.Add(b,
            //        Constraint.RelativeToParent((r) => getBackWitdh(r) + 10),
            //        Constraint.Constant(Device.OnPlatform(20, 10, 10)));






            //}
            //else
            //{

            //    var homeTapRecognizer = new TapGestureRecognizer
            //    {
            //        Command = new Command(() =>
            //        {
            //            Navigation.PushAsync(new StartPage(gps));

            //        }),
            //        NumberOfTapsRequired = 1
            //    };

            //    var homeImg = new Image();
            //    homeImg.Source = "btn_home.png";





            //    homeImg.GestureRecognizers.Add(homeTapRecognizer);

            //    Func<RelativeLayout, double> getBackWitdh = (parent) => img.GetSizeRequest(relativeLayout.Width, relativeLayout.Height).Request.Width;
            //    relativeLayout.Children.Add(homeImg,
            //        Constraint.RelativeToParent((r) => getBackWitdh(r) + 10),
            //        Constraint.Constant(10));

            //}

            //#endregion


            #region BUTTON REPORT

            if (Device.OS == TargetPlatform.iOS)
            {
                ImageButton b = new ImageButton();
                b.Text = "   ";
                b.Orientation = ImageOrientation.ImageToLeft;
                //b.InputTransparent = true;
                //b.BorderRadius = 0;
                //b.CornerRadius = 0;
                //b.HasShadow = false;
                b.ImageHeightRequest = 50;
                b.ImageWidthRequest = 50;


                b.Clicked += (sender, args) =>
                {
                    Navigation.PushAsync(new TakePictureScreen(_gps));
                };

                b.Image = "btn_criar_report.png";
                b.Source = ImageSource.FromFile("btn_criar_report.png");

                Func<RelativeLayout, double> getLabelWidth = (parent) => b.GetSizeRequest(relativeLayout.Width, relativeLayout.Height).Request.Width;
                relativeLayout.Children.Add(b,
                    Constraint.RelativeToParent((r) => r.Width - getLabelWidth(r)),
                    Constraint.Constant(Device.OnPlatform(20, 10, 10)));

            }
            else
            {

                var profileTapRecognizer = new TapGestureRecognizer
                {
                    Command = new Command(() =>
                    {
                        Navigation.PushAsync(new TakePictureScreen(_gps));
                    }),
                    NumberOfTapsRequired = 1
                };


                img = new Image();
                img.Source = "btn_criar_report.png";





                img.GestureRecognizers.Add(profileTapRecognizer);





                Func<RelativeLayout, double> getLabelWidth = (parent) => img.GetSizeRequest(relativeLayout.Width, relativeLayout.Height).Request.Width;
                relativeLayout.Children.Add(img,
                    Constraint.RelativeToParent((r) => r.Width - getLabelWidth(r)),
                    Constraint.Constant(10));

            }
            relativeLayout.Padding = new Thickness(0, 0, 0, Device.OnPlatform(0, 0, 10));


            #endregion

            #region LIST VIEW
            listView = new ListView
            {
                HasUnevenRows = true,
                ItemTemplate = new DataTemplate(typeof(IssueCell)),

                SeparatorColor = Color.FromHex("#74a633"),

            };

            #endregion




            Content = new StackLayout
            {
                //Padding = new Thickness(Device.OnPlatform(10, 0, 0), Device.OnPlatform(30, 0, 0), Device.OnPlatform(10, 0, 0), 0),
                //BackgroundColor = Device.OnPlatform(Color.Black, Color.Default, Color.Default),

                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,

                Children = { relativeLayout, activityIndicator, listView/*, activityIndicator */},
                //Padding = new Thickness(5, 5, 5, 5)
            };



            relativeLayout.IsVisible = false;
            listView.IsVisible = false;



            this.Padding = new Thickness(0, 0, 0, 0);
            this.BackgroundColor = Color.White;
            //this.BackgroundColor = Device.OnPlatform(Color.FromHex("363636"), Color.Default, Color.Default);
            GetData();
        }


        public void GetData()
        {
       



            if (StaticMethods.CheckConnection())
            {

                IssuesWebServiceSoapClient client = new IssuesWebServiceSoapClient(
                               new BasicHttpBinding(),
                               new EndpointAddress(StaticKeys.WebServiceUrl));


                client.GetNearByIssuesCompleted += OnGotResultReport;


                client.GetNearByIssuesAsync(null, _latitude, _longitude, _radius);


            }
            else
            {
                DisplayAlert(StaticKeys.ERROR_TITLE, "To use this app you have to be connected to the internet", StaticKeys.OK_BTN_TITLE);



            }

        }




        void bv_Clicked(object sender, EventArgs e)
        {

            Navigation.PushAsync(new TakePictureScreen(_gps));

        }

        void OnGotResultReport(object sender, GetNearByIssuesCompletedEventArgs e)
        {
            //IsBusy = true;
            Device.BeginInvokeOnMainThread(async () =>
            {
                string error = null;
                if (e.Error != null)
                    error = e.Error.Message;
                else if (e.Cancelled)
                    error = "Cancelled";

                if (!string.IsNullOrEmpty(error))
                {
                    await DisplayAlert("Error", error, "OK", "Cancel");
                }
                else
                {
                    if (e.Result != "{}")
                    {
                        var obj = JObject.Parse(e.Result);
                        var issues = (JArray)obj["Data"];
                        if (issues != null)
                        {


                            List<Issue> list = new List<Issue>();


                            foreach (JObject t in issues)
                            {

                                Issue tp = new Issue(Convert.ToInt32(t["ID"]), t["PhotoThumbnailUrl"] + "", t["Subject"] + "", t["StateName"] + "",
                                   t["Message"] + "", t["TypeName"] + "", Convert.ToDateTime(t["DateReported"] + ""), t["ParentName"] + "", t["Latitude"] + "",
                                   t["Longitude"] + "", t["PhotoUrl"] + "", t["PropertyID"] + "", t["PropertyName"] + "");
                                list.Add(tp);

                            }



                            if (list.Count < 0)
                            {
                                await DisplayAlert(StaticKeys.INFO_TITLE, "No Fish Specie Reports near you!", StaticKeys.OK_BTN_TITLE);
                                listView.IsVisible = false;
                            }
                            else
                            {
                                listView.ItemsSource = list;

                                listView.ItemTemplate = new DataTemplate(typeof(ViewCellNearBy));
                                listView.IsVisible = true;
                            }




                            //    this.Padding = new Thickness(Device.OnPlatform(10, 0, 0), Device.OnPlatform(30, 0, 0), Device.OnPlatform(10, 0, 0), 0);
                            //    this.BackgroundColor = Device.OnPlatform(Color.FromHex("363636"), Color.Default, Color.Default);
                        }

                        else
                        {
                            //var err = (JArray)obj["Error"];
                            await DisplayAlert(StaticKeys.ERROR_TITLE, "Could not get Fish Specie Reports. Try again later!", StaticKeys.OK_BTN_TITLE);
                            //IsBusy = false;
                        }
                        //resultsLabel.Text = e.Result;
                    }
                    else
                    {
                        listView.ItemsSource = new List<Issue>();
                        listView.ItemTemplate = new DataTemplate(typeof(IssueCell));
                        await DisplayAlert(StaticKeys.INFO_TITLE, "No Fish Specie Reports in the selected radius.", StaticKeys.OK_BTN_TITLE);
                    }

                }

                activityIndicator.IsRunning = false;
                activityIndicator.IsVisible = false;

                relativeLayout.IsVisible = true;
                listView.IsVisible = true;
            });
        }





    }
}
