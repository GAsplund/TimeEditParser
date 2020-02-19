using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Matcha.BackgroundService.Droid;

namespace TimeEditParser.Droid
{
    [Activity(Label = "TimeEditParser", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            BackgroundAggregator.Init(this);

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

            var intent = new Android.Content.Intent(this, typeof(ScheduleBroadcastReceiver));
            var pending = PendingIntent.GetBroadcast(this, 0, intent, PendingIntentFlags.UpdateCurrent);
            if(Notification.DayLastLesson != null) SendBroadcast(new Android.Content.Intent(this, typeof(ScheduleBroadcastReceiver)));
            var alarmManager = GetSystemService(AlarmService).JavaCast<AlarmManager>();
            DateTime now = DateTime.UtcNow;
            long firstNotification = (long)(new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0, DateTimeKind.Utc).AddMinutes(1) - new DateTime(1970, 1, 1)).TotalMilliseconds;
            alarmManager.SetRepeating(AlarmType.RtcWakeup, firstNotification, 20 * 1000, pending);
            ScheduleBroadcastReceiver recv = new ScheduleBroadcastReceiver();
            //RegisterReceiver(recv, new Android.Content.IntentFilter("ACTION_TIME_TICK"));
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}