using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GlowBabyGlow
{
    class SpeedShoes : Powerup
    {
        public SpeedShoes(int x, int y, World w)
            : base(x, y, w)
        {
            texture = TextureManager.pupIconSpeedshoes;
            icon = TextureManager.pupIconSpeedshoes;
            description = "speed up";
            color = Color.Red;
        }

        public override void Update(float dt)
        {
            base.Update(dt);
        }
    }
}
