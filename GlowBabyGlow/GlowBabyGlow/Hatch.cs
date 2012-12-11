using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GlowBabyGlow
{
    class Hatch : Tile
    {
        float timer = 0;
        bool retract = false;
        int move;
        int distMoved = 0;
        public Hatch(Point p, World w, int move) : base(p,w)
        {
            this.move = move;
            rect = new Rectangle((int)(Tile.Size * p.X * Config.screenR), 
                (int)(Tile.Size * p.Y * Config.screenR),
            (int)(Tile.Size * Config.screenR * 2), 
            (int)(Tile.Size * Config.screenR));
        }

        public bool Retract
        {
            get { return retract; }
            set { retract = value; }
        }

        public override void Draw(SpriteBatch sb, SpriteEffects effect)
        {
            sb.Draw(TextureManager.hatch, rect, Color.DarkGray);
        }

        public override void Update(float dt)
        {
            if (timer > 60 && !retract) // seconds
            {
                retract = true;
            }
            else
            {
                timer += dt / 1000;
            }

            if (distMoved < (Tile.Size * Config.screenR * 2)
                && retract)
            {
                rect.X += 1 * move;
                distMoved += 1;
            }
            base.Update(dt);
        }

    }
}
