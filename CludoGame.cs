#region

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using CludoEngine.Graphics;
using Microsoft.Xna.Framework.Content;

#endregion

namespace CludoEngine {

    public class CludoGame : Game {
        public GraphicsDeviceManager Graphics;
        public SpriteBatch SpriteBatch;
        public static CludoGame CurrentGame;

        public CludoGame() {
            CludoGame.CurrentGame = this;
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            SceneTypes = new Dictionary<string, Type>();
            LoadedScenes = new Dictionary<string, Scene>();
            Graphics.PreparingDeviceSettings += Graphics_PreparingDeviceSettings;
            for (var i = 0; i <= 10; i++) {
                Debugging.Debug.WriteLine(
                    "Warning: Mouse Position on fullscreen applications is broken. This applies to Phones also.");
            }
            Debugging.Debug.WriteLine(
                "###Cludo Engine 0.7.914 BETA###\nPlease use understanding their is likely to be many bugs. Please however, support us by visiting our github and contributing to the project! You could also complain about those pesky bugs...");
        }

        public static Scene CurrentScene { get; set; }
        public Dictionary<string, Scene> LoadedScenes { get; set; }
        private Dictionary<string, Type> SceneTypes { get; set; }

        private void Graphics_PreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e) {
            e.GraphicsDeviceInformation.PresentationParameters.PresentationInterval = PresentInterval.Two;
        }

        /// <summary>
        /// Adds a Scene Type. This is then used with LoadScene to load a Scene and then SetScene to set the current Scene.
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="name"></param>
        public void AddScene(Type scene, string name) {
            SceneTypes.Add(name, scene);
        }

        /// <summary>
        /// Adds a Scene Type. This is then used with LoadScene to load a Scene and then SetScene to set the current Scene.
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="name"></param>
        public void AddScene(Scene scene, string name) {
            SceneTypes.Add(name, typeof(Scene));
        }

        /// <summary>
        /// Disposes of a Scene.
        /// </summary>
        /// <param name="name"></param>
        public void DisposeScene(string name) {
            if (CurrentScene == LoadedScenes[name]) {
                CurrentScene = null;
            }
            LoadedScenes[name].Dispose();
            LoadedScenes.Remove(name);
        }

        /// <summary>
        /// Loads a Scene from Scene Types. Use Set scene for already Loaded Scenes.
        /// </summary>
        public Scene LoadScene(string sceneTypeName, string newSceneName) {
            var a =
                (Scene)
                    Activator.CreateInstance(SceneTypes[sceneTypeName], SpriteBatch, Graphics, GraphicsDevice, Window,
                        Content, 1080, 720);
            a.SetDrawSystem(new NormalDrawSystem(GraphicsDevice, a, new Vector2(Graphics.PreferredBackBufferWidth, Graphics.PreferredBackBufferHeight)));
            LoadedScenes.Add(newSceneName, a);
            return a;
        }

        /// <summary>
        /// Loads a Scene from Scene Types. Use Set scene for already Loaded Scenes.
        /// </summary>
        /// <param name="name"></param>
        public Scene LoadScene(string name) {
            return LoadScene(name, name);
        }

        /// <summary>
        /// Sets the current Scene. Must be a already loaded Scene.
        /// </summary>
        /// <param name="loadedSceneName"></param>
        public void SetScene(string loadedSceneName) {
            CurrentScene = LoadedScenes[loadedSceneName];
        }

        /// <summary>
        /// Loads and sets a Scene from Scene Types. Use Set scene for already Loaded Scenes.
        /// </summary>
        public Scene LoadAndSetScene(string sceneTypeName, string newSceneName) {
            var a =
                (Scene)
                    Activator.CreateInstance(SceneTypes[sceneTypeName], SpriteBatch, Graphics, GraphicsDevice, Window,
                        Content, 1080,720);
            LoadedScenes.Add(newSceneName, a);
            a.SetDrawSystem(new NormalDrawSystem(GraphicsDevice, a, new Vector2(Graphics.PreferredBackBufferWidth, Graphics.PreferredBackBufferHeight)));
            SetScene(newSceneName);
            return a;
        }

        /// <summary>
        /// Loads and sets a Scene from Scene Types. Use Set scene for already Loaded Scenes.
        /// </summary>
        public Scene LoadAndSetScene(string sceneTypeName) {
            var a =
                LoadAndSetScene(sceneTypeName, sceneTypeName);
            return a;
        }

        public virtual void StartGame() {
        }

        protected override void Draw(GameTime gameTime) {
            base.Draw(gameTime);
            if (CurrentScene != null) {
                CurrentScene.DrawSystem.Draw(SpriteBatch);
            }
        }

        protected override void Initialize() {
            base.Initialize();
        }

        protected override void LoadContent() {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            StartGame();
        }

        protected override void Update(GameTime gameTime) {
            base.Update(gameTime);
            if (CurrentScene != null) {
                CurrentScene.Update(gameTime);
            }
        }
    }
}