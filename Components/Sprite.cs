#region

using CludoEngine.Particle_System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using CludoEngine.Graphics;
using IDrawable = CludoEngine.Graphics.IDrawable;

#endregion

namespace CludoEngine.Components {

    public class Sprite : IComponent, IDrawable {
        private Animator _animator;
        private Vector2 _drawOrigin;
        private bool _isParticle;
        private Particle _particle;
        private Rectangle _source;
        private Texture2D _texture;

        public Sprite(Particle particle, string texture) {
            _isParticle = true;
            Name = "Unnamed";
            Type = "Sprite";
            Color = Color.White;
            _particle = particle;
            _texture = CludoGame.CurrentScene.Pipeline.GetTexture(texture);
            Width = TextureWidth;
            Height = TextureHeight;
            _source = new Rectangle(0, 0, Width, Height);
            Depth = 0.5f;
            particle.OnComponentAddedEvent += gameObject_OnComponentAddedEvent;
            particle.OnComponentRemovedEvent += gameObject_OnComponentRemovedEvent;
            var results = particle.GetComponentsByType("Animator");
            if (results.Any()) {
                _animator = (Animator)results.ElementAt(0);
            }
            particle.ToggleInternalDrawing(false);
        }

        public Sprite(Particle particle, Texture2D texture) {
            _isParticle = true;
            Name = "Unnamed";
            Type = "Sprite";
            Color = Color.White;
            _particle = particle;
            _texture = texture;
            Width = TextureWidth;
            Height = TextureHeight;
            _source = new Rectangle(0, 0, Width, Height);
            Depth = 0.5f;
            particle.OnComponentAddedEvent += gameObject_OnComponentAddedEvent;
            particle.OnComponentRemovedEvent += gameObject_OnComponentRemovedEvent;
            var results = particle.GetComponentsByType("Animator");
            if (results.Any()) {
                _animator = (Animator)results.ElementAt(0);
            }
            particle.ToggleInternalDrawing(false);
        }

        public Sprite(GameObject gameObject, string texture) {
            _isParticle = false;
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
                _animator = (Animator)results.ElementAt(0);
            }
        }

        public Sprite(GameObject gameObject, Texture2D texture) {
            _isParticle = false;
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
                _animator = (Animator)results.ElementAt(0);
            }
        }

        public Color Color { get; set; }
        public float Depth { get; set; }
        public SpriteEffects Effects { get; set; }
        public GameObject GameObject { get; internal set; }
        public int Height { get; set; }
        public int Id { get; set; }
        public Vector2 LocalPosition { get; set; }
        public float LocalRotation { get; set; }
        /// <summary>
        /// Move with the Camera.
        /// </summary>
        public bool DoTransform { get; set; } = true;

        public string Name { get; set; }

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

        public string Type { get; set; }
        public int Width { get; set; }

        public IComponent Clone(object[] args) {
            if (args.Length > 1) {
                if (args[1] == "Particle") {
                    return new Sprite((Particle)args[0], _texture) {
                        LocalPosition = LocalPosition,
                        Color = Color,
                        Depth = Depth,
                        Effects = Effects,
                        LocalRotation = LocalRotation
                    };
                }
            }
            return new Sprite(GameObject, _texture) {
                LocalPosition = LocalPosition,
                Color = Color,
                Depth = Depth,
                Effects = Effects,
                LocalRotation = LocalRotation
            };
        }

        /// <summary>
        /// Works only with NormalDrawSystem.
        /// </summary>
        /// <param name="sb"></param>
        public void Draw(SpriteBatch sb) {
            if (!_isParticle) {
                if (!DoTransform) {
                    sb.End();
                    var _drawSystem = (NormalDrawSystem)CludoGame.CurrentScene.DrawSystem;
                    _drawSystem.ReadySpriteBatch(sb, BlendState.NonPremultiplied, false);
                    drawSprite(sb);
                    sb.End();
                    _drawSystem.ReadySpriteBatch(sb);
                    return;
                }
                drawSprite(sb);
            }
            else {
                sb.Draw(_texture,
                new Rectangle(Convert.ToInt32(_particle.Position.X),
                    Convert.ToInt32(_particle.Position.Y), (int)_particle.Size.X, (int)_particle.Size.Y), SourceRectangle, _particle.Color,
                0, Vector2.Zero, Effects, Depth);
            }
        }

        public void Update(GameTime gt) {
            _drawOrigin = -LocalPosition;
        }

        private void gameObject_OnComponentAddedEvent(object sender, OnComponentAddedEventArgs args) {
            if (args.Added.Type == "Animator") {
                _animator = (Animator)args.Added;
            }
        }

        private void gameObject_OnComponentRemovedEvent(object sender, OnComponentRemovedEventArgs args) {
            if (args.Removed.Name == "Animator") {
                _animator = null;
            }
        }

        private void drawSprite(SpriteBatch sb) {
            sb.Draw(_texture,
                new Rectangle(Convert.ToInt32(GameObject.Position.X),
                    Convert.ToInt32(GameObject.Position.Y), Width, Height), SourceRectangle, Color,
                LocalRotation + GameObject.Rotation, _drawOrigin, Effects, Depth);
        }
    }
}