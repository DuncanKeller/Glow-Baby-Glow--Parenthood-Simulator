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
        World world;
        List<Enemy> enemies = new List<Enemy>();
        List<Enemy> toRemove = new List<Enemy>();
        List<Enemy> toAdd = new List<Enemy>();
        float timer;
        float enemyTime = 3; // seconds
        int spawnDistance = 175;

        public EnemyManager(World w)
        {
            world = w;
        }

        public List<Enemy> Enemies
        {
            get { return enemies; }
        }

        public void ClearEnemies()
        {
            enemies.Clear();
        }

        public void Remove(Enemy e)
        {
            toRemove.Add(e);
        }

        public void Add(Enemy e)
        {
            toAdd.Add(e);
        }

        public void Update(float dt)
        {
            //enemyTime -= (dt / 1000) * 0.05f;
            timer += dt / 1000;

            if (timer > enemyTime)
            {
                if (world.Backdrop.Stage != "tutorial")
                {
                    timer = 0;
                    Spawn();
                    enemyTime -= 0.02f;
                }
            }

            foreach (Enemy e in toRemove)
            {
                enemies.Remove(e);
            }

            foreach (Enemy e in toAdd)
            {
                enemies.Add(e);
            }

            toRemove.Clear();
            toAdd.Clear();

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
                int index = Config.rand.Next(world.Tiles.Count());
                bool colliding = false;

                foreach (Tile t in world.Tiles)
                {
                    if (t != world.Tiles[index])
                    {
                        Rectangle testRect = new Rectangle(world.Tiles[index].Rect.X + 5,
                            world.Tiles[index].Rect.Y + 5 - Tile.Size,
                            world.Tiles[index].Rect.Width - 10,
                            world.Tiles[index].Rect.Height - 10);
                        Rectangle testRect2 = testRect;
                        Rectangle testRect3 = testRect;
                        testRect2.X -= Tile.Size;
                        testRect3.X += Tile.Size;

                        if (t.Rect.Intersects(testRect) ||
                            t.Rect.Intersects(testRect2) ||
                            t.Rect.Intersects(testRect3))
                        {
                            colliding = true;
                        }
                            
                        
                    }
                }

                if (!colliding)
                {
                    bool tooClose = false;
                    foreach (Player p in world.Players)
                    {
                        Vector2 v = new Vector2(world.Tiles[index].Rect.Center.X, world.Tiles[index].Rect.Center.Y);
                        float dist = Vector2.Distance(v, p.Position);
                        if (dist < spawnDistance)
                        {
                            tooClose = true;
                            break;
                        }
                    }
                    if (tooClose)
                    { break; }
                    else
                    {
                        Enemy e = null;
                        int enemyPorb = Config.rand.Next(100);

                        if (enemyPorb < 3)
                        {
                            e = new BossEnemy(new Point(world.Tiles[index].Rect.Center.X,
                                world.Tiles[index].Rect.Y), world);
                        }
                        else if (enemyPorb < 15)
                        {
                            e = new SpeedEnemy(new Point(world.Tiles[index].Rect.Center.X,
                                world.Tiles[index].Rect.Y), world);
                        }
                        else if (enemyPorb < 22)
                        {
                            e = new FatEnemy(new Point(world.Tiles[index].Rect.Center.X,
                                world.Tiles[index].Rect.Y), world);
                        }
                        else
                        {
                            e = new Enemy(new Point(world.Tiles[index].Rect.Center.X,
                                world.Tiles[index].Rect.Y), world);
                        }
                    
                        e.MoveUp();
                        enemies.Add(e);
                        return;
                    }
                }
            }
        }

        public void Draw(SpriteBatch sb)
        {
            foreach (Enemy e in enemies)
            {
                e.Draw(sb, SpriteEffects.None);
            }
        }
    }
}
