using System;
using CludoEngine;
using CludoEngine.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Simple_Asteroid_game {
    public class Player : GameObject {
        private readonly Scene _scene;
        private Sprite _sprite;
        private int _health = 100;
        public Player(string name, Scene scene, Vector2 position) : base(name, scene, position) {
            _scene = scene;
            _sprite = new Sprite(this, "Spaceship");
            // change size.
            _sprite.Width = Convert.ToInt32(_sprite.Width * 0.25f);
            _sprite.Height = Convert.ToInt32(_sprite.Height * 0.25f);
            // center it
            _sprite.LocalPosition -= new Vector2(_sprite.TextureWidth / 2, _sprite.TextureHeight / 2);
            this.AddComponent("sprite", _sprite);
            // collider
            this.AddComponent("collider", new RectangleCollider(0, 0, 45, 80, 1f));
            // the engine uses radi http://www.rapidtables.com/convert/number/degrees-to-radians.htm
            _sprite.LocalRotation = -1.5707963268f;
            // lets put it at the bottom of the screen
            this.Position = new Vector2(scene.Camera.FOV.X / 2 + _sprite.Height / 2, scene.Camera.FOV.Y - _sprite.Width / 2 - 15);

            // to detect a collision for a bullet
            this.OnCollisionEvent += Player_OnCollisionEvent;
        }

        private void Player_OnCollisionEvent(object sender, OnCollisionEventArgs args) {
            _health -= 10;
            if (_health == 0) {
                CludoGame.CurrentGame.Exit();
            }
        }

        public override void Draw(SpriteBatch sb) {
            base.Draw(sb);
            sb.DrawString(_scene.Pipeline.DefaultFont, _health.ToString(), Vector2.One * 3, Color.Red);
        }

        public override void Update(GameTime gt) {
            base.Update(gt);
            if (Input.IsLeftMouseButtonDown() && Input.WasLeftMouseButtonUp()) {
                Shot s = new Shot("Shot", _scene, this.Position + new Vector2(0, -75), 0);
            }

            // basic follow stuff.
            var direction = Input.MousePosition - this.Position;
            // easy fix to twitch when there is no need to move on the Y axis
            direction.Y = 300;
            direction.Normalize();
            direction *= 750;
            Position = new Vector2(Position.X + (direction.X * (Single)gt.ElapsedGameTime.TotalSeconds), Position.Y);
        }
    }
}
