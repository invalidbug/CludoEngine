using CludoEngine.Particle_System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace CludoEngine.Components {

    public class RandomMovementEffect : IComponent {
        private readonly Particle _particle;
        private readonly int _maxSpeed;
        private readonly int _maxDistance;
        private float _currentDistance;
        private Vector2 _velocity;
        private Vector2 _lastPosition;
        private int _currentMaxDistance;
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }

        public RandomMovementEffect(Particle particle, int maxSpeed, int maxDistance = 256) {
            _particle = particle;
            _maxSpeed = maxSpeed;
            _maxDistance = maxDistance;
            NewMovement();
            _lastPosition = Vector2.Zero;
        }

        private void NewMovement() {
            Random ran = new Random();
            _currentMaxDistance = ran.Next(0, _maxDistance);
            _currentDistance = 0;
            _velocity = new Vector2((float)ran.Next(-2, 2),
                (float)ran.Next(-2, 2)) * _maxSpeed;
            if (_velocity == Vector2.Zero) {
                _velocity = Vector2.One
                * _maxSpeed;
            }
        }

        public void Draw(SpriteBatch sb) {
        }

        public IComponent Clone(object[] args) {
            return new RandomMovementEffect((Particle)args[0], _maxSpeed, _maxDistance);
        }

        public void Update(GameTime gt) {
            _currentDistance += Vector2.Distance(_lastPosition, _particle.LocalPosition);
            _lastPosition = _particle.LocalPosition;
            if (_currentDistance >= _maxDistance) {
                NewMovement();
            }
            _particle.LocalPosition += _velocity * (float)gt.ElapsedGameTime.TotalSeconds;
        }
    }
}