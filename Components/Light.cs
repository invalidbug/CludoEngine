#region

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace CludoEngine.Components {
    public class Light : GameObject {
        private Color _color;


        private float _intensity;

        private Scene _scene;

        public Sprite Sprite;

        public Light(Vector2 position, Scene scene) : base("Light",scene,position) {
            _scene = scene;
            scene.Pipeline.LoadContent<Texture2D>("BasicLightEffect", scene.Content, true);
            Sprite = new Sprite(this, "BasicLightEffect");
            scene.RenderTargets["Lights"].BlendState = BlendState.Additive;
            this.RenderTarget = "Lights";
            this.AddComponent("Texture", Sprite);
            _color = Sprite.Color;
            _intensity = 1f;
            this.Body.IsStatic = true;
        }

        public int Height {
            get { return Sprite.Height; }
            set { Sprite.Height = value; }
        }

        public float Intensity {
            get { return _intensity; }
            set {
                _intensity = value;
                LightColor = _color*value;
            }
        }

        public Color LightColor {
            get { return Sprite.Color; }
            set { Sprite.Color = value; }
        }

        public int Width {
            get { return Sprite.Width; }
            set { Sprite.Width = value; }
        }

        public string Type { get; set; }

        public override  void Draw(SpriteBatch sb) {
            Sprite.Draw(sb);
        }

        public override void Update(GameTime gt) {
            Sprite.Update(gt);
        }
    }
}