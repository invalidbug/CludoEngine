#region

using Microsoft.Xna.Framework.Graphics;
using TiledSharp;

#endregion

namespace Cludo_Engine
{
    public abstract class TiledPrefab : IUpdateable
    {
        private TmxObject _e;
        private Scene _scene;

        public TiledPrefab(TmxObject e, Scene scene)
        {
            _e = e;
            _scene = scene;
        }

        public virtual void Update(Microsoft.Xna.Framework.GameTime gt)
        {
        }

        public virtual void Start()
        {
        }

        public virtual void Draw(SpriteBatch sb)
        {
        }
    }
}