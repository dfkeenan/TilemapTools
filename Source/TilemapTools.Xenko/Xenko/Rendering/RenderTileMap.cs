using SiliconStudio.Xenko.Engine;
using SiliconStudio.Xenko.Rendering;

namespace TilemapTools.Xenko.Rendering
{
    [DefaultPipelinePlugin(typeof(TileMapPipelinePlugin))]
    public class RenderTileMap : RenderObject
    {
        public TileMapComponent TileMapComponent;
        public TransformComponent TransformComponent;
    }
}