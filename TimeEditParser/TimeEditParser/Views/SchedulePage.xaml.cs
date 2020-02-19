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
    public partial class SchedulePage : CarouselPage
    {
        ItemsViewModel viewModel;

        public SchedulePage()
        {
            InitializeComponent();
            BindingContext = viewModel = new ItemsViewModel();
        }

        // Event processed when a lesson is selected
        async void OnItemSelected(ListView sender, SelectedItemChangedEventArgs args)
        {
            //Get the booking to pass to the ItemDetailPage
            //var index = (sender.ItemsSource as List<Item>).IndexOf(sender.SelectedItem as Item);
            ((ListView)sender).SelectedItem = null;

            Item item = args.SelectedItem as Item;
            if (item == null)
                return;

            if (item.Text == "(No lessons for this day)")
                return;

            await Navigation.PushModalAsync(new NavigationPage(new ItemDetailPage(new ItemDetailViewModel(item))));

            // Manually deselect item
            TodayScheduleListView.SelectedItem = null;
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
            ListView TargetListView = TodayScheduleListView;
            //bool isCurrentWeek = false;

            try
            {
                // Get the schedule for the whole week, and save it in the cache
                dynamic scheduleWeeks = await Task.Run(() => ScheduleParser.GetSchedule());

                Application.Current.Properties["scheduleWeeksCache"] = scheduleWeeks;

                if (scheduleWeeks is bool) // Only bool if GetSchedule has returned false
                {
                    Console.WriteLine("Failed to retreive and parse schedule.");
                    TodayScheduleListView.IsRefreshing = false;
                    TomorrowScheduleListView.IsRefreshing = false;
                    await DisplayAlert("Error", "Could not fetch schedule website properly. Have you set the link in settings?", "Ok");
                    return;
                }
                foreach (int weekIndex in Enumerable.Range(1, scheduleWeeks.Count))
                {
                    switch (weekIndex)
                    {
                        case 1:
                            schedule = scheduleWeeks[0];
                            //isCurrentWeek = true;
                            SetScheduleWeek(schedule, TodayScheduleListView, true);
                            if (ScheduleParser.TodayIndex() - 1 <= schedule.Count) Notification.SetNotificationsForDay(schedule[ScheduleParser.TodayIndex() - 1]);
                            break;
                        case 2:
                            schedule = scheduleWeeks[1];
                            SetScheduleWeek(schedule, TomorrowScheduleListView, false);
                            break;
                    }
                    // Update the schedules for today and next day in the listViews
                    //await SetSchedule(weekSchedule[ScheduleParser.TodayIndex() - 1], TodayScheduleListView, "This week");
                    //TodayLabel.Text = "Today - " + weekSchedule[ScheduleParser.TodayIndex() - 1].Count + " lessons";
                    //await SetSchedule(weekSchedule[ScheduleParser.TodayIndex()], TomorrowScheduleListView, "Next Week");
                    //TomorrowLabel.Text = "Tomorrow - " + weekSchedule[ScheduleParser.TodayIndex()].Count + " lessons";
                }
            }
            catch (Exception e)
            {
                if (!(e is ArgumentOutOfRangeException))
                {
                    Console.WriteLine(e.Message + "\n" + e.StackTrace);
                    TodayScheduleListView.IsRefreshing = false;
                    TomorrowScheduleListView.IsRefreshing = false;
                    await DisplayAlert("Error", "Could not read schedule data. (" + e.Message + ")", "Ok");
                }
                return;
            }

        }


        // Set the items for the target listView
        private void SetScheduleWeek(Week ScheduleWeek, ListView TargetListView, bool isCurrentWeek)
        {
            // Create a list of each lesson
            List<ItemList> ScheduleWeekItem = new List<ItemList>();
            foreach (int i in Enumerable.Range(0, ScheduleWeek.Count))
            {
                ItemList ScheduleDay = new ItemList();
                foreach (Booking lessson in ScheduleWeek[i])
                {
                    ScheduleDay.Add(LessonToItem(lessson));
                }

                if (ScheduleWeek[i].Count == 0)
                {
                    ScheduleDay.Add(new Item { Text = "(No lessons for this day)", DayHasLessons = false });
                }

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
                            ScheduleDay.Heading = DateTime.Today.AddDays(ScheduleWeek.Count - ScheduleParser.TodayIndex() + i).ToShortDateString();
                            break;
                    }
                }
                else
                {
                    ScheduleDay.Heading = DateTime.Today.AddDays(ScheduleWeek.Count - ScheduleParser.TodayIndex() + 7 + i).ToShortDateString();
                }
            }


            TargetListView.ItemsSource = ScheduleWeekItem;

            TargetListView.IsRefreshing = false;
            Console.WriteLine("Successfully got schedule");
        }

        // Format for the lessons
        Item LessonToItem(Booking lesson)
        {
            return new Item()
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