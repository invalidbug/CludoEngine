using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace CludoEngine.Particle_System {
    public abstract class ParticleBase : IUpdateable {
        private readonly Vector2 _position;
        private readonly Vector2 _particleSize;
        private Color _currentColor;
        private List<Tuple<Color, float>> _colorList;
        private float _initialDuration;
        private float _remainingDuration;

        public Color DrawColor {
            get {
                if (DurationProgress <= 0) {
                    if (_colorList.Count() >= 2) {
                        _currentColor = _colorList[0].Item1;
                        _colorList.RemoveAt(0);
                        _initialDuration = _colorList[0].Item2;
                        _remainingDuration = 0;
                    }
                }
                return Color.Lerp(_currentColor, _colorList[0].Item1, Progress);
            }
        }
        public float Progress {
            get { return _remainingDuration / _initialDuration; }
        }
        public float ElapsedDuration {
            get {
                return _initialDuration - _remainingDuration;
            }
        }
        public float DurationProgress {
            get {
                return ElapsedDuration / _initialDuration;
            }
        }

        protected ParticleBase(Color initialColor, Color finalColor, Vector2 position, Vector2 particleSize, float particleLife) {
            _colorList = new List<Tuple<Color, float>>();
            _position = position;
            _particleSize = particleSize;
            this._currentColor = initialColor;
            AddColor(finalColor, particleLife);
            _remainingDuration = 0;
            _initialDuration = particleLife;
        }
        public void AddColor(Color c, float time) {
            _colorList.Add(Tuple.Create(c, time));
        }
        public void Update(GameTime gt) {
            _remainingDuration += (float)gt.ElapsedGameTime.TotalSeconds;
        }
    }
}
