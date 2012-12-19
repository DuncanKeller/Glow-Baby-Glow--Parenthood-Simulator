using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GlowBabyGlow
{
    public delegate void MenuAction();

    class MenuElement
    {
        protected GFont font;
        protected Vector2 pos;
        Vector2 destination;
        protected Texture2D texture;
        string text;
        int width;
        int height;
        protected Menu m;

        bool selected = false;

        MenuAction action;

        public string Text
        {
            get { return text; }
        }

        public Vector2 Position
        {
            get { return pos; }
            set { pos = value; }
        }

        public bool Selected
        {
            get { return selected; }
            set { selected = value; }
        }

        public MenuElement(string text, Texture2D texture, Vector2 pos, bool selectable, Menu m, MenuAction a)
        {
            this.text = text;
            this.texture = texture;
            this.pos = pos;
            destination = pos;
            font = new GFont(TextureManager.font, 4, 10);
            this.m = m;
            action = a;
        }

        public void SetSize(int w, int h)
        {
            width = w;
            height = h;
        }

        public virtual void ChangePosition(Vector2 newPos)
        {
            destination = newPos;
        }

        public virtual void Update(float dt)
        {
            pos = Vector2.Lerp(pos, destination, 0.10f);
        }

        public virtual void Draw(SpriteBatch sb, GraphicsDevice g)
        {
            RealDraw(sb, g, Color.White);
        }

        public virtual void Draw(SpriteBatch sb, GraphicsDevice g, Color c)
        {
            RealDraw(sb, g, c);
        }

        public void RealDraw(SpriteBatch sb, GraphicsDevice g, Color c)
        {
            sb.Begin();

            int s = selected ? 100 : 0;
            if (texture != null)
            {
                sb.Draw(texture, new Rectangle((int)(pos.X + m.Position.X), (int)(pos.Y + m.Position.Y), width, height), c);
            }
            font.Draw(sb, pos + m.Position, text, c);

            sb.End();
        }
    }
}
