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
        bool multi;
        float boatPos = Config.screenW / 3;
        bool boatMoveRight = true;

        public bool Locked
        {
            get { return locked; }
        }

        public levelMenu(Game1 g, bool multi)
            : base(g)
        {
            this.multi = multi;
            if (multi)
            {
                pos = new Vector2(-Config.screenW * 4, Config.screenH * 2);
                backdrop = TextureManager.bParkPlayground;
            }
            else
            {
                pos = new Vector2(Config.screenW, Config.screenH * 2);
                backdrop = TextureManager.bParkPond;
            }
            
            c = Color.White;
            destination = pos;
            //elements[0].Selected = true;

            float dist = Config.screenW / 6;
            float offset = dist / 3;
            
            elements.Add(new LevelElement("Alley", TextureManager.blankTexture,
                new Vector2(dist - offset, 50), true, this, delegate() { }, "alley"));
            elements.Add(new LevelElement("Landing Strip", TextureManager.blankTexture,
                new Vector2(dist * 2 - offset, 50), true, this, delegate() { }, "airport"));
            elements.Add(new LevelElement("The Outskirts", TextureManager.blankTexture,
                new Vector2(dist * 3 - offset, 50), true, this, delegate() { }, "jungle"));
            elements.Add(new LevelElement("Ruined City", TextureManager.blankTexture,
                new Vector2(dist * 4 - offset, 50), true, this, delegate() { }, "city"));
            elements.Add(new LevelElement("Powerplant", TextureManager.blankTexture,
                new Vector2(dist * 5 - offset, 50), true, this, delegate() { }, "powerplant"));

            elements[0].Selected = true;
        }

        public void Unlock()
        {
            locked = false;

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

            boatPos += (dt / 1000) * 20 * (boatMoveRight ? 1 : -1);

            if (boatPos > Config.screenW + Config.screenW / 20 ||
                boatPos < Config.screenW / 3)
            {
                boatMoveRight = !boatMoveRight;
            }

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

            if (Input.HoldingPrimary(Input.defaultIndex) &&
                !Input.HoldingPrimaryPrev(Input.defaultIndex))
            {
                if (!locked)
                {
                    locked = true;
                    elements[CurrentItem].ChangePosition(new Vector2(1, 1));

                    string level = GetCurrentWorld().LevelName;
                    GetCurrentWorld().Reset();
                    GetCurrentWorld().Init(level);
                    GetCurrentWorld().Automate = false;
                }
            }
            else if (Input.HoldingSecondary(Input.defaultIndex) &&
               !Input.HoldingSecondaryPrev(Input.defaultIndex))
            {
                if (multi)
                {
                    MenuSystem.SwitchMenu(new Vector2(-Config.screenW, 0), "multi");
                }
                else
                {
                    MenuSystem.SwitchMenu(new Vector2(Config.screenW, 0), "single-multi");
                }
            }
        }

        public override void Draw(SpriteBatch sb, GraphicsDevice g)
        {
            base.Draw(sb, g);

            int w = (int)(TextureManager.paperBoat.Width * Config.screenR);
            int h = (int)(TextureManager.paperBoat.Height * Config.screenR);
            if (!multi)
            {
                sb.Begin();
                sb.Draw(TextureManager.paperBoat, new Rectangle(
                    (int)boatPos + (int)pos.X,
                    (Config.screenH - h - (Config.screenH / 70)) + (int)pos.Y,
                    w, h), Color.White);
                sb.End();
            }
            DrawElements(sb, g);
        }
    }
}
