using Java.Util;
using System;
using System.Collections.Generic;
using TimeEditParser.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TimeEditParser.Views
{
    public partial class MainPage : MasterDetailPage
    {
        public MainPage()
        {
            InitializeComponent();
            masterPage.listView.ItemSelected += OnItemSelected;
            if (ApplicationSettings.FirstTimeUse)
            {
                Navigation.PushModalAsync(new NavigationPage(new WelcomePage()));
            }
            activePages.Add(typeof(SchedulePage), (NavigationPage)Detail);
        }

        private Dictionary<Type, NavigationPage> activePages = new Dictionary<Type, NavigationPage>();

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MasterPageItem;
            if (item != null)
            {
                if (!activePages.ContainsKey(item.TargetType)) 
                    activePages[item.TargetType] = new NavigationPage((Page)Activator.CreateInstance(item.TargetType));

                Detail = activePages[item.TargetType];
                masterPage.listView.SelectedItem = null;
                IsPresented = false;
            }
        }
    }    
}
