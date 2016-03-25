#region

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

#endregion

namespace Cludo_Engine
{
    public class Input : IEngineFeature, IUpdateable
    {
        public Input()
        {
            if (Instance == null)
                Instance = this;
            KeyboardState = Keyboard.GetState();
            MouseState = Mouse.GetState();
        }

        public static Input Instance { get; set; }
        public KeyboardState KeyboardState { get; set; }
        public KeyboardState LastKeyboardState { get; set; }
        public MouseState MouseState { get; set; }
        public MouseState LastMouseState { get; set; }

        public void Update(GameTime gt)
        {
            LastMouseState = MouseState;
            MouseState = Mouse.GetState();
            LastKeyboardState = KeyboardState;
            KeyboardState = Keyboard.GetState();
        }

        public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sb)
        {
        }

        #region Keyboard Is Keys

        public static bool IsKeyDown(Keys key)
        {
            return Instance.KeyboardState.IsKeyDown(key);
        }

        public static bool IsKeyUp(Keys key)
        {
            return Instance.KeyboardState.IsKeyUp(key);
        }

        public static bool WasKeyDown(Keys key)
        {
            return Instance.LastKeyboardState.IsKeyDown(key);
        }

        public static bool WasKeyUp(Keys key)
        {
            return Instance.LastKeyboardState.IsKeyUp(key);
        }

        #endregion

        #region Mouse Is Keys

        public static Rectangle MouseBounds()
        {
            return new Rectangle((int) MousePosition.X, (int) MousePosition.Y, 1, 1);
        }

        public static bool IsLeftMouseButtonDown()
        {
            return Instance.MouseState.LeftButton == ButtonState.Pressed;
        }

        public static bool IsLeftMouseButtonUp()
        {
            return Instance.MouseState.LeftButton == ButtonState.Released;
        }

        public static bool IsRightMouseButtonDown()
        {
            return Instance.MouseState.RightButton == ButtonState.Pressed;
        }

        public static bool IsRightMouseButtonUp()
        {
            return Instance.MouseState.RightButton == ButtonState.Released;
        }

        public static bool IsMiddleMouseButtonDown()
        {
            return Instance.MouseState.MiddleButton == ButtonState.Pressed;
        }

        public static bool IsMiddleMouseButtonUp()
        {
            return Instance.MouseState.MiddleButton == ButtonState.Released;
        }

        public static bool WasLeftMouseButtonDown()
        {
            return Instance.LastMouseState.LeftButton == ButtonState.Pressed;
        }

        public static bool WasLeftMouseButtonUp()
        {
            return Instance.LastMouseState.LeftButton == ButtonState.Released;
        }

        public static bool WasRightMouseButtonDown()
        {
            return Instance.LastMouseState.RightButton == ButtonState.Pressed;
        }

        public static bool WasRightMouseButtonUp()
        {
            return Instance.LastMouseState.RightButton == ButtonState.Released;
        }

        public static bool WasMiddleMouseButtonDown()
        {
            return Instance.LastMouseState.MiddleButton == ButtonState.Pressed;
        }

        public static bool WasMiddleMouseButtonUp()
        {
            return Instance.LastMouseState.MiddleButton == ButtonState.Released;
        }

        public static Vector2 MousePosition
        {
            get { return Instance.MouseState.Position.ToVector2(); }
        }

        public static Vector2 LastMousePosition
        {
            get { return Instance.LastMouseState.Position.ToVector2(); }
        }

        public static float MouseWheel()
        {
            return Instance.MouseState.ScrollWheelValue;
        }

        public static float MouseWheelDelta()
        {
            return Instance.MouseState.ScrollWheelValue - Instance.LastMouseState.ScrollWheelValue;
        }

        public static Vector2 MousePositionDelta()
        {
            return MousePosition - LastMousePosition;
        }

        #endregion
    }
}