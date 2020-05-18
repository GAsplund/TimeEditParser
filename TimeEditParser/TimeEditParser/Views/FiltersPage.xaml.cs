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
            FilterTypePicker.Items.Add("Names");
            FilterTypePicker.Items.Add("IDs");
        }

        void FilterTypeIndexChanged(object sender, EventArgs e)
        {
            Picker picker = (Picker)sender;
            switch (picker.SelectedIndex)
            {
                case 0: // Names
                    FiltersListView.ItemsSource = ApplicationSettings.FilterNames.ToArray();
                    break;
                case 1: // IDs
                    FiltersListView.ItemsSource = ApplicationSettings.FilterIDs.ToArray();
                    break;
                default:
                    FiltersListView.ItemsSource = null;
                    break;
            }
        }

        void OnDeleteFilterItem(object sender, EventArgs e)
        {
            var args = (MenuItem)sender;
            switch (FilterTypePicker.SelectedIndex)
            {
                case 0: // Names
                    List<string> IDsList = ApplicationSettings.FilterNames;
                    IDsList.Remove(args.CommandParameter.ToString());
                    ApplicationSettings.FilterNames = IDsList;
                    FiltersListView.ItemsSource = ApplicationSettings.FilterNames.ToArray();
                    break;
                case 1: // IDs
                    List<string> NamesList = ApplicationSettings.FilterIDs;
                    NamesList.Remove(args.CommandParameter.ToString());
                    ApplicationSettings.FilterIDs = NamesList;
                    FiltersListView.ItemsSource = ApplicationSettings.FilterIDs.ToArray();
                    break;
                default:
                    break;
            }

            
        }
    }
}