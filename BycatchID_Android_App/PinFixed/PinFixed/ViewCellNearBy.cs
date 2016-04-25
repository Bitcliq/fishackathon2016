using PinFixed.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XLabs.Forms.Controls;

namespace PinFixed
{
    public class ViewCellNearBy: ViewCell
    {
        Label stateLabel;

        Label subjectLabel;
        //Image imageDelete;
        Image imageState;
        Page currPage;

        public ViewCellNearBy()
        {



            if (Application.Current.MainPage.Navigation.NavigationStack.Count > 0)
            {
                int index = Application.Current.MainPage.Navigation.NavigationStack.Count - 1;
                currPage = Application.Current.MainPage.Navigation.NavigationStack[index];

            }

            var image = new PinFixed.UI.CircleImage
            {

                BorderColor = StaticKeys.IMAGE_BORDER_COLOR,
                //FillColor = Color.Olive,
                BorderThickness = 1,
                HeightRequest = 75,
                WidthRequest = 75,
                Aspect = Aspect.AspectFill,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center

            };



            image.SetBinding(Image.SourceProperty, new Binding("PhotoThumbnailUrl"));
            //image.WidthRequest = 70;
            //image.HeightRequest = 70;


            subjectLabel = new Label()
            {
                FontAttributes = FontAttributes.Bold,
                //FontFamily = "HelveticaNeue-Medium",
                FontFamily = Device.OnPlatform("HelveticaNeue-Medium", "Droid Sans Mono", WinPhone: "Comic Sans MS"),
                FontSize = 14,
                TextColor = Color.Black
            };
            subjectLabel.SetBinding(Label.TextProperty, "Subject");




            var areaLabel = new Label()
            {
                FontFamily = Device.OnPlatform("HelveticaNeue-Medium", "Droid Sans Mono", WinPhone: "Comic Sans MS"),
                FontSize = 12,
                TextColor = Color.FromHex("#666")
            };
            areaLabel.SetBinding(Label.TextProperty, "TypeNameFormatted");



           var  dateLabel = new Label()
            {
                FontFamily = Device.OnPlatform("HelveticaNeue-Medium", "Droid Sans Mono", WinPhone: "Comic Sans MS"),
                FontSize = 12,
                TextColor = Color.FromHex("#666")
            };
            dateLabel.SetBinding(Label.TextProperty, "DateFormated");



           // // Vet rating label and star image
           //var stateLabel = new Label()
           // {
           //     FontFamily = Device.OnPlatform("HelveticaNeue-Medium", "Droid Sans Mono", WinPhone: "Comic Sans MS"),
           //     FontSize = 12,
           //     TextColor = Color.FromHex("#666")
           // };

           // stateLabel.SetBinding(Label.TextProperty, "StateName");

           // imageState = new Image()
           // {
           //     Source = "star.png",
           //     HeightRequest = 12,
           //     WidthRequest = 12
           // };

            //var ratingStack = new StackLayout()
            //{
            //    Spacing = 3,
            //    Orientation = StackOrientation.Horizontal,
            //    Children = { imageState, stateLabel }
            //};

            var statusLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Children = { dateLabel }
            };


            var vetDetailsLayout = new StackLayout
            {
                Padding = new Thickness(10, 0, 0, 0),
                Spacing = 0,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Children = { subjectLabel, areaLabel, statusLayout }
            };

         



            var cellLayout = new StackLayout
            {
                Spacing = 0,
                Padding = new Thickness(10, 5, 10, 5),
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.FillAndExpand,

                Children = { image, vetDetailsLayout }
            };

            // this.View = cellLayout;








            this.View = cellLayout;


            //var viewLayout = new StackLayout()
            //{
            //    Orientation = StackOrientation.Horizontal,
            //    VerticalOptions = LayoutOptions.FillAndExpand,
            //    HorizontalOptions = LayoutOptions.FillAndExpand,

            //    Children = { image, nameLayout },
            //    Padding = new Thickness(10, 0, 0, 10),


            //};


            #region GOTO REPORT

            var tapRecognizer = new TapGestureRecognizer
            {
                Command = new Command(() =>
                {
                    Issue issue = ((Issue)BindingContext);


                    App.Navigation.PushAsync(new ReportScreen(null, issue, ((ListNearByIssues)currPage).GPS, null, null));

                    //Navigation.PushAsync(new ReportScreen(null, issue, gps, null, null));

                }),
                NumberOfTapsRequired = 1
            };




            image.GestureRecognizers.Add(tapRecognizer);

            #endregion

            



        }

         StackLayout CreateNameLayout()
        {

            subjectLabel = new ExtendedLabel
            {
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            subjectLabel.SetBinding(Label.TextProperty, "SubjectFormatted");
            subjectLabel.TextColor = Device.OnPlatform(Color.White, Color.Default, Color.Default);



            var dateLabel = new Label
            {
                HorizontalOptions = LayoutOptions.FillAndExpand
                
            };

            dateLabel.SetBinding(Label.TextProperty, "DateFormated");
            dateLabel.TextColor = Device.OnPlatform(Color.White, Color.Default, Color.Default);
            var nameLayout = new StackLayout()
            {
              
                HorizontalOptions = LayoutOptions.FillAndExpand,

                Orientation = StackOrientation.Vertical,
               

                Children = { subjectLabel, dateLabel }
            };

            return nameLayout;
        }

         StackLayout CreateStateLayout()
        {

            
            stateLabel = new ExtendedLabel
            {
                HorizontalOptions = LayoutOptions.FillAndExpand

            };


            stateLabel.SetBinding(Label.TextProperty, "StateName");

          
            stateLabel.TextColor = Device.OnPlatform(Color.White, Color.Default, Color.Default);


            imageState = new Image
            {
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Start


            };
            imageState.Source = "novo.png";



            var ll = new StackLayout()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Orientation = StackOrientation.Horizontal,
                Children = { imageState, stateLabel }
            };
            var Padding = new Thickness(10, 0, 10, 0);
            ll.Padding = Padding;
            return ll;
        }

        





        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            string state = ((Issue)BindingContext).StateName.ToLower();
            if (state == "novo")
            {
                imageState.Source = "novo.png";
            }
            else if (state + "" == "em aberto")
            {
                imageState.Source = "em_aberto.png";
            }
            else if (state == "em resolução")
            {
                imageState.Source = "em_aberto.png";
            }

            else if (state + "" == "resolvido")
            {
               
                imageState.Source = "resolvido.png";
            }

            //if (state + "" != "novo")
            //{
             
            //    imageDelete.IsVisible = false;
              
            //}


           
        }
    }
}

