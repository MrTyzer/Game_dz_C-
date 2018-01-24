﻿using System;
using System.Drawing;


namespace Game_dz
{
    public class Bullet : BaseObject, ICollision
    {
        public Bullet(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
        }
        public override void Draw()
        {
            Game.Buffer.Graphics.DrawRectangle(Pens.OrangeRed, Pos.X, Pos.Y, Size.Width, Size.Height);
        }
        public override void Update()
        {
            Pos.X = Pos.X + 10;
            if (Pos.X > Game.Width)
            {
                Respawn();
            }
        }

        public override void Respawn()
        {
            var rnd = new Random();
            Pos.Y = rnd.Next(0, Game.Height);
            Pos.X = 0;
        }

    }
}