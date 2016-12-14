using CludoEngine;
using CludoEngine.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TiledSharp;

namespace Game1 {

    public class House : TiledPrefab {
        private Texture2D _texture;
        private Sprite _sprite;

        public House(TmxObject e, Scene scene)
            : base(e, scene) {
            GameObject obj = new GameObject("House", scene, new Vector2((int)e.X, (int)e.Y));
            obj.Static = true;
            Sprite _sprite = new Sprite(obj, "House");
            //obj.Position += new Vector2(_sprite.Width/2,_sprite.Height/2);
            obj.AddComponent("Sprite", _sprite);
            scene.GameObjects.AddGameObject("House", obj);
        }
    }
}