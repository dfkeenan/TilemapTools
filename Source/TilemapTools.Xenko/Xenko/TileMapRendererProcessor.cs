using System;
using SiliconStudio.Xenko.Engine;
using SiliconStudio.Xenko.Rendering;

namespace TilemapTools.Xenko
{
    public class TileMapRendererProcessor : EntityProcessor<TileMapComponent, RenderTileMap>, IEntityComponentRenderProcessor
    {
        public TileMapRendererProcessor()
            : base(typeof(TransformComponent))
        {
           
        }

        public VisibilityGroup VisibilityGroup { get; set; }

        protected override RenderTileMap GenerateComponentData(Entity entity, TileMapComponent component)
        {
            return new RenderTileMap
            {
                TileMapComponent = component,
                TransformComponent = entity.Transform
            };
        }

        protected override bool IsAssociatedDataValid(Entity entity, TileMapComponent component, RenderTileMap associatedData)
        {
            return
                component == associatedData.TileMapComponent &&
                entity.Transform == associatedData.TransformComponent;
        }

        protected override void OnEntityComponentAdding(Entity entity, TileMapComponent component, RenderTileMap data)
        {
            VisibilityGroup.RenderObjects.Add(data);
        }

        protected override void OnEntityComponentRemoved(Entity entity, TileMapComponent component, RenderTileMap data)
        {
            VisibilityGroup.RenderObjects.Remove(data);
        }
    }
}