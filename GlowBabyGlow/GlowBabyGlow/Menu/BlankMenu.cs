using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GlowBabyGlow
{
    class BlankMenu : Menu
    {
        public BlankMenu(Game1 g)
            : base(g)
        {
            pos = new Vector2(0, Config.screenH);
            destination = pos;
            backdrop = TextureManager.bParkSky;
            c = Color.White;
        }


        public override void Update(float dt)
        {
            base.Update(dt);
        }

        public override void Draw(SpriteBatch sb, GraphicsDevice g)
        {
            base.Draw(sb, g);
        }
    }
}
