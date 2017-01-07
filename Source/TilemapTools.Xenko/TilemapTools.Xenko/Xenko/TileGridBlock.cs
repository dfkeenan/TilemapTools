using System.Collections.Generic;
using SiliconStudio.Core.Mathematics;

namespace TilemapTools.Xenko
{
    public class TileGridBlock : GridBlock<TileReference>
    {
        public TileGridBlock(int blockSize, ShortPoint location, IEqualityComparer<TileReference> cellEqualityComparer) : base(blockSize, location, cellEqualityComparer)
        {
            
        }

        /// <summary>
        /// The bounds of the block in grid local space
        /// </summary>
        internal BoundingBoxExt LocalBounds;

        /// <summary>
        /// The top, left corner of the block
        /// </summary>
        internal Vector2 Origin;

                
    }
}
