using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GlowBabyGlow
{
    abstract class Actor : Entity
    {
        protected Vector2 pos;
        protected float gravity = 50;
        protected Vector2 velocity = new Vector2();

        protected bool inAir = true;
        protected bool wallLeft = false;
        protected bool wallRight = false;

        protected bool onLadder = false;

        public Vector2 Position
        {
            get { return pos; }
        }

        public override void Update(float dt)
        {
            if (inAir && !onLadder)
            {
                velocity.Y += gravity;
            }

            pos.Y += velocity.Y * (dt / 1000);
            pos.X += velocity.X * (dt / 1000);

            rect.X = (int)pos.X; 
            rect.Y = (int)pos.Y;

            if (rect.Left > Config.screenW)
            {
                rect.X = -rect.Width + 1;
            }
            else if (rect.Right < 0)
            {
                rect.X = Config.screenW - 1;
            }
        }

        

        public override void Draw(SpriteBatch sb)
        {
            //remember to draw things so that screen wrapping looks OK

            base.Draw(sb);
        }
    }
}
