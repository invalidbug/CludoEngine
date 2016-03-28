using System;
using CludoEngine;
using CludoEngine.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpriteDrawingExample {
    public class Scene1 : Scene {
        // Create a gameobject
        GameObject object1;
        public Scene1(SpriteBatch sb, GraphicsDevice gd, GameWindow window, ContentManager content)
            : base(sb, gd, window, content) {
            // Init the game object, place it at 300, 300
            object1 = new GameObject("Name", this, new Vector2(300, 300));
            // Lets debug it for a representation of the location of the object.
            this.Debug = true;
            // Load the picture
            Pipeline.LoadContent<Texture2D>("a", content, true);
            // Create the sprite
            Sprite sp = new Sprite(object1, "a");
            // lets resize it
            sp.Width = 100;
            sp.Height = 100;
            // Set the local position.
            sp.LocalPosition = new Vector2(100,100);
            // add the component to the object
            object1.AddComponent("sprite", sp);
            // Add the object to the world
            GameObjects.AddGameObject("Name",object1);
        }

        public override void Update(GameTime gt) {
            base.Update(gt);
            // lets rotate the object. The *(Single)gt.ElapsedGameTime.TotalSeconds is to make sure the speed is the same no matter how much FPS you are getting.
            object1.Rotation += 0.25f*(Single)gt.ElapsedGameTime.TotalSeconds;
        }
    }
    public class Game1 : CludoGame {
        public override void StartGame() {
            // Add the scene to SceneTypes
            AddScene(typeof(Scene1), "Scene1");
            // Load and set the current scene and load the Scene as Hello World
            LoadAndSetScene("Scene1", "Hello World");
        }
    }
}
