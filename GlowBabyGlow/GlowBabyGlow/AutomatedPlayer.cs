using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Duncanimation;

namespace GlowBabyGlow
{
    class AutomatedPlayer
    {
        Texture2D texture;
        Vector2 pos;
        Vector2 velocity;
        Animator anim;

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
            anim = new Animator(TextureManager.testRun, 13, 6);
            anim.AddAnimation("run", 0, 17, 16.5f, true);
            anim.AddAnimation("idle", 18, 0, 0, true);
            anim.AddAnimation("jump", 24, 3, 15, true, 26);
            anim.AddAnimation("fall", 42, 3, 24, true, 44);
            anim.AddAnimation("shoot", 30, 10, 24, true);
            anim.AddAnimation("climb", 48, 8, 30, true);
            anim.AddAnimation("shake", 60, 17, 200, true);
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

            if (pos.Y + height > Config.screenH)
            {
                velocity.Y = 0;
                pos.Y = Config.screenH - height;
            }

            anim.Update(dt);
        }

        public void Draw(SpriteBatch sb)
        {
            anim.Draw(sb, new Rectangle((int)pos.X, (int)pos.Y, width, height),
                Color.White, 0, new Vector2(0, 0), SpriteEffects.None);
        }
    }
}
