using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Duncanimation;

namespace GlowBabyGlow
{
    class FatEnemy : Enemy
    {
        public FatEnemy(Point pos, World w)
            : base(pos, w)
        {
            health = 3;
            maxVeloc = 40;
            velocity.X = (int)(-maxVeloc * Config.screenR);
            idealVelocity.X = (int)(-maxVeloc * Config.screenR);
            testAnim = new Animator(TextureManager.zombieFat, 2, 6);
            testAnim.AddAnimation("default", 0, 11, 15, true);
            testAnim.Play("default");
        }

    }
}
