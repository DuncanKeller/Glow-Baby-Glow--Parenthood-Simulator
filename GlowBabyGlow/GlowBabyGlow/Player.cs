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
        int index = 0;
        static int width = 20;
        static int height = 35;

        float jumpStrength = 700;
        float acceleration = 50;
        float maxSpeed = 150;  //pixels per second
        float ladderSpeed = 150;

        bool holdingBaby = true;
        bool readyToThrow = false;
        float throwVelocity = 0;
        Baby baby = null;

        public bool HoldingBaby
        {
            get { return holdingBaby; }
        }

        public bool ReadyToThrow
        {
            get { return readyToThrow; }
            set { readyToThrow = value; }
        }

        public Player(Point pos)
        {
            this.pos = new Vector2(pos.X, pos.Y);
            rect = new Rectangle(pos.X, pos.Y, width, height);
        }

        public override void Update(float dt)
        {
            if (baby != null)
            {
                baby.Update(dt);
            }

            HandleMovement(dt);

            base.Update(dt);
        }

        public void Jump()
        {
            if (!inAir && !readyToThrow)
            {
                inAir = true;
                velocity.Y = -jumpStrength;
            }
        }

        public void Throw()
        {
            if (baby == null)
            {
                baby = new Baby(pos, throwVelocity, index);
                holdingBaby = false;
            }
        }

        public void HandleMovement(float dt)
        {
            float xInput = Input.GetThumbs(0).Left.X;
            float yInput = Input.GetThumbs(0).Left.Y;

            if (onLadder || readyToThrow)
            {
                xInput = 0;
            }
            else
            {
                //consider changing to yMax
                yInput = 0;
            }

            float xMax = Math.Abs(maxSpeed * xInput);
            float yMax = Math.Abs(ladderSpeed * yInput);

            velocity.X += xInput * acceleration;
            velocity.Y += -yInput * acceleration;

            if (velocity.X > xMax)
            { velocity.X = xMax; }
            else if (velocity.X < -xMax)
            { velocity.X = -xMax; }

            if (onLadder)
            {
                if (velocity.Y > yMax)
                { velocity.Y = yMax; }
                else if (velocity.Y < -yMax)
                { velocity.Y = -yMax; }
            }

            if (wallLeft && xInput < 0)
            { velocity.X = 0; }
            if (wallRight && xInput > 0)
            { velocity.X = 0; }
        }

        public void Collision(ref List<Tile> tiles, ref List<Ladder> ladders)
        {
            if (baby != null)
            {
                baby.Collision(ref tiles);

                if (baby.Rect.Intersects(rect)
                    && baby.ReadyToCatch)
                {
                    baby = null;
                    holdingBaby = true;
                }
            }

            bool fall = true;
            bool tileCollideLeft = false;
            bool tileCollideRight = false;

            // ladders
            if (!onLadder)
            {
                //if (Input.GetThumbs(index).Left.Y > 0.2)
                if(Input.GetThumbs(index).Left.Y > 0)
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
                if (Input.GetThumbs(index).Left.Y < 0)
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
            if (baby != null)
            {
                baby.Draw(sb);
            }

            sb.Draw(TextureManager.blankTexture, rect, Color.Green);
        }
    }
}
