using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Duncanimation;

namespace GlowBabyGlow
{
    class Enemy : Actor
    {
        bool movingRight = true;
        public static int width = 35;
        public static int height = 37;

        int health = 3;

        Animator animation;

        Vector2 idealVelocity = new Vector2();

        public Rectangle Rect
        {
            get { return rect; }
        }

        public Enemy(Point pos)
        {
            this.pos = new Vector2(pos.X, pos.Y);
            rect = new Rectangle(pos.X, pos.Y, width, height);
            hitRect = new Rectangle(rect.X, rect.Y, rect.Width / 2, rect.Height);
            hitOffset = new Point(rect.Width / 4, 0);
            
            animation = new Animator(TextureManager.zombieSheet, 2, 6);
            animation.AddAnimation("default", 0, 11, 15, true);
            animation.Play("default");
            velocity.X = -50;
            idealVelocity.X = -50;
        }

        public override void Update(float dt)
        {
            base.Update(dt);
            
            animation.Update(dt);

            if (health <= 0)
            {
                World.EnemyManager.Remove(this);
            }

            if (velocity.X > idealVelocity.X)
            {
                velocity.X -= (velocity.X - idealVelocity.X) / 5;
            }
            else if (velocity.X < idealVelocity.X)
            {
                velocity.X += (idealVelocity.X - velocity.X) / 8;
            }
        }

        public void Hit(Bullet b)
        {
            health--;
            velocity.X += b.Velocity.X / 3.5f;
        }

        public void Die()
        {

        }

        public void Collision(ref List<Tile> tiles, ref List<Ladder> ladders)
        {
            bool fall = true;
            bool tileCollideLeft = false;
            bool tileCollideRight = false;
            bool floorBelow = false;
            int floorPos = velocity.X > 0 ? rect.Width : -rect.Width;
            Rectangle checkFloor = new Rectangle((int)rect.Center.X + floorPos, rect.Bottom + 5, 1, 1);

            // air collision, landing, walls, etc
            foreach (Tile t in tiles)
            {
                if (inAir)
                {
                    int overlappingAbove = t.OverlappingAbove(hitRect);
                    if (overlappingAbove > 0 && velocity.Y > 0)
                    {
                        inAir = false;
                        velocity.Y = 0;
                        if (!onLadder)
                        {
                            pos.Y -= overlappingAbove;
                        }
                        onLadder = false;
                        return;
                    }
                }
                if (!inAir)
                {
                    if (t.StandingOn(hitRect))
                    {
                        fall = false;
                    }

                    if (checkFloor.Intersects(t.Rect))
                    {
                        floorBelow = true;
                    }
                }
                if (velocity.X < 0)
                {
                    int overlappingRight = t.OverlappingRight(hitRect);
                    if (overlappingRight > 0)
                    {
                        velocity.X = 0;

                        if (!wallLeft)
                        {
                            pos.X = t.Rect.Right;
                            velocity.X = -velocity.X;
                            idealVelocity.X = -idealVelocity.X;
                            tileCollideLeft = true;
                        }
                    }
                }
                if (velocity.X > 0)
                {
                    int overlappingLeft = t.OverlappingLeft(hitRect);
                    if (overlappingLeft > 0)
                    {
                        velocity.X = 0;

                        if (!wallRight)
                        {
                            pos.X = t.Rect.Left - rect.Width;
                            velocity.X = -velocity.X;
                            idealVelocity.X = -idealVelocity.X;
                            tileCollideRight = true;
                        }
                    }
                }
            }

            if (fall)
            {
                inAir = true;
            }
            if (!floorBelow)
            {
                // prevent falling off ledges
                velocity.X = -velocity.X;
                idealVelocity.X = -idealVelocity.X;
            }

            wallRight = tileCollideRight;
            wallLeft = tileCollideLeft;
        }

        public override void Draw(SpriteBatch sb)
        {
            int floorPos = velocity.X > 0 ? rect.Width : -rect.Width;
            Rectangle checkFloor = new Rectangle((int)rect.Center.X + floorPos, rect.Bottom + 5, 3, 3);

            SpriteEffects effect = idealVelocity.X > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            animation.Draw(sb, rect, Color.White, 0, Vector2.Zero, effect);
            //sb.Draw(TextureManager.blankTexture, checkFloor, Color.Red);
        }
    }
}