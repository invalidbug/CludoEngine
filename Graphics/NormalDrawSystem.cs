using FarseerPhysics.Collision;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CludoEngine.Graphics {

    public class NormalDrawSystem : IDrawSystem {
        private readonly GraphicsDevice _graphics;
        private readonly Scene _scene;
        private RenderTarget2D _buffer;

        public NormalDrawSystem(GraphicsDevice graphics,Scene scene, Vector2 resolution) {
            _graphics = graphics;
            this._scene = scene;
            this.Resolution = resolution;
            _buffer = new RenderTarget2D(graphics,(int)resolution.X,(int)resolution.Y);
        }

        public Vector2 Resolution { get; set; }

        public void ApplyResolutionChange() {
            _buffer = new RenderTarget2D(_graphics, (int)Resolution.X, (int)Resolution.Y);
        }


        public void ReadySpriteBatch(SpriteBatch sb) {
            ReadySpriteBatch(sb,BlendState.NonPremultiplied);
        }
        public void ReadySpriteBatch(SpriteBatch sb,BlendState state) {
            sb.Begin(SpriteSortMode.FrontToBack, state, null, null, null, null, _scene.Camera.GetViewMatrix());
        }
        public void Draw(SpriteBatch sb) {
            _graphics.Clear(Color.Black);
            _graphics.SetRenderTarget(_buffer);
            ReadySpriteBatch(sb);
            foreach (GameObject obj in _scene.GameObjects.Objects.Values) {
                obj.Draw(sb);
            }
            foreach (TiledPrefab prefab in _scene.LoadedTiledPrefabs) {
                prefab.Draw(sb);
            }
            sb.End();
            if (_scene.Debug) {
                _scene.DrawDebug(sb);
            }
            _graphics.SetRenderTarget(null);
            sb.Begin();
            sb.Draw(_buffer,new Rectangle(0,0,(int)Resolution.X,(int)Resolution.Y),new Rectangle(0,0,(int)_scene.Camera.FOV.X,(int)_scene.Camera.FOV.Y),Color.White);
            sb.End();
        }
    }
}