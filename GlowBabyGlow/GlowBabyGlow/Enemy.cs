using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace GlowBabyGlow
{
    class Enemy : Actor
    {
        bool movingRight = true;
        public static int width = 20;
        public static int height = 35;

        int health = 3;

        public Rectangle Rect
        {
            get { return rect; }
        }

        public Enemy(Point pos)
        {
            this.pos = new Vector2(pos.X, pos.Y);
            rect = new Rectangle(pos.X, pos.Y, width, height);
            velocity.X = 50;
        }

        public override void Update(float dt)
        {
            base.Update(dt);

            if (health <= 0)
            {
                World.EnemyManager.Remove(this);
            }
        }

        public void Hit(Bullet b)
        {
            health--;
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
                    int overlappingAbove = t.OverlappingAbove(rect);
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
                    if (t.StandingOn(rect))
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
                    int overlappingRight = t.OverlappingRight(rect);
                    if (overlappingRight > 0)
                    {
                        velocity.X = 0;

                        if (!wallLeft)
                        {
                            pos.X = t.Rect.Right;
                            velocity.X = -velocity.X;
                            tileCollideLeft = true;
                        }
                    }
                }
                if (velocity.X > 0)
                {
                    int overlappingLeft = t.OverlappingLeft(rect);
                    if (overlappingLeft > 0)
                    {
                        velocity.X = 0;

                        if (!wallRight)
                        {
                            pos.X = t.Rect.Left - rect.Width;
                            velocity.X = -velocity.X;
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
                velocity.X = -velocity.X;
            }

            wallRight = tileCollideRight;
            wallLeft = tileCollideLeft;
        }

        public override void Draw(SpriteBatch sb)
        {
            int floorPos = velocity.X > 0 ? rect.Width : -rect.Width;
            Rectangle checkFloor = new Rectangle((int)rect.Center.X + floorPos, rect.Bottom + 5, 3, 3);

            sb.Draw(TextureManager.blankTexture, rect, Color.DarkGoldenrod);
            //sb.Draw(TextureManager.blankTexture, checkFloor, Color.Red);
        }
    }
}