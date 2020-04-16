using System;
using Xamarin.Forms;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using TimeEditParser.Models;

namespace TimeEditParser
{
    public class Notification
    {
        // Public list of scheduled notifications, to be accessed by other classes.
        public static List<ScheduledNotification> scheduledEvents = new List<ScheduledNotification>();
        public static List<DateTime> scheduledNotifications = new List<DateTime>();

        public static ScheduledNotification DayFirstLesson;
        public static ScheduledNotification DayLastLesson;
        public static ScheduledNotification LastEvent;

        public static void ScheduleNotification(ScheduledNotification.NotificationType type, DateTime time, Booking lesson)
        {
            ScheduledNotification notification = new ScheduledNotification
            {
                Booking = lesson,
                Time = time,
                Type = type
            };
            scheduledEvents.Add(notification);
        }


        // Removes all stored and scheduled notifications
        public static void ClearNotifications()
        {
            scheduledEvents.Clear();
        }

        public static DateTime LastSetNotifications = DateTime.MinValue;

        public static DateTime DayEnd()
        {
            if (DayLastLesson == null) return DateTime.MinValue;
            else return DayLastLesson.Time;
        }

        public static bool DayEnded()
        {
            if (DayLastLesson == null) return true;
            else return DateTimePassed(DayLastLesson.Time);
        }

        public static (ScheduledNotification, bool) GetUpcomingEvent()
        {
            bool eventPassed = false;
            foreach (ScheduledNotification Event in scheduledEvents.ToList())
            {
                if (!ApplicationSettings.UseActiveNotification && (Event.Type == ScheduledNotification.NotificationType.AboutToEnd)) 
                { 
                    Event.Time.AddMinutes(-ApplicationSettings.MinutesBeforeNotification); 
                }
                else if (!ApplicationSettings.UseActiveNotification && (Event.Type == ScheduledNotification.NotificationType.AboutToStart)) 
                { 
                    Event.Time.AddMinutes(-ApplicationSettings.MinutesAfterNotification); 
                }
                if (DateTimePassed(Event.Time)) 
                { 
                    LastEvent = Event; scheduledEvents.Remove(Event);
                    eventPassed = true;
                }
                else break;
            }
            
            if (scheduledEvents.Count == 0) return (null, false);
            else if (eventPassed) return (scheduledEvents.First(), eventPassed);
            //if (CheckBookingOngoing(scheduledEvents.First().Booking)) return scheduledEvents.First();
            else return (scheduledEvents.First(), false);
        }

        static bool CheckBookingOngoing(Booking booking)
        {
            TimeSpan start = TimeSpan.ParseExact(booking.StartTime, "HH:mm",
                              CultureInfo.InvariantCulture);
            TimeSpan end = TimeSpan.ParseExact(booking.EndTime, "HH:mm",
                              CultureInfo.InvariantCulture);
            TimeSpan now = DateTime.Now.TimeOfDay;
            // see if start comes before end
            if (start < end)
                return start <= now && now <= end;
            // start is after end, so do the inverse comparison
            return !(end < now && now < start);
        }

        public static void SetNotificationsForDay(Day day)
        {
            // Only schedule notifications if it wasn't checked earlier today
            if (LastSetNotifications.Day != DateTime.Now.Day)
            {
                if (day.Count >= 1)
                {
                    DayFirstLesson = new ScheduledNotification
                    {
                        Booking = day.First(),
                        Time = DateTime.ParseExact(day.First().StartTime, "HH:mm", CultureInfo.InvariantCulture),
                        Type = ScheduledNotification.NotificationType.Start
                    };

                    DayLastLesson = new ScheduledNotification
                    {
                        Booking = day.Last(),
                        Time = DateTime.ParseExact(day.Last().EndTime, "HH:mm", CultureInfo.InvariantCulture),
                        Type = ScheduledNotification.NotificationType.End
                    };
                }

                // TODO: Make app not crash when TodayIndex is higher than schedule count
                foreach (Booking lesson in day)
                {
                    DateTime endTime = DateTime.ParseExact(lesson.EndTime, "HH:mm",
                              CultureInfo.InvariantCulture);
                    DateTime startTime = DateTime.ParseExact(lesson.StartTime, "HH:mm",
                              CultureInfo.InvariantCulture);

                    // Add notification for notification before lesson end IF IT'S NOT 0
                    if (ApplicationSettings.SendNotificationBefore == true && startTime.CompareTo(DateTime.Now) > 0 && ApplicationSettings.MinutesBeforeNotification > 0)
                        Notification.ScheduleNotification(ScheduledNotification.NotificationType.AboutToStart, startTime, lesson);

                    // Add notification for lesson start and lesson end, but only if their times have not passed yet
                    if (ApplicationSettings.SendNotificationAtStart == true && startTime.CompareTo(DateTime.Now) > 0)
                        Notification.ScheduleNotification(ScheduledNotification.NotificationType.Start, startTime, lesson);

                    if (ApplicationSettings.SendNotificationAfter == true && endTime.CompareTo(DateTime.Now) > 0 && ApplicationSettings.MinutesAfterNotification > 0)
                        Notification.ScheduleNotification(ScheduledNotification.NotificationType.AboutToEnd, endTime, lesson);

                    if (ApplicationSettings.SendNotificationAtEnd == true && endTime.CompareTo(DateTime.Now) > 0)
                        Notification.ScheduleNotification(ScheduledNotification.NotificationType.End, endTime, lesson);

                }

            }

            LastSetNotifications = DateTime.Now;
        }

        static bool DateTimePassed(DateTime check)
        {
            return DateTime.Compare(check, DateTime.Now) < 0;
        }

        static List<ScheduledNotification> scheduledEventsReversed()
        {
            List<ScheduledNotification> reversedList = scheduledEvents;
            reversedList.Reverse();
            return reversedList;
        }

    }
}
