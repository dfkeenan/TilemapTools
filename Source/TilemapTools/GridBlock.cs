using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using static System.Math;

namespace TilemapTools
{
    public class GridBlock<TCell, TCellSize> : IDisposable, IGridBlock<TCell, TCellSize>, IEnumerable<CellLocationPair<TCell>>
        where TCell : class
        where TCellSize : struct, IEquatable<TCellSize>
    {

        private int cellCount = 0;

        private TCell[] cells;

        public GridBlock(int blockSize, ShortPoint location, Grid<TCell, TCellSize> grid)
        {
            Grid = grid;
            BlockSize = blockSize;
            Location = location;
            cells = new TCell[BlockSize * BlockSize];
        }

        public int BlockSize { get; }

        public Grid<TCell, TCellSize> Grid { get; }

        public bool IsEmpty => cellCount == 0;

        public ShortPoint Location { get; }

        public int CellCount => cellCount;

        public TCell this[int x, int y]
        {
            get
            {
                return cells[y * BlockSize + x];
            }

            set
            {
                var index = y * BlockSize + x;

                if (cells[index] != value)
                {
                    if (value == null)
                        cellCount -= 1;
                    else if (cells[index] == null)
                        cellCount += 1;

                    OnCellContentChanged(x, y);
                }

                cells[index] = value;
            }
        }

        protected virtual void OnCellContentChanged(int x, int y)
        {

        }

        public virtual void Dispose()
        {
            
        }

        public IEnumerator<CellLocationPair<TCell>> GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }


        struct Enumerator : IEnumerator<CellLocationPair<TCell>>
        {
            private int index;
            private GridBlock<TCell,TCellSize> gridBlock;
            private CellLocationPair<TCell> current;

            public Enumerator(GridBlock<TCell, TCellSize> gridBlock)
            {
                index = 0;
                current = default(CellLocationPair<TCell>);
                this.gridBlock = gridBlock;
            }

            public CellLocationPair<TCell> Current => current;

            object IEnumerator.Current => Current;

            public void Dispose() { }

            public bool MoveNext()
            {
                var localGridBlock = gridBlock;
                var blockSize = localGridBlock.BlockSize;
                var blockLocation = localGridBlock.Location;

                if (index < localGridBlock.cells.Length)
                {
                    var currentCell = localGridBlock.cells[index];
                    int x, y;

                    GridBlock.GetCellLocation(ref index, ref blockSize, ref blockLocation, out x, out y);

                    current = new CellLocationPair<TCell>(currentCell, x, y);

                    return true;
                }

                return false;
            }

            public void Reset()
            {
                index = 0;
                current = default(CellLocationPair<TCell>);
            }
        }
    }

    public class GridBlock
    {
        public const int DefaultBlockSize = 16;

        public const int MinimumBlockSize = 1;

        //Block/Cell layout
        //+-------+-------+
        //| -1,1  |  1,1  |
        //+-------+-------+
        //| -1,-1 | 1,-1  |
        //+-------+-------+

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void GetBlockCellLocation(ref int x, ref int y, ref int blockSize, out ShortPoint blockLocation, out int cellX, out int cellY)
        {
            var signX = Sign(x);
            var signY = Sign(y);

            var zeroX = x - signX;
            var zeroY = y - signY;

            var blockKeyX = (short)(zeroX / blockSize + signX);
            var blockKeyY = (short)(zeroY / blockSize + signY);

            blockLocation = new ShortPoint(blockKeyX, blockKeyY);

            if (signX < 0)
                cellX = Math.Abs(blockSize + (zeroX % blockSize)) - 1;
            else
                cellX = Math.Abs(zeroX % blockSize);

            if (signY > 0)
                cellY = Math.Abs(blockSize - (zeroY % blockSize)) - 1;
            else
                cellY = Math.Abs(zeroY % blockSize);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void GetCellLocation(ref int index, ref int blockSize, ref ShortPoint blockLocation, out int x, out int y)
        {
            int localX = index / blockSize;
            int localY = index % blockSize;

            x = blockLocation.X * blockSize - localX;
            y = blockLocation.Y * blockSize - localY;

        }
    }
}