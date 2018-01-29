using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Game_dz
{
    class Ship : BaseObject
    {
        private int _energy = 100;
        public int Energy => _energy;
        public static event Message MessageDie;
        private Timer _ticRate = new Timer();
        private Timer _shootRate = new Timer();
        private bool _isShoot;
        private bool _isUp;
        private bool _isDown;

        public void EnergyLow(int n)
        {
            _energy -= n;
            if (_energy > 100)
                _energy = 100;
        }

        public Ship(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
            _ticRate.Interval = 20;
            _ticRate.Start();
            _shootRate.Interval = 100;
            _shootRate.Start();
            _isDown = false;
            _isUp = false;
            _isShoot = false;
        }

        public override void Draw()
        {
            Game.Buffer.Graphics.FillEllipse(Brushes.Wheat, Pos.X, Pos.Y, Size.Width, Size.Height);
        }
    
        public void ControlManagerKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey && !_isShoot)
            {
                _shootRate.Tick += Shoot;
                _isShoot = true;
            }
            if (e.KeyCode == Keys.W && !_isUp)
            {
                _ticRate.Tick += Up;
                _isUp = true;
            }
            if (e.KeyCode == Keys.S && !_isDown)
            {
                _ticRate.Tick += Down;
                _isDown = true;
            }
        }

        public void ControlManagerKeyUp(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey && _isShoot)
            {
                _shootRate.Tick -= Shoot;
                _isShoot = false;
            }
            if (e.KeyCode == Keys.W && _isUp)
            {
                _ticRate.Tick -= Up;
                _isUp = false;
            }
            if (e.KeyCode == Keys.S && _isDown)
            {
                _ticRate.Tick -= Down;
                _isDown = false;
            }
        }

        public override void Update()
        {
        }

        public void Shoot(object sender, EventArgs e)
        {
            Game._bullet.Add(new Bullet(new Point(Rect.X + 10, Rect.Y + 4),
            new Point(4, 0), new Size(4, 1)));
        }

        public void Up(object sender, EventArgs e)
        {
            if (Pos.Y > 0) Pos.Y = Pos.Y - Dir.Y;
        }

        public void Down(object sender, EventArgs e)
        {
            if (Pos.Y < Game.Height - 80) Pos.Y = Pos.Y + Dir.Y;
        }

        public void Die()
        {
            MessageDie?.Invoke();
        }
    }

}
