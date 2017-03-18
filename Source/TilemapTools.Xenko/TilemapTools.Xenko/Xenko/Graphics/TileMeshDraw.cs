using System;
using System.Collections.Generic;
using SiliconStudio.Xenko.Graphics;
using SiliconStudio.Core;

using Buffer = SiliconStudio.Xenko.Graphics.Buffer;

namespace TilemapTools.Xenko.Graphics
{
    public class TileMeshDraw : IDisposable
    {
        public VertexBufferBinding VertexBuffer { get; internal set;}
        public IndexBufferBinding IndexBuffer { get; internal set; }
        public List<DrawRange> Ranges { get; private set; }

        private TileMeshDraw()
        {

        }

        public void Dispose()
        {
            VertexBuffer.Buffer?.Dispose();
            VertexBuffer = default(VertexBufferBinding);

            IndexBuffer?.Buffer?.Dispose();
            IndexBuffer = default(IndexBufferBinding);

            Ranges?.Clear();
        }

        public static TileMeshDraw New<TVertex>(GraphicsDevice graphicsDevice, VertexDeclaration layout, TVertex[] vertexBuffer, int[] indexBuffer, IEnumerable<DrawRange> ranges)
            where TVertex : struct, IVertex
        {
            return New<TVertex, int>(graphicsDevice, layout, vertexBuffer, indexBuffer, ranges);
        }

        public static TileMeshDraw New<TVertex>(GraphicsDevice graphicsDevice, VertexDeclaration layout, TVertex[] vertexBuffer, short[] indexBuffer, IEnumerable<DrawRange> ranges)
            where TVertex : struct, IVertex
        {
            return New<TVertex, short>(graphicsDevice, layout, vertexBuffer, indexBuffer, ranges);
        }

        private static TileMeshDraw New<TVertex,TIndex>(GraphicsDevice graphicsDevice, VertexDeclaration layout, TVertex[] vertexBuffer, TIndex[] indexBuffer, IEnumerable<DrawRange> ranges)
            where TVertex : struct, IVertex
            where TIndex : struct
        {
            if (graphicsDevice == null)
                throw new ArgumentNullException(nameof(graphicsDevice));

            if (layout == null)
                throw new ArgumentNullException(nameof(layout));

            if (vertexBuffer == null)
                throw new ArgumentNullException(nameof(vertexBuffer));

            if (indexBuffer == null)
                throw new ArgumentNullException(nameof(indexBuffer));

            if (ranges == null)
                throw new ArgumentNullException(nameof(ranges));

            var batch = new TileMeshDraw()
            {
                VertexBuffer = new VertexBufferBinding(Buffer.Vertex.New<TVertex>(graphicsDevice, vertexBuffer,GraphicsResourceUsage.Default),layout,vertexBuffer.Length),
                IndexBuffer = new IndexBufferBinding(Buffer.Index.New(graphicsDevice, indexBuffer,GraphicsResourceUsage.Default), Utilities.SizeOf<TIndex>() == sizeof(Int32) , indexBuffer.Length),
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
