using System;
using System.Collections.Generic;

namespace TilemapTools
{
    public class Grid<TTileDefinition>: IDisposable
        where TTileDefinition : class
    {
        private readonly GridBlockCollection<TTileDefinition> blocks = new GridBlockCollection<TTileDefinition>();

        public int BlockSize { get; set; } = GridBlock.DefaultBlockSize;

        public TTileDefinition this[int x, int y]
        {
            get
            {
                GridBlock<TTileDefinition> block = null;
                ShortPoint blockLocation = default(ShortPoint);
                int tileX, tileY;
                int blockSsize = BlockSize;

                GridBlock.GetTileLocation(ref x, ref y, ref blockSsize, out blockLocation, out tileX, out tileY);

                if (blocks.TryGetItem(blockLocation, out block))
                {
                    return block[tileX, tileY];
                }

                return null;
            }

            set
            {
                GridBlock<TTileDefinition> block = null;
                ShortPoint blockLocation = default(ShortPoint);
                int tileX, tileY;
                int blockSsize = BlockSize;

                GridBlock.GetTileLocation(ref x, ref y, ref blockSsize, out blockLocation, out tileX, out tileY);

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

        protected virtual GridBlock<TTileDefinition> CreateBlock(ShortPoint blockLocation)
        {
            GridBlock<TTileDefinition> block = new GridBlock<TTileDefinition>(BlockSize, blockLocation, this);
            return block;
        }

        public virtual void Dispose()
        {
            foreach (var block in blocks)
            {
                block.Dispose();                
            }

            blocks.Clear(); 
        }
    }
}