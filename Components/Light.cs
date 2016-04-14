#region

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace CludoEngine.Components {
    public class Light : IComponent {
        private Color _color;

        private GameObject _gameobject;

        private float _intensity;

        private Scene _scene;

        private Sprite _sprite;

        public Light(GameObject obj, Scene scene) {
            _gameobject = obj;
            _scene = scene;
            scene.Pipeline.LoadContent<Texture2D>("BasicLightEffect", scene.Content, true);
            _sprite = new Sprite(obj, "BasicLightEffect");
            scene.RenderTargets["Lights"].BlendState = BlendState.Additive;
            obj.RenderTarget = "Lights";
            obj.AddComponent("Texture", _sprite);
            _color = _sprite.Color;
            _intensity = 1f;
        }

        public int Height {
            get { return _sprite.Height; }
            set { _sprite.Height = value; }
        }

        public float Intensity {
            get { return _intensity; }
            set {
                _intensity = value;
                LightColor = _color*value;
            }
        }

        public Color LightColor {
            get { return _sprite.Color; }
            set { _sprite.Color = value; }
        }

        public int Width {
            get { return _sprite.Width; }
            set { _sprite.Width = value; }
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public void Draw(SpriteBatch sb) {
            _sprite.Draw(sb);
        }

        public IComponent Clone(object[] args) {
            return new Light(_gameobject, _scene);
        }

        public void Update(GameTime gt) {
            _sprite.Update(gt);
        }
    }
}