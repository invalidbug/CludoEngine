#region

using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace CludoEngine.Components {
    public class Sprite : IComponent {
        private Animator _animator;
        private Vector2 _drawOrigin;
        private Rectangle _source;
        private Texture2D _texture;

        public Sprite(GameObject gameObject, string texture) {
            Name = "Unnamed";
            Type = "Sprite";
            Color = Color.White;
            GameObject = gameObject;
            _texture = gameObject.Scene.Pipeline.GetTexture(texture);
            Width = TextureWidth;
            Height = TextureHeight;
            _source = new Rectangle(0, 0, Width, Height);
            Depth = 0.5f;
            gameObject.OnComponentAddedEvent += gameObject_OnComponentAddedEvent;
            gameObject.OnComponentRemovedEvent += gameObject_OnComponentRemovedEvent;
            var results = gameObject.GetComponentsByType("Animator");
            if (results.Any()) {
                _animator = (Animator) results.ElementAt(0);
            }
        }

        public Sprite(GameObject gameObject, Texture2D texture) {
            Name = "Unnamed";
            Type = "Sprite";
            Color = Color.White;
            GameObject = gameObject;
            _texture = texture;
            Width = TextureWidth;
            Height = TextureHeight;
            _source = new Rectangle(0, 0, Width, Height);
            Depth = 0.5f;
            gameObject.OnComponentAddedEvent += gameObject_OnComponentAddedEvent;
            gameObject.OnComponentRemovedEvent += gameObject_OnComponentRemovedEvent;
            var results = gameObject.GetComponentsByType("Animator");
            if (results.Any()) {
                _animator = (Animator) results.ElementAt(0);
            }
        }

        public Color Color { get; set; }
        public float Depth { get; set; }
        public SpriteEffects Effects { get; set; }
        public GameObject GameObject { get; internal set; }
        public int Height { get; set; }
        public Vector2 LocalPosition { get; set; }
        public float LocalRotation { get; set; }

        public Rectangle SourceRectangle {
            get {
                if (_animator == null) {
                    return _source;
                }
                return _animator.GetRectangle();
            }
            set { _source = value; }
        }

        public int TextureHeight {
            get { return _texture.Height; }
        }

        public int TextureWidth {
            get { return _texture.Width; }
        }

        public int Width { get; set; }
        public int Id { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public void Draw(SpriteBatch sb) {
            sb.Draw(_texture,
                new Rectangle(Convert.ToInt32(GameObject.Position.X),
                    Convert.ToInt32(GameObject.Position.Y), Width, Height), SourceRectangle, Color,
                LocalRotation + GameObject.Rotation, _drawOrigin, Effects, Depth);
        }

        public IComponent Clone(object[] args) {
            return new Sprite(GameObject, _texture) {
                LocalPosition = LocalPosition,
                Color = Color,
                Depth = Depth,
                Effects = Effects,
                LocalRotation = LocalRotation
            };
        }

        public void Update(GameTime gt) {
            _drawOrigin = -LocalPosition;
            Debugging.Debug.WriteLine(_drawOrigin);
        }

        private void gameObject_OnComponentAddedEvent(object sender, OnComponentAddedEventArgs args) {
            if (args.Added.Type == "Animator") {
                _animator = (Animator) args.Added;
            }
        }

        private void gameObject_OnComponentRemovedEvent(object sender, OnComponentRemovedEventArgs args) {
            if (args.Removed.Name == "Animator") {
                _animator = null;
            }
        }
    }
}