﻿using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using TimeEditParser.Models;

namespace TimeEditParser.Views
{
    public partial class NewItemPage : ContentPage
    {
        public BookingListItem Item { get; set; }

        public NewItemPage()
        {
            InitializeComponent();

            Item = new BookingListItem
            {
                Text = "Item name",
                Description = "This is an item description."
            };

            BindingContext = this;
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "AddItem", Item);
            await Navigation.PopModalAsync();
        }

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}