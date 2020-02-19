using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TimeEditParser.Views
{
    public partial class MainPage : TabbedPage
    {
        public MainPage()
        {
            InitializeComponent();
            if (ApplicationSettings.FirstTimeUse)
            {
                Navigation.PushModalAsync(new NavigationPage(new WelcomePage()));
            }
        }
    }
}