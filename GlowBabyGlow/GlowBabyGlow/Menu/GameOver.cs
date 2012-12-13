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

        static int score;
        static bool newScore;

        public static bool Initialized
        {
            get { return initialized; }
        }

        public static void Init(World w)
        {
            newScore = false;
            startTimer = 1.5f;
            initialized = true;
            font = new GFont(TextureManager.font, 4, 10);
            position = -Config.screenH;
            rect = new Rectangle(0, -Config.screenH, Config.screenW, Config.screenH);
            world = w;
            world.Automate = false;
            CheckHighScore();
        }

        public static void CheckHighScore()
        {
            if (world != null)
            {
                if (world.Players.Count == 1)
                {
                    int score = world.Players[0].Score;

                    if (Config.highScore[world.LevelName] < score)
                    {
                        Config.highScore[world.LevelName] = score;
                        newScore = true;
                    }
                }
            }
        }

        public static void Reset()
        {
            position = -Config.screenH;
            rect.Y = (int)position;
            initialized = false;
            velocity = 0;
            CheckHighScore();
            Input.spaceBarPreventativeMeasureFlag = false;
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
                    velocity = -(velocity - (velocity / 4f));
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
            sb.Draw(TextureManager.blankTexture, rect, new Color(25,25,25));
            sb.Draw(TextureManager.blankTexture, new Rectangle(
                0, 0, Config.screenW, rect.Top), Color.Black);
            Vector2 pos = new Vector2(
                (Config.screenW / 2) - ((font.Size.X * "game over".Length) / 2),
                (Config.screenH / 2) - (Config.screenH / 6) + position);
            font.Draw(sb, pos, "game over", Color.Red);
        }

    }
}
