using System;
using SiliconStudio.Core;
using SiliconStudio.Core.Mathematics;
using TilemapTools.Xenko.Graphics;

namespace TilemapTools.Xenko
{
    [Display("Orthogonal")]
    [DataContract("OrthogonalTileGrid")]
    public class OrthogonalTileGrid : TileGrid
    {
        public override ITileMeshDrawBuilder CreateMeshDrawBuilder()
        {
            return new OrthogonalTileMeshDrawBuilder();
        }

        public override Point? GetCellLocation(ref Vector2 point)
        {
            var cell = point / CellSize;

            var x = (int)Math.Ceiling(cell.X < 0f ? cell.X - 1f : cell.X);

            if (x == 0)
                x = Math.Sign(cell.X);

            var y = (int)Math.Ceiling(cell.Y < 0f ? cell.Y - 1f : cell.Y);

            if (y == 0)
                y = Math.Sign(cell.Y);

            if (x != 0 && y != 0)
            {
               return new Point(x,y);
            }

            return null;
        }

    }
}
