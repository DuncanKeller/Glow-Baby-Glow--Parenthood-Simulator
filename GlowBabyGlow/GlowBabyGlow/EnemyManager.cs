using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GlowBabyGlow
{
    class EnemyManager
    {
        List<Enemy> enemies = new List<Enemy>();
        float timer;
        float enemyTime = 6; // seconds

        public void Update(float dt)
        {
            timer += dt / 1000;

            if (timer > enemyTime)
            {
                timer = 0;
            }
        }

        public void Spawn()
        {
            bool spawned = false;

            while (!spawned)
            {
                int index = Config.rand.Next(World.Tiles.Count());

                foreach (Tile t in World.Tiles)
                {
                    if (t != World.Tiles[index])
                    {
                        if (!t.StandingOn(World.Tiles[index].Rect))
                        {
                            bool tooClose = false;
                            foreach (Player p in World.Players)
                            {
                                Vector2 v = new Vector2(t.Rect.Center.X, t.Rect.Center.Y);
                                float dist = Vector2.Distance(v, p.Position);
                                if (dist < 100)
                                {
                                    tooClose = true;
                                    break;
                                }
                            }
                            if (tooClose)
                            { break; }
                            else
                            {
                                Enemy e = new Enemy(new Point(t.Rect.Center.X, t.Rect.Y - Enemy.height));
                                enemies.Add(e);
                                spawned = true;
                            }
                        }
                    }
                }
            }
        }
    }
}
