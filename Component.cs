﻿#region

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace CludoEngine
{
    public interface IComponent
    {
        int Id { get; set; }
        string Name { get; set; }
        string Type { get; set; }

        void Draw(SpriteBatch sb);

        void Update(GameTime gt);
    }
}