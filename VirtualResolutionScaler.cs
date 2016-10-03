#region

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace CludoEngine {
    public class VirtualResolutionScaler {
        private readonly GraphicsDevice _graphicsDevice;
        private readonly bool _resolutionCompensation;
        private readonly int _virtualHeight;
        private readonly Scene _scene;
        private readonly int _virtualWidth;
        private int _oldCompensation = 0;
        public VirtualResolutionScaler(Scene scene, int virtualWidth, int virtualHeight, GraphicsDevice graphicsDevice, bool resolutionCompensation) {
            _scene = scene;
            _virtualWidth = virtualWidth;
            _virtualHeight = virtualHeight;
            _graphicsDevice = graphicsDevice;
            _resolutionCompensation = resolutionCompensation;
            Createscale(virtualWidth, graphicsDevice);
        }

        public float Scale { get; private set; }

        public Viewport VirtualViewport {
            get { return new Viewport(0, 0, _virtualWidth, _virtualHeight); }
        }

        private void Createscale(int virtualWidth, GraphicsDevice graphicsDevice) {
            Scale = 1.0f;

        }

    }
}