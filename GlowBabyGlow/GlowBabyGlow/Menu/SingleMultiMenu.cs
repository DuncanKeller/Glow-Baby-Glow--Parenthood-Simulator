using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Duncanimation;

namespace GlowBabyGlow
{
    class SingleMultiMenu : Menu
    {
        float leftOffset;
        float rightOffset;
        float bottomOffset;
        float kiteOffset;
        float timer;
        float kiteTimer;
        float amplitude = 10 * Config.screenR;
        float speed = 5;
        GFont font;
        Animator kite;

        public SingleMultiMenu(Game1 g)
            : base(g)
        {
            pos = new Vector2(0, Config.screenH * 2);
            backdrop = TextureManager.bPark;
            elements.Add(new MenuElement("single player", null, new Vector2(
                0, Config.screenH / 3), true, this, delegate() { }));
            elements.Add(new MenuElement("multiplayer", null, new Vector2(
                0, (Config.screenH / 3) + (Config.screenH / 6) ), true, this, delegate() { }));

            kite = new Animator(TextureManager.kite, 1, 5);
            kite.AddAnimation("default", 0, 4, 10, true);
            kite.Play("default");

            font = new GFont(TextureManager.font, 4, 10);
            c = Color.White;
            destination = pos;
            elements[0].Selected = true;
        }

        public override void Update(float dt)
        {
            base.Update(dt);
            timer += (dt / 1000) * speed;
            leftOffset = (float)Math.Sin(timer) * amplitude;
            bottomOffset = (float)Math.Sin(timer) * amplitude;
            rightOffset = -(float)Math.Sin(timer) * amplitude;
            kiteTimer += (dt / 1000) * 0.3f;
            kiteOffset = (float)-Math.Sin(kiteTimer) * 100 * Config.screenR;
            kite.Update(dt);

            foreach (MenuElement e in elements)
            {
                e.Position = (new Vector2((Config.screenW / 2) - 
                    ((GFont.width * e.Text.Length) / 2), e.Position.Y));
            }

            if (Input.GetThumbs(Input.defaultIndex).X > 0.4 &&
                Input.GetPrevThumbs(Input.defaultIndex).X <= 0.4)
            {
                MenuSystem.SwitchMenu(new Vector2(-Config.screenW, 0), "level");
            }
            else if (Input.GetThumbs(Input.defaultIndex).X < -0.4 &&
                Input.GetPrevThumbs(Input.defaultIndex).X >= -0.4)
            {
                MenuSystem.SwitchMenu(new Vector2(Config.screenW, 0), "multi");
            }
            else if (Input.GetThumbs(Input.defaultIndex).Y < -0.4 &&
                Input.GetPrevThumbs(Input.defaultIndex).Y >= -0.4 &&
                Config.includeOptions)
            {
                MenuSystem.SwitchMenu(new Vector2(0, -Config.screenH), "options");
            }
        }

        public override void Draw(SpriteBatch sb, GraphicsDevice g)
        {
           
            base.Draw(sb, g);

            kite.Draw(sb, new Rectangle(
               Config.screenW - (Config.screenW / 8) + (int)pos.X, (int)pos.Y + (Config.screenH / 2) + (int)kiteOffset,
               (int)((TextureManager.kite.Width / 5) * Config.screenR),
                (int)(TextureManager.kite.Height * Config.screenR)),
                Color.White, 0, Vector2.Zero, SpriteEffects.None);

            LineBatch.DrawLineSkinny(sb, Color.Black, new Vector2(
                ((Config.screenW / 100.0f) * 81.9f) + pos.X,
                ((Config.screenH / 100.0f) * 88.1f) + pos.Y), new Vector2(
                    Config.screenW - (Config.screenW / 8) + (int)pos.X + 
                    ((int)((TextureManager.kite.Width / 6.9f) * Config.screenR)),
                    (int)pos.Y + (Config.screenH / 2) + (int)kiteOffset + 
                    (int)(((TextureManager.kite.Height / 2) - (TextureManager.kite.Height / 6)) * Config.screenR)));

            LineBatch.DrawLineSkinny(sb, Color.Black, new Vector2(
              ((Config.screenW / 100.0f) * 79.9f) + pos.X,
              ((Config.screenH / 100.0f) * 87.6f) + pos.Y), new Vector2(
              ((Config.screenW / 100.0f) * 82.0f) + pos.X,
              ((Config.screenH / 100.0f) * 88.1f) + pos.Y));

            LineBatch.DrawLineSkinny(sb, Color.Black, new Vector2(
              ((Config.screenW / 100.0f) * 79.9f) + pos.X,
              ((Config.screenH / 100.0f) * 87.0f) + pos.Y), new Vector2(
              ((Config.screenW / 100.0f) * 80.6f) + pos.X,
              ((Config.screenH / 100.0f) * 86.1f) + pos.Y));

            LineBatch.DrawLineSkinny(sb, Color.Black, new Vector2(
              ((Config.screenW / 100.0f) * 79.1f) + pos.X,
              ((Config.screenH / 100.0f) * 85.7f) + pos.Y), new Vector2(
              ((Config.screenW / 100.0f) * 80.2f) + pos.X,
              ((Config.screenH / 100.0f) * 85.8f) + pos.Y));

            foreach (MenuElement e in elements)
            {
                e.Draw(sb, g);
            }

            if (Config.includeOptions)
            {
                string s = "settings";
                Vector2 fontPos = new Vector2((Config.screenW / 2) - ((s.Length * (font.Size.X / 2)) / 2) + pos.X,
                    ((Config.screenH / 3) + ((Config.screenH / 6) * 2)) + pos.Y);
                font.Draw(sb, fontPos, s, Color.Gray, true);

                sb.Draw(TextureManager.menuArrow,
                 new Rectangle((int)(((Config.screenW / 2) + ((TextureManager.menuArrow.Height / 2 * Config.screenR) / 2)) + pos.X),
                     (int)(fontPos.Y + bottomOffset + (font.Size.Y / 2) + (int)amplitude + 5),
                     (int)(TextureManager.menuArrow.Width / 2 * Config.screenR),
                     (int)(TextureManager.menuArrow.Height / 2 * Config.screenR)),
                     new Rectangle(0, 0, TextureManager.menuArrow.Width, TextureManager.menuArrow.Height),
                     Color.White, (float)Math.PI / 2, Vector2.Zero, SpriteEffects.None, 0);
            }
            
            sb.Draw(TextureManager.menuArrow,
                new Rectangle((int)(elements[0].Position.X + ((elements[0].Text.Length + 1) * GFont.width) + rightOffset + pos.X),
                    (int)(elements[0].Position.Y + pos.Y),
                    (int)(TextureManager.menuArrow.Width * Config.screenR),
                    (int)(TextureManager.menuArrow.Height * Config.screenR)),
                    Color.White);

            sb.Draw(TextureManager.menuArrow,
                new Rectangle((int)(elements[1].Position.X + (-GFont.width) + leftOffset + pos.X),
                    (int)(elements[1].Position.Y + pos.Y),
                    (int)(TextureManager.menuArrow.Width * Config.screenR),
                    (int)(TextureManager.menuArrow.Height * Config.screenR)),
                    new Rectangle(0,0,TextureManager.menuArrow.Width, TextureManager.menuArrow.Height),
                    Color.White, 0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
        }
    }
}