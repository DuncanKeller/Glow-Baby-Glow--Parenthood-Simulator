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

        public Enemy(Point pos)
        {
            this.pos = new Vector2(pos.X, pos.Y);
            rect = new Rectangle(pos.X, pos.Y, width, height);
        }

        public override void Update(float dt)
        {
            if (movingRight)
            {
                velocity.X = 50;
            }
            else
            {
                velocity.X = -50;
            }

            base.Update(dt);
        }

        public void Collision(ref List<Tile> tiles, ref List<Ladder> ladders)
        {
            bool fall = true;
            bool tileCollideLeft = false;
            bool tileCollideRight = false;

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
            wallRight = tileCollideRight;
            wallLeft = tileCollideLeft;
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(TextureManager.blankTexture, rect, Color.DarkGoldenrod);
        }
    }
}