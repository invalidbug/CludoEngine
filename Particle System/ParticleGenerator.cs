#region

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

#endregion

namespace CludoEngine.Particle_System {

    public class ParticleGenerator {
        private readonly Particle _particleBase;
        private readonly float _particlesPerSecond;
        private readonly Vector2 _randomnessHorizontal;
        private readonly Vector2 _randomnessVertical;
        private readonly Scene _scene;
        private float _timeSinceLastParticle;
        public List<Particle> particles;
        public Vector2 Position;
        public bool IgnoreSafeSpawning { get; set; }
        public bool Enabled { get; set; }

        public ParticleGenerator(Vector2 position, Vector2 randomnessHorizontal, Vector2 randomnessVertical,
            float particlesPerSecond, Particle particleBase, Scene scene) {
            Position = position;
            _randomnessHorizontal = randomnessHorizontal;
            _randomnessVertical = randomnessVertical;
            _particlesPerSecond = particlesPerSecond;
            _particleBase = particleBase;
            _scene = scene;
            particles = new List<Particle>();
            IgnoreSafeSpawning = false;
            Enabled = true;
        }

        public void Update(GameTime gt) {
            _timeSinceLastParticle += (float)gt.ElapsedGameTime.TotalSeconds;
        Redo:
            if (_timeSinceLastParticle >= 1000 / _particlesPerSecond / 1000 && Enabled) {
                var particle = _particleBase.Clone();
                particle.LocalPosition =
                    new Vector2(new Random().Next((int)_randomnessHorizontal.X, (int)_randomnessHorizontal.Y),
                        new Random().Next((int)_randomnessVertical.X, (int)_randomnessVertical.Y));

                particles.Add(particle);
                _timeSinceLastParticle -= 1000 / _particlesPerSecond / 1000;
                if (IgnoreSafeSpawning) {
                    if (_timeSinceLastParticle > 0)
                        goto Redo;
                }
            }
            List<Particle> removingparticles = new List<Particle>();
            foreach (Particle i in particles) {
                i.Update(gt);
                if (i.TimeToDispose) {
                    removingparticles.Add(i);
                }
            }
            foreach (Particle i in removingparticles) {
                particles.Remove(i);
                i.Components.Clear();
            }
            removingparticles.Clear();
        }

        public void Draw(SpriteBatch sb) {
            foreach (var i in particles)
                i.Draw(sb);
        }
    }
}