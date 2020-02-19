using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace TimeEditParser.Droid
{
    [BroadcastReceiver]
    class ServiceCheckerDroid : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            if (Notification.DayLastLesson == null) return;
            //int result = DateTime.Compare(Notification.DayFirstLesson, DateTime.Now);
            //if (Notification.DayLastLesson)
            var alarmManager = Application.Context.GetSystemService(Context.AlarmService).JavaCast<AlarmManager>();
            PendingIntent sender = PendingIntent.GetBroadcast(Application.Context, 0, intent, 0);
            SendBroadcast(new Intent(Application.Context, typeof(ScheduleUpdateBroadcastReceiver)));
            var alarmManager = Application.Context.GetSystemService(Context.AlarmService).JavaCast<AlarmManager>();
            DateTime now = DateTime.UtcNow;
            long firstNotification = (long)(new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0, DateTimeKind.Utc).AddMinutes(1) - new DateTime(1970, 1, 1)).TotalMilliseconds;
            alarmManager.SetRepeating(AlarmType.Rtc, firstNotification, 60 * 1000, pending);
        }
    }
}