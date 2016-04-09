using System.Reflection.Emit;
using CludoEngine;
using CludoEngine.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Multiple_Gameobjects {
    public class Scene1 : Scene {
        public Scene1(SpriteBatch sb,GraphicsDeviceManager gm, GraphicsDevice gd, GameWindow window, ContentManager content)
            : base(sb,gm, gd, window, content) {
            // Set the worlds gravity
            Gravity = new Vector2(0, 9);
            // Create a Floor gameobject, set the position to 400, 300
            GameObject g = new GameObject("Floor", this, new Vector2(400, 300));
            // Add collider component. The position of said collider is 0, 0 based on 400,300 being the origin. The size is 800, 10 and density 10
            g.AddComponent("Floorcollider", new RectangleCollider(0, 0, 800, 10, 1f));
            // Set it to be static as we don't want it to move
            g.Static = true;
            // Add the gameobject to the world.
            GameObjects.AddGameObject("Floor", g);
        }

        public override void Draw(SpriteBatch sb) {
            base.Draw(sb);
            sb.Begin();
            sb.Draw(this.Line, new Rectangle(Input.MousePosition.ToPoint(),new Point(75,75)),Color.White);
            sb.End();
        }

        public override void Update(GameTime gt) {
            base.Update(gt);
            // Since were not drawing textures, we want to set the Debug to true so we can see
            // the representation of the Collider components and positions of objects.
            Debug = true;
            // Is said key currently down but not last frame?
            if (Input.IsKeyDown(Keys.A) && Input.WasKeyUp(Keys.A)) {
                // Create GameObject
                GameObject g = new GameObject("Circle", this, Input.MousePosition);
                // Add circle component
                g.AddComponent("collider", new CircleCollider(0, 0, 50f, 1f));
                // Add GameObject to the world.
                GameObjects.AddGameObject("Circle", g);
            }
            // Is said key currently down but not last frame?
            if (Input.IsKeyDown(Keys.S) && Input.WasKeyUp(Keys.S)) {
                // Create GameObject
                GameObject g = new GameObject("Rectangle", this, Input.MousePosition);
                // Add Rectangle component
                g.AddComponent("Rectangle", new RectangleCollider(0, 0, 125, 125, 1f));
                // Add GameObject to the world.
                GameObjects.AddGameObject("Rectangle", g);
            }
            // Is said key currently down but not last frame?
            if (Input.IsKeyDown(Keys.D) && Input.WasKeyUp(Keys.D)) {
                // Create GameObject
                GameObject g = new GameObject("Capsule", this, Input.MousePosition);
                // Add Capsule component
                g.AddComponent("Capsule", new CapsuleCollider(0, 0, 45, 45, 1f));
                // Add GameObject to the world.
                GameObjects.AddGameObject("Capsule", g);
            }
            if (Input.IsKeyDown(Keys.G)) {
                this.Camera.Zoom += 0.01f;
            }
        }
    }
    public class Game1 : CludoGame {
        public override void StartGame() {
            this.Graphics.IsFullScreen = true;
            this.Graphics.PreferredBackBufferWidth = 1366;
            this.Graphics.PreferredBackBufferHeight = 720;
            this.IsMouseVisible = true;
            // Add the scene to SceneTypes
            AddScene(typeof(Scene1), "Scene1");
            // Load and set the current scene and load the Scene as Hello World
            LoadAndSetScene("Scene1","Hello World");
        }
    }
}