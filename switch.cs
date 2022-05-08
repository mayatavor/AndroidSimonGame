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
    class Switch
    {
        private Random rand;
        private int[] rand_nums;

        public Switch()
        {
            rand = new Random();
            rand_nums = new int[4];
        }

        //rolls a random number to switch the colors after every round
        public void mixBoard()
        {
            int num = this.rand.Next(1, 5);
            this.rand_nums[0] = num;

            while (num == this.rand_nums[0]) // changes the place for the first color
            {
                num = this.rand.Next(1, 5);
            }
            this.rand_nums[1] = num;

            while (num == this.rand_nums[0] || num == this.rand_nums[1]) // changes the place for the second number - makes sure that it is not the same place
            {
                num = this.rand.Next(1, 5);
            }
            this.rand_nums[2] = num;

            while (num == this.rand_nums[0] || num == this.rand_nums[1] || num == this.rand_nums[2])// changes the place for the third number - makes sure that the place's not already taken
            {
                num = this.rand.Next(1, 5);
            }
            this.rand_nums[3] = num; // changes the place for the fourth number
        }

        public int[] getNums()
        {
            return this.rand_nums;
        }
    }
}