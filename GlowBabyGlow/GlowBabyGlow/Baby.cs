using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GlowBabyGlow
{
    class Baby : Actor
    {
        int index = 0;
        static int width = 10;
        static int height = 17;
        static float rotSpeed;
        float angle;

        float closestTile;

        float catchTimer = 0.25f;

        #region Properties


        public float ClosestTile
        {
            get { return closestTile; }
        }

        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        public Rectangle Rect
        {
            get { return rect; }
        }

        public bool ReadyToCatch
        {
            get { return catchTimer == 0; }
        }

        #endregion

        public Baby(Vector2 pos, float xVel, int index, World w) : base(w)
        {
            gravity = 175;
            this.pos = pos;
            rect = new Rectangle((int)pos.X, (int)pos.Y, width, height);
            velocity.Y = -175;
            velocity.X = xVel;
            rotSpeed = (float)((Math.PI) -(Config.rand.NextDouble() * Math.PI * 2));
            this.index = index;
        }

        public override void Update(float dt)
        {
            if (catchTimer > 0)
            {
                catchTimer -= dt / 1000;
            }
            else
            {
                catchTimer = 0;
            }
            angle += rotSpeed * (dt / 1000);

            base.Update(dt);
        }

        public void Collision(ref List<Tile> tiles)
        {
            closestTile = float.MaxValue;
            foreach (Tile t in tiles)
            {
                if (t.Rect.Y > pos.Y)
                {
                    float distance = Vector2.Distance(
                        new Vector2(t.Rect.Center.X, t.Rect.Center.Y), pos);
                    if (distance < closestTile)
                    {
                        closestTile = distance;
                    }
                }

                if (Rect.Intersects(t.Rect))
                {    
                    w.Explode();
                }
            }

            foreach (Bullet b in w.BulletManager.Bullets)
            {
                if (hitRect.Intersects(b.Rect))
                {
                    w.Explode();
                }
            }
        }

        public override void Draw(SpriteBatch sb, SpriteEffects effect)
        {

            if (w.Exploding)
            {
                if (w.ExplodeTimer < 1)
                {
                    Rectangle srcRect = new Rectangle((int)rect.Center.X - 75, (int)rect.Center.Y - 75, 150, 150);
                    sb.Draw(TextureManager.blackCircle, srcRect, Color.White);
                    sb.Draw(TextureManager.blankTexture, new Rectangle(0, 0, Config.screenW, srcRect.Top), Color.Black); // top
                    sb.Draw(TextureManager.blankTexture, new Rectangle(0, srcRect.Top, srcRect.X, srcRect.Bottom - srcRect.Top), Color.Black); // left
                    sb.Draw(TextureManager.blankTexture, new Rectangle(srcRect.Right, srcRect.Top, Config.screenW - srcRect.Right, srcRect.Bottom - srcRect.Top), Color.Black); // right
                    sb.Draw(TextureManager.blankTexture, new Rectangle(0, srcRect.Bottom, Config.screenW, Config.screenH - srcRect.Bottom), Color.Black);
                }
            }

            Rectangle wrapLeft = new Rectangle(rect.X - Config.screenW, rect.Y, rect.Width, rect.Height);
            Rectangle wrapRight = new Rectangle(rect.X + Config.screenW, rect.Y, rect.Width, rect.Height);

            if (w.ExplodeTimer < 1)
            {
                sb.Draw(TextureManager.baby, rect, new Rectangle(0, 0, TextureManager.baby.Width, TextureManager.baby.Height),
                   Color.White, angle, new Vector2(TextureManager.baby.Width / 2, TextureManager.baby.Height / 2),
                   SpriteEffects.None, 0);
                sb.Draw(TextureManager.baby, wrapLeft, new Rectangle(0, 0, TextureManager.baby.Width, TextureManager.baby.Height),
                    Color.White, angle, new Vector2(TextureManager.baby.Width / 2, TextureManager.baby.Height / 2),
                    SpriteEffects.None, 0);
                sb.Draw(TextureManager.baby, wrapRight, new Rectangle(0, 0, TextureManager.baby.Width, TextureManager.baby.Height),
                    Color.White, angle, new Vector2(TextureManager.baby.Width / 2, TextureManager.baby.Height / 2),
                    SpriteEffects.None, 0);
                base.Draw(sb, SpriteEffects.None);
            }
        }
    }
}