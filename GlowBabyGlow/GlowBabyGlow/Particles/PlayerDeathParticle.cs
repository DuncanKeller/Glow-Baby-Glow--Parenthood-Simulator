using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GlowBabyGlow
{
    class PlayerDeathParticle : Particle
    {
        Color c;
        float timer;
        Texture2D tex;

        public PlayerDeathParticle(Vector2 pos, Texture2D t)
            : base(pos)
        {
            this.tex = t;
            ySpeed = -400 * Config.screenR;
            xSpeed = 0;
            xRange = 0;
            yRange = 100;
            gravity = 800 * Config.screenR;
            rotSpeed = 0;
            c = Color.White;
            //c.A = (byte)10;

            maxLifetime = 10; // seconds
            life = maxLifetime;

            //rotSpeed = (float)(Math.PI - (Config.rand.NextDouble() * Math.PI * 2));
        }

        public override void Update(float dt)
        {
            timer += dt / 1000;

            if (timer > 0.5)
            {
                base.Update(dt);
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            Rectangle rect = new Rectangle((int)(pos.X), (int)(pos.Y), (int)tex.Width, (int)tex.Height);
            sb.Draw(tex, rect, new Rectangle(0, 0, tex.Width, tex.Height),
                c, angle, new Vector2(tex.Width / 2, tex.Height / 2), SpriteEffects.None, 0);
        }
    }
}
