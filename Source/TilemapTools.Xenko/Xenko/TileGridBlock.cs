using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using SiliconStudio.Core.Mathematics;

namespace TilemapTools.Xenko
{
    public class TileGridBlock : GridBlock<Tile>
    {
        public TileGridBlock(int blockSize, ShortPoint location, IEqualityComparer<Tile> cellEqualityComparer) : base(blockSize, location, cellEqualityComparer)
        {
            
        }

        public BoundingBoxExt LocalBounds { get; internal set; }
                
    }
}
