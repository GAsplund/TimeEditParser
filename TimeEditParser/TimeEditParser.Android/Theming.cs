using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Plugin.CurrentActivity;
using TimeEditParser.Utilities;
using static TimeEditParser.Utilities.Theming;

[assembly: Xamarin.Forms.Dependency(
          typeof(TimeEditParser.Droid.Theming))]
namespace TimeEditParser.Droid
{
    public partial class Theming : ISystemTheme
    {
        public Theme GetSystemTheme()
        {
            //Ensure the device is running Android Froyo or higher because UIMode was added in Android Froyo, API 8.0
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Froyo)
            {
                var uiModeFlags = CrossCurrentActivity.Current.AppContext.Resources.Configuration.UiMode & UiMode.NightMask;

                switch (uiModeFlags)
                {
                    case UiMode.NightYes:
                        return Theme.Dark;

                    case UiMode.NightNo:
                        return Theme.Light;

                    default:
                        throw new NotSupportedException($"UiMode {uiModeFlags} not supported");
                }
            }
            else
            {
                return Theme.Light;
            }
        }
    }
}