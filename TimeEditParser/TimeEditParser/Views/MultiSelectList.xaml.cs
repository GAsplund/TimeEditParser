﻿using System;
using System.Collections;
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
    public partial class MultiSelectList : ContentPage
    {
        public event EventHandler OnCheckedChanged;
        public MultiSelectList()
        {
            InitializeComponent();
            ToolbarItems.Add(new ToolbarItem
            {
                Text = "Done",
                Command = new Command(() => Navigation.PopModalAsync()),
            });
            ItemsList.ItemSelected += OnItemSelected;
        }

        public HashSet<string> checkedItems = new HashSet<string>();

        public void SearchArgChanged(object sender, EventArgs args)
        {
            FilterSearchResults();
        }

        private void FilterSearchResults()
        {
            if (string.IsNullOrEmpty(SearchBar.Text)) ItemsList.ItemsSource = itemsSource;
            else ItemsList.ItemsSource = itemsSource.Where(c => c.Title.StartsWith(SearchBar.Text));
        }

        private List<CheckedListItem> itemsSource = new List<CheckedListItem>();

        public List<CheckedListItem> ItemsSource 
        {
            get => itemsSource; set { itemsSource = value; ItemsList.ItemsSource = value; FilterSearchResults(); }
        }

        public void OnItemSelected(object sender, EventArgs args)
        {
            // TODO: Lessen amount of casts needed
            if ((CheckedListItem)ItemsList.SelectedItem == null) return;
            ((CheckedListItem)ItemsList.SelectedItem).IsChecked = !((CheckedListItem)ItemsList.SelectedItem).IsChecked;
            if (((CheckedListItem)ItemsList.SelectedItem).IsChecked) checkedItems.Add(((CheckedListItem)ItemsList.SelectedItem).Title);
            else checkedItems.Remove(((CheckedListItem)ItemsList.SelectedItem).Title);
            

            OnCheckedChanged?.Invoke((CheckedListItem)ItemsList.SelectedItem, EventArgs.Empty);

            ItemsList.SelectedItem = null;
        }

    }
}