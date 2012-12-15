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

        int smallWidth = Config.screenW / 16;
        int smallHeight = Config.screenH / 16;
        int largeWidth = Config.screenW / 2;
        int largeHeight = Config.screenH / 2;

        World world;
        Viewport vp;

        bool unlocked = false;
        bool recentlyUnlocked = false;
        float unlockTimer = 0.5f;
        float rot;
        float veloc = 0;
        float ypos;
        Dictionary<string, string> levelThatUnlocks = new Dictionary<string, string>();

        string levelname;
        GFont smallfont;
        int neededScore = 18;

        public bool Unlocked
        {
            get { return unlocked; }
        }

        public LevelElement(string text, Texture2D texture, Vector2 pos, bool selectable, Menu m, MenuAction a, string levelname) : 
            base(text, texture, pos, selectable,m, a)
        {
            if (levelname == "alley")
            { unlocked = true; }
            topLeft = pos;
            bottomRight = new Vector2(pos.X + smallWidth, pos.Y + smallHeight);

            topLeftDest = topLeft;
            bottomRightDest = bottomRight;

            world = new World();
            world.Init(levelname);

            vp = new Viewport();
            this.levelname = levelname;
            smallfont = new GFont(TextureManager.smallFont, 4, 10);

            levelThatUnlocks.Add("airport", "alley");
            levelThatUnlocks.Add("jungle", "airport");
            levelThatUnlocks.Add("city", "jungle");
            levelThatUnlocks.Add("powerplant", "city");

            if (levelThatUnlocks.ContainsKey(levelname))
            {
                if (Config.highScore[levelThatUnlocks[levelname]] >= neededScore)
                {
                    unlocked = true;
                }
            }
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

            if (recentlyUnlocked)
            {
                if (unlockTimer > 0)
                {
                    unlockTimer -= dt / 1000;
                }
                else
                {
                    veloc += 10 * (dt / 1000);
                    ypos += veloc;
                    rot += (float)(Math.PI / 4) * (dt / 1000);
                }
            }

            // hack to get the camera working
            if (topLeft.X <= 2)
            {
                world.Cam.Zoom = 1;
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

                if (!unlocked)
                {
                    if (Config.highScore[levelThatUnlocks[levelname]] >= neededScore)
                    {
                        unlocked = true;
                        recentlyUnlocked = true;
                    }
                }
            }
            else
            {
                if (unlocked)
                {
                    topLeftDest = new Vector2(0, 0);
                    bottomRightDest = new Vector2(Config.screenW, Config.screenH);
                }
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

            if (!unlocked || (unlockTimer > 0 && recentlyUnlocked))
            {

                sb.Draw(TextureManager.blankTexture, new Rectangle(
                    drawRect.X, drawRect.Y, drawRect.Width, drawRect.Height), new Color(100,100,100,220));

                float factor = drawRect.Width / (largeWidth * 2.0f);
                int w = (int)(TextureManager.padlockClosed.Width * factor);
                int h = (int)(TextureManager.padlockClosed.Height * factor);
                sb.Draw(TextureManager.padlockClosed, new Rectangle(drawRect.Center.X - (w / 2), drawRect.Center.Y - (h / 2),
                    w, h), Color.White);
            }
            else if (recentlyUnlocked)
            {
                float factor = drawRect.Width / (largeWidth * 2.0f);
                int w = (int)(TextureManager.padlockClosed.Width * factor);
                int h = (int)(TextureManager.padlockClosed.Height * factor);
                int diff = TextureManager.padlockOpen.Height - TextureManager.padlockClosed.Height;
                sb.Draw(TextureManager.padlockOpen, new Rectangle(drawRect.Center.X, (drawRect.Center.Y - (h / 2) + (int)ypos) ,
                    w, h), new Rectangle(0,0,TextureManager.padlockOpen.Width, TextureManager.padlockOpen.Height), Color.White,
                    rot, new Vector2(TextureManager.padlockOpen.Width / 2, TextureManager.padlockOpen.Height / 6), SpriteEffects.None, 0);
            }

            //sb.Draw(TextureManager.blankTexture, new Rectangle(
            //    drawRect.X + drawRect.Width / 8, drawRect.Y + drawRect.Width / 8,
            //    drawRect.Width / 2, drawRect.Height / 2), new Color(50, 50, 50, 50));

            if (Selected)
            {
                if (!unlocked)
                {
                    string t = "";
                    switch (Text)
                    {
                        case "Alley":
                            break;
                        case "Landing Strip":
                            t = "The Alley";
                            break;
                        case "The Outskirts":
                            t = "The Landing Strip";
                            break;
                        case "Ruined City":
                            t = "The Outskirts";
                            break;
                        case "Powerplant":
                            t = "The Ruined City";
                            break;
                    }
                    string text1 = "score " + neededScore + " points on";
                    string text2 = t + " to unlock";
                    smallfont.Draw(sb, new Vector2((Config.screenW / 2) - (((smallfont.Size.X / 2) * text1.Length) / 2),
                        Config.screenH - 15 - (smallfont.Size.Y)), text1, Color.Black, true);
                    smallfont.Draw(sb, new Vector2((Config.screenW / 2) - (((smallfont.Size.X / 2) * text2.Length) / 2),
                        Config.screenH - 15 - (smallfont.Size.Y / 2)), text2, Color.Black, true);
                }
                else
                {
                    font.Draw(sb, new Vector2((Config.screenW / 2) - ((font.Size.X * Text.Length) / 2),
                        Config.screenH - 15 - font.Size.Y), Text, Color.White);
                }
            }

            sb.End();
        }
    }
}
