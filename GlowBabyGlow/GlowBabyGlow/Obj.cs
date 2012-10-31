using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace GlowBabyGlow
{
    abstract class Obj
    {
        protected World w;

        public Obj(World w)
        {
            this.w = w;
        }

        public abstract void Draw(SpriteBatch sb, SpriteEffects effect);
    }
}
