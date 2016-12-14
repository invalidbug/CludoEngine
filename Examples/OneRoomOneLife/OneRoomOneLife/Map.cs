using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CludoEngine;
using CludoEngine.Components;
using Microsoft.Xna.Framework;

namespace OneRoomOneLife {
    public class Map {
        private readonly Scene _scene;
        private Sprite _sprite;
        private TileUtils _util;

        public Map(Scene _scene) {
            this._scene = _scene;
            GetMap = new GameObject("Map", _scene, Vector2.Zero);
            _sprite = new Sprite(GetMap, "map");
            _sprite.Depth = 0.5f;
            GetMap.AddComponent("Texture", _sprite);
            _scene.GameObjects.AddGameObject("Map", GetMap);
            _util = new TileUtils();

            // lets generate colliders
            // first make it static.
            GetMap.Static = true;
            // wall
            Vector2 pos = _util.ConvertTileToWorld(0, 2.3f);
            Vector2 size = _util.ConvertTileToWorld(27, 2.3f);
            GetMap.AddComponent("WallTop", new RectangleCollider((int)pos.X + (int)size.X/2, (int)pos.Y, (int)size.X, (int)size.Y, 1f));
            GetMap.AddComponent("WallBottom", new RectangleCollider(1280/2, 900, 1280, 1, 1f));
            GetMap.AddComponent("WallLeft", new RectangleCollider(0, 900/2, 1, 900, 1f));
            GetMap.AddComponent("WallRight", new RectangleCollider(1280, 900 / 2, 1, 900, 1f));

            // Piano
            GetMap.AddComponent("Piano", new RectangleCollider(0 + (84 / 2), 336 + (268/2), 84, 268, 1f));
            Vector2 l = _util.ConvertTileToWorld(1.8f, 8.8f);
            GetMap.AddComponent("Chair", new RectangleCollider((int)l.X, (int)l.Y+(50/2), 125, 100, 1f));

            // Desk
            GetMap.AddComponent("Desk", new RectangleCollider(709+(334/2), 132+(113/2),334, 113, 1f));
            GetMap.AddComponent("DeskChar", new RectangleCollider(825+(82/2), 224+(70/2), 82, 70, 1f));

            // bed
            GetMap.AddComponent("Bed", new RectangleCollider(0+(207/2), 154+(116/2), 207, 116, 1f));
        }
        //TODO: ew
        public GameObject GetMap { get; private set; }

        public void ArtifialToggle(bool t) {
            if (t) {
                _sprite.Height = 1280;
            } else {
                //_sprite.Height = 0;
            }
        }
    }
}
