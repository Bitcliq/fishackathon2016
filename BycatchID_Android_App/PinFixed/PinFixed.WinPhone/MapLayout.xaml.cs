using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Device.Location;
using Microsoft.Phone.Maps.Toolkit;
using Microsoft.Phone.Maps.Controls;
using System.Windows.Media.Imaging;

namespace PinFixed.WinPhone
{
    public partial class MapLayout : PhoneApplicationPage
    {
        MapPage _page;
        GeoCoordinate coord;
        public MapLayout(MapPage page)
        {
            this._page = page;
            InitializeComponent();

            Map.Center = new GeoCoordinate(38.793737, -9.434226);
            Map.ZoomLevel = 10;
        }

        private void Map_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {


            coord = this.Map.ConvertViewportPointToGeoCoordinate(e.GetPosition(this.Map));


            //MapLayer layer1 = new MapLayer();

            //Pushpin pushpin1 = new Pushpin();
          


            //pushpin1.GeoCoordinate = coord;
            ////pushpin1.Content = "My car";
            //MapOverlay overlay1 = new MapOverlay();
            //overlay1.Content = pushpin1;
            //overlay1.GeoCoordinate = coord;
            //layer1.Add(overlay1);

            //Map.Layers.Add(layer1);



            Map.Layers.Clear();


            BitmapImage img;
            img = new BitmapImage(new Uri("pin.png", UriKind.RelativeOrAbsolute));
            var image = new Image();
            image.Width = 50;
            image.Height = 50;
            image.Opacity = 100;
            image.Source = img;

            var mapOverlay = new MapOverlay();
            mapOverlay.Content = image;
            mapOverlay.GeoCoordinate = coord;
            var mapLayer = new MapLayer();
            mapLayer.Add(mapOverlay);
            Map.Layers.Add(mapLayer);
            Map.SetView(coord, 5);


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(coord != null)
                _page.SetCoords(coord.Latitude, coord.Longitude);
            else
                MessageBox.Show("Seleccione um Ponto", "Atenção", MessageBoxButton.OK);
        }
    }
}