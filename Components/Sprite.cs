#region

using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Cludo_Engine.Components
{
    public class Sprite : IComponent
    {
        private Animator _animator;
        private GameObject _gameObject;
        private Rectangle _source;
        private Texture2D _texture;

        public Sprite(GameObject gameObject, string texture)
        {
            Name = "Unnamed";
            Type = "Sprite";
            Color = Color.White;
            _gameObject = gameObject;
            _texture = gameObject.Scene.Pipeline.GetTexture(texture);
            Width = TextureWidth;
            Height = TextureHeight;
            _source = new Rectangle(0, 0, Width, Height);
            Depth = 0.5f;
            gameObject.OnComponentAddedEvent += gameObject_OnComponentAddedEvent;
            gameObject.OnComponentRemovedEvent += gameObject_OnComponentRemovedEvent;
            var results = gameObject.GetComponentsByType("Animator");
            if (results.Count() > 0)
            {
                _animator = (Animator) results.ElementAt(0);
            }
        }

        public Color Color { get; set; }
        public float Depth { get; set; }
        public SpriteEffects Effects { get; set; }
        public int Height { get; set; }
        public Vector2 LocalPosition { get; set; }
        public float LocalRotation { get; set; }

        public Rectangle SourceRectangle
        {
            get
            {
                if (_animator == null)
                {
                    return _source;
                }
                return _animator.GetRectangle();
            }
            set { _source = value; }
        }

        public int TextureHeight
        {
            get { return _texture.Height; }
        }

        public int TextureWidth
        {
            get { return _texture.Width; }
        }

        public int Width { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }

        public string Type { get; set; }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(_texture,
                new Rectangle(Convert.ToInt32(_gameObject.Position.X + LocalPosition.X),
                    Convert.ToInt32(_gameObject.Position.Y + LocalPosition.Y), Width, Height), SourceRectangle, Color,
                LocalRotation + _gameObject.Rotation, Vector2.Zero, Effects, Depth);
        }

        public void Update(GameTime gt)
        {
        }

        private void gameObject_OnComponentAddedEvent(object sender, OnComponentAddedEventArgs args)
        {
            if (args.Added.Type == "Animator")
            {
                _animator = (Animator) args.Added;
            }
        }

        private void gameObject_OnComponentRemovedEvent(object sender, OnComponentRemovedEventArgs args)
        {
            if (args.Removed.Name == "Animator")
            {
                _animator = null;
            }
        }
    }
}