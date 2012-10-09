using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GlowBabyGlow
{
    static class World
    {
        static  List<Player> players = new List<Player>();
        static List<Tile> tiles = new List<Tile>();
        static List<Ladder> ladders = new List<Ladder>();

        static EnemyManager enemies = new EnemyManager();
        static BulletManager bullets = new BulletManager();
        static CoinManager coins = new CoinManager();
        static ParticleManager particles = new ParticleManager();

        static bool exploded = false;
        static float explodeTime = 0;
        static float dtMod = 0;

        #region Properties

        public static float ExplodeTimer
        {
            get { return explodeTime; }
        }

        public static bool Exploding
        {
            get { return exploded; }
        }

        public static ParticleManager ParticleManager
        {
            get { return particles; }
        }

        public static EnemyManager EnemyManager
        {
            get { return enemies; }
        }

        public static BulletManager BulletManager
        {
            get { return bullets; }
        }

        public static CoinManager CoinManager
        {
            get { return coins; }
        }

        public static List<Player> Players
        {
            get { return players; }
        }

        public static List<Tile> Tiles
        {
            get { return tiles; }
        }

        #endregion

        public static void Init()
        {
            Hud.Init();    

            players.Add(new Player(new Point(500,300)));
            Load("tutorial");
            
        }

        static void Load(string filename)
        {
            Backdrop.SetStage(filename);
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
                        tiles.Add(new Tile(p));
                        break;
                    case "l":
                        ladders.Add(new Ladder(p));
                        break;
                }
            }

            tiles.Add(new Tile(new Point(-Tile.Size, Config.screenH - Tile.Size)));
            tiles.Add(new Tile(new Point(-Tile.Size * 2, Config.screenH - Tile.Size)));
            tiles.Add(new Tile(new Point(Config.screenW + Tile.Size, Config.screenH - Tile.Size)));
            tiles.Add(new Tile(new Point(Config.screenW + Tile.Size * 2, Config.screenH - Tile.Size)));

            sr.Close();
        }

        public static void Update(float dt)
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

            if (Players[0].Baby != null)
            {
                if (!exploded)
                {
                    if (Players[0].Baby.ClosestTile < 65 &&
                        Players[0].Baby.Velocity.Y > 0)
                    {
                        dt = Players[0].Baby.ClosestTile / 15;
                    }
                }
            }

            foreach (Player p in players)
            {
                p.Update(dt);
                p.Collision(ref tiles, ref ladders);
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
            Hud.Update(dt);
        }

        public static void UpdateExplosion(float dt)
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

        public static void Explode()
        {
            exploded = true;
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

        public static void Draw(SpriteBatch sb)
        {
            Backdrop.Draw(sb);

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

            Hud.Draw(sb);
            if (Backdrop.Stage == "tutorial")
            {
                Tutorial.Draw(sb);
            }
        }
    }
}
