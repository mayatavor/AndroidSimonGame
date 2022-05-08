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
using System.Threading.Tasks;

namespace maya_simon
{
    [Activity(Label = "crazyLevelActivity", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class crazyLevelActivity : Activity
    {
        private Button back, top_left, top_right, bottom_left, bottom_right;
        private AlertDialog.Builder alertDiaBuild;
        private List<int> allColorsSoFar;
        private game g;
        private color[] colors;
        private TextView score;
        private Switch s;
        private int number_of_colors, id;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.crazyLevel);
            back = FindViewById<Button>(Resource.Id.btnbackCrazyLevel);
            top_left = FindViewById<Button>(Resource.Id.btnTopLeft);
            top_right = FindViewById<Button>(Resource.Id.btnTopRight);
            bottom_left = FindViewById<Button>(Resource.Id.btnBottomLeft);
            bottom_right = FindViewById<Button>(Resource.Id.btnBottomRight);
            score = FindViewById<TextView>(Resource.Id.TvScoreCrazy);
            g = new game();
            s = new Switch();
            this.colors = new color[4];
            number_of_colors = 0;
            this.id = 0;
            this.colors = g.GetColors();

            back.Click += Back_to_menu_Click;
            top_right.Click += color_click;
            top_left.Click += color_click;
            bottom_left.Click += color_click;
            bottom_right.Click += color_click;

            number_of_colors = 0;
            allColorsSoFar = new List<int>();

            addColor();
            PlayRound();
        }

        private void Back_to_menu_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);
        }

        /*
         * this function is called when the player is clicking on a color button
         * it calls a function to find out the color clicked, play the color's sound 
         * the function gets the returned color id (this.id) and checks if the color is the currect color
         *      if not: it will show the user a dialog box with options
         *      is yes: it will up the score, mix the board again, add a number and play the sounds
         */
        private async void color_click(object sender, EventArgs e)
        {
            findAndPlaySound(sender);

            if (this.id != 0)
            {
                if (allColorsSoFar.ElementAt(0) != this.id)
                {
                    alertDiaBuild = new Android.App.AlertDialog.Builder(this);
                    alertDiaBuild.SetTitle("New Game");
                    alertDiaBuild.SetMessage("Start new game ?");
                    alertDiaBuild.SetPositiveButton("Yes", OK);
                    alertDiaBuild.SetNegativeButton("Back to menu", MENU);
                    alertDiaBuild.Create();
                    alertDiaBuild.Show();
                }
                else
                    allColorsSoFar.RemoveAt(0);

                if (allColorsSoFar.Count() == 0)
                {
                    await Task.Delay(500);
                    number_of_colors++;
                    this.score.Text = number_of_colors.ToString();
                    MixBoard();
                    copyToList();
                    addColor();
                    PlayRound();
                }
            }
        }

        private void MENU(object sender, DialogClickEventArgs e)
        {
            Intent intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);
        }

        private void OK(object sender, DialogClickEventArgs e)
        {
            this.g.EndGame();
            emptyList();
            addColor();
            PlayRound();
        }

        /*
         * this function plays the sounds of the colors in each round
         */
        public async void PlayRound()
        {
            Queue<int> copy = new Queue<int>();
            Queue<int> l = this.g.GetList();
            int id = 0;
            top_left.Clickable = false;
            top_right.Clickable = false;
            bottom_left.Clickable = false;
            bottom_right.Clickable = false;
            for (int i = 0; i < this.g.GetLen(); i++)
            {
                id = l.Dequeue();
                copy.Enqueue(id);
                await Task.Delay(600);
                if (id == 1)
                {
                    changeGreen();
                }
                else if (id == 2)
                {
                    changeRed();
                }
                else if (id == 3)
                {
                    changeYellow();
                }
                else if (id == 4)
                {
                    changeBlue();
                }
            }
            while (copy.Count() != 0)
            {
                l.Enqueue(copy.Dequeue());
            }

            top_right.Clickable = true;
            top_left.Clickable = true;
            bottom_left.Clickable = true;
            bottom_right.Clickable = true;
        }

        /*
         * this function empties the color list when the player wants to restart the game
         */
        private void emptyList()
        {
            for (int i = 0; i < this.allColorsSoFar.Count(); i++)
            {
                this.allColorsSoFar.RemoveAt(0);
            }
        }


        /*
         * this funciton copys the colors list back to the allColorsSoFar Queue from the game object
         */
        private void copyToList()
        {
            Queue<int> l = this.g.GetList();
            Queue<int> copy = new Queue<int>();
            int id;

            for (int i = 0; i < this.g.GetLen(); i++)
            {
                id = l.Dequeue();
                copy.Enqueue(id);
                this.allColorsSoFar.Add(id);
            }

            while (copy.Count() != 0)
            {
                l.Enqueue(copy.Dequeue());
            }
        }

        //adds a color to the colors list
        private void addColor()
        {
            int c = g.AddColor();
            allColorsSoFar.Add(c);
        }

        /*
         * this function gets a button that the player clicked
         * the function checks what place the button is in, and what the color of that button
         * it plays that color's sound
         */
        private async void findAndPlaySound(object sender)
        {
            if (sender == top_right)
            {
                if (top_right.Tag.Equals("red"))
                {
                    this.id = 2;
                    this.colors[1].playSound();
                    top_right.SetBackgroundResource(Resource.Drawable.redPressedTR);
                    await Task.Delay(500);
                    top_right.SetBackgroundResource(Resource.Drawable.redTR);
                }
                else if (top_right.Tag.Equals("green"))
                {
                    this.id = 1;
                    this.colors[0].playSound();
                    top_right.SetBackgroundResource(Resource.Drawable.greenPressedTR);
                    await Task.Delay(500);
                    top_right.SetBackgroundResource(Resource.Drawable.greenTR);
                }
                else if (top_right.Tag.Equals("yellow"))
                {
                    this.id = 3;
                    this.colors[2].playSound();
                    top_right.SetBackgroundResource(Resource.Drawable.yellowPressedTR);
                    await Task.Delay(500);
                    top_right.SetBackgroundResource(Resource.Drawable.yellowTR);
                }
                else
                {
                    this.id = 4;
                    this.colors[3].playSound();
                    top_right.SetBackgroundResource(Resource.Drawable.bluePressedTR);
                    await Task.Delay(500);
                    top_right.SetBackgroundResource(Resource.Drawable.blueTR);
                }
            }
            else if (sender == top_left)
            {
                if (top_left.Tag.Equals("red"))
                {
                    this.id = 2;
                    this.colors[1].playSound();
                    top_left.SetBackgroundResource(Resource.Drawable.redPressedTL);
                    await Task.Delay(500);
                    top_left.SetBackgroundResource(Resource.Drawable.redTL);
                }
                else if (top_left.Tag.Equals("green"))
                {
                    this.id = 1;
                    this.colors[0].playSound();
                    top_left.SetBackgroundResource(Resource.Drawable.greenPressedTL);
                    await Task.Delay(500);
                    top_left.SetBackgroundResource(Resource.Drawable.greenTL);
                }
                else if (top_left.Tag.Equals("yellow"))
                {
                    this.id = 3;
                    this.colors[2].playSound();
                    top_left.SetBackgroundResource(Resource.Drawable.yellowPressedTL);
                    await Task.Delay(500);
                    top_left.SetBackgroundResource(Resource.Drawable.yellowTL);
                }
                else
                {
                    this.id = 4;
                    this.colors[3].playSound();
                    top_left.SetBackgroundResource(Resource.Drawable.bluePressedTL);
                    await Task.Delay(500);
                    top_left.SetBackgroundResource(Resource.Drawable.blueTL);
                }
            }
            else if (sender == bottom_left)
            {
                if (bottom_left.Tag.Equals("red"))
                {
                    this.id = 2;
                    this.colors[1].playSound();
                    bottom_left.SetBackgroundResource(Resource.Drawable.redPressedBL);
                    await Task.Delay(500);
                    bottom_left.SetBackgroundResource(Resource.Drawable.redBL);
                }
                else if (bottom_left.Tag.Equals("green"))
                {
                    this.id = 1;
                    this.colors[0].playSound();
                    bottom_left.SetBackgroundResource(Resource.Drawable.greenPressedBL);
                    await Task.Delay(500);
                    bottom_left.SetBackgroundResource(Resource.Drawable.greenBL);
                }
                else if (bottom_left.Tag.Equals("yellow"))
                {
                    this.id = 3;
                    this.colors[2].playSound();
                    bottom_left.SetBackgroundResource(Resource.Drawable.yellowPressedBL);
                    await Task.Delay(500);
                    bottom_left.SetBackgroundResource(Resource.Drawable.yellowBL);
                }
                else
                {
                    this.id = 4;
                    this.colors[3].playSound();
                    bottom_left.SetBackgroundResource(Resource.Drawable.bluePressedBL);
                    await Task.Delay(500);
                    bottom_left.SetBackgroundResource(Resource.Drawable.blueBL);
                }
            }
            else
            {
                if (bottom_right.Tag.Equals("red"))
                {
                    this.id = 2;
                    this.colors[1].playSound();
                    bottom_right.SetBackgroundResource(Resource.Drawable.redPressedBR);
                    await Task.Delay(500);
                    bottom_right.SetBackgroundResource(Resource.Drawable.redBR);
                }
                else if (bottom_right.Tag.Equals("green"))
                {
                    this.id = 1;
                    this.colors[0].playSound();
                    bottom_right.SetBackgroundResource(Resource.Drawable.greenPressedBR);
                    await Task.Delay(500);
                    bottom_right.SetBackgroundResource(Resource.Drawable.greenBR);
                }
                else if (bottom_right.Tag.Equals("yellow"))
                {
                    this.id = 3;
                    this.colors[2].playSound();
                    bottom_right.SetBackgroundResource(Resource.Drawable.yellowPressedBR);
                    await Task.Delay(500);
                    bottom_right.SetBackgroundResource(Resource.Drawable.yellowBR);
                }
                else
                {
                    this.id = 4;
                    this.colors[3].playSound();
                    bottom_right.SetBackgroundResource(Resource.Drawable.bluePressedBR);
                    await Task.Delay(500);
                    bottom_right.SetBackgroundResource(Resource.Drawable.blueBR);
                }
            }
        }


        /*
         * these functions know what color they are looking for, and are going through the 4 places to find where that color is
         * once the color is found, the color backgroud is changed and the color's sound is played
         */

        private async void changeRed()
        {
            if (top_left.Tag.Equals("red"))
            {
                this.colors[1].playSound();
                top_left.SetBackgroundResource(Resource.Drawable.redPressedTL);
                await Task.Delay(500);
                top_left.SetBackgroundResource(Resource.Drawable.redTL);
            }
            else if (top_right.Tag.Equals("red"))
            {
                this.colors[1].playSound();
                top_right.SetBackgroundResource(Resource.Drawable.redPressedTR);
                await Task.Delay(500);
                top_right.SetBackgroundResource(Resource.Drawable.redTR);
            }
            else if (bottom_right.Tag.Equals("red"))
            {
                this.colors[1].playSound();
                bottom_right.SetBackgroundResource(Resource.Drawable.redPressedBR);
                await Task.Delay(500);
                bottom_right.SetBackgroundResource(Resource.Drawable.redBR);
            }
            else
            {
                this.colors[1].playSound();
                bottom_left.SetBackgroundResource(Resource.Drawable.redPressedBL);
                await Task.Delay(500);
                bottom_left.SetBackgroundResource(Resource.Drawable.redBL);
            }
        }
        private async void changeGreen()
        {
            if (top_left.Tag.Equals("green"))
            {
                this.colors[0].playSound();
                top_left.SetBackgroundResource(Resource.Drawable.greenPressedTL);
                await Task.Delay(500);
                top_left.SetBackgroundResource(Resource.Drawable.greenTL);
            }
            else if (top_right.Tag.Equals("green"))
            {
                this.colors[0].playSound();
                top_right.SetBackgroundResource(Resource.Drawable.greenPressedTR);
                await Task.Delay(500);
                top_right.SetBackgroundResource(Resource.Drawable.greenTR);
            }
            else if (bottom_right.Tag.Equals("green"))
            {
                this.colors[0].playSound();
                bottom_right.SetBackgroundResource(Resource.Drawable.greenPressedBR);
                await Task.Delay(500);
                bottom_right.SetBackgroundResource(Resource.Drawable.greenBR);
            }
            else
            {
                this.colors[0].playSound();
                bottom_left.SetBackgroundResource(Resource.Drawable.greenPressedBL);
                await Task.Delay(500);
                bottom_left.SetBackgroundResource(Resource.Drawable.greenBL);
            }
        }
        private async void changeBlue()
        {
            if (top_left.Tag.Equals("blue"))
            {
                this.colors[3].playSound();
                top_left.SetBackgroundResource(Resource.Drawable.bluePressedTL);
                await Task.Delay(500);
                top_left.SetBackgroundResource(Resource.Drawable.blueTL);
            }
            else if (top_right.Tag.Equals("blue"))
            {
                this.colors[3].playSound();
                top_right.SetBackgroundResource(Resource.Drawable.bluePressedTR);
                await Task.Delay(500);
                top_right.SetBackgroundResource(Resource.Drawable.blueTR);
            }
            else if (bottom_right.Tag.Equals("blue"))
            {
                this.colors[3].playSound();
                bottom_right.SetBackgroundResource(Resource.Drawable.bluePressedBR);
                await Task.Delay(500);
                bottom_right.SetBackgroundResource(Resource.Drawable.blueBR);
            }
            else
            {
                this.colors[3].playSound();
                bottom_left.SetBackgroundResource(Resource.Drawable.bluePressedBL);
                await Task.Delay(500);
                bottom_left.SetBackgroundResource(Resource.Drawable.blueBL);
            }
        }
        private async void changeYellow()
        {
            if (top_left.Tag.Equals("yellow"))
            {
                this.colors[2].playSound();
                top_left.SetBackgroundResource(Resource.Drawable.yellowPressedTL);
                await Task.Delay(500);
                top_left.SetBackgroundResource(Resource.Drawable.yellowTL);
            }
            else if (top_right.Tag.Equals("yellow"))
            {
                this.colors[2].playSound();
                top_right.SetBackgroundResource(Resource.Drawable.yellowPressedTR);
                await Task.Delay(500);
                top_right.SetBackgroundResource(Resource.Drawable.yellowTR);
            }
            else if (bottom_right.Tag.Equals("yellow"))
            {
                this.colors[2].playSound();
                bottom_right.SetBackgroundResource(Resource.Drawable.yellowPressedBR);
                await Task.Delay(500);
                bottom_right.SetBackgroundResource(Resource.Drawable.yellowBR);
            }
            else
            {
                this.colors[2].playSound();
                bottom_left.SetBackgroundResource(Resource.Drawable.yellowPressedBL);
                await Task.Delay(500);
                bottom_left.SetBackgroundResource(Resource.Drawable.yellowBL);
            }
        }

        /*
         * this function randomly picks a new place for each color and changes the background and the tag for that color
         */
        private void MixBoard()
        {
            s.mixBoard();
            int[] rands = s.getNums();

            //init top_right
            if (rands[0] == 1)
            { this.top_right.SetBackgroundResource(Resource.Drawable.greenTR); this.top_right.Tag = "green"; }
            else if (rands[0] == 2)
            { this.top_right.SetBackgroundResource(Resource.Drawable.redTR); this.top_right.Tag = "red"; }
            else if (rands[0] == 3)
            { this.top_right.SetBackgroundResource(Resource.Drawable.blueTR); this.top_right.Tag = "blue"; }
            else
            { this.top_right.SetBackgroundResource(Resource.Drawable.yellowTR); this.top_right.Tag = "yellow"; }

            //init top_left
            if (rands[1] == 1)
            { this.top_left.SetBackgroundResource(Resource.Drawable.greenTL); this.top_left.Tag = "green"; }
            else if (rands[1] == 2)
            { this.top_left.SetBackgroundResource(Resource.Drawable.redTL); this.top_left.Tag = "red"; }
            else if (rands[1] == 3)
            { this.top_left.SetBackgroundResource(Resource.Drawable.blueTL); this.top_left.Tag = "blue"; }
            else
            { this.top_left.SetBackgroundResource(Resource.Drawable.yellowTL); this.top_left.Tag = "yellow"; }

            //init bottom_left
            if (rands[2] == 1)
            { this.bottom_left.SetBackgroundResource(Resource.Drawable.greenBL); this.bottom_left.Tag = "green"; }
            else if (rands[2] == 2)
            { this.bottom_left.SetBackgroundResource(Resource.Drawable.redBL); this.bottom_left.Tag = "red"; }
            else if (rands[2] == 3)
            { this.bottom_left.SetBackgroundResource(Resource.Drawable.blueBL); this.bottom_left.Tag = "blue"; }
            else
            { this.bottom_left.SetBackgroundResource(Resource.Drawable.yellowBL); this.bottom_left.Tag = "yellow"; }

            //init bottom_right
            if (rands[3] == 1)
            { this.bottom_right.SetBackgroundResource(Resource.Drawable.greenBR); this.bottom_right.Tag = "green"; }
            else if (rands[3] == 2)
            { this.bottom_right.SetBackgroundResource(Resource.Drawable.redBR); this.bottom_right.Tag = "red"; }
            else if (rands[3] == 3)
            { this.bottom_right.SetBackgroundResource(Resource.Drawable.blueBR); this.bottom_right.Tag = "blue"; }
            else
            { this.bottom_right.SetBackgroundResource(Resource.Drawable.yellowBR); this.bottom_right.Tag = "yellow"; }
        }
    }
}