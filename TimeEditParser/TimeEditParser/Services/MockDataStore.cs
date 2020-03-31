using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeEditParser.Models;

namespace TimeEditParser.Services
{
    public class MockDataStore : IDataStore<BookingListItem>
    {
        List<BookingListItem> items;

        public MockDataStore()
        {
            items = new List<BookingListItem>();
            var mockItems = new List<BookingListItem>
            {
                //new Item { Id = Guid.NewGuid().ToString(), Text = "First item", Description="This is an item description." },
            };

            foreach (var item in mockItems)
            {
                items.Add(item);
            }
        }

        public async Task<bool> AddItemAsync(BookingListItem item)
        {
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(BookingListItem item)
        {
            var oldItem = items.Where((BookingListItem arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(oldItem);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var oldItem = items.Where((BookingListItem arg) => arg.Id == id).FirstOrDefault();
            items.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<BookingListItem> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<BookingListItem>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }
    }
}