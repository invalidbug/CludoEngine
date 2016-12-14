using CludoEngine;
using CludoEngine.Components;
using FarseerPhysics;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using TiledSharp;

namespace Game1 {

    public class Player : TiledPrefab {
        private Animator _animator;
        private List<Fixture> _collidedWithBottomJumper;
        private List<Fixture> _collidedWithMainBody;
        private float _currentJumpTime;
        private float _enteringYSpeed;

        public static bool doesMove;

        private bool _intro;
        private Vector2 _jump = new Vector2(0, ConvertUnits.ToSimUnits(-2000 * 11f));
        private float _jumpTimeLimit = 0.3143f;
        private GameObject _me;
        private LevelScene _scene;
        private int _speed = 11000;
        private Sprite _sprite;

        private float _timeForLastS = 0.5f;

        private float _timeSinceLastS = 1f;

        private bool _touchedground;

        public Player(TmxObject e, Scene scene)
            : base(e, scene) {
            doesMove = true;
            _intro = false;
            if (e.Properties.ContainsKey("Intro")) {
                _intro = true;
                doesMove = false;
            }
            scene.ClearColor = Color.CornflowerBlue;
            _me = new GameObject("Player", scene,
                new Vector2((int)e.X + (int)e.Width / 2, (int)e.Y + (int)e.Height / 2));
            _me.AddComponent("Collider", new CapsuleCollider(0, 10, 48, 48, 1f));

            var rect = PolygonTools.CreateRectangle(.18f, .2f);
            rect.Translate(new Vector2(0, .35f));
            var fixture = _me.Body.CreateFixture(new PolygonShape(rect, 2f));
            fixture.IsSensor = true;
            fixture.OnSeparation += S;
            fixture.OnCollision += D;

            var i = _me.AddComponent("Texture", new Sprite(_me, "Player"));
            _sprite = (Sprite)_me.Components[i];
            _sprite.LocalPosition = new Vector2(-32, -56);
            _sprite.Width = 64;
            _sprite.Height = 106;
            _sprite.Depth += 0.1f;
            var i2 = _me.AddComponent("animation", new Animator(new Vector2(640, 106), new Vector2(64, 106), 0.12f));
            _animator = (Animator)_me.GetComponent(i2);

            scene.GameObjects.AddGameObject("Player", _me);
            _me.StaticRotation = true;
            _me.Body.Friction = 1.0f;
            _collidedWithBottomJumper = new List<Fixture>();
            _collidedWithMainBody = new List<Fixture>();
            _scene = (LevelScene)scene;
            _me.Body.OnCollision += Body_OnCollision;
            _me.Body.OnSeparation += Body_OnSeparation;
            scene.Camera.Target = _me.Position;
            var cameraMode = new SmoothFollowMode(scene.Camera);
            cameraMode.Delta *= 2;
            scene.Camera.CameraMode = cameraMode;
            scene.Camera.CenterOnObject = true;
            _enteringYSpeed = 0;
            _me.Tags.Add("Player");
        }

        public override void Update(GameTime gt) {
            var goingdirection = false;
            _timeSinceLastS += (float)gt.ElapsedGameTime.TotalSeconds;
            _currentJumpTime += (float)gt.ElapsedGameTime.TotalSeconds;
            _scene.Camera.Target = new Vector2(_me.Position.X, _me.Position.Y - 125);
            _me.Velocity = new Vector2(0, _me.Velocity.Y);
            if (!_intro) {
                if (Input.IsKeyDown(Keys.S) && Input.WasKeyUp(Keys.S) && doesMove) {
                    if (_timeSinceLastS <= _timeForLastS) {
                        var force = new Vector2(0, -_jump.Y * 3);
                        _me.Body.ApplyForce(ref force);
                        goingdirection = true;
                        _timeSinceLastS = 1f;
                    } else {
                        _timeSinceLastS = 0f;
                    }
                }

                if (Input.IsKeyDown(Keys.D) && doesMove) {
                    _sprite.Effects = SpriteEffects.None;
                    _me.Velocity = new Vector2(_speed * (float)gt.ElapsedGameTime.TotalSeconds, _me.Velocity.Y);
                    goingdirection = true;
                }
                if (Input.IsKeyDown(Keys.A) && doesMove) {
                    _sprite.Effects = SpriteEffects.FlipHorizontally;
                    _me.Velocity = new Vector2(-_speed * (float)gt.ElapsedGameTime.TotalSeconds, _me.Velocity.Y);
                    goingdirection = true;
                }
                _touchedground = _collidedWithBottomJumper.Count > 0;

                if (_currentJumpTime >= _jumpTimeLimit && Input.IsKeyDown(Keys.Space) && Input.WasKeyUp(Keys.Space) &&
                    _touchedground) {
                    _me.Body.ApplyForce(ref _jump);
                    _touchedground = false;
                    _currentJumpTime = 0f;
                }
                var rightClimber = Utils.CreateRayCast(_scene, _me.Position + new Vector2(16, 0),
                    _me.Position + new Vector2(48, 0), _me);
                var leftClimber = Utils.CreateRayCast(_scene, _me.Position + new Vector2(-16, 0),
                    _me.Position + new Vector2(-48, 0), _me);
                CheckForClimbable(rightClimber);
                CheckForClimbable(leftClimber);
                if (goingdirection == false) {
                    _animator.Play(0, 0, 0, false);
                } else {
                    if (_animator.CurrentXFrame == 0) {
                        _animator.Play(0, 1, 6, true);
                    }
                }
                var ok = false;
                if (_enteringYSpeed < 50) {
                    ok = true;
                } else if (_enteringYSpeed > -250) {
                    ok = true;
                }
                if (goingdirection == false && _collidedWithMainBody.Count > 1 && _touchedground &&
                    _currentJumpTime > _jumpTimeLimit && ok && _enteringYSpeed < 100) {
                    _me.Body.GravityScale = 0;
                    _me.Velocity = Vector2.Zero;
                } else {
                    _me.Body.Friction = 1.0f;
                    _me.Body.GravityScale = 1;
                }
            } else {
                _me.Body.IsStatic = true;
            }
        }

        private bool Body_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact) {
            _collidedWithMainBody.Add(fixtureB);
            return true;
        }

        private void Body_OnSeparation(Fixture fixtureA, Fixture fixtureB) {
            _collidedWithMainBody.Remove(fixtureB);
        }

        private void CheckForClimbable(Raycast climber) {
            if (climber.GameObject != null) {
                if (climber.GameObject.Tags.Contains("Climbable")) {
                    if (Input.IsKeyDown(Keys.Space) && _currentJumpTime >= _jumpTimeLimit) {
                        _me.Body.ApplyForce(ref _jump);
                        _currentJumpTime = 0f;
                    }
                }
            }
        }

        private bool D(Fixture fixtureA, Fixture fixtureB, Contact contact) {
            if (fixtureB.IsSensor == false) {
                _collidedWithBottomJumper.Add(fixtureB);
                _enteringYSpeed = _me.Velocity.Y;
                return true;
            }
            if (fixtureB.IsSensor)
                return false;
            return false;
        }

        private void me_OnCollisionEvent(object sender, OnCollisionEventArgs args) {
        }

        private void S(Fixture fixtureA, Fixture fixtureB) {
            _collidedWithBottomJumper.Remove(fixtureB);
        }
    }
}