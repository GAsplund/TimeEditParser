using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TimeEditParser.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FiltersPage : ContentPage
    {
        public FiltersPage()
        {
            InitializeComponent();
            FilteredNamesListView.ItemsSource = ApplicationSettings.FilterNames.ToArray();
            FilteredIDsListView.ItemsSource = ApplicationSettings.FilterIDs.ToArray();
        }

        void OnDeleteName(object sender, EventArgs e)
        {
            var args = (MenuItem)sender;
            List<string> NameList = ApplicationSettings.FilterNames;
            NameList.Remove(args.CommandParameter.ToString());
            ApplicationSettings.FilterNames = NameList;
            FilteredNamesListView.ItemsSource = ApplicationSettings.FilterNames.ToArray();
        }

        void OnDeleteID(object sender, EventArgs e)
        {
            var args = (MenuItem)sender;
            List<string> IDList = ApplicationSettings.FilterIDs;
            IDList.Remove(args.CommandParameter.ToString());
            ApplicationSettings.FilterIDs = IDList;
            FilteredIDsListView.ItemsSource = ApplicationSettings.FilterIDs.ToArray();
        }
    }
}