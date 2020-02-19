using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using static TimeEditParser.Utilities.Theming;

[assembly: Xamarin.Forms.Dependency(
          typeof(TimeEditParser.iOS.Theming))]
namespace TimeEditParser.iOS
{
    public partial class Theming : Utilities.ISystemTheme
    {
        public Theme GetSystemTheme()
        {
            //Ensure the current device is running 12.0 or higher, because `TraitCollection.UserInterfaceStyle` was introduced in iOS 12.0
            if (UIDevice.CurrentDevice.CheckSystemVersion(12, 0))
            {
                var currentUIViewController = GetVisibleViewController();

                var userInterfaceStyle = currentUIViewController.TraitCollection.UserInterfaceStyle;

                switch (userInterfaceStyle)
                {
                    case UIUserInterfaceStyle.Light:
                        return Theme.Light;
                    case UIUserInterfaceStyle.Dark:
                        return Theme.Dark;
                    default:
                        throw new NotSupportedException($"UIUserInterfaceStyle {userInterfaceStyle} not supported");
                }
            }
            else
            {
                return Theme.Light;
            }
        }

        static UIViewController GetVisibleViewController()
        {
            UIViewController viewController = null;

            var window = UIApplication.SharedApplication.KeyWindow;

            if (window.WindowLevel == UIWindowLevel.Normal)
                viewController = window.RootViewController;

            if (viewController is null)
            {
                window = UIApplication.SharedApplication
                    .Windows
                    .OrderByDescending(w => w.WindowLevel)
                    .FirstOrDefault(w => w.RootViewController != null && w.WindowLevel == UIWindowLevel.Normal);

                viewController = window?.RootViewController ?? throw new InvalidOperationException("Could not find current view controller.");
            }

            while (viewController.PresentedViewController != null)
                viewController = viewController.PresentedViewController;

            return viewController;
        }

    }
}