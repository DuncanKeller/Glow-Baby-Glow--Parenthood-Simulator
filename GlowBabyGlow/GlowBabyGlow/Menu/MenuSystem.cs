using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GlowBabyGlow
{
    class MenuSystem
    {
        static Dictionary<string, Menu> menus = new Dictionary<string, Menu>();
        static float inputTimer;
        static Menu currentMenu;
        static Game1 g;

        public static void Init(Game1 game)
        {
            g = game;
            TitleMenu tm = new TitleMenu(game);
            menus.Add("title", tm);
            menus.Add("single-multi", new SingleMultiMenu(game));
            menus.Add("level", new levelMenu(game));
            menus.Add("multi", new MultiMenu(game));

            currentMenu = tm;
        }

        public static World GetCurrentLevel()
        {
            World w = (menus["level"] as levelMenu).GetCurrentWorld();
            Input.Init(w);
            return w;
        }

        public static void Reset()
        {
            (menus["level"] as levelMenu).Unlock();
            g.Reset();
        }

        public static void Update(float dt)
        {
            if (inputTimer > 0)
            { inputTimer -= dt / 1000; }
            else
            { inputTimer = 0; }

            //foreach (Menu m in menus)
            //{
            //    m.Update(dt);
            //}
            currentMenu.Update(dt);

            if (currentMenu != menus["level"])
            {
                if (Input.GetThumbs(0).Y > 0.2 &&
                    inputTimer == 0)
                {
                    currentMenu.CurrentItem++;
                    inputTimer = 0.4f;
                }
                if (Input.GetThumbs(0).Y < -0.2 &&
                 inputTimer == 0)
                {
                    currentMenu.CurrentItem--;
                    inputTimer = 0.4f;
                }
            }
            else
            {
                if (!(menus["level"] as levelMenu).Locked)
                {
                    if (Input.GetThumbs(0).X > 0.2 &&
                        inputTimer == 0)
                    {
                        currentMenu.CurrentItem++;
                        inputTimer = 0.2f;
                    }
                    if (Input.GetThumbs(0).X < -0.2 &&
                        inputTimer == 0)
                    {
                        currentMenu.CurrentItem--;
                        inputTimer = 0.2f;
                    }
                }
            }

            foreach (KeyValuePair<string, Menu> m in menus)
            {
                m.Value.UpdatePosition(dt);
            }
        }

        public static void SwitchMenu(Vector2 v, string s)
        {
            if (inputTimer == 0)
            {
                currentMenu = menus[s];
                foreach (KeyValuePair<string, Menu> m in menus)
                {
                    m.Value.ChangePos(v);
                }
                inputTimer = 0.35f;
            }
        }

        public static void Draw(SpriteBatch sb, GraphicsDevice g)
        {
            foreach (KeyValuePair<string, Menu> m in menus)
            {
                m.Value.Draw(sb, g);
            }
        }
    }
}