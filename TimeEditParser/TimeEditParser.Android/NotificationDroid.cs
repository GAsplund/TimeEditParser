using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.V4.App;
using TimeEditParser.Utilities;
using Android.Content;
using TimeEditParser.Models;
using System.Globalization;

[assembly: Xamarin.Forms.Dependency(
          typeof(TimeEditParser.Droid.NotificationDroid))]
namespace TimeEditParser.Droid
{
    public class NotificationDroid
    {
        private NotificationCompat.Builder builder;
        private NotificationManager notificationManager;
        private NotificationChannel channel;

        public bool notify = false;

        public NotificationDroid()
        {
            notificationManager = Application.Context.GetSystemService(Context.NotificationService) as NotificationManager;
            builder = new NotificationCompat.Builder(Application.Context, "0");
            builder.SetSmallIcon(Resource.Drawable.navigation_empty_icon);

            if (!(Build.VERSION.SdkInt < BuildVersionCodes.O))
            {
                // Notification channels are new in API 26 (and not a part of the
                // support library). There is no need to create a notification
                // channel on older versions of Android.

                var channelName = "Schedule";
                var channelDescription = "Notification for the schedule";
                channel = new NotificationChannel("0", channelName, NotificationImportance.Default)
                {
                    Description = channelDescription
                };

                notificationManager.CreateNotificationChannel(channel);
            }
        }

        public void NotifyNotification(ScheduledNotification scheduledEvent, bool notify = false)
        {

            if (ApplicationSettings.UseActiveNotification) SendNotificationStatic(scheduledEvent);
            else SendNotification(scheduledEvent);
            //DateTime TimeAfterDelay = DateTime.Now.AddHours(10);
            //if (DateTime.Compare(TimeEditParser.Notification.DayLastLesson.Time, TimeAfterDelay) > 0 && DateTime.Compare(TimeEditParser.Notification.DayLastLesson.Time, DateTime.Now) < 0) SendDayEnded(scheduledEvent);


        }

        public void SendDayEnded(ScheduledNotification scheduledEvent)
        {
            DateTime time = scheduledEvent.Time;
            TimeSpan timeLeft = DateTime.Now - time;
            builder.SetContentTitle("School day ended")
                               .SetContentText("Day ended " + Math.Round(timeLeft.TotalMinutes).ToString() + " minutes ago");

            builder.SetOngoing(false);

            Intent onDismissIntent = new Intent(Application.Context, typeof(NotifDismissBroadcastReceiver));
            PendingIntent onDismissPendingIntent = PendingIntent.GetBroadcast(Application.Context, 0, onDismissIntent, 0);
            builder.SetDeleteIntent(onDismissPendingIntent);

            Android.App.Notification notification = builder.Build();

            // Publish the notification:
            const int notificationId = 0;
            notificationManager.Notify(notificationId, notification);
        }

        public void SendNotificationStatic(ScheduledNotification scheduledEvent) 
        {
            Booking lesson = scheduledEvent.Booking;
            DateTime time = scheduledEvent.Time;
            TimeSpan timeLeft;
            double timeLeftMinutes;
            switch (scheduledEvent.Type)
            {
                case ScheduledNotification.NotificationType.AboutToStart:
                    timeLeft = time - DateTime.Now;
                    timeLeftMinutes = Math.Floor(timeLeft.TotalMinutes) + 1;

                    builder.SetContentTitle("Current lesson: No lesson")
                           .SetContentText("Next up: " + lesson.name + " in " + timeLeftMinutes.ToString() + " minutes (" + lesson.StartTime + ") at " + lesson.Location + ".");
                    break;
                case ScheduledNotification.NotificationType.Start:
                    timeLeft = DateTime.ParseExact(lesson.EndTime, "HH:mm",
                          CultureInfo.InvariantCulture) - DateTime.Now;
                    timeLeftMinutes = Math.Floor(timeLeft.TotalMinutes) + 1;

                    builder.SetContentTitle("Current lesson: " + lesson.name + " at " + lesson.Location)
                           .SetContentText("Lesson ends in " + timeLeftMinutes.ToString() + " minutes (" + lesson.EndTime + ")");
                    break;
                case ScheduledNotification.NotificationType.AboutToEnd:
                    timeLeft = DateTime.ParseExact(lesson.EndTime, "HH:mm",
                            CultureInfo.InvariantCulture) - DateTime.Now;
                    timeLeftMinutes = Math.Floor(timeLeft.TotalMinutes) + 1;

                    builder.SetContentTitle("Current lesson: " + lesson.name + " at " + lesson.Location)
                           .SetContentText("Lesson ends in " + timeLeftMinutes.ToString() + " minutes (" + lesson.EndTime + ")");
                    break;
                case ScheduledNotification.NotificationType.End:
                    timeLeft = DateTime.ParseExact(lesson.StartTime, "HH:mm",
                                CultureInfo.InvariantCulture) - DateTime.Now;
                    timeLeftMinutes = Math.Floor(timeLeft.TotalMinutes) + 1;

                    builder.SetContentTitle("Current lesson: No lesson")
                           .SetContentText("Next up: " + lesson.name + " in " + timeLeftMinutes.ToString() + " minutes (" + lesson.StartTime + ") at " + lesson.Location + ".");
                    break;
            }
            builder.SetOngoing(true);
            if (notify) builder.SetVibrate(new long[] { 0L });
            else builder.SetVibrate(new long[] { 200L });

            Android.App.Notification notification = builder.Build();

            // Publish the notification:
            const int notificationId = 0;
            notificationManager.Notify(notificationId, notification);

        }

        public void SendNotification(ScheduledNotification scheduledEvent)
        {
            switch (scheduledEvent.Type)
            {
                case ScheduledNotification.NotificationType.AboutToStart:
                    break;
                case ScheduledNotification.NotificationType.Start:
                    break;
                case ScheduledNotification.NotificationType.AboutToEnd:
                    break;
                case ScheduledNotification.NotificationType.End:
                    break;
            }
            builder.SetOngoing(false);
            if (notify) builder.SetVibrate(new long[] { 0L });
            else builder.SetVibrate(new long[] { 200L });
            Android.App.Notification notification = builder.Build();

            // Publish the notification:
            const int notificationId = 0;
            notificationManager.Notify(notificationId, notification);
        }

    }
}