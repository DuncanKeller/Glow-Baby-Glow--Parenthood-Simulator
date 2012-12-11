using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GlowBabyGlow
{
    class SingleMultiMenu : Menu
    {
        public SingleMultiMenu(Game1 g)
            : base(g)
        {
            pos = new Vector2(0, Config.screenH * 2);
            backdrop = TextureManager.bPark;
            elements.Add(new MenuElement("single player", null, new Vector2(
                0, Config.screenH / 3), true, this, delegate() { }));
            elements.Add(new MenuElement("multiplayer", null, new Vector2(
                0, (Config.screenH / 3) + (Config.screenH / 6) ), true, this, delegate() { }));
            c = Color.White;
            destination = pos;
            elements[0].Selected = true;
        }

        public override void Update(float dt)
        {
            base.Update(dt);

            foreach (MenuElement e in elements)
            {
                e.Position = (new Vector2((Config.screenW / 2) - 
                    ((GFont.width * e.Text.Length) / 2), e.Position.Y));
            }

            if (Input.GetThumbs(Input.defaultIndex).X > 0.2 &&
                Input.GetPrevThumbs(Input.defaultIndex).X <= 0.2)
            {
                MenuSystem.SwitchMenu(new Vector2(-Config.screenW, 0), "level");
            }
            else if (Input.GetThumbs(Input.defaultIndex).X < -0.2 &&
                Input.GetPrevThumbs(Input.defaultIndex).X >= -0.2)
            {
                MenuSystem.SwitchMenu(new Vector2(Config.screenW, 0), "multi");
            }
        }

        public override void Draw(SpriteBatch sb, GraphicsDevice g)
        {
            base.Draw(sb, g);
        }
    }
}
