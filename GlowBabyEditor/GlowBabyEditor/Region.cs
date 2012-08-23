using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace GlowBabyEditor
{
    class Region 
    {
        Rectangle rect;
        bool mouseOver = false;
        public const int SIZE = 30;
        string type = "e";

        public Region(int x, int y)
        {
            rect = new Rectangle(x, y, SIZE, SIZE);
        }

        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        public int X
        {
            get { return rect.X / SIZE; }
        }

        public int Y
        {
            get { return rect.Y / SIZE; }
        }

        public void Update(MouseState m, MouseState oldm, string symbol)
        {
            Rectangle mouseRect = new Rectangle(m.X, m.Y, 1, 1);
            if (rect.Intersects(mouseRect))
            {
                mouseOver = true;

                if (m.LeftButton == ButtonState.Pressed &&
                    oldm.LeftButton == ButtonState.Released)
                {
                    type = symbol;
                }
                if (m.RightButton == ButtonState.Pressed &&
                    oldm.RightButton == ButtonState.Released)
                {
                    type = "e";
                }
            }
            else
            {
                mouseOver = false;
            }
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(TextureManager.blankTexture, new Rectangle(rect.X, rect.Y, SIZE, 1), Color.White);
            sb.Draw(TextureManager.blankTexture, new Rectangle(rect.X, rect.Y, 1, SIZE), Color.White);

            switch (type)
            {
                case "w":
                    sb.Draw(TextureManager.blankTexture, new Rectangle(rect.X, rect.Y, SIZE, SIZE), Color.Blue);
                    break;
                case "l":
                    sb.Draw(TextureManager.blankTexture, new Rectangle(rect.X, rect.Y, SIZE, SIZE), Color.Yellow);
                    break;
                
            }

            if (mouseOver)
            {
                sb.Draw(TextureManager.blankTexture, new Rectangle(rect.X, rect.Y, SIZE, SIZE), new Color(100,0,100,100));
            }
        }
    }
}
