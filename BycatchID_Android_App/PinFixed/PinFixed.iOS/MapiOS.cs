using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using MapKit;
using System.Drawing;
using CoreLocation;
using CoreGraphics;
using System.IO;

[assembly: ExportRenderer(typeof(PinFixed.MapPage), typeof(PinFixed.iOS.MapiOS))]
namespace PinFixed.iOS
{
    public class MapiOS : PageRenderer
    {
        MapPage _page;

        MapDelegate mapDel;


        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);
            _page = e.NewElement as MapPage;

            _page.BackgroundColor = Color.FromHex("#74a633");

            #region NAVIGATION TOOL BAR


            var im = UIImage.FromFile("back.png");

           
            CoreGraphics.CGRect rectTollBar = new CoreGraphics.CGRect(10, 10, im.Size.Width, 60);

            var btnBack = new UIButton(rectTollBar);
            //UIButton.FromType(UIButtonType.RoundedRect);
            btnBack.SetImage(UIImage.FromFile("back.png"), UIControlState.Normal);
            
            btnBack.TouchUpInside += (sender, ea) =>
            {
                _page.GoBack();
            };

           

            Add(btnBack);

            CoreGraphics.CGRect rectTollBar2 = new CoreGraphics.CGRect(im.Size.Width + 30, 10, im.Size.Width, 60);


            var btnHome = new UIButton(rectTollBar2); // UIButton.FromType(UIButtonType.RoundedRect);
            btnHome.SetImage(UIImage.FromFile("btn_home.png"), UIControlState.Normal);

            btnHome.TouchUpInside += (sender, ea) =>
            {
                _page.GoHome();
            };

            Add(btnHome);


            CoreGraphics.CGRect rect1 = new CoreGraphics.CGRect(0, 60,  UIScreen.MainScreen.ApplicationFrame.Width, 0);


            var txt = new UITextView(rect1);
            txt.Text = "Select a location in the map.";
            txt.TextColor = UIColor.White;
            txt.BackgroundColor = UIColor.Black;






            //CGRect rect1 = txt.Frame;
            rect1.Height = txt.ContentSize.Height;
            //rect1.Top = 50;
            txt.Frame = rect1;

            Add(txt);

            #endregion

            #region MAP
            CoreGraphics.CGRect rect = new CoreGraphics.CGRect(0, 60 + rect1.Height, UIScreen.MainScreen.ApplicationFrame.Width, UIScreen.MainScreen.ApplicationFrame.Height  - 50);
            var mapView = new MKMapView(rect);// { ShowsUserLocation = true, Delegate = new MapDelegate() };

            //mapDel = new MapDelegate();
            //mapView.Delegate = mapDel;

            mapView.ShowsUserLocation = true;
            mapView.ZoomEnabled = true;


           
            CLLocationCoordinate2D coords = new CLLocationCoordinate2D(39.4062235540, -9.1316986084);
          

            MKCoordinateSpan span = new MKCoordinateSpan(MilesToLatitudeDegrees(20), MilesToLongitudeDegrees(20, coords.Latitude));


            mapView.Region = new MKCoordinateRegion(coords, span);
            
            mapView.MapType = MKMapType.Hybrid;

            mapView.ScrollEnabled = true;

            Add(mapView);
            #endregion


            #region BUTTON SELECT POINT

            CLLocationCoordinate2D? touchCoordinates = null;

            CoreGraphics.CGRect rect2 = new CoreGraphics.CGRect(0, rect.Height + 10, UIScreen.MainScreen.ApplicationFrame.Width, UIScreen.MainScreen.ApplicationFrame.Height - rect.Height + 10);
            
            
            var btn = new UIButton(rect2);
            btn.SetTitleColor(UIColor.White, UIControlState.Normal);
            btn.SetTitleColor(UIColor.White, UIControlState.Application);
            btn.SetTitleColor(UIColor.White, UIControlState.Disabled);
            btn.SetTitleColor(UIColor.White, UIControlState.Normal);
            btn.BackgroundColor = UIColor.Black;
            
