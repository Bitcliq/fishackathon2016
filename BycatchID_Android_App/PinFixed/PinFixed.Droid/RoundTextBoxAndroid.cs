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
using PinFixed.UI;
using Android.Graphics;
namespace PinFixed.Droid
{
   public  class RoundTextBoxAndroid : EntryRenderer
    {
          public RoundTextBoxAndroid()
		{
			this.SetWillNotDraw(false);
		}


        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == RoundTextBox.CornerRadiusProperty.PropertyName
                || e.PropertyName == RoundTextBox.StrokeProperty.PropertyName
                || e.PropertyName == RoundTextBox.StrokeThicknessProperty.PropertyName)
            {
                this.Invalidate();
            }
        }

        public override void Draw(Canvas canvas)
        {
            RoundTextBox rbv = (RoundTextBox)this.Element;

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