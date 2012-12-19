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
        survival,
        vsSurvival,
        hotPotato,
        theif
    }

    class GametypeCoopMenu : Menu
    {
        List<MenuElement> gameTypes = new List<MenuElement>();

       
        public GametypeCoopMenu(Game1 g)
            : base(g)
        {
            pos = new Vector2(-Config.screenW*2, Config.screenH * 2);
            backdrop = TextureManager.bPark;
            elements.Add(new MenuElement("cooperative", null, new Vector2(
                Config.screenW - (GFont.width * "cooperative".Length) - 10, 10), true, this, delegate() { }));
            c = Color.White;
            destination = pos;
            //elements[0].Selected = true;

            gameTypes.Add(new MenuElement("survival", null, new Vector2(
                (Config.screenW / 3) + 10, (Config.screenH / 20) * 4),
                true, this, delegate() { MenuSystem.gameType = GameType.survival; }));
            gameTypes.Add(new MenuElement("hot potato", null, new Vector2(
                (Config.screenW / 3) + 10, (Config.screenH / 20) * 7),
                true, this, delegate() { MenuSystem.gameType = GameType.hotPotato; }));

        }

        public override void Update(float dt)
        {
            base.Update(dt);

            if (Input.HoldingSecondary(Input.defaultIndex) &&
                  !Input.HoldingSecondaryPrev(Input.defaultIndex))
            {
                MenuSystem.SwitchMenu(new Vector2(-Config.screenW, 0), "multi");
            }

            foreach (MenuElement m in gameTypes)
            {
                m.Update(dt);
            }
        }

        public override void Draw(SpriteBatch sb, GraphicsDevice g)
        {
            base.Draw(sb, g);

            sb.Begin();

            DrawRegion(sb, 0, 0, Config.screenW, Config.screenH / 8, 
                Color.MediumPurple);
            DrawRegion(sb, Config.screenW / 3, Config.screenH / 8,
                Config.screenW - (Config.screenW / 3), Config.screenH - (Config.screenH / 8), 
                Color.MediumPurple);

            sb.End();

            base.DrawElements(sb, g);

            foreach (MenuElement m in gameTypes)
            {
                m.Draw(sb, g, new Color(250, 250, 250));
            }
           
        }

        public void DrawRegion(SpriteBatch sb, float x, float y, float w, float h, Color c)
        {
            sb.Draw(TextureManager.blankTexture, new Rectangle(
                (int)x + (int)pos.X, (int)y + (int)pos.Y,
                (int)w, (int)h), c);
                
        }

        public void DrawGradient(SpringParticle sb, Color c1, Color c2, float x, float y, float w, float h)
        {

        }
    }
}
