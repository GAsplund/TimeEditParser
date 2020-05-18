using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace TimeEditParser.CustomObjects
{
    public class ColoredSearchBar : SearchBar
    {
        public static readonly BindableProperty IconColorProperty =
        BindableProperty.Create(nameof(IconColor), typeof(Color), typeof(ColoredSearchBar));
        public Color IconColor
        {
            set { SetValue(IconColorProperty, value); }
            get { return (Color)GetValue(IconColorProperty); }
        }

    }
}

