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
        List<Coin> coins = new List<Coin>();
        List<Coin> toRemove = new List<Coin>();
        float timer;
        float coinTime = 2; // seconds
        int offset = 10;

        public List<Coin> Coins
        {
            get { return coins; }
        }

        public void Clearcoins()
        {
            coins.Clear();
        }

        public void Remove(Coin e)
        {
            toRemove.Add(e);
        }

        public void Update(float dt)
        {
            timer += dt / 1000;

            if (timer > coinTime)
            {
                timer = 0;
                Spawn();
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
                                Coin e = new Coin(World.Tiles[index].Rect.Center.X,
                                    World.Tiles[index].Rect.Y - Coin.size - offset);
                                coins.Add(e);
                                return;
                            }
                        }
                    }
                }
            }
        }

        public void Draw(SpriteBatch sb)
        {
            foreach (Coin e in coins)
            {
                e.Draw(sb);
            }
        }
    }
}
