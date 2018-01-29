using System;
using System.Drawing;
namespace Game_dz
{
    public abstract class BaseObject : ICollision
    {
        protected Point Pos;
        public Point Dir;
        protected Size Size;
        public delegate void Message();

        protected BaseObject(Point pos, Point dir, Size size)
        {
            Pos = pos;
            if (pos.X > Game.Width || pos.Y > Game.Height || pos.X < 0 || pos.Y < 0)
                throw new GameObjectException("Объект не может быть создан за пределами экрана");
            Dir = dir;
            if (dir.X > 100 || dir.Y > 100)
                throw new GameObjectException("Слишком большая скорость объекта");
            Size = size;
            if (size.Height < 0 || size.Width < 0)
                throw new GameObjectException("Объект не может иметь отрицательный размер");
        }
        public abstract void Draw();

        public virtual void Haste()
        {
            Dir.X += 2;
        }
        
        public virtual void Update()
        {
            Pos.X = Pos.X - Dir.X;
            if (Pos.X < 0)
            {
                Respawn();
            }

        }

        public bool Collision(ICollision o) => o.Rect.IntersectsWith(this.Rect);
        public Rectangle Rect => new Rectangle(Pos, Size);

        public virtual void Respawn()
        {
            var rnd = new Random();
            Pos.Y = rnd.Next(0, Game.Height);
            Pos.X = Game.Width;
        }

    }
}