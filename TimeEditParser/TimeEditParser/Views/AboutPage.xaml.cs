using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace TimeEditParser.Views
{
    public partial class AboutPage : ContentPage
    {
        public string StatusText { set { StatusLabel.Text = value; } }
        public AboutPage()
        {
            InitializeComponent();
            BuildNumberLabel.Text = VersionTracking.CurrentVersion + " Build " + VersionTracking.CurrentBuild;
        }
    }
}