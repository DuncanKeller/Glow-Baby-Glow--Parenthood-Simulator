using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GlowBabyGlow
{
    class SpringParticle : Particle
    {
        Color c;
        float size;

        public SpringParticle(Vector2 pos)
            : base(pos)
        {
            ySpeed = -50 + Config.rand.Next(50);
            xSpeed = 100 - Config.rand.Next(200);
            xRange = 100;
            yRange = 100;
            gravity = 50;
            rotSpeed = 0;

            //c.A = (byte)10;

            maxLifetime = 0.62f + (float)(Config.rand.NextDouble() * 0.5); // seconds
            life = maxLifetime;

            rotSpeed = (float)(Math.PI - (Config.rand.NextDouble() * Math.PI * 2));

            switch (Config.rand.Next(5))
            {
                case 0:
                    c = Color.Red;
                    break;
                case 1:
                    c = Color.Orange;
                    break;
                case 2:
                    c = Color.Green;
                    break;
                case 3:
                    c = Color.Purple;
                    break;
                default:
                    c = Color.Blue;
                    break;
            }

            size = 1 + Config.rand.Next(2);
        }

        public override void Draw(SpriteBatch sb)
        {
            Rectangle rect = new Rectangle((int)(pos.X - (size / 2)), (int)(pos.Y - (size / 2)), (int)size, (int)size);
            sb.Draw(TextureManager.blankTexture, rect, new Rectangle(0, 0, TextureManager.blankTexture.Width, TextureManager.blankTexture.Height),
                c, angle, new Vector2(TextureManager.blankTexture.Width / 2, TextureManager.blankTexture.Height / 2), SpriteEffects.None, 0);
        }
    }
}
