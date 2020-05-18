using System;
using System.Collections.Generic;
using System.Text;
using TimeEditParser.CustomObjects;
using Xamarin.Forms;

namespace TimeEditParser.SettingCells
{
    class TextSetting : Setting
    {
        public TextSetting()
        {
            TextBox = new ColoredEntry();
            TextBox.SetDynamicResource(ColoredEntry.TextColorProperty, "PrimaryTextColor");
            TextBox.SetDynamicResource(ColoredEntry.BorderColorProperty, "ListViewSeparatorColor");
        }
        internal View TextBox
        {
            set
            {
                base.Element = value;
            }
            get
            {
                return Element;
            }
        }

        public string Text
        {
            get
            {
                return (TextBox as ColoredEntry).Text;
            }
            set
            {
                (TextBox as ColoredEntry).Text = value;
            }
        }

        public event EventHandler Completed
        {
            add { (Element as ColoredEntry).Completed += value; }
            remove { (Element as ColoredEntry).Completed -= value; }
        }

        public string Placeholder
        {
            get { return (TextBox as ColoredEntry).Placeholder; }
            set { (TextBox as ColoredEntry).Placeholder = value; }
        }

    }
}
