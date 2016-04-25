using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using Xamarin.Forms.Platform.iOS;
using PinFixed;
using Xamarin.Forms;
using System.Drawing;
using PinFixed.iOS;
[assembly: ExportRendererAttribute(typeof(RoundButtoniOS), typeof(RoundButton))]
namespace PinFixed.iOS
{
    public class RoundButtoniOS : ViewRenderer<RoundButton, UIView>
    {
        UIView childView;

        protected override void OnElementChanged(ElementChangedEventArgs<RoundButton> e)
        {
            base.OnElementChanged(e);

            var rbv = e.NewElement;
            if (rbv != null)
            {
                var shadowView = new UIView();

                childView = new UIView()
                {
                    //BackgroundColor = rbv.Color.ToUIColor(),
                    Layer =
                    {
                        CornerRadius = (float)rbv.CornerRadius,
                        BorderColor = rbv.Stroke.ToCGColor(),
                        BorderWidth = (float)rbv.StrokeThickness,
                        MasksToBounds = true
                    },
                    AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight
                };

                shadowView.Add(childView);

                if (rbv.HasShadow)
                {
                    shadowView.Layer.ShadowColor = UIColor.Black.CGColor;
                    shadowView.Layer.ShadowOffset = new SizeF(3, 3);
                    shadowView.Layer.ShadowOpacity = 1;
                    shadowView.Layer.ShadowRadius = 5;
                }

                SetNativeControl(shadowView);
            }
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == RoundButton.CornerRadiusProperty.PropertyName)
                childView.Layer.CornerRadius = (float)this.Element.CornerRadius;
            else if (e.PropertyName == RoundButton.StrokeProperty.PropertyName)
                childView.Layer.BorderColor = this.Element.Stroke.ToCGColor();
            else if (e.PropertyName == RoundButton.StrokeThicknessProperty.PropertyName)
                childView.Layer.BorderWidth = 120;//(float)this.Element.StrokeThickness;
       

            else if (e.PropertyName == RoundButton.HasShadowProperty.PropertyName)
            {
                if (Element.HasShadow)
                {
                    NativeView.Layer.ShadowColor = UIColor.Black.CGColor;
                    NativeView.Layer.ShadowOffset = new SizeF(3, 3);
                    NativeView.Layer.ShadowOpacity = 1;
                    NativeView.Layer.ShadowRadius = 5;
                }
                else
                {
                    NativeView.Layer.ShadowColor = UIColor.Clear.CGColor;
                    NativeView.Layer.ShadowOffset = new SizeF();
                    NativeView.Layer.ShadowOpacity = 0;
                    NativeView.Layer.ShadowRadius = 0;
                }
            }
        }
    }

    
}