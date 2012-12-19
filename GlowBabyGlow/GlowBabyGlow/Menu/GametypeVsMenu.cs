using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GlowBabyGlow
{
    class GametypeVsMenu : Menu
    {
        List<MenuElement> gameTypes = new List<MenuElement>();
        int index = 0;

        public GametypeVsMenu(Game1 g)
            : base(g)
        {
            pos = new Vector2(-Config.screenW * 3, Config.screenH * 2);
            backdrop = TextureManager.bPark;
            elements.Add(new MenuElement("competitive", null, new Vector2(
                10, 10), true, this, delegate() { }));
            c = Color.White;
            destination = pos;
            //elements[0].Selected = true;

            gameTypes.Add(new MenuElement("vs survival", null, new Vector2(
                 ((Config.screenW / 3) * 2) - ("vs survival".Length * GFont.width) - 10, (Config.screenH / 20) * 4),
                true, this, delegate() { MenuSystem.gameType = GameType.vsSurvival; }));
            gameTypes.Add(new MenuElement("theif", null, new Vector2(
                 ((Config.screenW / 3) * 2) - ("theif".Length * GFont.width) - 10, (Config.screenH / 20) * 7),
                true, this, delegate() { MenuSystem.gameType = GameType.theif; }));

        }

        public override void Update(float dt)
        {
            base.Update(dt);

            if (Input.HoldingSecondary(Input.defaultIndex) &&
                  !Input.HoldingSecondaryPrev(Input.defaultIndex))
            {
                MenuSystem.SwitchMenu(new Vector2(-Config.screenW * 2, 0), "multi");
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
            }
            else if (Input.GetThumbs(Input.defaultIndex).Y > 0.2 &&
               Input.GetPrevThumbs(Input.defaultIndex).Y <= 0.2 &&
                index > 0)
            {
                index--;
            }

            if (Input.GetThumbs(Input.defaultIndex).X > 0.2 &&
                Input.GetPrevThumbs(Input.defaultIndex).X <= 0.2)
            {
                MenuSystem.SwitchMenu(new Vector2(-Config.screenW , 0), "coop");
            }

        }

        public override void Draw(SpriteBatch sb, GraphicsDevice g)
        {
            base.Draw(sb, g);

            sb.Begin();

            DrawRegion(sb, 0, 0, Config.screenW, Config.screenH / 8,
                Color.Red);
            DrawRegion(sb, 0, Config.screenH / 8,
                Config.screenW - (Config.screenW / 3), Config.screenH - (Config.screenH / 8),
                Color.Red);

            sb.End();

            base.DrawElements(sb, g);

            foreach (MenuElement m in gameTypes)
            {
                m.Draw(sb, g, new Color(250, 250, 250));
            }

            sb.Begin();
            sb.Draw(TextureManager.babyGlow, new Rectangle(
                (int)(gameTypes[index].Position.X + pos.X), (int)(gameTypes[index].Position.Y + pos.Y), 50, 50),
                Color.Yellow);
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