            if (_page.Pos != null)
            {

                CLLocationCoordinate2D destination = new CLLocationCoordinate2D(Convert.ToDouble(((Xamarin.Forms.Maps.Position)_page.Pos).Latitude), Convert.ToDouble(((Xamarin.Forms.Maps.Position)_page.Pos).Longitude));

                MKMapPoint mapPoint = MKMapPoint.FromCoordinate(destination);

                var annotation = new BasicMapAnnotation(destination, "", "");
               

                mapView.AddAnnotation(annotation);
                MKCoordinateRegion mapRegion = MKCoordinateRegion.FromDistance(destination, 200, 200);
                mapView.CenterCoordinate = destination;
                mapView.Region = mapRegion;
                btn.SetTitle("Take me there", UIControlState.Normal);
            }
            else if (_page.PosImageFromLibrary != null)
            {


                CLLocationCoordinate2D destination = new CLLocationCoordinate2D(Convert.ToDouble(((Xamarin.Forms.Maps.Position)_page.PosImageFromLibrary).Latitude), Convert.ToDouble(((Xamarin.Forms.Maps.Position)_page.PosImageFromLibrary).Longitude));

                MKMapPoint mapPoint = MKMapPoint.FromCoordinate(destination);

                var annotation = new BasicMapAnnotation(destination, "", "");

                mapView.AddAnnotation(annotation);
                MKCoordinateRegion mapRegion = MKCoordinateRegion.FromDistance(destination, 200, 200);
                mapView.CenterCoordinate = destination;
                mapView.Region = mapRegion;
                touchCoordinates = destination;

                btn.SetTitle("Select location", UIControlState.Normal);
            }
            else
            {
                if (_page.GPS != null)
                {
                    if (_page.GPS.PositionLatitude + "" != "" && _page.GPS.PositionLongitude + "" != "")
                    {

                        CLLocationCoordinate2D destination = new CLLocationCoordinate2D(Convert.ToDouble(_page.GPS.PositionLatitude), Convert.ToDouble(_page.GPS.PositionLongitude));

                        MKMapPoint mapPoint = MKMapPoint.FromCoordinate(destination);

                        var annotation = new BasicMapAnnotation(destination, "", "");


                        

                        mapView.AddAnnotation(annotation);
                        

                        MKCoordinateRegion mapRegion = MKCoordinateRegion.FromDistance(destination, 200, 200);
                        mapView.CenterCoordinate = destination;
                        mapView.Region = mapRegion;


                        touchCoordinates = destination;
                    }


                }


                btn.SetTitle("Select location", UIControlState.Normal);
            }



            btn.TouchUpInside += (sender, ea) =>
            {
                if (_page.Pos != null)
                {



                    //var marker = Marker.FromPosition(destination);
                    // marker.Title = string.Format("Localização");
                    //marker.Animated = true;
                    //marker.Map = mapView; // this is the line that I could not find anywhere else!

                    CLLocationCoordinate2D destination = new CLLocationCoordinate2D(Convert.ToDouble(((Xamarin.Forms.Maps.Position)_page.Pos).Latitude), Convert.ToDouble(((Xamarin.Forms.Maps.Position)_page.Pos).Longitude));


                    CLLocationCoordinate2D start = new CLLocationCoordinate2D(Convert.ToDouble(_page.GPS.PositionLatitude), Convert.ToDouble(_page.GPS.PositionLongitude));
                
                    NSUrl u = new NSUrl(@"http://maps.apple.com/?daddr=" + start.Latitude.ToString().Replace(",", ".") + "," + start.Longitude.ToString().Replace(",", ".") + "&saddr=" + destination.Latitude.ToString().Replace(",", ".") + "," + destination.Longitude.ToString().Replace(",", "."));
                    UIApplication.SharedApplication.OpenUrl(u);
                }
                else
                {

                    if (touchCoordinates != null)
                    {
                      
                        _page.SetCoords(((CLLocationCoordinate2D)touchCoordinates).Latitude, ((CLLocationCoordinate2D)touchCoordinates).Longitude);

                    }
                    else
                    {
                        UIAlertView _error = new UIAlertView("Information", "Select location", null, "Ok", null);

                        _error.Show();
                    }
                }
            };

