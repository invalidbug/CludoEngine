#region

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace CludoEngine.GUI
{
    public interface IControl
    {
        bool Hidden { get; set; }
        Vector2 Position { get; set; }
        Vector2 Size { get; set; }
        IControl ParentControl { get; set; }
        Vector2 GetTotalScreenPosition { get; set; }
        ControlState State { get; set; }
        Rectangle Bounds { get; set; }
        void Draw(SpriteBatch sb);
        bool TestMouse();
        void Update(GameTime gt);
    }
}