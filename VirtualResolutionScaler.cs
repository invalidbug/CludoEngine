#region

using Microsoft.Xna.Framework.Graphics;

#endregion

namespace CludoEngine {
    public class VirtualResolutionScaler {
        private readonly GraphicsDevice _graphicsDevice;
        private readonly int _virtualHeight;
        private readonly int _virtualWidth;

        public VirtualResolutionScaler(int virtualWidth, int virtualHeight, GraphicsDevice graphicsDevice) {
            _virtualWidth = virtualWidth;
            _virtualHeight = virtualHeight;
            _graphicsDevice = graphicsDevice;
            Createscale(virtualWidth, graphicsDevice);
        }

        public float Scale { get; private set; }

        public Viewport VirtualViewport {
            get { return new Viewport(0, 0, _virtualWidth, _virtualHeight); }
        }

        private void Createscale(int virtualWidth, GraphicsDevice graphicsDevice) {
            System.Diagnostics.Debug.WriteLine((float) graphicsDevice.Viewport.Width/virtualWidth + " " +
                                               (float) graphicsDevice.Viewport.Width/virtualWidth);
            Scale = graphicsDevice.Viewport.Width/virtualWidth;
        }
    }
}