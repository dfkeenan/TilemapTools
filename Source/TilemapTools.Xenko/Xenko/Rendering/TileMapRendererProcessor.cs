using System;
using SiliconStudio.Core.Mathematics;
using SiliconStudio.Xenko.Engine;
using SiliconStudio.Xenko.Rendering;

namespace TilemapTools.Xenko.Rendering
{
    public class TileMapRendererProcessor : EntityProcessor<TileMapComponent, RenderTileMap>, IEntityComponentRenderProcessor
    {
        public TileMapRendererProcessor()
            : base(typeof(TransformComponent))
        {
           
        }

        public VisibilityGroup VisibilityGroup { get; set; }

        public override void Draw(RenderContext context)
        {
            foreach (var tileMapStateKeyPair in ComponentDatas)
            {
                var renderTileMap = tileMapStateKeyPair.Value;

                renderTileMap.Enabled = renderTileMap.TileMapComponent.Enabled;

                if (renderTileMap.Enabled)
                {
                    //TODO: Is this where meshes should be updated/generated??


                    var transform = renderTileMap.TransformComponent;

                    // TODO GRAPHICS REFACTOR: Proper bounding box. Reuse calculations in sprite batch.
                    // For now we only set a center for sorting, but no extent (which disable culling)
                    renderTileMap.BoundingBox = new BoundingBoxExt { Center = transform.WorldMatrix.TranslationVector };
                    renderTileMap.RenderGroup = renderTileMap.TileMapComponent.Entity.Group;
                }
            }
        }

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