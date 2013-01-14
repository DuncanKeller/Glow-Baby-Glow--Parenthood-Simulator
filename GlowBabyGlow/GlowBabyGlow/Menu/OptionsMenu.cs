using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GlowBabyGlow
{
    class OptionsMenu : Menu
    {
        int index = 0;
        bool selectRight = false;
        Texture2D finger;
        float offset;
        float timer;
        int resIndex = 0;
        List<int> xres = new List<int>();
        List<int> yres = new List<int>();

        int oldx;
        int oldy;

        public OptionsMenu(Game1 g)
            : base(g)
        {
            pos = new Vector2(0, Config.screenH * 3);
            backdrop = TextureManager.bDirt;
            finger = TextureManager.finger;

            elements.Add(new MenuElement("resolution", null, GetPos(20, 5),
                true, this, delegate() { }));
            elements.Add(new MenuElement("something", null, GetPos(25, 17),
                true, this, delegate() { }));

            elements.Add(new MenuElement("full screen", null, GetPos(20, 35),
                true, this, delegate() { }));
            elements.Add(new MenuElement(GetText(Config.fullScrn), null, GetPos(25, 47),
                true, this, delegate() { }));

            elements.Add(new MenuElement("music", null, GetPos(20, 65),
                true, this, delegate() { }));
            elements.Add(new MenuElement(GetText(SoundManager.musicOn), null, GetPos(25, 77),
                true, this, delegate() { }));

            elements.Add(new MenuElement("sound", null, GetPos(50, 65),
                true, this, delegate() { }));
            elements.Add(new MenuElement(GetText(SoundManager.soundOn), null, GetPos(55, 77),
                true, this, delegate() { }));


            destination = pos;
            elements[0].Selected = true;
            elements[1].color = new Color(230,230,230);
            elements[3].color = new Color(230, 230, 230);
            elements[5].color = new Color(230, 230, 230);
            elements[7].color = new Color(230, 230, 230);
            elements[1].Text = Config.realW + " X " + Config.realH;

            oldx = Config.realW;
            oldy = Config.realH;

            AddResolutions();
        }

        void AddResolutions()
        {
            xres.Add(1024);
            yres.Add(768);
            xres.Add(1280);
            yres.Add(800);
            xres.Add(1280);
            yres.Add(1024);
            xres.Add(1360);
            yres.Add(768);
            xres.Add(1366);
            yres.Add(768);
            xres.Add(1440);
            yres.Add(900);
            xres.Add(1600);
            yres.Add(900);
            xres.Add(1680);
            yres.Add(1050);
            xres.Add(1920);
            yres.Add(1080);

            for (int i = 0; i < xres.Count; i++)
            {
                if (Config.realW == xres[i] &&
                    Config.realH == yres[i])
                {
                    resIndex = i;
                    break;
                }
            }
        }

        void SetResolution()
        {
            Config.newWidth = xres[resIndex];
            Config.newHeight = yres[resIndex];
            //game.SetRes();
        }

        string GetText(bool b)
        {
            if (b)
            {
                return "yep";
            }
            return "nope";
        }

        Vector2 GetPos(int x, int y)
        {
            return new Vector2((Config.screenW / 100) * x,
                (Config.screenH / 100) * y);
        }

        public void CheckRes()
        {
            if (oldx != xres[resIndex] ||
                oldy != yres[resIndex])
            {
                SetResolution();
            }
        }

        public override void Update(float dt)
        {
            timer += dt / 1000;
            offset = (float)Math.Sin(timer) * 5;
            if (Input.GetThumbs(Input.defaultIndex).Y > 0.5 &&
                Input.GetPrevThumbs(Input.defaultIndex).Y <= 0.5 &&
                index == 0 ||
                Input.HoldingSecondary(Input.defaultIndex) &&
                Input.HoldingSecondaryPrev(Input.defaultIndex))
            {
                CheckRes();
                MenuSystem.SwitchMenu(new Vector2(0, Config.screenH), "single-multi");
            }
            else if (Input.GetThumbs(Input.defaultIndex).Y > 0.5 &&
                Input.GetPrevThumbs(Input.defaultIndex).Y <= 0.5)
            {
                if (index > 0)
                {
                    index--;
                    selectRight = false;
                }
            }
            else if (Input.GetThumbs(Input.defaultIndex).Y < -0.5 &&
                Input.GetPrevThumbs(Input.defaultIndex).Y >= -0.5)
            {
                if (index < 2)
                {
                    index++;
                }
            }
            else if (Input.GetThumbs(Input.defaultIndex).X < -0.5 &&
                Input.GetPrevThumbs(Input.defaultIndex).X >= -0.5)
            {
                if (index == 0)
                {
                    if (resIndex > 0)
                    {
                        resIndex--;
                        elements[1].Text = xres[resIndex] + " X " + yres[resIndex];
                    }
                }
                else  if (index == 1)
                {
                    Config.fullScrn = !Config.fullScrn;
                    elements[3].Text = GetText(Config.fullScrn);
                }
                else if (index == 2)
                {
                    selectRight = false;
                }
            }
            else if (Input.GetThumbs(Input.defaultIndex).X > 0.5 &&
             Input.GetPrevThumbs(Input.defaultIndex).X <= 0.5)
            {
                if (index == 0)
                {
                    if (resIndex < xres.Count - 1)
                    {
                        resIndex++;
                        elements[1].Text = xres[resIndex] + " X " + yres[resIndex];
                    }
                }
                else if (index == 1)
                {
                    Config.fullScrn = !Config.fullScrn;
                    elements[3].Text = GetText(Config.fullScrn);
                }
                else if (index == 2)
                {
                    selectRight = true;
                }
            }

            if (Input.HoldingPrimary(Input.defaultIndex) &&
                !Input.HoldingPrimaryPrev(Input.defaultIndex))
            {
                if (index == 0)
                {
                    if (resIndex < xres.Count - 1)
                    {
                        resIndex++;
                        elements[1].Text = xres[resIndex] + " X " + yres[resIndex];
                    }
                    else
                    {
                        resIndex = 0;
                        elements[1].Text = xres[resIndex] + " X " + yres[resIndex];
                    }
                }
                else if (index == 1)
                {
                    Config.fullScrn = !Config.fullScrn;
                    elements[3].Text = GetText(Config.fullScrn);
                }
                else if (index == 2)
                {
                    if (!selectRight)
                    {
                        SoundManager.musicOn = !SoundManager.musicOn;
                        elements[5].Text = GetText(SoundManager.musicOn);
                    }
                    else
                    {
                        SoundManager.soundOn = !SoundManager.soundOn;
                        elements[7].Text = GetText(SoundManager.soundOn);
                    }
                }
            }

            base.Update(dt);
        }

        public override void Draw(SpriteBatch sb, GraphicsDevice g)
        {
            sb.Draw(backdrop, new Rectangle((int)pos.X, (int)pos.Y, Config.screenW, Config.screenH), Color.White);

            base.Draw(sb, g);

            int w = (int)(finger.Width * Config.screenR);
            int h = (int)(finger.Height * Config.screenR);
            Rectangle r = new Rectangle();
            if (index == 0)
            {
                r = new Rectangle((int)elements[1].Position.X - w,
                    (int)elements[1].Position.Y, w, h);
            }
            else if (index == 1)
            {
                r = new Rectangle((int)elements[3].Position.X - w,
                    (int)elements[3].Position.Y, w, h);
            }
            else if (index == 2)
            {
                if (!selectRight)
                {
                    r = new Rectangle((int)elements[5].Position.X - w,
                       (int)elements[5].Position.Y, w, h);
                }
                else
                {
                    r = new Rectangle((int)elements[7].Position.X - w,
                        (int)elements[7].Position.Y, w, h);
                }
            }

            r.X += (int)(pos.X + offset);
            r.Y += (int)pos.Y;

            sb.Draw(TextureManager.finger, r, Color.White);

           
        }
    }
}
