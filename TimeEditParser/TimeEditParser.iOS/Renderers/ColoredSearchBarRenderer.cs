using TimeEditParser.CustomObjects;
using TimeEditParser.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ColoredSearchBar), typeof(ColoredSearchBarRenderer))]
namespace TimeEditParser.iOS.Renderers
{
    public class ColoredSearchBarRenderer : SearchBarRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.SearchBar> e)
        {
            base.OnElementChanged(e);
            /*var newElement = ((ColoredSearchBar)e.NewElement);
            BorderColor = newElement.BorderColor.ToUIColor();
            if (newElement.BorderWidth != 0)
            {
                BorderWidth = newElement.BorderWidth;
            }
            var searchbar = (UISearchBar)Control;
            if (e.NewElement != null)
            {
                //Foundation.NSString _searchField = new Foundation.NSString(“searchField”);
                //var textFieldInsideSearchBar = (UITextField)searchbar.ValueForKey(_searchField);
                //textFieldInsideSearchBar.BackgroundColor = UIColor.FromRGB(0, 0, 12);
                //textFieldInsideSearchBar.TextColor = UIColor.White;
                // searchbar.Layer.BackgroundColor = UIColor.Blue.CGColor;
                //searchbar.TintColor = UIColor.White;
                //searchbar.BarTintColor = UIColor.White;
                searchbar.Layer.CornerRadius = 0;
                searchbar.Layer.BorderWidth = BorderWidth;
                searchbar.Layer.BorderColor = BorderColor.CGColor;
                //searchbar.ShowsCancelButton = false;
            }*/
        }
    }
}