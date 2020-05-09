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

        GroupSelector selector = new GroupSelector(ApplicationSettings.LinkGroups);

        public SettingsPage()
        {
            InitializeComponent();

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

            Picker ThemePicker = ThemeSelectorPickerSetting.Picker as Picker;

            ThemePicker.Items.Add("System");
            ThemePicker.Items.Add("Light");
            ThemePicker.Items.Add("Dark");

            if (!ApplicationSettings.ForceSetTheme) ThemePicker.SelectedIndex = 0;
            else if (!ApplicationSettings.EnableDarkTheme) ThemePicker.SelectedIndex = 1;
            else ThemePicker.SelectedIndex = 2;

            ThemePicker.SelectedIndexChanged += UpdateDarkThemeSetting;

            ThemePicker.SetDynamicResource(Picker.TextColorProperty, "PrimaryTextColor");

            SubMenuSetting GroupSelectionSetting = new SubMenuSetting(Navigation, selector) { Label = "Group Selection" };
            //GroupSelectionSetting.Tapped += OnGroupSelectionClicked;
            TimeEditTableSection.Add(GroupSelectionSetting);

            selector.SelectedGroupsChanged += SaveSelectedGroups;
        }

        // Event executed when the timeedit link is set
        void SaveLink(object sender, EventArgs args)
        {
            // Get link base
            int pos = LinkSettingEntryCell.Text.LastIndexOf('/');
            if (pos > 0) { ApplicationSettings.LinkBase = LinkSettingEntryCell.Text.Substring(0, pos) + "/"; }
            else { ApplicationSettings.LinkBase = LinkSettingEntryCell.Text; }

            Console.WriteLine("Successfully set link as "+LinkSettingEntryCell.Text);
        }

        // Event executed when schedule items are changed and must be saved.
        void SaveSelectedGroups(object sender, EventArgs args)
        {
            ApplicationSettings.LinkGroups = selector.EnabledGroups;
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
