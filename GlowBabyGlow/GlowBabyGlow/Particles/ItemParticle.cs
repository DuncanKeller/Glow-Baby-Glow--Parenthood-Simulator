using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GlowBabyGlow
{
    class ItemParticle : Particle
    {
        Color c;

        int size = (int)(15 * Config.screenR);

        public ItemParticle(Vector2 pos, Color c)
            : base(pos)
        {
            ySpeed = Config.rand.Next(24) - 12;
            xSpeed = Config.rand.Next(24) - 12;
            xRange = 100;
            yRange = 100;
            gravity = 0;
            rotSpeed = 0;
            this.c = c;
            //c.A = (byte)10;

            maxLifetime = 0.8f + (float)(Config.rand.NextDouble() * 1.2); // seconds
            life = maxLifetime;

            //double angle = (Config.rand.NextDouble() * Math.PI * 2);

            pos.X += (float)(Math.Cos(angle) * Coin.size);
            pos.Y += (float)(Math.Sin(angle) * Coin.size);
        }

        public override void Draw(SpriteBatch sb)
        {
            Rectangle rect = new Rectangle((int)(pos.X - (size / 2)), (int)(pos.Y - (size / 2)), size, size);
            sb.Draw(TextureManager.babyGlow, rect, c);
        }
    }
}
