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
            
        public static void Init(ContentManager content)
        {
            c = content;
            blankTexture = c.Load<Texture2D>("blank");
            testRun = c.Load<Texture2D>("runsheet");
        }
    }
}
