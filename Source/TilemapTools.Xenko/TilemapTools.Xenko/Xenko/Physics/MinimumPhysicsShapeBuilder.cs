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
    [Display("Minimum Collider Shape")]
    [DataContract("MinimumPhysicsShapeBuilder")]
    public class MinimumPhysicsShapeBuilder : TileMapPhysicsShapeBuilder
    {
        private Dictionary<int, RectangleF> previousBounds = new Dictionary<int, RectangleF>();
        private Dictionary<int, RectangleF> currentBounds = new Dictionary<int, RectangleF>();

        protected override void Update(TileGridBlock block, ref Vector2 cellSize)
        {
            RectangleF originCellBounds = new RectangleF(block.Origin.X, block.Origin.Y, cellSize.X, cellSize.Y);
            var originX = originCellBounds.X;

            var cellBounds = originCellBounds;
            int left = 0;


            RectangleF adjacentCellBounds = default(RectangleF);
            bool hasAdjacent = false;

            for (int y = 0; y < block.BlockSize; y++)
            {
                for (int x = 0; x < block.BlockSize; x++)
                {
                    var tile = block.GetCell(x, y);

                    if (!tile.IsEmpty)
                    {
                        if (IsLeftEdge(block, ref x, ref y))
                        {
                            cellBounds.X = originCellBounds.X;
                            cellBounds.Width = originCellBounds.Width;
                            left = x;
                            if (hasAdjacent = previousBounds.TryGetValue(x, out adjacentCellBounds))
                                previousBounds.Remove(x);
                        }

                        if (IsRightEdge(block, ref x, ref y))
                        {
                            if (hasAdjacent)
                            {
                                if (adjacentCellBounds.Width == cellBounds.Width)
                                {
                                    adjacentCellBounds.Height += cellBounds.Height;
                                    currentBounds[left] = adjacentCellBounds;
                                }
                                else
                                {
                                    var colliderBounds = CalculateColliderBounds(ref adjacentCellBounds, ref cellSize);

                                    //TODO: Change this so works for other tile grid types
                                    var shape = new BoxColliderShapeDesc() { Is2D = true, Size = colliderBounds.Extent, LocalOffset = colliderBounds.Center };

                                    AddTileColliderShape(shape);

                                    currentBounds[left] = cellBounds;
                                }

                                hasAdjacent = false;
                            }
                            else
                            {
                                currentBounds[left] = cellBounds;
                            }

                        }
                        else
                        {
                            cellBounds.Width += cellSize.X;
                        }                        
                        
                    }
                    originCellBounds.X += cellSize.X;

                }
                //increment
                originCellBounds.Y -= cellSize.Y;

                // reset
                originCellBounds.X = originX;
                cellBounds = originCellBounds;
                left = 0;

                foreach (var item in previousBounds.Values)
                {
                    var leftOver = item;
                    var colliderBounds = CalculateColliderBounds(ref leftOver, ref cellSize);

                    //TODO: Change this so works for other tile grid types
                    var shape = new BoxColliderShapeDesc() { Is2D = true, Size = colliderBounds.Extent, LocalOffset = colliderBounds.Center };

                    AddTileColliderShape(shape);
                }

                Utilities.Swap(ref previousBounds, ref currentBounds);

                currentBounds.Clear();
            }

            foreach (var item in previousBounds.Values)
            {
                var leftOver = item;
                var colliderBounds = CalculateColliderBounds(ref leftOver, ref cellSize);

                //TODO: Change this so works for other tile grid types
                var shape = new BoxColliderShapeDesc() { Is2D = true, Size = colliderBounds.Extent, LocalOffset = colliderBounds.Center };

                AddTileColliderShape(shape);
            }

            previousBounds.Clear();

        }
        
    }
}
