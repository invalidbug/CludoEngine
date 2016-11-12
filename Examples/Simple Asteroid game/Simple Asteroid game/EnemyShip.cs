using CludoEngine;
using CludoEngine.Components;
using System;
using Microsoft.Xna.Framework;

namespace Simple_Asteroid_game {
    public class EnemyShip : GameObject {
        private readonly Scene _scene;
        private Vector2 _currentGoal;
        private float _reloadTime = 0.4f;
        private float _timeSinceShot;
        public EnemyShip(string name, Scene scene, Vector2 position) : base(name, scene, position) {
            _scene = scene;
            var sprite = new Sprite(this, "Spaceship");
            // change size.
            sprite.Width = Convert.ToInt32(sprite.Width * 0.25f);
            sprite.Height = Convert.ToInt32(sprite.Height * 0.25f);
            // center it
            sprite.LocalPosition -= new Vector2(sprite.TextureWidth / 2, sprite.TextureHeight / 2);
            this.AddComponent("sprite", sprite);
            // collider
            this.AddComponent("collider", new RectangleCollider(0, 0, 45, 80, 1f));
            // the engine uses radi http://www.rapidtables.com/convert/number/degrees-to-radians.htm
            sprite.LocalRotation = 1.5707963268f;
            // lets put it at the bottom of the screen
            this.Position = new Vector2(scene.Camera.FOV.X / 2 + sprite.Height / 2,  30 + sprite.Width / 2);

            this.OnCollisionEvent += EnemyShip_OnCollisionEvent;
        }

        private void EnemyShip_OnCollisionEvent(object sender, OnCollisionEventArgs args) {
            MainScene.SpawnShip();
            Scene.GameObjects.RemoveObject(this.Id);
        }

        public Vector2 VectorDistance(Vector2 a, Vector2 b) {
            return new Vector2(Math.Abs(a.X - b.X), Math.Abs(a.Y - b.Y));
        }
        public override void Update(GameTime gt) {
            base.Update(gt);
            // simple shot timer.
            _timeSinceShot += (Single) gt.ElapsedGameTime.TotalSeconds;
            // before we shoot make sure site is lined up.
            if (VectorDistance(Position, MainScene.Player.Position).X < 30) {
                GenerateGoal();
                if (_timeSinceShot >= _reloadTime) {
                    Shoot();
                    _timeSinceShot = 0f;
                }
                return;
            }
            // basic follow stuff. This is 
            var direction = MainScene.Player.Position - this.Position;
            // easy fix to twitch when there is no need to move on the Y axis
            direction.Y = 300;
            direction.Normalize();
            direction *= 950;
            Position = new Vector2(Position.X + (direction.X * (Single)gt.ElapsedGameTime.TotalSeconds), Position.Y);
        }

        private void Shoot() {
            Shot s = new Shot("Shot", _scene, this.Position + new Vector2(0, 75), 1);
        }

        private void GenerateGoal() {
            _currentGoal = MainScene.Player.Position;
        }
    }
}
