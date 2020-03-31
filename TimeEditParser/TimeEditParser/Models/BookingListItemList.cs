using System;
using System.Collections.Generic;
using System.Text;

namespace TimeEditParser.Models
{
    class BookingListItemList : List<BookingListItem>
    {
        public string Heading { get; set; }
        public string Date { get; set; }
        public string Week { get; set; }
        public List<BookingListItem> Items => this;
    }
}
