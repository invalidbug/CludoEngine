using CludoEngine;
using CludoEngine.Components;
using Microsoft.Xna.Framework;
using TiledSharp;

namespace Game1 {

    public class Rowan : TiledPrefab {
        public GameObject _me;
        private Sprite _sprite;

        public Rowan(TmxObject e, Scene scene)
            : base(e, scene) {
            _me = new GameObject("Rowan", scene,
                 new Vector2((int)e.X, (int)e.Y));
            _sprite = new Sprite(_me, "Rowan");
            _sprite.Width = 53;
            _sprite.Height = 96;
            _me.Position += new Vector2(_sprite.Width / 2, _sprite.Height / 2);
            _me.AddComponent("Rowan", _sprite);

            _sprite.LocalPosition = new Vector2(-_sprite.Width / 2, -_sprite.Height / 2);

            _me.AddComponent("Colider", new RectangleCollider(0, 0, _sprite.Width, _sprite.Height, 1f));

            _me.Body.IsStatic = true;

            scene.GameObjects.AddGameObject("Rowan", _me);
        }
    }
}