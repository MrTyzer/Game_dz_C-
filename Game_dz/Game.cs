using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Game_dz
{
    public static class Game
    {
        private static BufferedGraphicsContext _context;
        public static BufferedGraphics Buffer;
        // Свойства
        // Ширина и высота игрового поля
        public static int Width { get; set; }
        public static int Height { get; set; }

        private static Bullet _bullet;
        private static Asteroid[] _asteroids;

        static Game()
        {
        }

        public static BaseObject[] _objs;
        public static void Load()
        {
            _objs = new BaseObject[15];
            _bullet = new Bullet(new Point(0, 200), new Point(5, 0), new Size(4, 1));
            _asteroids = new Asteroid[5];
            var rnd = new Random();
            for (var i = 0; i < _objs.Length; i++)
            {
                int r = rnd.Next(5, 25);
                _objs[i] = new Star(new Point(rnd.Next(0, Game.Width), rnd.Next(0, Game.Height)), new Point(20, r), new Size(2, 2));
            }
            for (var i = 0; i < _asteroids.Length; i++)
            {
                int r = rnd.Next(10, 60);
                _asteroids[i] = new Asteroid(new Point(rnd.Next(0, Game.Width), rnd.Next(0, Game.Height)), new Point(r / 6, r), new
                Size(r, r));
            }
        }


        public static void Init(Form form)
        {
            // Графическое устройство для вывода графики
            Graphics g;
            // предоставляет доступ к главному буферу графического контекста для текущего приложения
            _context = BufferedGraphicsManager.Current;
            g = form.CreateGraphics();// Создаём объект - поверхность рисования и связываем его с формой
                                      // Запоминаем размеры формы
                Width = form.Width;
                Height = form.Height;

            // Связываем буфер в памяти с графическим объектом.
            // для того, чтобы рисовать в буфере
            Buffer = _context.Allocate(g, new Rectangle(0, 0, Width, Height));

            try
            {
                Load();
            }
            catch (GameObjectException d)
            {
                Console.WriteLine(d.Message);
            }

            Timer timer = new Timer { Interval = 30 };
            timer.Start();
            timer.Tick += Timer_Tick;

        }
        public static void Draw()
        {
            // Проверяем вывод графики

            Buffer.Graphics.Clear(Color.Black);
            foreach (BaseObject obj in _objs)
                obj.Draw();
            foreach (BaseObject obj in _asteroids)
                obj.Draw();
            _bullet.Draw();
            Buffer.Render();
        }

        private static void Timer_Tick(object sender, EventArgs e)
        {
            Draw();
            Update();
        }

        public static void Update()
        {
            foreach (BaseObject obj in _objs)
                obj.Update();
            foreach (BaseObject obj in _asteroids)
            {
                if (obj.Collision(_bullet))
                {
                    System.Media.SystemSounds.Hand.Play();
                    _bullet.Respawn();
                    obj.Respawn();
                }
                obj.Update();
            }
            _bullet.Update();
        }

    }

}
