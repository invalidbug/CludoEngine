using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CludoEngine.Graphics {

    public interface IDrawSystem {
        Vector2 Resolution { get; set; }

        void Draw(SpriteBatch sb);

        void ApplyResolutionChange();

        void AddTiledPrefab(TiledPrefab prefab);

        void AddTileMap(TileMap map);
    }
}