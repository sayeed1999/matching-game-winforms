using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp4
{
    public partial class Form1 : Form
    {
        // Use this Random object to choose random icons for the squares
        Random random = new Random();

        // Each of these letters is an interesting icon
        // in the Webdings font,
        // and each icon appears twice in this list
        List<string> icons = new List<string>()
        {
            "!", "!", "N", "N", 
            ",", ",", "k", "k",
            "b", "b", "v", "v", 
            "w", "w", "z", "z"
        };

        List<Color> colors = new List<Color>() { 
            Color.Azure, 
            Color.Bisque, 
            Color.Aquamarine, 
            Color.Cornsilk,
        };

        Label firstClicked = null, secondClicked = null;

        public Form1()
        {
            InitializeComponent();

            AssignIconsToSquaresRandomly();
        }

        private bool CheckForWinner()
        {
            bool won = true;
            // Go through all of the labels in the TableLayoutPanel, 
            // checking each one to see if its icon is matched
            foreach(Control control in tableLayoutPanel1.Controls)
            {
                Label iconLabel  = control as Label;
                if (iconLabel != null)
                {
                    if (iconLabel.ForeColor == iconLabel.BackColor) won = false;
                }
            }
            return won;
        }

        private void AssignIconsToSquaresRandomly()
        {
            // The TableLayoutPanel has 16 labels,
            // and the icon list has 16 icons,
            // so an icon is pulled at random from the list
            // and added to each label
            foreach(Control control in tableLayoutPanel1.Controls)
            {
                Label iconLabel = control as Label;
                if (iconLabel != null )
                {
                    int randomNumber = random.Next(icons.Count);
                    iconLabel.Text = icons[randomNumber];

                    // the line below is simulating icon dissapearing
                    // by making backColor == foreColor
                    iconLabel.ForeColor = iconLabel.BackColor;

                    // this line makes sure no indices repeat
                    icons.RemoveAt(randomNumber);
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // stop the timer
            timer1.Stop();

            // hide both icons
            firstClicked.ForeColor = firstClicked.BackColor;
            secondClicked.ForeColor = secondClicked.BackColor;

            // reset first click & second click
            firstClicked = secondClicked = null;

        }

        /// <summary>
        /// Every label's click handler is handled by this event handler
        /// </summary>
        /// <param name="sender">The label that was clicked</param>
        /// <param name="e"></param>
        private void label1_Click(object sender, EventArgs e)
        {
            Label clickedLabel = sender as Label;
            if (clickedLabel != null)
            {
                // If the clicked label's foreColor & backColor is not same,
                // the player clicked an icon that's already been revealed --
                if (clickedLabel.ForeColor != clickedLabel.BackColor)
                {
                    return;
                }

                // If this is the first click, then we have to store it to match
                // it with player's second click!
                if (firstClicked == null)
                {
                    firstClicked = clickedLabel;
                    firstClicked.ForeColor = colors[random.Next(colors.Count)];
                    return;
                }

                // If this is the second click
                if (secondClicked == null)
                {
                    secondClicked = clickedLabel;
                    secondClicked.ForeColor = colors[random.Next(colors.Count)];

                    bool won = CheckForWinner();
                    if (won)
                    {
                        MessageBox.Show("Player won!");
                        Close();
                    }

                    // If player matches his two clicks, he proceeds without resetting!
                    if (firstClicked.Text == secondClicked.Text)
                    {
                        firstClicked = null;
                        secondClicked = null;
                        return;
                    }

                    // If the players fails to match his clicks,
                    // player will get only 750milliseconds to see his results
                    timer1.Start();
                    return;
                }

                // If this is the third click, our game dont allow third clicks!
                MessageBox.Show("3rd click is not allowed for the next 750 milliseconds!");
                return;
            }
        }
    }
}
