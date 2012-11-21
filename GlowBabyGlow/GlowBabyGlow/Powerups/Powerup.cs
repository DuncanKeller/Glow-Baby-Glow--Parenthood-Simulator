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

        float maxAlive = 12;
        bool active = false;
        float aliveTimer = 0;
        protected Player p;

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
            rect = new Rectangle(x, y, 20, 20);
            startingHeight = y;
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

            base.Update(dt);
        }

        public override void Draw(SpriteBatch sb, SpriteEffects effect)
        {
            sb.Draw(texture, rect, Color.White);
            base.Draw(sb, effect);
        }
    }
}
