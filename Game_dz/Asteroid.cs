using System;
using System.Drawing;

namespace Game_dz
{
    public class Asteroid : BaseObject ,ICloneable, IComparable<Asteroid>
    {
        public int Power { get; set; } = 3;
        public Asteroid(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
            Power = 1;
        }

        public object Clone()
        {
            Asteroid asteroid = new Asteroid(new Point(Pos.X, Pos.Y), new Point(Dir.X, Dir.Y),
            new Size(Size.Width, Size.Height))
            { Power = Power };
            // Не забываем скопировать новому астероиду Power нашего астероида
            return asteroid;
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
