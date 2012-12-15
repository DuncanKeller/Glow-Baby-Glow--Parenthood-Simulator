using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GlowBabyGlow
{
    class GlowParticle : Particle
    {
        Color c;
        int size;

        public GlowParticle(Vector2 pos)
            : base(pos)
        {
            ySpeed = -4 - Config.rand.Next(15);
            xSpeed = Config.rand.Next(10) - 5;
            xRange = 100;
            yRange = 100;
            gravity = 0;
            rotSpeed = 0;
            c = new Color(100, 255,
                Config.rand.Next(255));
            size = 10 + Config.rand.Next(30);
            size = (int)(size * 2 * Config.screenR);
            //c.A = (byte)10;

            maxLifetime = 2.6f + (float)(Config.rand.NextDouble() * 2.2); // seconds
            life = maxLifetime;

            double angle = 0;

            pos.X += (float)(Math.Cos(angle) * Coin.size);
            pos.Y += (float)(Math.Sin(angle) * Coin.size);
        }

        public override void Draw(SpriteBatch sb)
        {
            Rectangle rect = new Rectangle((int)(pos.X - (size / 2)), (int)(pos.Y - (size / 2)), size, size);
            sb.Draw(TextureManager.glowParticle, rect, c);
        }
    }
}
