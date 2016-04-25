using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
//using Android.Support.V4.App;
using Android.Gms.Maps;
using Android.Graphics;
using PinFixed.BL;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using System.Threading;
using Android.Gms.Maps.Model;
using Java.Lang;


[assembly: ExportRenderer(typeof(PinFixed.MapPage), typeof(PinFixed.Droid.MapAndroidRenderer))]

namespace PinFixed.Droid
{

    // Renderer for android map


    public class MapAndroidRenderer : PageRenderer
    {
        




        private static Android.Views.View _view;
        private MapFragment _mapFragment;
        private MyOnMapReady _mapReadyCallback;
        MapPage _page;
        bool _gettingMap = false;
        Activity _activity;
      

        	


        protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
        {
            try
            {

                base.OnElementChanged(e);

                _page = e.NewElement as MapPage;


                _activity = this.Context as Activity;


                _view = _activity.LayoutInflater.Inflate(Resource.Layout.MapLayout2, this, false);
                AddView(_view);


               Android.Widget.LinearLayout tool = _view.FindViewById<Android.Widget.LinearLayout>(Resource.Id.tool);


                tool.SetBackgroundColor(Android.Graphics.Color.Black);

               


                Android.Widget.Button ll = _view.FindViewById<Android.Widget.Button>(Resource.Id.btnT);

                ll.Click += ll_Click;




                Android.Widget.ImageView imgBack = _view.FindViewById<Android.Widget.ImageView>(Resource.Id.imageViewBack);

                imgBack.Click += imgBack_Click;



                Android.Widget.ImageView imgHome = _view.FindViewById<Android.Widget.ImageView>(Resource.Id.imageViewHome);

                imgHome.Click += imgHome_Click;


                _mapFragment = (MapFragment)_activity.FragmentManager.FindFragmentById(Resource.Id.mapTest);

        

                _mapReadyCallback = new MyOnMapReady(/*btn,*/ _page);
                _mapReadyCallback.MapReady += (sender, args) =>
                {
                    // set up annotations etc here
                    _gettingMap = false;
                };
                _gettingMap = true;
                _mapFragment.GetMapAsync(_mapReadyCallback);

                if (_page.Pos != null)
                {
                    ll.Text = "Take me there";


                }
            }
            catch (System.Exception ex)
            {
                // throw (ex);

                Fragment fragment = _activity.FragmentManager.FindFragmentById(Resource.Id.mapTest);

                if (fragment != null)
                {
                    if (fragment.IsResumed)
                    {
                        _activity.FragmentManager.BeginTransaction().Remove(fragment).Commit();





                        _mapFragment = (MapFragment)_activity.FragmentManager.FindFragmentById(Resource.Id.mapTest);



                        _mapReadyCallback = new MyOnMapReady(/*btn,*/ _page);
                        _mapReadyCallback.MapReady += (sender, args) =>
                        {
                            // set up annotations etc here
                            _gettingMap = false;
                        };
                        _gettingMap = true;
                        _mapFragment.GetMapAsync(_mapReadyCallback);

                    }
                }



            }

        }




        void imgHome_Click(object sender, EventArgs e)
        {

            _page.GoHome();
            Fragment fragment = _activity.FragmentManager.FindFragmentById(Resource.Id.mapTest);

            if (fragment != null)
            {
                if (fragment.IsResumed)
                {
                    _activity.FragmentManager.BeginTransaction().Remove(fragment).Commit();

                }
            }


        }

        void imgBack_Click(object sender, EventArgs e)
        {
            _page.GoBack();
            Fragment fragment = _activity.FragmentManager.FindFragmentById(Resource.Id.mapTest);

            if (fragment != null)
            {
                if (fragment.IsResumed)
                {
                    _activity.FragmentManager.BeginTransaction().Remove(fragment).Commit();

                }
            }

        }

        void ll_Click(object sender, EventArgs e)
        {

            //_page.SetCoords(39, -9);

            if (_page.Pos != null)
            {

                var lat = ((Xamarin.Forms.Maps.Position)_page.Pos).Latitude.ToString().Replace(",", ".");
                var ln = ((Xamarin.Forms.Maps.Position)_page.Pos).Longitude.ToString().Replace(",", ".");
                var ur = Android.Net.Uri.Parse("google.navigation:ll=" + lat + "," + ln + "&mode=w");
                var mapIntent = new Intent(Intent.ActionView, ur);


                _activity.StartActivity(mapIntent);

                Fragment fragment = _activity.FragmentManager.FindFragmentById(Resource.Id.mapTest);

                if (fragment != null)
                {
                    if (fragment.IsResumed)
                    {
                        _activity.FragmentManager.BeginTransaction().Remove(fragment).Commit();

                    }
                }

            }
            else
            {
                if (_mapReadyCallback.Clicked)
                {
                    _page.SetCoords(_mapReadyCallback.lat, _mapReadyCallback.lo);
                    Fragment fragment = _activity.FragmentManager.FindFragmentById(Resource.Id.mapTest);

                    if (fragment != null)
                    {
                        if (fragment.IsResumed)
                        {
                            _activity.FragmentManager.BeginTransaction().Remove(fragment).Commit();

                        }
                    }


                }
                else
                {
                    AlertDialog.Builder alert = new AlertDialog.Builder(_activity);

                    alert.SetTitle("Select location!");
                    alert.SetPositiveButton("OK", (senderAlert, args) =>
                    {

                    });

                    alert.Show();
                }

            }
    

        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            base.OnLayout(changed, l, t, r, b);
            var msw = MeasureSpec.MakeMeasureSpec(r - l, MeasureSpecMode.Exactly);
            var msh = MeasureSpec.MakeMeasureSpec(b - t, MeasureSpecMode.Exactly);
            _view.Measure(msw, msh);
            _view.Layout(0, 0, r - l, b - t);
        }



      


    }


