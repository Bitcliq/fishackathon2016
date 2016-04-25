using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
//using PinFixed.Core;
using System.ServiceModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PinFixed.BL;
using XLabs.Platform;
using XLabs.Platform.Services.Geolocation;
using XLabs.Platform.Services.Media;
using System.Threading;
using XLabs.Ioc;
using System.Windows.Input;

using PinFixed.UI;
//using Refractored.Xam.Settings;
using XLabs.Forms.Controls;
using XLabs.Platform.Device;
using XLabs.Enums;
using Plugin.Settings;



namespace PinFixed
{

    /// <summary>    
    /// Report Screen for Asking  Fish Species
    /// </summary>     
    /// <returns></returns>    
    /// 

    public class ReportScreen : ContentPage
    {
        PopupLayout _PopUpLayout;

        Entry txtProperties;
        private string propertyName = "";


        Picker pickerAreas;
        Picker pickerProperties;

        Dictionary<int, TypesWS> typesDic = new Dictionary<int, TypesWS>();
        Dictionary<int, PropertyWS> propertiesDic = new Dictionary<int, PropertyWS>();


        List<TypesWS> listAreas = new List<TypesWS>();
        List<PropertyWS> listProperties = new List<PropertyWS>();


        Entry txtSubject;

        CustomEditor txtMessage;

        Entry txtMessageDisabled;


        RoundButton buttonReport;


        GPSUtils _gps;
        double? _latitude;
        double? _longitude;
        byte[] _resizedImage;


        //Button buttonNearest;
        Button buttonProperties;


        int _reporterID;
        int? _radius = null;

        Issue _issue;

        protected override bool OnBackButtonPressed()
        {
            if (Device.OS == TargetPlatform.Android)
            {
                if (_issue != null)
                {
                    if (_issue.ID > 0)
                    {
                        return base.OnBackButtonPressed();
                    }
                    else
                    {
                        Navigation.PushAsync(new MapPage(_resizedImage, _gps, null, null));
                        return true;
                    }
                }
                else
                {
                    Navigation.PushAsync(new MapPage(_resizedImage, _gps, null, null));
                    return true;
                }
                //Navigation.PushAsync(new ReportScreen(_resizedImage, null, _gps, latitude, longitude));
            }
            else
            {
                return base.OnBackButtonPressed();
            }
        }




        #region NEAREST

        //async Task GetData()
        //{
        //    if (StaticMethods.CheckConnection())
        //    {

        //        IssuesWebServiceSoapClient client = new IssuesWebServiceSoapClient(
        //                       new BasicHttpBinding(),
        //                       new EndpointAddress(StaticKeys.WebServiceUrl));


        //        client.CountNearByIssuesCompleted += OnGotResultReport;


        //        if (_latitude == null)
        //        {
        //            if (_gps != null)
        //            {
        //                if (_gps.PositionLatitude != null)
        //                {
        //                    try
        //                    {
        //                        _latitude = Convert.ToDouble(_gps.PositionLatitude);
        //                    }
        //                    catch
        //                    {
        //                        //double[] latarr = this.mediaFile.Exif.GpsLatitude;
        //                    }
        //                }
        //            }
        //        }

        //        if (_longitude == null)
        //        {
        //            if (_gps != null)
        //            {
        //                if (_gps.PositionLongitude != null)
        //                {
        //                    try
        //                    {
        //                        _longitude = Convert.ToDouble(_gps.PositionLongitude);
        //                    }
        //                    catch
        //                    {
        //                    }
        //                }
        //            }
        //        }



        //        client.CountNearByIssuesAsync(null, (double)_latitude, (double)_longitude, _radius);

        //    }
        //    else
        //    {
        //        await DisplayAlert("Erro", "Para usar esta aplicação tem de ter acesso à internet", "OK");
        //    }
        //}

        //void OnGotResultReport(object sender, CountNearByIssuesCompletedEventArgs e)
        //{
        //    //IsBusy = true;
        //    Device.BeginInvokeOnMainThread(async () =>
        //    {
        //        string error = null;
        //        if (e.Error != null)
        //            error = e.Error.Message;
        //        else if (e.Cancelled)
        //            error = "Cancelled";

