using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GlowBabyGlow
{
    enum GameType
    {
        single,
        survival,
        vsSurvival,
        hotPotato,
        thief
    }

    class GametypeCoopMenu : Menu
    {
        List<MenuElement> gameTypes = new List<MenuElement>();
        int index = 0;
        MenuElement description;
        float fingerOffset = 0;
        float fingerTimer = 0;
        bool fingerDirectionRight = true;
       
        public GametypeCoopMenu(Game1 g)
            : base(g)
        {
            pos = new Vector2(-Config.screenW*2, Config.screenH * 2);
            backdrop = TextureManager.bParkWildernessRight;
            elements.Add(new MenuElement("cooperative", null, new Vector2(
                Config.screenW - (GFont.width * "cooperative".Length) - 10, 10), true, this, delegate() { }));
            c = Color.White;
            destination = pos;
            //elements[0].Selected = true;

            gameTypes.Add(new MenuElement("survival", null, new Vector2(
                (Config.screenW / 3) + 10, ((Config.screenH / 20) * 4) + (Config.screenH / 10)),
                true, this, delegate() { MenuSystem.gameType = GameType.survival; }));
            gameTypes.Add(new MenuElement("hot potato", null, new Vector2(
                (Config.screenW / 3) + 10, ((Config.screenH / 20) * 7) + (Config.screenH / 10)),
                true, this, delegate() { MenuSystem.gameType = GameType.hotPotato; }));

            description = new MenuElement("", null, new Vector2(20, (Config.screenH / 20) * 7),
                false, this, delegate() { });
            description.SetToDescr();
            UpdateDescr();

        }

        public override void Update(float dt)
        {
            base.Update(dt);

            MenuSystem.lastScreenVersus = false;

            if (Input.HoldingSecondary(Input.defaultIndex) &&
                  !Input.HoldingSecondaryPrev(Input.defaultIndex))
            {
                MenuSystem.SwitchMenu(new Vector2(-Config.screenW, 0), "multi");
                MenuSystem.gameType = GameType.single;
            }
            else if (Input.HoldingPrimary(Input.defaultIndex) &&
                  !Input.HoldingPrimaryPrev(Input.defaultIndex))
            {
                MenuSystem.SwitchMenu(new Vector2(Config.screenW * 2, 0), "multi-level");
                gameTypes[index].Evoke();
            }

            foreach (MenuElement m in gameTypes)
            {
                m.Update(dt);
            }

            if (Input.GetThumbs(Input.defaultIndex).Y < -0.2 &&
                Input.GetPrevThumbs(Input.defaultIndex).Y >= -0.2 &&
                index < gameTypes.Count - 1)
            {
                index++;
                UpdateDescr();
            }
            else if (Input.GetThumbs(Input.defaultIndex).Y > 0.2 &&
               Input.GetPrevThumbs(Input.defaultIndex).Y <= 0.2 &&
                index > 0)
            {
                index--;
                UpdateDescr();
            }

            fingerTimer += dt / 1000;
            if (fingerDirectionRight)
            {
                fingerOffset += (dt / 1000) * 75;
                if (fingerTimer > 0.8)
                {
                    fingerDirectionRight = !fingerDirectionRight;
                    fingerTimer -= 0.8f;
                }
            }
            else
            {
                fingerOffset -= (dt / 1000) * 150;
                if (fingerTimer > 0.4)
                {
                    fingerDirectionRight = !fingerDirectionRight;
                    fingerTimer -= 0.4f;
                }
            }


            if (Input.GetThumbs(Input.defaultIndex).X < -0.2 &&
                Input.GetPrevThumbs(Input.defaultIndex).X >= -0.2)
            {
                MenuSystem.SwitchMenu(new Vector2(Config.screenW, 0), "versus");
            }



        }

        public void UpdateDescr()
        {
            if (index == 0)
            {
                description.Text = "players must work together to get coins and stay alive";
            }
            else if (index == 1)
            {
                description.Text = "similar to survival, but there is only one baby- when thrown, a different player must catch";
            }
        }

        public override void Draw(SpriteBatch sb, GraphicsDevice g)
        {
            base.Draw(sb, g);
            /*
            sb.Begin();

            DrawRegion(sb, 0, 0, Config.screenW, Config.screenH / 8, 
                Color.MediumPurple);
            DrawRegion(sb, Config.screenW / 3, Config.screenH / 8,
                Config.screenW - (Config.screenW / 3), Config.screenH - (Config.screenH / 8), 
                Color.MediumPurple);
            int w = (int)(Config.screenW / 6);
            DrawGradient(sb, Color.Red, Color.MediumPurple, -w / 2, 0, w, Config.screenH / 8);

            sb.End();
            */
            base.DrawElements(sb, g);

            foreach (MenuElement m in gameTypes)
            {
                m.Draw(sb, g, new Color(250, 250, 250));
            }

            description.Draw(sb, g);

            sb.Begin();
            sb.Draw(TextureManager.finger, new Rectangle(
                (int)(gameTypes[index].Position.X + pos.X) + gameTypes[index].Text.Length * GFont.width + (int)(TextureManager.finger.Width * Config.screenR) + (int)(fingerOffset), 
                (int)(gameTypes[index].Position.Y + pos.Y) + (TextureManager.finger.Height / 2),
                (int)(TextureManager.finger.Width * Config.screenR), (int)(TextureManager.finger.Height * Config.screenR)),
                new Rectangle(0,0, TextureManager.finger.Width, TextureManager.finger.Height),
                Color.White, /* rot */ 0,
                new Vector2(TextureManager.finger.Width - TextureManager.finger.Width / 6, TextureManager.finger.Height / 2), 
                SpriteEffects.FlipHorizontally, 0);
            sb.End();
        }

        public void DrawRegion(SpriteBatch sb, float x, float y, float w, float h, Color c)
        {
            sb.Draw(TextureManager.blankTexture, new Rectangle(
                (int)x + (int)pos.X, (int)y + (int)pos.Y,
                (int)w, (int)h), c);
                
        }

        public void DrawGradient(SpriteBatch sb, Color c1, Color c2, float x, float y, float w, float h)
        {
            int steps = 40;
            int[] r = new int[steps];
            int[] g = new int[steps];
            int[] b = new int[steps];
            int step = (int)(w / steps);

            for (int i = 0; i < steps; i++)
            {
                r[i] = c1.R + (((c2.R - c1.R) / steps) * i);
                g[i] = c1.G + (((c2.G - c1.G) / steps) * i);
                b[i] = c1.B + (((c2.B - c1.B) / steps) * i);

                Rectangle rect = new Rectangle((int)(pos.X + x + step * i), (int)(y + pos.Y),
                    step + 1, (int)h);

                sb.Draw(TextureManager.blankTexture, rect, new Color(r[i], g[i], b[i]));
            }
        }
    }
}
