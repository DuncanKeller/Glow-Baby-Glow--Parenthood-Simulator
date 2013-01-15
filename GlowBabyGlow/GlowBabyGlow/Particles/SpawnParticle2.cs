using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GlowBabyGlow
{
    class SpawnParticle2 : Particle
    {
        Color c;

        int size;

        public SpawnParticle2(Vector2 pos)
            : base(pos)
        {
            ySpeed = -100 - Config.rand.Next(100);
            xSpeed = Config.rand.Next(150) - 75;
            xRange = 100;
            yRange = 100;
            gravity = 150;
            rotSpeed = 0;
            c = new Color(
                Config.rand.Next(255),
                Config.rand.Next(255),
                Config.rand.Next(255),
                100);
            //c.A = (byte)10;

            maxLifetime = 0.8f + (float)(Config.rand.NextDouble() * 1.2); // seconds
            life = maxLifetime;

            size = (int)(5 * Config.screenR) + Config.rand.Next(8);

            //double angle = (Config.rand.NextDouble() * Math.PI * 2);
        }

        public override void Draw(SpriteBatch sb)
        {
            Rectangle rect = new Rectangle((int)(pos.X - (size / 2)), (int)(pos.Y - (size / 2)), size, size);
            sb.Draw(TextureManager.babyGlow, rect, c);
        }
    }
}
