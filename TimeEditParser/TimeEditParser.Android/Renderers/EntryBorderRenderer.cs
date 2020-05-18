using Android.Content;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Graphics;
using TimeEditParser.CustomObjects;
using TimeEditParser.Droid.Renderers;
using Android.Speech.Tts;
using System.ComponentModel;

[assembly: ExportRenderer(typeof(ColoredEntry), typeof(EntryBorderRenderer))]
namespace TimeEditParser.Droid.Renderers
{
    class EntryBorderRenderer : EntryRenderer
    {
        public EntryBorderRenderer(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (Control == null || Element == null || e.OldElement != null) return;

            var element = (ColoredEntry)Element;
            var ourCustomColorHere = element.BorderColor.ToAndroid();
            Control.Background.SetColorFilter(ourCustomColorHere, PorterDuff.Mode.SrcAtop);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (Control == null || Element == null) return;
            if (e.PropertyName == "BorderColor")
            {
                var element = (ColoredEntry)Element;
                var ourCustomColorHere = element.BorderColor.ToAndroid();
                Control.Background.SetColorFilter(ourCustomColorHere, PorterDuff.Mode.SrcAtop);
            }
        }
    }
}
