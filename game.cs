using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace maya_simon
{
    class game
    {
        public Queue<int> list;
        private Random rand;
        private color[] colors;
        private int len;

        public game()
        {
            this.list = new Queue<int>();
            this.colors = new color[4];
            this.colors[0] = new color(1, "green");
            this.colors[1] = new color(2, "red");
            this.colors[2] = new color(3, "yellow");
            this.colors[3] = new color(4, "blue");
            this.rand = new Random();
            len = 0;
        }

        //this function randomly pick a new color to add to the game
        public int AddColor()
        {
            int num = this.rand.Next(1, 5);
            this.list.Enqueue(num);
            this.len++;
            return num;
        }

        //this function enpties the list when the game ends
        public void EndGame()
        {
            for (int i = 0; i < this.len; i++)
            {
                this.list.Dequeue();
            }
            this.len = 0;
        }

        //getters
        public int GetLen()
        {
            return len;
        }
        public Queue<int> GetList()
        {
            return this.list;
        }
        public color[] GetColors()
        {
            return this.colors;
        }
    }
}