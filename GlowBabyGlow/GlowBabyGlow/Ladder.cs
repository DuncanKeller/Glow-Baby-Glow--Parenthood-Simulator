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
     
        public Ladder(Point pos, World w)
            : base(pos, w)
        {

        }

        public override void Update(float dt)
        {
            base.Update(dt);
        }

        public override void Draw(SpriteBatch sb, SpriteEffects effect) 
        {
            //sb.Draw(TextureManager.ladder, new Rectangle(rect.X - 2, rect.Y - 2, rect.Width, rect.Height), new Color(0, 0, 0, 100));
            sb.Draw(TextureManager.ladder, rect, Color.White);
            Rectangle wrapLeft = new Rectangle(rect.X - Config.screenW, rect.Y, rect.Width, rect.Height);
            Rectangle wrapRight = new Rectangle(rect.X + Config.screenW, rect.Y, rect.Width, rect.Height);
            sb.Draw(TextureManager.ladder, wrapLeft, Color.White);
            sb.Draw(TextureManager.ladder, wrapRight, Color.White);
        }

        public bool LadderBelow(Rectangle r)
        {
            Rectangle testRect = new Rectangle(
                r.Center.X - 1, r.Bottom + 2, 2, (r.Height / 2));
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