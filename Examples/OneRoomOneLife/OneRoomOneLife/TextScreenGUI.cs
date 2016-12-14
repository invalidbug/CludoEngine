using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CludoEngine;
using CludoEngine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using IDrawable = CludoEngine.Graphics.IDrawable;

namespace OneRoomOneLife {
    public delegate void ScreenFished(string args, object sender);
    public class TextScreenGUI : GameObject, IDrawable {
        private readonly Scene _scene;
        private string _fullText;
        private string _currentText;
        private float _time;
        private float _timeEnd;
        private SoundEffect _sound;
        private Texture2D _textWindow;
        private bool _draw;
        private SpriteFont _font;
        private Vector2 _letterSize;
        private int _letterPerLine;
        private int _lineSpacing;
        public TextScreenGUI(string name, Scene scene, Vector2 position)
            : base(name, scene, position) {
            _scene = scene;
            _time = 0;
            _timeEnd = 0.01f;
            Depth = 0.8f;
            scene.GameObjects.AddGameObject("gui", this);
            _sound = scene.Content.Load<SoundEffect>("bloop");
            _textWindow = scene.Content.Load<Texture2D>("TextWindow");
            _font = scene.Pipeline.GetFont("Font");
            _draw = false;
            ((NormalDrawSystem)scene.DrawSystem).AddDrawable(this, true);
            _letterSize = _font.MeasureString("Q");
            // I dont understand why the following line works, the number should be 1280...
            _letterPerLine = (int)(1940 / _letterSize.X);
            _lineSpacing = (int)(475 / _letterSize.Y);
        }

        public event ScreenFished ScreenFinishedEvent;
        public bool DoTransform { get; set; }
        public float Depth { get; set; }
        public Color Color { get; set; }

        public override void Update(GameTime gt) {
            base.Update(gt);
            if (_currentText == _fullText) {
                if (_draw == false) {
                    return;
                }
                if (Input.IsKeyDown(Keys.Enter) && Input.WasKeyUp(Keys.Enter)) {
                    _draw = false;
                    if (ScreenFinishedEvent != null) {
                        ScreenFinishedEvent(_fullText, this);
                    }
                }
                return;
            }
            _time += (Single)gt.ElapsedGameTime.TotalSeconds;
            if (_time > _timeEnd) {
                _currentText += _fullText.Substring(_currentText.Length,
                    Math.Min(1, _fullText.Length - _currentText.Length));
                _time = 0;
                _sound.Play(0.5f, new Random().Next(1, 10) / 10, 0f);
            }

        }

        public void Play(string text) {
            _currentText = "";
            _fullText = text;
            _draw = true;
            for (int i = 1; i <= _fullText.Length / _letterPerLine; i += 1) {
                if (_fullText.Length > _letterPerLine*i) {
                    _fullText=_fullText.Insert(_letterPerLine*i, Environment.NewLine);
                }
            }
        }


        public void Draw(SpriteBatch sb) {
            if (_draw) {
                sb.Draw(_textWindow, new Rectangle(0, 475, 1280, 425), new Rectangle(0, 0, 1280, 425), Color.White, 0f, Vector2.Zero, SpriteEffects.None, this.Depth);
                sb.DrawString(this._font, _currentText, new Vector2(25, 490), Color.White, 0f, Vector2.Zero,
                    Vector2.One, SpriteEffects.None, this.Depth + 0.01f);
            }
        }
    }
}
