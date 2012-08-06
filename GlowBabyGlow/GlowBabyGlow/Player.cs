using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GlowBabyGlow
{
    class Player : Entity
    {
        static int index = 0;
        static int width = 20;
        static int height = 35;

        Vector2 pos;
        float gravity = 50;
        Vector2 velocity = new Vector2();
        float jumpStrength = 900;
        float speed = 50;
        float maxSpeed = 200;  //pixels per second

        bool inAir = true;
        bool wallLeft = false;
        bool wallRight = false;

        public Player(Point pos)
        {
            this.pos = new Vector2(pos.X, pos.Y);
            rect = new Rectangle(pos.X, pos.Y, width, height);
        }

        public override void Update(float dt)
        {
            HandleMovement(dt);

            rect.X = (int)pos.X;
            rect.Y = (int)pos.Y;
        }

        public void Jump()
        {
            inAir = true;
            velocity.Y = -jumpStrength;
        }

        public void HandleMovement(float dt)
        {
            float xInput = Input.GetThumbs(0).Left.X;
            float xMax = Math.Abs(maxSpeed * xInput);

            velocity.X += xInput * speed;

            if (velocity.X > xMax)
            { velocity.X = xMax; }
            else if (velocity.X < -xMax)
            { velocity.X = -xMax; }

            if (wallLeft && xInput < 0)
            { velocity.X = 0; }

            if (inAir)
            {
                velocity.Y += gravity;
            }

            pos.Y += velocity.Y * (dt / 1000);
            pos.X += velocity.X * (dt / 1000);
        }

        public void Collision(ref List<Tile> tiles)
        {
            bool fall = true;
            bool tileCollideLeft = false;
            bool tileCollideRight = false;

            foreach (Tile t in tiles)
            {
                if (inAir)
                {
                    int overlappingAbove = t.OverlappingAbove(rect);
                    if (overlappingAbove > 0)
                    {
                        inAir = false;
                        velocity.Y = 0;
                        pos.Y -= overlappingAbove;
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
