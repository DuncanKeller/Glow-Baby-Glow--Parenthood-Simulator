using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace GlowBabyGlow
{
    class World
    {
        bool paused = false;
        bool unpausing = false;
        float pauseCurtain;
        List<Player> players = new List<Player>();
        List<Tile> tiles = new List<Tile>();
        List<Ladder> ladders = new List<Ladder>();
        List<Point> spawnPoints = new List<Point>();
        Dictionary<Player, Baby> babies = new Dictionary<Player, Baby>();

        EnemyManager enemies;
        BulletManager bullets = new BulletManager();
        CoinManager coins;
        ParticleManager particles = new ParticleManager();
        PowerupManager powerups;

        Backdrop backdrop = new Backdrop();
        Hud hud = new Hud();
        Camera cam = new Camera();
        GFont font;

        bool exploded = false;
        float explodeTime = 0;
        float dtMod = 0;
        string levelName;

        #region Properties

        public bool Automate
        {
            set
            {
                foreach (Player p in players)
                {
                    p.Automate = value;
                }
            }
        }

        public bool Paused
        {
            get { return paused; }
            set
            {
                if (value == true)
                {
                    paused = true;
                }
                else
                {
                    unpausing = true;
                }
            }
        }

        public int PlayersAlive
        {
            get
            {
                int alive = 0;
                foreach (Player p in players)
                {
                    if (p.Lives > 0) { alive++; }
                }
                return alive;
            }
        }

        public Dictionary<Player, Baby> Babies
        {
            get
            {
                return babies;
            }
            set
            {
                babies = value;
            }
        }

        public Camera Cam
        {
            get { return cam; }
        }

        public string LevelName
        {
            get { return levelName; }
        }

        public float ExplodeTimer
        {
            get { return explodeTime; }
        }

        public Backdrop Backdrop
        {
            get { return backdrop; }
        }

        public bool Exploding
        {
            get
            {
                if (MenuSystem.gameType == GameType.thief ||
                    MenuSystem.gameType == GameType.vsSurvival)
                {
                    return false;
                }
                else
                {
                    return exploded;
                }
            }
        }

        public ParticleManager ParticleManager
        {
            get { return particles; }
        }

        public EnemyManager EnemyManager
        {
            get { return enemies; }
        }

        public PowerupManager PowerupManager
        {
            get { return powerups; }
        }

        public BulletManager BulletManager
        {
            get { return bullets; }
        }

        public CoinManager CoinManager
        {
            get { return coins; }
        }

        public List<Player> Players
        {
            get { return players; }
        }

        public List<Tile> Tiles
        {
            get { return tiles; }
        }

        #endregion

        public void AddPlayers()
        {
            int[] index = MenuSystem.Players();
            for (int i = 0; i < index.Length; i++)
            {
                AddPlayer(index[i]);
            }
        }

        public void Init(string level)
        {
            font = new GFont(TextureManager.font, 4, 10);
            Tutorial.Init(this);
            backdrop.Init(this);
            hud.Init(this);
            coins = new CoinManager(this);
            enemies = new EnemyManager(this);
            powerups = new PowerupManager(this);

            Load(level);
            cam = new Camera();
            cam.Pos = new Vector2(Config.screenW / 2, Config.screenH / 2);
            AddPlayers();
            //players.Add(new Player(spawnPoints[0], this, 0));
        }

        public void AddPlayer(int index)
        {
            players.Add(new Player(spawnPoints[Players.Count], this, index));
        }

        void Load(string filename)
        {
            levelName = filename;
            backdrop.SetStage(filename);
            StreamReader sr = new StreamReader("Maps\\" + filename + ".txt");

            if (levelName == "powerplant")
            {
                tiles.Add(new Hatch(new Point(14, 13), this, -1));
                tiles.Add(new Hatch(new Point(16, 13), this, 1));
            }

            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                string[] info = line.Split(',');
                string type = info[0];
                int x = Int32.Parse(info[1]);
                int y = Int32.Parse(info[2]);

                Point p = new Point((int)(x * Tile.Size ),
                    (int)(y * Tile.Size));

                switch (type)
                {
                    case "w":
                        tiles.Add(new Tile(p, this));
                        break;
                    case "l":
                        ladders.Add(new Ladder(p, this));
                        break;
                    case "s":
                        spawnPoints.Add(new Point((int)(p.X * Config.screenR),
                             (int)(p.Y * Config.screenR) - Player.height));
                        break;
                }
            }
            sr.Close();

        }

        public void Update(float dt)
        {
            if (!paused)
            {
                if (exploded)
                {
                    UpdateExplosion(dt);
                }
                //dt = 5;
                if (dtMod != 0)
                {
                    dt = dtMod;
                }

                if (!exploded)
                {
                    foreach (Player p in players)
                    {
                        if (p.Baby != null)
                        {
                            if (p.Baby.ClosestTile < 75 * Config.screenR &&
                                p.Baby.Velocity.Y > 0)
                            {
                                dt = p.Baby.ClosestTile / 15;
                            }
                        }
                    }
                }

                List<Player> toRemove = new List<Player>();

                foreach (Player p in players)
                {
                    p.Update(dt);
                    p.Collision(ref tiles, ref ladders);
                }

                foreach (KeyValuePair<Player, Baby> b in babies.ToList())
                {
                    if (b.Value != null)
                    {
                        b.Value.Update(dt);
                    }
                }

                foreach (Player p in toRemove)
                {
                    players.Remove(p);
                }

                foreach (Tile t in tiles)
                {
                    t.Update(dt);
                }

                foreach (Ladder l in ladders)
                {
                    l.Update(dt);
                }

                enemies.Update(dt);
                enemies.Collision(ref tiles, ref ladders);
                bullets.Update(dt);
                bullets.Collision(ref tiles, ref ladders);
                coins.Update(dt);
                powerups.Update(dt);
                powerups.Collision(ref players);
                particles.Update(dt);

                if (Backdrop.Stage == "tutorial")
                {
                    Tutorial.Update(dt, Players[0]);
                }
                Backdrop.Update(dt);
                hud.Update(dt);
                UpdateGameOverScreen(dt);
            }
        }

        public void UpdateGameOverScreen(float dt)
        {
            if (PlayersAlive == 0)
            {
                if (!GameOver.Initialized)
                {
                    GameOver.Init(this);
                }
                GameOver.Update(dt);
            }

            if (GameOver.ReadyToContinue)
            {
                if (Input.HoldingPrimary(0))
                {
                    GameOver.Reset();
                    Reset();
                    Init(levelName);
                    Automate = false;
                }
                else if (Input.HoldingSecondary(0))
                {
                    GameOver.Reset();
                    Reset();
                    Init(levelName);
                    MenuSystem.Reset();
                }
            }
        }

        public void Reset()
        {
            explodeTime = 0;
            exploded = false;
            dtMod = 0;
            enemies.KillEnemies();
            bullets.ClearBullets();
            coins.ClearCoins();
            ladders.Clear();
            particles.ClearParticles();
            tiles.Clear();
            backdrop = new Backdrop();
            GameOver.Reset();
            players.Clear();
        }

        public void UpdateExplosion(float dt)
        {
            explodeTime += dt / 1000;

            if (explodeTime < 1)
            {
                dtMod = 0.0001f;
            }
            else if (explodeTime < 5)
            {
                if (explodeTime < 2)
                {
                    SoundManager.Play(SoundManager.iExplode);
                }
                

                if (dtMod < dt)
                {
                    dtMod += dt / 20;
                }
                else
                {
                    dtMod = dt;
                }
            }
            else
            {
                dtMod = 0;
                if (MenuSystem.gameType == GameType.vsSurvival ||
                MenuSystem.gameType == GameType.thief)
                {
                    exploded = false;
                    explodeTime = 0;
                }
            }
        }

        public void SpawnBaby()
        {
            List<Player> candidates = new List<Player>();
            foreach (Player p in players)
            {
                if (p.Alive)
                {
                    candidates.Add(p);
                }
            }
            if (candidates.Count > 0)
            {
                int index = Config.rand.Next(candidates.Count);
                candidates[index].HoldingBaby = true;
                candidates[index].BabyLife = candidates[index].MaxBabyLife / 2;
            }
        }

        public void Explode()
        {
            if (!exploded)
            {
                SoundEffectInstance splodesfx1 = SoundManager.explosion1.CreateInstance();
                splodesfx1.Play();
            }

            exploded = true;
            if (MenuSystem.gameType != GameType.vsSurvival &&
                MenuSystem.gameType != GameType.thief)
            {
                foreach (Player p in players)
                {
                    p.Explode();
                }
                foreach (Tile t in tiles)
                {
                    (t as Entity).Explode();
                }
                foreach (Ladder l in ladders)
                {
                    (l as Entity).Explode();
                }
                powerups.ClearPowerups();
                //particles.ClearParticles();
                enemies.KillEnemies();
                bullets.ClearBullets();
                coins.ClearCoins();
            }
            else
            {

            }
        }

        public void Draw(SpriteBatch sb)
        {
            backdrop.Draw(sb);

            foreach (Tile tile in tiles)
            {
                tile.DrawBack(sb, SpriteEffects.None);
            }
            foreach (Tile tile in tiles)
            {
                tile.Draw(sb, SpriteEffects.None);
            }
            foreach (Ladder l in ladders)
            {
                l.Draw(sb, SpriteEffects.None);
            }
            foreach (Player p in players)
            {
                p.Draw(sb, SpriteEffects.None); 
            }

            enemies.Draw(sb);
            powerups.Draw(sb);
            bullets.Draw(sb);
            coins.Draw(sb);
            particles.Draw(sb);

            hud.Draw(sb);

            if (Backdrop.Stage == "tutorial")
            {
                Tutorial.Draw(sb);
            }
            if (GameOver.Initialized)
            {
                GameOver.Draw(sb);
            }

            if (paused)
            {
                //sb.Draw(TextureManager.blankTexture, new Rectangle(0, 0, Config.screenW, Config.screenH),
                //    new Color(100, 150, 100, 140));
                int centerx = (int)((Config.screenW / 2) - (font.Size.X * "paused".Length) / 2);
                int centery = (int)((Config.screenH / 2) - (font.Size.Y) / 2);
                if (unpausing)
                {
                    pauseCurtain = Vector2.Lerp(new Vector2(pauseCurtain, 0),
                        new Vector2(0, 0), 0.08f).X;
                    if (pauseCurtain < Config.screenW / 30)
                    {
                        paused = false;
                        unpausing = false;
                    }
                }
                else
                {
                    pauseCurtain = Vector2.Lerp(new Vector2(pauseCurtain, 0), 
                        new Vector2((Config.screenW / 2) + (Config.screenW / 50), 0), 0.045f).X;
                }
                int width = Config.screenW / 2;

                sb.Draw(TextureManager.curtain, new Rectangle((int)pauseCurtain - width, 0, width, Config.screenH), Color.White);
                sb.Draw(TextureManager.curtain, new Rectangle(Config.screenW - (int)pauseCurtain, 0, width, Config.screenH), 
                    new Rectangle(0, 0, TextureManager.curtain.Width, TextureManager.curtain.Height ), Color.White,
                    0, new Vector2(0,0), SpriteEffects.FlipHorizontally, 0 );
                font.Draw(sb, new Vector2(centerx, centery), "paused", Color.White);
                sb.Draw(TextureManager.pauseBorder, new Rectangle(0, 0, Config.screenW, Config.screenH), Color.White);

            }
            else
            {
                pauseCurtain = 0;
            }
        }
    }
}
