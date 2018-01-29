using System;
using System.Drawing;

namespace Game_dz
{
    public class Asteroid : BaseObject , IComparable<Asteroid>
    {
        public int Power { get; set; } = 3;
        public Asteroid(Point pos, Point dir, Size size , int power) : base(pos, dir, size)
        {
            Power = power;
        }

        public static Asteroid Spawn(int diff)
        {
            var rnd = new Random();
            int r = rnd.Next(30, 80);
            return new Asteroid(new Point(Game.Width, rnd.Next(0, Game.Height)), new Point((400 / r) + diff * 2, r), new
            Size(r, r), (r / 15) + diff);
        }

        public void Empower()
        {
            Power += 1;
        }

        public void PowerLow()
        {
            Power -= 1;
            if (Power < 1)
            {
                Respawn();
                System.Media.SystemSounds.Asterisk.Play();
                Power = Size.Height / 10;
                Game.Points += 10 * Power;
            }
        }

        int IComparable<Asteroid>.CompareTo(Asteroid obj)
        {
            if (Power > obj.Power)
                return 1;
            if (Power < obj.Power)
                return -1;
            return 0;
        }


        public override void Draw()
        {
            Game.Buffer.Graphics.FillEllipse(Brushes.White, Pos.X, Pos.Y, Size.Width, Size.Height);
        }
    }
}
