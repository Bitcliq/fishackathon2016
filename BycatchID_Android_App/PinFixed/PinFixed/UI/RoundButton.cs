using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PinFixed
{
    public class RoundButton : Button
    {
        public RoundButton()
        {
        }

        public static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create<RoundButton, double>(
            p => p.CornerRadius, default(double));
    
        public double CornerRadius
        {
            get { return (double)base.GetValue(CornerRadiusProperty); }
            set { base.SetValue(CornerRadiusProperty, value); }
        }

        public static readonly BindableProperty StrokeProperty =
            BindableProperty.Create<RoundButton, Color>(
                p => p.Stroke, Color.Transparent);

        public Color Stroke
        {
            get { return (Color)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }

        public static readonly BindableProperty StrokeThicknessProperty =
            BindableProperty.Create<RoundButton, double>(
                p => p.StrokeThickness, default(double));

        public double StrokeThickness
        {
            get { return (double)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }

        public static readonly BindableProperty HasShadowProperty =
            BindableProperty.Create<RoundButton, bool>(
                p => p.HasShadow, default(bool));

        public bool HasShadow
        {
            get { return (bool)GetValue(HasShadowProperty); }
            set { SetValue(HasShadowProperty, value); }
        }
            
    }
}
