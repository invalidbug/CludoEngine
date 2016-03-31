#region

using Microsoft.Xna.Framework;

#endregion

namespace CludoEngine.Particle_System {
    public class ParticleGenerator {
        private readonly float _particlesPerSecond;
        private readonly ParticleBase _particleType;
        private readonly Vector2 _position;
        private readonly Vector2 _randomnessHorizontal;
        private readonly Vector2 _randomnessVertical;
        private readonly Scene _scene;

        public ParticleGenerator(Vector2 position, Vector2 randomnessHorizontal, Vector2 randomnessVertical,
            float particlesPerSecond, ParticleBase particleType, Scene scene) {
            _position = position;
            _randomnessHorizontal = randomnessHorizontal;
            _randomnessVertical = randomnessVertical;
            _particlesPerSecond = particlesPerSecond;
            _particleType = particleType;
            _scene = scene;
        }
    }
}