using PinFixed.BL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using Xamarin.Forms;
using XLabs.Enums;
using XLabs.Forms.Controls;

namespace PinFixed
{
    public class FullImage : ContentPage
    {
        public FullImage(string src)
        {
            NavigationPage.SetHasNavigationBar(this, false);

            var relativeLayout = new RelativeLayout();
            relativeLayout.HorizontalOptions = LayoutOptions.FillAndExpand;
            relativeLayout.VerticalOptions = LayoutOptions.Start;
            relativeLayout.Padding = new Thickness(0, Device.OnPlatform(0, 10, 0), 0, 0);

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
                    Navigation.PopAsync();
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
                        Navigation.PopAsync();

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



            Image image = new Image();

            image.Source = src;
      
            Content = new StackLayout
            {
                Children = {
                    //stack
					relativeLayout, image
				}
            };
        }
    }
}
