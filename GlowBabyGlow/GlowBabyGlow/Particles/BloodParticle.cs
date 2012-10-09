using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GlowBabyGlow
{
    class BloodParticle : Particle
    {
        Color c;
        float size;

        public BloodParticle(Vector2 pos, float dir)
            : base(pos)
        {
            ySpeed = -Config.rand.Next(125);
            xSpeed = Config.rand.Next(100) * dir;
            xRange = 100;
            yRange = 100;
            gravity = 150;
            rotSpeed = 0;
            
            //c.A = (byte)10;

            maxLifetime = 0.62f + (float)(Config.rand.NextDouble() * 0.5); // seconds
            life = maxLifetime;

            //rotSpeed = (float)( Math.PI - (Config.rand.NextDouble() * Math.PI * 2) );

            switch (Config.rand.Next(5))
            {
                case 0:
                    c = Color.Red;
                    break;
                case 1:
                    c = Color.DarkRed;
                    break;
                default:
                    c = Color.Red;
                    break;
            }

            size = (float)Config.rand.NextDouble() * 4;
        }

        public override void Draw(SpriteBatch sb)
        {
            Rectangle rect = new Rectangle((int)(pos.X - (size / 2)), (int)(pos.Y - (size / 2)), (int)size, (int)size);
            sb.Draw(TextureManager.blankTexture, rect, new Rectangle(0, 0, TextureManager.blankTexture.Width, TextureManager.blankTexture.Height),
                c, angle, new Vector2(TextureManager.blankTexture.Width / 2, TextureManager.blankTexture.Height / 2), SpriteEffects.None, 0);
        }
    }
}
