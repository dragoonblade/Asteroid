using System;
using System.Collections.Generic;
using System.Text;
using Tao.OpenGl;
using System.Windows.Forms;
using ShadowEngine;
using ShadowEngine.Sound;
using ShadowEngine.ContentLoading; 

namespace Naves
{
    public class SpaceShip
    {
        int lives = 3;
        Position p = new Position(0, 0, 0);
        int count;
        int compare = 10;
        ModelContainer m;
        bool move;

        public bool Move
        {
            get { return move; }
            set { move = value; }
        }

        public int Lives
        {
            get { return lives; }
            set { lives = value; }
        }

        public void Restart()
        {
            lives = 3;
            p = new Position(0, 0, 0);
        }

        public void Create()
        {
             m = ContentManager.GetModelByName("nave.3DS");
             m.CreateDisplayList(); 
        }

        public void Draws()
        {
            if (lives > 0)
            {
                switch (Main.Move)
                {
                    case 1:
                        {
                            if (p.x < 3)
                                p.x += 0.08f;
                            break;
                        }
                    case -1:
                        {
                            if (p.x > -3)
                                p.x -= 0.08f;
                            break;
                        }
                    case 2:
                        {
                            if (p.y < 3)
                                p.y += 0.08f;
                            break;
                        }
                    case -2:
                        {
                            if (p.y > -3)
                                p.y -= 0.08f;
                            break;
                        }
                    default:
                        break;
                }

                count++;
                if (count == compare)
                {
                    if (compare == 25)
                    {
                        compare = 10;
                    }
                    Position wingLeft = new Position(this.p.x - 1f, this.p.y, this.p.z);
                    Position wingRight = new Position(this.p.x + 1f, this.p.y, this.p.z);
                    if (AsteroidGenerator.CheckCollision(this.p, 1.1f))
                    {
                        Death(); 
                    }
                    else
                        if (AsteroidGenerator.CheckCollision(wingRight, 1.1f))
                        {
                            Death(); 
                        }
                        else
                            if (AsteroidGenerator.CheckCollision(wingLeft, 1.1f))
                            {
                                Death(); 
                            }
                    count = 0;
                }


                Gl.glPushMatrix();
                Gl.glTranslatef(p.x, p.y, p.z);
                Gl.glScalef(0.3f, 0.3f, 0.3f);
                m.DrawWithTextures();
                Gl.glPopMatrix();
                Gl.glDisable(Gl.GL_TEXTURE_2D);
            }

           
        }
        void Death()
        {
            compare = 25;
            lives--;
            p = new Position(0, 0, 0);
        }
    }
}
