using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconStudio.Core.Mathematics;
using SiliconStudio.Xenko.Graphics;

namespace TilemapTools.Xenko
{
    public abstract class CachedQuadBatchBuilder<TVertex> : IDisposable
        where TVertex : struct, IVertex
    {
        private readonly Dictionary<Texture, List<Tuple<Rectangle, RectangleF>>> quads = new Dictionary<Texture, List<Tuple<Rectangle, RectangleF>>>();

        public void Clear()
        {
            quads.Clear();
        }

        public void Add(Texture texture, Rectangle source, RectangleF destination)
        {
            List<Tuple<Rectangle, RectangleF>> list;

            if(!quads.TryGetValue(texture, out list))
            {
                quads[texture] = list = new List<Tuple<Rectangle, RectangleF>>();
            }

            list.Add(new Tuple<Rectangle, RectangleF>(source, destination));
        }


        public CachedQuadBatch ToCachedQuadBatch(GraphicsDevice graphicsDevice)
        {
            var vertices = new List<TVertex>();
            short[] indices = null;
            var ranges = new List<CachedQuadBatch.DrawRange>();


            int quadCount = 0;

            foreach (var pair in quads)
            {
                foreach (var quad in pair.Value)
                {
                    var source = quad.Item1;
                    var dest = quad.Item2;

                    vertices.Add(CreateVertex(source.TopLeft, dest.TopLeft, new Vector2(pair.Key.Width, pair.Key.Height)));
                    vertices.Add(CreateVertex(source.BottomLeft, dest.BottomLeft, new Vector2(pair.Key.Width, pair.Key.Height)));
                    vertices.Add(CreateVertex(source.TopRight, dest.TopRight, new Vector2(pair.Key.Width, pair.Key.Height)));
                    vertices.Add(CreateVertex(source.BottomRight, dest.BottomRight, new Vector2(pair.Key.Width, pair.Key.Height)));
                }

                ranges.Add(new CachedQuadBatch.DrawRange
                {
                    Texture = pair.Key,
                    StartIndex = quadCount * 6,
                    IndexCount = pair.Value.Count * 6
                });

                quadCount += pair.Value.Count;
            }

            indices = new short[quadCount * 6];

            for (int i = 0; i < quadCount; i++)
            {
                indices[i * 6 + 0] = (short)(i * 4);
                indices[i * 6 + 1] = (short)(i * 4 + 1);
                indices[i * 6 + 2] = (short)(i * 4 + 2);
                indices[i * 6 + 3] = (short)(i * 4 + 1);
                indices[i * 6 + 4] = (short)(i * 4 + 3);
                indices[i * 6 + 5] = (short)(i * 4 + 2);
            }

            var batch = new CachedQuadBatch()
            {
                VertexBuffer = SiliconStudio.Xenko.Graphics.Buffer.Vertex.New(graphicsDevice, vertices.ToArray()),
                VertexSize = vertices[0].GetLayout().CalculateSize(),
                IndexBuffer = SiliconStudio.Xenko.Graphics.Buffer.Index.New(graphicsDevice, indices),
                Ranges = ranges,
            };

            return batch;
        }

        protected abstract TVertex CreateVertex(Vector2 source, Vector2 destination, Vector2 size);

        public void Dispose()
        {
            quads.Clear();
        }
    }

    public class CachedQuadBatchBuilder : CachedQuadBatchBuilder<VertexPositionTexture>
    {
        protected override VertexPositionTexture CreateVertex(Vector2 source, Vector2 destination, Vector2 size)
        {
            return new VertexPositionTexture(new Vector3(destination, 0f), (source / size));
        }
    }
}
