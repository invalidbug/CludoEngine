using CludoEngine;
using CludoEngine.Components;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TiledSharp;

namespace Game1 {

    public class QuickSeq : TiledPrefab {
        private ISequenceStep sequence;
        private bool update = false;

        public QuickSeq(TmxObject e, Scene scene)
            : base(e, scene) {
            string name = e.Properties["Seq"];
            switch (name) {
                case "Disapear":
                sequence = new DissapearStep(scene);
                break;
            }
            GameObject a = new GameObject("QuickSeq", scene, new Vector2((int)e.X + (int)e.Width / 2, (int)e.Y + (int)e.Height / 2));
            a.AddComponent("Collider", new RectangleCollider(0, 0, (int)e.Width, (int)e.Height, 0f));
            a.Body.IsSensor = true;
            a.Body.IsStatic = true;
            a.Body.OnCollision += Body_OnCollision;
        }

        private bool Body_OnCollision(Fixture fixturea, Fixture fixtureb, Contact contact) {
            update = true;
            return false;
        }

        public override void Update(GameTime gt) {
            if (update)
                sequence.Update(gt);
        }

        public override void Draw(SpriteBatch sb) {
            if (update)
                sequence.Draw(sb);
        }
    }
}