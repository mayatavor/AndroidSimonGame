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
using System.Threading;


namespace maya_simon
{
    [Activity(Label = "levelNumberOne", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class levelNumberOne : Activity
    {
        private Button red, green, blue, yellow, back;
        private game g;
        private color[] colors;
        private TextView score;
        private List<int> allColorsSoFar;
        private AlertDialog.Builder alertDiaBuild;

        private EditText nameScore;
        private Button enterScore, newGame, backToMenu;
        private string name;
        private int place;

        private int scoreNow;
        ISharedPreferences sp;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.levelOne);

            red = FindViewById<Button>(Resource.Id.btnred4);
            yellow = FindViewById<Button>(Resource.Id.btnyellow4);
            green = FindViewById<Button>(Resource.Id.btngreen4);
            blue = FindViewById<Button>(Resource.Id.btnblue4);
            back = FindViewById<Button>(Resource.Id.btnbackLevel1);
            score = FindViewById<TextView>(Resource.Id.TvScore);
            sp = this.GetSharedPreferences("scores", FileCreationMode.Private);
            allColorsSoFar = new List<int>();
            g = new game();
            scoreNow = 0;
            place = -1;
            this.colors = new color[4];
            name = "1";

            this.colors = g.GetColors();

            red.Click += color_click;
            blue.Click += color_click;
            green.Click += color_click;
            yellow.Click += color_click;

            back.Click += Back_Click;

            addColor();
            PlayRound();
        }

        //goes back to the main menu
        private void Back_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);
        }


        /*
         * this function is called when the player is clicking on a color button
         * it calls a function to find out the color clicked, play the color's sound 
         * the function gets the returned color id and checks if the color is the currect color
         *      if not: it will show the user a dialog box with options
         *      is yes: it will up the score, add a number and play the sounds
         */
        private async void color_click(object sender, EventArgs e)
        {
            int id = 0;
            if (sender == green)
            {
                id = 1;
                this.colors[id - 1].playSound();
                green.SetBackgroundResource(Resource.Drawable.greenPressedTL);
                await Task.Delay(500);
                green.SetBackgroundResource(Resource.Drawable.greenTL);
            }
            else if (sender == red)
            {
                id = 2;
                this.colors[id - 1].playSound();
                red.SetBackgroundResource(Resource.Drawable.redPressedTR);
                await Task.Delay(500);
                red.SetBackgroundResource(Resource.Drawable.redTR);
            }
            else if (sender == yellow)
            {
                id = 3;
                this.colors[id - 1].playSound();
                yellow.SetBackgroundResource(Resource.Drawable.yellowPressedBL);
                await Task.Delay(500);
                yellow.SetBackgroundResource(Resource.Drawable.yellowBL);
            }
            else if (sender == blue)
            {
                id = 4;
                this.colors[id - 1].playSound();
                blue.SetBackgroundResource(Resource.Drawable.bluePressedBR);
                await Task.Delay(500);
                blue.SetBackgroundResource(Resource.Drawable.blueBR);
            }

            if (id != 0)
            {
                if (allColorsSoFar.ElementAt(0) != id)
                {
                    place = checkForScore();
                    if (place == 0)
                    {
                        //creates a dialog box
                        alertDiaBuild = new Android.App.AlertDialog.Builder(this);
                        alertDiaBuild.SetTitle("New Game");
                        alertDiaBuild.SetMessage("Start new game ?");
                        alertDiaBuild.SetPositiveButton("Yes", OK);
                        alertDiaBuild.SetNegativeButton("Back to menu", MENU);
                        alertDiaBuild.Create();
                        alertDiaBuild.Show();
                    }
                    else
                    {
                        SetContentView(Resource.Layout.custom_dialog);

                        nameScore = FindViewById<EditText>(Resource.Id.ETname);
                        enterScore = FindViewById<Button>(Resource.Id.btnEnter);
                        backToMenu = FindViewById<Button>(Resource.Id.btnMenu);
                        newGame = FindViewById<Button>(Resource.Id.btnNewGame);

                        enterScore.Click += EnterScore_Click;
                        backToMenu.Click += BackToMenu_Click;
                        newGame.Click += NewGame_Click;
                    }
                }
                else
                    allColorsSoFar.RemoveAt(0);

                if (allColorsSoFar.Count() == 0) // another round was successful
                {
                    addToScore(); // adds to the player's score

                    copyToList();
                    addColor();
                    PlayRound();
                }
            }
        }

        //this function is called when the user's lost and chose to restart the game
        //it creates a new game object and delets the old game information
        private void NewGame_Click(object sender, EventArgs e)
        {
            SetContentView(Resource.Layout.levelOne);

            red = FindViewById<Button>(Resource.Id.btnred4);
            yellow = FindViewById<Button>(Resource.Id.btnyellow4);
            green = FindViewById<Button>(Resource.Id.btngreen4);
            blue = FindViewById<Button>(Resource.Id.btnblue4);
            back = FindViewById<Button>(Resource.Id.btnbackLevel1);
            score = FindViewById<TextView>(Resource.Id.TvScore);
            sp = this.GetSharedPreferences("scores", FileCreationMode.Private);
            allColorsSoFar = new List<int>();
            g = new game();
            scoreNow = 0;
            place = -1;
            this.colors = new color[4];
            name = "1";

            this.colors = g.GetColors();

            red.Click += color_click;
            blue.Click += color_click;
            green.Click += color_click;
            yellow.Click += color_click;

            back.Click += Back_Click;

            this.g.EndGame();
            emptyList();
            addColor();
            PlayRound();
        }

        private void BackToMenu_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);
        }

        private void EnterScore_Click(object sender, EventArgs e)
        {
            this.name = this.nameScore.Text;

            if (name == "")
                Toast.MakeText(this, "Please Enter Your Name If You Want To Be On The Score Board", ToastLength.Long).Show();

            else
            {
                inputToScore();
                Intent intent = new Intent(this, typeof(scores));
                StartActivity(intent);
            }
        }

        private void MENU(object sender, DialogClickEventArgs e)
        {
            Intent intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);
        }

        private void OK(object sender, DialogClickEventArgs e)
        {
            //restart the game
            this.g.EndGame();
            emptyList();
            addColor();
            PlayRound();
        }

        /*
         * this function plays the color's sounds and changes the color background
         */
        public async void PlayRound()
        {
            Queue<int> copy = new Queue<int>();
            Queue<int> l = this.g.GetList();
            int id = 0;
            green.Clickable = false;
            red.Clickable = false;
            blue.Clickable = false;
            yellow.Clickable = false;
            for (int i = 0; i < this.g.GetLen(); i++)
            {
                id = l.Dequeue();
                copy.Enqueue(id);
                await Task.Delay(300);
                if (id == 1) // the color green
                {
                    this.colors[id - 1].playSound();
                    green.SetBackgroundResource(Resource.Drawable.greenPressedTL);
                    await Task.Delay(500);
                    green.SetBackgroundResource(Resource.Drawable.greenTL);
                }
                else if (id == 2) // get color red
                {
                    this.colors[id - 1].playSound();
                    red.SetBackgroundResource(Resource.Drawable.redPressedTR);
                    await Task.Delay(500);
                    red.SetBackgroundResource(Resource.Drawable.redTR);
                }
                else if (id == 3) // get color yellow
                {
                    this.colors[id - 1].playSound();
                    yellow.SetBackgroundResource(Resource.Drawable.yellowPressedBL);
                    await Task.Delay(500);
                    yellow.SetBackgroundResource(Resource.Drawable.yellowBL);
                }
                else if (id == 4) // get color blue
                {
                    this.colors[id - 1].playSound();
                    blue.SetBackgroundResource(Resource.Drawable.bluePressedBR);
                    await Task.Delay(500);
                    blue.SetBackgroundResource(Resource.Drawable.blueBR);
                }
            }
            while (copy.Count() != 0)
            {
                l.Enqueue(copy.Dequeue());
            }

            green.Clickable = true;
            red.Clickable = true;
            blue.Clickable = true;
            yellow.Clickable = true;
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
         * this function empties the color list when the player wants to restart the game
         */
        private void emptyList()
        {
            for (int i = 0; i < this.allColorsSoFar.Count(); i++)
            {
                this.allColorsSoFar.RemoveAt(0);
            }
        }

        //this function is called after a successful round
        //it adds to the score
        private void addToScore()
        {
            int num = this.g.GetLen(); // gets the number of colors in the round
            this.scoreNow += num * num; // raises the number of colors to the power of 2 and adds it to the score
            this.scoreNow += 10; // adds 10 to the score
            this.score.Text = scoreNow.ToString(); // converts to string
        }


        /*
         * checks if the player as scored a score high enough
         * returns 0 if no
         *         1 if he got to first place
         *         2 if he got to second place
         *         3 if he got to third place
         */
        private int checkForScore()
        {
            int s = sp.GetInt("scoreFirst", 0);
            if (this.scoreNow > s) // checks if the player got to first place
                return 1;
            if (this.scoreNow == s)// checks if the player got to second place
                return 2;
            s = sp.GetInt("scoreSecond", 0);
            if (this.scoreNow > s) // checks if the player got to second place
                return 2;
            if (this.scoreNow == s) // checks if the player got to third place
                return 3;
            s = sp.GetInt("scoreThird", 0);
            if (this.scoreNow > s) // checks if the player got to third place
                return 3;
            return 0; // the player did not get on the best players list
        }

        /*
         * changes the file to contain the first three best scores
         */
        private void inputToScore()
        {
            ISharedPreferencesEditor editor = sp.Edit();

            string player1 = sp.GetString("playerFirst", "---");
            int score1 = sp.GetInt("scoreFirst", 0);

            string player2 = sp.GetString("playerSecond", "---");
            int score2 = sp.GetInt("scoreSecond", 0);

            if (place == 1) //if the player got to first place, we push down the first place to second and the second to third
            {
                editor.PutString("playerFirst", this.name);
                editor.PutString("playerSecond", player1);
                editor.PutString("playerThird", player2);

                editor.PutInt("scoreFirst", this.scoreNow);
                editor.PutInt("scoreSecond", score1);
                editor.PutInt("scoreThird", score2);
            }
            else if (place == 2) //if the player got to second place, we push down the second place to third
            {
                editor.PutString("playerSecond", this.name);
                editor.PutString("playerThird", player2);

                editor.PutInt("scoreSecond", this.scoreNow);
                editor.PutInt("scoreThird", score2);
            }
            else //if the player got to third place, we change the third place name and score to this player
            {
                editor.PutString("playerThird", this.name);

                editor.PutInt("scoreThird", this.scoreNow);
            }
            editor.Commit();
        }
    }
}