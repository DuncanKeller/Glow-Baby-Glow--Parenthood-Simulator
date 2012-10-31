using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GlowBabyGlow
{
    class GooParticle : Particle
    {
        Color c;
        float size;

        public GooParticle(Vector2 pos)
            : base(pos)
        {
            ySpeed = Config.rand.Next(125) + 100;
            xSpeed = Config.rand.Next(200) - 100;
            xRange = 100;
            yRange = 100;
            gravity = 50;
            rotSpeed = 0;

            //c.A = (byte)10;

            maxLifetime = 0.62f + (float)(Config.rand.NextDouble() * 0.5); // seconds
            life = maxLifetime;

            //rotSpeed = (float)( Math.PI - (Config.rand.NextDouble() * Math.PI * 2) );

            switch (Config.rand.Next(3))
            {
                case 0:
                    c = Color.Green;
                    break;
                case 1:
                    c = Color.DarkGreen;
                    break;
                default:
                    c = Color.LimeGreen;
                    break;
            }

            c.A = (byte)100;

            size = 5 + (float)Config.rand.NextDouble() * 10;
        }

        public override void Draw(SpriteBatch sb)
        {
            Rectangle rect = new Rectangle((int)(pos.X - (size / 2)), (int)(pos.Y - (size / 2)), (int)size, (int)size);
            sb.Draw(TextureManager.babyGlow, rect, new Rectangle(0, 0, TextureManager.babyGlow.Width, TextureManager.babyGlow.Height),
                c, angle, new Vector2(TextureManager.blankTexture.Width / 2, TextureManager.blankTexture.Height / 2), SpriteEffects.None, 0);
        }
    }
}