using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Game_dz
{
    class Program
    {

        static void Main(string[] args)
        {
            int width = 800;
            int height = -700;

            try
            {
            SplashScreen.FormInit(width, height);
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.WriteLine(e);
                width = 800;
                height = 600;
                SplashScreen.FormInit(width, height);
            }

            SplashScreen.MainForm.Show();
            Application.Run(SplashScreen.MainForm);
        }


    }
}
