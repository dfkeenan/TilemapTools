using System;
using System.Runtime.CompilerServices;

namespace TilemapTools
{
    public class GridBlock<TTileDefinition> : IDisposable
        where TTileDefinition : class
    {

        private int tileCount = 0;

        private TTileDefinition[] tiles;

        public GridBlock(int blockSize, ShortPoint blockKey, Grid<TTileDefinition> grid)
        {
            Grid = grid;
            BlockSize = blockSize;
            Key = blockKey;
            tiles = new TTileDefinition[BlockSize * BlockSize];
        }

        public int BlockSize { get; }

        public Grid<TTileDefinition> Grid { get; }

        public bool IsEmpty => tileCount == 0;

        public ShortPoint Key { get; }

        public TTileDefinition this[int x, int y]
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



        public virtual void Dispose()
        {
            
        }
    }

    public class GridBlock
    {
        public const int DefaultBlockSize = 16;

        public const int MinimumBlockSize = 1;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void GetTileLocation(ref int x, ref int y, ref int blockSize, out ShortPoint blockKey, out int tileX, out int tileY)
        {
            //Block/Tile layout
            //+-------+-------+
            //| -1,1  |  1,1  |
            //+-------+-------+
            //| -1,-1 | 1,-1  |
            //+-------+-------+
            var blockKeyX = (short)((x / blockSize) + Math.Sign(x));
            var blockKeyY = (short)((y / blockSize) + Math.Sign(y));

            blockKey = new ShortPoint(blockKeyX, blockKeyY);

            tileX = blockSize - (x % blockSize);
            tileY = blockSize - (y % blockSize);
        }
    }
}