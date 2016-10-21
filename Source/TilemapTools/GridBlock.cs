using System;
using System.Runtime.CompilerServices;

namespace TilemapTools
{
    public class GridBlock
    {
        public const int DefaultBlockSize = 16;

        public const int MinimumBlockSize = 1;

        private int tileCount = 0;

        private TileDefinition[] tiles;

        public GridBlock(int blockSize, GridBlockKey blockKey, Grid grid)
        {
            Grid = grid;
            BlockSize = blockSize;
            Key = blockKey;
            tiles = new TileDefinition[BlockSize * BlockSize];
        }

        public int BlockSize { get; }

        public Grid Grid { get; }

        public bool IsEmpty => tileCount == 0;

        public GridBlockKey Key { get; }

        public TileDefinition this[int x, int y]
        {
            get
            {
                return tiles[y * BlockSize + x];
            }

            set
            {
                var index = y * BlockSize + x;

                if (tiles[index] != value)
                {
                    if (value == null)
                        tileCount -= 1;
                    else if (tiles[index] == null)
                        tileCount += 1;
                }

                tiles[index] = value;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void GetTileLocation(ref int x, ref int y, ref int blockSize, out GridBlockKey blockKey, out int tileX, out int tileY)
        {
            //Block layout
            //+-------+-------+
            //| -1,1  |  1,1  |
            //+-------+-------+
            //| -1,-1 | 1,-1  |
            //+-------+-------+
            var blockKeyX = (short)((x / blockSize) + Math.Sign(x));
            var blockKeyY = (short)((y / blockSize) + Math.Sign(y));

            blockKey = new GridBlockKey(blockKeyX, blockKeyY);

            tileX = blockSize - (x % blockSize);
            tileY = blockSize - (y % blockSize);
        }
    }
}