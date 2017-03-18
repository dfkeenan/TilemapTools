using System;
using System.Collections.Generic;
using System.Linq;
using SiliconStudio.Core;
using SiliconStudio.Core.Mathematics;
using SiliconStudio.Xenko.Graphics;

namespace TilemapTools.Xenko.Graphics
{
    public abstract class TileMeshDrawBuilder<TVertex> : IDisposable, ITileMeshDrawBuilder where TVertex : struct, IVertex
    {
        private readonly Dictionary<Texture, List<Tuple<Rectangle, RectangleF>>> tilesByTexture = new Dictionary<Texture, List<Tuple<Rectangle, RectangleF>>>();
        private readonly VertexDeclaration layout;

        private short[] indexBuffer;
        private TVertex[] vertexBuffer;

        protected TileMeshDrawBuilder(VertexDeclaration layout, int indiciesPerTile, int verticiesPerTile)
        {
            if (layout == null)
                throw new ArgumentNullException(nameof(layout));

            this.layout = layout;
            IndiciesPerTile = indiciesPerTile;
            VerticiesPerTile = verticiesPerTile;
        }

        public int IndiciesPerTile { get; }
        public int VerticiesPerTile { get; }

        public void Clear()
        {
            tilesByTexture.Clear();
        }

        public virtual TileMeshDraw Build(TileGridBlock block, ITileDefinitionSource tileDefinitionSource, GraphicsContext graphicsContext, ref Vector2 cellSize)
        {
            TileMeshDraw tileMeshDraw = null;

            AggregateTiles(block, tileDefinitionSource, graphicsContext, ref cellSize);
            CompleteBuild(graphicsContext, block.BlockSize * block.BlockSize, ref tileMeshDraw);

            return tileMeshDraw;
        }

        public virtual void Recycle(TileMeshDraw tileMeshDraw, TileGridBlock block, ITileDefinitionSource tileDefinitionSource, GraphicsContext graphicsContext, ref Vector2 cellSize)
        {
            AggregateTiles(block, tileDefinitionSource, graphicsContext, ref cellSize);
            CompleteBuild(graphicsContext, block.BlockSize * block.BlockSize, ref tileMeshDraw);
        }

