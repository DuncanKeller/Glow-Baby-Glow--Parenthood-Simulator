using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GlowBabyGlow
{
    class levelMenu : Menu
    {
        bool locked = false;

        public bool Locked
        {
            get { return locked; }
        }

        public levelMenu(Game1 g)
            : base(g)
        {
            pos = new Vector2(Config.screenW, Config.screenH);
            backdrop = TextureManager.blankTexture;

            c = Color.Green;
            destination = pos;
            //elements[0].Selected = true;
            
            elements.Add(new LevelElement("Alley", TextureManager.blankTexture,
                new Vector2(100, 50), true, this, delegate() { }, "alley"));
            elements.Add(new LevelElement("Landing Strip", TextureManager.blankTexture,
                new Vector2(200, 50), true, this, delegate() { }, "airport"));
            elements.Add(new LevelElement("The Outskirts", TextureManager.blankTexture,
                new Vector2(300, 50), true, this, delegate() { }, "jungle"));
            //elements.Add(new LevelElement("Ghost Town", TextureManager.blankTexture,
            //    new Vector2(400, 50), true, this, delegate() { }, "city"));
            elements.Add(new LevelElement("Powerplant", TextureManager.blankTexture,
                new Vector2(500, 50), true, this, delegate() { }, "powerplant"));

            elements[0].Selected = true;
        }

        public World GetCurrentWorld()
        {
            if (elements[CurrentItem] is LevelElement)
            {
                return (elements[CurrentItem] as LevelElement).GetWorld();
            }
            else
            {
                return null;
            }
        }

        public override void Update(float dt)
        {
            base.Update(dt);

            if (!locked)
            {
                foreach (MenuElement e in elements)
                {
                    if (e.Selected)
                    {
                        e.ChangePosition(new Vector2(1, 0));
                    }
                    else
                    {
                        e.ChangePosition(Vector2.Zero);
                    }
                }
            }
            else
            {
                if ((elements[CurrentItem] as LevelElement).GetCompleted())
                {
                    game.ChangeLevel(GetCurrentWorld().LevelName);
                }
            }

            if (Input.HoldingPrimary(0))
            {
                if (!locked)
                {
                    locked = true;
                    elements[CurrentItem].ChangePosition(new Vector2(1, 1));
                    
                }
            }
        }

        public override void Draw(SpriteBatch sb, GraphicsDevice g)
        {
            base.Draw(sb, g);
        }
    }
}
