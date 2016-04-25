using PinFixed.BL;
using PinFixed.UI;
using Plugin.Settings;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using Xamarin.Forms;
using XLabs.Enums;
using XLabs.Forms.Controls;

namespace PinFixed
{

    // Settings ui for nearest items


    public class SettingsPage : ContentPage
    {

        RoundButton buttonSave;

        Picker pickerMeasure;

        Picker pickerRadius;
        //ExtendedSlider slider;

        //Label labelCurrentValue;
        public SettingsPage()
        {
         
            #region COPIED FROM TAKE PIC

            NavigationPage.SetHasNavigationBar(this, false);
            var relativeLayout = new RelativeLayout();
            relativeLayout.HorizontalOptions = LayoutOptions.FillAndExpand;
            relativeLayout.VerticalOptions = LayoutOptions.Start;
            relativeLayout.BackgroundColor = StaticKeys.TOPBAR_BACKGROUND_COLOR;
            

            var labelTitle = new Label();
            labelTitle.Text = "Settings";
            labelTitle.TextColor = Device.OnPlatform( StaticKeys.LABEL_TEXT_COLOR,  StaticKeys.LABEL_TEXT_COLOR, Color.Default);
            labelTitle.FontSize = 20;




            var labelHelp = new Label();
            labelHelp.Text = "Configure radius for Fish Species Reports Search:\nMeasure- in meters (m) or Kilometers (km)";
            labelHelp.TextColor = Device.OnPlatform(StaticKeys.LABEL_TEXT_COLOR, StaticKeys.LABEL_TEXT_COLOR, Color.Default);//StaticKeys.LABEL_TEXT_COLOR;
            labelHelp.FontSize = 12;



            #region BUTTON BACK


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
                    Navigation.PopAsync();
                };

                b.Image = "back.png";
                b.Source = ImageSource.FromFile("back.png");


               


                relativeLayout.Children.Add(b, Constraint.RelativeToParent((parent) =>
                {
                    return 10;
                }), Constraint.Constant(Device.OnPlatform(20, 10, 10)));



            }
            else
            {

                var homeTapRecognizer = new TapGestureRecognizer
                {
                    Command = new Command(() =>
                    {
                        Navigation.PopAsync();


                    }),
                    NumberOfTapsRequired = 1
                };

                var homeImg = new Image();
                homeImg.Source = "back.png";




                homeImg.GestureRecognizers.Add(homeTapRecognizer);
                //relativeLayout.Children.Add(homeImg, Constraint.RelativeToParent((parent) =>
                //{
                //    return 0;
                //}));

                relativeLayout.Children.Add(homeImg, Constraint.RelativeToParent((parent) =>
                {
                    return 10;
                }), Constraint.Constant(Device.OnPlatform(20, 10, 10)));

            }
            #endregion




            #region SETTINGS
            int radius = CrossSettings.Current.GetValueOrDefault(StaticKeys.APP_PREFIX + "Radius", 100);
            string type = CrossSettings.Current.GetValueOrDefault(StaticKeys.APP_PREFIX + "Measure", "m");


            var labelMeasure = new Label();
            labelMeasure.Text = "Measure";
            labelMeasure.TextColor = Device.OnPlatform(StaticKeys.LABEL_TEXT_COLOR, StaticKeys.LABEL_TEXT_COLOR, Color.Default); //StaticKeys.LABEL_TEXT_COLOR;
            pickerMeasure = new Picker();

            pickerMeasure.Items.Add("m");
            pickerMeasure.Items.Add("km");




            var labelRadius = new Label();
            labelRadius.Text = "Radius";
            labelRadius.TextColor = Device.OnPlatform(StaticKeys.LABEL_TEXT_COLOR, StaticKeys.LABEL_TEXT_COLOR, Color.Default); //StaticKeys.LABEL_TEXT_COLOR;

            pickerRadius = new Picker();
            if (type != "m")
            {
                pickerMeasure.SelectedIndex = 1;
                pickerRadius.Items.Add("1");
                pickerRadius.Items.Add("2");
                pickerRadius.Items.Add("3");
                pickerRadius.Items.Add("4");
                pickerRadius.Items.Add("5");
                pickerRadius.Items.Add("6");
                pickerRadius.Items.Add("7");
                pickerRadius.Items.Add("8");
                pickerRadius.Items.Add("9");
                pickerRadius.Items.Add("10");
                pickerRadius.Items.Add("All");





                //slider = new ExtendedSlider
                //{

                //    Maximum = 5.0f,
                //    Minimum = 1.0f,
                //    Value = 1.0f,
                //    StepValue = 1.0f,
                //    HorizontalOptions = LayoutOptions.FillAndExpand,
                //};
            }
            else
            {
                pickerMeasure.SelectedIndex = 0;


                pickerRadius.Items.Add("100");
                pickerRadius.Items.Add("200");
                pickerRadius.Items.Add("300");
                pickerRadius.Items.Add("400");
                pickerRadius.Items.Add("500");
                pickerRadius.Items.Add("600");
                pickerRadius.Items.Add("700");
                pickerRadius.Items.Add("800");
                pickerRadius.Items.Add("900");
                pickerRadius.Items.Add("1000");
                pickerRadius.Items.Add("All");

                //slider = new ExtendedSlider
                //{

                //    Maximum = 900.0f,
                //    Minimum = 100.0f,
                //    Value = 100.0f,
                //    StepValue = 100.0f,
                //    HorizontalOptions = LayoutOptions.FillAndExpand,
                //};

            }

