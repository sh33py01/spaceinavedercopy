using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMPLib;


namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {

        bool goLeft, goRight;
        int playerSpeed = 12;
        int enemySpeed = 5;
        int score = 0;
        int enemyBulletTimer = 300;

        PictureBox[] sadInvadersArray;

        bool shooting;
        bool isGameOver;


        WindowsMediaPlayer play = new WindowsMediaPlayer();
        WindowsMediaPlayer bang = new WindowsMediaPlayer();


        public Form1()
        {
            InitializeComponent();
            gameSetup();
            play.URL = "epic.mp3";//bg music
            bang.URL = "bang.mp3";//space craft shooting
        }

        private void mainGameTimerEvent(object sender, EventArgs e)
        {

            txtScore.Text = "Score :" + score;

            if (goLeft)
            {
                player.Left -= playerSpeed;
            }


            if(goRight)
            {
                player.Left += playerSpeed;
            }

            enemyBulletTimer -= 10;

            if(enemyBulletTimer < 1)
            {
                enemyBulletTimer = 300;
                makeBullet("sadBullet");
            }

            foreach(Control x in this.Controls)
            {
                if(x is PictureBox && (string)x.Tag == "sadinavders")
                {
                    x.Left += enemySpeed;

                    if(x.Left > 730)
                    {
                        x.Top += 65;
                        x.Left = -80;
                    }

                    if(x.Bounds.IntersectsWith(player.Bounds))
                    {
                        gameOver("Invaders Won, Loser");
                    }

                    foreach(Control y in this.Controls)
                    {
                        if (y is PictureBox && (string)y.Tag == "bullet")
                        {
                            if (y.Bounds.IntersectsWith(x.Bounds))
                            {
                                this.Controls.Remove(x);
                                this.Controls.Remove(y);
                                score += 1;
                                shooting = false;
                            }
                        }
                    }
                }
                if(x is PictureBox && (string)x.Tag =="bullet")
                {
                    x.Top -= 20;

                    if(x.Top < 15)
                    {
                        this.Controls.Remove(x);
                        shooting = false;
                    }
                }
                if(x is PictureBox && (string)x.Tag == "sadBullet")
                {
                    x.Top += 20;
                    if (x.Top > 620)
                    {
                        this.Controls.Remove(x);
                    }
                    
                    if(x.Bounds.IntersectsWith(player.Bounds))
                    {
                        this.Controls.Remove(x);
                        gameOver("You are dead, how can you not dodge bullets");
                    }
                }
            }
            if(score > 8)
            {
                enemySpeed = 12;
            }

            if( score == sadInvadersArray.Length)
            {
                gameOver("Wow you did something with your life");
            }
        }


        private void keyisdown(object sender, KeyEventArgs e)//key tracking the aircraft
        {
            if (e.KeyCode == Keys.Left)
            {
                goLeft = true;
            }

            if (e.KeyCode == Keys.Right)
            {
                goRight = true;
            }
            
        }

        private void keyisup(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {

                goLeft = false;
            }

            if (e.KeyCode == Keys.Right)
            {

                goRight = false;
            }

            if (e.KeyCode == Keys.Space && shooting == false)
            {
                shooting = true;
                //call sound
                bang.controls.play();
                makeBullet("bullet");
            }

            if(e.KeyCode == Keys.Enter && isGameOver == true)
            {
                removeAll();
                gameSetup();
            }
        }

        private void makeInvaders()
        {

            sadInvadersArray = new PictureBox[15];

            int left = 0;

            for(int i= 0; i < sadInvadersArray.Length; i++)
            {
                sadInvadersArray[i] = new PictureBox();
                sadInvadersArray[i].Size = new Size(60,50);
                sadInvadersArray[i].Image = Properties.Resources.inavders;
                sadInvadersArray[i].Top =5 ;
                sadInvadersArray[i].Tag = "sadinavders";
                sadInvadersArray[i].Left = left;
                sadInvadersArray[i].SizeMode = PictureBoxSizeMode.StretchImage;
                this.Controls.Add(sadInvadersArray[i]);
                left = left - 80;
            }
        }

        private void gameSetup()
        {

            txtScore.Text = "Score : 0";
            score = 0;
            isGameOver = false;

            enemyBulletTimer = 300;
            enemySpeed = 5;
            shooting = false;

            makeInvaders();
            gameTimer.Start();

        }

        private void gameOver(string message)
        {

            isGameOver = true;
            gameTimer.Stop();
            txtScore.Text = "Score: " + score + " " + message;
            play.controls.stop();
            bang.controls.stop();
        }

        private void removeAll()
        {
            foreach(PictureBox i  in sadInvadersArray)
            {
                this.Controls.Remove(i);
            }

            foreach(Control x in this.Controls)
            {

                if (x is PictureBox)
                {
                    if((string)x.Tag == "bullet" || (string)x.Tag == "sadBullet")
                    {
                        this.Controls.Remove(x);
                    }
                }

            }

        }

        private void axWindowsMediaPlayer1_Enter(object sender, EventArgs e)
        {
            play.controls.play();
            bang.controls.play();

        }

        private void makeBullet(string bulletTag)
        {

            PictureBox bullet = new PictureBox();
            bullet.Image = Properties.Resources.bullet;
            bullet.Size = new Size(5, 20);
            bullet.Tag = bulletTag;
            bullet.Left = player.Left + player.Width / 2;

            if ((string)bullet.Tag == "bullet")
            {
                bullet.Top = player.Top - 20;
                
            }
            else if ((string)bullet.Tag == "sadBullet")
            {
                bullet.Top = -100;
            }
            this.Controls.Add(bullet);
            bullet.BringToFront();
            


        }
    } 
}
