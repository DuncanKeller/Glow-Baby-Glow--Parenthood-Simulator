using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GlowBabyGlow
{
    class BulletManager
    {
        List<Bullet> bullets = new List<Bullet>();

        public List<Bullet> Bullets
        {
            get { return bullets; }
        }

        public void ClearBullets()
        {
            bullets.Clear();
        }

        public void Update(float dt)
        {
            foreach (Bullet b in bullets)
            {
                b.Update(dt);
            }
        }

        public void Collision(ref List<Tile> tiles, ref List<Ladder> ladders)
        {
            foreach (Bullet b in bullets)
            {
                b.Collision(ref tiles);
            }
        }

        public void Draw(SpriteBatch sb)
        {
            foreach (Bullet b in bullets)
            {
                b.Draw(sb);
            }
        }
    }
}
