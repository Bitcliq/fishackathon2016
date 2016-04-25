using Microsoft.Phone.Maps.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WinPhone;

[assembly: ExportRenderer(typeof(PinFixed.MapPage), typeof(PinFixed.WinPhone.MapWindows))]

namespace PinFixed.WinPhone
{
    public class MapWindows : PageRenderer
    {
        MapPage _page;

        Map map; 

        protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
        {
            base.OnElementChanged(e);
            _page = e.NewElement as MapPage;


            map = new Map();

            var p = new MapLayout(_page);
            this.Children.Add(p);
            
            
        }

    }
}