    public class MyOnMapReady : Java.Lang.Object, IOnMapReadyCallback
    {
        public GoogleMap Map { get; private set; }
        public double lat;
        public double lo;
        MapPage _page;
        public MarkerOptions mo;
        public bool Clicked = false;
        // private Android.Widget.Button _selectBtn;

        public event EventHandler MapReady;
        public MyOnMapReady(/*Android.Widget.Button btn,*/ MapPage p)
        {
            // _selectBtn = btn;
            _page = p;
        }

        public void OnMapReady(GoogleMap googleMap)
        {
            BitmapDescriptor b = BitmapDescriptorFactory.FromResource(Resource.Drawable.pin);

            Map = googleMap;
            Map.MapType = GoogleMap.MapTypeHybrid;
            

            if (_page.Pos == null && _page.PosImageFromLibrary == null)
            {
               
                

                Map.AnimateCamera(CameraUpdateFactory.NewLatLngZoom(new LatLng(39.4062235540, -9.1316986084), 12));

                Map.MapClick += Map_MapClick;
                var handler = MapReady;
                if (handler != null)
                    handler(this, EventArgs.Empty);

            }
            //MarkerOptions mo = new MarkerOptions();
            //mo.SetPosition(new LatLng(39, -9));

            //Map.AddMarker(mo);



            if (_page.Pos != null)
            {
                MarkerOptions markerOpt1 = new MarkerOptions();

                 lat = ((Xamarin.Forms.Maps.Position)_page.Pos).Latitude;
                 lo = ((Xamarin.Forms.Maps.Position)_page.Pos).Longitude;
                 markerOpt1.SetPosition(new LatLng(lat, lo));//.InvokeIcon(b);

                Map.AddMarker(markerOpt1);

                Map.AnimateCamera(CameraUpdateFactory.NewLatLngZoom(new LatLng(lat, lo), 18));
            }
            else if (_page.PosImageFromLibrary != null)
            {


                Clicked = true;

                MarkerOptions markerOpt1 = new MarkerOptions();

                 lat = ((Xamarin.Forms.Maps.Position)_page.PosImageFromLibrary).Latitude;
                 lo = ((Xamarin.Forms.Maps.Position)_page.PosImageFromLibrary).Longitude;
                 markerOpt1.SetPosition(new LatLng(lat, lo));//.InvokeIcon(b);

                Map.AddMarker(markerOpt1);

                Map.AnimateCamera(CameraUpdateFactory.NewLatLngZoom(new LatLng(lat, lo), 18));
                
                Map.MapClick += Map_MapClick;
                var handler = MapReady;
                if (handler != null)
                    handler(this, EventArgs.Empty);
            }
            else if (_page.GPS != null)
            {
                if (_page.GPS.PositionLatitude + "" != "" && _page.GPS.PositionLongitude + "" != "")
                {
                    Clicked = true;
                    MarkerOptions markerOpt1 = new MarkerOptions();

                     lat = Convert.ToDouble(_page.GPS.PositionLatitude);
                    lo = Convert.ToDouble(_page.GPS.PositionLongitude);
                    markerOpt1.SetPosition(new LatLng(lat, lo));//.InvokeIcon(b);

                    Map.AddMarker(markerOpt1);

                    Map.AnimateCamera(CameraUpdateFactory.NewLatLngZoom(new LatLng(lat, lo), 18));

                    Map.MapClick += Map_MapClick;
                    var handler = MapReady;
                    if (handler != null)
                        handler(this, EventArgs.Empty);

                }
            }
        }

        void Map_MapClick(object sender, GoogleMap.MapClickEventArgs e)
        {
            Clicked = true;
            lat = e.Point.Latitude;
            lo = e.Point.Longitude;


            Map.Clear();

           BitmapDescriptor b = BitmapDescriptorFactory.FromResource(Resource.Drawable.pin);// ("pin.png");

            MarkerOptions mo = new MarkerOptions();
          

 
            mo.SetPosition(new LatLng(lat, lo));//.InvokeIcon(b);

            Map.AddMarker(mo);

      

        }
    }

}