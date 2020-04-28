using System;
using System.Collections.Generic;
using TimeEditParser.SettingCells;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TimeEditParser.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {

        public SettingsPage()
        {
            InitializeComponent();

            dynamic linkbase = ApplicationSettings.LinkBase;
            if(!(linkbase == ""))
            {
                dynamic groups = Utilities.ScheduleSearch.GetGroups(ApplicationSettings.LinkBase, false);
                if(!(groups is bool))
                {
                    foreach (string group in groups.Keys)
                    {
                        ScheduleGroupPicker.Items.Add(group);
                    }
                }

                ScheduleGroupPicker.SelectedIndex = ScheduleGroupPicker.Items.IndexOf(ApplicationSettings.GroupName);
            }

            // Set the values of the settings to the saved values
            LinkSettingEntryCell.Text = ApplicationSettings.LinkBase;

            // Start notifications
            ToggleNotifAtStartSwitchCell.On = ApplicationSettings.SendNotificationAtStart; // At start
            ToggleNotifBeforeSwitchCell.On = ApplicationSettings.SendNotificationBefore; // Before start
            SaveTimeBeforeEntryCell.Text = ApplicationSettings.MinutesBeforeNotification.ToString(); // Time before start

            // End notifications
            ToggleNotifAfterSwitchCell.On = ApplicationSettings.SendNotificationAfter; // Before end
            ToggleNotifAtEndSwitchCell.On = ApplicationSettings.SendNotificationAtEnd; // At end
            SaveTimeAfterEntryCell.Text = ApplicationSettings.MinutesAfterNotification.ToString(); // Time before end

            // Attach event for index changed
            ScheduleGroupPicker.SelectedIndexChanged += UpdateScheduleGroup;
            TimeEditTableSection.Add( new PickerSetting() { Picker = ScheduleGroupPicker, Label = "Schedule group" });

            Picker ThemePicker = ThemeSelectorPickerSetting.Picker as Picker;

            ThemePicker.Items.Add("System");
            ThemePicker.Items.Add("Light");
            ThemePicker.Items.Add("Dark");

            if (!ApplicationSettings.ForceSetTheme) ThemePicker.SelectedIndex = 0;
            else if (!ApplicationSettings.EnableDarkTheme) ThemePicker.SelectedIndex = 1;
            else ThemePicker.SelectedIndex = 2;

            ThemePicker.SelectedIndexChanged += UpdateDarkThemeSetting;

            ThemePicker.SetDynamicResource(Picker.TextColorProperty, "PrimaryTextColor");
            ScheduleGroupPicker.SetDynamicResource(Picker.TextColorProperty, "PrimaryTextColor");
        }

        Picker ScheduleGroupPicker = new Picker() { HorizontalOptions = LayoutOptions.StartAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand, Title = "Pick a group", WidthRequest = 1000 };

        // Event executed when Picker item has been selected
        void UpdateScheduleGroup(object sender, EventArgs args)
        {
            var picker = sender as Picker;
            ApplicationSettings.GroupID = Utilities.ScheduleSearch.GetGroups(ApplicationSettings.LinkBase)[picker.SelectedItem.ToString()];
            ApplicationSettings.GroupName = picker.SelectedItem.ToString();
        }

        // Event executed when the timeedit link is set
        void SaveLink(object sender, EventArgs args)
        {
            // Get link base
            int pos = LinkSettingEntryCell.Text.LastIndexOf('/');
            if (pos > 0) { ApplicationSettings.LinkBase = LinkSettingEntryCell.Text.Substring(0, pos) + "/"; }
            else { ApplicationSettings.LinkBase = LinkSettingEntryCell.Text; }

            // Add items to the picker
            foreach(string group in Utilities.ScheduleSearch.GetGroups(ApplicationSettings.LinkBase, false).Keys)
            {
                ScheduleGroupPicker.Items.Add(group);
            }

            Console.WriteLine("Successfully set link as "+LinkSettingEntryCell.Text);
        }

        // Event executed when "Send notifications before" is toggled
        void ToggleNotifBefore(object sender, EventArgs args)
        {
            var cell = sender as Switch;
            ApplicationSettings.SendNotificationBefore = cell.IsToggled;
        }

        // Event executed when "Send notifications after" is toggled
        void ToggleNotifAfter(object sender, EventArgs args)
        {
            var cell = sender as Switch;
            ApplicationSettings.SendNotificationAfter = cell.IsToggled;
        }
        // Event executed when "Send notifications at start" is toggled
        void ToggleNotifAtStart(object sender, EventArgs args)
        {
            var cell = sender as Switch;
            ApplicationSettings.SendNotificationAtStart = cell.IsToggled;
        }
        // Event executed when "Send notifications at end" is toggled
        void ToggleNotifAtEnd(object sender, EventArgs args)
        {
            var cell = sender as Switch;
            ApplicationSettings.SendNotificationAtEnd = cell.IsToggled;
        }

        void SaveTimeBefore(object sender, EventArgs args)
        {
            var input = sender as EntryCell;
            ApplicationSettings.MinutesBeforeNotification = Convert.ToInt32(input.Text);
        }

        void SaveTimeAfter(object sender, EventArgs args)
        {
            var input = sender as EntryCell;
            ApplicationSettings.MinutesAfterNotification = Convert.ToInt32(input.Text);
        }

        void OnGroupSelectionClicked(object sender, EventArgs args)
        {
            Navigation.PushModalAsync(new NavigationPage(new GroupSelector()));
        }

            void UpdateDarkThemeSetting(object sender, EventArgs args)
        {
            Picker picker = sender as Picker;
            switch (picker.SelectedIndex)
            {
                case 0:
                    ApplicationSettings.ForceSetTheme = false;
                    break;
                case 1:
                    ApplicationSettings.ForceSetTheme = true;
                    ApplicationSettings.EnableDarkTheme = false;
                    break;
                case 2:
                    ApplicationSettings.ForceSetTheme = true;
                    ApplicationSettings.EnableDarkTheme = true;
                    break;
            }
            Utilities.Theming.SetTheme();
        }
    }
}
