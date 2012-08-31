using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GlowBabyGlow
{
    class Baby : Actor
    {
        int index = 0;
        static int width = 10;
        static int height = 17;
        static float rotSpeed;

        float catchTimer = 0.25f;

        public Rectangle Rect
        {
            get { return rect; }
        }

        public bool ReadyToCatch
        {
            get { return catchTimer == 0; }
        }

        public Baby(Vector2 pos, float xVel, int index) : base()
        {
            gravity = 4;
            this.pos = pos;
            rect = new Rectangle((int)pos.X, (int)pos.Y, width, height);
            velocity.Y = -200;
            velocity.X = xVel;
            this.index = index;
        }

        public override void Update(float dt)
        {
            if (catchTimer > 0)
            {
                catchTimer -= dt / 1000;
            }
            else
            {
                catchTimer = 0;
            }

            base.Update(dt);
        }

        public void Collision(ref List<Tile> tiles)
        {

        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(TextureManager.blankTexture, rect, Color.LimeGreen);

            base.Draw(sb);
        }
    }
}
