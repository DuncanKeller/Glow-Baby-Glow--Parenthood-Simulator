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

        public static GameType gameType = GameType.single;

        public static int[] Players()
        {
            if (menus.ContainsKey("multi"))
            {
                if ((menus["multi"] as MultiMenu).GetPlayers().Length > 0)
                {
                    return (menus["multi"] as MultiMenu).GetPlayers();
                }
            }
            
            int[] index = new int[1];
            index[0] = Input.defaultIndex;
            return index;
        }

        public static void Init(Game1 game)
        {
            g = game;
            TitleMenu tm = new TitleMenu(game);
            menus.Add("title", tm);
            menus.Add("blank", new BlankMenu(game));
            menus.Add("single-multi", new SingleMultiMenu(game));
            menus.Add("level", new levelMenu(game, false));
            menus.Add("multi", new MultiMenu(game));
            menus.Add("versus", new GametypeVsMenu(game));
            menus.Add("coop", new GametypeCoopMenu(game));
            menus.Add("multi-level", new levelMenu(game, true));

            currentMenu = tm;
        }

        public static World GetCurrentLevel()
        {
            World w = null;
            if (currentMenu == menus["multi-level"])
            {
                w = (menus["multi-level"] as levelMenu).GetCurrentWorld();
            }
            else
            {
                w = (menus["level"] as levelMenu).GetCurrentWorld();
            }
            Input.Init(w);
            return w;
        }

        public static void Reset()
        {
            (menus["level"] as levelMenu).Unlock();
            (menus["multi-level"] as levelMenu).Unlock();
            g.Reset();
            Input.spaceBarPreventativeMeasureFlag = false;
        }

        public static void Update(float dt)
        {
            if (inputTimer > 0)
            { inputTimer -= dt / 1000; }
            else
            { inputTimer = 0; }

            inputTimer = 0;

            currentMenu.Update(dt);

            if (currentMenu != menus["level"] && currentMenu != menus["multi-level"])
            {
                if (Input.GetThumbs(Input.defaultIndex).Y > 0.2 &&
                    inputTimer == 0 && Input.GetPrevThumbs(Input.defaultIndex).Y <= 0.2)
                {
                    currentMenu.CurrentItem++;
                    inputTimer = 0.4f;
                }
                if (Input.GetThumbs(Input.defaultIndex).Y < -0.2 &&
                    inputTimer == 0 && Input.GetPrevThumbs(Input.defaultIndex).Y >= -0.2)
                {
                    currentMenu.CurrentItem--;
                    inputTimer = 0.4f;
                }
            }
            else
            {
                if (!(menus["level"] as levelMenu).Locked
                    && !(menus["multi-level"] as levelMenu).Locked)
                {
                    if (Input.GetThumbs(Input.defaultIndex).X > 0.2 &&
                        inputTimer == 0 && Input.GetPrevThumbs(Input.defaultIndex).X <= 0.2)
                    {
                        currentMenu.CurrentItem++;
                        inputTimer = 0.2f;
                    }
                    if (Input.GetThumbs(Input.defaultIndex).X < -0.2 &&
                        inputTimer == 0 && Input.GetPrevThumbs(Input.defaultIndex).X >= -0.2)
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