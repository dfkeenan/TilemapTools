using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconStudio.Core.Mathematics;
using SiliconStudio.Xenko.Graphics;

namespace TilemapTools.Xenko.Graphics
{
    public abstract class TileMeshDrawBuilder<TVertex> : IDisposable
        where TVertex : struct, IVertex
    {
        private readonly Dictionary<Texture, List<Tuple<Rectangle, RectangleF>>> tilesByTexture = new Dictionary<Texture, List<Tuple<Rectangle, RectangleF>>>();
        private readonly VertexDeclaration layout;

        protected TileMeshDrawBuilder(VertexDeclaration layout, int indiciesPerTile)
        {
            if (layout == null)
                throw new ArgumentNullException(nameof(layout));

            this.layout = layout;
            IndiciesPerTile = indiciesPerTile;
        }

        public int IndiciesPerTile { get; }

        public void Clear()
        {
            tilesByTexture.Clear();
        }

        public void Add(Texture texture, Rectangle source, RectangleF destination)
        {
            List<Tuple<Rectangle, RectangleF>> list;

            if (!tilesByTexture.TryGetValue(texture, out list))
            {
                tilesByTexture[texture] = list = new List<Tuple<Rectangle, RectangleF>>();
            }

            list.Add(new Tuple<Rectangle, RectangleF>(source, destination));
        }


        public TileMeshDraw Build(GraphicsDevice graphicsDevice)
        {
            var vertices = new List<TVertex>();
            short[] indices = null;
            var ranges = new List<TileMeshDraw.DrawRange>();

            int tileCount = 0;

            foreach (var tileGroup in tilesByTexture)
            {
                var textureWidth = tileGroup.Key.Width;
                var textureHeight = tileGroup.Key.Height;

                foreach (var tile in tileGroup.Value)
                {
                    var source = tile.Item1;
                    var dest = tile.Item2;

                    BuildTile(vertices, textureWidth, textureHeight, source, dest);
                }

                ranges.Add(new TileMeshDraw.DrawRange
                {
                    Texture = tileGroup.Key,
                    StartIndex = tileCount * IndiciesPerTile,
                    IndexCount = tileGroup.Value.Count * IndiciesPerTile
                });

                tileCount += tileGroup.Value.Count;
            }

            indices = new short[tileCount * IndiciesPerTile];

            BuildIndicies(indices, tileCount);

            return Build(graphicsDevice, vertices.ToArray(), indices.ToArray(), ranges);
        }

        protected abstract void BuildIndicies(short[] indices, int tileCount);        

        protected abstract void BuildTile(List<TVertex> vertices, int textureWidth, int textureHeight, Rectangle source, RectangleF dest);

        protected TileMeshDraw Build(GraphicsDevice graphicsDevice, TVertex[] vertices, short[] indices, IEnumerable<TileMeshDraw.DrawRange> ranges)
        {
            return TileMeshDraw.New<TVertex>(graphicsDevice, layout, vertices, indices, ranges);
        }

        protected abstract TVertex CreateVertex(Vector2 source, Vector2 destination, Vector2 size);

        public void Dispose()
        {
            tilesByTexture.Clear();
        }
    }
}
