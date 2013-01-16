using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GlowBabyGlow
{
    class PowerupManager
    {
        World world;
        List<Powerup> powerups = new List<Powerup>();
        List<Powerup> toRemove = new List<Powerup>();
        List<Powerup> toAdd = new List<Powerup>();
        float timer;
        float powerupTime = 30; // seconds
        int spawnDistance = 175;
        int hudDistance = 0;
        int width;
        int height;
        bool showPowerup = false;
        //float powerupTime = 0; // seconds

        GFont font;

        public PowerupManager(World w)
        {
            world = w;
            font = new GFont(TextureManager.smallFont, 5, 10);
            width = (int)(TextureManager.pupBackdrop.Width * Config.screenR);
            height = (int)(TextureManager.pupBackdrop.Height * Config.screenR);
        }

        public bool ShowPowerup
        {
            get { return showPowerup; }
        }

        public int Width
        {
            get { return width; }
        }

        public int HudDistance
        {
            get { return hudDistance; }
        }

        public List<Powerup> Powerups
        {
            get { return powerups; }
        }

        public void ClearPowerups()
        {
            powerups.Clear();
        }

        public void Remove(Powerup e)
        {
            toRemove.Add(e);
        }

        public void Add(Powerup e)
        {
            toAdd.Add(e);
        }

        public void Update(float dt)
        {
            if (powerups.Count == 0)
            {
                timer += dt / 1000;
            }

            if (timer > powerupTime)
            {
                if (world.Backdrop.Stage != "tutorial")
                {
                    timer = 0;
                    Spawn();
                }
            }

            foreach (Powerup e in toRemove)
            {
                powerups.Remove(e);
            }

            foreach (Powerup e in toAdd)
            {
                powerups.Add(e);
            }

            toRemove.Clear();
            toAdd.Clear();

            foreach (Powerup e in powerups)
            {
                e.Update(dt);
            }
        }

        public void Collision(ref List<Player> players)
        {
            foreach (Powerup pow in powerups)
            {
                foreach (Player p in players)
                {
                    if (p.HitRect.Intersects(pow.Rect))
                    {
                        pow.Activate(p);
                        p.Powerup = pow;
                        toRemove.Add(pow);

                        for (int i = 0; i < Coin.numParticles; i++)
                        {
                            PowerupParticle gp = new PowerupParticle(new Vector2(
                                pow.Rect.Center.X, pow.Rect.Center.Y));
                            world.ParticleManager.AddParticle(gp);
                        }
                    }
                }
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
                        Powerup e;

                        int num = Config.rand.Next(5);
                        int x = world.Tiles[index].Rect.Center.X;
                        int y = world.Tiles[index].Rect.Y;

                        switch (num)
                        {
                            case 0:
                                e = new Pacifier(x, y, world);
                                break;
                            case 1:
                                e = new PiercingShot(x, y, world);
                                break;
                            case 2:
                                e = new SpringShoes(x, y, world);
                                break;
                            case 3:
                                e = new SpeedShoes(x, y, world);
                                break;
                            case 4:
                                e = new SpeedShoes(x, y, world);
                                break;
                            default:
                                e = new SpeedShoes(x, y, world);
                                break;
                        }

                        powerups.Add(e);
                        return;
                    }
                }
            }
        }

        public void DrawIcon(SpriteBatch sb)
        {
            showPowerup = false;
            if(world.Players.Count > 1)
            { return; }
            foreach (Player p in world.Players)
            {
                int alpha = 254;
                
                if (p.Powerup != null)
                {
                   
                    int maxCharge = (int)(width / (p.Powerup.MaxAliveTime));
                    int charge = (int)(maxCharge * p.Powerup.AliveTimer) + 50;
                    if (charge - width > 0)
                    {
                        int newMax = (int)(maxCharge * p.Powerup.MaxAliveTime) + 50;
                        charge = width - (width / ((newMax - charge) + 1));
                    }

                    // check for player overlapping 
                    if (p.HitRect.Intersects(new Rectangle(charge - width, 0, width, height)))
                    {
                        alpha = 100;
                    }

                    if (charge > 0)
                    {
                        showPowerup = true;
                    }

                    sb.Draw(TextureManager.pupBackdrop, new Rectangle(charge - width, 0, width, height),
                        new Color(alpha,alpha,alpha,alpha));
                    int xoffset = Config.screenW / 25;
                    int yoffset = Config.screenW / 30;
                    // change width to p.Powerup.Icon.width
                    hudDistance = (charge - width);
                    sb.Draw(p.Powerup.Icon, new Rectangle((charge - width) + xoffset, yoffset,
                        p.Powerup.Icon.Width / 2, p.Powerup.Icon.Height / 2), 
                        new Color(alpha, alpha, alpha, alpha));
                    xoffset = (Config.screenW / 25);
                    yoffset = Config.screenW / 65;
                    font.Draw(sb, new Vector2((charge - width) + (xoffset / 3), yoffset), p.Powerup.Description,
                         new Color(alpha, alpha, alpha, alpha), true);
                }
            }
        }

        public void Draw(SpriteBatch sb)
        {
            foreach (Powerup e in powerups)
            {
                e.Draw(sb, SpriteEffects.None);
            }
        }
    }
}
