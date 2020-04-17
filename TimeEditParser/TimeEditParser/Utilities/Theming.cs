using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

#if __ANDROID__
using System;
using System.Threading.Tasks;
using Android.Content.Res;
using Plugin.CurrentActivity;
#elif __IOS__
using UIKit;
#endif

namespace TimeEditParser.Utilities
{
    public partial class Theming
    {
        public enum Theme
        {
            Dark,
            Light
        }
        public static void SetTheme()
        {
            ICollection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;
            if (mergedDictionaries != null)
            {
                mergedDictionaries.Clear();

                Theme systemTheme;

                if (!ApplicationSettings.ForceSetTheme) systemTheme = DependencyService.Get<ISystemTheme>().GetSystemTheme();
                else
                {
                    if (ApplicationSettings.EnableDarkTheme) systemTheme = Theme.Dark;
                    else systemTheme = Theme.Light;
                }
                

                switch (systemTheme)
                {
                    case Theme.Dark:
                        mergedDictionaries.Add(new Themes.DarkTheme());
                        break;
                    case Theme.Light:
                    default:
                        mergedDictionaries.Add(new Themes.LightTheme());
                        break;
                }
            }

        }
    }
}
