#region

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace CludoEngine.Graphics
{
    public class SpriteSheet
    {
        private Pipeline.CludoContentPipeline _pipeline;
        public Dictionary<string, Rectangle> Frames;
        public string SheetName;

        public SpriteSheet(string sheetname, Pipeline.CludoContentPipeline pipeline)
        {
            _pipeline = pipeline;
            SheetName = sheetname;
            Frames = new Dictionary<string, Rectangle>();
        }

        public Texture2D Sheet
        {
            get { return _pipeline.GetTexture(SheetName); }
        }

        public void AddFrame(string name, Rectangle source)
        {
            Frames.Add(name, source);
        }

        public Rectangle GetFrame(string name)
        {
            return Frames[name];
        }

        public void SetFrame(string name, Rectangle newSource)
        {
            if (Frames.ContainsKey(name))
            {
                Frames[name] = newSource;
            }
            else
            {
                AddFrame(name, newSource);
            }
        }
    }
}