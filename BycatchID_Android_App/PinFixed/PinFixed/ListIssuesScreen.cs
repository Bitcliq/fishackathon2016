using Newtonsoft.Json.Linq;
using PinFixed.BL;
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
using XLabs.Platform.Services;

namespace PinFixed
{
    public class ListIssuesScreen : ContentPage
    {
        StackLayout stLayout;

        RelativeLayout relativeLayout;
        private GPSUtils _gps;
        Image img;

        ActivityIndicator activityIndicator;
        public ListView listView;


        public GPSUtils GPS
        {
            get { return _gps; }
        }



        private int _reportedBy;

        PopupLayout _PopUpLayout;

        //RoundButton buttonFilter;

        Image imgFilter;

        Picker pickerStates;
        Picker pickerOrder;
        Picker pickerAreas;
        Picker pickerProperties;
        List<TypesWS> listAreas = new List<TypesWS>();
        List<PropertyWS> listProperties = new List<PropertyWS>();
        List<Issue> list;

        string filteredState = "";
        string filteredOrder = "Descendente";
        string filteredArea = "";
        string filteredProperty = "";


        public ListIssuesScreen(GPSUtils gps, int reportedBy)
        {



            _reportedBy = reportedBy;
            _PopUpLayout = new PopupLayout();
            _gps = gps;

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
                //b.InputTransparent = true;
                //b.BorderRadius = 0;
                //b.CornerRadius = 0;
                //b.HasShadow = false;
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

            #region BUTTON HOME


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
                    Navigation.PushAsync(new StartPage(gps));
                };

                b.Image = "btn_home.png";
                b.Source = ImageSource.FromFile("btn_home.png");

                Func<RelativeLayout, double> getBackWitdh = (parent) => b.GetSizeRequest(relativeLayout.Width, relativeLayout.Height).Request.Width;
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

                Func<RelativeLayout, double> getBackWitdh = (parent) => img.GetSizeRequest(relativeLayout.Width, relativeLayout.Height).Request.Width;
                relativeLayout.Children.Add(homeImg,
                    Constraint.RelativeToParent((r) => getBackWitdh(r) + 10),
                    Constraint.Constant(10));

            }

