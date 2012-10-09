using System;
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

        float speed = 450;
        Vector2 velocity;
        Vector2 pos;

        Player player;

        public Player Player
        {
            get { return player; }
        }

        public Rectangle Rect
        {
            get { return rect; }
        }

        public Vector2 Pos
        {
            get { return pos; }
        }

        public Vector2 Velocity
        {
            get { return velocity; }
        }

        public Bullet(Vector2 pos, int direction, Player p) : base()
        {
            player = p;
            this.pos = pos;
            rect = new Rectangle((int)pos.X, (int)pos.Y, width, height);
            velocity = new Vector2(direction * speed, 0);
        }

        public override void Update(float dt)
        {
            rect = new Rectangle((int)pos.X, (int)pos.Y, width, height);
            pos.X += velocity.X * (dt / 1000);
        }

        public void Collision(ref List<Tile> tiles)
        {
            foreach (Enemy e in World.EnemyManager.Enemies)
            {
                if (rect.Intersects(e.Rect))
                {
                    e.Hit(this);
                    World.BulletManager.RemoveBullet(this);
                }
            }
        }

        public override void Draw(SpriteBatch sb, SpriteEffects effect)
        {
            sb.Draw(TextureManager.blankTexture, rect, Color.White);
            base.Draw(sb, effect);
        }
    }
}
