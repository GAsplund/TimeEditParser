using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using TimeEditParser.Models;
using TimeEditParser.Views;
using TimeEditParser.ViewModels;
using Plugin.LocalNotifications;
using System.Globalization;

namespace TimeEditParser.Views
{
    public partial class SchedulePage : ContentPage
    {
        ItemsViewModel viewModel;

        public SchedulePage()
        {
            InitializeComponent();
            BindingContext = viewModel = new ItemsViewModel();
        }

        // Event called when a lesson is selected
        async void OnItemSelected(ListView sender, EventArgs e)
        {
            //Get the booking to pass to the ItemDetailPage
            //var index = (sender.ItemsSource as List<Item>).IndexOf(sender.SelectedItem as Item);
            ((ListView)sender).SelectedItem = null;

            BookingListItem item = (e as SelectedItemChangedEventArgs).SelectedItem as BookingListItem;
            if (item == null)
                return;

            if (item.Text == "(No lessons for this day)")
                return;

            await Navigation.PushModalAsync(new NavigationPage(new ItemDetailPage(new ItemDetailViewModel(item))));

            // Manually deselect item
            //TodayScheduleListView.SelectedItem = null;
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewItemPage()));
        }

        async void LaunchFiltersPage(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new FiltersPage()));
        }

        // Event processed everytime the schedule is initially rendered
        async protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);

            await SetSchedule();
        }

        // Event processed everytime listView is pulled to update
        async void UpdateSchedule(object sender, EventArgs args)
        {
            await SetSchedule();
        }


        async Task SetSchedule()
        {
            // Declare variables with fallbacks
            Week schedule = new Week();
            //ListView TargetListView = TodayScheduleListView;
            List<Week> scheduleWeeks;

            // Attempt to get the schedule for the whole week, and save it in the cache
            try
            {   
                scheduleWeeks = await Task.Run(() => ScheduleParser.GetSchedule());
            }
            catch
            {
                Console.WriteLine("Failed to retreive and parse schedule.");

                Refresher.IsRefreshing = false;
                await DisplayAlert("Error", "Could not fetch schedule website properly. Have you set the link in settings?", "Ok");
                return;
            }

            ScheduleWeekListItem ScheduleWeekItem = new ScheduleWeekListItem();

            // Attempt to read data and display it
            try
            {
                Application.Current.Properties["scheduleWeeksCache"] = scheduleWeeks;

                foreach (int weekIndex in Enumerable.Range(1, scheduleWeeks.Count))
                {
                    switch (weekIndex)
                    {
                        case 1:
                            schedule = scheduleWeeks[0];
                            ScheduleWeekView scheduleWeek = new ScheduleWeekView();
                            
                            AddScheduleWeek(schedule, true, ScheduleWeekItem);
                            if (ScheduleParser.TodayIndex() - 1 <= schedule.Count) Notification.SetNotificationsForDay(schedule[ScheduleParser.TodayIndex() - 1]);
                            break;
                        case 2:
                            schedule = scheduleWeeks[1];
                            AddScheduleWeek(schedule, false, ScheduleWeekItem);
                            break;
                    }
                }
                ScheduleWeekView TargetListView = new ScheduleWeekView();
                ScheduleWeeksStackLayout.Children.Add(TargetListView);
                TargetListView.TodayScheduleListView.ItemsSource = ScheduleWeekItem;
            }
            catch (Exception e)
            {
                if (!(e is ArgumentOutOfRangeException))
                {
                    Console.WriteLine(e.Message + "\n" + e.StackTrace);
                    Refresher.IsRefreshing = false;
                    await DisplayAlert("Error", "Could not parse schedule data.\nMessage: " + e.Message, "Ok");
                }
                return;
            }
            Refresher.IsRefreshing = false;
        }


        // Set the items for the target listView
        private void AddScheduleWeek(Week ScheduleWeek, bool isCurrentWeek, ScheduleWeekListItem ScheduleWeekItem)
        {
            // Create a list of each lesson
            
            ScheduleWeekItem.Add(new BookingListItemList { IsWeekHeader = true, Date = "Week " + (ScheduleWeek.First().Date.DayOfYear / 7).ToString()});

            foreach (int i in Enumerable.Range(0, ScheduleWeek.Count))
            {
                BookingListItemList ScheduleDay = new BookingListItemList();
                foreach (Booking lessson in ScheduleWeek[i])
                {
                    ScheduleDay.Add(LessonToItem(lessson));
                }

                if (ScheduleWeek[i].Count == 0)
                {
                    ScheduleDay.Add(new BookingListItem { Text = "(No lessons for this day)", DayHasLessons = false });
                }

                ScheduleDay.IsWeekHeader = false;
                ScheduleWeekItem.Add(ScheduleDay);

                // Only set the day names relative to today if it's the current week.
                if (isCurrentWeek)
                {
                    switch (ScheduleWeekItem.Count - ScheduleParser.TodayIndex())
                    {
                        case 0:
                            ScheduleDay.Heading = "Today";
                            break;
                        case -1:
                            ScheduleDay.Heading = "Yesterday";
                            break;
                        case 1:
                            ScheduleDay.Heading = "Tomorrow";
                            break;
                        default:
                            ScheduleDay.Heading = ScheduleWeek[i].Date.DayOfWeek.ToString();
                            break;
                    }
                    ScheduleDay.Date = ScheduleWeek[i].Date.ToString("yyyy-MM-dd");
                }
                else
                {
                    ScheduleDay.Heading = ScheduleWeek[i].Date.AddDays(7).DayOfWeek.ToString();
                    ScheduleDay.Date = ScheduleWeek[i].Date.AddDays(7).ToString("yyyy-MM-dd");
                }
                
            }

            int totalDays = 0;
            foreach (Day day in ScheduleWeek)
            {
                foreach(Booking booking in day)
                {

                }
            }

            //TargetListView.TodayScheduleListView.HasUnevenRows = false;
            
            //TargetListView.TodayScheduleListView.HasUnevenRows = true;
            //TargetListView.TodayScheduleListView.HeightRequest = -1;
            //TargetListView.HeightRequest = -1;
            // TODO: Fix scroll alignment
            //int items = 0;
            //foreach (BookingListItemList item in ScheduleWeekItem) foreach (BookingListItem _ in item) items++;
            //var adjust = Device.RuntimePlatform != Device.Android ? 1 : -items + 2;
            //TargetListView.TodayScheduleListView.HeightRequest = (items * TargetListView.TodayScheduleListView.RowHeight) - adjust;
            //object itemmss = TargetListView.TodayScheduleListView.ViewCells;


            
            Console.WriteLine("Successfully got schedule");
        }

        // Format for the lessons
        BookingListItem LessonToItem(Booking lesson)
        {
            return new BookingListItem()
            {
                Text = lesson.Text,
                Description = lesson.Description,
                StartTime = lesson.StartTime,
                EndTime = lesson.EndTime,
                Booking = lesson,
                DayHasLessons = true
            };
        }

    }
}