        //        if (!string.IsNullOrEmpty(error))
        //        {
        //            await DisplayAlert("Error", error, "OK", "Cancel");
        //        }
        //        else
        //        {

        //            var obj = JObject.Parse(e.Result);

        //            if (obj["Data"] != null)
        //            {
        //                try
        //                {
        //                    int cont = Convert.ToInt32(obj["Data"]);

        //                    if (cont > 0)
        //                    {
        //                        buttonNearest.Text = "Ver " + cont + " Ocorrências";
        //                        buttonNearest.IsVisible = true;
        //                    }
        //                    else
        //                        buttonNearest.IsVisible = false;
        //                }
        //                catch (Exception)
        //                {
        //                    buttonNearest.IsVisible = false;
        //                }
        //            }
        //            else
        //            {
        //                // erro
        //                buttonNearest.IsVisible = false;
        //            }
        //        }

        //    });
        //}


        #endregion


        async Task<bool> GetTypes()
        {
            var result = await StaticMethods.GetTypes();



            pickerAreas.Items.Clear();

            pickerAreas.Title = "";

            foreach (var item in typesDic)
            {

                if (item.Value.ParentID == null)
                {
                    pickerAreas.Items.Add(item.Value.Name);
                    listAreas.Add(item.Value);
                }
            }

            return result;
        }


        async Task<bool> GetProperties()
        {
            var result = await StaticMethods.GetPropeties();

            pickerProperties.Items.Clear();

            pickerProperties.Title = "-- Select --";
            pickerProperties.HorizontalOptions = LayoutOptions.FillAndExpand;



            foreach (var item in propertiesDic)
            {
                pickerProperties.Items.Add(item.Value.Name);
                listProperties.Add(item.Value);
            }



            return result;
        }


