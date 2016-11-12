using CludoEngine;
using CludoEngine.Components;
using Microsoft.Xna.Framework;

namespace Simple_Asteroid_game {
    public class Shot : GameObject {
        private readonly Scene _scene;

        public Shot(string name, Scene scene, Vector2 position, int direction) : base(name, scene, position) {
            _scene = scene;
            Sprite sprite = new Sprite(this,"Shot");
            if (direction == 1) {
                sprite.LocalRotation = MathHelper.ToRadians(180);
                Velocity = new Vector2(0, 700);
            }
            else {
                Velocity = new Vector2(0,-700);
            }
            this.Body.Friction = 100;
            this.Body.IsBullet = true;
            AddComponent("sprite", sprite);
            AddComponent("Collider", new RectangleCollider(0, 0, sprite.Width, sprite.Height,12f));


            scene.GameObjects.AddGameObject("shot",this);

            this.OnCollisionEvent += Shot_OnCollisionEvent;
        }

        private void Shot_OnCollisionEvent(object sender, OnCollisionEventArgs args) {
            // remove ourselves.
            _scene.GameObjects.RemoveObject(this.Id);
        }
    }
}
