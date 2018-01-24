using System;
using System.Drawing;

namespace Game_dz
{
    public interface ICollision
    {
        bool Collision(ICollision obj);
        Rectangle Rect { get; }
    }
}
