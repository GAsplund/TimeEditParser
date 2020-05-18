using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using TimeEditParser.Models;
using TimeEditParser.ViewModels;
using System.Globalization;
using System.Diagnostics;

namespace TimeEditParser.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SchedulePage : ContentPage
    {
        ItemsViewModel viewModel;
        ScheduleWeekView scheduleList = new ScheduleWeekView();

        public SchedulePage()
        {
            InitializeComponent();
            BindingContext = viewModel = new ItemsViewModel();
            scheduleList.ScheduleListView.Refreshing += UpdateSchedule;
            scheduleList.ScheduleListView.IsPullToRefreshEnabled = true;
            ScheduleContentView.Content = scheduleList;
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
            List<Week> scheduleWeeks;

            // Attempt to get the schedule for the whole week, and save it in the cache
            try
            {   
                scheduleWeeks = await Task.Run(() => ScheduleParser.GetSchedule());
            }
            catch
            {
                Console.WriteLine("Failed to retreive and parse schedule.");

                scheduleList.ScheduleListView.IsRefreshing = false;
                await DisplayAlert("Error", "Could not fetch schedule properly. Have you set the correct link in settings?", "Ok");
                return;
            }

            ScheduleWeekListItem ScheduleWeekItem = new ScheduleWeekListItem();

            // Attempt to read data and display it
            try
            {
                Application.Current.Properties["scheduleWeeksCache"] = scheduleWeeks;

                foreach (int weekIndex in Enumerable.Range(1, scheduleWeeks.Count))
                {
                    if (weekIndex == 1) {
                        schedule = scheduleWeeks[0];
                        AddScheduleWeek(schedule, true, ScheduleWeekItem);
                        if (ScheduleParser.TodayIndex() <= schedule.Count) Notification.SetNotificationsForDay(schedule[ScheduleParser.TodayIndex() - 1]);
                    }
                    else {
                        schedule = scheduleWeeks[weekIndex - 1];
                        AddScheduleWeek(schedule, false, ScheduleWeekItem);
                    }
                }
                // Set the ItemsSource of the ListView with the schedule items if it changed
                if(ScheduleWeekItem != scheduleList.ScheduleListView.ItemsSource)
                    scheduleList.ScheduleListView.ItemsSource = ScheduleWeekItem;
            }
            catch (Exception e)
            {
                if (!(e is ArgumentOutOfRangeException))
                {
                    Console.WriteLine(e.Message + "\n" + e.StackTrace);
                    scheduleList.ScheduleListView.IsRefreshing = false;
                    await DisplayAlert("Error", "Could not parse schedule data.\nMessage: " + e.Message, "Ok");
                }
                return;
            }

            // Finished updating schedule
            scheduleList.ScheduleListView.IsRefreshing = false;
            Debug.WriteLine("Schedule was successfully updated");
        }


        // Set the items for the target listView
        private void AddScheduleWeek(Week ScheduleWeek, bool isCurrentWeek, ScheduleWeekListItem ScheduleWeekItem)
        {
            // Create a list of each lesson
            
            int week = CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(ScheduleWeek.First().Date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            ScheduleWeekItem.Add(new BookingListItemList { IsWeekHeader = true, Date = "Week " + week});

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
                        case 1:
                            ScheduleDay.Heading = "Today";
                            break;
                        case 0:
                            ScheduleDay.Heading = "Yesterday";
                            break;
                        case 2:
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
                    ScheduleDay.Heading = ScheduleWeek[i].Date.DayOfWeek.ToString();
                    ScheduleDay.Date = ScheduleWeek[i].Date.ToString("yyyy-MM-dd");
                }
                
            }
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