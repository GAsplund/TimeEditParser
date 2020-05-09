using Android.App;
using Android.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeEditParser.Models;
using TimeEditParser.Objects;
using TimeEditParser.SettingCells;
using TimeEditParser.Utilities;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TimeEditParser.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FilterSelector : ContentPage
    {
        public event EventHandler<EventArgs> EnabledGroupsChanged;
        Dictionary<string, FilterCategory> categories;
        Dictionary<string, Setting> currentSettings = new Dictionary<string, Setting>();
        Dictionary<string, string> searchResults;
        public Dictionary<string, string> enabledGroups;
        Picker filterTypePicker;
        string dataType = "0";

        public FilterSelector(Dictionary<string, FilterCategory> categories, Dictionary<string, string> enabledGroups)
        {
            InitializeComponent();

            ToolbarItems.Add(new ToolbarItem
            {
                Text = "Done",
                Command = new Command(() => Navigation.PopModalAsync()),
            });
            filterTypePicker = new Picker();
            filterTypePicker.SelectedIndexChanged += PickerIndexChanged;
            filterTypePicker.SelectedIndexChanged += SearchFilter;
            FilterTypePickerSetting.Picker = filterTypePicker;

            // <local:SubMenuSetting Label="View Results" Tapped="ShowFilteredItems"/>
            SubMenuSetting searchResultsMenu = new SubMenuSetting(Navigation) { Label = "View Results" }; 
            searchResultsMenu.Tapped += ShowFilteredItems;
            SearchResultsSection.Add(searchResultsMenu);

            foreach (KeyValuePair<string, FilterCategory> category in categories) 
            {
                filterTypePicker.Items.Add(category.Key);
            }
            this.categories = categories;
            this.enabledGroups = enabledGroups;
        }

        public async void PickerIndexChanged(object sender, EventArgs args)
        {
            searchResults?.Clear();
            Picker picker = sender as Picker;
            string selectedCategory = picker.Items[picker.SelectedIndex];
            FilterCategory selectedFilterCategory = categories[selectedCategory];
            dataType = selectedFilterCategory.Value;

            if(FiltersTableView.Root.Count <= 1) { Log.Wtf("", "FiltersTableView has count of less than 2."); return; }
            TableSection section = FiltersTableView.Root[1];
            section.Clear();
            currentSettings.Clear();

            switch (selectedFilterCategory)
            {
                case FilterCategoryDropDown dd:
                    
                    foreach(KeyValuePair<string, List<FilterCategory>> valuePair in dd.Filters)
                    {
                        List<CheckedListItem> items = new List<CheckedListItem>();
                        MultiSelectSetting multiSelectSetting = new MultiSelectSetting(Navigation) { Label = valuePair.Key };
                        MultiSelectList list = multiSelectSetting.SubMenu as MultiSelectList;

                        list.OnCheckedChanged += SearchFilter;
                        foreach (FilterCategory category in valuePair.Value)
                        {
                            items.Add(new CheckedListItem { Title = category.Name, IsChecked = enabledGroups.Keys.Contains(category.Name) });
                        }
                        list.ItemsList.ItemsSource = items;
                        //PickerSetting groupPickerSetting = new PickerSetting() { Picker = groupPicker, Label = valuePair.Key };

                        section.Add(multiSelectSetting);
                        currentSettings[valuePair.Key] = multiSelectSetting;
                    }
                    
                    break;
            }
        }

        async void SearchFilter(object sender, EventArgs args)
        {
            if (filterTypePicker.SelectedIndex < 0) return;
            string selectedCategory = filterTypePicker.Items[filterTypePicker.SelectedIndex];
            //FilterCategory selectedFilter = categories[selectedCategory];

            Dictionary<string, Dictionary<string, List<string>>> items = new Dictionary<string, Dictionary<string, List<string>>>();

            //string dataPrefix;

            foreach(KeyValuePair<string, Setting> valuePair in currentSettings)
            {
                
                switch (valuePair.Value)
                {
                    case MultiSelectSetting ms:
                        HashSet<string> selectedItems = ((MultiSelectList)ms.SubMenu).checkedItems;
                        if (selectedItems.Count <= 0) continue;
                        List<FilterCategory> filters = ((FilterCategoryDropDown)categories[selectedCategory]).Filters[ms.Label];
                        foreach (FilterCategory filter in filters)
                        {
                            if (selectedItems.Contains(filter.Name))
                            {
                                if (!items.ContainsKey(filter.DataParam)) items.Add(filter.DataParam, new Dictionary<string, List<string>>());
                                if (!items[filter.DataParam].ContainsKey(filter.DataPrefix)) items[filter.DataParam].Add(filter.DataPrefix, new List<string>());

                                items[filter.DataParam][filter.DataPrefix].Add(filter.Value);
                            }
                        }
                        break;
                }
            }

            searchResults = ScheduleSearch.SearchFilters(dataType, items);
        }

        async void ShowFilteredItems(object sender, EventArgs args)
        {
            MultiSelectList filteredList = new MultiSelectList();
            filteredList.OnCheckedChanged += OnSelectedItemsChanged;
            if (searchResults == null) filteredList.ItemsList.ItemsSource = new List<string>();
            else
            {
                List<CheckedListItem> items = new List<CheckedListItem>();
                foreach(KeyValuePair<string, string> itemKvp in searchResults) 
                    items.Add(new CheckedListItem { Title = itemKvp.Key, IsChecked = enabledGroups.Values.Contains(itemKvp.Value) });

                filteredList.ItemsList.ItemsSource = items;
            }
            await Navigation.PushModalAsync(new NavigationPage(filteredList) { Title = "Item Selection" });
        }

        async void OnSelectedItemsChanged(object sender, EventArgs args)
        {
            CheckedListItem item = sender as CheckedListItem;
            if(item.IsChecked)
            {
                enabledGroups.Add(item.Title, searchResults[item.Title]);
            }
            else if (enabledGroups.ContainsKey(item.Title)) enabledGroups.Remove(item.Title);
            EnabledGroupsChanged?.Invoke(this, null);
        }

    }
}