using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeEditParser.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TimeEditParser.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GroupSelector : ContentPage
    {
        public event EventHandler<EventArgs> SelectedGroupsChanged;
        public Dictionary<string, string> EnabledGroups;
        FilterSelector selector;

        public GroupSelector(Dictionary<string, string> preenabled = null)
        {
            InitializeComponent();
            EnabledGroups = preenabled ?? new Dictionary<string, string>();
            ToolbarItems.Add(new ToolbarItem
            {
                Text = "Done",
                Command = new Command(() => Navigation.PopModalAsync()),
            });
            selector = new FilterSelector(Utilities.ScheduleSearch.GetFilters(ApplicationSettings.LinkBase), EnabledGroups);
            selector.EnabledGroupsChanged += EnabledGroupsChanged;
            GroupsList.ItemSelected += GroupItemSelected;
            UpdateListViewFilters();
        }

        private void SelectFilters(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new NavigationPage(selector) { Title = "Filter Selection" });
        }

        private void EnabledGroupsChanged(object sender, EventArgs args)
        {
            EnabledGroups = selector.enabledGroups;
            UpdateListViewFilters();
        }

        private void GroupItemSelected(object sender, EventArgs args)
        {
            if (GroupsList.SelectedItem == null) return;
            CheckedListItem selected = (CheckedListItem)GroupsList.SelectedItem;
            ObservableCollection<CheckedListItem> items = (ObservableCollection<CheckedListItem>)GroupsList.ItemsSource;
            items.Remove((CheckedListItem)GroupsList.SelectedItem);
            EnabledGroups.Remove(selected.Title);
            GroupsList.SelectedItem = null;
            //GroupsList.ItemsSource = null;
            GroupsList.ItemsSource = items;
            SelectedGroupsChanged?.Invoke(EnabledGroups, null);
        }

        private void UpdateListViewFilters()
        {
            ObservableCollection<CheckedListItem> items = new ObservableCollection<CheckedListItem>();
            foreach (string enabledGroup in EnabledGroups.Keys)
                items.Add(new CheckedListItem { IsChecked = true, Title = enabledGroup });
            GroupsList.ItemsSource = null;
            GroupsList.ItemsSource = items;
            SelectedGroupsChanged?.Invoke(EnabledGroups, null);
        }

    }
}