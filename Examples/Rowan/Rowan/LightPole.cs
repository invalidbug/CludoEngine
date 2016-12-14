using CludoEngine;
using CludoEngine.Components;
using Microsoft.Xna.Framework;
using TiledSharp;

namespace Game1 {

    internal class LightPole : TiledPrefab {
        private readonly Scene _scene;
        private GameObject _lightpoleGameObject;
        private Light l;
        private float maxIntensity = 0.701f;
        private float minIntensity = 0.63f;
        private float intensityPerSecondCurrent;

        public LightPole(TmxObject e, Scene scene)
            : base(e, scene) {
            _scene = scene;
            l = new Light(new Vector2((int)e.X, (int)e.Y), scene);
            l.Intensity = .7f;
            l.Position -= new Vector2(l.Sprite.TextureWidth / 2, l.Sprite.TextureHeight / 1.3f);
            scene.GameObjects.AddGameObject("Light", l);
            l.Body.GravityScale = 0;
            l.Body.IsStatic = false;
            l.Velocity = new Vector2(-300, 0);
            intensityPerSecondCurrent = 0.0453f;

            _lightpoleGameObject = new GameObject("LightPole", scene, new Vector2(-700, (float)e.Y));
            _lightpoleGameObject.Body.GravityScale = 0f;
            _lightpoleGameObject.Velocity = new Vector2(-300, 0);

            var i = _lightpoleGameObject.AddComponent("Sprite", new Sprite(_lightpoleGameObject, "LightPole"));
            Sprite sprite = (Sprite)_lightpoleGameObject.GetComponent(i);
            sprite.Width = (int)(sprite.Width * 1.8f);
            sprite.Height = (int)(sprite.Height * 1.8f);
            _lightpoleGameObject.Position -= new Vector2(sprite.Width / 2, sprite.Height / 2);
            scene.GameObjects.AddGameObject("LightPole", _lightpoleGameObject);
        }

        public override void Start() {
            base.Start();
            _scene.GameObjects.GetGameObject("Stars").Static = false;
            _scene.GameObjects.GetGameObject("Stars").Body.GravityScale = 0;
            _scene.GameObjects.GetGameObject("Stars").Velocity = new Vector2(-432, 0);
        }

        public override void Update(GameTime gt) {
            l.Intensity += intensityPerSecondCurrent * (float)gt.ElapsedGameTime.TotalSeconds;
            if (l.Intensity >= maxIntensity || l.Intensity <= minIntensity) {
                intensityPerSecondCurrent = -intensityPerSecondCurrent;
            }
            if (_lightpoleGameObject.Position.X < -600) {
                _lightpoleGameObject.Position = new Vector2(1280, _lightpoleGameObject.Position.Y);
                l.Position = new Vector2(1320 - (l.Sprite.TextureWidth / 2), l.Position.Y);
            }
        }
    }
}