﻿using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace TimeEditParser.Views
{
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
            TimeEditTableSection.Add( new ViewModels.PickerCell() { Picker = ScheduleGroupPicker, Label = "Schedule group" });

            UseDarkThemeSwitchCell.On = ApplicationSettings.EnableDarkTheme;
            ForceSetThemeSwitchCell.On = ApplicationSettings.ForceSetTheme;

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

        // Event executed when "Send notifications before" is toggled
        void ToggleNotifBefore(object sender, EventArgs args)
        {
            var cell = sender as SwitchCell;
            ApplicationSettings.SendNotificationBefore = cell.On;
        }

        // Event executed when "Send notifications after" is toggled
        void ToggleNotifAfter(object sender, EventArgs args)
        {
            var cell = sender as SwitchCell;
            ApplicationSettings.SendNotificationAfter = cell.On;
        }
        // Event executed when "Send notifications at start" is toggled
        void ToggleNotifAtStart(object sender, EventArgs args)
        {
            var cell = sender as SwitchCell;
            ApplicationSettings.SendNotificationAtStart = cell.On;
        }
        // Event executed when "Send notifications at end" is toggled
        void ToggleNotifAtEnd(object sender, EventArgs args)
        {
            var cell = sender as SwitchCell;
            ApplicationSettings.SendNotificationAtEnd = cell.On;
        }

        void ToggleDarkTheme(object sender, EventArgs args)
        {
            if (ForceSetThemeSwitchCell.On) return;
            var cell = sender as SwitchCell;
            ApplicationSettings.EnableDarkTheme = cell.On;
            switch (cell.On)
            {
                case true:
                    Utilities.Theming.SetTheme();
                    break;
                case false:
                    Utilities.Theming.SetTheme();
                    break;
            }
        }

        void ToggleForceSetTheme(object sender, EventArgs args)
        {
            var cell = sender as SwitchCell;
            ApplicationSettings.ForceSetTheme = cell.On;
            switch (cell.On)
            {
                case true:
                    Utilities.Theming.SetTheme();
                    break;
                case false:
                    ToggleDarkTheme(UseDarkThemeSwitchCell, EventArgs.Empty);
                    break;
            }
        }

    }
}
