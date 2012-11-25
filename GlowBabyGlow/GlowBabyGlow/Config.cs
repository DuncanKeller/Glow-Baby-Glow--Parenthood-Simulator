using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GlowBabyGlow
{
    class Config
    {
        public static int screenW = 960;
        public static int screenH = 540;
        public static Random rand = new Random();
        public static Dictionary<string, int> highScore = new Dictionary<string, int>();

        public static void Init()
        {
            highScore.Add("alley", 0);
            highScore.Add("airport", 0);
            highScore.Add("jungle", 0);
            highScore.Add("city", 0);
            highScore.Add("powerplant", 0);
        }
    }
}
