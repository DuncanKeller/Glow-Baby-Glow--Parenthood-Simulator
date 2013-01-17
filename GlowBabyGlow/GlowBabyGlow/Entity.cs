using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Duncanimation;

namespace GlowBabyGlow
{
    abstract class Entity : Obj
    {
        protected Rectangle rect;
        protected Animator testAnim;

        protected bool sploded = false;
        Vector2 explodeVelocity = new Vector2();
        Vector2 newPos = new Vector2();

        public Entity(World w)
            : base(w)
        {

        }

        public virtual void Explode()
        {
            if (!sploded)
            {
                if (!(this is Enemy))
                {
                    newPos.X = rect.X;
                    newPos.Y = rect.Y;
                    sploded = true;
                    explodeVelocity = new Vector2(0, 0);
                    explodeVelocity.X += (float)(Config.rand.NextDouble() * 500) - 250;
                    explodeVelocity.Y += (float)(Config.rand.NextDouble() * 500) - 500;
                }
            }
        }

        public virtual void Update(float dt)
        {
            if (sploded)
            {
                explodeVelocity.Y += 700 * (dt / 1000);
                newPos.X += (explodeVelocity.X * (dt / 1000));
                newPos.Y += (explodeVelocity.Y * (dt / 1000));
                rect.X = (int)newPos.X;
                rect.Y = (int)newPos.Y;
            }
        }

        public override void Draw(SpriteBatch sb, SpriteEffects effect)
        {
            
        }
        
    }
}