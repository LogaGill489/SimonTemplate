using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Media;
using System.Drawing.Drawing2D;

namespace SimonSays
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //launches menu screen
            ScreenChange(this, new MenuScreen());
        }

        public static void ScreenChange(object sender, UserControl next)
        {
            //creates a form
            Form form;

            //checks if the sender is a form
            if (sender is Form)
            {
                //if yes, make the variable "form" the sender form
                form = sender as Form;
            }
            else
            {
                //if not, sender is a usercontrol and is stored as the usercontrol "current"
                UserControl current = sender as UserControl;
                form = current.FindForm();
                form.Controls.Remove(current);
            }

            //put the new usercontrol in the centre of the screen
            next.Location = new Point((form.Width - next.Width) / 2, (form.Height - next.Height) / 2);

            //display the new usercontrol
            form.Controls.Add(next);
        }
    }
}
