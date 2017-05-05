using System;
using System.Collections.Generic;
using System.Text;
using Tao.OpenGl; 

namespace Naves
{
    public class Camera
    {
        public void SelectCamara(int camera)
        {
            //I will work with the model matrix
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            //reset
            Gl.glLoadIdentity();

            switch (camera)
            {
                case 0:
                    {
                        Glu.gluLookAt(0, 2, 4, 0, 0, -2, 0, 1, 0);
                        break;
                    }
                case 1:
                    {
                        Glu.gluLookAt(0, 3, 5, 0, 1, -4, 0, 1, 0);
                        break;
                    }
                case 2:
                    {
                        Glu.gluLookAt(0, 70, 0, 0, 0, -16, 0, 1, 0);
                        break;
                    }
                case 3:
                    {
                        Glu.gluLookAt(3, 3, -47, 1, 0, 1, 0, 1, 0);
                        break;
                    }
            }
        }

        public void SetPerspective()
        {
            //select the projection matrix
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            //reset
            Gl.glLoadIdentity();
            //45 =  vision angle
            //1  = Height-to-width ratio
            //0.1f = Minimum distance in which is painted
            //1000 = maximum distance
            Glu.gluPerspective(65, 1, 0.1f, 1000);
            SelectCamara(0);
        }
    }
}
