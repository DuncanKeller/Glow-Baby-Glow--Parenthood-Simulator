using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GlowBabyGlow
{
    class MultiMenu : Menu
    {
        Dictionary<int, AutomatedPlayer> players = new Dictionary<int, AutomatedPlayer>();
        public Dictionary<int, int> playerinfo = new Dictionary<int, int>();
        public Dictionary<int, int> playertabPos = new Dictionary<int, int>();
        public Dictionary<int, bool> playertabRetract = new Dictionary<int, bool>();

        public MultiMenu(Game1 g)
            : base(g)
        {
            pos = new Vector2(-Config.screenW, Config.screenH * 2);
            backdrop = TextureManager.bParkBandStand;
            elements.Add(new MenuElement("press a to join", null, new Vector2(
                0, Config.screenH / 3), true, this, delegate() { }));
            c = Color.White;
            destination = pos;
            //elements[0].Selected = true;

            
        }

        public int[] GetPlayers()
        {
            int[] info = new int[playerinfo.Count];

            for (int i = 0; i < info.Length; i++)
            {
                info[i] = playerinfo[i];
            }

            return info;
        }

        public override void Update(float dt)
        {
            base.Update(dt);

            foreach (MenuElement e in elements)
            {
                e.Position = (new Vector2((Config.screenW / 2) -
                    ((GFont.width * e.Text.Length) / 2), e.Position.Y));
            }

            for (int i = 0; i < 4; i++)
            {
                // add player
                if (Input.HoldingPrimary(i) &&
                    !Input.HoldingPrimaryPrev(i))
                {
                    if (!playerinfo.ContainsKey(i))
                    {
                        playerinfo.Add(i, playerinfo.Count);
                        if (!playertabPos.ContainsKey(i))
                        {
                            playertabPos.Add(i, playerinfo.Count);
                        }
                        if (!playerinfo.ContainsKey(i))
                        {
                            playertabRetract.Add(i, false);
                        }
                        else
                        {
                            playertabRetract[i] = false;
                        }
                        if (!players.Keys.Contains(i))
                        {
                            AutomatedPlayer p = new AutomatedPlayer(Config.screenW + Player.width,
                                (Config.screenW / 6) * (i + 1), i);
                            players.Add(i, p);
                        }
                        else
                        {
                            players[i].SetDest((Config.screenW / 6) * (i + 1));
                        }
                    }
                    else
                    {
                        if (playerinfo.Count > 1)
                        {
                            MenuSystem.SwitchMenu(new Vector2(Config.screenW, 0), "coop");
                        }
                    }
                }
                // remove player
                if (Input.HoldingSecondary(i) &&
                    !Input.HoldingSecondaryPrev(i) ||
                    Input.GetThumbs(i).X > 0.5f)
                {
                    if (playerinfo.ContainsKey(i))
                    {
                        playerinfo.Remove(i);
                        playertabRetract[i] = true;
                        players[i].SetDest(Config.screenW + (Config.screenW / 20));
                    }
                    else
                    {
                        if (playertabPos.Count == 0)
                        {
                            MenuSystem.SwitchMenu(new Vector2(-Config.screenW, 0), "single-multi");
                        }
                    }
                }
            }

            foreach (int i in playertabRetract.Keys)
            {
                if (playertabRetract[i])
                {
                    playertabPos[i] = (int)Vector2.Lerp(new Vector2(playertabPos[i], 0),
                        new Vector2(0, 0), 0.2f).X;
                }
                else
                {
                    playertabPos[i] = (int)Vector2.Lerp(new Vector2(playertabPos[i], 0),
                        new Vector2(Config.screenW / 5, 0), 0.2f).X;
                }
            }

            foreach (int i in players.Keys)
            {
                players[i].Update(dt);
            }
        }

        public override void Draw(SpriteBatch sb, GraphicsDevice g)
        {
            base.Draw(sb, g);
            int alpha = 180;
            int alpha2 = 120;

            if (MenuSystem.CurrentMenu is MultiMenu)
            {
                foreach (int i in players.Keys)
                {
                    players[i].Draw(sb, pos);
                }
            }

            int count = 0;
            foreach (int i in playertabRetract.Keys)
            {
                Rectangle playerTab = new Rectangle(
                    playertabPos[i] - (Config.screenW / 5), (Config.screenH / 8) +
                    ((Config.screenH / 5) * count), 
                    Config.screenW / 5, Config.screenH / 6);

                // tab
                sb.Draw(TextureManager.blankTexture,
                    new Rectangle(playerTab.X, playerTab.Y,
                        playerTab.Width, playerTab.Height / 5),
                    new Color(20, 20, 20, alpha2));

                sb.Draw(TextureManager.blankTexture,
                    new Rectangle(playerTab.X, playerTab.Y + (playerTab.Height / 5),
                        playerTab.Width, (playerTab.Height / 5) * 2 ),
                    new Color(Config.playerColors[i].R, Config.playerColors[i].G, Config.playerColors[i].B, alpha));

                sb.Draw(TextureManager.blankTexture,
                    new Rectangle(playerTab.X, playerTab.Y + ((playerTab.Height / 5) * 3 ),
                        playerTab.Width, (playerTab.Height / 5) * 2 ),
                    new Color(20, 20, 20, alpha2));

                sb.Draw(TextureManager.blankTexture,
                    new Rectangle(playerTab.X, playerTab.Y - 1, playerTab.Width, 2), Color.Black);
                sb.Draw(TextureManager.blankTexture,
                    new Rectangle(playerTab.X, playerTab.Bottom - 1, playerTab.Width, 2), Color.Black);
                sb.Draw(TextureManager.blankTexture,
                    new Rectangle(playerTab.Right, playerTab.Y - 1, 2, playerTab.Height + 2), Color.Black);

                // icons

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

                sb.Draw(face, new Rectangle(playerTab.Right - (playerTab.Width / 10) -
                    ((int)(TextureManager.face.Width * Config.screenR)),
                    playerTab.Top + playerTab.Height / 10,
                    (int)(TextureManager.face.Width * Config.screenR),
                    (int)(TextureManager.face.Height * Config.screenR)),
                    Color.White);

                sb.Draw(TextureManager.xboxGuide[i], new Rectangle((playerTab.Left + (playerTab.Width / 2)) -
                    ((int)(TextureManager.xboxGuide[i].Width * Config.screenR)),
                    playerTab.Top + playerTab.Height / 10,
                    (int)(TextureManager.face.Height * Config.screenR),
                    (int)(TextureManager.face.Height * Config.screenR)),
                    Color.White);

                count++;
             }

        }
    }
}
