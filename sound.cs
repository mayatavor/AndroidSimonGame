using Android.App;
using Android.Content;
using Android.Media;
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
    class sound
    {
        private string name;
        protected MediaPlayer player;

        //sets the sounds for each color
        public sound(string name)
        {
            this.name = name;
            this.player = new MediaPlayer();
            if (this.name == "blue")
                player = MediaPlayer.Create(global::Android.App.Application.Context, Resource.Raw.blue);
            else if (this.name == "red")
                player = MediaPlayer.Create(global::Android.App.Application.Context, Resource.Raw.red);
            else if (this.name == "green")
                player = MediaPlayer.Create(global::Android.App.Application.Context, Resource.Raw.green);
            else
                player = MediaPlayer.Create(global::Android.App.Application.Context, Resource.Raw.yellow);
        }
    }
}