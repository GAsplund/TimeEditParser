using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace TimeEditParser.Droid
{
    [BroadcastReceiver]
    class NotifDismissBroadcastReceiver : BroadcastReceiver
    {
        // Day ended notification dismissed
        public override void OnReceive(Context context, Intent intent)
        {
            // Unregister schedule status BroadcastReceiver
            Application.Context.UnregisterReceiver(ScheduleBroadcastReceiver.CurrentInstance);
        }
    }
}