using Android.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeEditParser.Objects;
using TimeEditParser.SettingCells;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TimeEditParser.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FilterSelector : ContentPage
    {
        List<FilterCategory> categories;
        public FilterSelector(List<FilterCategory> categories)
        {
            InitializeComponent();

            TableSection typeSelectionSection = new TableSection()
            {
                Title = "Filter 1"
            };
            Picker groupPicker = new Picker();
            groupPicker.SelectedIndexChanged += pickerIndexChanged;
            PickerSetting groupPickerSetting = new PickerSetting() { Picker = groupPicker };
            groupPickerSetting.Label = "Type";
            typeSelectionSection.Add(groupPickerSetting);
            FiltersTableView.Root.Add(typeSelectionSection);

            FiltersTableView.Root.Add(new TableSection() { Title = "Filter Settings" });
            FiltersTableView.Root.Add(new TableSection() { Title = "Filter Results" });

            foreach (FilterCategory category in categories) 
            {
                groupPicker.Items.Add(category.Name);
            }
            this.categories = categories;
        }

        public void pickerIndexChanged(object sender, EventArgs args)
        {
            Picker picker = sender as Picker;
            string selectedCategory = picker.Items[picker.SelectedIndex];

            if(FiltersTableView.Root.Count <= 1) { Log.Wtf("", "FiltersTableView has count of less than 2."); return; }
            TableSection section = FiltersTableView.Root[1];
            section.Clear();

            List<FilterCategory> selectedFilters = (from category in categories where category.Name == selectedCategory select category).ToList();
            if(selectedFilters.Count != 1) { Log.Wtf("", "SelectedFilters should only have a count of 1"); return; }
            switch (selectedFilters.First())
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
                    }
                    
                    break;
            }
        }
    }
}