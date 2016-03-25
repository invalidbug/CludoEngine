#region

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Cludo_Engine.Components
{
    public class Light : IComponent
    {
        private Color _color;

        private float _intensity;

        private Sprite _sprite;

        public Light(GameObject obj, Scene scene, Vector2 position)
            : this(obj, scene, position, 50, 50)
        {
        }

        public Light(GameObject obj, Scene scene, Vector2 position, int width, int height)
        {
            scene.Pipeline.LoadContent<Texture2D>("BasicLightEffect", scene.Content, true);
            _sprite = new Sprite(obj, "BasicLightEffect");
            scene.RenderTargets["Lights"].BlendState = BlendState.Additive;
            obj.RenderTarget = "Lights";
            obj.AddComponent("Texture", _sprite);
            _color = _sprite.Color;
            _intensity = 1f;
        }

        public int Height
        {
            get { return _sprite.Height; }
            set { _sprite.Height = value; }
        }

        public float Intensity
        {
            get { return _intensity; }
            set
            {
                _intensity = value;
                LightColor = _color*value;
            }
        }

        public Color LightColor
        {
            get { return _sprite.Color; }
            set { _sprite.Color = value; }
        }

        public int Width
        {
            get { return _sprite.Width; }
            set { _sprite.Width = value; }
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public void Draw(SpriteBatch sb)
        {
            _sprite.Draw(sb);
        }

        public void Update(GameTime gt)
        {
            _sprite.Update(gt);
        }
    }
}