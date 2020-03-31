using System;

namespace TimeEditParser.Models
{
    public class BookingListItem
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public string Description { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public bool DayHasLessons { get; set; }
        public Booking Booking { get; set; }
    }
}