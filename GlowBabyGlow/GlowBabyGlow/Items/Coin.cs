using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GlowBabyGlow
{
    class Coin : Entity
    {
        double offset = 0;
        float time;
        public static int size = (int)(60 * Config.screenR);
        public static int numParticles = 25;
        int magnitude = (int)(10 * Config.screenR);
        int startingHeight;

        public Rectangle Rect
        {
            get { return rect; }
        }

        public Coin(int x, int y, World w)
            : base(w)
        {
            rect = new Rectangle(
                x, y, size, size);
            startingHeight = rect.Y;
        }

        public override void Update(float dt)
        {
            time += dt / 500;
            rect.Y = startingHeight + (int)offset;
            offset = Math.Sin(time) * magnitude;
        }

        public override void Draw(SpriteBatch sb, SpriteEffects effect)
        {
            sb.Draw(TextureManager.coin, rect, Color.White);
            Rectangle wrapLeft = new Rectangle(rect.X - Config.screenW, rect.Y, rect.Width, rect.Height);
            Rectangle wrapRight = new Rectangle(rect.X + Config.screenW, rect.Y, rect.Width, rect.Height);
            sb.Draw(TextureManager.coin, wrapLeft, Color.White);
            sb.Draw(TextureManager.coin, wrapRight, Color.White);
        }
    }
}