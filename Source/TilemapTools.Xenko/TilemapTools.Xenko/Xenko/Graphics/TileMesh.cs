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
        private Dictionary<ShortPoint, TileMeshDraw> previousTileMeshDraws;

        private readonly List<TileMeshDraw> tileMeshDrawsForRecycle;
        private readonly List<TileGridBlock> pendingBlocks;

        public TileMesh(ITileMeshDrawBuilder tileMeshDrawBuilder, ITileDefinitionSource tileDefinitionSource)
        {
            this.tileMeshDrawBuilder = tileMeshDrawBuilder;
            this.tileDefinitionSource = tileDefinitionSource;
            tileMeshDraws = new Dictionary<ShortPoint, TileMeshDraw>();
            previousTileMeshDraws = new Dictionary<ShortPoint, TileMeshDraw>();
            tileMeshDrawsForRecycle = new List<TileMeshDraw>();
            pendingBlocks = new List<TileGridBlock>();

        }

        public void Dispose()
        {
            Utilities.Dispose(ref tileMeshDrawBuilder);
            foreach (var item in tileMeshDraws.Values)
            {
                item.Dispose();
            }
            tileMeshDraws.Clear();

            foreach (var item in previousTileMeshDraws.Values)
            {
                item.Dispose();
            }
            previousTileMeshDraws.Clear();

            for (int i = 0; i < tileMeshDrawsForRecycle.Count; i++)
            {
                tileMeshDrawsForRecycle[i].Dispose();
            }

            tileMeshDrawsForRecycle.Clear();
        }

        public void GetTileMeshDraws(IList<TileGridBlock> blocks, GraphicsContext graphicsContext, ref Vector2 cellSize, IList<TileMeshDraw> tileMeshDrawsOut)
        {
            if (blocks == null)
                throw new ArgumentNullException(nameof(blocks));

            if (graphicsContext == null)
                throw new ArgumentNullException(nameof(graphicsContext));

            if (tileMeshDrawsOut == null)
                throw new ArgumentNullException(nameof(tileMeshDrawsOut));


            if (tileMeshDrawBuilder == null) return;



            for (int i = 0; i < blocks.Count; i++)
            {
                var currentBlock = blocks[i];
                TileMeshDraw tileMeshDraw = null;

                if(previousTileMeshDraws.TryGetValue(currentBlock.Location, out tileMeshDraw))
                {
                    if(currentBlock.VisualyInvalidated)
                    {
                        pendingBlocks.Add(currentBlock);
                        tileMeshDrawsForRecycle.Add(tileMeshDraw);
                    }
                    else
                    {
                        tileMeshDraws[currentBlock.Location] = tileMeshDraw;
                        tileMeshDrawsOut.Add(tileMeshDraw);
                    }

                    previousTileMeshDraws.Remove(currentBlock.Location);
                }
                else
                {
                    pendingBlocks.Add(currentBlock);
                }
                
            }
            
            if (pendingBlocks.Count > 0)
            {                

                for (int i = 0; i < pendingBlocks.Count; i++)
                {
                    var currentBlock = pendingBlocks[i];

                    TileMeshDraw tileMeshDraw = null;

                    if (tileMeshDrawsForRecycle.Count > 0)
                    {
                        var lastIndex = tileMeshDrawsForRecycle.Count - 1;
                        tileMeshDraw = tileMeshDrawsForRecycle[lastIndex];
                        tileMeshDrawsForRecycle.RemoveAt(lastIndex);

                        tileMeshDrawBuilder.Recycle(tileMeshDraw, currentBlock, tileDefinitionSource, graphicsContext, ref cellSize);
                    }
                    else
                    {
                        tileMeshDraw = tileMeshDrawBuilder.Build(currentBlock, tileDefinitionSource, graphicsContext, ref cellSize);
                    }

                    tileMeshDraws[currentBlock.Location] = tileMeshDraw;
                    currentBlock.VisualyInvalidated = false;
                    tileMeshDrawsOut.Add(tileMeshDraw);
                }

            }

            tileMeshDrawsForRecycle.AddRange(previousTileMeshDraws.Values);
            previousTileMeshDraws.Clear();
            pendingBlocks.Clear();                        
            Utilities.Swap(ref tileMeshDraws, ref previousTileMeshDraws);

            //Reduce cache.
            while (tileMeshDrawsForRecycle.Count > 4) //Should be configurable. Yes it should.
            {
                var lastIndex = tileMeshDrawsForRecycle.Count - 1;
                tileMeshDrawsForRecycle[lastIndex].Dispose();
                tileMeshDrawsForRecycle.RemoveAt(lastIndex);
            }
                        
        }        
    }
}
