using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace TimeEditParser.SettingCells
{
    internal class PickerSetting : Setting
    {
        public PickerSetting()
        {
            Picker = new Picker();
            Tapped += settingTapped;
        }
        internal View Picker
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
        private void settingTapped(object sender, EventArgs args)
        {
            if (!Picker.IsFocused) Picker.Focus();
        }
    }
}
