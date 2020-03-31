using System;
using System.Collections.Generic;
using System.Text;

namespace TimeEditParser.Models
{
    class ScheduleWeekListItem : List<BookingListItemList>
    {
        public string Week { get; set; }
    }
}
