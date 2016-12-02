using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconStudio.Core;
using SiliconStudio.Xenko.Graphics;

namespace TilemapTools.Xenko.Graphics
{
    public class TileMesh:IDisposable
    {
        private ITileMeshDrawBuilder tileMeshDrawBuilder;
        private readonly Dictionary<ShortPoint, TileMeshDraw> tileMeshDraws;

        public TileMesh(ITileMeshDrawBuilder tileMeshDrawBuilder)
        {
            this.tileMeshDrawBuilder = tileMeshDrawBuilder;
            tileMeshDraws = new Dictionary<ShortPoint, Graphics.TileMeshDraw>();
        }

        public void Dispose()
        {
            Utilities.Dispose(ref tileMeshDrawBuilder);
        }

        public bool TryGetTileMeshDraw(TileGridBlock block, GraphicsDevice graphicsDevice,out TileMeshDraw tileMeshDraw)
        {
            tileMeshDraw = null;

            if(tileMeshDraws.TryGetValue(block.Location, out tileMeshDraw))
            {
                return true;
            }


            var blockSize = block.BlockSize;
            for (int y = 0; y < blockSize; y++)
            {
                for (int x = 0; x < blockSize; x++)
                {
                    var tile = block.GetCell(x, y);

                    if (tile == null) continue;

                    //tileMeshDrawBuilder.Add(tile.)
                }
            }

            tileMeshDraws[block.Location] = tileMeshDraw = tileMeshDrawBuilder.Build(graphicsDevice);

            return false;
        }
    }
}
