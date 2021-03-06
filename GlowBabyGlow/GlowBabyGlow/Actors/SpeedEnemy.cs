﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Duncanimation;

namespace GlowBabyGlow
{
    class SpeedEnemy : Enemy
    {
        public SpeedEnemy(Point pos, World w)
            : base(pos, w)
        {
            maxVeloc = 220;
            velocity.X = (int)(-maxVeloc * Config.screenR);
            idealVelocity.X = (int)(-maxVeloc * Config.screenR);
            testAnim = new Animator(TextureManager.zombieSpeedy, 2, 6);
            testAnim.AddAnimation("default", 0, 11, 25, true);
            testAnim.Play("default");
        }
    }
}