using Android.Content.Res;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace TimeEditParser.SettingCells
{
    class SubMenuSetting : Setting
    {
        internal INavigation parentNav;
        public SubMenuSetting(INavigation navigation, Page SubMenu = null)
        {
            parentNav = navigation;
            this.SubMenu = SubMenu;
            Tapped += SettingTapped;
            GrayLabel = new Label
            {
                HorizontalOptions = LayoutOptions.EndAndExpand,
                VerticalOptions = LayoutOptions.Center
            };
            GrayLabel.SetDynamicResource(Xamarin.Forms.Label.TextColorProperty, "SecondaryTextColor");
        }

        internal View GrayLabel
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

        public string GrayText
        {
            set
            {
                ((Label)GrayLabel).Text = value;
            }
            get
            {
                return ((Label)GrayLabel).Text;
            }
        }

        private void SettingTapped(object sender, EventArgs args)
        {
            if (SubMenu == null) return;
            parentNav.PushModalAsync(new NavigationPage(SubMenu));
        }

        internal Page SubMenu;
    }
}
