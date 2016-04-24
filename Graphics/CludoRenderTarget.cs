#region

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace CludoEngine.Graphics {
    public class CludoRenderTarget {
        private Dictionary<float, List<IDrawable>> _objectstodraw;
        private Scene _scene;
        public float Layer;
        public RenderTarget2D Target;

        public CludoRenderTarget(Scene scene) {
            Target = new RenderTarget2D(Scene.GraphicsDevice, scene.GameWindow.ClientBounds.Width,
                scene.GameWindow.ClientBounds.Height);
            _scene = scene;
            _objectstodraw = new Dictionary<float, List<IDrawable>>();
            scene.GameWindow.ClientSizeChanged += GameWindow_ClientSizeChanged;
            ForceDraw = false;
            if (BlendState == null) {
                BlendState = BlendState.NonPremultiplied;
            }
            Transform = true;
        }

        public CludoRenderTarget(Scene scene, int width, int height) {
            Target = new RenderTarget2D(Scene.GraphicsDevice, width, height);
            _scene = scene;
            _objectstodraw = new Dictionary<float, List<IDrawable>>();
            ForceDraw = false;
            if (BlendState == null) {
                BlendState = BlendState.NonPremultiplied;
            }
        }

        public BlendState BlendState { get; set; }
        public bool ForceDraw { get; set; }
        public bool Transform { get; set; }

        public void AddDrawable(float layer, IDrawable drawable) {
            if (!_objectstodraw.ContainsKey(layer)) {
                _objectstodraw.Add(layer, new List<IDrawable>());
                UpdateList();
            }
            _objectstodraw[layer].Add(drawable);
        }

        public void Draw(SpriteBatch sb) {
            Scene.GraphicsDevice.SetRenderTarget(Target);
            Scene.GraphicsDevice.Clear(Color.Transparent);
            if (Transform) {
                sb.Begin(SpriteSortMode.FrontToBack, BlendState, SamplerState.PointClamp, null, null, null,
                    _scene.Camera.GetViewMatrix());
            }
            else {
                sb.Begin(SpriteSortMode.FrontToBack, BlendState, SamplerState.PointClamp);
            }
            foreach (var pair in _objectstodraw) {
                var hasToDraw = ForceDraw;
                if (pair.Value.Count == 0) {
                    continue;
                }
                for (var i = 0; i < pair.Value.Count; i++) {
                    pair.Value[i].Draw(sb);
                }
            }
            sb.End();
        }

        public void RemoveDrawable(float layer, IDrawable drawable) {
            if (!_objectstodraw.ContainsKey(layer)) {
                return;
            }
            _objectstodraw[layer].Remove(drawable);
        }

        private void GameWindow_ClientSizeChanged(object sender, EventArgs e) {
            Target = new RenderTarget2D(Scene.GraphicsDevice, _scene.GameWindow.ClientBounds.Width,
                _scene.GameWindow.ClientBounds.Height);
        }

        private void UpdateList() {
            var newdic = new Dictionary<float, List<IDrawable>>();
            foreach (var pair in _objectstodraw) {
                newdic.Add(pair.Key, pair.Value);
            }
            var z = newdic.OrderBy(k => k.Key);
            _objectstodraw.Clear();
            foreach (var pair in z) {
                _objectstodraw.Add(pair.Key, pair.Value);
            }
            GC.Collect();
        }
    }
}