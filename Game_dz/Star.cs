using System;
using System.Drawing;

namespace Game_dz
{
    public class Star : BaseObject
    {
        public Star(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
        }

        public override void Draw()
        {
            Game.Buffer.Graphics.DrawLine(Pens.White, Pos.X, Pos.Y, Pos.X + Size.Width, Pos.Y + Size.Height);
            Game.Buffer.Graphics.DrawLine(Pens.White, Pos.X + Size.Width, Pos.Y, Pos.X, Pos.Y + Size.Height);
        }

        public override void Respawn()
        {
            var rnd = new Random();
            Pos.Y = rnd.Next(0, Game.Height);
            Pos.X = Game.Width;
        }
    }
}
