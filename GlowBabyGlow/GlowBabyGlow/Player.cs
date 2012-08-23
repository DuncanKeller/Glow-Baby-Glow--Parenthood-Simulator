using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GlowBabyGlow
{
    class Player : Actor
    {
        static int index = 0;
        static int width = 20;
        static int height = 35;

        float jumpStrength = 900;
        float acceleration = 50;
        float maxSpeed = 200;  //pixels per second
        float ladderSpeed = 100;

        public Player(Point pos)
        {
            this.pos = new Vector2(pos.X, pos.Y);
            rect = new Rectangle(pos.X, pos.Y, width, height);
        }

        public override void Update(float dt)
        {
            HandleMovement(dt);

            base.Update(dt);
        }

        public void Jump()
        {
            inAir = true;
            velocity.Y = -jumpStrength;
        }

        public void HandleMovement(float dt)
        {
            //float xInput = Input.GetThumbs(0).Left.X;
            float xInput = Input.GetThumbsDebugX();
            float xMax = Math.Abs(maxSpeed * xInput);

            float yInput = Input.GetThumbsDebugY();
            float yMax = Math.Abs(ladderSpeed * yInput);

            if (onLadder)
            {
                xInput = 0;
            }
            else
            {
                //consider changing to yMax
                yInput = 0;
            }

            velocity.X += xInput * acceleration;

            velocity.Y += yInput * acceleration;

            if (velocity.X > xMax)
            { velocity.X = xMax; }
            else if (velocity.X < -xMax)
            { velocity.X = -xMax; }

            if (onLadder)
            {
                if (velocity.Y > ladderSpeed)
                { velocity.Y = ladderSpeed; }
                else if (velocity.Y < -ladderSpeed)
                { velocity.Y = -ladderSpeed; }
            }

            if (wallLeft && xInput < 0)
            { velocity.X = 0; }
            if (wallRight && xInput > 0)
            { velocity.X = 0; }
        }

        public void Collision(ref List<Tile> tiles, ref List<Ladder> ladders)
        {
            bool fall = true;
            bool tileCollideLeft = false;
            bool tileCollideRight = false;

            // ladders
            if (!onLadder)
            {
                //if (Input.GetThumbs(index).Left.Y > 0.2)
                if(Input.GetThumbsDebugY() < 0)
                {
                    foreach (Ladder l in ladders)
                    {
                        if (l.LadderAbove(rect))
                        {
                            if (!onLadder)
                            {
                                onLadder = true;
                                velocity.X = 0;
                                velocity.Y = 0;
                            }
                        }
                    }  
                }
                //else if (Input.GetThumbs(index).Left.Y < -0.2)
                if (Input.GetThumbsDebugY() > 0)
                {
                    foreach (Ladder l in ladders)
                    {
                        if (l.LadderBelow(rect))
                        {
                            if (!onLadder)
                            {
                                onLadder = true;
                                velocity.X = 0;
                                velocity.Y = 0;
                            }
                        }
                    }
                }
            }
            else
            {
                bool stillOnLadder = false;
                foreach (Ladder l in ladders)
                {
                    if (l.Rect.Intersects(rect))
                    {
                        stillOnLadder = true;
                    }
                }
                onLadder = stillOnLadder;
            }

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
                if (Input.GetThumbs(index).Left.X < 0)
                {
                    int overlappingRight = t.OverlappingRight(rect);
                    if (overlappingRight > 0)
                    {
                        velocity.X = 0;

                        if (!wallLeft)
                        {
                            pos.X = t.Rect.Right;
                            tileCollideLeft = true;
                        }
                    }
                }
                if (Input.GetThumbs(index).Left.X > 0)
                {
                    int overlappingLeft = t.OverlappingLeft(rect);
                    if (overlappingLeft > 0)
                    {
                        velocity.X = 0;

                        if (!wallRight)
                        {
                            pos.X = t.Rect.Left - rect.Width;
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
            sb.Draw(TextureManager.blankTexture, rect, Color.Green);
        }
    }
}
