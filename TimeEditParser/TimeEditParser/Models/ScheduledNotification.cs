using System;
using System.Collections.Generic;
using System.Text;

namespace TimeEditParser.Models
{
    public class ScheduledNotification
    {
        public enum NotificationType
        {
            AboutToStart,
            Start,
            AboutToEnd,
            End
        }
        public Booking Booking;
        public NotificationType Type;
        public DateTime Time;
    }
}
