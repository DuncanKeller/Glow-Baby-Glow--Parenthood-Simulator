﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GlowBabyGlow
{
    class Bullet : Entity
    {
        static int width = 6;
        static int height = 4;

        float speed;
        Vector2 velocity;
        Rectangle rect;

        public Vector2 Velocity
        {
            get { return velocity; }
        }

        public Bullet(Vector2 pos, int direction) : base()
        {
            rect = new Rectangle((int)pos.X, (int)pos.Y, width, height);
            velocity = new Vector2(direction * speed, 0);
        }

        public override void Update(float dt)
        {
            rect.X += (int)velocity.X;
        }

        public void Collision(ref List<Tile> tiles)
        {
            foreach (Enemy e in World.EnemyManager.Enemies)
            {
                if (rect.Intersects(e.Rect))
                {
                    e.Hit(this);
                }
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
        }
    }
}