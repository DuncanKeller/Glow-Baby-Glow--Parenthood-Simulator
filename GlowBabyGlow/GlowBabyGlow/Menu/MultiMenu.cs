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

        public Dictionary<int, int> playerinfo = new Dictionary<int, int>();
        public Dictionary<int, int> playertabPos = new Dictionary<int, int>();

        List<Color> playerColors = new List<Color>();

        public MultiMenu(Game1 g)
            : base(g)
        {
            pos = new Vector2(-Config.screenW, Config.screenH);
            backdrop = TextureManager.blankTexture;
            elements.Add(new MenuElement("press a to join", null, new Vector2(
                0, Config.screenH / 3), true, this, delegate() { }));
            c = Color.Red;
            destination = pos;
            //elements[0].Selected = true;

            playerColors.Add(Color.Green);
            playerColors.Add(Color.Red);
            playerColors.Add(Color.Blue);
            playerColors.Add(Color.Pink);
        }


        public override void Update(float dt)
        {
            base.Update(dt);


            foreach (MenuElement e in elements)
            {
                e.Position = (new Vector2((Config.screenW / 2) -
                    ((GFont.width * e.Text.Length) / 2), e.Position.Y));
            }

            if (Input.HoldingSecondary(0) ||
                Input.GetThumbs(0).X > 0.2)
            {
                MenuSystem.SwitchMenu(new Vector2(-Config.screenW, 0), "single-multi");
            }

            for (int i = 0; i < 4; i++)
            {
                // add player
                if (Input.HoldingPrimary(i) &&
                    !Input.HoldingPrimaryPrev(i))
                {
                    if (!playerinfo.ContainsKey(i))
                    {
                        playertabPos.Add(i, playerinfo.Count);
                        playerinfo.Add(i, playerinfo.Count);
                    }
                    else
                    {
                        MenuSystem.SwitchMenu(new Vector2(Config.screenW, 0), "multi-level");
                    }
                }
                // remove player
                if (Input.HoldingSecondary(i))
                {
                    if (playerinfo.ContainsKey(i))
                    {
                        playertabPos.Remove(i);
                        playerinfo.Remove(i);
                    }
                }
            }

            for (int i = 0; i < playertabPos.Count; i++)
            {
                playertabPos[i] = (int)Vector2.Lerp(new Vector2(playertabPos[i], 0), new Vector2(Config.screenW / 5, 0), 0.2f).X;
            }
        }

        public override void Draw(SpriteBatch sb, GraphicsDevice g)
        {
            base.Draw(sb, g);
            int alpha = 120;

            sb.Begin();

            for (int i = 0; i < playerinfo.Count; i++)
            {
                Rectangle playerTab = new Rectangle(playertabPos[i] - (Config.screenW / 5), (Config.screenH / 8) +
                    ((Config.screenH / 8) * i), 
                    Config.screenW / 5, Config.screenH / 6);

                sb.Draw(TextureManager.blankTexture, playerTab, new Color(0, 0, 0, alpha));

                sb.Draw(TextureManager.face, new Rectangle(playerTab.Right - (playerTab.Width / 10) - (TextureManager.face.Width / 2), playerTab.Top + playerTab.Height / 10,
                    TextureManager.face.Width / 2, TextureManager.face.Height / 2), playerColors[i]);
            }

            sb.End();
        }
    }
}
