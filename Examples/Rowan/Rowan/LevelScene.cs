using CludoEngine;
using CludoEngine.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Game1 {

    public class LevelScene : Scene {
        private Texture2D texture;

        public LevelScene(SpriteBatch sb, GraphicsDeviceManager graphicsDeviceManager, GraphicsDevice gd, GameWindow window,
            ContentManager content, int FOVWidth, int FOVHeight)
            : base(sb, graphicsDeviceManager, gd, window, content, 1080, 720) {
        }

        public override void StartScene() {
            texture = new Texture2D(GraphicsDevice, 1, 1);
            texture.SetData(new[] { Color.Black *0.2f });
            GameObject background = new GameObject("Background", this, Vector2.Zero);
            background.Body.IsStatic = true;
            var sprite = new Sprite(background, texture);
            sprite.Width = 1920;
            sprite.Height = 1080;
            sprite.Depth = 1.0f;
            sprite.DoTransform = false;
            background.AddComponent("Sprite", sprite);
            GameObjects.AddGameObject("Background", background);

            GameObject Sky = new GameObject("Sky", this, Vector2.Zero);
            Sky.Body.IsStatic = true;
            var sprite2 = new Sprite(Sky, Pipeline.LoadContent<Texture2D>("Sky", Content, false));
            sprite2.DoTransform = false;
            sprite2.Depth = 0.02f;
            sprite2.Width = 1920;
            sprite2.Height = 1080;
            Sky.AddComponent("Sprite", sprite2);

            //TOdo: stars
            GameObject Stars = new GameObject("Stars", this, Vector2.Zero);
            Stars.Body.IsStatic = true;
            var sprite3 = new Sprite(Sky, Pipeline.LoadContent<Texture2D>("nightstars", Content, false));
            sprite3.Depth = 0.05f;
            Stars.AddComponent("Sprite", sprite3);


            GameObjects.AddGameObject("Stars", Stars);
            GameObjects.AddGameObject("Sky", Sky);
            Gravity = new Vector2(0, 9.82f);
            AddTiledPrefab("Player", typeof(Player));
            AddTiledPrefab("NewLevel", typeof(LevelLoaderPrefab));
            AddTiledPrefab("Rowan", typeof(Rowan));
            AddTiledPrefab("Intro", typeof(RowanSeq));
            AddTiledPrefab("Text", typeof(TextScreen));
            AddTiledPrefab("BadGuy", typeof(BadGuyTiled));
            AddTiledPrefab("QuickSeq", typeof(QuickSeq));
            AddTiledPrefab("Light", typeof(LightPrefab));
            AddTiledPrefab("Car", typeof(Car));
            AddTiledPrefab("LightPole", typeof(LightPole));
            AddTiledPrefab("RainDropParticle", typeof(RainStorm));
            AddTiledPrefab("IntroCredits", typeof(IntroCredits));
            AddTiledPrefab("House", typeof(House));

        }


        public override void Update(GameTime gt) {
            base.Update(gt);
        }

        public void LoadRowanMap(string name) {
            TileMap = new TileMap(this, name + ".tmx");
            TileMap.Depth = 0.06f;
        }
    }
}