using System;
using System.Collections.Generic;
using SiliconStudio.Core;
using SiliconStudio.Xenko.Graphics;
using SiliconStudio.Core.Mathematics;

namespace TilemapTools.Xenko.Graphics
{
    public class TileMesh:IDisposable
    {
        private ITileMeshDrawBuilder tileMeshDrawBuilder;
        private readonly ITileDefinitionSource tileDefinitionSource;
        private Dictionary<ShortPoint, TileMeshDraw> tileMeshDraws;
        private Dictionary<ShortPoint, TileMeshDraw> tileMeshDrawsSwap;

        public TileMesh(ITileMeshDrawBuilder tileMeshDrawBuilder, ITileDefinitionSource tileDefinitionSource)
        {
            this.tileMeshDrawBuilder = tileMeshDrawBuilder;
            tileMeshDraws = new Dictionary<ShortPoint, Graphics.TileMeshDraw>();
            tileMeshDrawsSwap = new Dictionary<ShortPoint, Graphics.TileMeshDraw>();
            this.tileDefinitionSource = tileDefinitionSource;
        }

        public void Dispose()
        {
            Utilities.Dispose(ref tileMeshDrawBuilder);
            foreach (var item in tileMeshDraws.Values)
            {
                item.Dispose();
            }
            tileMeshDraws.Clear();
        }

        public void GetTileMeshDraws(IList<TileGridBlock> blocks, GraphicsDevice graphicsDevice, ref Vector2 cellSize, IList<TileMeshDraw> tileMeshDrawsOut)
        {
            if (blocks == null)
                throw new ArgumentNullException(nameof(blocks));

            if (graphicsDevice == null)
                throw new ArgumentNullException(nameof(graphicsDevice));

            if (tileMeshDrawsOut == null)
                throw new ArgumentNullException(nameof(tileMeshDrawsOut));


            if (tileMeshDrawBuilder == null) return;

            for (int i = 0; i < blocks.Count; i++)
            {
                var currentBlock = blocks[i];
                TileMeshDraw tileMeshDraw = null;

                if(currentBlock.VisualyInvalidated || !tileMeshDraws.TryGetValue(currentBlock.Location, out tileMeshDraw))
                {
                    tileMeshDraw = BuildTileMeshDraw(currentBlock, graphicsDevice, ref cellSize);
                }

                tileMeshDrawsSwap[currentBlock.Location] = tileMeshDraw;
                tileMeshDrawsOut.Add(tileMeshDraw);
                currentBlock.VisualyInvalidated = false;
            }

            Utilities.Swap(ref tileMeshDraws, ref tileMeshDrawsSwap);
            tileMeshDrawsSwap.Clear();

        }

        private TileMeshDraw BuildTileMeshDraw(TileGridBlock block, GraphicsDevice graphicsDevice, ref Vector2 cellSize)
        {
            //TODO: might require changes for non-orthogonal maps
            var blockSize = block.BlockSize;
            RectangleF outRect = new RectangleF();
            outRect.X = block.Origin.X;
            outRect.Y = block.Origin.Y;
            outRect.Width = cellSize.X;
            outRect.Height = cellSize.Y;
            for (int y = 0; y < blockSize; y++)
            {
                for (int x = 0; x < blockSize; x++)
                {
                    var tileRef = block.GetCell(x, y);

                    if (!tileRef.IsEmpty)
                    {
                        var tile = tileDefinitionSource.GetTile(ref tileRef);

                        if (tile != null && tile.CanCacheTileMesh)
                        {
                            var frame = tile[0];

                            tileMeshDrawBuilder.Add(frame.Texture, ref frame.TextureRegion, ref outRect);
                        }
                    }
                    outRect.X += cellSize.X;
                }
                outRect.X = block.Origin.X;
                outRect.Y -= cellSize.Y;
            }

            return tileMeshDrawBuilder.Build(graphicsDevice);
        }
    }
}
