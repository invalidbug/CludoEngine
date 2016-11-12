using CludoEngine;

namespace Simple_Asteroid_game {
    
    public class Game1 : CludoGame {
        private Scene _scene;
        public override void StartGame() {
            // Add scene type
            AddScene(typeof(MainScene), "MainScene");
            // Change resolution and stuffs
            this.Graphics.PreferredBackBufferHeight = 720;
            this.Graphics.PreferredBackBufferWidth = 1080;
            this.Graphics.ApplyChanges();

            // Create scene, and load it.
            _scene = LoadScene("MainScene");
            SetScene("MainScene");
            
            
            this.IsMouseVisible = true;
        }
    }
}
