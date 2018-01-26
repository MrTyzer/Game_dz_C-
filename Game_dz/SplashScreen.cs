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

        public static Button Button1 { get; private set; }
        public static Button Button2 { get; private set; }
        public static Button Button3 { get; private set; }
        public static Label Box { get; private set; }


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
            Button1 = new Button();
            Button2 = new Button();
            Button3 = new Button();
            Button1.Text = "Начало игры";
            Button2.Text = "Рекорды";
            Button3.Text = "Выход";
            Button1.AutoSize = true;
            Button2.AutoSize = true;
            Button3.AutoSize = true;
            Button1.Location = new Point((width / 2) - Button1.Width / 2, height / 2);
            Button2.Location = new Point((width / 2) - Button2.Width / 2, (height / 2) + 30);
            Button3.Location = new Point((width / 2) - Button3.Width / 2, (height / 2) + 60);
            form.Controls.Add(Button1);
            form.Controls.Add(Button2);
            form.Controls.Add(Button3);
            Button1.Click += StartGame;
            Button3.Click += Quit;
            MainForm = form;
            Init(MainForm);
        }


        public static void StartGame(Object sender, EventArgs e)
        {
            Game.Init(MainForm);
            Button1.Hide();
            Button2.Hide();
            MainForm.KeyPreview = true;
            Button3.Location = new Point(0, Game.Height - 60);
            Game.Draw();
        }


        public static void Quit(Object sender, EventArgs e)
        {
            Application.Exit();
        }

    }
}
