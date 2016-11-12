using System;
using CludoEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Simple_Asteroid_game {
    public class MainScene : Scene {
        public static Player Player;
        public static bool timeToSpawn = false;
        
        public MainScene(SpriteBatch sb, GraphicsDeviceManager graphicsDeviceManager, GraphicsDevice gd, GameWindow window, ContentManager content, int FOVWidth, int FOVHeight) : base(sb, graphicsDeviceManager, gd, window, content, FOVWidth, FOVHeight) {
        }
        public override void StartScene() {
            base.StartScene();
            Pipeline.LoadContent<Texture2D>("Spaceship", Content, true);
            Pipeline.LoadContent<Texture2D>("Shot", Content, true);


            Player = new Player("Player1", this, Vector2.One* 350);
            Player.Body.IsStatic = true;
            GameObjects.AddGameObject("Player1", Player);

            EnemyShip ship = new EnemyShip("Enemy", this, Vector2.One);
            ship.Body.IsStatic = true;
            GameObjects.AddGameObject("Enemy", ship);
            // enable this to see the collision boxes
            this.Debug = false;
        }

        private float _timeSinceSpawn;
        public static void SpawnShip() {

            timeToSpawn = true;
        }

        public void ActuallySpawnShip() {
            EnemyShip ship = new EnemyShip("Enemy", CludoGame.CurrentScene, Vector2.One);
            ship.Body.IsStatic = true;
            CludoGame.CurrentScene.GameObjects.AddGameObject("Enemy", ship);
            timeToSpawn = false;
        }
        public override void Update(GameTime gt) {
            base.Update(gt);
            if (timeToSpawn) {
                _timeSinceSpawn += (Single) gt.ElapsedGameTime.TotalSeconds;
                if (_timeSinceSpawn > 3.5f) {
                    ActuallySpawnShip();
                    _timeSinceSpawn = 0;
                }
            }

        }
    }
}
