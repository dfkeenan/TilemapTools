using System;
using System.Collections;
using System.Collections.Generic;

namespace TilemapTools
{
    public class Grid<TCell, TCellSize> : IDisposable, IGrid<TCell, TCellSize>, IEnumerable<CellLocationPair<TCell>>
        where TCellSize : struct, IEquatable<TCellSize>
    {
        private int blockSize;
        private TCellSize cellSize;

        public Grid(IEqualityComparer<TCell> cellEqualityComparer = null)
        {
            CellEqualityComparer = cellEqualityComparer ?? EqualityComparer<TCell>.Default;

            Blocks = new GridBlockCollection<IGridBlock<TCell, TCellSize>>();
            blockSize = GridBlock.DefaultBlockSize;
        }

        public int BlockSize
        {
            get { return blockSize; }
            set
            {
                if (blockSize == value)
                    return;

                if (value < GridBlock.MinimumBlockSize)
                    throw new ArgumentOutOfRangeException(nameof(value), $"{nameof(value)} must be greater than or equal to {GridBlock.MinimumBlockSize}.");

                blockSize = value;

                OnBlockSizeChanged();
            }
        }

        public virtual TCellSize CellSize
        {
            get { return cellSize; }
            set
            {
                if (cellSize.Equals(value))
                    return;
                cellSize = value;

                OnCellSizeChanged();
            }
        }

        protected IEqualityComparer<TCell> CellEqualityComparer { get; }

        protected GridBlockCollection<IGridBlock<TCell, TCellSize>> Blocks { get; private set; }

        public TCell this[int x, int y]
        {
            get
            {
                CheckLocation(ref x, ref y);

                IGridBlock<TCell, TCellSize> block = null;
                ShortPoint blockLocation = default(ShortPoint);
                int cellX, cellY;
                int blockSize = BlockSize;

                GridBlock.GetBlockCellLocation(ref x, ref y, ref blockSize, out blockLocation, out cellX, out cellY);

                if (Blocks.TryGetItem(blockLocation, out block))
                {
                    return block[cellX, cellY];
                }

                return default(TCell);
            }

            set
            {
                CheckLocation(ref x, ref y);

                IGridBlock<TCell, TCellSize> block = null;
                ShortPoint blockLocation = default(ShortPoint);
                int tileX, tileY;
                int blockSize = BlockSize;

                GridBlock.GetBlockCellLocation(ref x, ref y, ref blockSize, out blockLocation, out tileX, out tileY);

                if (!Blocks.TryGetItem(blockLocation, out block))
                {
                    if (CellEqualityComparer.Equals(value, default(TCell)))
                    {
                        return;
                    }
                    block = CreateBlock(blockLocation);
                    Blocks.Add(block);
                }

                block[tileX, tileY] = value;
            }
        }

        public void ClearCell(int x, int y) => this[x, y] = default(TCell);

        /// <summary>
        /// Removes Empty <see cref="GridBlock{TCell, TCellSize}"/>
        /// </summary>
        public void TrimExcess() => Blocks.RemoveAll(b => b.IsEmpty, true);

        public virtual void Dispose()
        {
            foreach (var block in Blocks)
            {
                block.Dispose();
            }

            Blocks.Clear();
        }

        protected virtual IGridBlock<TCell, TCellSize> CreateBlock(ShortPoint blockLocation)
        {
            return new GridBlock<TCell, TCellSize>(BlockSize, blockLocation, CellEqualityComparer);
        }

        protected virtual void OnBlockSizeChanged()
        {
            ResizeBlocks();
        }

        protected virtual void OnCellSizeChanged()
        {
            Blocks.ForEach(b => (b as GridBlock<TCell,TCellSize>)?.OnCellSizeChanged(this.cellSize));
        }

        protected void ResizeBlocks()
        {
            if (this.Blocks.Count == 0)
                return;

            var tempBlocks = Blocks;
            Blocks = new GridBlockCollection<IGridBlock<TCell, TCellSize>>();

            for (int i = 0; i < Blocks.Count; i++)
            {
                foreach (var cell in Blocks[i])
                {
                    this[cell.X, cell.Y] = cell.Content;
                }
            }
        }

        private void CheckLocation(ref int x, ref int y)
        {
            if (x == 0) throw new ArgumentOutOfRangeException(nameof(x));
            if (y == 0) throw new ArgumentOutOfRangeException(nameof(y));
        }

        IEnumerator<CellLocationPair<TCell>> IEnumerable<CellLocationPair<TCell>>.GetEnumerator()
        {
            for (int i = 0; i < Blocks.Count; i++)
            {
                foreach (var cell in Blocks[i])
                {
                    yield return cell;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<CellLocationPair<TCell>>)this).GetEnumerator();
        }
    }
}