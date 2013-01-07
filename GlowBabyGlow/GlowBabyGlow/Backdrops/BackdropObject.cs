using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GlowBabyGlow
{
    class BackdropObject
    {
        float paralax;
        Texture2D texture;
        World w;
        float center;
        public BackdropObject(World world, float maxParalaxDistance, Texture2D texture)
        {
            this.texture = texture;
            this.paralax = maxParalaxDistance;
            this.w = world;
            center = Config.screenW / 2;
        }

        public void Update()
        {
            float c = w.Players[0].Position.X;
            for (int i = 1; i < w.Players.Count; i++)
            {
                c = (c + w.Players[i].Position.X) / 2;
            }
            center = Vector2.Lerp(new Vector2(center, 0),
                new Vector2(c, 0), 0.2f).X;
        }

        public void Draw(SpriteBatch sb)
        {
            float factor = center / Config.screenW;
            int x = (int)(factor * paralax);
            sb.Draw(texture, new Rectangle(x, 0, Config.screenW, Config.screenH), Color.White);
        }
    }
}
