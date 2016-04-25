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
using XLabs.Forms.Controls;

namespace PinFixed
{

    // Simple Issue
    class IssueCell : ViewCell
    {
        Label stateLabel;
        Label dateLabel;

        Label subjectLabel;
     

        Page currPage;

        public IssueCell()
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
                FontFamily = Device.OnPlatform("HelveticaNeue-Medium", "Droid Sans Mono", WinPhone: "Segoe UI"),
                FontSize = Device.OnPlatform(14, 14, 20),
                //FontSize = 14,
                TextColor = Device.OnPlatform(Color.Black, Color.Black, Color.White)
            };
            subjectLabel.SetBinding(Label.TextProperty, "Subject");




            var areaLabel = new Label()
            {
                FontFamily = Device.OnPlatform("HelveticaNeue-Medium", "Droid Sans Mono", WinPhone: "Segoe UI"),
                FontSize = Device.OnPlatform(12, 12, 18),
                //FontSize = 12,
                TextColor = Color.FromHex("#666")
            };
            areaLabel.SetBinding(Label.TextProperty, "TypeNameFormatted");



            dateLabel = new Label()
            {
                FontFamily = Device.OnPlatform("HelveticaNeue-Medium", "Droid Sans Mono", WinPhone: "Segoe UI"),
                FontSize = Device.OnPlatform(12, 12, 18),
                //FontSize = 12,
                TextColor = Color.FromHex("#666")
            };
            dateLabel.SetBinding(Label.TextProperty, "DateFormated");



            //// Vet rating label and star image
            //stateLabel = new Label()
            //{
            //    FontFamily = Device.OnPlatform("HelveticaNeue-Medium", "Droid Sans Mono", WinPhone: "Segoe UI"),
            //    FontSize = Device.OnPlatform(12, 12, 18),
            //    //FontSize = 12,
            //    TextColor = Color.FromHex("#666")
            //};

            //stateLabel.SetBinding(Label.TextProperty, "StateName");

       

            //var ratingStack = new StackLayout()
            //{
            //    Spacing = 3,
            //    Orientation = StackOrientation.Horizontal,
            //    Children = {  stateLabel }
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
                Children = { subjectLabel, areaLabel, statusLayout}
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





            var grid = new Grid()
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
                    //new ColumnDefinition { Width =  GridLength.Auto  },
                    // new ColumnDefinition { Width = 30 }

                     new ColumnDefinition {Width = new GridLength(5, GridUnitType.Star)},
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},


                }

            };


            grid.Children.Add(cellLayout, 0, 0);
          

            this.View = grid;
            


            #region GOTO REPORT

            var tapRecognizer = new TapGestureRecognizer
            {
                Command = new Command(() =>
                {
                    Issue issue = ((Issue)BindingContext);


                    App.Navigation.PushAsync(new ReportScreen(null, issue, ((ListIssuesScreen)currPage).GPS, null, null));

                    //Navigation.PushAsync(new ReportScreen(null, issue, gps, null, null));

                }),
                NumberOfTapsRequired = 1
            };




            image.GestureRecognizers.Add(tapRecognizer);

            #endregion

   

      
        }

        StackLayout CreateNameLayout()
        {

            subjectLabel = new Label
            {
                HorizontalOptions = LayoutOptions.FillAndExpand
            };


            subjectLabel.FontFamily = Device.OnPlatform ("Verdana", "Droid Sans Mono", WinPhone: "Comic Sans MS");
            subjectLabel.FontSize = Device.OnPlatform(
                12,
                Device.GetNamedSize(NamedSize.Medium, subjectLabel),
                Device.GetNamedSize(NamedSize.Large, subjectLabel)
            );

            subjectLabel.SetBinding(Label.TextProperty, "SubjectFormatted");
            subjectLabel.TextColor = Device.OnPlatform(Color.White, Color.Default, Color.Default);



            var dateLabel = new Label
            {
                HorizontalOptions = LayoutOptions.FillAndExpand

            };

            dateLabel.FontFamily = Device.OnPlatform("Verdana", "Droid Sans Mono", WinPhone: "Comic Sans MS");
            dateLabel.FontSize = Device.OnPlatform(
                10,
                Device.GetNamedSize(NamedSize.Small, dateLabel),
                Device.GetNamedSize(NamedSize.Large, dateLabel)
            );



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

            stateLabel.FontFamily = Device.OnPlatform("Verdana", "Droid Sans Mono", WinPhone: "Comic Sans MS");
            stateLabel.FontSize = Device.OnPlatform(
                10,
                Device.GetNamedSize(NamedSize.Small, stateLabel),
                Device.GetNamedSize(NamedSize.Large, stateLabel)
            );


            stateLabel.TextColor = Device.OnPlatform(Color.White, Color.Default, Color.Default);


         




            var ll = new StackLayout()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Orientation = StackOrientation.Horizontal,
                Children = {  stateLabel }
            };
            var Padding = new Thickness(10, 0, 10, 0);
            ll.Padding = Padding;
            return ll;
        }







    }
}
