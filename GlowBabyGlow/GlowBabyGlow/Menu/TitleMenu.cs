﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GlowBabyGlow
{
    class TitleMenu : Menu
    {
        ParticleManager pm = new ParticleManager();
        float timer = 0;
        float fonttimer = 0;

        class Letter
        {
            float dampening = 0.075f;

            Texture2D texture;
            Vector2 idealPos;
            Vector2 currentPos;
            Vector2 veloc;

            public Letter(Texture2D t, Vector2 cp, Vector2 ip)
            {
                veloc = new Vector2();
                texture = t;
                idealPos = new Vector2(ip.X * (Config.screenW / 100.0f),
                    ip.Y * (Config.screenH / 100.0f));
                currentPos = new Vector2(cp.X * (Config.screenW / 100.0f),
                    cp.Y * (Config.screenH / 100.0f));
            }

            public void Update(float dt)
            {
                veloc.X += ((idealPos.X - currentPos.X) / 2.5f) - (veloc.X * dampening);
                veloc.Y += ((idealPos.Y - currentPos.Y) / 2.5f) - (veloc.Y * dampening);

                if (Math.Abs(veloc.X) < 1
                    && Math.Abs(idealPos.X - currentPos.X) < 1)
                { veloc.X = 0; }
                if (Math.Abs(veloc.Y) < 1
                    && Math.Abs(idealPos.Y - currentPos.Y) < 1)
                { veloc.Y = 0; }

                currentPos.X += veloc.X * dt / 1000;
                currentPos.Y += veloc.Y * dt / 1000;
            }

            public void Draw(SpriteBatch sb, float x, float y)
            {
                sb.Draw(texture, new Rectangle(
                    (int)(currentPos.X + x), 
                    (int)(currentPos.Y + y), 
                    (int)(texture.Width * Config.screenR),
                    (int)(texture.Height * Config.screenR)), Color.White);
            }
        }

        List<Letter> row1 = new List<Letter>();
        List<Letter> row2 = new List<Letter>();
        int letter1height = 60;
        int letter2height = 70;

        public TitleMenu(Game1 g) : base(g)
        {
            pos = new Vector2(0, 0);
            destination = pos;
            backdrop = TextureManager.titleScreen;
            elements.Add(new MenuElement("press start", null, new Vector2(
                0, Config.screenH - GFont.height), false, this, null));
            c = Color.White;

            Init();
        }

        public Vector2 GenPos()
        {
            return new Vector2(
                Config.rand.Next(100),
                Config.rand.Next(100));
        }

        public void Init()
        {
            //glow
            row1.Add(new Letter(TextureManager.l_G,
                GenPos(), new Vector2(24, 37)));
            row1.Add(new Letter(TextureManager.l_L,
                GenPos(), new Vector2(33, 37)));
            row1.Add(new Letter(TextureManager.l_O,
                GenPos(), new Vector2(39, 37)));
            row1.Add(new Letter(TextureManager.l_W,
                GenPos(), new Vector2(48, 37)));

            //baby,
            row1.Add(new Letter(TextureManager.l_B1,
                GenPos(), new Vector2(17, 17)));
            row1.Add(new Letter(TextureManager.l_A,
                GenPos(), new Vector2(25, 17)));
            row1.Add(new Letter(TextureManager.l_B2,
                GenPos(), new Vector2(35, 17)));
            row1.Add(new Letter(TextureManager.l_Y,
                GenPos(), new Vector2(41, 17)));
            row1.Add(new Letter(TextureManager.l_Comma1,
                GenPos(), new Vector2(49, 31)));

            //glow,
            row1.Add(new Letter(TextureManager.l_G,
                GenPos(), new Vector2(10, 0)));
            row1.Add(new Letter(TextureManager.l_L,
                GenPos(), new Vector2(19, 0)));
            row1.Add(new Letter(TextureManager.l_O,
                GenPos(), new Vector2(25, 0)));
            row1.Add(new Letter(TextureManager.l_W,
                GenPos(), new Vector2(34, 0)));
            row1.Add(new Letter(TextureManager.l_Comma1,
                GenPos(), new Vector2(42, 13)));

            int left = -10;
            int right = 105;

            // parenthood
            row2.Add(new Letter(TextureManager.smallLetters["p"],
                new Vector2(left, letter1height), LetterPos1(0)));
            row2.Add(new Letter(TextureManager.smallLetters["a"],
                new Vector2(left, letter1height), LetterPos1(1)));
            row2.Add(new Letter(TextureManager.smallLetters["r"],
                new Vector2(left, letter1height), LetterPos1(2)));
            row2.Add(new Letter(TextureManager.smallLetters["e"],
                new Vector2(left, letter1height), LetterPos1(3)));
            row2.Add(new Letter(TextureManager.smallLetters["n"],
                new Vector2(left, letter1height), LetterPos1(4)));
            row2.Add(new Letter(TextureManager.smallLetters["t"],
                new Vector2(left, letter1height), LetterPos1(5)));
            row2.Add(new Letter(TextureManager.smallLetters["h"],
                new Vector2(left, letter1height), LetterPos1(6)));
            row2.Add(new Letter(TextureManager.smallLetters["o"],
                new Vector2(left, letter1height), LetterPos1(7)));
            row2.Add(new Letter(TextureManager.smallLetters["o"],
                new Vector2(left, letter1height), LetterPos1(8)));
            row2.Add(new Letter(TextureManager.smallLetters["d"],
                new Vector2(left, letter1height), LetterPos1(9)));

            row2.Add(new Letter(TextureManager.smallLetters["s"],
                new Vector2(right, letter2height), LetterPos2(0)));
            row2.Add(new Letter(TextureManager.smallLetters["i"],
                new Vector2(right, letter2height), LetterPos2(1)));
            row2.Add(new Letter(TextureManager.smallLetters["m"],
                new Vector2(right, letter2height), LetterPos2(2)));
            row2.Add(new Letter(TextureManager.smallLetters["u"],
                new Vector2(right, letter2height), LetterPos2(3)));
            row2.Add(new Letter(TextureManager.smallLetters["l"],
                new Vector2(right, letter2height), LetterPos2(4)));
            row2.Add(new Letter(TextureManager.smallLetters["a"],
                new Vector2(right, letter2height), LetterPos2(5)));
            row2.Add(new Letter(TextureManager.smallLetters["t"],
                new Vector2(right, letter2height), LetterPos2(6)));
            row2.Add(new Letter(TextureManager.smallLetters["o"],
                new Vector2(right, letter2height), LetterPos2(7)));
            row2.Add(new Letter(TextureManager.smallLetters["r"],
                new Vector2(right, letter2height), LetterPos2(8)));

        }

        public Vector2 LetterPos1(int i)
        {
            float s = (4.5f );
            return new Vector2(10 + (i * s), letter1height);
        }

        public Vector2 LetterPos2(int i)
        {
            float s = (4.5f );
            return new Vector2(20 + (i * s), letter2height);
        }

        public override void Update(float dt)
        {
            base.Update(dt);
            elements[0].Position = new Vector2((Config.screenW / 2) - 
                ((GFont.width * "press start".Length) / 2), elements[0].Position.Y);
            if (Input.HoldingPrimary(Input.defaultIndex) ||
                Input.StartPressed())
            {
                MenuSystem.SwitchMenu(new Vector2(0, -Config.screenH * 2),"single-multi");
            }

            foreach (Letter l in row1)
            { l.Update(dt); }

            if (fonttimer > 1)
            {
                foreach (Letter l in row2)
                { l.Update(dt); }
            }

            pm.Update(dt);

            fonttimer += dt / 1000;
            timer += dt / 1000;
            if (timer > 0.15f)
            {
                timer = 0;
                pm.AddParticle(new GlowParticle(new Vector2(
                    (Config.screenW / 100.0f) * 86,
                    (Config.screenH / 100.0f) * 15)));
                pm.AddParticle(new GlowParticle(new Vector2(
                   (Config.screenW / 100.0f) * 90,
                   (Config.screenH / 100.0f) * 23)));
            }
        }

        public override void Draw(SpriteBatch sb, GraphicsDevice g)
        {
            base.Draw(sb, g);

            foreach (Letter l in row1)
            { l.Draw(sb, pos.X, pos.Y); }

            foreach (Letter l in row2)
            { l.Draw(sb, pos.X, pos.Y); }

            pm.Draw(sb);
           
        }
    }
}
