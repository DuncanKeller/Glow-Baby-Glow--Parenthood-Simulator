using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GlowBabyGlow
{
    class EnemyManager
    {
        List<Enemy> enemies = new List<Enemy>();
        float timer;
        float enemyTime = 5; // seconds

        public List<Enemy> Enemies
        {
            get { return enemies; }
        }

        public void ClearEnemies()
        {
            enemies.Clear();
        }

        public void Update(float dt)
        {
            timer += dt / 1000;

            if (timer > enemyTime)
            {
                timer = 0;
                Spawn();
            }

            foreach (Enemy e in enemies)
            {
                e.Update(dt);
            }
        }

        public void Collision(ref List<Tile> tiles, ref List<Ladder> ladders)
        {
            foreach (Enemy e in enemies)
            {
                e.Collision(ref tiles, ref ladders);
            }
        }

        public void Spawn()
        {
            while (true)
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
                                Vector2 v = new Vector2(World.Tiles[index].Rect.Center.X, World.Tiles[index].Rect.Center.Y);
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
                                Enemy e = new Enemy(new Point(World.Tiles[index].Rect.Center.X, 
                                    World.Tiles[index].Rect.Y - Enemy.height));
                                enemies.Add(e);
                                return;
                            }
                        }
                    }
                }
            }
        }

        public void Draw(SpriteBatch sb)
        {
            foreach (Enemy e in enemies)
            {
                e.Draw(sb);
            }
        }
    }
}
