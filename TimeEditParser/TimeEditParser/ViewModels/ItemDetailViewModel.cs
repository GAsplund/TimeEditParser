using System;

using TimeEditParser.Models;

namespace TimeEditParser.ViewModels
{
    public class ItemDetailViewModel : BaseViewModel
    {
        public BookingListItem Item { get; set; }
        public ItemDetailViewModel(BookingListItem item = null)
        {
            Title = item?.Text;
            Item = item;
        }
    }
}
