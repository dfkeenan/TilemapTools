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
    [Display("Collider Shape Per Tile")]
    [DataContract("PerTilePhysicsShapeBuilder")]
    public class PerTilePhysicsShapeBuilder : TileMapPhysicsShapeBuilder
    {
        protected override void Update(TileGridBlock block, ref Vector2 cellSize)
        {
            RectangleF cellBounds = new RectangleF(block.Origin.X,block.Origin.Y, cellSize.X,cellSize.Y);
            var originX = cellBounds.X;

            for (int y = 0; y < block.BlockSize; y++)
            {
                for (int x = 0; x < block.BlockSize; x++)
                {
                    var tile = block.GetCell(x, y);

                    if(!tile.IsEmpty)
                    {
                        var colliderBounds = CalculateColliderBounds(ref cellBounds, ref cellSize);

                        //TODO: Change this so works for other tile grid types
                        var shape = new BoxColliderShapeDesc() { Is2D = true, Size = colliderBounds.Extent, LocalOffset = colliderBounds.Center };
                        AddTileColliderShape(shape);
                    }

                    cellBounds.X += cellSize.X;                
                }
                cellBounds.Y -= cellSize.Y;

                cellBounds.X = originX;
            }
        }
        
    }
}
