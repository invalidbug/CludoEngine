using CludoEngine;
using CludoEngine.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace Game1 {

    public class DissapearStep : ISequenceStep {
        private readonly Scene _scene;
        public bool Done { get; set; }
        private float timetodisapear;
        private BadGuy guy;
        private GameObject rowan;

        public DissapearStep(Scene scene) {
            _scene = scene;
            
            if (scene.GameObjects.Exists("BadGuy")) {
                guy = (BadGuy)scene.GameObjects.GetGameObject("BadGuy");
            }
            if (scene.GameObjects.Exists("Rowan")) {
                rowan = scene.GameObjects.GetGameObject("Rowan");
            }
        }

        public void Update(GameTime gt) {
            if (guy == null && rowan == null) {
                return;
            }
            if (_scene.GameObjects.Exists("BadGuy") && guy == null) {
                guy = (BadGuy)_scene.GameObjects.GetGameObject("BadGuy");
            }
            if (_scene.GameObjects.Exists("Rowan") && rowan == null) {
                rowan = _scene.GameObjects.GetGameObject("Rowan");
            }
            timetodisapear += (float)gt.ElapsedGameTime.TotalSeconds;
            float percent = timetodisapear / 1f;
            percent = 1f - percent;
            if (percent <= 0f) {
                Done = true;
            }
            if (guy != null) {
                Sprite sprite = (Sprite)guy.GetComponents("BadGuySprite").ElementAt(0);
                sprite.Color = Color.White * percent;
            }

            if (rowan != null) {
                Sprite sprite = (Sprite)rowan.GetComponents("Rowan").ElementAt(0);
                sprite.Color = Color.White * percent;
            }
        }

        public void Draw(SpriteBatch gt) {
        }
    }
}