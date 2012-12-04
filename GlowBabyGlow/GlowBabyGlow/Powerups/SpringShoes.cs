using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GlowBabyGlow
{
    class SpringShoes : Powerup
    {
        public SpringShoes(int x, int y, World w)
            : base(x, y, w)
        {
            texture = TextureManager.pupIconSpringshoes;
            icon = TextureManager.pupIconSpringshoes;
            description = "air gordans";
        }

        public override void Update(float dt)
        {
            base.Update(dt);
        }
    }
}
