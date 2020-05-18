using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace TimeEditParser.CustomObjects
{
    public partial class ColoredTableView : TableView
    {
        public static BindableProperty SeparatorColorProperty = BindableProperty.Create("SeparatorColor", typeof(Color), typeof(ColoredTableView), Color.White);
        public Color SeparatorColor
        {
            get
            {
                return (Color)GetValue(SeparatorColorProperty);
            }
            set
            {
                SetValue(SeparatorColorProperty, value);
            }
        }
        public ColoredTableView()
        {
            //InitializeComponent();
        }
    }
}
