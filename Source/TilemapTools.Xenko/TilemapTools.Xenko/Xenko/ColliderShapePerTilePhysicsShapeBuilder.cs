﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconStudio.Core;
using SiliconStudio.Core.Mathematics;
using SiliconStudio.Xenko.Physics;

namespace TilemapTools.Xenko
{
    [Display("Collider Shape Per Tile")]
    [DataContract("ColliderShapePerTilePhysicsShapeBuilder")]
    public class ColliderShapePerTilePhysicsShapeBuilder : TileMapPhysicsShapeBuilder
    {
        protected override void Update(TileGridBlock block, ref Vector2 cellSize)
        {
            var origin = new Vector3( block.Origin + cellSize / 2f, 0);
            origin.Y -= cellSize.Y; //TODO: Find out why this is meeded. COuld be a bug in the calculation of TileGridBlock.Origin

            var originX = origin.X;

            var size = new Vector3(cellSize, 0);
            for (int y = 0; y < block.BlockSize; y++)
            {
                for (int x = 0; x < block.BlockSize; x++)
                {
                    var tile = block.GetCell(x, y);

                    if(!tile.IsEmpty)
                    {
                        //TODO: Change this so works for other tile grid types
                        var shape = new BoxColliderShapeDesc() { Is2D = true, Size = size, LocalOffset = origin };
                        AddTileColliderShape(shape);
                    }

                    origin.X += cellSize.X;                
                }
                origin.Y -= cellSize.Y;

                origin.X = originX;
            }
        }
    }
}