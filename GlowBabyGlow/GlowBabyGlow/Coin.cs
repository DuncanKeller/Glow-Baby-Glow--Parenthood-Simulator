﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GlowBabyGlow
{
    class Coin : Entity
    {
        double offset = 0;
        float time;
        public static int size = 30;
        int magnitude = 5;
        int startingHeight;

        public Coin(int x, int y)
            : base()
        {
            rect = new Rectangle(x, y, size, size);
            startingHeight = y;
        }

        public override void Update(float dt)
        {
            time += dt / 500;
            rect.Y = startingHeight + (int)offset;
            offset = Math.Sin(time) * magnitude;
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(TextureManager.coin, rect, Color.White);
        }
    }
}