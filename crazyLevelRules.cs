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
    [Activity(Label = "crazyLevelRules", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class crazyLevelRules : Activity
    {
        private Button back_to_menu, play_game;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.crazyLevelRules);
            back_to_menu = FindViewById<Button>(Resource.Id.btnCrazyRulesBack);
            play_game = FindViewById<Button>(Resource.Id.btnPlayGame);

            play_game.Click += Play_game_Click;
            back_to_menu.Click += Back_to_menu_Click;
        }

        //goes to the crazy level screen
        private void Play_game_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(crazyLevelActivity));
            StartActivity(intent);
        }

        //goes back to the main menu
        private void Back_to_menu_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);
        }
    }
}