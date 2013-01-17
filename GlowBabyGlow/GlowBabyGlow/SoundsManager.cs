using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace GlowBabyGlow
{
    static class SoundManager
    {
        static ContentManager c;

        public static bool soundOn = true;
        public static bool musicOn = true;

        public static SoundEffect swoosh;

        public static SoundEffect shake;
        public static SoundEffect gun;
        public static SoundEffect spring;
        public static SoundEffect run;
        public static SoundEffect reload;
        public static SoundEffect enemyDeath;
        public static SoundEffect spawn;
        public static SoundEffect spawn2;
        public static SoundEffect coin;
        public static SoundEffect[] cry = new SoundEffect[8];
        public static SoundEffect explosion1;
        public static SoundEffect explosion2;

        public static SoundEffectInstance iSpawn;
        public static SoundEffectInstance iExplode;

        public static void Init(ContentManager content)
        {
            c = content;
            ConfigureSound();
        }

        public static void LoadInstances()
        {
            iSpawn = spawn.CreateInstance();
            iExplode = explosion2.CreateInstance();
        }

        public static void Play(SoundEffectInstance s)
        {
            if (s.State != SoundState.Playing)
            {
                s.Play();
            }
        }

        public static void ConfigureSound()
        {
            SoundEffect.MasterVolume = soundOn ? 1 : 0;
            MediaPlayer.Volume = musicOn ? 1 : 0;
        }

        public static void LoadContent()
        {
            for (int i = 0; i < cry.Length; i++)
            {
                cry[i] = c.Load<SoundEffect>("Sound\\cry" + i.ToString());
            }
            swoosh = c.Load<SoundEffect>("Sound\\swoosh");
            shake = c.Load<SoundEffect>("Sound\\shake");
            gun = c.Load<SoundEffect>("Sound\\gun");
            spring = c.Load<SoundEffect>("Sound\\spring");
            run = c.Load<SoundEffect>("Sound\\run");
            reload = c.Load<SoundEffect>("Sound\\reload");
            enemyDeath = c.Load<SoundEffect>("Sound\\zombie-death");
            spawn = c.Load<SoundEffect>("Sound\\spawn");
            spawn2 = c.Load<SoundEffect>("Sound\\spawn2");
            coin = c.Load<SoundEffect>("Sound\\coin");
            explosion1 = c.Load<SoundEffect>("Sound\\explode1");
            explosion2 = c.Load<SoundEffect>("Sound\\explode2");

            LoadInstances();
        }
    }
}
