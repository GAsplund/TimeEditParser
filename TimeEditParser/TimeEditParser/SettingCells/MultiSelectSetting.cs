using System;
using System.Collections.Generic;
using System.Text;
using TimeEditParser.Models;
using TimeEditParser.Views;
using Xamarin.Forms;

namespace TimeEditParser.SettingCells
{
    class MultiSelectSetting : SubMenuSetting
    {
        public MultiSelectSetting(INavigation navigation) : base(navigation, new MultiSelectList())
        {
            parentNav = navigation;
            ((MultiSelectList)SubMenu).OnCheckedChanged += SettingCheckedChanged;
        }

        void SettingCheckedChanged(object sender, EventArgs args)
        {
            GrayText = (SubMenu as MultiSelectList).checkedItems.Count.ToString() + " selected";
        }

    }
}
