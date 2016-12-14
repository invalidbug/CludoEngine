using CludoEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TiledSharp;

namespace Game1 {

    public class RowanSeq : TiledPrefab {
        private SceneSequence _seq;
        private TmxObject tmx;
        private Scene scene;

        public RowanSeq(TmxObject e, Scene scene)
            : base(e, scene) {
            this.scene = scene;
            this.tmx = e;
        }

        public override void Start() {
            _seq = new SceneSequence();
            switch (tmx.Properties["Seq"]) {
                case "Intro":
                _seq.AddStep(new RowanSeqStep("This is you, lookin' fancy eh?",
                    scene.GameObjects.GetGameObject("Player").Position, scene));
                _seq.AddStep(new RowanSeqStep("This is yo girl, protect her!",
                    scene.GameObjects.GetGameObject("Rowan").Position, scene));
                _seq.AddStep(new RowanSeqStep("This is badguy", scene.GameObjects.GetGameObject("Player").Position + new Vector2(75, -28), scene, true));
                _seq.AddStep(new DissapearStep(scene));
                _seq.AddStep(new RowanSeqStep("Your girl just got kidnapped", scene.GameObjects.GetGameObject("Player").Position + new Vector2(75, -28), scene));
                _seq.AddStep(new RowanSeqStep("You're not very good at this.", scene.GameObjects.GetGameObject("Player").Position + new Vector2(75, -28), scene));
                _seq.AddStep(new RowanSeqStep("You must save her!", scene.GameObjects.GetGameObject("Player").Position, scene));
                _seq.AddStep(new LoadLevelStep());
                break;

                case "second":
                _seq.AddStep(new RowanSeqStep("This is badguy", scene.GameObjects.GetGameObject("BadGuy").Position, scene, true, false));
                break;
            }
        }

        public override void Update(GameTime gt) {
            base.Update(gt);
            _seq.Update(gt);
        }

        public override void Draw(SpriteBatch sb) {
            base.Draw(sb);
            _seq.Draw(sb);
        }
    }
}