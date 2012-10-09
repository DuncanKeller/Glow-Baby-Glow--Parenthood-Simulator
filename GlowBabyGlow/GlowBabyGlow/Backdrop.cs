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
        static List<Entity> backdrops = new List<Entity>();
        static string stage;

        public static string Stage
        {
            get { return stage; }
        }

        public static void SetStage(string s)
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
                        backdrops.Add(new Cloud(i));
                    }
                    break;
            }
        }

        public static void Update(float dt)
        {
            foreach (Entity e in backdrops)
            {
                e.Update(dt);
            }
        }

        public static void Draw(SpriteBatch sb)
        {
            Texture2D backdrop = null;

            switch (stage)
            {
                case "tutorial":
                    backdrop = TextureManager.bTutorial;
                    break;
                case "park":
                    backdrop = TextureManager.bPark;
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
