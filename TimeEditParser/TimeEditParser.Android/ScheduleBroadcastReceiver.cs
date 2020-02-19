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
using TimeEditParser.Models;

namespace TimeEditParser.Droid
{
    [BroadcastReceiver]
    //[Android.Runtime.Register("android.intent.action.TIME_TICK")]
    class ScheduleBroadcastReceiver : BroadcastReceiver
    {
        static NotificationDroid notif = new NotificationDroid();
        public static bool DayEndDismissed = false;

        public override void OnReceive(Context context, Intent intent)
        {
            (ScheduledNotification notification, bool eventPassed) = Notification.GetUpcomingEvent();

            if (Notification.DayLastLesson == null) return;

            if (notification == null && Notification.DayEnded())
            {
                // Notification for day end was dismissed, terminate the PendingIntent
                if (DayEndDismissed) PendingIntent.GetBroadcast(Application.Context, 0, intent, 0).Cancel();
                // Day has ended, send day ended notification, dynamically updating
                else if (ApplicationSettings.UseActiveNotification) notif.SendDayEnded(Notification.DayLastLesson);
                // Day has ended, send day ended notification
                else { notif.SendDayEnded(notification); DayEndDismissed = true; }
                return;
            }
            else if (notification == null) return;

            // Send ongoing notification
            if (ApplicationSettings.UseActiveNotification)
            {   
                notif.notify = eventPassed;
                notif.SendNotificationStatic(notification);
                DayEndDismissed = false;
            }
            // Send normal notification
            else if (eventPassed)
            {
                notif.notify = true;
                notif.SendNotification(notification);
                DayEndDismissed = false;
            }
        }
    }
}