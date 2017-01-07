using System;
using System.Collections.Generic;
using SiliconStudio.Xenko.Graphics;
using SiliconStudio.Core;

using Buffer = SiliconStudio.Xenko.Graphics.Buffer;

namespace TilemapTools.Xenko.Graphics
{
    public class TileMeshDraw : IDisposable
    {
        public VertexBufferBinding VertexBuffer { get; private set;}
        public IndexBufferBinding IndexBuffer { get; private set; }
        public List<DrawRange> Ranges { get; private set; }

        private TileMeshDraw()
        {

        }

        public void Dispose()
        {
            VertexBuffer.Buffer.Dispose();
            VertexBuffer = default(VertexBufferBinding);

            IndexBuffer.Buffer.Dispose();
            IndexBuffer = default(IndexBufferBinding);

            Ranges?.Clear();
        }

        public static TileMeshDraw New<TVertex>(GraphicsDevice graphicsDevice, VertexDeclaration layout, TVertex[] vertices, int[] indices, IEnumerable<DrawRange> ranges)
            where TVertex : struct, IVertex
        {
            return New<TVertex, int>(graphicsDevice, layout, vertices, indices, ranges);
        }

        public static TileMeshDraw New<TVertex>(GraphicsDevice graphicsDevice, VertexDeclaration layout, TVertex[] vertices, short[] indices, IEnumerable<DrawRange> ranges)
            where TVertex : struct, IVertex
        {
            return New<TVertex, short>(graphicsDevice, layout, vertices, indices, ranges);
        }

        private static TileMeshDraw New<TVertex,TIndex>(GraphicsDevice graphicsDevice, VertexDeclaration layout, TVertex[] vertices, TIndex[] indices, IEnumerable<DrawRange> ranges)
            where TVertex : struct, IVertex
            where TIndex : struct
        {
            if (graphicsDevice == null)
                throw new ArgumentNullException(nameof(graphicsDevice));

            if (layout == null)
                throw new ArgumentNullException(nameof(layout));

            if (vertices == null)
                throw new ArgumentNullException(nameof(vertices));

            if (indices == null)
                throw new ArgumentNullException(nameof(indices));

            if (ranges == null)
                throw new ArgumentNullException(nameof(ranges));

            var batch = new TileMeshDraw()
            {
                VertexBuffer = new VertexBufferBinding(Buffer.Vertex.New<TVertex>(graphicsDevice, vertices),layout,vertices.Length),
                IndexBuffer = new IndexBufferBinding(Buffer.Index.New(graphicsDevice, indices), Utilities.SizeOf<TVertex>() == sizeof(Int32) , indices.Length),
                Ranges = new List<DrawRange>(ranges),
            };

            return batch;
        }

        public class DrawRange
        {
            public Texture Texture;
            public int StartIndex;
            public int IndexCount;
        }
    }
}
