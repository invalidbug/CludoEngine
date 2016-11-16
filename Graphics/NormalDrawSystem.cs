using System;
using System.Collections.Generic;
using System.Linq;
using FarseerPhysics.Collision;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CludoEngine.Graphics {

    public class NormalDrawSystem : IDrawSystem {
        private readonly GraphicsDevice _graphics;
        private readonly Scene _scene;
        private RenderTarget2D _buffer;
        private List<IDrawable> drawables;
        private bool stillInit = true;
        public NormalDrawSystem(GraphicsDevice graphics, Scene scene, Vector2 resolution) {
            _graphics = graphics;
            this._scene = scene;
            this.Resolution = resolution;
            _buffer = new RenderTarget2D(graphics, (int)resolution.X, (int)resolution.Y);
            drawables = new List<IDrawable>();
            scene.GameObjects.OnGameObjectAddedEvent += onGameObjectAdded;
            scene.GameObjects.OnGameObjectRemovedEvent += onGameObjectRemoved;
            foreach (GameObject gameObject in scene.GameObjects.Objects.Values) {
                onGameObjectAdded(null, new OnGameObjectAddedEventArgs(gameObject));
            }
            stillInit = false;
            SortDrawables();
        }

        private void onGameObjectRemoved(object sender, OnGameObjectRemovedEventArgs args) {
            IEnumerable<IComponent> a = from i in args.GameObject.Components.Values
                                        where i.GetType() == typeof(IDrawable)
                                        select i;
            foreach (var component in a) {
                var i = (IDrawable)component;
                if (drawables.Contains(i)) {
                    drawables.Add(i);
                }
            }
            SortDrawables();
        }

        private void onGameObjectAdded(object sender, OnGameObjectAddedEventArgs args) {
            IEnumerable<IComponent> a = from i in args.GameObject.Components.Values
                                        where i.GetType().GetInterfaces().Contains(typeof(IDrawable))
                                        select i;
            foreach (var component in a) {
                var i = (IDrawable) component;
                if (!drawables.Contains(i)) {
                    AddDrawable(i,false);
                }
            }
            SortDrawables();
        }

        private void SortDrawables() {
            if (stillInit)
                return;
            drawables = drawables.OrderBy(x => x.Depth).ToList();
        }

        public Vector2 Resolution { get; set; }

        public void ApplyResolutionChange() {
            _buffer = new RenderTarget2D(_graphics, (int)Resolution.X, (int)Resolution.Y);
        }

        public void AddTiledPrefab(TiledPrefab prefab) {
            AddDrawable((IDrawable)prefab);
        }

        public void AddTileMap(TileMap map) {
            AddDrawable((IDrawable)map);
        }

        public void AddDrawable(IDrawable drawable, bool sort = true) {
            drawables.Add(drawable);
            if(sort)
                SortDrawables();
        }

        public void ReadySpriteBatch(SpriteBatch sb) {
            ReadySpriteBatch(sb, BlendState.NonPremultiplied, true);
        }

        public void ReadySpriteBatch(SpriteBatch sb, BlendState state) {
            ReadySpriteBatch(sb, state, true);
        }
        public void ReadySpriteBatch(SpriteBatch sb, BlendState state, bool transform) {
            if (transform) {
                sb.Begin(SpriteSortMode.FrontToBack, state, null, null, null, null, _scene.Camera.GetViewMatrix());
                return;
            }
            sb.Begin(SpriteSortMode.FrontToBack, state);
        }
        public virtual void DrawTilemap(SpriteBatch sb) {
            if (CludoGame.CurrentScene.TileMap != null)
                CludoGame.CurrentScene.TileMap.Draw(sb);
        }

        public void Draw(SpriteBatch sb) {
            _graphics.Clear(Color.Black);
            _graphics.SetRenderTarget(_buffer);
            ReadySpriteBatch(sb);
            foreach (IDrawable i in drawables) {
                i.Draw(sb);
            }
            sb.End();
            if (_scene.Debug) {
                _scene.DrawDebug(sb);
            }
            _graphics.SetRenderTarget(null);
            sb.Begin();
            sb.Draw(_buffer, new Rectangle(0, 0, (int)Resolution.X, (int)Resolution.Y), new Rectangle(0, 0, (int)_scene.Camera.FOV.X, (int)_scene.Camera.FOV.Y), Color.White);
            sb.End();
        }
    }
}