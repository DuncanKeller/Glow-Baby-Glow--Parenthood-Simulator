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
        Dictionary<int, Player> players = new Dictionary<int, Player>();
        public Dictionary<int, int> playerinfo = new Dictionary<int, int>();
        public Dictionary<int, int> playertabPos = new Dictionary<int, int>();
        public Dictionary<int, bool> playertabRetract = new Dictionary<int, bool>();

        public List<Color> playerColors = new List<Color>();

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

            playerColors.Add(Color.Green);
            playerColors.Add(Color.Red);
            playerColors.Add(Color.Blue);
            playerColors.Add(Color.Pink);
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
                            Player p = new Player(new Point(Config.screenW + Player.width,
                                Config.screenH - Player.height - 10), null, i);
                            players.Add(i, p);
                        }
                    }
                    else
                    {
                        if (playerinfo.Count > 1)
                        {
                            MenuSystem.SwitchMenu(new Vector2(Config.screenW, 0), "multi-level");
                        }
                    }
                }
                // remove player
                if (Input.HoldingSecondary(i) &&
                    !Input.HoldingSecondaryPrev(i))
                {
                    if (playerinfo.ContainsKey(i))
                    {
                        playerinfo.Remove(i);
                        playertabRetract[i] = true;
                    }
                    else
                    {
                        if (playerinfo.Count == 0)
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
        }

        public override void Draw(SpriteBatch sb, GraphicsDevice g)
        {
            base.Draw(sb, g);
            int alpha = 180;
            int alpha2 = 120;

            sb.Begin();

            foreach (int i in playertabRetract.Keys)
            {
                Rectangle playerTab = new Rectangle(
                    playertabPos[i] - (Config.screenW / 5), (Config.screenH / 8) +
                    ((Config.screenH / 5) * i), 
                    Config.screenW / 5, Config.screenH / 6);

                // tab
                sb.Draw(TextureManager.blankTexture,
                    new Rectangle(playerTab.X, playerTab.Y,
                        playerTab.Width, playerTab.Height / 5),
                    new Color(20, 20, 20, alpha2));

                sb.Draw(TextureManager.blankTexture,
                    new Rectangle(playerTab.X, playerTab.Y + (playerTab.Height / 5),
                        playerTab.Width, (playerTab.Height / 5) * 2 ),
                    new Color(playerColors[i].R, playerColors[i].G, playerColors[i].B, alpha));

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
                sb.Draw(TextureManager.face, new Rectangle(playerTab.Right - (playerTab.Width / 10) -
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

             }

            sb.End();
        }
    }
}
