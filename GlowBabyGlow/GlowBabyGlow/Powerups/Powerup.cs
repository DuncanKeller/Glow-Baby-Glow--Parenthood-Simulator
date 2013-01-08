using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GlowBabyGlow
{
    abstract class Powerup : Entity
    {
        double offset = 0;
        float time;
        int magnitude = 5;
        int startingHeight;
        protected Texture2D texture;
        protected Texture2D icon;
        protected string description;

        float maxAlive = 20;
        bool active = false;
        float aliveTimer = 0;
        protected Player p;

        float size = 20 * Config.screenR;

        protected Color color;
        float particleTimer = 0;
        float particleTime = 0.4f;

        List<Particle> particles = new List<Particle>();

        public Rectangle Rect
        {
            get { return rect; }
        }

        public Texture2D Icon
        {
            get { return icon; }
        }

        public string Description
        {
            get { return description; }
        }

        public void Activate(Player p)
        {
            aliveTimer = maxAlive;
            active = true;
            this.p = p;
        }

        public bool Active
        {
            get { return (aliveTimer > 0); }
        }

        public float MaxAliveTime
        {
            get { return maxAlive; }
        }

        public float AliveTimer
        {
            get { return aliveTimer; }
        }

        public Powerup(int x, int y, World w) : base(w)
        {
            rect = new Rectangle(x, y - (int)(size * 2f), (int)size, (int)size);
            startingHeight = rect.Y;
        }

        public override void Update(float dt)
        {
            time += dt / 500;
            rect.Y = startingHeight + (int)offset;
            offset = Math.Sin(time) * magnitude;

            if (active)
            {
                aliveTimer -= dt / 1000;
            }

            if (particleTimer >= particleTime)
            {
                particles.Add(new ItemParticle(
                    new Vector2(rect.Center.X, rect.Center.Y),
                    color));
                particleTime = 0;
            }
            else
            {
                particleTimer += dt / 1000;
            }

            List<Particle> remove = new List<Particle>();

            foreach (Particle p in particles)
            {
                p.Update(dt);
                if (!p.Alive)
                {
                    remove.Add(p);
                }
            }

            foreach (Particle p in remove)
            {
                particles.Remove(p);
            }

            base.Update(dt);
        }

        public override void Draw(SpriteBatch sb, SpriteEffects effect)
        {
            foreach (Particle p in particles)
            {
                p.Draw(sb);
            }
            sb.Draw(TextureManager.babyGlow, rect, color);
            sb.Draw(texture, rect, Color.White);
            base.Draw(sb, effect);
        }
    }
}
