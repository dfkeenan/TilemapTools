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
    [Display("Collider Shape Per Edge")]
    [DataContract("PerEdgePhysicsShapeBuilder")]
    public class PerEdgePhysicsShapeBuilder : PhysicsShapeBuilder
    {
        private Dictionary<int, Rectangle> previousBounds = new Dictionary<int, Rectangle>();
        private Dictionary<int, Rectangle> currentBounds = new Dictionary<int, Rectangle>();

        protected override void Update(PhysicsShapeBuilderContext context, TileGridBlock block)
        {
            throw new NotImplementedException();
            //Rectangle cellSelection = new Rectangle(0, 0, 1, 1);
            //int left = 0;

            //Rectangle adjacent = default(Rectangle);
            //bool hasAdjacent = false;

            //for (int y = 0; y < block.BlockSize; y++)
            //{
            //    for (int x = 0; x < block.BlockSize; x++)
            //    {
            //        var tile = block.GetCell(x, y);

            //        if (!tile.IsEmpty)
            //        {
            //            if (IsLeftEdge(block, ref x, ref y))
            //            {
            //                cellSelection.X = left = x;
            //                cellSelection.Width = 1;
            //                if (hasAdjacent = previousBounds.TryGetValue(x, out adjacent))
            //                    previousBounds.Remove(x);
            //            }

            //            if (IsRightEdge(block, ref x, ref y))
            //            {
            //                if (hasAdjacent)
            //                {
            //                    if (adjacent.Width == cellSelection.Width)
            //                    {
            //                        adjacent.Height += 1;
            //                        currentBounds[left] = adjacent;
            //                    }
            //                    else
            //                    {
            //                        context.AddColliderShape(context.ColliderShapeProvider.CalculateColliderShape(ref adjacent, block));
            //                        currentBounds[left] = cellSelection;
            //                    }

            //                    hasAdjacent = false;
            //                }
            //                else
            //                {
            //                    currentBounds[left] = cellSelection;
            //                }

            //            }
            //            else
            //            {
            //                cellSelection.Width += 1;
            //            }

            //        }

            //    }
            //    cellSelection.Y += 1;
            //    cellSelection.X = left = 0;
            //    cellSelection.Width = 1;

            //    foreach (var item in previousBounds.Values)
            //    {
            //        var leftOver = item;
            //        context.AddColliderShape(context.ColliderShapeProvider.CalculateColliderShape(ref leftOver, block));
            //    }

            //    Utilities.Swap(ref previousBounds, ref currentBounds);

            //    currentBounds.Clear();
            //}

            //foreach (var item in previousBounds.Values)
            //{
            //    var leftOver = item;
            //    context.AddColliderShape(context.ColliderShapeProvider.CalculateColliderShape(ref leftOver, block));
            //}

            //previousBounds.Clear();

        }


    }
}
