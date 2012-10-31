using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GlowBabyGlow
{
    class Hud
    {
        GFont font;
        World world;

        public void Init(World w)
        {
            world = w;
            font = new GFont(TextureManager.font, 4, 10);
        }

        public void Update(float dt)
        {

        }

        public void Draw(SpriteBatch sb)
        {
            if (world.Backdrop.Stage != "tutorial")
            {
                if (world.Players.Count > 0)
                {
                    font.Draw(sb, new Vector2(10, 10), "score: " + world.Players[0].Score);
                    Rectangle rect = new Rectangle(Config.screenW - 10 - (font.Size.X * 3), 10,
                        TextureManager.face.Width / 2, TextureManager.face.Height / 2);
                    if (world.Players[0].Lives > 0)
                    {
                        font.Draw(sb, new Vector2(rect.Right, 10), "x" + (world.Players[0].Lives - 1));
                    }
                    sb.Draw(TextureManager.face, rect, Color.White);
                }
            }
        }
    }
}
