using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GlowBabyGlow
{
    class SparkParticle : Particle
    {
        Color c;
        float size;

        public SparkParticle(Vector2 pos, float dir)
            : base(pos)
        {
            ySpeed = -50 + Config.rand.Next(50);
            xSpeed = dir * 100 - Config.rand.Next(50);
            xRange = 100;
            yRange = 100;
            gravity = 150;
            rotSpeed = 0;

            //c.A = (byte)10;

            maxLifetime = 0.62f + (float)(Config.rand.NextDouble() * 0.5); // seconds
            life = maxLifetime;

            rotSpeed = (float)(Math.PI - (Config.rand.NextDouble() * Math.PI * 2));

            switch (Config.rand.Next(5))
            {
                case 0:
                    c = Color.Yellow;
                    break;
                case 1:
                    c = Color.Gold;
                    break;
                case 2:
                    c = Color.LightYellow;
                    break;
                default:
                    c = Color.White;
                    break;
            }

            size = 1;
        }

        public override void Draw(SpriteBatch sb)
        {
            Rectangle rect = new Rectangle((int)(pos.X - (size / 2)), (int)(pos.Y - (size / 2)), (int)size, (int)size);
            sb.Draw(TextureManager.blankTexture, rect, new Rectangle(0, 0, TextureManager.blankTexture.Width, TextureManager.blankTexture.Height),
                c, angle, new Vector2(TextureManager.blankTexture.Width / 2, TextureManager.blankTexture.Height / 2), SpriteEffects.None, 0);
        }
    }
}
