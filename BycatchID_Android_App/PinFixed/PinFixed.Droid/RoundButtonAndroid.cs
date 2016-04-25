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
using PinFixed;
using Android.Graphics;
using PinFixed.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRendererAttribute(typeof(RoundButtonAndroid), typeof(RoundButton))]

namespace PinFixed.Droid
{
    public class RoundButtonAndroid: ButtonRenderer
    {
        public RoundButtonAndroid()
		{
			this.SetWillNotDraw(false);
		}


        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == RoundButton.CornerRadiusProperty.PropertyName
                || e.PropertyName == RoundButton.StrokeProperty.PropertyName
                || e.PropertyName == RoundButton.StrokeThicknessProperty.PropertyName)
            {
                this.Invalidate();
            }
        }

        public override void Draw(Canvas canvas)
        {
            RoundButton rbv = (RoundButton)this.Element;

            Rect rc = new Rect();
            GetDrawingRect(rc);

            Rect interior = rc;
            interior.Inset((int)rbv.StrokeThickness, (int)rbv.StrokeThickness);

            Paint p = new Paint();
            //{
            //    Color = rbv.Color.ToAndroid(),
            //    AntiAlias = true,
            //};

            canvas.DrawRoundRect(new RectF(interior), (float)rbv.CornerRadius, (float)rbv.CornerRadius, p);

            p.Color = rbv.Stroke.ToAndroid();
            p.StrokeWidth = (float)rbv.StrokeThickness;
            p.SetStyle(Paint.Style.Stroke);

            canvas.DrawRoundRect(new RectF(rc), (float)rbv.CornerRadius, (float)rbv.CornerRadius, p);
        }
    }
}