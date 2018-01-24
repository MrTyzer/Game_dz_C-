using System;
using System.Drawing;

namespace Game_dz
{
    public class Asteroid : BaseObject
    {
        public int Power { get; set; }
        public Asteroid(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
            Power = 1;
        }
        public override void Draw()
        {
            Game.Buffer.Graphics.FillEllipse(Brushes.White, Pos.X, Pos.Y, Size.Width, Size.Height);
        }

        public override void Respawn()
        {
            var rnd = new Random();
            Pos.Y = rnd.Next(0, Game.Height);
            Pos.X = Game.Width;
        }
    }
}
