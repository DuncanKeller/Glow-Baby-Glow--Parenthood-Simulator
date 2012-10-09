using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace GlowBabyGlow
{
    abstract class Obj
    {
        public abstract void Draw(SpriteBatch sb, SpriteEffects effect);
    }
}
