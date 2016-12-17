using System;
using SiliconStudio.Xenko.Engine;

namespace TilemapTools.Xenko
{
    public class TileMapProcessor : EntityProcessor<TileMapComponent, TileMapComponent>
    {
        protected override TileMapComponent GenerateComponentData(Entity entity, TileMapComponent component)
        {
            return component;
        }
    }
}