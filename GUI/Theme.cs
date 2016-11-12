#region

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace CludoEngine.GUI {

    public class Theme {

        public Theme(SpriteFont font, Texture2D buttonTexture, Rectangle buttonFrameSource, Texture2D textboxTexture,
            Rectangle textboxFrameSource, Texture2D formTexture, Rectangle formFrameSource) {
            Button = new GuiTexture { Texture = buttonTexture, Source = buttonFrameSource };
            Textbox = new GuiTexture { Texture = textboxTexture, Source = textboxFrameSource };
            Form = new GuiTexture { Texture = formTexture, Source = formFrameSource };
            Font = font;
        }

        public GuiTexture Button { get; set; }
        public GuiTexture Textbox { get; set; }
        public GuiTexture Form { get; set; }
        public SpriteFont Font { get; set; }
    }
}