            if (radius + "" == "-1") // tudo
            {
                
                    pickerRadius.SelectedIndex = 10;
                
            }
            else
            { 
                int index = 0;
                for (int i = 0; i < pickerRadius.Items.Count; i++)
                {

                    if (pickerRadius.Items[i] + "" == radius + "")
                    {
                        index = i;
                        break;
                    }

                }
                pickerRadius.SelectedIndex = index;
            }


            buttonSave = new RoundButton
            {
                Text = "Save Settings",

                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                //WidthRequest = 250,
                //HeightRequest = 60,

                WidthRequest = Device.OnPlatform(250, 250, 300),
                HeightRequest = Device.OnPlatform(60, 60, 80),
                Stroke = Color.Black,
                StrokeThickness = 50,
                CornerRadius = 50,
                FontSize = 20,
                TextColor = StaticKeys.BUTTON_TEXT_COLOR,
                BackgroundColor = StaticKeys.BUTTON_BACKGROUND_COLOR
            };



            //<me:RoundButton x:Name="rbv" HorizontalOptions="Center" VerticalOptions="CenterAndExpand" WidthRequest="120" HeightRequest="50"
            //        Stroke="Green" StrokeThickness="50" CornerRadius="50"
            //              Text="Reportar" FontSize="20" TextColor="Black"
            //       BackgroundColor="White" Clicked="OnButtonClicked"  />


            buttonSave.Clicked += buttonSend_Clicked;
            buttonSave.IsEnabled = true;

            var stack = new StackLayout
            {

                Padding = new Thickness(Device.OnPlatform(10, 0, 0), Device.OnPlatform(30, 0, 0), Device.OnPlatform(10, 0, 0), 0),
                Children = {labelTitle, labelHelp, labelMeasure, pickerMeasure, labelRadius, pickerRadius, buttonSave }

            };

            #endregion
          


            stack.Spacing = 5;
            stack.Padding = 10;
            this.BackgroundColor = Device.OnPlatform(Color.White, Color.White, Color.Default);

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


         



            Content = new StackLayout
            {
               
                Children = 
                    {
					    relativeLayout, stack
				    }
            };

            //this.Padding = new Thickness(Device.OnPlatform(10, 0, 0), Device.OnPlatform(30, 0, 0), Device.OnPlatform(10, 0, 0), 0);

            

            #endregion

            pickerMeasure.SelectedIndexChanged +=pickerMeasure_SelectedIndexChanged;


        }

        void pickerMeasure_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (pickerMeasure.SelectedIndex == 1)
            {
                pickerRadius.Items.Clear();
                pickerRadius.Items.Add("1");
                pickerRadius.Items.Add("2");
                pickerRadius.Items.Add("3");
                pickerRadius.Items.Add("4");
                pickerRadius.Items.Add("5");
                pickerRadius.Items.Add("6");
                pickerRadius.Items.Add("7");
                pickerRadius.Items.Add("8");
                pickerRadius.Items.Add("9");
                pickerRadius.Items.Add("10");
                pickerRadius.Items.Add("All");

                //slider.Minimum = 1.0f;

                //slider.Maximum = 5.0f;
                //slider.Value = 1.0f;
                //slider.StepValue = 1.0f;

             

            }
            else
            {
                pickerRadius.Items.Clear();
                pickerRadius.Items.Add("100");
                pickerRadius.Items.Add("200");
                pickerRadius.Items.Add("300");
                pickerRadius.Items.Add("400");
                pickerRadius.Items.Add("500");
                pickerRadius.Items.Add("600");
                pickerRadius.Items.Add("700");
                pickerRadius.Items.Add("800");
                pickerRadius.Items.Add("900");
                pickerRadius.Items.Add("1000");
                pickerRadius.Items.Add("All");

          


            }
        }

        void buttonSend_Clicked(object sender, EventArgs e)
        {
            // save settings

            int radius = -1;

            try
            {
                radius = Convert.ToInt32(pickerRadius.Items[pickerRadius.SelectedIndex]);
            }
            catch (Exception)
            {
            }

            CrossSettings.Current.AddOrUpdateValue(StaticKeys.APP_PREFIX + "Radius", radius);

            CrossSettings.Current.AddOrUpdateValue(StaticKeys.APP_PREFIX + "Measure", pickerMeasure.Items[pickerMeasure.SelectedIndex]);

            Navigation.PopAsync();
        }
    }
}