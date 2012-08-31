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

        public static List<Player> Players
        {
            get { return players; }
        }

        public static List<Tile> Tiles
        {
            get { return tiles; }
        }

        public static void Init()
        {
            players.Add(new Player(new Point(500,300)));
            Load("kerfuffle");
        }

        static void Load(string filename)
        {
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
            foreach (Player p in players)
            {
                p.Update(dt);
                p.Collision(ref tiles, ref ladders);
            }
            enemies.Update(dt);
            enemies.Collision(ref tiles, ref ladders);
        }

        public static void Draw(SpriteBatch sb)
        {
            foreach (Player p in players)
            {
                p.Draw(sb);
            }

            enemies.Draw(sb);

            foreach (Tile tile in tiles)
            {
                tile.Draw(sb);
            }
            foreach (Ladder l in ladders)
            {
                l.Draw(sb);
            }
        }
    }
}
