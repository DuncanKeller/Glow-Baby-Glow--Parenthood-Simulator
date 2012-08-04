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
        float speed = 0;
        float maxSpeed = 200;  //pixels per second

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

        public void HandleMovement(float dt)
        {
            float xInput = Input.GetLeftThumbs(0).Left.X;
            float xMax = Math.Abs(maxSpeed * xInput);

            speed += xInput * 50;

            if (speed > xMax)
            { speed = xMax; }
            else if (speed < -xMax)
            { speed = -xMax; }

            pos.X += speed * (dt / 1000);
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(TextureManager.blankTexture, rect, Color.Green);
        }
    }
}
