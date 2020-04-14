using System;
using Xamarin.Forms;

namespace TimeEditParser.Views
{
    class NonScrollableListView : ListView
    {
        public NonScrollableListView()
            : base(ListViewCachingStrategy.RecycleElement)
        {
            if (Device.RuntimePlatform == Device.UWP)
                BackgroundColor = Color.White;
        }
    }
}
