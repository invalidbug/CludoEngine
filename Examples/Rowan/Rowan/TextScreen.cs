using CludoEngine;
using CludoEngine.Components;
using CludoEngine.GUI;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using TiledSharp;

namespace Game1 {

    public class TextScreen : TiledPrefab {
        private readonly Scene _scene;
        private List<Fixture> playercolliders;
        private static Form _form;
        private static Gui _gui;
        private string _text;
        private float _currentTime;
        private bool pressedEnter;

        public TextScreen(TmxObject e, Scene scene)
            : base(e, scene) {
            _scene = scene;
            pressedEnter = false;
            string text = e.Properties["Text"];
            _text = text;
            GameObject a = new GameObject("LevelLoader", scene, new Vector2((int)e.X + (int)e.Width / 2, (int)e.Y + (int)e.Height / 2));
            a.AddComponent("Collider", new RectangleCollider(0, 0, (int)e.Width, (int)e.Height, 0f));
            a.Body.IsStatic = true;
            a.Body.IsSensor = true;
            a.Body.OnCollision += Body_OnCollision;
            a.Body.OnSeparation += Body_OnSeparation;
            playercolliders = new List<Fixture>();
            if (_gui == null) {
                _gui = new Gui();
                var theme = new Theme(scene.Pipeline.GetFont("font1", scene), scene.Pipeline.GetTexture("null"), Rectangle.Empty
                , scene.Pipeline.GetTexture("null"), Rectangle.Empty, scene.Pipeline.GetTexture("MessageWindow", scene), Rectangle.Empty);
                _form = new Form(theme);
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
                _form.Hidden = true;
                _form.IsMoveable = false;
            }
        }

        public override void Update(GameTime gt) {
            base.Update(gt);
            if (!playercolliders.Any())
                return;
            _currentTime += (float)gt.ElapsedGameTime.TotalSeconds;
            _gui.Update(gt);
        }

        public override void Draw(SpriteBatch sb) {
            base.Draw(sb);
            if (!playercolliders.Any())
                return;
            if (_currentTime > 2.9f) {
                if (Input.IsKeyDown(Keys.Enter)) {
                    pressedEnter = true;
                    Player.doesMove = true;
                }
            } else {
                Player.doesMove = false;
            }
            if (!pressedEnter)
                _gui.Draw(sb);
        }

        private void Body_OnSeparation(Fixture fixturea, Fixture fixtureb) {
            playercolliders.Remove(fixtureb);
            _form.Hidden = true;
        }

        private bool Body_OnCollision(Fixture fixturea, Fixture fixtureb, Contact contact) {
            if (((GameObject)fixtureb.Body.UserData).Tags.Contains("Player")) {
                if (!playercolliders.Contains(fixtureb) && fixtureb.IsSensor == false) {
                    playercolliders.Add(fixtureb);
                    if (!pressedEnter)
                        Player.doesMove = false;
                    Label l = ((Label)_form.Controls["Label"]);
                    l.Text = _text.Replace("\n", Environment.NewLine);
                    var a = new Vector2(_form.Size.X / 2 - l.Size.X / 2, _form.Size.Y / 2 - l.Size.Y / 2);
                    l.Position = a;
                    _form.Hidden = false;
                }
            }
            return true;
        }
    }
}