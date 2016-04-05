#region

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

#endregion

namespace CludoEngine {

    public class Input : IEngineFeature, IUpdateable {
        private readonly Scene _scene;
        private bool _gesturesEnabled;

        public Input(Scene scene) {
            _scene = scene;
            if (Instance == null) {
                Instance = this;
                GesturesEnabled = false;
            }
            KeyboardState = Keyboard.GetState();
            MouseState = Mouse.GetState();
        }

        public delegate void GestureDelegate(object sender, GestureSample args);

        public static event GestureDelegate GestureEvent;

        public static Input Instance { get; set; }

        public static Vector2 LastMousePosition {
            get { return Instance.LastMouseState.Position.ToVector2(); }
        }

        public static Vector2 MousePosition {
            get { return Instance.MouseState.Position.ToVector2(); }
        }

        public static Vector2 WorldLastMousePosition {
            get { return Utils.ConvertToScreenSpace(Instance._scene, Instance.LastMouseState.Position.ToVector2()); }
        }

        public static Vector2 WorldLastMousePositionOnlyZoom {
            get { return Utils.ConvertToScreenSpaceOnlyZoom(Instance._scene, Instance.LastMouseState.Position.ToVector2()); }
        }

        public static Vector2 WorldMousePosition {
            get { return Utils.ConvertToScreenSpace(Instance._scene, Instance.MouseState.Position.ToVector2()); }
        }
        public static Vector2 WorldMousePositionOnlyZoom {
            get { return Utils.ConvertToScreenSpaceOnlyZoom(Instance._scene, Instance.MouseState.Position.ToVector2()); }
        }

        public bool GesturesEnabled {
            get {
                return _gesturesEnabled;
            }
            set {
                if (value) {
                    // Its gross but I hope it doesn't affect performance.
                    TouchPanel.EnabledGestures = GestureType.DoubleTap | GestureType.DragComplete | GestureType.Flick |
                                                 GestureType.FreeDrag | GestureType.Hold | GestureType.HorizontalDrag |
                                                 GestureType.Pinch | GestureType.PinchComplete | GestureType.Tap |
                                                 GestureType.VerticalDrag;
                    _gesturesEnabled = true;
                } else {
                    TouchPanel.EnabledGestures = GestureType.None;
                    _gesturesEnabled = false;
                }
            }
        }

        public KeyboardState KeyboardState { get; set; }
        public KeyboardState LastKeyboardState { get; set; }
        public MouseState LastMouseState { get; set; }
        public MouseState MouseState { get; set; }

        public static bool IsKeyDown(Keys key) {
            return Instance.KeyboardState.IsKeyDown(key);
        }

        public static bool IsKeyUp(Keys key) {
            return Instance.KeyboardState.IsKeyUp(key);
        }

        public static bool IsLeftMouseButtonDown() {
            return Instance.MouseState.LeftButton == ButtonState.Pressed;
        }

        public static bool IsLeftMouseButtonUp() {
            return Instance.MouseState.LeftButton == ButtonState.Released;
        }

        public static bool IsMiddleMouseButtonDown() {
            return Instance.MouseState.MiddleButton == ButtonState.Pressed;
        }

        public static bool IsMiddleMouseButtonUp() {
            return Instance.MouseState.MiddleButton == ButtonState.Released;
        }

        public static bool IsRightMouseButtonDown() {
            return Instance.MouseState.RightButton == ButtonState.Pressed;
        }

        public static bool IsRightMouseButtonUp() {
            return Instance.MouseState.RightButton == ButtonState.Released;
        }

        public static Rectangle MouseBounds() {
            return new Rectangle((int)MousePosition.X, (int)MousePosition.Y, 1, 1);
        }

        public static Vector2 MousePositionDelta() {
            return MousePosition - LastMousePosition;
        }

        public static float MouseWheel() {
            return Instance.MouseState.ScrollWheelValue;
        }

        public static float MouseWheelDelta() {
            return Instance.MouseState.ScrollWheelValue - Instance.LastMouseState.ScrollWheelValue;
        }

        public static bool WasKeyDown(Keys key) {
            return Instance.LastKeyboardState.IsKeyDown(key);
        }

        public static bool WasKeyUp(Keys key) {
            return Instance.LastKeyboardState.IsKeyUp(key);
        }

        public static bool WasLeftMouseButtonDown() {
            return Instance.LastMouseState.LeftButton == ButtonState.Pressed;
        }

        public static bool WasLeftMouseButtonUp() {
            return Instance.LastMouseState.LeftButton == ButtonState.Released;
        }

        public static bool WasMiddleMouseButtonDown() {
            return Instance.LastMouseState.MiddleButton == ButtonState.Pressed;
        }

        public static bool WasMiddleMouseButtonUp() {
            return Instance.LastMouseState.MiddleButton == ButtonState.Released;
        }

        public static bool WasRightMouseButtonDown() {
            return Instance.LastMouseState.RightButton == ButtonState.Pressed;
        }

        public static bool WasRightMouseButtonUp() {
            return Instance.LastMouseState.RightButton == ButtonState.Released;
        }

        public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sb) {
        }

        public void Update(GameTime gt) {
            LastMouseState = MouseState;
            MouseState = Mouse.GetState();
            LastKeyboardState = KeyboardState;
            KeyboardState = Keyboard.GetState();
            if (GesturesEnabled && GestureEvent != null) {
                if (TouchPanel.IsGestureAvailable) {
                    while (TouchPanel.IsGestureAvailable) {
                        var g = TouchPanel.ReadGesture();
                        GestureEvent(this, g);
                    }
                }
            }
        }

        #region Keyboard Is Keys
        #endregion

        #region Mouse Is Keys
        #endregion
    }
}