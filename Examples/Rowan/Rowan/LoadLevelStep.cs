using CludoEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game1 {

    public class LoadLevelStep : ISequenceStep {
        public bool Done { get; set; }

        public void Update(GameTime gt) {
            ((Game1)CludoGame.CurrentGame).LoadRowanScene("map1");
        }

        public void Draw(SpriteBatch gt) {
        }
    }
}