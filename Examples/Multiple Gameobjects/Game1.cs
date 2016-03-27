using Cludo_Engine;
using Cludo_Engine.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Multiple_Gameobjects {
    public class Scene1 : Scene {
        public Scene1(SpriteBatch sb, GraphicsDevice gd, GameWindow window, ContentManager content)
            : base(sb, gd, window, content) {
            this.Gravity = new Vector2(0, 9);
            GameObject g = new GameObject("Floor", this, new Vector2(400, 300));
            g.AddComponent("collider", new RectangleCollider(0, 0, 800, 10, 1f));
            g.Body.IsStatic = true;
            GameObjects.AddGameObject("Circlde", g);
        }

        public override void Update(GameTime gt) {
            base.Update(gt);
            this.Debug = true;

            if (Input.IsKeyDown(Keys.A) && Input.WasKeyUp(Keys.A)) {
                GameObject g = new GameObject("Circle", this, Input.MousePosition);
                g.AddComponent("collider", new CircleCollider(0, 0, 50f, 1f));
                GameObjects.AddGameObject("Circle", g);
            }
            if (Input.IsKeyDown(Keys.S) && Input.WasKeyUp(Keys.S)) {
                GameObject g = new GameObject("Rectangle", this, Input.MousePosition);
                g.AddComponent("Rectangle", new RectangleCollider(0, 0, 125, 125, 1f));
                GameObjects.AddGameObject("Rectangle", g);
            }
            if (Input.IsKeyDown(Keys.D) && Input.WasKeyUp(Keys.D)) {
                GameObject g = new GameObject("Capsule", this, Input.MousePosition);
                g.AddComponent("Capsule", new CapsuleCollider(0, 0, 45, 45, 1f));
                GameObjects.AddGameObject("Capsule", g);
            }
        }
    }
    public class Game1 : CludoGame {
        public override void StartGame() {
            AddScene(typeof(Scene1), "Scene1");
            LoadScene("Scene1");
            SetScene("Scene1");
        }
    }
}