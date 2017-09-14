using SiliconStudio.Xenko.Engine;
using SiliconStudio.Xenko.Rendering;
using TilemapTools.Xenko.Graphics;

namespace TilemapTools.Xenko.Rendering
{
    public class RenderTileMap : RenderObject
    {
        public TileMapComponent TileMapComponent;
        public TransformComponent TransformComponent;
        public TileMesh TileMesh;
        public TileGrid Grid;
    }
}