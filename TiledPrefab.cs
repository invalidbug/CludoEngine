#region

using CludoEngine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TiledSharp;
using IDrawable = CludoEngine.Graphics.IDrawable;

#endregion

namespace CludoEngine {

    public abstract class TiledPrefab : IUpdateable, IDrawable {
        private TmxObject _e;
        private Scene _scene;

        public TiledPrefab(TmxObject e, Scene scene) {
            _e = e;
            _scene = scene;
            DoTransform = true;
            Depth = 0.5f;
            Color = Color.White;
            scene.DrawSystem.AddTiledPrefab(this);
        }

        public virtual void Update(Microsoft.Xna.Framework.GameTime gt) {
        }

        public virtual void Start() {
        }

        public virtual bool DoTransform { get; set; }
        public virtual float Depth { get; set; }
        public virtual Color Color { get; set; }

        public virtual void Draw(SpriteBatch sb) {
        }
    }
}