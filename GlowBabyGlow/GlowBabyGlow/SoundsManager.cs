using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace GlowBabyGlow
{
    static class SoundManager
    {
        static ContentManager c;

        public static bool soundOn = true;
        public static bool musicOn = true;

        public static SoundEffect shake;
        public static SoundEffect gun;
        public static SoundEffect spring;
        public static SoundEffect run;
        public static SoundEffect reload;
        public static SoundEffect[] cry = new SoundEffect[8];

        public static void Init(ContentManager content)
        {
            c = content;
        }

        public static void LoadContent()
        {
            for (int i = 0; i < cry.Length; i++)
            {
                cry[i] = c.Load<SoundEffect>("Sound\\cry" + i.ToString());
            }
            shake = c.Load<SoundEffect>("Sound\\shake");
            gun = c.Load<SoundEffect>("Sound\\gun");
            spring = c.Load<SoundEffect>("Sound\\spring");
            run = c.Load<SoundEffect>("Sound\\run");
            reload = c.Load<SoundEffect>("Sound\\reload");
        }
    }
}
