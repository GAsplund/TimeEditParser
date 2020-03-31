using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace TimeEditParser.SettingCells
{
    class TextSetting : Setting
    {
        public TextSetting()
        {
            TextBox = new Entry();
            TextBox.SetDynamicResource(Entry.TextColorProperty, "PrimaryTextColor");
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
                return (TextBox as Entry).Text;
            }
            set
            {
                (TextBox as Entry).Text = value;
            }
        }

        public event EventHandler Completed
        {
            add { (Element as Entry).Completed += value; }
            remove { (Element as Entry).Completed -= value; }
        }

        public string Placeholder
        {
            get { return (TextBox as Entry).Placeholder; }
            set { (TextBox as Entry).Placeholder = value; }
        }

    }
}
