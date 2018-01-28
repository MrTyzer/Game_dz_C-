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

        public static Asteroid Spawn()
        {
            var rnd = new Random();
            int r = rnd.Next(20, 50);
            return new Asteroid(new Point(Game.Width, rnd.Next(0, Game.Height)), new Point(150 / r, r), new
            Size(r, r), r / 10);
        }

        public void SwapSpeed(Asteroid obj)
        {
            if (Power < obj.Power)
                Dir = obj.Dir;
            else
                obj.Dir = Dir; 
        }

        public void PowerLow()
        {
            Power -= 1;
            if (Power < 1)
            {
                Respawn();
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
