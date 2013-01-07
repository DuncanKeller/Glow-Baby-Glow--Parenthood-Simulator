using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GlowBabyGlow
{
    class Cloud : Entity
    {
        Texture2D texture;
        Vector2 pos = new Vector2();
        float speed = 0;

        public Cloud(int num, World w) : base(w)
        {
            int x = Config.rand.Next(Config.screenW - 2) + 1;
            int y = Config.rand.Next(300) - 100;
            speed = (float)(Config.rand.NextDouble() * 5);

            texture = TextureManager.clouds[num];
            rect = new Rectangle(x, y, texture.Width / 2, texture.Height / 2);
            pos.X = x;
            pos.Y = y;
        }

        public override void Update(float dt)
        {
            base.Update(dt);

            pos.X += speed * dt / 1000;

            if (pos.X > Config.screenW)
            {
                pos.X = 0;
            }

            rect.X = (int)pos.X;
        }

        public override void Draw(SpriteBatch sb, SpriteEffects effect)
        {
            sb.Draw(texture, rect, Color.White);
            sb.Draw(texture, new Rectangle(rect.X - Config.screenW, rect.Y, rect.Width, rect.Height), Color.White);
        }
    }
}