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
    public partial class GroupSelector : ContentPage
    {
        public GroupSelector()
        {
            InitializeComponent();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new NavigationPage(new FilterSelector(Utilities.ScheduleSearch.GetFilters(ApplicationSettings.LinkBase))));
        }
    }
}