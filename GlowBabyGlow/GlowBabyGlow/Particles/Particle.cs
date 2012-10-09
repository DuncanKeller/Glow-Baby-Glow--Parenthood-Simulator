using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GlowBabyGlow
{
    class Particle
    {
        protected float rotSpeed;
        protected int xRange;
        protected int yRange;
        protected float gravity;
        protected float ySpeed;
        protected float xSpeed;
        protected float maxLifetime;

        protected float angle;
        protected float life;
        protected Vector2 pos;

        public bool Alive
        {
            get { return life > 0; }
        }

        public Particle(Vector2 pos)
        {
            this.pos = pos;
        }

        public virtual void Update(float dt)
        {
            ySpeed += (gravity * (dt / 1000));
            pos.X += xSpeed * (dt / 1000);
            pos.Y += ySpeed * (dt / 1000);
            angle += rotSpeed * (dt / 1000);

            if (life > 0)
            {
                life -= dt / 1000;
            }
            else
            {
                life = 0;
            }

            if (pos.X > Config.screenW)
            {
                pos.X = 0;
            }
            else if (pos.X < 0)
            {
                pos.X = Config.screenW;
            }
        }

        public virtual void Draw(SpriteBatch sb)
        {
            sb.Draw(TextureManager.blankTexture, new Rectangle((int)pos.X, (int)pos.Y, 5, 5), Color.White);
        }
    }
}