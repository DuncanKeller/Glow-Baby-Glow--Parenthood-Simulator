using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GlowBabyGlow
{
    class ParticleManager
    {
        List<Particle> particles = new List<Particle>();
        List<Particle> toRemove = new List<Particle>();
        List<Particle> toAdd = new List<Particle>();

        public List<Particle> Particles
        {
            get { return particles; }
        }

        public void ClearParticles()
        {
            particles.Clear();
        }

        public void RemoveParticle(Particle p)
        {
            toRemove.Add(p);
        }

        public void AddParticle(Particle p)
        {
            toAdd.Add(p);
        }

        public void Update(float dt)
        {
            foreach (Particle p in toRemove)
            {
                particles.Remove(p);
            }

            foreach (Particle p in toAdd)
            {
                particles.Add(p);
            }

            toRemove.Clear();
            toAdd.Clear();

            foreach (Particle p in particles)
            {
                p.Update(dt);

                if (!p.Alive)
                {
                    RemoveParticle(p);
                }
            }
        }

        public void Draw(SpriteBatch sb)
        {
            foreach (Particle p in particles)
            {
                p.Draw(sb);
            }
        }
    }
}
