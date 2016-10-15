using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TilemapTools
{
    public class Grid
    {
        private readonly Dictionary<GridBlockKey, GridBlock> blocks = new Dictionary<GridBlockKey, GridBlock>();
        
        public TileDefinition this[int x, int y]
        {
            get
            {
                GridBlock block = null;
                GridBlockKey blockKey = default(GridBlockKey);
                int tileX, tileY;

                GridBlock.GetTileLocation(ref x, ref y, out blockKey, out tileX, out tileY);

                if(blocks.TryGetValue(blockKey, out block))
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

                GridBlock.GetTileLocation(ref x, ref y, out blockKey, out tileX, out tileY);

                if (!blocks.TryGetValue(blockKey, out block))
                {
                    if (value == null)
                    {
                        return;
                    }
                    blocks[blockKey] = block = new GridBlock();
                }                

                block[tileX, tileY] = value;
            }
        }

        
    }
}
