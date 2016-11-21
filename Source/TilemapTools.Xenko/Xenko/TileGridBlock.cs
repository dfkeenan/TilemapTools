using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconStudio.Core.Mathematics;

namespace TilemapTools.Xenko
{
    public abstract class TileGridBlock : GridBlock<Tile, Vector2>
    {
        public TileGridBlock(int blockSize, ShortPoint location, IEqualityComparer<Tile> cellEqualityComparer) : base(blockSize, location, cellEqualityComparer)
        {

        }
    }
}
