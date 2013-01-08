using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

namespace GlowBabyGlow
{
    class Config
    {
        //public static int screenW = 1366;
        //public static int screenH = 768;
        public static int screenW = (int)(1920  / 2);
        public static int screenH = (int)(1080 / 2);
        public static float screenR;
        public static bool fullScrn = false;
        public static float fontRatio = 1;
        public static Random rand = new Random();
        public static Dictionary<string, int> highScore = new Dictionary<string, int>();
        public static List<Color> playerColors = new List<Color>();
        
        static IAsyncResult result;
        static string filename = "save.dat";
        static bool xbox = false;
        static StorageContainer storageContainer;

        public static bool poop = true; // delete this


        public static void Init()
        {
            playerColors.Add(Color.Green);
            playerColors.Add(Color.Red);
            playerColors.Add(Color.Blue);
            playerColors.Add(Color.Yellow);

            screenR = screenW / 1920.0f;
            if (screenW <= 960)
            {
                fontRatio = screenR;
            }

            highScore.Add("alley", 0);
            highScore.Add("airport", 0);
            highScore.Add("jungle", 0);
            highScore.Add("city", 0);
            highScore.Add("powerplant", 0);

            //xbox save
#if XBOX
            xbox = true;
#endif

            if (xbox)
            {
                result = StorageDevice.BeginShowSelector(PlayerIndex.One, new AsyncCallback(XboxLoadCallback), null);
            }
            else
            {
                PCLoad();
            }
        }

        public static void InitiateSave()
        {
            if (xbox)
            {

            }
            else
            {
                BinaryWriter bw = new BinaryWriter(File.Open(filename, FileMode.Create));
                SaveData(bw);
            }
        }

        public static void SaveData(BinaryWriter bw)
        {
            // highscore -1
            // highscore -2
            // highscore -3
            // highscore -4
            // highscore -5

            bw.Write(highScore["alley"]);
            bw.Write(highScore["airport"]);
            bw.Write(highScore["jungle"]);
            bw.Write(highScore["city"]);
            bw.Write(highScore["powerplant"]);
        }

        public static void XboxLoadCallback(IAsyncResult result)
        {
            StorageDevice device = StorageDevice.EndShowSelector( result );
            if (device.IsConnected)
            {
                IAsyncResult containerResult = device.BeginOpenContainer("SaveGame", null, null);
                containerResult.AsyncWaitHandle.WaitOne();
                StorageContainer container = device.EndOpenContainer(containerResult);
                containerResult.AsyncWaitHandle.Close();
                storageContainer = container;
                if (container.FileExists(filename))
                {
                    Stream stream = container.OpenFile(filename, FileMode.Open);
                    BinaryReader br = new BinaryReader(stream);
                    Load(br);
                }
            }
        }

        public static void XboxSaveCallback(IAsyncResult result)
        {
            if (storageContainer != null)
            {
                Stream stream = storageContainer.OpenFile(filename, FileMode.Create);
                BinaryWriter bw = new BinaryWriter(stream);
                SaveData(bw);
            }
        }

        public static void Load(BinaryReader br)
        {
            try
            {
                highScore["alley"] = br.ReadInt32();
                highScore["airport"] = br.ReadInt32();
                highScore["jungle"] = br.ReadInt32();
                highScore["city"] = br.ReadInt32();
                highScore["powerplant"] = br.ReadInt32();
            }
            catch(Exception e)
            {
                // todo
            }
        }

        public static void PCLoad()
        {
            if (File.Exists(filename))
            {
                BinaryReader br = new BinaryReader(File.OpenRead(filename));
                Load(br);
            }
        }
    }
}
