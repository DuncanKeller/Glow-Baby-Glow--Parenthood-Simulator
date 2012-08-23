using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace GlowBabyEditor
{
    class Editor
    {
        static List<Region> wallRegions = new List<Region>();
        static List<Region> ladderRegions = new List<Region>();
        static List<Region> currentRegion;
        static MouseState mouse;
        static MouseState oldMouse;
        static KeyboardState key;
        static KeyboardState oldkey;

        public static void Init()
        {
            for(int x = 0; x < Game1.w; x += Region.SIZE)
            {
                for (int y = 0; y < Game1.h; y += Region.SIZE)
                {
                    wallRegions.Add(new Region(x, y));
                    ladderRegions.Add(new Region(x, y));
                }
            }
            currentRegion = wallRegions;
        }

        public static void Update()
        {
            mouse = Mouse.GetState();
            key = Keyboard.GetState();

            if (key.IsKeyDown(Keys.D1) &&
                oldkey.IsKeyUp(Keys.D1))
            {
                currentRegion = wallRegions;
            }
            else if (key.IsKeyDown(Keys.D2) &&
                oldkey.IsKeyUp(Keys.D2))
            {
                currentRegion = ladderRegions;
            }

            if (key.IsKeyDown(Keys.S) &&
                oldkey.IsKeyUp(Keys.S))
            {
                Save();
            }
            else if (key.IsKeyDown(Keys.L) &&
                oldkey.IsKeyUp(Keys.L))
            {
                Load();
            }

            if (key.IsKeyDown(Keys.Up) &&
                oldkey.IsKeyUp(Keys.Up))
            {
                Mouse.SetPosition(mouse.X, mouse.Y - Region.SIZE);
            }
            else if (key.IsKeyDown(Keys.Down) &&
                oldkey.IsKeyUp(Keys.Down))
            {
                Mouse.SetPosition(mouse.X, mouse.Y + Region.SIZE);
            }
            else if (key.IsKeyDown(Keys.Left) &&
                oldkey.IsKeyUp(Keys.Left))
            {
                Mouse.SetPosition(mouse.X - Region.SIZE, mouse.Y);
            }
            else if (key.IsKeyDown(Keys.Right) &&
                oldkey.IsKeyUp(Keys.Right))
            {
                Mouse.SetPosition(mouse.X + Region.SIZE, mouse.Y);
            }

            foreach (Region r in currentRegion)
            {
                string type = "e";
                if (currentRegion == wallRegions)
                {
                    type = "w";
                }
                else if (currentRegion == ladderRegions)
                {
                    type = "l";
                }

                r.Update(mouse, oldMouse, type);
            }

            oldkey = key;
            oldMouse = mouse;
        }

        public static void Save()
        {
            StreamWriter sw = new StreamWriter("test.txt");
            foreach (Region r in wallRegions)
            {
                if (r.Type != "e")
                {
                    sw.WriteLine(r.Type + "," + r.X + "," + r.Y);
                }
            }
            foreach (Region r in ladderRegions)
            {
                if (r.Type != "e")
                {
                    sw.WriteLine(r.Type + "," + r.X + "," + r.Y);
                }
            }
            sw.Flush();
            sw.Close();
        }

        public static void Load()
        {
            StreamReader sr = new StreamReader("test.txt");
            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                string[] info = line.Split(',');
                string type = info[0];
                int x = Int32.Parse(info[1]);
                int y = Int32.Parse(info[2]);

                if (type == "w")
                {
                    foreach (Region r in wallRegions)
                    {
                        if (r.X == x && r.Y == y)
                        {
                            r.Type = type;
                            break;
                        }
                    }
                }
                else if (type == "l")
                {
                    foreach (Region r in ladderRegions)
                    {
                        if (r.X == x && r.Y == y)
                        {
                            r.Type = type;
                            break;
                        }
                    }
                }
            }
            sr.Close();
        }

        public static void Draw(SpriteBatch sb)
        {
            foreach (Region r in wallRegions)
            {
                r.Draw(sb);
            }
            foreach (Region r in ladderRegions)
            {
                r.Draw(sb);
            }
        }
    }
}
