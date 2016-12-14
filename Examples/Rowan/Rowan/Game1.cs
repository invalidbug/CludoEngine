using CludoEngine;
using Microsoft.Xna.Framework.Graphics;

namespace Game1 {

    public class Game1 : CludoGame {
        private LevelScene _scene;

        public override void StartGame() {
            AddScene(typeof(LevelScene), "LevelScene");
            this.Graphics.PreferredBackBufferWidth = 1080;
            this.Graphics.PreferredBackBufferHeight = 720;
            this.Graphics.IsFullScreen = false;
            this.Graphics.ApplyChanges();

            LoadRowanScene("Intro");
        }

        public void LoadRowanScene(string map) {
            foreach (Scene i in this.LoadedScenes.Values) {
                i.Dispose();
            }
            this.LoadedScenes.Clear();
            _scene = (LevelScene)LoadScene("LevelScene");
            _scene.Pipeline.LoadContent<Texture2D>("Player", Content, true);
            _scene.Pipeline.LoadContent<Texture2D>("Rowan", Content, true);
            _scene.Pipeline.LoadContent<Texture2D>("BadGuy", Content, true);
            _scene.Pipeline.LoadContent<Texture2D>("car", Content, true);
            _scene.Pipeline.LoadContent<Texture2D>("LightPole", Content, true);
            _scene.Pipeline.LoadContent<Texture2D>("RainDrop", Content, true);
            _scene.Pipeline.LoadContent<SpriteFont>("superfont", Content, true);
            _scene.Pipeline.LoadContent<Texture2D>("House", Content, true);

            _scene.Debug = false;
            SetScene("LevelScene");
            System.Threading.Thread.Sleep(10);
            _scene.LoadRowanMap(map);
        }
    }
}