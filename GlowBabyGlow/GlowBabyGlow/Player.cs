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
        static int width = 20;
        static int height = 35;

        Vector2 pos;
        float gravity = 50;
        Vector2 velocity = new Vector2();
        float jumpStrength = 900;
        float speed = 50;
        float maxSpeed = 200;  //pixels per second

        bool inAir = true;

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
            float xInput = Input.GetLeftThumbs(0).Left.X;
            float xMax = Math.Abs(maxSpeed * xInput);

            velocity.X += xInput * speed;

            if (velocity.X > xMax)
            { velocity.X = xMax; }
            else if (velocity.X < -xMax)
            { velocity.X = -xMax; }

            if (inAir)
            {
                velocity.Y += gravity;
            }

            pos.Y += velocity.Y * (dt / 1000);
            pos.X += velocity.X * (dt / 1000);
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(TextureManager.blankTexture, rect, Color.Green);
        }
    }
}
