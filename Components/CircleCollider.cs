﻿#region

using System;

#endregion

namespace CludoEngine.Components {

    public struct CircleCollider : IComponent {

        public CircleCollider(int localX, int localY, float Radius, float density)
            : this() {
            LocalX = localX;
            LocalY = localY;
            this.Radius = Radius;
            Density = density;
            Type = "CircleCollider";
        }

        public float Density { get; set; }
        public float Radius { get; set; }
        public int Id { get; set; }
        public int LocalX { get; set; }
        public int LocalY { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }

        public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sb) {
        }

        public IComponent Clone(object[] args) {
            throw new NotImplementedException();
        }

        public void Update(Microsoft.Xna.Framework.GameTime gt) {
        }
    }
}