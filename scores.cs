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
    [Activity(Label = "scores", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class scores : Activity
    {
        Button backScores;
        ISharedPreferences sp;
        TextView player1, player2, player3, player1score, player2score, player3score;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.scores);

            sp = this.GetSharedPreferences("scores", FileCreationMode.Private);

            backScores = FindViewById<Button>(Resource.Id.btnbackscores);
            player1 = FindViewById<TextView>(Resource.Id.TVplayer1);
            player2 = FindViewById<TextView>(Resource.Id.TVplayer2);
            player3 = FindViewById<TextView>(Resource.Id.TVplayer3);
            player1score = FindViewById<TextView>(Resource.Id.TVplayer1score);
            player2score = FindViewById<TextView>(Resource.Id.TVplayer2score);
            player3score = FindViewById<TextView>(Resource.Id.TVplayer3score);

            putScores();

            backScores.Click += BackScores_Click;
        }

        //show the players and their scores from the file
        private void putScores()
        {
            player1.Text = sp.GetString("playerFirst", "---");
            player1score.Text = sp.GetInt("scoreFirst", 0).ToString();

            player2.Text = sp.GetString("playerSecond", "---");
            player2score.Text = sp.GetInt("scoreSecond", 0).ToString();

            player3.Text = sp.GetString("playerThird", "---");
            player3score.Text = sp.GetInt("scoreThird", 0).ToString();
        }

        private void BackScores_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);
        }
    }
}