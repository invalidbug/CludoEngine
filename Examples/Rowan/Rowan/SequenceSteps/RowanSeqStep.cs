using CludoEngine;
using CludoEngine.Components;
using CludoEngine.GUI;
using CludoEngine.Particle_System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;

namespace Game1 {

    public struct RowanSeqStep : ISequenceStep {
        private Form _form;
        private static Gui _gui;
        private static float TimePerMessage;
        private Vector2 _position;
        private Scene _scene;
        private readonly bool _appearBadGuy;
        private readonly bool _showtextscreen;
        private string _text;
        private float _time;

        public RowanSeqStep(string text, Vector2 position, Scene scene, bool appearBadGuy = false, bool showtextscreen = true)
            : this() {
            scene.Camera.Zoom = 1f;
            _text = text;
            _position = position;
            _scene = scene;
            _appearBadGuy = appearBadGuy;
            _showtextscreen = showtextscreen;
            TimePerMessage = 0.5f;

            var theme = new Theme(scene.Pipeline.GetFont("font1", scene), scene.Pipeline.GetTexture("null"), Rectangle.Empty
                , scene.Pipeline.GetTexture("null"), Rectangle.Empty, scene.Pipeline.GetTexture("MessageWindow", scene), Rectangle.Empty);
            if (_gui == null)
                _gui = new Gui();
            var formSize = scene.Pipeline.GetTexture("MessageWindow").Bounds.Size.ToVector2() * new Vector2(1.25f, 1.25f);
            _form = new Form(theme) {
                ForceEntireTexture = true,
                ForceSelection = true,
                IsMoveable = false,
                Size = formSize,
                Position = new Vector2(1080 / 2 - formSize.X / 2, 720 / 1.1f - formSize.Y)
            };
            Label l = new Label(theme) {
                Text = text,
                MultiLine = true,
                CharacterCountPerLine = 64,
                LabelColor = Color.Black
            };
            _form.AddControl("Label", l);
            l.Position = new Vector2(formSize.X / 2 - l.Size.X / 2, formSize.Y / 2 - l.Size.Y / 2);
            _gui.AddControl(text, _form);
            scene.Camera.ClampingEnabled = false;
            //scene.Camera.Zoom = 2.5f;
            _form.Hidden = true;

            // Need to put textures into Rowans content folder and redo theme init.
            // add multi line label to form etc... finish intro scene
            // add particle effects for bad guy appearance and disappearance
            // create levels, finish level changing make obstacles and create sound
            // preferably some swing music because I feel like that fits this game well
            // and game should be based in the 60's. Create timescale (just change the game time
            // value) for the ending scene, animate the kissing scene draw guy waking up in bed and
            // come to the realization its a dream, maybe make the time zone in the wake up scene modern
            // Finish commenting out engine and making code more readable. Where to go from here?
        }

        public bool Done { get; set; }

        public void Draw(SpriteBatch sb) {
            _gui.Draw(sb);
            sb.Begin(SpriteSortMode.Deferred, null, null, null, null, null, CludoGame.CurrentScene.Camera.GetViewMatrix());
            if (gen != null && _showtextscreen)
                gen.Draw(sb);
            sb.End();
            _form.Hidden = false;
        }

        private BadGuy a;
        private float _badguyopacityTime;
        private ParticleGenerator gen;

        public void Update(GameTime gt) {
            if (_appearBadGuy && a == null) {
                a = new BadGuy(_position, _scene);
                _scene.GameObjects.AddGameObject("BadGuy", a);
                Particle p = new Particle(new Vector2(0, 0), new Vector2(10, 10), Color.Blue, 3f);
                p.AddComponent("Color", new SinglePixelParticleDrawer(p));
                ColorLerper lerp = (ColorLerper)p.AddComponentAndReturnComponent("lerper", new ColorLerper(p));
                lerp.AddColor(Color.Red, 1f);
                lerp.AddColor(Color.Blue, 1f);
                lerp.AddColor(Color.Yellow, 1f);
                p.AddComponent("Movement", new RandomMovementEffect(p, 15, 20));
                gen = new ParticleGenerator(a.Position, new Vector2(-10, 10), new Vector2(-10, 10), 27, p, _scene);
                p.Generator = gen;
                gen.Enabled = true;
                gen.IgnoreSafeSpawning = false;
            }
            if (gen != null) {
                gen.Update(gt);
                _badguyopacityTime += (float)gt.ElapsedGameTime.TotalSeconds;
                Sprite sprite = (Sprite)a.GetComponents("BadGuySprite").ElementAt(0);
                sprite.Color = Color.White * (_badguyopacityTime / 2f);
                if (_badguyopacityTime > 1.84f) {
                    gen.Enabled = false;
                }
                if (!gen.particles.Any() && gen.Enabled == false) {
                    this.Done = true;
                }
            }
            this._scene.Camera.Target = _position;
            _time += (Single)gt.ElapsedGameTime.TotalSeconds;
            if (!(_time >= TimePerMessage)) return;
            if (Input.IsKeyDown(Keys.Space) || Input.IsKeyDown(Keys.Enter) || Input.IsKeyDown(Keys.Escape)) {
                if (Input.WasKeyUp(Keys.Space)) {
                    Finish();
                    return;
                }
                if (Input.WasKeyUp(Keys.Enter)) {
                    Finish();
                    return;
                }
                if (Input.WasKeyUp(Keys.Escape)) {
                    Finish();
                    return;
                }
            }
        }

        private void Finish() {
            _form.Hidden = true;
            Done = true;
        }
    }
}