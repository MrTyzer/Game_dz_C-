using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Game_dz
{
    class MedKit : BaseObject
    {
        public MedKit(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
        }

        public override void Draw()
        {
            Game.Buffer.Graphics.FillEllipse(Brushes.Red, Pos.X, Pos.Y, Size.Width, Size.Height);
        }
    }
}
