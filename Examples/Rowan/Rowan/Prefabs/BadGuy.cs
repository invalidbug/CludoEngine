using CludoEngine;
using CludoEngine.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TiledSharp;

namespace Game1 {

    public class BadGuyTiled : TiledPrefab {

        public BadGuyTiled(TmxObject e, Scene scene)
            : base(e, scene) {
            BadGuy g = new BadGuy(new Vector2((int)e.X, (int)e.Y), scene);
            g.Sprite.Effects = e.Properties.ContainsKey("Flipped") ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
        }
    }

    public class BadGuy : GameObject {
        public Sprite Sprite;

        public BadGuy(Vector2 position, Scene scene)
            : base("BadGuy", scene, position) {
            Sprite = new Sprite(this, "BadGuy");
            this.Position += new Vector2(Sprite.Width / 2, Sprite.Height / 2);
            this.AddComponent("BadGuySprite", Sprite);

            Sprite.LocalPosition = new Vector2(-Sprite.Width / 2, -Sprite.Height / 2);

            this.AddComponent("Colider", new RectangleCollider(0, 0, 57, 114, 1f));

            this.Body.IsStatic = true;
            this.Body.IsSensor = true;

            scene.GameObjects.AddGameObject("BadGuy", this);
        }
    }
}