#region

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TiledSharp;

#endregion

namespace Cludo_Engine
{
    internal struct TileSheet
    {
        public Texture2D Texture { get; set; }
        public int Id { get; set; }

        public TileSheet(Texture2D textur, int I)
            : this()
        {
            Texture = textur;
            Id = I;
        }
    }

    public class TileMap : IDrawable
    {
        private TmxMap _map;
        private Scene _scene;
        private Dictionary<int, TileSheet> _tilesets;

        public Body Terrain;

        public TileMap(Scene scene, string tiledMapName)
        {
            // Create tmx map
            _map = new TmxMap(tiledMapName);
            _scene = scene;
            // create tilesets dictionary
            _tilesets = new Dictionary<int, TileSheet>();
            ParseTilemap(scene);
        }

        public void Draw(SpriteBatch sb)
        {
            Draw(sb, false);
        }

        public void AddToTarget()
        {
            _scene.RenderTargets["Game"].AddDrawable(CludoRenderTargetLayers.Game() - 0.15F, this);
        }

        public bool TestIfDrawNeeded()
        {
            return true;
        }

        private void ParseTilemap(Scene scene)
        {
            ParseColliderLayer(scene);
            ParseGameObjects(scene);
            StartTiledPrefabs(scene);
        }

        private void StartTiledPrefabs(Scene scene) {
            foreach (TiledPrefab prefab in scene.LoadedTiledPrefabs)
            {
                prefab.Start();
            }
        }

        private void ParseGameObjects(Scene scene)
        {
            foreach (var i in _map.ObjectGroups)
            {
                foreach (var obj in i.Objects)
                {
                    if (obj.Properties.ContainsKey("Static") == false && obj.Properties.Count > 0)
                    {
                        // Its a prefab or type of prefab!
                        if (obj.Properties.ContainsKey("Prefab"))
                        {
                            scene.CreateTiledPrefab(obj.Properties["Prefab"], obj);
                        }
                    }
                    else
                    {
                        // Its a normal collider, seperate from tilemap.
                        var name = obj.Name;
                        if (String.IsNullOrEmpty(name))
                        {
                            name = obj.ToString();
                        }
                        var a = new GameObject(name, scene, new Vector2((float) obj.X, (float) obj.Y));
                        // Basic and Ellipse
                        switch (obj.ObjectType.ToString())
                        {
                            case "Basic":
                                //rectangle
                                Components.RectangleCollider c;
                                // create the collider
                                c = new Components.RectangleCollider((int) obj.Width/2, (int) obj.Height/2,
                                    Convert.ToInt32((float) obj.Width), Convert.ToInt32((float) obj.Height), 1f);
                                a.Rotation = MathHelper.ToRadians((float) obj.Rotation);
                                a.AddComponent("", c);
                                if (obj.Properties.ContainsKey("Static"))
                                {
                                    if (obj.Properties["Static"].ToLower() == "true")
                                    {
                                        a.Static = true;
                                    }
                                    else
                                    {
                                        a.Static = false;
                                    }
                                }
                                if (obj.Properties.ContainsKey("Friction"))
                                    a.Body.Friction = float.Parse(obj.Properties["Friction"],
                                        CultureInfo.InvariantCulture.NumberFormat);
                                if (obj.Properties.ContainsKey("Restitution"))
                                    a.Body.Restitution = float.Parse(obj.Properties["Restitution"],
                                        CultureInfo.InvariantCulture.NumberFormat);
                                if (obj.Properties.ContainsKey("Linear Damping"))
                                    a.Body.LinearDamping = float.Parse(obj.Properties["Linear Damping"],
                                        CultureInfo.InvariantCulture.NumberFormat);
                                if (obj.Properties.ContainsKey("Angular Damping"))
                                    a.Body.AngularDamping = float.Parse(obj.Properties["Angular Damping"],
                                        CultureInfo.InvariantCulture.NumberFormat);
                                break;

                            case "Ellipse":
                                //Ellipse
                                break;

                            default:
                                System.Diagnostics.Debug.Write("Didnt get Object type, possible TiledSharp error?");
                                break;
                        }
                        scene.GameObjects.AddGameObject(name, a);
                    }
                }
            }
        }

        private void ParseColliderLayer(Scene scene)
        {
            // create render target
            var target = new RenderTarget2D(Scene.GraphicsDevice, _map.Width*_map.TileWidth, _map.Height*_map.TileHeight);
            var anyvisible = false;
            // for each layer...
            foreach (var i in _map.Layers)
            {
                if (i.Visible)
                {
                    // if it properly contained "tileset" key
                    if (i.Properties.ContainsKey("Tileset"))
                    {
                        // Add by the index of i(the tmxlayer), tilesheet loading the Tileset property and the tileset id
                        _tilesets.Add(_map.Layers.IndexOf(i),
                            new TileSheet(
                                scene.Pipeline.LoadContent<Texture2D>(i.Properties["Tileset"], scene.Content),
                                Convert.ToInt32(i.Properties["ID"])));
                        if (i.Name.Contains("Collision Layer"))
                        {
                            anyvisible = true;
                            Scene.GraphicsDevice.SetRenderTarget(target);
                            Scene.GraphicsDevice.Clear(Color.Transparent);
                            Scene.SpriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied,
                                SamplerState.PointClamp);
                            DrawLayer(Scene.SpriteBatch, i, true);
                            Scene.SpriteBatch.End();
                        }
                    }
                }
            }
            if (anyvisible)
                Terrain = target.ConvertToBody(scene);
            target.Dispose();
            Scene.GraphicsDevice.SetRenderTarget(null);
            AddToTarget();
        }

        public int GetTilesInARow(int width, int tilesize)
        {
            return width/tilesize;
        }

        public Rectangle GetSpritePosRect(int tileSetWidth, int tileNumber, int tilesize)
        {
            return new Rectangle(tileNumber%GetTilesInARow(tileSetWidth, tilesize)*tilesize,
                tileNumber/GetTilesInARow(tileSetWidth, tilesize)*tilesize, tilesize, tilesize);
        }

        public void Draw(SpriteBatch sb, bool drawCollisionLayer = false)
        {
            foreach (var i in _map.Layers)
            {
                if (drawCollisionLayer == false && i.Name.Contains("Collision Layer"))
                {
                    continue;
                }
                if (!i.Properties.ContainsKey("Tileset"))
                {
                    continue;
                }
                DrawLayer(sb, i);
            }
        }

        public void DrawLayer(SpriteBatch sb, TmxLayer i, bool drawCollisionLayer = false)
        {
            if (drawCollisionLayer == false && i.Name.Contains("Collision Layer"))
            {
                return;
            }
            foreach (var t in i.Tiles)
            {
                if (t.Gid == 0)
                {
                    continue;
                }
                var sheet = _tilesets[_map.Layers.IndexOf(i)];
                var offset = _map.Tilesets.ElementAt(sheet.Id).FirstGid;

                var x = t.X*_map.TileWidth;
                var y = t.Y*_map.TileHeight;

                var tilesetRec = GetSpritePosRect(sheet.Texture.Width, t.Gid - offset, 64);
                sb.Draw(sheet.Texture, new Rectangle(x, y, 64, 64), tilesetRec, Color.White);
            }
        }

        public void DrawLayer(SpriteBatch sb, string name, bool drawCollisionLayer = false)
        {
            if (drawCollisionLayer == false && name.Contains("Collision Layer"))
            {
                return;
            }
            var i = _map.Layers[name];
            DrawLayer(sb, i, drawCollisionLayer);
        }
    }
}