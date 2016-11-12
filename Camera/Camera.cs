#region

using FarseerPhysics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace CludoEngine {

    /// <summary>
    /// Interface for a Camera mode, which gives the ability to modify the camera position based on the Target supplied.
    /// Use CameraInstance.CameraMode = ICameraModeInstance;
    /// </summary>
    public interface ICameraMode {

        /// <summary>
        /// Called every Update, enables movement of the Camera based on Target supplied(CameraInstance.Target = Vector2Instance)
        /// </summary>
        void Update(Vector2 target, GameTime gt);
    }

    public class Camera {

        // Scene instance
        private Scene _scene;

        private Vector2 _position;
        internal Vector2 PosCompensation;

        public Camera(Scene scene, Viewport viewport) {
            // Origin of Camera to the middle of the screen. viewport is the window size.
            Origin = new Vector2(viewport.Width / 2, viewport.Height / 2);
            Zoom = 1.0f;
            _scene = scene;
            // by default, centering on object is enabled.
            CenterOnObject = true;
            // by default, clamping is enabled.
            ClampingEnabled = true;
            // by default the max/min clamp are at the highest possible range. I am not sure how big float numbers may affect performance.
            MaxClampX = float.MaxValue;
            MaxClampY = float.MaxValue;
            MinClampX = 0f;
            MinClampY = 0f;
            PosCompensation = Vector2.Zero;
        }

        // The instance of ICameraMode that will be updated in Cameras Update Method
        public ICameraMode CameraMode { get; set; }

        /// <summary>
        /// If set to true, the screen, unless CameraMode affects otherwise, the target will always be the center of the screen.
        /// </summary>
        public bool CenterOnObject { get; set; }

        /// <summary>
        /// Enables or disables Clamping
        /// </summary>
        public bool ClampingEnabled { get; set; }

        public float MaxClampX { get; set; }
        public float MaxClampY { get; set; }
        public float MinClampX { get; set; }
        public float MinClampY { get; set; }

        /// <summary>
        /// Camera Origin, AKA the location the Camera rotates around.
        /// </summary>
        public Vector2 Origin { get; set; }

        /// <summary>
        /// Camera Position
        /// </summary>
        public Vector2 Position {
            get { return _position + PosCompensation; }
            set { _position = value; }
        }

        /// <summary>
        /// The FOV of the Camera.
        /// </summary>
        public Vector2 FOV { get; set; }

        /// <summary>
        /// Camera Rotation
        /// </summary>
        public float Rotation { get; set; }

        /// <summary>
        /// The object the Camera focuses on unless CameraMode applies otherwise.
        /// </summary>
        public Vector2 Target { get; set; }

        /// <summary>
        /// Camera Zoom.
        /// </summary>
        public float Zoom { get; set; }

        /// <summary>
        /// Get the View Matrix of the Camera. This is used with all spritebatchinstance.begin() statements that draw objects that should be affected by the Camera. Specialized for Farseer Physics Cordinates.
        /// </summary>
        /// <returns></returns>
        public Matrix GetFarseerViewMatrix() {
            return Matrix.CreateTranslation(new Vector3(ConvertUnits.ToSimUnits(-Position), 0.0f)) *
                   Matrix.CreateTranslation(new Vector3(ConvertUnits.ToSimUnits(-Origin), 0.0f)) *
                   Matrix.CreateRotationZ(Rotation) *
                   Matrix.CreateScale(Zoom,
                       Zoom , 1) *
                   Matrix.CreateTranslation(new Vector3(ConvertUnits.ToSimUnits(Origin), 0.0f));
        }

        /// <summary>
        /// Get the View Matrix of the Camera. This is used with all spritebatchinstance.begin() statements that draw objects that should be affected by the Camera.
        /// </summary>
        /// <returns></returns>
        public Matrix GetViewMatrix() {
            return Matrix.CreateTranslation(new Vector3(-Position, 0.0f)) *
                   Matrix.CreateTranslation(new Vector3(-Origin, 0.0f)) *
                   Matrix.CreateRotationZ(Rotation) *
                   Matrix.CreateScale(Zoom,
                       Zoom, 1) *
                   Matrix.CreateTranslation(new Vector3(Origin, 0.0f));
        }


        public Matrix GetViewMatrixOnlyZoom() {
            return
                Matrix.CreateTranslation(new Vector3(-Origin, 0.0f)) *
                Matrix.CreateScale(Zoom + _scene.VirtualResolutionScaler.Scale,
                    Zoom + _scene.VirtualResolutionScaler.Scale, 1);
        }

        /// <summary>
        /// Updates the Camera and CameraMode
        /// </summary>
        /// <param name="gt"></param>
        public void Update(GameTime gt) {
            // Make mode isn't null
            if (CameraMode != null) {
                // Center the Camera
                if (CenterOnObject) {
                    // If your camera is breaking this is likely why, if your game is in fullscreen.
                    // If you find this, please alert me.
                    Target -= new Vector2(_scene.GameWindow.ClientBounds.Width / 2,
                        _scene.GameWindow.ClientBounds.Height / 2);
                }
                // Update the Camera Mode
                CameraMode.Update(Target, gt);
            }
            // Is clamping enabled?
            if (ClampingEnabled) {
                // Create 2 float variables, x and y
                float x = Position.X, y = Position.Y;
                // let the clamping magic begin!
                x = MathHelper.Clamp(x, MinClampX, MaxClampX);
                y = MathHelper.Clamp(y, MinClampY, MaxClampY);
                // and set the position...
                Position = new Vector2(x, y);
            }
            // Lets just make sure to make sure our position is a type of Int so no weird drawing happens!
            Position = new Vector2((int)Position.X, (int)Position.Y);
        }
    }
}