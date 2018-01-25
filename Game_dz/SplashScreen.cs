using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Game_dz
{
    public static class SplashScreen
    {
        // Свойства
        // Ширина и высота игрового поля
        public static int Width { get; set; }
        public static int Height { get; set; }

        public static Form MainForm { get; set; }

        public static Button button1 { get; private set; }
        public static Button button2 { get; private set; }
        public static Button button3 { get; private set; }


        static SplashScreen()
        {
        }

        public static void Init(Form form)
        {
            // Графическое устройство для вывода графики
            Graphics g;
            // предоставляет доступ к главному буферу графического контекста для текущего приложения
            g = form.CreateGraphics();// Создаём объект - поверхность рисования и связываем его с формой
                                      // Запоминаем размеры формы
            Width = form.Width;
            Height = form.Height;
            // Связываем буфер в памяти с графическим объектом.
            // для того, чтобы рисовать в буфере
            Image newImage = Image.FromFile("Galaxy.jpg");
            form.BackgroundImage = newImage;
        }

        public static void FormInit(int width, int height)
        {
            if (width < 0 || width > 1000 || height < 0 || height > 1000)
                throw new ArgumentOutOfRangeException();
            Form form = new Form();
            form.Width = width;
            form.Height = height;
            button1 = new Button();
            button2 = new Button();
            button3 = new Button();
            button1.Text = "Начало игры";
            button2.Text = "Рекорды";
            button3.Text = "Выход";
            button1.AutoSize = true;
            button2.AutoSize = true;
            button3.AutoSize = true;
            button1.Location = new Point((width / 2) - button1.Width / 2, height / 2);
            button2.Location = new Point((width / 2) - button2.Width / 2, (height / 2) + 30);
            button3.Location = new Point((width / 2) - button3.Width / 2, (height / 2) + 60);
            form.Controls.Add(button1);
            form.Controls.Add(button2);
            form.Controls.Add(button3);
            button1.Click += StartGame;
            button3.Click += Quit;
            MainForm = form;
            Init(MainForm);
        }


        public static void StartGame(Object sender, EventArgs e)
        {
            Game.Init(MainForm);
            button1.Hide();
            button2.Hide();
            button3.Location = new Point(0, 0);
            Game.Draw();
        }


        public static void Quit(Object sender, EventArgs e)
        {
            Application.Exit();
        }

    }
}
