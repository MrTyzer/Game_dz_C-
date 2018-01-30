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
        #region Private data
        private static BufferedGraphicsContext _context;
        private static Timer _timer = new Timer();
        private static Timer _difficultyTimer = new Timer();
        private static int _difficultyMultiplier = 0;
        private static MedKit _kit;
        private static List<Asteroid> _asteroids;
        private static Ship _ship;
        #endregion

        #region Public Data
        public static BufferedGraphics Buffer;
        public static Random Rnd = new Random();
        public static List<Bullet> _bullet = new List<Bullet>();
        public static List<Bullet> BulletsToDestroy = new List<Bullet>();
        public static BaseObject[] _objs;
        public static event EventHandler<string> Log;
        #endregion

        #region Properties
        // Ширина и высота игрового поля
        public static int Width { get; private set; }
        public static int Height { get; private set; }
        //Игровые очки
        public static int Points { get; set; } = 0;
        #endregion

        #region Public methods
        public static void Load()
        {
            _objs = new BaseObject[15];
            _asteroids = new List<Asteroid>();
            var rnd = new Random();
            int d = rnd.Next(5, Height);
            _kit = new MedKit(new Point(Width, d), new Point(7, 0), new Size(15, 15));
            for (var i = 0; i < _objs.Length; i++)
            {
                int r = rnd.Next(5, 25);
                _objs[i] = new Star(new Point(rnd.Next(0, Width), rnd.Next(0, Height)), new Point(20, r), new Size(2, 2));
            }
            for (var i = 0; i < 3; i++)
            {
                int r = rnd.Next(30, 80);
                _asteroids.Add(new Asteroid(new Point(Width, rnd.Next(0, Height)), new Point(400 / r, r), new
                Size(r, r), r / 15));
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
            _ship = new Ship(new Point(8, 400), new Point(10, 10), new Size(10, 10));

            // Связываем буфер в памяти с графическим объектом.
            // для того, чтобы рисовать в буфере
            Buffer = _context.Allocate(g, new Rectangle(0, 0, Width, Height));

            Ship.MessageDie += Finish;
            form.KeyDown += Form_KeyDown;
            form.KeyUp += Form_KeyUp;
            Log += WriteToLog;

            try
            {
                Load();
            }
            catch (GameObjectException d)
            {
                Console.WriteLine(d.Message);
            }
            _difficultyTimer.Interval = 10000;
            _difficultyTimer.Start();
            _difficultyTimer.Tick += DifficultyInc;
            _timer.Interval = 20;
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
            _ship.ControlManagerKeyDown(e);
        }

        public static void Form_KeyUp(object sender, KeyEventArgs e)
        {
            _ship.ControlManagerKeyUp(e);
        }


        public static void Draw()
        {
            // Проверяем вывод графики

            Buffer.Graphics.Clear(Color.Black);
            foreach (BaseObject obj in _objs)
                obj?.Draw();
            foreach (Asteroid a in _asteroids)
            {
                a?.Draw();
            }

            foreach (Bullet a in _bullet)
            {
                a?.Draw();
            }

            _kit?.Draw();
            _ship?.Draw();
            if (_ship != null)
                Buffer.Graphics.DrawString("Energy:" + _ship.Energy, SystemFonts.DefaultFont, Brushes.White, 0, 0);
            Buffer.Graphics.DrawString("Points:" + Points, SystemFonts.DefaultFont, Brushes.White, 80, 0);
            Buffer.Render();

        }

        

        public static void Update()
        {
            foreach (BaseObject obj in _objs)
                obj?.Update();
            foreach (Asteroid obj in _asteroids)
            {
                if (_bullet.Any())
                {
                    foreach (Bullet bul in _bullet)
                    {
                        if (obj.Collision(bul))
                        {
                            obj?.PowerLow();
                            BulletsToDestroy.Add(bul);
                        }
                    }
                    foreach (Bullet bul in BulletsToDestroy)
                    {
                        _bullet.Remove(bul);
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

            foreach (Bullet bul in _bullet)
            {
                bul.Update();
            }
                if (_kit.Collision(_ship))
                {
                    _ship?.EnergyLow(-10);
                    System.Media.SystemSounds.Asterisk.Play();
                    _kit?.Respawn();
                    Log(_kit, "Use med kit");
                }
            
            _ship?.Update();
            _kit?.Update();
        }

        public static void Finish()
        {
            _timer.Stop();
            Buffer.Graphics.DrawString("The End", new Font(FontFamily.GenericSansSerif, 60,
            FontStyle.Underline), Brushes.White, 200, 100);
            Buffer.Render();
            SplashScreen.Results.Add(Points);
            if (SplashScreen.Results.Count > 5)
            {
                SplashScreen.Results.Remove(SplashScreen.Results.Last());
            }
            using (StreamWriter sw = new StreamWriter("Records.txt", true))
            {

                foreach (int res in SplashScreen.Results)
                    sw.WriteLine(res.ToString());
            }
        }
        #endregion

        #region Timer methods

        private static void Timer_Tick(object sender, EventArgs e)
        {
            Draw();
            Update();
        }

        private static void DifficultyInc(object sender, EventArgs e)
        {
            _asteroids.Add(Asteroid.Spawn(_difficultyMultiplier));
            _kit.Haste();
            foreach (Asteroid ast in _asteroids)
            {
                ast.Haste();
                ast.Empower();
            }
            _difficultyMultiplier += 1;
        }
        #endregion
    }
}
