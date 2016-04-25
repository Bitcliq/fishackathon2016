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
using PinFixed;
using PinFixed.UI;
using PinFixed.Droid;
using Android.Views.InputMethods;

//[assembly: ExportRenderer(typeof(CustomEditor), typeof(EditorCustomRenderer))]

namespace PinFixed.Droid
{
    public class EditorCustomRenderer : EditorRenderer
    {
        //protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        //{
        //    base.OnElementPropertyChanged(sender, e);
        //    Control.ImeOptions = global::Android.Views.InputMethods.ImeAction.Done;
        //}
        //protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        //{
        //    base.OnElementChanged(e);

        //    if (Control != null)
        //    {

        //        Control.ImeOptions = global::Android.Views.InputMethods.ImeAction.Done;
        //    }
        //}

        // Override the OnElementChanged method so we can tweak this renderer post-initial setup
       

    }
}