using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace GlowBabyGlow
{
    static class TextureManager
    {
        static ContentManager c;
        public static Texture2D blankTexture;
        public static Texture2D testRun;
        public static Texture2D testBaby;
        public static Texture2D baby;
        public static Texture2D babyGlow;
        public static Texture2D blackCircle;

        public static Texture2D tile;

        public static Texture2D bPark;
        public static Texture2D bTutorial;
        public static Texture2D[] clouds = new Texture2D[4];

        public static Texture2D ladder;
        public static Texture2D coin;
        public static Texture2D zombieSheet;

        public static Texture2D font;
        public static Texture2D face;

        public static void Init(ContentManager content)
        {
            c = content;
            // characters
            blankTexture = c.Load<Texture2D>("blank");
            testRun = c.Load<Texture2D>("runsheet");
            testBaby = c.Load<Texture2D>("runsheet-baby");
            baby = c.Load<Texture2D>("Actors\\baby");
            babyGlow = c.Load<Texture2D>("Actors\\glow");
            blackCircle = c.Load<Texture2D>("circle");

            // tiles
            tile = c.Load<Texture2D>("Tiles\\tile");

            // items
            ladder = c.Load<Texture2D>("Objects\\ladder");
            coin = c.Load<Texture2D>("Objects\\coin");

            // enemies
            zombieSheet = c.Load<Texture2D>("Actors\\zombie-sheet");

            // backdrops
            bTutorial = c.Load<Texture2D>("Backdrops\\tutorial");
            bPark = c.Load<Texture2D>("Backdrops\\testBackground");
            clouds[0] = c.Load<Texture2D>("Backdrops\\cloud1");
            clouds[1] = c.Load<Texture2D>("Backdrops\\cloud2");
            clouds[2] = c.Load<Texture2D>("Backdrops\\cloud3");
            clouds[3] = c.Load<Texture2D>("Backdrops\\cloud4");
        
            // fonts
            font = c.Load<Texture2D>("Fonts\\font");
            face = c.Load<Texture2D>("Hud\\face");
        }
    }
}