using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GlowBabyGlow
{
    class DarkCloud : Entity
    {
        Texture2D texture;
        Vector2 pos = new Vector2();
        float speed = 0;
        int height;
        float timer;
        int amplitude = 15;
        float offset;

        public DarkCloud(int num, World w)
            : base(w)
        {
            int x = num * ((Config.screenW / 4) + Config.rand.Next(50)) + Config.screenW / 10;
            int y = Config.rand.Next(75);
            speed = (float)(Config.rand.NextDouble() * 5);
            offset = (float)Config.rand.NextDouble();

            texture = TextureManager.darkClouds[num];
            rect = new Rectangle(x, y, 
                (int)(texture.Width * Config.screenR), 
                (int)(texture.Height * Config.screenR));
            pos.X = x;
            height = y;
            pos.Y = y;
        }

        public override void Update(float dt)
        {
            base.Update(dt);
            timer += (dt / 2000) * speed;

            pos.Y = height + (float)Math.Sin(timer + offset) * amplitude;
                        rect.X = (int)pos.X;
            rect.Y = (int)pos.Y;
        }

        public override void Draw(SpriteBatch sb, SpriteEffects effect)
        {
            sb.Draw(texture, rect, Color.White);
        }
    }
}