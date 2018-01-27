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
    public static class Game
    {
        private static BufferedGraphicsContext _context;
        public static BufferedGraphics Buffer;
        private static Timer _timer = new Timer();
        public static Random Rnd = new Random();
        public static int Width { get; set; }
        public static int Height { get; set; }
        public static int Points { get; private set; } = 0;
        private static Bullet _bullet;
        private static MedKit _kit;
        private static Asteroid[] _asteroids;
        public static event EventHandler<string> Log;

        private static Ship _ship;
        // Свойства
        // Ширина и высота игрового поля

        static Game()
        {
        }

        public static BaseObject[] _objs;
        public static void Load()
        {
            _objs = new BaseObject[15];
            _asteroids = new Asteroid[5];
            var rnd = new Random();
            int d = rnd.Next(5, Height);
            _kit = new MedKit(new Point(Width, d), new Point(7, 0), new Size(15, 15));
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
            _ship = new Ship(new Point(10, 400), new Point(5, 5), new Size(10, 10));

            // Связываем буфер в памяти с графическим объектом.
            // для того, чтобы рисовать в буфере
            Buffer = _context.Allocate(g, new Rectangle(0, 0, Width, Height));

            Ship.MessageDie += Finish;
            form.KeyDown += Form_KeyDown;
            Log += WriteToLog;

            try
            {
                Load();
            }
            catch (GameObjectException d)
            {
                Console.WriteLine(d.Message);
            }
            _timer.Interval = 30;
            _timer.Start();
            _timer.Tick += Timer_Tick;

        }

        public static void WriteToLog(object sender, string message)
        {
            Console.WriteLine(message);
            using (StreamWriter sw = new StreamWriter("log.txt", true))
            {
                sw.WriteLine(message);
            }

        }


        public static void Form_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey)
                _bullet = new Bullet(new Point(_ship.Rect.X + 10, _ship.Rect.Y + 4),
            new Point(4, 0), new Size(4, 1));
            if (e.KeyCode == Keys.W)
                _ship.Up();
            if (e.KeyCode == Keys.S)
                _ship.Down();
        }

        public static void Draw()
        {
            // Проверяем вывод графики

            Buffer.Graphics.Clear(Color.Black);
            foreach (BaseObject obj in _objs)
                obj.Draw();
            foreach (Asteroid a in _asteroids)
            {
                a?.Draw();
            }
            _kit?.Draw();
            _bullet?.Draw();
            _ship?.Draw();
            if (_ship != null)
                Buffer.Graphics.DrawString("Energy:" + _ship.Energy, SystemFonts.DefaultFont, Brushes.White, 0, 0);
            Buffer.Graphics.DrawString("Points:" + Points, SystemFonts.DefaultFont, Brushes.White, 80, 0);
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
                obj?.Update();
            foreach (BaseObject obj in _asteroids)
            {
                if (_bullet != null)
                {
                    if (obj.Collision(_bullet))
                    {
                        System.Media.SystemSounds.Asterisk.Play();
                        _bullet = null;
                        obj?.Respawn();
                        Points += 10;
                        Log(obj, "Asteroid destroyed");
                    }
                }
                if (obj.Collision(_ship))
                {
                    _ship?.EnergyLow(10);
                    obj?.Respawn();
                    System.Media.SystemSounds.Hand.Play();
                    Log(obj, "Ship was hit by asteroid");
                    if (_ship.Energy <= 0)
                        _ship?.Die();
                }

                obj?.Update();
            }
            if (_kit.Collision(_ship))
            {
                _ship?.EnergyLow(-10);
                System.Media.SystemSounds.Asterisk.Play();
                _kit?.Respawn();
                Log(_kit, "Use med kit");
            }
            _bullet?.Update();
            _ship?.Update();
            _kit?.Update();
        }

        public static void Finish()
        {
            _timer.Stop();
            Buffer.Graphics.DrawString("The End", new Font(FontFamily.GenericSansSerif, 60,
            FontStyle.Underline), Brushes.White, 200, 100);
            Buffer.Render();
        }


    }

}
