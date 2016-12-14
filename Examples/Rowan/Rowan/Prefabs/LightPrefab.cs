using CludoEngine;
using CludoEngine.Components;
using Microsoft.Xna.Framework;
using TiledSharp;

namespace Game1 {

    public class LightPrefab : TiledPrefab {
        private Light l;
        private float maxIntensity = 0.734f;
        private float minIntensity = 0.673f;
        private float intensityPerSecondCurrent;

        public LightPrefab(TmxObject e, Scene scene)
            : base(e, scene) {
            l = new Light(new Vector2((int)e.X, (int)e.Y), scene);
            l.Intensity = .7f;
            l.Position -= new Vector2(l.Sprite.TextureWidth / 2, l.Sprite.TextureHeight / 2);
            scene.GameObjects.AddGameObject("Light", l);
            intensityPerSecondCurrent = 0.043f;
        }

        public override void Update(GameTime gt) {
            l.Intensity += intensityPerSecondCurrent * (float)gt.ElapsedGameTime.TotalSeconds;
            if (l.Intensity >= maxIntensity || l.Intensity <= minIntensity) {
                intensityPerSecondCurrent = -intensityPerSecondCurrent;
            }
        }
    }
}