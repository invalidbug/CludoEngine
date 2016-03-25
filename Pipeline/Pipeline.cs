#region

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Cludo_Engine.Pipeline
{
    public class CludoContentPipeline
    {
        public GraphicsDevice Device;

        /// <summary>
        /// The dictionary containing the fonts.
        /// </summary>
        public Dictionary<string, SpriteFont> Fonts;

        /// <summary>
        /// The dictionary containing the textures. They key being the texture name.
        /// </summary>
        public Dictionary<string, Texture2D> Textures;

        /// <summary>
        /// Constructor
        /// </summary>
        public CludoContentPipeline(GraphicsDevice gd)
        {
            Textures = new Dictionary<string, Texture2D>();
            Fonts = new Dictionary<string, SpriteFont>();
            Device = gd;
        }

        public SpriteFont DefaultFont { get; set; }
        public Texture2D DefaultFormSkin { get; set; }

        /// <summary>
        /// Adds a font to the pipeline.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="font"></param>
        public void AddFont(string name, SpriteFont font)
        {
            Fonts.Add(name, font);
            DefaultFont = font;
        }

        /// <summary>
        /// Adds a texture to the pipeline.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="texture"></param>
        public void AddTexture(string name, Texture2D texture)
        {
            Textures.Add(name, texture);
        }

        /// <summary>
        /// Gets a font.
        /// </summary>
        /// <param name="name">The name of the font.</param>
        /// <returns></returns>
        public SpriteFont GetFont(string name)
        {
            if (Fonts.ContainsKey(name))
            {
                return Fonts[name];
            }
            throw new ArgumentException("Font: " + name + " doesn't exist! Please try loading the font first!");
        }
        /// <summary>
        /// Gets a font.
        /// </summary>
        /// <param name="name">The name of the font.</param>
        /// <param name="scene">The scene currently loaded.</param>
        /// <returns></returns>
        public SpriteFont GetFont(string name, Scene scene) {
            if (Fonts.ContainsKey(name)) {
                return Fonts[name];
            }
            LoadContent<SpriteFont>(name, scene.Content);
            return GetFont(name);
        }
        /// <summary>
        /// Gets a texture from the currently loaded textures.(Does not attempt to load a texture from Filesystem, try AddTexture)
        /// </summary>
        /// <param name="name">The name of the texture.</param>
        /// <returns></returns>
        public Texture2D GetTexture(string name)
        {
            if (name == "null") return null;
            if (Textures.ContainsKey(name))
            {
                return Textures[name];
            }
            throw new ArgumentException("Texture: " + name + " doesn't exist!");
        }

        /// <summary>
        /// Gets a texture from the currently loaded textures, if there is no Texture loaded, it will be loaded.
        /// </summary>
        /// <param name="name">The name of the texture.</param>
        /// <param name="scene">The scene currently loaded.</param>
        /// <returns></returns>
        public Texture2D GetTexture(string name,Scene scene)
        {
            if (scene == null) throw new ArgumentNullException("scene");
            if (Textures.ContainsKey(name))
            {
                return Textures[name];
            }
            else
            {
                LoadContent<Texture2D>(name, scene.Content, true);
                return GetTexture(name);
            }
        }

        /// <summary>
        /// Loads content then returns it. Optional: Add to pipeline.
        /// </summary>
        /// <typeparam name="T">The type of content being loaded</typeparam>
        /// <param name="name">The name of the content</param>
        /// <param name="content">Content Manager</param>
        /// <param name="addToPipeline">If the content should be added to the pipeline. Judging on performance, it'll be faster to nest this function with a AddTexture because this function in order to add to the pipeline, we have to figure out what type it is then select the right function(which in the texture case, is AddTexture).</param>
        /// <returns></returns>
        public T LoadContent<T>(string name, ContentManager content, bool addToPipeline = true)
        {
            var t = content.Load<T>(name);
            if (!addToPipeline) return t;
            switch (t.GetType().ToString())
            {
                case "Microsoft.Xna.Framework.Graphics.Texture2D":
                    AddTexture(name, t);
                    break;

                case "Microsoft.Xna.Framework.Graphics.SpriteFont":
                    AddFont(name, t);
                    break;
            }
            return t;
        }

        /// <summary>
        /// Loads content then returns it. Optional: Add to pipeline.
        /// </summary>
        /// <typeparam name="T">The type of content being loaded</typeparam>
        /// <param name="name">The name of the content</param>
        /// <param name="game">The game class.</param>
        /// <param name="addToPipeline">If the content should be added to the pipeline. Judging on performance, it'll be faster to nest this function with a AddTexture because this function in order to add to the pipeline, we have to figure out what type it is then select the right function(which in the texture case, is AddTexture).</param>
        /// <returns></returns>
        public T LoadContent<T>(string name, Game game, bool addToPipeline = true)
        {
            return LoadContent<T>(name, game.Content, addToPipeline);
        }

        private void AddFont(string name, object font)
        {
            Fonts.Add(name, (SpriteFont) font);
            DefaultFont = (SpriteFont) font;
        }

        private void AddTexture(string name, object texture)
        {
            if (!Textures.ContainsKey(name))
                Textures.Add(name, (Texture2D) texture);
        }
    }
}