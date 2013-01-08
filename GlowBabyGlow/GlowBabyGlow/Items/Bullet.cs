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
        static int width = (int)(6 * Config.screenR);
        static int height = (int)(4 * Config.screenR);

        float speed = (int)(900 * Config.screenR);
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

        public Bullet(Vector2 pos, int direction, Player p) : base(p.World)
        {
            player = p;
            this.pos = pos;
            rect = new Rectangle((int)pos.X, (int)pos.Y,
                (int)(width * Config.screenR),
                (int)(height * Config.screenR));
            velocity = new Vector2(direction * speed, 0);
        }

        public override void Update(float dt)
        {
            rect = new Rectangle((int)pos.X, (int)pos.Y, width, height);
            pos.X += velocity.X * (dt / 1000);
        }

        public void Collision(ref List<Tile> tiles)
        {
            foreach (Enemy e in w.EnemyManager.Enemies)
            {
                if (rect.Intersects(e.Rect))
                {
                    e.Hit(this);
                    if (!(player.Powerup is PiercingShot))
                    {
                        w.BulletManager.RemoveBullet(this);
                    }
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
