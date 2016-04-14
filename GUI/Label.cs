#region

using System;
using Microsoft.Xna.Framework;

#endregion

namespace CludoEngine.GUI {
    public class Label : IControl {
        private int _characterCountPerLine;
        private bool _inParentControl;
        private bool _multiLine;
        private IControl _parentControl;
        private string _text;
        private Theme _theme;

        public Label(Theme theme) {
            _theme = theme;
            LabelColor = Color.White;
            _text = "";
            MultiLine = false;
            CharacterCountPerLine = 40;
        }

        public int CharacterCountPerLine {
            get { return _characterCountPerLine; }
            set {
                _characterCountPerLine = value;
                UpdateLines();
            }
        }

        public bool MultiLine {
            get { return _multiLine; }
            set {
                _multiLine = value;
                UpdateLines();
            }
        }

        public Color LabelColor { get; set; }

        public string Text {
            get { return _text; }
            set {
                if (Text.Length < 32767) {
                    _text = value;
                    if (MultiLine) {
                        UpdateLines();
                    }
                    var vectorBounds = _theme.Font.MeasureString(_text);
                    Size = vectorBounds;
                    Bounds = new Rectangle((int) Position.X, (int) Position.Y, (int) vectorBounds.X,
                        (int) vectorBounds.Y);
                }
                else {
                    throw new ArgumentException("Cannot handle strings longer than " + 32764 + " in length!");
                }
            }
        }

        public Rectangle Bounds { get; set; }

        public Vector2 GetTotalScreenPosition {
            get {
                if (_inParentControl) {
                    return Position + _parentControl.GetTotalScreenPosition;
                }
                return Position;
            }
            set { throw new ArgumentException("Cannot set TotalScreenPosition."); }
        }

        public bool Hidden { get; set; }

        public IControl ParentControl {
            get { return _parentControl; }
            set {
                _parentControl = value;
                _inParentControl = _parentControl != null;
            }
        }

        public Vector2 Position { get; set; }

        public Vector2 Size { get; set; }

        public ControlState State { get; set; }

        public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sb) {
            if (!Hidden) {
                sb.End();
                sb.Begin();
                sb.DrawString(_theme.Font, _text, GetTotalScreenPosition, LabelColor);
                sb.End();
            }
        }

        public bool TestMouse() {
            return Bounds.Intersects(Input.MouseBounds());
        }

        public void Update(GameTime gt) {
        }

        private void UpdateLines() {
            _text = _text.Replace("\n", "");
            if (_text.Length <= CharacterCountPerLine) {
                return;
            }
            for (var i = CharacterCountPerLine; i <= _text.Length; i += CharacterCountPerLine) {
                _text = _text.Insert(i, "\n");
                i++;
            }
        }
    }
}