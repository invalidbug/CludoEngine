using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CludoEngine;
using CludoEngine.Components;
using CludoEngine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace OneRoomOneLife {
    public class RoomScene : Scene {
        private Player _player;

        public RoomScene(SpriteBatch sb, GraphicsDeviceManager graphicsDeviceManager, GraphicsDevice gd, GameWindow window,
            ContentManager content, int FOVWidth, int FOVHeight)
            : base(sb, graphicsDeviceManager, gd, window, content, 1280, 900) {
        }

        public Map Map { get; set; }
        public override void StartScene() {
            base.StartScene();
            this.World.Gravity = Vector2.Zero;
            LoadContent();
            this.Debug = false;

            Map = new Map(this);
            _player = new Player(this);

        }

        public override void Update(GameTime gt) {
            base.Update(gt);
            _player.Update(gt);
        }

        private void LoadContent() {
            Pipeline.LoadContent<Texture2D>("map", Content, true);
            Pipeline.LoadContent<Texture2D>("Character", Content, true);
            Pipeline.LoadContent<Texture2D>("Unreal", Content, true);
            Pipeline.LoadContent<Texture2D>("SocialMediaNetwork", Content, true);
            Pipeline.LoadContent<Texture2D>("MapNight", Content, true);
            Pipeline.LoadContent<Texture2D>("Black", Content, true);
        }
    }
}
