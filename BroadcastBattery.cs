using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace maya_simon
{
    [BroadcastReceiver(Enabled = true)]
    [IntentFilter(new[] { Intent.ActionBatteryChanged })]
    public class BroadcastBattery : BroadcastReceiver
    {
        private TextView battery;

        public BroadcastBattery()
        {}

        public BroadcastBattery(TextView tv)
        {
            this.battery = tv;
        }

        //finds the phone battery precent
        public override void OnReceive(Context context, Intent intent)
        {
            int b = intent.GetIntExtra("level", 0);
            this.battery.Text = "" + b + "%";
        }
    }
}