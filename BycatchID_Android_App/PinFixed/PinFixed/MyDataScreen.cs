﻿using Newtonsoft.Json.Linq;
using PCLCrypto;
using PinFixed.BL;
using Plugin.Settings;
//using Refractored.Xam.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.ServiceModel;
using System.Text;
using Xamarin.Forms;
using XLabs.Enums;
using XLabs.Forms.Controls;
using XLabs.Ioc;
using XLabs.Platform.Device;



namespace PinFixed
{
    public class MyDataScreen : ContentPage
    {
        Entry txtName;
        Entry txtEmail;
        //Entry txtPassword;
        Entry txtPhone;

        private NewIssue newIssue;
        private GPSUtils _gps;

        ActivityIndicator activityIndicator;
        RoundButton buttonSend;
        protected override bool OnBackButtonPressed()
        {
            return base.OnBackButtonPressed();
        }

        public MyDataScreen(NewIssue issue, GPSUtils gps)
        {
            newIssue = issue;
            _gps = gps;


            activityIndicator = new ActivityIndicator
            {

                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Color = Color.Black,
                IsVisible = false
            };



            NavigationPage.SetHasNavigationBar(this, false);


            var relativeLayout = new RelativeLayout();
            relativeLayout.BackgroundColor = StaticKeys.TOPBAR_BACKGROUND_COLOR;

            relativeLayout.HorizontalOptions = LayoutOptions.FillAndExpand;
            relativeLayout.VerticalOptions = LayoutOptions.Start;

            #region BUTTON BACK
            ImageButton bbak = new ImageButton();
            var backImg = new Image();
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
                        Navigation.PopAsync();

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
                        //Navigation.PushAsync(new StartPage(gps));
                        Navigation.PopAsync();

                    }),
                    NumberOfTapsRequired = 1
                };

                backImg = new Image();
                backImg.Source = "back.png";

                backImg.GestureRecognizers.Add(backTapRecognizer);
                //relativeLayout.Children.Add(backImg, Constraint.RelativeToParent((parent) =>
                //{
                //    return 0;
                //}));


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
                    Navigation.PushAsync(new StartPage(gps));
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
                        Navigation.PushAsync(new StartPage(gps));

                    }),
                    NumberOfTapsRequired = 1
                };

                var homeImg = new Image();
                homeImg.Source = "btn_home.png";

                homeImg.GestureRecognizers.Add(homeTapRecognizer);

                Func<RelativeLayout, double> getBackWitdh = (parent) => backImg.GetSizeRequest(relativeLayout.Width, relativeLayout.Height).Request.Width;
                relativeLayout.Children.Add(homeImg,
                    Constraint.RelativeToParent((r) => getBackWitdh(r) + 10),
                    Constraint.Constant(10));
            }
            #endregion


            int idAux = CrossSettings.Current.GetValueOrDefault(StaticKeys.APP_PREFIX + "repid", 0);
            if (idAux > 0)
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
                        GetDataAndGotoList();
                    };

                    b.Image = "btn_lista.png";
                    b.Source = ImageSource.FromFile("btn_lista.png");

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
                        Constraint.Constant(10));

                }
                #endregion

            }




            var labelName = new Label();
            labelName.Text = "Name";
            labelName.TextColor = StaticKeys.LABEL_TEXT_COLOR;
            txtName = new Entry();

            var labelEmail = new Label();
            labelEmail.Text = "Email";
            labelEmail.TextColor = StaticKeys.LABEL_TEXT_COLOR;
            txtEmail = new Entry();
            txtEmail.Keyboard = Keyboard.Email;


            //var labelPassword = new Label();
            //labelPassword.Text = "Senha";
            //labelPassword.TextColor = Device.OnPlatform(Color.White, Color.Default, Color.Default);
            //txtPassword = new Entry();
            //txtPassword.IsPassword = true;

            var labelPhone = new Label();
            labelPhone.Text = "Phone";
            labelPhone.TextColor = StaticKeys.LABEL_TEXT_COLOR;
            txtPhone = new Entry();
            txtPhone.Keyboard = Keyboard.Telephone;

       



            buttonSend = new RoundButton
            {
                Text = "Send question",

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

            buttonSend.Clicked += buttonSend_Clicked;
            buttonSend.IsEnabled = true;

            #region debug purposes




            txtName.Text = CrossSettings.Current.GetValueOrDefault(StaticKeys.APP_PREFIX + "Name", "");
            txtEmail.Text = CrossSettings.Current.GetValueOrDefault(StaticKeys.APP_PREFIX + "Email", ""); //"CrossSettings.Current.GetValueOrDefault("repid", 0);";

            

            txtPhone.Text = CrossSettings.Current.GetValueOrDefault(StaticKeys.APP_PREFIX + "Phone", "");


            var stack = new StackLayout();

            stack.Spacing = 5;
            stack.Padding = 10;

            stack.Children.Add(labelName);
            stack.Children.Add(txtName);
            stack.Children.Add(labelEmail);
            stack.Children.Add(txtEmail);
            //stack.Children.Add(labelPassword);
            //stack.Children.Add(txtPassword);
            stack.Children.Add(labelPhone);
            stack.Children.Add(txtPhone);
            stack.Children.Add(buttonSend);

            #endregion
            //Content = new StackLayout
            //{
            //    Children = {
            //        labelName,
            //        txtName,
            //        labelEmail,
            //        txtEmail,
            //        labelPassword,
            //        txtPassword,
            //        labelPhone,
            //        txtPhone,
            //        buttonSend
            //    }
            //};
            //var constraintY = Constraint.RelativeToParent(parent => parent.Y + homeImg.Y + homeImg.Height + 20);

            //relativeLayout.Children.Add(stack,

            //    Constraint.Constant(0),
            //    //Constraint.Constant(0));
            //    constraintY,

            //    widthConstraint: Constraint.RelativeToParent((parent) =>
            //    {
            //        return parent.Width;
            //    }),
            //    heightConstraint: Constraint.RelativeToParent((parent) =>
            //    {
            //        return parent.Height;
            //    }));


            ScrollView sc = new ScrollView();
            sc.Orientation = ScrollOrientation.Vertical;
            sc.VerticalOptions = LayoutOptions.FillAndExpand;

            sc.Content = stack;

            // Content = relativeLayout;


            relativeLayout.Padding =  new Thickness(0, Device.OnPlatform(0, 10, 0), 0, 0);
            Content = new StackLayout
            {
                Children = 
                    {
                        relativeLayout, sc, activityIndicator
                    }
            };

            //Content = new StackLayout
            //{
            //    Padding = new Thickness(0, Device.OnPlatform(20, 0, 0), 0, 0),
            //    BackgroundColor = Device.OnPlatform(Color.FromHex("363636"), Color.Default, Color.Default),
            //    Children = 
            //        {
            //            relativeLayout
            //        }
            //};


            //this.Padding = new Thickness(Device.OnPlatform(10, 0, 0), Device.OnPlatform(30, 0, 0), Device.OnPlatform(10, 0, 0), 0);
            //this.BackgroundColor = Device.OnPlatform(Color.FromHex("363636"), Color.Default, Color.Default);
            //this.BackgroundColor = Color.White;
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


        void buttonSend_Clicked(object sender, EventArgs e)
        {

            buttonSend.IsEnabled = false;
            IssuesWebServiceSoapClient client = new IssuesWebServiceSoapClient(
                        new BasicHttpBinding(),
                        new EndpointAddress(StaticKeys.WebServiceUrl));


            client.ReportIssueAndRegisterUserCompleted += OnGotResultReport;

            //byte[] file = StaticMethods.ReadFully(newIssue.MediaFile);
            decimal? lat = Convert.ToDecimal(newIssue.Latitude);

            decimal? ln = Convert.ToDecimal(newIssue.Longitude);

            //if (newIssue.Latitude != null && newIssue.Longitude != null)
            //{
            //    string latStr = newIssue.Latitude + "";

            //    string lonStr = newIssue.Longitude + "";

            //    lat = Convert.ToDecimal(latStr.Replace(".", ","));
            //    ln = Convert.ToDecimal(lonStr.Replace(".", ","));

            //}


            IDevice device = Resolver.Resolve<IDevice>();



            activityIndicator.IsRunning = true;
            activityIndicator.IsVisible = true;
            int? prop = null;


            if (newIssue.PropertyID + "" != "")
                prop = Convert.ToInt32(newIssue.PropertyID);





            client.ReportIssueAndRegisterUserAsync(null, txtName.Text, txtEmail.Text, "", txtPhone.Text, null,
                                                   newIssue.Subject, newIssue.Message, newIssue.FileName, newIssue.ResizedImage, "image/jpeg",
                                                   newIssue.ResizedImage.Length, lat, ln, newIssue.TypeID, newIssue.Orientation, device.Name, prop, newIssue.PropertyName);
            

        }

        void OnGotResultReport(object sender, ReportIssueAndRegisterUserCompletedEventArgs e)
        {
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
                    buttonSend.IsEnabled = true;
                }
                else
                {

                    var obj = JObject.Parse(e.Result);
                    var issue = (JArray)obj["Data"];
                    if (issue != null)
                    {
                        await DisplayAlert(StaticKeys.INFO_TITLE, "Request sent succcessfully", StaticKeys.OK_BTN_TITLE);

                        var id = Convert.ToInt32(issue[0]["ReportedBy"]);
                        CrossSettings.Current.AddOrUpdateValue(StaticKeys.APP_PREFIX + "repid", id);

                        CrossSettings.Current.AddOrUpdateValue(StaticKeys.APP_PREFIX + "Name", txtName.Text);
                        CrossSettings.Current.AddOrUpdateValue(StaticKeys.APP_PREFIX + "Email", txtEmail.Text);
                        //CrossSettings.Current.AddOrUpdateValue("P", txtPassword.Text);
                        CrossSettings.Current.AddOrUpdateValue(StaticKeys.APP_PREFIX + "Phone", txtPhone.Text);

                        buttonSend.IsEnabled = true;
                        activityIndicator.IsRunning = false;
                        activityIndicator.IsVisible = false;

                        await Navigation.PushAsync(new ListIssuesScreen(_gps, id));


                        StaticMethods.GetPropeties();
                    
                    }

                    else
                    {
                        //var err = (JArray)obj["Error"];
                        await DisplayAlert(StaticKeys.ERROR_TITLE, "Could not report request. Try again later", StaticKeys.OK_BTN_TITLE);
                        buttonSend.IsEnabled = true;
                    }
                    //resultsLabel.Text = e.Result;
                }
            });
        }

    }
}
