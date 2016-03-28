#region

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace CludoEngine.GUI
{
    public delegate void Click(object sender, object args);

    public delegate void Hover(object sender, object args);

    public delegate void Release(object sender, object args);

    public class Button : IControl
    {
        private bool _inParentControl;

        private IControl _parentControl;

        private readonly Theme _theme;

        public Button(Theme theme)
        {
            _theme = theme;
            Label = new Label(_theme)
            {
                ParentControl = this
            };
        }

        public Label Label { get; set; }

        public string Text
        {
            get { return Label.Text; }
            set
            {
                Label.Text = value;
                UpdateText();
            }
        }

        public Rectangle Bounds
        {
            get
            {
                var vectorBounds = GetTotalScreenPosition;
                return new Rectangle((int) vectorBounds.X, (int) vectorBounds.Y, (int) Size.X, (int) Size.Y);
            }
            set
            {
                throw new ArgumentException(
                    "Cannot set bounds of Button! Please note coming in sooner version once we can verify the affects of a RenderTarget");
            }
        }

        public Vector2 GetTotalScreenPosition
        {
            get
            {
                if (_inParentControl)
                {
                    return Position + _parentControl.GetTotalScreenPosition;
                }
                return Position;
            }
            set { throw new ArgumentException("Cannot set TotalScreenPosition."); }
        }

        public bool Hidden { get; set; }

        public IControl ParentControl
        {
            get { return _parentControl; }
            set
            {
                _parentControl = value;
                _inParentControl = _parentControl != null;
            }
        }

        public Vector2 Position { get; set; }
        public Vector2 Size { get; set; }
        public ControlState State { get; set; }

        public void Draw(SpriteBatch sb)
        {
            if (Hidden != false) return;
            sb.End();
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
            var rect = new Rectangle(
                _theme.Button.Source.X + _theme.Button.Source.Width*Convert.ToInt32(State), _theme.Button.Source.Y,
                _theme.Button.Source.Width, _theme.Button.Source.Height);
            var totalScreenPos = GetTotalScreenPosition;
            sb.Draw(_theme.Button.Texture,
                new Rectangle((int) totalScreenPos.X, (int) totalScreenPos.Y, Bounds.Width, Bounds.Height), rect,
                Color.White);
            sb.End();
            Label.Draw(sb);
        }

        public bool TestMouse()
        {
            return Bounds.Intersects(Input.MouseBounds());
        }

        public void Update(GameTime gt)
        {
            if (TestMouse())
            {
                if (Input.IsLeftMouseButtonDown())
                {
                    State = ControlState.Selected;
                    if (!Input.WasLeftMouseButtonUp()) return;
                    if (ClickEvent != null)
                    {
                        ClickEvent(this, null);
                    }
                }
                else
                {
                    State = ControlState.Hover;
                    if (Input.WasLeftMouseButtonDown())
                    {
                        if (ReleaseEvent != null)
                        {
                            ReleaseEvent(this, null);
                        }
                    }
                    else
                    {
                        if (HoverEvent != null)
                        {
                            HoverEvent(this, null);
                        }
                    }
                }
            }
            else
            {
                State = ControlState.NotSelected;
            }
        }

        public event Click ClickEvent;

        public event Hover HoverEvent;

        public event Release ReleaseEvent;

        private void UpdateText()
        {
            Label.Position = new Vector2(Position.X + Size.X/2 - Label.Size.X/2, Position.Y + Size.Y/2 - Label.Size.Y/2);
        }
    }
}