        protected virtual void AggregateTiles(TileGridBlock block, ITileDefinitionSource tileDefinitionSource, GraphicsContext graphicsContext, ref Vector2 cellSize)
        {
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

                            Add(frame.Texture, ref frame.TextureRegion, ref outRect);
                        }
                    }
                    outRect.X += cellSize.X;
                }
                outRect.X = block.Origin.X;
                outRect.Y -= cellSize.Y;
            }
        }

        protected abstract void BuildIndicies(short[] indices, int tileCount);        

        protected abstract void BuildTile(TVertex[] vertices, ref int vertexIndex, int textureWidth, int textureHeight, Rectangle source, RectangleF dest);

        protected abstract TVertex CreateVertex(Vector2 source, Vector2 destination, Vector2 size);

        public void Dispose()
        {
            tilesByTexture.Clear();
        }

        protected virtual void Add(Texture texture, ref Rectangle source, ref RectangleF destination)
        {
            List<Tuple<Rectangle, RectangleF>> list;

            if (!tilesByTexture.TryGetValue(texture, out list))
            {
                tilesByTexture[texture] = list = new List<Tuple<Rectangle, RectangleF>>();
            }

            list.Add(new Tuple<Rectangle, RectangleF>(source, destination));
        }


        protected virtual void CompleteBuild(GraphicsContext graphicsContext, int maxTileCount, ref TileMeshDraw tileMeshDraw)
        {
            var indexBufferLength = maxTileCount * IndiciesPerTile;
            bool updateIndexBuffer = false;

            if (indexBuffer == null || indexBuffer.Length != indexBufferLength)
            {
                indexBuffer = new short[indexBufferLength];
                BuildIndicies(indexBuffer, maxTileCount);
                updateIndexBuffer = true;
            }


            var vertexBufferLength = maxTileCount * IndiciesPerTile;

            if (vertexBuffer == null || vertexBuffer.Length != vertexBufferLength)
                vertexBuffer = new TVertex[vertexBufferLength];

            tileMeshDraw?.Ranges.Clear();
                       
            var ranges = tileMeshDraw?.Ranges ?? new List<TileMeshDraw.DrawRange>();

            int tileCount = 0;
            int vertexIndex = 0;
            foreach (var tileGroup in tilesByTexture)
            {
                var textureWidth = tileGroup.Key.Width;
                var textureHeight = tileGroup.Key.Height;

                foreach (var tile in tileGroup.Value)
                {
                    var source = tile.Item1;
                    var dest = tile.Item2;

                    BuildTile(vertexBuffer, ref vertexIndex, textureWidth, textureHeight, source, dest);
                }

                ranges.Add(new TileMeshDraw.DrawRange
                {
                    Texture = tileGroup.Key,
                    StartIndex = tileCount * IndiciesPerTile,
                    IndexCount = tileGroup.Value.Count * IndiciesPerTile
                });

                tileCount += tileGroup.Value.Count;
            }

            if(tileMeshDraw == null)
            {

                tileMeshDraw = TileMeshDraw.New<TVertex>(graphicsContext.CommandList.GraphicsDevice, layout, vertexBuffer, indexBuffer, ranges);
            }
            else
            {                
                tileMeshDraw.VertexBuffer = UpdateVertexBuffer(graphicsContext, tileMeshDraw.VertexBuffer);

                if (updateIndexBuffer)
                    tileMeshDraw.IndexBuffer = UpdateIndexBuffer(graphicsContext, tileMeshDraw.IndexBuffer);
            }
            
            tilesByTexture.Clear();            
        }

        
        private IndexBufferBinding UpdateIndexBuffer(GraphicsContext graphicsContext, IndexBufferBinding indexBufferBinding)
        {
            int indexStructSize = Utilities.SizeOf<short>();

            if (indexBufferBinding.Count != indexBuffer.Length)
            {
                indexBufferBinding.Buffer.Dispose();
                return new IndexBufferBinding(SiliconStudio.Xenko.Graphics.Buffer.Index.New<short>(graphicsContext.CommandList.GraphicsDevice, indexBuffer, GraphicsResourceUsage.Dynamic), indexStructSize == sizeof(Int32), indexBuffer.Length);
            }
            else
            {

                //var mappedIndices = graphicsContext.CommandList.MapSubresource(indexBufferBinding.Buffer, 0, MapMode.Write, false, 0, indexStructSize * indexBufferBinding.Count);
                //var indexPointer = mappedIndices.DataBox.DataPointer;

                //for (int i = 0; i < indexBuffer.Length; i++)
                //{


                //    indexPointer += indexStructSize;
                //}

                //graphicsContext.CommandList.UnmapSubresource(mappedIndices);
                indexBufferBinding.Buffer.SetData(graphicsContext.CommandList, indexBuffer);

                return indexBufferBinding;
            }
        }

        public VertexBufferBinding UpdateVertexBuffer(GraphicsContext graphicsContext, VertexBufferBinding vertexBufferBinding)
        {
            if (vertexBufferBinding.Count != vertexBuffer.Length)
            {
                vertexBufferBinding.Buffer.Dispose();
                return new VertexBufferBinding(SiliconStudio.Xenko.Graphics.Buffer.Vertex.New<TVertex>(graphicsContext.CommandList.GraphicsDevice, vertexBuffer, GraphicsResourceUsage.Dynamic), vertexBufferBinding.Declaration, vertexBuffer.Length);
            }
            else
            {
                //var vertexStructSize = vertexBufferBinding.Declaration.CalculateSize();

                //var mappedVertices = graphicsContext.CommandList.MapSubresource(vertexBufferBinding.Buffer, 0, MapMode.WriteNoOverwrite, false, 0, vertexStructSize * vertexBufferBinding.Count);
                //var vertexPointer = mappedVertices.DataBox.DataPointer;

                //for (int i = 0; i < vertexBuffer.Length; i++)
                //{
                //    UpdateVertex(vertexPointer, vertexBuffer[i]);

                //    vertexPointer += vertexStructSize;
                //}

                //graphicsContext.CommandList.UnmapSubresource(mappedVertices);

                vertexBufferBinding.Buffer.SetData(graphicsContext.CommandList, vertexBuffer);
                return vertexBufferBinding;
            }
        }

        protected abstract unsafe void UpdateVertex(IntPtr bufferPointer, TVertex vertex);
    }

    public abstract class TileMeshDrawBuilder : TileMeshDrawBuilder<VertexPositionTexture>
    {
        public TileMeshDrawBuilder(int indiciesPerTile, int verticiesPerTile) : base(VertexPositionTexture.Layout, indiciesPerTile, verticiesPerTile)
        {
        }

        protected override VertexPositionTexture CreateVertex(Vector2 source, Vector2 destination, Vector2 size)
        {
            return new VertexPositionTexture(new Vector3(destination, 0f), (source / size));
        }

        protected override unsafe void UpdateVertex(IntPtr bufferPointer, VertexPositionTexture vertex)
        {
            var pointer = (VertexPositionTexture*)bufferPointer;

            pointer->Position = vertex.Position;
            pointer->TextureCoordinate = vertex.TextureCoordinate;
        }
    }
}
