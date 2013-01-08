using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GlowBabyGlow
{
    class Pacifier : Powerup
    {
        public Pacifier(int x, int y, World w)
            : base(x, y, w)
        {
            texture = TextureManager.blackCircle;
            icon = TextureManager.pupIconPacifier;
            description = "shake break";
            color = Color.LightBlue;
        }

        public override void Update(float dt)
        {
            base.Update(dt);
        }
    }
}
