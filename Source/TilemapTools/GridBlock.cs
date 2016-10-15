using System;
using System.Runtime.CompilerServices;

namespace TilemapTools
{
    public class GridBlock
    {
        internal const int BlockSize = 16;

        private TileDefinition[] tiles;

        public GridBlock()
        {
            tiles = new TileDefinition[BlockSize * BlockSize];
        }

        public TileDefinition this[int x, int y]
        {
            get
            {
                return tiles[y * BlockSize + x];
            }

            set
            {
                tiles[y * BlockSize + x] = value;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void GetTileLocation(ref int x, ref int y, out GridBlockKey blockKey, out int tileX, out int tileY)
        {
            //Block layout
            //+-------+-------+
            //| -1,1  |  1,1  |
            //+-------+-------+
            //| -1,-1 | 1,-1  |
            //+-------+-------+
            var blockKeyX = (short)((x / BlockSize) + Math.Sign(x));
            var blockKeyY = (short)((y / BlockSize) + Math.Sign(y));

            blockKey = new GridBlockKey(blockKeyX, blockKeyY);

            tileX = BlockSize - (x % BlockSize);
            tileY = BlockSize - (y % BlockSize);
        }
    }
}