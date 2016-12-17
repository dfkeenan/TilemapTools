using System;
using SiliconStudio.Core.Mathematics;
using SiliconStudio.Xenko.Engine;
using SiliconStudio.Xenko.Rendering;
using TilemapTools.Xenko.Graphics;

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
                    if(renderTileMap.TileMesh == null)
                    {
                        renderTileMap.TileMesh = new TileMesh(renderTileMap.Grid.CreateMeshDrawBuilder(), renderTileMap.Grid as ITileDefinitionSource);
                    }


                    // TODO GRAPHICS REFACTOR: Proper bounding box.
                    // For now we only set a center for sorting, but no extent (which disable culling)
                    var world = renderTileMap.TransformComponent.WorldMatrix;
                    renderTileMap.BoundingBox = new BoundingBoxExt { Center = world.TranslationVector };
                    renderTileMap.RenderGroup = renderTileMap.TileMapComponent.Entity.Group;
                }
            }
        }

        protected override RenderTileMap GenerateComponentData(Entity entity, TileMapComponent component)
        {
            return new RenderTileMap
            {
                TileMapComponent = component,
                TransformComponent = entity.Transform,
                Grid = component.Grid,
            };
        }

        protected override bool IsAssociatedDataValid(Entity entity, TileMapComponent component, RenderTileMap associatedData)
        {
            return
                component == associatedData.TileMapComponent &&
                component.Grid == associatedData.Grid &&
                entity.Transform == associatedData.TransformComponent;
        }

        protected override void OnEntityComponentAdding(Entity entity, TileMapComponent component, RenderTileMap data)
        {
            VisibilityGroup.RenderObjects.Add(data);
        }

        protected override void OnEntityComponentRemoved(Entity entity, TileMapComponent component, RenderTileMap data)
        {
            data.TileMesh?.Dispose();
            data.TileMesh = null;
            VisibilityGroup.RenderObjects.Remove(data);
        }
    }
}