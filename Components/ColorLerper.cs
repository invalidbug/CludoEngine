#region

using CludoEngine.Particle_System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace CludoEngine.Components {

    public class ColorLerper : IComponent {
        private readonly List<Tuple<Color, float>> _colorList;
        private readonly bool _isGameObject;
        private readonly Particle _particle;
        private readonly Sprite _sprite;
        private Color _dcolor;
        private float _timesincelastcolor;
        public Color ColortochangefromColor;

        public ColorLerper(GameObject gameObject) {
            Name = "ColorLerperComponent";
            Type = "ColorLerperComponent";
            _colorList = new List<Tuple<Color, float>>();
            _sprite = (Sprite)gameObject.GetComponentsByType("Sprite").ElementAt(0);
            _isGameObject = true;
        }

        public ColorLerper(Particle particle) {
            _particle = particle;
            Name = "ColorLerperComponent";
            Type = "ColorLerperComponent";
            _colorList = new List<Tuple<Color, float>>();
            _isGameObject = false;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }

        public void Draw(SpriteBatch sb) {
        }

        public void Update(GameTime gt) {
            if (_colorList.Count == 0) {
                return;
            }
            _timesincelastcolor += (float)gt.ElapsedGameTime.TotalSeconds;
            var progress = _timesincelastcolor / _colorList[0].Item2;
            _dcolor = Color.Lerp(ColortochangefromColor, _colorList[0].Item1, progress);
            if (_timesincelastcolor >= _colorList[0].Item2) {
                _colorList.RemoveAt(0);
                _timesincelastcolor = 0f;
                ColortochangefromColor = _dcolor;
            }
            if (_isGameObject) {
                _sprite.Color = _dcolor;
            } else {
                _particle.Color = _dcolor;
            }
        }

        public IComponent Clone(object[] args) {
            var lerper = _isGameObject ? new ColorLerper(_sprite.GameObject) : new ColorLerper((Particle)args[0]);
            foreach (var i in _colorList) {
                lerper.AddColor(i.Item1, i.Item2);
            }
            return lerper;
        }

        public void AddColor(Color color, float time) {
            _colorList.Add(Tuple.Create(color, time));
        }
    }
}