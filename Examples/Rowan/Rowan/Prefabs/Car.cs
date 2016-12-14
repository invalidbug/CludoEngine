using System;
using CludoEngine;
using CludoEngine.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TiledSharp;

namespace Game1 {

    public class Car : TiledPrefab {
        private readonly Scene _scene;
        private readonly GameObject _carGameObject;
        private readonly Sprite starSprite;
        private readonly bool _moves;
        private float _textureRatio;

        public Car(TmxObject e, Scene scene)
            : base(e, scene) {
            _scene = scene;

            _carGameObject = new GameObject("Car", scene, new Vector2((float)e.X, (float)e.Y));
            _carGameObject.Static = true;
            var i = _carGameObject.AddComponent("Sprite", new Sprite(_carGameObject, "car"));
            Sprite _sprite = (Sprite)_carGameObject.GetComponent(i);
            _sprite.Depth = 0.9f;
            _sprite.Effects = SpriteEffects.FlipHorizontally;
            if (e.Properties.ContainsKey("TextureRatio")) {
                float ratio = float.Parse(e.Properties["TextureRatio"]);
                _sprite.Width = (int)(_sprite.TextureWidth * ratio);
                _sprite.Height = (int)(_sprite.TextureHeight * ratio);
            }
            Vector2 r = _carGameObject.Position;
            scene.GameObjects.AddGameObject("Car", _carGameObject);
            
            starSprite = ((Sprite)_scene.GameObjects.GetGameObject("Stars").GetComponent(0));
            if (e.Properties.ContainsKey("Move")) {
                if (e.Properties["Move"].ToLower().Contains("t")) {
                    _moves = true;
                } else {
                    _moves = false;
                }
            } else {
                _moves = true;
            }
        }

        public override void Update(GameTime gt) {
            base.Update(gt);
            // TODO: Stars will not appear after the image has moved over 5500, but limit it at 3000
            if (_moves)
                starSprite.LocalPosition -= new Vector2(.711f * (Single)gt.ElapsedGameTime.TotalSeconds, 0);
        }
    }
}