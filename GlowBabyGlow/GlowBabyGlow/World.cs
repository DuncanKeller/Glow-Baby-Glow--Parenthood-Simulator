using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GlowBabyGlow
{
    class World
    {
        List<Player> players = new List<Player>();
        List<Tile> tiles = new List<Tile>();
        List<Ladder> ladders = new List<Ladder>();

        EnemyManager enemies;
        BulletManager bullets = new BulletManager();
        CoinManager coins;
        ParticleManager particles = new ParticleManager();

        Backdrop backdrop = new Backdrop();
        Hud hud = new Hud();
        Camera cam = new Camera();

        bool exploded = false;
        float explodeTime = 0;
        float dtMod = 0;
        string levelName;

        #region Properties

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
            get { return exploded; }
        }

        public ParticleManager ParticleManager
        {
            get { return particles; }
        }

        public EnemyManager EnemyManager
        {
            get { return enemies; }
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

        public void Init(string level)
        {
            Tutorial.Init(this);
            backdrop.Init(this);
            hud.Init(this);
            coins = new CoinManager(this);
            enemies = new EnemyManager(this);

            players.Add(new Player(new Point(100,400), this));
            Load(level);
            cam = new Camera();
            cam.Pos = new Vector2(Config.screenW / 2, Config.screenH / 2);
            
        }

        void Load(string filename)
        {
            levelName = filename;
            backdrop.SetStage(filename);
            StreamReader sr = new StreamReader("Maps\\" + filename + ".txt");

            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                string[] info = line.Split(',');
                string type = info[0];
                int x = Int32.Parse(info[1]);
                int y = Int32.Parse(info[2]);

                Point p = new Point(x * Tile.Size, y * Tile.Size);

                switch (type)
                {
                    case "w":
                        tiles.Add(new Tile(p, this));
                        break;
                    case "l":
                        ladders.Add(new Ladder(p, this));
                        break;
                }
            }

            tiles.Add(new Tile(new Point(-Tile.Size, Config.screenH - Tile.Size), this));
            tiles.Add(new Tile(new Point(-Tile.Size * 2, Config.screenH - Tile.Size), this));
            tiles.Add(new Tile(new Point(Config.screenW + Tile.Size, Config.screenH - Tile.Size), this));
            tiles.Add(new Tile(new Point(Config.screenW + Tile.Size * 2, Config.screenH - Tile.Size), this));

            sr.Close();
        }

        public void Update(float dt)
        {
            if (players.Count == 0)
            {
                if (!GameOver.Initialized)
                {
                    GameOver.Init(this);
                }
                GameOver.Update(dt);
            }
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

                        if (p.Baby.ClosestTile < 65 &&
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

                if (p.Lives == 0)
                {
                    toRemove.Add(p);
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
            particles.Update(dt);

            if (Backdrop.Stage == "tutorial")
            {
                Tutorial.Update(dt, Players[0]);
            }
            Backdrop.Update(dt);
            hud.Update(dt);
        }

        public void Reset()
        {
            exploded = false;
            //dtMod = 0;
            enemies.ClearEnemies();
            bullets.ClearBullets();
            coins.ClearCoins();
            tiles.Clear();
            backdrop = new Backdrop();
            GameOver.Reset();
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
            }
        }

        public void Explode()
        {
            exploded = true;
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

            enemies.ClearEnemies();
            bullets.ClearBullets();
            coins.ClearCoins();
        }

        public void Draw(SpriteBatch sb)
        {
            backdrop.Draw(sb);

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
            bullets.Draw(sb);
            coins.Draw(sb);
            particles.Draw(sb);

            hud.Draw(sb);
            if (Backdrop.Stage == "tutorial")
            {
                Tutorial.Draw(sb);
            }

            GameOver.Draw(sb);
        }
    }
}
