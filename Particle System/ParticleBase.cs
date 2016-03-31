#region

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace CludoEngine.Particle_System {
    public abstract class ParticleBase : IUpdateable, ICloneable<ParticleBase> {
        private readonly List<Tuple<Color, float>> _colorList;
        protected readonly Vector2 _particleSize;
        private Color _currentColor;
        protected float _initialStartDuration;
        protected float _remainingDuration;
        protected float _startDuration;
        protected Color initialColor, initialSecondColor;

        protected ParticleBase(Color initialColor, Color initalSecondColor, Vector2 particleSize, float particleLife) {
            _colorList = new List<Tuple<Color, float>>();
            _particleSize = particleSize;
            this.initialColor = initialColor;
            initialSecondColor = initalSecondColor;
            _currentColor = initialColor;
            AddColor(initalSecondColor, particleLife);
            _remainingDuration = 0;
            _startDuration = particleLife;
        }

        public Color DrawColor {
            get {
                System.Diagnostics.Debug.WriteLine(Progress);
                if (Progress >= 1) {
                    if (_colorList.Count() >= 2) {
                        _currentColor = _colorList[0].Item1;
                        _colorList.RemoveAt(0);
                        _startDuration = _colorList[0].Item2;
                        _remainingDuration = 0;
                    }
                }
                return Color.Lerp(_currentColor, _colorList[0].Item1, Progress);
            }
        }

        public float Progress {
            get { return _remainingDuration/_startDuration; }
        }

        public abstract ParticleBase Clone();

        public virtual void Update(GameTime gt) {
            _remainingDuration += (float) gt.ElapsedGameTime.TotalSeconds;
        }

        public void AddColor(Color c, float time) {
            _colorList.Add(Tuple.Create(c, time));
        }

        public virtual void Draw(SpriteBatch sb) {
        }
    }
}