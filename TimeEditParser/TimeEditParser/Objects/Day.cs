using System;
using System.Collections.Generic;
using System.Text;

namespace TimeEditParser.Models
{
    public class Day : List<Booking>
    {
        public DateTime Date;
        public Day(DateTime date)
        {
            this.Date = date;
        }
    }
}
