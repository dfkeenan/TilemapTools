using System;
using SiliconStudio.Xenko.Engine;
using SiliconStudio.Xenko.Rendering;

namespace TilemapTools.Xenko
{
    public class TileMapRendererProcessor : EntityProcessor<TileMapComponent, RenderTileMap>, IEntityComponentRenderProcessor
    {
        public VisibilityGroup VisibilityGroup { get; set; }

        protected override RenderTileMap GenerateComponentData(Entity entity, TileMapComponent component)
        {
            throw new NotImplementedException();
        }
    }
}