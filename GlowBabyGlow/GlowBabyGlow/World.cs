using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GlowBabyGlow
{
    static class World
    {
        static  List<Player> players = new List<Player>();
        static List<Tile> tiles = new List<Tile>();
        static List<Ladder> ladders = new List<Ladder>();

        public static List<Player> Players
        {
            get { return players; }
        }

        public static void Init()
        {
            players.Add(new Player(new Point(500,300)));
            for (int i = 0; i < 20; i++)
            {
                tiles.Add(new Tile(new Point(100 + (i * 30), 420)));
            }
            for (int i = 0; i < 11; i++)
            {
                tiles.Add(new Tile(new Point(100, 420 - (i * 30))));
            }
            for (int i = 0; i < 11; i++)
            {
                tiles.Add(new Tile(new Point(700, 420 - (i * 30))));
            }
        }

        public static void Update(float dt)
        {
            foreach (Player p in players)
            {
                p.Update(dt);
                p.Collision(ref tiles, ref ladders);
            }
        }

        public static void Draw(SpriteBatch sb)
        {
            foreach (Player p in players)
            {
                p.Draw(sb);
            }

            foreach (Tile tile in tiles)
            {
                tile.Draw(sb);
            }
        }
    }
}
