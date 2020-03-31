using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace TimeEditParser.SettingCells
{
    class SwitchSetting : Setting
    {
        public SwitchSetting()
        {
            Switch = new Switch();
        }

        internal View Switch
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

        public event EventHandler<ToggledEventArgs> OnChanged
        {
            add { (Element as Switch).Toggled += value; }
            remove { (Element as Switch).Toggled -= value; }
        }

        public bool On
        {
            get
            {
                return (Switch as Switch).IsToggled;
            }
            set
            {
                (Switch as Switch).IsToggled = value;
            }
        }

    }
}
