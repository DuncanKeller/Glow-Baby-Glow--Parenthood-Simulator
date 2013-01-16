using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GlowBabyGlow
{
    class Hud
    {
        GFont font;
        World world;
        int scorePos;

        public void Init(World w)
        {
            world = w;
            font = new GFont(TextureManager.font, 4, 10);
        }

        public void Update(float dt)
        {

        }

        public void Draw(SpriteBatch sb)
        {
            if (world.Backdrop.Stage != "tutorial")
            {
                if (world.Players.Count > 0)
                {
                    if (world.PowerupManager.ShowPowerup)
                    {
                        scorePos = world.PowerupManager.HudDistance + world.PowerupManager.Width;
                        //scorePos = (int)Vector2.Lerp(
                        //    new Vector2(scorePos, 0), new Vector2(10 + world.PowerupManager.Width, 0), 0.3f).X;
                    }
                    else
                    {
                        scorePos = 10;
                        //scorePos = (int)Vector2.Lerp(
                        //    new Vector2(scorePos, 0), new Vector2(10, 0), 0.3f).X;
                    }

                    Rectangle rect = new Rectangle(Config.screenW - 10 - (font.Size.X * 3), 10,
                            (int)((TextureManager.face.Width) * Config.fontRatio),
                            (int)((TextureManager.face.Height) * Config.fontRatio));

                    if (MenuSystem.gameType == GameType.single ||
                        MenuSystem.gameType == GameType.hotPotato ||
                        MenuSystem.gameType == GameType.survival)
                    {
                        font.Draw(sb, new Vector2(scorePos, 10), "score:" + world.Players[0].Score, Color.White);
                    }
                    else
                    {
                        for (int i = 0; i < world.Players.Count; i++)
                        {
                            float x = ((Config.screenW / world.Players.Count) * i) + 20;
                            Texture2D face;
                            switch (i)
                            {
                                case 0:
                                    face = TextureManager.face;
                                    break;
                                case 1:
                                    face = TextureManager.faceSanta;
                                    break;
                                case 2:
                                    face = TextureManager.faceBum;
                                    break;
                                case 3:
                                    face = TextureManager.facePedo;
                                    break;
                                default:
                                    face = TextureManager.face;
                                    break;
                            }

                            sb.Draw(face, new Vector2(x, 20), Color.White);
                            font.Draw(sb, new Vector2(x + face.Width, 20 + (GFont.height / 2)), world.Players[i].Score.ToString(), Color.White, true);
                            
                        }
                    }


                    if (MenuSystem.gameType == GameType.single)
                    {
                        if (world.Players[0].Lives > 0)
                        {
                            font.Draw(sb, new Vector2(rect.Right, 10), "x" + (world.Players[0].Lives - 1), Color.White);
                        }

                        sb.Draw(TextureManager.face, rect, Color.White);

                        world.PowerupManager.DrawIcon(sb);
                    }
                    
                }
            }
        }
    }
}
