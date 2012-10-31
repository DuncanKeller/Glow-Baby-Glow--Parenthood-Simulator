using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GlowBabyGlow
{
    abstract class Menu
    {
        protected Vector2 pos;
        protected List<MenuElement> elements = new List<MenuElement>();
        protected Texture2D backdrop;
        protected Color c;
        protected Game1 game;
        protected Vector2 destination;
        int currentitem = 0;

        public int CurrentItem
        {
            get { return currentitem; }
            set
            {
                if (value >= 0 && value < elements.Count)
                {
                    elements[currentitem].Selected = false;
                    currentitem = value;
                    elements[value].Selected = true;
                }
            }
        }

        public Vector2 Position
        {
            get { return pos; }
        }

        public Menu(Game1 g)
        {
            game = g;
        }

        public virtual void Update(float dt)
        {
            foreach (MenuElement e in elements)
            {
                e.Update(dt);
            }
        }

        public virtual void UpdatePosition(float dt)
        {
            pos = ToDest();
        }

        public void ChangePos(Vector2 dest)
        {
             destination = destination + dest;
        }

        public Vector2 ToDest()
        {
            return Vector2.Lerp(pos, destination, 0.15f);
        }

        public Vector2 ToMove()
        {
            return pos - Vector2.Lerp(pos, destination, 0.15f);
        }

        public void Move(Vector2 v)
        {
            pos += v;
            destination += v;
        }

        public virtual void Draw(SpriteBatch sb, GraphicsDevice g)
        {
            sb.Begin();
            sb.Draw(backdrop, new Rectangle((int)pos.X, (int)pos.Y, Config.screenW, Config.screenH), c);
            sb.End();

            foreach (MenuElement element in elements)
            {
                element.Draw(sb, g);
            }
        }
    }
}