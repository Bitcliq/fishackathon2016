using Foundation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(PinFixed.UI.CircleImage), typeof(PinFixed.iOS.ImageCircleRenderer))]
namespace PinFixed.iOS
{

     /// <summary>
    /// ImageCircle Implementation
    /// </summary>
    [Preserve(AllMembers = true)]
    public class ImageCircleRenderer : ImageRenderer
    {
        /// <summary>
        /// Used for registration with dependency service
        /// </summary>
        public async static void Init()
        {
            var temp = DateTime.Now;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
        {
            base.OnElementChanged(e);
            if (Element == null)
                return;
            CreateCircle();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == VisualElement.HeightProperty.PropertyName ||
                e.PropertyName == VisualElement.WidthProperty.PropertyName ||
              e.PropertyName == PinFixed.UI.CircleImage.BorderColorProperty.PropertyName ||
              e.PropertyName == PinFixed.UI.CircleImage.BorderThicknessProperty.PropertyName ||
              e.PropertyName == PinFixed.UI.CircleImage.FillColorProperty.PropertyName)
            {
                CreateCircle();
            }
        }

        private void CreateCircle()
        {
            try
            {
                double min = Math.Min(Element.Width, Element.Height);
                Control.Layer.CornerRadius = (float)(min / 2.0);
                Control.Layer.MasksToBounds = false;
                Control.Layer.BorderColor = ((PinFixed.UI.CircleImage)Element).BorderColor.ToCGColor();
                Control.Layer.BorderWidth = ((PinFixed.UI.CircleImage)Element).BorderThickness;
                Control.BackgroundColor = ((PinFixed.UI.CircleImage)Element).FillColor.ToUIColor();
                Control.ClipsToBounds = true;
            }
            catch (Exception ex)
            {
                //Debug.WriteLine("Unable to create circle image: " + ex);
            }
        }
    }
}
