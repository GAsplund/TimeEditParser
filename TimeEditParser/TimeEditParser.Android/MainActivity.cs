using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace TimeEditParser.Droid
{
    [Activity(Label = "TimeEditParser", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.SetFlags("FastRenderers_Experimental");
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            LoadApplication(new App());
            Utilities.Theming.SetTheme();

            if (Notification.DayLastLesson != null)
            {
                Models.Day scheduleDay = ScheduleParser.SetSchedule()[0][ScheduleParser.TodayIndex() - 1];
                Notification.SetNotificationsForDay(scheduleDay);
            }

            // Register ScheduleBroadcastReceiver to fire every minute
            Android.Content.IntentFilter filter = new Android.Content.IntentFilter();
            filter.AddAction("android.intent.action.TIME_TICK");
            ScheduleBroadcastReceiver SchBroadcastRecv = new ScheduleBroadcastReceiver();
            Application.Context.RegisterReceiver(SchBroadcastRecv, filter);
            ScheduleBroadcastReceiver.CurrentInstance = SchBroadcastRecv;
            //Android.Content.ContextWrapper.RegisterReceiver(SchBroadcastRecv, filter);

        }


        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}