        public ReportScreen(byte[] resizedImage, Issue issue, GPSUtils gps, double? latitude, double? longitude)
        {

            _PopUpLayout = new PopupLayout();


            try
            {

                //int rd = CrossSettings.Current.GetValueOrDefault(StaticKeys.APP_PREFIX + "Radius", 100);
                //string type = CrossSettings.Current.GetValueOrDefault(StaticKeys.APP_PREFIX + "Measure", "m");



                //if (rd != -1)
                //    _radius = rd;

                //if (type != "m") // it is km
                //{
                //    if (_radius != null)
                //        _radius = _radius * 1000;
                //}

                _resizedImage = resizedImage;
                this._gps = gps;
                this._latitude = latitude;
                this._longitude = longitude;
                this._issue = issue;

                //buttonNearest = new Button(); 
                //buttonNearest.IsVisible = false;

                //   var t = GetData();



                NavigationPage.SetHasNavigationBar(this, false);


                var relativeLayout = new RelativeLayout();
                relativeLayout.HorizontalOptions = LayoutOptions.FillAndExpand;
                relativeLayout.VerticalOptions = LayoutOptions.Start;
                relativeLayout.BackgroundColor = StaticKeys.TOPBAR_BACKGROUND_COLOR;



                ImageButton bbak = new ImageButton();
                var backImg = new Image();
                #region BUTTON BACK

                if (Device.OS == TargetPlatform.iOS)
                {
                    bbak = new ImageButton();
                    bbak.Text = "   ";
                    bbak.Orientation = ImageOrientation.ImageToLeft;


                    bbak.ImageHeightRequest = 50;
                    bbak.ImageWidthRequest = 50;


                    bbak.Clicked += (sender, args) =>
                    {
                        if (Device.OS == TargetPlatform.Android)
                        {
                            Navigation.PushAsync(new MapPage(resizedImage, _gps, null, null));

                            //Navigation.PushAsync(new ReportScreen(_resizedImage, null, _gps, latitude, longitude));
                        }
                        else
                        {
                            App.Navigation.PopAsync();
                        }
                    };

                    bbak.Image = "back.png";
                    bbak.Source = ImageSource.FromFile("back.png");

                    relativeLayout.Children.Add(bbak, Constraint.RelativeToParent((parent) =>
                    {
                        return 10;
                    }), Constraint.Constant(Device.OnPlatform(20, 10, 10)));

                }
                else
                {
                    var backTapRecognizer = new TapGestureRecognizer
                    {
                        Command = new Command(() =>
                        {
                            //IReadOnlyList<Page> lst = App.Navigation.NavigationStack;

                            //Page p = lst[lst.Count - 1];

                            if (Device.OS == TargetPlatform.Android)
                            {
                                Navigation.PushAsync(new MapPage(resizedImage, _gps, null, null));

                                //Navigation.PushAsync(new ReportScreen(_resizedImage, null, _gps, latitude, longitude));
                            }
                            else
                            {
                                App.Navigation.PopAsync();
                            }

                        }),
                        NumberOfTapsRequired = 1
                    };


                    backImg.Source = "back.png";



                    backImg.GestureRecognizers.Add(backTapRecognizer);
                    relativeLayout.Children.Add(backImg, Constraint.RelativeToParent((parent) =>
                    {
                        return 0;
                    }), Constraint.Constant(Device.OnPlatform(20, 10, 10)));

                }
                #endregion



                #region BUTTON HOME

                if (Device.OS == TargetPlatform.iOS)
                {
                    ImageButton b = new ImageButton();
                    b.Text = "   ";
                    b.Orientation = ImageOrientation.ImageToLeft;


                    b.ImageHeightRequest = 50;
                    b.ImageWidthRequest = 50;


                    b.Clicked += (sender, args) =>
                    {
                        _resizedImage = null;
                        issue = null;

                        GC.Collect();

                        App.Navigation.PopToRootAsync();
                    };

                    b.Image = "btn_home.png";
                    b.Source = ImageSource.FromFile("btn_home.png");

                    Func<RelativeLayout, double> getBackWitdh = (parent) => bbak.GetSizeRequest(relativeLayout.Width, relativeLayout.Height).Request.Width;
                    relativeLayout.Children.Add(b,
                        Constraint.RelativeToParent((r) => getBackWitdh(r) + 10),
                        Constraint.Constant(Device.OnPlatform(20, 10, 10)));

                }
                else
                {
                    var homeTapRecognizer = new TapGestureRecognizer
                    {
                        Command = new Command(() =>
                        {
                            _resizedImage = null;
                            issue = null;

                            GC.Collect();

                            App.Navigation.PopToRootAsync();
                            //Navigation.PushAsync(new StartPage(gps));

                        }),
                        NumberOfTapsRequired = 1
                    };

                    var homeImg = new Image();
                    homeImg.Source = "btn_home.png";




                    homeImg.GestureRecognizers.Add(homeTapRecognizer);

                    Func<RelativeLayout, double> getBackWitdh = (parent) => backImg.GetSizeRequest(relativeLayout.Width, relativeLayout.Height).Request.Width;
                    relativeLayout.Children.Add(homeImg,
                        Constraint.RelativeToParent((r) => getBackWitdh(r) + 10),
                         Constraint.Constant(Device.OnPlatform(20, 10, 10)));

                }
                #endregion



                //this.mediaFile = mediaFile;

                if (issue != null)
                {
                    #region BUTTON REPORT

                    if (Device.OS == TargetPlatform.iOS)
                    {
                        ImageButton b = new ImageButton();
                        b.Text = "   ";
                        b.Orientation = ImageOrientation.ImageToLeft;


                        b.ImageHeightRequest = 50;
                        b.ImageWidthRequest = 50;


                        b.Clicked += (sender, args) =>
                        {
                            Xamarin.Forms.Maps.Position pos = new Xamarin.Forms.Maps.Position(Convert.ToDouble(issue.Latitude.Replace(".", ",")), Convert.ToDouble(issue.Longitude.Replace(".", ",")));

                            Navigation.PushAsync(new MapPage(resizedImage, _gps, pos, null));
                        };

                        b.Image = "mapa.png";
                        b.Source = ImageSource.FromFile("mapa.png");

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
                                Xamarin.Forms.Maps.Position pos = new Xamarin.Forms.Maps.Position(Convert.ToDouble(issue.Latitude.Replace(".", ",")), Convert.ToDouble(issue.Longitude.Replace(".", ",")));

                                Navigation.PushAsync(new MapPage(resizedImage, _gps, pos, null));
                            }),
                            NumberOfTapsRequired = 1
                        };





                        var imgMap = new Image();
                        imgMap.Source = "mapa.png";




                        imgMap.GestureRecognizers.Add(profileTapRecognizer);


                        Func<RelativeLayout, double> getLabelWidth = (parent) => imgMap.GetSizeRequest(relativeLayout.Width, relativeLayout.Height).Request.Width;
                        relativeLayout.Children.Add(imgMap,
                            Constraint.RelativeToParent((r) => r.Width - getLabelWidth(r)),
                             Constraint.Constant(Device.OnPlatform(20, 10, 10)));


                        //relativeLayout.Padding = new Thickness(0, 0, 0, Device.OnPlatform(30, 0, 0));
                    }
                    #endregion

                }
                else
                {
                    _reporterID = CrossSettings.Current.GetValueOrDefault(StaticKeys.APP_PREFIX + "repid", 0);
                    if (_reporterID > 0)
                    {
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
                                GetDataAndGotoList();
                            };

                            b.Image = "btn_lista.png";
                            b.Source = ImageSource.FromFile("btn_lista.png");

                            Func<RelativeLayout, double> getWitdh = (parent) => b.GetSizeRequest(relativeLayout.Width, relativeLayout.Height).Request.Width;
                            relativeLayout.Children.Add(b,
                                Constraint.RelativeToParent((r) => r.Width - getWitdh(r)),
                                 Constraint.Constant(Device.OnPlatform(20, 10, 10)));

                        }
                        else
                        {
                            var profileTapRecognizer = new TapGestureRecognizer
                            {
                                Command = new Command(() =>
                                {
                                    GetDataAndGotoList();
                                }),
                                NumberOfTapsRequired = 1
                            };

                            Image img = new Image();
                            img.Source = "btn_lista.png";

                            img.GestureRecognizers.Add(profileTapRecognizer);





                            Func<RelativeLayout, double> getWitdh = (parent) => img.GetSizeRequest(relativeLayout.Width, relativeLayout.Height).Request.Width;
                            relativeLayout.Children.Add(img,
                                Constraint.RelativeToParent((r) => r.Width - getWitdh(r)),
                                Constraint.Constant(Device.OnPlatform(20, 10, 10)));

                        }
                        #endregion
                    }
                }

                #region IMAGE
                var image = new Image();

                if (resizedImage != null)
                {
                    image.Source = ImageSource.FromStream(() => new MemoryStream(resizedImage));
                }


                image.HeightRequest = 100;
                #endregion


                #region AREAS
                var labelAreas = new Label();
                if (issue != null)
                {
                
                    labelAreas.Text = "Fish Specie";
                    labelAreas.TextColor = StaticKeys.LABEL_TEXT_COLOR;
                    pickerAreas = new Picker();






                    typesDic = StaticMethods.typesDic;

                    if (typesDic == null || typesDic.Count == 0)
                    {

                        // GetTypes();
                        var result1 = GetTypes();


                    }


                    pickerAreas.Title = "-- Selecione --";

                    foreach (var item in typesDic)
                    {

                        if (item.Value.ParentID == null)
                        {
                            pickerAreas.Items.Add(item.Value.Name);
                            listAreas.Add(item.Value);
                        }
                    }
                }
                #endregion

                #region PROPERTIES

                var labelProperties = new Label();
                labelProperties.Text = "Ocean";

                labelProperties.TextColor = StaticKeys.LABEL_TEXT_COLOR; // Device.OnPlatform(Color.White, Color.Default, Color.Default);


                pickerProperties = new Picker();



                propertiesDic = StaticMethods.propertiesDic;

                if (propertiesDic == null || propertiesDic.Count == 0)
                {

                    var result = GetProperties();
                    //StaticMethods.GetPropeties();


                }
                pickerProperties.Title = "-- Select --";
                pickerProperties.HorizontalOptions = LayoutOptions.FillAndExpand;



                foreach (var item in propertiesDic)
                {
                    pickerProperties.Items.Add(item.Value.Name);
                    listProperties.Add(item.Value);
                }


                buttonProperties = new RoundButton
                {
                    Text = "+",

                    WidthRequest = Device.OnPlatform(50, 50, 100),
                    //HeightRequest = Device.OnPlatform(30, 30, 80),

                    Stroke = Color.Black,
                    //StrokeThickness = 10,
                    CornerRadius = 1,
                    FontSize = 20,
                    TextColor = StaticKeys.BUTTON_TEXT_COLOR,
                    BackgroundColor = StaticKeys.BUTTON_BACKGROUND_COLOR,
                    //WidthRequest = 50,
                    HorizontalOptions = LayoutOptions.EndAndExpand

                };


                buttonProperties.Clicked += buttonProperties_Clicked;





                var gridProperties = new Grid()
                {
                    //BackgroundColor = Color.Aqua,
                    //Padding = 5,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    // BackgroundColor = Device.OnPlatform(Color.FromHex("363636"), Color.Default, Color.Default),
                    RowDefinitions = 
                        {
                            new RowDefinition { Height = GridLength.Auto }

                   
                        },
                    ColumnDefinitions = {
                  

                             new ColumnDefinition {Width = new GridLength(5, GridUnitType.Star)},
                            new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},


                    }

                };


                gridProperties.Children.Add(pickerProperties, 0, 0);
                gridProperties.Children.Add(buttonProperties, 1, 0);


                //var propertiesLayout = new StackLayout()
                //{

                //    HorizontalOptions = LayoutOptions.StartAndExpand,
                //    //VerticalOptions = LayoutOptions.FillAndExpand,
                //    Orientation = StackOrientation.Horizontal,
                //    Children = { pickerProperties, buttonProperties }
                //};



                #endregion




                #region SUBJECT

                var labelSubject = new Label();
                labelSubject.Text = "Subject";
                labelSubject.TextColor = StaticKeys.LABEL_TEXT_COLOR;
                txtSubject = new Entry();

                #endregion

                #region MESSAGE

                var labelMessage = new Label();
                labelMessage.Text = "Message";
                labelMessage.TextColor = StaticKeys.LABEL_TEXT_COLOR;

                if (issue != null)
                {
                    txtMessageDisabled = new Entry { HeightRequest = 100 };
                    txtMessageDisabled.IsEnabled = false;

                }

                txtMessage = new CustomEditor { HeightRequest = 100 };



                #endregion



                //button Nearest

                //buttonNearest.Clicked += buttonNearest_Clicked;



                #region BUTTON NEXT
                buttonReport = new RoundButton
                {
                    Text = "Next",

                    //WidthRequest = 120,
                    //HeightRequest = 50,

                    WidthRequest = Device.OnPlatform(120, 120, 120),
                    HeightRequest = Device.OnPlatform(50, 50, 80),


                    Stroke = Color.Black,
                    StrokeThickness = 10,
                    CornerRadius = 1,
                    FontSize = 20,
                    TextColor = StaticKeys.BUTTON_TEXT_COLOR,
                    BackgroundColor = StaticKeys.BUTTON_BACKGROUND_COLOR

                };
                buttonReport.Clicked += buttonReport_Clicked;

                #endregion






                #region NEAR BY


                double lat = 0, lon = 0;

             
                if (_gps != null)
                {

                    if (_gps.PositionLatitude + "" != "")
                    {
                    
                        try
                        {
                            lat = Convert.ToDouble(_gps.PositionLatitude);
                        }
                        catch
                        {
                        }


                        try
                        {
                            lon = Convert.ToDouble(_gps.PositionLongitude);

                        }
                        catch
                        {
                        }

                    }
                    else
                        _gps.GetPosition();

                }



                #region BUTTON

                if (Device.OS == TargetPlatform.iOS)
                {
                    ImageButton b = new ImageButton();
                    b.Text = "   ";
                    b.Orientation = ImageOrientation.ImageToLeft;


                    b.ImageHeightRequest = 50;
                    b.ImageWidthRequest = 50;


                    b.Clicked += (sender, args) =>
                    {
                        Navigation.PushAsync(new ListNearByIssues(_gps, lat, lon));
                    };

                    b.Image = "icone_perto_de_mim.png";
                    b.Source = ImageSource.FromFile("icone_perto_de_mim.png");



                    Func<RelativeLayout, double> getImgWidth = (parent) => b.GetSizeRequest(relativeLayout.Width, relativeLayout.Height).Request.Width;
                    relativeLayout.Children.Add(b,
                        Constraint.RelativeToParent((r) => r.Width - getImgWidth(r) - getImgWidth(r) - 10),
                        Constraint.Constant(Device.OnPlatform(20, 10, 10)));




                }
                else
                {
                    var profileTapRecognizer = new TapGestureRecognizer
                    {
                        Command = new Command(() =>
                        {
                            Navigation.PushAsync(new ListNearByIssues(_gps, lat, lon));
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


                var stack = new StackLayout();

                stack.Spacing = 5;
                stack.Padding = 10;

                stack.Children.Add(image);

                if(issue != null)
                { 
                    stack.Children.Add(labelAreas);
                    stack.Children.Add(pickerAreas);
                }

                stack.Children.Add(labelProperties);
                stack.Children.Add(gridProperties);

                stack.Children.Add(labelSubject);
                stack.Children.Add(txtSubject);
                stack.Children.Add(labelMessage);

                if (issue != null)
                {
                    stack.Children.Add(txtMessageDisabled);
                }
                else
                {
                    stack.Children.Add(txtMessage);
                }

                var stackBtutton = new StackLayout();

                stackBtutton.Spacing = 15;
                stackBtutton.Children.Add(buttonReport);
                //stackBtutton.Children.Add(buttonNearest);
                stack.Children.Add(stackBtutton);

             


                relativeLayout.Padding = new Thickness(0, Device.OnPlatform(0, 10, 0), 0, 0);


                var scrollview = new ScrollView
                {
                    Content = new StackLayout
                    {
                        //Orientation = StackOrientation.Vertical,
                        Children = 
                        {
                            relativeLayout, stack
                        }
                    }
                };


                stackBtutton.Padding = 20; //new Thickness(Device.OnPlatform(10, 0, 0), Device.OnPlatform(30, 0, 0), Device.OnPlatform(10, 0, 0), 0);


                _PopUpLayout.Padding = new Thickness(0, 0, 0, 0);
                _PopUpLayout.Content = scrollview;

                Content = _PopUpLayout;








                if (issue != null)
                {
                    //buttonNearest.IsVisible = false;
                    //buttonNearest.IsEnabled = false;

                    image.Source = issue.PhotoThumbnailUrl;
                    txtMessage.Text = issue.Message;

                    int index = 0;
                    for (int i = 0; i < pickerAreas.Items.Count; i++)
                    {
                        if (issue.ParentName + "" != "")
                        {
                            if (pickerAreas.Items[i] + "" == issue.ParentName)
                            {
                                index = i;
                                break;
                            }
                        }
                        else
                        {
                            if (pickerAreas.Items[i] + "" == issue.TypeName)
                            {
                                index = i;
                                break;
                            }
                        }
                    }
                    pickerAreas.SelectedIndex = index;

                      index = -1;

                    #region OCEAN
                    for (int i = 0; i < pickerProperties.Items.Count; i++)
                    {
                        if (issue.PropertyName + "" != "")
                        {
                            if (pickerProperties.Items[i] + "" == issue.PropertyName)
                            {
                                index = i;
                                break;
                            }
                        }


                    }
                    pickerProperties.SelectedIndex = index;




                    //var grid = new Grid()
                    //{
                    //    //BackgroundColor = Color.Aqua,

                    //    VerticalOptions = LayoutOptions.FillAndExpand,
                    //    HorizontalOptions = LayoutOptions.FillAndExpand,
                    //    // BackgroundColor = Device.OnPlatform(Color.FromHex("363636"), Color.Default, Color.Default),
                    //    RowDefinitions = 
                    //    {
                    //        new RowDefinition { Height = GridLength.Auto },
                    //        new RowDefinition { Height = GridLength.Auto }

                    //    }

                    //};

                    #endregion



                    txtSubject.Text = issue.Subject;
                    txtSubject.IsEnabled = false;
                    buttonReport.IsEnabled = false;
                    buttonReport.TextColor = Color.Gray;
                    pickerAreas.IsEnabled = false;

                    txtMessage.IsEnabled = false;

                    pickerProperties.IsEnabled = false;
                    buttonProperties.IsEnabled = false;
                    buttonProperties.TextColor = Color.Gray;
                    var imageTapRecognizer = new TapGestureRecognizer
                    {
                        Command = new Command(() =>
                        {
                            Navigation.PushAsync(new FullImage(issue.PhotoUrl));


                        }),
                        NumberOfTapsRequired = 1
                    };
                    image.GestureRecognizers.Add(imageTapRecognizer);



                }

                this.BackgroundColor = Device.OnPlatform(Color.White, Color.White, Color.Black);//,  Color.White;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        #region PROPERITES BUTTON CLICK

        void buttonProperties_Clicked(object sender, EventArgs e)
        {


            var lbl = new Label();
            lbl.TextColor = StaticKeys.LABEL_TEXT_COLOR;
            lbl.Text = "Name";

            txtProperties = new Entry();


            var btnAdd = new RoundButton
            {
                Text = "Add",


                WidthRequest = Device.OnPlatform(120, 120, 200),
                HeightRequest = Device.OnPlatform(30, 30, 80),

                //WidthRequest = 120,
                //HeightRequest = 30,
                Stroke = Color.Black,
                StrokeThickness = 10,
                CornerRadius = 1,
                FontSize = 12,
                TextColor = StaticKeys.BUTTON_TEXT_COLOR,
                BackgroundColor = StaticKeys.BUTTON_BACKGROUND_COLOR
            };


            btnAdd.Clicked += buttonAdd_Clicked;


            var btnCancel = new RoundButton
            {
                Text = "Cancel",

                //WidthRequest = 120,
                //HeightRequest = 30,
                WidthRequest = Device.OnPlatform(120, 120, 200),
                HeightRequest = Device.OnPlatform(50, 50, 80),
                Stroke = Color.Black,
                StrokeThickness = 10,
                CornerRadius = 1,
                FontSize = 12,
                TextColor = StaticKeys.BUTTON_TEXT_COLOR,
                BackgroundColor = StaticKeys.BUTTON_BACKGROUND_COLOR

            };


            btnCancel.Clicked += btnCancel_Clicked;



            var gridProperties = new Grid()
            {
                //BackgroundColor = Color.Aqua,
                Padding = 5,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                // BackgroundColor = Device.OnPlatform(Color.FromHex("363636"), Color.Default, Color.Default),
                RowDefinitions = 
                        {
                            new RowDefinition { Height = GridLength.Auto }

                   
                        },
                ColumnDefinitions = {
                  

                             new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
                            new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},


                    }

            };


            gridProperties.Children.Add(btnAdd, 0, 0);
            gridProperties.Children.Add(btnCancel, 1, 0);



            var popup = new StackLayout
            {
                WidthRequest = Device.OnPlatform(250, 250, 250), // 250
                HeightRequest = Device.OnPlatform(150, 150, 250), //150,
                BackgroundColor = StaticKeys.POPUP_BACKGROUND_COLOR,
                Orientation = StackOrientation.Vertical,
                //VerticalOptions = LayoutOptions.FillAndExpand,
                //HorizontalOptions = LayoutOptions.FillAndExpand,
                Children = { lbl, txtProperties, gridProperties },// btnAdd, btnCancel },
                Padding = new Thickness(5, 5, 5, 5)

            };

            //_PopUpLayout.ShowPopup(popup, buttonProperties, PopupLayout.PopupLocation.Bottom);


            _PopUpLayout.ShowPopup(popup);//, buttonProperties, PopupLayout.PopupLocation.Bottom ); 

        }

        void btnCancel_Clicked(object sender, EventArgs e)
        {
            _PopUpLayout.DismissPopup();
        }

        #endregion


    

        private string INFO_DESC = "Set Ocean name";
        private string INFO_SUBJECT = "Set Subject";
      



        void buttonAdd_Clicked(object sender, EventArgs e)
        {

            // add to WebService

            if (txtProperties.Text != null)
            {
                propertyName = txtProperties.Text.Trim();

                if (propertyName == "")
                {
                    DisplayAlert(StaticKeys.INFO_TITLE, INFO_DESC, StaticKeys.OK_BTN_TITLE);
                    return;
                }
            }
            else
            {
                propertyName = "";
                DisplayAlert(StaticKeys.INFO_TITLE, INFO_DESC, StaticKeys.OK_BTN_TITLE);
                return;
            }

            PropertyWS p = new PropertyWS(-1, propertyName);

            pickerProperties.Items.Add(propertyName);


            listProperties.Add(p);

            int index = 0;

            for (int i = 0; i < pickerProperties.Items.Count; i++)
            {

                if (pickerProperties.Items[i] + "" == propertyName)
                {
                    index = i;
                    break;
                }

            }
            pickerProperties.SelectedIndex = index;

            _PopUpLayout.DismissPopup();



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

        #region REPORT BUTTON
        void buttonReport_Clicked(object sender, EventArgs e)
        {



            if (txtSubject.Text != null)
            {
                if (txtSubject.Text.Trim() == "")
                {
                    DisplayAlert(StaticKeys.INFO_TITLE, INFO_SUBJECT, StaticKeys.OK_BTN_TITLE);
                    return;

                }
            }
            else
            {
                DisplayAlert(StaticKeys.INFO_TITLE, INFO_SUBJECT, StaticKeys.OK_BTN_TITLE);
                return;
            }





            if (_latitude == null)
            {
                if (_gps != null)
                {
                    if (_gps.PositionLatitude != null)
                    {
                        try
                        {
                            _latitude = Convert.ToDouble(_gps.PositionLatitude);
                        }
                        catch
                        {
                            //double[] latarr = this.mediaFile.Exif.GpsLatitude;
                        }
                    }
                }
            }

            if (_longitude == null)
            {
                if (_gps != null)
                {
                    if (_gps.PositionLongitude != null)
                    {
                        try
                        {
                            _longitude = Convert.ToDouble(_gps.PositionLongitude);
                        }
                        catch
                        {
                        }
                    }
                }
            }

            int? prop = null;
            string propName = "";
            if (pickerProperties.SelectedIndex >= 0)
            {

                PropertyWS p = listProperties[pickerProperties.SelectedIndex];
                if (p.ID > 0)
                    prop = p.ID;
                propName = p.Name;

            }
            else
            {
                if (txtProperties != null)
                    propName = txtProperties.Text;
            }

            NewIssue newissue = new NewIssue(txtSubject.Text, txtMessage.Text, _resizedImage, Guid.NewGuid().ToString() + ".jpg", _latitude, _longitude, null, null,
                     "android", prop + "", propName);

            Navigation.PushAsync(new MyDataScreen(newissue, _gps));


        }

        #endregion



        #region GET NEARTES BUTTON CLICK

        void buttonNearest_Clicked(object sender, EventArgs e)
        {
            if (_latitude == null)
            {
                if (_gps != null)
                {
                    if (_gps.PositionLatitude != null)
                    {
                        try
                        {
                            _latitude = Convert.ToDouble(_gps.PositionLatitude);
                        }
                        catch
                        {
                            //double[] latarr = this.mediaFile.Exif.GpsLatitude;
                        }
                    }
                }
            }

            if (_longitude == null)
            {
                if (_gps != null)
                {
                    if (_gps.PositionLongitude != null)
                    {
                        try
                        {
                            _longitude = Convert.ToDouble(_gps.PositionLongitude);
                        }
                        catch
                        {
                        }
                    }
                }
            }

            if (_latitude != null && _longitude != null)
                Navigation.PushAsync(new ListNearByIssues(_gps, (double)_latitude, (double)_longitude));
            else
                DisplayAlert("Erro", "Não foi possível obter as ocorrências.", "OK");
        }
        #endregion


    }
}
