using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using TimeEditParser.Models;
using TimeEditParser.ViewModels;
using System.Collections.Generic;

namespace TimeEditParser.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        ItemDetailViewModel viewModel;

        public ItemDetailPage(ItemDetailViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }

        async void RemoveLesson(object sender, EventArgs args)
        {
            List<string> LessonIDRemoveFilter = ApplicationSettings.FilterIDs;
            LessonIDRemoveFilter.Add(viewModel.Item.Booking.Id);
            ApplicationSettings.FilterIDs = LessonIDRemoveFilter;
            await App.Current.SavePropertiesAsync();
            await App.Current.MainPage.Navigation.PopModalAsync();
        }

        async void RemoveLessonName(object sender, EventArgs args)
        {
            List<string> LessonNameRemoveFilter = ApplicationSettings.FilterNames;
            LessonNameRemoveFilter.Add(viewModel.Item.Booking.name);
            ApplicationSettings.FilterNames = LessonNameRemoveFilter;
            await App.Current.SavePropertiesAsync();
            await App.Current.MainPage.Navigation.PopModalAsync();
        }

        public ItemDetailPage()
        {
            InitializeComponent();

            var item = new Item
            {
                Text = "Item 1",
                Description = "This is an item description."
            };

            viewModel = new ItemDetailViewModel(item);
            BindingContext = viewModel;
        }
    }
}