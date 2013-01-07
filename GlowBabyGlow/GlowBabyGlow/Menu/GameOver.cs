using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GlowBabyGlow
{
    static class GameOver
    {
        static World world;
        static Rectangle rect;
        static float position;
        static float velocity = 0;
        static GFont font;
        static bool initialized = false;
        static float startTimer;

        static int score;
        static bool newScore;

        static List<int> winnerNums = new List<int>();
        static Dictionary<int, int> scores = new Dictionary<int, int>();

        public static bool Initialized
        {
            get { return initialized; }
        }

        public static bool ReadyToContinue
        {
            get
            {
                return (initialized == true && rect.Bottom > Config.screenH / 2);
            }
        }

        public static void Init(World w)
        {
            newScore = false;
            startTimer = 1.5f;
            initialized = true;
            font = new GFont(TextureManager.font, 4, 10);
            position = -Config.screenH;
            rect = new Rectangle(0, -Config.screenH, Config.screenW, Config.screenH);
            world = w;
            world.Automate = false;
            CheckHighScore();
            EvaluateWinner();
        }

        public static void CheckHighScore()
        {
            if (world != null)
            {
                if (MenuSystem.gameType == GameType.single)
                {
                    score = world.Players[0].Score;

                    if (Config.highScore[world.LevelName] < score)
                    {
                        Config.highScore[world.LevelName] = score;
                        newScore = true;
                    }
                }
            }
        }

        public static void EvaluateWinner()
        {
            // see who won in a competitive match
            winnerNums.Clear();
            scores.Clear();
            int highscore = -1;
            Player winner = null;

            bool tie = false;
            List<Player> otherWinners = new List<Player>();

            foreach (Player p in world.Players)
            {
                if (p.Score > highscore)
                {
                    highscore = p.Score;
                    winner = p;
                }
                else if (p.Score == highscore)
                {
                    otherWinners.Add(p);
                    tie = true;
                }
                scores.Add(p.Index, p.Score);
            }

            winnerNums.Add(winner.Index);

            foreach (Player p in otherWinners)
            {
                winnerNums.Add(p.Index);
            }
        }

        public static void Reset()
        {
            position = -Config.screenH;
            rect.Y = (int)position;
            initialized = false;
            velocity = 0;
            CheckHighScore();
            Input.spaceBarPreventativeMeasureFlag = false;
        }

        public static void Update(float dt)
        {
            if (startTimer == 0)
            {
                if (Math.Abs(velocity) < 1 &&
                    position >= 0)
                {
                    velocity = 0;
                    position = 0;
                }
                else if (rect.Bottom > Config.screenH)
                {
                    velocity = -(velocity - (velocity / 4f));
                }
                else
                {
                    velocity += (65 * 2) * Config.screenR;
                }

                position += velocity * (dt / 1000);
                rect.Y = (int)position;
            }
            else
            {
                if (startTimer > 0)
                { startTimer -= dt / 1000; }
                else
                { startTimer = 0; }
            }
        }

        public static void Draw(SpriteBatch sb)
        {
            sb.Draw(TextureManager.blankTexture, rect, new Color(25, 25, 25));
            sb.Draw(TextureManager.blankTexture, new Rectangle(
                0, 0, Config.screenW, rect.Top), new Color(25, 25, 25));

            if (MenuSystem.gameType == GameType.single ||
                MenuSystem.gameType == GameType.hotPotato ||
                MenuSystem.gameType == GameType.survival)
            {
                Vector2 pos = new Vector2(
                    (Config.screenW / 2) - ((font.Size.X * "game over".Length) / 2),
                    (Config.screenH / 2) - (Config.screenH / 6) + position);
                font.Draw(sb, pos, "game over", Color.Red);
                string scoreText = "final score: " + score;
                Vector2 scorePos = new Vector2(
                    (Config.screenW / 2) - (((font.Size.X / 2) * scoreText.Length) / 2),
                    (Config.screenH / 2) - (Config.screenH / 6) + position + GFont.height);
                font.Draw(sb, scorePos, scoreText, new Color(254, 254, 254), true);
            }
            else
            {
                Rectangle r = new Rectangle(
                    Config.screenW / 8,
                    (int)((Config.screenH - (int)((TextureManager.winPose[winnerNums[0]].Height * Config.screenR))) + position),
                    (int)(TextureManager.winPose[winnerNums[0]].Width * Config.screenR),
                    (int)((TextureManager.winPose[winnerNums[0]].Height * Config.screenR)));

                string text = "player " + (winnerNums[0] + 1) + " wins";
                if (winnerNums.Count > 1)
                { text = "tie game"; }

                Vector2 pos = new Vector2(
                 (Config.screenW / 2) - ((font.Size.X * text.Length) / 2),
                 (Config.screenH / 20) + position);

                Color c = new Color(254, 254, 254);
                if (winnerNums.Count == 1)
                { c = Config.playerColors[winnerNums[0]]; }
                font.Draw(sb, pos, text, c);

                for (int i = 0; i < winnerNums.Count; i++)
                {
                    sb.Draw(TextureManager.winPose[winnerNums[i]], r, Color.White);
                    r.X += Config.screenW / 14;
                }

                for (int i = 0; i < scores.Count; i++)
                {
                    int[] scoresArr = new int[scores.Count];
                    scores.Keys.CopyTo(scoresArr, 0);

                    Vector2 facePos = new Vector2(Config.screenW - TextureManager.face.Width - 20,
                        Config.screenH - ((i + 1) * TextureManager.face.Height) - ((i + 1) * 20) + position);

                    Vector2 scorePos = new Vector2(Config.screenW - TextureManager.face.Width - 40 - (scores[scoresArr[i]].ToString().Length * (GFont.width / 2)),
                       Config.screenH - ((i + 1) * TextureManager.face.Height) - ((i + 1) * 20) + position + (GFont.height / 2));

                    Texture2D face;

                   
                    switch (scoresArr[i])
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

                    sb.Draw(face, facePos, Color.White);
                    font.Draw(sb, scorePos, scores[scoresArr[i]].ToString(), Color.White, true);
                }

            }
        }
    }
}
