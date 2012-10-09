using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GlowBabyGlow
{
    static class Hud
    {
        static GFont font;

        public static void Init()
        {
            font = new GFont(TextureManager.font, 4, 10);
        }

        public static void Update(float dt)
        {

        }

        public static void Draw(SpriteBatch sb)
        {
            if (Backdrop.Stage != "tutorial")
            {
                font.Draw(sb, new Vector2(10, 10), "score: " + World.Players[0].Score);
                Rectangle rect = new Rectangle(Config.screenW - 10 - (font.Size.X * 3), 10,
                    TextureManager.face.Width / 2, TextureManager.face.Height / 2);
                font.Draw(sb, new Vector2(rect.Right, 10), "x" + (World.Players[0].Lives - 1));
                sb.Draw(TextureManager.face, rect, Color.White);
            }
        }
    }
}
