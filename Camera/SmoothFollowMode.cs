#region

using Microsoft.Xna.Framework;

#endregion

namespace CludoEngine {
    /// <summary>
    /// A camera mode to smooth follow a target.
    /// </summary>
    public struct SmoothFollowMode : ICameraMode {
        // Camera instance
        private Camera _currentCamera;
        // Last camera position
        private Vector2 _lastPosition;

        public SmoothFollowMode(Camera camera)
            : this() {
            _currentCamera = camera;
            // Default delta is 0.05
            Delta = 0.05f;
        }

        public delegate void TargetReached();

        public event TargetReached TargetReachedEvent;
        public float Delta { get; set; }

        public void Update(Vector2 target, GameTime gt) {
            if (_currentCamera.Position.ToPoint() == target.ToPoint()) {
                if (_lastPosition == target) {
                    if (TargetReachedEvent != null) {
                        TargetReachedEvent();
                    }
                }
                _lastPosition = target;
                return;
            }
            var difference = target - _currentCamera.Position;
            _currentCamera.Position += difference * Delta;
        }
    }
}