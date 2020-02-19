using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TimeEditParser.Services;
using TimeEditParser.Views;
using Matcha.BackgroundService;
using Xamarin.Essentials;

namespace TimeEditParser
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            VersionTracking.Track();
            DependencyService.Register<MockDataStore>();
            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
            //BackgroundAggregatorService.Add(() => new UpdateScheduleTask(5));

            //Start the background service
            //BackgroundAggregatorService.StartBackgroundService();
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
