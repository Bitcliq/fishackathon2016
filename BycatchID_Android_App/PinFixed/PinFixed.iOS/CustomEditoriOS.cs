using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
// Additional needed using's that the classes are found without namespaces
//

//
using Xamarin.Forms;  // Needed for the base-Editor
using Xamarin.Forms.Platform.iOS; // Needed for iOS-stuff
//
using Foundation;
using UIKit;
using PinFixed.UI;
using PinFixed.iOS;

[assembly: ExportRenderer(typeof(CustomEditor), typeof(CustomEditoriOS))] // if you add the namespaces in the using
// If you don't want the namespaces in the using, you have to reference it like below:
//[assembly: ExportRenderer(typeof(MatrixGuide.MG_Editor), typeof(MatrixGuide.iOS.EditorCustomRenderer))]

namespace PinFixed.iOS// The iOS-Namespace to my App 
{
    public class CustomEditoriOS : EditorRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement == null)
            {   // perform initial setup
                // lets get a reference to the native control
                var nativeTextView = (UITextView)Control;
                // do whatever you want to the UITextField here!
                //nativeTextView.Font = UIFont.SystemFontOfSize(18);

         

                nativeTextView.Layer.BorderColor = UIColor.Clear.FromHexString("#dddddd", 1.0f).CGColor; //UIColor.LightGray.CGColor;
                nativeTextView.Layer.BorderWidth = 1f;
                nativeTextView.Editable = Control.Editable;
                
                

            }
        }
    }
}