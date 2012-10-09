using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GlowBabyGlow
{
    static class Tutorial
    {
        static int size = 100;
        static float timer;
        static int counter;
        static Rectangle destination = new Rectangle(0,0, size, size);

        static bool cueCounter = true;
        static float cueTime = 1.5f;
        static string text = "";
        static GFont font = new GFont(TextureManager.font, 4, 10);

        public static void Update(float dt, Player p)
        {
            if (counter == 0)
            {
                p.Baby = null;
                p.HoldingBaby = false;
            }
            if (counter < 3)
            {
                p.BabyLife = p.MaxBabyLife / 2;
            }

            if (cueCounter)
            {
                timer += dt;
                if (timer >= cueTime)
                {
                    if (counter == 0)
                    {
                        destination.X = Config.screenW / 20;
                        destination.Y = Config.screenH / 4;
                        text = "";
                    }
                    else if (counter == 1)
                    {
                        Enemy e = new Enemy(new Point(
                        Config.screenW / 3, (Config.screenH / 2) - Tile.Size));
                        World.EnemyManager.Add(e);
                        text = "shoot this vagrant";
                    }
                    else if (counter == 2)
                    {
                        destination.X = Config.screenW / 2;
                        destination.Y = Config.screenH - (Config.screenH / 5);
                        text = "";
                    }
                    else if (counter == 3)
                    {
                        Baby b = new Baby(new Vector2(
                            Config.screenW / 2, Config.screenH / 16),
                            0, p.Index);
                        b.Velocity = new Vector2(0, 0);
                        p.Baby = b;
                        text = "catch the baby";
                    }
                    else if (counter == 4)
                    {
                        text = "shake the baby";
                    }
                    else if (counter == 5)
                    {
                        destination.X = Config.screenW / 20;
                        destination.Y = Config.screenH / 4;
                        text = "keep the baby alive";
                    }
                    else if (counter == 6)
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            World.EnemyManager.Spawn();
                        }
                        text = "kill";
                    }

                    counter++;
                    timer = 0;
                    cueCounter = false;

                    return;
                }
            }
            else
            {
                if (counter == 1)
                {
                    if (p.HitRect.Intersects(destination))
                    {
                        cueCounter = true;
                    }
                }
                else if (counter == 2)
                {
                    if (World.EnemyManager.Enemies.Count == 0)
                    {
                        cueCounter = true;
                    }
                }
                else if (counter == 3)
                {
                    if (p.HitRect.Intersects(destination))
                    {
                        cueCounter = true;
                    }
                }
                else if (counter == 4)
                {
                    if (p.HoldingBaby)
                    {
                        cueCounter = true;
                    }
                }
                else if (counter == 5)
                {
                    if (p.BabyLife >= p.MaxBabyLife - 1)
                    {
                        cueCounter = true;
                    }
                }
                else if (counter == 6)
                {
                    if (p.HitRect.Intersects(destination))
                    {
                        cueCounter = true;
                    }
                }
            }
        }

        public static void Draw(SpriteBatch sb)
        {
            font.Draw(sb, new Vector2(10, 10), text);
        }

    }
}
