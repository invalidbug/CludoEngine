using System;
using System.Collections.Generic;
using System.Deployment.Internal;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CludoEngine;
using CludoEngine.Components;
using CludoEngine.Graphics;
using CludoEngine.Pipeline;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace OneRoomOneLife {
    public class Player {
        private readonly Scene _scene;
        private readonly GameObject _player;
        private readonly int _speed;
        private readonly Sprite _sprite;
        private readonly Animator _animator;
        private Dictionary<string, Vector2> _interactables;
        private TextScreenGUI _gui;
        private bool _showingTwitter = false;
        private float _twitterTime = 3.5f;
        private float _twitterCurrent = 0f;
        private int _stage = 0;

        public Player(Scene scene) {
            this._scene = scene;
            _player = new GameObject("Map", _scene, Vector2.One*300);
            _sprite = new Sprite(_player, "Character");
            _sprite.Depth = 0.6f;
            int width = (int)(64 * 1.5f);
            int height = (int)(128 * 1.5f);
            _sprite.Width = width;
            _sprite.Height = height;
            _animator = new Animator(new Vector2(_sprite.TextureWidth, _sprite.TextureHeight), new Vector2(64, 128), 0.25f);
            _animator.SetFrame(0, 0);
            _player.AddComponent("Animator", _animator);
            _player.AddComponent("Texture", _sprite);
            _player.AddComponent("Collider", new RectangleCollider(width / 2 + 5, height / 2 + 65, width - 20, height - 130, 1f));
            _player.StaticRotation = true;
            _scene.GameObjects.AddGameObject("Player", _player);


            _speed = 14500;


            // add interactables
            _interactables = new Dictionary<string, Vector2>();
            _interactables.Add("Bed", new Vector2(99, 208));
            _interactables.Add("DeskChair", new Vector2(858, 280));
            _interactables.Add("PianoChair", new Vector2(100, 486));


            _gui = new TextScreenGUI("TextScreen", scene, Vector2.Zero);
            _gui.ScreenFinishedEvent += _gui_ScreenFinishedEvent;
        }

        private void _gui_ScreenFinishedEvent(string args, object sender) {
            switch (args) {
                case "Maybe I should just check twitter first...":
                ((Sprite)((RoomScene)_scene).Map.GetMap.GetComponent(0)).Texture =
                    _scene.Pipeline.GetTexture("SocialMediaNetwork");
                _showingTwitter = true;
                break;
                case "Interesting stuff... 3AM already?!":
                ((Sprite)((RoomScene)_scene).Map.GetMap.GetComponent(0)).Texture =
    _scene.Pipeline.GetTexture("MapNight");
                ArtifialToggle(true);
                _stage += 1;
                break;
            }
            if (args.StartsWith("A world where there is only wizards, no real Humans.")) {
                Morning();
                ArtifialToggle(true);
            }
            if (args.StartsWith("Theres like these warships and you attack each other")) {
                Morning();
                ArtifialToggle(true);
            }
            if (args.StartsWith("A MMORPG, ah my dream! Perfecto! ")) {
                Morning();
                ArtifialToggle(true);
            }

            if (args.StartsWith("After years")) {
                CludoGame.CurrentGame.Exit();
            }
        }

        private void Morning() {
            ((Sprite)((RoomScene)_scene).Map.GetMap.GetComponent(0)).Texture =
                _scene.Pipeline.GetTexture("map");
            _gui.Play("What a pretty Morning! Can't wait to get started on my game.");
        }

        public void Update(GameTime gt) {
            if (_showingTwitter) {
                _twitterCurrent += (Single)gt.ElapsedGameTime.TotalSeconds;
                if (_twitterCurrent > _twitterTime) {
                    _showingTwitter = false;
                    _twitterCurrent = 0;
                    _gui.Play("Interesting stuff... 3AM already?!");
                }
            }
            if (_sprite.Height == 0)
                return;
            // get direction
            Vector2 velc = Vector2.Zero;
            if (Input.IsKeyDown(Keys.A)) {
                velc.X = -1;
            }
            if (Input.IsKeyDown(Keys.D)) {
                velc.X = 1;
            }
            if (Input.IsKeyDown(Keys.S)) {
                velc.Y = 1;
            }
            if (Input.IsKeyDown(Keys.W)) {
                velc.Y = -1;
            }

            // animate character.
            if (!(velc.X == 0)) {
                if (velc.X == -1) {
                    _sprite.Effects = SpriteEffects.FlipHorizontally;
                    if (_animator.CurrentXFrame == 0)
                        _animator.Play(0, 0, 0, 3, true);
                } else {
                    _sprite.Effects = SpriteEffects.None;
                    if (_animator.CurrentXFrame == 0)
                        _animator.Play(0, 0, 0, 3, true);
                }
            } else {
                if (velc.Y == 0) {
                    _animator.SetFrame(0, 0);
                } else {
                    if (_animator.CurrentXFrame == 0)
                        _animator.Play(0, 0, 0, 3, true);
                }
            }

            // move character
            velc = velc * (_speed * (Single)gt.ElapsedGameTime.TotalSeconds);
            if (velc.X != 0 & velc.Y != 0) {
                velc.X /= 2;
                velc.Y /= 2;
            }
            _player.Velocity = velc;



            // are we close to anything that would interact with F?
            KeyValuePair<string, float> closest = new KeyValuePair<string, float>();
            foreach (KeyValuePair<string, Vector2> i in _interactables) {
                float distance = Vector2.Distance(i.Value, Utils.PositionOfFixture(_player, 0));
                if (distance < 140) {
                    if (string.IsNullOrEmpty(closest.Key))
                        closest = new KeyValuePair<string, float>(i.Key, distance);
                    else {
                        if (closest.Value > distance) {
                            closest = new KeyValuePair<string, float>(i.Key, distance);
                        }
                    }
                }
            }
            if (closest.Key != null) {
                if (Input.IsKeyDown(Keys.F) && Input.WasKeyUp(Keys.F)) {
                    Interact(closest.Key);
                }
            }
        }

        public void ArtifialToggle(bool t) {
            if (t) {
                _sprite.Height = (int)(128 * 1.5f);
            } else {
                _sprite.Height = 0;
            }
        }

        public void Interact(string Object) {
            switch (Object) {
                case "Bed":
                if (_stage == 0) {
                    _gui.Play("I don't want to sleep right now.");
                }
                if (_stage == 1) {
                    ((Sprite)((RoomScene)_scene).Map.GetMap.GetComponent(0)).Texture =
                        _scene.Pipeline.GetTexture("Black");
                    _gui.Play(
                        "A world where there is only wizards, no real Humans. Theres this majestic old wizard that teaches you how to battle but, he has a brother. His brother is some super evil dude and you have to defeat him.");
                    ArtifialToggle(false);
                }
                if (_stage == 2) {
                    ((Sprite)((RoomScene)_scene).Map.GetMap.GetComponent(0)).Texture =
                        _scene.Pipeline.GetTexture("Black");
                    _gui.Play(
                        "Theres like these warships and you attack each other. Its super realistic for the most part. The ships would be really high detailed and allow upgrades. What a amazing game idea, so original! Much better than my last... failed project");
                    ArtifialToggle(false);
                }
                if (_stage == 3) {
                    ((Sprite)((RoomScene)_scene).Map.GetMap.GetComponent(0)).Texture =
                        _scene.Pipeline.GetTexture("Black");
                    _gui.Play(
                        "A MMORPG, ah my dream! Perfecto! There's no membership or paid items, however people can donate for access to dedicated servers! NEVER DONE BEFORE. Perfect.");
                    ArtifialToggle(false);
                }
                if (_stage == 4) {
                    ((Sprite)((RoomScene)_scene).Map.GetMap.GetComponent(0)).Texture =
                        _scene.Pipeline.GetTexture("Black");
                    this.ArtifialToggle(false);
                    _gui.Play(
                        "After years of coming up with ideas for games, procrastinating then giving up when someone else made it, you've grown old... you didn't wake up this time.");
                }
                break;
                case "DeskChair":
                _gui.Play("Maybe I should just check twitter first...");
                ((Sprite)((RoomScene)_scene).Map.GetMap.GetComponent(0)).Texture =
                    _scene.Pipeline.GetTexture("Unreal");
                this.ArtifialToggle(false);
                break;
                case "PianoChair":
                if (_stage == 0 || _stage == 1)
                    _gui.Play("I don't feel motivated enough to play the piano right now. I need to work on my game!");
                else {
                    _gui.Play("I'm already mad about those.... SKIDS STEALING MY IDEA!... How could I play now?");
                }
                break;
            }
        }
    }
}
