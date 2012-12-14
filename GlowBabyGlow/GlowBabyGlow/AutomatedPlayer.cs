using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GlowBabyGlow
{
    class AutomatedPlayer
    {
        Texture2D texture;
        Vector2 pos;
        Vector2 velocity;
        float destination;
        int width;
        int height;
        float maxSpeed = (int)(350 * Config.screenR);
        float acceleration = (int)(100 * Config.screenR);
        protected float gravity = (int)(4000 * Config.screenR);

        public AutomatedPlayer(int x, int d)
        {
            width = Player.width;
            height = Player.height;
            pos = new Vector2(x, Config.screenH - height - 10);
            destination = d;
            velocity = new Vector2();
        }

        public void Update(float dt)
        {
            velocity.Y += gravity * (dt / 1000);

            if (pos.X < destination - width)
            {
                velocity.X += acceleration;
            }
            else if (pos.X > destination + width)
            {
                velocity.X -= acceleration;
            }

            if (velocity.X > maxSpeed)
            {
                velocity.X = maxSpeed;
            }
            if (velocity.X < -maxSpeed)
            {
                velocity.X = -maxSpeed;
            }

            pos.X += velocity.X;
            pos.Y += velocity.Y;
        }

        public void Draw(SpriteBatch sb)
        {

        }
    }
}
