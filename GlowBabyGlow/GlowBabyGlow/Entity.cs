using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GlowBabyGlow
{
    abstract class Entity
    {
        protected Rectangle rect;

        public abstract void Update(float dt);
        public abstract void Draw(SpriteBatch sb);
    }
}
