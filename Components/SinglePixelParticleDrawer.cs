#region

using CludoEngine.Particle_System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace CludoEngine.Components {
    public class SinglePixelParticleDrawer : IComponent {
        private readonly Particle _particle;

        public SinglePixelParticleDrawer(Particle particle) {
            particle.ToggleInternalDrawing(false);
            _particle = particle;
            Name = "SinglePixelParticleDrawer";
            Type = "SinglePixelParticleDrawer";
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }

        public void Draw(SpriteBatch sb) {
            sb.Draw(CludoGame.CurrentScene.Line,
                Utils.CreateRectangle(_particle.Generator.Position.X + (int)_particle.LocalPosition.X,
                    (int)_particle.Generator.Position.Y + _particle.LocalPosition.Y, _particle.Size.X, _particle.Size.Y),
                _particle.Color);
        }

        public IComponent Clone(object[] args) {
            return new SinglePixelParticleDrawer((Particle) args[0]);
        }

        public void Update(GameTime gt) {
        }
    }
}