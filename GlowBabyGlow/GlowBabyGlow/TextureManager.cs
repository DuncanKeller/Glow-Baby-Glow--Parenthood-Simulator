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
        public static bool loaded = false;
        static ContentManager c;
        static LoadAction finishLoad;

        public static Texture2D[] letterbox = new Texture2D[5];

        public static Texture2D blankTexture;
        public static Texture2D testRun;
        public static Texture2D testBaby;
        public static Texture2D baby;
        public static Texture2D babyGlow;
        public static Texture2D blackCircle;
        public static Texture2D glowParticle;
        public static Texture2D santa;
        public static Texture2D santaBaby;
        public static Texture2D bum;
        public static Texture2D bumBaby;
        public static Texture2D pedo;
        public static Texture2D pedoBaby;
        public static Texture2D throwArrow;
        public static Texture2D reloading;

        public static Texture2D titleScreen;
        public static Texture2D pauseBorder;
        public static Texture2D curtain;
        public static Texture2D menuArrow;

        public static Texture2D face;
        public static Texture2D faceBum;
        public static Texture2D facePedo;
        public static Texture2D faceSanta;
        public static Texture2D[] winPose = new Texture2D[4];

        public static Texture2D padlockClosed;
        public static Texture2D padlockOpen;
        public static Texture2D newLevelUnlock;

        public static Texture2D tile;
        public static Texture2D hatch;

        public static Texture2D[] xboxGuide = new Texture2D[4];
        public static Texture2D finger;

        public static Texture2D l_G;
        public static Texture2D l_L;
        public static Texture2D l_O;
        public static Texture2D l_W;
        public static Texture2D l_B1;
        public static Texture2D l_A;
        public static Texture2D l_B2;
        public static Texture2D l_Y;
        public static Texture2D l_Comma1;
        public static Texture2D l_Comma2;

        public static Dictionary<string, Texture2D> smallLetters = new Dictionary<string, Texture2D>();

        public static Texture2D kite;
        public static Texture2D bPark;
        public static Texture2D bParkSky;
        public static Texture2D bParkPond;
        public static Texture2D bParkPlayground;
        public static Texture2D bParkWildernessLeft;
        public static Texture2D bParkWildernessRight;
        public static Texture2D bParkBandStand;
        public static Texture2D bAlley;
        public static Texture2D bTutorial;
        public static Texture2D bAirport;
        public static Texture2D bCity;
        public static Texture2D bPowerplant;
        public static Texture2D bJungle;
        public static Texture2D[] clouds = new Texture2D[4];
        public static Texture2D[] darkClouds = new Texture2D[3];
        public static Texture2D rain;
        public static Texture2D airplane;
        public static Texture2D smallPlane;
        public static Texture2D[] parxAirport = new Texture2D[6];
        public static Texture2D[] parxJungle = new Texture2D[6];
        public static Texture2D paperBoat;
        public static Texture2D bDirt;

        public static Texture2D ladder;
        public static Texture2D coin;
        public static Texture2D zombieSheet;
        public static Texture2D zombieSpeedy;
        public static Texture2D zombieFat;
        public static Texture2D zombieLarge;
        public static Texture2D arrow;

        public static Texture2D font;
        public static Texture2D smallFont;

        public static Texture2D pupBackdrop;
        public static Texture2D pupIconSpeedshoes;
        public static Texture2D pupIconSpringshoes;
        public static Texture2D pupIconArrow;
        public static Texture2D pupIconArrowWhite;
        public static Texture2D pupIconPacifier;

        public static void Init(ContentManager content)
        {
            c = content;
        }

        public static void LoadContent(LoadAction a)
        {
            finishLoad = a;

            letterbox[0] = c.Load<Texture2D>("Tiles\\letterbox1");
            letterbox[1] = c.Load<Texture2D>("Tiles\\letterbox2");
            letterbox[2] = c.Load<Texture2D>("Tiles\\letterbox3");
            letterbox[3] = c.Load<Texture2D>("Tiles\\letterbox4");
            letterbox[4] = c.Load<Texture2D>("Tiles\\letterbox5");

            // characters
            blankTexture = c.Load<Texture2D>("blank");
            testRun = c.Load<Texture2D>("Actors\\runsheet");
            testBaby = c.Load<Texture2D>("Actors\\runsheet-baby");
            baby = c.Load<Texture2D>("Actors\\baby");
            babyGlow = c.Load<Texture2D>("Actors\\glow");
            blackCircle = c.Load<Texture2D>("circle");
            glowParticle = c.Load<Texture2D>("fuzz");
            santa = c.Load<Texture2D>("Actors\\sprite-santa");
            santaBaby = c.Load<Texture2D>("Actors\\sprite-santa-baby");
            bum = c.Load<Texture2D>("Actors\\sprite-bum");
            bumBaby = c.Load<Texture2D>("Actors\\sprite-bum-baby");
            pedo = c.Load<Texture2D>("Actors\\sprite-pedo");
            pedoBaby = c.Load<Texture2D>("Actors\\sprite-pedo-baby");
            throwArrow = c.Load<Texture2D>("Actors\\throwArrow");
            reloading = c.Load<Texture2D>("Actors\\reloading");

            //menu
            titleScreen = c.Load<Texture2D>("Menu\\title-screen");
            padlockClosed = c.Load<Texture2D>("Menu\\padlock1");
            padlockOpen = c.Load<Texture2D>("Menu\\padlock2");
            pauseBorder = c.Load<Texture2D>("Menu\\border");
            curtain = c.Load<Texture2D>("Menu\\curtain");
            finger = c.Load<Texture2D>("Menu\\finger");
            menuArrow = c.Load <Texture2D>("Menu\\menu-arrow");
            newLevelUnlock = c.Load<Texture2D>("Menu\\newLevel");

            face = c.Load<Texture2D>("Hud\\face");
            faceBum = c.Load<Texture2D>("Hud\\face-bum");
            facePedo = c.Load<Texture2D>("Hud\\face-pedo");
            faceSanta = c.Load<Texture2D>("Hud\\face-santa");
            winPose[0] = c.Load<Texture2D>("Menu\\win-normal");
            winPose[1] = c.Load<Texture2D>("Menu\\win-santa");
            winPose[2] = c.Load<Texture2D>("Menu\\win-bum");
            winPose[3] = c.Load<Texture2D>("Menu\\win-pedo");

            // multiplayer
            xboxGuide[0] = c.Load<Texture2D>("Hud\\xbox-1");
            xboxGuide[1] = c.Load<Texture2D>("Hud\\xbox-2");
            xboxGuide[2] = c.Load<Texture2D>("Hud\\xbox-3");
            xboxGuide[3] = c.Load<Texture2D>("Hud\\xbox-4");

            // tiles
            tile = c.Load<Texture2D>("Tiles\\tile");
            hatch = c.Load<Texture2D>("Tiles\\hatch");

            // items
            ladder = c.Load<Texture2D>("Objects\\ladder");
            coin = c.Load<Texture2D>("Objects\\coin");
            arrow = c.Load<Texture2D>("Objects\\arrow");

            // powerups
            pupBackdrop = c.Load<Texture2D>("Powerups\\powerup-backdrop");
            pupIconSpeedshoes = c.Load<Texture2D>("Powerups\\sprintshoes");
            pupIconSpringshoes = c.Load<Texture2D>("Powerups\\springshoes");
            pupIconArrow = c.Load<Texture2D>("Powerups\\pup-arrow");
            pupIconArrowWhite = c.Load<Texture2D>("Powerups\\pup-arrow-white");
            pupIconPacifier = c.Load<Texture2D>("Powerups\\pacifier");

            // enemies
            zombieSheet = c.Load<Texture2D>("Actors\\zombie-sheet");
            zombieFat = c.Load<Texture2D>("Actors\\zombie-sheet-fat");
            zombieSpeedy = c.Load<Texture2D>("Actors\\zombie-sheet-fast");
            zombieLarge = c.Load<Texture2D>("Actors\\zombie-sheet-large");

            // backdrops
            bTutorial = c.Load<Texture2D>("Backdrops\\tutorial");
            bPark = c.Load<Texture2D>("Backdrops\\park");
            bParkSky = c.Load<Texture2D>("Backdrops\\park-sky");
            bParkPond = c.Load<Texture2D>("Backdrops\\level-select");
            bParkBandStand = c.Load<Texture2D>("Backdrops\\park-multi");
            bParkPlayground = c.Load<Texture2D>("Backdrops\\multi-level");
            bParkWildernessLeft = c.Load<Texture2D>("Backdrops\\wilderness-left");
            bParkWildernessRight = c.Load<Texture2D>("Backdrops\\wilderness-right");
            paperBoat = c.Load<Texture2D>("Backdrops\\paper-boat");
            bDirt = c.Load<Texture2D>("Backdrops\\dirt");
            bAlley = c.Load<Texture2D>("Backdrops\\alley");
            bAirport = c.Load<Texture2D>("Backdrops\\airport");
            bCity = c.Load<Texture2D>("Backdrops\\city");
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
            kite = c.Load<Texture2D>("Backdrops\\kite");
            //jungleGround = c.Load<Texture2D>("Backdrops\\Jungle\\ground");
            //for (int i = 0; i < 4; i++)
            //{
            //    parxAirport[i] = c.Load<Texture2D>("Backdrops\\Airport\\" + i);
            //}
            //for (int i = 0; i < 5; i++)
            //{
            //    parxJungle[i] = c.Load<Texture2D>("Backdrops\\Jungle\\" + i);
            //}

            // titlescreen letters
            l_G = c.Load<Texture2D>("Menu\\Letters\\g");
            l_L = c.Load<Texture2D>("Menu\\Letters\\l");
            l_O = c.Load<Texture2D>("Menu\\Letters\\o");
            l_W = c.Load<Texture2D>("Menu\\Letters\\w");
            l_B1 = c.Load<Texture2D>("Menu\\Letters\\b1");
            l_B2 = c.Load<Texture2D>("Menu\\Letters\\b2");
            l_A = c.Load<Texture2D>("Menu\\Letters\\a");
            l_Y = c.Load<Texture2D>("Menu\\Letters\\y");
            l_Comma1 = c.Load<Texture2D>("Menu\\Letters\\comma");
            l_Comma2 = c.Load<Texture2D>("Menu\\Letters\\comma2");

            smallLetters["p"] = c.Load<Texture2D>("Menu\\SmallLetters\\s_p");
            smallLetters["a"] = c.Load<Texture2D>("Menu\\SmallLetters\\s_a");
            smallLetters["r"] = c.Load<Texture2D>("Menu\\SmallLetters\\s_r");
            smallLetters["e"] = c.Load<Texture2D>("Menu\\SmallLetters\\s_e");
            smallLetters["n"] = c.Load<Texture2D>("Menu\\SmallLetters\\s_n");
            smallLetters["t"] = c.Load<Texture2D>("Menu\\SmallLetters\\s_t");
            smallLetters["h"] = c.Load<Texture2D>("Menu\\SmallLetters\\s_h");
            smallLetters["o"] = c.Load<Texture2D>("Menu\\SmallLetters\\s_o");
            smallLetters["d"] = c.Load<Texture2D>("Menu\\SmallLetters\\s_d");

            smallLetters["s"] = c.Load<Texture2D>("Menu\\SmallLetters\\s_s");
            smallLetters["i"] = c.Load<Texture2D>("Menu\\SmallLetters\\s_i");
            smallLetters["m"] = c.Load<Texture2D>("Menu\\SmallLetters\\s_m");
            smallLetters["u"] = c.Load<Texture2D>("Menu\\SmallLetters\\s_u");
            smallLetters["l"] = c.Load<Texture2D>("Menu\\SmallLetters\\s_l");

            // fonts
            font = c.Load<Texture2D>("Fonts\\font");
            smallFont = c.Load<Texture2D>("Fonts\\small-font");

            SoundManager.LoadContent();

            finishLoad();

            loaded = true;
        }
    }
}