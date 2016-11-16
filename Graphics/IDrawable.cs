using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CludoEngine.Graphics {
    public interface IDrawable {
        bool DoTransform { get; set; }
        float Depth { get; set; }
        Color Color { get; set; }
        void Draw(SpriteBatch sb);
    }
}
