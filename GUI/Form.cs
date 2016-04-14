#region

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace CludoEngine.GUI {
    public class Form : IControl {
        private Dictionary<string, IControl> _controls;
        private bool _inParentControl;
        private IControl _parentControl;
        private IControl _selected;
        private Theme _theme;
        public bool IsMoveable = true;

        public Form(Theme theme) {
            _theme = theme;
            _controls = new Dictionary<string, IControl>();
            State = ControlState.NotSelected;
            Hidden = false;
            ForceSelection = false;
            ForceEntireTexture = false;
        }

        public bool ForceSelection { get; set; }
        public bool ForceEntireTexture { get; set; }

        public Rectangle Bounds {
            get { return new Rectangle(Position.ToPoint(), Size.ToPoint()); }
            set {
                throw new ArgumentException(
                    "Cannot set bounds of Label! Please note coming in sooner version once we can verify the affects of a RenderTarget");
            }
        }

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

        public void Draw(SpriteBatch sb) {
            if (ForceSelection) {
                State = ControlState.Selected;
            }
            if (Hidden) {
                return;
            }
            Rectangle rect;
            if (ForceEntireTexture) {
                rect = new Rectangle(
                    0, 0,
                    _theme.Form.Texture.Width, _theme.Form.Texture.Height);
            }
            else {
                rect = new Rectangle(
                    _theme.Form.Source.X + _theme.Form.Source.Width*Convert.ToInt32(State), _theme.Form.Source.Y,
                    _theme.Form.Source.Width, _theme.Form.Source.Height);
            }
            sb.End();
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
            var totalScreenPos = GetTotalScreenPosition;
            sb.Draw(_theme.Form.Texture,
                new Rectangle((int) totalScreenPos.X, (int) totalScreenPos.Y, Bounds.Width, Bounds.Height), rect,
                Color.White);
            Gui.DrawControls(sb, _controls, _selected);
            sb.End();
        }

        public bool TestMouse() {
            return Bounds.Intersects(Input.MouseBounds());
        }

        public void Update(GameTime gt) {
            if (Hidden) {
                return;
            }
            State = ControlState.Selected;
            _selected = Gui.UpdateControls(gt, _controls, _selected);
            if (!Input.IsLeftMouseButtonDown() || !IsMoveable || !TestMouse()) {
                return;
            }
            if (_selected != null) {
                if (_selected.GetType() == typeof (Form)) {
                    return;
                }
            }
            Position += Input.MousePositionDelta();
        }

        public void AddControl(string name, IControl control) {
            _controls.Add(name, control);
            control.ParentControl = this;
        }
    }
}