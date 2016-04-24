#region

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace CludoEngine.Components {
    public class Light : GameObject {
        private Color _color;


        private float _intensity;

        private Scene _scene;

        private Sprite _sprite;

        public Light(Vector2 position, Scene scene) : base("Light",scene,position) {
            _scene = scene;
            scene.Pipeline.LoadContent<Texture2D>("BasicLightEffect", scene.Content, true);
            _sprite = new Sprite(this, "BasicLightEffect");
            scene.RenderTargets["Lights"].BlendState = BlendState.Additive;
            this.RenderTarget = "Lights";
            this.AddComponent("Texture", _sprite);
            _color = _sprite.Color;
            _intensity = 1f;
            this.Body.IsStatic = true;
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

        public string Type { get; set; }

        public override  void Draw(SpriteBatch sb) {
            _sprite.Draw(sb);
        }

        public override void Update(GameTime gt) {
            _sprite.Update(gt);
        }
    }
}