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

namespace maya_simon
{
    [Activity(Label = "rules", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class rulesActivity : Activity
    {
        private Button rulesBack, youtube;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.rules);
            rulesBack = FindViewById<Button>(Resource.Id.btnbackRules);
            youtube = FindViewById<Button>(Resource.Id.btnYoutube);

            youtube.Click += Youtube_Click;
            rulesBack.Click += RulesBack_Click;
        }

        //sents to youtube to watch the simon instructions
        private void Youtube_Click(object sender, EventArgs e)
        {
            string videoId = "1Yqj76Q4jJ4";
            Intent intent = new Intent();
            intent.SetAction(Intent.ActionView);
            Android.Net.Uri data = Android.Net.Uri.Parse("vnd.youtube://" + videoId);
            intent.SetData(data);
            StartActivity(intent);
        }

        private void RulesBack_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);
        }
    }
}