using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using TimeEditParser.CustomObjects;
using TimeEditParser.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ColoredTableView), typeof(ColoredTableViewRenderer))]
namespace TimeEditParser.Droid.Renderers
{
    public class ColoredTableViewRenderer : TableViewRenderer
    {
        public ColoredTableViewRenderer(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<TableView> e)
        {
            base.OnElementChanged(e);
            if (Control == null)
                return;

            var listView = Control as Android.Widget.ListView;
            var coloredTableView = (ColoredTableView)Element;
            listView.Divider = new ColorDrawable(coloredTableView.SeparatorColor.ToAndroid());
            listView.DividerHeight = 3;
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == "SeparatorColor")
            {
                var listView = Control as Android.Widget.ListView;
                var coloredTableView = (ColoredTableView)Element;
                listView.Divider = new ColorDrawable(coloredTableView.SeparatorColor.ToAndroid());
                listView.DividerHeight = 3;
            }
        }
    }
}