using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PinFixed.UI
{
    public class RoundTextBox : Entry
    {
        public RoundTextBox()
        {
        }

        public static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create<RoundTextBox, double>(
            p => p.CornerRadius, default(double));
    
        public double CornerRadius
        {
            get { return (double)base.GetValue(CornerRadiusProperty); }
            set { base.SetValue(CornerRadiusProperty, value); }
        }

        public static readonly BindableProperty StrokeProperty =
            BindableProperty.Create<RoundTextBox, Color>(
                p => p.Stroke, Color.Transparent);

        public Color Stroke
        {
            get { return (Color)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }

        public static readonly BindableProperty StrokeThicknessProperty =
            BindableProperty.Create<RoundTextBox, double>(
                p => p.StrokeThickness, default(double));

        public double StrokeThickness
        {
            get { return (double)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }

        public static readonly BindableProperty HasShadowProperty =
            BindableProperty.Create<RoundTextBox, bool>(
                p => p.HasShadow, default(bool));

        public bool HasShadow
        {
            get { return (bool)GetValue(HasShadowProperty); }
            set { SetValue(HasShadowProperty, value); }
        }
    }
}
