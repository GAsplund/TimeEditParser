﻿using System;
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
    class ScheduleBroadcastReceiver : BroadcastReceiver
    {
        static NotificationDroid notif = new NotificationDroid();
        public static ScheduleBroadcastReceiver CurrentInstance;

        public override void OnReceive(Context context, Intent intent)
        {
            CurrentInstance = this;
            (ScheduledNotification notification, bool eventPassed) = Notification.GetUpcomingEvent();

            // Sanity check: There should always be a last lesson of the day. Otherwise wait until next broadcast.
            if (Notification.DayLastLesson == null) return;

            // Check if day has ended
            if (notification == null && Notification.DayEnded())
            {
                // Day has ended, send day ended notification, updating every minute
                if (ApplicationSettings.UseActiveNotification) notif.SendDayEnded(Notification.DayLastLesson);
                // Day has ended, send day ended notification and unregister this BroadcastReceiver
                else { notif.SendDayEnded(notification); Application.Context.UnregisterReceiver(this); }
                return;
            }
            // Do not send status for next upcoming event if there is no upcoming event
            else if (notification == null) return;

            // Send ongoing notification (display minutes left)
            if (ApplicationSettings.UseActiveNotification)
            {   
                notif.notify = eventPassed;
                notif.SendNotificationStatic(notification);
            }

            // Send normal notification (only display if something happened)
            else if (eventPassed)
            {
                notif.notify = true;
                notif.SendNotification(notification);
            }
        }
    }
}