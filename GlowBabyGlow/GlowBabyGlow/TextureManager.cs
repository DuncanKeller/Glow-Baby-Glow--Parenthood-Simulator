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

        public static Texture2D tile;

        public static Texture2D bPark;

        public static Texture2D ladder;
        public static Texture2D coin;
        public static Texture2D zombieSheet;

        public static void Init(ContentManager content)
        {
            c = content;
            blankTexture = c.Load<Texture2D>("blank");
            testRun = c.Load<Texture2D>("runsheet");
            testBaby = c.Load<Texture2D>("runsheet-baby");

            tile = c.Load<Texture2D>("Tiles\\tile");

            ladder = c.Load<Texture2D>("Objects\\ladder");
            coin = c.Load<Texture2D>("Objects\\coin");

            zombieSheet = c.Load<Texture2D>("Actors\\zombie-sheet");

            bPark = c.Load<Texture2D>("Backdrops\\testBackground");
        }
    }
}