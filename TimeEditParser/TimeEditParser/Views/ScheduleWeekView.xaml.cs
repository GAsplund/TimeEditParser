using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeEditParser.Models;
using TimeEditParser.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TimeEditParser.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScheduleWeekView : ContentView
    {
        public ScheduleWeekView()
        {
            InitializeComponent();
        }

        // Event called when a lesson is selected
        async void OnItemSelected(ListView sender, EventArgs e)
        {
            //Get the booking to pass to the ItemDetailPage
            //var index = (sender.ItemsSource as List<Item>).IndexOf(sender.SelectedItem as Item);
            sender.SelectedItem = null;

            BookingListItem item = (e as SelectedItemChangedEventArgs).SelectedItem as BookingListItem;
            if (item == null)
                return;

            if (item.Text == "(No lessons for this day)")
                return;

            await Navigation.PushModalAsync(new NavigationPage(new ItemDetailPage(new ItemDetailViewModel(item))));
        }
    }
}