using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Media;
using System.Drawing.Drawing2D;
using System.Threading;

namespace SimonSays
{
    public partial class GameScreen : UserControl
    {
        #region Variables

        public static List<int> compMoves = new List<int>(); //computer moves list
        int moveCount; //track total player moves
        Random randGen = new Random(); //random generator

        //graphics paths 
        GraphicsPath curvePath = new GraphicsPath();
        GraphicsPath innerCurve = new GraphicsPath();

        //region and matrix variables
        Region buttonRegion = new Region();
        Matrix transformMatrix = new Matrix();

        //soundplayer variables
        SoundPlayer greenSound = new SoundPlayer(Properties.Resources.green);
        SoundPlayer redSound = new SoundPlayer(Properties.Resources.red);
        SoundPlayer yellowSound = new SoundPlayer(Properties.Resources.yellow);
        SoundPlayer blueSound = new SoundPlayer(Properties.Resources.blue);
        SoundPlayer loseSound = new SoundPlayer(Properties.Resources.mistake);

        #endregion

        public GameScreen()
        {
            InitializeComponent();
        }

        private void GameScreen_Load(object sender, EventArgs e)
        {
            if (MenuScreen.reverseGame == true)
            {
                //creates reverse game's shape
                curvePath.AddArc(new Rectangle(greenButton.Width / 2, greenButton.Width / 2, greenButton.Width, greenButton.Height), 180, 90);
                curvePath.AddArc(new Rectangle(greenButton.Width * 3 / 4, greenButton.Width * 3 / 4, greenButton.Width - 20, greenButton.Height - 20), 180, 90);

                //create region and exclude inner circle
                buttonRegion = new Region(curvePath);
            }
            else
            {
                //create ellipse and region
                curvePath.AddEllipse(0, 0, greenButton.Width * 2, greenButton.Height * 2);

                //creates inner curve
                innerCurve.AddEllipse(greenButton.Width - 40, greenButton.Height - 40, 82, 82);

                //create region and exclude inner circle
                buttonRegion = new Region(curvePath);
                buttonRegion.Exclude(innerCurve);
            }

            //change green button
            greenButton.Region = buttonRegion;

            //rotate region onto red button
            transformMatrix.RotateAt(90, new PointF(greenButton.Width / 2, greenButton.Height / 2));
            buttonRegion.Transform(transformMatrix);
            redButton.Region = buttonRegion;

            //transform to other buttons
            buttonRegion.Transform(transformMatrix);
            blueButton.Region = buttonRegion;

            buttonRegion.Transform(transformMatrix);
            yellowButton.Region = buttonRegion;

            //clear list in event of new game and do setup for a fresh game run
            compMoves.Clear();
            this.Refresh();
            ComputerTurn();
        }

        private void ComputerTurn()
        {
            //if game is in reverse, reverse the list back to its initial state
            if (MenuScreen.reverseGame == true)
            {
                compMoves.Reverse();
            }

            //creates and adds new random to list
            compMoves.Add(randGen.Next(1, 5));

            //sleep variable + a half second sleep upon each computer turn for looks
            int sleepTime = 500;
            Thread.Sleep(500);

            //for loop that shows computer's moves
            for (int i = 0; i < compMoves.Count; i++)
            {
                //displays each button in the list
                switch (compMoves[i])
                {
                    case 1:
                        //light up button and play it's noise
                        greenButton.BackColor = Color.LawnGreen;
                        greenButton.Refresh();
                        greenSound.Play();

                        //delay to show lit up button
                        Thread.Sleep(sleepTime);

                        //reset
                        greenButton.BackColor = Color.ForestGreen;
                        greenButton.Refresh();

                        //small delay in event of two of the same buttons in a row
                        Thread.Sleep(100);
                        break;
                    case 2: //repeat across each button
                        redButton.BackColor = Color.Salmon;
                        redButton.Refresh();
                        redSound.Play();

                        Thread.Sleep(sleepTime);

                        redButton.BackColor = Color.DarkRed;
                        redButton.Refresh();
                        Thread.Sleep(100);
                        break;
                    case 3:
                        yellowButton.BackColor = Color.PaleGoldenrod;
                        yellowButton.Refresh();
                        yellowSound.Play();

                        Thread.Sleep(sleepTime);

                        yellowButton.BackColor = Color.Goldenrod;
                        yellowButton.Refresh();
                        Thread.Sleep(100);
                        break;
                    case 4:
                        blueButton.BackColor = Color.Blue;
                        blueButton.Refresh();
                        blueSound.Play();

                        Thread.Sleep(sleepTime);

                        blueButton.BackColor = Color.DarkBlue;
                        blueButton.Refresh();
                        Thread.Sleep(100);
                        break;
                }
            }

            //if game is in reverse, reverse the list
            if (MenuScreen.reverseGame == true)
            {
                compMoves.Reverse();
            }
        }

        //variable to show player's button click
        int clickSleep = 200;

        //TODO: create one of these event methods for each button
        private void greenButton_Click(object sender, EventArgs e)
        {
            //visuals and sound to display a button click
            greenButton.BackColor = Color.LawnGreen;
            greenButton.Refresh();
            greenSound.Play();

            Thread.Sleep(clickSleep);

            greenButton.BackColor = Color.ForestGreen;
            greenButton.Refresh();

            //check if the current click is equal to the computer's move
            if (compMoves[moveCount] == 1)
            {
                //if yes, add to the list and check if the entire list has been run through
                moveCount++;
                if (moveCount == compMoves.Count())
                {
                    moveCount = 0;
                    ComputerTurn();
                }
            }
            else
            {
                GameOver();
            }
        }

        //repeats greenButton code for other 3 buttons, with adjustments for each relative button
        private void redButton_Click(object sender, EventArgs e)
        {
            redButton.BackColor = Color.Salmon;
            redButton.Refresh();
            redSound.Play();

            Thread.Sleep(clickSleep);

            redButton.BackColor = Color.DarkRed;
            redButton.Refresh();

            if (compMoves[moveCount] == 2)
            {
                moveCount++;
                if (moveCount == compMoves.Count())
                {
                    moveCount = 0;
                    ComputerTurn();
                }
            }
            else
            {
                GameOver();
            }
        }

        private void yellowButton_Click(object sender, EventArgs e)
        {
            yellowButton.BackColor = Color.PaleGoldenrod;
            yellowButton.Refresh();

            Thread.Sleep(clickSleep);

            yellowButton.BackColor = Color.Goldenrod;
            yellowButton.Refresh();

            if (compMoves[moveCount] == 3)
            {
                moveCount++;
                yellowSound.Play();
                if (moveCount == compMoves.Count())
                {
                    moveCount = 0;
                    ComputerTurn();
                }
            }
            else
            {
                GameOver();
            }
        }

        private void blueButton_Click(object sender, EventArgs e)
        {
            blueButton.BackColor = Color.Blue;
            blueButton.Refresh();

            Thread.Sleep(clickSleep);

            blueButton.BackColor = Color.DarkBlue;
            blueButton.Refresh();

            if (compMoves[moveCount] == 4)
            {
                moveCount++;
                blueSound.Play();
                if (moveCount == compMoves.Count())
                {
                    moveCount = 0;
                    ComputerTurn();
                }
            }
            else
            {
                GameOver();
            }
        }

        //game over method
        public void GameOver()
        {
            //plays lose sound and switches to game over screen
            loseSound.Play();
            Thread.Sleep(1000);
            Form1.ScreenChange(this, new GameOverScreen());
        }
    }
}
