using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GlowBabyGlow
{
    class TitleMenu : Menu
    {
        public TitleMenu(Game1 g) : base(g)
        {
            pos = new Vector2(0, 0);
            destination = pos;
            backdrop = TextureManager.blankTexture;
            elements.Add(new MenuElement("press start", null, new Vector2(
                0, Config.screenH - 60), false, this, null));
            c = Color.Yellow;
        }

        
        public override void Update(float dt)
        {
            base.Update(dt);
            elements[0].Position = new Vector2((Config.screenW / 2) - 
                ((GFont.width * "press start".Length) / 2), elements[0].Position.Y);
            if (Input.HoldingPrimary(0) )
            {
                MenuSystem.SwitchMenu(new Vector2(0, -Config.screenH),"single-multi");
            }
        }

        public override void Draw(SpriteBatch sb, GraphicsDevice g)
        {
            base.Draw(sb, g);
        }
    }
}
