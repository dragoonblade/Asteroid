using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Tao.OpenGl;
using ShadowEngine;
using ShadowEngine.OpenGL;
using ShadowEngine.Sound;

namespace Naves
{
    public partial class Main : Form
    {
        //handle the viewport
        uint hdc;
        Controller con = new Controller();
        static int move;
        int score;
        bool started;
        int lives = 3;
        int level = 1;
        int levelCount;
        int count1, count2;
        bool pressed;
        bool stop;

        public static int Move
        {
            get { return Main.move; }
            set { Main.move = value; }
        }

        public Main()
        {
            InitializeComponent();
            hdc = (uint)pnlViewPort.Handle;
            string error = "";
            //Graph window initialization command
            OpenGLControl.OpenGLInit(ref hdc, pnlViewPort.Width, pnlViewPort.Height, ref error);
            //Initiates the position of the camera as well as defines in perspective angle, etc. etc.
            con.Camara.SetPerspective();
            if (error != "")
            {
                MessageBox.Show("Something happened");
            }

            #region lights

            //Setting lights
            float[] materialAmbient = { 0.5F, 0.5F, 0.5F, 1.0F };
            float[] materialDiffuse = { 1f, 1f, 1f, 1.0f };
            float[] materialSpecular = { 1.0F, 1.0F, 1.0F, 1.0F };
            //Material brightness
            float[] materialShininess = { 1.0F };
            //Position of light
            float[] ambientLightPosition = { 10F, -10F, 10F, 1.0F };
            //Light intensity in RGB
            float[] lightAmbient = { 0.85F, 0.85F, 0.85F, 1.0F };

            Lighting.MaterialAmbient = materialAmbient;
            Lighting.MaterialDiffuse = materialDiffuse;
            Lighting.MaterialSpecular = materialSpecular;
            Lighting.MaterialShininess = materialShininess;
            Lighting.AmbientLightPosition = ambientLightPosition;
            Lighting.LightAmbient = lightAmbient;

            #endregion

            //Enable lights
            Lighting.SetupLighting();
            //load textures
            ContentManager.SetModelList("models\\");
            ContentManager.LoadModels();
            ContentManager.SetTextureList("textures\\");
            ContentManager.LoadTextures();
            con.CreateObjects();
            //Background color
            Gl.glClearColor(0, 0, 0, 1);//red green blue alpha 
            Gl.glShadeModel(Gl.GL_SMOOTH);  
        }

        private void tmrPaint_Tick(object sender, EventArgs e)
        {
            // opengl clean
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
            //Draw the scene
            con.DrawScene();
            //Change the buffer
            Winapi.SwapBuffers(hdc);
            //Finish painting
            Gl.glFlush();

            if (started)
            {
                score += 1;
                count1++;
                count2++;
                levelCount++;
                if (levelCount == 450)
                {
                    level++;
                    AsteroidGenerator.GenerateAsteroid(35, false);
                    lblLevel.Text = level.ToString();
                    levelCount = 0;
                }
                if (count1 == 4)
                {
                    lblScore.Text = score.ToString();
                    count1 = 0;
                    lives = con.Nave.Lives;
                }
                if (count2 == 20)
                {
                    ShowLife(lives);
                    count2 = 0;
                    if (lives == 0)
                    {
                        started = false; 
                        MessageBox.Show("Game Over");
                        con.ResetGame();
                        score = 0;
                        level = 1;
                        lives = 3;
                        started = true;
                        count1 = 0;
                        count2 = 0;
                        lblLevel.Text = level.ToString();   
                    }
                }
            }
        }

        void ShowLife(int vidas)
        {
            if (vidas == 3)
            {
                life1.Visible = true;
                life2.Visible = true;
                life3.Visible = true;
            }
            if (vidas == 2)
            {
                life1.Visible = true;
                life2.Visible = true;
                life3.Visible = false;
            }
            if (vidas == 1)
            {
                life1.Visible = true;
                life2.Visible = false;
                life3.Visible = false;
            }
            if (vidas == 0)
            {
                life1.Visible = false;
                life2.Visible = false;
                life3.Visible = false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (started == false)
            {
                con.BeginGame();
                started = true;
                btnStart.Text = "Pause";
            }
            else
            {
                if (stop)
                {
                    stop = false;
                    tmrPaint.Enabled = true;
                    btnStart.Text = "Pause";
                }
                else
                {
                    stop = true;
                    tmrPaint.Enabled = false;
                    btnStart.Text = "Resume";
                }
            }
        }

        private void btnRestart_Click(object sender, EventArgs e)
        {
            con.ResetGame();
            score = 0;
            if (stop)
            {
                stop = false;
                tmrPaint.Enabled = true;
                btnStart.Text = "Pause";
                AudioPlayback.StopAllSounds();
            }
        }

        private void Main_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A)
            {
                move = -1;
            }
            if (e.KeyCode == Keys.D)
            {
                move = 1;
            }
            if (e.KeyCode == Keys.W)
            {
                move = 2;
            }
            if (e.KeyCode == Keys.S)
            {
                move = -2;
            }
            pressed = true;
        }

        private void Main_KeyUp(object sender, KeyEventArgs e)
        {
            move = 0;
            pressed = false; 
        }
    }
}
