using CludoEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using CludoEngine.Graphics;
using TiledSharp;

namespace Game1 {

    public class RainDrop {
        public Vector2 position { get; set; }
        public float TimeSinceLastCheck { get; set; }
        public float Z { get; set; }
        public bool ReadyToReset {
            get {
                if (position.Y > 1080) {
                    return true;
                }

                return false;
            }
        }
    }

    public class RainStorm : TiledPrefab {
        public static Scene scene;
        private Texture2D rainDrop;
        public List<RainDrop> drops;
        private Random rnd;

        public RainStorm(TmxObject e, Scene scene)
            : base(e, scene) {
            RainStorm.scene = scene;
            rnd = new Random(21354);
            rainDrop = scene.Pipeline.GetTexture("RainDrop");
            drops = new List<RainDrop>();
            while (drops.Count < 3250) {
                drops.Add(new RainDrop() { position = new Vector2(rnd.Next(1100, 1900), rnd.Next(-1200, 0)), Z = rnd.Next(8,19)/10});
            }
            this.Depth = 0.99f;
        }

        public override void Draw(SpriteBatch sb) {
            foreach (RainDrop i in drops) {
                if (i.position.X > 1080)
                    continue;
                sb.End();
                sb.Begin(SpriteSortMode.BackToFront, BlendState.NonPremultiplied, SamplerState.PointClamp);
                sb.Draw(rainDrop, i.position);
                sb.End();
                ((NormalDrawSystem)scene.DrawSystem).ReadySpriteBatch(sb);
            }
        }

        public override void Update(GameTime gt) {
            for (int index = 0; index < drops.Count; index++) {
                if (drops[index].TimeSinceLastCheck > 1) {
                    if (drops[index].ReadyToReset) {
                        drops[index].position = new Vector2(rnd.Next(1100, 2100), rnd.Next(-1200, 0));
                    }
                    drops[index].TimeSinceLastCheck = 0;
                }
                drops[index].TimeSinceLastCheck += (Single)gt.ElapsedGameTime.TotalSeconds;
                float movement = (250*drops[index].Z) * (Single)gt.ElapsedGameTime.TotalSeconds;
                drops[index].position += new Vector2(-movement, movement);
            }
        }
    }
}