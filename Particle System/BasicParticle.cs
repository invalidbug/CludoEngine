#region

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace CludoEngine.Particle_System {
    public class BasicParticle : ParticleBase, ICloneable<ParticleBase> {
        private readonly Scene _scene;
        private Color _drawColor;

        public BasicParticle(Color initialColor, Color initalSecondColor, Vector2 particleSize, float particleLife,
            Scene scene)
            : base(initialColor, initalSecondColor, particleSize, particleLife) {
            _scene = scene;
        }

        public Vector2 Gravity { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }

        public ParticleBase Clone() {
            return new BasicParticle(initialColor, initialSecondColor, _particleSize, _initialStartDuration, _scene);
        }

        public override void Draw(SpriteBatch sb) {
            base.Draw(sb);
            sb.Draw(_scene.Line, Utils.CreateRectangle(0, 0, _particleSize.X, _particleSize.Y), _drawColor);
        }

        public override void Update(GameTime gt) {
            base.Update(gt);
            // We just want to calculate this on updates not draws for performance.
            _drawColor = DrawColor;
            Position += Velocity;
            Velocity -= Gravity;
        }
    }
}