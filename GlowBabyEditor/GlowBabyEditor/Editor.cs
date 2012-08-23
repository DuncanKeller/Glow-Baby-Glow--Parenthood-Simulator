using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
