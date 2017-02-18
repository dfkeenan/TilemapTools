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
    public class PerTilePhysicsShapeBuilder : PhysicsShapeBuilder
    {
        protected override void Update(PhysicsShapeBuilderContext context, TileGridBlock block)
        {
            Rectangle cellSelection = new Rectangle(0,0,1,1);

            for (int y = 0; y < block.BlockSize; y++)
            {
                for (int x = 0; x < block.BlockSize; x++)
                {
                    var tile = block.GetCell(x, y);

                    if(!tile.IsEmpty)
                    {
                        context.AddColliderShape(context.ColliderShapeProvider.CalculateColliderShape(ref cellSelection, block));
                    }

                    cellSelection.X += 1;                
                }
                cellSelection.Y += 1;

                cellSelection.X = 0;
            }
        }
        
    }
}
