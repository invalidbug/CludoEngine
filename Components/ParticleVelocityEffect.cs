#region

using CludoEngine.Particle_System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace CludoEngine.Components {

    public class ParticleVelocityEffect : IComponent {
        private readonly Particle _particle;
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public Vector2 Velocity { get; set; }
        public Vector2 Gravity;

        public ParticleVelocityEffect(Particle particle, Vector2 velocity, Vector2 gravity) {
            _particle = particle;
            Gravity = gravity;
            Velocity = velocity;
            Name = "No Name";
            Type = "ParticleVelocityEffect";
        }

        public void Draw(SpriteBatch sb) {
        }

        public IComponent Clone(object[] args) {
            return new ParticleVelocityEffect((Particle)args[0], this.Velocity, this.Gravity);
        }

        public void Update(GameTime gt) {
            Velocity -= (Gravity * (float)gt.ElapsedGameTime.TotalSeconds);
            _particle.LocalPosition += Velocity * (float)gt.ElapsedGameTime.TotalSeconds;
        }
    }
}