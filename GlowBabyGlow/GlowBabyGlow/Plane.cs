using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GlowBabyGlow
{
    class Plane : Entity
    {
        Texture2D texture;
        Texture2D smallTexture;

        Vector2 pos = new Vector2();
        float speed = 0;
        float timer;
        int height;
        int width;
        bool moving = false;
        bool speedUp = false;
        bool smallPlane = false;

        Vector2 smallPos = new Vector2();

        public Plane(World w)
            : base(w)
        {
            texture = TextureManager.airplane;
            smallTexture = TextureManager.smallPlane;
            height = (int)(texture.Height * Config.screenR);
            width = (int)(texture.Width * Config.screenR);
            pos.X = -width - 10;
            pos.Y = Config.screenH-height + (Config.screenH / 5);
            speed = 0;
            timer = 5 + Config.rand.Next(30);

            smallPos.X = Config.screenW;
            smallPos.Y = 25;
            
            rect = new Rectangle((int)pos.X, (int)pos.Y, width, height);
        }

        public override void Update(float dt)
        {
            base.Update(dt);
            timer -= dt / 1000;

            if (!smallPlane)
            {
                if (timer <= 0 && !moving)
                {
                    moving = true;
                    timer = 4 + Config.rand.Next(15);
                    speed = 200;
                }

                if (moving)
                {
                    if (!speedUp)
                    {
                        if (speed > 0)
                        {
                            speed -= 20 * (dt / 1000);
                        }
                        else
                        {
                            speed = 0;
                            speedUp = true;
                            moving = false;
                        }
                    }
                    else
                    {
                        speed += 10 * (dt / 1000);

                        if (pos.X - width > Config.screenW)
                        {
                            speed = 0;
                            moving = false;
                            speedUp = false;
                            timer = 10 + Config.rand.Next(5);
                            pos.X = -width - 10;
                            smallPlane = true;
                        }
                    }

                    if (timer <= 0)
                    {
                        moving = true;
                    }
                }

                pos.X += speed * (dt / 1000);
            }
            else
            {
                if (timer <= 0)
                {
                    if (smallPos.X + smallTexture.Width > 0)
                    {
                        smallPos.X -= 35 * (dt / 100);
                        smallPos.Y -= 1.5f * (dt / 100);
                    }
                    else
                    {
                        timer = 5 + Config.rand.Next(30);
                        smallPlane = false;
                        smallPos.Y = 25;
                        smallPos.X = Config.screenW;
                    }
                }
            }

            rect.X = (int)pos.X;
            rect.Y = (int)pos.Y;
        }

        public override void Draw(SpriteBatch sb, SpriteEffects effect)
        {
            sb.Draw(texture, rect, Color.White);
            Rectangle smallRect = new Rectangle((int)smallPos.X, (int)smallPos.Y,
                (int)((smallTexture.Width / 1.5f) * Config.screenR),
                (int)((smallTexture.Height / 1.5f) * Config.screenR));
            sb.Draw(smallTexture, smallRect, Color.Gray);
        }
    }
}