            #endregion


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
                    Constraint.RelativeToParent((r) => r.Width - getLabelWidth(r) - 5),
                    Constraint.Constant(10));

            }



            #endregion

            #region LIST VIEW
            listView = new ListView
           {
               HasUnevenRows = true,
               ItemTemplate = new DataTemplate(typeof(IssueCell)),

               SeparatorColor = Color.FromHex("#74a633"),
               // ItemsSource = list,

               //RowHeight = 120
               //VerticalOptions = LayoutOptions.FillAndExpand,
               //HorizontalOptions = LayoutOptions.FillAndExpand,
               //BackgroundColor = Device.OnPlatform(Color.FromHex("363636"), Color.Default, Color.Default)
           };

            #endregion

            //#region FILTER

            ////#region BUTTON
            ////buttonFilter = new RoundButton
            ////{
            ////    Text = "Pesquisa",

            ////    WidthRequest = 120,
            ////    HeightRequest = 50,
            ////    Stroke = Color.Black,
            ////    StrokeThickness = 10,
            ////    CornerRadius = 1,
            ////    FontSize = 20,
            ////    TextColor = Color.White,
            ////    BackgroundColor = Color.FromHex("#74a633"),
            ////    BorderColor = Color.Black,



            ////};


            ////buttonFilter.Clicked += buttonFilter_Clicked;
            ////#endregion


            //if (Device.OS == TargetPlatform.iOS)
            //{
            //    ImageButton b = new ImageButton();
            //    b.Text = "   ";
            //    b.Orientation = ImageOrientation.ImageToLeft;
            //    //b.InputTransparent = true;
            //    //b.BorderRadius = 0;
            //    //b.CornerRadius = 0;
            //    //b.HasShadow = false;
            //    b.ImageHeightRequest = 50;
            //    b.ImageWidthRequest = 50;


            //    b.Clicked += (sender, args) =>
            //    {
            //        #region STATES
            //        var lblStates = new Label();
            //        lblStates.TextColor = StaticKeys.LABEL_TEXT_COLOR;
            //        lblStates.Text = "Estado";





            //        pickerStates = new Picker();
            //        pickerStates.Title = "-- Selecione --";
            //        pickerStates.Items.Add("Todos");
            //        pickerStates.Items.Add("Novo");
            //        pickerStates.Items.Add("Em aberto");
            //        pickerStates.Items.Add("Em resolução");
            //        pickerStates.Items.Add("Resolvido");

            //        if (filteredState + "" != "")
            //        {
            //            int index = 0;
            //            for (int i = 0; i < pickerStates.Items.Count; i++)
            //            {

            //                if (pickerStates.Items[i] + "" == filteredState + "")
            //                {
            //                    index = i;
            //                    break;
            //                }

            //            }
            //            pickerStates.SelectedIndex = index;
            //        }
            //        #endregion

            //        #region AREA
            //        var labelAreas = new Label();
            //        labelAreas.Text = "Área";
            //        labelAreas.TextColor = StaticKeys.LABEL_TEXT_COLOR;
            //        pickerAreas = new Picker();


            //        Dictionary<int, TypesWS> typesDic = new Dictionary<int, TypesWS>();
            //        typesDic = StaticMethods.typesDic;

            //        if (typesDic == null || typesDic.Count == 0)
            //        {

            //            StaticMethods.GetTypes();


            //        }
            //        pickerAreas.Title = "-- Selecione --";
            //        pickerAreas.Items.Add("Todas");
            //        foreach (var item in typesDic)
            //        {

            //            if (item.Value.ParentID == null)
            //            {
            //                pickerAreas.Items.Add(item.Value.Name);
            //                listAreas.Add(item.Value);
            //            }
            //        }

            //        #endregion


            //        #region PROPERTY

            //        var labelProperty = new Label();
            //        labelProperty.Text = "Propriedade";
            //        labelProperty.TextColor = StaticKeys.LABEL_TEXT_COLOR;
            //        pickerProperties = new Picker();


            //        Dictionary<int, PropertyWS> propsDic = new Dictionary<int, PropertyWS>();
            //        propsDic = StaticMethods.propertiesDic;

            //        if (propsDic == null || propsDic.Count == 0)
            //        {

            //            StaticMethods.GetPropeties();


            //        }
            //        pickerProperties.Title = "-- Selecione --";
            //        pickerProperties.Items.Add("Todas");

            //        foreach (var item in propsDic)
            //        {


            //            pickerProperties.Items.Add(item.Value.Name);
            //            listProperties.Add(item.Value);

            //        }

            //        #endregion

            //        #region ORDER
            //        var lblOrder = new Label();
            //        lblOrder.TextColor = StaticKeys.LABEL_TEXT_COLOR;
            //        lblOrder.Text = "Ordenação";




            //        pickerOrder = new Picker();
            //        pickerStates.Title = "-- Selecione --";

            //        pickerOrder.Items.Add("Descendente");
            //        pickerOrder.Items.Add("Ascendente");

            //        #endregion

            //        #region select default

            //        SelectItemInPicker(pickerOrder, filteredOrder);
            //        SelectItemInPicker(pickerAreas, filteredArea);
            //        SelectItemInPicker(pickerStates, filteredState);

            //        SelectItemInPicker(pickerProperties, filteredProperty);

            //        //if (filteredOrder + "" != "")
            //        //{

            //        //    for (int i = 0; i < pickerOrder.Items.Count; i++)
            //        //    {

            //        //        if (pickerOrder.Items[i] + "" == filteredOrder + "")
            //        //        {
            //        //            index = i;
            //        //            break;
            //        //        }

            //        //    }
            //        //    pickerOrder.SelectedIndex = index;
            //        //}
            //        //else
            //        //    pickerOrder.SelectedIndex = 0; // vêm na ordem descencente

            //        //index = 0;
            //        //if (filteredArea + "" != "")
            //        //{
            //        //    for (int i = 0; i < pickerAreas.Items.Count; i++)
            //        //    {

            //        //        if (pickerAreas.Items[i] + "" == filteredArea + "")
            //        //        {
            //        //            index = i;
            //        //            break;
            //        //        }

            //        //    }
            //        //    pickerAreas.SelectedIndex = index;
            //        //}
            //        //else
            //        //    pickerAreas.SelectedIndex = 0; // vêm na ordem descencente


            //        //index = 0;
            //        //if (filteredState + "" != "")
            //        //{
            //        //    for (int i = 0; i < pickerStates.Items.Count; i++)
            //        //    {

            //        //        if (pickerStates.Items[i] + "" == filteredState + "")
            //        //        {
            //        //            index = i;
            //        //            break;
            //        //        }

            //        //    }
            //        //    pickerStates.SelectedIndex = index;
            //        //}
            //        //else
            //        //    pickerStates.SelectedIndex = 0; // vêm na ordem descencente

            //        #endregion

            //        #region BUTTONS
            //        var btnFilter = new RoundButton
            //        {
            //            Text = "Pesquisar",

            //            WidthRequest = 120,
            //            HeightRequest = 50,
            //            Stroke = Color.Black,
            //            StrokeThickness = 10,
            //            CornerRadius = 1,
            //            FontSize = 20,
            //            TextColor = StaticKeys.BUTTON_TEXT_COLOR,
            //            BackgroundColor = StaticKeys.BUTTON_BACKGROUND_COLOR

            //        };

            //        //btnFilter.Text = "Fechar";
            //        btnFilter.Clicked += btnFilter_Clicked;
            //        btnFilter.WidthRequest = 100;

            //        #endregion


            //        #region POPUP
            //        var popup = new StackLayout
            //        {
            //            WidthRequest = 250,
            //            HeightRequest = 350,
            //            BackgroundColor = StaticKeys.POPUP_BACKGROUND_COLOR,
            //            Orientation = StackOrientation.Vertical,
            //            //VerticalOptions = LayoutOptions.FillAndExpand,
            //            //HorizontalOptions = LayoutOptions.FillAndExpand,
            //            Children = { lblStates, pickerStates, labelAreas, pickerAreas, labelProperty, pickerProperties, lblOrder, pickerOrder, btnFilter },
            //            Padding = new Thickness(5, 5, 5, 5)

            //        };

            //        //XLabs.Forms.Controls.PopupLayout pop = new XLabs.Forms.Controls.PopupLayout();
            //        //pop.Content = popup;

            //        //pop.ShowPopup(popup, Constraint.Constant(10), Constraint.Constant(10));
            //        //_PopUpLayout.Padding = new Thickness(5, 5, 5, 5);

            //        listView.IsEnabled = false;
            //        listView.IsVisible = false;
            //        _PopUpLayout.ShowPopup(popup);//, Constraint.Constant(30), Constraint.Constant(30)); 

            //        #endregion
            //    };

            //    b.Image = "icone_search.png";
            //    b.Source = ImageSource.FromFile("icone_search.png");

            //    Func<RelativeLayout, double> getImgWidth = (parent) => b.GetSizeRequest(relativeLayout.Width, relativeLayout.Height).Request.Width;
            //    relativeLayout.Children.Add(b,
            //        Constraint.RelativeToParent((r) => r.Width - getImgWidth(r) - getImgWidth(r) - 10),
            //        Constraint.Constant(Device.OnPlatform(20, 10, 10)));

            //}
            //else
            //{

            //    var filterTapRecognizer = new TapGestureRecognizer
            //    {
            //        Command = new Command(() =>
            //        {

            //            #region STATES
            //            var lblStates = new Label();
            //            lblStates.TextColor = StaticKeys.LABEL_TEXT_COLOR;
            //            lblStates.Text = "Estado";





            //            pickerStates = new Picker();
            //            pickerStates.Title = "-- Selecione --";
            //            pickerStates.Items.Add("Todos");
            //            pickerStates.Items.Add("Novo");
            //            pickerStates.Items.Add("Em aberto");
            //            pickerStates.Items.Add("Em resolução");
            //            pickerStates.Items.Add("Resolvido");

            //            if (filteredState + "" != "")
            //            {
            //                int index = 0;
            //                for (int i = 0; i < pickerStates.Items.Count; i++)
            //                {

            //                    if (pickerStates.Items[i] + "" == filteredState + "")
            //                    {
            //                        index = i;
            //                        break;
            //                    }

            //                }
            //                pickerStates.SelectedIndex = index;
            //            }
            //            #endregion

            //            #region AREA
            //            var labelAreas = new Label();
            //            labelAreas.Text = "Área";
            //            labelAreas.TextColor = StaticKeys.LABEL_TEXT_COLOR;
            //            pickerAreas = new Picker();


            //            Dictionary<int, TypesWS> typesDic = new Dictionary<int, TypesWS>();
            //            typesDic = StaticMethods.typesDic;

            //            if (typesDic == null || typesDic.Count == 0)
            //            {

            //                StaticMethods.GetTypes();


            //            }
            //            pickerAreas.Title = "-- Selecione --";
            //            pickerAreas.Items.Add("Todas");
            //            foreach (var item in typesDic)
            //            {

            //                if (item.Value.ParentID == null)
            //                {
            //                    pickerAreas.Items.Add(item.Value.Name);
            //                    listAreas.Add(item.Value);
            //                }
            //            }

            //            #endregion


            //            #region PROPERTY

            //            var labelProperty = new Label();
            //            labelProperty.Text = "Propriedade";
            //            labelProperty.TextColor = StaticKeys.LABEL_TEXT_COLOR;
            //            pickerProperties = new Picker();


            //            Dictionary<int, PropertyWS> propsDic = new Dictionary<int, PropertyWS>();
            //            propsDic = StaticMethods.propertiesDic;

            //            if (propsDic == null || propsDic.Count == 0)
            //            {

            //                StaticMethods.GetPropeties();


            //            }
            //            pickerProperties.Title = "-- Selecione --";
            //            pickerProperties.Items.Add("Todas");

            //            foreach (var item in propsDic)
            //            {


            //                pickerProperties.Items.Add(item.Value.Name);
            //                listProperties.Add(item.Value);

            //            }

            //            #endregion

            //            #region ORDER
            //            var lblOrder = new Label();
            //            lblOrder.TextColor = StaticKeys.LABEL_TEXT_COLOR;
            //            lblOrder.Text = "Ordenação";




            //            pickerOrder = new Picker();
            //            pickerStates.Title = "-- Selecione --";

            //            pickerOrder.Items.Add("Descendente");
            //            pickerOrder.Items.Add("Ascendente");

            //            #endregion

            //            #region select default

            //            SelectItemInPicker(pickerOrder, filteredOrder);
            //            SelectItemInPicker(pickerAreas, filteredArea);
            //            SelectItemInPicker(pickerStates, filteredState);

            //            SelectItemInPicker(pickerProperties, filteredProperty);

            //            //if (filteredOrder + "" != "")
            //            //{

            //            //    for (int i = 0; i < pickerOrder.Items.Count; i++)
            //            //    {

            //            //        if (pickerOrder.Items[i] + "" == filteredOrder + "")
            //            //        {
            //            //            index = i;
            //            //            break;
            //            //        }

            //            //    }
            //            //    pickerOrder.SelectedIndex = index;
            //            //}
            //            //else
            //            //    pickerOrder.SelectedIndex = 0; // vêm na ordem descencente

            //            //index = 0;
            //            //if (filteredArea + "" != "")
            //            //{
            //            //    for (int i = 0; i < pickerAreas.Items.Count; i++)
            //            //    {

            //            //        if (pickerAreas.Items[i] + "" == filteredArea + "")
            //            //        {
            //            //            index = i;
            //            //            break;
            //            //        }

            //            //    }
            //            //    pickerAreas.SelectedIndex = index;
            //            //}
            //            //else
            //            //    pickerAreas.SelectedIndex = 0; // vêm na ordem descencente


            //            //index = 0;
            //            //if (filteredState + "" != "")
            //            //{
            //            //    for (int i = 0; i < pickerStates.Items.Count; i++)
            //            //    {

            //            //        if (pickerStates.Items[i] + "" == filteredState + "")
            //            //        {
            //            //            index = i;
            //            //            break;
            //            //        }

            //            //    }
            //            //    pickerStates.SelectedIndex = index;
            //            //}
            //            //else
            //            //    pickerStates.SelectedIndex = 0; // vêm na ordem descencente

            //            #endregion

            //            #region BUTTONS
            //            var btnFilter = new RoundButton
            //            {
            //                Text = "Pesquisar",

            //                WidthRequest = 120,
            //                HeightRequest = 50,
            //                Stroke = Color.Black,
            //                StrokeThickness = 10,
            //                CornerRadius = 1,
            //                FontSize = 20,
            //                TextColor = StaticKeys.BUTTON_TEXT_COLOR,
            //                BackgroundColor = StaticKeys.BUTTON_BACKGROUND_COLOR

            //            };

            //            //btnFilter.Text = "Fechar";
            //            btnFilter.Clicked += btnFilter_Clicked;
            //            btnFilter.WidthRequest = 100;

            //            #endregion


            //            #region POPUP
            //            var popup = new StackLayout
            //            {
            //                WidthRequest = 250,
            //                HeightRequest = 350,
            //                BackgroundColor = StaticKeys.POPUP_BACKGROUND_COLOR,
            //                Orientation = StackOrientation.Vertical,
            //                //VerticalOptions = LayoutOptions.FillAndExpand,
            //                //HorizontalOptions = LayoutOptions.FillAndExpand,
            //                Children = { lblStates, pickerStates, labelAreas, pickerAreas, labelProperty, pickerProperties, lblOrder, pickerOrder, btnFilter },
            //                Padding = new Thickness(5, 5, 5, 5)

            //            };

            //            //XLabs.Forms.Controls.PopupLayout pop = new XLabs.Forms.Controls.PopupLayout();
            //            //pop.Content = popup;

            //            //pop.ShowPopup(popup, Constraint.Constant(10), Constraint.Constant(10));
            //            //_PopUpLayout.Padding = new Thickness(5, 5, 5, 5);

            //            listView.IsEnabled = false;
            //            listView.IsVisible = false;
            //            _PopUpLayout.ShowPopup(popup);//, Constraint.Constant(30), Constraint.Constant(30)); 

            //            #endregion
            //        }),
            //        NumberOfTapsRequired = 1
            //    };


            //    imgFilter = new Image();
            //    imgFilter.Source = "icone_search.png";





            //    imgFilter.GestureRecognizers.Add(filterTapRecognizer);


            //    Func<RelativeLayout, double> getImgWidth = (parent) => imgFilter.GetSizeRequest(relativeLayout.Width, relativeLayout.Height).Request.Width;
            //    relativeLayout.Children.Add(imgFilter,
            //        Constraint.RelativeToParent((r) => r.Width - getImgWidth(r) - getImgWidth(r) - 10),
            //        Constraint.Constant(10));

            //}
            //#endregion

            relativeLayout.Padding = new Thickness(0, Device.OnPlatform(0, 10, 0), 0, 0);

            stLayout = new StackLayout
            {
                //Padding = new Thickness(Device.OnPlatform(10, 0, 0), Device.OnPlatform(30, 0, 0), Device.OnPlatform(10, 0, 0), 0),
                //BackgroundColor = Device.OnPlatform(Color.Black, Color.Default, Color.Default),

                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,

                Children = { relativeLayout, activityIndicator, listView/*, activityIndicator */},
                //Padding = new Thickness(5, 5, 5, 5)
            };


            _PopUpLayout.Padding = new Thickness(0, 0, 0, 0);

            _PopUpLayout.Content = stLayout;

            Content = _PopUpLayout;
            //buttonFilter.IsVisible = false;
            relativeLayout.IsVisible = false;
            listView.IsVisible = false;





            //this.BackgroundColor = Device.OnPlatform(Color.FromHex("363636"), Color.Default, Color.Default);
            GetData();
        }




        #region Button FILTER

        void SelectItemInPicker(Picker picker, string selectedValue)
        {

            if (selectedValue + "" != "")
            {
                int index = 0;
                for (int i = 0; i < picker.Items.Count; i++)
                {

                    if (picker.Items[i] + "" == selectedValue + "")
                    {
                        index = i;
                        break;
                    }

                }
                picker.SelectedIndex = index;
            }


        }

        void buttonFilter_Clicked(object sender, EventArgs e)
        {

            #region STATES
            var lblStates = new Label();
            lblStates.TextColor = Device.OnPlatform(Color.White, Color.Default, Color.Default);
            lblStates.Text = "Estado";





            pickerStates = new Picker();
            pickerStates.Title = "-- Selecione --";
            pickerStates.Items.Add("Todos");
            pickerStates.Items.Add("Novo");
            pickerStates.Items.Add("Em aberto");
            pickerStates.Items.Add("Em resolução");
            pickerStates.Items.Add("Resolvido");

            if (filteredState + "" != "")
            {
                int index = 0;
                for (int i = 0; i < pickerStates.Items.Count; i++)
                {

                    if (pickerStates.Items[i] + "" == filteredState + "")
                    {
                        index = i;
                        break;
                    }

                }
                pickerStates.SelectedIndex = index;
            }
            #endregion

            #region AREA
            var labelAreas = new Label();
            labelAreas.Text = "Área";
            labelAreas.TextColor = Device.OnPlatform(Color.White, Color.Default, Color.Default);
            pickerAreas = new Picker();


            Dictionary<int, TypesWS> typesDic = new Dictionary<int, TypesWS>();
            typesDic = StaticMethods.typesDic;

            if (typesDic == null || typesDic.Count == 0)
            {

                StaticMethods.GetTypes();


            }
            pickerAreas.Title = "-- Selecione --";
            pickerAreas.Items.Add("Todas");
            foreach (var item in typesDic)
            {

                if (item.Value.ParentID == null)
                {
                    pickerAreas.Items.Add(item.Value.Name);
                    listAreas.Add(item.Value);
                }
            }

            #endregion


            #region PROPERTY

            var labelProperty = new Label();
            labelProperty.Text = "Propriedade";
            labelProperty.TextColor = Device.OnPlatform(Color.White, Color.Default, Color.Default);
            pickerProperties = new Picker();


            Dictionary<int, PropertyWS> propsDic = new Dictionary<int, PropertyWS>();
            propsDic = StaticMethods.propertiesDic;

            if (propsDic == null || propsDic.Count == 0)
            {

                StaticMethods.GetPropeties();


            }
            pickerProperties.Title = "-- Selecione --";
            pickerProperties.Items.Add("Todas");

            foreach (var item in propsDic)
            {


                pickerProperties.Items.Add(item.Value.Name);
                listProperties.Add(item.Value);

            }

            #endregion

            #region ORDER
            var lblOrder = new Label();
            lblOrder.TextColor = Device.OnPlatform(Color.White, Color.Default, Color.Default);
            lblOrder.Text = "Ordenação";




            pickerOrder = new Picker();
            pickerStates.Title = "-- Selecione --";

            pickerOrder.Items.Add("Descendente");
            pickerOrder.Items.Add("Ascendente");

            #endregion

            #region select default

            SelectItemInPicker(pickerOrder, filteredOrder);
            SelectItemInPicker(pickerAreas, filteredArea);
            SelectItemInPicker(pickerStates, filteredState);

            SelectItemInPicker(pickerProperties, filteredProperty);

            //if (filteredOrder + "" != "")
            //{

            //    for (int i = 0; i < pickerOrder.Items.Count; i++)
            //    {

            //        if (pickerOrder.Items[i] + "" == filteredOrder + "")
            //        {
            //            index = i;
            //            break;
            //        }

            //    }
            //    pickerOrder.SelectedIndex = index;
            //}
            //else
            //    pickerOrder.SelectedIndex = 0; // vêm na ordem descencente

            //index = 0;
            //if (filteredArea + "" != "")
            //{
            //    for (int i = 0; i < pickerAreas.Items.Count; i++)
            //    {

            //        if (pickerAreas.Items[i] + "" == filteredArea + "")
            //        {
            //            index = i;
            //            break;
            //        }

            //    }
            //    pickerAreas.SelectedIndex = index;
            //}
            //else
            //    pickerAreas.SelectedIndex = 0; // vêm na ordem descencente


            //index = 0;
            //if (filteredState + "" != "")
            //{
            //    for (int i = 0; i < pickerStates.Items.Count; i++)
            //    {

            //        if (pickerStates.Items[i] + "" == filteredState + "")
            //        {
            //            index = i;
            //            break;
            //        }

            //    }
            //    pickerStates.SelectedIndex = index;
            //}
            //else
            //    pickerStates.SelectedIndex = 0; // vêm na ordem descencente

            #endregion

            #region BUTTONS
            var btnFilter = new RoundButton
                {
                    Text = "Pesquisar",

                    WidthRequest = 120,
                    HeightRequest = 50,
                    Stroke = Color.Black,
                    StrokeThickness = 10,
                    CornerRadius = 1,
                    FontSize = 20,
                    TextColor = Color.Black,
                    BackgroundColor = Color.White,
                    BorderColor = Color.Black,

                };

            //btnFilter.Text = "Fechar";
            btnFilter.Clicked += btnFilter_Clicked;
            btnFilter.WidthRequest = 100;

            #endregion


            #region POPUP
            var popup = new StackLayout
            {
                WidthRequest = 250,
                HeightRequest = 325,
                BackgroundColor = Color.Black, // for Android and WP 
                Orientation = StackOrientation.Vertical,
                //VerticalOptions = LayoutOptions.FillAndExpand,
                //HorizontalOptions = LayoutOptions.FillAndExpand,
                Children = { lblStates, pickerStates, labelAreas, pickerAreas, labelProperty, pickerProperties, lblOrder, pickerOrder, btnFilter },
                Padding = new Thickness(5, 5, 5, 5)

            };

            //XLabs.Forms.Controls.PopupLayout pop = new XLabs.Forms.Controls.PopupLayout();
            //pop.Content = popup;

            //pop.ShowPopup(popup, Constraint.Constant(10), Constraint.Constant(10));
            //_PopUpLayout.Padding = new Thickness(5, 5, 5, 5);

            listView.IsEnabled = false;
            listView.IsVisible = false;
            _PopUpLayout.ShowPopup(popup);//, Constraint.Constant(30), Constraint.Constant(30)); 

            #endregion
        }

        void btnFilter_Clicked(object sender, EventArgs e)
        {
            if (_PopUpLayout.IsPopupActive)
            {

                if (list!= null)
                {
                    // filter the list
                    var results = new List<Issue>();


                    if (pickerStates.SelectedIndex > 0)
                    {
                        filteredState = pickerStates.Items[pickerStates.SelectedIndex];

                        for (var i = 0; i < list.Count; i++)
                        {
                            var issue = list[i] as Issue;
                            if (issue.StateName == filteredState)
                            {
                                results.Add(issue);
                            }
                        }


                    }
                    else
                    {
                        results = new List<Issue>(list);
                        filteredState = "";
                    }


                    if (pickerAreas.SelectedIndex > 0)
                    {
                        filteredArea = pickerAreas.Items[pickerAreas.SelectedIndex];

                        for (var i = results.Count - 1; i >= 0; i--)
                        {
                            var issue = results[i] as Issue;
                            //string s = issue.SubjectFormatted;
                            //System.Diagnostics.Debug.WriteLine( s + " " + issue.ParentName);
                            if (issue.ParentName != filteredArea)
                            {

                                //System.Diagnostics.Debug.WriteLine("i - " + i + " - " + s);
                                results.RemoveAt(i);

                                //bool removed =  results.Remove(issue);


                            }
                        }


                    }
                    else
                        filteredArea = "";

                    if (pickerProperties.SelectedIndex > 0)
                    {
                        filteredProperty = pickerProperties.Items[pickerProperties.SelectedIndex];

                        for (var i = results.Count - 1; i >= 0; i--)
                        {
                            var issue = results[i] as Issue;
                            //string s = issue.SubjectFormatted;
                            //System.Diagnostics.Debug.WriteLine( s + " " + issue.ParentName);
                            if (issue.PropertyName != filteredProperty)
                            {

                                //System.Diagnostics.Debug.WriteLine("i - " + i + " - " + s);
                                results.RemoveAt(i);

                                //bool removed =  results.Remove(issue);


                            }
                        }


                    }
                    else
                        filteredProperty = "";

                    listView.ItemsSource = null;




                    if (pickerOrder.SelectedIndex >= 0)
                    {
                        filteredOrder = pickerOrder.Items[pickerOrder.SelectedIndex];
                        if (pickerOrder.SelectedIndex == 1)
                            listView.ItemsSource = results.OrderBy(c => c.DateReported.Date);//.ThenBy(c=> c.DateReported.TimeOfDay);
                        else
                            listView.ItemsSource = results.OrderByDescending(c => c.DateReported.Date);//.ThenBy(c => c.DateReported.TimeOfDay); 
                    }
                    else
                        listView.ItemsSource = results;

                }
                _PopUpLayout.Padding = new Thickness(0, 0, 0, 0);
                _PopUpLayout.DismissPopup();
                listView.IsEnabled = true;
                listView.IsVisible = true;

            }
        }


        #endregion

        public void GetData()
        {
            if (StaticMethods.CheckConnection())
            {

                IssuesWebServiceSoapClient client = new IssuesWebServiceSoapClient(
                               new BasicHttpBinding(),
                               new EndpointAddress(StaticKeys.WebServiceUrl));


                client.GetMyIssuesCompleted += OnGotResultReport;


                client.GetMyIssuesAsync(_reportedBy, null, null);

            }
            else
            {
                DisplayAlert(StaticKeys.ERROR_TITLE, "To use this app you have to be connected to the internet", StaticKeys.OK_BTN_TITLE);

               

            }
        }




        //void bv_Clicked(object sender, EventArgs e)
        //{

        //    Navigation.PushAsync(new TakePictureScreen(_gps));

        //}

        void OnGotResultReport(object sender, GetMyIssuesCompletedEventArgs e)
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
                    await DisplayAlert(StaticKeys.ERROR_TITLE, error, StaticKeys.OK_BTN_TITLE, StaticKeys.CANCEL_BTN_TITLE);
                }
                else
                {

                    if (e.Result != "{}")
                    {
                        var obj = JObject.Parse(e.Result);
                        var issues = (JArray)obj["Data"];
                        if (issues != null)
                        {


                            list = new List<Issue>();


                            foreach (JObject t in issues)
                            {


                                Issue tp = new Issue(Convert.ToInt32(t["ID"]), t["PhotoThumbnailUrl"] + "", t["Subject"] + "", t["StateName"] + "",
                                   t["Message"] + "", t["TypeName"] + "", Convert.ToDateTime(t["DateReported"] + ""), t["ParentName"] + "",
                                   t["Latitude"] + "", t["Longitude"] + "", t["PhotoUrl"] + "", t["PropertyID"] + "", t["PropertyName"] + "");
                                list.Add(tp);

                            }




                            listView.ItemsSource = list;

                            listView.ItemTemplate = new DataTemplate(typeof(IssueCell));
                            //listView.ItemSelected += (senderList, eList) =>
                            //{
                            //    return;


                            //};




                            //this.Padding = new Thickness(Device.OnPlatform(10, 0, 0), Device.OnPlatform(30, 0, 0), Device.OnPlatform(10, 0, 0), 0);
                            listView.BackgroundColor = Device.OnPlatform(Color.White, Color.White, Color.Default);
                        }

                        else
                        {

                            await DisplayAlert(StaticKeys.INFO_TITLE, "Could not get fish type reported.", StaticKeys.OK_BTN_TITLE);

                        }
                    }
                    else
                    {
                        listView.ItemsSource = new List<Issue>();
                        await DisplayAlert(StaticKeys.INFO_TITLE, "No Fish Type reported yet.", StaticKeys.OK_BTN_TITLE);
                    }


                }
                activityIndicator.IsRunning = false;
                activityIndicator.IsVisible = false;

                relativeLayout.IsVisible = true;
                listView.IsVisible = true;
                //buttonFilter.IsVisible = true;
            });
        }





    }
}
