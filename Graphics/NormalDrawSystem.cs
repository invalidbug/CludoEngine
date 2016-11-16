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
            ReadySpriteBatch(sb,BlendState.NonPremultiplied, true);
        }

        public void ReadySpriteBatch(SpriteBatch sb,BlendState state) {
            ReadySpriteBatch(sb,state, true);
        }
        public void ReadySpriteBatch(SpriteBatch sb, BlendState state, bool transform) {
            if (transform) {
                sb.Begin(SpriteSortMode.FrontToBack, state, null, null, null, null, _scene.Camera.GetViewMatrix());
                return;
            }
            sb.Begin(SpriteSortMode.FrontToBack, state);
        }
        public virtual void DrawTilemap(SpriteBatch sb) {
            if(CludoGame.CurrentScene.TileMap != null)
                CludoGame.CurrentScene.TileMap.Draw(sb);
        }

        public void Draw(SpriteBatch sb) {
            _graphics.Clear(Color.Black);
            _graphics.SetRenderTarget(_buffer);
            ReadySpriteBatch(sb);
            _scene.GameObjects.Draw(sb);
            foreach (TiledPrefab prefab in _scene.LoadedTiledPrefabs) {
                prefab.Draw(sb);
            }
            DrawTilemap(sb);
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