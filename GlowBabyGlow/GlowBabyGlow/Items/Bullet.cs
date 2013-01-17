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
            // draw tail
            int trailLength = (int)(75 * Config.screenR);
            int trailLength2 = trailLength / 2;
            int trailLength3 = trailLength2 / 2;
            int a = 150;

            if(velocity.X > 0)
            {
                if(rect.X - player.HitRect.Right < trailLength)
                {
                    trailLength = rect.X - player.HitRect.Right;
                    trailLength2 = 0;
                    trailLength3 = 0;
                }

                sb.Draw(TextureManager.blankTexture, new Rectangle(rect.Left - trailLength, rect.Center.Y, trailLength, 1),
                    new Color(a, a, a, a));
                sb.Draw(TextureManager.blankTexture, new Rectangle((rect.Left - trailLength) - trailLength2, rect.Center.Y, trailLength2, 1),
                    new Color(a / 2, a / 2, a / 2, a / 2));
                sb.Draw(TextureManager.blankTexture, new Rectangle(((rect.Left - trailLength) - trailLength2) - trailLength3, rect.Center.Y, trailLength3, 1),
                    new Color(a / 4, a / 4, a / 4, a / 4));
            }
            else
            {
                if(player.HitRect.Left - rect.Right < trailLength)
                {
                    trailLength = player.HitRect.Left - rect.Right;
                    trailLength2 = 0;
                    trailLength3 = 0;
                }

                sb.Draw(TextureManager.blankTexture, new Rectangle(rect.Right, rect.Center.Y, trailLength, 1),
                    new Color(a, a, a, a));
                sb.Draw(TextureManager.blankTexture, new Rectangle(rect.Right + trailLength, rect.Center.Y, trailLength2, 1),
                   new Color(a / 2, a / 2, a / 2, a / 2));
                sb.Draw(TextureManager.blankTexture, new Rectangle(rect.Right + trailLength + trailLength2, rect.Center.Y, trailLength3, 1),
                   new Color(a / 4, a / 4, a / 4, a / 4));
            }

            Rectangle r2 = new Rectangle(rect.X - 1, rect.Y - 1, rect.Width + 2, rect.Height + 2);
            sb.Draw(TextureManager.blankTexture, r2, Color.Black);
            sb.Draw(TextureManager.blankTexture, rect, Color.White);

            base.Draw(sb, effect);
        }
    }
}
