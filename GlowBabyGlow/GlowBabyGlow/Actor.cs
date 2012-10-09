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
        protected float gravity = 2000;
        protected Vector2 velocity = new Vector2();

        protected bool inAir = true;
        protected bool wallLeft = false;
        protected bool wallRight = false;

        protected bool onLadder = false;

        protected Rectangle hitRect;
        protected Point hitOffset;

        public Vector2 Position
        {
            get { return pos; }
        }

        public Rectangle HitRect
        {
            get { return hitRect; }
        }

        public override void Update(float dt)
        {
            if (inAir && !onLadder)
            {
                velocity.Y += gravity * (dt / 1000);
            }

            pos.Y += velocity.Y * (dt / 1000);
            pos.X += velocity.X * (dt / 1000);

            rect.X = (int)pos.X; 
            rect.Y = (int)pos.Y;

            if (rect.Center.X > Config.screenW)
            {
                pos.X = -rect.Width + rect.Width / 2;
            }
            else if (rect.Center.X < 0)
            {
                pos.X = Config.screenW - rect.Width / 2;
            }

            hitRect.X = rect.X + hitOffset.X;
            hitRect.Y = rect.Y + hitOffset.Y;
        }

        public override void Draw(SpriteBatch sb, SpriteEffects effect)
        {
            //remember to draw things so that screen wrapping looks OK
            if (testAnim != null)
            {
                testAnim.Draw(sb, rect, Color.White, 0, Vector2.Zero, effect);
                Rectangle wrapLeft = new Rectangle(rect.X - Config.screenW, rect.Y, rect.Width, rect.Height);
                Rectangle wrapRight = new Rectangle(rect.X + Config.screenW, rect.Y, rect.Width, rect.Height);
                testAnim.Draw(sb, wrapLeft, Color.White, 0, Vector2.Zero, effect);
                testAnim.Draw(sb, wrapRight, Color.White, 0, Vector2.Zero, effect);
            }
            base.Draw(sb, effect);
        }
    }
}
