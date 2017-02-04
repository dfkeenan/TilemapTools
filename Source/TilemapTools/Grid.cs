using System;
using System.Collections;
using System.Collections.Generic;

namespace TilemapTools
{
    public abstract class Grid<TCell, TCellSize,TGridBlock> : IGrid<TCell, TCellSize>, IEnumerable<CellLocationPair<TCell>>
        where TCellSize : struct, IEquatable<TCellSize>
        where TGridBlock : class, IGridBlock<TCell>
    {
        private int blockSize;
        private TCellSize cellSize;

        public Grid(IEqualityComparer<TCell> cellEqualityComparer = null)
        {
            CellEqualityComparer = cellEqualityComparer ?? EqualityComparer<TCell>.Default;

            Blocks = new GridBlockCollection<TGridBlock>();
            blockSize = GridBlock.DefaultBlockSize;
        }

        public virtual int BlockSize
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

        protected GridBlockCollection<TGridBlock> Blocks { get; private set; }

        public TCell this[int x, int y]
        {
            get
            {
                CheckLocation(ref x, ref y);

                TGridBlock block = null;
                ShortPoint blockLocation = default(ShortPoint);
                int cellX, cellY;
                int blockSize = BlockSize;

                GridBlock.GetBlockCellLocation(ref x, ref y, ref blockSize, out blockLocation, out cellX, out cellY);

                if (Blocks.TryGetItem(blockLocation, out block))
                {
                    return block.GetCell(cellX, cellY);
                }

                return default(TCell);
            }

            set
            {
                CheckLocation(ref x, ref y);

                TGridBlock block = null;
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
                    AddBlock(block);
                }

                var currentValue = block.GetCell(tileX, tileY);

                if (block.SetCell(tileX, tileY, value))
                    OnCellContentChanged(tileX, tileY, block, currentValue, value);
            }
        }

        public void ClearCell(int x, int y) => this[x, y] = default(TCell);

        /// <summary>
        /// Removes Empty grid blocks/>
        /// </summary>
        public void TrimExcess()
        {
            var remove = new HashSet<TGridBlock>();
            for (int i = 0; i < Blocks.Count; i++)
            {
                remove.Add(Blocks[i]);
                OnBlockRemoved(Blocks[i]);
            }

            Blocks.RemoveAll(remove.Contains);
        }

        protected virtual void AddBlock(TGridBlock block)
        {
            Blocks.Add(block);
            OnBlockAdded(block);
        }

        protected virtual void OnBlockAdded(TGridBlock block)
        {
            
        }

        protected virtual void OnBlockRemoved(TGridBlock block)
        {

        }

        protected abstract TGridBlock CreateBlock(ShortPoint blockLocation);
        

        protected virtual void OnBlockSizeChanged()
        {
            ResizeBlocks();
        }

        protected virtual void OnCellSizeChanged()
        {
            
        }

        protected virtual void OnCellContentChanged(int x, int y, TGridBlock block, TCell oldValue, TCell newValue)
        {

        }

        protected void ResizeBlocks()
        {
            if (this.Blocks.Count == 0)
                return;

            var tempBlocks = Blocks;
            Blocks = new GridBlockCollection<TGridBlock>();

            for (int i = 0; i < tempBlocks.Count; i++)
            {
                OnBlockRemoved(tempBlocks[i]);
                foreach (var cell in (IEnumerable<CellLocationPair<TCell>>)tempBlocks[i])
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
                foreach (var cell in (IEnumerable<CellLocationPair<TCell>>)Blocks[i])
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

    public class Grid<TCell, TCellSize> : Grid<TCell, TCellSize, GridBlock<TCell>>
        where TCellSize : struct, IEquatable<TCellSize>
    {
        protected override GridBlock<TCell> CreateBlock(ShortPoint blockLocation)
        {
            return new GridBlock<TCell>(BlockSize, blockLocation, CellEqualityComparer);
        }
    }
}