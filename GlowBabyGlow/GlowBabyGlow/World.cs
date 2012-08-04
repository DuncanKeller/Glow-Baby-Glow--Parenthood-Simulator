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

        public static List<Player> Players
        {
            get { return players; }
        }

        public static void Init()
        {
            players.Add(new Player(new Point(500,300)));
        }

        public static void Update(float dt)
        {
            foreach (Player p in players)
            {
                p.Update(dt);
            }
        }

        public static void Draw(SpriteBatch sb)
        {
            foreach (Player p in players)
            {
                p.Draw(sb);
            }
        }
    }
}
