using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using System;
using Android.Content;


// https://www.youtube.com/watch?v=1Yqj76Q4jJ4
// video rules



namespace maya_simon
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class MainActivity : AppCompatActivity
    {
        private Button level1, level2, rules, scores;
        private TextView battery;
        private BroadcastBattery bcr;
        private Button playOrPause;
        private bool isPlaying;
        private Intent music;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.menu);

            level1 = FindViewById<Button>(Resource.Id.btnLevel1);
            level2 = FindViewById<Button>(Resource.Id.btnLevel2);
            rules = FindViewById<Button>(Resource.Id.btnRules);
            scores = FindViewById<Button>(Resource.Id.btnScores);
            battery = FindViewById<TextView>(Resource.Id.tvBattery);
            playOrPause = FindViewById<Button>(Resource.Id.btnMusic);
            bcr = new BroadcastBattery(battery);
            isPlaying = false;

            rules.Click += Rules_Click;
            scores.Click += Scores_Click;
            level1.Click += choose_level;
            level2.Click += choose_level;
            playOrPause.Click += Music_Click;
        }

        //stop or start music
        private void Music_Click(object sender, EventArgs e)
        {
            if (sender == playOrPause)
            {
                if (!isPlaying)
                {
                    music = new Intent(this, typeof(MusicService));
                    StartService(music);
                    playOrPause.SetBackgroundResource(Resource.Drawable.pause);
                }
                else
                {
                    music = new Intent(this, typeof(MusicService));
                    //MusicService.SetDestroy(true);
                    StopService(music);
                    playOrPause.SetBackgroundResource(Resource.Drawable.play);
                }
                isPlaying = !isPlaying;
            }
        }

        public override bool OnCreateOptionsMenu(Android.Views.IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.first_menu, menu);
            return true;
        }

        //show battery or not
        public override bool OnOptionsItemSelected(Android.Views.IMenuItem item)
        {
            if (item.ItemId == Resource.Id.show)
            {
                battery.Visibility = Android.Views.ViewStates.Visible;
                return true;
            }
            else if (item.ItemId == Resource.Id.hide)
            {
                battery.Visibility = Android.Views.ViewStates.Invisible;
                return true;
            }
            return base.OnOptionsItemSelected(item);
        }

        protected override void OnResume()
        {
            base.OnResume();
            RegisterReceiver(bcr, new IntentFilter(Intent.ActionBatteryChanged));
        }
        protected override void OnPause()
        {
            base.OnPause();
            UnregisterReceiver(bcr);
        }

        /*
         * if the player clicked on the level1 button it will send him to level 1
         * else it will send him to level 2 - crazy level
         */
        private void choose_level(object sender, EventArgs e)
        {
            music = new Intent(this, typeof(MusicService));
            StopService(music);
            if (sender == level1)
            {
                Intent intent = new Intent(this, typeof(levelNumberOne));
                StartActivity(intent);
            }
            else
            {
                Intent intent = new Intent(this, typeof(crazyLevelRules));
                StartActivity(intent);
            }
        }

        private void Scores_Click(object sender, System.EventArgs e)
        {
            music = new Intent(this, typeof(MusicService));
            StopService(music);
            Intent intent = new Intent(this, typeof(scores));
            StartActivity(intent);
        }

        private void Rules_Click(object sender, System.EventArgs e)
        {
            music = new Intent(this, typeof(MusicService));
            StopService(music);
            Intent intent = new Intent(this, typeof(rulesActivity));
            StartActivity(intent);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}