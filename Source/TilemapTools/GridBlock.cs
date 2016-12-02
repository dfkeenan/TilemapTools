using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using static System.Math;

namespace TilemapTools
{
    public class GridBlock<TCell> : IGridBlock<TCell>, IEnumerable<CellLocationPair<TCell>>
    {
        private readonly IEqualityComparer<TCell> cellEqualityComparer;
        private int cellCount = 0;
        private TCell[] cells;

        public GridBlock(int blockSize, ShortPoint location, IEqualityComparer<TCell> cellEqualityComparer)
        {
            if (cellEqualityComparer == null)
                throw new ArgumentNullException(nameof(cellEqualityComparer));

            BlockSize = blockSize;
            Location = location;

            cells = new TCell[BlockSize * BlockSize];
            this.cellEqualityComparer = cellEqualityComparer;
        }

        public int BlockSize { get; }

        public bool IsEmpty => cellCount == 0;

        public ShortPoint Location { get; }

        public int CellCount => cellCount;

        public TCell GetCell(int x, int y)
        {
            return cells[y * BlockSize + x];
        }

        public bool SetCell(int x, int y, TCell value)
        {
            bool changed = false;

            var index = y * BlockSize + x;

            if (!cellEqualityComparer.Equals(cells[index], value))
            {
                if (cellEqualityComparer.Equals(value, default(TCell)))
                    cellCount -= 1;
                else if (cellEqualityComparer.Equals(cells[index], default(TCell)))
                    cellCount += 1;

                changed = true;
                OnCellContentChanged(x, y);
            }

            cells[index] = value;
            return changed;
        }

        protected virtual void OnCellContentChanged(int x, int y)
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
            private GridBlock<TCell> gridBlock;
            private CellLocationPair<TCell> current;

            public Enumerator(GridBlock<TCell> gridBlock)
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

                    index++;

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

    public static class GridBlock
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
        public static void GetBlockCellLocation(ref int x, ref int y, ref int blockSize, out ShortPoint blockLocation, out int cellX, out int cellY)
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
        public static void GetCellLocation(ref int index, ref int blockSize, ref ShortPoint blockLocation, out int x, out int y)
        {
            int localX = index % blockSize;
            int localY = index / blockSize;

            var leftCellX = blockLocation.X * blockSize;

            if (blockLocation.X > 0)
                leftCellX = leftCellX - blockSize + 1;

            x = leftCellX + localX;

            var topCellY = blockLocation.Y * blockSize;

            if (blockLocation.Y < 0)
                topCellY = topCellY + blockSize - 1;

            y = topCellY - localY;
            
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CalculateEdges(ref int blockSize, ref ShortPoint location, out int left, out int top, out int right, out int bottom)
        {
            if (location.X < 0)
            {
                left = location.X * blockSize;
            }
            else
            {
                left = (location.X - 1) * blockSize;
            }
            right = left + blockSize;


            if (location.Y < 0)
            {
                top = (location.Y + 1) * blockSize;
            }
            else
            {
                top = location.Y * blockSize;
            }
            bottom = top - blockSize;
        }
    }
}