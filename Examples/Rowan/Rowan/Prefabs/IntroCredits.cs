using CludoEngine;
using CludoEngine.Debugging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using CludoEngine.Graphics;
using TiledSharp;

namespace Game1 {

    public class IntroCredits : TiledPrefab {
        private readonly Scene _scene;
        public float time;
        public int state;

        public IntroCredits(TmxObject e, Scene scene)
            : base(e, scene) {
            _scene = scene;
            state = 0;
            this.Depth = 1.0f;
        }

        public override void Update(GameTime gt) {
            time += (Single)gt.ElapsedGameTime.TotalSeconds;
            if (time > 9) {
                state = 1;
            }
            if (time > 14.3f) {
                state = 2;
            }
            if (time > 24) {
                state = 3;
            }
            if (time > 27) {
                state = 4;
            }
            if (time > 32) {
                state = 5;
            }
            if (time > 37) {
                state = 6;
            }
            if (time > 39) {
                state = 7;
            }
        }

        public override void Draw(SpriteBatch sb) {
            sb.End();
            Vector2 RowanSize = _scene.Pipeline.GetFont("superfont").MeasureString("Rowan") * new Vector2(7, 7);
            Vector2 DarinSize = _scene.Pipeline.GetFont("superfont").MeasureString("A game created by Darin Boyer") * new Vector2(1.4f, 1);
            switch (state) {
                case (0):
                sb.Begin(SpriteSortMode.BackToFront, BlendState.NonPremultiplied, SamplerState.PointClamp);
                sb.Draw(CludoGame.CurrentScene.Line, Vector2.Zero, new Rectangle(0, 0, 1080, 720), Color.Black);
                sb.DrawString(_scene.Pipeline.GetFont("superfont"), "Rowan", new Vector2(1080 / 2 - (RowanSize.X / 2), 200), Color.White, 0, Vector2.Zero, new Vector2(7, 7), SpriteEffects.None, 0f);
                sb.DrawString(_scene.Pipeline.GetFont("superfont"), "A game created by Darin Boyer", new Vector2(1080 / 2 - (DarinSize.X / 2), 400), Color.White, 0, Vector2.Zero, new Vector2(1.4f, 1), SpriteEffects.None, 0f);
                sb.End();
                break;

                case (1):
                sb.Begin(SpriteSortMode.BackToFront, BlendState.NonPremultiplied, SamplerState.PointClamp);
                float transparentPercentage = (((14.3f - 9) / (time - 9)) - 1);
                sb.Draw(CludoGame.CurrentScene.Line, Vector2.Zero, new Rectangle(0, 0, 1080, 720), Color.Black * transparentPercentage);
                sb.DrawString(_scene.Pipeline.GetFont("superfont"), "Rowan", new Vector2(1080 / 2 - (RowanSize.X / 2), 200), Color.White * transparentPercentage, 0, Vector2.Zero, new Vector2(7, 7), SpriteEffects.None, 0f);
                sb.DrawString(_scene.Pipeline.GetFont("superfont"), "A game created by Darin Boyer", new Vector2(1080 / 2 - (DarinSize.X / 2), 400), Color.White * transparentPercentage, 0, Vector2.Zero, new Vector2(1.4f, 1), SpriteEffects.None, 0f);
                sb.End();
                Debug.WriteLine(transparentPercentage);
                break;

                case (2):
                sb.Begin(SpriteSortMode.BackToFront, BlendState.NonPremultiplied);
                sb.DrawString(_scene.Pipeline.GetFont("superfont"), "With inspiration from" + Environment.NewLine + "Undertale and Super Meat Boy", new Vector2(500, 600), Color.White, 0, Vector2.Zero, new Vector2(1.2f, 1), SpriteEffects.None, 0f);
                sb.End();
                break;

                case (4):
                sb.Begin(SpriteSortMode.BackToFront, BlendState.NonPremultiplied);
                sb.DrawString(_scene.Pipeline.GetFont("superfont"), "A game about anxiety", new Vector2(500, 600), Color.White, 0, Vector2.Zero, new Vector2(1.2f, 1), SpriteEffects.None, 0f);
                sb.End();
                break;

                case (6):
                sb.Begin(SpriteSortMode.BackToFront, BlendState.NonPremultiplied);
                sb.Draw(CludoGame.CurrentScene.Line, Vector2.Zero, new Rectangle(0, 0, 1080, 720), Color.Black * ((time - 28) / (32 - 28)));
                sb.End();
                break;

                case (7):
                ((Game1)CludoGame.CurrentGame).LoadRowanScene("map1");
                break;
            }
            ((NormalDrawSystem)_scene.DrawSystem).ReadySpriteBatch(sb);
        }
    }
}