using System;
using System.Collections.Generic;
using SiliconStudio.Xenko.Graphics;
using SiliconStudio.Xenko.Rendering;
using TilemapTools.Xenko.Graphics;

namespace TilemapTools.Xenko.Rendering
{
    public class TileMapRenderFeature : RootRenderFeature
    {
        private TileMeshRenderer tileMeshRenderer;

        public override Type SupportedRenderObjectType => typeof(RenderTileMap);

        protected override void InitializeCore()
        {
            base.InitializeCore();
            tileMeshRenderer = new TileMeshRenderer(Context.GraphicsDevice);
        }

        public override void Prepare(RenderDrawContext context)
        {
            base.Prepare(context);
            
        }

        public override void Draw(RenderDrawContext context, RenderView renderView, RenderViewStage renderViewStage, int startIndex, int endIndex)
        {
            base.Draw(context, renderView, renderViewStage, startIndex, endIndex);

            var visibleBlocks = new List<TileGridBlock>();

            for (var index = startIndex; index < endIndex; index++)
            {
                var renderNodeReference = renderViewStage.SortedRenderNodes[index].RenderNode;
                var renderNode = GetRenderNode(renderNodeReference);
                var renderTileMap = (RenderTileMap)renderNode.RenderObject;

                var tileMapComp = renderTileMap.TileMapComponent;
                var transformComp = renderTileMap.TransformComponent;
                var tileMesh = renderTileMap.TileMesh;
                var grid = tileMapComp.Grid;

                if (grid == null || tileMesh == null)
                    continue;

                var world = renderTileMap.TransformComponent.WorldMatrix;
                var viewProjection = renderView.ViewProjection;

                grid.FindVisibleGridBlocks(ref world, ref viewProjection, visibleBlocks);

                tileMeshRenderer.Begin(context.GraphicsContext, world, viewProjection);
                for (int i = 0; i < visibleBlocks.Count; i++)
                {
                    var block = visibleBlocks[i];
                    TileMeshDraw tileMeshDraw;

                    if(tileMesh.TryGetTileMeshDraw(block, out tileMeshDraw))
                    {
                        tileMeshRenderer.Draw(tileMeshDraw);
                    }
                }
                tileMeshRenderer.End();

                visibleBlocks.Clear();
            }
        }
        
    }
}