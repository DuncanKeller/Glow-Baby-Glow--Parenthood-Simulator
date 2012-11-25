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
    }
}
