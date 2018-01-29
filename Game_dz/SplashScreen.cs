using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.IO;

namespace Game_dz
{
    public static class SplashScreen
    {
        // Свойства
        // Ширина и высота игрового поля
        public static int Width { get; private set; }
        public static int Height { get; private set; }

        public static Form MainForm { get; set; }
        public static SortedSet<int> Results = new SortedSet<int>(); 

        //Виджеты
        public static Button Button1 { get; private set; }
        public static Button Button2 { get; private set; }
        public static Button Button3 { get; private set; }
        public static Button Button4 { get; private set; }
        public static ListView ListBox { get; private set; }


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
            Button4 = new Button();
            ListBox = new ListView();
            ListBox.Location = new Point(200, 200);
            ListBox.Hide();
            Button1.Text = "Начало игры";
            Button2.Text = "Рекорды";
            Button3.Text = "Выход";
            Button4.Text = "Назад";
            Button1.AutoSize = true;
            Button2.AutoSize = true;
            Button3.AutoSize = true;
            Button4.AutoSize = true;
            Button4.Hide();
            Button1.Location = new Point((width / 2) - Button1.Width / 2, height / 2);
            Button2.Location = new Point((width / 2) - Button2.Width / 2, (height / 2) + 30);
            Button3.Location = new Point((width / 2) - Button3.Width / 2, (height / 2) + 60);
            form.Controls.Add(Button1);
            form.Controls.Add(Button2);
            form.Controls.Add(Button3);
            form.Controls.Add(Button4);
            form.Controls.Add(ListBox);
            using (StreamReader sr = new StreamReader("Records.txt"))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    int buf;
                    Int32.TryParse(line, out buf);
                    Results.Add(buf);
                } 
            }
            Button1.Click += StartGame;
            Button2.Click += Records;
            Button3.Click += Quit;
            Button4.Click += Back;
            MainForm = form;
            Init(MainForm);
        }


        public static void StartGame(Object sender, EventArgs e)
        {
            Game.Init(MainForm);
            Button1.Hide();
            Button2.Hide();
            Button4.Hide();
            MainForm.KeyPreview = true;
            Button3.Location = new Point(0, Game.Height - 60);
            Game.Draw();
        }

        public static void Records(Object sender, EventArgs e)
        {
            Button3.Hide();
            Button2.Hide();
            Button1.Hide();
            Button4.Location = new Point(400, 400);
            Button4.Show();
            ListBox.Show();
            ListBox.Clear();
            int i = 1;
            foreach (int res in Results.Reverse())
            {
                ListBox.Items.Add(i.ToString() + ") " + res.ToString());
                i += 1;
            }
        }

        public static void Back(Object sender, EventArgs e)
        {
            ListBox.Hide();
            Button1.Show();
            Button2.Show();
            Button3.Show();
            Button4.Hide();
        }


        public static void Quit(Object sender, EventArgs e)
        {
            Application.Exit();
        }

    }
}
