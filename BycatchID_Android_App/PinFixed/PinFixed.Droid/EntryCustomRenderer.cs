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
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using PinFixed.Droid;
using PinFixed.UI;
using PinFixed.BL;

[assembly: ExportRenderer(typeof(CustomEntry), typeof(EntryCustomRenderer))]

namespace PinFixed.Droid
{
    public class EntryCustomRenderer: EntryRenderer
    {

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                
                EditText textField = (EditText)Control;



                textField.SetBackgroundResource(Resource.Drawable.CustomEntryStyle);
                //this.Control.Layer.CornerRadius = 3f;
                //this.Control.Layer.BorderColor = StaticKeys.LABEL_BORDER_COLOR// Color.Red.ToCGColor();
            }
        }

    }
}