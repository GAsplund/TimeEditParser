using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace TimeEditParser.CustomObjects
{
    public class ColoredEntry : Entry
    {
        public ColoredEntry() { }

        public static readonly BindableProperty BorderColorProperty =
            BindableProperty.Create("BorderColor",
                                    typeof(Color),
                                    typeof(ColoredEntry),
                                    Color.Black);

        public Color BorderColor
        {
            get { return (Color)this.GetValue(BorderColorProperty); }
            set { this.SetValue(BorderColorProperty, value); }
        }

    }
}
