using PinFixed.BL;
using Plugin.Settings;
//using Refractored.Xam.Settings;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XLabs.Enums;
using XLabs.Forms.Controls;
using XLabs.Ioc;
using XLabs.Platform.Device;
using XLabs.Platform.Services;

namespace PinFixed
{
    /// <summary>    
    /// Start Screen
    /// </summary>     
    /// <returns></returns>    
    /// 


    public partial class StartPage : ContentPage
    {
        GPSUtils _gps;
        public StartPage(GPSUtils gps)
        {
            try
            {
                InitializeComponent();
                this._gps = gps;




                //StaticMethods.GetTypes();

                StaticMethods.GetPropeties();


                this.BackgroundImage = "bg_home.png";

                NavigationPage.SetHasNavigationBar(this, false);

                RelativeLayout rl = this.FindByName<RelativeLayout>("rl");

                #region BUTTON SETTINGS


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
                        Navigation.PushAsync(new SettingsPage());
                    };

                    b.Image = "icone_definicoes.png";
                    b.Source = ImageSource.FromFile("icone_definicoes.png");

                    rl.Children.Add(b, Constraint.RelativeToParent((parent) =>
                    {
                        return 0;
                    }));

                }

                else
                {
                    var settingsTapRecognizer = new TapGestureRecognizer
                    {
                        Command = new Command(() =>
                        {
                            Navigation.PushAsync(new SettingsPage());

                            //Navigation.PopAsync();
                        }),
                        NumberOfTapsRequired = 1
                    };




                    var settingsImg = new Image();
                    settingsImg.Source = "icone_definicoes.png";


                    //if (Device.OS == TargetPlatform.iOS)
                    //{
                    //    settingsImg.WidthRequest = 35;
                    //    settingsImg.HeightRequest = 35;

                    //}

                    settingsImg.GestureRecognizers.Add(settingsTapRecognizer);
                    rl.Children.Add(settingsImg, Constraint.RelativeToParent((parent) =>
                    {
                        return 0;
                    }), Constraint.Constant(10));


                }



                //rl.Children.Add(b, Constraint.Constant(50), Constraint.Constant(50), Constraint.Constant(200), Constraint.Constant(200));
                #endregion


                #region GET REPORTER

                int idAux = 0;


                idAux = CrossSettings.Current.GetValueOrDefault(StaticKeys.APP_PREFIX + "repid", 0);
               



                if (Device.OS == TargetPlatform.iOS)
                {
                    ImageButton b = new ImageButton();
                    b.Text = "   ";
                    b.Orientation = ImageOrientation.ImageToRight;
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
                    b.Source = "btn_lista";

                    Func<RelativeLayout, double> getLabelWidth = (parent) => b.GetSizeRequest(rl.Width, rl.Height).Request.Width;
                    rl.Children.Add(b,
                        Constraint.RelativeToParent((r) => r.Width - getLabelWidth(r)),
                        Constraint.Constant(0));

                }
                else
                {

                    #region BUTTON REPORT
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
                        NumberOfTapsRequired = 1,

                    };





                    Image img = new Image();
                    img.Source = "btn_lista.png";





                    img.GestureRecognizers.Add(profileTapRecognizer);




                    Func<RelativeLayout, double> getLabelWidth = (parent) => img.GetSizeRequest(rl.Width, rl.Height).Request.Width;
                    rl.Children.Add(img,
                        Constraint.RelativeToParent((r) => r.Width - getLabelWidth(r)), Constraint.Constant(10));

                    #endregion

                }




                #endregion


                #region BUTTON NEAR BY


                double lat = 0, lon = 0;

                bool canGetData = false;
                if (_gps != null)
                {
                    if (_gps.PositionLatitude + "" != "")
                    {
                        canGetData = true;
                        if (_gps.PositionLatitude + "" != "")
                        {
                            canGetData = true;


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
                    else
                        gps.GetPosition();
                }




                #region BUTTONS
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

                            Navigation.PushAsync(new ListNearByIssues(_gps, lat, lon));
                        }
                        else
                            DisplayAlert(StaticKeys.INFO_TITLE, StaticKeys.NO_LOCATION, StaticKeys.OK_BTN_TITLE);
                    };

                    b.Image = "icone_perto_de_mim.png";
                    b.Source = ImageSource.FromFile("icone_perto_de_mim.png");




                    Func<RelativeLayout, double> getImgWidth = (parent) => b.GetSizeRequest(rl.Width, rl.Height).Request.Width;
                    rl.Children.Add(b,
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

                                Navigation.PushAsync(new ListNearByIssues(_gps, lat, lon));
                            }
                            else
                                DisplayAlert(StaticKeys.INFO_TITLE, StaticKeys.NO_LOCATION, StaticKeys.OK_BTN_TITLE);
                        }),
                        NumberOfTapsRequired = 1
                    };

                    Image img = new Image();
                    img.Source = "icone_perto_de_mim.png";

                    img.GestureRecognizers.Add(profileTapRecognizer);






                    Func<RelativeLayout, double> getImgWidth = (parent) => img.GetSizeRequest(rl.Width, rl.Height).Request.Width;
                    rl.Children.Add(img,
                        Constraint.RelativeToParent((r) => r.Width - getImgWidth(r) - getImgWidth(r) - 10)
                       , Constraint.Constant(10));
                }

                #endregion

                #endregion

                rbv.TextColor = StaticKeys.BUTTON_TEXT_COLOR;
                rbv.BackgroundColor = StaticKeys.BUTTON_BACKGROUND_COLOR;
                this.Padding = new Thickness(Device.OnPlatform(10, 0, 0), Device.OnPlatform(30, 0, 0), Device.OnPlatform(10, 0, 0), 0);


                if (Device.OS == TargetPlatform.WinPhone)
                {
                    rbv.WidthRequest = 200;
                    rbv.HeightRequest = 80;
                }
                    


            }
            catch (Exception ex)
            {
                throw (ex);

            }


        }





        private void GetDataAndGotoList()
        {

            if (StaticMethods.CheckConnection())
            {
                int idAux = CrossSettings.Current.GetValueOrDefault(StaticKeys.APP_PREFIX + "repid", 0);
                if (idAux > 0)
                {
                    StaticMethods.GetTypes();

                    var id = Convert.ToInt32(idAux);
                    Navigation.PushAsync(new ListIssuesScreen(_gps, id));
                }
            }
            else
                DisplayAlert("Erro", "Para usar esta aplicação tem de ter acesso à internet", "OK");
        }



        void OnButtonClicked(object sender, EventArgs args)
        {


            if (StaticMethods.CheckConnection())
            {
                //Navigation.PushAsync(new MapPage(null, _gps, null));
                Navigation.PushAsync(new TakePictureScreen(_gps));
            }
            else
                DisplayAlert("Erro", "Para usar esta aplicação tem de ter acesso à internet", "OK");
        }



        void LoginClicked(object sender, EventArgs args)
        {


            if (StaticMethods.CheckConnection())
            {
                //Navigation.PushAsync(new MapPage(null, _gps, null));
                Navigation.PushAsync(new TakePictureScreen(_gps));
            }
        }

        protected override bool OnBackButtonPressed()
        {
            return base.OnBackButtonPressed();
        }


    }
}
