using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Duncanimation;

namespace GlowBabyGlow
{
    class BossEnemy : Enemy
    {
        public BossEnemy(Point pos, World w)
            : base(pos, w)
        {
            health = 6;
            maxVeloc = 75;
            width = (int)(35 * 4 * Config.screenR);
            height = (int)(37 * 4 * Config.screenR);
            
            velocity.X = (int)(-maxVeloc * Config.screenR);
            idealVelocity.X = (int)(-maxVeloc * Config.screenR);
            testAnim = new Animator(TextureManager.zombieSheet, 2, 6);
            testAnim.AddAnimation("default", 0, 11, 15, true);
            testAnim.Play("default");

            rect = new Rectangle(pos.X, pos.Y, width, height);
            hitRect = new Rectangle(rect.X, rect.Y, rect.Width / 2, rect.Height);
            hitOffset = new Point(rect.Width / 4, 0);
        }
    }
}
