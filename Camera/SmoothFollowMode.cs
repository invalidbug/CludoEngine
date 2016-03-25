#region

using Microsoft.Xna.Framework;

#endregion

namespace Cludo_Engine
{
    public struct SmoothFollowMode : ICameraMode
    {
        private Camera _currentCamera;

        private Vector2 _lastPosition;

        public SmoothFollowMode(Camera camera)
            : this()
        {
            _currentCamera = camera;
            Delta = 0.05f;
        }

        public delegate void TargetReached();

        public event TargetReached TargetReachedEvent;
        public float Delta { get; set; }

        public void Update(Vector2 target, GameTime gt)
        {
            if (_currentCamera.Position.ToPoint() == target.ToPoint())
            {
                if (_lastPosition == target)
                {
                    if (TargetReachedEvent != null)
                    {
                        TargetReachedEvent();
                    }
                }
                _lastPosition = target;
                return;
            }
            var difference = target - _currentCamera.Position;
            _currentCamera.Position += difference*Delta;
        }
    }
}