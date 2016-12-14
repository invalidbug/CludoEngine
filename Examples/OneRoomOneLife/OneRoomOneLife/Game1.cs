using CludoEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace OneRoomOneLife {
    public class Game1 : CludoGame {
        private Scene _room;
        public override void StartGame() {
            AddScene(typeof(RoomScene), "MainScene");
            this.Graphics.PreferredBackBufferWidth = 1280;
            this.Graphics.PreferredBackBufferHeight = 900;
            this.Graphics.ApplyChanges();
            _room = new RoomScene(SpriteBatch,Graphics,GraphicsDevice,this.Window,Content,1280,900);
            _room = LoadScene("MainScene");
            SetScene("MainScene");

            base.StartGame();
        }

    }
}
