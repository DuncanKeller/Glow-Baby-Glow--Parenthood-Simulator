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
        static World w;
        static int size = 80;
        static float timer;
        static int counter;
        static Rectangle destination = new Rectangle(0,0, size, size);

        static bool cueCounter = true;
        static float cueTime = 1.5f;
        static string text = "";
        static string text2 = "";
        static GFont font = new GFont(TextureManager.smallFont, 5, 10);
        
        static float wave = 0;
        static float arrowTimer = 0;
        static float speed = 2;
        static float amplitude = 15;

        static bool done = false;
        static float doneTimer = 0;

        public static bool Done
        {
            get { return done; }
        }

        public static void Init(World world)
        {
            w = world;
        }

        public static void Update(float dt, Player p)
        {
            arrowTimer += (dt / 1000) * speed;
            wave = (float)Math.Sin(arrowTimer) * amplitude;

            if (counter == 0)
            {
                p.Baby = null;
                p.HoldingBaby = false;
            }
            if (counter < 4)
            {
                p.BabyLife = p.MaxBabyLife / 2;
            }

            if (done)
            {
                doneTimer += dt / 1000;
            }

            if (doneTimer > 2.2f)
            {
                Config.tutorial = false;
            }
            
            if (cueCounter)
            {
                timer += dt;
                if (timer >= cueTime)
                {
                    if (counter == 0)
                    {
                        destination.X = Config.screenW / 17;
                        destination.Y = (Config.screenH / 4) + (Config.screenH / 10);
                        text = "welcome to glow, baby, glow!";
                    }
                    else if (counter == 1)
                    {
                        Enemy e = new Enemy(new Point(
                        Config.screenW / 3, (Config.screenH / 2) - Tile.Size), w);
                        w.EnemyManager.Add(e);
                        text = "shoot that nasty looking thing over there";
                        text2 = "using ";
                        if (Input.keys)
                        {
                            text2 += "shift";
                        }
                        else
                        {
                            text2 += "#";
                        }
                    }
                    else if (counter == 2)
                    {
                        destination.X = Config.screenW / 2 - (Config.screenW / 30);
                        destination.Y = Config.screenH - (Config.screenH / 5);
                        text = "so far so good";
                        text2 = "hey, go stand over there";
                       
                    }
                    else if (counter == 3)
                    {
                        for (int i = 0; i < 50; i++)
                        {
                            Vector2 pos = new Vector2(Config.screenW / 2, Config.screenH / 5);
                            pos.X += (float)(Config.rand.NextDouble() * Config.screenW / 10) - Config.screenW / 20;
                            GooParticle gp = new GooParticle(pos);
                            w.ParticleManager.AddParticle(gp);
                        }
                        Baby b = new Baby(new Vector2(
                            Config.screenW / 2, Config.screenH / 5),
                            0, p.Index, w);
                        b.Velocity = new Vector2(0, 0);
                        p.Baby = b;
                        text = "catch the baby!";
                        text2 = "";
                    }
                    else if (counter == 4)
                    {
                        text = "rock the baby by holding ";
                        if (Input.keys)
                        {
                            text += "shift";
                            text2 += "and mashing left and right";
                        }
                        else
                        {
                            text += "#";
                            text2 += "and rotating the control stick $";
                        }
                    }
                    else if (counter == 5)
                    {
                        destination.X = Config.screenW / 2;
                        destination.Y = (Config.screenH / 4) + (Config.screenH / 10);
                        text = "oh yeah, whatever you do-";
                        text2 = "dont drop the baby!!!";
                    }
                    else if (counter == 6)
                    {
                        destination.X = Config.screenW / 17;
                        destination.Y = (Config.screenH / 4) + (Config.screenH / 10);
                        text = "you can give the baby a little toss-";
                        if (Input.keys)
                        {
                            text2 = "hold " + "space" + ", aim, and release!";
                        }
                        else
                        {
                            text2 = "hold " + "@" + ", aim, and release!";
                        }
                    }
                    else if (counter == 7)
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            w.EnemyManager.Spawn();
                        }
                        text = "take care of these guys";
                        text2 = "coins will give you points!";
                    }
                    else if (counter == 8)
                    {
                        text = "good job, man";
                        text2 = "you can figure the rest out yourself";
                        destination.X =  (Config.screenW / 20);
                        destination.Y = Config.screenH - (Config.screenH / 5);
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
                    if (w.EnemyManager.Enemies.Count == 0)
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
                else if (counter == 7)
                {
                    if (p.HitRect.Intersects(destination))
                    {
                        cueCounter = true;
                    }
                }
                else if (counter == 8)
                {
                    if (w.EnemyManager.Enemies.Count == 0)
                    {
                        cueCounter = true;
                    }
                }
                else if (counter == 9)
                {
                    if (p.HitRect.Intersects(destination))
                    {
                        cueCounter = true;
                        done = true;
                    }
                }
            }
        }

        public static void Draw(SpriteBatch sb)
        {
            if (text != "")
            {
                if (text2 != "")
                {
                    sb.Draw(TextureManager.blankTexture, new Rectangle(0, 0, Config.screenW, font.Size.Y / 2), new Color(0, 0, 0, 100));
                }
                else
                {
                    sb.Draw(TextureManager.blankTexture, new Rectangle(0, 0, Config.screenW, font.Size.Y), new Color(0, 0, 0, 100));
                }
            }
            font.Draw(sb, new Vector2(10, 10), text, Color.GhostWhite, true);
            font.Draw(sb, new Vector2(10, 20 + (font.Size.Y / 2)), text2, Color.GhostWhite, true);

            int width = 60;
            int height = 80;
            Rectangle arrowRect = new Rectangle(destination.Center.X - (width / 2), destination.Top - height + (int)wave,
                width, height);

            if (counter == 1 ||
                counter == 3 ||
                counter == 6 ||
                counter == 7 ||
                counter == 9)
            {
                sb.Draw(TextureManager.arrow, arrowRect, Color.White);
                //sb.Draw(TextureManager.blankTexture, destination, new Color(100, 0, 0, 100));
            }
        }

    }
}
