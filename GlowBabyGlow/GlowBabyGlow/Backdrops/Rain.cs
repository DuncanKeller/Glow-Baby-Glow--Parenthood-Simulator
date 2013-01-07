using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GlowBabyGlow
{
    class Rain : Entity
    {
        Texture2D texture;
        Vector2 pos = new Vector2();
        float speed = 0;
        Color c;

        public Rain(World w)
            : base(w)
        {
            int x = Config.rand.Next(Config.screenW * 2); 
            int y = Config.rand.Next(Config.screenH);
            speed = (float)(Config.rand.NextDouble() * 400) + 200;

            texture = TextureManager.rain;
            rect = new Rectangle(x, y, texture.Width, texture.Height);
            pos.X = x;
            pos.Y = y;
            float blue = (float)(Config.rand.NextDouble() / 4) + .25f;
            c = new Color(0, 0, blue);
        }

        public override void Update(float dt)
        {
            base.Update(dt);

            pos.X -= speed * dt / 1000;
            pos.Y += speed * dt / 1000;

            if (pos.Y > Config.screenH)
            {
                pos.Y = 0;
                pos.X = Config.rand.Next(Config.screenW * 2);
            }

            rect.X = (int)pos.X;
            rect.Y = (int)pos.Y;
        }

        public override void Draw(SpriteBatch sb, SpriteEffects effect)
        {
            sb.Draw(texture, rect, c);
            
        }
    }
}