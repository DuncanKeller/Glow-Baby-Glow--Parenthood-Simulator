using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GlowBabyGlow
{
    class GoldParticle : Particle
    {
        Color c;

        public GoldParticle(Vector2 pos)
            : base(pos)
        {
            ySpeed = -50 - Config.rand.Next(75);
            xSpeed = Config.rand.Next(100) - 50;
            xRange = 100;
            yRange = 100;
            gravity = 150;
            rotSpeed = 0;
            c = new Color(255, 255,
                Config.rand.Next(255));
            //c.A = (byte)10;

            maxLifetime = 0.6f + (float)(Config.rand.NextDouble() * 1.2); // seconds
            life = maxLifetime;

            double angle = (Config.rand.NextDouble() * Math.PI * 2);

            pos.X += (float)(Math.Cos(angle) * Coin.size);
            pos.Y += (float)(Math.Sin(angle) * Coin.size);
        }

        public override void Draw(SpriteBatch sb)
        {
            Rectangle rect = new Rectangle((int)(pos.X - 1), (int)(pos.Y - 1), 2, 2);
            sb.Draw(TextureManager.blankTexture, rect, c);
        }
    }
}
