//January 22 2021
//By: Dylan Smith
//A spin on the classic Snake game

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake_Final_Project
{
    public partial class Form1 : Form
    {
        //Declaring all global variables

        Random random = new Random();

        Rectangle slowBallRec;

        int playerScore = 0;
        int playerScoreCounter = 0;

        int snakeX = 345;
        int snakeY = 475;

        int snakeHeight = 35;
        int snakeLength = 35;
        int snakeSpeed = 25;

        int foodHeight = 35;
        int foodWidth = 35;
        int foodX = 1;
        int foodY = 1;
        int foodCounter = 0;

        int slowballHeight = 35;
        int slowballWidth = 35;
        int slowballX = 0;
        int slowballY = 0;

        bool up = false;
        bool down = false;
        bool left = false;
        bool right = false;

        bool slowballExists = false;

        string gameState = "waiting";

        Pen drawPen = new Pen(Color.Black, 4);

        SolidBrush purpleBrush = new SolidBrush(Color.MediumPurple);
        SolidBrush orangeBrush = new SolidBrush(Color.DarkOrange);
        SolidBrush redBrush = new SolidBrush(Color.DarkRed);
        SolidBrush blackBrush = new SolidBrush(Color.Black);
        Font smallFont = new Font("Consolas", 24);
        Font largeFont = new Font("Consolas", 40);


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    up = true;
                    down = false;
                    left = false;
                    right = false;
                    break;
                case Keys.Down:
                    up = false;
                    down = true;
                    left = false;
                    right = false;
                    break;
                case Keys.Left:
                    up = false;
                    left = true;
                    down = false;
                    right = false;
                    break;
                case Keys.Right:
                    up = false;
                    left = false;
                    down = false;
                    right = true;
                    break;
                case Keys.Space:
                    if (gameState == "waiting" || gameState == "over")
                    {
                        GameInitialize();
                    }
                    break;
                case Keys.Escape:
                    if (gameState == "waiting" || gameState == "over" || gameState == "running")
                    {
                        Application.Exit();
                    }
                    break;
            }
        }


        // Paint method to draw eveything on the correct screen 
        private void Form1_Paint(object sender, PaintEventArgs e)
        {

            if (gameState == "running")
            {
                e.Graphics.FillRectangle(purpleBrush, snakeX, snakeY, snakeLength, snakeHeight);

                e.Graphics.FillRectangle(redBrush, foodX, foodY, foodHeight, foodWidth);

                if (slowballExists == true)
                {
                    e.Graphics.FillRectangle(orangeBrush, slowballX, slowballY, slowballHeight, slowballWidth);
                }

                e.Graphics.DrawString($"{playerScore}", smallFont, blackBrush, 155, 35);
            }

            else if (gameState == "over")
            {
                e.Graphics.DrawString($"{playerScore}", largeFont, blackBrush, 530, 355);
            }
        }

        // Method for the timer
        private void gameTimer_Tick(object sender, EventArgs e)
        {
            // Checking to move player
            if (up == true && snakeY > 100)
            {
                snakeY -= snakeSpeed;
            }

            if (down == true && snakeY < this.Height - snakeHeight)
            {
                snakeY += snakeSpeed;
            }

            if (left == true && snakeX > 0)
            {
                snakeX -= snakeSpeed;
            }

            if (right == true && snakeX < this.Width - snakeLength)
            {
                snakeX += snakeSpeed;
            }

            // Making rectangles to allow for collision detection
            Rectangle snakeRec = new Rectangle(snakeX, snakeY, snakeLength, snakeHeight);
            
            Rectangle foodRec = new Rectangle(foodX, foodY, foodWidth, foodHeight);

            // Collision with top and bottom wall
            if (snakeY < 100 || snakeY > this.Height - snakeHeight - 1)
            {
                this.BackgroundImage = Properties.Resources.Gameover;

                gameState = "over";

                gameTimer.Enabled = false;
            }

            // Collision with right and left wall
            if (snakeX < 1 || snakeX > this.Width - snakeLength)
            {        
                this.BackgroundImage = Properties.Resources.Gameover;

                gameState = "over";

                gameTimer.Enabled = false;
            }

            // Snake colliding with food
            if (snakeRec.IntersectsWith(foodRec))
            {
                snakeSpeed += 7;

                foodCounter = 0;

                playerScore++;
            }

            // Snake colliding with slowball/powerup
            if (snakeRec.IntersectsWith(slowBallRec))
            {
                snakeSpeed -= 35;

                playerScore++;

                slowballExists = false;
            }

            // Code to tell it wether or not it can spawn another piece of food
            if (foodCounter == 0)
            {
                foodCounter = 1;

                foodY = random.Next(100, 700);
                while (foodY % 35 != 0)
                {
                    foodY = random.Next(100, 700);
                }

                foodX = random.Next(0, 700);
                while (foodX % 35 != 0)
                {
                    foodX = random.Next(0, 700);
                }
            }

            // Checking to see if player score is in multiples of 5 and spawns a powerup
            if (playerScore % 5 == 0 && playerScore / 5 >= playerScoreCounter && playerScore != 0)
            {
                slowballExists = true;

                playerScoreCounter++;

                slowballY = random.Next(100, 700);
                while (slowballY % 35 != 0)
                {
                    slowballY = random.Next(100, 700);
                }

                slowballX = random.Next(0, 700);
                while (slowballX % 35 != 0)
                {
                    slowballX = random.Next(0, 700);
                }
            }

            if (playerScore % 5 == 0 && playerScore / 5 >= playerScoreCounter)
            {
                slowBallRec = new Rectangle(slowballY, slowballX, slowballWidth, slowballHeight);
            }

            Refresh();
        }

        // Custom Method to initialize all variables at the start of the game
        public void GameInitialize()
        {
            gameTimer.Enabled = true;

            gameState = "running";
            this.BackgroundImage = Properties.Resources.Snake_Hud;

            playerScore = 0;

            snakeX = 350;
            snakeY = 350;
            snakeSpeed = 35;

            up = false;
            down = false;
            left = false;
            down = false;
        }
    }
}
