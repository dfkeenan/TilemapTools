using System.Collections.Generic;

namespace TilemapTools
{
    public class Grid
    {
        private readonly Dictionary<GridBlockKey, GridBlock> blocks = new Dictionary<GridBlockKey, GridBlock>();

        public int BlockSize { get; set; } = GridBlock.DefaultBlockSize;

        public TileDefinition this[int x, int y]
        {
            get
            {
                GridBlock block = null;
                GridBlockKey blockKey = default(GridBlockKey);
                int tileX, tileY;
                int blockSsize = BlockSize;

                GridBlock.GetTileLocation(ref x, ref y, ref blockSsize, out blockKey, out tileX, out tileY);

                if (blocks.TryGetValue(blockKey, out block))
                {
                    return block[tileX, tileY];
                }

                return null;
            }

            set
            {
                GridBlock block = null;
                GridBlockKey blockKey = default(GridBlockKey);
                int tileX, tileY;
                int blockSsize = BlockSize;

                GridBlock.GetTileLocation(ref x, ref y, ref blockSsize, out blockKey, out tileX, out tileY);

                if (!blocks.TryGetValue(blockKey, out block))
                {
                    if (value == null)
                    {
                        return;
                    }
                    blocks[blockKey] = block = CreateBlock(blockKey);
                }

                block[tileX, tileY] = value;
            }
        }

        protected virtual GridBlock CreateBlock(GridBlockKey blockKey)
        {
            GridBlock block = new GridBlock(BlockSize, blockKey, this);
            return block;
        }
    }
}