using System;

namespace TilemapTools
{
    public class Grid<TCell, TCellSize> : IDisposable, IGrid<TCell, TCellSize> 
        where TCell : class
        where TCellSize : struct, IEquatable<TCellSize>
    {
        private GridBlockCollection<IGridBlock<TCell, TCellSize>> blocks;

        private int blockSize;
        private TCellSize cellSize;

        public Grid()
        {
            blocks = new GridBlockCollection<IGridBlock<TCell, TCellSize>>();
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

                if (blocks.TryGetItem(blockLocation, out block))
                {
                    return block[cellX, cellY];
                }

                return null;
            }

            set
            {
                CheckLocation(ref x, ref y);

                IGridBlock<TCell, TCellSize> block = null;
                ShortPoint blockLocation = default(ShortPoint);
                int tileX, tileY;
                int blockSize = BlockSize;

                GridBlock.GetBlockCellLocation(ref x, ref y, ref blockSize, out blockLocation, out tileX, out tileY);

                if (!blocks.TryGetItem(blockLocation, out block))
                {
                    if (value == null)
                    {
                        return;
                    }
                    block = CreateBlock(blockLocation);
                    blocks.Add(block);
                }

                block[tileX, tileY] = value;
            }
        }

        private void CheckLocation(ref int x, ref int y)
        {
            if (x == 0) throw new ArgumentOutOfRangeException(nameof(x));
            if (y == 0) throw new ArgumentOutOfRangeException(nameof(y));
        }

        public virtual void Dispose()
        {
            foreach (var block in blocks)
            {
                block.Dispose();
            }

            blocks.Clear();
        }

        protected virtual IGridBlock<TCell, TCellSize> CreateBlock(ShortPoint blockLocation)
        {
            return new GridBlock<TCell, TCellSize>(BlockSize, blockLocation, this);
        }



        protected virtual void OnBlockSizeChanged()
        {
            ResizeBlocks();
        }

        protected void ResizeBlocks()
        {
            if (this.blocks.Count == 0)
                return;

            var tempGrid = CloneEmptyGrid();

            foreach (var block in blocks)
            {
                foreach (var cell in block)
                {
                    tempGrid[cell.X, cell.Y] = cell.Content;
                }
            }

            this.blocks = tempGrid.blocks;
        }

        protected virtual Grid<TCell, TCellSize> CloneEmptyGrid()
        {
            var tempGrid = new Grid<TCell, TCellSize>();
            tempGrid.cellSize = this.cellSize;
            tempGrid.blockSize = this.blockSize;

            return tempGrid;
        }

        protected virtual void OnCellSizeChanged()
        {

        }
    }
}