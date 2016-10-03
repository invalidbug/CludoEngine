#region

using CludoEngine.Particle_System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace CludoEngine.Components {
    public class SinglePixelParticleDrawer : IComponent {
        private readonly Particle _particle;

        public SinglePixelParticleDrawer(Particle particle, Texture2D t = null) {
            particle.ToggleInternalDrawing(false);
            _particle = particle;
            Name = "SinglePixelParticleDrawer";
            Type = "SinglePixelParticleDrawer";
            if (t == null) {
                texture = CludoGame.CurrentScene.Line;
            }
            else {
                this.texture = t;
            }
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public Texture2D texture;

        public void Draw(SpriteBatch sb) {
            sb.Draw(texture,
                Utils.CreateRectangle(_particle.Generator.Position.X + (int)_particle.LocalPosition.X,
                    (int)_particle.Generator.Position.Y + _particle.LocalPosition.Y, _particle.Size.X, _particle.Size.Y),
                _particle.Color);
        }

        public IComponent Clone(object[] args) {
            return new SinglePixelParticleDrawer((Particle) args[0],texture);
        }

        public void Update(GameTime gt) {
        }
    }
}