namespace CludoEngine
{
    public interface IDrawable
    {
        void AddToTarget();

        void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sb);

        bool TestIfDrawNeeded();
    }
}