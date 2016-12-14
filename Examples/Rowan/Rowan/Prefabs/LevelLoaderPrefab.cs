using CludoEngine;
using CludoEngine.Components;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using Microsoft.Xna.Framework;
using TiledSharp;

namespace Game1 {

    public class LevelLoaderPrefab : TiledPrefab {
        private readonly Scene _scene;
        private string level;

        public LevelLoaderPrefab(TmxObject e, Scene scene)
            : base(e, scene) {
            level = e.Properties["Level"];
            _scene = scene;
            GameObject a = new GameObject("LevelLoader", scene, new Vector2((int)e.X + (int)e.Width / 2, (int)e.Y + (int)e.Height / 2));
            a.AddComponent("Collider", new RectangleCollider(0, 0, (int)e.Width, (int)e.Height, 0f));
            a.Body.IsSensor = true;
            a.Body.IsStatic = true;
            a.Body.OnCollision += Body_OnCollision;
        }

        private bool Body_OnCollision(Fixture fixturea, Fixture fixtureb, Contact contact) {
            if (((GameObject)fixtureb.Body.UserData).Tags.Contains("Player")) {
                ((Game1)CludoGame.CurrentGame).LoadRowanScene(level);
            }
            return false;
        }
    }
}