using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GlowBabyGlow
{
    class LevelElement : MenuElement
    {
        Vector2 topLeft;
        Vector2 bottomRight;

        Vector2 topLeftDest;
        Vector2 bottomRightDest;

        int smallWidth = Config.screenW / 20;
        int smallHeight = Config.screenH / 20;
        int largeWidth = Config.screenW / 2;
        int largeHeight = Config.screenH / 2;

        World world;
        Viewport vp;

        public LevelElement(string text, Texture2D texture, Vector2 pos, bool selectable, Menu m, MenuAction a, string levelname) : 
            base(text, texture, pos, selectable,m, a)
        {
            topLeft = pos;
            bottomRight = new Vector2(pos.X + smallWidth, pos.Y + smallHeight);

            topLeftDest = topLeft;
            bottomRightDest = bottomRight;

            world = new World();
            world.Init(levelname);

            vp = new Viewport();
        }

        public World GetWorld()
        {
            return world;
        }

        public bool GetCompleted()
        {
            if (bottomRight.X > bottomRightDest.X - 1 &&
                bottomRightDest.X == Config.screenW)
            {
                return true;
            }
            return false;
        }


        public override void Update(float dt)
        {
            topLeft = Vector2.Lerp(topLeft, topLeftDest, 0.10f);
            bottomRight = Vector2.Lerp(bottomRight, bottomRightDest, 0.10f);
            if (Selected)
            {
                world.Update(dt);
            }
        }
        
        public override void ChangePosition(Vector2 newPos)
        {
            if (newPos == Vector2.Zero)
            {
                topLeftDest = pos;
                bottomRightDest = new Vector2(pos.X + smallWidth, pos.Y + smallHeight);
            }
            else if (newPos.Y == 0)
            {
                topLeftDest = new Vector2((Config.screenW / 2) - (largeWidth / 2),
                    (Config.screenH / 2) - (largeHeight / 2));
                bottomRightDest = new Vector2(topLeftDest.X + largeWidth, topLeftDest.Y + largeHeight);
            }
            else
            {
                topLeftDest = new Vector2(0, 0);
                bottomRightDest = new Vector2(Config.screenW, Config.screenH);
            }
        }

        public override void Draw(SpriteBatch sb, GraphicsDevice g)
        {
            Rectangle drawRect = new Rectangle((int)(topLeft.X + m.Position.X), 
                (int)(topLeft.Y + m.Position.Y),
                (int)(bottomRight.X - topLeft.X), (int)(bottomRight.Y - topLeft.Y));

            if (drawRect.Right > Config.screenW ||
                drawRect.X < 0 || drawRect.Y < 0 ||
                drawRect.Bottom > Config.screenH)
            { return; }

            Viewport temp = g.Viewport;

            world.Cam.Zoom = (float)drawRect.Width / (float)Config.screenW;

            vp.Bounds = drawRect;
            g.Viewport = vp;

            sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp,
               DepthStencilState.Default, RasterizerState.CullNone, null,
               world.Cam.get_transformation(g));

            sb.Draw(texture, drawRect, Color.White);
            world.Draw(sb);

            sb.End();

            g.Viewport = temp;

            // outisde of viewport
            sb.Begin();

            sb.Draw(TextureManager.blankTexture, new Rectangle(
                drawRect.X - 2, drawRect.Y - 2, drawRect.Width + 4, 4), Color.Black);
            sb.Draw(TextureManager.blankTexture, new Rectangle(
                drawRect.X - 2, drawRect.Bottom - 2, drawRect.Width + 4, 4), Color.Black);
            sb.Draw(TextureManager.blankTexture, new Rectangle(
                drawRect.X - 2, drawRect.Y - 2, 4, drawRect.Height + 4), Color.Black);
            sb.Draw(TextureManager.blankTexture, new Rectangle(
                drawRect.Right - 2, drawRect.Y - 2, 4, drawRect.Height + 4), Color.Black);

            //sb.Draw(TextureManager.blankTexture, new Rectangle(
            //    drawRect.X + drawRect.Width / 8, drawRect.Y + drawRect.Width / 8,
            //    drawRect.Width / 2, drawRect.Height / 2), new Color(50, 50, 50, 50));

            if (Selected)
            {
                font.Draw(sb, new Vector2((Config.screenW / 2) - ((font.Size.X * Text.Length) / 2),
                    Config.screenH - 15 - font.Size.Y), Text, Color.White);
            }

            sb.End();
        }
    }
}
