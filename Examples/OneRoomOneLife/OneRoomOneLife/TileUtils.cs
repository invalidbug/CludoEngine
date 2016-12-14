using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace OneRoomOneLife {
    public class TileUtils {
        public int TileWidth { get; set; }
        public int TileHeight { get; set; }

        public TileUtils() {
            TileWidth = 48;
            TileHeight = 48;
        }

        public Vector2 ConvertTileToWorld(float x, float y) {
            return new Vector2(x*this.TileWidth, y*this.TileHeight);
        }
    }
}
