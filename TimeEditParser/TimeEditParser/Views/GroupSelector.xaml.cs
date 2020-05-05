using System;
using System.Collections.Generic;
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
        Dictionary<string, string> enabledGroups = ApplicationSettings.LinkGroups;
        FilterSelector selector;

        public GroupSelector()
        {
            InitializeComponent();
            selector = new FilterSelector(Utilities.ScheduleSearch.GetFilters(ApplicationSettings.LinkBase), enabledGroups);
            selector.enabledGroupsChanged += EnabledGroupsChanged;
            UpdateListViewFilters();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new NavigationPage(selector));
        }

        private void EnabledGroupsChanged(object sender, EventArgs args)
        {
            enabledGroups = selector.enabledGroups;
            UpdateListViewFilters();
        }

        private void UpdateListViewFilters()
        {
            List<CheckedListItem> items = new List<CheckedListItem>();
            foreach (string enabledGroup in enabledGroups.Keys)
                items.Add(new CheckedListItem { IsChecked = true, Title = enabledGroup });
            GroupsList.ItemsSource = null;
            GroupsList.ItemsSource = items;
            ApplicationSettings.LinkGroups = enabledGroups/*string.Join(",-1,", enabledGroups.Values)*/;
        }

    }
}