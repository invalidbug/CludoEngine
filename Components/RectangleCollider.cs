#region

using Microsoft.Xna.Framework;

#endregion

namespace CludoEngine.Components {

    public struct RectangleCollider : IComponent {

        public RectangleCollider(int localX, int localY, int width, int height, float density)
            : this() {
            LocalX = localX;
            LocalY = localY;
            Width = width;
            Height = height;
            Density = density;
            Type = "RectangleCollider";
        }

        public float Density { get; set; }
        public int Height { get; internal set; }
        public int Id { get; set; }
        public int LocalX { get; set; }
        public int LocalY { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int Width { get; internal set; }

        public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sb) {
        }

        public IComponent Clone(object[] args) {
            throw new System.NotImplementedException();
        }

        public void Update(GameTime gt) {
        }
    }
}