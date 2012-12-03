using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GlowBabyGlow
{
    class Backdrop
    {
        List<Entity> backdrops = new List<Entity>();
        string stage;
        World w;

        public string Stage
        {
            get { return stage; }
        }

        public void Init(World w)
        {
            this.w = w;
        }

        public void SetStage(string s)
        {
            backdrops.Clear();
            stage = s;

            switch (stage)
            {
                case "tutorial":
                    
                    break;
                case "park":
                    for (int i = 0; i < 4; i++)
                    {
                        backdrops.Add(new Cloud(i, w));
                    }
                    break;
                case "airport":
                    
                    backdrops.Add(new Plane(w));
                    for (int i = 0; i < 3; i++)
                    {
                        backdrops.Add(new DarkCloud(i, w));
                    }
                    for (int i = 0; i < 1000; i++)
                    {
                        backdrops.Add(new Rain(w));
                    }
                    break;
            }
        }

        public void Update(float dt)
        {
            foreach (Entity e in backdrops)
            {
                e.Update(dt);
            }
        }

        public void Draw(SpriteBatch sb)
        {
            Texture2D backdrop = null;

            switch (stage)
            {
                case "test":
                    backdrop = TextureManager.bPark;
                    break;
                case "tutorial":
                    backdrop = TextureManager.bTutorial;
                    break;
                case "alley":
                    backdrop = TextureManager.bAlley;
                    break;
                case "park":
                    backdrop = TextureManager.bPark;
                    break;
                case "airport":
                    backdrop = TextureManager.bAirport;
                    break;
                case "jungle":
                    backdrop = TextureManager.bJungle;
                    break;
                case "city":
                    backdrop = TextureManager.bCity;
                    break;
                case "powerplant":
                    backdrop = TextureManager.bPowerplant;
                    break;  
            }

            sb.Draw(backdrop, new Rectangle(0, 0, Config.screenW, Config.screenH), Color.White);

            foreach (Entity e in backdrops)
            { 
                e.Draw(sb, SpriteEffects.None);
            }
        }
    }
}