            Add(btn);
            #endregion

            #region MAP CLICK

            if (_page.Pos == null)
            {
                mapView.AddGestureRecognizer(new UITapGestureRecognizer(r =>
                {
                    if (mapView.Annotations.Count() > 0)
                        mapView.RemoveAnnotations(mapView.Annotations);

                    CGPoint pointInView = r.LocationInView(mapView);
                    touchCoordinates = mapView.ConvertPoint(pointInView, mapView);
                    MKMapPoint mapPoint = MKMapPoint.FromCoordinate((CLLocationCoordinate2D)touchCoordinates);

                    var annotation = new BasicMapAnnotation((CLLocationCoordinate2D)touchCoordinates, "", "");

                    mapView.AddAnnotation(annotation);

                    MKCoordinateRegion mapRegion = MKCoordinateRegion.FromDistance((CLLocationCoordinate2D)touchCoordinates, 200, 200);
                    mapView.CenterCoordinate = (CLLocationCoordinate2D)touchCoordinates;
                    mapView.Region = mapRegion;



                }));
            }
            #endregion




        }

        public double MilesToLatitudeDegrees(double miles)
        {
            double earthRadius = 3960.0; // in miles
            double radiansToDegrees = 180.0 / Math.PI;
            return (miles / earthRadius) * radiansToDegrees;
        }

        /// <summary>Converts miles to longitudinal degrees at a specified latitude</summary>
        public double MilesToLongitudeDegrees(double miles, double atLatitude)
        {
            double earthRadius = 3960.0; // in miles
            double degreesToRadians = Math.PI / 180.0;
            double radiansToDegrees = 180.0 / Math.PI;
            // derive the earth's radius at that point in latitude
            double radiusAtLatitude = earthRadius * Math.Cos(atLatitude * degreesToRadians);
            return (miles / radiusAtLatitude) * radiansToDegrees;
        }


        protected class MapDelegate : MKMapViewDelegate
        {
            string pId = "PinAnnotation";
            string mId = "BasicAnnotation";

           // protected string annotationIdentifier = "BasicAnnotation";
            //UIButton detailButton;
            /// <summary>
            /// This is very much like the GetCell method on the table delegate
            /// </summary>
            public override MKAnnotationView GetViewForAnnotation(MKMapView mapView, IMKAnnotation annotation)
            {
            


                MKAnnotationView anView;

                if (annotation is MKUserLocation)
                    return null;

                if (annotation is BasicMapAnnotation)
                {

                    // show monkey annotation
                    anView = mapView.DequeueReusableAnnotation(mId);

                    if (anView == null)
                        anView = new MKAnnotationView(annotation, mId);

                    anView.Image = UIImage.FromFile("pin.png");
                    anView.CanShowCallout = true;
                    anView.Draggable = true;
                    anView.RightCalloutAccessoryView = UIButton.FromType(UIButtonType.DetailDisclosure);

                }
                else
                {

                    // show pin annotation
                    anView = (MKPinAnnotationView)mapView.DequeueReusableAnnotation(pId);

                    if (anView == null)
                        anView = new MKPinAnnotationView(annotation, pId);

                    ((MKPinAnnotationView)anView).PinColor = MKPinAnnotationColor.Red;
                    anView.CanShowCallout = true;
                }

                return anView;

            }

            // as an optimization, you should override this method to add or remove annotations as the 
            // map zooms in or out.
            public override void RegionChanged(MKMapView mapView, bool animated) { }
        }


        class BasicMapAnnotation : MKAnnotation
        {
            string title, subtitle;
            public override CLLocationCoordinate2D Coordinate { get { return this.Coords; } }
            public CLLocationCoordinate2D Coords;
            public override string Title { get { return title; } }
            public override string Subtitle { get { return subtitle; } }
            public BasicMapAnnotation(CLLocationCoordinate2D coordinate, string title, string subtitle)
            {
                this.Coords = coordinate;
                this.title = title;
                this.subtitle = subtitle;
            }
        }

    }


}