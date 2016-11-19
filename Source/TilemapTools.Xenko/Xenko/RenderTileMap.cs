using SiliconStudio.Xenko.Engine;
using SiliconStudio.Xenko.Rendering;

namespace TilemapTools.Xenko
{
    [DefaultPipelinePlugin(typeof(TileMapPipelinePlugin))]
    public class RenderTileMap : RenderObject
    {
        public TileMapComponent TileMapComponent;
        public TransformComponent TransformComponent;
    }
}