using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconStudio.Core.Mathematics;

namespace TilemapTools.Xenko
{
    public class TileGrid: Grid<Tile,Vector2>
    {
        protected override IGridBlock<Tile, Vector2> CreateBlock(ShortPoint blockLocation)
        {
            return new TileGridBlock(BlockSize, blockLocation, CellEqualityComparer);
        }
    }
}
