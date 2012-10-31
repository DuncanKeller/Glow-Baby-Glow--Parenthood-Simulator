using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GlowBabyGlow
{
    static class GameOver
    {
        static World world;
        static Rectangle rect;
        static float position;
        static float velocity = 0;
        static GFont font;
        static bool initialized = false;
        static float startTimer;

        public static bool Initialized
        {
            get { return initialized; }
        }

        public static void Init(World w)
        {
            startTimer = 1.5f;
            initialized = true;
            font = new GFont(TextureManager.font, 4, 10);
            position = -Config.screenH;
            rect = new Rectangle(0, -Config.screenH, Config.screenW, Config.screenH);
            world = w;
        }

        public static void Reset()
        {
            position = -Config.screenH;
            rect.Y = (int)position;
            initialized = false;
            velocity = 0;
        }

        public static void Update(float dt)
        {
            if (startTimer == 0)
            {
                if (Math.Abs(velocity) < 1 &&
                    position >= 0)
                {
                    velocity = 0;
                    position = 0;
                }
                else if (rect.Bottom > Config.screenH)
                {
                    velocity = -(velocity - (velocity / 3.5f));
                }
                else
                {
                    velocity += 65;
                }

                position += velocity * (dt / 1000);
                rect.Y = (int)position;
            }
            else
            {
                if (startTimer > 0)
                { startTimer -= dt / 1000; }
                else
                { startTimer = 0; }
            }
        }

        public static void Draw(SpriteBatch sb)
        {
            sb.Draw(TextureManager.blankTexture, rect, Color.Black);
            sb.Draw(TextureManager.blankTexture, new Rectangle(
                0, 0, Config.screenW, rect.Top), Color.Black);
        }

    }
}
