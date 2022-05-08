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
    class color : sound
    {
        int color_id { set; get; }

        public color(int color_id, string name) : base(name)
        {
            this.color_id = color_id;
        }
        //plays this color's sound
        public void playSound()
        {
            this.player.Start();
        }
    }
}