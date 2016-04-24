#region

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace CludoEngine.Particle_System {
    public class Particle : ComponentSystem, ICloneable<Particle> {
        private readonly float _timetodispose;
        private float _currentlivetime;
        private bool _draws;
        public ParticleGenerator Generator;

        public Particle(Vector2 localPosition, Vector2 size, Color color, float timetodispose) {
            Components = new Dictionary<int, IComponent>();
            _timetodispose = timetodispose;
            LocalPosition = localPosition;
            Size = size;
            Color = color;
        }

        public Texture2D Texture { get; set; }
        public Color Color { get; set; }
        public Vector2 LocalPosition { get; set; }
        public Vector2 Position { get { return LocalPosition + Generator.Position; } }
        public Vector2 Size { get; set; }
        public bool TimeToDispose { get; internal set; }

        public Particle Clone() {
            var p = new Particle(LocalPosition, Size, Color, _timetodispose) {Generator = Generator};
            p.ToggleInternalDrawing(_draws);
            foreach (var i in Components.Values) {
                p.AddComponent(i.Name, i.Clone(new object[] {p,"Particle"}));
            }
            return p;
        }

        public void Draw(SpriteBatch sb) {
            if (Texture != null && _draws) {
                sb.Draw(Texture,
                    Utils.CreateRectangle(Generator.Position.X + LocalPosition.X, Generator.Position.Y + LocalPosition.Y,
                        Size.X, Size.Y), Color);
            }
            DrawComponets(sb);
        }

        public void Update(GameTime gt) {
            UpdateComponents(gt);
            _currentlivetime += (float) gt.ElapsedGameTime.TotalSeconds;
            if (_currentlivetime >= _timetodispose) {
                TimeToDispose = true;
            }
        }

        public void ToggleInternalDrawing() {
            ToggleInternalDrawing(!_draws);
        }

        public void ToggleInternalDrawing(bool value) {
            _draws = value;
        }
    }
}