using CludoEngine;
using CludoEngine.Components;
using Microsoft.Xna.Framework;
using TiledSharp;

namespace Game1 {

    public class ClimbablePrefab : TiledPrefab {
        private GameObject _me;

        public ClimbablePrefab(TmxObject e, Scene scene)
            : base(e, scene) {
            _me = new GameObject("Climable", scene, new Vector2((int)e.X, (int)e.Y));
            _me.AddComponent("Climbable",
                new RectangleCollider((int)e.Width / 2, (int)e.Height / 2, (int)e.Width, (int)e.Height, 1f));
            _me.Static = true;
            _me.Body.Friction = 1.0f;
            _me.Tags.Add("Climbable");
        }
    }
}