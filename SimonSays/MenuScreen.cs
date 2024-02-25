using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SimonSays
{
    public partial class MenuScreen : UserControl
    {

        public static bool reverseGame = false;
        public MenuScreen()
        {
            InitializeComponent();
        }

        private void newButton_Click(object sender, EventArgs e)
        {
            reverseGame = false;

            //changes screen via the method on form1
            Form1.ScreenChange(this, new GameScreen());
        }


        private void exitButton_Click(object sender, EventArgs e)
        {
            //closes application
            Application.Exit();
        }

        private void reverseButton_Click(object sender, EventArgs e)
        {
            reverseGame = true;

            Form1.ScreenChange(this, new GameScreen());
        }
    }
}
