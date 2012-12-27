using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GlowBabyGlow
{
    class CoinManager
    {
        World world;
        List<Coin> coins = new List<Coin>();
        List<Coin> toRemove = new List<Coin>();
        float timer;
        float coinTime = 6; // seconds
        int offset = 10;

        public CoinManager(World w)
        {
            world = w;
        }

        public List<Coin> Coins
        {
            get { return coins; }
        }

        public void ClearCoins()
        {
            timer = 0;
            coins.Clear();
        }

        public void Remove(Coin e)
        {
            toRemove.Add(e);
        }

        public void Update(float dt)
        {
            timer += dt / 1000;

            if (MenuSystem.gameType != GameType.thief)
            {
                if (timer > coinTime)
                {
                    timer = 0;
                    Spawn();
                }
            }

            foreach (Coin e in toRemove)
            {
                coins.Remove(e);
            }

            foreach (Coin e in coins)
            {
                e.Update(dt);
            }
        }

        public void Collision(ref List<Tile> tiles, ref List<Ladder> ladders)
        {
            foreach (Coin e in coins)
            {
                //e.Collision(ref tiles, ref ladders);
            }
        }

        public void Spawn()
        {
            while (coins.Count < 2)
            {
                int index = Config.rand.Next(world.Tiles.Count());
                bool colliding = false;

                foreach (Tile t in world.Tiles)
                {
                    if (t != world.Tiles[index])
                    {
                        if (world.Tiles[index].Rect.Right > 0 &&
                            world.Tiles[index].Rect.Left < Config.screenW)
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
                }

                if(!colliding)
                {
                     bool tooClose = false;
                                foreach (Player p in world.Players)
                                {
                                    Vector2 v = new Vector2(world.Tiles[index].Rect.Center.X, world.Tiles[index].Rect.Center.Y);
                                    float dist = Vector2.Distance(v, p.Position);
                                    if (dist < 150)
                                    {
                                        tooClose = true;
                                        break;
                                    }
                                }
                                if (tooClose)
                                { break; }
                                else
                                {
                                    Coin e = new Coin(world.Tiles[index].Rect.Center.X,
                                        world.Tiles[index].Rect.Y - Coin.size - offset, world);
                                    coins.Add(e);
                                    return;
                                }
                }
            }
        }

        public void Draw(SpriteBatch sb)
        {
            foreach (Coin e in coins)
            {
                e.Draw(sb, SpriteEffects.None);
            }
        }
    }
}
