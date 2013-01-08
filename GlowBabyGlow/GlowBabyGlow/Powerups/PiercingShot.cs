using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GlowBabyGlow
{
    class PiercingShot : Powerup
    {
        public PiercingShot(int x, int y, World w)
            : base(x, y, w)
        {
            texture = TextureManager.pupIconArrow;
            icon = TextureManager.pupIconArrow;
            description = "piercing";
            color = Color.Yellow;
        }

        public override void Update(float dt)
        {
            base.Update(dt);
        }
    }
}
