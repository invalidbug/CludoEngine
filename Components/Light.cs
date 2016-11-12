#region

using System;
using CludoEngine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace CludoEngine.Components {
    /// <summary>
    /// A light object that depends on the NormalDrawingSystem
    /// </summary>
    public class Light : GameObject {
        private Color _color;

        private float _intensity;

        private Scene _scene;

        private NormalDrawSystem _drawSystem;

        public Sprite Sprite;

        public Light(Vector2 position, Scene scene)
            : base("Light", scene, position) {
            _scene = scene;
            scene.Pipeline.LoadContent<Texture2D>("BasicLightEffect", scene.Content, true);
            Sprite = new Sprite(this, "BasicLightEffect");
            try {
                _drawSystem = (NormalDrawSystem) _scene.DrawSystem;
            }
            catch (InvalidCastException e) {
                throw new InvalidCastException("Can't use Light object because it is dependent on NormalLightSystem!");
            }
            catch (NullReferenceException d) {
                throw new NullReferenceException("DrawSystem cannot be null!");
            }
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
                LightColor = _color * value;
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

        public override void Draw(SpriteBatch sb) {
            sb.End();
            _drawSystem.ReadySpriteBatch(sb,BlendState.Additive);
            Sprite.Draw(sb);
            sb.End();
            _drawSystem.ReadySpriteBatch(sb);
        }

        public override void Update(GameTime gt) {
            Sprite.Update(gt);
        }
    }
}