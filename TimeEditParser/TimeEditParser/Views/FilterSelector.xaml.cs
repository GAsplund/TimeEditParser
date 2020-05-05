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
        public event EventHandler<EventArgs> enabledGroupsChanged;
        Dictionary<string, FilterCategory> categories;
        Dictionary<string, Setting> currentSettings = new Dictionary<string, Setting>();
        Dictionary<string, string> searchResults;
        public Dictionary<string, string> enabledGroups;
        Picker picker;
        string dataType = "0";
        public FilterSelector(Dictionary<string, FilterCategory> categories, Dictionary<string, string> enabledGroups)
        {
            InitializeComponent();

            TableSection typeSelectionSection = new TableSection();
            picker = new Picker();
            picker.SelectedIndexChanged += pickerIndexChanged;
            PickerSetting groupPickerSetting = new PickerSetting() { Picker = picker };
            groupPickerSetting.Label = "Type";
            typeSelectionSection.Add(groupPickerSetting);
            FiltersTableView.Root.Add(typeSelectionSection);

            FiltersTableView.Root.Add(new TableSection() { Title = "Filter Settings" });
            FiltersTableView.Root.Add(new TableSection() { /*Title = "Filter Results"*/ });

            ViewCell searchResultsCell = new ViewCell(); Button searchResultsButton = new Button { Text = "View results" };
            searchResultsButton.Clicked += ShowFilteredItems;
            searchResultsCell.View = searchResultsButton;
            FiltersTableView.Root.Last().Add(searchResultsCell);

            foreach (KeyValuePair<string, FilterCategory> category in categories) 
            {
                picker.Items.Add(category.Key);
            }
            this.categories = categories;
            this.enabledGroups = enabledGroups;
        }

        public void pickerIndexChanged(object sender, EventArgs args)
        {
            Picker picker = sender as Picker;
            string selectedCategory = picker.Items[picker.SelectedIndex];
            FilterCategory selectedFilterCategory = categories[selectedCategory];
            dataType = selectedFilterCategory.Value;

            if(FiltersTableView.Root.Count <= 1) { Log.Wtf("", "FiltersTableView has count of less than 2."); return; }
            TableSection section = FiltersTableView.Root[2];
            section.Clear();
            currentSettings.Clear();

            switch (selectedFilterCategory)
            {
                case FilterCategoryDropDown dd:
                    
                    foreach(KeyValuePair<string, List<FilterCategory>> valuePair in dd.Filters)
                    {
                        Picker groupPicker = new Picker();
                        foreach (FilterCategory category in valuePair.Value)
                        {
                            groupPicker.Items.Add(category.Name);
                        }
                        PickerSetting groupPickerSetting = new PickerSetting() { Picker = groupPicker, Label = valuePair.Key };

                        section.Add(groupPickerSetting);
                        currentSettings[valuePair.Key] = groupPickerSetting;
                    }
                    
                    break;
            }
        }

        public void SearchFilter(object sender, EventArgs args)
        {
            if (picker.SelectedIndex < 0) return;
            string selectedCategory = picker.Items[picker.SelectedIndex];
            //FilterCategory selectedFilter = categories[selectedCategory];

            Dictionary<string, Dictionary<string, List<string>>> items = new Dictionary<string, Dictionary<string, List<string>>>();

            //string dataPrefix;

            foreach(KeyValuePair<string, Setting> valuePair in currentSettings)
            {
                
                switch (valuePair.Value)
                {
                    case PickerSetting ps:
                        Picker picker = (Picker)ps.Picker;
                        if (picker.SelectedIndex < 0) continue;
                        List<FilterCategory> filters = ((FilterCategoryDropDown)categories[selectedCategory]).Filters[ps.Label];
                        FilterCategory filter = filters[picker.SelectedIndex];
                        //dataType = filter.DataType;

                        if (!items.ContainsKey(filter.DataParam)) items.Add(filter.DataParam, new Dictionary<string, List<string>>());
                        if (!items[filter.DataParam].ContainsKey(filter.DataPrefix)) items[filter.DataParam].Add(filter.DataPrefix, new List<string>());

                        items[filter.DataParam][filter.DataPrefix].Add(filter.Value);
                        break;
                }
            }

            searchResults = ScheduleSearch.SearchFilters(dataType, items);
        }

        public void ShowFilteredItems(object sender, EventArgs args)
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
            Navigation.PushModalAsync(new NavigationPage(filteredList));
        }

        public void OnSelectedItemsChanged(object sender, EventArgs args)
        {
            CheckedListItem item = sender as CheckedListItem;
            if(item.IsChecked)
            {
                enabledGroups.Add(item.Title, searchResults[item.Title]);
            }
            else if (enabledGroups.ContainsKey(item.Title)) enabledGroups.Remove(item.Title);
            enabledGroupsChanged?.Invoke(this, null);
        }

    }
}