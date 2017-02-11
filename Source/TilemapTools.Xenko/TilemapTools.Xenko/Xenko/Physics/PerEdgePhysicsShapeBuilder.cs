using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconStudio.Core;
using SiliconStudio.Core.Mathematics;
using SiliconStudio.Xenko.Physics;

namespace TilemapTools.Xenko.Physics
{
    //[Display("Collider Shape Per Edge")]
    //[DataContract("PerEdgePhysicsShapeBuilder")]
    //public class PerEdgePhysicsShapeBuilder : TileMapPhysicsShapeBuilder
    //{
    //    protected override void Update(TileGridBlock block, ref Vector2 cellSize)
    //    {
    //        var verticals = new Dictionary<int, RectangleF>();

    //        RectangleF originCellBounds = new RectangleF(block.Origin.X, block.Origin.Y, cellSize.X, cellSize.Y);
    //        var originX = originCellBounds.X;

    //        var horizontalEdgeBounds = originCellBounds;

    //        for (int y = 0; y < block.BlockSize; y++)
    //        {
    //            var below = y + 1;

    //            for (int x = 0; x < block.BlockSize; x++)
    //            {
    //                var tile = block.GetCell(x, y);

    //                if (!tile.IsEmpty)
    //                {
    //                    if (IsHorizontalEdge(block, ref x, ref y))
    //                    {
    //                        if(IsLeftEdge(block, ref x, ref y))
    //                        {
    //                            horizontalEdgeBounds.X = originCellBounds.X;
    //                        }

    //                        if (IsRightEdge(block, ref x, ref y))
    //                        {
    //                            var colliderBounds = CalculateColliderBounds(ref horizontalEdgeBounds, ref cellSize);

    //                            //TODO: Change this so works for other tile grid types
    //                            var shape = new BoxColliderShapeDesc() { Is2D = true, Size = colliderBounds.Extent, LocalOffset = colliderBounds.Center };

    //                            AddTileColliderShape(shape);
    //                            horizontalEdgeBounds.X = originCellBounds.X;
    //                            horizontalEdgeBounds.Width = originCellBounds.Width;
    //                        }
    //                        else
    //                        {
    //                            horizontalEdgeBounds.Width += cellSize.X;
    //                        }                            
    //                    }
    //                    else if (IsVerticalEdge(block, ref x, ref y))
    //                    {                           

    //                        RectangleF verticalEdgeBounds;

    //                        if (verticals.TryGetValue(x, out verticalEdgeBounds))
    //                        {
    //                            verticalEdgeBounds.Height += cellSize.Y;
    //                        }
    //                        else
    //                        {
    //                            verticalEdgeBounds = originCellBounds;

    //                        }                             

    //                        if (IsBottomEdge(block, ref x, ref below))
    //                        {
    //                            var colliderBounds = CalculateColliderBounds(ref verticalEdgeBounds, ref cellSize);

    //                            //TODO: Change this so works for other tile grid types
    //                            var shape = new BoxColliderShapeDesc() { Is2D = true, Size = colliderBounds.Extent, LocalOffset = colliderBounds.Center };

    //                            AddTileColliderShape(shape);

    //                            verticals.Remove(x);
    //                        }
    //                        else
    //                        {

    //                            verticals[x] = verticalEdgeBounds;
    //                        }
                            
    //                    }
    //                }

    //                originCellBounds.X += cellSize.X;
    //            }
    //            //increment
    //            originCellBounds.Y -= cellSize.Y;
                
    //            // reset
    //            originCellBounds.X = originX;
    //            horizontalEdgeBounds = originCellBounds;
    //        }
    //    }

        
    //}
}
