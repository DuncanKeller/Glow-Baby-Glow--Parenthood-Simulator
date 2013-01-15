using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GlowBabyGlow
{
    class SpawnParticle : Particle
    {
        Color c;

        public SpawnParticle(Vector2 pos)
            : base(pos)
        {
            ySpeed = Config.rand.Next(120) - 60;
            xSpeed = Config.rand.Next(120) - 60;
            xRange = 100;
            yRange = 100;
            gravity = 0;
            rotSpeed = 0;
            c = Color.White;
            //c.A = (byte)10;

            maxLifetime = 0.3f + (float)(Config.rand.NextDouble() * .8f); // seconds
            life = maxLifetime;

            double angle = (Config.rand.NextDouble() * Math.PI * 2);

            //pos.X += (float)(Math.Cos(angle) * Coin.size);
            //pos.Y += (float)(Math.Sin(angle) * Coin.size);
        }

        public override void Draw(SpriteBatch sb)
        {
            Rectangle rect = new Rectangle((int)(pos.X - 1), (int)(pos.Y - 1), 2, 2);
            sb.Draw(TextureManager.blankTexture, rect, c);
        }
    }
}
