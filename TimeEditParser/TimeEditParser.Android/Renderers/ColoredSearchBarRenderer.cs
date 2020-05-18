using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Runtime;
using Android.Widget;
using System;
using System.ComponentModel;
using TimeEditParser.CustomObjects;
using TimeEditParser.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ColoredSearchBar), typeof(ColoredSearchBarRenderer))]
namespace TimeEditParser.Droid.Renderers
{
    public class ColoredSearchBarRenderer : SearchBarRenderer
    {

        public ColoredSearchBarRenderer(Context context) : base(context) { }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            var newElement = ((ColoredSearchBar)sender);
            var searchView = (Control as SearchView);

            int searchIconId = Context.Resources.GetIdentifier("android:id/search_mag_icon", null, null);
            if (searchIconId > 0)
            {
                var searchPlateIcon = searchView.FindViewById(searchIconId);
                (searchPlateIcon as ImageView).SetColorFilter(newElement.IconColor.ToAndroid(), PorterDuff.Mode.SrcIn);
            }
        }
    }
}