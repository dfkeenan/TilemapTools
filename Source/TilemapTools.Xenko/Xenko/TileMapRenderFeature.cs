using System;
using SiliconStudio.Xenko.Rendering;

namespace TilemapTools.Xenko
{
    public class TileMapRenderFeature : RootRenderFeature
    {
        public override Type SupportedRenderObjectType => typeof(RenderTileMap);

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

                var renderSprite = (RenderTileMap)renderNode.RenderObject;

                var tileMapComp = renderSprite.TileMapComponent;
                var transfoComp = renderSprite.TransformComponent;

                var grid = tileMapComp.Grid;
                if (grid == null)
                    continue;

                var world = renderSprite.TransformComponent.WorldMatrix;
                var viewProjection = renderView.ViewProjection;


            }
        }
    }
}