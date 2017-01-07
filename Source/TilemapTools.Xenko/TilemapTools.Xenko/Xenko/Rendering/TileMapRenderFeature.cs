using System;
using System.Collections.Generic;
using SiliconStudio.Xenko.Rendering;
using TilemapTools.Xenko.Graphics;

namespace TilemapTools.Xenko.Rendering
{
    public class TileMapRenderFeature : RootRenderFeature
    {
        private TileMeshRenderer tileMeshRenderer;
        private List<TileGridBlock> visibleBlocks = new List<TileGridBlock>();
        private List<TileMeshDraw> visibleTileMeshDraws = new List<TileMeshDraw>();


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
                var cellSize = grid.CellSize;

                grid.FindVisibleGridBlocks(ref world, ref viewProjection, visibleBlocks);
                if (visibleBlocks.Count > 0)
                {
                    tileMeshRenderer.Begin(context.GraphicsContext, world, viewProjection);

                    tileMesh.GetTileMeshDraws(visibleBlocks, context.GraphicsDevice, ref cellSize, visibleTileMeshDraws);

                    for (int i = 0; i < visibleTileMeshDraws.Count; i++)
                    {
                        var tileMeshDraw = visibleTileMeshDraws[i];
                        tileMeshRenderer.Draw(tileMeshDraw);                        
                    }
                    tileMeshRenderer.End();

                    visibleBlocks.Clear();
                    visibleTileMeshDraws.Clear();
                }
            }
        }
        protected override void Destroy()
        {
            base.Destroy();
            visibleBlocks.Clear();
        }

    }
}