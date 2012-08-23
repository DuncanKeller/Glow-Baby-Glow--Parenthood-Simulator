using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GlowBabyGlow
{
    class Ladder : Tile
    {
     
        public Ladder(Point pos)
            : base(pos)
        {

        }

        public override void Draw(SpriteBatch sb) 
        {
            sb.Draw(TextureManager.blankTexture, rect, Color.Yellow);
        }

        public bool LadderBelow(Rectangle r)
        {
            Rectangle testRect = new Rectangle(
                r.Center.X - 1, r.Center.Y, 2, (r.Height / 2) + 5);
            if (testRect.Bottom > rect.Top &&
                testRect.Top < rect.Bottom &&
                testRect.Right > rect.Left &&
                testRect.Left < rect.Right)
            {
                return true;
            }
            return false;
        }

        public bool LadderAbove(Rectangle r)
        {
            Rectangle testRect = new Rectangle(
                r.Center.X - 1, r.Center.Y - (rect.Height / 2) - 5, 2, (r.Height / 2) + 5);
            if (testRect.Bottom > rect.Top &&
                testRect.Top < rect.Bottom &&
                testRect.Right > rect.Left &&
                testRect.Left < rect.Right)
            {
                return true;
            }
            return false;
        }
    }
}