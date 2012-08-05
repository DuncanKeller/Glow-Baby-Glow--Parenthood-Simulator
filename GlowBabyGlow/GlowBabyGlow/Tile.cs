using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GlowBabyGlow
{
    class Tile : Obj
    {
        int size = 50;
        Rectangle rect;

        public Tile(Point pos)
        {
            rect = new Rectangle(pos.X, pos.Y, size, size);
        }

        public int OverlappingAbove(Rectangle r)
        {
            if (r.Bottom > rect.Top &&
                r.Top < rect.Bottom &&
                r.Right > rect.Left &&
                r.Left < rect.Right)
            {
                return r.Bottom - rect.Top;
            }
            return 0;
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(TextureManager.blankTexture, rect, Color.Purple);
        }
    }
}
