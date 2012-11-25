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

        public static Texture2D padlockClosed;
        public static Texture2D padlockOpen;

        public static Texture2D tile;

        public static Texture2D bPark;
        public static Texture2D bAlley;
        public static Texture2D bTutorial;
        public static Texture2D bAirport;
        public static Texture2D bPowerplant;
        public static Texture2D bJungle;
        public static Texture2D[] clouds = new Texture2D[4];
        public static Texture2D[] darkClouds = new Texture2D[3];
        public static Texture2D rain;
        public static Texture2D airplane;
        public static Texture2D smallPlane;

        public static Texture2D ladder;
        public static Texture2D coin;
        public static Texture2D zombieSheet;
        public static Texture2D arrow;

        public static Texture2D font;
        public static Texture2D face;
        public static Texture2D smallFont;

        public static Texture2D pupBackdrop;
        public static Texture2D pupIconSpeedshoes;
        public static Texture2D pupIconSpringshoes;
        public static Texture2D pupIconArrow;
        public static Texture2D pupIconPacifier;

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

            //menu
            padlockClosed = c.Load<Texture2D>("Menu\\padlock1");
            padlockOpen = c.Load<Texture2D>("Menu\\padlock2");

            // tiles
            tile = c.Load<Texture2D>("Tiles\\tile");

            // items
            ladder = c.Load<Texture2D>("Objects\\ladder");
            coin = c.Load<Texture2D>("Objects\\coin");
            arrow = c.Load<Texture2D>("Objects\\arrow");

            // powerups
            pupBackdrop = c.Load<Texture2D>("Powerups\\powerup-backdrop");
            pupIconSpeedshoes = c.Load<Texture2D>("Powerups\\sprintshoes");
            pupIconSpringshoes = c.Load<Texture2D>("Powerups\\springshoes");
            pupIconArrow = c.Load<Texture2D>("Powerups\\pup-arrow");
            pupIconPacifier = c.Load<Texture2D>("Powerups\\springshoes");

            // enemies
            zombieSheet = c.Load<Texture2D>("Actors\\zombie-sheet");

            // backdrops
            bTutorial = c.Load<Texture2D>("Backdrops\\tutorial");
            bPark = c.Load<Texture2D>("Backdrops\\testBackground");
            bAlley = c.Load<Texture2D>("Backdrops\\alley");
            bAirport = c.Load<Texture2D>("Backdrops\\airport");
            bJungle = c.Load<Texture2D>("Backdrops\\jungle");
            bPowerplant = c.Load<Texture2D>("Backdrops\\powerplant");
            clouds[0] = c.Load<Texture2D>("Backdrops\\cloud1");
            clouds[1] = c.Load<Texture2D>("Backdrops\\cloud2");
            clouds[2] = c.Load<Texture2D>("Backdrops\\cloud3");
            clouds[3] = c.Load<Texture2D>("Backdrops\\cloud4");
            rain = c.Load<Texture2D>("Backdrops\\rain");
            darkClouds[0] = c.Load<Texture2D>("Backdrops\\darkcloud1");
            darkClouds[1] = c.Load<Texture2D>("Backdrops\\darkcloud2");
            darkClouds[2] = c.Load<Texture2D>("Backdrops\\darkcloud3");
            airplane = c.Load<Texture2D>("Backdrops\\plane");
            smallPlane = c.Load<Texture2D>("Backdrops\\small-plane");
            

            // fonts
            font = c.Load<Texture2D>("Fonts\\font");
            face = c.Load<Texture2D>("Hud\\face");
            smallFont = c.Load<Texture2D>("Fonts\\small-font");
        }
    }
}