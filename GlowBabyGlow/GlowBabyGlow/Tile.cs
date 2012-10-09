using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GlowBabyGlow
{
    class Tile : Entity
    {
        static int size = 30;

        public Rectangle Rect
        {
            get { return rect; }
        }

        public static int Size
        {
            get { return size; }
        }

        public Tile(Point pos)
        {
            rect = new Rectangle(pos.X, pos.Y, size, size);
        }

        public override void Update(float dt)
        {
            base.Update(dt);
        }

        /// <summary>
        /// tile is above r
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public int OverlappingAbove(Rectangle r)
        {
            Rectangle testRect = new Rectangle(
               r.Center.X - 1, r.Y, 2, r.Height);
            if (testRect.Bottom > rect.Top &&
                testRect.Top < rect.Bottom &&
                testRect.Right > rect.Left &&
                testRect.Left < rect.Right)
            {
                return r.Bottom - rect.Top;
            }
            return 0;
        }

        public bool StandingOn(Rectangle r)
        {
            Rectangle testRect = new Rectangle(
                r.Center.X - 1, r.Y, 2, r.Height);
            if (testRect.Bottom > rect.Top - 2 &&
                testRect.Top < rect.Bottom &&
                testRect.Right > rect.Left &&
                testRect.Left < rect.Right)
            {
                return true;
            }
            return false;
        }

        public int OverlappingRight(Rectangle r)
        {
            if (r.Bottom > rect.Top &&
                r.Top < rect.Bottom &&
                r.Right > rect.Left + rect.Width / 2 &&
                r.Left < rect.Right)
            {
                return rect.Right - r.Left;
            }
            return 0;
        }

        public int OverlappingLeft(Rectangle r)
        {
            if (r.Bottom > rect.Top &&
                r.Top < rect.Bottom &&
                r.Right > rect.Left &&
                r.Left < rect.Right - rect.Width / 2)
            {
                return r.Right - rect.Left;
            }
            return 0;
        }

        public override void Draw(SpriteBatch sb, SpriteEffects effect)
        {
            sb.Draw(TextureManager.tile, rect, Color.White);
        }
    }
}
