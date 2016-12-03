using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconStudio.Core;
using SiliconStudio.Xenko.Graphics;
using SiliconStudio.Core.Mathematics;

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

            if (tileMeshDrawBuilder == null) return false;

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

                    if (tile == null || !tile.CanCacheTileMesh) continue;

                    var frame = tile[0];

                    RectangleF outRect = new RectangleF();

                    tileMeshDrawBuilder.Add(frame.Texture, frame.TextureRegion, outRect);
                }
            }

            tileMeshDraws[block.Location] = tileMeshDraw = tileMeshDrawBuilder.Build(graphicsDevice);

            return false;
        }
    }
}
